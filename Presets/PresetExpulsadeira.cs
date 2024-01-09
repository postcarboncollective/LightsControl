using MudBlazor.Utilities;

namespace LightsControl;

public class PresetExpulsadeira : Preset
{
    public SyncColor Color = new SyncColor(new MudColor(255, 255, 255, 255));

    public override void Run()
    {
        float r = (Color.Value.R / 255f) * 100;
        float g = (Color.Value.G / 255f) * 100;
        float b = (Color.Value.B / 255f) * 100;
        if (Toggle[(int)Lights.Strobe].Value) PM.Strobe.Set(r, g, b, 0, 100, 0, 0, 0);
        if (Toggle[(int)Lights.Par].Value) PM.Par.Set(100, 0, 100, 0);
        if (Toggle[(int)Lights.Bar1].Value) PM.Bar[0].Set(r, g, b, 100, 0, 0);
        if (Toggle[(int)Lights.Bar2].Value) PM.Bar[1].Set(r, g, b, 100, 0, 0);
    }
}