using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyMarket.Application.Common.Abstractions
{
    /// <summary>
    /// Abstraction for current user context. Application/Handlers depend on this,
    /// not on ASP.NET types or HttpContext.
    /// </summary>
    public interface ICurrentUserService
    {
        bool IsAuthenticated { get; }
        string? UserId { get; }
        string? Email { get; }
        IReadOnlyList<string> Roles { get; }

        bool IsInRole(string role);
        string GetRequiredUserId(); // throws if not authenticated
    }
}
