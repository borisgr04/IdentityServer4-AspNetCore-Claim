using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Http;

namespace WebApiNet4x.Controllers
{
    [RoutePrefix("api/sinauth")]
    public class SinAuthController : ApiController
    {
       [Authorize]
       [Route("")]
        public IHttpActionResult Get()
        {
            var stringClaimsBuilder = new StringBuilder();
            var user = (HttpContext.Current.User.Identity as ClaimsIdentity);
            user.getu = "Portal";
            foreach (var claim in System.Security.Claims.ClaimsPrincipal.Current.Claims)
            {

                stringClaimsBuilder.Append($"{claim.Type} {claim.Value}");
            }
            stringClaimsBuilder.Append("Änibal el mejor");
            return Ok(stringClaimsBuilder.ToString());//System.Security.Claims.ClaimsPrincipal.Current.Claims ); 
        }
    }
}
