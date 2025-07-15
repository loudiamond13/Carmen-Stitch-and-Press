using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP.Domain.Entities
{
    public class MoneyTransfer
    {
        public int MoneyTransfersId { get; set; } = 0;

        public string TransferFrom { get; set; }

        public string TransferTo { get; set; }

        public DateTime TransferDate { get; set; } = DateTime.Now;

        public decimal TransferAmount { get; set; }
    }
}
