using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;


namespace Dalssoft.DiagramNet
{
	[Serializable]
	public class TurbinaElementAltaResultados: RectangleElement, IControllable
	{
		[NonSerialized]
		private TurbinaResultadosAltaController controller;

		public TurbinaElementAltaResultados(): base() {}

		public TurbinaElementAltaResultados(Rectangle rec): base(rec) {}

		public TurbinaElementAltaResultados(Point l, Size s): base(l, s) {}

        public TurbinaElementAltaResultados(int top, int left, int width, int height) : base(top, left, width, height) { }

		internal override void Draw(Graphics g)
		{
			IsInvalidated = false;

			Rectangle r = GetUnsignedRectangle(
				new Rectangle(
				location.X, location.Y, 
				size.Width, size.Height));

			//Fill elipse
            Color fill1;
			Color fill2;

			Brush b;

			if (opacity == 100)
			{
				fill1 = fillColor1;
				fill2 = fillColor2;
			}
			else
			{
				fill1 = Color.FromArgb((int) (255.0f * (opacity / 100.0f)), fillColor1);
				fill2 = Color.FromArgb((int) (255.0f * (opacity / 100.0f)), fillColor2);
			}
			
			if (fillColor2 == Color.Empty)
				b = new SolidBrush(fill1);
			else
			{
				Rectangle rb = new Rectangle(r.X, r.Y, r.Width + 1, r.Height + 1);
				b = new LinearGradientBrush(
					rb,
					fill1, 
					fill2, 
					LinearGradientMode.Horizontal);
			}
           
            Pen p1 = new Pen(Color.Black, 1);
            Point[] puntos = new Point[4];

            puntos[0].X = this.Location.X;
            puntos[0].Y = this.Location.Y + this.Size.Height / 4;
            puntos[1].X = this.Location.X + this.Size.Width;
            puntos[1].Y = this.Location.Y;
            puntos[2].X = this.Location.X + this.Size.Width;
            puntos[2].Y = this.Location.Y + this.Size.Height;
            puntos[3].X = this.Location.X;
            puntos[3].Y = this.Location.Y + 3 * this.Size.Height / 4;     

            g.DrawPolygon(p1, puntos);

            g.FillPolygon(b, puntos);
			
			p1.Dispose();
			b.Dispose();
		}
		
		IController IControllable.GetController()
		{
			if (controller == null)
				controller = new TurbinaResultadosAltaController(this);
			return controller;
		}
	}
}
