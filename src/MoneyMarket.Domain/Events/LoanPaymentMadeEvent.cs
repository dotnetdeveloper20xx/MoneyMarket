using MoneyMarket.Domain.Abstractions;

namespace MoneyMarket.Domain.Events;

public record LoanPaymentMadeEvent(Guid LoanId, Guid InstallmentId, decimal Amount) : IDomainEvent;
