using System.Timers;
using MudBlazor;

namespace LightsControl;

using System;
using System.IO.Ports;  
using System.Threading;

public static class Arduino
{
    public static SerialPort SerialPort;
    public static System.Timers.Timer ReadTimer = new();
    public static System.Timers.Timer WriteTimer = new();
    public static int current = 0;

    public static void Init()
    {
        string[] ports = SerialPort.GetPortNames();
        if (ports.Length == 0) return;
        
        // SerialPort = new SerialPort("/dev/ttyACM0", 9600);
        SerialPort = new SerialPort(ports[0], 9600);
        SerialPort.Open();
        
        // ReadTimer.Elapsed += Read;
        // ReadTimer.Interval = 200;
        // ReadTimer.Start();
        
        // WriteTimer.Elapsed += Iter;
        // WriteTimer.Interval = 100;
        // WriteTimer.Start();

        Task.Run(async () =>
        {
            await Task.Delay(5000);
            foreach (var x in PM.Led)
            {
                Write($"{x.Address}|Init|{x.Size}");
            }
            Write("0|Off");
        });
    }

    private static void Iter(object? sender, ElapsedEventArgs e)
    {
        Write($"1|Set|0|255|0|{current}|4");
        current++;
        current %= 30;
    }

    public static void Write(string value)
    {
        if (SerialPort != null)
        {
            SerialPort.WriteLine(value);            
        }
    }
    
    public static void Read(object? sender, ElapsedEventArgs e)
    {
        if (SerialPort != null)
        {
            string message = SerialPort.ReadExisting();
            if(!string.IsNullOrWhiteSpace(message)) Console.WriteLine(message);               
        }
    }
}