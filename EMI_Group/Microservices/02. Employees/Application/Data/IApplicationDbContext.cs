using Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace Application.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Employee> Employees { get; set; }
        DbSet<Position> Positions { get; set; }
        DbSet<PositionHistory> PositionHistories { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<UserRole> UserRoles { get; set; }
        DbSet<Department> Departments { get; set; }
        DbSet<Project> Projects { get; set; }
        DbSet<EmployeeProject> EmployeeProjects { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
