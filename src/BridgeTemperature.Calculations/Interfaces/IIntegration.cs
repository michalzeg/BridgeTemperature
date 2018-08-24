namespace BridgeTemperature.IntegrationFunctions
{
    public interface IIntegration
    {
        double Moment { get; }
        double NormalForce { get; }
    }
}