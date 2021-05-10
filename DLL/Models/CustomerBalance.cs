using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Models
{
    public class CustomerBalance
    {
        public long CustomerBalanceId { get; set; }
        public string Email { get; set; }
        public Decimal Balance { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
