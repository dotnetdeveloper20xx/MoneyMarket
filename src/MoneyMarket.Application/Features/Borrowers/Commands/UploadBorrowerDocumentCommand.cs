using MediatR;
using MoneyMarket.Application.Common.Models;
using MoneyMarket.Application.Features.Borrowers.Dtos;

namespace MoneyMarket.Application.Features.Borrowers.Commands
{
    public sealed record UploadBorrowerDocumentCommand(UploadDocumentDto File)
       : IRequest<ApiResponse<string>>;
}
