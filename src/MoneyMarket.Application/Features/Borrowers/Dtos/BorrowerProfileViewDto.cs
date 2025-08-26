namespace MoneyMarket.Application.Features.Borrowers.Dtos
{
    public sealed record BorrowerProfileViewDto(
      Guid Id,
      string FullName,
      DateTime DateOfBirth,
      int Age,
      string NationalIdNumber,
      AddressDto Address,
      string PhoneNumber,
      string Email,
      EmploymentInfoDto? Employment,
      IReadOnlyCollection<DebtItemDto> Debts,
      string Status,
      DateTime DateCreated,
      DateTime LastUpdated
  );
}
