using MoneyMarket.Domain.Abstractions;

namespace MoneyMarket.Domain.Events;

public record LoanDeclinedEvent(Guid LoanId, string Reason) : IDomainEvent;
