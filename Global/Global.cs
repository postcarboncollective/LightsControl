namespace LightsControl;

public enum LightType
{
    None = 0,
    Full = 1,
    Split = 2,
    Fill = 3,
    iFill = 4,
}

public enum SwitchType
{
    None = 0,
    Random = 1,
    Sequential = 2,
}

public static class Global
{
    public static FunctionBlackout Blackout = new();
    public static FunctionExpulsadeira Expulsadeira = new();
    public static FunctionStrobe Strobe = new();
    public static FunctionOscillator Oscillator = new();
    public static FunctionSwitchStrobe SwitchStrobe = new();
    public static FunctionSwitchOscillator SwitchOscillator = new();
    public static List<Function> Functions = [Blackout, Expulsadeira, Strobe, Oscillator, SwitchStrobe, SwitchOscillator];
    public static Random Rand = new Random();

    public static void StopAllFunctions()
    {
        foreach (var f in Functions)
        {
            f.Stop();
        }
        
    }

    public static void SetLeds(bool[] a1, bool[] a2, LedFunction function, double r, double g, double b, byte p1, byte p2)
    {
        Arduino.Write(Arduino.CreateByte(a1), Arduino.CreateByte(a2), (byte)function, (byte)(r*254), (byte)(g*254), (byte)(b*254), p1, p2);
    }
}