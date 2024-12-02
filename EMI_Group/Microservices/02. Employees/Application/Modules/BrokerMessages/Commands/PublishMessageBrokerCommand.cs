using Domain.Interfaces.IPublishMessage;
using MediatR;
using Shared;

namespace Application.Modules.BrokerMessages.Commands
{
    public class PublishMessageBrokerCommand : IRequest<RequestResult<bool>>
    {
        public string QueueName { get; }
        public object Message { get; }

        public PublishMessageBrokerCommand(string queueName, object message)
        {
            QueueName = queueName;
            Message = message;
        }
    }

    public class PublishMessageCommandHandler : IRequestHandler<PublishMessageBrokerCommand, RequestResult<bool>>
    {
        private readonly IMessageQueueService _messageProducer;

        public PublishMessageCommandHandler(IMessageQueueService messageProducer)
        {
            _messageProducer = messageProducer;
        }

        public async Task<RequestResult<bool>> Handle(PublishMessageBrokerCommand request, CancellationToken cancellationToken)
        {
            await _messageProducer.SendMessageAsync(request.QueueName, request.Message);
            return RequestResult<bool>.SuccessOperation();
        }
    }
}
