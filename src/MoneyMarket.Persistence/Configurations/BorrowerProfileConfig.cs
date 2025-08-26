using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyMarket.Domain.Borrowers;

namespace MoneyMarket.Persistence.Configurations
{
    public sealed class BorrowerProfileConfig : IEntityTypeConfiguration<BorrowerProfile>
    {
        public void Configure(EntityTypeBuilder<BorrowerProfile> b)
        {
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.UserId).IsUnique();

            b.Property(x => x.FirstName).HasMaxLength(150).IsRequired();
            b.Property(x => x.LastName).HasMaxLength(150).IsRequired();
            b.Property(x => x.NationalIdNumber).HasMaxLength(150).IsRequired();
            b.Property(x => x.PhoneNumber).HasMaxLength(150).IsRequired();
            b.Property(x => x.Email).HasMaxLength(256).IsRequired();

            b.OwnsOne(x => x.CurrentAddress, a =>
            {
                a.Property(p => p.House).HasMaxLength(150).IsRequired();
                a.Property(p => p.Street).HasMaxLength(150).IsRequired();
                a.Property(p => p.City).HasMaxLength(150).IsRequired();
                a.Property(p => p.Country).HasMaxLength(150).IsRequired();
                a.Property(p => p.PostCode).HasMaxLength(50).IsRequired();
            });

            b.OwnsOne(x => x.Employment, e =>
            {
                e.Property(p => p.EmployerName).HasMaxLength(200);
                e.Property(p => p.JobTitle).HasMaxLength(150);
                e.Property(p => p.LengthOfEmployment).HasMaxLength(100);
                e.Property(p => p.GrossAnnualIncome).HasColumnType("decimal(18,2)");
                e.Property(p => p.AdditionalIncomeSources).HasMaxLength(2000);
            });

            b.Navigation(x => x.CurrentAddress).IsRequired();
            b.Navigation(x => x.Employment).IsRequired(false);

            // Debts (owned collection)
            b.OwnsMany(x => x.Debts, d =>
            {
                d.ToTable("BorrowerDebts");
                d.WithOwner().HasForeignKey("BorrowerProfileId");
                d.HasKey(x => x.Id);
                d.Property(x => x.Id).ValueGeneratedNever();
                d.Property(p => p.LenderName).HasMaxLength(200).IsRequired();
                d.Property(p => p.DebtType).HasMaxLength(100).IsRequired();
                d.Property(p => p.Amount).HasColumnType("decimal(18,2)");
                d.HasIndex("BorrowerProfileId");
               
            });

            b.Property(x => x.PhotoPath).HasMaxLength(1024);

            // Documents (owned collection)
            b.OwnsMany(x => x.Documents, d =>
            {
                d.ToTable("BorrowerDocuments");
                d.WithOwner().HasForeignKey("BorrowerProfileId");
                d.HasKey(x => x.Id);
                d.Property(x => x.Id).ValueGeneratedNever();
                d.Property(x => x.Type).HasConversion<int>();
                d.Property(x => x.FileName).HasMaxLength(256).IsRequired();
                d.Property(x => x.StoragePath).HasMaxLength(1024).IsRequired();
                d.Property(x => x.UploadedAtUtc);
                d.HasIndex("BorrowerProfileId");
               
            });

            // Audit trail (owned collection)
            b.OwnsMany(x => x.AuditTrail, a =>
            {
                a.ToTable("BorrowerAuditTrail");
                a.WithOwner().HasForeignKey("BorrowerProfileId");
                a.HasKey(x => x.Id);
                a.Property(x => x.Id).ValueGeneratedNever();
                a.Property(x => x.Action).HasMaxLength(64);
                a.Property(x => x.Reason).HasMaxLength(2000);
                a.Property(x => x.PerformedBy).HasMaxLength(256);
                a.Property(x => x.OccurredAtUtc);
                a.Property(x => x.OldStatus).HasConversion<int?>();
                a.Property(x => x.NewStatus).HasConversion<int?>();
               
            });
      
        }
    }
}
