using System;
using ActionFilters;
using Entities.Extensions;
using Entities.Models;
using Entities.Models.Reduced;
using LoggerService.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;

namespace CRMTestAPI.Controllers
{
    [Route("customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryWrapper _repositories;

        public CustomersController(ILoggerManager logger, IRepositoryWrapper repositories)
        {
            _logger = logger;
            _repositories = repositories;
        }

        [HttpGet, Authorize]
        public IActionResult GetAll()
        {
            return Ok(_repositories.Customer.FindAll()); //TODO: SET PHOTO URL, INCLUDE USERS
        }

        [HttpGet("{id}", Name = "GetCustomerById"), Authorize]
        [ServiceFilter(typeof(EntityExistsActionFilter<Customer>))]
        public IActionResult GetById(Guid id)
        {
            return Ok(HttpContext.Items["entity"]); //TODO: SET PHOTO URL, USERS
        }

        [HttpPost, Authorize]
        [ServiceFilter(typeof(EntityIsValidActionFilter))]
        public IActionResult Post([FromBody] CustomerProfile customerProfile)
        {
            Customer customer = new Customer();
            customer.Map(customerProfile);
            _repositories.Customer.Create(customer); //TODO: SET CREATED_BY
            _repositories.Customer.Save();
            return CreatedAtRoute("GetCustomerById", new {id = customer.Id}, customer);
        }

        [HttpPut("{id}"), Authorize]
        [ServiceFilter(typeof(EntityExistsActionFilter<Customer>))]
        [ServiceFilter(typeof(EntityIsValidActionFilter))]
        public IActionResult Put([FromBody] CustomerProfile customer, Guid id)
        {
            ((Customer) HttpContext.Items["entity"]).Map(customer);
            _repositories.Customer.Update((Customer) HttpContext.Items["entity"]);
            _repositories.Customer.Save();
            return NoContent();
        }

        [HttpDelete("{id}"), Authorize]
        [ServiceFilter(typeof(EntityExistsActionFilter<Customer>))]
        public IActionResult Delete(Guid id)
        {
            _repositories.Customer.Delete((Customer) HttpContext.Items["entity"]);
            _repositories.Customer.Save();
            return NoContent();
        }
    }
}