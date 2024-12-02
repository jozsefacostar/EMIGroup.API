namespace Domain.Interfaces.IPublishMessage
{
    public interface IMessageQueueService
    {
        Task SendMessageAsync<T>(string queueName, T message);
        Task StartListening(string queueName);
    }
}
