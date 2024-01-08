using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Rug.Osc;

namespace LightsControl;

public static class Osc
{
    public static IPAddress Address;
    public static OscSender Sender;

    public static OscReceiver Receiver;
    public static Thread Thread;

    public static void Init(string address, int outputPort, int inputPort)
    {
        Address = IPAddress.Parse(address);
        Sender = new OscSender(Address, 58293, outputPort);
        Sender.Connect();

        // Receiver = new OscReceiver(inputPort);
        // Thread = new Thread(new ThreadStart(ListenLoop));
        // Receiver.Connect();
        // Thread.Start();
    }

    public static void Send(string addr, object[] args)
    {
        OscMessage msg = new(addr, args);
        OscBundle bundle = new(OscTimeTag.FromDataTime(DateTime.UtcNow + TimeSpan.FromSeconds(0.125)), msg);
        Sender.Send(bundle);
    }

    public static void SendDmx(int addr, float val)
    {
        Send("/dmx" + addr, new object[] { val / 100 });
    }

    public static void SendDmx(List<int> addr, float val)
    {
        foreach (var x in addr)
        {
            Send("/dmx" + x, new object[] { val / 100 });
        }
    }

    public static void ListenLoop()
    {
        try
        {
            while (Receiver.State != OscSocketState.Closed)
            {
                if (Receiver.State == OscSocketState.Connected)
                {
                    OscPacket packet = Receiver.Receive();
                    string x = packet.ToString();
                    if (x.StartsWith("/0/dmx/"))
                    {
                        x = x.Remove(0, "/0/dmx/".Length);
                        int address = int.Parse(x.Split(",")[0]);
                        address = address + 1;
                        x = x.Split(",")[1];
                        x = x.Remove(0, 1);
                        x = x.Split("f")[0];
                        float value = float.Parse(x);
                        Console.WriteLine($"{address} - {value}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            if (Receiver.State == OscSocketState.Connected)
            {
                Console.WriteLine("Exception in listen loop");
                Console.WriteLine(ex.Message);
            }
        }
    }
}