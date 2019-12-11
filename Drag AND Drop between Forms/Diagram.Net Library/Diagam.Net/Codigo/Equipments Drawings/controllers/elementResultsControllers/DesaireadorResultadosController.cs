using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Dalssoft.DiagramNet
{
	/// <summary>
	/// This class is the controller for ElipseElement
	/// </summary>
	internal class DesaireadorResultadosController: RectangleController, IController
	{
		public DesaireadorResultadosController(BaseElement element): base(element)
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

                Point punto = new Point();
                punto.X = el.Location.X + el.Size.Width / 7;
                punto.Y = el.Location.Y + 4 * el.Size.Height / 8;

                Point punto1 = new Point();
                punto1.X = el.Location.X + 2 * el.Size.Width / 7;
                punto1.Y = el.Location.Y + 4 * el.Size.Height / 8;

                Point punto2 = new Point();
                punto2.X = el.Location.X;
                punto2.Y = el.Location.Y + 5 * el.Size.Height / 8;

                Point punto3 = new Point();
                punto3.X = el.Location.X;
                punto3.Y = el.Location.Y + 7 * el.Size.Height / 8;

                Point punto4 = new Point();
                punto4.X = el.Location.X + el.Size.Width / 7;
                punto4.Y = el.Location.Y + 8 * el.Size.Height / 8;

                Point punto5 = new Point();
                punto5.X = el.Location.X + 2 * el.Size.Width / 7;
                punto5.Y = el.Location.Y + 8 * el.Size.Height / 8;

                Point punto6 = new Point();
                punto6.X = el.Location.X + 2 * el.Size.Width / 7;
                punto6.Y = el.Location.Y + 2 * el.Size.Height / 8;

                Point punto7 = new Point();
                punto7.X = el.Location.X + 3 * el.Size.Width / 7;
                punto7.Y = el.Location.Y + 1 * el.Size.Height / 8;

                Point punto8 = new Point();
                punto8.X = el.Location.X + 4 * el.Size.Width / 7;
                punto8.Y = el.Location.Y + 1 * el.Size.Height / 8;

                Point punto9 = new Point();
                punto9.X = el.Location.X + 5 * el.Size.Width / 7;
                punto9.Y = el.Location.Y + 2 * el.Size.Height / 8;

                Point punto10 = new Point();
                punto10.X = el.Location.X + 5 * el.Size.Width / 7;
                punto10.Y = el.Location.Y + 4 * el.Size.Height / 8;

                Point punto11 = new Point();
                punto11.X = el.Location.X + 6 * el.Size.Width / 7;
                punto11.Y = el.Location.Y + 4 * el.Size.Height / 8;

                Point punto12 = new Point();
                punto12.X = el.Location.X + 7 * el.Size.Width / 7;
                punto12.Y = el.Location.Y + 5 * el.Size.Height / 8;

                Point punto13 = new Point();
                punto13.X = el.Location.X + 7 * el.Size.Width / 7;
                punto13.Y = el.Location.Y + 7 * el.Size.Height / 8;

                Point punto14 = new Point();
                punto14.X = el.Location.X + 6 * el.Size.Width / 7;
                punto14.Y = el.Location.Y + 8 * el.Size.Height / 8;

                Point punto15 = new Point();
                punto15.X = el.Location.X + el.Size.Width / 7;
                punto15.Y = el.Location.Y + 8 * el.Size.Height / 8;

                g.DrawLine(p1, punto, punto1);
                g.DrawLine(p1, punto, punto2);
                g.DrawLine(p1, punto2, punto3);
                g.DrawLine(p1, punto3, punto4);
                g.DrawLine(p1, punto4, punto5);
                g.DrawLine(p1, punto1, punto6);
                g.DrawLine(p1, punto6, punto7);
                g.DrawLine(p1, punto7, punto8);
                g.DrawLine(p1, punto8, punto9);
                g.DrawLine(p1, punto9, punto10);
                g.DrawLine(p1, punto10, punto11);
                g.DrawLine(p1, punto11, punto12);
                g.DrawLine(p1, punto12, punto13);
                g.DrawLine(p1, punto13, punto14);
                g.DrawLine(p1, punto14, punto15);

                p1.Dispose();
                //brush.Dispose();
            }
       }

		#endregion
	}
}
