using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoneyMarket.Application.Features.Lenders.Commands;
using MoneyMarket.Application.Features.Lenders.Dtos;

namespace MoneyMarket.API.Controllers.Lenders
{
    [ApiController]
    [Route("api/lenders/applications")]
    public sealed class LenderApplicationsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public LenderApplicationsController(IMediator mediator) => _mediator = mediator;

        // CREATE (Business Registration) — returns DTO directly (no ApiResponse)
        [HttpPost("business")]
        public Task<LenderApplicationSummaryDto> CreateBusiness(
            [FromBody] UpsertBusinessRegistrationDto dto,
            CancellationToken ct)
            => _mediator.Send(new CreateLenderBusinessCommand(dto), ct);

        // UPDATE (Business Registration) — returns DTO directly (no ApiResponse)
        [HttpPut("business")]
        public Task<LenderApplicationSummaryDto> UpdateBusiness(
            [FromBody] UpsertBusinessRegistrationDto dto,
            CancellationToken ct)
            => _mediator.Send(new UpdateLenderBusinessCommand(dto), ct);

        // FINANCIAL (matches your existing style)
        [HttpPost("financial")]
        public Task<LenderApplicationSummaryDto> UpsertFinancial(
            [FromBody] UpsertFinancialCapacityDto dto,
            CancellationToken ct)
            => _mediator.Send(new UpsertLenderFinancialCommand(dto), ct);

        // Optional: RISK (same style for consistency)
        [HttpPost("risk")]
        public Task<LenderApplicationSummaryDto> UpsertRisk(
            [FromBody] UpsertRiskManagementDto dto,
            CancellationToken ct)
            => _mediator.Send(new UpsertLenderRiskCommand(dto), ct);
    }
}
