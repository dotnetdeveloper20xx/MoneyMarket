namespace MoneyMarket.Application.Features.Borrowers.Dtos
{
    public sealed record EmploymentInfoDto(
     string EmployerName,
     string JobTitle,
     string LengthOfEmployment,
     decimal GrossAnnualIncome,
     string? AdditionalSources
 );
}
