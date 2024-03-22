namespace LightsControl;

public class DmxChannel
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

    public DmxChannel(int addr, int value)
    {
        Address = addr;
        Value = value;
    }
}