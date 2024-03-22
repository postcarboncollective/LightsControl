using System.Timers;
using MudBlazor.Utilities;

namespace LightsControl;

public class FunctionSwitch : Function
{
    public System.Timers.Timer Timer = new();
    public ColorFunction Color;
    public int Index = 0;

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
        Color = new(new MudColor(0, 255, 0, 255), this);
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
        if (AudioEnabled)
        {
            if (triggered)
            {
                if (Audio.Volume < AudioTrigger) triggered = false;
            }
            else
            {
                if (Audio.Volume >= AudioTrigger)
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
        for (int i = 0; i < PM.Lights.Count; i++)
        {
            if (base.Switch[i].Value)
            {
                PM.Lights[i].SetColor(r, g, b);
            }
        }
    }

    public void SwitchLight()
    {
        PM.Lights[Index].SetBrightness(0);
        List<int> possibleLights = new();
        foreach (var x in base.Switch)
        {
            if (x.Value)
            {
                if ((int)x.Index != Index)
                {
                    possibleLights.Add((int)x.Index);
                }
            }
        }
        if (possibleLights.Count > 0)
        {
            Index = possibleLights[Global.Rand.Next(possibleLights.Count)];
        }
        if (Index >= (int)Lights.Bar1 && Index <= (int)Lights.Bar2)
        {
            if (PM.Bar[Index - (int)Lights.Bar1].Type != (int)LightType.Full)
            {
                PM.Lights[Index].SetBrightness(Global.Rand.NextSingle());
            }
            else PM.Lights[Index].SetBrightness(1);
        }
        else PM.Lights[Index].SetBrightness(1);
    }
}