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
    internal class ZoomAndDragControlVerbsCollection : DesignerVerbCollection
    {
        ZoomAndDragControl zdControl;

        public ZoomAndDragControlVerbsCollection(IComponent zdControl)
        {
            this.zdControl = zdControl as ZoomAndDragControl;
            this.Add(new DesignerVerb("Перевернуть цвета", new EventHandler(OnInvertColors)));
            this.Add(new DesignerVerb("Вкл/Выкл сетку", new EventHandler(OnGridChanged)));
        }
        private void OnInvertColors(object sender, System.EventArgs e)
        {
            PropertyDescriptor smallColor = GetPropertyByName("SmallGridColor");
            PropertyDescriptor bigColor = GetPropertyByName("BigGridColor");
            Color tmp = (Color)smallColor.GetValue(zdControl);
            smallColor.SetValue(zdControl, bigColor.GetValue(zdControl));
            bigColor.SetValue(zdControl, tmp);
        }
        private void OnGridChanged(object sender, System.EventArgs e)
        {
            PropertyDescriptor grid = GetPropertyByName("Grid");
            bool tmp = (bool)grid.GetValue(zdControl);
            grid.SetValue(zdControl, !tmp);
            
        }
        private PropertyDescriptor GetPropertyByName(string propName)
        {
            PropertyDescriptor prop = TypeDescriptor.GetProperties(zdControl)[propName];
            if(prop == null)
            {
                throw new ArgumentException("Свойства не существует", propName);
            }
            return prop;
        }

    }
}
