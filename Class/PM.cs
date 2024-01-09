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
    public static StrobeLight Strobe = new(1);
    public static ParLight Par = new(9);
    public static List<BarLight> Bar = new() { new BarLight(17), new BarLight(65) };
    public static List<object> Lights = new() { Strobe, Par, Bar };
}