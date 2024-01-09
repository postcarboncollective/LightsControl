using LightsControl.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LightsControl;

public class SyncBool
{
    bool val;
    public bool Value
    {
        get => val;
        set
        {
            val = value;
            Global.Sync?.Invoke();
        }
    }

    public SyncBool(bool value)
    {
        Value = value;
    }
}