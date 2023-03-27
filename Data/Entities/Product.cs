using System.Collections.Generic;

namespace Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public int ProductCategoryId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public ProductCategory Category { get; set; }
        public ICollection<ReceiptDetail> ReceiptDetails { get; set; }
    }
}
