using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.ValueObjects;
using Domain.Entities;
namespace Infraestructure.Persistence.Configuration;
public class EmployeeProjectConfiguration : IEntityTypeConfiguration<EmployeeProject>
{
    public void Configure(EntityTypeBuilder<EmployeeProject> builder)
    {
        builder.ToTable("EmployeeProjects");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Project).IsRequired();
        builder.Property(x => x.Employee).IsRequired();
        builder.OwnsOne(u => u.AuditInfo, a =>
        {
            a.Property(f => f.CreatedAt).HasColumnName("CreatedAt").IsRequired(false);
            a.Property(f => f.ModifiedAt).HasColumnName("ModifiedAt").IsRequired(false);
            a.Property(f => f.Active).HasColumnName("Active").IsRequired();
            a.Property(f => f.CreatedBy).HasColumnName("CreatedBy").IsRequired(false);
            a.Property(f => f.ModifiedBy).HasColumnName("ModifiedBy").IsRequired(false);
        });



        builder.HasOne(x => x.EmployeeNavigation)
          .WithMany(x => x.LstEmployeeEmployeeProject)
          .HasForeignKey(x => x.Employee)
          .HasConstraintName("FK_EmployeeProject_Employee")
          .OnDelete(DeleteBehavior.ClientSetNull);


        builder.HasOne(x => x.ProjectNavigation)
          .WithMany(x => x.LstProjectEmployeeProject)
          .HasForeignKey(x => x.Project)
          .HasConstraintName("FK_EmployeeProject_Project")
          .OnDelete(DeleteBehavior.ClientSetNull);

    }
}

