using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using SimpleCL.Enums.Item;
using SimpleCL.Enums.Item.Type;
using SimpleCL.Model.Coord;
using SimpleCL.Ui;
using SimpleCL.Util.Extension;

namespace SimpleCL
{
    // ReSharper disable once InconsistentNaming
    internal static class SimpleCL
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
        // }
    }
}