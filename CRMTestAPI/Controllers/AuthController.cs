using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Contracts;
using CryptoHelper;
using CRMTestAPI;
using Entities.Models;
using Entities.Models.Reduced;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace PrematureKidsAPI.Controllers
{
    public class AuthController : ControllerBase
    {
        private IOptions<AppConfig> _config;
        private ILoggerManager _logger;
        private IRepositoryWrapper _repository;

        public AuthController(ILoggerManager logger, IRepositoryWrapper repository, IOptions<AppConfig> config)
        {
            _logger = logger;
            _repository = repository;
            _config = config;
        }

        [HttpPost, Route("login")]
        public IActionResult Login([FromBody] Login user)
        {
            User dbUser = _repository.User.FindWithRolesByEmail(user.Email);

            if (UserDoesNotExist(user, dbUser)) return Unauthorized();

            return Ok(new {Token = new JwtSecurityTokenHandler().WriteToken(GetJwtSecurityToken(dbUser))});
        }

        private bool UserDoesNotExist(Login user, User dbUser)
        {
            return dbUser == null || (dbUser != null && !Crypto.VerifyHashedPassword(dbUser.Password, user.Password));
        }

        private JwtSecurityToken GetJwtSecurityToken(User dbUser)
        {
            var claims = dbUser.Roles.Select(role => new Claim("role", role.Type.ToLower())).ToList();
            claims.AddRange(new List<Claim>() {new Claim("sub", dbUser.Id.ToString())});
            return new JwtSecurityToken(
                issuer: "http://localhost:5000",
                audience: "http://localhost:5000",
                claims: claims,
                expires: DateTime.Now.AddMonths(_config.Value.JwtExpirationInMonths),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Value.JwtSecretKey)),
                    SecurityAlgorithms.HmacSha256
                )
            );
        }
    }
}