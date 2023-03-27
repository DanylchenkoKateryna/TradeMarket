using Data.Entities;
using FluentValidation;
using System;

namespace Data.FluentValidation
{
    public class ReceiptValidation : AbstractValidator<Receipt>
    {
        public ReceiptValidation()
        {
            RuleFor(receipt => receipt.IsCheckedOut).Must(isCheckedOut).NotEmpty().WithMessage("Enter true or false");
            RuleFor(receipt => receipt.OperationDate).Must(BeAValidDate).WithMessage("Invalid operation date");
        }
        protected bool BeAValidDate(DateTime date)
        {
            int currentYear = DateTime.Now.Year;
            var date2 = date.Year;
            return date2 <= currentYear;
        }
        protected bool isCheckedOut(bool check)
        {
            return check == true || check == false ? true : false; 
        }
    }
}
