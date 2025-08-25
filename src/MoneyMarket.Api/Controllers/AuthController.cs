using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyMarket.Application.Common.Models;
using MoneyMarket.Application.Features.Auth.Commands.Login;
using MoneyMarket.Application.Features.Auth.Commands.RegisterUser;
using MoneyMarket.Application.Features.Auth.Dtos;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    public AuthController(IMediator mediator) => _mediator = mediator;

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<string>>> Register([FromBody] RegisterUserCommand cmd, CancellationToken ct)
        => Ok(await _mediator.Send(cmd, ct));

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthResult>>> Login([FromBody] LoginCommand cmd, CancellationToken ct)
        => Ok(await _mediator.Send(cmd, ct));
}
