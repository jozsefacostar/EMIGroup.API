using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Persistence.Configuration
{
    public class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            builder.ToTable("Positions");
            builder.HasKey(c => new { c.Id });
            builder.Property(x => x.Id).IsRequired();
            builder.Property(c => c.Name).HasMaxLength(200).IsRequired(true);
            builder.Property(c => c.RegularJob).IsRequired();

            // Aquí configuras correctamente el Value Object AuditInfo
            builder.OwnsOne(u => u.AuditInfo, a =>
            {
                a.Property(f => f.CreatedAt).HasColumnName("CreatedAt").IsRequired(false);
                a.Property(f => f.ModifiedAt).HasColumnName("ModifiedAt").IsRequired(false);
                a.Property(f => f.Active).HasColumnName("Active").IsRequired(true);
                a.Property(f => f.CreatedBy).HasColumnName("CreatedBy").IsRequired(false);
                a.Property(f => f.ModifiedBy).HasColumnName("ModifiedBy").IsRequired(false);
            });

            builder.HasOne(x => x.DepartmentNavigation)
              .WithMany(x => x.LstDepartmentPositions)
              .HasForeignKey(x => x.Department)
              .HasConstraintName("FK_Positions_Department")
              .OnDelete(DeleteBehavior.ClientSetNull);

        }
    }
}
