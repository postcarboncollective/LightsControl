namespace LightsControl;

public enum Lights : byte
{
    Strobe = 0,
    Par = 1,
    Bar1 = 2,
    Bar2 = 3,
}

public static class PM
{
    public static LightStrobe Strobe = new(1);
    public static LightPar Par = new(9);
    public static List<LightBar> Bar = new() { new LightBar(17), new LightBar(65) };
    public static List<Light> Lights = [Strobe, Par, Bar[0], Bar[1]];
}