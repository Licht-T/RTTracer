using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestProgram
{
    class GraphViewer: Form
    {
        private Bitmap bitmap;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            if (this.bitmap != null)
            {
                g.DrawImage(this.bitmap, this.ClientRectangle);
            }
        }

        public GraphViewer(string title,Bitmap bitmap)
        {
            this.SetStyle(ControlStyles.ResizeRedraw, true);

            this.Text = title;
            this.bitmap = bitmap;

            //this.ClientSize = this.bitmap.Size;
            this.Invalidate();
        }

    }
}
