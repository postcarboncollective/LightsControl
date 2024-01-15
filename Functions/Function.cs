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
                        if (f.Switch[i].Value == true) f.Switch[i].Value = false;
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
        if (Switch[(int)Lights.Strobe].Value) PM.Strobe.Set(0, 0, 0, 0, 0, 0, 0, 0);
        if (Switch[(int)Lights.Par].Value) PM.Par.Set(0, 0, 0, 0);
        if (Switch[(int)Lights.Bar1].Value) PM.Bar[0].Set(0, 0, 0, 0, 0, 0);
        if (Switch[(int)Lights.Bar2].Value) PM.Bar[1].Set(0, 0, 0, 0, 0, 0);
    }
}