using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using SimpleCL.Client;
using SimpleCL.Enums.Commons;
using SimpleCL.Ui;

namespace SimpleCL
{
    internal static class Program
    {
        public static Gui Gui { get; private set; }
        
        [STAThread]
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(Gui = new Gui());
        }

        // public static void Main(string[] args)
        // {
        //     const string dll = @"C:\Users\burak\Desktop\fuck_burak\Release\fuck_burak.dll";
        //     const string srPath = @"C:\Program Files (x86)\SilkroadTR";
        //
        //     var thrd = new Thread(ProxyThread);
        //     thrd.Start();
        // }
        //
        // public static void ProxyThread()
        // {
        //     Console.WriteLine("start");
        //     
        //     var server = new TcpListener(IPAddress.Parse("127.0.0.1"), 60070);
        //     server.Start();
        //     var client = server.AcceptTcpClient();
        //     server.Stop();
        //
        //     Console.WriteLine("done");
        // }
    }
}