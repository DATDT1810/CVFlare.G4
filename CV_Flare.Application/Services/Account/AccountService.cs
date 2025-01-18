using CV_Flare.Application.DTOs;
using CV_Flare.Application.Interface.Account;
using CV_FLare.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV_Flare.Application.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public Task<string> CheckEmail(string email)
        {
            return _accountRepository.CheckEmail(email);
        }

        public Task<IEnumerable<User>> GetAllUserAsync()
        {
            return _accountRepository.GetAllUserAsync();
        }

        public Task<User> GetUserByEmailAsync(string email)
        {
            return _accountRepository.GetUserByEmailAsync(email);
        }

        public Task<User> GetUserByIdAsync(int id)
        {
            return _accountRepository.GetUserByIdAsync(id);
        }

        public Task<TokenResponseDTO> Login(AccountDTO accountDTO)
        {
            return _accountRepository.Login(accountDTO);
        }

        public Task<TokenResponseDTO> LoginGoogle(string email)
        {
            return _accountRepository.LoginGoogle(email);
        }

        public Task<bool> Logout(string email)
        {
            return _accountRepository.Logout(email);
        }

        public Task<TokenResponseDTO> RefreshToken(TokenRefreshDTO refreshToken)
        {
            return _accountRepository.RefreshToken(refreshToken);
        }

        public Task<IdentityUser> Register(AccountDTO accountDTO)
        {
            return _accountRepository.Register(accountDTO);
        }

        public Task<bool> ResetPassword(string email, string password)
        {
            return _accountRepository.ResetPassword(email, password);
        }
    }
}
