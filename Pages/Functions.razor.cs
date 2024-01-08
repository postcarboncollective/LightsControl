namespace LightsControl.Pages;

public partial class Functions
{
    public void Blackout()
    {
        Osc.SendDmx(Enumerable.Range(1, 512).ToList(), 0);
    }

    public void Expulsadeira()
    {
        Osc.SendDmx(PM.Strobe.Strobe, 0);
        Osc.SendDmx(Enumerable.Range(1, 512).ToList(), 100);
    }
}