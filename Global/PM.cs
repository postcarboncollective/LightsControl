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

    public static void KillLight(Lights index)
    {
        switch (index)
        {
            case Lights.Strobe:
                Strobe.Set(0, 0, 0, 0, 0, 0, 0, 0);
                break;
            case Lights.Par:
                Par.Set(0, 0, 0, 0);
                break;
            case Lights.Bar1:
                Bar[0].Set(0, 0, 0, 0, 0, 0);
                break;
            case Lights.Bar2:
                Bar[1].Set(0, 0, 0, 0, 0, 0);
                break;
        }
    }

    public static void SetLight(Lights index, double r, double g, double b)
    {
        switch (index)
        {
            case Lights.Strobe:
                Strobe.Set(r, g, b, 0, 1, 0, 0, 0);
                break;
            case Lights.Par:
                Par.Set((r + g + b) / 3, 0, 1, 0);
                break;
            case Lights.Bar1:
                Bar[0].Set(r, g, b, 1, 0, 0);
                break;
            case Lights.Bar2:
                Bar[1].Set(r, g, b, 1, 0, 0);
                break;
        }
    }
}