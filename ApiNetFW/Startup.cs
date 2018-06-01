using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;

[assembly: OwinStartup(typeof(ApiNetFW.Startup))]

namespace ApiNetFW
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                AuthenticationType = "oidc",
                SignInAsAuthenticationType = "Cookies",
                Authority = "https://demo.identityserver.io/",
                RedirectUri = "http://localhost:5003/signin-oidc",
                ClientId = "mplicit.shortlived",
                RequireHttpsMetadata = false,
                ClientSecret= "123654",
                Scope= "Api1",
            });

            // Para obtener más información sobre cómo configurar la aplicación, visite https://go.microsoft.com/fwlink/?LinkID=316888
            //app.UseCookieAuthentication(new CookieAuthenticationOptions());
            //app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            //{
            //    Authority = "http://www.abcdefgh.com:5000",
            //    ValidationMode = ValidationMode.ValidationEndpoint,
            //    RequiredScopes = new[] { "AuthorizationWebApiNETFramework" },

            //    //    options.Authority = "http://localhost:5000";
            //    //options.RequireHttpsMetadata = false;
            //    //options.ApiName = "Api1";
            //});

            ////configure web api
            //var config = new HttpConfiguration();
            //config.MapHttpAttributeRoutes();

            ////require authentication for all controllers

            //config.Filters.Add(new AuthorizeAttribute());

            //app.UseWebApi(config);
        }
    }
}
