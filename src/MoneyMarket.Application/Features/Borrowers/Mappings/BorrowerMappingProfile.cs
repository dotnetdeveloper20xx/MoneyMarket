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
        }
    }
}
