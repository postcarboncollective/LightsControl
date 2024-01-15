namespace LightsControl;

public abstract class Function
{
    public readonly List<Switch> Switch = new List<Switch>();
    public bool executing = false;

    protected Function()
    {
        for (int i = 0; i < Enum.GetNames(typeof(Lights)).Length; i++) Switch.Add(new Switch(false, (Lights)i, this));
    }

    public void Run()
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
            }
        }
        Start();
    }

    protected virtual void Start()
    {
    }

    public virtual void Stop()
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