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
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Ad bilgisi giriniz");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama bilgisi giriniz");
            RuleFor(x => x.Barcode).NotEmpty().WithMessage("Barkod bilgisi giriniz");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Fiyat bilgisi giriniz");
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Kategori seçiniz");
        }
    }
}
