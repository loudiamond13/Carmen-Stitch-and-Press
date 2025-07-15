using CSP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP.Domain.Logics.Interfaces
{
    public interface IOrderLogic
    {
        Task CreateOrderWithItemsAsync(Order order);
        Task CreateOrderAsync(Order order);
        Task<Order> GetOrderAsync(int orderId);
        Task<List<Order>> GetAllUndeletedOrdersAsync();


        Task<List<Order>> GetAllOpenOrdersAsync();

        Task<bool> TrueIfOrderExistsAsync(int orderId);

        Task<Order> GetOrderWithExpensesAync(int orderId);

        Task CloseOpenOrder(int orderId, bool open);
         Task<bool> isOrderFullyPaid(int orderId);
        /*Task UpdateOrderTotalAmount(int orderId)*/

    }
}
