﻿using FootballApi.Exceptions;
using FootballApi.Models;
using FootballApi.Models.UserOperations;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FootballApi.Services
{
    public interface IUserControllerService
    {
        IPasswordHasher<ApiUser> PasswordHasher { get; }

        string login(LoginUser data);
        void register(CreateUser data);
    }

    public class UserControllerService : IUserControllerService
    {
        private readonly FootballDBContext dBContext;
        private readonly IPasswordHasher<ApiUser> passwordHasher;
        private readonly AuthenticationSettings authenticationSettings;

        public UserControllerService(FootballDBContext dBContext, IPasswordHasher<ApiUser> passwordHasher, AuthenticationSettings authenticationSettings)
        {
            this.dBContext = dBContext;
            this.passwordHasher = passwordHasher;
            this.authenticationSettings = authenticationSettings;
        }
        public IPasswordHasher<ApiUser> PasswordHasher { get; }

        public string login(LoginUser data)
        {
            var user = dBContext.Users.Where(u => u.Email == data.Email).FirstOrDefault();
            if (user is null)
            {
                throw new UnauthorizedException("Invalid email");
            }
            var result = passwordHasher.VerifyHashedPassword(user, user.HashedPassword, data.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedException("Invalid password");
            }
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(authenticationSettings.JwtExpireDays);
            var token = new JwtSecurityToken(authenticationSettings.JwtIssuer,
                            authenticationSettings.JwtIssuer,
                            claims,
                            expires: expires,
                            signingCredentials: cred);
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public void register(CreateUser data)
        {
            var mails = dBContext.Users.Select(u => u.Email).ToList();
            if (mails.Contains(data.Email))
            {
                throw new BadRequestException("Email is already taken, try to login");
            }

            ApiUser user = new() { Email = data.Email };
            var hashedPassword = passwordHasher.HashPassword(user, data.Password);
            user.HashedPassword = hashedPassword;
            dBContext.Users.Add(user);
            dBContext.SaveChanges();
        }
    }
}
