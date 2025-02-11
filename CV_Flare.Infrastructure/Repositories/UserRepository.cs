using AutoMapper;
using CV_Flare.Application.DTOs;
using CV_Flare.Application.Interface.Account;
using CV_FLare.Domain.Models;
using CV_Flare.Infrastructure.DB;
using CV_Flare.Infrastructure.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Runtime.Intrinsics.X86;
using Microsoft.AspNetCore.Mvc;

namespace CV_Flare.Infrastructure.Repositories
{
    public class UserRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRepository(ApplicationDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration config, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public async Task<string> CheckEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user.Email;
        }

        public async Task<TokenResponseDTO> Login(AccountDTO accountDTO)
        {
            if (string.IsNullOrWhiteSpace(accountDTO.Email) || string.IsNullOrWhiteSpace(accountDTO.Password))
            {
                throw new ArgumentNullException("Email and password must not be null");
            }

            // check user identity
            var user = await _userManager.FindByEmailAsync(accountDTO.Email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found");
            }

            var result = await _signInManager.PasswordSignInAsync(accountDTO.Email, accountDTO.Password, false, false);
            if (result.Succeeded)
            {
                var token = await GenerateAccessToken(user);
                if (token != null)
                {
                    return new TokenResponseDTO
                    {
                        AccessToken = token.AccessToken,
                        RefreshToken = token.RefreshToken
                    };
                }
            }

            throw new UnauthorizedAccessException("Invalid login attempt");
        }

