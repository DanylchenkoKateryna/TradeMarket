using Data.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.FluentValidation
{
    public class ReceiptValidation : AbstractValidator<Receipt>
    {
        public ReceiptValidation()
        {
            RuleFor(receipt => receipt.IsCheckedOut).NotNull().NotEmpty();
        }
    }
}
