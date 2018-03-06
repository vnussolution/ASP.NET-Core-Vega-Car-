using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vega.ViewModels;

namespace Vega.Controllers {
    public class AccountController : Controller {

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController (SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager) {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Login () {
            return View ();
        }

        [HttpPost]
        public async Task<IActionResult> Login (LoginViewModel viewModel) {
            if (!ModelState.IsValid) {
                return View (viewModel);
            }

            var user = await _userManager.FindByNameAsync (viewModel.UserName);
            if (user != null) {
                var result = await _signInManager.PasswordSignInAsync (user, viewModel.Password, false, false);
                if (result.Succeeded) {
                    return RedirectToAction ("PieIndex", "Home");
                }
            }
            ModelState.AddModelError ("", "User name/password not found");
            return View (viewModel);
        }

        public IActionResult Register () {
            return View ();
        }

        [HttpPost]
        public async Task<IActionResult> Register (LoginViewModel viewModel) {
            if (ModelState.IsValid) {
                //get username
                var user = new IdentityUser () { UserName = viewModel.UserName };

                //create new user and return result if succeed
                var result = await _userManager.CreateAsync (user, viewModel.Password);
                if (result.Succeeded) {
                    return RedirectToAction ("PieIndex", "Home");
                }
            }
            return View (viewModel);
        }

        public async Task<IActionResult> Logout () {
            await _signInManager.SignOutAsync ();
            return RedirectToAction ("PieIndex", "Home");
        }
    }
}