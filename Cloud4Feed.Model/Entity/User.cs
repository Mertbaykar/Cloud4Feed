using Cloud4Feed.Model.Request.Product;
using Cloud4Feed.Model.Request.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud4Feed.Model.Entity
{
    public class User : EntityBase
    {
        private User()
        {
            
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }


        public static User Create(CreateUserRequest createUserRequest)
        {
            User user = new User();
            user.FirstName = createUserRequest.FirstName;
            user.LastName = createUserRequest.LastName;
            user.Email = createUserRequest.Email;
            user.Password = createUserRequest.Password;
            return user;
        }

        //public User Update(UpdateCategoryRequest updateCategoryRequest)
        //{
        //    this.Name = updateCategoryRequest.Name;
        //    this.Description = updateCategoryRequest.Description;
        //    this.ChangeActiveStatus(updateCategoryRequest.IsActive);
        //    return this;
        //}
    }
}
