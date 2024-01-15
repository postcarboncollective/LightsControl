namespace LightsControl;

public class LightPar
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

    public void Set(double brightness, double strobe, double uv, double audio)
    {
        Brightness.Value = brightness;
        Strobe.Value = strobe;
        UV.Value = uv;
        Audio.Value = audio;
    }
}

// Par 9-12
// 9 -   Brightness
// 10 -  Strobe
// 11 -  UltraViolet
// 12 -  Audio Reactive