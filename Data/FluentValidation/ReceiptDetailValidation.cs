using Data.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.FluentValidation
{
    public class ReceiptDetailValidation : AbstractValidator<ReceiptDetail>
    {
        public ReceiptDetailValidation()
        {
            RuleFor(receiptD => receiptD.Quantity).ExclusiveBetween(0,999);
        }
    }
}
