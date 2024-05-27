using Cloud4Feed.Model.Entity;
using Cloud4Feed.Model.Request.Product;
using Cloud4Feed.Model.Response.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud4Feed.Application.Repository.Contract
{
    public interface IProductRepository
    {
        Task<Product> Create(CreateProductRequest createProductRequest);
        Task<Product> Update(UpdateProductRequest updateProductRequest);
        IEnumerable<ProductResponse> GetAll();
        ProductResponse? Get(Guid id);
        Task Delete(Guid id);
    }
}
