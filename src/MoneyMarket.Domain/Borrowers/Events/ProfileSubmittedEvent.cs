using MediatR;
namespace MoneyMarket.Domain.Borrowers.Events
{
    public sealed record ProfileSubmittedEvent(Guid BorrowerProfileId, string UserId, string Email) : INotification;
}
