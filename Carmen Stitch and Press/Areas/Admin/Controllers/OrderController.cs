using Carmen_Stitch_and_Press.Areas.Admin.ViewModels;
using Carmen_Stitch_and_Press.Utilities;
using CSP.Domain.Entities;
using CSP.Domain.Logics.Interfaces;
using CSP.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Carmen_Stitch_and_Press.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Constants.Role_Admin)]
    [Route("Admin/Order")]
    public class OrderController : Controller
    {
        private readonly IIdentityUserServices _identityUserServices;
        private readonly IOrderLogic _orderLogic;
        private readonly IPaymentLogic _paymentLogic;
        private readonly IOrderItemLogic _orderItemLogic;
        private readonly IDiscountLogic _discountLogic;

        public OrderController(
            IIdentityUserServices identityUserServices,
            IOrderLogic orderLogic,
            IPaymentLogic paymentLogic,
            IOrderItemLogic orderItemLogic,
            IDiscountLogic discountLogic)
        {
            _identityUserServices = identityUserServices;
            _orderLogic = orderLogic;
            _paymentLogic = paymentLogic;
            _orderItemLogic = orderItemLogic;
            _discountLogic = discountLogic;
        }

        #region Index
        [Route("Index")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            return View();
        }
        #endregion

        #region
        [Route("Create")]
        public async Task<IActionResult> Create()
        {
            List<CarmenStitchAndPressUserModel> adminUsers = await _identityUserServices.GetUsersByRole(Constants.Role_Admin);
            ViewBag.AdminUsers = adminUsers;

            return View();
        }
        #endregion

        #region
        [HttpGet]
        [Route("View/{orderId}")]
        public async Task<IActionResult> View(int orderId)
        {
            try
            {
                if (orderId != 0)
                {
                    List<CarmenStitchAndPressUserModel> adminUsers = await _identityUserServices.GetUsersByRole(Constants.Role_Admin);
                    ViewBag.AdminUsers = adminUsers;

                    OrderViewModel orderViewModel = new OrderViewModel();
                    Order order = await _orderLogic.GetOrderAsync(orderId);

                    orderViewModel = new OrderViewModel
                    {
                        OrderId = orderId,
                        OrderName = order.OrderName,
                        OrderDate = order.OrderDate,
                        Note = order.Note,
                        CreatedBy = order.CreatedBy,
                        IsOpen = order.IsOpen,
                        TotalAmount = order.TotalAmount,
                        TotalBalance = order.TotalBalance,
                        PaidAmount = order.PaidAmount,
                        TotalDiscount = order.TotalDiscount,
                        TotalExpenses = order.TotalExpenses
                    };

                    orderViewModel.Payments = order.Payments.Select(x => new PaymentViewModel
                    {
                        PaymentId = x.PaymentId,
                        PaymentDate = x.PaymentDate,
                        PayerName = x.PayerName,
                        CreatedBy = x.CreatedBy,
                        Amount = x.Amount,
                        PayTo = x.PayTo,
                    }).ToList();

                    orderViewModel.Items = order.OrderItems.Select(x => new OrderItemViewModel
                    {
                        OrderItemId = x.OrderItemId,
                        Description = x.Description,
                        Quantity = x.Quantity,
                        Price = x.Price,
                        IsDone = x.IsDone,
                    }).ToList();

                    orderViewModel.Discounts = order.Discounts.Select(x => new DiscountViewModel
                    {
                        OrderDiscountId = x.OrderDiscountId,
                        Amount = x.Amount,
                        Description = x.Description,
                    }).ToList();

                    return View("View", orderViewModel);
                }

                return Ok(new { success = false, message = "Invalid Order Id." });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = ex.Message });
            }
        }
        #endregion

        #region add order Item
        [HttpGet]
        [Route("AddOrderItem/{orderId}")]
        public async Task<IActionResult> AddOrderItem(int orderId)
        {
            try
            {
                if (orderId != 0 && await _orderLogic.TrueIfOrderExistsAsync(orderId))
                {
                    return PartialView("AddOrderItem", orderId);
                }
                return Ok(new { success = false, message = "Invalid Order ID." });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = ex.Message });
            }
        }
        #endregion

        #region add order payment by id
        [HttpGet]
        [Route("AddOrderPayment/{orderId}")]
        public async Task<IActionResult> AddOrderPayment(int orderId)
        {
            try
            {
                if (orderId != 0 && await _orderLogic.TrueIfOrderExistsAsync(orderId))
                {
                    PaymentViewModel paymentViewModel = new();

                    paymentViewModel.carmenStitchAndPressUsers = await _identityUserServices.GetUsersByRole(Constants.Role_Admin);
                    paymentViewModel.OrderId = orderId;

                    return PartialView("AddOrderPayment", paymentViewModel);
                }
                return Ok(new { success = false, message = "Invalid Order ID." });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = ex.Message });
            }
        }
        #endregion

        #region create discount
        [HttpGet]
        [Route("AddOrderDiscount/{orderId}")]
        public async Task<IActionResult> AddOrderDiscount(int orderId) 
        {
            try
            {
                if (orderId != 0 && await _orderLogic.TrueIfOrderExistsAsync(orderId)) 
                {
                    return PartialView("AddOrderDiscount", orderId);
                }
                return Ok(new { success = false, message = "Invalid Order Id." });
            }
            catch (Exception ex) 
            {
                return Ok(new { success = false, message = ex.Message });
            }
        }
        #endregion
        #region
        [HttpGet]
        [Route("EditOrderItem/{itemId}")]
        public async Task<IActionResult> EditOrderItem(int itemId) 
        {
            try
            {
                if (itemId != 0) 
                {
                    var item = await _orderItemLogic.GetOrderItemAsync(itemId);
                    if (item != null) 
                    {
                        return PartialView("EditOrderItem", item);
                    }
                    return Ok(new { success = false, message = "Invalid order item Id." });
                }
                return Ok(new { success = false, message = "Invalid order item Id." });
            }
            catch (Exception ex) 
            {
                return Ok(new { success = false, message = ex.Message });
            }
        }
        #endregion
        #region edit order payment
        [Route("EditOrderPayment/{paymentId}")]
        [HttpGet]
        public async Task<IActionResult> EditOrderPayment(int paymentId) 
        {
            try
            {
                if (paymentId != 0) 
                {
                    var payment = await _paymentLogic.GetPaymentAsync(paymentId);

                    if (payment != null)
                    {
                        PaymentViewModel paymentViewModel = new();

                        paymentViewModel.carmenStitchAndPressUsers = await _identityUserServices.GetUsersByRole(Constants.Role_Admin);
                        paymentViewModel.OrderId = payment.OrderId;
                        paymentViewModel.Amount = payment.Amount;
                        paymentViewModel.PayerName = payment.PayerName;
                        paymentViewModel.PaymentId = payment.PaymentId;
                        paymentViewModel.PayTo = payment.PayTo;
                        return PartialView("EditOrderPayment", paymentViewModel);
                    }
                    return Ok(new { success = false, message = "Unable to get payment." });
                }
                return Ok(new { success = false, message = "Invalid order payment Id." });
            }
            catch (Exception ex) 
            {
                return Ok(new { success = false, message = ex.Message });
            }
        }
        #endregion
    }
}
