using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;


namespace Dalssoft.DiagramNet
{
	[Serializable]
	public class DesaireadorResultadoNode: NodeElement, IControllable, ILabelElement
	{
        protected DesaireadorElementResultados desaireador;
		protected LabelElement label = new LabelElement();
        protected ConnectorElement[] connectors12=new ConnectorElement[10];

		[NonSerialized]
        private DesaireadorResultadosController controller;

        
		public DesaireadorResultadoNode(): this(0, 0, 100, 100,9)
		{
        
        }

		public DesaireadorResultadoNode(Rectangle rec,int tipoelemento2): this(rec.Location, rec.Size,tipoelemento2)
		{ 
        
        }

		public DesaireadorResultadoNode(Point l, Size s,int tipoelemento2): this(l.X, l.Y, s.Width, s.Height,tipoelemento2) 
		{
           
        }

        public DesaireadorResultadoNode(int top, int left, int width, int height, int tipoelemento2)
            : base(top, left, width, height, tipoelemento2)
		{
            desaireador = new DesaireadorElementResultados(top, left, width, height);
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
                desaireador.BorderColor = value;
				base.BorderColor = value;
			}
		}

		public Color FillColor1
		{
			get
			{
                return desaireador.FillColor1;
			}
			set
			{
                desaireador.FillColor1 = value;
			}
		}

		public Color FillColor2
		{
			get
			{
                return desaireador.FillColor2;
			}
			set
			{
                desaireador.FillColor2 = value;
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
                desaireador.Opacity = value;
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
                desaireador.Visible = value;
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
                desaireador.Location = value;
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
                desaireador.Size = value;
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
                desaireador.BorderWidth = value;
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
            base.location = desaireador.Location;
            base.size = desaireador.Size;
            base.borderColor = desaireador.BorderColor;
            base.borderWidth = desaireador.BorderWidth;
            base.opacity = desaireador.Opacity;
            base.visible = desaireador.Visible;
           
		}

		internal override void Draw(Graphics g)
		{
			IsInvalidated = false;
            desaireador.Draw(g);
		}

		IController IControllable.GetController()
		{
			if (controller == null)
                controller = new DesaireadorResultadosController(this);
			return controller;
		}
	}
}
