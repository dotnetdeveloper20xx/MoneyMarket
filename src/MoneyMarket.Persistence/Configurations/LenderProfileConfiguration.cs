using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyMarket.Domain.Lenders;

namespace MoneyMarket.Persistence.Configurations
{
    public sealed class LenderProfileConfiguration : IEntityTypeConfiguration<LenderProfile>
    {
        public void Configure(EntityTypeBuilder<LenderProfile> l)
        {
            l.HasKey(x => x.Id);
            l.Property(x => x.UserId).IsRequired();
            l.Property(x => x.Email).IsRequired();
            l.Property(x => x.IsDisabled).HasDefaultValue(false);
            l.Property(x => x.DisabledReason).HasMaxLength(512);
        }
    }
}
