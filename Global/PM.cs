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

    public static void KillLight(Lights index)
    {
        Lights[(int)index].SetBrightness(0);
    }

    public static void SetColor(Lights index, double r, double g, double b)
    {
        Lights[(int)index].SetColor(r, g, b);
    }

    public static void SetBrightness(Lights index, double brightness)
    {
        Lights[(int)index].SetBrightness(brightness);
    }
}