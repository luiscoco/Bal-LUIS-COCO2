using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Dalssoft.DiagramNet
{
	/// <summary>
	/// This class is the controller for ElipseElement
	/// </summary>
	internal class ImageController: RectangleController, IController
	{
        protected Image image;

		public ImageController(BaseElement element): base(element)
		{
		}
	
		#region IController Members

        public override bool HitTest(Point p)
        {
            GraphicsPath gp = new GraphicsPath();
            Matrix mtx = new Matrix();

            Point elLocation = el.Location;
            Size elSize = el.Size;
            gp.AddRectangle(new Rectangle(elLocation.X,
                elLocation.Y,
                elSize.Width,
                elSize.Height));
            gp.Transform(mtx);

            return gp.IsVisible(p);
        }

        public override bool HitTest(Rectangle r)
        {
            GraphicsPath gp = new GraphicsPath();
            Matrix mtx = new Matrix();

            Point elLocation = el.Location;
            Size elSize = el.Size;
            gp.AddRectangle(new Rectangle(elLocation.X,
                elLocation.Y,
                elSize.Width,
                elSize.Height));
            gp.Transform(mtx);
            Rectangle retGp = Rectangle.Round(gp.GetBounds());
            return r.Contains(retGp);
        }


        protected virtual void DrawBorder(Graphics g, Rectangle r)
        {
            if (el.BorderWidth> 0)
            {
                Pen p = new Pen(el.BorderColor,el.BorderWidth);
                g.DrawRectangle(p, r);
                p.Dispose();
            }
        }

        public override void DrawSelection(Graphics g)
        {
            el.IsInvalidated = false;

            Rectangle r = el.GetUnsignedRectangle();

            if (Image != null)
                g.DrawImage(image,r);

            DrawBorder(g,r);
        }

        public Image Image
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
                if (image != null)
                    //el.Size = image.Size;
                el.OnAppearanceChanged(new EventArgs());
            }
        }

		#endregion
	}
}
