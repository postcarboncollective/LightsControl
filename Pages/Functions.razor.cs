namespace LightsControl.Pages;

public partial class Functions
{
    public void Blackout()
    {
        Osc.SendDmx(Enumerable.Range(1, 512).ToList(), 0);
    }

    public void Expulsadeira()
    {
        float r = (ExpulsadeiraColor.R / 255f) * 100;
        float g = (ExpulsadeiraColor.G / 255f) * 100;
        float b = (ExpulsadeiraColor.B / 255f) * 100;
        PM.Strobe.Set(r, g, b, 0, 100, 0, 0, 0);
        PM.Par.Set(100, 0, 100, 0);
        PM.Bar[0].Set(r, g, b, 100, 0, 0);
        PM.Bar[1].Set(r, g, b, 100, 0, 0);
    }
}