using LightsControl.Shared;
using Microsoft.AspNetCore.Components;
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
            Global.Sync.Invoke();
            Osc.SendDmx(Address, value);
        }
    }

    public DmxChannel(int addr, int value)
    {
        Address = addr;
        Value = value;
    }
}