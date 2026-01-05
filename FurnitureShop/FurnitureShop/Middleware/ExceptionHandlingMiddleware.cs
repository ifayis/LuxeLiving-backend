using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using FurnitureShop.Application.Common;

namespace FurnitureShop.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DbUpdateException)
            {
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                await WriteResponse(context, "Database constraint violation");
            }
            catch (ArgumentException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await WriteResponse(context, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                await WriteResponse(context, ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await WriteResponse(context, ex.Message);
            }
            catch (Exception)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await WriteResponse(context, "Something went wrong");
            }
        }

        private static async Task WriteResponse(HttpContext context, string message)
        {
            context.Response.ContentType = "application/json";

            var response = ApiResponse<object>.Fail(
                message,
                context.Response.StatusCode
            );

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response)
            );
        }
    }
}
