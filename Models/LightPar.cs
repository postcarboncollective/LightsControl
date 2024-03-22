using MudBlazor;

namespace LightsControl;

public class LightPar : Light
{
    public DmxChannel Brightness;
    public DmxChannel Strobe;
    public DmxChannel UV;
    public DmxChannel Audio;

    public LightPar(int addr)
    {
        Brightness = new DmxChannel(addr, 0);
        Strobe = new DmxChannel(addr + 1, 0);
        UV = new DmxChannel(addr + 2, 0);
        Audio = new DmxChannel(addr + 3, 0);
    }

    public override void Set(double r, double g, double b, double brightness)
    {
        Brightness.Value = brightness;
        UV.Value = brightness;
    }

    public override void SetColor(double r, double g, double b)
    {
        // UV.Value = (r + g + b) / 3;
    }

    public override void SetBrightness(double brightness)
    {
        Brightness.Value = brightness;
        UV.Value = brightness;
    }
    
    public override void Kill()
    {
        Brightness.Value = 0;
    }
}

// Par 9-12
// 9 -   Brightness
// 10 -  Strobe
// 11 -  UltraViolet
// 12 -  Audio Reactive