using MudBlazor;

namespace LightsControl;

public class LightPar : Light
{
    public Dmx Brightness;
    public Dmx Strobe;
    public Dmx UV;
    public Dmx Audio;

    public LightPar(int addr)
    {
        Brightness = new Dmx(addr, 0);
        Strobe = new Dmx(addr + 1, 0);
        UV = new Dmx(addr + 2, 0);
        Audio = new Dmx(addr + 3, 0);
    }

    public override void Set(double r, double g, double b, double brightness)
    {
        Brightness.Value = brightness;
        UV.Value = (r + g + b) / 3;
    }

    public override void SetColor(double r, double g, double b)
    {
        UV.Value = (r + g + b) / 3;
    }

    public override void SetBrightness(double brightness)
    {
        Brightness.Value = brightness;
    }
}

// Par 9-12
// 9 -   Brightness
// 10 -  Strobe
// 11 -  UltraViolet
// 12 -  Audio Reactive