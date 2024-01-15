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
        if (value) Function.Run();
        else
        {
            switch (Index)
            {
                case Lights.Strobe:
                    PM.Strobe.Set(0, 0, 0, 0, 0, 0, 0, 0);
                    break;
                case Lights.Par:
                    PM.Par.Set(0, 0, 0, 0);
                    break;
                case Lights.Bar1:
                    PM.Bar[0].Set(0, 0, 0, 0, 0, 0);
                    break;
                case Lights.Bar2:
                    PM.Bar[1].Set(0, 0, 0, 0, 0, 0);
                    break;
            }
            var q = Function.Switch.Where(x => x.Value == true).ToList();
            if (q.Count == 0) Function.Stop();
        }
    }
}