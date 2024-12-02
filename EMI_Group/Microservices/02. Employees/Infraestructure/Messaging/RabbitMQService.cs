using Domain.Interfaces.IPublishMessage;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Infraestructure.Messaging.Interface;
using Infraestructure.Messaging.Proccess;
using System.Threading.Channels;

namespace Infraestructure.Messaging
{
    public class RabbitMQService : IMessageQueueService
    {
        private readonly IConnection _connection;
        private QueueProcessorFactory _queueProcessorFactory;
        private IModel _channel; // Canal para escuchar los mensajes
        private int _processingCount = 0; // Contador de los mensajes que se están procesando


        public RabbitMQService(IConnection connection, QueueProcessorFactory queueProcessorFactory)
        {
            _connection = connection;
            _queueProcessorFactory = queueProcessorFactory;
        }

        public RabbitMQService(string hostName = "localhost")
        {
            var factory = new ConnectionFactory() { HostName = hostName };
            _connection = factory.CreateConnection();
        }

        public async Task SendMessageAsync<T>(string queueName, T message)
        {
            _channel = _connection.CreateModel();
            var jsonMessage = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(jsonMessage);
            _channel.QueueDeclare(queueName, false, false, false, null);
            _channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
        }

        public Task StartListening(string queueName)
        {
            // Creamos y registramos los procesadores
            var processors = new List<IQueueProcessor>
                {
                    new EmailQueueProcessor(),
                    new ErrorLogQueueProcessor()
                };

            _queueProcessorFactory = new QueueProcessorFactory(processors);
            // Creamos el canal de escucha
            _channel = _connection.CreateModel();
            ListenToQueue(_channel, queueName);
            return Task.CompletedTask;
        }

        /* Método que se encarga de escuchar los mensajes de la cola */
        private void ListenToQueue(IModel channel, string queueName)
        {
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                // Incrementamos el contador de mensajes en proceso
                Interlocked.Increment(ref _processingCount);

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var processor = _queueProcessorFactory.GetProcessor(queueName);

                if (processor != null)
                {
                    await processor.ProcessMessageAsync(message);

                    if (Interlocked.Decrement(ref _processingCount) == 0)
                        OnAllMessagesProcessed();

                }
            };

            // Comienza a consumir mensajes de la cola
            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }

        /* Método que se llama cuando todos los mensajes han sido procesados */
        private void OnAllMessagesProcessed()
        {
            Console.WriteLine("Todos los mensajes han sido procesados.");
            StopListening(_channel);
        }

        /* Método para detener la escucha y cerrar el canal */
        public void StopListening(IModel channel)
        {
            channel?.Close();
            Console.WriteLine("Escucha detenida y canal cerrado.");
        }
    }
}
