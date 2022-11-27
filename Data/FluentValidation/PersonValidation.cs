using Data.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.FluentValidation
{
    public class PersonValidation : AbstractValidator<Person> 
    {
        public PersonValidation()
        {
            RuleFor(person => person.Name).NotNull().NotEmpty().WithMessage("Required");
            RuleFor(person => person.Name).NotNull().Length(1, 55);
            RuleFor(person => person.Surname).NotNull().NotEmpty().WithMessage("Required");
            RuleFor(person => person.Surname).NotNull().Length(1, 55);
            RuleFor(person => person.BirthDate).Must(BeAValidAge).WithMessage("Invalid {BirthDate}");
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
