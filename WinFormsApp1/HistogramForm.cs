using System;
using System.Collections.Generic;
using System.Text;

namespace WinFormsApp1
{
    internal class HistogramForm: Form
    {
        private readonly Dictionary<string, int> data;

        public HistogramForm(Dictionary<string, int> data)
        {
            this.data = data;
            Text = "Распределение студентов по специальностям";
            ClientSize = new Size(600, 400);
            DoubleBuffered = true;
            ResizeRedraw = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (data.Count == 0) return;

            int max = data.Values.Max();
            int barWidth = ClientSize.Width / data.Count;
            int bottom = ClientSize.Height - 40;
            int i = 0;

            foreach (var pair in data)
            {
                int h = (int)((bottom - 40) * (double)pair.Value / max);
                var rect = new Rectangle(i * barWidth + 10, bottom - h, barWidth - 20, h);

                e.Graphics.FillRectangle(Brushes.SteelBlue, rect);
                e.Graphics.DrawString(pair.Value.ToString(), Font, Brushes.Black, rect.X, rect.Y - 18);
                e.Graphics.DrawString(pair.Key, Font, Brushes.Black, rect.X, bottom + 5);
                i++;
            }
        }
    }
}
