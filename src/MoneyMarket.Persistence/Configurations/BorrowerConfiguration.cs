using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyMarket.Domain.Entities;
using MoneyMarket.Persistence.Identity;

namespace MoneyMarket.Persistence.Configurations;

public class BorrowerConfiguration : IEntityTypeConfiguration<Borrower>
{
    public void Configure(EntityTypeBuilder<Borrower> builder)
    {
        builder.HasKey(b => b.BorrowerId);

        builder.Property(b => b.FirstName).HasMaxLength(150);
        builder.Property(b => b.LastName).HasMaxLength(150);
        builder.Property(b => b.NationalIdNumber).HasMaxLength(150);
        builder.Property(b => b.PhoneNumber).HasMaxLength(150);
        builder.Property(b => b.Email).HasMaxLength(256);

        
        builder.HasOne<ApplicationUser>() 
            .WithMany()
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Value Object: Address
        builder.OwnsOne(b => b.Address, a =>
        {
            a.Property(p => p.House).HasMaxLength(150);
            a.Property(p => p.Street).HasMaxLength(150);
            a.Property(p => p.City).HasMaxLength(150);
            a.Property(p => p.Country).HasMaxLength(150);
            a.Property(p => p.PostCode).HasMaxLength(20);
        });

        // Value Object Collection: Debts
        builder.OwnsMany(b => b.ExistingDebts, d =>
        {
            d.WithOwner().HasForeignKey("BorrowerId");
            d.HasKey("BorrowerId", "LenderName", "DebtType"); 
            d.Property(p => p.LenderName).HasMaxLength(150);
            d.Property(p => p.DebtType).HasMaxLength(100);
            d.Property(p => p.Amount).HasColumnType("decimal(18,2)");
        });
    }
}
