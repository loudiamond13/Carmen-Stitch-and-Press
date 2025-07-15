using Carmen_Stitch_and_Press.Areas.Admin.ViewModels;
using Carmen_Stitch_and_Press.Utilities;
using CSP.Domain.Entities;
using CSP.Domain.Logics;
using CSP.Domain.Logics.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carmen_Stitch_and_Press.Areas.Admin.APIControllers
{
    [Authorize(Roles = Constants.Role_Admin)]
    [Area("Admin")]
    [ApiController]
    [Route("api/Admin/FinancialManagement")]
    public class FinancialManagementAPIController : ControllerBase
    {
        private readonly IMoneyTransferLogic _moneyTransferLogic;
        private readonly IIdentityUserServices _identityUserServicesLogic;
        public FinancialManagementAPIController(IMoneyTransferLogic moneyTransferLogic, IIdentityUserServices identityUserServices)
        {
            _moneyTransferLogic = moneyTransferLogic;
            _identityUserServicesLogic = identityUserServices;
        }

        #region Transfer money
        [Route("TransferMoney")]
        [HttpPost]
        public async Task<IActionResult> TransferMoney([FromForm] MoneyTransfer moneyTransfer) 
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _moneyTransferLogic.CreateMoneyTransferAsync(moneyTransfer);

                    return Ok(new { success = true, message = "Transfer money successfully." });
                }
                return Ok(new { success = false, message = "Please fill the required fields." });
            }
            catch (Exception ex) 
            {
                return Ok(new { success = false, message = ex.Message});
            }
        }

        #endregion
        #region get transfer transactions
        [HttpGet]
        [Route("TransferTransactions")]
        public async Task<IActionResult> TransferTransactions() 
        {
            try
            {
                var transactions = await _moneyTransferLogic.GetAllTransfersAsync();
                var adminUsers = await _identityUserServicesLogic.GetUsersByRole(Constants.Role_Admin);
                List<FinancialManagementViewModel> financialManagementViewModel = new();

                financialManagementViewModel = transactions.Select(x => new FinancialManagementViewModel
                {
                    TransferFrom = adminUsers.FirstOrDefault(user => user.Email == x.TransferFrom)?.FirstName,
                    TransferTo = adminUsers.FirstOrDefault(user => user.Email == x.TransferTo)?.FirstName,
                    TransferAmount = x.TransferAmount,
                    TransferDateString = x.TransferDate.ToString("MM/dd/yyyy")
                }).ToList();

                return Ok(financialManagementViewModel);
            }
            catch (Exception ex) 
            {
                return Ok(new { success = false, message = ex.Message });
            }
        }
        #endregion
    }
}
