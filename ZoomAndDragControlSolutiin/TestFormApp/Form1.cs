using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestFormApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.propertyGrid1.SelectedObject = this.zoomAndDragControl1;
        }

        private void zoomAndDragControl1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Red, 6), 0, 0, 500, 500);
        }
    }
}
