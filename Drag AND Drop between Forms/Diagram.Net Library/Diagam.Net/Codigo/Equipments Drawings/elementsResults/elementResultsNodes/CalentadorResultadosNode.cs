using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;


namespace Dalssoft.DiagramNet
{
	[Serializable]
	public class CalentadorResultadoNode: NodeElement, IControllable, ILabelElement
	{
        protected CalentadorElementResultados calentador;
		protected LabelElement label = new LabelElement();
        protected ConnectorElement[] connectors12=new ConnectorElement[10];

		[NonSerialized]
        private CalentadorResultadosController controller;

        
		public  CalentadorResultadoNode(): this(0, 0, 100, 100,9)
		{
        
        }

		public  CalentadorResultadoNode(Rectangle rec,int tipoelemento2): this(rec.Location, rec.Size,tipoelemento2)
		{ 
        
        }

		public  CalentadorResultadoNode(Point l, Size s,int tipoelemento2): this(l.X, l.Y, s.Width, s.Height,tipoelemento2) 
		{
           
        }

        public CalentadorResultadoNode(int top, int left, int width, int height, int tipoelemento2)
            : base(top, left, width, height, tipoelemento2)
		{
            calentador = new CalentadorElementResultados(top, left, width, height);
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
                calentador.BorderColor = value;
				base.BorderColor = value;
			}
		}

		public Color FillColor1
		{
			get
			{
                return calentador.FillColor1;
			}
			set
			{
                calentador.FillColor1 = value;
			}
		}

		public Color FillColor2
		{
			get
			{
                return calentador.FillColor2;
			}
			set
			{
                calentador.FillColor2 = value;
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
                calentador.Opacity = value;
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
                calentador.Visible = value;
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
                calentador.Location = value;
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
                calentador.Size = value;
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
                calentador.BorderWidth = value;
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
            base.location = calentador.Location;
            base.size = calentador.Size;
            base.borderColor = calentador.BorderColor;
            base.borderWidth = calentador.BorderWidth;
            base.opacity = calentador.Opacity;
            base.visible = calentador.Visible;
           
		}

		internal override void Draw(Graphics g)
		{
			IsInvalidated = false;
            calentador.Draw(g);
		}

		IController IControllable.GetController()
		{
			if (controller == null)
                controller = new CalentadorResultadosController(this);
			return controller;
		}
	}
}
