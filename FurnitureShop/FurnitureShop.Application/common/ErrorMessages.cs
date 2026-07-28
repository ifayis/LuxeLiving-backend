namespace FurnitureShop.Application.Common
{
    public static class ErrorMessages
    {
        // Common
        public const string ValidationFailed = "Validation failed";

        // Category
        public const string CategoryInactive = "Selected category is inactive.";
        public const string CategoryAlreadyExists = "Category already exists.";
        public const string CategoryNotFound = "Category not found.";

        // Auth
        public const string InvalidCredentials = "Invalid credentials";
        public const string UserAlreadyExists = "User already exists.";
        public const string UserBlocked = "Your account has been blocked. Please contact support.";
        public const string InvalidRefreshToken = "Invalid refresh token.";
        public const string RefreshTokenExpired = "Refresh token has expired.";
        public const string UserNotFound = "User not found.";
        public const string CurrentPasswordIncorrect = "Current password is incorrect.";
        public const string PasswordCannotBeSame = "New password cannot be the same as the current password.";
        public const string EmailNotVerified = "Email address is not verified.";
        public const string InvalidResetToken = "Invalid password reset token.";
        public const string ResetTokenExpired = "Password reset token has expired.";
        public const string InvalidVerificationToken = "Invalid email verification token.";
        public const string VerificationTokenExpired = "Email verification token has expired.";
        public const string InvalidToken = "Invalid authentication token.";
        public const string AccountLocked = "Your account has been temporarily locked due to multiple failed login attempts. Please try again later.";

        // Cart
        public const string CartNotFound = "Cart not found.";
        public const string CartItemNotFound = "Cart item not found.";
        public const string CartEmpty = "Your cart is empty.";

        // Products
        public const string ProductAlreadyExists = "Product already exists.";
        public const string ProductNotFound = "product not found";
        public const string ProductUnavailable = "This product is currently unavailable.";
        public const string InvalidProductPrice = "Selling price cannot be greater than the original price.";
        public const string InvalidProduct = "Invalid product.";
        public const string InvalidQuantity = "Invalid quantity.";
        public const string OutOfStock = "Product is out of stock.";
        public const string ProductInactive = "Product is inactive.";

        // Wishlist
        public const string ProductAlreadyInWishlist = "Product already exists in your wishlist.";
        public const string WishlistNotFound = "Wishlist not found.";
        public const string WishlistItemNotFound = "Wishlist item not found.";
        public const string WishlistEmpty = "Wishlist is empty.";

        // Shipping Address
        public const string MaximumShippingAddressesReached = "You can add a maximum of 5 shipping addresses.";
        public const string AddressNotFound = "Shipping address not found.";

        //Order
        public const string OrderNotFound = "Order not found.";
    }
}
