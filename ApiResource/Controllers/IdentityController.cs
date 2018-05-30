using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ApiResource.Controllers
{
    [Produces("application/json")]
    public class IdentityController : Controller
    {
        [HttpGet]
        [Authorize("Founder")]
        [Route("api/resourcewithpolicy")]
        public IActionResult ResourceWithPolicy()
        {
            //return new JsonResult(new { ApiName = "Api1", AuthorizationType = "With Policy" });
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }

        [HttpGet]
        [Authorize]
        [Route("api/resourcewithoutpolicy")]
        public IActionResult ResourceWithoutPolicy()
        {
            return new JsonResult(new { ApiName = "Api1", AuthorizationType = "Without Policy" });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("api/roles")]
        public IActionResult ResourceRoles()
        {
            return new JsonResult(new { ApiName = "Api1", AuthorizationType = "Without Policy" });
        }

        [HttpGet]
        [Authorize(Roles = "Front")]
        [Route("api/front")]
        public IActionResult ResourceFront()
        {
            return new JsonResult(new { ApiName = "Api1", AuthorizationType = "Without Policy" });
        }

    }
}