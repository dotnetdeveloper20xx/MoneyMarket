using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyMarket.Domain.Entities;

namespace MoneyMarket.Persistence.Configurations;

public class LoanConfiguration : IEntityTypeConfiguration<Loan>
{
    public void Configure(EntityTypeBuilder<Loan> builder)
    {
        builder.HasKey(l => l.LoanId);

        builder.Property(l => l.RequestedAmount).HasColumnType("decimal(18,2)");
        builder.Property(l => l.ApprovedAmount).HasColumnType("decimal(18,2)");
        builder.Property(l => l.InterestRate).HasColumnType("decimal(5,2)");
        builder.Property(l => l.Fees).HasColumnType("decimal(18,2)");
        builder.Property(l => l.TotalRepayableAmount).HasColumnType("decimal(18,2)");

        // Repayments (one-to-many)
        builder.HasMany(l => l.RepaymentSchedule)
            .WithOne()
            .HasForeignKey(r => r.LoanId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
