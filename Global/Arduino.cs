namespace LightsControl;

using System;
using System.IO.Ports;  
using System.Threading;

public static class Arduino
{
    public static SerialPort SerialPort;
    static int val = 0;

    public static void Init()
    {
        string[] ports = SerialPort.GetPortNames();
        if (ports.Length == 0) return;
        
        SerialPort = new SerialPort(ports[0], 9600);
        SerialPort.Open();
        
        // StartReading();
        
        Task.Run(() =>
        {
            while (true)
            {
                SerialPort.WriteLine("Arduino|Fill|0|255|0");
                Thread.Sleep(200);
                SerialPort.WriteLine("Arduino|Fill|0|0|0");
                Thread.Sleep(200);
            }
        });
    }

    public static void StartReading()
    {
        Task.Run(() =>
        {
            while (true)
            {
                string message = SerialPort.ReadExisting();
                if(!string.IsNullOrWhiteSpace(message)) Console.WriteLine(message);                
                Thread.Sleep(200);
            }
        });
    }
}