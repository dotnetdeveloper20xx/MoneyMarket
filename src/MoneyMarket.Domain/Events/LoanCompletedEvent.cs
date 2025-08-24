using MoneyMarket.Domain.Abstractions;

namespace MoneyMarket.Domain.Events;

public record LoanCompletedEvent(Guid LoanId) : IDomainEvent;
