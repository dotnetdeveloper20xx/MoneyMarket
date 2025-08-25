using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyMarket.Application.Features.Auth.Dtos;
using MoneyMarket.Application.Features.Auth.Queries.GetCurrentUser;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    public UsersController(IMediator mediator) => _mediator = mediator;

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<MeDto>> Me(CancellationToken ct)
        => Ok(await _mediator.Send(new GetCurrentUserQuery(), ct));
}
