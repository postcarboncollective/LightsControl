namespace LightsControl;

public class ComponentSwitch(bool val, int index, Function function)
{
    public bool Value
    {
        get => val;
        set => Set(value);
    }

    public readonly int Index = index;
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
            PM.Lights[Index].Set(0, 0, 0, 0);
            var q = Function.Switch.Where(x => x.Value == true).ToList();
            if (q.Count == 0) Function.Stop();
        }
    }
}