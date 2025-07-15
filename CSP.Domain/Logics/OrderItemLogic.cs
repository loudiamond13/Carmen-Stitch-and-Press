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
    public class OrderItemLogic : IOrderItemLogic
    {
        #region
        #endregion
        private readonly IOrderItemRepository _orderItemRepository;

        public OrderItemLogic(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;

        }

        #region create order item
        public async Task CreateOrderItemAsync(OrderItem orderItem) 
        {
            if (orderItem != null) 
            {
                await _orderItemRepository.AddAsync(orderItem);
                await _orderItemRepository.SaveAsync();
            }
        }
        #endregion

        #region Get Order Item Async
        public async Task<OrderItem> GetOrderItemAsync(int orderId) 
        {
            OrderItem orderItem = await _orderItemRepository.GetAsync(x => x.OrderItemId == orderId);

            if (orderItem != null) 
            {
                return orderItem;
            }
            return null;
        }
        #endregion

        #region get order items
        public async Task<List<OrderItem>> GetAllOrderItemsAsync() 
        {
            List<OrderItem> orderItems = await _orderItemRepository.GetAllAsync();
            return orderItems;
        }
        #endregion

        #region
        public async Task<List<OrderItem>> GetAllOrderItemsByOrderIdAsync(int orderId) 
        {
            if (orderId != 0) 
            {
                List<OrderItem> orderItems = await _orderItemRepository.FindByAsync(x => x.OrderId == orderId);
                return orderItems;
            }
            return null;
        }
        #endregion

        #region add order item
        public async Task AddOrderItemAsync(OrderItem orderItem)
        {
            if (orderItem != null)
            {
                await _orderItemRepository.AddAsync(orderItem);
                await _orderItemRepository.SaveAsync();


            }
        }
        #endregion

        #region delete order item 
        public async Task<bool> DeleteOrderItemByIdAsync(int orderItemId) 
        {
            OrderItem? orderItem = await _orderItemRepository.GetAsync(x => x.OrderItemId == orderItemId);

            if (orderItem != null) 
            {
                 _orderItemRepository.Delete(orderItem);
                await _orderItemRepository.SaveAsync();
                return true;
            }

            return false;
        }
        #endregion
        #region update order item 
        public async Task UpdateOrderItemAsync(OrderItem orderItem) 
        {
            if (orderItem != null) 
            {
                OrderItem orderItemFromDB = await _orderItemRepository.GetAsync(x => x.OrderItemId == orderItem.OrderItemId);
                if (orderItemFromDB != null) 
                {
                    orderItemFromDB.Price = orderItem.Price;
                    orderItemFromDB.Description = orderItem.Description;
                    orderItemFromDB.Quantity = orderItem.Quantity;

                    await _orderItemRepository.SaveAsync();
                }
            }
        }
        #endregion
        #region done or undone order item
        public async Task DoneUndoneOrderItem(int orderItemId, bool isDone) 
        {
            if (orderItemId != 0) 
            {
                OrderItem orderItem = await _orderItemRepository.GetAsync(x => x.OrderItemId == orderItemId);
                if (orderItem != null) 
                {
                    orderItem.IsDone = isDone;
                    await _orderItemRepository.SaveAsync();
                }
            }
        }
        #endregion
    }
}
