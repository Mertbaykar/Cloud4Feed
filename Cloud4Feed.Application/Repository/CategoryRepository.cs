using Cloud4Feed.Application.Repository.Contract;
using Cloud4Feed.Application.Service.Internal.Contract;
using Cloud4Feed.Infrastructure.DBContext;
using Cloud4Feed.Model.Entity;
using Cloud4Feed.Model.Request.Category;
using Cloud4Feed.Model.Request.Product;
using Cloud4Feed.Model.Response.Category;
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
    public class CategoryRepository : RepositoryBase, ICategoryRepository
    {
        const string categoriesCache = "categories";

        readonly IValidator<Category> validator;
        readonly ICacheService cacheService;

        public CategoryRepository(ECommerceContext eCommerceContext, IValidator<Category> validator, ICacheService cacheService) : base(eCommerceContext)
        {
            this.validator = validator;
            this.cacheService = cacheService;
        }

        private async Task ValidateByDB(Category category)
        {
            if (await MasterDataDb.Category.AnyAsync(c => c.Name == category.Name))
                throw new Exception("Kategori önceden tanımlanmış");
        }

        public async Task<Category> Create(CreateCategoryRequest createCategoryRequest)
        {
            Category category = Category.Create(createCategoryRequest);
            await base.Validate(category, validator);
            await ValidateByDB(category);
            var result = await base.Add(category);
            cacheService.Remove(categoriesCache);
            return result;
        }

        public async Task Delete(Guid id)
        {
            await base.Delete<Category>(id);
            cacheService.Remove(categoriesCache);
        }

        public async Task<Category> Update(UpdateCategoryRequest updateCategoryRequest)
        {
            Category? category = await base.Get<Category>(updateCategoryRequest.Id);
            if (category == null)
                throw new Exception("Kategori bulunamadı");

            category.Update(updateCategoryRequest);
            await base.Validate(category, validator);
            await ValidateByDB(category);
            await Save();
            cacheService.Remove(categoriesCache);
            return category;
        }

        public IEnumerable<MiniCategoryResponse> GetAll()
        {
            // cache dolu ise
            var cachedCategories = cacheService.Get<IEnumerable<MiniCategoryResponse>>(categoriesCache);
            if (cachedCategories != null && cachedCategories.Count() > 0)
                return cachedCategories;

            var categories = MasterDataDb.Category.AsNoTracking()
                 .Select(c => new MiniCategoryResponse
                 {
                     Id = c.Id,
                     Name = c.Name,
                     Description = c.Description,
                 }).ToList();
            // cache yenile
            cacheService.Set(categoriesCache, categories);
            return categories;
        }

        public CategoryResponse? Get(Guid id)
        {
            var category = MasterDataDb.Category
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => new CategoryResponse
                 {
                     Id = c.Id,
                     Name = c.Name,
                     Description = c.Description,
                     Products = c.Products.Select(p => new CategoryProductResponse
                     {
                         Id = p.Id,
                         Name = p.Name,
                         Barcode = p.Barcode,
                         Description = p.Description,
                         Price = p.Price,
                     })
                 }).FirstOrDefault();
            return category;
        }
    }
}
