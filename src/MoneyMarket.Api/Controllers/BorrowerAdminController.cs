using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyMarket.Application.Features.Borrowers.Commands;
using MoneyMarket.Application.Features.Borrowers.Dtos;

namespace MoneyMarket.Api.Controllers
{
    [ApiController]
    [Route("api/admin/borrowers")]
    [Authorize(Roles = "Admin,CRM")]
    public sealed class BorrowerAdminController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BorrowerAdminController(IMediator mediator) => _mediator = mediator;

        [HttpPost("{profileId:guid}/review")]
        public async Task<IActionResult> Review(Guid profileId, [FromBody] ReviewProfileDto dto, CancellationToken ct)
            => Ok(await _mediator.Send(new ReviewBorrowerProfileCommand(profileId, dto.Approve, dto.Reason), ct));
    }
}
