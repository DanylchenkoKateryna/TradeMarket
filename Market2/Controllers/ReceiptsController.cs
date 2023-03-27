using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using LoggerService;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Data.Entities;
using Market2.ErrorModel;
using FluentValidation;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReceiptsController : ControllerBase
    {
        private readonly IReceiptService _receiptService;
        private readonly IProductService _productService;
        private readonly ILoggerManager _logger;
        private readonly IErrorMessageProvider _messageProvider;
        private readonly IValidator<Receipt> _validator;
        private readonly IValidator<ReceiptDetail> _validator2;


        // private readonly IValidator<Product> _validator;
        public ReceiptsController(IReceiptService receiptService, IProductService productService, ILoggerManager logger,IErrorMessageProvider messageProvider,IValidator<Receipt> validator, IValidator<ReceiptDetail> validator2)
        {
            _receiptService = receiptService;
            _productService = productService;
            _logger = logger;
            _messageProvider = messageProvider;
            _validator = validator;
            _validator2 = validator2;
        }
        //GET/api/receipts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReceiptModel>>> Get()
        {
            var allProducts = await _receiptService.GetAllAsync();
            return Ok(allProducts);
        }

        //GET/api/receipts/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ReceiptModel>> GetById(int id)
        {
            var allProductById = await _receiptService.GetByIdAsync(id);
            if (allProductById == null)
            {
                var message = _messageProvider.NotFoundMessage<Receipt>(id);
                _logger.LogInfo(message);
                return NotFound(message);
            }
            else
            {
                return Ok(allProductById);
            }
        }

        //GET/api/receipts/{id}/details
        [HttpGet("{id}/details")]
        public async Task<ActionResult<ReceiptModel>> GetDetailsById(int id)
        {
            var allReceiptsDetailstById = await _receiptService.GetReceiptDetailsAsync(id);
            if (!allReceiptsDetailstById.Any())
            {
                var message = $"Receipt details with id: {id} doesn't exist in the database.";
                _logger.LogInfo(message);
                return NotFound(message);
            }
            else
            {
                return Ok(allReceiptsDetailstById);
            }
        }

        //GET/api/receipts/{id}/sum
        [HttpGet("{id}/sum")]
        public async Task<ActionResult<ReceiptModel>> GetSumById(int id)
        {
            decimal? sumById = await _receiptService.ToPayAsync(id);

            if (sumById == null)
            {
                var message = _messageProvider.NotFoundMessage<Receipt>(id);
                _logger.LogInfo(message);
                return NotFound(message);
            }
            else
            {
                return Ok(sumById);
            }
        }

        //GET/api/receipts/period?startDate=2021-12-1&endDate=2020-12-31
        [HttpGet("period/")]
        public async Task<ActionResult<IEnumerable<ReceiptModel>>> GetByPeriod(DateTime startDate, DateTime endDate)
        {
            var allReceiptsDetailstByPeriod = await _receiptService.GetReceiptsByPeriodAsync(startDate, endDate);
            if (!allReceiptsDetailstByPeriod.Any())
            {
                var message = $"Receipts with period: {startDate} - {endDate} doesn't exist in the database.";
                _logger.LogInfo(message);
                return NotFound(message);
            }
            else
            {
                return Ok(allReceiptsDetailstByPeriod);
            }
        }

        //POST/api/receipts
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] CreateReceiptModel model)
        {
            if(model == null)
            {
                var message = _messageProvider.BadRequestMessage<CreateReceiptModel>();
                _logger.LogInfo(message);
                return BadRequest(message);
            }
            var checkCustomer = await _receiptService.findCustomer(model.CustomerId);
            if (!checkCustomer)
            {
                var message = _messageProvider.NotFoundMessage<Customer>(model.CustomerId);
                _logger.LogInfo(message);
                return NotFound(message);
            }
            var receipt = _receiptService.mappToReceipt(model);
            var result = await _validator.ValidateAsync(receipt);
            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
                return BadRequest(errorMessages);
            }
            var receiptModel = _receiptService.mappToReceiptModel(model);
            await _receiptService.AddAsync(receiptModel);
            return Ok();
        }

        //POST/api/receipts/details
        [HttpPost("details")]
        public async Task<ActionResult> Add([FromBody] ReceiptDetailModel model)
        {
            if (model == null)
            {
                var message = _messageProvider.BadRequestMessage<ReceiptDetailModel>();
                _logger.LogInfo(message);
                return BadRequest(message);
            }
            var receipt =await _receiptService.GetByIdAsync(model.ReceiptId);
            if(receipt== null)
            {
                var message = _messageProvider.NotFoundMessage<ReceiptModel>(model.ReceiptId);
                _logger.LogInfo(message);
                return NotFound(message);
            }
            var product=await _productService.GetByIdAsync(model.ProductId);
            if(product== null)
            {
                var message = _messageProvider.NotFoundMessage<Product>(model.ProductId);
                _logger.LogInfo(message);
                return NotFound(message);
            }
            var receiptDetailModel = _receiptService.mappToReceiptDetail(model);
            var result = await _validator2.ValidateAsync(receiptDetailModel);
            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
                return BadRequest(errorMessages);
            }
            await _receiptService.AddReceiptDetailAsync(model);
            return Ok();
        }

        //PUT/api/receipts/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] CreateReceiptModel model)
        {
            if (model == null)
            {
                var message = _messageProvider.BadRequestMessage<CreateReceiptModel>();
                _logger.LogInfo(message);
                return BadRequest(message);
            }
            var receiptById = await _receiptService.GetByIdAsync(id);
            if (receiptById == null)
            {
                var message = _messageProvider.NotFoundMessage<Receipt>(id);
                _logger.LogInfo(message);
                return NotFound(message);
            }
            var checkCustomer = await _receiptService.findCustomer(model.CustomerId);
            if (!checkCustomer)
            {
                var message = _messageProvider.NotFoundMessage<Customer>(model.CustomerId);
                _logger.LogInfo(message);
                return NotFound(message);
            }
            var receipt = _receiptService.mappToReceipt(model);
            var result = await _validator.ValidateAsync(receipt);
            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
                return BadRequest(errorMessages);
            }
            var receiptModel = _receiptService.mappToReceiptModel(model);
            await _receiptService.UpdateAsync(id,receiptModel);
            return Ok();
        }

        //PUT/api/receipts/{id}/products/add/{productId}/{quantity}
        [HttpPut("{id}/products/add/{productId}/{quantity}")]
        public async Task<ActionResult> Add(int id, int productId, int quantity)
        {
            var receipt = await _receiptService.GetByIdAsync(id);
            if(receipt== null)
            {
                var message = _messageProvider.NotFoundMessage<Receipt>(id);
                _logger.LogInfo(message);
                return NotFound(message);
            }

            if (quantity < 0)
            {
                var message =$"Quantity: {quantity} less then 0";
                _logger.LogInfo(message);
                return NotFound(message);
            }

            var receiptProduct = await _receiptService.GetReceiptDetailsAsync(id);
            var productById = receiptProduct.Where(x => x.ProductId == productId).FirstOrDefault();
            if (productById == null)
            {
                var message = $"Receipt: {id} with product id: {productId} doesn`t exist in database";
                _logger.LogInfo(message);
                return NotFound(message);
            }

            await _receiptService.AddProductAsync(productId, id, quantity);
            return Ok();
        }

        //PUT/api/receipts/{id}/products/remove/{productId}/{quantity}
        [HttpPut("{id}/products/remove/{productId}/{quantity}")]
        public async Task<ActionResult> RemoveFromReceipt(int id, int productId, int quantity)
        {
            var receipt = await _receiptService.GetByIdAsync(id);
            if (receipt == null)
            {
                var message = _messageProvider.NotFoundMessage<Receipt>(id);
                _logger.LogInfo(message);
                return NotFound(message);
            }

            var receiptProduct = await _receiptService.GetReceiptDetailsAsync(id);
            var productById = receiptProduct.Where(x => x.ProductId == productId).FirstOrDefault();
            if(productById == null )
            {
                var message = $"Receipt: {id} with product id: {productId} doesn`t exist in database";
                _logger.LogInfo(message);
                return NotFound(message);
            }

            if(productById.Quantity < quantity || quantity < 0)
            {
                var message = $"Count of product with id:{productId} less than quantity: {quantity} or quantity:{quantity} less than 0";
                _logger.LogInfo(message);
                return NotFound(message);
            }

            await _receiptService.RemoveProductAsync(productId, id, quantity);
            return Ok();
        }

        //PUT/api/receipts/{id}/checkout
        [HttpPut("{id}/checkout")]
        public async Task<ActionResult> CheckOut(int id)
        {
            var receipt =await _receiptService.GetByIdAsync(id);
            if (receipt == null)
            {
                var message = _messageProvider.NotFoundMessage<Receipt>(id);
                _logger.LogInfo(message);
                return NotFound(message);
            }
            await _receiptService.CheckOutAsync(id);
            return Ok();
        }

        //DELETE/api/receipts/{id}
        [HttpDelete("{receiptId}")]
        public async Task<ActionResult> Delete(int receiptId)
        {
            var receipt=await _receiptService.GetByIdAsync(receiptId);
            if (receipt == null)
            {
                var message = _messageProvider.NotFoundMessage<Receipt>(receiptId);
                _logger.LogInfo(message);
                return NotFound(message);
            }
            await _receiptService.DeleteAsync(receiptId);
            return Ok();
        }
    }
}
