using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;


namespace Dalssoft.DiagramNet
{
	[Serializable]
	public class BombaResultadoNode: NodeElement, IControllable, ILabelElement
	{
        protected BombaElementResultados bomba;
		protected LabelElement label = new LabelElement();
        protected ConnectorElement[] connectors12=new ConnectorElement[10];

		[NonSerialized]
        private BombaResultadosController controller;

        
		public BombaResultadoNode(): this(0, 0, 100, 100,9)
		{
        
        }

		public BombaResultadoNode(Rectangle rec,int tipoelemento2): this(rec.Location, rec.Size,tipoelemento2)
		{ 
        
        }

		public BombaResultadoNode(Point l, Size s,int tipoelemento2): this(l.X, l.Y, s.Width, s.Height,tipoelemento2) 
		{
           
        }

        public BombaResultadoNode(int top, int left, int width, int height, int tipoelemento2)
            : base(top, left, width, height, tipoelemento2)
		{
            bomba = new BombaElementResultados(top, left, width, height);
            connectors12 = base.connects;
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
                bomba.BorderColor = value;
				base.BorderColor = value;
			}
		}

		public Color FillColor1
		{
			get
			{
                return bomba.FillColor1;
			}
			set
			{
                bomba.FillColor1 = value;
			}
		}

		public Color FillColor2
		{
			get
			{
                return bomba.FillColor2;
			}
			set
			{
                bomba.FillColor2 = value;
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
                bomba.Opacity = value;
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
                bomba.Visible = value;
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
                bomba.Location = value;
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
                bomba.Size = value;
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
                bomba.BorderWidth = value;
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
            base.location = bomba.Location;
            base.size = bomba.Size;
            base.borderColor = bomba.BorderColor;
            base.borderWidth = bomba.BorderWidth;
            base.opacity = bomba.Opacity;
            base.visible = bomba.Visible;
           
		}

		internal override void Draw(Graphics g)
		{
			IsInvalidated = false;
            bomba.Draw(g);
		}

		IController IControllable.GetController()
		{
			if (controller == null)
                controller = new BombaResultadosController(this);
			return controller;
		}
	}
}
