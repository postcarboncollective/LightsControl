using MudBlazor.Utilities;

namespace LightsControl;

public static class Global
{
    public static FunctionBlackout Blackout = new();
    public static FunctionExpulsadeira Expulsadeira = new();
    public static FunctionStrobe Strobe = new();
    public static FunctionOscillator Oscillator = new();
    public static List<Function> Functions = [Blackout, Expulsadeira, Strobe, Oscillator];

    public static void StopAllFunctions()
    {
        foreach (var f in Functions)
        {
            f.Stop();
        }
    }
}