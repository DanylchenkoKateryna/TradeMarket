using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData
                (
                new Product
                {
                    Id = 1,
                    ProductCategoryId = 1,
                    ProductName = "Apple",
                    Price = 19
                },
                new Product
                {
                    Id = 2,
                    ProductCategoryId = 2,
                    ProductName = "pepper",
                    Price = 98
                },
                new Product
                {
                    Id = 3,
                    ProductCategoryId = 3,
                    ProductName = "Grape juice",
                    Price = 45
                },
                new Product
                {
                    Id = 4,
                    ProductCategoryId = 4,
                    ProductName = "Chicken",
                    Price = 60
                },
                new Product
                {
                    Id = 5,
                    ProductCategoryId = 5,
                    ProductName = "Rice",
                    Price = 90
                }
                );
        }
    }
}
