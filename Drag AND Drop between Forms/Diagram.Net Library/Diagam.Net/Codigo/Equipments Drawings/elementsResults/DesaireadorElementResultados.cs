using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;


namespace Dalssoft.DiagramNet
{
	[Serializable]
	public class DesaireadorElementResultados: RectangleElement, IControllable
	{
		[NonSerialized]
		private DesaireadorResultadosController controller;

		public DesaireadorElementResultados(): base() {}

		public DesaireadorElementResultados(Rectangle rec): base(rec) {}

		public DesaireadorElementResultados(Point l, Size s): base(l, s) {}

        public DesaireadorElementResultados(int top, int left, int width, int height) : base(top, left, width, height) { }

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

            Point punto = new Point();
            punto.X = this.Location.X + this.Size.Width / 7;
            punto.Y = this.Location.Y + 4 * this.Size.Height / 8;

            Point punto1 = new Point();
            punto1.X = this.Location.X + 2 * this.Size.Width / 7;
            punto1.Y = this.Location.Y + 4 * this.Size.Height / 8;

            Point punto2 = new Point();
            punto2.X = this.Location.X;
            punto2.Y = this.Location.Y + 5 * this.Size.Height / 8;

            Point punto3 = new Point();
            punto3.X = this.Location.X;
            punto3.Y = this.Location.Y + 7 * this.Size.Height / 8;

            Point punto4 = new Point();
            punto4.X = this.Location.X + this.Size.Width / 7;
            punto4.Y = this.Location.Y + 8 * this.Size.Height / 8;

            Point punto5 = new Point();
            punto5.X = this.Location.X + 2 * this.Size.Width / 7;
            punto5.Y = this.Location.Y + 8 * this.Size.Height / 8;

            Point punto6 = new Point();
            punto6.X = this.Location.X + 2 * this.Size.Width / 7;
            punto6.Y = this.Location.Y + 2 * this.Size.Height / 8;

            Point punto7 = new Point();
            punto7.X = this.Location.X + 3 * this.Size.Width / 7;
            punto7.Y = this.Location.Y + 1 * this.Size.Height / 8;

            Point punto8 = new Point();
            punto8.X = this.Location.X + 4 * this.Size.Width / 7;
            punto8.Y = this.Location.Y + 1 * this.Size.Height / 8;

            Point punto9 = new Point();
            punto9.X = this.Location.X + 5 * this.Size.Width / 7;
            punto9.Y = this.Location.Y + 2 * this.Size.Height / 8;

            Point punto10 = new Point();
            punto10.X = this.Location.X + 5 * this.Size.Width / 7;
            punto10.Y = this.Location.Y + 4 * this.Size.Height / 8;

            Point punto11 = new Point();
            punto11.X = this.Location.X + 6 * this.Size.Width / 7;
            punto11.Y = this.Location.Y + 4 * this.Size.Height / 8;

            Point punto12 = new Point();
            punto12.X = this.Location.X + 7 * this.Size.Width / 7;
            punto12.Y = this.Location.Y + 5 * this.Size.Height / 8;

            Point punto13 = new Point();
            punto13.X = this.Location.X + 7 * this.Size.Width / 7;
            punto13.Y = this.Location.Y + 7 * this.Size.Height / 8;

            Point punto14 = new Point();
            punto14.X = this.Location.X + 6 * this.Size.Width / 7;
            punto14.Y = this.Location.Y + 8 * this.Size.Height / 8;

            Point punto15 = new Point();
            punto15.X = this.Location.X + this.Size.Width / 7;
            punto15.Y = this.Location.Y + 8 * this.Size.Height / 8;

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
			b.Dispose();
		}
		
		IController IControllable.GetController()
		{
			if (controller == null)
				controller = new DesaireadorResultadosController(this);
			return controller;
		}
	}
}
