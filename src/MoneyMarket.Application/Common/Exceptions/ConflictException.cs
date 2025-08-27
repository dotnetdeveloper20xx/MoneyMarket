namespace MoneyMarket.Application.Common.Exceptions
{
    /// <summary>
    /// Used to indicate a 409 Conflict (e.g., duplicate resource).
    /// </summary>
    public sealed class ConflictException : Exception
    {
        public ConflictException() { }

        public ConflictException(string message) : base(message) { }

        public ConflictException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
