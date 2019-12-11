using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;


namespace Dalssoft.DiagramNet
{
	[Serializable]
	public class BombaElementResultados: RectangleElement, IControllable
	{
		[NonSerialized]
		private BombaResultadosController controller;

		public BombaElementResultados(): base() {}

		public BombaElementResultados(Rectangle rec): base(rec) {}

		public BombaElementResultados(Point l, Size s): base(l, s) {}

        public BombaElementResultados(int top, int left, int width, int height) : base(top, left, width, height) { }

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
            Point puntos = new Point();
            puntos.X = this.Location.X + this.Size.Width / 3;
            puntos.Y = this.Location.Y + this.Size.Height / 3;

            Point puntos1 = new Point();
            puntos1.X = this.Location.X + 2 * this.Size.Width / 3;
            puntos1.Y = this.Location.Y + this.Size.Height / 3;

            Point puntos2 = new Point();
            puntos2.X = this.Location.X;
            puntos2.Y = this.Location.Y + this.Size.Height / 3;

            Point puntos3 = new Point();
            puntos3.X = this.Location.X + this.Size.Width / 3;
            puntos3.Y = this.Location.Y + 2 * this.Size.Height / 3;

            Point puntos4 = new Point();
            puntos4.X = this.Location.X;
            puntos4.Y = this.Location.Y + 2 * this.Size.Height / 3;

            Size tam = new Size(2 * this.Size.Width / 3, 2 * this.Size.Height / 3);

            Rectangle rec = new Rectangle(puntos, tam);

            g.DrawEllipse(p1, rec);
            g.FillEllipse(b,rec);

            g.DrawLine(p1, puntos1, puntos2);
            g.DrawLine(p1, puntos3, puntos4);
            g.DrawLine(p1, puntos2, puntos4);
			
			p1.Dispose();
			b.Dispose();
		}
		
		IController IControllable.GetController()
		{
			if (controller == null)
				controller = new BombaResultadosController(this);
			return controller;
		}
	}
}
