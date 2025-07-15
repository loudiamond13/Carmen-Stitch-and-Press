using CSP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP.Domain.Logics.Interfaces
{
    public interface IPaymentLogic
    {
        Task<Payment> GetPaymentAsync(int id);
        Task<List<Payment>> GetUndeletedPaymentsByOrderIdAsync(int orderId);
        Task<List<Payment>> GetUndeletedPaymentsAsync();
        Task CreatePaymentAsync(Payment payment);

        Task<bool> DeleteOrderPaymentByIdAsync(int paymentId);
        Task CreateOrderPaymentAsync(Payment payment);
        Task EditOrderPayment(Payment payment);

    }
}
