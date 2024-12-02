using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace Web.Employee.API.Middlewares.HealthCheck
{
    public static class HealthCheckMiddleware
    {
        public static void ConfigureHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
            .AddSqlServer(configuration["ConnectionStrings:SqlServer"], healthQuery: "select 1", name: "Conexión a base de datos", failureStatus: HealthStatus.Unhealthy)
            .AddCheck<RemoteHealthCheck>("End points de integración con terceros", failureStatus: HealthStatus.Unhealthy, tags: new[] { "Third's  integration" })
            .AddCheck<MemoryHealthCheck>($"Memoria del servicio", failureStatus: HealthStatus.Unhealthy, tags: new[] { "Memory Service" })
            .AddCheck<LoggingHealthCheck>("Monitoreo de errores", failureStatus: HealthStatus.Unhealthy, tags: new[] { "Logging Error" });

            services.AddHealthChecksUI(opt =>
            {
                opt.SetEvaluationTimeInSeconds(10); //time in seconds between check    
                opt.MaximumHistoryEntriesPerEndpoint(60); //maximum history of checks    
                opt.SetApiMaxActiveRequests(1); //api requests concurrency    
                opt.AddHealthCheckEndpoint("FeedBack API", "/api/health"); //map health check api    

            })
                .AddInMemoryStorage();
        }
    }

    public class RemoteHealthCheck : IHealthCheck
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public RemoteHealthCheck(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                var response = await httpClient.GetAsync("https://api.ipify.org");
                if (response.IsSuccessStatusCode)
                {
                    return HealthCheckResult.Healthy($"Remote endpoints is healthy.");
                }

                return HealthCheckResult.Unhealthy("Remote endpoint is unhealthy");
            }
        }
    }

    public class MemoryHealthCheck : IHealthCheck
    {
        private readonly IOptionsMonitor<MemoryCheckOptions> _options;

        public MemoryHealthCheck(IOptionsMonitor<MemoryCheckOptions> options)
        {
            _options = options;
        }

        public string Name => "memory_check";

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            var options = _options.Get(context.Registration.Name);

            // Include GC information in the reported diagnostics.
            var allocated = GC.GetTotalMemory(forceFullCollection: false);
            var data = new Dictionary<string, object>()
        {
            { "AllocatedBytes", allocated },
            { "Gen0Collections", GC.CollectionCount(0) },
            { "Gen1Collections", GC.CollectionCount(1) },
            { "Gen2Collections", GC.CollectionCount(2) },
        };
            var status = allocated < options.Threshold ? HealthStatus.Healthy : HealthStatus.Unhealthy;

            return Task.FromResult(new HealthCheckResult(
                status,
                description: $"Reports degraded status if allocated {allocated} bytes >= {options.Threshold} bytes.",
                exception: null,
                data: data));
        }
    }

    public class LoggingHealthCheck : IHealthCheck
    {
        private readonly ILogger<LoggingHealthCheck> _logger;

        public LoggingHealthCheck(ILogger<LoggingHealthCheck> logger)
        {
            _logger = logger;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var logFilePath = "c:\\Temp\\EmiGroup\\NLogSample\\Log_Error.log"; // Define the path of the log file

                if (File.Exists(logFilePath))
                {

                    // Try to open the file with a FileStream
                    using (var stream = new FileStream(logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (var reader = new StreamReader(stream))
                    {
                        // Read the last 10 lines of the log file
                        var lastLines = new List<string>();
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            lastLines.Add(line);
                            if (lastLines.Count > 10)
                            {
                                lastLines.RemoveAt(0);
                            }
                        }

                        foreach (var logLine in lastLines)
                        {
                            if (logLine.Contains("Exception"))
                            {
                                return Task.FromResult(HealthCheckResult.Unhealthy("Se encontraron errores recientes en los logs."));
                            }
                        }
                    }
                }
                return Task.FromResult(HealthCheckResult.Healthy("No se encontraron errores recientes en los logs."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al leer el archivo de log.");
                return Task.FromResult(HealthCheckResult.Unhealthy("No se pudo acceder al archivo de log."));
            }
        }
    }


    public class MemoryCheckOptions
    {
        public string Memorystatus { get; set; }
        public long Threshold { get; set; } = 1024L * 1024L * 1024L;
    }

}
