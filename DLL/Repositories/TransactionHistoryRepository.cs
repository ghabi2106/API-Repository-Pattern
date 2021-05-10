using DLL.DBContext;
using DLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Repositories
{
    public interface ITransactionHistoryRepository : IBaseRepository<TransactionHistory>
    {

    }

    public class TransactionHistoryRepository : BaseRepository<TransactionHistory>, ITransactionHistoryRepository
    {
        public TransactionHistoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
