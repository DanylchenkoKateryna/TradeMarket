using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration
{
    public class ReceiptDetailConfigurationn : IEntityTypeConfiguration<ReceiptDetail>
    {
        public void Configure(EntityTypeBuilder<ReceiptDetail> builder)
        {
            builder.HasData
                (
                new ReceiptDetail
                {
                    Id = 1,
                    ReceiptId = 1,
                    ProductId = 1,
                    UnitPrice = 19,
                    DiscountUnitPrice = 15,
                    Quantity = 1
                },
                new ReceiptDetail
                {
                    Id = 2,
                    ReceiptId = 1,
                    ProductId = 2,
                    UnitPrice = 98,
                    DiscountUnitPrice = 90,
                    Quantity = 2
                },
                new ReceiptDetail
                {
                    Id = 3,
                    ReceiptId = 2,
                    ProductId = 5,
                    UnitPrice = 90,
                    DiscountUnitPrice = 85,
                    Quantity = 7
                },
                new ReceiptDetail
                {
                    Id = 4,
                    ProductId = 4,
                    ReceiptId = 3,
                    UnitPrice = 60,
                    DiscountUnitPrice = 60,
                    Quantity = 2
                },
                new ReceiptDetail
                {
                    Id = 5,
                    ProductId = 2,
                    ReceiptId = 3,
                    UnitPrice = 45,
                    DiscountUnitPrice = 35,
                    Quantity = 1
                }
                );
        }
    }
}
