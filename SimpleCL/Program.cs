using System;
using System.Windows.Forms;
using SimpleCL.Database;
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
        //     Console.WriteLine(GameDatabase.GetInstance().GetItemData(575)["name"]);
        // }
    }
}