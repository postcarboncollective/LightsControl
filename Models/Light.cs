namespace LightsControl;

public abstract class Light
{
    public abstract void Set(double r, double g, double b, double brightness);
    public abstract void SetColor(double r, double g, double b);
    public abstract void SetBrightness(double brightness);
}