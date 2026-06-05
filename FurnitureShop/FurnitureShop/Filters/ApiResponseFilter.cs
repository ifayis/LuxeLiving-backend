using FurnitureShop.Application.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FurnitureShop.API.Filters
{
    public class ApiResponseFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(
            ResultExecutingContext context,
            ResultExecutionDelegate next)
        {
            if (context.Result is ObjectResult objectResult1 &&
                objectResult1.Value != null &&
                objectResult1.Value.GetType().IsGenericType &&
                objectResult1.Value.GetType().GetGenericTypeDefinition() == typeof(ApiResponse<>))
            {
                await next();
                return;
            }

            if (context.Result is ObjectResult objectResult2 &&
                objectResult2.StatusCode is >= 200 and < 300)
            {
                context.Result = new ObjectResult(
                    ApiResponse<object>.Success(objectResult2.Value)
                )
                {
                    StatusCode = objectResult2.StatusCode
                };
            }
            else if (context.Result is EmptyResult)
            {
                context.Result = new ObjectResult(
                    ApiResponse<object>.Success(null)
                )
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }

            await next();
        }
    }
}
