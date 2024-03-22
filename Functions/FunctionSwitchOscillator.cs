using System.Timers;
using Microsoft.Extensions.Logging.Console;
using MudBlazor;
using MudBlazor.Utilities;

namespace LightsControl;

public class FunctionSwitchOscillator : Function
{
    public System.Timers.Timer Timer = new();
    public ColorFunction Color;
    public double Speed = 0.125f;
    public int OscillatorType = 1;
    public List<ComponentInvert> Inverted = new List<ComponentInvert>();
    public int Type = 1;
    public int Index = 0;

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
    bool timeTriggered = false;

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
    bool audioTriggered = false;

    public FunctionSwitchOscillator()
    {
        for (int i = 0; i < Enum.GetNames(typeof(Lights)).Length; i++) Inverted.Add(new ComponentInvert(false, this));
        invertedLeds = Inverted.GetRange((int)Lights.Led1, Enum.GetNames(typeof(Lights)).Length - (int)Lights.Led1);
        Color = new(new MudColor(0, 255, 0, 255), this);
        Timer.Interval = (1 / 32f) * 1000;
        Timer.Elapsed += Execute;
    }

    protected override void Start()
    {
        GetNewIndex();
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

    public void Execute(object? sender, ElapsedEventArgs args)
    {
        if (AudioEnabled && OscillatorType != 3)
        {
            time = Audio.Volume / AudioMax;
            if (time > 1)
            {
                time = 1;
                timeTriggered = true;
            }
        }
        else
        {
            time += (Speed / 2);
            if (time > 1)
            {
                time = 0;
                timeTriggered = true;
            }
        }

        switch (OscillatorType)
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
                    if (audioTriggered)
                    {
                        if (Audio.Volume < AudioMax) audioTriggered = false;
                    }
                    else
                    {
                        if (Audio.Volume >= AudioMax)
                        {
                            audioTriggered = true;
                            if (walkDirection == 1) walkDirection = -1;
                            else walkDirection = 1;
                        }
                    }
                }
                else
                {
                    if (time % 0.1f <= 0.01f)
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

        if (timeTriggered)
        {
            timeTriggered = false;
            PM.Lights[Index].Kill();
            GetNewIndex();
            if (Index >= (int)Lights.Led1)
            {
                PM.Led[Index - (int)Lights.Led1].Type = LedType;
                PM.Led[Index - (int)Lights.Led1].SplitSize = (byte)LedSplitSize;
            }
        }
        
        if (Inverted[Index].Value) PM.Lights[Index].SetBrightness(inv);
        else PM.Lights[Index].SetBrightness(val);
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
            if (OscillatorType == (int)SwitchType.Random)
            {
                Index = possibleLights[Global.Rand.Next(possibleLights.Count)];
            }
            else if (OscillatorType == (int)SwitchType.Sequential)
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