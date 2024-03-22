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
    public List<ComponentInvert> InvertedLeds;
    public bool[] LedA1 = { false, false, false, false, false, false, false, false };
    public bool[] LedA2 = { false, false, false, false, false, false, false, false };
    public bool[] LedA1Normal = { false, false, false, false, false, false, false, false };
    public bool[] LedA2Normal = { false, false, false, false, false, false, false, false };
    public bool[] LedA1Inverted = { false, false, false, false, false, false, false, false };
    public bool[] LedA2Inverted = { false, false, false, false, false, false, false, false };
    public bool HasLeds = false;

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
        InvertedLeds = Inverted.GetRange((int)Lights.Led1, Enum.GetNames(typeof(Lights)).Length-(int)Lights.Led1);
        Color = new(new MudColor(255, 255, 255, 255), this);
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
        HasLeds = false;
        InvertedLeds = Inverted.GetRange((int)Lights.Led1, Enum.GetNames(typeof(Lights)).Length-(int)Lights.Led1);
        for (int i = 0; i < SwitchLed.Count; i++)
        {
            if (i < 8)
            {
                LedA1[i] = SwitchLed[i].Value;
                LedA1Normal[i] = SwitchLed[i].Value;
            }
            else if (i < 16)
            {
                LedA2[i - 8] = SwitchLed[i].Value;
                LedA2Normal[i - 8] = SwitchLed[i].Value;
            }
            if (HasLeds == false && SwitchLed[i].Value) HasLeds = true;
        }
        for(int i=0; i<SwitchLed.Count; i++)
        {
            if (i < 8)
            {
                if (InvertedLeds[i].Value)
                {
                    if (LedA1Normal[i])
                    {
                        LedA1Inverted[i] = true;
                        LedA1Normal[i] = false;
                    }
                }                            
            }
            else if (i < 16)
            {
                if (InvertedLeds[i].Value)
                {
                    if (LedA2Normal[i])
                    {
                        LedA2Inverted[i] = true;
                        LedA2Normal[i] = false;
                    }
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
                    if (PM.Bar[i - (int)Lights.Bar1].Type != (int)LightFunction.Full)
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
        if (HasLeds)
        {
            switch (LedType)
            {
                case (int)LedFunction.Full:
                    Global.SetLeds(LedA1Normal, LedA2Normal, LedFunction.Full, R, G, B, 0, 0);
                    Global.SetLeds(LedA1Inverted, LedA2Inverted, LedFunction.Full, 0, 0, 0, 0, 0);
                    break;
                case (int)LedFunction.Set:
                    Global.SetLeds(LedA1, LedA2, LedFunction.Set, R, G, B, (byte)(Global.Rand.NextSingle()*PM.Led[0].Size), (byte)LedSplitSize);
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
                    if (PM.Bar[i - (int)Lights.Bar1].Type != (int)LightFunction.Full)
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
        if (HasLeds)
        {
            switch (LedType)
            {
                case (int)LedFunction.Full:
                    Global.SetLeds(LedA1Inverted, LedA2Inverted, LedFunction.Full, R, G, B, 0, 0);
                    Global.SetLeds(LedA1Normal, LedA2Normal, LedFunction.Full, 0, 0, 0, 0, 0);
                    break;
                case (int)LedFunction.Set:
                    Global.SetLeds(LedA1, LedA2, LedFunction.Set, R, G, B, (byte)(Global.Rand.NextSingle()*PM.Led[0].Size), (byte)LedSplitSize);
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