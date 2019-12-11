using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using Arrows;

namespace Dalssoft.DiagramNet
{
	/// <summary>
	/// This class is the controller for LineElement
	/// </summary>
	internal class LineController: IController
	{
		//parent element
		protected LineElement el;

		public LineController(LineElement element)
		{
			el = element;
		}

		#region IController Members

		public BaseElement OwnerElement
		{
			get
			{
				return el;
			}
		}

		public bool HitTest(Point p)
		{
			GraphicsPath gp = new GraphicsPath();
			Matrix mtx = new Matrix();

			Pen pen = new Pen(el.BorderColor, el.BorderWidth + 4);
			pen.StartCap = el.StartCap;
			pen.EndCap = el.EndCap;
			gp.AddLine(el.Point1, el.Point2);
			gp.Transform(mtx);
			//Rectangle retGp = Rectangle.Round(gp.GetBounds());
			return gp.IsOutlineVisible (p, pen);
		}

		public bool HitTest(Rectangle r)
		{
			GraphicsPath gp = new GraphicsPath();
			Matrix mtx = new Matrix();

			gp.AddRectangle(new Rectangle(el.Location.X,
				el.Location.Y,
				el.Size.Width,
				el.Size.Height));
			gp.Transform(mtx);
			Rectangle retGp = Rectangle.Round(gp.GetBounds());
			return r.Contains (retGp);
		}

		public void DrawSelection(Graphics g)
		{
			Pen p = new Pen(Color.Red, el.BorderWidth + 2);
            
            SolidBrush b = new SolidBrush(Color.Red);
			p.StartCap = el.StartCap;
			p.EndCap = el.EndCap;

            if (el.linetypearrow1 == true)
            {
                //El primer argumento de ArrowRenderer indica el tamaño de la Punta de la FLECHA
                //El segundo argumento de ArrowRenderer indica si la Punta de la FLECHA esta rellena de color o no
                ArrowRenderer a = new ArrowRenderer(8, (float)Math.PI / 6, false);
                //Angulo de la Punta de la FLECHA
                a.SetThetaInDegrees(35);
                g.SmoothingMode = SmoothingMode.HighQuality;
                //Dibujar una FLECHA, el primer argumento indica el color de la linea, el segundo el color de la punta de la flecha
                //El segundo argumento indica las coordenadas del punto de origen y punto final de la FLECHA
                a.DrawArrow(g, p, b, el.Point1.X, el.Point1.Y, el.Point2.X, el.Point2.Y);
            }

            else if (el.linetypearrow1 == false)
            {
                g.DrawLine(p, el.Point1, el.Point2);
            }
		}

		#endregion

	}
}
