using System;
using System.Linq;
using ActionFilters;
using CryptoHelper;
using Entities.Extensions;
using Entities.Models;
using Entities.Models.Reduced;
using LoggerService.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;

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
            return Ok(_repositories.User.FindAll().Include(user => user.Roles).Select(user => user.ToReducedUser()));
        }

        [HttpGet("{id}", Name = "GetUserById"), Authorize(Roles = "admin")]
        [ServiceFilter(typeof(EntityExistsActionFilter<User>))]
        public IActionResult GetById(Guid id)
        {
            //return Ok((HttpContext.Items["entity"] as User).ToReducedUser());
            return Ok(_repositories.User.FindWithRolesById(id).ToReducedUser());
        }

        [HttpPost, Authorize(Roles = "admin")]
        [ServiceFilter(typeof(EntityIsValidActionFilter))]
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
        [ServiceFilter(typeof(EntityExistsActionFilter<User>))]
        [ServiceFilter(typeof(EntityIsValidActionFilter))]
        public IActionResult Put([FromBody] UserProfile user, Guid id)
        {
            (HttpContext.Items["entity"] as User).Map(user);
            _repositories.User.Update(HttpContext.Items["entity"] as User);
            _repositories.User.Save();
            return NoContent();
        }

        [HttpDelete("{id}"), Authorize(Roles = "admin")]
        [ServiceFilter(typeof(EntityExistsActionFilter<User>))]
        public IActionResult Delete(Guid id)
        {
            _repositories.User.Delete(HttpContext.Items["entity"] as User);
            _repositories.User.Save();
            return NoContent();
        }
    }
}