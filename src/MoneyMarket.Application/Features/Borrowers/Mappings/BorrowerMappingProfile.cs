using AutoMapper;
using MoneyMarket.Application.Features.Borrowers.Dtos;
using MoneyMarket.Domain.Borrowers;

namespace MoneyMarket.Application.Features.Borrowers.Mappings
{
    public sealed class BorrowerMappingProfile : Profile
    {
        public BorrowerMappingProfile()
        {
            // DTO -> Value Objects
            CreateMap<AddressDto, Address>()
                .ConstructUsing(d => new Address(d.House, d.Street, d.City, d.Country, d.PostCode));

            CreateMap<EmploymentInfoDto, EmploymentInfo>()
                .ConstructUsing(d => new EmploymentInfo(d.EmployerName, d.JobTitle, d.LengthOfEmployment, d.GrossAnnualIncome, d.AdditionalSources));

            CreateMap<DebtItemDto, ExistingDebt>()
                .ConstructUsing(d => new ExistingDebt(d.LenderName, d.DebtType, d.Amount));

            // Domain -> View DTO
            CreateMap<Address, AddressDto>();
            CreateMap<EmploymentInfo, EmploymentInfoDto>()
                .ForCtorParam("AdditionalSources", opt => opt.MapFrom(src => src.AdditionalIncomeSources));

            CreateMap<ExistingDebt, DebtItemDto>();

            CreateMap<BorrowerProfile, BorrowerProfileViewDto>()
                .ForCtorParam("FullName", opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForCtorParam("Address", opt => opt.MapFrom(src => src.CurrentAddress))
                .ForCtorParam("Employment", opt => opt.MapFrom(src => src.Employment))
                .ForCtorParam("Debts", opt => opt.MapFrom(src => src.Debts))
                .ForCtorParam("Status", opt => opt.MapFrom(src => src.Status.ToString()))
                .ForCtorParam("DateCreated", opt => opt.MapFrom(src => src.CreatedAtUtc))
                .ForCtorParam("LastUpdated", opt => opt.MapFrom(src => src.UpdatedAtUtc));

            CreateMap<BorrowerDocument, /* optional view dto if you need */ object>();
            // PhotoPath is already on profile; no special mapping needed unless you add to view DTO

            CreateMap<BorrowerDocument, BorrowerDocumentViewDto>()
                .ForCtorParam("FileName", o => o.MapFrom(s => s.FileName))
                .ForCtorParam("Url", o => o.MapFrom(s => s.StoragePath))
                .ForCtorParam("Type", o => o.MapFrom(s => s.Type.ToString()))
                .ForCtorParam("UploadedAt", o => o.MapFrom(s => s.UploadedAtUtc));

            CreateMap<AuditTrailEntry, AuditEntryViewDto>()
                .ForCtorParam("Action", o => o.MapFrom(s => s.Action))
                .ForCtorParam("OldStatus", o => o.MapFrom(s => s.OldStatus.HasValue ? s.OldStatus.Value.ToString() : null))
                .ForCtorParam("NewStatus", o => o.MapFrom(s => s.NewStatus.HasValue ? s.NewStatus.Value.ToString() : null))
                .ForCtorParam("Reason", o => o.MapFrom(s => s.Reason))
                .ForCtorParam("PerformedBy", o => o.MapFrom(s => s.PerformedBy))
                .ForCtorParam("At", o => o.MapFrom(s => s.OccurredAtUtc));

            CreateMap<BorrowerProfile, BorrowerProfileViewDto>()
                // previous mappings...
                .ForCtorParam("PhotoPath", o => o.MapFrom(s => s.PhotoPath))
                .ForCtorParam("Documents", o => o.MapFrom(s => s.Documents))
                .ForCtorParam("Audit", o => o.MapFrom(s => s.AuditTrail));

        }
    }
}
