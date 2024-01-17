using System.Timers;
using MudBlazor.Utilities;

namespace LightsControl;

public class FunctionStrobe : Function
{
    public System.Timers.Timer Timer = new();
    public ColorFunction Color;
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
        Color = new(new MudColor(255, 255, 255, 255), this);
        Speed = speed;
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

    void SwitchState()
    {
        if (State) Off();
        else On();
    }

    void On()
    {
        State = true;
        foreach (Lights light in Enum.GetValues(typeof(Lights)))
        {
            if (Switch[(int)light].Value)
            {
                if (Inverted[(int)light]) PM.KillLight(light);
                else
                {
                    if (light >= Lights.Bar1 && light <= Lights.Bar2)
                    {
                        if (PM.Bar[(int)light - (int)Lights.Bar1].Type != (int)BarType.Full) PM.SetBrightness(light, Global.Rand.NextSingle());
                        else PM.SetBrightness(light, 1);
                    }
                    else PM.SetBrightness(light, 1);
                }
            }
        }
    }

    void Off()
    {
        State = false;
        foreach (Lights light in Enum.GetValues(typeof(Lights)))
        {
            if (Switch[(int)light].Value)
            {
                if (Inverted[(int)light])
                {
                    if (light >= Lights.Bar1 && light <= Lights.Bar2)
                    {
                        if (PM.Bar[(int)light - (int)Lights.Bar1].Type != (int)BarType.Full) PM.SetBrightness(light, Global.Rand.NextSingle());
                        else PM.SetBrightness(light, 1);
                    }
                    else PM.SetBrightness(light, 1);
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

    public override void ResetBarType(int index)
    {
        if (index >= (int)Lights.Bar1 && index <= (int)Lights.Bar2)
        {
            if (PM.Bar[index - (int)Lights.Bar1].Type > (int)BarType.Split)
            {
                PM.Bar[index - (int)Lights.Bar1].Type = (int)BarType.Full;
            }
        }
    }
}