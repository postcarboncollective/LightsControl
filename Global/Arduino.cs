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
    public static System.Timers.Timer Timer = new();
    public static bool BlinkState = false;

    public static void Init()
    {
        string[] ports = SerialPort.GetPortNames();
        if (ports.Length == 0) return;
        
        SerialPort = new SerialPort(ports[0], 9600);
        SerialPort.Open();
        
        // ReadTimer.Elapsed += Read;
        // ReadTimer.Interval = 200;
        // ReadTimer.Start();
        
        // Timer.Elapsed += Blink;
        // Timer.Interval = 50;
        // Timer.Start();
    }
    
    public static void Write(string value)
    {
        // Console.WriteLine(value);
        SerialPort.WriteLine(value);
    }
    
    public static void Read(object? sender, ElapsedEventArgs e)
    {
        string message = SerialPort.ReadExisting();
        if(!string.IsNullOrWhiteSpace(message)) Console.WriteLine(message);                
    }
}