namespace LightsControl;

public abstract class Preset
{
    public List<SyncBool> Toggle = new List<SyncBool>();

    protected Preset()
    {
        for (int i = 0; i < Enum.GetNames(typeof(Lights)).Length; i++) Toggle.Add(new SyncBool(true));
    }

    public virtual void Run()
    {
    }

    public virtual void Stop()
    {
    }

    public virtual Task Execute()
    {
        return Task.CompletedTask;
    }
}