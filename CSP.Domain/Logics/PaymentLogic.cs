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
    public class PaymentLogic : IPaymentLogic
    {
        #region
        #endregion
        private readonly IPaymentRepository _paymentRepository;
        public PaymentLogic(IPaymentRepository payment)
        {
            _paymentRepository = payment;
        }

        #region Create payment
        public async Task CreatePaymentAsync(Payment payment) 
        {
            if (payment != null) 
            {
                await _paymentRepository.AddAsync(payment);
                await _paymentRepository.SaveAsync();
            }
        }
        #endregion

        #region get a payment
        public async Task<Payment> GetPaymentAsync(int id) 
        {
            if (id != 0) 
            {
                Payment payment = await _paymentRepository.GetAsync(x => x.PaymentId == id);

                if (payment != null) 
                {
                    return payment;
                }
                return null;
            }
            return null;
        }
        #endregion

        #region get payments
        public async Task<List<Payment>> GetUndeletedPaymentsByOrderIdAsync(int orderId) 
        {
            if (orderId != 0)
            {
                List<Payment> payments = await _paymentRepository.FindByAsync(x => x.OrderId == orderId && x.IsDeleted != false);

                if (payments != null)
                {
                    return payments;
                }
                return null;
            }
            return null;
        }
        #endregion


        #region create order payment by order id
        public async Task CreateOrderPaymentAsync(Payment payment) 
        {
            await _paymentRepository.AddAsync(payment);
            await _paymentRepository.SaveAsync();
        }
        #endregion

        #region delete order payment by order id
        public async Task<bool> DeleteOrderPaymentByIdAsync(int paymentId) 
        {
            Payment payment = await _paymentRepository.GetAsync(x => x.PaymentId == paymentId);

            if (payment != null) 
            {
                payment.IsDeleted = true;
                await _paymentRepository.SaveAsync();
                return true;
            }
            return false ;
        }
        #endregion
        #region get all undeleted payments
        public async Task<List<Payment>> GetUndeletedPaymentsAsync() 
        {
            return await _paymentRepository.FindByAsync(x => !x.IsDeleted);
        }
        #endregion
        #region edit order payment
        public async Task EditOrderPayment(Payment payment) 
        {
            Payment paymentFromDb = await _paymentRepository.GetAsync(x => x.PaymentId == payment.PaymentId);

            if (paymentFromDb != null) 
            {
                paymentFromDb.PayerName = payment.PayerName;
                paymentFromDb.Amount = payment.Amount;
                paymentFromDb.PayTo = payment.PayTo;

                await _paymentRepository.SaveAsync();
            }
        }
        #endregion
    }
}
