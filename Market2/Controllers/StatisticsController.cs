using Business.Interfaces;
using Business.Models;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradeMarketProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController:ControllerBase
    {
        private readonly IStatisticService _statisticService;
        private readonly IProductService _productService;
        private readonly ILoggerManager _logger;
        public StatisticsController(IStatisticService statisticService, ILoggerManager logger, IProductService productService)
        {
            _statisticService = statisticService;
            _logger = logger;
            _productService = productService;
        }

        //GET/api/statistic/MostPopularProducts?productCount=2
        [HttpGet("mostPopularProducts/{productCount}")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> MostPopularProducts(int productCount)
        {
            var allProducts = await _statisticService.GetMostPopularProductsAsync(productCount);

            if(!allProducts.Any()) 
            {
                var message = $"Most popular product with count {productCount} doesn`t exist in the database";
                _logger.LogError(message);
                return NotFound(message);
            }
            return Ok(allProducts);
        }

        //GET/api/statisic/customer/{id}/{productCount}
        [HttpGet("customer/{id}/{productCount}")]
        public async Task<ActionResult<IEnumerable<ReceiptModel>>> GetFavoriteCustomerProduct(int id,int productCount)
        {
            if (productCount < 0)
            {
                var message = $"Product count: {productCount} less than 0";
                _logger.LogError(message);
                return NotFound(message);
            }
            var product = await _statisticService.GetCustomerMostPopularProductsAsync(productCount,id);
            
            if(!product.Any())
            {
                var message = $"Customer with id: {id} doesn't exist in database"; ;
                _logger.LogError(message);
                return NotFound(message);
            }
            return Ok(product);
        }

        //GET/api/statistic/activity/{customerCount}?startDate= 2020-7-21&endDate= 2020-7-22
        [HttpGet("activity/{customerCount}")]
        public async Task<ActionResult<IEnumerable<ReceiptModel>>> GetMostActiveCustomers(int customerCount,[FromQuery]DateTime startDate,[FromQuery]DateTime endDate)
        {
            var customer = await _statisticService.GetMostValuableCustomersAsync(customerCount,startDate,endDate);

            if(!customer.Any())
            {
                var message = $"Most active customers with count {customerCount} doesn`t exist in the database";
                _logger.LogError(message);
                return NotFound(message);
            }
            return Ok(customer);
        }

        //GET/api/statistic/income/{categoryId}?startDate= 2020-7-21&endDate= 2020-7-22
        [HttpGet("income/{categoryId}")]
        public async Task<ActionResult<IEnumerable<CustomerActivityModel>>> GetIncome(int categoryId, [FromQuery] DateTime startDate, DateTime endDate)
        {
            var category = await _productService.GetProductCategoriesByIdAsync(categoryId);
            if (category is null)
            {
                var message = $"Product category with id {categoryId} doesn`t exist in the database";
                _logger.LogError(message);
                return NotFound(message);
            }
            var income = await _statisticService.GetIncomeOfCategoryInPeriod(categoryId, startDate, endDate);
            return Ok(income);
        }
    }
}
