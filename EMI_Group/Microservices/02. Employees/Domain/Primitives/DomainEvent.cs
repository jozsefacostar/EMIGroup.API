using MediatR;

namespace Domain.Primitives
{
    public record DomainEvent(int id) : INotification;
    
}
