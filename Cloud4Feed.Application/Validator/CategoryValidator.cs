using Cloud4Feed.Model.Entity;
using Cloud4Feed.Model.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud4Feed.Application.Validator
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Ad bilgisi giriniz");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama bilgisi giriniz");
         
        }
    }
}
