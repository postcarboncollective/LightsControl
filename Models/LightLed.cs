namespace LightsControl;

public enum LedType
{
    None = 0,
    Full = 1,
    Split = 2,
    Fill = 3,
    iFill = 4,
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
                Arduino.Write($"{Address}|Fill|{(int)Math.Floor((Red*Brightness)*255)}|{(int)Math.Floor((Green*Brightness)*255)}|{(int)Math.Floor((Blue*Brightness)*255)}");
                break;
            case (int)LedType.Split:
                float val = (float)(Brightness * Size);
                Arduino.Write($"{Address}|Set|{(int)Math.Floor((Red)*255)}|{(int)Math.Floor((Green)*255)}|{(int)Math.Floor((Blue)*255)}|{val}|1");
                break;
            case (int)LedType.Fill:
                // for (int i = 0; i < Brightness.Count; i++)
                // {
                //     int index = i;
                //     float div = 1f / Brightness.Count;
                //     float next = (i + 1) * div;
                //     float val = i * div;
                //     if (brightness > next) Brightness[index].Value = 1;
                //     else if (brightness > val) Brightness[index].Value = (brightness - val) * Brightness.Count;
                //     else Brightness[index].Value = 0;
                // }
                break;
            case (int)LedType.iFill:
                // for (int i = 0; i < Brightness.Count; i++)
                // {
                //     int index = (Brightness.Count - 1) - i;
                //     float div = 1f / Brightness.Count;
                //     float next = (i + 1) * div;
                //     float val = i * div;
                //     if (brightness > next) Brightness[index].Value = 1;
                //     else if (brightness > val) Brightness[index].Value = (brightness - val) * Brightness.Count;
                //     else Brightness[index].Value = 0;
                // }
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
                Arduino.Write($"{Address}|Fill|{(int)Math.Floor((Red*Brightness)*255)}|{(int)Math.Floor((Green*Brightness)*255)}|{(int)Math.Floor((Blue*Brightness)*255)}");
                break;
            case (int)LedType.Split:
                float val = (float)(Brightness * Size);
                Arduino.Write($"{Address}|Set|{(int)Math.Floor((Red)*255)}|{(int)Math.Floor((Green)*255)}|{(int)Math.Floor((Blue)*255)}|{val}|1");
                break;
            case (int)LedType.Fill:
                // for (int i = 0; i < Brightness.Count; i++)
                // {
                //     int index = i;
                //     float div = 1f / Brightness.Count;
                //     float next = (i + 1) * div;
                //     float val = i * div;
                //     if (brightness > next) Brightness[index].Value = 1;
                //     else if (brightness > val) Brightness[index].Value = (brightness - val) * Brightness.Count;
                //     else Brightness[index].Value = 0;
                // }
                break;
            case (int)LedType.iFill:
                // for (int i = 0; i < Brightness.Count; i++)
                // {
                //     int index = (Brightness.Count - 1) - i;
                //     float div = 1f / Brightness.Count;
                //     float next = (i + 1) * div;
                //     float val = i * div;
                //     if (brightness > next) Brightness[index].Value = 1;
                //     else if (brightness > val) Brightness[index].Value = (brightness - val) * Brightness.Count;
                //     else Brightness[index].Value = 0;
                // }
                break;
        }
    }
}
