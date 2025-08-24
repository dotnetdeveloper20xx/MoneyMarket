using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyMarket.Domain.Entities;

namespace MoneyMarket.Persistence.Configurations;

public class RepaymentInstallmentConfiguration : IEntityTypeConfiguration<RepaymentInstallment>
{
    public void Configure(EntityTypeBuilder<RepaymentInstallment> builder)
    {
        builder.HasKey(r => r.InstallmentId);

        builder.Property(r => r.PrincipalAmount).HasColumnType("decimal(18,2)");
        builder.Property(r => r.InterestAmount).HasColumnType("decimal(18,2)");
        builder.Property(r => r.TotalDue).HasColumnType("decimal(18,2)");

        // 👇 Tell EF to ignore computed property
        builder.Ignore(r => r.TotalDue);
    }
}
