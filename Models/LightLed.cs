namespace LightsControl;

public enum LedType
{
    None = 0,
    Full = 1,
    Split = 2,
    Fill = 3,
    iFill = 4,
}

public enum LedFunction
{
    Off = 0,
    Full = 1,
    Set = 2,
    Fill = 3,
    iFill = 4
}

public class LightLed : Light
{
    public int Type = 1;
    public int Address;
    public int Size;
    public double Red;
    public double Green;
    public double Blue;
    public double Brightness;

    public LightLed(int addr, int size)
    {
        Address = addr;
        Size = size;
        Red = 0;
        Green = 0;
        Blue = 0;
        Brightness = 1;
    }
    
    public override void Set(double r, double g, double b, double brightness)
    {
        Red = r;
        Green = g;
        Blue = b;
        Brightness = brightness;
        switch (Type)
        {
            case (int)LedType.Full:
                Arduino.Write($"{Address}|Full|{(int)Math.Floor((Red*Brightness)*255)}|{(int)Math.Floor((Green*Brightness)*255)}|{(int)Math.Floor((Blue*Brightness)*255)}");
                break;
            case (int)LedType.Split:
                int valSplit = (int)(Brightness * Size);
                Arduino.Write($"{Address}|Set|{(int)Math.Floor((Red)*255)}|{(int)Math.Floor((Green)*255)}|{(int)Math.Floor((Blue)*255)}|{valSplit}|1");
                break;
            case (int)LedType.Fill:
                int valFill = (int)(Brightness * Size);
                Arduino.Write($"{Address}|Fill|{(int)Math.Floor((Red)*255)}|{(int)Math.Floor((Green)*255)}|{(int)Math.Floor((Blue)*255)}|{valFill}");
                break;
            case (int)LedType.iFill:
                int valiFill = (int)(Brightness * Size);
                Arduino.Write($"{Address}|iFill|{(int)Math.Floor((Red)*255)}|{(int)Math.Floor((Green)*255)}|{(int)Math.Floor((Blue)*255)}|{valiFill}");
                break;
        }
    }

    public override void SetColor(double r, double g, double b)
    {
        Red = r;
        Green = g;
        Blue = b;
        Set(Red, Green,Blue, Brightness);
    }

    public override void SetBrightness(double brightness)
    {
        Brightness = brightness;
        switch (Type)
        {
            case (int)LedType.Full:
                Arduino.Write($"{Address}|Full|{(int)Math.Floor((Red*Brightness)*255)}|{(int)Math.Floor((Green*Brightness)*255)}|{(int)Math.Floor((Blue*Brightness)*255)}");
                break;
            case (int)LedType.Split:
                int valSplit = (int)(Brightness * Size);
                Arduino.Write($"{Address}|Set|{(int)Math.Floor((Red)*255)}|{(int)Math.Floor((Green)*255)}|{(int)Math.Floor((Blue)*255)}|{valSplit}|1");
                break;
            case (int)LedType.Fill:
                int valFill = (int)(Brightness * Size);
                Arduino.Write($"{Address}|Fill|{(int)Math.Floor((Red)*255)}|{(int)Math.Floor((Green)*255)}|{(int)Math.Floor((Blue)*255)}|{valFill}");
                break;
            case (int)LedType.iFill:
                int valiFill = (int)(Brightness * Size);
                Arduino.Write($"{Address}|iFill|{(int)Math.Floor((Red)*255)}|{(int)Math.Floor((Green)*255)}|{(int)Math.Floor((Blue)*255)}|{valiFill}");
                break;
        }
    }
}
