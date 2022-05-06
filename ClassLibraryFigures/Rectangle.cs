using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Figures.ClassLibraryFigures
{
    [Serializable]
    public class Rectangle : Figure, ISerializable
    {
        public event onClickDelegate onClick;

        public Rectangle()//construct
        {

        }

        private Rectangle(SerializationInfo info, StreamingContext context)//custom serialization construct
        {
            Location = (Point)info.GetValue("loc", Location.GetType());
            BackColor = (Color)info.GetValue("col", BackColor.GetType());
            Selected = info.GetBoolean("sel");
            Height = info.GetInt16("hei");
            Width = info.GetInt16("wid");
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo info,StreamingContext context)//ISerializable impl
        {
            info.AddValue("loc", Location);
            info.AddValue("col", BackColor);
            info.AddValue("sel", Selected);
            info.AddValue("hei", Height);
            info.AddValue("wid", Width);
        }

        public override void Paint(Graphics g)//paint
        {
            var BorderColor = Selected
                ? Color.Red
                : BackColor;

            if (Filled)
                using (var brush = new SolidBrush(BackColor))
                    g.FillRectangle(brush, Location.X, Location.Y, Width, Height);

            using (var pen = new Pen(BorderColor, 3))
                g.DrawRectangle(pen, Location.X, Location.Y, Width, Height);
        }

        public override void Move(int newX, int newY)//move
        {
            Location = new Point(newX, newY);
        }

        public override bool ContainsPoint(Point p)//contains/onClick
        {
            var temp = 
                Location.X < p.X && p.X < Location.X + Width &&
                Location.Y < p.Y && p.Y < Location.Y + Height;

            if (temp)
                onClick?.Invoke();//triggers onClick

            return temp;
        }

        public override int GetArea()//area
        {
            return Width * Height;
        }

        public override bool Intersects(Rectangle frame)//intersection
        {
            return
                this.Location.X < frame.Location.X + frame.Width &&
                frame.Location.X < this.Location.X + this.Width &&
                this.Location.Y < frame.Location.Y + frame.Height &&
                frame.Location.Y < this.Location.Y + this.Height;
        }
    }
}
