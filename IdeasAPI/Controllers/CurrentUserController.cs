using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdeasAPI.Models;
using IdeasAPI.Domain;
using IdeasAPI.Repository;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace IdeasAPI.Controllers
{

    [Route("me")]
    [Authorize]
    public class CurrentUserController : Controller
    {

        private readonly IUserRepository _repository;
        public CurrentUserController(IUserRepository repository)
        {
            _repository = repository;
        }

        // GET me
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var email = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault();
            if (email == null)
                return Unauthorized();

            User user = await _repository.Query()
                .FirstOrDefaultAsync(x => x.email.Equals(email.Value, StringComparison.CurrentCultureIgnoreCase));
            if (user == null)
                return NotFound();

            var avatarUrl = "https://www.gravatar.com/avatar/" + Functions.MD5(email.Value) + "?d=mm&s=200";
            var result = new UserModel
            {
                email = user.email,
                name = user.name,
                avatar_url = avatarUrl
            };

            return Ok(result);
        }

    }
}
