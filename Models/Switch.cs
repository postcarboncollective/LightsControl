namespace LightsControl;

public class Switch(bool val, Lights index, Function function)
{
    public bool Value
    {
        get => val;
        set => Set(value);
    }

    public readonly Lights Index = index;
    public readonly Function Function = function;

    void Set(bool value)
    {
        val = value;
        if (value)
        {
            Function.Run();
        }
        else
        {
            PM.KillLight(Index);
            var q = Function.Switch.Where(x => x.Value == true).ToList();
            if (q.Count == 0) Function.Stop();
        }
    }
}