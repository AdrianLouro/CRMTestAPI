using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using CRMTestAPI.Configuration;
using Entities.Extensions;
using Entities.Models;
using Entities.Models.Reduced;
using LoggerService.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Repositories.Contracts;
using Security;
using static System.DateTime;
using static CryptoHelper.Crypto;
using static Microsoft.IdentityModel.Tokens.SecurityAlgorithms;

namespace CRMTestAPI.Controllers
{
    public class AuthController : ControllerBase
    {
        private IOptions<AuthConfig> _config;
        private ILoggerManager _logger;
        private IRepositoryWrapper _repository;

        public AuthController(ILoggerManager logger, IRepositoryWrapper repository, IOptions<AuthConfig> config)
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
            return dbUser.IsNull()
                   || dbUser.IsDeleted()
                   || !VerifyHashedPassword(dbUser.Password, user.Password);
        }

        private JwtSecurityToken GetJwtSecurityToken(User dbUser)
        {
            return JwtSecurityTokenFactory.GetToken(
                _config.Value.Issuer,
                _config.Value.Audience,
                dbUser.Id.ToString(),
                dbUser.Roles.Select(role => role.Type),
                Now.AddMonths(_config.Value.JwtExpirationInMonths),
                _config.Value.JwtSecretKey,
                HmacSha256
            );
        }
    }
}