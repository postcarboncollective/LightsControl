using MudBlazor.Utilities;

namespace LightsControl;

public class PresetStrobe : Preset
{
    public SyncColor Color = new SyncColor(new MudColor(255, 255, 255, 255));
    public SyncDouble Speed = new SyncDouble(0.5f);
    public List<SyncBool> Inverted = new List<SyncBool>();
    public SyncBool Executing = new SyncBool(false);
    static CancellationTokenSource Token = new();

    public PresetStrobe()
    {
        for (int i = 0; i < Enum.GetNames(typeof(Lights)).Length; i++) Inverted.Add(new SyncBool(false));
    }

    public override void Run()
    {
        Function.StopAll();
        Executing.Value = true;
        Execute().WaitAsync(Token.Token);
    }

    public override void Stop()
    {
        if (Token.Token.CanBeCanceled)
        {
            Executing.Value = false;
            Token.Cancel();
            Token = new();
        }
    }

    public override async Task Execute()
    {
        while (Executing.Value)
        {
            On();
            await Task.Delay(TimeSpan.FromSeconds((1 - Speed.Value) / 2), Token.Token);
            Off();
            await Task.Delay(TimeSpan.FromSeconds((1 - Speed.Value) / 2), Token.Token);
        }
        Kill();
    }

    void On()
    {
        double r = (Color.Value.R / 255f);
        double g = (Color.Value.G / 255f);
        double b = (Color.Value.B / 255f);
        if (Toggle[(int)Lights.Strobe].Value)
        {
            if (Inverted[(int)Lights.Strobe].Value) PM.Strobe.Set(0, 0, 0, 0, 0, 0, 0, 0);
            else PM.Strobe.Set(r, g, b, 0, 1, 0, 0, 0);
        }
        if (Toggle[(int)Lights.Par].Value)
        {
            if (Inverted[(int)Lights.Par].Value) PM.Par.Set(0, 0, 0, 0);
            else PM.Par.Set((r + g + b) / 3, 0, 1, 0);
        }
        if (Toggle[(int)Lights.Bar1].Value)
        {
            if (Inverted[(int)Lights.Bar1].Value) PM.Bar[0].Set(0, 0, 0, 0, 0, 0);
            else PM.Bar[0].Set(r, g, b, 1, 0, 0);
        }
        if (Toggle[(int)Lights.Bar2].Value)
        {
            if (Inverted[(int)Lights.Bar2].Value) PM.Bar[1].Set(0, 0, 0, 0, 0, 0);
            else PM.Bar[1].Set(r, g, b, 1, 0, 0);
        }
    }

    void Off()
    {
        double r = (Color.Value.R / 255f);
        double g = (Color.Value.G / 255f);
        double b = (Color.Value.B / 255f);
        if (Toggle[(int)Lights.Strobe].Value)
        {
            if (!Inverted[(int)Lights.Strobe].Value) PM.Strobe.Set(0, 0, 0, 0, 0, 0, 0, 0);
            else PM.Strobe.Set(r, g, b, 0, 1, 0, 0, 0);
        }
        if (Toggle[(int)Lights.Par].Value)
        {
            if (!Inverted[(int)Lights.Par].Value) PM.Par.Set(0, 0, 0, 0);
            else PM.Par.Set((r + g + b) / 3, 0, (r + g + b) / 3, 0);
        }
        if (Toggle[(int)Lights.Bar1].Value)
        {
            if (!Inverted[(int)Lights.Bar1].Value) PM.Bar[0].Set(0, 0, 0, 0, 0, 0);
            else PM.Bar[0].Set(r, g, b, 1, 0, 0);
        }
        if (Toggle[(int)Lights.Bar2].Value)
        {
            if (!Inverted[(int)Lights.Bar2].Value) PM.Bar[1].Set(0, 0, 0, 0, 0, 0);
            else PM.Bar[1].Set(r, g, b, 1, 0, 0);
        }
    }

    void Kill()
    {
        if (Toggle[(int)Lights.Strobe].Value) PM.Strobe.Set(0, 0, 0, 0, 0, 0, 0, 0);
        if (Toggle[(int)Lights.Par].Value) PM.Par.Set(0, 0, 0, 0);
        if (Toggle[(int)Lights.Bar1].Value) PM.Bar[0].Set(0, 0, 0, 0, 0, 0);
        if (Toggle[(int)Lights.Bar2].Value) PM.Bar[1].Set(0, 0, 0, 0, 0, 0);
    }
}