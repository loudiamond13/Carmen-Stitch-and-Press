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
    public class DiscountLogic : IDiscountLogic
    {
        #region
        #endregion
        private readonly IDiscountRepository _discountRepository;

        public DiscountLogic(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }
        #region
        public async Task<Discount> GetDiscountByIdAsync(int id)
        {
            Discount discount = await _discountRepository.GetAsync(x => x.OrderDiscountId == id);
            if (discount != null)
            {
                return discount;
            }
            return null;
        }
        #endregion
        #region
        public async Task<List<Discount>> GetDiscountsByOrderIdAsync(int orderId)
        {
            List<Discount> discounts = await _discountRepository.FindByAsync(x => x.OrderId == orderId);

            if (discounts != null)
            {
                return discounts;
            }
            return null;
        }
        #endregion

        #region create dicsount
        public async Task CreateDiscountAsync(Discount discount) 
        {
            if (discount != null) 
            {
                await _discountRepository.AddAsync(discount);
                await _discountRepository.SaveAsync();
            }
        }

        #endregion

        #region delete discount by id
        public async Task<bool> DeleteDiscountByIdAsync(int discountId) 
        {
            if (discountId != 0) 
            {
                Discount discount = await _discountRepository.GetAsync(x => x.OrderDiscountId == discountId);
                if (discount != null) 
                {
                    _discountRepository.Delete(discount);
                    await _discountRepository.SaveAsync();
                    return  true;
                }
                return false;
            }
            return false;
        }
        #endregion
    }
}
