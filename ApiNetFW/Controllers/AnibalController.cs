using System.Web.Http;

namespace ApiNetFW.Controllers
{
    public class AnibalController : ApiController
    {
        [HttpGet]
        //[Authorize(Roles = "Front")]
        [Route("api/anibal")]
        public IHttpActionResult ResourceFront()
        {
            var response = new AnibalResponse() { Mensaje = "Api consumida correctamente" };
            return Ok(response);
        }
    }

    class AnibalResponse
    {
        public string Mensaje { get; set; }
    }
}
