namespace FurnitureShop.Application.Common
{
    public static class ResponseMessages
    {
        // Common
        public const string Success = "Success";

        // Category
        public const string CategoryCreated = "Category created successfully";
        public const string CategoryDeleted = "Category deleted successfully";
        public const string CategoriesDeleted = "All categories deleted";

        // Product
        public const string ProductCreated = "Product created successfully";
        public const string ProductUpdated = "Product updated successfully";
        public const string ProductActivated = "Product activated";
        public const string ProductDeactivated = "Product deactivated";
        public const string ProductDeleted = "Product deleted successfully";
        public const string ProductsDeleted = "All products deleted successfully";

        // Wishlist
        public const string WishlistItemAdded = "Item adde to Wishlist";
        public const string WishlistFetched = "Wishlist items fetched Successfully";
        public const string WishlistCleared = "Wishlist successfully cleared";


        // Auth
        public const string LoginSuccess = "Login successful";
        public const string UserRegistered = "User registered successfully";
    }
}
