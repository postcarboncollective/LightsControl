using System.Collections;
using System.Timers;
using MudBlazor;

namespace LightsControl;

using System;
using System.IO.Ports;
using System.Threading;

public static class Arduino
{
    public static SerialPort SerialPort;
    public static System.Timers.Timer WriteTimer = new();
    public static byte current = 0;

    public static void Init()
    {
        string[] ports = SerialPort.GetPortNames();

        foreach (var x in ports)
        {   
            Console.WriteLine(x);
        }

        if (ports.Length == 0) return;

        SerialPort = new SerialPort(ports[0], 9600);
        // SerialPort.DataReceived += OnSerialDataReceived;
        SerialPort.Open();

        Task.Run(async () => { await Setup(); });
    }

    public async static Task Setup()
    {
        await Task.Delay(5000);
        foreach (var x in PM.Led)
        {
            bool[] a1 = new bool[] { false, false, false, false, false, false, false, false };
            bool[] a2 = new bool[] { false, false, false, false, false, false, false, false };
            if (x.Address < 8) a1[x.Address] = true;
            else if (x.Address < 16) a2[x.Address] = true;
            await Task.Delay(250);
            Write(CreateByte(a1), CreateByte(a2), (byte)LedFunction.Init, x.Size, 0, 0, 0, 0);
        }
        byte addr1 = CreateByte(new bool[] { true, true, true, true, true, true, true, true });
        byte addr2 = CreateByte(new bool[] { true, true, true, true, true, true, true, true });
        await Task.Delay(250);
        Write(addr1, addr2, (byte)LedFunction.Off, 0, 0, 0, 0, 0);
    }

    public static void Write(byte addr1, byte addr2, byte function, byte r, byte g, byte b, byte p1, byte p2)
    {
        if (SerialPort != null)
        {
            SerialPort.Write(new byte[] { 255, addr1, addr2, function, r, g, b, p1, p2 }, 0, 9);
        }
    }
    
    public static byte CreateByte(bool[] bits)
    {
        if (bits.Length > 8) throw new ArgumentOutOfRangeException();
        return (byte)bits.Select((val, i) => Convert.ToByte(val) << i).Sum();
    }
    
    private static void OnSerialDataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        if (SerialPort != null)
        {
            string message = SerialPort.ReadLine();
            if (!string.IsNullOrWhiteSpace(message)) Console.WriteLine(message);
        }
    }
}