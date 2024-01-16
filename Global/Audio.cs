using System.Timers;
using JackSharp;
using JackSharp.Processing;

namespace LightsControl;

public static class Audio
{
    public static Processor Processor = new Processor("Lights Control", 1, 0, 0, 0, true);
    public static double Read = 0;

    public static void Init()
    {
        Processor.ProcessFunc += ProcessAudio;
        Processor.Start();
    }

    static void ProcessAudio(ProcessBuffer buffer)
    {
        Read = buffer.AudioIn[0].Audio.Average(x => Math.Abs(x));
    }
}