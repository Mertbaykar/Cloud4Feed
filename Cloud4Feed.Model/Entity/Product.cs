using Cloud4Feed.Model.Request.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud4Feed.Model.Entity
{
    public class Product : EntityBase
    {
        private Product()
        {

        }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Barcode { get; private set; }
        public decimal Price { get; private set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; private set; }
        public Guid CategoryId { get; private set; }

        public static Product Create(CreateProductRequest createProductRequest)
        {
            Product product = new Product();
            product.Name = createProductRequest.Name;
            product.Description = createProductRequest.Description;
            product.Barcode = createProductRequest.Barcode;
            product.Price = createProductRequest.Price;
            product.CategoryId = createProductRequest.CategoryId;
            return product;
        }

        public Product Update(UpdateProductRequest updateProductRequest)
        {
            this.Name = updateProductRequest.Name;
            this.Description = updateProductRequest.Description;
            this.Barcode = updateProductRequest.Barcode;
            this.Price = updateProductRequest.Price;
            this.CategoryId = updateProductRequest.CategoryId;
            this.ChangeActiveStatus(updateProductRequest.IsActive);
            return this;
        }
    }
}
