using System;
using System.Linq;
using Contracts;
using CryptoHelper;
using Entities.Extensions;
using Entities.Models;
using Entities.Models.Reduced;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CRMTestAPI.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryWrapper _repositories;

        public UsersController(ILoggerManager logger, IRepositoryWrapper repositories)
        {
            _logger = logger;
            _repositories = repositories;
        }

        [HttpGet, Authorize(Roles = "admin")]
        public IActionResult GetAll()
        {
            return Ok(_repositories.User.FindAll());
        }

        [HttpGet("{id}", Name = "GetUserById"), Authorize(Roles = "admin")]
        public IActionResult GetById(Guid id)
        {
            return Ok(_repositories.User.FindWithRolesById(id));
        }

        [HttpPost, Authorize(Roles = "admin")]
        public IActionResult Post([FromBody] User user)
        {
            User dbUser = _repositories.User.FindByEmail(user.Email);
            if (!dbUser.IsNull())
            {
                var error = new ModelStateDictionary();
                error.AddModelError("Email", "Email is already in use.");
                return Conflict(new BadRequestObjectResult(error));
            }

            user.Password = Crypto.HashPassword(user.Password);
            _repositories.User.Create(user);
            _repositories.User.Save();
            return CreatedAtRoute("GetUserById", new {id = user.Id}, user);
        }

        [HttpPut("{id}"), Authorize(Roles = "admin")]
        public IActionResult Put([FromBody] UserProfile user, Guid id)
        {
            User dbUser = _repositories.User.FindByCondition(u => u.Id.Equals(id)).FirstOrDefault();
            dbUser.Map(user);
            _repositories.User.Update(dbUser);
            _repositories.User.Save();
            return NoContent();
        }

        [HttpDelete("{id}"), Authorize(Roles = "admin")]
        public IActionResult Delete(Guid id)
        {
            User user = _repositories.User.FindByCondition(u => u.Id.Equals(id)).FirstOrDefault();
            _repositories.User.Delete(user);
            _repositories.User.Save();
            return NoContent();
        }
    }
}