using Microsoft.EntityFrameworkCore;
using MoneyMarket.Domain.Entities;
using System.Collections.Generic;

namespace MoneyMarket.Application.Common.Abstractions;

public interface IAppDbContext
{
    DbSet<Loan> Loans { get; }
    DbSet<Borrower> Borrowers { get; }
    DbSet<Lender> Lenders { get; }
    DbSet<RepaymentInstallment> RepaymentInstallments { get; }
    DbSet<Funding> Fundings { get; } 
    Task<int> SaveChangesAsync(CancellationToken ct = default);

}
