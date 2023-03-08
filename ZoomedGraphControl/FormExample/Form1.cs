using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormExample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            zoomedGraphControl1.Zoom = trackBar1.Value / 100f;
        }

        private void zoomedGraphControl1_Click(object sender, EventArgs e)
        {
            
        }

        private void zoomedGraphControl1_MouseClick(object sender, MouseEventArgs e)
        {
            //if(e.Button == MouseButtons.Left)
            //    MessageBox.Show(e.Location.ToString());
            //ZoomAndDragContolLib.TransformedMouseEventArgs e2 = new ZoomAndDragContolLib.TransformedMouseEventArgs(e, zoomedGraphControl1.inverseMatrix);
            //if (e.Button == MouseButtons.Left)
            //    MessageBox.Show(e2.Location.ToString());
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var g = zoomedGraphControl1.CreateGraphics();
            g.FillEllipse(new SolidBrush(Color.AliceBlue), new RectangleF(0, 0, 100, 100));
            var g2 = zoomedGraphControl1.CreateTransformedGraphics();
            g2.FillEllipse(new SolidBrush(Color.AliceBlue), new RectangleF(0, 0, 100, 100));
            zoomedGraphControl1.Zoom = 1f;
        }

        private void zoomedGraphControl1_MouseClick(object sender, ZoomAndDragContolLib.TransformedMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                MessageBox.Show(e.OriginalLocation.ToString());
            //ZoomAndDragContolLib.TransformedMouseEventArgs e2 = new ZoomAndDragContolLib.TransformedMouseEventArgs(e, zoomedGraphControl1.inverseMatrix);
            if (e.Button == MouseButtons.Left)
                MessageBox.Show(e.Location.ToString());
        }

        private void zoomedGraphControl1_MouseEnter(object sender, EventArgs e)
        {

        }
    }
}
