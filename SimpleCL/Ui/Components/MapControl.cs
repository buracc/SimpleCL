using System;
using System.Drawing;
using System.Windows.Forms;

namespace SimpleCL.Ui.Components
{
	public class MapControl : Control
	{
		private Image _image;
		public Image Image
		{
			get => _image;
			set {
				_image = value;
				try
				{
					RecreateHandle();
				}
				catch { }
			}
		}
		public MapControl()
		{
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			base.BackColor = Color.Transparent;
			SetStyle(ControlStyles.ResizeRedraw, true);
		}
		
		protected override CreateParams CreateParams
		{
			get {
				CreateParams cp = base.CreateParams;
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
			catch { }
		}
		
		protected override void OnPaint(PaintEventArgs e)
		{
			if (_image != null) e.Graphics.DrawImage(_image,0,0, _image.Size.Width, _image.Size.Height);
		}
		
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			// Do not paint background
		}
		
		public void RePaint()
		{
			try
			{
				RecreateHandle();
			}
			catch { }
		}
	}
}
