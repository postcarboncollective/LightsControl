namespace LightsControl;

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
    private byte bits1;
    private byte bits2;
    public byte SplitSize = 1;

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
            case (int)LightType.Full:
                Arduino.Write(bits1, bits2, (byte)LedFunction.Full, (byte)(Red*Brightness*254), (byte)(Green*Brightness*254), (byte)(Blue*Brightness*254), 0, 0);
                break;
            case (int)LightType.Split:
                byte valSplit = (byte)(Brightness * 200);
                Arduino.Write(bits1, bits2, (byte)LedFunction.Set, (byte)(Red*254), (byte)(Green*254), (byte)(Blue*254), valSplit, SplitSize);
                break;
            case (int)LightType.Fill:
                byte valFill = (byte)(Brightness * 200);
                Arduino.Write(bits1, bits2, (byte)LedFunction.Fill, (byte)(Red*254), (byte)(Green*254), (byte)(Blue*254), valFill, 0);
                break;
            case (int)LightType.iFill:
                byte valiFill = (byte)(Brightness * 200);
                Arduino.Write(bits1, bits2, (byte)LedFunction.iFill, (byte)(Red*254), (byte)(Green*254), (byte)(Blue*254), valiFill, 0);
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
            case (int)LightType.Full:
                Arduino.Write(bits1, bits2, (byte)LedFunction.Full, (byte)(Red*Brightness*254), (byte)(Green*Brightness*254), (byte)(Blue*Brightness*254), 0, 0);
                break;
            case (int)LightType.Split:
                byte valSplit = (byte)(Brightness * 200);
                Arduino.Write(bits1, bits2, (byte)LedFunction.Set, (byte)(Red*254), (byte)(Green*254), (byte)(Blue*254), valSplit, SplitSize);
                break;
            case (int)LightType.Fill:
                byte valFill = (byte)(Brightness * 200);
                Arduino.Write(bits1, bits2, (byte)LedFunction.Fill, (byte)(Red*254), (byte)(Green*254), (byte)(Blue*254), valFill, 0);
                break;
            case (int)LightType.iFill:
                byte valiFill = (byte)(Brightness * 200);
                Arduino.Write(bits1, bits2, (byte)LedFunction.iFill, (byte)(Red*254), (byte)(Green*254), (byte)(Blue*254), valiFill, 0);
                break;
        }
    }
    
    public override void Kill()
    {
        Arduino.Write(bits1, bits2, (byte)LedFunction.Off, 0, 0, 0, 0, 0);
    }
}
