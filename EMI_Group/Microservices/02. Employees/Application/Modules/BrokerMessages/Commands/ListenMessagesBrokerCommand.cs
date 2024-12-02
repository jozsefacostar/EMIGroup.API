using Domain.Interfaces.IPublishMessage;
using MediatR;
using Shared;

namespace Application.Modules.BrokerMessages.Commands
{
    public class ListenMessagesBrokerCommand : IRequest<RequestResult<bool>>
    {
        public string QueueName { get; }

        public ListenMessagesBrokerCommand(string queueName)
        {
            QueueName = queueName;
        }
    }

    public class ListenMessageCommandHandler : IRequestHandler<ListenMessagesBrokerCommand, RequestResult<bool>>
    {
        private readonly IMessageQueueService _messageProducer;

        public ListenMessageCommandHandler(IMessageQueueService messageProducer)
        {
            _messageProducer = messageProducer;
        }

        public async Task<RequestResult<bool>> Handle(ListenMessagesBrokerCommand request, CancellationToken cancellationToken)
        {
            await _messageProducer.StartListening(request.QueueName);
            return RequestResult<bool>.SuccessOperation();
        }
    }
}
