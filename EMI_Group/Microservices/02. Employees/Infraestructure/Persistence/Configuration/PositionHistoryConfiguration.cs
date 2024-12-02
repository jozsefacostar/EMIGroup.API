using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class PositionHistoryConfiguration : IEntityTypeConfiguration<PositionHistory>
{
    public void Configure(EntityTypeBuilder<PositionHistory> builder)
    {
        builder.ToTable("PositionHistories");
        builder.HasKey(c => new { c.Id });
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.Salary).IsRequired();

        // Mapeo del Value Object DateRange
        builder.OwnsOne(x => x.DateRange, dateRange =>
        {
            dateRange.Property(d => d.StartDate).HasColumnName("StartDate").IsRequired();
            dateRange.Property(d => d.EndDate).HasColumnName("EndDate").IsRequired(false);
        });

        // Aquí configuras correctamente el Value Object AuditInfo
        builder.OwnsOne(u => u.AuditInfo, a =>
        {
            a.Property(f => f.CreatedAt).HasColumnName("CreatedAt").IsRequired(false);
            a.Property(f => f.ModifiedAt).HasColumnName("ModifiedAt").IsRequired(false);
            a.Property(f => f.Active).HasColumnName("Active").IsRequired(true);
            a.Property(f => f.CreatedBy).HasColumnName("CreatedBy").IsRequired(false);
            a.Property(f => f.ModifiedBy).HasColumnName("ModifiedBy").IsRequired(false);
        });

        builder.HasOne(x => x.EmployeeNavigation)
            .WithMany(x => x.LstPositionHistoryEmployee)
            .HasForeignKey(x => x.EmployeeId)
            .HasConstraintName("FK_PositionHistory_Employee")
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder.HasOne(x => x.PositionNavigation)
            .WithMany(x => x.LstPositionHistoryPosition)
            .HasForeignKey(x => x.PositionId)
            .HasConstraintName("FK_PositionHistory_Position")
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}
