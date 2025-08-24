using MoneyMarket.Domain.Abstractions;

namespace MoneyMarket.Domain.Events;

public record LoanSubmittedEvent(Guid LoanId, Guid BorrowerId, decimal RequestedAmount) : IDomainEvent;
