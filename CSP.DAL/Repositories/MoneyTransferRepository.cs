using CSP.DAL.DbContexts;
using CSP.DAL.Repositories.Base;
using CSP.Domain.Entities;
using CSP.Domain.IRepositories;
using CSP.Domain.IRepositories.IBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP.DAL.Repositories
{
    public class MoneyTransferRepository :GenericRepository<CarmenStitchAndPressDBContext, MoneyTransfer>, IMoneyTransferRepository
    {
        public MoneyTransferRepository(CarmenStitchAndPressDBContext carmenStitchAndPressDBContext):base(carmenStitchAndPressDBContext)
        {
            
        }
    }
}
