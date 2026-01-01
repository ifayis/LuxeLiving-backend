namespace FurnitureShop.Application.Common
{
    public static class ResponseMessages
    {
        // Common
        public const string Success = "Success";
        public const string NotFound = "Resource not found";
        public const string ValidationFailed = "Validation failed";
        public const string AlreadyExists = "Resource already exists";

        // Category
        public const string CategoryCreated = "Category created successfully";
        public const string CategoryDeleted = "Category deleted successfully";
        public const string CategoriesDeleted = "All categories deleted";

        // Product
        public const string ProductCreated = "Product created successfully";
        public const string ProductUpdated = "Product updated successfully";
        public const string ProductActivated = "Product activated";
        public const string ProductDeactivated = "Product deactivated";

        // Auth
        public const string LoginSuccess = "Login successful";
        public const string InvalidCredentials = "Invalid credentials";
        public const string UserRegistered = "User registered successfully";
    }
}
