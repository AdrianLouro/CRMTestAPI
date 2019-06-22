using System;
using System.Linq;
using ActionFilters;
using Entities.Extensions;
using Entities.Models;
using Entities.Models.Reduced;
using LoggerService.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Repositories.Contracts;
using static CryptoHelper.Crypto;

namespace CRMTestAPI.Controllers
{
    [Route("users")]
    [Authorize(Roles = "admin", Policy = "NotDeleted")]
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

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_repositories.User.FindAllNotDeletedWithRoles().Select(user => user.ToReducedUser()));
        }

        [HttpGet("{id}", Name = "GetUserById")]
        [ServiceFilter(typeof(EntityExistsActionFilter<User>))]
        public IActionResult GetById(Guid id)
        {
            //return Ok((User) (HttpContext.Items["entity"]).ToReducedUser());
            return Ok(_repositories.User.FindWithRolesById(id).ToReducedUser());
        }

        [HttpPost]
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

            user.Password = HashPassword(user.Password);
            _repositories.User.Create(user);
            _repositories.User.Save();
            return CreatedAtRoute("GetUserById", new {id = user.Id}, user.ToReducedUser());
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(EntityExistsActionFilter<User>))]
        [ServiceFilter(typeof(EntityIsValidActionFilter))]
        public IActionResult Put([FromBody] UserProfile user, Guid id)
        {
            ((User) HttpContext.Items["entity"]).Map(user);
            _repositories.User.Update((User) HttpContext.Items["entity"]);
            _repositories.User.Save();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(EntityExistsActionFilter<User>))]
        public IActionResult Delete(Guid id)
        {
            ((User) HttpContext.Items["entity"]).DeletedAt = DateTime.Now;
            _repositories.User.Save();
            return NoContent();
        }
    }
}