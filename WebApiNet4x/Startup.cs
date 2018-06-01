using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Threading.Tasks;
using System.Web.Http;
using IdentityServer3.AccessTokenValidation;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartup(typeof(WebApiNet4x.Startup))]

namespace WebApiNet4x
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            DesactivarLaAsignacionPredeterminadaDeJwt();

            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = "http://localhost:5000",
                RequiredScopes = new[] { "Api1" },
                DelayLoadMetadata = true,
            });

            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );
            app.UseWebApi(config);


            //app.UseCookieAuthentication(new CookieAuthenticationOptions());
            //app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            //{
            //    Authority = "http://localhost:5000",
            //    ValidationMode = ValidationMode.ValidationEndpoint,
            //    RequiredScopes = new[] { "Api1" },
            //    ClientId = "Api1",
            //    DelayLoadMetadata = true,
            //    //RequireHttpsMetadata = false,
            //    ClientSecret = "123654",
            //    //    options.Authority = "http://localhost:5000";
            //    //options.RequireHttpsMetadata = false;
            //    //ApiName = "Api1",
            //});

            ////configure web api
            //var config = new HttpConfiguration();
            //config.MapHttpAttributeRoutes();

            ////require authentication for all controllers

            //config.Filters.Add(new AuthorizeAttribute());

            //app.UseWebApi(config);
        }

        private static void DesactivarLaAsignacionPredeterminadaDeJwt()
        {
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();
        }
    }
}
