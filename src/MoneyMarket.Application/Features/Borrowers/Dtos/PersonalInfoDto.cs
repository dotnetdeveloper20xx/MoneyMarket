namespace MoneyMarket.Application.Features.Borrowers.Dtos
{
    public sealed record PersonalInfoDto(
     string FirstName,
     string LastName,
     DateTime DateOfBirth,
     string NationalIdNumber,
     AddressDto Address,
     string PhoneNumber,
     string Email
 );
}
