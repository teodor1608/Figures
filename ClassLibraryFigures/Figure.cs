using System;
using System.Drawing;

namespace Figures.ClassLibraryFigures
{
    public delegate void onClickDelegate();
    public delegate void onSizeChangedDelegate();
    public delegate void onLocationChangedDelegate();

    [Serializable]
    public class Figure
    {
        public event onSizeChangedDelegate onSizeChanged;
        public event onLocationChangedDelegate onLocationChanged;

        private Point _location { get; set; }
        public Point Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
                onLocationChanged?.Invoke();
            }
        }

        public Color BackColor { get; set; }

        public bool Filled { get; set; } = true;

        public bool Selected { get; set; }

        private int _height { get; set; }
        public int Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
                onSizeChanged?.Invoke();
            }
        }

        private int _width { get; set; }
        public int Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
                onSizeChanged?.Invoke();
            }
        }

        public virtual void Paint(Graphics g)
        {
            throw new Exception("Paint not overridden!");
        }

        public virtual int GetArea()
        {
            throw new Exception("GetArea not overridden!");
        }

        public virtual void Move(int newX, int newY)
        {
            throw new Exception("Move not overridden!");
        }

        public virtual bool ContainsPoint(Point p)
        {
            throw new Exception("ContainsPoint not overridden!");
        }

        public virtual bool Intersects(Rectangle frame)
        {
            throw new Exception("Intersects not overridden!");
        }
    }
}
