using CSP.Domain.Entities;
using CSP.Domain.IRepositories;
using CSP.Domain.Logics.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP.Domain.Logics
{
    public class OrderLogic : IOrderLogic
    {
        #region
        #endregion
        private readonly IOrderRepository _orderRepository;
        public OrderLogic(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
             
        }

        #region
        public async Task CreateOrderWithItemsAsync(Order order)
        {
            try
            {
                if (order != null && order.OrderItems != null && order.OrderItems.Any())
                {
                    await _orderRepository.AddAsync(order);
                    await _orderRepository.SaveAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region Create Order
        public async Task CreateOrderAsync(Order order)
        {
            if (order != null)
            {
               // order.OrderDate = DateTime.Now;
               // order.IsOpen = true;

                await _orderRepository.AddAsync(order);
                await _orderRepository.SaveAsync();
            }
        }
        #endregion

        #region Get Order 
        public async Task<Order> GetOrderAsync(int id)
        {
            var order = await _orderRepository.GetAsync(x => x.OrderId == id,
                                                        q => q.Include(x => x.Discounts)
                                                        .Include(o => o.OrderItems)
                                                        .Include(p => p.Payments.Where(x => !x.IsDeleted))
                                                        .Include(e => e.Expenses)
                                                        );
            if (order != null)
            {
                return order;
            }
            return null;
        }
        #endregion

        #region  Get All Orders Async
        public async Task<List<Order>> GetAllUndeletedOrdersAsync()
        {
            List<Order> orders = await _orderRepository.FindByAsync(x=> x.IsDeleted == false || x.IsDeleted == null);
            if (orders != null)
            {
                return orders;
            }

            return null;
        }
        #endregion

        #region get all open orders
        public async Task<List<Order>> GetAllOpenOrdersAsync()
        {
            List<Order> openOrders = await _orderRepository.FindByAsync(x => x.IsOpen == true);
            return openOrders;
        }
        #endregion

        #region
        public async Task<bool> TrueIfOrderExistsAsync(int orderId)
        {
            if (orderId != 0)
            {
                var result = await _orderRepository.GetAsync(x => x.OrderId == orderId);
                if (result != null)
                {
                    return true;
                }
                return false;
            }

            return false;
        }
        #endregion


        #region get an order by id with expenses
        public async Task<Order> GetOrderWithExpensesAync(int orderId)
        {
            var order = await _orderRepository.GetAsync(x => x.OrderId == orderId,
                                                            q => q.Include(x => x.Expenses));


            return order;

        }

        #endregion
        #region close or open order
        public async Task CloseOpenOrder(int orderId, bool open) 
        {
            var order = await _orderRepository.GetAsync(x => x.OrderId == orderId);
            if (order != null) 
            {
               order.IsOpen = open;
                await _orderRepository.SaveAsync();
            }
           
        }

        #endregion
        #region check if an order id fully paid 
        public async Task<bool> isOrderFullyPaid(int orderId)
        {
            Order order = await _orderRepository.GetAsync(x => x.OrderId == orderId);

            if (order?.TotalBalance == 0) 
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
