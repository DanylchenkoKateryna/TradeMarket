using System;

namespace Business.Models
{
    public class CreateReceiptModel
    {
        public int CustomerId { get; set; }
        public DateTime OperationDate { get; set; }
        public bool IsCheckedOut { get; set; }
    }
}
