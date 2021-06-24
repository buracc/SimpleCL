using System.Collections.Generic;

namespace Pk2Extractor.Api
{
	public class Pk2Folder
	{
		public string Name { get; set; }
		public long Position { get; set; }
		public List<Pk2File> Files { get; set; }
		public List<Pk2Folder> SubFolders { get; set; }
	}
}
