using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasData
            (
                new Customer
                {
                    Id = 1,
                    PersonId = 1,
                    DiscountValue = 15
                }, new Customer
                {
                    Id = 2,
                    PersonId = 2,
                    DiscountValue = 20
                }, new Customer
                {
                    Id = 3,
                    PersonId = 3,
                    DiscountValue = 16
                }
            );
        }
    }
}
