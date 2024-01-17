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
        Kill();
        SetColor();
        foreach (Lights light in Enum.GetValues(typeof(Lights)))
        {
            if (Switch[(int)light].Value)
            {
                PM.SetBrightness(light, 1);
            }
        }
        Executing = true;
    }

    public override void Stop()
    {
        Executing = false;
        Kill();
    }

    public override void SetColor()
    {
        double r = (Color.Value.R / 255f);
        double g = (Color.Value.G / 255f);
        double b = (Color.Value.B / 255f);
        foreach (Lights light in Enum.GetValues(typeof(Lights)))
        {
            if (Switch[(int)light].Value)
            {
                PM.SetColor(light, r, g, b);
            }
        }
    }
    
    public override void ResetBarType(int index)
    {
        if (index >= (int)Lights.Bar1 && index <= (int)Lights.Bar2)
        {
            PM.Bar[index - (int)Lights.Bar1].Type = (int)BarType.Full;
        }
    }
}