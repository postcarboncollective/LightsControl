using MudBlazor.Utilities;

namespace LightsControl;

public class FunctionBlackout : Function
{
    protected override void Start()
    {
        ResetBarType();
        Kill();
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