using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;


namespace Dalssoft.DiagramNet
{
	[Serializable]
	public class ArcoElementResultados: RectangleElement, IControllable
	{
		[NonSerialized]
		private ArcoResultadosController controller;

		public ArcoElementResultados(): base() {}

		public ArcoElementResultados(Rectangle rec): base(rec) {}

		public ArcoElementResultados(Point l, Size s): base(l, s) {}

        public ArcoElementResultados(int top, int left, int width, int height) : base(top, left, width, height) { }

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

            Point[] puntos = new Point[2];
            puntos[0].X = this.Location.X;
            puntos[0].Y = this.Location.Y + this.Size.Height;
            puntos[1].X = this.Location.X + this.Size.Width / 4;
            puntos[1].Y = this.Location.Y + this.Size.Height;
            g.DrawLines(p1, puntos);

            Point[] puntos1 = new Point[2];
            puntos1[0].X = this.Location.X + 3 * this.Size.Width / 4;
            puntos1[0].Y = this.Location.Y + this.Size.Height;
            puntos1[1].X = this.Location.X + 4 * this.Size.Width / 4;
            puntos1[1].Y = this.Location.Y + this.Size.Height;
            g.DrawLines(p1, puntos1);

            Point puntos2 = new Point();
            puntos2.X = this.Location.X + this.Size.Width / 4;
            puntos2.Y = this.Location.Y + 3 * this.Size.Height / 4;

            Size tama = new Size(this.Size.Width / 2, this.Size.Height / 2);
            Rectangle forarco = new Rectangle(puntos2, tama);

            //g.DrawRectangle(p1,forarco);
            g.DrawArc(p1, forarco, -180, 180);
			
			p1.Dispose();
			b.Dispose();
		}
		
		IController IControllable.GetController()
		{
			if (controller == null)
				controller = new ArcoResultadosController(this);
			return controller;
		}
	}
}
