using Data.Entities;
using FluentValidation;

namespace Data.FluentValidation
{
    public class ReceiptDetailValidation : AbstractValidator<ReceiptDetail>
    {
        public ReceiptDetailValidation()
        {
            RuleFor(receiptD => receiptD.Quantity).Must(BeAValidQuantity).WithMessage("Quantity invalid");
            RuleFor(receiptD => receiptD.UnitPrice).NotEmpty().NotNull();
            RuleFor(receiptD => receiptD.DiscountUnitPrice).ExclusiveBetween(0, 100);
        }
        protected bool BeAValidQuantity(int quantity)
        {
            return quantity >= 0 ? true : false;
        }
    }
}
