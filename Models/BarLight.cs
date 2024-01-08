namespace LightsControl;

public class BarLight
{
    public List<int> Brightness { get; set; } = new();
    public List<int> Strobe { get; set; }= new();
    public List<int> Red { get; set; } = new();
    public List<int> Green { get; set; } = new();
    public List<int> Blue { get; set; } = new();
    public List<int> Macro { get; set; } = new();

    public BarLight(int addr)
    {
        Brightness.Clear();
        Strobe.Clear();
        Red.Clear();
        Green.Clear();
        Blue.Clear();
        Macro.Clear();
        for (int i = 0; i < 8; i++)
        {
            Brightness.Add((addr + 0) + (i * 6));
            Strobe.Add((addr + 1) + (i * 6));
            Red.Add((addr + 2) + (i * 6));
            Green.Add((addr + 3) + (i * 6));
            Blue.Add((addr + 4) + (i * 6));
            Macro.Add((addr + 5) + (i * 6));
        }
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