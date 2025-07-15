using Carmen_Stitch_and_Press.Areas.Admin.ViewModels;
using Carmen_Stitch_and_Press.Utilities;
using CSP.Domain.Entities;
using CSP.Domain.Logics.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Carmen_Stitch_and_Press.Areas.Admin.APIControllers
{
    [Authorize(Roles = Constants.Role_Admin)]
    [Area("Admin")]
    [Route("api/[area]/order")]
    [ApiController]
    public class OrderAPIController : ControllerBase
    {
        private readonly IOrderLogic _orderLogic;
        private readonly IOrderItemLogic _orderItemLogic;
        private readonly IPaymentLogic _paymentLogic;
        private readonly IDiscountLogic _discountLogic;
        public OrderAPIController(
            IOrderItemLogic orderItemLogic, 
            IOrderLogic orderLogic, 
            IPaymentLogic paymentLogic,
            IDiscountLogic discountLogic
            )
        {
            _orderItemLogic = orderItemLogic;
            _orderLogic = orderLogic;
            _paymentLogic = paymentLogic;
            _discountLogic = discountLogic;
        }

        [HttpGet]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var orders = await _orderLogic.GetAllUndeletedOrdersAsync();

                List<OrderViewModel> ordersVM = new();

                foreach (var order in orders)
                {

                    ordersVM.Add(new OrderViewModel
                    {
                        OrderId = order.OrderId,
                        OrderName = order.OrderName,
                        TotalAmount = order.TotalAmount,
                        TotalBalance = order.TotalBalance,
                        PaidAmount = order.PaidAmount,
                        TotalDiscount = order.TotalDiscount,
                        TotalExpenses = order.TotalExpenses,
                        IsOpen = order.IsOpen
                        
                    });
                }

                return Ok(ordersVM);
            }
            catch (Exception ex)
            {
                return Ok(new { message = ex.Message, success = false });
            }
        }
        #region Create Order
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromForm] OrderViewModel orderViewModel)
        {
            try
            {
                Order order = new Order()
                {
                    OrderName = orderViewModel.OrderName,
                    Note = orderViewModel.Note,
                    OrderItems = orderViewModel.Items.Select(x => new OrderItem
                    {
                        Description = x.Description,
                        Price = x.Price,
                        Quantity = x.Quantity,
                    }).ToList()
                };

                //assign Payments only if there are any
                if (!string.IsNullOrEmpty(orderViewModel.PayerName))
                {
                    order.Payments = new List<Payment>
                    {
                        new Payment
                                {
                                    PayerName = orderViewModel.PayerName,
                                    PayTo = orderViewModel.PayTo,
                                    Amount = orderViewModel.PaymentAmount
                                }
                    };
                }

                if (!string.IsNullOrEmpty(orderViewModel.DiscountDesc))
                {
                    order.Discounts = new List<Discount>
                    {
                        new Discount
                            {
                                Description = orderViewModel.DiscountDesc,
                                Amount = orderViewModel.DiscountAmt
                            }
                    };
                }

                await _orderLogic.CreateOrderWithItemsAsync(order);
                return Ok(new { message = "Created order successfully.", success = true, redirectUrl = "/Admin/Order/Index" });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = ex.Message });
            }
        }
        #endregion

        #region Add order item
        [HttpPost]
        [Route("AddOrderItem")]
        public async Task<IActionResult> AddOrderItem([FromForm] OrderItemViewModel orderItemViewModel)
        {
            try
            {
                if (orderItemViewModel.OrderId != 0 && await _orderLogic.TrueIfOrderExistsAsync(orderItemViewModel.OrderId))
                {
                    OrderItem orderItem = new OrderItem
                    {
                        OrderId = orderItemViewModel.OrderId,
                        Description = orderItemViewModel.Description,
                        Price = orderItemViewModel.Price,
                        Quantity = orderItemViewModel.Quantity

                    };
                    await _orderItemLogic.AddOrderItemAsync(orderItem);

                    return Ok(new { success = true, message = "Order Item successfully added." });
                }
                return Ok(new { success = false, message = "Invalid Order Id." });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = ex.Message });
            }
        }
        #endregion

        #region remove order item
        [HttpDelete]
        [Route("RemoveOrderItem/{orderItemId}")]
        public async Task<IActionResult> RemoveOrderItem(int orderItemId)
        {
            try
            {
                if (orderItemId != 0 && await _orderItemLogic.DeleteOrderItemByIdAsync(orderItemId))
                {
                    return Ok(new { success = true, message = "Order Item Deleted Successfully." });
                }
                return Ok(new { success = false, message = "Invalid Order Item Id." });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = ex.Message });
            }
        }
        #endregion

        #region add order payment by 
        [HttpPost]
        [Route("AddOrderPayment")]
        public async Task<IActionResult> AddOrderPayment([FromForm] PaymentViewModel paymentViewModel)
        {
            try
            {
                if (paymentViewModel.OrderId != 0 && await _orderLogic.TrueIfOrderExistsAsync(paymentViewModel.OrderId))
                {
                    Payment payment = new Payment
                    {
                        OrderId = paymentViewModel.OrderId,
                        PayerName = paymentViewModel.PayerName,
                        Amount = paymentViewModel.Amount,
                        PayTo = paymentViewModel.PayTo

                    };

                    await _paymentLogic.CreatePaymentAsync(payment);
                    return Ok(new { success = true, message = "Added a payment successfully." });
                }
                return Ok(new { success = false, message = "Failed to add payment." });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region
        [HttpDelete]
        [Route("RemoveOrderPayment/{paymentId}")]
        public async Task<IActionResult> RemoveOrderPayment(int paymentId)
        {
            try
            {
                if (paymentId != 0 && await _paymentLogic.DeleteOrderPaymentByIdAsync(paymentId))
                {
                    return Ok(new { success = true, message = "Order payment deleted successfully." });
                }
                return Ok(new { success = false, message = "Invalid Order Payment Id" });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = ex.Message });
            }
        }
        #endregion

        #region create order discount
        [HttpPost]
        [Route("AddOrderDiscount")]
        public async Task<IActionResult> AddOrderDiscount([FromForm] DiscountViewModel discountViewModel) 
        {
            try
            {
                if (discountViewModel.OrderId != 0 && await _orderLogic.TrueIfOrderExistsAsync(discountViewModel.OrderId)) 
                {
                    Discount discount = new Discount
                    {
                        Amount = discountViewModel.Amount,
                        Description = discountViewModel.Description,
                        OrderId = discountViewModel.OrderId,
                    };

                    await _discountLogic.CreateDiscountAsync(discount);
                    return Ok(new { success = true, message = "Order discount added successfully." });
                }
                return Ok(new { success = false, message = "Failed to add discount." });
            }
            catch (Exception ex) 
            {
                return Ok(new { success = false, message = ex.Message });
            }
        }
        #endregion
        #region delete discount
        [HttpDelete]
        [Route("RemoveOrderDiscount/{discountId}")]
        public async Task<IActionResult> RemoveOrderDiscount(int discountId) 
        {
            try
            {
                if (discountId != 0 && await _discountLogic.DeleteDiscountByIdAsync(discountId)) 
                {
                    return Ok(new { success = true, message = "Order discount deleted successfully." });
                }
                return Ok(new { success = false, message = "Invalid Discount Id." });
            }
            catch (Exception ex) 
            {
                return Ok(new { success = false, message = ex.Message });
            }
        }
        #endregion
        #region remove discount
        [HttpDelete]
        [Route("RemoveDiscount/{discId}")]
        public async Task<IActionResult> RemoveDiscount(int discId) 
        {
            try
            {
                if (discId != 0 && await _discountLogic.DeleteDiscountByIdAsync(discId)) 
                {
                    return Ok(new { success = true, message = "Order discount delete successfully." });
                }
                return Ok(new { success = false, message = "Invalid discount Id." });
            }
            catch (Exception ex) 
            {
                return Ok(new { success = false, message = ex.Message });
            }
        }
        #endregion
        #region update order item
        [HttpPost]
        [Route("EditOrderItem")]
        public async Task<IActionResult> EditOrderItem([FromForm] OrderItem orderItem) 
        {
            try
            {
                if (orderItem.OrderItemId != 0 && orderItem.Quantity != 0 && orderItem.Price != 0) 
                {
                    await _orderItemLogic.UpdateOrderItemAsync(orderItem);
                    return Ok(new { success = true, message = "Order Item updated successfully."});
                }
                return Ok(new { success = false, message = "Please fill the required fields." });
            }
            catch (Exception ex) 
            {
                return Ok(new { success = false, message = ex.Message });
            }
        }
        #endregion
        #region undone and done order item 
        [HttpPost]
        [Route("DoneUndoneOrderItem")]
        public async Task<IActionResult> DoneUndoneOrderItem([FromQuery] int itemId, [FromQuery] bool isDone) 
        {
            try
            {
                if (itemId != 0) 
                {
                    //if passed in value for isDone is false, make it false, same as if it is true make it false
                    bool doneValue = isDone == true ? false : true;
                    await _orderItemLogic.DoneUndoneOrderItem(itemId, doneValue);

                    if (doneValue)
                    {
                        return Ok(new { success = true, message = "Finished Order Item." });

                    }
                    else 
                    {
                        return Ok(new { success = true, message = "Opened Order Item." });
                    }
                }
                return Ok(new { success = false, message = "Unable to finish order item." });
            }
            catch (Exception ex) 
            {
                return Ok(new { success = false, message = ex.Message });
            }
        }
        #endregion
        #region Close or open an order
        [HttpPost]
        [Route("CloseOpenOrder")]
        public async Task<IActionResult> CloseOpenOrder([FromQuery]int orderId, [FromQuery]bool isOpen) 
        {
            try
            {
                if (await _orderLogic.TrueIfOrderExistsAsync(orderId)) 
                {
                    //if order is open, set it to false to close order, otherwise set it to true to open order
                    bool open = isOpen == true ? false : true;

                    //if open == false, this to close order, check if it order is fully paid
                    if (await _orderLogic.isOrderFullyPaid(orderId) && !open)
                    {
                        await _orderLogic.CloseOpenOrder(orderId, open);
                        return Ok(new { success = true, message = "Closed the order successfully." });
                    }
                    //else if open == true, this is to open order
                    else if (open == true) 
                    {
                        await _orderLogic.CloseOpenOrder(orderId, open);
                        return Ok(new { success = true, message = "Opened the order successfully." });
                    }

                    return Ok(new { success = false, message = "Order is not fully paid!" });

                }
                return Ok(new { success = false, message = "Unable to close order." });

            }
            catch (Exception ex) 
            {
                return Ok(new { success = false, message = ex.Message });

            }
        }
        #endregion
        #region edit order payment
        [Route("EditOrderPayment")]
        [HttpPost]
        public async Task<IActionResult> EditOrderPayment([FromForm] PaymentViewModel paymentViewModel) 
        {
            try
            {
                if (ModelState.IsValid) 
                {
                    Payment payment = new();

                    payment.PaymentId = paymentViewModel.PaymentId;
                    payment.Amount = paymentViewModel.Amount;
                    payment.PayerName = paymentViewModel.PayerName;
                    payment.PayTo = paymentViewModel.PayTo;

                    await _paymentLogic.EditOrderPayment(payment);
                    return Ok(new { success = true, message = "Payment updated successfully." });
                }
                return Ok(new { success = false, message = "Please fill the required fields." });
            }
            catch (Exception ex) 
            {
                return Ok(new { success = false, message = ex.Message });
            }
        }
        #endregion
    }
}
