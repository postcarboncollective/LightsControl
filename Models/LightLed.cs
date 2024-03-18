namespace LightsControl;

public enum LedType
{
    None = 0,
    Full = 1,
    Split = 2,
    Fill = 3,
    iFill = 4,
}

public enum LedFunction : byte
{
    Off = 0,
    Full = 1,
    Set = 2,
    Fill = 3,
    iFill = 4,
    Init = 200,
}

public class LightLed : Light
{
    public int Type = 1;
    public int Address;
    public byte Size;
    public double Red;
    public double Green;
    public double Blue;
    public double Brightness;
    byte bits1; 
    byte bits2;

    public LightLed(int addr, byte size)
    {
        Address = addr;
        Size = size;
        Red = 0;
        Green = 0;
        Blue = 0;
        Brightness = 1;
        bool[] b1 = new bool[] { false, false, false, false, false, false, false, false};
        bool[] b2 = new bool[] { false, false, false, false, false, false, false, false};
        if (Address < 8) b1[Address] = true;
        else if (Address < 16) b2[Address - 8] = true;
        bits1 = Arduino.CreateByte(b1);
        bits2 = Arduino.CreateByte(b2);
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
                Arduino.Write(bits1, bits2, (byte)LedFunction.Full, (byte)Math.Floor((Red*Brightness)*200), (byte)Math.Floor((Green*Brightness)*200), (byte)Math.Floor((Blue*Brightness)*200), 0, 0);
                break;
            case (int)LedType.Split:
                byte valSplit = (byte)(Brightness * Size);
                Arduino.Write(bits1, bits2, (byte)LedFunction.Set, (byte)Math.Floor((Red)*200), (byte)Math.Floor((Green)*200), (byte)Math.Floor((Blue)*200), valSplit, 1);
                break;
            case (int)LedType.Fill:
                byte valFill = (byte)(Brightness * Size);
                Arduino.Write(bits1, bits2, (byte)LedFunction.Fill, (byte)Math.Floor((Red)*200), (byte)Math.Floor((Green)*200), (byte)Math.Floor((Blue)*200), valFill, 0);
                break;
            case (int)LedType.iFill:
                byte valiFill = (byte)(Brightness * Size);
                Arduino.Write(bits1, bits2, (byte)LedFunction.iFill, (byte)Math.Floor((Red)*200), (byte)Math.Floor((Green)*200), (byte)Math.Floor((Blue)*200), valiFill, 0);
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
                Arduino.Write(bits1, bits2, (byte)LedFunction.Full, (byte)Math.Floor((Red*Brightness)*200), (byte)Math.Floor((Green*Brightness)*200), (byte)Math.Floor((Blue*Brightness)*200), 0, 0);
                break;
            case (int)LedType.Split:
                byte valSplit = (byte)(Brightness * Size);
                Arduino.Write(bits1, bits2, (byte)LedFunction.Set, (byte)Math.Floor((Red)*200), (byte)Math.Floor((Green)*200), (byte)Math.Floor((Blue)*200), valSplit, 1);
                break;
            case (int)LedType.Fill:
                byte valFill = (byte)(Brightness * Size);
                Arduino.Write(bits1, bits2, (byte)LedFunction.Fill, (byte)Math.Floor((Red)*200), (byte)Math.Floor((Green)*200), (byte)Math.Floor((Blue)*200), valFill, 0);
                break;
            case (int)LedType.iFill:
                byte valiFill = (byte)(Brightness * Size);
                Arduino.Write(bits1, bits2, (byte)LedFunction.iFill, (byte)Math.Floor((Red)*200), (byte)Math.Floor((Green)*200), (byte)Math.Floor((Blue)*200), valiFill, 0);
                break;
        }
    }
}
