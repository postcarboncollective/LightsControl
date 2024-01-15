namespace LightsControl;

public abstract class Function
{
    public List<ToggleFunction> Toggle = new List<ToggleFunction>();
    public bool executing = false;

    protected Function()
    {
        for (int i = 0; i < Enum.GetNames(typeof(Lights)).Length; i++) Toggle.Add(new ToggleFunction((Lights)i, false, this));
    }

    public void Run()
    {
        for (int i = 0; i < Toggle.Count; i++)
        {
            if (Toggle[i].Value == true)
            {
                foreach (var f in Global.Functions)
                {
                    if (f != this)
                    {
                        if (f.Toggle[i].Value == true) f.Toggle[i].Value = false;
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
        if (Toggle[(int)Lights.Strobe].Value) PM.Strobe.Set(0, 0, 0, 0, 0, 0, 0, 0);
        if (Toggle[(int)Lights.Par].Value) PM.Par.Set(0, 0, 0, 0);
        if (Toggle[(int)Lights.Bar1].Value) PM.Bar[0].Set(0, 0, 0, 0, 0, 0);
        if (Toggle[(int)Lights.Bar2].Value) PM.Bar[1].Set(0, 0, 0, 0, 0, 0);
    }
}