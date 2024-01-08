namespace LightsControl.Pages;

public partial class Functions
{
    public void Blackout()
    {
        if (BlackoutStrobe) PM.Strobe.Set(0, 0, 0, 0, 0, 0, 0, 0);
        if (BlackoutPar) PM.Par.Set(0, 0, 0, 0);
        if (BlackoutBar1) PM.Bar[0].Set(0, 0, 0, 0, 0, 0);
        if (BlackoutBar2) PM.Bar[1].Set(0, 0, 0, 0, 0, 0);
    }

    public void Expulsadeira()
    {
        float r = (ExpulsadeiraColor.R / 255f) * 100;
        float g = (ExpulsadeiraColor.G / 255f) * 100;
        float b = (ExpulsadeiraColor.B / 255f) * 100;
        if (ExpulsadeiraStrobe) PM.Strobe.Set(r, g, b, 0, 100, 0, 0, 0);
        if (ExpulsadeiraPar) PM.Par.Set(100, 0, 100, 0);
        if (ExpulsadeiraBar1) PM.Bar[0].Set(r, g, b, 100, 0, 0);
        if (ExpulsadeiraBar2) PM.Bar[1].Set(r, g, b, 100, 0, 0);
    }
}