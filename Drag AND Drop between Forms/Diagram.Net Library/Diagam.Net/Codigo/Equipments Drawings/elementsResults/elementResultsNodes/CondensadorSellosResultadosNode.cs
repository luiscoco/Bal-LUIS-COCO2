using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;


namespace Dalssoft.DiagramNet
{
	[Serializable]
	public class CondensadorSellosResultadoNode: NodeElement, IControllable, ILabelElement
	{
        protected CondensadorSellosElementResultados condensadorsellos;
		protected LabelElement label = new LabelElement();
        protected ConnectorElement[] connectors12=new ConnectorElement[10];

		[NonSerialized]
        private CondensadorSellosResultadosController controller;

        
		public CondensadorSellosResultadoNode(): this(0, 0, 100, 100,9)
		{
        
        }

		public CondensadorSellosResultadoNode(Rectangle rec,int tipoelemento2): this(rec.Location, rec.Size,tipoelemento2)
		{ 
        
        }

        public CondensadorSellosResultadoNode(Point l, Size s, int tipoelemento2)
            : this(l.X, l.Y, s.Width, s.Height, tipoelemento2) 
		{
           
        }

        public CondensadorSellosResultadoNode(int top, int left, int width, int height, int tipoelemento2)
            : base(top, left, width, height, tipoelemento2)
		{
            condensadorsellos = new CondensadorSellosElementResultados(top, left, width, height);
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
                condensadorsellos.BorderColor = value;
				base.BorderColor = value;
			}
		}

		public Color FillColor1
		{
			get
			{
                return condensadorsellos.FillColor1;
			}
			set
			{
                condensadorsellos.FillColor1 = value;
			}
		}

		public Color FillColor2
		{
			get
			{
                return condensadorsellos.FillColor2;
			}
			set
			{
                condensadorsellos.FillColor2 = value;
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
                condensadorsellos.Opacity = value;
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
                condensadorsellos.Visible = value;
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
                condensadorsellos.Location = value;
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
                condensadorsellos.Size = value;
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
                condensadorsellos.BorderWidth = value;
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
            base.location = condensadorsellos.Location;
            base.size = condensadorsellos.Size;
            base.borderColor = condensadorsellos.BorderColor;
            base.borderWidth = condensadorsellos.BorderWidth;
            base.opacity = condensadorsellos.Opacity;
            base.visible = condensadorsellos.Visible;
           
		}

		internal override void Draw(Graphics g)
		{
			IsInvalidated = false;
            condensadorsellos.Draw(g);
		}

		IController IControllable.GetController()
		{
			if (controller == null)
                controller = new CondensadorSellosResultadosController(this);
			return controller;
		}
	}
}
