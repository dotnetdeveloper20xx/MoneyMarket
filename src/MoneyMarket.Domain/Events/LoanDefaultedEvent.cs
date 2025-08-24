using MoneyMarket.Domain.Abstractions;

namespace MoneyMarket.Domain.Events;

public record LoanDefaultedEvent(Guid LoanId) : IDomainEvent;
