using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Dalssoft.DiagramNet
{
	/// <summary>
	/// This class is the controller for ElipseElement
	/// </summary>
	internal class ArcoResultadosController: RectangleController, IController
	{
		public ArcoResultadosController(BaseElement element): base(element)
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

                Point[] puntos = new Point[2];
                puntos[0].X = el.Location.X;
                puntos[0].Y = el.Location.Y + el.Size.Height;
                puntos[1].X = el.Location.X + el.Size.Width / 4;
                puntos[1].Y = el.Location.Y + el.Size.Height;
                g.DrawLines(p1, puntos);

                Point[] puntos1 = new Point[2];
                puntos1[0].X = el.Location.X + 3 * el.Size.Width / 4;
                puntos1[0].Y = el.Location.Y + el.Size.Height;
                puntos1[1].X = el.Location.X + 4 * el.Size.Width / 4;
                puntos1[1].Y = el.Location.Y + el.Size.Height;
                g.DrawLines(p1, puntos1);

                Point puntos2 = new Point();
                puntos2.X = el.Location.X + el.Size.Width / 4;
                puntos2.Y = el.Location.Y + 3 * el.Size.Height / 4;

                Size tama = new Size(el.Size.Width / 2, el.Size.Height / 2);
                Rectangle forarco = new Rectangle(puntos2, tama);

                //g.DrawRectangle(p1,forarco);
                g.DrawArc(p1, forarco, -180, 180);

                p1.Dispose();
                //brush.Dispose();

            }
       }

		#endregion
	}
}
