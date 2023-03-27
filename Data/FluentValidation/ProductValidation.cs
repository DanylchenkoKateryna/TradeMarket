using Data.Entities;
using FluentValidation;
using System;

namespace Data.FluentValidation
{
    public class ProductValidation : AbstractValidator<Product>
    {
        public ProductValidation()
        {
            RuleFor(product => product.ProductName).NotEmpty().Length(1, 55).WithMessage($"Product name is required");
            RuleFor(product => product.Price).Must(BeAValidPrice).WithMessage("Invalid price");
        }
        protected bool BeAValidPrice(decimal price)
        {
            return price > 0 ? true : false;
        }
    }
}
