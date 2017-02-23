using Autofac;
using Autofac.Integration.WebApi;
using Experion.Marina.Common;
using Experion.Marina.IocConfig;
using System;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Experion.Marina.Web.Api
{
    public static class WebApiConfig
    {
        /// <summary>
        /// Registers the specified configuration.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public static void Register(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();

            // Web API configuration and services
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.JsonFormatter.MediaTypeMappings
                .Add(new RequestHeaderMapping("Accept",
                              "text/html",
                              StringComparison.InvariantCultureIgnoreCase,
                              true,
                              "application/json"));

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            RegisterComponents(builder);
            builder.RegisterWebApiFilterProvider(config);

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        /// <summary>
        /// Registers the components.
        /// </summary>
        /// <param name="builder">The builder.</param>
        private static void RegisterComponents(ContainerBuilder builder)
        {
            builder.RegisterType<MarinaLogger>().As<ILogger>()
                .WithParameter((pi, c) => pi.ParameterType == (typeof(Type)), (pi, c) => typeof(ILogger));

            builder.RegisterModule(new NLogModule());

            builder.RegisterDbComponents();         // Register Database related Components
            builder.RegisterGeneralComponents();    // Register General Components
            builder.RegisterEntities();             // Register Entities
            builder.RegisterBusinessServices();     // Register Business Services
            builder.RegisterDataServices();         // Register Data Services
        }
    }
}
