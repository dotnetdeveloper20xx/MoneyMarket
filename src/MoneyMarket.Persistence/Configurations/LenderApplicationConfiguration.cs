using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyMarket.Domain.Lenders;
using MoneyMarket.Persistence.Identity;

namespace MoneyMarket.Persistence.Configurations
{
    public sealed class LenderApplicationConfiguration : IEntityTypeConfiguration<LenderApplication>
    {
        public void Configure(EntityTypeBuilder<LenderApplication> b)
        {
            b.ToTable("LenderApplications");

            b.HasKey(x => x.LenderApplicationId);

            b.Property(x => x.Email).IsRequired().HasMaxLength(256);
            b.Property(x => x.Status).HasConversion<int>();

            // Concurrency
            b.Property(x => x.RowVersion)
             .IsRowVersion()
             .IsConcurrencyToken();

            // FK -> AspNetUsers(Id)
            b.HasIndex(x => x.UserId).HasDatabaseName("IX_LenderApplications_UserId");

            b.HasOne<ApplicationUser>()
             .WithMany()
             .HasForeignKey(x => x.UserId)
             .HasConstraintName("FK_LenderApplications_AspNetUsers_UserId")
             .OnDelete(DeleteBehavior.Restrict);

            // Owned: BusinessRegistration
            b.OwnsOne(x => x.BusinessRegistration, nb =>
            {
                nb.Property(p => p.BusinessName).IsRequired().HasMaxLength(256);
                nb.Property(p => p.RegistrationNumber).IsRequired().HasMaxLength(128);

                nb.Property(p => p.ProofOfIncorporationDocuments)
                  .HasColumnType("nvarchar(max)")
                  .HasConversion(JsonListConverters.ReadOnlyStringListToJsonConverter)
                  .Metadata.SetValueComparer(JsonListConverters.ReadOnlyStringListComparer);

                nb.Property(p => p.LendingLicenses)
                  .HasColumnType("nvarchar(max)")
                  .HasConversion(JsonListConverters.ReadOnlyStringListToJsonConverter)
                  .Metadata.SetValueComparer(JsonListConverters.ReadOnlyStringListComparer);

                nb.Property(p => p.ComplianceStatement)
                  .HasColumnType("nvarchar(max)");
            });

            // Owned: FinancialCapacity
            b.OwnsOne(x => x.FinancialCapacity, nb =>
            {
                nb.Property(p => p.FundingSourceType).IsRequired().HasMaxLength(100);
                nb.Property(p => p.FundingSourceDescription).HasColumnType("nvarchar(max)");

                nb.Property(p => p.CapitalReserveDocuments)
                  .HasColumnType("nvarchar(max)")
                  .HasConversion(JsonListConverters.ReadOnlyStringListToJsonConverter)
                  .Metadata.SetValueComparer(JsonListConverters.ReadOnlyStringListComparer);
            });

            // Owned: RiskManagement
            b.OwnsOne(x => x.RiskManagement, nb =>
            {
                nb.Property(p => p.UnderwritingPolicy).HasColumnType("nvarchar(max)");

                nb.Property(p => p.RiskAssessmentTools)
                  .HasColumnType("nvarchar(max)")
                  .HasConversion(JsonListConverters.ReadOnlyStringListToJsonConverter)
                  .Metadata.SetValueComparer(JsonListConverters.ReadOnlyStringListComparer);

                nb.Property(p => p.PaymentCollectionProcess).HasColumnType("nvarchar(max)");
                nb.Property(p => p.CommunicationPlan).HasColumnType("nvarchar(max)");
                nb.Property(p => p.DefaultHandlingStrategy).HasColumnType("nvarchar(max)");
                nb.Property(p => p.PricingStrategy).HasColumnType("nvarchar(max)");
            });
        }
    }
}
