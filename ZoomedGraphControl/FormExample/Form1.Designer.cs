namespace FormExample
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.zoomedGraphControl1 = new ZoomAndDragContolLib.ZoomAndDragControl();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(49, 29);
            this.trackBar1.Maximum = 1000;
            this.trackBar1.Minimum = 2;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(256, 56);
            this.trackBar1.TabIndex = 1;
            this.trackBar1.Value = 100;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Silver;
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.trackBar1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 713);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1388, 134);
            this.panel1.TabIndex = 2;
            this.panel1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseClick);
            this.panel1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseClick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(624, 29);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(124, 90);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // zoomedGraphControl1
            // 
            this.zoomedGraphControl1.BackColor = System.Drawing.Color.Black;
            this.zoomedGraphControl1.BigGridColor = System.Drawing.Color.Gray;
            this.zoomedGraphControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zoomedGraphControl1.DragMouseButtons = System.Windows.Forms.MouseButtons.Right;
            this.zoomedGraphControl1.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.zoomedGraphControl1.Location = new System.Drawing.Point(0, 0);
            this.zoomedGraphControl1.Name = "zoomedGraphControl1";
            this.zoomedGraphControl1.Size = new System.Drawing.Size(1388, 847);
            this.zoomedGraphControl1.SmallGridColor = System.Drawing.Color.White;
            this.zoomedGraphControl1.TabIndex = 0;
            this.zoomedGraphControl1.Text = "zoomedGraphControl1";
            this.zoomedGraphControl1.MouseClick += new System.EventHandler<ZoomAndDragContolLib.TransformedMouseEventArgs>(this.zoomedGraphControl1_MouseClick);
            this.zoomedGraphControl1.Click += new System.EventHandler(this.zoomedGraphControl1_Click);
            this.zoomedGraphControl1.MouseEnter += new System.EventHandler(this.zoomedGraphControl1_MouseEnter);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1388, 847);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.zoomedGraphControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ZoomAndDragContolLib.ZoomAndDragControl zoomedGraphControl1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
    }
}

