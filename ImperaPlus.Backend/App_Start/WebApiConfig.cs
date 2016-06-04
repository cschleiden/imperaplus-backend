using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Validation;
using ImperaPlus.Backend.Filters;
using ImperaPlus.Backend.Validation;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using ImperaPlus.Backend.Diagnostics;

namespace ImperaPlus.Backend
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            //config.SuppressDefaultHostAuthentication();
            //config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType)); 

            // Web API routes
            config.MapHttpAttributeRoutes();

            // config.Routes.MapHttpRoute(
            //     name: "DefaultApi",
            //     routeTemplate: "api/{controller}/{id}",
            //     defaults: new { id = RouteParameter.Optional }
            // );

            // Diagnostic filters
            config.Filters.Add(new MiniProfilerActionFilter());
            config.Filters.Add(new ApiExceptionFilterAttribute());

            // Configure validation
            config.Filters.Add(new CheckModelForNullAttribute());
            config.Filters.Add(new ValidateModelAttribute());

            config.Services.Replace(typeof(IBodyModelValidator), new PrefixlessBodyModelValidator(config.Services.GetBodyModelValidator()));

            // Configure allowed media type formatting
            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());
            config.Formatters.Add(new XmlMediaTypeFormatter());

            // Use camel case for JSON data.
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
            config.Formatters.JsonFormatter.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;

            // Serialize enums using string values
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter
            {
                CamelCaseText = false
            });

            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
        }
    }
}