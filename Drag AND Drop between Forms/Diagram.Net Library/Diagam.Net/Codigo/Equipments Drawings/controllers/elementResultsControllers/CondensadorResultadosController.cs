using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Dalssoft.DiagramNet
{
	/// <summary>
	/// This class is the controller for ElipseElement
	/// </summary>
	internal class CondensadorResultadosController: RectangleController, IController
	{
		public CondensadorResultadosController(BaseElement element): base(element)
		{
		}
	
		#region IController Members

        public override bool HitTest(Point p)
        {
            GraphicsPath gp = new GraphicsPath();
            Matrix mtx = new Matrix();

            Point elLocation = el.Location;
            Size elSize = el.Size;
            gp.AddRectangle(new Rectangle(elLocation.X,
                elLocation.Y,
                elSize.Width,
                elSize.Height));
            gp.Transform(mtx);

            return gp.IsVisible(p);
        }

        public override bool HitTest(Rectangle r)
        {
            GraphicsPath gp = new GraphicsPath();
            Matrix mtx = new Matrix();

            Point elLocation = el.Location;
            Size elSize = el.Size;
            gp.AddRectangle(new Rectangle(elLocation.X,
                elLocation.Y,
                elSize.Width,
                elSize.Height));
            gp.Transform(mtx);
            Rectangle retGp = Rectangle.Round(gp.GetBounds());
            return r.Contains(retGp);
        }

		public override void DrawSelection(System.Drawing.Graphics g)
		{
			Color selColor = Color.Red;
			int border = 3;

            if ((el.Location.X > 0) && (el.Location.Y > 0) && (el.Size.Width > 0) && (el.Size.Height > 0))
            {

                Rectangle r = BaseElement.GetUnsignedRectangle(
                    new Rectangle(
                        el.Location.X - border, el.Location.Y - border,
                        el.Size.Width + (border * 2), el.Size.Height + (border * 2)));

                //HatchBrush brush = new HatchBrush(HatchStyle.SmallCheckerBoard, Color.Red, Color.Transparent);
                //Pen p = new Pen(brus, border);

                //Pen p=new Pen(Color,Anchura del pincel)
                Pen p1 = new Pen(Color.Red, 2);

                Point[] puntos = new Point[4];
                puntos[0].X = el.Location.X;
                puntos[0].Y = el.Location.Y + 2 * el.Size.Height / 8;
                puntos[1].X = el.Location.X;
                puntos[1].Y = el.Location.Y;
                puntos[2].X = el.Location.X + el.Size.Width;
                puntos[2].Y = el.Location.Y;
                puntos[3].X = el.Location.X + el.Size.Width;
                puntos[3].Y = el.Location.Y + 2 * el.Size.Height / 8;

                g.DrawLines(p1, puntos);

                Point[] puntos1 = new Point[12];
                puntos1[0].X = el.Location.X;
                puntos1[0].Y = el.Location.Y + 2 * el.Size.Height / 8;
                puntos1[1].X = el.Location.X + el.Size.Width / 6;
                puntos1[1].Y = el.Location.Y + 1 * el.Size.Height / 8;
                puntos1[2].X = el.Location.X + el.Size.Width / 6;
                puntos1[2].Y = el.Location.Y + 3 * el.Size.Height / 8;
                puntos1[3].X = el.Location.X + 2 * el.Size.Width / 6;
                puntos1[3].Y = el.Location.Y + 1 * el.Size.Height / 8;
                puntos1[4].X = el.Location.X + 2 * el.Size.Width / 6;
                puntos1[4].Y = el.Location.Y + 3 * el.Size.Height / 8;
                puntos1[5].X = el.Location.X + 3 * el.Size.Width / 6;
                puntos1[5].Y = el.Location.Y + 1 * el.Size.Height / 8;
                puntos1[6].X = el.Location.X + 3 * el.Size.Width / 6;
                puntos1[6].Y = el.Location.Y + 3 * el.Size.Height / 8;
                puntos1[7].X = el.Location.X + 4 * el.Size.Width / 6;
                puntos1[7].Y = el.Location.Y + 1 * el.Size.Height / 8;
                puntos1[8].X = el.Location.X + 4 * el.Size.Width / 6;
                puntos1[8].Y = el.Location.Y + 3 * el.Size.Height / 8;
                puntos1[9].X = el.Location.X + 5 * el.Size.Width / 6;
                puntos1[9].Y = el.Location.Y + 1 * el.Size.Height / 8;
                puntos1[10].X = el.Location.X + 5 * el.Size.Width / 6;
                puntos1[10].Y = el.Location.Y + 3 * el.Size.Height / 8;
                puntos1[11].X = el.Location.X + 6 * el.Size.Width / 6;
                puntos1[11].Y = el.Location.Y + 2 * el.Size.Height / 8;

                g.DrawLines(p1, puntos1);

                Point[] puntos2 = new Point[4];
                puntos2[0].X = el.Location.X + el.Size.Width / 6;
                puntos2[0].Y = el.Location.Y + 5 * el.Size.Height / 8;
                puntos2[1].X = el.Location.X + el.Size.Width / 6;
                puntos2[1].Y = el.Location.Y + 8 * el.Size.Height / 8;
                puntos2[2].X = el.Location.X + 5 * el.Size.Width / 6;
                puntos2[2].Y = el.Location.Y + 8 * el.Size.Height / 8;
                puntos2[3].X = el.Location.X + 5 * el.Size.Width / 6;
                puntos2[3].Y = el.Location.Y + 5 * el.Size.Height / 8;

                g.DrawLines(p1, puntos2);

                Point[] puntos3 = new Point[4];
                puntos3[0].X = el.Location.X;
                puntos3[0].Y = el.Location.Y + 2 * el.Size.Height / 8;
                puntos3[1].X = el.Location.X + el.Size.Width / 19;
                puntos3[1].Y = el.Location.Y + 7 * el.Size.Height / 17;
                puntos3[2].X = el.Location.X + 3 * el.Size.Width / 19;
                puntos3[2].Y = el.Location.Y + 9 * el.Size.Height / 17;
                puntos3[3].X = el.Location.X + el.Size.Width / 6;
                puntos3[3].Y = el.Location.Y + 7 * el.Size.Height / 8;
                g.DrawCurve(p1, puntos3);

                Point[] puntos4 = new Point[4];
                puntos4[0].X = el.Location.X + el.Size.Width;
                puntos4[0].Y = el.Location.Y + 2 * el.Size.Height / 8;
                puntos4[1].X = el.Location.X + 18 * el.Size.Width / 19;
                puntos4[1].Y = el.Location.Y + 7 * el.Size.Height / 17;
                puntos4[2].X = el.Location.X + 16 * el.Size.Width / 19;
                puntos4[2].Y = el.Location.Y + 9 * el.Size.Height / 17;
                puntos4[3].X = el.Location.X + 5 * el.Size.Width / 6;
                puntos4[3].Y = el.Location.Y + 5 * el.Size.Height / 8;

                g.DrawCurve(p1, puntos4);               

                p1.Dispose();
                //brush.Dispose();

            }
       }

		#endregion
	}
}
