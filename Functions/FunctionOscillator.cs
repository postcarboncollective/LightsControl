using System.Timers;
using Microsoft.Extensions.Logging.Console;
using MudBlazor.Utilities;

namespace LightsControl;

public class FunctionOscillator : Function
{
    public System.Timers.Timer Timer = new();
    public MudColor Color = new MudColor(255, 255, 255, 255);
    public double Speed = 0.125f;
    public int Type = 1;
    public List<bool> Inverted = new List<bool>();
    double time = 0;
    double val = 0;
    double inv = 1;

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

    public FunctionOscillator()
    {
        for (int i = 0; i < Enum.GetNames(typeof(Lights)).Length; i++) Inverted.Add(false);
        Timer.Interval = (1 / 32f) * 1000;
        Timer.Elapsed += Execute;
    }

    protected override void Start()
    {
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
        foreach (Lights light in Enum.GetValues(typeof(Lights)))
        {
            if (Switch[(int)light].Value)
            {
                if (Inverted[(int)light]) PM.SetLight(light, r * inv, g * inv, b * inv);
                else PM.SetLight(light, r * val, g * val, b * val);
            }
        }
    }
}