using MudBlazor.Utilities;

namespace LightsControl;

public class FunctionBlackout : Function
{
    public override void ResetType(int index)
    {
        if (index >= (int)Lights.Bar1 && index <= (int)Lights.Bar2)
        {
            PM.Bar[index - (int)Lights.Bar1].Type = (int)BarType.Full;
        }
    }
}