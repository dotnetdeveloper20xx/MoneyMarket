using MoneyMarket.Domain.Abstractions;

namespace MoneyMarket.Domain.Events;

public record LoanApprovedEvent(Guid LoanId, decimal ApprovedAmount, decimal InterestRate, int TermMonths) : IDomainEvent;
