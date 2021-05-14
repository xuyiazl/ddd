using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using XUCore.Extensions;

namespace DDD.Infrastructure.Language
{
    public class LanguageRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!values.ContainsKey("culture"))
                return false;

            var culture = values["culture"].SafeString().ToLower();

            return culture == "en-us" || culture == "zh-cn";
        }
    }

}
