using Investment.Domain.Exceptions;
using System.Net;

namespace Investment.API.Middlewares
{
    public class AppExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            int status;
            try
            {
                await next(context);
            }
            catch (InvalidPropertyException e)
            {
                status = (int)HttpStatusCode.BadRequest;
                context.Response.StatusCode = status;
                await context.Response.WriteAsJsonAsync(new { message = e.Message, statusCode = status });
            }
            catch (NotFoundException e)
            {
                status = (int)HttpStatusCode.NotFound;
                context.Response.StatusCode = status;
                await context.Response.WriteAsJsonAsync(new { message = e.Message, statusCode = status });
            }
            catch (UnauthorizedException e)
            {
                status = (int)HttpStatusCode.Unauthorized;
                context.Response.StatusCode = status;
                await context.Response.WriteAsJsonAsync(new { message = e.Message, statusCode = status });
            }
            catch (ForbiddenException e)
            {
                status = (int)HttpStatusCode.Forbidden;
                context.Response.StatusCode = status;
                await context.Response.WriteAsJsonAsync(new { message = e.Message, statusCode = status });
            }
            catch (Exception e)
            {
                status = 500;
                context.Response.StatusCode = status;
                await context.Response.WriteAsJsonAsync(new { message = e.Message, statusCode = status });
            }
        }
    }
}
