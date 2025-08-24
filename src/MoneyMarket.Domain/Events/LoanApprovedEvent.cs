using MoneyMarket.Domain.Abstractions;

namespace MoneyMarket.Domain.Events;

public record LoanApprovedEvent(Guid LoanId, Guid LenderId, decimal ApprovedAmount, decimal InterestRate, int TermMonths) : IDomainEvent;
