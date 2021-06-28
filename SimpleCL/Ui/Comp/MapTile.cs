using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleCL.Ui.Comp
{
	public class MapTile : PictureBox
	{
		public int SectorX { get; }
		public int SectorY { get; }
		public MapTile(int sectorX, int sectorY)
		{
			base.DoubleBuffered = true;
			SectorX = sectorX;
			SectorY = sectorY;
		}
		public async void LoadAsyncTile(string path,Size size)
		{
			if (File.Exists(path))
			{
				Image = await Task.Run( () => GetTile(path, size));
			}
		}
		private Bitmap GetTile(string path, Size size)
		{
			return new Bitmap(Image.FromFile(path), size);
		}

		public override string ToString()
		{
			return SectorX + " " + SectorY + " Size: " + Size;
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			((System.ComponentModel.ISupportInitialize) (this)).BeginInit();
			this.SuspendLayout();
			((System.ComponentModel.ISupportInitialize) (this)).EndInit();
			this.ResumeLayout(false);
		}
	}
}