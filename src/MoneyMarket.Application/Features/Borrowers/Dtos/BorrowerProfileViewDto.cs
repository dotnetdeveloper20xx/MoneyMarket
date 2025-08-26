namespace MoneyMarket.Application.Features.Borrowers.Dtos
{
    public sealed record BorrowerProfileViewDto(
     Guid Id,
     string FullName,
     DateTime DateOfBirth,
     int Age,
     string NationalIdNumber,
     AddressDto Address,
     string PhoneNumber,
     string Email,
     EmploymentInfoDto? Employment,
     IReadOnlyCollection<DebtItemDto> Debts,
     string Status,
     DateTime DateCreated,
     DateTime LastUpdated,
     string? PhotoPath,
     IReadOnlyCollection<BorrowerDocumentViewDto> Documents,
     IReadOnlyCollection<AuditEntryViewDto> Audit
 );

    public sealed record BorrowerDocumentViewDto(string FileName, string Url, string Type, DateTime UploadedAt);
    public sealed record AuditEntryViewDto(string Action, string? OldStatus, string? NewStatus, string? Reason, string PerformedBy, DateTime At);


}
