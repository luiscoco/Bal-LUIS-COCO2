using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;


namespace Dalssoft.DiagramNet
{
	[Serializable]
	public class CondensadorElementResultados: RectangleElement, IControllable
	{
		[NonSerialized]
		private CondensadorResultadosController controller;

		public CondensadorElementResultados(): base() {}

		public CondensadorElementResultados(Rectangle rec): base(rec) {}

		public CondensadorElementResultados(Point l, Size s): base(l, s) {}

        public CondensadorElementResultados(int top, int left, int width, int height) : base(top, left, width, height) { }

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
            puntos[0].Y = this.Location.Y + 2 * this.Size.Height / 8;
            puntos[1].X = this.Location.X;
            puntos[1].Y = this.Location.Y;
            puntos[2].X = this.Location.X + this.Size.Width;
            puntos[2].Y = this.Location.Y;
            puntos[3].X = this.Location.X + this.Size.Width;
            puntos[3].Y = this.Location.Y + 2 * this.Size.Height / 8;

            g.DrawLines(p1, puntos);

            Point[] puntos1 = new Point[12];
            puntos1[0].X = this.Location.X;
            puntos1[0].Y = this.Location.Y + 2 * this.Size.Height / 8;
            puntos1[1].X = this.Location.X + this.Size.Width / 6;
            puntos1[1].Y = this.Location.Y + 1 * this.Size.Height / 8;
            puntos1[2].X = this.Location.X + this.Size.Width / 6;
            puntos1[2].Y = this.Location.Y + 3 * this.Size.Height / 8;
            puntos1[3].X = this.Location.X + 2 * this.Size.Width / 6;
            puntos1[3].Y = this.Location.Y + 1 * this.Size.Height / 8;
            puntos1[4].X = this.Location.X + 2 * this.Size.Width / 6;
            puntos1[4].Y = this.Location.Y + 3 * this.Size.Height / 8;
            puntos1[5].X = this.Location.X + 3 * this.Size.Width / 6;
            puntos1[5].Y = this.Location.Y + 1 * this.Size.Height / 8;
            puntos1[6].X = this.Location.X + 3 * this.Size.Width / 6;
            puntos1[6].Y = this.Location.Y + 3 * this.Size.Height / 8;
            puntos1[7].X = this.Location.X + 4 * this.Size.Width / 6;
            puntos1[7].Y = this.Location.Y + 1 * this.Size.Height / 8;
            puntos1[8].X = this.Location.X + 4 * this.Size.Width / 6;
            puntos1[8].Y = this.Location.Y + 3 * this.Size.Height / 8;
            puntos1[9].X = this.Location.X + 5 * this.Size.Width / 6;
            puntos1[9].Y = this.Location.Y + 1 * this.Size.Height / 8;
            puntos1[10].X = this.Location.X + 5 * this.Size.Width / 6;
            puntos1[10].Y = this.Location.Y + 3 * this.Size.Height / 8;
            puntos1[11].X = this.Location.X + 6 * this.Size.Width / 6;
            puntos1[11].Y = this.Location.Y + 2 * this.Size.Height / 8;

            g.DrawLines(p1, puntos1);

            Point[] puntos2 = new Point[4];
            puntos2[0].X = this.Location.X + this.Size.Width / 6;
            puntos2[0].Y = this.Location.Y + 5 * this.Size.Height / 8;
            puntos2[1].X = this.Location.X + this.Size.Width / 6;
            puntos2[1].Y = this.Location.Y + 8 * this.Size.Height / 8;
            puntos2[2].X = this.Location.X + 5 * this.Size.Width / 6;
            puntos2[2].Y = this.Location.Y + 8 * this.Size.Height / 8;
            puntos2[3].X = this.Location.X + 5 * this.Size.Width / 6;
            puntos2[3].Y = this.Location.Y + 5 * this.Size.Height / 8;

            g.DrawLines(p1, puntos2);

            Point[] puntos3 = new Point[4];
            puntos3[0].X = this.Location.X;
            puntos3[0].Y = this.Location.Y + 2 * this.Size.Height / 8;
            puntos3[1].X = this.Location.X + this.Size.Width / 19;
            puntos3[1].Y = this.Location.Y + 7 * this.Size.Height / 17;
            puntos3[2].X = this.Location.X + 3 * this.Size.Width / 19;
            puntos3[2].Y = this.Location.Y + 9 * this.Size.Height / 17;
            puntos3[3].X = this.Location.X + this.Size.Width / 6;
            puntos3[3].Y = this.Location.Y + 7 * this.Size.Height / 8;
            g.DrawCurve(p1, puntos3);

            Point[] puntos4 = new Point[4];
            puntos4[0].X = this.Location.X + this.Size.Width;
            puntos4[0].Y = this.Location.Y + 2 * this.Size.Height / 8;
            puntos4[1].X = this.Location.X + 18 * this.Size.Width / 19;
            puntos4[1].Y = this.Location.Y + 7 * this.Size.Height / 17;
            puntos4[2].X = this.Location.X + 16 * this.Size.Width / 19;
            puntos4[2].Y = this.Location.Y + 9 * this.Size.Height / 17;
            puntos4[3].X = this.Location.X + 5 * this.Size.Width / 6;
            puntos4[3].Y = this.Location.Y + 5 * this.Size.Height / 8;

            g.DrawCurve(p1, puntos4);
        

			p1.Dispose();
			b.Dispose();
		}
		
		IController IControllable.GetController()
		{
			if (controller == null)
				controller = new CondensadorResultadosController(this);
			return controller;
		}
	}
}
