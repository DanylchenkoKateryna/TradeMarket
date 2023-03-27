using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class StatisticService : IStatisticService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public StatisticService(IUnitOfWork _unitOfWork, IMapper _mapper)
        {
            this._unitOfWork = _unitOfWork;
            this._mapper = _mapper;
        }

        public async Task<IEnumerable<ProductModel>> GetCustomerMostPopularProductsAsync(int productCount, int customerId)
        {
            var receipts = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
            var customerReceipt = receipts.Where(x => x.CustomerId == customerId);
            var details = customerReceipt.SelectMany(x => x.ReceiptDetails)
                .OrderByDescending(x => x.Quantity)
                .Select(x => x.Product).Take(productCount);

            return _mapper.Map<IEnumerable<ProductModel>>(details);
        }

        public async Task<decimal> GetIncomeOfCategoryInPeriod(int categoryId, DateTime startDate, DateTime endDate)
        {
            var getAllReceipts = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
            var receiptsByDate =new List<Receipt>();
            if (endDate == DateTime.MinValue)
            {
                receiptsByDate = getAllReceipts.Where(x => x.OperationDate >= startDate).ToList();
            }
            else if(endDate == DateTime.MinValue && startDate == DateTime.MinValue)
            {
                receiptsByDate = getAllReceipts.ToList();
            }
            else
            {
                receiptsByDate = getAllReceipts.Where(x => x.OperationDate >= startDate && x.OperationDate <= endDate).ToList();
            }
            var filteredByCategory = receiptsByDate.SelectMany(r => r.ReceiptDetails).Where(rd => rd.Product.ProductCategoryId == categoryId);
            return filteredByCategory.Select(rd => rd.DiscountUnitPrice * rd.Quantity).Sum();
        }

        public async Task<IEnumerable<ProductModel>> GetMostPopularProductsAsync(int productCount)
        {
            var allReceipts = await _unitOfWork.ReceiptDetails.GetAllWithDetailsAsync();
            var details = allReceipts.OrderByDescending(x => x.Quantity).Select(x=>x.Product).Take(productCount);
            return _mapper.Map<IEnumerable<ProductModel>>(details);
        }

        public async Task<IEnumerable<CustomerActivityModel>> GetMostValuableCustomersAsync(int customerCount, DateTime startDate, DateTime endDate)
        {
            var allReceipts = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
            var receiptsByDate = new List<Receipt>();
            if (endDate == DateTime.MinValue)
            {
                receiptsByDate = allReceipts.Where(x => x.OperationDate >= startDate).ToList();
            }
            else if (endDate == DateTime.MinValue && startDate == DateTime.MinValue)
            {
                receiptsByDate = allReceipts.ToList();
            }
            else
            {
                receiptsByDate = allReceipts.Where(x => x.OperationDate >= startDate && x.OperationDate <= endDate).ToList();
            }

            var customersOrderedByTotalSpend = receiptsByDate
                .GroupBy(r => r.CustomerId)
                .Select(g => new CustomerActivityModel
                {
                    CustomerId = g.Key,
                    CustomerName = g.Select(r => r.Customer.Person.Name + " " + r.Customer.Person.Surname)
                    .First(),
                    ReceiptSum = g.SelectMany(r => r.ReceiptDetails).Select(rd => rd.Quantity * rd.DiscountUnitPrice).Sum()
                })
                .OrderByDescending(c => c.ReceiptSum)
                .Take(customerCount)
                .ToList();
            return customersOrderedByTotalSpend;
        }
    }
}
