using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyMarket.Application.Common.Models;
using MoneyMarket.Application.Features.Lenders.Commands.Products;
using MoneyMarket.Application.Features.Lenders.Dtos.Products;
using MoneyMarket.Application.Features.Lenders.Queries.Products;
using MoneyMarket.Domain.Common;

namespace MoneyMarket.Api.Controllers
{
    [ApiController]
    [Route("api/lenders/products")]
    [Authorize(Roles = Roles.Lender)]
    public sealed class LenderProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public LenderProductsController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public Task<LenderProductViewDto> Create([FromBody] CreateProductDto dto, CancellationToken ct)
            => _mediator.Send(new CreateProductCommand(dto), ct);

        [HttpPut("{productId:guid}")]
        public Task<LenderProductViewDto> Update([FromRoute] Guid productId, [FromBody] CreateProductDto dto, CancellationToken ct)
            => _mediator.Send(new UpdateProductCommand(productId, dto), ct);

        [HttpDelete("{productId:guid}")]
        public Task<bool> Deactivate([FromRoute] Guid productId, CancellationToken ct)
            => _mediator.Send(new DeactivateProductCommand(productId), ct);

        [HttpGet("mine")]
        public Task<PagedResult<LenderProductViewDto>> ListMine([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
            => _mediator.Send(new ListMyProductsQuery(pageNumber, pageSize), ct);
    }

    // Public, for borrowers to browse
    [ApiController]
    [Route("api/products")]
    [AllowAnonymous] // or [Authorize] if you prefer
    public sealed class PublicProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PublicProductsController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public Task<PagedResult<LenderProductViewDto>> List([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
            => _mediator.Send(new ListPublicProductsQuery(pageNumber, pageSize), ct);
    }

}
