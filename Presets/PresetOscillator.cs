using Microsoft.Extensions.Logging.Console;
using MudBlazor.Utilities;

namespace LightsControl;

public class PresetOscillator : Preset
{
    public SyncColor Color = new SyncColor(new MudColor(255, 255, 255, 255));
    public SyncDouble Speed = new SyncDouble(0.125f);
    public SyncInt Type = new SyncInt(1);
    public List<SyncBool> Inverted = new List<SyncBool>();
    public SyncBool Executing = new SyncBool(false);
    static CancellationTokenSource Token = new();
    double time = 0;
    double val = 0;
    double inv = 1;

    public PresetOscillator()
    {
        for (int i = 0; i < Enum.GetNames(typeof(Lights)).Length; i++) Inverted.Add(new SyncBool(false));
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
                    inv = (Math.Sin((1 - time) * (Math.PI * 2)) / 2) + 0.5f;
                    break;
                case 2:
                    val = time;
                    inv = 1 - time;
                    break;
            }
            double r = (Color.Value.R / 255f);
            double g = (Color.Value.G / 255f);
            double b = (Color.Value.B / 255f);
            if (Toggle[(int)Lights.Strobe].Value)
            {
                if (Inverted[(int)Lights.Strobe].Value) PM.Strobe.Set(r * inv, g * inv, b * inv, 0, 1, 0, 0, 0);
                else PM.Strobe.Set(r * val, g * val, b * val, 0, 1, 0, 0, 0);
            }
            if (Toggle[(int)Lights.Par].Value)
            {
                if (Inverted[(int)Lights.Par].Value) PM.Par.Set(((r + g + b) * inv) / 3, 0, inv, 0);
                else PM.Par.Set(((r + g + b) * val) / 3, 0, val, 0);
            }
            if (Toggle[(int)Lights.Bar1].Value)
            {
                if (Inverted[(int)Lights.Bar1].Value) PM.Bar[0].Set(r * inv, g * inv, b * inv, 1, 0, 0);
                else PM.Bar[0].Set(r * val, g * val, b * val, 1, 0, 0);
            }
            if (Toggle[(int)Lights.Bar2].Value)
            {
                if (Inverted[(int)Lights.Bar2].Value) PM.Bar[1].Set(r * inv, g * inv, b * inv, 1, 0, 0);
                else PM.Bar[1].Set(r * val, g * val, b * val, 1, 0, 0);
            }
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