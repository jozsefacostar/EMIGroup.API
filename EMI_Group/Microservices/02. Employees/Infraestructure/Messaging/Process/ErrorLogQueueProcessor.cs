using Infraestructure.Messaging.Interface;
using Messageging.Process.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infraestructure.Messaging.Proccess
{
    public class ErrorLogQueueProcessor : IQueueProcessor
    {
        public string QueueCode => "error_log_queue";
        private readonly SendInfoToApiError sendPetitionHttp = new SendInfoToApiError();
        private readonly RabbitMQService _sendProcessBroker = new RabbitMQService();

        public ErrorLogQueueProcessor() { }
        public ErrorLogQueueProcessor(RabbitMQService sendProcessBroke)
        {
            _sendProcessBroker = sendProcessBroke;
        }

        public async Task ProcessMessageAsync(dynamic message)
        {
            Error? error = JsonSerializer.Deserialize<Error>(message.ToString());
            try
            {
                await sendPetitionHttp.SendErrorAsync(error);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                error.Data = $" Exception RE-Process :  {ex.ToString()} - \n\n {error.Data}";
                await _sendProcessBroker.SendMessageAsync(QueueCode, error);
            }
        }
    }
    public class Error
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? Data { get; set; }
        public string? Module { get; set; }
    }
}
