using System;
using System.Linq;
using Contracts;
using Entities.Extensions;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            return Ok(_repositories.User.FindByCondition(user => user.Id.Equals(id)).FirstOrDefault());
        }

        [HttpPost, Authorize(Roles = "admin")]
        public IActionResult Post([FromBody] User user)
        {
            user.Id = Guid.NewGuid();
            _repositories.User.Create(user);
            _repositories.User.Save();
            return CreatedAtRoute("GetUserById", new {id = user.Id}, user);
        }

        [HttpPut("{id}"), Authorize(Roles = "admin")]
        public IActionResult Put(Guid id, [FromBody] User user)
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