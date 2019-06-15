using System;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRMTestAPI.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private ILoggerManager _logger;

        public UsersController(ILoggerManager logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok();
        }

        [HttpGet("{id}", Name = "GetUserById")]
        public IActionResult GetById(Guid id)
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            user.Id = Guid.NewGuid();
            return CreatedAtRoute("GetUserById", new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] User user)
        {
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            return NoContent();
        }
    }
}