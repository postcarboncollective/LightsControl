using MudBlazor.Utilities;

namespace LightsControl;

public class PresetStrobe : Preset
{
    public MudColor Color = new MudColor(255, 255, 255, 255);
    public double Speed = 0.5f;
    public List<bool> Inverted = new List<bool>();
    public bool Executing = false;
    static CancellationTokenSource Token = new();

    public PresetStrobe()
    {
        for (int i = 0; i < Enum.GetNames(typeof(Lights)).Length; i++) Inverted.Add(false);
    }

    public override void Run()
    {
        Function.StopAll();
        Executing = true;
        Execute().WaitAsync(Token.Token);
    }

    public override void Stop()
    {
        if (Token.Token.CanBeCanceled)
        {
            Executing = false;
            Token.Cancel();
            Token = new();
        }
    }

    public override async Task Execute()
    {
        while (Executing)
        {
            On();
            await Task.Delay(TimeSpan.FromSeconds((1 - Speed) / 2), Token.Token);
            Off();
            await Task.Delay(TimeSpan.FromSeconds((1 - Speed) / 2), Token.Token);
        }
        Kill();
    }

    void On()
    {
        double r = (Color.R / 255f);
        double g = (Color.G / 255f);
        double b = (Color.B / 255f);
        if (Toggle[(int)Lights.Strobe])
        {
            if (Inverted[(int)Lights.Strobe]) PM.Strobe.Set(0, 0, 0, 0, 0, 0, 0, 0);
            else PM.Strobe.Set(r, g, b, 0, 1, 0, 0, 0);
        }
        if (Toggle[(int)Lights.Par])
        {
            if (Inverted[(int)Lights.Par]) PM.Par.Set(0, 0, 0, 0);
            else PM.Par.Set((r + g + b) / 3, 0, 1, 0);
        }
        if (Toggle[(int)Lights.Bar1])
        {
            if (Inverted[(int)Lights.Bar1]) PM.Bar[0].Set(0, 0, 0, 0, 0, 0);
            else PM.Bar[0].Set(r, g, b, 1, 0, 0);
        }
        if (Toggle[(int)Lights.Bar2])
        {
            if (Inverted[(int)Lights.Bar2]) PM.Bar[1].Set(0, 0, 0, 0, 0, 0);
            else PM.Bar[1].Set(r, g, b, 1, 0, 0);
        }
    }

    void Off()
    {
        double r = (Color.R / 255f);
        double g = (Color.G / 255f);
        double b = (Color.B / 255f);
        if (Toggle[(int)Lights.Strobe])
        {
            if (!Inverted[(int)Lights.Strobe]) PM.Strobe.Set(0, 0, 0, 0, 0, 0, 0, 0);
            else PM.Strobe.Set(r, g, b, 0, 1, 0, 0, 0);
        }
        if (Toggle[(int)Lights.Par])
        {
            if (!Inverted[(int)Lights.Par]) PM.Par.Set(0, 0, 0, 0);
            else PM.Par.Set((r + g + b) / 3, 0, (r + g + b) / 3, 0);
        }
        if (Toggle[(int)Lights.Bar1])
        {
            if (!Inverted[(int)Lights.Bar1]) PM.Bar[0].Set(0, 0, 0, 0, 0, 0);
            else PM.Bar[0].Set(r, g, b, 1, 0, 0);
        }
        if (Toggle[(int)Lights.Bar2])
        {
            if (!Inverted[(int)Lights.Bar2]) PM.Bar[1].Set(0, 0, 0, 0, 0, 0);
            else PM.Bar[1].Set(r, g, b, 1, 0, 0);
        }
    }

    void Kill()
    {
        if (Toggle[(int)Lights.Strobe]) PM.Strobe.Set(0, 0, 0, 0, 0, 0, 0, 0);
        if (Toggle[(int)Lights.Par]) PM.Par.Set(0, 0, 0, 0);
        if (Toggle[(int)Lights.Bar1]) PM.Bar[0].Set(0, 0, 0, 0, 0, 0);
        if (Toggle[(int)Lights.Bar2]) PM.Bar[1].Set(0, 0, 0, 0, 0, 0);
    }
}