namespace LightsControl;

public class LightPar
{
    public SyncDmx Brightness;
    public SyncDmx Strobe;
    public SyncDmx UV;
    public SyncDmx Audio;

    public LightPar(int addr)
    {
        Brightness = new SyncDmx(addr, 0);
        Strobe = new SyncDmx(addr + 1, 0);
        UV = new SyncDmx(addr + 2, 0);
        Audio = new SyncDmx(addr + 3, 0);
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