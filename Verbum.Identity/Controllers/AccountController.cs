using Microsoft.AspNetCore.Mvc;
using Verbum.Identity.Models;
using Verbum.Identity.Repositories;

namespace Verbum.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly AuthRepository _authRepository;

        public AccountController(AuthRepository authRepository)
        {
            _authRepository = authRepository;
        }


        [HttpPost("register")]
        public async Task<ActionResult> Reg(RegModel viewModel)
        {
            var rezult = await _authRepository.Register(viewModel);

            if (rezult == "Registered")
                return Ok(rezult);
            else return BadRequest(rezult);
        }


        [HttpPut("update")]
        public async Task<ActionResult> Update(AppUser user) {

            var rezult = await _authRepository.UpdateUser(user);

            return Ok(rezult);
        }

    
    }
}
