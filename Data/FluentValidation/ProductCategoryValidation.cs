using Data.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.FluentValidation
{
    public class ProductCategoryValidation : AbstractValidator<ProductCategory>
    {
        public ProductCategoryValidation() 
        {
            RuleFor(pc => pc.CategoryName).NotEmpty().WithMessage("Category name is required");
        }
    }
}
