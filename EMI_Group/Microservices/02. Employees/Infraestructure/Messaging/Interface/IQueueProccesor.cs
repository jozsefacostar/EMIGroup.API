namespace Infraestructure.Messaging.Interface
{
    public interface IQueueProcessor
    {
        string QueueCode { get; }  // Nombre de la cola
        Task ProcessMessageAsync(object message);
    }
}
