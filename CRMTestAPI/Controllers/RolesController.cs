using System;
using ActionFilters;
using Entities.Extensions;
using Entities.Models;
using LoggerService.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Repositories.Contracts;

namespace CRMTestAPI.Controllers
{
    [Route("roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryWrapper _repositories;

        public RolesController(ILoggerManager logger, IRepositoryWrapper repositories)
        {
            _logger = logger;
            _repositories = repositories;
        }


        [HttpGet("{id}", Name = "GetRoleById"), Authorize(Roles = "admin")]
        [ServiceFilter(typeof(EntityExistsActionFilter<Role>))]
        public IActionResult GetById(Guid id)
        {
            return Ok(HttpContext.Items["entity"]);
        }

        [HttpPost, Authorize(Roles = "admin")]
        [ServiceFilter(typeof(EntityIsValidActionFilter))]
        public IActionResult Post([FromBody] Role role)
        {
            if (_repositories.User.FindById(role.UserId).IsNull())
            {
                var error = new ModelStateDictionary();
                error.AddModelError("User", "Could not find the given user.");
                return NotFound(error);
            }

            if (!_repositories.Role.FindByUserIdAndType(role.UserId, role.Type).IsNull())
            {
                var error = new ModelStateDictionary();
                error.AddModelError("Type", "Already found role type for the given user.");
                return Conflict(error);
            }

            _repositories.Role.Create(role);
            _repositories.Role.Save();
            return CreatedAtRoute("GetRoleById", new {id = role.Id}, role);
        }

        [HttpDelete("{id}"), Authorize(Roles = "admin")]
        [ServiceFilter(typeof(EntityExistsActionFilter<Role>))]
        public IActionResult Delete(Guid id)
        {
            _repositories.Role.Delete((Role) HttpContext.Items["entity"]);
            _repositories.Role.Save();
            return NoContent();
        }
    }
}