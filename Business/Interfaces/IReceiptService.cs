using Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Entities;

namespace Business.Interfaces
{
    public interface IReceiptService : ICrud<ReceiptModel>
    {
        Task AddProductAsync(int productId, int receiptId, int quantity);
        Task AddReceiptDetailAsync(ReceiptDetailModel model);

        Task RemoveProductAsync(int productId, int receiptId, int quantity);

        Task<IEnumerable<ReceiptDetailModel>> GetReceiptDetailsAsync(int receiptId);

        Task<decimal?> ToPayAsync(int receiptId);

        Task CheckOutAsync(int receiptId);

        Task<IEnumerable<ReceiptModel>> GetReceiptsByPeriodAsync(DateTime? startDate, DateTime? endDate);
        ReceiptModel mappToReceiptModel(CreateReceiptModel model);
        Receipt mappToReceipt(CreateReceiptModel model);
        ReceiptDetail mappToReceiptDetail(ReceiptDetailModel model);
        Task<bool> findCustomer(int customerId);
    }
}
