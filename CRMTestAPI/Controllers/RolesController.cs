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
        public IActionResult GetById(Guid id)
        {
            return Ok(_repositories.Role.FindById(id));
        }

        [HttpPost, Authorize(Roles = "admin")]
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
                error.AddModelError("Type", "Already found role type for given user.");
                return NotFound(error);
            }

            _repositories.Role.Create(role);
            _repositories.Role.Save();
            return CreatedAtRoute("GetRoleById", new {id = role.Id}, role);
        }

        [HttpDelete("{id}"), Authorize(Roles = "admin")]
        public IActionResult Delete(Guid id)
        {
            _repositories.Role.Delete(_repositories.Role.FindById(id));
            _repositories.Role.Save();
            return NoContent();
        }
    }
}