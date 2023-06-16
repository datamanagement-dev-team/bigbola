using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlueBrown.BigBola.Api
{
    public class ApiKeyAttribute : ServiceFilterAttribute
    {
        public ApiKeyAttribute()
            : base(typeof(ApiKeyAuthorizationFilter))
        {
        }
    }

    public class ApiKeyAuthorizationFilter : IAuthorizationFilter
    {
        private const string XApiKey = "Operation-API-Key";
        private const string XApiValue = "Value";

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool apiKeyExists = context.HttpContext.Request.Headers.ContainsKey(XApiKey);

            if (!apiKeyExists)
            {
                context.Result = new UnauthorizedResult();
            }

            string apiValue = context.HttpContext.Request.Headers[XApiKey];

            if(!apiValue.Equals(XApiValue))
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
