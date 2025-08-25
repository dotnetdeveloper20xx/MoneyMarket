using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyMarket.Api.Common.Extensions;
using MoneyMarket.Application.Features.Auth.Dtos;
using MoneyMarket.Application.Features.Auth.Queries.GetCurrentUser;
using MoneyMarket.Domain.Entities;
using System.Security.Claims;

namespace MoneyMarket.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator) => _mediator = mediator;

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<MeDto>> Me(CancellationToken ct)
        {
            var id = User.UserId();
            if (string.IsNullOrEmpty(id)) return Unauthorized("Missing user id claim.");

            var email = User.Email() ?? string.Empty; 
            var roles = User.Roles();

            return Ok(await _mediator.Send(new GetCurrentUserQuery(id, email, roles), ct));
        }
    }
}
