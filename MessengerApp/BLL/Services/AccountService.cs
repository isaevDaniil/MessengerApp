using MessengerApp.BLL.DTO;
using MessengerApp.BLL.Interfaces;
using MessengerApp.DAL.Entities;
using MessengerApp.DAL.Intrefaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MessengerApp.BLL.Services
{
    public class AccountService : IAccountService
    {
        private IUnitOfWork _repository;

        public AccountService(IUnitOfWork unitOfWork)
        {
            _repository = unitOfWork;
        }

        public async Task<AuthorizationResponseDTO> SignInAsync(UserLogInDTO userLogInDto)
        {
            var identity = await GetIdentity(userLogInDto.Login, userLogInDto.Password);
            if (identity == null)
            {
                throw new ArgumentException("Неверное имя пользователя или пароль");
            }

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var authResponse = new AuthorizationResponseDTO { AccessToken = encodedJwt, UserLogin = identity.Name };
            return authResponse;
        }

        public async Task<AuthorizationResponseDTO> SignUpAsync(UserLogInDTO userLogInDto)
        {
            var user = await _repository.Users.CreateUserAsync(new User { Login = userLogInDto.Login, Password = userLogInDto.Password });

            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                };
            var identity = new ClaimsIdentity(claims, "Token");

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var authResponse = new AuthorizationResponseDTO { AccessToken = encodedJwt, UserLogin = identity.Name };
            return authResponse;
        }

        private async Task<ClaimsIdentity> GetIdentity(string username, string password)
        {
            var users = await _repository.Users.GetAllUsersAsync();
            var user = users.FirstOrDefault(x => x.Login == username && x.Password == password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                };
                var claimsIdentity = new ClaimsIdentity(claims, "Token");
                return claimsIdentity;
            }
            return null;
        }
    }
}
