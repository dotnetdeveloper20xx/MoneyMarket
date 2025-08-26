namespace MoneyMarket.Application.Features.Lenders.Dtos
{
    public sealed record UpsertBusinessRegistrationDto(
     string BusinessName,
     string RegistrationNumber,
     List<string> ProofOfIncorporationDocuments,
     List<string> LendingLicenses,
     string ComplianceStatement);
}
