namespace TestFormApp
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
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.zoomAndDragControl1 = new ZoomAndDragControlLib.ZoomAndDragControl();
            this.SuspendLayout();
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Right;
            this.propertyGrid1.Location = new System.Drawing.Point(690, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(202, 460);
            this.propertyGrid1.TabIndex = 1;
            // 
            // zoomAndDragControl1
            // 
            this.zoomAndDragControl1.BigGridColor = System.Drawing.Color.Gray;
            this.zoomAndDragControl1.DragMouseButtons = System.Windows.Forms.MouseButtons.None;
            this.zoomAndDragControl1.Location = new System.Drawing.Point(120, 92);
            this.zoomAndDragControl1.Name = "zoomAndDragControl1";
            this.zoomAndDragControl1.Size = new System.Drawing.Size(390, 280);
            this.zoomAndDragControl1.SmallGridColor = System.Drawing.Color.White;
            this.zoomAndDragControl1.TabIndex = 2;
            this.zoomAndDragControl1.Text = "zoomAndDragControl1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 460);
            this.Controls.Add(this.zoomAndDragControl1);
            this.Controls.Add(this.propertyGrid1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private ZoomAndDragControlLib.ZoomAndDragControl zoomAndDragControl1;
    }
}

