using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyMarket.Application.Features.Lenders.Commands;
using MoneyMarket.Application.Features.Lenders.Dtos;
using MoneyMarket.Application.Features.Lenders.Queries;

namespace MoneyMarket.Api.Controllers
{
    [ApiController]
    [Authorize(Roles = "Lender")] // user gets this role on approval
    [Route("api/lenders/profile")]
    public sealed class LenderProfilesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public LenderProfilesController(IMediator mediator) => _mediator = mediator;

        [HttpGet("me")]
        public Task<LenderProfileDto?> GetMine(CancellationToken ct)
            => _mediator.Send(new GetMyLenderProfileQuery(), ct);

        public sealed record SetPhotoPathBody(string? PhotoPath);

        [HttpPut("photo-path")]
        public Task<LenderProfileDto> SetPhotoPath([FromBody] SetPhotoPathBody body, CancellationToken ct)
            => _mediator.Send(new SetLenderPhotoPathCommand(body.PhotoPath), ct);
    }
}
