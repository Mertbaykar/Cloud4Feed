using Cloud4Feed.Model.Request.Category;
using Cloud4Feed.Model.Request.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud4Feed.Model.Entity
{
    public class Category : EntityBase
    {
        private Category()
        {
            
        }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public ICollection<Product> Products { get; private set; }


        public static Category Create(CreateCategoryRequest createCategoryRequest)
        {
            Category category = new Category();
            category.Name = createCategoryRequest.Name;
            category.Description = createCategoryRequest.Description;
            return category;
        }

        public Category Update(UpdateCategoryRequest updateCategoryRequest)
        {
            this.Name = updateCategoryRequest.Name;
            this.Description = updateCategoryRequest.Description;
            this.ChangeActiveStatus(updateCategoryRequest.IsActive);
            return this;
        }
    }
}
