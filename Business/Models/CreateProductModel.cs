
namespace Business.Models
{
    public class CreateProductModel
    {
        public int ProductCategoryId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
    }
}
