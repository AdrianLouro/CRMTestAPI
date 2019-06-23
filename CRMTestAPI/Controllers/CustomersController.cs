using System;
using System.Threading.Tasks;
using ActionFilters;
using CRMTestAPI.Configuration;
using Entities.Extensions;
using Entities.Models;
using Entities.Models.Reduced;
using FileSystemService.Contracts;
using LoggerService.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Repositories.Contracts;
using static System.IO.Directory;
using static System.IO.Path;
using static System.Security.Claims.ClaimTypes;
using static FileSystemService.MimeTypesHelper;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace CRMTestAPI.Controllers
{
    [Route("customers")]
    [Authorize(Policy = "NotDeleted")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryWrapper _repositories;
        private IImageWriter _imageWriter;
        private IOptions<DirectoryConfig> _config;

        public CustomersController(ILoggerManager logger, IRepositoryWrapper repositories, IImageWriter imageWriter,
            IOptions<DirectoryConfig> config)
        {
            _logger = logger;
            _repositories = repositories;
            _imageWriter = imageWriter;
            _config = config;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_repositories.Customer.FindAll());
        }

        [HttpGet("{id}", Name = "GetCustomerById")]
        [ServiceFilter(typeof(EntityExistsActionFilter<Customer>))]
        public IActionResult GetById(Guid id)
        {
            Customer customer = (Customer) HttpContext.Items["entity"];
            customer.PhotoPath = Url.Action("GetPhoto", "Customers", new {id = customer.Id});
            return Ok(customer);
        }

        [HttpPost]
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

        [HttpPut("{id}")]
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

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(EntityExistsActionFilter<Customer>))]
        public IActionResult Delete(Guid id)
        {
            _repositories.Customer.Delete((Customer) HttpContext.Items["entity"]);
            _repositories.Customer.Save();
            return NoContent();
        }

        [HttpPost("{id}/photos")]
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

        [HttpGet("{id}/photos")]
        [ServiceFilter(typeof(EntityExistsActionFilter<Customer>))]
        public IActionResult GetPhoto(Guid id)
        {
            var file = Combine(GetCurrentDirectory(), _config.Value.Uploads,
                ((Customer) HttpContext.Items["entity"]).PhotoName);

            if (((Customer) HttpContext.Items["entity"]).PhotoName == null
                || !System.IO.File.Exists(file))
            {
                return NotFound("Could not find a photo for the given customer.");
            }

            return PhysicalFile(file, MimeTypes[GetExtension(file)]);
        }
    }
}