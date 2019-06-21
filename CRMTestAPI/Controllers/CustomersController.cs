﻿using System;
using System.Threading.Tasks;
using ActionFilters;
using Entities.Extensions;
using Entities.Models;
using Entities.Models.Reduced;
using FileSystemService.Contracts;
using LoggerService.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;
using static System.IO.Directory;
using static System.IO.Path;
using static System.Security.Claims.ClaimTypes;
using static FileSystemService.MimeTypesHelper;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace CRMTestAPI.Controllers
{
    [Route("customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryWrapper _repositories;
        private IImageWriter _imageWriter;

        public CustomersController(ILoggerManager logger, IRepositoryWrapper repositories, IImageWriter imageWriter)
        {
            _logger = logger;
            _repositories = repositories;
            _imageWriter = imageWriter;
        }

        [HttpGet, Authorize]
        public IActionResult GetAll()
        {
            return Ok(_repositories.Customer.FindAll());
        }

        [HttpGet("{id}", Name = "GetCustomerById"), Authorize]
        [ServiceFilter(typeof(EntityExistsActionFilter<Customer>))]
        public IActionResult GetById(Guid id)
        {
            return Ok(HttpContext.Items["entity"]);
        }

        [HttpPost, Authorize]
        [ServiceFilter(typeof(EntityIsValidActionFilter))]
        public IActionResult Post([FromBody] CustomerProfile customerProfile)
        {
            Customer customer = new Customer();
            customer.Map(customerProfile);
            customer.CreatedById = Guid.Parse(User.FindFirst(NameIdentifier).Value);
            _repositories.Customer.Create(customer);
            _repositories.Customer.Save();
            return CreatedAtRoute("GetCustomerById", new {id = customer.Id}, customer);
        }

        [HttpPut("{id}"), Authorize]
        [ServiceFilter(typeof(EntityExistsActionFilter<Customer>))]
        [ServiceFilter(typeof(EntityIsValidActionFilter))]
        public IActionResult Put([FromBody] CustomerProfile customer, Guid id)
        {
            Customer dbCustomer = ((Customer) HttpContext.Items["entity"]);
            dbCustomer.Map(customer);
            dbCustomer.LastUpdatedById = Guid.Parse(User.FindFirst(NameIdentifier).Value);
            _repositories.Customer.Update(dbCustomer);
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

        [HttpPost("{id}/photos"), Authorize]
        [ServiceFilter(typeof(EntityExistsActionFilter<Customer>))]
        public async Task<IActionResult> UploadPhoto(Guid id, [FromForm] IFormFile file)
        {
            var uploadedImageName = await _imageWriter.Write(file);
            if (uploadedImageName == null)
            {
                return StatusCode(Status500InternalServerError);
            }

            ((Customer) HttpContext.Items["entity"]).PhotoName = uploadedImageName;
            _repositories.Customer.Update((Customer) HttpContext.Items["entity"]);
            _repositories.Customer.Save();
            return NoContent();
        }

        [HttpGet("{id}/photo"), Authorize]
        [ServiceFilter(typeof(EntityExistsActionFilter<Customer>))]
        public IActionResult GetPhoto(Guid id)
        {
            if (((Customer) HttpContext.Items["entity"]).PhotoName == null)
            {
                return NotFound("Could not find a photo for the given customer.");
            }

            var file = Combine(GetCurrentDirectory(), "wwwroot", "uploads", "customer_photos",
                ((Customer) HttpContext.Items["entity"]).PhotoName);
            return PhysicalFile(file, MimeTypes[GetExtension(file)]);
        }
    }
}