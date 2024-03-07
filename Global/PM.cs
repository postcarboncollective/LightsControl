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
}

public static class PM
{
    public static LightStrobe Strobe = new(1);
    public static LightPar Par = new(9);
    public static List<LightBar> Bar = new() { new LightBar(17), new LightBar(65) };
    public static List<LightLed> Led = new() { new LightLed(1, 100), new LightLed(2, 100), new LightLed(3, 100), new LightLed(4, 100)};
    public static List<Light> Lights = [Strobe, Par, Bar[0], Bar[1], Led[0], Led[1], Led[2], Led[3]];
}