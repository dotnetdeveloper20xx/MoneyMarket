using MoneyMarket.Domain.Abstractions;

namespace MoneyMarket.Domain.Events;

public record LoanFundedEvent(Guid LoanId, decimal Amount) : IDomainEvent;
