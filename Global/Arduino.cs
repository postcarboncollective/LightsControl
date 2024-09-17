using System.Collections;
using System.Timers;
using MudBlazor;

namespace LightsControl;

using System;
using System.IO.Ports;
using System.Threading;

public static class Arduino
{
    public static SerialPort SerialPort = new();
    public static List<string> Ports = new();
    public static System.Timers.Timer WriteTimer = new();
    public static byte current = 0;
    public static bool SerialPortError = false;

    public static void Init()
    {
        OpenSerialPort();
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

    public static async void OpenSerialPort()
    {
        try
        {
            SerialPort = new();
            Ports = SerialPort.GetPortNames().ToList();
            List<string> toRemove = new();
            foreach (var port in Ports)
            {
                Console.WriteLine(port);
                // if (port.StartsWith("/dev/ttyAMA") || port.StartsWith("/dev/ttyUSB"))
                if (port.StartsWith("/dev/ttyAMA"))
                {
                    toRemove.Add(port);
                }
            }
            foreach (var port in toRemove) Ports.Remove(port);

            if (Ports.Count > 0)
            {
                SerialPort = new SerialPort(Ports[0], 9600);
                SerialPort.ReadTimeout = 1000;
                SerialPort.WriteTimeout = 1000;
                // SerialPort.DataReceived += OnSerialDataReceived;
                SerialPort.RtsEnable = true;
                SerialPort.DtrEnable = true;
                SerialPort.Open();
                SerialPortError = false;
                Console.WriteLine($"Opened Serial Port -> {Ports[0]}");
            }
            else RetryOpenSerialPort();
        }
        catch
        {
            RetryOpenSerialPort();
        }
    }

    static async void RetryOpenSerialPort()
    {
        Console.WriteLine("Failed to open Serial Port. Trying again in 2 seconds.");
        await Task.Delay(2000);
        ResetSerialPort();
    }

    public static void ResetSerialPort()
    {
        try
        {
            SerialPort.DiscardInBuffer();
            SerialPort.DiscardOutBuffer();
            SerialPort.Close();
            SerialPort.Dispose();
            OpenSerialPort();
        }
        catch
        {
            OpenSerialPort();
        }
    }

    public static void Write(byte addr1, byte addr2, byte function, byte r, byte g, byte b, byte p1, byte p2)
    {
        if (SerialPort.IsOpen)
        {
            try
            {
                SerialPort.Write(new byte[] { 255, addr1, addr2, function, r, g, b, p1, p2 }, 0, 9);
            }
            catch (Exception ex)
            {
                if (SerialPortError == false)
                {
                    SerialPortError = true;
                    Console.WriteLine($"SerialPort.Write -> Error \n {ex.Message} \n {ex.StackTrace} \n {ex.InnerException} \n {ex.Source}");
                    ResetSerialPort();
                }
            }
        }
    }

    private static void OnSerialDataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        if (SerialPort.IsOpen)
        {
            try
            {
                string message = SerialPort.ReadLine();
                if (!string.IsNullOrWhiteSpace(message)) Console.WriteLine(message);
            }
            catch (Exception ex)
            {
                if (SerialPortError == false)
                {
                    SerialPortError = true;
                    Console.WriteLine($"SerialPort.Read -> Error \n {ex.Message} \n {ex.StackTrace} \n {ex.InnerException} \n {ex.Source}");
                    ResetSerialPort();
                }
            }
        }
    }

    public static byte CreateByte(bool[] bits)
    {
        if (bits.Length > 8) throw new ArgumentOutOfRangeException();
        return (byte)bits.Select((val, i) => Convert.ToByte(val) << i).Sum();
    }
}