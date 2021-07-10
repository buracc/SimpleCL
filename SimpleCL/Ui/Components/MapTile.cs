using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using SimpleCL.Interaction.Pathing;
using SimpleCL.Models.Coordinates;

namespace SimpleCL.Ui.Components
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

        public async void LoadAsyncTile(string path, Size size)
        {
            if (File.Exists(path))
            {
                Image = await Task.Run(() => GetTile(path, size));
            }
        }

        private Bitmap GetTile(string path, Size size)
        {
            return new(Image.FromFile(path), size);
        }

        public override string ToString()
        {
            return SectorX + " " + SectorY + " Size: " + Size;
        }

        protected override void OnMove(EventArgs e)
        {
            RecreateHandle();
        }

        public void Destroy()
        {
            Image?.Dispose();
            Dispose(true);
        }
    }
}