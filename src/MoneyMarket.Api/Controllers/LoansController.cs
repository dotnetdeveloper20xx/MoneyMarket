using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyMarket.Api.Contracts.Loans;
using MoneyMarket.Application.Common.Models;
using MoneyMarket.Application.Features.Loans.Commands.ApproveLoan;
using MoneyMarket.Application.Features.Loans.Commands.SubmitLoan;
using MoneyMarket.Application.Features.Loans.Dtos;
using MoneyMarket.Application.Features.Loans.Queries.GetOpenLoans;

namespace MoneyMarket.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : ControllerBase
    {
        private readonly IMediator _mediator;
        public LoansController(IMediator mediator) => _mediator = mediator;

        // Handler should pull BorrowerId from ICurrentUserService, not from the request body.
        [Authorize(Roles = "Borrower")]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Guid>>> Submit([FromBody] SubmitLoanCommand cmd, CancellationToken ct)
            => Ok(await _mediator.Send(cmd, ct));

        [Authorize(Policy = "CanApproveLoan")]
        [HttpPost("{loanId:guid}/approve")]
        public async Task<ActionResult<ApiResponse<bool>>> Approve(Guid loanId, [FromBody] ApproveLoanRequest body, CancellationToken ct)
        {
            var cmd = new ApproveLoanCommand(loanId, body.ApprovedAmount, body.InterestRate, body.Fees);
            var result = await _mediator.Send(cmd, ct);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("open")]
        public async Task<ActionResult<IReadOnlyList<LoanSummaryDto>>> Open([FromQuery] int page = 1, [FromQuery] int size = 20, CancellationToken ct = default)
            => Ok(await _mediator.Send(new GetOpenLoansQuery(page, size), ct));
    }
}
