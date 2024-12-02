using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using Domain.ValueObjects;
namespace Infraestructure.Persistence.Configuration;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(c => new { c.Id });
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.Username).HasMaxLength(15).IsRequired();
        builder.Property(x => x.PasswordHash).IsRequired(true);

        // Aquí configuras correctamente el Value Object AuditInfo
        builder.OwnsOne(u => u.AuditInfo, a =>
        {
            a.Property(f => f.CreatedAt).HasColumnName("CreatedAt").IsRequired(false);
            a.Property(f => f.ModifiedAt).HasColumnName("ModifiedAt").IsRequired(false);
            a.Property(f => f.Active).HasColumnName("Active").IsRequired(true);
            a.Property(f => f.CreatedBy).HasColumnName("CreatedBy").IsRequired(false);
            a.Property(f => f.ModifiedBy).HasColumnName("ModifiedBy").IsRequired(false);
        });

        builder.HasOne(x => x.UserRoleNavigation)
            .WithMany(x => x.LstUser)
            .HasForeignKey(x => x.UserRole)
            .HasConstraintName("FK_User_UseRole")
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}