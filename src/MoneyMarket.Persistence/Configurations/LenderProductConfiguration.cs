using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyMarket.Domain.Lenders;

namespace MoneyMarket.Persistence.Configurations
{
    public sealed class LenderProductConfiguration : IEntityTypeConfiguration<LenderProduct>
    {
        public void Configure(EntityTypeBuilder<LenderProduct> b)
        {
            b.HasKey(x => x.LenderProductId);
            b.Property(x => x.Name).IsRequired();
            b.Property(x => x.TermType).HasConversion<int>();
            b.Property(x => x.InterestRate).HasPrecision(6, 4);
            b.Property(x => x.MinAmount).HasPrecision(18, 2);
            b.Property(x => x.MaxAmount).HasPrecision(18, 2);
            b.Property(x => x.IsActive).HasDefaultValue(true);
            b.HasIndex(x => new { x.LenderId, x.IsActive });
        }
    }
}
