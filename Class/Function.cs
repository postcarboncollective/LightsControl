using MudBlazor.Utilities;

namespace LightsControl;

public static class Function
{
    public static void Init()
    {
        for (int i = 0; i < Enum.GetNames(typeof(Lights)).Length; i++)
        {
            BlackoutToggle.Add(new SyncBool(true));
            ExpulsadeiraToggle.Add(new SyncBool(true));
            StrobeToggle.Add(new SyncBool(true));
        }
    }

    public static List<SyncBool> BlackoutToggle = new List<SyncBool>();

    public static void Blackout()
    {
        if (BlackoutToggle[(int)Lights.Strobe].Value) PM.Strobe.Set(0, 0, 0, 0, 0, 0, 0, 0);
        if (BlackoutToggle[(int)Lights.Par].Value) PM.Par.Set(0, 0, 0, 0);
        if (BlackoutToggle[(int)Lights.Bar1].Value) PM.Bar[0].Set(0, 0, 0, 0, 0, 0);
        if (BlackoutToggle[(int)Lights.Bar2].Value) PM.Bar[1].Set(0, 0, 0, 0, 0, 0);
    }

    public static List<SyncBool> ExpulsadeiraToggle = new List<SyncBool>();
    public static SyncColor ExpulsadeiraColor = new SyncColor(new MudColor(255, 255, 255, 255));

    public static void Expulsadeira()
    {
        float r = (ExpulsadeiraColor.Value.R / 255f) * 100;
        float g = (ExpulsadeiraColor.Value.G / 255f) * 100;
        float b = (ExpulsadeiraColor.Value.B / 255f) * 100;
        if (ExpulsadeiraToggle[(int)Lights.Strobe].Value) PM.Strobe.Set(r, g, b, 0, 100, 0, 0, 0);
        if (ExpulsadeiraToggle[(int)Lights.Par].Value) PM.Par.Set(100, 0, 100, 0);
        if (ExpulsadeiraToggle[(int)Lights.Bar1].Value) PM.Bar[0].Set(r, g, b, 100, 0, 0);
        if (ExpulsadeiraToggle[(int)Lights.Bar2].Value) PM.Bar[1].Set(r, g, b, 100, 0, 0);
    }

    public static List<SyncBool> StrobeToggle = new List<SyncBool>();
    public static SyncColor StrobeColor = new SyncColor(new MudColor(255, 255, 255, 255));
    public static SyncFloat StrobeSpeed = new SyncFloat(0.125f);
    public static SyncBool StrobeExecuting = new SyncBool(false);
    static CancellationTokenSource StrobeToken = new();

    public static void StrobeCancel()
    {
        if (StrobeToken.Token.CanBeCanceled)
        {
            StrobeExecuting.Value = false;
            StrobeToken.Cancel();
            StrobeToken = new();
        }
    }

    public static void Strobe()
    {
        StrobeCancel();
        StrobeExecuting.Value = true;
        StrobeExecute().WaitAsync(StrobeToken.Token);
    }

    static async Task StrobeExecute()
    {
        while (StrobeExecuting.Value)
        {
            float r = (StrobeColor.Value.R / 255f) * 100;
            float g = (StrobeColor.Value.G / 255f) * 100;
            float b = (StrobeColor.Value.B / 255f) * 100;

            // Console.WriteLine("Strobe On");
            if (StrobeToggle[(int)Lights.Strobe].Value) PM.Strobe.Set(r, g, b, 0, 100, 0, 0, 0);
            if (StrobeToggle[(int)Lights.Par].Value) PM.Par.Set(100, 0, 100, 0);
            if (StrobeToggle[(int)Lights.Bar1].Value) PM.Bar[0].Set(r, g, b, 100, 0, 0);
            if (StrobeToggle[(int)Lights.Bar2].Value) PM.Bar[1].Set(r, g, b, 100, 0, 0);
            await Task.Delay(TimeSpan.FromSeconds(StrobeSpeed.Value / 2), StrobeToken.Token);

            // Console.WriteLine("Strobe Off");
            if (StrobeToggle[(int)Lights.Strobe].Value) PM.Strobe.Set(0, 0, 0, 0, 0, 0, 0, 0);
            if (StrobeToggle[(int)Lights.Par].Value) PM.Par.Set(0, 0, 0, 0);
            if (StrobeToggle[(int)Lights.Bar1].Value) PM.Bar[0].Set(0, 0, 0, 0, 0, 0);
            if (StrobeToggle[(int)Lights.Bar2].Value) PM.Bar[1].Set(0, 0, 0, 0, 0, 0);
            await Task.Delay(TimeSpan.FromSeconds(StrobeSpeed.Value / 2), StrobeToken.Token);
        }
    }
}