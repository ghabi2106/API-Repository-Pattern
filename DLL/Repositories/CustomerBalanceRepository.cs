using DLL.DBContext;
using DLL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Repositories
{
    public interface ICustomerBalanceRepository : IBaseRepository<CustomerBalance>
    {
        Task MustUpdateBalanceAsync(string email, decimal amount);
    }

    public class CustomerBalanceRepository : BaseRepository<CustomerBalance>, ICustomerBalanceRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerBalanceRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task MustUpdateBalanceAsync(string email, decimal amount)
        {
            var customerBalance =
                await _context.CustomerBalances.FirstOrDefaultAsync(x => x.Email == "ghabiassaad@gmail.com");
            customerBalance.Balance += amount;
            bool isUpdated = false;
            do
            {
                try
                {

                    if (await _context.SaveChangesAsync() > 0)
                    {
                        isUpdated = true;
                    };
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        if (!(entry.Entity is CustomerBalance)) continue;
                        var databaseEntry = entry.GetDatabaseValues();
                        var databaseValues = (CustomerBalance)databaseEntry.ToObject();
                        databaseValues.Balance += amount;

                        entry.OriginalValues.SetValues(databaseEntry);
                        entry.CurrentValues.SetValues(databaseValues);
                    }
                }
            } while (!isUpdated);
        }
    }
}
