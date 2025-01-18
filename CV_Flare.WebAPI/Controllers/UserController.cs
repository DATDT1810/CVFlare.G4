using CV_Flare.Application.DTOs;
using CV_Flare.Application.Interface.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CV_Flare.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAccountService _accountService;
        //private readonly IEmailService emailService;

        public UserController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] AccountDTO accountDTO)
        {
            if (accountDTO.Email != null && accountDTO.Password != null)
            {
                var result = await _accountService.Login(accountDTO);
                if (result != null)
                {
                    var token = new
                    {
                        accessToken = result.AccessToken,
                        refreshToken = result.RefreshToken,
                    };
                    return Ok(result);
                }
            }
            return Unauthorized("Invalid credentials");
        }


        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(AccountDTO accountDTO)
        {
            if (accountDTO.Email != null && accountDTO.Password != null)
            {
                var result = await _accountService.Register(accountDTO);
                if (result != null)
                {
                    return Ok(result);
                    // trả về thông tin user
                }
            }
            return StatusCode(500, "Create failed");
        }

        [HttpPost]
        [Route("Logout")]
        //[Authorize]
        public async Task<IActionResult> Logout()
        {
            var user = User.FindFirstValue(ClaimTypes.Email);
            if (user != null)
            {
                var result = await _accountService.Logout(user);
                if (result)
                {
                    return StatusCode(200);
                }
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordRequest passwordRequest)
        {
            if (passwordRequest != null)
            {

                var code = HttpContext.Session.GetString("code")?.Trim();

                if (passwordRequest.Code.Trim().Equals(code))
                {

                    var result = await _accountService.ResetPassword(passwordRequest.Email, passwordRequest.Password);
                    if (result)
                    {
                        HttpContext.Session.Remove("code");
                        return Ok();
                    }
                }

            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {
            var userList = await _accountService.GetAllUserAsync();
            return Ok(userList);
        }
        [HttpGet("{id}", Name = "GetUserById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _accountService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }
    }
}
