using LightsControl.Shared;
using MudBlazor;

namespace LightsControl;

public class DmxChannel
{
    public int Address;
    float val;
    public float Value
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