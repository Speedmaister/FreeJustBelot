using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FreeJustBelot.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //config.Routes.MapHttpRoute(
            //   name: "Games",
            //   routeTemplate: "api/games/{action}",
            //   defaults: new { controller = "games" }
            //   );

            config.Routes.MapHttpRoute(
                name: "DefaultUri",
                routeTemplate: "api/{controller}/{action}",
                defaults: new { }
                );

            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            config.EnableSystemDiagnosticsTracing();
        }
    }
}
