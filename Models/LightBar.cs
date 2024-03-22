namespace LightsControl;

public class LightBar : Light
{
    public int Type = 1;
    public List<DmxChannel> Brightness;
    public List<DmxChannel> Strobe;
    public List<DmxChannel> Red;
    public List<DmxChannel> Green;
    public List<DmxChannel> Blue;
    public List<DmxChannel> Macro;

    public LightBar(int addr)
    {
        Brightness = new();
        Strobe = new();
        Red = new();
        Green = new();
        Blue = new();
        Macro = new();
        for (int i = 0; i < 8; i++)
        {
            Brightness.Add(new DmxChannel(addr + (i * 6), 0));
            Strobe.Add(new DmxChannel((addr + 1) + (i * 6), 0));
            Red.Add(new DmxChannel((addr + 2) + (i * 6), 0));
            Green.Add(new DmxChannel((addr + 3) + (i * 6), 0));
            Blue.Add(new DmxChannel((addr + 4) + (i * 6), 0));
            Macro.Add(new DmxChannel((addr + 5) + (i * 6), 0));
        }
    }

    public override void Set(double r, double g, double b, double brightness)
    {
        switch (Type)
        {
            case (int)LightType.Full:
                for (int i = 0; i < Red.Count; i++)
                {
                    Red[i].Value = r;
                    Green[i].Value = g;
                    Blue[i].Value = b;
                    Brightness[i].Value = brightness;
                }
                break;
            case (int)LightType.Split:
                for (int i = 0; i < Red.Count; i++)
                {
                    Red[i].Value = r;
                    Green[i].Value = g;
                    Blue[i].Value = b;

                    float div = 1f / Red.Count;
                    float next = (i + 1) * div;
                    float val = i * div;
                    if (brightness < next && brightness >= val) Brightness[i].Value = 1;
                    else Brightness[i].Value = 0;
                }
                break;
            case (int)LightType.Fill:
                for (int i = 0; i < Red.Count; i++)
                {
                    int index = i;
                    Red[index].Value = r;
                    Green[index].Value = g;
                    Blue[index].Value = b;

                    float div = 1f / Red.Count;
                    float next = (i + 1) * div;
                    float val = i * div;
                    if (brightness > next) Brightness[index].Value = 1;
                    else if (brightness > val) Brightness[index].Value = (brightness - val) * Brightness.Count;
                    else Brightness[index].Value = 0;
                }
                break;
            case (int)LightType.iFill:
                for (int i = 0; i < Red.Count; i++)
                {
                    int index = (Red.Count - 1) - i;
                    Red[index].Value = r;
                    Green[index].Value = g;
                    Blue[index].Value = b;

                    float div = 1f / Red.Count;
                    float next = (i + 1) * div;
                    float val = i * div;
                    if (brightness > next) Brightness[index].Value = 1;
                    else if (brightness > val) Brightness[index].Value = (brightness - val) * Brightness.Count;
                    else Brightness[index].Value = 0;
                }
                break;
        }
    }

    public override void SetColor(double r, double g, double b)
    {
        SetRed(r);
        SetGreen(g);
        SetBlue(b);
    }

    public override void SetBrightness(double brightness)
    {
        switch (Type)
        {
            case (int)LightType.Full:
                for (int i = 0; i < Brightness.Count; i++)
                {
                    Brightness[i].Value = brightness;
                }
                break;
            case (int)LightType.Split:
                for (int i = 0; i < Brightness.Count; i++)
                {
                    float div = 1f / Brightness.Count;
                    float next = (i + 1) * div;
                    float val = i * div;
                    if (brightness < next && brightness >= val) Brightness[i].Value = 1;
                    else Brightness[i].Value = 0;
                }
                break;
            case (int)LightType.Fill:
                for (int i = 0; i < Brightness.Count; i++)
                {
                    int index = i;
                    float div = 1f / Brightness.Count;
                    float next = (i + 1) * div;
                    float val = i * div;
                    if (brightness > next) Brightness[index].Value = 1;
                    else if (brightness > val) Brightness[index].Value = (brightness - val) * Brightness.Count;
                    else Brightness[index].Value = 0;
                }
                break;
            case (int)LightType.iFill:
                for (int i = 0; i < Brightness.Count; i++)
                {
                    int index = (Brightness.Count - 1) - i;
                    float div = 1f / Brightness.Count;
                    float next = (i + 1) * div;
                    float val = i * div;
                    if (brightness > next) Brightness[index].Value = 1;
                    else if (brightness > val) Brightness[index].Value = (brightness - val) * Brightness.Count;
                    else Brightness[index].Value = 0;
                }
                break;
        }
    }