        public async Task<TokenResponseDTO> LoginGoogle(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            // chưa có thì tạo thông tin đăng nhập lần đầu
            if (user == null)
            {
                user = new IdentityUser()
                {
                    UserName = email,
                    Email = email
                };
                var identity = await _userManager.CreateAsync(user);
                if (identity.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync(StaticUserRoles.User))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.User));
                    }
                    await _userManager.AddToRoleAsync(user, StaticUserRoles.User);

                    using (var transaction = await _context.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            var accountNew = new User();

                            // Start
                            // Set giá trị mặc định cho Account
                            accountNew.UserEmail = user.Email;
                            accountNew.UserPassword = RandomPassword.GenaratePasss(6, true);
                            accountNew.Username = user.Email;
                            accountNew.UserFullname = "Anonymous Customer";
                            accountNew.UserRole = 1;
                            accountNew.UserPhone = "";
                            accountNew.UserCreateAt = DateTime.Now;
                            accountNew.UserImg = "https://res.cloudinary.com/dgqkuowgr/image/upload/v1729254303/user/haoncce171957%40fpt.edu.vn.jpg";
                            accountNew.AspUId = user.Id;
                            // End 

                            _context.Users.Add(accountNew);
                            await _context.SaveChangesAsync();

                            var wallet = new Wallet()
                            {
                                Balance = 0,
                                UserId = accountNew.UserId
                            };
                            _context.Wallets.Add(wallet);
                            await _context.SaveChangesAsync();

                            await transaction.CommitAsync();

                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            throw;
                        }
                    }
                }
            }

            var result = await _signInManager.CanSignInAsync(user);
            var token = await GenerateAccessToken(user);
            if (token != null)
            {
                return new TokenResponseDTO
                {
                    AccessToken = token.AccessToken,
                    RefreshToken = token.RefreshToken
                };
            }
            throw new UnauthorizedAccessException("Invalid login attempt");
        }

        private async Task<TokenResponseDTO> GenerateAccessToken(IdentityUser identityUser)
        {
            var authClaims = new List<Claim>
             {
                 new Claim(ClaimTypes.Email, identityUser.Email),
                 new Claim(JwtRegisteredClaimNames.Sub, identityUser.Id),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
             };
            var userRole = await _userManager.GetRolesAsync(identityUser);
            foreach (var roles in userRole)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
            }
            // key value
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            // access token
            var token = new JwtSecurityToken(
                issuer: _config["JWT:Issuer"],
                audience: _config["JWT:Audience"],
                expires: DateTime.UtcNow.AddMinutes(30),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256Signature)
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = GenerateRefreshToken();

            await _userManager.SetAuthenticationTokenAsync(identityUser, "CVFlare", "RefreshToken", refreshToken.RefreshToken);


            return new TokenResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public TokenRefreshDTO GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                var token = Convert.ToBase64String(randomNumber).Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", "");

                var expiration = DateTime.UtcNow.AddDays(3);

                return new TokenRefreshDTO()
                {
                    RefreshToken = token,
                    Expiration = expiration
                };
            }
        }

        public async Task<bool> Logout(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                await _signInManager.SignOutAsync();
                await _userManager.RemoveAuthenticationTokenAsync(user, "CVFlare", "RefreshToken");
                return true;
            }
            return false;
        }

        public async Task<TokenResponseDTO> RefreshToken(TokenRefreshDTO refreshToken)
        {
            var userList = await _userManager.Users.ToListAsync(); // Lấy tất cả người dùng từ database

            var user = userList.SingleOrDefault(u =>
                _userManager.GetAuthenticationTokenAsync(u, "CVFlare", "RefreshToken").Result == refreshToken.RefreshToken);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid refresh token.");
            }

            if (refreshToken.Expiration < DateTime.UtcNow)
            {
                await _userManager.RemoveAuthenticationTokenAsync(user, "CVFlare", "RefreshToken");
                throw new UnauthorizedAccessException("Refresh token expired.");
            }
            var tokens = await GenerateAccessToken(user);
            return tokens;
        }

        public async Task<IdentityUser> Register(AccountDTO accountDTO)
        {
            if (string.IsNullOrWhiteSpace(accountDTO.Email) || string.IsNullOrWhiteSpace(accountDTO.Password))
            {
                throw new ArgumentNullException("Email and password must not be null or empty");
            }

            var email = await _userManager.FindByEmailAsync(accountDTO.Email);
            if (email != null)
            {
                throw new InvalidOperationException("Email already exists");
            }

            var user = new IdentityUser
            {
                UserName = accountDTO.Email,
                Email = accountDTO.Email
            };

            var identityResult = await _userManager.CreateAsync(user, accountDTO.Password);
            if (identityResult.Succeeded)
            {
                // Gán vai trò User
                if (!await _roleManager.RoleExistsAsync(StaticUserRoles.User))
                {
                    await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.User));
                }
                await _userManager.AddToRoleAsync(user, StaticUserRoles.User);

                // Giao dịch thêm vào bảng Users và Wallets
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var accountNew = new User
                        {
                            Username = accountDTO.Email,
                            UserEmail = accountDTO.Email,
                            UserPassword = accountDTO.Password,
                            UserFullname = "Anonymous Customer",
                            UserRole = 1,
                            UserPhone = "+84",
                            UserImg = "",
                            UserCreateAt = DateTime.Now,
                            AspUId = user.Id
                        };

                        _context.Users.Add(accountNew);
                        await _context.SaveChangesAsync();

                        var wallet = new Wallet
                        {
                            Balance = 0,
                            UserId = accountNew.UserId
                        };
                        _context.Wallets.Add(wallet);

                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        return user;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        await _userManager.DeleteAsync(user);
                        Console.WriteLine($"Error during registration: {ex.Message}");
                        throw;
                    }
                }
            }

            throw new InvalidOperationException("User registration failed");
        }

        public async Task<bool> ResetPassword(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                // Mã hóa mật khẩu mới
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, password);
                var identity = await _userManager.UpdateAsync(user);
                if (identity.Succeeded)
                {
                    var account = await _context.Users.FirstOrDefaultAsync(a => a.AspUId == user.Id);
                    if (account != null)
                    {
                        account.UserPassword = password;
                        _context.Users.Update(account);
                        await _context.SaveChangesAsync();
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<IEnumerable<User>> GetAllUserAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == email);
            if (user == null) throw new Exception("Account not found");
            return user;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null)
            {
                throw new Exception($"Account with ID {id} not found.");
            }
            return user;
        }

        public async Task<UserProfileDTO> GetUserProfile(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail.Equals(email));
            if (user == null)
            {
                throw new Exception();
            }
            return _mapper.Map<UserProfileDTO>(user);
        }

        public async Task<UserProfileDTO> UpdateUserProfile(UserProfileDTO userProfileDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(a => a.UserId == userProfileDTO.UserId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            _mapper.Map(userProfileDTO, user);
            _context.Users.Update(user);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return _mapper.Map<UserProfileDTO>(user);
            }
            throw new Exception("No changes were made to the user profile.");
        }

        public async Task<User> GetUserById(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.AspUId == id);
            if (user == null)
            {
                throw new Exception($"Account with ID {id} not found.");
            }
            return user;
        }
    }
}
