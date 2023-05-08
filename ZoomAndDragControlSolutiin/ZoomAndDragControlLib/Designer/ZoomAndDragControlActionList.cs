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
    internal class ZoomAndDragControlActionList : DesignerActionList
    {
        ZoomAndDragControl zdControl;
        DesignerActionUIService designerService;

        public ZoomAndDragControlActionList(IComponent component) : base(component)
        {
            this.zdControl = component as ZoomAndDragControl;
            this.designerService = this.GetService(typeof(DesignerActionUIService)) as DesignerActionUIService;
        }
        public bool Grid
        {
            get { return zdControl.Grid; }
            set { GetPropertyByName("Grid").SetValue(this.zdControl, value); this.designerService.Refresh(this.Component); }
        }
        public Color SmallGridColor
        {
            get { return zdControl.SmallGridColor; }
            set { GetPropertyByName("SmallGridColor").SetValue(this.zdControl, value); this.designerService.Refresh(this.Component); }
        }
        public Color BigGridColor
        {
            get { return zdControl.BigGridColor; }
            set { GetPropertyByName("BigGridColor").SetValue(this.zdControl, value); this.designerService.Refresh(this.Component); }
        }
        public void InvertColors()
        {
            Color tmp = this.SmallGridColor;
            this.SmallGridColor = this.BigGridColor;
            this.BigGridColor = tmp;
            this.designerService.Refresh(this.Component);
        }

        private PropertyDescriptor GetPropertyByName(string propName)
        {
            PropertyDescriptor prop = TypeDescriptor.GetProperties(zdControl)[propName];
            if (prop == null)
            {
                throw new ArgumentException("Свойство не существует", propName);
            }
            return prop;
        }
        public override DesignerActionItemCollection GetSortedActionItems()
        {
            DesignerActionItemCollection items = new DesignerActionItemCollection
            {
                new DesignerActionHeaderItem("Свойства", "Properties"),
                new DesignerActionHeaderItem("Методы", "Methods"),
                new DesignerActionHeaderItem("Информация", "Info")
                };
            items.Add(new DesignerActionPropertyItem("Grid", "Сетка", "Properties", "Включена ли сетка"));
            if (this.Grid)
            {
                items.Add(new DesignerActionPropertyItem("SmallGridColor", "Цвет маленькой сетки", "Properties", ""));
                items.Add(new DesignerActionPropertyItem("BigGridColor", "Цвет большой сетки", "Properties", ""));
            }
            if (this.BigGridColor != this.SmallGridColor)
                items.Add(new DesignerActionMethodItem(this, "InvertColors", "Перевернуть цвета", "Methods", "", false)); 
            string info = string.Format("Размер {0} х {1}", this.zdControl.Width, this.zdControl.Height);
            items.Add(new DesignerActionTextItem(info, "info"));
	        return items;
        }
    }
}

