using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;


namespace Dalssoft.DiagramNet
{
	[Serializable]
	public class RectangleNode: NodeElement, IControllable, ILabelElement
	{
		protected RectangleElement rectangle;
		protected LabelElement label = new LabelElement();

		[NonSerialized]
		private RectangleController controller;

		public RectangleNode(): this(0, 0, 100, 100,1)
		{}

		public RectangleNode(Rectangle rec,int tipoelemento): this(rec.Location, rec.Size,tipoelemento)
		{}

		public RectangleNode(Point l, Size s,int tipoelemento): this(l.X, l.Y, s.Width, s.Height,tipoelemento) 
		{}

		public RectangleNode(int top, int left, int width, int height,int tipoelemento): base(top, left, width, height,tipoelemento)
		{
			rectangle = new RectangleElement(top, left, width, height);
			SyncContructors();
		}

        public ConnectorElement[] Conectores
        {
            get
            {
                return base.connects;
            }

            set
            {
                base.connects = value;
            }
        }

		public override Color BorderColor
		{
			get
			{
				return base.BorderColor;
			}
			set
			{
				rectangle.BorderColor = value;
				base.BorderColor = value;
			}
		}

		public Color FillColor1
		{
			get
			{
				return rectangle.FillColor1;
			}
			set
			{
				rectangle.FillColor1 = value;
			}
		}

		public Color FillColor2
		{
			get
			{
				return rectangle.FillColor2;
			}
			set
			{
				rectangle.FillColor2 = value;
			}
		}

		public override int Opacity
		{
			get
			{
				return base.Opacity;
			}
			set
			{
				rectangle.Opacity = value;
				base.Opacity = value;
			}
		}

		public override bool Visible
		{
			get
			{
				return base.Visible;
			}
			set
			{
				rectangle.Visible = value;
				base.Visible = value;
			}
		}

		public override Point Location
		{
			get
			{	
				return base.Location;
			}
			set
			{
				rectangle.Location = value;
				base.Location = value;
			}
		}

		public override Size Size
		{
			get
			{
				return base.Size;
			}
			set
			{
				rectangle.Size = value;
				base.Size = value;
			}
		}

		public override int BorderWidth
		{
			get
			{
				return base.BorderWidth;
			}
			set
			{
				rectangle.BorderWidth = value;
				base.BorderWidth = value;
			}
		}

		public virtual LabelElement Label 
		{
			get
			{
				return label;
			}
			set
			{
				label = value;
				OnAppearanceChanged(new EventArgs());
			}
		}

		private void SyncContructors()
		{
			base.location = rectangle.Location;
			base.size = rectangle.Size;
			base.borderColor = rectangle.BorderColor;
			base.borderWidth = rectangle.BorderWidth;
			base.opacity = rectangle.Opacity;
			base.visible = rectangle.Visible;
		}

		internal override void Draw(Graphics g)
		{
			IsInvalidated = false;

			rectangle.Draw(g);
		}

		IController IControllable.GetController()
		{
			if (controller == null)
				controller = new RectangleController(this);
			return controller;
		}

	}
}
