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

        Receiver = new OscReceiver(inputPort);
        Thread = new Thread(new ThreadStart(ListenLoop));
        Receiver.Connect();
        Thread.Start();
    }

    public static void Send(string addr, object[] args)
    {
        OscMessage msg = new(addr, args);
        OscBundle bundle = new(OscTimeTag.FromDataTime(DateTime.UtcNow + TimeSpan.FromSeconds(0.125)), msg);
        Sender.Send(bundle);
    }

    public static void SendDmx(int addr, double val)
    {
        Send("/dmx" + addr, new object[] { val });
    }

    public static void SendDmx(List<int> addr, double val)
    {
        foreach (var x in addr)
        {
            Send("/dmx" + x, new object[] { val });
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
                    string[] str = packet.ToString().Split(",");
                    Audio.Volume = float.Parse(str[1].Replace("f", ""));
                    Console.WriteLine(Audio.Volume);
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