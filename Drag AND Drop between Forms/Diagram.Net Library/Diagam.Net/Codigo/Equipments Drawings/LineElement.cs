using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using Arrows;


namespace Dalssoft.DiagramNet
{
	[Serializable]
	public class LineElement: BaseElement, IControllable 
	{
		protected Point point1;
		protected Point point2;
		protected LineCap startCap = LineCap.Round;
		protected LineCap endCap = LineCap.Round;
		protected bool needCalcLine = false;
        public bool linetypearrow1 = true;

		[NonSerialized]
		private LineController controller;

		internal LineElement(int x1, int y1, int x2, int y2,bool linetypearrow)
			: this(new Point(x1, y1), new Point(x2, y2),linetypearrow) {}

		internal LineElement(Point p1, Point p2,bool linetypearrow)
		{
            linetypearrow1 = linetypearrow;

			point1 = p1;			
			point2 = p2;
			
			borderWidth = 1;
			borderColor = Color.Black;
		}

		public virtual Point Point1
		{
			get
			{
				CalcLine();
				return point1;
			}
			set
			{
				point1 = value;
				needCalcLine = true;
				OnAppearanceChanged(new EventArgs());
			}
		}

		public virtual Point Point2
		{
			get
			{
				CalcLine();
				return point2;
			}
			set
			{
				point2 = value;
				needCalcLine = true;
				OnAppearanceChanged(new EventArgs());
			}
		}

		public virtual LineCap StartCap
		{
			get
			{
				return startCap;
			}
			set
			{
				startCap = value;
				OnAppearanceChanged(new EventArgs());
			}
		}

		public virtual LineCap EndCap
		{
			get
			{
				return endCap;
			}
			set
			{
				endCap = value;
				OnAppearanceChanged(new EventArgs());
			}
		}

		internal override void Draw(Graphics g)
		{
			IsInvalidated = false;

			Rectangle r = RectangleElement.GetUnsignedRectangle(
				new Rectangle(
				location.X, location.Y, 
				size.Width, size.Height));

			//Fill rectangle
			Color borderColor1;
			if (opacity == 100)
				borderColor1 = borderColor;
			else
				borderColor1 = Color.FromArgb((int) (255.0f * (opacity / 100.0f)), borderColor);
			
			Pen p;
			p = new Pen(borderColor1, borderWidth);
            SolidBrush b = new SolidBrush(Color.Red);
			
			p.StartCap = startCap;
			p.EndCap = endCap;

            if (linetypearrow1 == true)
            {
                //El primer argumento de ArrowRenderer indica el tamaño de la Punta de la FLECHA
                //El segundo argumento de ArrowRenderer indica si la Punta de la FLECHA esta rellena de color o no
                ArrowRenderer a = new ArrowRenderer(8, (float)Math.PI / 6, false);
                //Angulo de la Punta de la FLECHA
                a.SetThetaInDegrees(35);
                g.SmoothingMode = SmoothingMode.HighQuality;
                //Dibujar una FLECHA, el primer argumento indica el color de la linea, el segundo el color de la punta de la flecha
                //El segundo argumento indica las coordenadas del punto de origen y punto final de la FLECHA
                a.DrawArrow(g, p, b, point1.X, point1.Y, point2.X, point2.Y);
            }

            else if (linetypearrow1 == false)
            {
                g.DrawLine(p, point1, point2);
            }

			//g.DrawLine(p, point1, point2);
			p.Dispose();
		}

		internal void CalcLine()
		{
			if (needCalcLine == false) return;

			//Find Location and Size
			if (point1.X < point2.X)
			{
				location.X = point1.X;
				size.Width = point2.X - point1.X;
			}
			else
			{
				location.X = point2.X;
				size.Width = point1.X - point2.X;
			}

			if (point1.Y < point2.Y)
			{
				location.Y = point1.Y;
				size.Height = point2.Y - point1.Y;
			}
			else
			{
				location.Y = point2.Y;
				size.Height = point1.Y - point2.Y;
			}

			needCalcLine = false;
		}
		
		#region IControllable Members

		IController IControllable.GetController()
		{
			if (controller == null)
				controller = new LineController(this);
			return controller;
		}

		#endregion
	}
}