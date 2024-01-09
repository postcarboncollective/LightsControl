using MudBlazor.Utilities;

namespace LightsControl;

public static class Function
{
    public static PresetBlackout Blackout = new();
    public static PresetExpulsadeira Expulsadeira = new();
    public static PresetStrobe Strobe = new();

    public static void StopAll()
    {
        Blackout.Stop();
        Expulsadeira.Stop();
        Strobe.Stop();
    }
}