    public override void Kill()
    {
        foreach (var x in Brightness)
        {
            x.Value = 0;
        }
    }

    public void SetRed(double val)
    {
        foreach (var x in Red) x.Value = val;
    }

    public void SetGreen(double val)
    {
        foreach (var x in Green) x.Value = val;
    }

    public void SetBlue(double val)
    {
        foreach (var x in Blue) x.Value = val;
    }

    public void SetStrobe(double val)
    {
        foreach (var x in Strobe) x.Value = val;
    }

    public void SetMacro(double val)
    {
        foreach (var x in Macro) x.Value = val;
    }
}

// Bar1 17 - 64
// 17 -  P1 Brightness
// 18 -  P1 Strobe
// 19 -  P1 Red
// 20 -  P1 Green
// 21 -  P1 Blue
// 22 -  P1 Feira
// 23 -  P2 Brightness
// 24 -  P2 Strobe
// 25 -  P2 Red
// 26 -  P2 Green
// 27 -  P2 Blue
// 28 -  P2 Feira
// 29 -  P3 Brightness
// 30 -  P3 Strobe
// 31 -  P3 Red
// 32 -  P3 Green
// 33 -  P3 Blue
// 34 -  P3 Feira
// 35 -  P4 Brightness
// 36 -  P4 Strobe
// 37 -  P4 Red
// 38 -  P4 Green
// 39 -  P4 Blue
// 40 -  P4 Feira
// 41 -  P5 Brightness
// 42 -  P5 Strobe
// 43 -  P5 Red
// 44 -  P5 Green
// 45 -  P5 Blue
// 46 -  P5 Feira
// 47 -  P6 Brightness
// 48 -  P6 Strobe
// 49 -  P6 Red
// 50 -  P6 Green
// 51 -  P6 Blue
// 52 -  P6 Feira
// 53 -  P7 Brightness
// 54 -  P7 Strobe
// 55 -  P7 Red
// 56 -  P7 Green
// 57 -  P7 Blue
// 58 -  P7 Feira
// 59 -  P8 Brightness
// 60 -  P8 Strobe
// 61 -  P8 Red
// 62 -  P8 Green
// 63 -  P8 Blue
// 64 -  P8 Feira

// Bar2 65-112
// 65 -  P1 Brightness
// 66 -  P1 Strobe
// 67 -  P1 Red
// 68 -  P1 Green
// 69 -  P1 Blue
// 70 -  P1 Feira
// 71 -  P2 Brightness
// 72 -  P2 Strobe
// 73 -  P2 Red
// 74 -  P2 Green
// 75 -  P2 Blue
// 76 -  P2 Feira
// 77 -  P3 Brightness
// 78 -  P3 Strobe
// 79 -  P3 Red
// 80 -  P3 Green
// 81 -  P3 Blue
// 82 -  P3 Feira
// 83 -  P4 Brightness
// 84 -  P4 Strobe
// 85 -  P4 Red
// 86 -  P4 Green
// 87 -  P4 Blue
// 88 -  P4 Feira
// 89 -  P5 Brightness
// 90 -  P5 Strobe
// 91 -  P5 Red
// 92 -  P5 Green
// 93 -  P5 Blue
// 94 -  P5 Feira
// 95 -  P6 Brightness
// 96 -  P6 Strobe
// 97 -  P6 Red
// 98 -  P6 Green
// 99 -  P6 Blue
// 100 - P6 Feira
// 101 - P7 Brightness
// 102 - P7 Strobe
// 103 - P7 Red
// 104 - P7 Green
// 105 - P7 Blue
// 106 - P7 Feira
// 107 - P8 Brightness
// 108 - P8 Strobe
// 109 - P8 Red
// 110 - P8 Green
// 111 - P8 Blue
// 112 - P8 Feira