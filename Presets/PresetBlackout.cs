using MudBlazor.Utilities;

namespace LightsControl;

public class PresetBlackout : Preset
{
    public override void Run()
    {
        Function.StopAll();
        if (Toggle[(int)Lights.Strobe]) PM.Strobe.Set(0, 0, 0, 0, 0, 0, 0, 0);
        if (Toggle[(int)Lights.Par]) PM.Par.Set(0, 0, 0, 0);
        if (Toggle[(int)Lights.Bar1]) PM.Bar[0].Set(0, 0, 0, 0, 0, 0);
        if (Toggle[(int)Lights.Bar2]) PM.Bar[1].Set(0, 0, 0, 0, 0, 0);
    }
}