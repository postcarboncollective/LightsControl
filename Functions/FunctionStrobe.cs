using System.Timers;
using MudBlazor.Utilities;

namespace LightsControl;

public class FunctionStrobe : Function
{
    public System.Timers.Timer Timer = new();
    public MudColor Color = new MudColor(255, 255, 255, 255);
    public List<bool> Inverted = new List<bool>();
    public bool State = false;

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

    public double AudioTrigger = 0.5;
    bool triggered = false;
    bool audioEnabled = false;
    public bool AudioEnabled
    {
        get => audioEnabled;
        set
        {
            audioEnabled = value;
            if (value) Timer.Interval = (1 / 32f) * 1000;
            else Speed = speed;
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
        ResetBarType();
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
        if (AudioEnabled)
        {
            if (triggered)
            {
                if (Audio.Read < AudioTrigger) triggered = false;
            }
            else
            {
                if (Audio.Read >= AudioTrigger)
                {
                    triggered = true;
                    SwitchState();
                }
            }
        }
        else SwitchState();
    }

    void SwitchState()
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
        foreach (Lights light in Enum.GetValues(typeof(Lights)))
        {
            if (Switch[(int)light].Value)
            {
                if (Inverted[(int)light]) PM.KillLight(light);
                else
                {
                    if (light >= Lights.Bar1 && light <= Lights.Bar2)
                    {
                        if (PM.Bar[(int)light - (int)Lights.Bar1].Type != (int)BarType.Full) PM.SetLight(light, r, g, b, Global.Rand.NextSingle());
                        else PM.SetLight(light, r, g, b, 1);
                    }
                    else PM.SetLight(light, r, g, b, 1);
                }
            }
        }
    }

    void Off()
    {
        State = false;
        double r = (Color.R / 255f);
        double g = (Color.G / 255f);
        double b = (Color.B / 255f);
        foreach (Lights light in Enum.GetValues(typeof(Lights)))
        {
            if (Switch[(int)light].Value)
            {
                if (Inverted[(int)light])
                {
                    if (light >= Lights.Bar1 && light <= Lights.Bar2)
                    {
                        if (PM.Bar[(int)light - (int)Lights.Bar1].Type != (int)BarType.Full) PM.SetLight(light, r, g, b, Global.Rand.NextSingle());
                        else PM.SetLight(light, r, g, b, 1);
                    }
                    else PM.SetLight(light, r, g, b, 1);
                }
                else PM.KillLight(light);
            }
        }
    }

    public override void Kill()
    {
        base.Kill();
        State = false;
    }

    public void ResetBarType()
    {
        for (int i = 0; i < Switch.Count; i++)
        {
            if (Switch[i].Value)
            {
                if (i >= (int)Lights.Bar1 && i <= (int)Lights.Bar2)
                {
                    if (PM.Bar[i - (int)Lights.Bar1].Type > (int)BarType.Split)
                    {
                        PM.Bar[i - (int)Lights.Bar1].Type = (int)BarType.Full;
                    }
                }
            }
        }
    }
}