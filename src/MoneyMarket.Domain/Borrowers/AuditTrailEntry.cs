namespace MoneyMarket.Domain.Borrowers
{
    public sealed class AuditTrailEntry
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Action { get; private set; } = default!; // Submitted/Approved/Rejected
        public ProfileStatus? OldStatus { get; private set; }
        public ProfileStatus? NewStatus { get; private set; }
        public string? Reason { get; private set; }
        public string PerformedBy { get; private set; } = default!; // userId or system
        public DateTime OccurredAtUtc { get; private set; }
        private AuditTrailEntry() { }
        public AuditTrailEntry(string action, ProfileStatus? oldStatus, ProfileStatus? newStatus, string? reason, string performedBy, DateTime at)
        { Action = action; OldStatus = oldStatus; NewStatus = newStatus; Reason = reason; PerformedBy = performedBy; OccurredAtUtc = at; }
    }
}
