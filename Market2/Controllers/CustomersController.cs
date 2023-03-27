using System.Linq;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using FluentValidation;
using LoggerService;
using Market2.ErrorModel;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILoggerManager _logger;
        private readonly IErrorMessageProvider _messageProvider;
        private readonly IValidator<Customer> _validator;

        public CustomersController(ICustomerService customerService, ILoggerManager logger,IErrorMessageProvider messageProvider, IValidator<Customer> validator)
        {
            _customerService = customerService;
            _logger = logger;
            _messageProvider = messageProvider;
            _validator = validator;
        }
        // GET: api/customers
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var allCustomers = await _customerService.GetAllAsync();
            return Ok(allCustomers);
        }

        //GET: api/customers/1
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerModel>> GetById(int id)
        {
            var allCustomersById = await _customerService.GetByIdAsync(id);
            if (allCustomersById == null)
            {
                var message = _messageProvider.NotFoundMessage<Customer>(id);
                _logger.LogInfo(message);
                return NotFound(message);
            }
            else
            {
                return Ok(allCustomersById);
            }
        }

        //GET: api/customers/products/1
        [HttpGet("products/{id}")]
        public async Task<ActionResult<CustomerModel>> GetByProductId(int id)
        {
            var customerByProduct = await _customerService.GetCustomersByProductIdAsync(id);
            if (!customerByProduct.Any())
            {
                var message = $"Customer with product id: {id} doesn't exist in the database.";
                _logger.LogInfo(message);
                return NotFound(message);
            }
            else
            {
                return Ok(customerByProduct);
            }
        }

        // POST: api/customers
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] CreateCustomerModel value)
        {
            if (value == null)
            {
                var message = _messageProvider.BadRequestMessage<CreateCustomerModel>();
                _logger.LogInfo(message);
                return NotFound(message);
            }
            var customer=_customerService.mappToCustomer(value);
            var result = await _validator.ValidateAsync(customer);
            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
                return BadRequest(errorMessages);
            }
            var customerModel = _customerService.mappToCustomerModel(value);
            await _customerService.AddAsync(customerModel);
            return Ok();
        }

        // PUT: api/customers/1
        [HttpPut("{Id}")]
        public async Task<ActionResult> Update(int Id, [FromBody] CreateCustomerModel value)
        {
            var customer = _customerService.mappToCustomer(value);
            var result = await _validator.ValidateAsync(customer);
            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
                return BadRequest(errorMessages);
            }
            var CustomerById =await _customerService.GetByIdAsync(Id);
            if (CustomerById == null)
            {
                var message = _messageProvider.NotFoundMessage<Customer>(Id);
                _logger.LogInfo(message);
                return NotFound(message);
            }
            var customerModel = _customerService.mappToCustomerModel(value);
            await _customerService.UpdateAsync(Id, customerModel);
            return Ok();
        }

        // DELETE: api/customers/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var customerById = await _customerService.GetByIdAsync(id);
            if (customerById == null)
            {
                var message = _messageProvider.NotFoundMessage<Customer>(id);
                _logger.LogInfo(message);
                return NotFound(message);
            }
            await _customerService.DeleteAsync(id);
            return Ok();
        }
    }
}
