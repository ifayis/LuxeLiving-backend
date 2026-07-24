namespace FurnitureShop.Application.Common
{
    public static class ResponseMessages
    {
        // Common
        public const string Success = "Success";

        // Cart
        public const string CartRemoved = "Item removed from the cart successful";
        public const string CartCleared = "Cart cleared";

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
        public const string WishlistAdded = "Item Added to Wishlist";
        public const string WishlistItemsMoved = "Items moved to cart";
        public const string WishlistCleared = "Wishlist successfully cleared";
        public const string WishlistItemRemoved = "Item removed from Wishlist";

        // Orders
        public const string NoOrders = "No orders yet";
        // Checkout
        public const string CheckoutPayment = "Payment under process";
        public const string ExecutePayment = "Order created";

        // Shipping Address
        public const string AddressAdded = "Address saved";
        public const string AddressUpdated = "Address Updated";
        public const string AddressDeleted = "Address Deleted";

        // Auth
        public const string LoginSuccess = "Login successful";
        public const string UserRegistered = "User registered successfully";
        public const string LogoutSuccessful = "Logged out successfully.";
        public const string PasswordChangedSuccessfully = "Password changed successfully.";
        public const string PasswordResetEmailSent = "Password reset email has been sent.";
        public const string PasswordResetSuccessful = "Password reset successfully.";
        public const string EmailVerifiedSuccessfully = "Email verified successfully.";
        public const string VerificationEmailSent = "Verification email sent successfully.";
    }
}
