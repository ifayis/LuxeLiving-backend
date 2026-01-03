namespace FurnitureShop.Application.Common
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        public static ApiResponse<T> Success(
            T? data,
            string message = ResponseMessages.Success,
            int statusCode = 200)
        {
            return new ApiResponse<T>
            {
                StatusCode = statusCode,
                Message = message,
                Data = data
            };
        }

        public static ApiResponse<T> Fail(
            string message,
            int statusCode = 400,
            T? data = default)
        {
            return new ApiResponse<T>
            {
                StatusCode = statusCode,
                Message = message,
                Data = data
            };
        }
    }
}
