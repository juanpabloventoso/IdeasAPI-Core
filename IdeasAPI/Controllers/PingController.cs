using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdeasAPI.Models;
using IdeasAPI.Domain;
using IdeasAPI.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace IdeasAPI.Controllers
{

    [Route("[controller]")]
    public class PingController : Controller
    {


        // GET ping
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            return Ok("Hello :)");
        }

    }
}
