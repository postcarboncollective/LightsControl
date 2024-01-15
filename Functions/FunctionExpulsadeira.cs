using MudBlazor.Utilities;

namespace LightsControl;

public class FunctionExpulsadeira : Function
{
    public MudColor Color = new MudColor(255, 255, 255, 255);

    protected override void Start()
    {
        float r = (Color.R / 255f);
        float g = (Color.G / 255f);
        float b = (Color.B / 255f);
        if (Switch[(int)Lights.Strobe].Value) PM.Strobe.Set(r, g, b, 0, 1, 0, 0, 0);
        if (Switch[(int)Lights.Par].Value) PM.Par.Set(1, 0, 1, 0);
        if (Switch[(int)Lights.Bar1].Value) PM.Bar[0].Set(r, g, b, 1, 0, 0);
        if (Switch[(int)Lights.Bar2].Value) PM.Bar[1].Set(r, g, b, 1, 0, 0);
    }
}