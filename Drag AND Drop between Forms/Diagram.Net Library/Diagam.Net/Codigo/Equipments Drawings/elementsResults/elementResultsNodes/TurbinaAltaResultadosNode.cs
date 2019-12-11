using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;


namespace Dalssoft.DiagramNet
{
	[Serializable]
	public class TurbinaAltaResultadoNode: NodeElement, IControllable, ILabelElement
	{
        protected TurbinaElementAltaResultados turbina;
		protected LabelElement label = new LabelElement();
        protected ConnectorElement[] connectors12=new ConnectorElement[10];

		[NonSerialized]
        private TurbinaResultadosAltaController controller;

        
		public TurbinaAltaResultadoNode(): this(0, 0, 100, 100,9)
		{
        
        }

		public TurbinaAltaResultadoNode(Rectangle rec,int tipoelemento2): this(rec.Location, rec.Size,tipoelemento2)
		{ 
        
        }

		public TurbinaAltaResultadoNode(Point l, Size s,int tipoelemento2): this(l.X, l.Y, s.Width, s.Height,tipoelemento2) 
		{
           
        }

		public TurbinaAltaResultadoNode(int top, int left, int width, int height,int tipoelemento2): base(top, left, width,height,tipoelemento2)
		{
            turbina = new TurbinaElementAltaResultados(top, left, width, height);
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
				turbina.BorderColor = value;
				base.BorderColor = value;
			}
		}

		public Color FillColor1
		{
			get
			{
				return turbina.FillColor1;
			}
			set
			{
				turbina.FillColor1 = value;
			}
		}

		public Color FillColor2
		{
			get
			{
				return turbina.FillColor2;
			}
			set
			{
				turbina.FillColor2 = value;
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
				turbina.Opacity = value;
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
				turbina.Visible = value;
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
				turbina.Location = value;
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
				turbina.Size = value;
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
				turbina.BorderWidth = value;
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
			base.location = turbina.Location;
			base.size = turbina.Size;
			base.borderColor = turbina.BorderColor;
			base.borderWidth = turbina.BorderWidth;
			base.opacity = turbina.Opacity;
			base.visible = turbina.Visible;
           
		}

		internal override void Draw(Graphics g)
		{
			IsInvalidated = false;
			turbina.Draw(g);
		}

		IController IControllable.GetController()
		{
			if (controller == null)
                controller = new TurbinaResultadosAltaController(this);
			return controller;
		}
	}
}
