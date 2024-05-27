using Cloud4Feed.Model.Entity;
using Cloud4Feed.Model.Request.Category;
using Cloud4Feed.Model.Request.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud4Feed.Application.Repository.Contract
{
    public interface IUserRepository
    {
        Task<User> Create(CreateUserRequest createUserRequest);
        bool Authenticate(string email, string password);
    }
}
