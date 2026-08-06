namespace LuxeLiving.Application.Common
{
    public static class OrderNumberGenerator
    {
        public static string Generate()
        {
            return
                $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(100000, 999999)}";
        }
    }
}