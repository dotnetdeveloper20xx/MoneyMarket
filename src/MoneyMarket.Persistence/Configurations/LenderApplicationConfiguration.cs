using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyMarket.Domain.Lenders;

namespace MoneyMarket.Persistence.Configurations
{
    public sealed class LenderApplicationConfiguration : IEntityTypeConfiguration<LenderApplication>
    {
        public void Configure(EntityTypeBuilder<LenderApplication> b)
        {
            b.HasKey(x => x.LenderApplicationId);
            b.Property(x => x.Email).IsRequired();
            b.Property(x => x.Status).HasConversion<int>();

            // Flatten complex info as owned types
            b.OwnsOne(x => x.BusinessRegistration, nb =>
            {
                nb.Property(p => p.BusinessName).IsRequired();
                nb.Property(p => p.RegistrationNumber).IsRequired();

                // Persist list<string> as JSON if using SQL Server 2022+ & EF 8 value conversion,
                // otherwise create a separate table. For brevity, keep as JSON nvarchar(MAX).
                nb.Property(p => p.ProofOfIncorporationDocuments).HasColumnType("nvarchar(max)");
                nb.Property(p => p.LendingLicenses).HasColumnType("nvarchar(max)");
                nb.Property(p => p.ComplianceStatement).HasColumnType("nvarchar(max)");
            });

            b.OwnsOne(x => x.FinancialCapacity, nb =>
            {
                nb.Property(p => p.FundingSourceType).IsRequired();
                nb.Property(p => p.FundingSourceDescription).HasColumnType("nvarchar(max)");
                nb.Property(p => p.CapitalReserveDocuments).HasColumnType("nvarchar(max)");
            });

            b.OwnsOne(x => x.RiskManagement, nb =>
            {
                nb.Property(p => p.UnderwritingPolicy).HasColumnType("nvarchar(max)");
                nb.Property(p => p.RiskAssessmentTools).HasColumnType("nvarchar(max)");
                nb.Property(p => p.PaymentCollectionProcess).HasColumnType("nvarchar(max)");
                nb.Property(p => p.CommunicationPlan).HasColumnType("nvarchar(max)");
                nb.Property(p => p.DefaultHandlingStrategy).HasColumnType("nvarchar(max)");
                nb.Property(p => p.PricingStrategy).HasColumnType("nvarchar(max)");
            });


            b.HasKey(x => x.LenderApplicationId);
            b.Property(x => x.RowVersion)
             .IsRowVersion()
             .IsConcurrencyToken();
            
        }
    }
}
