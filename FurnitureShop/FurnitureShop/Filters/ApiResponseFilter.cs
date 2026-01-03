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
            if (context.Result is ObjectResult { Value: ApiResponse<object> })
            {
                await next();
                return;
            }

            if (context.Result is ObjectResult objectResult &&
                objectResult.StatusCode is >= 200 and < 300)
            {
                context.Result = new ObjectResult(
                    ApiResponse<object>.Success(objectResult.Value)
                )
                {
                    StatusCode = objectResult.StatusCode
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
