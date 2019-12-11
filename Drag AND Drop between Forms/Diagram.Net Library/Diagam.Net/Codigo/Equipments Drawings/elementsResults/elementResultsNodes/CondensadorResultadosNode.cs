using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;


namespace Dalssoft.DiagramNet
{
	[Serializable]
	public class CondensadorResultadoNode: NodeElement, IControllable, ILabelElement
	{
        protected CondensadorElementResultados condensador;
		protected LabelElement label = new LabelElement();
        protected ConnectorElement[] connectors12=new ConnectorElement[10];

		[NonSerialized]
        private CondensadorResultadosController controller;

        
		public CondensadorResultadoNode(): this(0, 0, 100, 100,9)
		{
        
        }

		public CondensadorResultadoNode(Rectangle rec,int tipoelemento2): this(rec.Location, rec.Size,tipoelemento2)
		{ 
        
        }

		public CondensadorResultadoNode(Point l, Size s,int tipoelemento2): this(l.X, l.Y, s.Width, s.Height,tipoelemento2) 
		{
           
        }

        public CondensadorResultadoNode(int top, int left, int width, int height, int tipoelemento2)
            : base(top, left, width, height, tipoelemento2)
		{
            condensador = new CondensadorElementResultados(top, left, width, height);
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
                condensador.BorderColor = value;
				base.BorderColor = value;
			}
		}

		public Color FillColor1
		{
			get
			{
                return condensador.FillColor1;
			}
			set
			{
                condensador.FillColor1 = value;
			}
		}

		public Color FillColor2
		{
			get
			{
                return condensador.FillColor2;
			}
			set
			{
                condensador.FillColor2 = value;
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
                condensador.Opacity = value;
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
                condensador.Visible = value;
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
                condensador.Location = value;
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
                condensador.Size = value;
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
                condensador.BorderWidth = value;
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
            base.location = condensador.Location;
            base.size = condensador.Size;
            base.borderColor = condensador.BorderColor;
            base.borderWidth = condensador.BorderWidth;
            base.opacity = condensador.Opacity;
            base.visible = condensador.Visible;
           
		}

		internal override void Draw(Graphics g)
		{
			IsInvalidated = false;
            condensador.Draw(g);
		}

		IController IControllable.GetController()
		{
			if (controller == null)
                controller = new CondensadorResultadosController(this);
			return controller;
		}
	}
}
