using CV_Flare.Application.DTOs;
using CV_FLare.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV_Flare.Application.Interface.Account
{
    public interface IAccountRepository
    {
        Task<TokenResponseDTO> Login(AccountDTO accountDTO);
        Task<TokenResponseDTO> RefreshToken(TokenRefreshDTO refreshToken);
        Task<IdentityUser> Register(AccountDTO accountDTO);
        Task<bool> Logout(string email);
        Task<TokenResponseDTO> LoginGoogle(string email);
        Task<string> CheckEmail(string email);
        Task<bool> ResetPassword(string email, string password);

        Task<IEnumerable<User>> GetAllUserAsync();
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByIdAsync(int id);    

    }
}
