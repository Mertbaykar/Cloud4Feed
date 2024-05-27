using Cloud4Feed.Application.Auth;
using Cloud4Feed.Application.Repository;
using Cloud4Feed.Application.Repository.Contract;
using Cloud4Feed.Model.Entity;
using Cloud4Feed.Model.Request.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cloud4Feed.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ServiceFilter(typeof(BasicAuthenticationFilter))]
    public class UserController : ControllerBase
    {
        readonly IUserRepository userRepository;

        public UserController(IUserRepository productRepository)
        {
            this.userRepository = productRepository;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        {
            try
            {
                User user = await userRepository.Create(request);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
