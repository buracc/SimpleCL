using System.Windows.Forms;
using SimpleCL.Ui;

namespace SimpleCL
{
    // ReSharper disable once InconsistentNaming
    internal static class Program
    {
        public static Gui Gui { get; private set; }

        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(Gui = new Gui());
        }

        // public static void Main(string[] args)
        // {
        // }
    }
}