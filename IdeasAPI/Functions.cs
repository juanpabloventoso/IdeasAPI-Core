using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using IdeasAPI.Domain;
using System.Text;
using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Linq;
using IdeasAPI.Repository;
using Microsoft.EntityFrameworkCore;

namespace IdeasAPI
{
    public static class Functions
    {

        // Generate a MD5 hash from a string
        public static string MD5(string value)
        {
            byte[] FHash = System.Security.Cryptography.MD5.Create().ComputeHash(System.Text.Encoding.ASCII.GetBytes(value));
            System.Text.StringBuilder FSB = new System.Text.StringBuilder();
            for (int I = 0; I < FHash.Length; I++)
                FSB.Append(FHash[I].ToString("X2"));
            return FSB.ToString().ToLower();
        }

        // Build a new JWT from an user
        public static string BuildToken(User user, IConfiguration config)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(config["Jwt:Issuer"],
                config["Jwt:Issuer"],
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds,
                claims: new List<Claim> { new Claim(ClaimTypes.Name, user.email) }
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Generate a hard to guess refresh token ID
        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        // Get the current user entity from context
        public static async Task<User> GetCurrentUser(HttpContext context, IUserRepository repository)
        {
            var email = context.User.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault();
            if (email == null)
                return null;

            return await repository.Query()
                .FirstOrDefaultAsync(x => x.email.Equals(email.Value, StringComparison.CurrentCultureIgnoreCase));

        }

    }
}
