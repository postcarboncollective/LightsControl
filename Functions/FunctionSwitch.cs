using System.Timers;
using MudBlazor.Utilities;

namespace LightsControl;

public class FunctionSwitch : Function
{
    public System.Timers.Timer Timer = new();
    public ColorFunction Color;
    public Lights Index = Lights.Bar1;

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

    public FunctionSwitch()
    {
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
                    SwitchLight();
                }
            }
        }
        else SwitchLight();
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

    public void SwitchLight()
    {
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
            Index = possibleLights[Global.Rand.Next(possibleLights.Count)];
        }
        if (Index >= Lights.Bar1 && Index <= Lights.Bar2)
        {
            if (PM.Bar[(int)Index - (int)Lights.Bar1].Type != (int)BarType.Full)
            {
                PM.SetBrightness(Index, Global.Rand.NextSingle());
            }
            else PM.SetBrightness(Index, 1);
        }
        else PM.SetBrightness(Index, 1);
    }

    public override void ResetBarType(int index)
    {
        if (index >= (int)Lights.Bar1 && index <= (int)Lights.Bar2)
        {
            if (PM.Bar[index - (int)Lights.Bar1].Type > (int)BarType.Split)
            {
                PM.Bar[index - (int)Lights.Bar1].Type = (int)BarType.Split;
            }
        }
    }
}