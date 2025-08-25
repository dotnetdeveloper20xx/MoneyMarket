using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyMarket.Domain.Entities;

namespace MoneyMarket.Persistence.Configurations
{
    public class FundingConfiguration : IEntityTypeConfiguration<Funding>
    {
        public void Configure(EntityTypeBuilder<Funding> b)
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Amount).HasColumnType("decimal(18,2)");
            b.HasOne(x => x.Loan).WithMany(l => l.Fundings).HasForeignKey(x => x.LoanId);
            b.HasOne(x => x.Lender).WithMany().HasForeignKey(x => x.LenderId);

            b.HasIndex(f => f.IdempotencyKey).IsUnique().HasFilter("[IdempotencyKey] IS NOT NULL");
        }
    }
}
