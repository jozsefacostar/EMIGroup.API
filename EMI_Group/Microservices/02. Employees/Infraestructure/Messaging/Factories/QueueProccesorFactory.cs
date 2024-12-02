using Infraestructure.Messaging.Interface;

public class QueueProcessorFactory
{
    private readonly Dictionary<string, IQueueProcessor> _processors;
    public QueueProcessorFactory(IEnumerable<IQueueProcessor> processors)
    {
        _processors = processors.ToDictionary(p => p.QueueCode, p => p);
    }
    public IQueueProcessor GetProcessor(string queueName)
    {
        if (_processors.TryGetValue(queueName, out var processor))
        {
            return processor;
        }
        return null;
    }
}