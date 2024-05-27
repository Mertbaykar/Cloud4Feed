using Cloud4Feed.Application.Repository.Contract;
using Cloud4Feed.Infrastructure.DBContext;
using Cloud4Feed.Model.Entity;
using Cloud4Feed.Model.Request.Product;
using Cloud4Feed.Model.Response.Product;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud4Feed.Application.Repository
{
    public class ProductRepository : RepositoryBase, IProductRepository
    {
        readonly IValidator<Product> validator;

        public ProductRepository(ECommerceContext eCommerceContext, IValidator<Product> validator) : base(eCommerceContext)
        {
            this.validator = validator;
        }

        private async Task ValidateByDB(Product product)
        {
            if (!await MasterDataDb.Category.AnyAsync(c => c.Id == product.CategoryId))
                throw new Exception("Kategori bulunamadı");

            if (await MasterDataDb.Product.AnyAsync(p => p.Name == product.Name || p.Barcode == product.Barcode))
                throw new Exception("Bu ürün önceden tanımlanmış");
        }

        public async Task<Product> Create(CreateProductRequest createProductRequest)
        {
            Product product = Product.Create(createProductRequest);
            await base.Validate(product, validator);
            await ValidateByDB(product);
            return await base.Add(product);
        }

        public async Task Delete(Guid id)
        {
            await base.Delete<Product>(id);
        }

        public async Task<Product> Update(UpdateProductRequest updateProductRequest)
        {
            Product? product = await base.Get<Product>(updateProductRequest.Id);
            if (product == null)
                throw new Exception("Ürün bulunamadı");

            product.Update(updateProductRequest);
            await base.Validate(product, validator);
            await ValidateByDB(product);
            await Save();
            return product;
        }

        public IEnumerable<ProductResponse> GetAll()
        {
            var products = MasterDataDb.Product.AsNoTracking()
                .Select(p => new ProductResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Barcode = p.Barcode,
                    Description = p.Description,
                    Price = p.Price,
                    Category = new ProductCategoryResponse
                    {
                        Id = p.Category.Id,
                        Name = p.Category.Name,
                        Description = p.Category.Description,
                    }
                }).ToList();
            return products;
        }

        public ProductResponse? Get(Guid id)
        {
          var product =  MasterDataDb.Product.AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new ProductResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Barcode = p.Barcode,
                    Description = p.Description,
                    Price = p.Price,
                    Category = new ProductCategoryResponse
                    {
                        Id = p.Category.Id,
                        Name = p.Category.Name,
                        Description = p.Category.Description,
                    }
                }).FirstOrDefault();
            return product;
        }
    }
}
