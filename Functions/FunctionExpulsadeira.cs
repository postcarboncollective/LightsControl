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
        ResetBarType();
        Executing = true;
        double r = (Color.Value.R / 255f);
        double g = (Color.Value.G / 255f);
        double b = (Color.Value.B / 255f);
        foreach (Lights light in Enum.GetValues(typeof(Lights)))
        {
            if (Switch[(int)light].Value) PM.SetLight(light, r, g, b, 1);
        }
    }

    public override void Stop()
    {
        Executing = false;
    }

    public void ResetBarType()
    {
        for (int i = 0; i < Switch.Count; i++)
        {
            if (Switch[i].Value)
            {
                if (i >= (int)Lights.Bar1 && i <= (int)Lights.Bar2)
                {
                    PM.Bar[i - (int)Lights.Bar1].Type = (int)BarType.Full;
                }
            }
        }
    }
}