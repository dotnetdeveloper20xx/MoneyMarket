namespace MoneyMarket.Domain.Enums;

public enum LoanStatus
{
    Draft = 0,
    Submitted = 1,
    UnderReview = 2,
    Approved = 3,
    PendingFunding = 4,
    Funded = 5,
    Active = 6,
    InArrears = 7,
    Restructured = 8,
    Completed = 9,
    Declined = 10,
    Defaulted = 11,
    Cancelled = 12
}
