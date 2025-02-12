using AutoMapper.Internal;
using Azure.Core;
using CV_Flare.Application.DTOs;
using CV_Flare.Application.Interface.Account;
using CV_Flare.Application.Interface.Email;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IEmailService _emailService;

        public UserController(IAccountService accountService, IEmailService emailService)
        {
            _accountService = accountService;
            _emailService = emailService;
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
        [Route("LoginWithGoogle")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] EmailDTO emailDTO)
        {
            if (emailDTO.email != null)
            {
                var result = await _accountService.LoginGoogle(emailDTO.email);
                if (result != null)
                {
                    var token = new
                    {
                        AccessToken = result.AccessToken,
                        RefreshToken = result.RefreshToken
                    };
                    return Ok(token);
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
        [Authorize]
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

        [HttpPost]
        [Route("CheckEmail")]
        public async Task<IActionResult> CheckEmail([FromBody] EmailDTO emailDTO)
        {
            if (emailDTO.email != null)
            {
                var result = await _accountService.CheckEmail(emailDTO.email);
                if (result != null)
                {
                    return Ok();
                }
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("SendCode")]
        public async Task<IActionResult> SendCode(EmailDTO emailDTO)
        {
            if (emailDTO != null)
            {
                MailRequest mailRequest = new MailRequest();
                mailRequest.ToEmail = emailDTO.email;
                mailRequest.Subject = "Reset Password";

                // tạo mã code 6 số lưu vào session
                var code = new Random().Next(100000, 999999).ToString();
                HttpContext.Session.SetString("code", code);

                string content = "This is the verify code to reset your password";
                mailRequest.Body = _emailService.GetCodeHtmlContent(content, code); ;
                await _emailService.SendEmailAsync(mailRequest);
                return Ok();
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

        [HttpGet("uid/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _accountService.GetUserById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpGet]
        [Route("GetUserProfile")]
        public async Task<IActionResult> GetUserProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.Email);
            //var userId = "datdtce171751@fpt.edu.vn"; // test api
            if (userId != null)
            {
                var userProfile = await _accountService.GetUserProfile(userId);
                if (userProfile != null) return Ok(userProfile);
            }
            return NotFound();
            //var user = await _accountService.GetUserByEmailAsync(email);
            //if (user == null) return NotFound();
            //return Ok(user);
        }

        //[Authorize]
        [HttpPost]
        [Route("UpdateUserProfile")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UserProfileDTO userProfileDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updateProfile = await _accountService.UpdateUserProfile(userProfileDTO);
                return Ok(updateProfile);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "An error occurred while updating the profile.");
            }
        }

    }
}
