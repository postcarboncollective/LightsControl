using System.Timers;
using Microsoft.Extensions.Logging.Console;
using MudBlazor;
using MudBlazor.Utilities;

namespace LightsControl;

public class FunctionOscillator : Function
{
    public System.Timers.Timer Timer = new();
    public ColorFunction Color;
    public double Speed = 0.125f;
    public int Type = 1;
    public List<ComponentInvert> Inverted = new List<ComponentInvert>();

    public int LedType = 1;
    public int LedSplitSize = 1;
    List<ComponentInvert> invertedLeds;
    bool[] ledA1 = { false, false, false, false, false, false, false, false };
    bool[] ledA2 = { false, false, false, false, false, false, false, false };
    bool[] ledA1Normal = { false, false, false, false, false, false, false, false };
    bool[] ledA2Normal = { false, false, false, false, false, false, false, false };
    bool[] ledA1Inverted = { false, false, false, false, false, false, false, false };
    bool[] ledA2Inverted = { false, false, false, false, false, false, false, false };
    bool hasLeds = false;
    
    double time = 0;
    double val = 0;
    double inv = 1;
    double walk = 0.5f;
    int walkDirection = 1;

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
    bool triggered = false;

    public FunctionOscillator()
    {
        for (int i = 0; i < Enum.GetNames(typeof(Lights)).Length; i++) Inverted.Add(new ComponentInvert(false, this));
        invertedLeds = Inverted.GetRange((int)Lights.Led1, Enum.GetNames(typeof(Lights)).Length - (int)Lights.Led1);
        Color = new(new MudColor(0, 255, 0, 255), this);
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

    public override void ResetLeds()
    {
        hasLeds = false;
        invertedLeds = Inverted.GetRange((int)Lights.Led1, Enum.GetNames(typeof(Lights)).Length - (int)Lights.Led1);
        
        for (int i = 0; i < SwitchLed.Count; i++)
        {
            if (i < 8) ledA1[i] = SwitchLed[i].Value;
            else if (i < 16) ledA2[i - 8] = SwitchLed[i].Value;
            if (hasLeds == false && SwitchLed[i].Value) hasLeds = true;
        }

        for (int i = 0; i < SwitchLed.Count; i++)
        {
            if (i < 8)
            {
                if (invertedLeds[i].Value)
                {
                    if (ledA1[i])
                    {
                        ledA1Normal[i] = false;
                        ledA1Inverted[i] = true;
                    }
                }
                else if (ledA1[i])
                {
                    ledA1Normal[i] = true;
                    ledA1Inverted[i] = false;
                }
                else
                {
                    ledA1Normal[i] = false;
                    ledA1Inverted[i] = false;
                }
            }
            else if (i < 16)
            {
                if (invertedLeds[i].Value)
                {
                    if (ledA2[i])
                    {
                        ledA2Normal[i] = false;
                        ledA2Inverted[i] = true;
                    }
                }
                else if (ledA2[i])
                {
                    ledA2Normal[i] = true;
                    ledA2Inverted[i] = false;
                }
                else
                {
                    ledA2Normal[i] = false;
                    ledA2Inverted[i] = false;
                }
            }
        }
    }

    public void Execute(object? sender, ElapsedEventArgs args)
    {
        if (AudioEnabled && Type != 3)
        {
            time = Audio.Volume / AudioMax;
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
            case 3:
                if (AudioEnabled)
                {
                    if (triggered)
                    {
                        if (Audio.Volume < AudioMax) triggered = false;
                    }
                    else
                    {
                        if (Audio.Volume >= AudioMax)
                        {
                            triggered = true;
                            if (walkDirection == 1) walkDirection = -1;
                            else walkDirection = 1;
                        }
                    }
                }
                else
                {
                    if (time % 0.1f <= 0.01) 
                    {
                        int[] numbers = { -1, 1 };
                        walkDirection = numbers[Global.Rand.Next(0, 2)];
                    }                    
                }
                walk += (Speed / 2) * walkDirection;
                if (walk > 1) walk = 0;
                else if (walk < 0) walk = 1;
                val = walk;
                inv = 1 - walk;
                break;
        }

        for (int i = 0; i < (int)Lights.Led1; i++)
        {
            if (Switch[i].Value)
            {
                if (Inverted[i].Value) PM.Lights[i].SetBrightness(inv);
                else PM.Lights[i].SetBrightness(val);
            }
        }

        if (hasLeds)
        {
            switch (LedType)
            {
                case (int)LightType.Full:
                    Global.SetLeds(ledA1Normal, ledA2Normal, LedFunction.Full, R * val, G * val, B * val, 0, 0);
                    Global.SetLeds(ledA1Inverted, ledA2Inverted, LedFunction.Full, R * inv, G * inv, B * inv, 0, 0);
                    break;
                case (int)LightType.Split:
                    Global.SetLeds(ledA1Normal, ledA2Normal, LedFunction.Set, R, G, B, (byte)(val * 200), (byte)LedSplitSize);
                    Global.SetLeds(ledA1Inverted, ledA2Inverted, LedFunction.Set, R, G, B, (byte)(inv * 200), (byte)LedSplitSize);
                    break;
                case (int)LightType.Fill:
                    Global.SetLeds(ledA1Normal, ledA2Normal, LedFunction.Fill, R, G, B, (byte)(val * 200), 0);
                    Global.SetLeds(ledA1Inverted, ledA2Inverted, LedFunction.Fill, R, G, B, (byte)(inv * 200), 0);
                    break;
                case (int)LightType.iFill:
                    Global.SetLeds(ledA1Normal, ledA2Normal, LedFunction.iFill, R, G, B, (byte)(val * 200), 0);
                    Global.SetLeds(ledA1Inverted, ledA2Inverted, LedFunction.iFill, R, G, B, (byte)(inv * 200), 0);
                    break;
            }
        }
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
}