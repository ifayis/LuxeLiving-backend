using FurnitureShop.Application.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace FurnitureShop.API.Filters
{
    public class ApiResponseFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // nothing here
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Only wrap successful ObjectResult (Ok, Created, etc.)
            if (context.Result is ObjectResult result && result.StatusCode is >= 200 and < 300)
            {
                var data = result.Value;

                // Build ApiResponse<T> dynamically
                var apiResponseType = typeof(ApiResponse<>)
                    .MakeGenericType(data?.GetType() ?? typeof(object));

                var successMethod = apiResponseType.GetMethod("Success");

                var apiResponse = successMethod!.Invoke(
                    null,
                    new object?[] { data, "Success", result.StatusCode ?? 200 }
                );

                context.Result = new ObjectResult(apiResponse)
                {
                    StatusCode = result.StatusCode
                };
            }
        }
    }
}
