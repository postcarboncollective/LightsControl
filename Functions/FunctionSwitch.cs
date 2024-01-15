using System.Timers;
using MudBlazor.Utilities;

namespace LightsControl;

public class FunctionSwitch : Function
{
    public System.Timers.Timer Timer = new();
    public MudColor Color = new MudColor(255, 255, 255, 255);
    public Lights Index = Lights.Bar1;
    Random rand = new Random();

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

    double speed = 0.5f;
    public double Speed
    {
        get => speed;
        set
        {
            speed = value;
            Timer.Interval = ((1 - speed) / 2) * 1000;
        }
    }

    public FunctionSwitch()
    {
        Speed = speed;
        Timer.Elapsed += Execute;
    }

    protected override void Start()
    {
        Kill();
        Executing = true;
    }

    public override void Stop()
    {
        Executing = false;
        Kill();
    }

    public void Execute(object? sender, ElapsedEventArgs args)
    {
        double r = (Color.R / 255f);
        double g = (Color.G / 255f);
        double b = (Color.B / 255f);
        PM.KillLight(Index);
        List<Lights> possibleLights = new();
        foreach (var x in Switch)
        {
            if (x.Value)
            {
                if (x.Index != Index)
                {
                    possibleLights.Add(x.Index);
                }
            }
        }
        if (possibleLights.Count > 0)
        {
            Index = possibleLights[rand.Next(possibleLights.Count)];
            PM.SetLight(Index, r, g, b);
        }
    }
}