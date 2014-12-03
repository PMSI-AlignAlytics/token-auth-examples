using System;
using Autofac.Integration.WebApi;
using Owin;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Newtonsoft.Json.Serialization;
using System.Web.Http;
using System.Net.Http.Formatting;
using WebApp.Provider;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Microsoft.Owin.Security.Jwt;
using Autofac;
using WebApp.Controllers;

[assembly: OwinStartup(typeof(WebApp.Startup))]
namespace WebApp
{
    public class Startup
    {
        private IAppConfig _appConfig;

        public Startup() { }

        public Startup(IAppConfig appConfig)
        {
            _appConfig = appConfig;
        }

        public IContainer Configure(IAppConfig config)
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(typeof(AccountController).Assembly);
            builder.RegisterInstance(config).As<IAppConfig>();

            var container = builder.Build();

            return container;
        }

        public void Configuration(IAppBuilder app)
        {
            var path = "";

          
            if (_appConfig == null)
            {
                Console.WriteLine("starting in dev mode");
                _appConfig = new AppConfig
                {
                    Name = "omega",
                    Key = "omega will make life more happier",
                    DisplayName = "omega"
                };
                path = @".\client";
            }
            else
            {
                path = @"..\client";
            }

            var container = Configure(_appConfig);
            app.UseAutofacMiddleware(container);

            app.UseFileServer(new FileServerOptions()
            {
                RequestPath = PathString.Empty,
                FileSystem = new PhysicalFileSystem(path)
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

            app.UseAutofacWebApi(config);
            app.UseWebApi(config);

        }

        public void ConfigureJwtAuth(IAppBuilder app)
        {
            var options = new JwtBearerAuthenticationOptions()
            {
                Provider = new BearerAuthenticationProvider(_appConfig),
                AllowedAudiences = new List<string> {
                    "*"
                },
                IssuerSecurityTokenProviders = new List<SymmetricKeyIssuerSecurityTokenProvider>
                {
                    new SymmetricKeyIssuerSecurityTokenProvider(_appConfig.Name, System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(_appConfig.Key)))
                }
            };

            app.UseJwtBearerAuthentication(options);

        }


    }
}