using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Dalssoft.DiagramNet
{
	[Serializable]
	public class ImageElement : BaseElement, IControllable
	{
        protected Image image;
        protected Int32 tipoequipo1 = 0;
        protected Image imagen1=null;

		[NonSerialized]
		private ImageController controller;

		public ImageElement(): this(0, 0, 100, 100,1)
		{
        
        }

		public ImageElement(Rectangle rec,Int32 tipoequipo,Image imagen2): this(rec.Location, rec.Size,tipoequipo,imagen2)
		{}
		
		public ImageElement(Point l, Size s, Int32 tipoequipo,Image imagen2): this(l.X, l.Y, s.Width, s.Height,tipoequipo,imagen2) 
		{}

        public ImageElement(int top, int left, int width, int height, Int32 tipoequipo,Image imagen2)
        {
            tipoequipo1 = tipoequipo;
            imagen1 = imagen2;
            location = new Point(top, left);
            size = new Size(width, height);
        }

        public ImageElement(int top, int left, int width, int height, Int32 tipoequipo)
        {
            tipoequipo1 = tipoequipo;
            location = new Point(top, left);
            size = new Size(width, height);
        }

		protected virtual void DrawBorder(Graphics g, Rectangle r)
		{
			if (borderWidth > 0)
            {
                Pen p = new Pen(borderColor, borderWidth);
                g.DrawRectangle(p, r);
                p.Dispose();
            }
		}

		internal override void Draw(Graphics g)
		{
			IsInvalidated = false;

			Rectangle r = GetUnsignedRectangle();
           
            if (imagen1 != null)

                g.DrawImage(imagen1,r);

			DrawBorder(g, r);
		}

        public Image Image
        {
            get
            {
                return imagen1;
            }
            set
            {
                imagen1 = value;
                if (imagen1 != null)
                   // Size = image.Size;
                OnAppearanceChanged(new EventArgs());
            }
        }
		
		IController IControllable.GetController()
		{
			if (controller == null)
				controller = new ImageController(this);
			return controller;
		}
	
	}
}
