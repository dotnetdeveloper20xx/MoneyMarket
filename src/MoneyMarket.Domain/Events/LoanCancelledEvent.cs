using MoneyMarket.Domain.Abstractions;

namespace MoneyMarket.Domain.Events;

public record LoanCancelledEvent(Guid LoanId) : IDomainEvent;
