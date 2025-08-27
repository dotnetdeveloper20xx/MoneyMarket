using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyMarket.Application.Features.Lenders.Commands;
using MoneyMarket.Application.Features.Lenders.Dtos;

namespace MoneyMarket.Api.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/admin/lenders/applications")]
    public sealed class AdminLenderApplicationsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AdminLenderApplicationsController(IMediator mediator) => _mediator = mediator;

        [HttpPost("{applicationId:guid}/approve")]
        public Task<LenderApplicationSummaryDto> Approve(Guid applicationId, CancellationToken ct)
            => _mediator.Send(new ApproveLenderApplicationCommand(applicationId), ct);

        public sealed record RejectBody(string Reason);

        [HttpPost("{applicationId:guid}/reject")]
        public Task<LenderApplicationSummaryDto> Reject(Guid applicationId, [FromBody] RejectBody body, CancellationToken ct)
            => _mediator.Send(new RejectLenderApplicationCommand(applicationId, body.Reason), ct);
    }
}
