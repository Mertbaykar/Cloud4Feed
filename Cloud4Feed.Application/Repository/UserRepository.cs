using Cloud4Feed.Application.Repository.Contract;
using Cloud4Feed.Infrastructure.DBContext;
using Cloud4Feed.Model.Entity;
using Cloud4Feed.Model.Request.Product;
using Cloud4Feed.Model.Request.User;
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
    public class UserRepository : RepositoryBase, IUserRepository
    {
        readonly IValidator<User> validator;

        public UserRepository(ECommerceContext eCommerceContext, IValidator<User> validator) : base(eCommerceContext)
        {
            this.validator = validator;
        }

        private async Task ValidateByDB(User user)
        {
            if (await MasterDataDb.User.AnyAsync(u => u.Email == user.Email || (u.FirstName.ToLower() == user.FirstName.ToLower() && u.LastName.ToLower() == user.LastName.ToLower())))
                throw new Exception("Kullanıcı zaten tanımlanmış");
        }

        public async Task<User> Create(CreateUserRequest createUserRequest)
        {
            User user = User.Create(createUserRequest);
            await base.Validate(user,validator);
            await ValidateByDB(user);
            return await base.Add(user);
        }

        public bool Authenticate(string email, string password)
        {
            return MasterDataDb.User.Any(u => u.Email == email && u.Password == u.Password && u.IsActive);
        }
    }
}
