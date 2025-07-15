using Carmen_Stitch_and_Press.Areas.Admin.ViewModels;
using Carmen_Stitch_and_Press.Utilities;
using CSP.Domain.Logics.Interfaces;
using CSP.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Carmen_Stitch_and_Press.Areas.Admin.Controllers
{
    [Authorize(Roles = Constants.Role_Admin)]
    [Area("Admin")]
    [Route("Admin/FinancialManagement")]
    public class FinancialManagementController : Controller
    {
        private readonly IMoneyTransferLogic _moneyTransferLogic;
        private readonly IIdentityUserServices _identityUserServices;
        private readonly IExpenseLogic _expenseLogic;
        private readonly IPaymentLogic _paymentLogic;
        private readonly IOrderLogic _orderLogic;
        private readonly IOrderItemLogic _orderItemLogic;

        public FinancialManagementController(
            IMoneyTransferLogic moneyTransferLogic,
            IIdentityUserServices identityUserServices,
            IExpenseLogic expenseLogic,
            IPaymentLogic paymentLogic,
            IOrderLogic orderLogic,
            IOrderItemLogic orderItemLogic
            )
        {
            _expenseLogic = expenseLogic;
            _paymentLogic = paymentLogic;
            _moneyTransferLogic = moneyTransferLogic;
            _identityUserServices = identityUserServices;
            _orderLogic = orderLogic;
            _orderItemLogic = orderItemLogic ;
        }

        #region 
         
        [Route("Index")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                FinancialManagementViewModel financialManagementVM = new();

                //get all admin users
                var adminUsers = await _identityUserServices.GetUsersByRole(Constants.Role_Admin);
                var expenses = await _expenseLogic.GetAllExpensesAsync();
                var payments = await _paymentLogic.GetUndeletedPaymentsAsync();
                var orders = await _orderLogic.GetAllUndeletedOrdersAsync();
                var transfers = await _moneyTransferLogic.GetAllTransfersAsync();
                var orderItems = await _orderItemLogic.GetAllOrderItemsAsync();

                financialManagementVM.carmenStitchAndPressUserModels = adminUsers.Select(x => new CarmenStitchAndPressUserModel
                {
                    Id = x.Id,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    UserName = x.UserName,
                }).ToList();

                // process the money on hand of the admin/money holders
                for (var i = 0; i < financialManagementVM.carmenStitchAndPressUserModels.Count; i++)
                {
                    var username = financialManagementVM.carmenStitchAndPressUserModels[i].UserName.Trim().ToLower();

                    //get money on hand by summing (received payments + received transfers) MINUS (sent transfers + spent money for expenses)
                    var totalReceivedAmount = payments
                                            .Where(x => x.PayTo.Trim().ToLower() == username)
                                            .Sum(x => x.Amount);

                    var totalReceivedTransfers = transfers
                                                    .Where(x => x.TransferTo.Trim().ToLower() == username)
                                                    .Sum(x => x.TransferAmount);

                    var totalSpent = expenses
                                        .Where(x => x.PaidBy.Trim().ToLower() == username)
                                        .Sum(x => x.Amount);

                    var totalTransfered = transfers
                                                .Where(x => x.TransferFrom.Trim().ToLower() == username)
                                                .Sum(x => x.TransferAmount);

                    financialManagementVM.carmenStitchAndPressUserModels[i].MoneyOnHand = (totalReceivedAmount + totalReceivedTransfers) - (totalSpent + totalTransfered);
                }

                //process amounts
                financialManagementVM.TotalReceivedMoney = payments.Sum(x => x.Amount);
                financialManagementVM.AllOrdersTotal = orders.Sum(x => x.TotalAmount);
                financialManagementVM.TotalExpenses = expenses.Sum(x => x.Amount);
                financialManagementVM.TotalProfit = financialManagementVM.AllOrdersTotal - financialManagementVM.TotalExpenses;
                financialManagementVM.Collectible = financialManagementVM.AllOrdersTotal - financialManagementVM.TotalReceivedMoney;
                financialManagementVM.AcceptedOrderQtyTotal = orderItems.Sum(x => x.Quantity);

                return View(financialManagementVM);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
        #region transfer money
        [HttpGet]
        [Route("TransferMoney")]
        public async Task<IActionResult> TransferMoney() 
        {
            try
            {
                var adminUsers = await _identityUserServices.GetUsersByRole(Constants.Role_Admin);

                ViewBag.UsersFrom = adminUsers;
                ViewBag.UsersTo = adminUsers.Select(x => x).Where(x => x.UserName != User.Identity?.Name);

                return PartialView("TransferMoney");
            }
            catch (Exception ex) 
            {
                return Ok(new { success = false, message = ex.Message });
            }
            
        }
        #endregion
        #region view money transactions
        [HttpGet]
        [Route("TransferTransactions")]
        public IActionResult TransferTransactions() 
        {
            return View();
        }
        #endregion
    }
}
