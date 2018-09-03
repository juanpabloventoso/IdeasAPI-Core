using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdeasAPI.Models;
using IdeasAPI.Domain;
using IdeasAPI.Repository;
using Microsoft.Extensions.Configuration;

namespace IdeasAPI.Controllers
{

    [Route("[controller]")]
    public class UsersController : Controller
    {

        private readonly IConfiguration _config;
        private readonly IUserRepository _repository;

        public UsersController(IConfiguration config, IUserRepository repository)
        {
            _config = config;
            _repository = repository;
        }


        // POST users
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]UserModel value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User
            {
                email = value.email,
                name = value.name,
                password = Functions.MD5(value.password),
                refresh_token = IdeasAPI.Functions.GenerateRefreshToken()
            };
            await _repository.InsertAsync(user);

            var token = IdeasAPI.Functions.BuildToken(user, _config);
            return Created("users/{user.email}", new { jwt = token, user.refresh_token });
        }

    }
}
