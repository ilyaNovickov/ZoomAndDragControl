using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoomAndDragControlLib
{
    public class ZoomAndDragControlDesigner : System.Windows.Forms.Design.ControlDesigner
    {
        DesignerActionListCollection actionLists;
        DesignerVerbCollection verbs;
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (actionLists == null) // Если еще не создавали actionLists
                {
                    // Создаем список
                    actionLists = new DesignerActionListCollection();
                    // Добавляем тег
                    actionLists.Add(new ZoomAndDragControlActionList(this.Component));
                }
                return actionLists;
            }
        }
        public override DesignerVerbCollection Verbs
        {
            get
            {
                if (verbs == null)
                {
                    verbs = new ZoomAndDragControlVerbsCollection(this.Component);
                }
                return verbs;
            }
        }
        //From: https://learn.microsoft.com/ru-RU/dotnet/api/system.windows.forms.design.controldesigner?view=windowsdesktop-7.0
        #region
        private bool mouseover = false;
        private Color lineColor = Color.White;

        public ZoomAndDragControlDesigner()
        {

        }

        public Color OutlineColor
        {
            get
            {
                return lineColor;
            }
            set
            {
                lineColor = value;
            }
        }

        protected override void OnMouseEnter()
        {
            this.mouseover = true;
            this.Control.Refresh();
        }

        protected override void OnMouseLeave()
        {
            this.mouseover = false;
            this.Control.Refresh();
        }

        protected override void OnPaintAdornments(System.Windows.Forms.PaintEventArgs e)
        {
            if (this.mouseover)
            {
                e.Graphics.DrawRectangle(new Pen(new SolidBrush(this.lineColor), 6), 0, 0, this.Control.Size.Width,
                    this.Control.Size.Height);
            }
        }

        protected override void PreFilterProperties(System.Collections.IDictionary properties)
        {
            base.PreFilterProperties(properties);
            PropertyDescriptor pd = TypeDescriptor.CreateProperty(
                typeof(ZoomAndDragControlDesigner),
                "OutlineColor",
                typeof(System.Drawing.Color),
                new Attribute[] { new DesignOnlyAttribute(true) });

            properties.Add("OutlineColor", pd);
            //много
            properties.Remove("BackgroundImage");
        }
        #endregion 
    }
}
