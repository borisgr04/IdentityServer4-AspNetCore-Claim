using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiResource.Controllers
{
    [Produces("application/json")]
    [Route("api/values")]
    public class ValuesController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(new { ApiName = "Api1", AuthorizationType = "Ejemplo" });
        }
    }
}