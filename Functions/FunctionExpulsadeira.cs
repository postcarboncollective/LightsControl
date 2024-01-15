using MudBlazor.Utilities;

namespace LightsControl;

public class FunctionExpulsadeira : Function
{
    public ColorFunction Color;
    public bool Executing = false;

    public FunctionExpulsadeira()
    {
        Color = new ColorFunction(new MudColor(255, 255, 255, 255), this);
    }

    protected override void Start()
    {
        Executing = true;
        double r = (Color.Value.R / 255f);
        double g = (Color.Value.G / 255f);
        double b = (Color.Value.B / 255f);
        foreach (Lights light in Enum.GetValues(typeof(Lights)))
        {
            if (Switch[(int)light].Value) PM.SetLight(light, r, g, b);
        }
    }

    public override void Stop()
    {
        Executing = false;
    }
}