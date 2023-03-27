using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration
{
    public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.HasData
                (
                new ProductCategory
                {
                    Id = 1,
                    CategoryName = "Fruits"
                },
                new ProductCategory
                {
                    Id = 2,
                    CategoryName = "Vegetables"
                },
                new ProductCategory
                {
                    Id = 3,
                    CategoryName = "Drinks"
                },
                new ProductCategory
                {
                    Id = 4,
                    CategoryName = "Meat"
                },
                new ProductCategory
                {
                    Id = 5,
                    CategoryName = "Grocery"
                }
                );
        }
    }
}
