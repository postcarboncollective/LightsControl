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
        R = (Color.Value.R / 255f);
        G = (Color.Value.G / 255f);
        B = (Color.Value.B / 255f);
        for (int i = 0; i < PM.Lights.Count; i++)
        {
            if (Switch[i].Value)
            {
                PM.Lights[i].SetColor(R, G, B);
            }
        }
    }
}