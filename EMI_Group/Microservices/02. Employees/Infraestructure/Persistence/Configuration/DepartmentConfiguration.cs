using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.ValueObjects;
using Domain.Entities;
namespace Infraestructure.Persistence.Configuration;
public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("Departments");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Code).IsRequired().HasMaxLength(20);
        builder.OwnsOne(u => u.AuditInfo, a =>
        {
            a.Property(f => f.CreatedAt).HasColumnName("CreatedAt").IsRequired(false);
            a.Property(f => f.ModifiedAt).HasColumnName("ModifiedAt").IsRequired(false);
            a.Property(f => f.Active).HasColumnName("Active").IsRequired();
            a.Property(f => f.CreatedBy).HasColumnName("CreatedBy").IsRequired(false);
            a.Property(f => f.ModifiedBy).HasColumnName("ModifiedBy").IsRequired(false);
        });
    }
}

