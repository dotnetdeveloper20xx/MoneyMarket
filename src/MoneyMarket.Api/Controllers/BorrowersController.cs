
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyMarket.Application.Features.Borrowers.Commands;
using MoneyMarket.Application.Features.Borrowers.Dtos;
using MoneyMarket.Application.Features.Borrowers.Queries;
using MoneyMarket.Domain.Borrowers;


namespace MoneyMarket.API.Controllers;

[ApiController]
[Route("api/borrowers")]
[Authorize(Roles = "Borrower")]
public sealed class BorrowersController : ControllerBase
{
    private readonly IMediator _mediator;
    public BorrowersController(IMediator mediator) => _mediator = mediator;

    // Step 1 - Personal info (create or update)
    [HttpPost("personal")]
    public async Task<IActionResult> UpsertPersonal([FromBody] PersonalInfoDto dto, CancellationToken ct)
        => Ok(await _mediator.Send(new UpsertPersonalInfoCommand(dto), ct));

    // Step 2 - Financial info
    [HttpPost("financial")]
    public async Task<IActionResult> UpsertFinancial([FromBody] EmploymentInfoDto dto, CancellationToken ct)
        => Ok(await _mediator.Send(new UpsertFinancialInfoCommand(dto), ct));

    // Step 2b - Debts
    [HttpPost("debts")]
    public async Task<IActionResult> UpsertDebts([FromBody] IReadOnlyCollection<DebtItemDto> debts, CancellationToken ct)
        => Ok(await _mediator.Send(new UpsertDebtsCommand(debts), ct));

    // Submit
    [HttpPost("submit")]
    public async Task<IActionResult> Submit(CancellationToken ct)
        => Ok(await _mediator.Send(new SubmitProfileCommand(), ct));

    // Profile page (me)
    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile(CancellationToken ct)
        => Ok(await _mediator.Send(new GetMyProfileQuery(), ct));

    [HttpPost("photo")] 
    [RequestSizeLimit(20_000_000)] // 20 MB request body
    [RequestFormLimits(MultipartBodyLengthLimit = 20_000_000)]
    public async Task<IActionResult> UploadPhoto([FromForm] IFormFile file, CancellationToken ct)
    {
        await using var stream = file.OpenReadStream();
        var cmd = new UploadProfilePhotoCommand(new UploadProfilePhotoDto(stream, file.FileName, file.ContentType));
        return Ok(await _mediator.Send(cmd, ct));
    }

    [HttpPost("documents")]
    [RequestSizeLimit(20_000_000)]
    [RequestFormLimits(MultipartBodyLengthLimit = 20_000_000)]
    public async Task<IActionResult> UploadDocument([FromForm] IFormFile file, [FromForm] DocumentType type, CancellationToken ct)
    {
        await using var stream = file.OpenReadStream();
        var cmd = new UploadBorrowerDocumentCommand(new UploadDocumentDto(type, stream, file.FileName, file.ContentType));
        return Ok(await _mediator.Send(cmd, ct));
    }
}
