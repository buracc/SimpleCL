using System;
using System.IO;
using System.Linq;

namespace SimpleCL.Util
{
    public class DirectoryUtils
    {
        public static DirectoryInfo GetRootDir()
        {
            return Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent;
        }

        public static string[] GetDbFiles()
        {
            return Directory.GetFiles(GetRootDir().FullName, "*.db3", SearchOption.AllDirectories);
        }

        public static string GetDbFile(string server)
        {
            return GetDbFiles().First(x => x.Contains(server + ".db3"));
        }
    }
}