namespace LightsControl;

public abstract class Function
{
    public readonly List<SwitchFunction> Switch = new List<SwitchFunction>();

    protected Function()
    {
        for (int i = 0; i < Enum.GetNames(typeof(Lights)).Length; i++) Switch.Add(new SwitchFunction(false, (Lights)i, this));
    }

    public void Run()
    {
        Reset();
        Start();
    }

    protected virtual void Start()
    {
    }

    public virtual void Stop()
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
                ResetBarType(i);
            }
        }
    }

    public virtual void ResetBarType(int index)
    {
    }

    public virtual void Kill()
    {
        foreach (Lights light in Enum.GetValues(typeof(Lights)))
        {
            if (Switch[(int)light].Value) PM.KillLight(light);
        }
    }
}