using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Common.Messaging;
using ITransactionalRequest = MoneyMarket.Application.Common.Abstractions.ITransactionalRequest;

namespace MoneyMarket.Application.Common.Behaviors;

public sealed class UnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IUnitOfWork _uow;

    public UnitOfWorkBehavior(IUnitOfWork uow) => _uow = uow;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var response = await next();
        if (request is ITransactionalRequest)
            await _uow.SaveChangesAsync(ct);
        return response;
    }
}
