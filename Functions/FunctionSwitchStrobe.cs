using System.Diagnostics;
using System.Timers;
using MudBlazor.Utilities;

namespace LightsControl;

public class FunctionSwitchStrobe : Function
{
    public System.Timers.Timer Timer = new();
    public ColorFunction Color;
    public int Type = 1;
    public int Index = 0;

    public int LedType = 1;
    public int LedSplitSize = 1;

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
    bool audioTriggered = false;
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

    public FunctionSwitchStrobe()
    {
        Color = new(new MudColor(0, 255, 0, 255), this);
        Speed = speed;
        Timer.Elapsed += Execute;
    }

    protected override void Start()
    {
        GetNewIndex();
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
            if (audioTriggered)
            {
                if (Audio.Volume < AudioTrigger) audioTriggered = false;
            }
            else
            {
                if (Audio.Volume >= AudioTrigger)
                {
                    audioTriggered = true;
                    SwitchLight();
                }
            }
        }
        else SwitchLight();
    }

    public override void SetColor()
    {
        R = (Color.Value.R / 255f);
        G = (Color.Value.G / 255f);
        B = (Color.Value.B / 255f);
        for (int i = 0; i < PM.Lights.Count; i++)
        {
            if (Switch[i].Value)
            {
                PM.Lights[i].SetColor(R, G, B);
            }
        }
    }

    public void SwitchLight()
    {
        PM.Lights[Index].Kill();
        GetNewIndex();
        if (Index >= (int)Lights.Bar1 && Index <= (int)Lights.Bar2)
        {
            if (PM.Bar[Index - (int)Lights.Bar1].Type != (int)LightType.Full)
            {
                PM.Lights[Index].SetBrightness(Global.Rand.NextSingle());
            }
            else PM.Lights[Index].SetBrightness(1);
        }
        else if (Index >= (int)Lights.Led1)
        {
            PM.Led[Index - (int)Lights.Led1].Type = LedType;
            PM.Led[Index - (int)Lights.Led1].SplitSize = (byte)LedSplitSize;
            if (LedType != (int)LightType.Full) PM.Lights[Index].SetBrightness(Global.Rand.NextSingle());
            else PM.Lights[Index].SetBrightness(1);
        }
        else PM.Lights[Index].SetBrightness(1);
    }

    void GetNewIndex()
    {
        List<int> possibleLights = new();
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
            if (Type == (int)SwitchType.Random)
            {
                Index = possibleLights[Global.Rand.Next(possibleLights.Count)];
            }
            else if (Type == (int)SwitchType.Sequential)
            {
                bool set = false;
                foreach (var x in possibleLights)
                {
                    if (x > Index)
                    {
                        Index = x;
                        set = true;
                        break;
                    }
                }

                if (set == false)
                {
                    Index = possibleLights[0];
                }
            }
        }
    }
}