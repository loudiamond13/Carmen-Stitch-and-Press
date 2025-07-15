using Carmen_Stitch_and_Press.Areas.Company.ViewModels;
using Carmen_Stitch_and_Press.Utilities;
using CSP.Domain.Logics.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Carmen_Stitch_and_Press.Areas.Company.APIController
{
    [Area("Company")]
    [Authorize(Roles = Constants.Role_Company)]
    [ApiController]
    [Route("api/[area]/user")]
    public class UserAPIController : ControllerBase
    {
        private readonly IIdentityUserServices _identityUserServices;
        public UserAPIController(IIdentityUserServices identityUserServices)
        {
            _identityUserServices = identityUserServices;
        }


        #region
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            try
            {
                List<UserViewModel> userViewModel = new List<UserViewModel>();
                var users = await _identityUserServices.GetAllUsers();
                foreach (var user in users)
                {
                    if (user.UserName != User.Identity?.Name) 
                    {
                        userViewModel.Add(new UserViewModel
                        {
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            LockoutEnd = user.LockoutEnd,
                            Id = user.Id,
                            Role = "Test"
                        });
                    }
                }
                return Ok(userViewModel);
            }
            catch (Exception ex)
            {
                return Ok(new { message = ex.Message, success = false });
            }
        }
        #endregion
        #region lock or unlock user
        [Route("UnlockLock/{userId}")]
        [HttpPost]
        public async Task<IActionResult> UnlockLock(string userId) 
        {
            try
            {
                var user = await _identityUserServices.GetUserByIdAsync(userId);
                if (!string.IsNullOrEmpty(userId) && user != null)
                {
                    

                    await _identityUserServices.UnlockLockUserByIdAsync(userId);

                    return Ok(new { message = "User has been unlocked/locked successfully..", success = true });
                }

                return Ok(new { message = "Unable to unlock/lock user.", success = false });
            }
            catch (Exception ex) 
            {
                return Ok(new { message = ex.Message, success = false });
            }
        }
        #endregion
    }
}
