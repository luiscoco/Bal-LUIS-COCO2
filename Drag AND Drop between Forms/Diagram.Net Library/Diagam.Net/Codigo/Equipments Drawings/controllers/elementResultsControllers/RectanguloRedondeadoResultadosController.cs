using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Dalssoft.DiagramNet
{
	/// <summary>
	/// This class is the controller for ElipseElement
	/// </summary>
	internal class RectanguloRedondeadoResultadosController: RectangleController, IController
	{
		public RectanguloRedondeadoResultadosController(BaseElement element): base(element)
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

                Point punto = new Point(el.Location.X + el.Size.Width / 3, el.Location.Y + el.Size.Height / 3);

                float radius = 20;
                GraphicsPath gp = new GraphicsPath();
                gp.AddLine(el.Location.X + radius, el.Location.Y,el.Location.X + el.Size.Width - (radius * 2), el.Location.Y);
                gp.AddArc(el.Location.X + el.Size.Width - (radius * 2), el.Location.Y, radius * 2, radius * 2, 270, 90);
                gp.AddLine(el.Location.X + el.Size.Width, el.Location.Y + radius, el.Location.X + el.Size.Width, el.Location.Y + el.Size.Height - (radius * 2));
                gp.AddArc(el.Location.X + el.Size.Width - (radius * 2), el.Location.Y + el.Size.Height - (radius * 2), radius * 2, radius * 2, 0, 90);
                gp.AddLine(el.Location.X + el.Size.Width - (radius * 2), el.Location.Y + el.Size.Height, el.Location.X + radius, el.Location.Y + el.Size.Height);
                gp.AddArc(el.Location.X, el.Location.Y + el.Size.Height - (radius * 2), radius * 2, radius * 2, 90, 90);
                gp.AddLine(el.Location.X, el.Location.Y + el.Size.Height - (radius * 2), el.Location.X, el.Location.Y + radius);
                gp.AddArc(el.Location.X, el.Location.Y, radius * 2, radius * 2, 180, 90);
                gp.CloseFigure();
                g.DrawPath(p1, gp);

                //g.FillPath(b, gp);

                p1.Dispose();
                //brush.Dispose();
            }
       }

		#endregion
	}
}
