using MudBlazor.Utilities;

namespace LightsControl;

public class FunctionExpulsadeira : Function
{
    public MudColor Color = new MudColor(255, 255, 255, 255);

    protected override void Start()
    {
        double r = (Color.R / 255f);
        double g = (Color.G / 255f);
        double b = (Color.B / 255f);
        foreach (Lights light in Enum.GetValues(typeof(Lights)))
        {
            if (Switch[(int)light].Value) PM.SetLight(light, r, g, b);
        }
    }
}