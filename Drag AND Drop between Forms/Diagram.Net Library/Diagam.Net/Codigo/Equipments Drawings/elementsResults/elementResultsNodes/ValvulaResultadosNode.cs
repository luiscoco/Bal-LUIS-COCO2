using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;


namespace Dalssoft.DiagramNet
{
	[Serializable]
	public class ValvulaResultadoNode: NodeElement, IControllable, ILabelElement
	{
        protected ValvulaElementResultados valvula;
		protected LabelElement label = new LabelElement();
        protected ConnectorElement[] connectors12=new ConnectorElement[10];

		[NonSerialized]
        private ValvulaResultadosController controller;

        
		public ValvulaResultadoNode(): this(0, 0, 100, 100,9)
		{
        
        }

		public ValvulaResultadoNode(Rectangle rec,int tipoelemento2): this(rec.Location, rec.Size,tipoelemento2)
		{ 
        
        }

		public ValvulaResultadoNode(Point l, Size s,int tipoelemento2): this(l.X, l.Y, s.Width, s.Height,tipoelemento2) 
		{
           
        }

        public ValvulaResultadoNode(int top, int left, int width, int height, int tipoelemento2)
            : base(top, left, width, height, tipoelemento2)
		{
            valvula = new ValvulaElementResultados(top, left, width, height);
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
                valvula.BorderColor = value;
				base.BorderColor = value;
			}
		}

		public Color FillColor1
		{
			get
			{
                return valvula.FillColor1;
			}
			set
			{
                valvula.FillColor1 = value;
			}
		}

		public Color FillColor2
		{
			get
			{
                return valvula.FillColor2;
			}
			set
			{
                valvula.FillColor2 = value;
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
                valvula.Opacity = value;
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
                valvula.Visible = value;
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
                valvula.Location = value;
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
                valvula.Size = value;
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
                valvula.BorderWidth = value;
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
            base.location = valvula.Location;
            base.size = valvula.Size;
            base.borderColor = valvula.BorderColor;
            base.borderWidth = valvula.BorderWidth;
            base.opacity = valvula.Opacity;
            base.visible = valvula.Visible;
           
		}

		internal override void Draw(Graphics g)
		{
			IsInvalidated = false;
            valvula.Draw(g);
		}

		IController IControllable.GetController()
		{
			if (controller == null)
                controller = new ValvulaResultadosController(this);
			return controller;
		}
	}
}
