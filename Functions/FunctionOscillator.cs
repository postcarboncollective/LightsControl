using System.Timers;
using Microsoft.Extensions.Logging.Console;
using MudBlazor.Utilities;

namespace LightsControl;

public class FunctionOscillator : Function
{
    public System.Timers.Timer Timer = new();
    public ColorFunction Color;
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

    public double AudioMax = 0.5f;
    public bool AudioEnabled = false;

    public FunctionOscillator()
    {
        for (int i = 0; i < Enum.GetNames(typeof(Lights)).Length; i++) Inverted.Add(false);
        Color = new(new MudColor(255, 255, 255, 255), this);
        Timer.Interval = (1 / 32f) * 1000;
        Timer.Elapsed += Execute;
    }

    protected override void Start()
    {
        Kill();
        SetColor();
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
        if (AudioEnabled)
        {
            time = Audio.Read / AudioMax;
            if (time > 1) time = 1;
        }
        else
        {
            time += (Speed / 2);
            time %= 1f;
        }
        switch (Type)
        {
            case 1:
                val = time;
                inv = 1 - time;
                break;
            case 2:
                val = 1 - ((Math.Cos(time * (Math.PI * 2)) / 2) + 0.5f);
                inv = 1 - ((Math.Cos((1 - time) * (Math.PI * 2)) / 2) + 0.5f);
                break;
        }
        foreach (Lights light in Enum.GetValues(typeof(Lights)))
        {
            if (Switch[(int)light].Value)
            {
                if (Inverted[(int)light]) PM.SetBrightness(light, inv);
                else PM.SetBrightness(light, val);
            }
        }
    }

    public override void SetColor()
    {
        double r = (Color.Value.R / 255f);
        double g = (Color.Value.G / 255f);
        double b = (Color.Value.B / 255f);
        foreach (Lights light in Enum.GetValues(typeof(Lights)))
        {
            if (Switch[(int)light].Value)
            {
                PM.SetColor(light, r, g, b);
            }
        }
    }
}