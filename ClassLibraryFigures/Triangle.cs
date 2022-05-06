using System;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Figures.ClassLibraryFigures
{
    [Serializable]
    public class Triangle : Figure, ISerializable
    {
        public event onClickDelegate onClick;

        private Point[] _points { get; set; }//Essential for drawing the triangle & the ContainsPoint method & the Intersects method
        public Point[] Points { get; set; }

        public Triangle()//construct
        {

        }

        public Triangle(SerializationInfo info, StreamingContext context)//...
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
            Points = TriangleExtensions.GeneratePoints(Location, Width, Height);//recalc points

            var BorderColor = Selected
                ? Color.Red
                : BackColor;

            if (Filled)
                using (var brush = new SolidBrush(BackColor))
                    g.FillPolygon(brush, Points);

            using (var pen = new Pen(BorderColor, 3))
                g.DrawPolygon(pen,Points);
        }

        public override void Move(int newX, int newY)//...
        {
            Location = new Point(newX, newY);
        }

        public override bool ContainsPoint(Point p)//...
        {
            Points = TriangleExtensions.GeneratePoints(Location, Width, Height);//recalc points

            float d1, d2, d3;
            bool has_neg, has_pos;

            d1 = TriangleExtensions.Sign(p, Points[0], Points[1]);
            d2 = TriangleExtensions.Sign(p, Points[1], Points[2]);
            d3 = TriangleExtensions.Sign(p, Points[2], Points[0]);

            has_neg = (d1 < 0) || (d2 < 0) || (d3 < 0);
            has_pos = (d1 > 0) || (d2 > 0) || (d3 > 0);

            var temp = !(has_neg && has_pos);

            if (temp)
                onClick?.Invoke();//triggers onClick

            return temp;
        }

        public override int GetArea()//...
        {
            Points = TriangleExtensions.GeneratePoints(Location, Width, Height);

            return  Math.Abs(Points.Take(Points.Length - 1)
                   .Select((p, i) => (Points[i + 1].X - p.X) * (Points[i + 1].Y + p.Y))
                   .Sum() / 2); ;
        }

        public override bool Intersects(Rectangle frame)//...
        {
            Points = TriangleExtensions.GeneratePoints(Location, Width, Height);//recalc points

            bool temp = false;
            for (int i = 0; i < 3; i++)
            {
                if (temp == true)
                    break;

                var p1 = Points[i];
                var p2 = Points[i];
                if (i == 2)
                {
                    p2 = Points[0];
                }
                else
                {
                    p2 = Points[i + 1];
                }

                temp = TriangleExtensions.LineIntersectsLine(p1, p2, new Point(frame.Location.X, frame.Location.Y), new Point(frame.Location.X + frame.Width, frame.Location.Y)) ||
                TriangleExtensions.LineIntersectsLine(p1, p2, new Point(frame.Location.X + frame.Width, frame.Location.Y), new Point(frame.Location.X + frame.Width, frame.Location.Y + frame.Height)) ||
                TriangleExtensions.LineIntersectsLine(p1, p2, new Point(frame.Location.X + frame.Width, frame.Location.Y + frame.Height), new Point(frame.Location.X, frame.Location.Y + frame.Height)) ||
                TriangleExtensions.LineIntersectsLine(p1, p2, new Point(frame.Location.X, frame.Location.Y + frame.Height), new Point(frame.Location.X, frame.Location.Y)) ||
                (frame.ContainsPoint(p1) && frame.ContainsPoint(p2));
            }
            return temp;
        }
    }
}
