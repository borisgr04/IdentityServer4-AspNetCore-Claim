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
    [RoutePrefix("api/Prueba")]
    public class PruebaController : ApiController
    {
        
        [Authorize(Roles = "Prueba")]
        public IHttpActionResult Get()
        {
            var stringClaimsBuilder = new StringBuilder();
            stringClaimsBuilder.Append("Änibal el mejor y esto es una prueba de que si funciona nojoda");
            foreach (var claim in System.Security.Claims.ClaimsPrincipal.Current.Claims)
            {

                stringClaimsBuilder.Append($"{claim.Type} {claim.Value}");
            }
            return Ok(stringClaimsBuilder.ToString());//System.Security.Claims.ClaimsPrincipal.Current.Claims ); 
        }
    }
}
