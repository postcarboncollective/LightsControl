namespace LightsControl;

public class ParLight
{
    public DmxChannel Brightness;
    public DmxChannel Strobe;
    public DmxChannel UV;
    public DmxChannel Audio;

    public ParLight(int addr)
    {
        Brightness = new DmxChannel(addr, 0);
        Strobe = new DmxChannel(addr + 1, 0);
        UV = new DmxChannel(addr + 2, 0);
        Audio = new DmxChannel(addr + 3, 0);
    }

    public void Set(float brightness, float strobe, float uv, float audio)
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