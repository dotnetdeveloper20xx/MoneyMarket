
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyMarket.Application.Features.Borrowers.Commands;
using MoneyMarket.Application.Features.Borrowers.Dtos;
using MoneyMarket.Application.Features.Borrowers.Queries;


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
}
