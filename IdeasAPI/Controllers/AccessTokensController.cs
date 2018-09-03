using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdeasAPI.Models;
using IdeasAPI.Domain;
using IdeasAPI.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace IdeasAPI.Controllers
{
    [Route("access-tokens")]
    [Authorize]
    public class AccessTokensController : Controller
    {

        private readonly IConfiguration _config;
        private readonly IUserRepository _repository;

        public AccessTokensController(IConfiguration config, IUserRepository repository)
        {
            _config = config;
            _repository = repository;
        }

        // Authenticate a new user
        private async Task<User> Authenticate(LoginModel login)
        {
            return await _repository.Query()
                .FirstOrDefaultAsync(x => x.email.Equals(login.email, StringComparison.CurrentCultureIgnoreCase) &&
                x.password.Equals(Functions.MD5(login.password), StringComparison.CurrentCultureIgnoreCase));
        }


        // Get the current refresh token for the user searching by email
        private async Task<User> GetUserByRefreshToken(string refresh_token)
        {
            User user = await _repository.Query()
                .FirstOrDefaultAsync(x => x.refresh_token.Equals(refresh_token, StringComparison.CurrentCultureIgnoreCase));
            return user;
        }

        // Save the refresh token for the user in the repository
        public async Task<string> SaveRefreshToken(User user)
        {
            user.refresh_token = IdeasAPI.Functions.GenerateRefreshToken();
            await _repository.UpdateAsync(user);
            return user.refresh_token;
        }

        // Empty the refresh token for the user in the repository
        private async void EmptyRefreshToken(User user)
        {
            user.refresh_token = String.Empty;
            await _repository.UpdateAsync(user);
        }

        // Generate a new token for a given email and password
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]LoginModel login)
        {
            var user = await Authenticate(login);

            if (user == null)
                return Unauthorized();

            // Save the refresh token and get a new JWT
            var token = IdeasAPI.Functions.BuildToken(user, _config);
            var refreshToken = await SaveRefreshToken(user);

            return Ok(new { jwt = token, refresh_token = refreshToken });
        }

        // Refresh an expired token
        [AllowAnonymous]
        [Route("access-tokens/refresh")]
        [HttpPost]
        public async Task<IActionResult> Refresh([FromBody]string refresh_token)
        {
            var user = await GetUserByRefreshToken(refresh_token);

            if (user == null)
                return NotFound();

            var token = IdeasAPI.Functions.BuildToken(user, _config);

            return Ok(new { jwt = token });
        }

        // Delete the access token (logout)
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody]string refresh_token)
        {
            var user = await GetUserByRefreshToken(refresh_token);

            if (user == null)
                return NotFound();

            EmptyRefreshToken(user);

            return NoContent();
        }
    }
}
