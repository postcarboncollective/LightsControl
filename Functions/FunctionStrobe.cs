using System.Timers;
using MudBlazor.Utilities;

namespace LightsControl;

public class FunctionStrobe : Function
{
    public System.Timers.Timer Timer = new();
    public MudColor Color = new MudColor(255, 255, 255, 255);
    public List<bool> Inverted = new List<bool>();
    public bool State = false;

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

    public FunctionStrobe()
    {
        for (int i = 0; i < Enum.GetNames(typeof(Lights)).Length; i++) Inverted.Add(false);
        Speed = speed;
        Timer.Elapsed += Execute;
    }

    protected override void Start()
    {
        Executing = true;
    }

    public override void Stop()
    {
        Executing = false;
        Kill();
    }

    public void Execute(object? sender, ElapsedEventArgs args)
    {
        if (State) Off();
        else On();
    }

    void On()
    {
        State = true;
        double r = (Color.R / 255f);
        double g = (Color.G / 255f);
        double b = (Color.B / 255f);
        if (Switch[(int)Lights.Strobe].Value)
        {
            if (Inverted[(int)Lights.Strobe]) PM.Strobe.Set(0, 0, 0, 0, 0, 0, 0, 0);
            else PM.Strobe.Set(r, g, b, 0, 1, 0, 0, 0);
        }
        if (Switch[(int)Lights.Par].Value)
        {
            if (Inverted[(int)Lights.Par]) PM.Par.Set(0, 0, 0, 0);
            else PM.Par.Set((r + g + b) / 3, 0, 1, 0);
        }
        if (Switch[(int)Lights.Bar1].Value)
        {
            if (Inverted[(int)Lights.Bar1]) PM.Bar[0].Set(0, 0, 0, 0, 0, 0);
            else PM.Bar[0].Set(r, g, b, 1, 0, 0);
        }
        if (Switch[(int)Lights.Bar2].Value)
        {
            if (Inverted[(int)Lights.Bar2]) PM.Bar[1].Set(0, 0, 0, 0, 0, 0);
            else PM.Bar[1].Set(r, g, b, 1, 0, 0);
        }
    }

    void Off()
    {
        State = false;
        double r = (Color.R / 255f);
        double g = (Color.G / 255f);
        double b = (Color.B / 255f);
        if (Switch[(int)Lights.Strobe].Value)
        {
            if (!Inverted[(int)Lights.Strobe]) PM.Strobe.Set(0, 0, 0, 0, 0, 0, 0, 0);
            else PM.Strobe.Set(r, g, b, 0, 1, 0, 0, 0);
        }
        if (Switch[(int)Lights.Par].Value)
        {
            if (!Inverted[(int)Lights.Par]) PM.Par.Set(0, 0, 0, 0);
            else PM.Par.Set((r + g + b) / 3, 0, (r + g + b) / 3, 0);
        }
        if (Switch[(int)Lights.Bar1].Value)
        {
            if (!Inverted[(int)Lights.Bar1]) PM.Bar[0].Set(0, 0, 0, 0, 0, 0);
            else PM.Bar[0].Set(r, g, b, 1, 0, 0);
        }
        if (Switch[(int)Lights.Bar2].Value)
        {
            if (!Inverted[(int)Lights.Bar2]) PM.Bar[1].Set(0, 0, 0, 0, 0, 0);
            else PM.Bar[1].Set(r, g, b, 1, 0, 0);
        }
    }

    public override void Kill()
    {
        base.Kill();
        State = false;
    }
}