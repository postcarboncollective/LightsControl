using System.Timers;
using Microsoft.Extensions.Logging.Console;
using MudBlazor.Utilities;

namespace LightsControl;

public class PresetOscillator : Preset
{
    public System.Timers.Timer Timer = new();
    public MudColor Color = new MudColor(255, 255, 255, 255);
    public double Speed = 0.125f;
    public int Type = 1;
    public List<bool> Inverted = new List<bool>();
    double time = 0;
    double val = 0;
    double inv = 1;

    bool executing = false;
    public bool Executing
    {
        get => executing;
        set
        {
            executing = value;
            if (executing) Timer.Start();
            else Timer.Stop();
        }
    }

    public PresetOscillator()
    {
        for (int i = 0; i < Enum.GetNames(typeof(Lights)).Length; i++) Inverted.Add(false);
        Timer.Interval = (1 / 32f) * 1000;
        Timer.Elapsed += Execute;
    }

    public override void Run()
    {
        Function.StopAll();
        Executing = true;
    }

    public override void Stop()
    {
        time = 0;
        Executing = false;
        Kill();
    }

    public void Execute(object? sender, ElapsedEventArgs args)
    {
        time += (Speed / 2);
        time %= 1f;
        switch (Type)
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
        double r = (Color.R / 255f);
        double g = (Color.G / 255f);
        double b = (Color.B / 255f);
        if (Toggle[(int)Lights.Strobe])
        {
            if (Inverted[(int)Lights.Strobe]) PM.Strobe.Set(r * inv, g * inv, b * inv, 0, 1, 0, 0, 0);
            else PM.Strobe.Set(r * val, g * val, b * val, 0, 1, 0, 0, 0);
        }
        if (Toggle[(int)Lights.Par])
        {
            if (Inverted[(int)Lights.Par]) PM.Par.Set(((r + g + b) * inv) / 3, 0, inv, 0);
            else PM.Par.Set(((r + g + b) * val) / 3, 0, val, 0);
        }
        if (Toggle[(int)Lights.Bar1])
        {
            if (Inverted[(int)Lights.Bar1]) PM.Bar[0].Set(r * inv, g * inv, b * inv, 1, 0, 0);
            else PM.Bar[0].Set(r * val, g * val, b * val, 1, 0, 0);
        }
        if (Toggle[(int)Lights.Bar2])
        {
            if (Inverted[(int)Lights.Bar2]) PM.Bar[1].Set(r * inv, g * inv, b * inv, 1, 0, 0);
            else PM.Bar[1].Set(r * val, g * val, b * val, 1, 0, 0);
        }
    }

    void Kill()
    {
        if (Toggle[(int)Lights.Strobe]) PM.Strobe.Set(0, 0, 0, 0, 0, 0, 0, 0);
        if (Toggle[(int)Lights.Par]) PM.Par.Set(0, 0, 0, 0);
        if (Toggle[(int)Lights.Bar1]) PM.Bar[0].Set(0, 0, 0, 0, 0, 0);
        if (Toggle[(int)Lights.Bar2]) PM.Bar[1].Set(0, 0, 0, 0, 0, 0);
    }
}