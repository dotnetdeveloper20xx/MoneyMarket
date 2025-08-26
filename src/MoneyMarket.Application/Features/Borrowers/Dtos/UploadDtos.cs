using MoneyMarket.Domain.Borrowers;

namespace MoneyMarket.Application.Features.Borrowers.Dtos
{
    public sealed record UploadProfilePhotoDto(Stream Content, string FileName, string ContentType);
    public sealed record UploadDocumentDto(DocumentType Type, Stream Content, string FileName, string ContentType);

    public sealed record ReviewProfileDto(bool Approve, string? Reason);
}
