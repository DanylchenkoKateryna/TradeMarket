using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Data.Configuration
{
    public class ReceiptConfiguration : IEntityTypeConfiguration<Receipt>
    {
        public void Configure(EntityTypeBuilder<Receipt> builder)
        {
            builder.HasData
                (
                new Receipt
                {
                    Id = 1,
                    CustomerId = 1,
                    OperationDate = new DateTime(2023, 2, 9, 15, 0, 0),
                    IsCheckedOut = true
                },
                 new Receipt
                 {
                     Id = 2,
                     CustomerId = 2,
                     OperationDate = new DateTime(2023, 2, 9, 15, 40, 0),
                     IsCheckedOut = false
                 },
                  new Receipt
                  {
                      Id = 3,
                      CustomerId = 3,
                      OperationDate = new DateTime(2023, 2, 9, 16, 8, 45),
                      IsCheckedOut = true
                  }
                );
        }
    }
}
