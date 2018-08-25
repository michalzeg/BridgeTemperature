namespace BridgeTemperature.Shared.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsNaN(this double initialValue)
        {
            return double.IsNaN(initialValue);
        }
    }
}