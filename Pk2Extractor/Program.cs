namespace Pk2Extractor
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var pk2extractor = new Api.Pk2Extractor(@"C:\Program Files (x86)\SilkroadTR\Media.pk2", @"C:\Users\burak\AppData\Local\Programs\phBot Testing\Data\TRSRO.db3");
            pk2extractor.LoadNameReferences();
            // pk2extractor.LoadTextReferences();
            // pk2extractor.AddTextReferences();
            pk2extractor.StoreModels();
            // pk2extractor.AddItems();
        }
    }
}