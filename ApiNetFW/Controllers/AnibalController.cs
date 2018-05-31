using System.Web.Http;

namespace ApiNetFW.Controllers
{
    public class AnibalController : ApiController
    {
        [HttpGet]
        [Authorize]
        [Route("api/anibal")]
        public IHttpActionResult ResourceFront()
        {
            var response = new AnibalResponse() { Mensaje = "Api consumida correctamente juana la loca" };
            return Ok(response);
        }
    }

    class AnibalResponse
    {
        public string Mensaje { get; set; }
    }
}
