using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyMarket.Application.Features.Lenders.Commands;
using MoneyMarket.Application.Features.Lenders.Dtos;
using MoneyMarket.Domain.Common;
using MoneyMarket.Domain.Lenders;

namespace MoneyMarket.Api.Controllers
{
    [ApiController]
    [Route("api/lenders/application")]
    [Authorize(Roles = Roles.Lender)] // must be logged in as Lender to apply
    public sealed class LenderApplicationController : ControllerBase
    {
        private readonly IMediator _mediator;
        public LenderApplicationController(IMediator mediator) => _mediator = mediator;

        [HttpPost("business")]
        public Task<LenderApplicationSummaryDto> UpsertBusiness([FromBody] UpsertBusinessRegistrationDto dto, CancellationToken ct)
            => _mediator.Send(new UpsertLenderBusinessCommand(dto), ct);

        [HttpPost("financial")]
        public Task<LenderApplicationSummaryDto> UpsertFinancial([FromBody] UpsertFinancialCapacityDto dto, CancellationToken ct)
            => _mediator.Send(new UpsertLenderFinancialCommand(dto), ct);

        [HttpPost("risk")]
        public Task<LenderApplicationSummaryDto> UpsertRisk([FromBody] UpsertRiskManagementDto dto, CancellationToken ct)
            => _mediator.Send(new UpsertLenderRiskCommand(dto), ct);

        [HttpPost("submit")]
        public Task<LenderApplicationSummaryDto> Submit(CancellationToken ct)
            => _mediator.Send(new SubmitLenderApplicationCommand(), ct);

        [HttpGet("me")]
        public Task<LenderApplication?> GetMine(CancellationToken ct)
            => _mediator.Send(new GetMyLenderApplicationQuery(), ct);
    }

}
