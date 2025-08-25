using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyMarket.Application.Features.Auth.Dtos
{
    public sealed record MeDto(string UserId, string Email, string[] Roles);
}
