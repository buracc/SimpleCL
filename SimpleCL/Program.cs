using System.Collections.Generic;
using System.Windows.Forms;
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
    }
}