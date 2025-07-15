using CSP.Domain.Entities;
using CSP.Domain.IRepositories;
using CSP.Domain.Logics.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP.Domain.Logics
{
    public class MoneyTransferLogic:IMoneyTransferLogic
    {
        public readonly IMoneyTransferRepository _moneyTransferRepository;

        public MoneyTransferLogic(IMoneyTransferRepository moneyTransferRepository)
        {
            _moneyTransferRepository = moneyTransferRepository;
        }

        #region get money transfers by receiver
        public async Task<List<MoneyTransfer>> GetTransfersByReceiver(string email) 
        {
            List<MoneyTransfer> transfers = await _moneyTransferRepository
                                                .FindByAsync(x => x.TransferTo.Trim().Equals(email.Trim(), StringComparison.CurrentCultureIgnoreCase));

            return transfers;
        }
        #endregion
        #region create transfer
        public async Task CreateMoneyTransferAsync(MoneyTransfer moneyTransfer) 
        {
            if (moneyTransfer != null) 
            {
                await _moneyTransferRepository.AddAsync(moneyTransfer);
                await _moneyTransferRepository.SaveAsync();
            }
        }
        #endregion
        #region get all money transfers
        public async Task<List<MoneyTransfer>> GetAllTransfersAsync() 
        {
            return await _moneyTransferRepository.GetAllAsync();
        }
        #endregion
    }
}
