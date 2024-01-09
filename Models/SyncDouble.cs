namespace LightsControl;

public class SyncDouble
{
    double val;
    public double Value
    {
        get => val;
        set
        {
            val = value;
            Global.Sync?.Invoke();
        }
    }

    public SyncDouble(double value)
    {
        Value = value;
    }
}