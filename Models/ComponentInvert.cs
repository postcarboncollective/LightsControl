namespace LightsControl;

public class ComponentInvert(bool val, Function function)
{
    public bool Value
    {
        get => val;
        set => Set(value);
    }
    
    public readonly Function Function = function;

    void Set(bool value)
    {
        val = value;
        Function.ResetLeds();
    }
}