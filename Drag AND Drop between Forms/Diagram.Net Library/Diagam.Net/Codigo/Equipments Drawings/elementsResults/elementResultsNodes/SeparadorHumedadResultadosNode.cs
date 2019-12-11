using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;


namespace Dalssoft.DiagramNet
{
	[Serializable]
	public class SeparadorHumedadResultadoNode: NodeElement, IControllable, ILabelElement
	{
        protected SeparadorHumedadElementResultados sephum;
		protected LabelElement label = new LabelElement();
        protected ConnectorElement[] connectors12=new ConnectorElement[10];

		[NonSerialized]
        private SeparadorHumedadResultadosController controller;

        
		public SeparadorHumedadResultadoNode(): this(0, 0, 100, 100,9)
		{
        
        }

		public SeparadorHumedadResultadoNode(Rectangle rec,int tipoelemento2): this(rec.Location, rec.Size,tipoelemento2)
		{ 
        
        }

        public SeparadorHumedadResultadoNode(Point l, Size s, int tipoelemento2)
            : this(l.X, l.Y, s.Width, s.Height, tipoelemento2) 
		{
           
        }

		public SeparadorHumedadResultadoNode(int top, int left, int width, int height,int tipoelemento2): base(top, left, width,height,tipoelemento2)
		{
            sephum = new SeparadorHumedadElementResultados(top, left, width, height);
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
				sephum.BorderColor = value;
				base.BorderColor = value;
			}
		}

		public Color FillColor1
		{
			get
			{
				return sephum.FillColor1;
			}
			set
			{
				sephum.FillColor1 = value;
			}
		}

		public Color FillColor2
		{
			get
			{
				return sephum.FillColor2;
			}
			set
			{
				sephum.FillColor2 = value;
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
				sephum.Opacity = value;
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
				sephum.Visible = value;
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
				sephum.Location = value;
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
				sephum.Size = value;
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
				sephum.BorderWidth = value;
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
			base.location = sephum.Location;
			base.size = sephum.Size;
			base.borderColor = sephum.BorderColor;
			base.borderWidth = sephum.BorderWidth;
			base.opacity = sephum.Opacity;
			base.visible = sephum.Visible;
           
		}

		internal override void Draw(Graphics g)
		{
			IsInvalidated = false;
			sephum.Draw(g);
		}

		IController IControllable.GetController()
		{
			if (controller == null)
                controller = new SeparadorHumedadResultadosController(this);
			return controller;
		}
	}
}
