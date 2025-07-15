using CSP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP.Domain.Logics.Interfaces
{
    public interface IMoneyTransferLogic
    {
        Task<List<MoneyTransfer>> GetAllTransfersAsync();
        Task<List<MoneyTransfer>> GetTransfersByReceiver(string receiverId);
        Task CreateMoneyTransferAsync(MoneyTransfer moneyTransfer);
    }
}
