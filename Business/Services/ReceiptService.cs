using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ReceiptService : IReceiptService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public ReceiptService(IUnitOfWork _unitOfWork, IMapper _mapper)
        {
            this._unitOfWork = _unitOfWork;
            this._mapper = _mapper;
        }

        public async Task<decimal?> ToPayAsync(int receiptId)
        {
            var receipt = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
            if(receipt == null) { return null; }
            var obj = receipt.ReceiptDetails.Sum(x => x.Quantity * x.DiscountUnitPrice);
            return _mapper.Map<decimal>(obj);
        }

        public async Task CheckOutAsync(int receiptId)
        {
            var check = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
            check.IsCheckedOut = true;
            _unitOfWork.ReceiptRepository.Update(check);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<ReceiptDetailModel>> GetReceiptDetailsAsync(int receiptId)
        {
            var receipt = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
            if (receipt is null)
            {
                return Enumerable.Empty<ReceiptDetailModel>();
            }
            var ReceiptDetails = receipt.ReceiptDetails.Where(x => x.ReceiptId == receiptId);
            return _mapper.Map<IEnumerable<ReceiptDetailModel>>(ReceiptDetails);
        }

        public async Task<IEnumerable<ReceiptModel>> GetReceiptsByPeriodAsync(DateTime? startDate, DateTime? endDate)
        {
            var getAllReceipts = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
            var receiptsByDate = new List<Receipt>();
            if (startDate == DateTime.MinValue && endDate != DateTime.MinValue)
            {
                receiptsByDate = getAllReceipts.Where(x=>x.OperationDate <= endDate).ToList();

            }
            else if(startDate != DateTime.MinValue && endDate == DateTime.MinValue)
            {
                receiptsByDate = getAllReceipts.Where(x => x.OperationDate >= startDate).ToList();

            }
            else if(endDate != DateTime.MinValue && startDate != DateTime.MinValue)
            {
                receiptsByDate = getAllReceipts.Where(x => x.OperationDate >= startDate && x.OperationDate <= endDate).ToList();
            }
            else
            {
                receiptsByDate=getAllReceipts.ToList();
            }
            return _mapper.Map<IEnumerable<ReceiptModel>>(receiptsByDate);
        }

        public async Task<IEnumerable<ReceiptModel>> GetAllAsync()
        {
            var obj = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<ReceiptModel>>(obj);
        }
        public async Task AddProductAsync(int productId, int receiptId, int quantity)
        {
            var receipt = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
            if (receipt != null && receipt.ReceiptDetails.Select(x => x.ProductId).Contains(productId))
            {
                var rec = receipt.ReceiptDetails.Where(x => x.ProductId == productId).FirstOrDefault();
                rec.Quantity += quantity;
                _unitOfWork.ReceiptDetails.Update(rec);
            }
            await _unitOfWork.SaveAsync();
        }
        public async Task RemoveProductAsync(int productId, int receiptId, int quantity)
        {
            var receipt = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
            var obj = receipt.ReceiptDetails.Where(x => x.ProductId == productId).FirstOrDefault();
            if (obj.Quantity == quantity)
            {
                _unitOfWork.ReceiptDetails.Delete(obj);
            }
            else if(obj.Quantity > quantity)
            {
                obj.Quantity -= quantity;
                _unitOfWork.ReceiptDetails.Update(obj);
            }
            await _unitOfWork.SaveAsync();
        }
        public async Task<ReceiptModel> GetByIdAsync(int id)
        {
            var obj = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(id);
            return _mapper.Map<ReceiptModel>(obj);
        }
        public async Task AddReceiptDetailAsync(ReceiptDetailModel model)
        {
            _unitOfWork.ReceiptDetails.Add(_mapper.Map<ReceiptDetail>(model));
            await _unitOfWork.SaveAsync();
        }
        
        public async Task AddAsync(ReceiptModel model)
        { 
            _unitOfWork.ReceiptRepository.Add(_mapper.Map<Receipt>(model));
            await _unitOfWork.SaveAsync();
        }
        public async Task UpdateAsync(int id, ReceiptModel model)
        { 
            var receipt = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(id);
            var customer=await _unitOfWork.CustomerRepository.GetByIdWithDetailsAsync(model.CustomerId);
            receipt.OperationDate=model.OperationDate;
            receipt.IsCheckedOut=model.IsCheckedOut;
            receipt.CustomerId = model.CustomerId;
            receipt.Customer = customer;
            _unitOfWork.ReceiptRepository.Update(receipt);
        }
        public async Task DeleteAsync(int modelId)
        {
            var toBeDeleted = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(modelId);

            foreach (var item in toBeDeleted.ReceiptDetails)
            {
                _unitOfWork.ReceiptDetails.Delete(item);
            }
            _unitOfWork.ReceiptRepository.Delete(toBeDeleted);
            await _unitOfWork.SaveAsync();
        }
        public ReceiptModel mappToReceiptModel(CreateReceiptModel model)
        {
            return _mapper.Map<ReceiptModel>(model);
        }
        public Receipt mappToReceipt(CreateReceiptModel model)
        {
            var receiptModel = mappToReceiptModel(model);
            return _mapper.Map<Receipt>(receiptModel);
        }
        public ReceiptDetail mappToReceiptDetail(ReceiptDetailModel model)
        {
            return _mapper.Map<ReceiptDetail>(model);
        }
        public async Task<bool> findCustomer(int customerId)
        {
            var customer=await _unitOfWork.CustomerRepository.GetByIdWithDetailsAsync(customerId); 
            return customer != null;
        }
    }
}
