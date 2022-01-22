using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Verbum.Identity.Data;
using Verbum.Identity.Models;

namespace Verbum.Identity.Repositories
{
    public class AuthRepository
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
     

        public AuthRepository(SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager, AuthDbContext context)

        {
            _signInManager = signInManager;
            _userManager = userManager;
            
        }

        public async Task<string> Register(RegModel viewModel)
        {
          
            
                var userTmpEmail = await _userManager.FindByEmailAsync(viewModel.Email);
                var userTmpName = await _userManager.FindByNameAsync(viewModel.UserName);

                if (userTmpEmail != null || userTmpName != null)
                {
                    return "Пользователь с таким Логином или Email уже существует!!!";
                }

                AppUser user = new AppUser();
                user.UserName = viewModel.UserName;
                user.Email = viewModel.Email;


                var result = await _userManager.CreateAsync(user, viewModel.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return "Registered";
                }


             return "Registration error";
           
        }


        public async Task<string> UpdateUser(AppUser user) {

            if (user != null) {
                var userDb = await _userManager.FindByIdAsync(user.Id);
                if (userDb != null) {
                    userDb.FirstName = user.FirstName;
                    userDb.LastName = user.LastName;
                    userDb.PhoneNumber = user.PhoneNumber;
                    await _userManager.UpdateAsync(userDb);
                    return "ok";
                }
                return "Пользователь не найден!!!";
            }
            return "Нечего обновлять";
        }


        public async Task<ActionResult<AppUser>> GetUser(string name) {
            var user = await _userManager.FindByNameAsync(name);

            return user;
        }

    }
}
