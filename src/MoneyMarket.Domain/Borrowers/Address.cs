namespace MoneyMarket.Domain.Borrowers
{
    public sealed class Address
    {
        public string House { get; private set; } = default!;
        public string Street { get; private set; } = default!;
        public string City { get; private set; } = default!;
        public string Country { get; private set; } = default!;
        public string PostCode { get; private set; } = default!;
        private Address() { }
        public Address(string house, string street, string city, string country, string postCode)
        { House = house; Street = street; City = city; Country = country; PostCode = postCode; }
    }
}
