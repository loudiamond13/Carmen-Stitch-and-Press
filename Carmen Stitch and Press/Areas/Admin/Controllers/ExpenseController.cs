using Carmen_Stitch_and_Press.Areas.Admin.ViewModels;
using Carmen_Stitch_and_Press.Utilities;
using CSP.Domain.Logics.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Carmen_Stitch_and_Press.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Constants.Role_Admin)]
    [Route("Admin/Expense")]
    public class ExpenseController : Controller
    {
        private readonly IIdentityUserServices _identityUserServices;
        private readonly IOrderItemLogic _orderItemLogic;
        public ExpenseController(IIdentityUserServices identityUserServices, IOrderItemLogic orderItemLogic)
        {
            _identityUserServices = identityUserServices;
            _orderItemLogic = orderItemLogic;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region Add Order Expense
        [HttpGet]
        [Route("AddOrderExpense/{orderId}")]
        public async Task<IActionResult> AddOrderExpense(int orderId)
        {
            try
            {
                if (orderId != 0)
                {
                    ExpenseViewModel expenseViewModel = new();
                    expenseViewModel.carmenStitchAndPressUsers = await _identityUserServices.GetUsersByRole(Constants.Role_Admin);
                    expenseViewModel.OrderId = orderId;

                    return PartialView("AddOrderExpense", expenseViewModel);
                }
                return Ok(new { success = false, message = "Invalid order Id." });

            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = ex.Message });
            }
        }
        #endregion
        #region View an order expenses
        [Route("ViewOrderExpenses")]
        [HttpGet]
        public async Task<IActionResult> ViewOrderExpenses([FromQuery] int orderId, [FromQuery] string orderName)
        {
            var orderItems = await _orderItemLogic.GetAllOrderItemsByOrderIdAsync(orderId);
            ExpenseViewModel expenseViewModel = new();
            expenseViewModel.OrderName = orderName;
            expenseViewModel.OrderId = orderId;
            expenseViewModel.OrderTotalQty = orderItems.Sum(x=>x.Quantity).ToString();
            return View("ViewOrderExpenses", expenseViewModel);
        }
        #endregion

        #region add company expenses
        [HttpGet]
        [Route("AddCompanyExpense")]
        public async Task<IActionResult> AddCompanyExpense()
        {
            try
            {
                ExpenseViewModel expenseViewModel = new();
                expenseViewModel.carmenStitchAndPressUsers = await _identityUserServices.GetUsersByRole(Constants.Role_Admin);
                return PartialView("AddCompanyExpense", expenseViewModel);
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = ex.Message });
            }
        }
        #endregion
        #region view company expenses
        [Route("ViewCompanyExpense")]
        [HttpGet]
        public IActionResult ViewCompanyExpense() 
        {
            return View("ViewCompanyExpense");
        }
        #endregion
        #region
        #endregion
    }
}
