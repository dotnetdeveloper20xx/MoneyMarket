using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyMarket.Api.Contracts.Fundings;
using MoneyMarket.Application.Common.Models;
using MoneyMarket.Application.Features.Fundings.Commands.FundLoan;
using MoneyMarket.Application.Features.Fundings.DTOs;
using MoneyMarket.Application.Features.Fundings.Queries.MyFundings;
using MoneyMarket.Domain.Entities;
using System.Security.Claims;

namespace MoneyMarket.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FundingsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public FundingsController(IMediator mediator) => _mediator = mediator;

        [Authorize(Roles = "Lender")]
        [HttpPost("{loanId:guid}")]
        public async Task<ActionResult<ApiResponse<bool>>> Fund(Guid loanId, [FromBody] FundLoanRequest body, CancellationToken ct)
        {
            var lenderId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var idem = Request.Headers.TryGetValue("Idempotency-Key", out var v) ? v.ToString() : null;
            var cmd = new FundLoanCommand(loanId, lenderId, body.Amount, idem);
            return Ok(await _mediator.Send(cmd, ct));
        }

        [Authorize(Roles = "Lender")]
        [HttpGet("my")]
        public async Task<ActionResult<IReadOnlyList<FundingSummaryDto>>> My(CancellationToken ct)
        {
            var lenderId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return Ok(await _mediator.Send(new MyFundingsQuery(lenderId), ct));
        }
    }
}
