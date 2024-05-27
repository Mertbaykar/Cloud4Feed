using Cloud4Feed.Model.Entity;
using Cloud4Feed.Model.Request.Category;
using Cloud4Feed.Model.Response.Category;
using Cloud4Feed.Model.Response.Product;

namespace Cloud4Feed.Application.Repository.Contract
{
    public interface ICategoryRepository
    {
        Task<Category> Create(CreateCategoryRequest createProductRequest);
        Task<Category> Update(UpdateCategoryRequest createProductRequest);
        IEnumerable<MiniCategoryResponse> GetAll();
        CategoryResponse? Get(Guid id);
        Task Delete(Guid id);
    }
}
