using Microsoft.Extensions.Logging.Console;
using MudBlazor.Utilities;

namespace LightsControl;

public class PresetOscillator : Preset
{
    public SyncColor Color = new SyncColor(new MudColor(255, 255, 255, 255));
    public SyncDouble Speed = new SyncDouble(0.125f);
    public SyncInt Type = new SyncInt(1);
    public SyncBool Executing = new SyncBool(false);
    static CancellationTokenSource Token = new();
    double time = 0;
    double val = 0;

    public PresetOscillator()
    {
    }

    public override void Run()
    {
        Function.StopAll();
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
            time = 0;
        }
    }

    public override async Task Execute()
    {
        while (Executing.Value)
        {
            time += Speed.Value;
            time %= 1f;
            switch (Type.Value)
            {
                case 1:
                    val = (Math.Sin(time * (Math.PI * 2)) / 2) + 0.5f;
                    break;
                case 2:
                    val = time;
                    break;
            }
            double r = (Color.Value.R / 255f) * val;
            double g = (Color.Value.G / 255f) * val;
            double b = (Color.Value.B / 255f) * val;
            if (Toggle[(int)Lights.Strobe].Value) PM.Strobe.Set(r, g, b, 0, 1, 0, 0, 0);
            if (Toggle[(int)Lights.Par].Value) PM.Par.Set((r + g + b) / 3, 0, val, 0);
            if (Toggle[(int)Lights.Bar1].Value) PM.Bar[0].Set(r, g, b, 1, 0, 0);
            if (Toggle[(int)Lights.Bar2].Value) PM.Bar[1].Set(r, g, b, 1, 0, 0);
            await Task.Delay(TimeSpan.FromSeconds(0.01f), Token.Token);
        }
        Kill();
    }

    void Kill()
    {
        if (Toggle[(int)Lights.Strobe].Value) PM.Strobe.Set(0, 0, 0, 0, 0, 0, 0, 0);
        if (Toggle[(int)Lights.Par].Value) PM.Par.Set(0, 0, 0, 0);
        if (Toggle[(int)Lights.Bar1].Value) PM.Bar[0].Set(0, 0, 0, 0, 0, 0);
        if (Toggle[(int)Lights.Bar2].Value) PM.Bar[1].Set(0, 0, 0, 0, 0, 0);
    }
}