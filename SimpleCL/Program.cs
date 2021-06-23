using System;
using System.Windows.Forms;
using SimpleCL.Enums.Item;
using SimpleCL.Enums.Item.Type;
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
        //     Console.WriteLine(ItemParser.GetItemSubType(ItemCategory.Equipment, 6, 2).GetType());
        // }
    }
}