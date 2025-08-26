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
    [Route("api/crm/borrowers")]
    [Authorize(Roles = Roles.CreditRiskManager)]
    public sealed class CrmBorrowersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CrmBorrowersController(IMediator mediator) => _mediator = mediator;


        [HttpGet]
        public async Task<ActionResult<PagedResult<BorrowerRowDto>>> List([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _mediator.Send(new ListBorrowersPagedQuery(pageNumber, pageSize));
            return Ok(result);
        }


        [HttpGet("{id:guid}")]
        public async Task<ActionResult<BorrowerDetailsDto>> Details([FromRoute] Guid id)
        {
            var dto = await _mediator.Send(new GetBorrowerDetailsQuery(id));
            if (dto is null) return NotFound();
            return Ok(dto);
        }


        public sealed record DisableRequest(string? Reason);


        [HttpPost("{id:guid}/disable")]
        public async Task<ActionResult> Disable([FromRoute] Guid id, [FromBody] DisableRequest body)
        {
            var ok = await _mediator.Send(new DisableBorrowerCommand(id, body?.Reason));
            return ok ? NoContent() : NotFound();
        }
    }
}
