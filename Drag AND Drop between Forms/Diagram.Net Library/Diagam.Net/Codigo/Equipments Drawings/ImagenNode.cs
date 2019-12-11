using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;


namespace Dalssoft.DiagramNet
{
	[Serializable]
	public class ImageNode: NodeElement, IControllable, ILabelElement
	{
		protected ImageElement imagen1;
		protected LabelElement label = new LabelElement();
        protected ConnectorElement[] connectors12=new ConnectorElement[10];
        protected Image imagen10;
        private Int32 tipoelemento1=1;

		[NonSerialized]
		private ImageController controller;

		public ImageNode(Rectangle rec,int tipoelemento2,Image imagen2): this(rec.Location, rec.Size,tipoelemento2,imagen2)
		{ 
        
        }

		public ImageNode(Point l, Size s,int tipoelemento2,Image imagen2): this(l.X, l.Y, s.Width, s.Height,tipoelemento2,imagen2) 
		{
           
        }

		public ImageNode(int top, int left, int width, int height,int tipoelemento2,Image imagen2): base(top, left, width,height,tipoelemento2)
		{
            tipoelemento1 = tipoelemento2;
            imagen10 = imagen2;
			imagen1 = new ImageElement(top, left, width, height,tipoelemento1,imagen10);
            connectors12 = base.connects;
			SyncContructors();
		}

        public ImageNode(): this(0, 0, 100, 100, 9,null)
        {

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
				imagen1.BorderColor = value;
				base.BorderColor = value;
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
				imagen1.Visible = value;
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
				imagen1.Location = value;
				base.Location = value;
			}
		}

        public Image Image
        {
            get
            {
                return imagen10;
            }
            set
            {
                imagen10 = value;
                if (imagen10 != null)
                    //Size = imagen1.Size;
                OnAppearanceChanged(new EventArgs());
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
				imagen1.Size = value;
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
				imagen1.BorderWidth = value;
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
			base.location = imagen1.Location;
			base.size = imagen1.Size;
			base.borderColor = imagen1.BorderColor;
			base.borderWidth = imagen1.BorderWidth;
			base.opacity = imagen1.Opacity;
			base.visible = imagen1.Visible;
           
		}

		internal override void Draw(Graphics g)
		{
			IsInvalidated = false;
            imagen1.Image = imagen10;
			imagen1.Draw(g);
		}

		IController IControllable.GetController()
		{
			if (controller == null)
				controller = new ImageController(this);
			return controller;
		}
	}
}
