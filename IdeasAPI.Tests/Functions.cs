
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdeasAPI.Tests
{
    public static class Functions
    {

        // Add a mock identity claim to the controller context
        public static void AddMockIdentityToContext(Microsoft.AspNetCore.Mvc.Controller controller, string identity)
        {
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.User = new ClaimsPrincipal();
            controller.ControllerContext.HttpContext.User.AddIdentity(new ClaimsIdentity());
            ((ClaimsIdentity)controller.ControllerContext.HttpContext.User.Identity).AddClaim(new Claim(ClaimTypes.Name, identity));
        }

    }
}
