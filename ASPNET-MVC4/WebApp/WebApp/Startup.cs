using System;
using System.IdentityModel.Tokens;
using Owin;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Newtonsoft.Json.Serialization;
using System.Web.Http;
using System.Net.Http.Formatting;
using Microsoft.Owin.Security.OAuth;
using WebApp.Provider;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Microsoft.Owin.Security.Jwt;
using WebApp.Handler;
using Microsoft.AspNet.Identity;

[assembly: OwinStartup(typeof(WebApp.Startup))]
namespace WebApp
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            app.UseFileServer(new FileServerOptions()
            {
                RequestPath = PathString.Empty,
                FileSystem = new PhysicalFileSystem(@".\client"),
            });

            ConfigureJwtAuth(app);

            HttpConfiguration config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            app.UseWebApi(config);

            

        }

        public void ConfigureJwtAuth(IAppBuilder app)
        {
            var options = new JwtBearerAuthenticationOptions()
            {
                Provider = new BearerAuthenticationProvider(),
                AllowedAudiences = new List<string> {
                    "*"
                },
                IssuerSecurityTokenProviders = new List<SymmetricKeyIssuerSecurityTokenProvider>
                {
                    new SymmetricKeyIssuerSecurityTokenProvider("omega", System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("omega will make life more happier")))
                }
            };

            app.UseJwtBearerAuthentication(options);

        }


    }
}