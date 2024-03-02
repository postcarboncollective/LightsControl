namespace LightsControl;

public class LightLed : Light
{
    public int Type = 1;
    public int Address;
    public double Red;
    public double Green;
    public double Blue;
    public double Brightness;


    public LightLed(int addr)
    {
        Address = addr;
        Red = 0;
        Green = 0;
        Blue = 0;
        Brightness = 1;
    }
    
    public override void Set(double r, double g, double b, double brightness)
    {
        Red = r;
        Green = g;
        Blue = b;
        Brightness = brightness;
        Arduino.Write($"{Address}|Fill|{(int)Math.Floor((Red*Brightness)*255)}|{(int)Math.Floor((Green*Brightness)*255)}|{(int)Math.Floor((Blue*Brightness)*255)}");
    }

    public override void SetColor(double r, double g, double b)
    {
        Red = r;
        Green = g;
        Blue = b;
        Arduino.Write($"{Address}|Fill|{(int)Math.Floor((Red*Brightness)*255)}|{(int)Math.Floor((Green*Brightness)*255)}|{(int)Math.Floor((Blue*Brightness)*255)}");
    }

    public override void SetBrightness(double brightness)
    {
        Brightness = brightness;
        Arduino.Write($"{Address}|Fill|{(int)Math.Floor((Red*Brightness)*255)}|{(int)Math.Floor((Green*Brightness)*255)}|{(int)Math.Floor((Blue*Brightness)*255)}");
    }
}
