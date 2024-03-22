namespace LightsControl;

public abstract class Function
{
    public readonly List<ComponentSwitch> Switch = new List<ComponentSwitch>();
    public readonly List<ComponentSwitch> SwitchLed = new List<ComponentSwitch>();
    public double R = 1.0;
    public double G = 1.0;
    public double B = 1.0;

    protected Function()
    {
        for (int i = 0; i < Enum.GetNames(typeof(Lights)).Length; i++) Switch.Add(new ComponentSwitch(false, i, this));
        SwitchLed = Switch.GetRange((int)Lights.Led1, Enum.GetNames(typeof(Lights)).Length-(int)Lights.Led1);
    }

    public void Run()
    {
        Reset();
        ResetLeds();
        Kill();
        SetColor();
        Start();
    }

    protected virtual void Start()
    {
    }

    public virtual void Stop()
    {
    }

    public virtual void SetColor()
    {
    }

    public void Reset()
    {
        for (int i = 0; i < Switch.Count; i++)
        {
            if (Switch[i].Value)
            {
                foreach (var f in Global.Functions)
                {
                    if (f != this)
                    {
                        if (f.Switch[i].Value) f.Switch[i].Value = false;
                    }
                }
                ResetType(i);
            }
        }
    }

    public virtual void ResetLeds()
    {
    }

    public void ResetType(int index)
    {
        if (index >= (int)Lights.Bar1 && index <= (int)Lights.Bar2)
        {
            PM.Bar[index - (int)Lights.Bar1].Type = (int)LightFunction.Full;
        }
    }

    public virtual void Kill()
    {
        for (int i = 0; i < PM.Lights.Count; i++)
        {
            if (Switch[i].Value)
            {
                PM.Lights[i].Set(0, 0, 0, 0);
            }
        }
    }
}