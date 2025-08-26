namespace MoneyMarket.Application.Features.Borrowers.Dtos
{
    public sealed record AddressDto(
      string House,
      string Street,
      string City,
      string Country,
      string PostCode
  );
}
