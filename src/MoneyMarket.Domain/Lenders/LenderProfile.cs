namespace MoneyMarket.Domain.Lenders
{
    public sealed class LenderProfile
    {
        public Guid Id { get; private set; }
        public string UserId { get; private set; } = default!;
        public string Email { get; private set; } = default!;
        public string? DisplayName { get; private set; }


        // CRM control
        public bool IsDisabled { get; private set; }
        public string? DisabledReason { get; private set; }
        public DateTime? DisabledAtUtc { get; private set; }


        private LenderProfile() { }


        public LenderProfile(string userId, string email, string? displayName = null)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Email = email;
            DisplayName = displayName;
        }


        public void Disable(string? reason)
        {
            if (IsDisabled) return;
            IsDisabled = true;
            DisabledReason = reason;
            DisabledAtUtc = DateTime.UtcNow;
        }


        public void Enable()
        {
            if (!IsDisabled) return;
            IsDisabled = false;
            DisabledReason = null;
            DisabledAtUtc = null;
        }
    }
}
