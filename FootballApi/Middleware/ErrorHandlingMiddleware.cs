using FootballApi.Exceptions;

namespace FootballApi.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = 400;
                context.Response.WriteAsync(ex.Message);
            }
            catch (UnauthorizedException ex)
            {
                context.Response.StatusCode = 401;
                context.Response.WriteAsync(ex.Message);
            }
            catch(ConflictException ex)
            {
                context.Response.StatusCode = 409;
                context.Response.WriteAsync(ex.Message);
            }
            catch(BadRequestException ex)
            {
                context.Response.StatusCode = 500;
                context.Response.WriteAsync(ex.Message);
            }
            
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.WriteAsync("Somthing went wrong");
            }
        }
    }
}
