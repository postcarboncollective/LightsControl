namespace LightsControl;

public static class PM
{
    public static StrobeLight Strobe = new(1);
    public static ParLight Par = new(9);
    public static List<BarLight> Bar = new() { new BarLight(17), new BarLight(65) };
    public static List<object> Lights = new() { Strobe, Par, Bar };
}