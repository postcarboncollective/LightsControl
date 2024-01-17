using MudBlazor.Utilities;

namespace LightsControl;

public class ColorFunction(MudColor value, Function function)
{
    public MudColor Value
    {
        get => value;
        set => Set(value);
    }

    public readonly Function Function = function;

    void Set(MudColor color)
    {
        value = color;
        Function.SetColor();
    }
}