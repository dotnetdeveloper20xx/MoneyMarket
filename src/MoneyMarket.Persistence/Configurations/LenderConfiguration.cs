using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyMarket.Domain.Entities;
using MoneyMarket.Persistence.Identity;

namespace MoneyMarket.Persistence.Configurations;

public class LenderConfiguration : IEntityTypeConfiguration<Lender>
{
    public void Configure(EntityTypeBuilder<Lender> builder)
    {
        builder.HasKey(l => l.LenderId);        
        builder.Property(x => x.IsDisabled).HasDefaultValue(false);
        builder.Property(x => x.DisabledReason).HasMaxLength(512);
        builder.Property(x => x.Email).HasMaxLength(256);

        builder.Property(l => l.BusinessName).HasMaxLength(200);
        builder.Property(l => l.RegistrationNumber).HasMaxLength(100);
        builder.Property(l => l.ComplianceStatement).HasMaxLength(500);

        //  Relationship to Identity User
        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.PhotoPath)            
                     .HasMaxLength(1024)
                     .IsRequired(false);
    }
}
