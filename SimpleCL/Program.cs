using System;
using System.IO;
using System.Windows.Forms;
using SimpleCL.Ui;
using SimpleCL.Util;

namespace SimpleCL
{
    internal class Program
    {
        public static Gui Gui { get; set; }

        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(Gui = new Gui());
        }

        // public static void Main(string[] args)
        // {
        //     Console.WriteLine(DirectoryUtils.GetDbFile("TRSRO"));
        // }
    }
}