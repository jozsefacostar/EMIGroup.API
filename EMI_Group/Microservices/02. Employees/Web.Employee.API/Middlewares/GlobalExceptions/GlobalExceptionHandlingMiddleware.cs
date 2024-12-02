using Application.Modules.BrokerMessages.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared;
using System.Net;
using System.Text;
using System.Text.Json;
using Web.Employee.API.Middlewares.GlobalExceptions.DTOs;

namespace Web.Employee.API.Middlewares.GlobalExceptions
{
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
        private readonly IMediator _mediator;

        public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                context.Request.EnableBuffering();
                await next(context);
            }
            catch (Exception ex)
            {
                var commandName = context.GetEndpoint()?.DisplayName ?? "Unknown Command";
                var (requestBody, routeParams, queryParams) = await GetRequestDetails(context);
                var problemDetails = await BuildProblemDetailsAsync(ex, requestBody, routeParams, queryParams);
                var response = BuildErrorResponse(commandName, problemDetails);
                _logger.LogError(ex, "An error occurred while processing the request: {Response}", JsonSerializer.Serialize(response));
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                await sendMessageBrokers(response);
            }
        }

        /* Función que envia los eventos al Service Broker */
        private async Task sendMessageBrokers(RequestResult<dynamic> response)
        {
            /* Se emite evento para que el envio de correo */
            await _mediator.Send(new PublishMessageBrokerCommand("email_queue", new EmailData { Subject = "Transaction Error - Send with Integration RabbitMQ - " + DateTime.Now, To = "ingjozsefacosta@gmail.com", Body = response?.Data?.Title }));
            /* Se emite evento para que se guarde información en la base de datos de errores en transacciones */
            await _mediator.Send(new PublishMessageBrokerCommand("error_log_queue", new Error { Data = response.Data.Title, Message = response.Message, Module = response.Module, Success = false }));
        }

        /* Función que obtiene el detalle de la petición */
        private async Task<(string requestBody, string routeParams, string queryParams)> GetRequestDetails(HttpContext context)
        {
            string requestBody = string.Empty;
            if (context.Request.ContentLength > 0)
            {
                requestBody = await ReadRequestBodyAsync(context);
            }
            string routeParams = context.Request.Path;
            string queryParams = context.Request.QueryString.Value ?? string.Empty;

            return (requestBody, routeParams, queryParams);
        }

        /* Función que construye el problem detail */
        private async Task<ProblemDetails> BuildProblemDetailsAsync(Exception ex, string requestBody, string routeParams, string queryParams)
        {
            var detailsBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(requestBody)) detailsBuilder.Append("Payload: ").Append(requestBody);
            if (!string.IsNullOrEmpty(queryParams)) detailsBuilder.Append("\nQuery Parameters: ").Append(queryParams);
            if (!string.IsNullOrEmpty(routeParams)) detailsBuilder.Append("\nRoute Parameters: ").Append(routeParams);

            return new ProblemDetails
            {
                Status = await MapErrorCodeAsync(ex),
                Type = "Server Error",
                Title = ex.InnerException?.Message ?? ex.Message,
                Detail = detailsBuilder.ToString()
            };
        }

        /* Función que construye el error response */
        private RequestResult<dynamic> BuildErrorResponse(string commandName, ProblemDetails problemDetails)
        {
            return new RequestResult<dynamic>
            {
                Success = false,
                Data = problemDetails,
                Message = "No es posible realizar el proceso",
                Module = commandName
            };
        }

        // Método auxiliar para leer el body del request
        private async Task<string> ReadRequestBodyAsync(HttpContext context)
        {
            // Leer el body solo si está presente
            if (context.Request.Body.CanSeek)
            {
                context.Request.Body.Position = 0; // Restablecer la posición del stream
                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
                {
                    var body = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0; // Restablecer la posición para el próximo middleware
                    return body;
                }
            }
            return string.Empty;
        }

        private static readonly Dictionary<Type, int> ExceptionToStatusCodeMap = new()
        {
            { typeof(ArgumentException), (int)HttpStatusCode.BadRequest },
            { typeof(BadHttpRequestException), (int)HttpStatusCode.BadRequest },
            { typeof(UnauthorizedAccessException), (int)HttpStatusCode.Unauthorized },
            { typeof(KeyNotFoundException), (int)HttpStatusCode.NotFound },
            { typeof(NotImplementedException), (int)HttpStatusCode.NotImplemented },
            { typeof(TimeoutException), (int)HttpStatusCode.RequestTimeout },
            { typeof(InvalidOperationException), (int)HttpStatusCode.Conflict },
            { typeof(AccessViolationException), (int)HttpStatusCode.Forbidden },
            { typeof(FormatException), (int)HttpStatusCode.UnprocessableEntity },
            { typeof(ObjectDisposedException), (int)HttpStatusCode.Gone },
            { typeof(DivideByZeroException), (int)HttpStatusCode.BadRequest },
            { typeof(StackOverflowException), (int)HttpStatusCode.InternalServerError },
            { typeof(OutOfMemoryException), (int)HttpStatusCode.InsufficientStorage }
        };

        public Task<int> MapErrorCodeAsync(Exception ex)
        {
            int statusCode = ExceptionToStatusCodeMap.TryGetValue(ex.GetType(), out var code)
                ? code
                : (int)HttpStatusCode.InternalServerError;

            return Task.FromResult(statusCode);
        }
    }

}
