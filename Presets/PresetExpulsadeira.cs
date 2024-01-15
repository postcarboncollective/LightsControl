using MudBlazor.Utilities;

namespace LightsControl;

public class PresetExpulsadeira : Preset
{
    public MudColor Color = new MudColor(255, 255, 255, 255);

    public override void Run()
    {
        Function.StopAll();
        float r = (Color.R / 255f);
        float g = (Color.G / 255f);
        float b = (Color.B / 255f);
        if (Toggle[(int)Lights.Strobe]) PM.Strobe.Set(r, g, b, 0, 1, 0, 0, 0);
        if (Toggle[(int)Lights.Par]) PM.Par.Set(1, 0, 1, 0);
        if (Toggle[(int)Lights.Bar1]) PM.Bar[0].Set(r, g, b, 1, 0, 0);
        if (Toggle[(int)Lights.Bar2]) PM.Bar[1].Set(r, g, b, 1, 0, 0);
    }
}