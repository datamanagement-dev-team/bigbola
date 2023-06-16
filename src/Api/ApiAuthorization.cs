using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlueBrown.BigBola.Api
{
	//todo move to Filters folder
	internal class ApiKeyAttribute : ServiceFilterAttribute
	{
		public ApiKeyAttribute()
			: base(typeof(ApiKeyAuthorizationFilter))
		{
		}
	}

	internal class ApiKeyAuthorizationFilter : IAuthorizationFilter
	{
		//todo read from settings
		private readonly string _headerKey = "Operation-API-Key";
		//todo read from settings
		private readonly string _headerValue = "Value";

		public void OnAuthorization(AuthorizationFilterContext context)
		{
			var headerExists = context.HttpContext.Request.Headers.ContainsKey(_headerKey);

			if (!headerExists)
				throw new AuthenticationException();

			var headerValue = context.HttpContext.Request.Headers[_headerKey];

			if (!string.Equals(headerValue, _headerValue))
				throw new AuthenticationException();
		}
	}

	internal class AuthenticationException : Exception { }
}
