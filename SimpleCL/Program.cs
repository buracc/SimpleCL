using System;
using System.Windows.Forms;
using SimpleCL.Enums.Item;
using SimpleCL.Enums.Item.Type;
using SimpleCL.Model.Coord;
using SimpleCL.Ui;

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
        //     LocalPoint point = new LocalPoint(27244, 51, 1423, 180);
        //     WorldPoint wp = WorldPoint.FromLocal(point);
        //     LocalPoint local = LocalPoint.FromWorld(wp);
        //
        //     Console.WriteLine(point);
        //     Console.WriteLine(wp);
        //     Console.WriteLine(local);
        // }
    }
}