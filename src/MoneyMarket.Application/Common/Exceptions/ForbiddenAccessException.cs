namespace MoneyMarket.Application.Common.Exceptions
{
    public sealed class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException() { }

        public ForbiddenAccessException(string message) : base(message) { }
    }
}
