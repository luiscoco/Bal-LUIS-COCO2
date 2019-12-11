using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Dalssoft.DiagramNet
{
	/// <summary>
	/// This class is the controller for ElipseElement
	/// </summary>
	internal class BombaResultadosController: RectangleController, IController
	{
		public BombaResultadosController(BaseElement element): base(element)
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

			Rectangle r = BaseElement.GetUnsignedRectangle(
				new Rectangle(
					el.Location.X - border, el.Location.Y - border,
					el.Size.Width + (border * 2), el.Size.Height + (border * 2)));

			//HatchBrush brush = new HatchBrush(HatchStyle.SmallCheckerBoard, Color.Red, Color.Transparent);
			//Pen p = new Pen(brus, border);

            //Pen p=new Pen(Color,Anchura del pincel)
            Pen p1 = new Pen(Color.Red, 2);
            Point puntos = new Point();
            puntos.X = el.Location.X + el.Size.Width / 3;
            puntos.Y = el.Location.Y + el.Size.Height / 3;

            Point puntos1 = new Point();
            puntos1.X = el.Location.X + 2 * el.Size.Width / 3;
            puntos1.Y = el.Location.Y + el.Size.Height / 3;

            Point puntos2 = new Point();
            puntos2.X = el.Location.X;
            puntos2.Y = el.Location.Y + el.Size.Height / 3;

            Point puntos3 = new Point();
            puntos3.X = el.Location.X + el.Size.Width / 3;
            puntos3.Y = el.Location.Y + 2 * el.Size.Height / 3;

            Point puntos4 = new Point();
            puntos4.X = el.Location.X;
            puntos4.Y = el.Location.Y + 2 * el.Size.Height / 3;

            Size tam = new Size(2 * el.Size.Width / 3, 2 * el.Size.Height / 3);

            Rectangle rec = new Rectangle(puntos, tam);

            g.DrawEllipse(p1, rec);
            //g.FillEllipse(brush, rec);

            g.DrawLine(p1, puntos1, puntos2);
            g.DrawLine(p1, puntos3, puntos4);
            g.DrawLine(p1, puntos2, puntos4);

            //g.FillPolygon(brush, puntos);
           
            p1.Dispose();
            //brush.Dispose();
		}

		#endregion
	}
}
