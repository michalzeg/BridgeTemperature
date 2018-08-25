namespace BridgeTemperature.Calculations.Interfaces
{
    public interface IIntegration
    {
        double Moment { get; }
        double NormalForce { get; }
    }
}