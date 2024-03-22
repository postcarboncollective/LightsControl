using System.Timers;
using MudBlazor.Utilities;

namespace LightsControl;

public class FunctionStrobe : Function
{
    public System.Timers.Timer Timer = new();
    public ColorFunction Color;
    public List<ComponentInvert> Inverted = new List<ComponentInvert>();
    public bool State = false;

    public int LedType = 1;
    public int LedSplitSize = 1;
    List<ComponentInvert> invertedLeds;
    readonly bool[] ledA1 = { false, false, false, false, false, false, false, false };
    bool[] ledA2 = { false, false, false, false, false, false, false, false };
    bool[] ledA1Normal = { false, false, false, false, false, false, false, false };
    bool[] ledA2Normal = { false, false, false, false, false, false, false, false };
    bool[] ledA1Inverted = { false, false, false, false, false, false, false, false };
    bool[] ledA2Inverted = { false, false, false, false, false, false, false, false };
    bool hasLeds = false;

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
        for (int i = 0; i < Enum.GetNames(typeof(Lights)).Length; i++) Inverted.Add(new ComponentInvert(false, this));
        invertedLeds = Inverted.GetRange((int)Lights.Led1, Enum.GetNames(typeof(Lights)).Length - (int)Lights.Led1);
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
                    SwitchState();
                }
            }
        }
        else SwitchState();
    }

    public override void SetColor()
    {
        R = (Color.Value.R / 255f);
        G = (Color.Value.G / 255f);
        B = (Color.Value.B / 255f);
        for (int i = 0; i < (int)Lights.Led1; i++)
        {
            if (Switch[i].Value)
            {
                PM.Lights[i].SetColor(R, G, B);
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

        // Lights
        for (int i = 0; i < (int)Lights.Led1; i++)
        {
            if (Switch[i].Value)
            {
                if (i >= (int)Lights.Bar1 && i <= (int)Lights.Bar2)
                {
                    if (PM.Bar[i - (int)Lights.Bar1].Type != (int)LightType.Full)
                    {
                        PM.Lights[i].SetBrightness(Global.Rand.NextSingle());
                    }
                    else
                    {
                        if (Inverted[i].Value) PM.Lights[i].SetBrightness(0);
                        else PM.Lights[i].SetBrightness(1);
                    }
                }
                else
                {
                    if (Inverted[i].Value) PM.Lights[i].SetBrightness(0);
                    else PM.Lights[i].SetBrightness(1);
                }
            }
        }

        // Leds
        if (hasLeds)
        {
            switch (LedType)
            {
                case (int)LedFunction.Full:
                    Global.SetLeds(ledA1Normal, ledA2Normal, LedFunction.Full, R, G, B, 0, 0);
                    Global.SetLeds(ledA1Inverted, ledA2Inverted, LedFunction.Off, 0, 0, 0, 0, 0);
                    break;
                case (int)LedFunction.Set:
                    Global.SetLeds(ledA1, ledA2, LedFunction.Set, R, G, B, (byte)(Global.Rand.NextSingle() * 200), (byte)LedSplitSize);
                    break;
            }
        }
    }

    void Off()
    {
        State = false;

        // Lights
        for (int i = 0; i < (int)Lights.Led1; i++)
        {
            if (Switch[i].Value)
            {
                if (i >= (int)Lights.Bar1 && i <= (int)Lights.Bar2)
                {
                    if (PM.Bar[i - (int)Lights.Bar1].Type != (int)LightType.Full)
                    {
                        PM.Lights[i].SetBrightness(Global.Rand.NextSingle());
                    }
                    else
                    {
                        if (Inverted[i].Value) PM.Lights[i].SetBrightness(1);
                        else PM.Lights[i].SetBrightness(0);
                    }
                }
                else
                {
                    if (Inverted[i].Value) PM.Lights[i].SetBrightness(1);
                    else PM.Lights[i].SetBrightness(0);
                }
            }
        }

        // Leds
        if (hasLeds)
        {
            switch (LedType)
            {
                case (int)LightType.Full:
                    Global.SetLeds(ledA1Inverted, ledA2Inverted, LedFunction.Full, R, G, B, 0, 0);
                    Global.SetLeds(ledA1Normal, ledA2Normal, LedFunction.Off, 0, 0, 0, 0, 0);
                    break;
                case (int)LightType.Split:
                    Global.SetLeds(ledA1, ledA2, LedFunction.Set, R, G, B, (byte)(Global.Rand.NextSingle() * 200), (byte)LedSplitSize);
                    break;
            }
        }
    }

    public override void Kill()
    {
        base.Kill();
        State = false;
    }
}