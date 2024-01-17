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
        for (int i = 0; i < PM.Lights.Count; i++)
        {
            if (Switch[i].Value)
            {
                PM.Lights[i].SetColor(r, g, b);
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
        for (int i = 0; i < PM.Lights.Count; i++)
        {
            if (Switch[i].Value)
            {
                if (Inverted[i]) PM.Lights[i].SetBrightness(0);
                else
                {
                    if (i >= (int)Lights.Bar1 && i <= (int)Lights.Bar2)
                    {
                        if (PM.Bar[i - (int)Lights.Bar1].Type != (int)BarType.Full) PM.Lights[i].SetBrightness(Global.Rand.NextSingle());
                        else PM.Lights[i].SetBrightness(1);
                    }
                    else PM.Lights[i].SetBrightness(1);
                }
            }
        }
    }

    void Off()
    {
        State = false;
        for (int i = 0; i < PM.Lights.Count; i++)
        {
            if (Switch[i].Value)
            {
                if (Inverted[i])
                {
                    if (i >= (int)Lights.Bar1 && i <= (int)Lights.Bar2)
                    {
                        if (PM.Bar[i - (int)Lights.Bar1].Type != (int)BarType.Full) PM.Lights[i].SetBrightness(Global.Rand.NextSingle());
                        else PM.Lights[i].SetBrightness(1);
                    }
                    else PM.Lights[i].SetBrightness(1);
                }
                else PM.Lights[i].SetBrightness(0);
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