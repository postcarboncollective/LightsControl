namespace LightsControl;

public class LightStrobe
{
    public Dmx Red;
    public Dmx Green;
    public Dmx Blue;
    public Dmx White;
    public Dmx Brightness;
    public Dmx Strobe;
    public Dmx Switch;
    public Dmx Speed;

    public LightStrobe(int addr)
    {
        Red = new(addr, 0);
        Green = new(addr + 1, 0);
        Blue = new(addr + 2, 0);
        White = new(addr + 3, 0);
        Brightness = new(addr + 4, 0);
        Strobe = new(addr + 5, 0);
        Switch = new(addr + 6, 0);
        Speed = new(addr + 7, 0);
    }

    public void Set(double r, double g, double b, double white, double brightness, double strobe, double swtch, double speed)
    {
        Red.Value = r;
        Green.Value = g;
        Blue.Value = b;
        White.Value = white;
        Brightness.Value = brightness;
        Strobe.Value = strobe;
        Switch.Value = swtch;
        Speed.Value = speed;
    }
}

// Strobe 1-8
// 1 -   Red
// 2 -   Green
// 3 -   Blue
// 4 -   White
// 5 -   Brightness
// 6 -   Strobe
// 7 -   Sine Switch
// 8 -   Sine Speed