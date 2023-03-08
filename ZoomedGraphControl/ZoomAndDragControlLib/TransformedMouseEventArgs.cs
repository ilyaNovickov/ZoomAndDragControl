using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZoomAndDragContolLib
{
    //Класс для обработки событий связанных с мышью при преобразовании координат
    public class TransformedMouseEventArgs
    {
        #region Конструктор
        //Конструктор класса
        public TransformedMouseEventArgs(MouseEventArgs e, Matrix inverseMatrix)
        {
            //Иницилизация значений MouseEventArgs и обратной матрицы 
            this.e = e;
            this.inverseMatrix = inverseMatrix;
        }
        #endregion
        #region Данные
        MouseEventArgs e;//Класс MouseEventArgs
        Matrix inverseMatrix;//Обратная матрица для преобразования координат
        #endregion
        #region Свойства
        /// <summary>
        /// Преобразованная координата X
        /// </summary>
        public float X
        {
            get //Метод возвращающий значение свойства
            { 
                //Иницилизация массива, содержащего координату X
                PointF[] transformedPoint = new PointF[] { new Point(e.X, 0) };
                inverseMatrix.TransformPoints(transformedPoint);//Преобразование координаты
                return transformedPoint[0].X;//Вовращение значения координаты X
            }
        }
        /// <summary>
        /// Преобразованная координата Y
        /// </summary>
        public float Y
        {
            get//Метод возвращающий значение свойства
            {
                //Иницилизация массива, содержащего координату Y
                PointF[] transformedPoint = new PointF[] { new Point(0, e.Y) };
                inverseMatrix.TransformPoints(transformedPoint);//Преобразование координаты
                return transformedPoint[0].Y;//Вовращение значения координаты Y
            }
        }
        /// <summary>
        /// Преобразованное положение курсора мыши
        /// </summary>
        public PointF Location => new PointF(X, Y);
        /// <summary>
        /// Нажатая кнопка мыши
        /// </summary>
        public MouseButtons Button => e.Button;
        /// <summary>
        /// Оригинальная координата X
        /// </summary>
        public int OriginalX => e.X;
        /// <summary>
        /// Оригинальная координата Y
        /// </summary>
        public int OriginalY => e.Y;
        /// <summary>
        /// Ориганальное положение курсора мыши
        /// </summary>
        public Point OriginalLocation => new Point(e.X, e.Y);
        #endregion
    }
}
