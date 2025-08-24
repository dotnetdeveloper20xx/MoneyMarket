namespace MoneyMarket.Domain.ValueObjects;

public sealed record Address(
    string House,
    string Street,
    string City,
    string Country,
    string PostCode)
{
    public static Address Empty => new("", "", "", "", "");
    public bool IsEmpty() => string.IsNullOrWhiteSpace(House)
                             && string.IsNullOrWhiteSpace(Street)
                             && string.IsNullOrWhiteSpace(City)
                             && string.IsNullOrWhiteSpace(Country)
                             && string.IsNullOrWhiteSpace(PostCode);
}
