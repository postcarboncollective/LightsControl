namespace LightsControl;

public class ParLight
{
    public int Brightness { get; set; } = 1;
    public int Strobe { get; set; } = 2;
    public int UV { get; set; } = 3;
    public int Audio { get; set; } = 4;

    public ParLight(int addr)
    {
        Brightness = addr;
        Strobe = addr + 1;
        UV = addr + 2;
        Audio = addr + 3;
    }
}

// Par 9-12
// 9 -   Brightness
// 10 -  Strobe
// 11 -  UltraViolet
// 12 -  Audio Reactive