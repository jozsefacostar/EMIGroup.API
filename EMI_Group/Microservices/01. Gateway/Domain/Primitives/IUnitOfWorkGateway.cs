namespace Domain.Primitives
{
    public interface IUnitOfWorkGateway
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
