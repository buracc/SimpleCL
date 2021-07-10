using System;
using System.Drawing;
using System.Windows.Forms;
using SimpleCL.Models.Character;

namespace SimpleCL.Ui.Components
{
    public class MapControl : Control
    {
        public Image Image { get; set; }

        public MapControl()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            base.BackColor = Color.Transparent;
            SetStyle(ControlStyles.ResizeRedraw, true);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x20;
                return cp;
            }
        }

        protected override void OnMove(EventArgs e)
        {
            try
            {
                RecreateHandle();
            }
            catch
            {
                // ignored
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Image != null)
            {
                e.Graphics.DrawImage(Image, 0, 0, Image.Size.Width, Image.Size.Height);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        public void Destroy()
        {
            Image?.Dispose();
            if (Tag is LocalPlayer)
            {
                return;
            }
            
            Dispose(true);
        }
    }
}