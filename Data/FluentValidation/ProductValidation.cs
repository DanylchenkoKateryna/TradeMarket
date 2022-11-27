using Data.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.FluentValidation
{
    public class ProductValidation : AbstractValidator<Product>
    {
        public ProductValidation()
        {
            RuleFor(product=>product.ProductName).NotEmpty().NotNull().Length(1, 55);
            RuleFor(product => product.Price).NotEmpty().NotNull();
        }
    }
}
