using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyMarket.Application.Common.Models;
using MoneyMarket.Application.Features.CRM.Commands;
using MoneyMarket.Application.Features.CRM.Dtos;
using MoneyMarket.Application.Features.CRM.Queries;
using MoneyMarket.Domain.Common;

namespace MoneyMarket.Api.Controllers
{
    [ApiController]
    [Route("api/crm/lenders")]
    [Authorize(Roles = Roles.CreditRiskManager)]
    public sealed class CrmLendersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CrmLendersController(IMediator mediator) => _mediator = mediator;


        [HttpGet]
        public async Task<ActionResult<PagedResult<LenderRowDto>>> List([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _mediator.Send(new ListLendersPagedQuery(pageNumber, pageSize));
            return Ok(result);
        }


        [HttpGet("{id:guid}")]
        public async Task<ActionResult<LenderDetailsDto>> Details([FromRoute] Guid id)
        {
            var dto = await _mediator.Send(new GetLenderDetailsQuery(id));
            if (dto is null) return NotFound();
            return Ok(dto);
        }


        public sealed record DisableRequest(string? Reason);


        [HttpPost("{id:guid}/disable")]
        public async Task<ActionResult> Disable([FromRoute] Guid id, [FromBody] DisableRequest body)
        {
            var ok = await _mediator.Send(new DisableLenderCommand(id, body?.Reason));
            return ok ? NoContent() : NotFound();
        }
    }
}
