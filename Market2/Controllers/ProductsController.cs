using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Market2.ErrorModel;
using Data.Entities;
using FluentValidation;

namespace TradeMarketProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILoggerManager _logger;
        private readonly IErrorMessageProvider _messageProvider;
        private readonly IValidator<Product> _validator;
        private readonly IValidator<ProductCategory> _validator2;

        public ProductsController(IProductService productService, ILoggerManager logger, IErrorMessageProvider messageProvider,IValidator<Product> validator, IValidator<ProductCategory> validator2)
        {
            _productService = productService;
            _logger = logger;
            _messageProvider = messageProvider;
            _validator = validator;
            _validator2 = validator2;
        }

        //GET/api/products/getAll
        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> Get()
        {
            var allProducts = await _productService.GetAllAsync();
            return Ok(allProducts);
        }

        //GET/api/products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductModel>> GetById(int id)
        {
            var allProductById = await _productService.GetByIdAsync(id);
            if (allProductById == null)
            {
                var message = _messageProvider.NotFoundMessage<Product>(id);
                _logger.LogInfo(message);
                return NotFound(message);
            }
            else
            {
                return Ok(allProductById);
            }
        }

        //GET/api/products/categories
        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetCategories()
        {
            var allCategories = await _productService.GetAllProductCategoriesAsync();
            return Ok(allCategories);
        }

        //GET/api/products/categories/{categoryId}
        [HttpGet("categories/{categoryId}")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetCategoriesById(int categoryId)
        {
            var allCategories = await _productService.GetProductCategoriesByIdAsync(categoryId);
            if(allCategories == null)
            {
                var message = $"Product category with id: {categoryId} doesn't exist in the database.";
                _logger.LogInfo(message);
                return NotFound(message);
            }
            else
            {
                return Ok(allCategories);
            }
        }

        //GET/api/products?categoryId=1&minPrice=15&maxPrice=50
        [HttpGet("getByFilter")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetByFilter(int? categoryId, int? minPrice, int? maxPrice)
        {
            FilterSearchModel newFilter = new FilterSearchModel()
            {
                CategoryId = categoryId,
                MinPrice = minPrice,
                MaxPrice = maxPrice
            };
            var productsByFilter = await _productService.GetByFilterAsync(newFilter);

            if (productsByFilter == null || !productsByFilter.Any())
            {
                return NotFound();
            }
            return Ok(productsByFilter);
        }


        //POST/api/products
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] CreateProductModel model)
        {
            var category = await _productService.GetProductCategoriesByIdAsync(model.ProductCategoryId);
            if (category == null)
            {
                var message = _messageProvider.NotFoundMessage<ProductCategory>(model.ProductCategoryId);
                _logger.LogInfo(message);
                return NotFound(message);
            }
            var product = _productService.mappToProduct(model);
            var result = await _validator.ValidateAsync(product);
            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
                return BadRequest(errorMessages);
            }
            var productModel = _productService.mappToProductModel(model);
            await _productService.AddAsync(productModel);
            return Ok();
        }

        //POST/api/products/categories
        [HttpPost("categories")]
        public async Task<ActionResult> AddCategory([FromBody] CreateProductCategoryModel categoryModel)
        {
            var product = _productService.mappToProductCategory(categoryModel);
            var result = await _validator2.ValidateAsync(product);
            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
                return BadRequest(errorMessages);
            }
            var category = _productService.mappToProductCategoryModel(categoryModel);
            await _productService.AddCategoryAsync(category);
            return Ok();
        }


        //PUT/api/products/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] CreateProductModel model)
        {
            var productById = await _productService.GetByIdAsync(id);
            if (productById == null)
            {
                var message = _messageProvider.NotFoundMessage<Product>(id);
                _logger.LogInfo(message);
                return NotFound(message);
            }
            var category =await _productService.GetProductCategoriesByIdAsync(model.ProductCategoryId);
            if (category == null)
            {
                var message = _messageProvider.NotFoundMessage<ProductCategory>(model.ProductCategoryId);
                _logger.LogInfo(message);
                return NotFound(message);
            }
            var product = _productService.mappToProduct(model);
            var result = await _validator.ValidateAsync(product);
            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
                return BadRequest(errorMessages);
            }
            var productModel = _productService.mappToProductModel(model);
            await _productService.UpdateAsync(id, productModel);
            return Ok();
        }

        //PUT/api/products/categories/{categoryId}
        [HttpPut("categories/{categoryId}")]
        public async Task<ActionResult> Update(int categoryId, [FromBody] CreateProductCategoryModel categoryModel)
        {
            var product = _productService.mappToProductCategory(categoryModel);
            var result = await _validator2.ValidateAsync(product);
            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
                return BadRequest(errorMessages);
            }
            var productById = await _productService.GetProductCategoriesByIdAsync(categoryId);
            if (productById == null)
            {
                var message = $"Product category with id: {categoryId} doesn't exist in the database.";
                _logger.LogInfo(message);
                return NotFound(message);
            }
            var category = _productService.mappToProductCategoryModel(categoryModel);
            await _productService.UpdateCategoryAsync(categoryId, category);
            return Ok();
        }

        //DELETE/api/products/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> Delete(int id)
        {
            var productById = await _productService.GetByIdAsync(id);
            if (productById == null)
            {
                var message = _messageProvider.NotFoundMessage<Product>(id);
                _logger.LogInfo(message);
                return NotFound(message);
            }
            await _productService.DeleteAsync(id);
            return Ok();
        }

        //DELETE: api/products/categories/{categoryId}
        [HttpDelete("categories/{categoryId}")]
        public async Task<ActionResult> DeleteCategory(int categoryId)
        {
            var productById = await _productService.GetProductCategoriesByIdAsync(categoryId);
            if (productById == null)
            {
                var message = $"Product category with id: {categoryId} doesn't exist in the database.";
                _logger.LogInfo(message);
                return NotFound(message);
            }
            await _productService.RemoveCategoryAsync(categoryId);
            return Ok();
        }
    }
}
