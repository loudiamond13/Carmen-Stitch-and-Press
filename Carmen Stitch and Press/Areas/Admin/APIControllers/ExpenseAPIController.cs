using Carmen_Stitch_and_Press.Areas.Admin.ViewModels;
using Carmen_Stitch_and_Press.Utilities;
using CSP.Domain.Entities;
using CSP.Domain.Logics.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carmen_Stitch_and_Press.Areas.Admin.APIControllers
{
    [ApiController]
    [Authorize(Roles = Constants.Role_Admin)]
    [Route("api/Admin/Expense")]
    [Area("Admin")]
    public class ExpenseAPIController : ControllerBase
    {
        private readonly IOrderLogic _orderLogic;
        private readonly IExpenseLogic _expenseLogic;
        public ExpenseAPIController(IOrderLogic orderLogic, IExpenseLogic expense)
        {
            _orderLogic = orderLogic;
            _expenseLogic = expense;
        }
        [Route("Index")]
        [HttpGet]
        public async Task< IActionResult> Index()
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
                        //TotalAmount = order.TotalAmount,
                        //TotalBalance = order.TotalBalance,
                        //PaidAmount = order.PaidAmount,
                        //TotalDiscount = order.TotalDiscount,
                        TotalExpenses = order.TotalExpenses,
                        OrderDate = order.OrderDate,
                        
                    });
                }

                return Ok(ordersVM);
            }
            catch (Exception ex)
            {
                return Ok(new { message = ex.Message, success = false });
            }
        }

        #region add expense to an order
        [HttpPost]
        [Route("AddOrderExpense")]
        public async Task<IActionResult> AddOrderExpense([FromForm] ExpenseViewModel expenseViewModel) 
        {
            try
            {
                if (expenseViewModel.Amount != 0 && !string.IsNullOrEmpty(expenseViewModel.PaidBy) && !string.IsNullOrEmpty(expenseViewModel.Description)) 
                {
                    Expense expense = new Expense
                    {
                        OrderId = expenseViewModel.OrderId,
                        Amount = expenseViewModel.Amount,
                        PaidBy = expenseViewModel.PaidBy,
                        Description = expenseViewModel.Description,
                        IsCompanyExpenses = false
                    };

                    await _expenseLogic.CreateExpenseAsync(expense);
                    return Ok(new { message = "Expense added successfully.", success = true });
                }
                return Ok(new { message = "Please fill all the fields.", success = false });
            }
            catch (Exception ex) 
            {
                return Ok(new { message = ex.Message, success = false });
            }
        }
        #endregion

        #region view expenses of an order
        [HttpGet]
        [Route("OrderExpenses/{orderId}")]
        public async Task<IActionResult> OrderExpenses(int orderId) 
        {
            try
            {
                if (orderId != 0 && await _orderLogic.TrueIfOrderExistsAsync(orderId)) 
                {
                    List < ExpenseViewModel > expenseViewModel = new();
                    var expenses = await _expenseLogic.GetExpensesByOrderIdAsync(orderId);

                    expenseViewModel = expenses.Select(x => new ExpenseViewModel
                    {
                        ExpensesId = x.ExpensesId,
                        SpendDateString = x.SpendDate.ToString("MM/dd/yyyy"),
                        Amount = x.Amount,
                        Description = x.Description
                    }).ToList();

                    return Ok(expenseViewModel);
                }
                return Ok(new { message = "Failed to load order expenses.", success = false });
            }
            catch (Exception ex) 
            {
                return Ok(new { message = ex.Message, success = false });
            }
        }
        #endregion
        #region add company expense
        [HttpPost]
        [Route("AddCompanyExpense")]
        public async Task<IActionResult> AddCompanyExpense([FromForm] ExpenseViewModel expenseViewModel) 
        {
            try
            {
                if (expenseViewModel.Amount != 0 && !string.IsNullOrEmpty(expenseViewModel.PaidBy) && !string.IsNullOrEmpty(expenseViewModel.Description))
                {
                    Expense expense = new Expense
                    {
                        OrderId =null,
                        Amount = expenseViewModel.Amount,
                        PaidBy = expenseViewModel.PaidBy,
                        Description = expenseViewModel.Description,
                        IsCompanyExpenses = true
                    };

                    await _expenseLogic.CreateExpenseAsync(expense);
                    return Ok(new { message = "Expense added successfully.", success = true });
                }
                return Ok(new { message = "Please fill all the fields.", success = false });
            }
            catch (Exception ex) 
            {
                return Ok(new { message = ex.Message, success = false });
            }
        }
        #endregion
        #region get Company Expenses
        [HttpGet]
        [Route("CompanyExpenses")]
        public async Task<IActionResult> CompanyExpenses() 
        {
            try
            {
                List<ExpenseViewModel> expenseViewModel = new();
                var expenses = await _expenseLogic.GetAllCompanyExpenses();

                expenseViewModel = expenses.Select(x => new ExpenseViewModel
                {
                    SpendDateString = x.SpendDate.ToString("MM/dd/yyyy"),
                    Amount = x.Amount,
                    Description = x.Description,
                    ExpensesId = x.ExpensesId
                }).ToList();

                return Ok(expenseViewModel);
            }
            catch (Exception ex) 
            {
                return Ok(new { message = ex.Message, success = false });
            }
        }
        #endregion
        #region delete order expense
        [Route("DeleteOrderExpense/{expenseId}")]
        public async Task<IActionResult> DeleteOrderExpense(int expenseId) 
        {
            try
            {
                if (expenseId != 0) 
                {
                    if (await _expenseLogic.DeleteExpenseAsync(expenseId)) //will return true if expense deleted successfully
                    {
                        return Ok(new { message = "Expense deleted successfully!", success = true });
                    }
                    return Ok(new { message = "Failed to delete expense.", success = false });
                }
                return Ok(new { message = "Invalid Expense Id.", success = false });
            }
            catch (Exception ex) 
            {
                return Ok(new { message = ex.Message, success = false });
            }
        }
        #endregion
    }
}
