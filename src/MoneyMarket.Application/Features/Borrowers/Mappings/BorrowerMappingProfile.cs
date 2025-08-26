namespace MoneyMarket.Application.Features.Borrowers.Mappings
{
    using AutoMapper;
    using MoneyMarket.Application.Features.Borrowers.Dtos;
    using MoneyMarket.Domain.Borrowers;

    public sealed class BorrowerMappingProfile : Profile
    {
        public BorrowerMappingProfile()
        {
            // DTO -> Value Objects (Domain)
            CreateMap<AddressDto, Address>()
                .ConstructUsing(d => new Address(d.House, d.Street, d.City, d.Country, d.PostCode));

            CreateMap<EmploymentInfoDto, EmploymentInfo>()
                .ConstructUsing(d => new EmploymentInfo(
                    d.EmployerName,
                    d.JobTitle,
                    d.LengthOfEmployment,
                    d.GrossAnnualIncome,
                    d.AdditionalSources
                ))
                // Make validator happy: explicitly map differently-named property
                .ForMember(dest => dest.AdditionalIncomeSources,
                    opt => opt.MapFrom(src => src.AdditionalSources));

            CreateMap<DebtItemDto, ExistingDebt>()
                .ConstructUsing(d => new ExistingDebt(d.LenderName, d.DebtType, d.Amount))
                // Id is generated in the ctor; explicitly ignore
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // Value Objects (Domain) -> DTOs
            CreateMap<Address, AddressDto>();

            CreateMap<EmploymentInfo, EmploymentInfoDto>()
                .ForCtorParam("AdditionalSources",
                    opt => opt.MapFrom(src => src.AdditionalIncomeSources));

            CreateMap<ExistingDebt, DebtItemDto>();

            // Extra view DTOs (documents & audit)
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

            // Domain -> View DTO (single map; no duplicates)
            CreateMap<BorrowerProfile, BorrowerProfileViewDto>()
                .ForCtorParam("FullName", o => o.MapFrom(s => $"{s.FirstName} {s.LastName}"))
                .ForCtorParam("Address", o => o.MapFrom(s => s.CurrentAddress))
                .ForCtorParam("Employment", o => o.MapFrom(s => s.Employment))
                .ForCtorParam("Debts", o => o.MapFrom(s => s.Debts))
                .ForCtorParam("Status", o => o.MapFrom(s => s.Status.ToString()))
                .ForCtorParam("DateCreated", o => o.MapFrom(s => s.CreatedAtUtc))
                .ForCtorParam("LastUpdated", o => o.MapFrom(s => s.UpdatedAtUtc))
                .ForCtorParam("PhotoPath", o => o.MapFrom(s => s.PhotoPath))
                .ForCtorParam("Documents", o => o.MapFrom(s => s.Documents))
                .ForCtorParam("Audit", o => o.MapFrom(s => s.AuditTrail));


        }
    }

}
