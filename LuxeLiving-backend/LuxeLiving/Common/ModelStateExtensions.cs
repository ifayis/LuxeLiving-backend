using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LuxeLiving.API.Common
{
    public static class ModelStateExtensions
    {
        public static Dictionary<string, string[]> ToErrorDictionary(
            this ModelStateDictionary modelState)
        {
            return modelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(
                    x => x.Key,
                    x => x.Value.Errors
                          .Select(e => e.ErrorMessage)
                          .ToArray()
                );
        }
    }
}
