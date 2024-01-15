namespace LightsControl;

public class SyncDmx
{
    public int Address;
    double val;
    public double Value
    {
        get => val;
        set
        {
            val = value;
            Osc.SendDmx(Address, value);
        }
    }

    public SyncDmx(int addr, int value)
    {
        Address = addr;
        Value = value;
    }
}