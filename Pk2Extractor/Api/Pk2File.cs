namespace Pk2Extractor.Api
{
	public class Pk2File
	{
		public string Name { get; set; }
		public long Position { get; set; }
		public uint Size { get; set; }
		public Pk2Folder ParentFolder { get; set; }
	}
}