using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SimpleCL.Ui.Components
{
    public enum ProgressBarDisplayMode
    {
        NoText,
        Percentage,
        CurrProgress,
        CustomText,
        TextAndPercentage,
        TextAndCurrProgress
    }

    public class TextProgressBar : ProgressBar
    {
        [Description("Font of the text on ProgressBar"), Category("Additional Options")]
        public Font TextFont { get; set; } = new Font(FontFamily.GenericSansSerif, 8, FontStyle.Regular);

        private SolidBrush _textColourBrush = (SolidBrush) Brushes.Black;

        [Category("Additional Options")]
        public Color TextColor
        {
            get => _textColourBrush.Color;
            set
            {
                _textColourBrush.Dispose();
                _textColourBrush = new SolidBrush(value);
            }
        }

        private SolidBrush _progressColourBrush = (SolidBrush) Brushes.LightGreen;

        [Category("Additional Options"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public Color ProgressColor
        {
            get => _progressColourBrush.Color;
            set
            {
                _progressColourBrush.Dispose();
                _progressColourBrush = new SolidBrush(value);
            }
        }

        private ProgressBarDisplayMode _visualMode = ProgressBarDisplayMode.CurrProgress;

        [Category("Additional Options"), Browsable(true)]
        public ProgressBarDisplayMode VisualMode
        {
            get => _visualMode;
            set
            {
                _visualMode = value;
                Invalidate(); //redraw component after change value from VS Properties section
            }
        }

        private string _text = string.Empty;

        [Description("If it's empty, % will be shown"), Category("Additional Options"), Browsable(true),
         EditorBrowsable(EditorBrowsableState.Always)]
        public string CustomText
        {
            get => _text;
            set
            {
                _text = value;
                Invalidate(); //redraw component after change value from VS Properties section
            }
        }

        private string _textToDraw
        {
            get
            {
                var text = VisualMode switch
                {
                    ProgressBarDisplayMode.Percentage => PercentageStr,
                    ProgressBarDisplayMode.CurrProgress => CurrProgressStr,
                    ProgressBarDisplayMode.TextAndCurrProgress => $"{CustomText}: {CurrProgressStr}",
                    ProgressBarDisplayMode.TextAndPercentage => $"{CustomText}: {PercentageStr}",
                    _ => CustomText
                };

                return text;
            }
            set => _text = value;
        }

        private string PercentageStr => $"{(int) ((float) Value - Minimum) / ((float) Maximum - Minimum) * 100} %";

        private string CurrProgressStr => $"{Value}/{Maximum}";

        public TextProgressBar()
        {
            Value = Minimum;
            FixComponentBlinking();
        }

        private void FixComponentBlinking()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer,
                true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;

            DrawProgressBar(g);

            DrawStringIfNeeded(g);
        }

        private void DrawProgressBar(Graphics g)
        {
            var rect = ClientRectangle;

            ProgressBarRenderer.DrawHorizontalBar(g, rect);

            rect.Inflate(-3, -3);

            if (Value <= 0)
            {
                return;
            }

            var clip = new Rectangle(rect.X, rect.Y, (int) Math.Round(((float) Value / Maximum) * rect.Width),
                rect.Height);

            g.FillRectangle(_progressColourBrush, clip);
        }

        private void DrawStringIfNeeded(Graphics g)
        {
            if (VisualMode == ProgressBarDisplayMode.NoText)
            {
                return;
            }

            var text = _textToDraw;

            var len = g.MeasureString(text, TextFont);

            var location = new Point(Width / 2 - (int) len.Width / 2, Height / 2 - (int) len.Height / 2);

            g.DrawString(text, TextFont, _textColourBrush, location);
        }

        public new void Dispose()
        {
            _textColourBrush.Dispose();
            _progressColourBrush.Dispose();
            base.Dispose();
        }
    }
}