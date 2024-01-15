using MudBlazor.Utilities;

namespace LightsControl;

public class FunctionBlackout : Function
{
    protected override void Start()
    {
        Kill();
    }
}