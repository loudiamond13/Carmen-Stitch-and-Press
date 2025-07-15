using CSP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP.Domain.Logics.Interfaces
{
    public interface IDiscountLogic
    {
        Task<Discount> GetDiscountByIdAsync(int id);
        Task<List<Discount>> GetDiscountsByOrderIdAsync(int orderId);
        Task CreateDiscountAsync(Discount discount);
        Task<bool> DeleteDiscountByIdAsync(int discountId);
    }
}
