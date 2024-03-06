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
        for (int i = 0; i < PM.Lights.Count; i++)
        {
            if (Switch[i].Value)
            {
                PM.Lights[i].SetBrightness(1);
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
        for (int i = 0; i < PM.Lights.Count; i++)
        {
            if (Switch[i].Value)
            {
                PM.Lights[i].SetColor(r, g, b);
            }
        }
    }

    public override void ResetType(int index)
    {
        if (index >= (int)Lights.Bar1 && index <= (int)Lights.Bar2)
        {
            PM.Bar[index - (int)Lights.Bar1].Type = (int)BarType.Full;
        }
    }
}