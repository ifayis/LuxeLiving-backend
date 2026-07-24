namespace FurnitureShop.Application.Common
{
    public class EmailSettings
    {
        public const string SectionName = "EmailSettings";

        public string DisplayName { get; set; } = string.Empty;

        public string From { get; set; } = string.Empty;

        public string Host { get; set; } = string.Empty;

        public int Port { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public bool UseSsl { get; set; } = true;
    }
}