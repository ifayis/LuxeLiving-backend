using System.Net;
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
            catch (DbUpdateException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                await WriteResponse(context, "Database constraint violation", ex.InnerException?.Message);
            }
            catch (ArgumentNullException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await WriteResponse(context, "Invalid input", ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                await context.Response.WriteAsJsonAsync(new
                {
                    StatusCode = 409,
                    Message = ex.Message
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                await context.Response.WriteAsJsonAsync(
                    ApiResponse<object>.Fail(ex.Message, 401)
                );
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await WriteResponse(context, "Something went wrong", ex.Message);
            }
        }

        private static async Task WriteResponse(HttpContext context, string message, string? details = null)
        {
            context.Response.ContentType = "application/json";

            var response = ApiResponse<object>.Fail(
                message,
                context.Response.StatusCode
            );

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
