namespace LightsControl;

public enum Lights : byte
{
    Strobe = 0,
    Par = 1,
    Bar1 = 2,
    Bar2 = 3,
    Led1 = 4,
    Led2 = 5,
    Led3 = 6,
    Led4 = 7,
    Led5 = 8,
    Led6 = 9,
    Led7 = 10,
    Led8 = 11,
}

public static class PM
{
    public static LightStrobe Strobe = new(1);
    public static LightPar Par = new(9);
    public static List<LightBar> Bar = new()
    {
        new LightBar(17), 
        new LightBar(65)
    };
    public static List<LightLed> Led = new()
    {
        new LightLed(0, 90), 
        new LightLed(1, 90), 
        new LightLed(2, 90), 
        new LightLed(3, 90),
        new LightLed(4, 90),
        new LightLed(5, 90),
        new LightLed(6, 90),
        new LightLed(7, 90),
    };
    public static List<Light> Lights = [Strobe, Par, Bar[0], Bar[1], Led[0], Led[1], Led[2], Led[3], Led[4], Led[5], Led[6], Led[7]];
}