using MudBlazor.Utilities;

namespace LightsControl;

public class FunctionBlackout : Function
{
    protected override void Start()
    {
        Kill();
    }

    public override void ResetBarType(int index)
    {
        if (index >= (int)Lights.Bar1 && index <= (int)Lights.Bar2)
        {
            PM.Bar[index - (int)Lights.Bar1].Type = (int)BarType.Full;
        }
    }
}