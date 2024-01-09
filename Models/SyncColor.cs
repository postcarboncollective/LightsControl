using MudBlazor.Utilities;

namespace LightsControl;

public class SyncColor
{
    MudColor val;
    public MudColor Value
    {
        get => val;
        set
        {
            val = value;
            Global.Sync?.Invoke();
        }
    }

    public SyncColor(MudColor value)
    {
        Value = value;
    }
}