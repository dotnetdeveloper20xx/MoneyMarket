namespace MoneyMarket.Infrastructure.Ids
{
    public sealed class GuidGenerator : IGuidGenerator
    {
        public Guid NewGuid() => Guid.NewGuid();
    }
}
