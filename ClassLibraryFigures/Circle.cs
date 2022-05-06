using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Figures.ClassLibraryFigures
{
    [Serializable]
    public class Circle : Figure, ISerializable
    {
        public event onClickDelegate onClick;

        private int _radius { get; set; }//Essential for the ContainsPoint method & the Intersects method
        public int Radius { get; set; }

        public Circle()//construct
        {

        }

        public Circle(SerializationInfo info, StreamingContext context)//...
        {
            Location = (Point)info.GetValue("loc", Location.GetType());
            BackColor = (Color)info.GetValue("col", BackColor.GetType());
            Selected = info.GetBoolean("sel");
            Height = info.GetInt16("hei");
            Width = info.GetInt16("wid");
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)//...
        {
            info.AddValue("loc", Location);
            info.AddValue("col", BackColor);
            info.AddValue("sel", Selected);
            info.AddValue("hei", Height);
            info.AddValue("wid", Width);
        }

        public override void Paint(Graphics g)//...
        {
            Radius = CircleExtensions.GenerateRadius(Width);

            var BorderColor = Selected
                ? Color.Red
                : BackColor;

            if (Filled)
                using (var brush = new SolidBrush(BackColor))
                    g.FillEllipse(brush, Location.X, Location.Y, Width, Width);

            using (var pen = new Pen(BorderColor, 3))
                g.DrawEllipse(pen, Location.X, Location.Y, Width, Width);

            /*using (var brush = new SolidBrush(Color.Orange)) //center&rad ilustration
            {
                g.FillEllipse(brush, Location.X + (Width / 2), Location.Y + (Width / 2), 10, 10);
                g.FillEllipse(brush, Location.X + (Width / 2)+Radius, Location.Y + (Width / 2), 10, 10);
            }*/
        }

        public override void Move(int newX, int newY)//...
        {
            Location = new Point(newX, newY);
        }

        public override bool ContainsPoint(Point p)//...
        {
            Radius = CircleExtensions.GenerateRadius(Width);//recalc rad

            var DeltaX = Location.X + (Width/2) - p.X;//calc delta
            var DeltaY = Location.Y +(Width/2) - p.Y;
            var temp = (DeltaX * DeltaX + DeltaY * DeltaY) < (Radius * Radius);//point is within if delta < rad

            if (temp)
                onClick?.Invoke();//triggers onClick

            return temp;
        }

        public override int GetArea()//...
        {
            Radius = CircleExtensions.GenerateRadius(Width);

            return (int)3.14 * Radius * Radius;
        }

        public override bool Intersects(Rectangle frame)//...
        {
            /* var NearestX = Math.Max(frame.Location.X, Math.Min(Location.X + (Width / 2), frame.Location.X + frame.Width)); //calculates the nearest point regarding to the frame/rectangle
             var NearestY = Math.Max(frame.Location.Y, Math.Min(Location.Y + (Width / 2), frame.Location.Y + frame.Height));*/
            Radius = CircleExtensions.GenerateRadius(Width);//recalc rad

            var DeltaX = Location.X + (Width / 2) - Math.Max(frame.Location.X, Math.Min(Location.X + (Width / 2), frame.Location.X + frame.Width));//calc delta
            var DeltaY = Location.Y + (Width / 2) - Math.Max(frame.Location.Y, Math.Min(Location.Y + (Width / 2), frame.Location.Y + frame.Height));

            return (DeltaX * DeltaX + DeltaY * DeltaY) < (Radius * Radius);//intersection when delta is within
        }
    }
}
