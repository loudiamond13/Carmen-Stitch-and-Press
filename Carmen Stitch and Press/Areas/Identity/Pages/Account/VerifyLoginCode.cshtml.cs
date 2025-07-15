using CSP.Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Carmen_Stitch_and_Press.Areas.Identity.Pages.Account
{
    public class VerifyLoginCodeModel : PageModel
    {
        private readonly UserManager<CarmenStitchAndPressUserModel> _userManager;
        private readonly SignInManager<CarmenStitchAndPressUserModel> _signInManager;

        public VerifyLoginCodeModel(
            UserManager<CarmenStitchAndPressUserModel> userManager,
            SignInManager<CarmenStitchAndPressUserModel> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }


        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
        public class InputModel 
        {
            public string ReturnUrl { get; set; } = "";
            [Required]
            public string Code { get; set; } = "";

        }

        public async Task<IActionResult> OnPostAsync()
        {
            var email = TempData["Email"]?.ToString();
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToPage("./Login");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || user.LoginVerificationCode != Input.Code)
            {
                
                ModelState.AddModelError(string.Empty, "Invalid verification code.");
                TempData["Email"] = email;
                return Page();
            }

            //Check expiration
            if (user.CodeSentAt == null || user.CodeSentAt < DateTime.UtcNow.AddMinutes(-10))
            {
                ModelState.AddModelError(string.Empty, "Verification code has expired.");
                return Page();
            }

            //clear the code and sign in
            user.LoginVerificationCode = null;
            user.CodeSentAt = null;
            await _userManager.UpdateAsync(user);

            if (!bool.TryParse(TempData["RememberMe"]?.ToString(), out var rememberMe))
            {
                rememberMe = false; //default fallback
            }
            RememberMe = rememberMe;


            var authProperties = new AuthenticationProperties
            {
                IsPersistent = rememberMe,
                ExpiresUtc = rememberMe ? DateTime.UtcNow.AddDays(365) : null,
                AllowRefresh = true,
                IssuedUtc = DateTime.UtcNow
            };

            //var claims = new List<Claim>
            //{
            //    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
            //    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
            //};

           // var principal = await _signInManager.CreateUserPrincipalAsync(user);

         //   ClaimsIdentity claimsIdentity = new ClaimsIdentity(IdentityConstants.ApplicationScheme, ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
         //   await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal,authProperties);
            await _signInManager.SignInAsync(user, authProperties, IdentityConstants.ApplicationScheme);

            // Ensure the cookie reflects the latest security stamp
            await _signInManager.RefreshSignInAsync(user);

            return LocalRedirect(ReturnUrl ?? "~/");
        }
    }
}
