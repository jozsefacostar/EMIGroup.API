using Application.Data;
using Domain.Entities;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Persistence
{

    public class ApplicationDbContext : DbContext, IApplicationDbContext, IUnitOfWorkGateway
    {
        private readonly IPublisher _publisher;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IPublisher publisher)
            : base(options)
        {
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var domainEvents = ChangeTracker.Entries<AggregateRoot>()
                .Select(e => e.Entity)
                .Where(x => x.GetDomainEvents().Any())
                .SelectMany(e => e.GetDomainEvents());
            var result = await base.SaveChangesAsync(cancellationToken);
            foreach (var publish in domainEvents)
                await _publisher.Publish(publish, cancellationToken);
            return result;
        }
    }
}
