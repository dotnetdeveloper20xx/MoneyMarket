using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyMarket.Domain.Entities;
using MoneyMarket.Persistence.Identity;

namespace MoneyMarket.Persistence.Configurations;

public sealed class LenderConfiguration : IEntityTypeConfiguration<Lender>
{
    public void Configure(EntityTypeBuilder<Lender> b)
    {
        b.ToTable("Lenders");
        b.HasKey(x => x.LenderId);

        b.Property(x => x.Email).HasMaxLength(256);
        b.Property(x => x.BusinessName).HasMaxLength(256);
        b.Property(x => x.RegistrationNumber).HasMaxLength(128);
        b.Property(x => x.ComplianceStatement).HasMaxLength(2048);
        b.Property(x => x.PhotoPath).HasMaxLength(1024);

        // One profile per user? If yes, keep IsUnique(); otherwise remove it.
        b.HasIndex(x => x.UserId)
         .IsUnique()
         .HasDatabaseName("IX_Lenders_UserId");

        b.HasOne<ApplicationUser>()
         .WithMany()
         .HasForeignKey(x => x.UserId)
         .HasConstraintName("FK_Lenders_AspNetUsers_UserId")
         .OnDelete(DeleteBehavior.Restrict);
    }
}
