using BlueBrown.BigBola.Application;

namespace BlueBrown.BigBola.Api
{
    public class RequestValidationMiddlware
    {
        private readonly RequestDelegate _next;

        public RequestValidationMiddlware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (RequestValidationException ex)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(ex.Message);
            }
        }
    }
}
