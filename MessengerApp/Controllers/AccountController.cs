using MessengerApp.BLL.DTO;
using MessengerApp.BLL.Interfaces;
using MessengerApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MessengerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("signUp")]
        public async Task<IActionResult> SignUp([FromForm] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { errorText = "Некорректные данные" });
            }
            try
            {
                var authResponse = await _accountService.SignUpAsync(new UserLogInDTO { Login = model.Login, Password = model.Password });
                return Ok(authResponse);
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { errorText = e.Message });
            }
        }

        [HttpPost("signIn")]
        public async Task<IActionResult> SignIn([FromForm] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { errorText = "Некорректные данные" });
            }
            try
            {
                var authResponse = await _accountService.SignInAsync(new UserLogInDTO { Login = model.Login, Password = model.Password });
                return Ok(authResponse);
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { errorText = e.Message });
            }
        }
    }
}
