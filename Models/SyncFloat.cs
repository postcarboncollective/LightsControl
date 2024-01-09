using LightsControl.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LightsControl;

public class SyncFloat
{
    float val;
    public float Value
    {
        get => val;
        set
        {
            val = value;
            Global.Sync?.Invoke();
        }
    }

    public SyncFloat(float value)
    {
        Value = value;
    }
}