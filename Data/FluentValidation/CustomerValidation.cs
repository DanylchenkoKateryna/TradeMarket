using Data.Entities;
using FluentValidation;
using System;

namespace Data.FluentValidation
{
    public class CustomerValidation : AbstractValidator<Customer>
    {
        public CustomerValidation()
        {
            RuleFor(customer=>customer.Person.Name).NotEmpty().Length(1, 55).WithMessage($"Name is required");
            RuleFor(customer => customer.Person.Surname).NotEmpty().Length(1, 55).WithMessage($"Surname is required");
            RuleFor(customer => customer.Person.BirthDate).Must(BeAValidAge).WithMessage("Invalid {BirthDate}");
            RuleFor(customer => customer.DiscountValue).ExclusiveBetween(-1, 999);
        }
        protected bool BeAValidAge(DateTime date)
        {
            int currentYear = DateTime.Now.Year;
            int dobYear = date.Year;

            if (dobYear <= currentYear && dobYear > (currentYear - 120))
            {
                return true;
            }
            return false;
        }
    }
}
