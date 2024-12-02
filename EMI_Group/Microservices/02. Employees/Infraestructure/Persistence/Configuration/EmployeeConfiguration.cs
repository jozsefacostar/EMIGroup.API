using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.ValueObjects;

namespace Infraestructure.Persistence.Configuration
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees");
            builder.HasKey(c => new { c.Id });
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.IdNum).HasMaxLength(15).IsRequired();
            builder.HasIndex(x => x.IdNum).IsUnique();
            builder.Property(c => c.Name).HasMaxLength(200).IsRequired(true);
            builder.Property(x => x.CurrentPosition).IsRequired(false);
            builder.Property(c => c.Salary).IsRequired();

            builder.OwnsOne(u => u.AuditInfo, a =>
            {
                a.Property(f => f.CreatedAt).HasColumnName("CreatedAt").IsRequired(false);
                a.Property(f => f.ModifiedAt).HasColumnName("ModifiedAt").IsRequired(false);
                a.Property(f => f.Active).HasColumnName("Active").IsRequired();
                a.Property(f => f.CreatedBy).HasColumnName("CreatedBy").IsRequired(false);
                a.Property(f => f.ModifiedBy).HasColumnName("ModifiedBy").IsRequired(false);
            });

            builder.HasOne(x => x.CurrentPositionNavigation)
              .WithMany(x => x.LstPositionEmployees)
              .HasForeignKey(x => x.CurrentPosition)
              .HasConstraintName("FK_Employee_Position")
              .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}