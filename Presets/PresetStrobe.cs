using MudBlazor.Utilities;

namespace LightsControl;

public class PresetStrobe : Preset
{
    public SyncColor Color = new SyncColor(new MudColor(255, 255, 255, 255));
    public SyncDouble Speed = new SyncDouble(0.125f);
    public SyncBool Executing = new SyncBool(false);
    static CancellationTokenSource Token = new();

    public override void Run()
    {
        Stop();
        Executing.Value = true;
        Execute().WaitAsync(Token.Token);
    }

    public override void Stop()
    {
        if (Token.Token.CanBeCanceled)
        {
            Executing.Value = false;
            Token.Cancel();
            Token = new();
        }
    }

    public override async Task Execute()
    {
        while (Executing.Value)
        {
            On();
            await Task.Delay(TimeSpan.FromSeconds(Speed.Value / 2), Token.Token);
            Off();
            await Task.Delay(TimeSpan.FromSeconds(Speed.Value / 2), Token.Token);
        }
        Off();
    }

    void On()
    {
        // Console.WriteLine("Strobe On");
        float r = (Color.Value.R / 255f);
        float g = (Color.Value.G / 255f);
        float b = (Color.Value.B / 255f);
        if (Toggle[(int)Lights.Strobe].Value) PM.Strobe.Set(r, g, b, 0, 1, 0, 0, 0);
        if (Toggle[(int)Lights.Par].Value) PM.Par.Set(1, 0, 1, 0);
        if (Toggle[(int)Lights.Bar1].Value) PM.Bar[0].Set(r, g, b, 1, 0, 0);
        if (Toggle[(int)Lights.Bar2].Value) PM.Bar[1].Set(r, g, b, 1, 0, 0);
    }

    void Off()
    {
        // Console.WriteLine("Strobe Off");
        if (Toggle[(int)Lights.Strobe].Value) PM.Strobe.Set(0, 0, 0, 0, 0, 0, 0, 0);
        if (Toggle[(int)Lights.Par].Value) PM.Par.Set(0, 0, 0, 0);
        if (Toggle[(int)Lights.Bar1].Value) PM.Bar[0].Set(0, 0, 0, 0, 0, 0);
        if (Toggle[(int)Lights.Bar2].Value) PM.Bar[1].Set(0, 0, 0, 0, 0, 0);
    }
}