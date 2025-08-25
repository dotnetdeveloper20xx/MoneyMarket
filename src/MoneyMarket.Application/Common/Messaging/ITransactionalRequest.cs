namespace MoneyMarket.Application.Common.Messaging;

/// <summary>
/// Marker for commands that should automatically commit in UnitOfWorkBehavior.
/// </summary>
public interface ITransactionalRequest { }
