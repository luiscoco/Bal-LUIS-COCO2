using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;


namespace Dalssoft.DiagramNet
{
	[Serializable]
	public class RectanguloRedondeadoElementResultados: RectangleElement, IControllable
	{
		[NonSerialized]
		private RectanguloRedondeadoResultadosController controller;

		public RectanguloRedondeadoElementResultados(): base() {}

		public RectanguloRedondeadoElementResultados(Rectangle rec): base(rec) {}

		public RectanguloRedondeadoElementResultados(Point l, Size s): base(l, s) {}

        public RectanguloRedondeadoElementResultados(int top, int left, int width, int height) : base(top, left, width, height) { }

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

            Point punto = new Point(this.Location.X + this.Size.Width / 3, this.Location.Y + this.Size.Height / 3);

            float radius = 20;
            GraphicsPath gp = new GraphicsPath();
            gp.AddLine(this.Location.X + radius, this.Location.Y, this.Location.X + this.Size.Width - (radius * 2), this.Location.Y);
            gp.AddArc(this.Location.X + this.Size.Width - (radius * 2), this.Location.Y, radius * 2, radius * 2, 270, 90);
            gp.AddLine(this.Location.X + this.Size.Width, this.Location.Y + radius, this.Location.X + this.Size.Width, this.Location.Y + this.Size.Height - (radius * 2));
            gp.AddArc(this.Location.X + this.Size.Width - (radius * 2), this.Location.Y + this.Size.Height - (radius * 2), radius * 2, radius * 2, 0, 90);
            gp.AddLine(this.Location.X + this.Size.Width - (radius * 2), this.Location.Y + this.Size.Height, this.Location.X + radius, this.Location.Y + this.Size.Height);
            gp.AddArc(this.Location.X, this.Location.Y + this.Size.Height - (radius * 2), radius * 2, radius * 2, 90, 90);
            gp.AddLine(this.Location.X, this.Location.Y + this.Size.Height - (radius * 2), this.Location.X, this.Location.Y + radius);
            gp.AddArc(this.Location.X, this.Location.Y, radius * 2, radius * 2, 180, 90);
            gp.CloseFigure();
            g.DrawPath(p1, gp);
            g.FillPath(b, gp);           
			
			p1.Dispose();
			b.Dispose();
		}
		
		IController IControllable.GetController()
		{
			if (controller == null)
				controller = new RectanguloRedondeadoResultadosController(this);
			return controller;
		}
	}
}
