using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;


namespace Dalssoft.DiagramNet
{
	[Serializable]
	public class CirculoElementResultados: RectangleElement, IControllable
	{
		[NonSerialized]
		private CirculoResultadosController controller;

		public CirculoElementResultados(): base() {}

		public CirculoElementResultados(Rectangle rec): base(rec) {}

		public CirculoElementResultados(Point l, Size s): base(l, s) {}

        public CirculoElementResultados(int top, int left, int width, int height) : base(top, left, width, height) { }

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

            g.DrawEllipse(p1, new Rectangle(this.Location, this.Size));
            g.FillEllipse(b, new Rectangle(this.Location, this.Size));
			
			p1.Dispose();
			b.Dispose();
		}
		
		IController IControllable.GetController()
		{
			if (controller == null)
				controller = new CirculoResultadosController(this);
			return controller;
		}
	}
}
