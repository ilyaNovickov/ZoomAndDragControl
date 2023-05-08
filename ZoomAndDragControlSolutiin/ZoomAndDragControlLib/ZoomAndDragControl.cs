using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace ZoomAndDragControlLib
{
    //Класс элемента управления с поддержко перетаскивания и маштабирования
    [DefaultEvent("MouseClick")]//Установка события по умолчанию
    [Designer(typeof(ZoomAndDragControlDesigner))]
    public partial class ZoomAndDragControl : Control
    {
        #region Конструктор
        //Конструктор
        public ZoomAndDragControl()
        {
            InitializeComponent();
            //Установка флагов для рисования элемента
            //UserPaint - элемент сам рисует себя
            //Selectable - элемент может иметь фокус
            //ResizeRedraw - элемент перерисовывается при изменении своего размера
            //OptimizedDoubleBuffer - элемент сначала рисуется в буфера, а потом на экране
            //AllPaintingInWmPaint - OnPaint и OnPaintBackground вызываются по вызовы окна
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw |
                ControlStyles.Selectable | ControlStyles.UserPaint, true);
            this.DoubleBuffered = true;
        }
        #endregion
        #region Данные
        float zoom = 1.00f;//Значение маштаба элемента
        float maxZoom = 10f;
        float minZoom = 0.02f;
        bool grid = true;//Включена ли сетка
        readonly Matrix matrix = new Matrix();//Матрица трансфориации (для получения преобразованных координат)
        readonly Matrix inverseMatrix = new Matrix();//Обратная матрица трансформации (для получения обычных координат)
        PointF translation = new PointF();//Вектор смещения 
        Point lastLocation;//Последние координаты курсора мыши
        Color smallGridColor = Color.White;//Цвет маленькой решётки
        Color bigGridColor = Color.Gray;//Цвет большой решётки
        int smallGridStep = 12;//Шаг маленькой решётки
        int bigGridStep = 60;//Шаг большой решётки
        #endregion
        #region Свойства
        /// <summary>
        /// Значение маштаба элемента управления
        /// </summary>
        [Category("UserProperties")]//Установка атрибута категории
        [Description("Значение маштаба элемента управления")]//Установка атрибута описания
        [DefaultValue(1.00f)]//Установка атрибута значения по умолчанию
        public float Zoom
        {
            //Метод возвращающий значение свойства
            get { return zoom; }
            set //Метод устанавливающий значение свойства
            { 
                if (value < minZoom)//Если значение маштаба меньше минимального
                    zoom = minZoom;//Установка минимального значения
                else if (value > maxZoom)//Иначе если значение маштаба больше максимального
                    zoom = maxZoom;//Установка максимального значения 
                else
                    zoom = value;//Установка значения
                //Если событие изменения маштаба не пустое, то
                if (zoomChanged != null)//Генерация события изменения значения маштаба
                    zoomChanged(this, EventArgs.Empty);
                this.Invalidate();//Вызов перерисовки
            }
        }
        /// <summary>
        /// Максимальное значение маштаба элемента управления
        /// </summary>
        [Category("UserProperties")]//Установка атрибута категории
        [Description("Минимальное значение маштаба элемента управления")]//Установка атрибута описания
        [DefaultValue(10.00f)]//Установка атрибута значения по умолчанию
        public float MaxZoom
        {
            //Метод возвращающий значение свойства
            get { return maxZoom; }
            set //Метод устанавливающий значение свойства
            {
                if (value <= 0)//Если значение отрицательное, то ошибка
                    throw new Exception("Маштаб не может быть отрицательным");
                if (value < minZoom)//Если минимальное значение м аштаба больше максимального, то ошибка
                    throw new Exception("Максимальное значение маштаба не может быть меньше минимального");
                maxZoom = value;//Установка значения
            }
        }
        /// <summary>
        /// Минимальное значение маштаба элемента управления
        /// </summary>
        [Category("UserProperties")]//Установка атрибута категории
        [Description("Минимальное значение маштаба элемента управления")]//Установка атрибута описания
        [DefaultValue(0.02f)]//Установка атрибута значения по умолчанию
        public float MinZoom
        {
            //Метод возвращающий значение свойства
            get { return minZoom; }
            set //Метод устанавливающий значение свойства
            {
                if (value <= 0)//Если значение отрицательное, то ошибка
                    throw new Exception("Маштаб не может быть отрицательным");
                if (value > maxZoom)//Если минимальное значение м аштаба больше максимального, то ошибка
                    throw new Exception("Минимальное значение маштаба не может быть меньше мальсимального");
                minZoom = value;//Установка значения
            }
        }
        /// <summary>
        /// Свойство определяющее включена ли сетка
        /// </summary>
        [Category("UserProperties")]//Установка атрибута категории
        [Description("Свойство определяющее включена ли сетка")]//Установка атрибута описания
        [DefaultValue(true)]//Установка атрибута значения по умолчанию
        [TypeConverter(typeof(BooleanTypeConverter))]
        public bool Grid
        {
            get { return grid; }//Метод возвращающий значение свойства
            set { grid = value; this.Invalidate(); }//Метод устанавливающий значение свойства
        }
        /// <summary>
        /// Свойство определяющее цвет маленькой сетки
        /// </summary>
        [Category("UserProperties")]//Установка атрибута категории
        [Description("Свойство определяющее цвет маленькой сетки")]//Установка атрибута описания
        [DefaultValue("Color.White")]//Установка атрибута значения по умолчанию
        public Color SmallGridColor
        {
            get { return smallGridColor; }//Метод возвращающий значение свойства
            set { smallGridColor = value; this.Invalidate(); }//Метод устанавливающий значение свойства
                                        //Перерисовка
        }
        /// <summary>
        /// Свойство определяющее цвет большой сетки
        /// </summary>
        [Category("UserProperties")]//Установка атрибута категории
        [Description("Свойство определяющее шаг маленькой сетки")]//Установка атрибута описания
        [DefaultValue("Color.Gray")]//Установка атрибута значения по умолчанию
        public Color BigGridColor
        {
            get { return bigGridColor; }//Метод возвращающий значение свойства
            set { bigGridColor = value; this.Invalidate(); }//Метод устанавливающий значение свойства
                                        //Перерисовка
        }
        /// <summary>
        /// Свойство определяющее шаг маленькой сетки
        /// </summary>
        [Category("UserProperties")]//Установка атрибута категории
        [Description("Свойство определяющее шаг маленькой сетки")]//Установка атрибута описания
        [DefaultValue(12)]//Установка атрибута значения по умолчанию
        public int SmallGridStep
        {
            get { return smallGridStep; }//Метод возвращающий значение свойства
            set//Метод устанавливающий значение свойства
            {
                if (value < 12)//Если значение <12
                {
                    smallGridStep = 12;//то установказначения 12
                    this.Invalidate();//вызов перерисовки
                    return;//Выход из метода
                }
                smallGridStep = value;//Установка значения 
                this.Invalidate();//Вызов перерисовки
            }
        }
        /// <summary>
        /// Свойство определяющее шаг большой сетки
        /// </summary>
        [Category("UserProperties")]//Установка атрибута категории
        [Description("Свойство определяющее шаг маленькой сетки")]//Установка атрибута описания
        [DefaultValue(60)]//Установка атрибута значения по умолчанию
        public int BigGridStep
        {
            get { return bigGridStep; }//Метод возвращающий значение свойства
            set//Метод устанавливающий значение свойства
            {
                if (value < 60)//Если значение <60
                {
                    bigGridStep = 60;
                    this.Invalidate();//вызов перерисовки
                    return;//Выход из метода
                }
                bigGridStep = value;//Установка значения 
                this.Invalidate();//Вызов перерисовки
            }
        }
        /// <summary>
        /// Свойство определяющее кнопку мыши для перетаскивания элемента управления
        /// </summary>
        [Category("UserProperties")]//Установка атрибута категории
        [Description("Свойство определяющее шаг маленькой сетки")]//Установка атрибута описания
        [DefaultValue("MouseButtons.None")]//Установка атрибута значения по умолчанию
        public MouseButtons DragMouseButtons
        {
            get; 
            set;//Методы возвращающие и устанавливающие значение свойства
        }
        #endregion
        #region События
        //Экземпляры событий
        event EventHandler<TransformedMouseEventArgs> mouseClick;
        event EventHandler<TransformedMouseEventArgs> mouseUp;
        event EventHandler<TransformedMouseEventArgs> mouseDown;
        event EventHandler<TransformedMouseEventArgs> mouseMove;
        event EventHandler zoomChanged;
        /// <summary>
        /// Событие клика мышью
        /// </summary>
        [Description("Событие клика мышью")]//Установка атрибута для описания события
        public new event EventHandler<TransformedMouseEventArgs> MouseClick
        {
            add { mouseClick += value; }//Добавление делегата к событию
            remove { mouseClick -= value; }//Удаление делегата из события
        }
        /// <summary>
        /// Событие отжатия кнопки мыши
        /// </summary>
        [Description("Событие отжатия кнопки мыши")]//Установка атрибута для описания события
        public new event EventHandler<TransformedMouseEventArgs> MouseUp
        {
            add { mouseUp += value; }//Добавление делегата к событию
            remove { mouseUp -= value; }//Удаление делегата из события
        }
        /// <summary>
        /// Событие нажатия кнопки мыши
        /// </summary>
        [Description("Событие нажатия кнопки мыши")]//Установка атрибута для описания события
        public new event EventHandler<TransformedMouseEventArgs> MouseDown
        {
            add { mouseDown += value; }//Добавление делегата к событию
            remove { mouseDown -= value; }//Удаление делегата из события
        }
        /// <summary>
        /// Событие движения курсора мыши
        /// </summary>
        [Description("Событие движения курсора мыши")]//Установка атрибута для описания события
        public new event EventHandler<TransformedMouseEventArgs> MouseMove
        {
            add { mouseMove += value; }//Добавление делегата к событию
            remove { mouseMove -= value; }//Удаление делегата из события
        }
        /// <summary>
        /// Событие изменения маштаба элемента управления
        /// </summary>
        [Description("Событие изменения маштаба элемента управления")]//Установка атрибута для описания события
        public event EventHandler ZoomChanged
        {
            add { zoomChanged += value; }//Добавление делегата к событию
            remove { zoomChanged -= value; }//Удаление делегата из события
        }
        #endregion
        #region Для матриц
        //Метод изменения значений матриц преобразования
        void UpdateMatrices()
        {
            //Иницилизация значения центра элемента управления
            PointF center = new PointF(this.Width / 2.0f, this.Height / 2.0f);
            //Для матрицы преобразования
            matrix.Reset();//Сброс матрицы до еденичной
            matrix.Translate(translation.X, translation.Y);//Добавление смещения к матрице
            matrix.Translate(center.X, center.Y);//Добавление смещения к центру
            matrix.Scale(zoom, zoom);//Добавление маштаба к матрице
            matrix.Translate(-center.X, -center.Y);//Удаление смещения к центру с учётом маштаба
            //Для обратной матрицы преобразования
            inverseMatrix.Reset();//Сброс матрицы до еденичной
            inverseMatrix.Translate(center.X, center.Y);//Добавление смещения к матрице
            inverseMatrix.Scale(1.0f / zoom, 1.0f / zoom);//Добавление обратного маштаба к матрице
            inverseMatrix.Translate(-center.X, -center.Y);//Удаление смещения к центру с применением маштаба
            inverseMatrix.Translate(-translation.X, -translation.Y);//Добавление обратного смещения к матрице
        }
        /// <summary>
        /// Метод сброса матрицы преобразования
        /// </summary>
        public void ResetMatrix()
        {
            //Вектор перемещения равень нулю
            translation = PointF.Empty;
            //Маштаб равень 100%
            Zoom = 1f;
            //Обновления матрицы преобразования
            UpdateMatrices();
        }
        #endregion
        #region Рисование
        //Рисование элемента
        protected override void OnPaint(PaintEventArgs e)
        {
            /*
                //Настройка класса Graphics
                e.Graphics.PageUnit = GraphicsUnit.Pixel;//Еденицы измерения координат - пиксели
                e.Graphics.CompositingQuality = CompositingQuality.GammaCorrected;//Качество орисовки - с гаммо-коррекцией
                e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;//Порядок смещения пикселя для орисовки - высокий 
                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;//Интерполяция - высокого качества, бикубическая
                //Изменение значений матриц преобразования
                UpdateMatrices();
                e.Graphics.Transform = matrix;//Преобразование класса Graphics
                //для рисования в соотведсвии с матрицей преобразования
                if (grid == true)//Если сетка - вкл
                    DrawGrid(e);//Рисовать сетку
                //Методы проверки рисования
                e.Graphics.DrawString("HelloWorld!", new Font(new FontFamily(GenericFontFamilies.SansSerif), 72),
                    new SolidBrush(Color.Red), new Point(0, 0));
                e.Graphics.FillRectangle(new SolidBrush(Color.Yellow), new Rectangle(0, 0, 100, 100));
            */
            UpdateMatrices();//Обновление значений матриц
            e.Graphics.Transform = matrix;//Преобразование класса Graphics
                                          //для рисования в соотведсвии с матрицей преобразования
            e.Graphics.PageUnit = GraphicsUnit.Point;//Еденицы измерения координат - пиксели
            e.Graphics.CompositingQuality = CompositingQuality.AssumeLinear;//Качество орисовки - с гаммо-коррекцией
            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;//Порядок смещения пикселя для орисовки - высокий 
            e.Graphics.InterpolationMode = InterpolationMode.Low;//Интерполяция - высокого качества, бикубическая
            base.OnPaint(e);
        }
        //Рисование фона
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            UpdateMatrices();//Обновление значений матриц
            e.Graphics.Transform = matrix;//Преобразование класса Graphics
                                          //для рисования в соотведсвии с матрицей преобразования
            e.Graphics.PageUnit = GraphicsUnit.Point;//Еденицы измерения координат - пиксели
            e.Graphics.CompositingQuality = CompositingQuality.AssumeLinear;//Качество орисовки - с гаммо-коррекцией
            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;//Порядок смещения пикселя для орисовки - высокий 
            e.Graphics.InterpolationMode = InterpolationMode.Low;//Интерполяция - высокого качества, бикубическая
            if (grid == true)//Если сетка - вкл
                DrawGrid(e);//Рисовать сетку
        }
        //Рисование сетки для элемента
        protected void DrawGrid(PaintEventArgs e)
        {
            //Получение массива с координатами левого-верхнего и правого-нижнего угла
            //элемента управления (где будет рисование)
            PointF[] points = new PointF[]
            {
                new PointF(e.ClipRectangle.Left , e.ClipRectangle.Top),
                new PointF(e.ClipRectangle.Right, e.ClipRectangle.Bottom)
            };
            //Преобразование координат левого-верхнего и правого-нижнего угла элемента
            //в соотведствии с обратной матрицей преобразования
            inverseMatrix.TransformPoints(points);
            //Иницилизация преобразованных координат
            float left = points[0].X;//Преобразованное координаты левой части прямоугольника для рисования
            float right = points[1].X;//Преобразованное координаты правой части прямоугольника для рисования
            float top = points[0].Y;//Преобразованное координаты верхней части прямоугольника для рисования
            float bottom = points[1].Y;//Преобразованное координаты нижней части прямоугольника для рисования
            /*Маленькая сетка*/
            //Иницилизация смещения координат XY начала маленькой сетки
            float smallXOffset = ((float)Math.Round(left / smallGridStep) * smallGridStep);
            float smallYOffset = ((float)Math.Round(top / smallGridStep) * smallGridStep);
            //Рисование линий сетки по вертикали
            for (float x = smallXOffset; x < right; x += smallGridStep)
            {
                e.Graphics.DrawLine(new Pen(smallGridColor), x, top, x, bottom);
            }
            //Рисование линий сетки по горизонтали
            for (float y = smallYOffset; y < bottom; y += smallGridStep)
            {
                e.Graphics.DrawLine(new Pen(smallGridColor), left, y, right, y);
            }
            /*Большая сетка*/
            //Иницилизация смещения координат XY начала большой сетки
            float bigXOffset = ((float)Math.Round(left / bigGridStep) * bigGridStep);
            float bigYOffset = ((float)Math.Round(top / bigGridStep) * bigGridStep);
            //Рисование линий сетки по вертикали
            for (float x = bigXOffset; x < right; x += bigGridStep)
            {
                e.Graphics.DrawLine(new Pen(bigGridColor), x, top, x, bottom);
            }
            //Рисование линий сетки по горизонтали
            for (float y = bigYOffset; y < bottom; y += bigGridStep)
            {
                e.Graphics.DrawLine(new Pen(bigGridColor), left, y, right, y);
            }

        }
        #endregion
        #region Работа с мышью
        //Нажатие и удержание кнопки мыши на элемент
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            //Запоминание координат курсора мыши 
            lastLocation = e.Location;
            //Генерация события нажатия на мышь
            if (mouseDown != null)
                mouseDown(this, new TransformedMouseEventArgs(e, inverseMatrix));
        }
        //Движения курсора мыши по элементу
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            //Если кнопка мыши для перетаскивания не установлена
            if (e.Button == MouseButtons.None)
                return;//то выход из метода
            //Если нажата правая кнопка миши
            //то содержание панельки двигается
            if (e.Button == DragMouseButtons)
            {
                //Иницилизация текущего положения курсора
                Point currentLocation = e.Location;
                //Подсчёт изменения координат курсора мыши
                var deltaX = (lastLocation.X - currentLocation.X);
                var deltaY = (lastLocation.Y - currentLocation.Y);
                //Если есть изменение положения курсора мыши
                if ( (deltaX != 0) || (deltaY != 0) )
                {
                    //Изменение вектора смещения
                    translation.X -= deltaX;
                    translation.Y -= deltaY;
                    //Запоминание нового положения курсора мыши
                    lastLocation = currentLocation;
                    //Вызов перерисовки элемента
                    this.Refresh();
                }
            }
            //Генерация события движения мыши
            if (mouseMove != null)
                mouseMove(this, new TransformedMouseEventArgs(e, inverseMatrix));
        }
        //Отпускание кнопки мыши от элемента управления
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            //Генерация события отжатия мыши
            if (mouseUp != null)
                mouseUp(this, new TransformedMouseEventArgs(e, inverseMatrix));
        }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            //Генерация события клика мышью
            if (mouseClick != null)
                mouseClick(this, new TransformedMouseEventArgs(e, inverseMatrix));

        }
        #endregion
        #region Методы
        /// <summary>
        /// Создание экземпляра класса Graphics с учётом матриц преобразований элемента управления 
        /// </summary>
        /// <returns></returns>
        public Graphics CreateTransformedGraphics()
        {
            //Создание экземпляра класса Graphics у элемента
            Graphics g = this.CreateGraphics();
            g.Transform = matrix;//Преобразование класса Graphics
            return g;//Вовращение класса Graphics
        }
        /// <summary>
        /// Метод получения матрицы преобразования элемента управления
        /// </summary>
        /// <returns></returns>
        public Matrix GetTransformMatrix()
        {
            return matrix;
        }
        /// <summary>
        /// Метод получения обратной матрицы преобразования элемента управления
        /// </summary>
        /// <returns></returns>
        public Matrix GetInverseTransformMatrix()
        {
            return inverseMatrix;
        }
        #endregion
    }
}
