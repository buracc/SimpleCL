using System.IO;

namespace Pk2Extractor
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var pk2Path = @"C:\Program Files (x86)\SilkroadTR\Media.pk2";
            var phDbPath = @"C:\Users\burak\AppData\Local\Programs\phBot Testing\Data\TRSRO.db3";
            var dbPath = @"C:\Users\burak\AppData\Local\Programs\phBot Testing\Data\TRSRO_DB.db3";
            
            File.Copy(phDbPath, dbPath, true);
            File.SetAttributes(dbPath, File.GetAttributes(dbPath) & ~FileAttributes.ReadOnly);
            
            var pk2Extractor = new Api.Pk2Extractor(pk2Path, dbPath);
            pk2Extractor.StoreTextReferences();
            pk2Extractor.StoreModels();
            pk2Extractor.StoreTeleportBuildings();
            pk2Extractor.StoreTeleportLinks();
            // pk2extractor.AddItems();
        }
    }
}