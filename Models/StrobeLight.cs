namespace LightsControl;

public class StrobeLight
{
    public int Red { get; set; } = 1;
    public int Green { get; set; } = 2;
    public int Blue { get; set; } = 3;
    public int White { get; set; } = 4;
    public int Brightness { get; set; } = 5;
    public int Strobe { get; set; } = 6;
    public int Switch { get; set; } = 7;
    public int Speed { get; set; } = 8;

    public StrobeLight(int addr)
    {
        Red = addr;
        Green = addr + 1;
        Blue = addr + 2;
        White = addr + 3;
        Brightness = addr + 4;
        Strobe = addr + 5;
        Switch = addr + 6;
        Speed = addr + 7;
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