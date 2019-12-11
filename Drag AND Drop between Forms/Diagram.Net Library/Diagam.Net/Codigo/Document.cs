using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Dalssoft.DiagramNet
{
	/// <summary>
	/// This class control the elements collection and visualization.
	/// </summary>
	[Serializable]
	public class Document: IDeserializationCallback 
	{
        //M�todo Dibujo de Elementos
        private OpcionDibujo metododibujo = OpcionDibujo.FixedSize;
       
		//Draw properties
		private SmoothingMode smoothingMode = SmoothingMode.HighQuality;
		private PixelOffsetMode pixelOffsetMode = PixelOffsetMode.Default;
		private CompositingQuality compositingQuality = CompositingQuality.AssumeLinear;

		//Action
		private DesignerAction action = DesignerAction.Select;
		private ElementType elementType = ElementType.RectangleNode;
		private LinkType linkType = LinkType.RightAngle;
       
		// Element Collection
		internal ElementCollection elements = new ElementCollection();

		// Selections Collections
		private ElementCollection selectedElements = new ElementCollection();
		private ElementCollection selectedNodes = new ElementCollection();

		//Document Size
		private Point location = new Point(100, 100);
		private Size size = new Size(0, 0);
		private Size windowSize = new Size(0, 0);

		//Zoom
		private float zoom = 1.0f;

		//Grid
		private Size gridSize = new Size(60, 60);
        private Boolean gridView = true;
        private Boolean gridType = true;
        private Color gridColor = Color.FromName("control");
        private Boolean gridHash = true;
        private Int32 gridanchura = 1;
        private bool snapToGrid = true;

		//Events
		private bool canFireEvents = true;

		public Document(){}

		#region Add Methods
		public void AddElement(BaseElement el)
		{
			elements.Add(el);
			el.AppearanceChanged +=new EventHandler(element_AppearanceChanged);
			OnAppearancePropertyChanged(new EventArgs());
		}

		public void AddElements(ElementCollection els)
		{
			AddElements(els.GetArray());
		}

		public void AddElements(BaseElement[] els)
		{
			elements.EnabledCalc = false;
			foreach (BaseElement el in els)
			{
				this.AddElement(el);
			}
			elements.EnabledCalc = true;
		}

		internal bool CanAddLink(ConnectorElement connStart, ConnectorElement connEnd)
		{
			return ((connStart != connEnd) && (connStart.ParentElement != connEnd.ParentElement));
		}

		public BaseLinkElement AddLink(ConnectorElement connStart, ConnectorElement connEnd)
		{
			if (CanAddLink(connStart, connEnd))
			{
				BaseLinkElement lnk;
				
				if (linkType == LinkType.Straight)
					lnk = new StraightLinkElement(connStart, connEnd);
				else // (linkType == LinkType.RightAngle)
					lnk = new RightAngleLinkElement(connStart, connEnd);

				elements.Add(lnk);
				lnk.AppearanceChanged +=new EventHandler(element_AppearanceChanged);
				OnAppearancePropertyChanged(new EventArgs());
				return lnk;
			}
			else
			{
				return null;
			}
		}
		#endregion

		#region Delete Methods

		public void DeleteElement(BaseElement el)
		{
			if ((el != null) && !(el is ConnectorElement))
			{
				//Delete link
				if (el is BaseLinkElement)
				{
					BaseLinkElement lnk = (BaseLinkElement) el;
					DeleteLink(lnk);
					return;
				}

				//Delete node
				if (el is NodeElement)
				{
					NodeElement conn = ((NodeElement) el);
					foreach (ConnectorElement elconn in conn.Connectors)
					{
						BaseLinkElement lnk;
						for (int i = elconn.Links.Count - 1; i>=0; i--)
						{
							lnk = (BaseLinkElement) elconn.Links[i];
							DeleteLink(lnk);
						}
					}
					
					if (selectedNodes.Contains(el))
						selectedNodes.Remove(el);
				}

				if (SelectedElements.Contains(el))
					selectedElements.Remove(el);

				elements.Remove(el);
				
				OnAppearancePropertyChanged(new EventArgs());	
			}
			el = null;
		}

		public void DeleteElement(Point p)
		{
			BaseElement selectedElement = FindElement(p);
			DeleteElement(selectedElement);
		}

		public void DeleteSelectedElements()
		{
			selectedElements.EnabledCalc = false;
			selectedNodes.EnabledCalc = false;

			for(int i = selectedElements.Count - 1; i >= 0; i-- )
			{
				DeleteElement(selectedElements[i]);
			}

			selectedElements.EnabledCalc = true;
			selectedNodes.EnabledCalc = true;
		}

		public void DeleteLink(BaseLinkElement lnk)
		{
			if (lnk != null)
			{
				lnk.Connector1.RemoveLink(lnk);
				lnk.Connector2.RemoveLink(lnk);
							
				if (elements.Contains(lnk))
					elements.Remove(lnk);
				if (selectedElements.Contains(lnk))
					selectedElements.Remove(lnk);
				OnAppearancePropertyChanged(new EventArgs());
			}
		}
		#endregion

		#region Select Methods
		public void ClearSelection()
		{
			selectedElements.Clear();
			selectedNodes.Clear();
			OnElementSelection(this, new ElementSelectionEventArgs(selectedElements));
		}

		public void SelectElement(BaseElement el)
		{
			selectedElements.Add(el);
			if (el is NodeElement)
			{
				selectedNodes.Add(el);
			}
			if (canFireEvents)
				OnElementSelection(this, new ElementSelectionEventArgs(selectedElements));
		}

		public void SelectElements(BaseElement[] els)
		{
			selectedElements.EnabledCalc = false;
			selectedNodes.EnabledCalc = false;

			canFireEvents = false;
			
			try
			{
				foreach(BaseElement el in els)
				{
					SelectElement(el);
				}
			}
			finally
			{
				canFireEvents = true;
			}
			selectedElements.EnabledCalc = true;
			selectedNodes.EnabledCalc = true;
			
			OnElementSelection(this, new ElementSelectionEventArgs(selectedElements));
		}

		public void SelectElements(Rectangle selectionRectangle)
		{
			selectedElements.EnabledCalc = false;
			selectedNodes.EnabledCalc = false;
			
			// Add all "hitable" elements
			foreach(BaseElement element in elements)
			{
				if (element is IControllable)
				{
					IController ctrl = ((IControllable)element).GetController();
					if (ctrl.HitTest(selectionRectangle))
					{
						if (!(element is ConnectorElement))
							selectedElements.Add(element);
						
						if (element is NodeElement)
							selectedNodes.Add(element);
					}
				}
			}

			//if the seleciont isn't a expecific link, remove links
			// without 2 elements in selection
			if (selectedElements.Count > 1)
			{
				foreach(BaseElement el in elements)
				{
					BaseLinkElement lnk = el as BaseLinkElement;
					if (lnk == null) continue;
					
					if ((!selectedElements.Contains(lnk.Connector1.ParentElement)) ||
						(!selectedElements.Contains(lnk.Connector2.ParentElement)))
					{
						selectedElements.Remove(lnk);
					}
				}
			}

			selectedElements.EnabledCalc = true;
			selectedNodes.EnabledCalc = true;
			
			OnElementSelection(this, new ElementSelectionEventArgs(selectedElements));
		}

		public void SelectAllElements()
		{
			selectedElements.EnabledCalc = false;
			selectedNodes.EnabledCalc = false;

			foreach(BaseElement element in elements)
			{
				if (!(element is ConnectorElement))
					selectedElements.Add(element);
					
				if (element is NodeElement)
					selectedNodes.Add(element);
			}

			selectedElements.EnabledCalc = true;
			selectedNodes.EnabledCalc = true;
			
		}

		public BaseElement FindElement(Point point)
		{
			BaseElement el;
			if ((elements != null) && (elements.Count > 0))
			{
				// First, find elements
				for(int i = elements.Count - 1; i >=0 ; i--)
				{
					el = elements[i];

					if (el is BaseLinkElement)
						continue;

					//Find element in a Connector array
					if (el is NodeElement)
					{
						NodeElement nel = (NodeElement) el;
						foreach(ConnectorElement cel in nel.Connectors)
						{
							IController ctrl = ((IControllable) cel).GetController();
							if (ctrl.HitTest(point))
								return cel;
						}
					}

					//Find element in a Container Element
					if (el is IContainer)
					{
						BaseElement inner = FindInnerElement((IContainer) el, point);
						if (inner != null)
							return inner;
					}

					//Find element by hit test
					if (el is IControllable)
					{
						IController ctrl = ((IControllable) el).GetController();
						if (ctrl.HitTest(point))
							return el;
					}
				}

				// Then, find links
				for(int i = elements.Count - 1; i >=0 ; i--)
				{
					el = elements[i];

					if (!(el is BaseLinkElement))
						continue;
					
					if (el is IControllable)
					{
						IController ctrl = ((IControllable) el).GetController();
						if (ctrl.HitTest(point))
							return el;
					}
				} 
			}
			return null;
		}

		private BaseElement FindInnerElement(IContainer parent, Point hitPos)
		{
			foreach (BaseElement el in parent.Elements)
			{
				if (el is IContainer)
				{
					BaseElement retEl = FindInnerElement((IContainer)el, hitPos);
					if (retEl != null)
						return retEl;
				}
				
				if (el is IControllable)
				{
					IController ctrl = ((IControllable) el).GetController();

					if (ctrl.HitTest(hitPos))
						return el;
				}
			}
			return null;
		}
		#endregion

		#region Position Methods
		public void MoveUpElement(BaseElement el)
		{
			int i = elements.IndexOf(el);
			if (i != elements.Count - 1)
			{
				elements.ChangeIndex(i, i + 1);
				OnAppearancePropertyChanged(new EventArgs());
			}
		}

		public void MoveDownElement(BaseElement el)
		{
			int i = elements.IndexOf(el);
			if (i != 0)
			{
				elements.ChangeIndex(i, i - 1);
				OnAppearancePropertyChanged(new EventArgs());
			}
		}

		public void BringToFrontElement(BaseElement el)
		{
			int i = elements.IndexOf(el);
			for (int x = i + 1; x <= elements.Count - 1; x++)
			{
				elements.ChangeIndex(i, x);
				i = x;
			}
			OnAppearancePropertyChanged(new EventArgs());
		}

		public void SendToBackElement(BaseElement el)
		{
			int i = elements.IndexOf(el);
			for (int x = i - 1; x >= 0; x--)
			{
				elements.ChangeIndex(i, x);
				i = x;
			}
			OnAppearancePropertyChanged(new EventArgs());
		}
		#endregion

		internal void CalcWindow(bool forceCalc)
		{
			elements.CalcWindow(forceCalc);
			selectedElements.CalcWindow(forceCalc);
			selectedNodes.CalcWindow(forceCalc);
		}

		#region Properties

        public OpcionDibujo ElementDrawingMethod
        {
            get
            {
                return metododibujo;
            }

            set
            {
                metododibujo = value;
            }
        }

        public Int32 GridWidth
        {
            get
            {
                return gridanchura;
            }

            set
            {
                gridanchura = value;

            }
        }
      
        public Boolean GridView
        {
            get
            {
                return gridView;
            }

            set
            {
                gridView = value;            
            }
        }

        public Boolean GridType
        {
            get
            {
                return gridType;
            }

            set
            {
                gridType = value;
            }
        }

        public Boolean GridHash
        {
            get
            {
                return gridHash;
            }

            set
            {
                gridHash = value;
            }
        }

        public Color GridColor
        {
            get
            {
                return gridColor;
            }

            set
            {
                gridColor = value;
            }
        }

		public ElementCollection Elements
		{
			get
			{
				return elements;
			}
		}

		public ElementCollection SelectedElements
		{
			get
			{
				return selectedElements;
			}
		}

		public ElementCollection SelectedNodes
		{
			get
			{
				return selectedNodes;
			}
		}

		public Point Location
		{
			get
			{
				return elements.WindowLocation;
			}
		}

		public Size Size
		{
			get
			{
				return elements.WindowSize;
			}
		}

		internal Size WindowSize
		{
			set
			{
				windowSize = value;
			}
		}

		public SmoothingMode SmoothingMode
		{
			get
			{
				return smoothingMode;
			}
			set
			{
				smoothingMode = value;
				OnAppearancePropertyChanged(new EventArgs());
			}
		}

		public PixelOffsetMode PixelOffsetMode
		{
			get
			{
				return pixelOffsetMode;
			}
			set
			{
				pixelOffsetMode = value;
				OnAppearancePropertyChanged(new EventArgs());
			}
		}

		public CompositingQuality CompositingQuality
		{
			get
			{
				return compositingQuality;
			}
			set
			{
				compositingQuality = value;
				OnAppearancePropertyChanged(new EventArgs());
			}
		}

		public DesignerAction Action
		{
			get
			{
				return action;
			}
			set
			{
				action = value;
				OnPropertyChanged(new EventArgs());
			}
		}

		public float Zoom
		{
			get
			{
				return zoom;
			}
			set
			{
				zoom = value;
				OnPropertyChanged(new EventArgs());
			}
		}

		public ElementType ElementType
		{
			get
			{
				return elementType;
			}
			set
			{
				elementType = value;
				OnPropertyChanged(new EventArgs());
			}
		}

		public LinkType LinkType
		{
			get
			{
				return linkType;
			}
			set
			{
				linkType = value;
				OnPropertyChanged(new EventArgs());
			}
		}

		public Size GridSize
		{
			get
			{
				return gridSize;
			}
			set
			{
				gridSize = value;
				OnAppearancePropertyChanged(new EventArgs());
			}
		}

        public bool SnapToGrid
        {
            get
            {
                return snapToGrid;
            }
            set
            {
                snapToGrid = value;
                OnPropertyChanged(new EventArgs());
            }
        }
		#endregion

		#region Draw Methods

        //Dibuja los Elementos dentro del rect�ngulo "clippingRegion"
		internal void DrawElements(Graphics g, Rectangle clippingRegion)
		{
			//Draw Links first
			for (int i = 0; i <= elements.Count - 1; i++)
			{
				BaseElement el = elements[i];
				if ((el is BaseLinkElement) && (NeedDrawElement(el, clippingRegion)))
					el.Draw(g);
											
				if (el is ILabelElement)
					((ILabelElement) el).Label.Draw(g);
			}

			//Draw the other elements
			for (int i = 0; i <= elements.Count - 1; i++)
			{
				BaseElement el = elements[i];

				if (!(el is BaseLinkElement) && (NeedDrawElement(el, clippingRegion)))
				{
					if (el is NodeElement)
					{
						NodeElement n = (NodeElement) el;
						n.Draw(g, (action == DesignerAction.Connect));
					}
					else
					{
						el.Draw(g);
					}

					if (el is ILabelElement)
						((ILabelElement) el).Label.Draw(g);
				}
			}
		}

        //Esta funci�n evalua si un Elemento dado es cortado por el Rect�ngulo de Selecci�n y por tanto si es necesario redibujarlo o no.
        //Esta funci�n recibe dos argumentos:
        //a) Un el Rect�ngulo de Selecci�n enviado por el Usuario.
        //b) El Elemento que vamos a comprobar si corta el rect�ngulo "clippingRegion"
		private bool NeedDrawElement(BaseElement el, Rectangle clippingRegion)
		{
			if (!el.Visible) return false;

			Rectangle elRectangle = el.GetUnsignedRectangle();
			elRectangle.Inflate(5, 5);
			return clippingRegion.IntersectsWith(elRectangle);
		}

        //Esta funci�n Dibuja los elementos dentro del rect�ngulo de selecci�n "clippingRegion"
		internal void DrawSelections(Graphics g, Rectangle clippingRegion)
		{
			for(int i = selectedElements.Count - 1; i >=0 ; i--)
			{
				if (selectedElements[i] is IControllable)
				{
					IController ctrl = ((IControllable) selectedElements[i]).GetController();
					ctrl.DrawSelection(g);

					if (selectedElements[i] is BaseLinkElement)
					{
						BaseLinkElement link = (BaseLinkElement) selectedElements[i];
						ctrl = ((IControllable) link.Connector1).GetController();
						ctrl.DrawSelection(g);

						ctrl = ((IControllable) link.Connector2).GetController();
						ctrl.DrawSelection(g);
					}
				}
			}
		}

        //Funci�n para Dibujar el GRID
		internal void DrawGrid(Graphics g, Rectangle clippingRegion)
		{
//			ControlPaint.DrawGrid(g, clippingRegion, gridSize, Color.LightGray);
            Pen p=new Pen(Color.Red,1);
            Pen p1=new Pen(Color.Blue,1);

            int gridWidth = gridSize.Width;
            int gridHeight = gridSize.Height;
            if (gridWidth == 0 || gridHeight == 0)
                return;
            if (gridWidth < 10)
                gridWidth *= 10 / gridWidth + 1;
            if (gridHeight < 10)
                gridHeight *= 10 / gridHeight + 1;

            if (GridHash == true)
            {
                p = new Pen(new HatchBrush(HatchStyle.LargeGrid | HatchStyle.Percent90, GridColor, Color.Transparent), GridWidth);
            }
            else if (GridHash == false)
            {
                p1 = new Pen(GridColor, GridWidth);
            }
			
			int maxX = location.X + this.Size.Width;
			int maxY = location.Y + this.Size.Height;

			if (windowSize.Width / zoom > maxX)
				maxX = (int)(windowSize.Width / zoom);

			if (windowSize.Height / zoom > maxY)
				maxY = (int)(windowSize.Height / zoom);

            // Create a 1 x 1 bitmap and set the color
            Bitmap pt = new Bitmap(1, 1);
            pt.SetPixel(0, 0, Color.Black);

            if (GridView == false)
            { 
            
            }

            else if (GridView==true)
            {
              if (GridType == true)
              {

                for (int i = 0; i < maxX; i += gridWidth)
                {
                    if (GridHash == true)
                    {
                        g.DrawLine(p, i, 0, i, maxY);

                    }
                    else if (GridHash == false)
                    {
                        g.DrawLine(p1, i, 0, i, maxY);
                    }
                }

                for (int i = 0; i < maxY; i += gridHeight)
                {
                    if (GridHash == true)
                    {
                        g.DrawLine(p, 0, i, maxX, i);
                    }
                    else if (GridHash == false)
                    {
                        g.DrawLine(p1, 0, i, maxX, i);
                    }
                }
              }

              else if (GridType==false)
              {
                for (int i = 0; i < maxX; i += gridWidth)
                {
                    for (int j = 0; j < maxY; j += gridHeight)
                    {
                        g.DrawImageUnscaled(pt, i, j);
                    }
                }
            
              }
            }
			p.Dispose();
		}
		#endregion

		#region Events Raising
		
		// Property Changed
		[field: NonSerialized]
		public event EventHandler PropertyChanged; 

		protected virtual void OnPropertyChanged(EventArgs e)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, e);

            
		}

		// Appearance Property Changed
		[field: NonSerialized]
		public event EventHandler AppearancePropertyChanged;

		protected virtual void OnAppearancePropertyChanged(EventArgs e)
		{
			OnPropertyChanged(e);

			if (AppearancePropertyChanged != null)
				AppearancePropertyChanged(this, e);
		}

		// Element Property Changed
		[field: NonSerialized]
		public event EventHandler ElementPropertyChanged;

		protected virtual void OnElementPropertyChanged(object sender, EventArgs e)
		{
			if (ElementPropertyChanged != null)
				ElementPropertyChanged(sender, e);
		}

		// Element Selection
		public delegate void ElementSelectionEventHandler(object sender, ElementSelectionEventArgs e);
		
		[field: NonSerialized]
		public event ElementSelectionEventHandler ElementSelection;

		protected virtual void OnElementSelection(object sender, ElementSelectionEventArgs e)
		{
			if (ElementSelection != null)
				ElementSelection(sender, e);
		}
		

		#endregion

		#region Events Handling
		private void RecreateEventsHandlers()
		{
			foreach(BaseElement el in elements)
				el.AppearanceChanged +=new EventHandler(element_AppearanceChanged);			
		}

		[SecurityPermissionAttribute(SecurityAction.Demand,SerializationFormatter=true)]
		private void element_AppearanceChanged(object sender, EventArgs e)
		{
			OnElementPropertyChanged(sender, e);
		}
		#endregion
	
		#region IDeserializationCallback Members
		void IDeserializationCallback.OnDeserialization(object sender)
		{
			RecreateEventsHandlers();
		}
		#endregion
	}
}
