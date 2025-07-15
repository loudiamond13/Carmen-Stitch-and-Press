using CSP.DAL.DbContexts;
using CSP.DAL.Repositories.Base;
using CSP.Domain.Entities;
using CSP.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP.DAL.Repositories
{
    public class OrderRepository: GenericRepository<CarmenStitchAndPressDBContext, Order>, IOrderRepository
    {
        public OrderRepository(CarmenStitchAndPressDBContext carmenStitchAndPressDBContext) : base(carmenStitchAndPressDBContext)
        {
            
        }
    }
}
