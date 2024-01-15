namespace LightsControl;

public class Dmx
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

    public Dmx(int addr, int value)
    {
        Address = addr;
        Value = value;
    }
}