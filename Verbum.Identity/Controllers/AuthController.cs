using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Verbum.Identity.Data;
using Verbum.Identity.Models;

namespace Verbum.Identity.Controllers
{
    
    public class AuthController : Controller
    {

        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IIdentityServerInteractionService _interactionService;
        private readonly UserDbContext _userContext;

        public AuthController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager,
           IIdentityServerInteractionService identityServerInteractionService, UserDbContext userDbContext) =>
            (_signInManager, _userManager, _interactionService, _userContext) = (signInManager, userManager, identityServerInteractionService, userDbContext);

        [HttpGet]
        public IActionResult Login(string returnUrl) {
            
            var viewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl,
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel) {

            if (!ModelState.IsValid) {
                return View(viewModel);
            }

            var user = await _userManager.FindByNameAsync(viewModel.UserName);
            if (user == null) {
                ModelState.AddModelError(string.Empty, "Користувача з таким нікнеймом не знайдено");
                return View(viewModel);
            }

            var result = await _signInManager.PasswordSignInAsync(viewModel.UserName, viewModel.Password, false, false);
            if (result.Succeeded) {
                return Redirect(viewModel.ReturnUrl);
            }
            ModelState.AddModelError(string.Empty, "Помилка авторизації");
            return View(viewModel);
        }


        [HttpGet]
        public IActionResult Register(string returnUrl) {

            var viewModel = new RegistrationViewModel
            {
                ReturnUrl = returnUrl,
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationViewModel viewModel) {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var userTmpEmail = await _userManager.FindByEmailAsync(viewModel.Email);
            var userTmpName = await _userManager.FindByNameAsync(viewModel.UserName);

            if (userTmpEmail != null || userTmpName != null)
            {
                ModelState.AddModelError(string.Empty, "Нікнейм або пошта вже існують");
                return View(viewModel);
            }

            var User = new AppUser
            {
                UserName = viewModel.UserName,
                Email = viewModel.Email,

            };

            

            var result = await _userManager.CreateAsync(User, viewModel.Password);
            if (result.Succeeded) {
                await _signInManager.SignInAsync(User, false);
                var currentUser = await _userManager.FindByNameAsync(viewModel.UserName);

                var verbumUser = new VerbumUser
                {
                    Id = Guid.Parse(currentUser.Id),
                    NickName = viewModel.UserName,
                    Email = viewModel.Email,
                    IsOnline = false,
                    UserRegistrationDate = DateTime.UtcNow
                };

                await _userContext.Users.AddAsync(verbumUser);
                await _userContext.SaveChangesAsync();
                return Redirect(viewModel.ReturnUrl);
            }
            ModelState.AddModelError(string.Empty, "Помилка реєстрації! Зверніть увагу!!! Пароль повинен бути не меньше 6 символів," +
                " також повинен включати хоч би одну цифру, маленьку та великі літери");
            return View(viewModel);

        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signInManager.SignOutAsync();
            var logoutRequest = await _interactionService.GetLogoutContextAsync(logoutId);
            return Redirect(logoutRequest.PostLogoutRedirectUri);
        }
    }

}

