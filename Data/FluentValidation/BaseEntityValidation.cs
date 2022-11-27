using Data.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.FluentValidation
{
    public class BaseEntityValidation:AbstractValidator<BaseEntity>
    {
        public BaseEntityValidation()
        {
            RuleFor(baseEntity => baseEntity.Id).NotNull().NotEmpty();
        }
    }
}
