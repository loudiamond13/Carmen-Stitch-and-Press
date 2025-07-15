using CSP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP.Domain.Logics.Interfaces
{
    public interface IOrderItemLogic
    {
        Task<OrderItem> GetOrderItemAsync(int id);
        Task<List<OrderItem>> GetAllOrderItemsAsync();
        Task<List<OrderItem>> GetAllOrderItemsByOrderIdAsync(int orderId);
        Task CreateOrderItemAsync(OrderItem orderItem);
        Task AddOrderItemAsync(OrderItem orderItem);
        Task<bool> DeleteOrderItemByIdAsync(int orderItemId);

        Task UpdateOrderItemAsync(OrderItem orderItem);

        Task DoneUndoneOrderItem(int orderItemId, bool isDone);
    }
}
