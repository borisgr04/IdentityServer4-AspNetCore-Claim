using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Results;

namespace WebApiNet4x.Controllers
{
    [RoutePrefix("api/test")]
    public class TestController : ApiController
    {
        //(Roles ="Admin")
        [Authorize (Roles ="Admin")]
        public IHttpActionResult Get()
        {
            var stringClaimsBuilder = new StringBuilder();
            foreach (var claim in System.Security.Claims.ClaimsPrincipal.Current.Claims)
            {

                stringClaimsBuilder.Append($"{claim.Type} {claim.Value}");
            }
            stringClaimsBuilder.Append("Änibal el mejor");
            return Ok(stringClaimsBuilder.ToString());//System.Security.Claims.ClaimsPrincipal.Current.Claims ); 
        }
    }
}
