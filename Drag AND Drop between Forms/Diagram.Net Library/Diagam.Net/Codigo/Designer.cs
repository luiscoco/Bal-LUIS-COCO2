using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Xml;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel;

using Drag_AND_Drop_between_Forms;

namespace Dalssoft.DiagramNet
{
	public class Designer : System.Windows.Forms.UserControl
	{
		private System.ComponentModel.IContainer components;

        public Int32 TipodeEquipo=1;
        public Image imagen;

		#region Designer Control Initialization
		//Document
		private Document document = new Document();

        //Método de Dibujar Elementos
        public OpcionDibujo metododibujar = OpcionDibujo.FixedSize;

		// Drag and Drop
		MoveAction moveAction = null;

		// Selection
		BaseElement selectedElement;
		private bool isMultiSelection = false;
		private RectangleElement selectionArea = new RectangleElement(0,0,0,0);
		private IController[] controllers;
		private BaseElement mousePointerElement;
	
		// Resize
		private ResizeAction resizeAction = null;

		// Add Element
		private bool isAddSelection = false;
		
		// Link
		private bool isAddLink = false;
		private ConnectorElement connStart;
		private ConnectorElement connEnd;
		private BaseLinkElement linkLine;

		// Label
		private bool isEditLabel = false;
		private LabelElement selectedLabel;
		private System.Windows.Forms.TextBox labelTextBox = new TextBox();
		private EditLabelAction editLabelAction = null;

		//Undo
		[NonSerialized]
		private UndoManager undo = new UndoManager(5);
		private bool changed = false;
        
        //Puntero hacia la Aplicación Principal
        public Aplicacion punteroaplicacion;

        //
        public bool dragdropluis = false;

        //Dibujar Línea


		public Designer()
		{
            // This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// This change control to not flick
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			
			// Selection Area Properties
			selectionArea.Opacity = 40;
			selectionArea.FillColor1 = SystemColors.Control;
			selectionArea.FillColor2 = Color.Empty;
			selectionArea.BorderColor = SystemColors.Control;

			// Link Line Properties
			//linkLine.BorderColor = Color.FromArgb(127, Color.DarkGray);
			//linkLine.BorderWidth = 4;

			// Label Edit
			labelTextBox.BorderStyle = BorderStyle.FixedSingle;
			labelTextBox.Multiline = true;
			labelTextBox.Hide();
			this.Controls.Add(labelTextBox);

			//EventsHandlers
			RecreateEventsHandlers();
		}
		#endregion

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// Designer
			// 
			this.AutoScroll = true;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.Name = "Designer";

		}
		#endregion

		public new void Invalidate()
		{
            if (document.Elements.Count > 0)
            {
                for (int i = 0; i <= document.Elements.Count - 1; i++)
                {
                    BaseElement el = document.Elements[i];

                    Invalidate(el);

                    if (el is ILabelElement)
                    {
                        Invalidate(((ILabelElement)el).Label);
                    }
                }
            }
            else
            {
                base.Invalidate();
            }

           // if ((moveAction != null) && (moveAction.IsMoving))
           // {
           //     this.AutoScrollMinSize = new Size((int)((document.Location.X + document.Size.Width + 10) * document.Zoom), (int)((document.Location.Y + document.Size.Height + 10) * document.Zoom));
           //     base.Invalidate();
           // }
		}

		private void Invalidate(BaseElement el)
		{
			this.Invalidate(el, false);
		}

		private void Invalidate(BaseElement el, bool force)
		{
			if (el == null) return;

			if ((force) || (el.IsInvalidated))
			{
				Rectangle invalidateRec = Goc2Gsc(el.invalidateRec);
				invalidateRec.Inflate(10, 10);
				base.Invalidate(invalidateRec);
			}			
		}

		#region Events Overrides
		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			GraphicsContainer gc;
			Matrix mtx;
			g.PageUnit = GraphicsUnit.Pixel;

			Point scrollPoint = this.AutoScrollPosition;

           
            this.AutoScrollMinSize = new Size((int)((document.Location.X + document.Size.Width + 10) * document.Zoom), (int)((document.Location.Y + document.Size.Height + 10) * document.Zoom));
           

			g.TranslateTransform(scrollPoint.X, scrollPoint.Y);

            //Grid Snap
            //g.DrawEllipse(Pens.Teal, pos.X-5, pos.Y-5, 10, 10);

			//Zoom
			mtx = g.Transform;
			gc = g.BeginContainer();
			
			g.SmoothingMode = document.SmoothingMode;
			g.PixelOffsetMode = document.PixelOffsetMode;
			g.CompositingQuality = document.CompositingQuality;
			
			g.ScaleTransform(document.Zoom, document.Zoom);

			Rectangle clipRectangle = Gsc2Goc(e.ClipRectangle);
         
            document.DrawGrid(g, clipRectangle);

			document.DrawElements(g, clipRectangle);

			if (!((resizeAction != null) && (resizeAction.IsResizing)))
				document.DrawSelections(g, e.ClipRectangle);

			if ((isMultiSelection) || (isAddSelection))
				DrawSelectionRectangle(g);
 
			if (isAddLink)
			{
				linkLine.CalcLink();
				linkLine.Draw(g);
			}
			if ((resizeAction != null) && ( !((moveAction != null) && (moveAction.IsMoving))))
				resizeAction.DrawResizeCorner(g);

			if (mousePointerElement != null)
			{
				if (mousePointerElement is IControllable)
				{
					IController ctrl = ((IControllable) mousePointerElement).GetController();
					ctrl.DrawSelection(g);
				}
			}

			g.EndContainer(gc);
			g.Transform = mtx;

			base.OnPaint(e);

		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground (e);

			Graphics g = e.Graphics;
			GraphicsContainer gc;
			Matrix mtx;
			g.PageUnit = GraphicsUnit.Pixel;
			mtx = g.Transform;
			gc = g.BeginContainer();
			
			Rectangle clipRectangle = Gsc2Goc(e.ClipRectangle);
			
			//document.DrawGrid(g, clipRectangle);

			g.EndContainer(gc);
			g.Transform = mtx;

		}


		protected override void OnKeyDown(KeyEventArgs e)
		{
			//Delete element
			if (e.KeyCode == Keys.Delete)
			{
				DeleteSelectedElements();
				EndGeneralAction();
				base.Invalidate();
			}

			//Undo
			if (e.Control && e.KeyCode == Keys.Z)
			{
				if (undo.CanUndo)
					Undo();
			}

			//Copy
			if ((e.Control) && (e.KeyCode == Keys.C))
			{
				this.Copy();
			}

			//Paste
			if ((e.Control) && (e.KeyCode == Keys.V))
			{
				this.Paste();
			}

			//Cut
			if ((e.Control) && (e.KeyCode == Keys.X))
			{
				this.Cut();
			}

			base.OnKeyDown (e);
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize (e);
			document.WindowSize = this.Size;
		}
//-----------------------------------------------------------------------------------------------------------------------------------
		#region Mouse Events
                  
        protected override void OnMouseDown(MouseEventArgs e)
		{
			Point mousePoint;

			//ShowSelectionCorner((document.Action==DesignerAction.Select));

			switch (document.Action)
			{
				// SELECT
				case DesignerAction.Connect:
				case DesignerAction.Select:
					if (e.Button == MouseButtons.Left)
					{
						mousePoint = Gsc2Goc(new Point(e.X, e.Y));
						
						//Verify resize action
						StartResizeElement(mousePoint);
						if ((resizeAction != null) && (resizeAction.IsResizing)) break;

						//Verify label editing
						if (isEditLabel)
						{
							EndEditLabel();
						}

						// Search element by click
						selectedElement = document.FindElement(mousePoint);	
						
						if (selectedElement != null)
						{
							//Events
							ElementMouseEventArgs eventMouseDownArg = new ElementMouseEventArgs(selectedElement, e.X, e.Y);
							OnElementMouseDown(eventMouseDownArg);

							//Double-click to edit Label
							if ((e.Clicks == 2) && (selectedElement is ILabelElement))
							{
								selectedLabel = ((ILabelElement) selectedElement).Label;
								StartEditLabel();
								break;
							}

                            //Double-click sobre un Elemento tipo Turbina
                            if ((e.Clicks == 2) && (selectedElement is TurbinaNode))
                            {
                                punteroaplicacion.numequipos++;
                                Turbina luisturbina = new Turbina(punteroaplicacion, punteroaplicacion.numecuaciones, punteroaplicacion.numvariables,0,0);
                                luisturbina.ShowDialog();
                            }
                            
							// Element selected
							if (selectedElement is ConnectorElement)
							{
								StartAddLink((ConnectorElement) selectedElement, mousePoint);
								selectedElement = null;
							}
							else
								StartSelectElements(selectedElement, mousePoint);
						}
						else
						{
							// If click is on neutral area, clear selection
							document.ClearSelection();
							Point p = Gsc2Goc(new Point(e.X, e.Y));;
							isMultiSelection = true;
							selectionArea.Visible = true;
							selectionArea.Location = p;
							selectionArea.Size = new Size(0, 0);
							
							if (resizeAction != null)
								resizeAction.ShowResizeCorner(false);
						}
						base.Invalidate();
					}
					break;

				// ADD
				case DesignerAction.Add:

					if (e.Button == MouseButtons.Left)
					{
						mousePoint = Gsc2Goc(new Point(e.X, e.Y));
						StartAddElement(mousePoint);
					}
					break;

				// DELETE
				case DesignerAction.Delete:
					if (e.Button == MouseButtons.Left)
					{
						mousePoint = Gsc2Goc(new Point(e.X, e.Y));
						DeleteElement(mousePoint);
					}					
					break;
			}
			
			base.OnMouseDown(e);
            //base.OnMouseDown(this.MouseSnap(e));
		}

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            punteroaplicacion.textBox10.Text = "Nº Steps: " + Convert.ToString(e.Delta);

            base.OnMouseWheel(e);
        }

		protected override void OnMouseMove(MouseEventArgs e)
		{
            Point mousePoint1 = Gsc2Goc(new Point(e.X, e.Y));
            punteroaplicacion.textBox8.Text = "Position X1: " + Convert.ToString(e.X) + "  Position Y1: " + Convert.ToString(e.Y);
            punteroaplicacion.textBox11.Text = "Position X1: " + Convert.ToString(e.X) + "  Position Y1: " + Convert.ToString(e.Y);

            Rectangle luis = new Rectangle(mousePoint1, new Size(60, 60));
            if (dragdropluis == true)
            {
                EndAddElement(luis);
                dragdropluis = false;
            }

			if (e.Button == MouseButtons.None)
			{
				this.Cursor = Cursors.Arrow;
				Point mousePoint = Gsc2Goc(new Point(e.X, e.Y));

				if ((resizeAction != null)
					&& ((document.Action == DesignerAction.Select)				
						|| ((document.Action == DesignerAction.Connect)
							&& (resizeAction.IsResizingLink))))
				{
					this.Cursor = resizeAction.UpdateResizeCornerCursor(mousePoint);
				}
				
				if (document.Action == DesignerAction.Connect)
				{
					BaseElement mousePointerElementTMP = document.FindElement(mousePoint);
					if (mousePointerElement != mousePointerElementTMP)
					{
						if (mousePointerElementTMP is ConnectorElement)
						{
							mousePointerElement = mousePointerElementTMP;
							mousePointerElement.Invalidate();
							this.Invalidate(mousePointerElement, true);
						}
						else if (mousePointerElement != null)
						{
							mousePointerElement.Invalidate();
							this.Invalidate(mousePointerElement, true);
							mousePointerElement = null;
						}
						
					}
				}
				else
				{
					this.Invalidate(mousePointerElement, true);
					mousePointerElement = null;
				}
			}			

			if (e.Button == MouseButtons.Left)
			{
				Point dragPoint = Gsc2Goc(new Point(e.X, e.Y));

				if ((resizeAction != null) && (resizeAction.IsResizing))
				{
					resizeAction.Resize(dragPoint);
					this.Invalidate();					
				}

				if ((moveAction != null) && (moveAction.IsMoving))
				{
					moveAction.Move(dragPoint);
					this.Invalidate();
				}
				
				if ((isMultiSelection) || (isAddSelection))
				{
					Point p = Gsc2Goc(new Point(e.X, e.Y));
					selectionArea.Size = new Size (p.X - selectionArea.Location.X, p.Y - selectionArea.Location.Y);
					selectionArea.Invalidate();
					this.Invalidate(selectionArea, true);
				}
				
				if (isAddLink)
				{
					selectedElement = document.FindElement(dragPoint);
					if ((selectedElement is ConnectorElement) 
						&& (document.CanAddLink(connStart, (ConnectorElement) selectedElement)))
						linkLine.Connector2 = (ConnectorElement) selectedElement;
					else
						linkLine.Connector2 = connEnd;

					IMoveController ctrl = (IMoveController) ((IControllable) connEnd).GetController();
					ctrl.Move(dragPoint);
					
					//this.Invalidate(linkLine, true); //TODO
					base.Invalidate();
				}
			}

			base.OnMouseMove (e);
            //base.OnMouseMove(this.MouseSnap(e));
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			Rectangle selectionRectangle = selectionArea.GetUnsignedRectangle();
			
			if ((moveAction != null) && (moveAction.IsMoving))
			{
				ElementEventArgs eventClickArg = new ElementEventArgs(selectedElement);
				OnElementClick(eventClickArg);

				moveAction.End();
				moveAction = null;

				ElementMouseEventArgs eventMouseUpArg = new ElementMouseEventArgs(selectedElement, e.X, e.Y);
				OnElementMouseUp(eventMouseUpArg);
				
				if (changed)
					AddUndo();
			}

			// Select
			if (isMultiSelection)
			{
				EndSelectElements(selectionRectangle);
			}
			// Add element
			else if (isAddSelection)
			{
				EndAddElement(selectionRectangle);
			}
			
			// Add link
			else if (isAddLink)
			{
				Point mousePoint = Gsc2Goc(new Point(e.X, e.Y));
				EndAddLink();
				
				AddUndo();
			}
			
			// Resize
			if (resizeAction != null)
			{
				if (resizeAction.IsResizing)
				{
					Point mousePoint = Gsc2Goc(new Point(e.X, e.Y));
					resizeAction.End(mousePoint);
				
					AddUndo();
				}
				resizeAction.UpdateResizeCorner();
			}

			RestartInitValues();

			base.Invalidate();

			base.OnMouseUp(e);
            //base.OnMouseUp(this.MouseSnap(e));

		}

		#endregion

		#endregion
		
		#region Events Raising
		
		// element handler
		public delegate void ElementEventHandler(object sender, ElementEventArgs e);

		#region Element Mouse Events
		
		// CLICK
		[Category("Element")]
		public event ElementEventHandler ElementClick;
		
		protected virtual void OnElementClick(ElementEventArgs e)
		{
			if (ElementClick != null)
			{
				ElementClick(this, e);
			}
		}

		// mouse handler
		public delegate void ElementMouseEventHandler(object sender, ElementMouseEventArgs e);

		// MOUSE DOWN
		[Category("Element")]
		public event ElementMouseEventHandler ElementMouseDown;
		
		protected virtual void OnElementMouseDown(ElementMouseEventArgs e)
		{
			if (ElementMouseDown != null)
			{
				ElementMouseDown(this, e);
			}
		}

		// MOUSE UP
		[Category("Element")]
		public event ElementMouseEventHandler ElementMouseUp;
		
		protected virtual void OnElementMouseUp(ElementMouseEventArgs e)
		{
			if (ElementMouseUp != null)
			{
				ElementMouseUp(this, e);
			}
		}

		#endregion
		 
		#region Element Move Events
		// Before Move
		[Category("Element")]
		public event ElementEventHandler ElementMoving;
		
		protected virtual void OnElementMoving(ElementEventArgs e)
		{
			if (ElementMoving != null)
			{
				ElementMoving(this, e);
			}
		}

		// After Move
		[Category("Element")]
		public event ElementEventHandler ElementMoved;
		
		protected virtual void OnElementMoved(ElementEventArgs e)
		{
			if (ElementMoved != null)
			{
				ElementMoved(this, e);
			}
		}
		#endregion

		#region Element Resize Events
		// Before Resize
		[Category("Element")]
		public event ElementEventHandler ElementResizing;
		
		protected virtual void OnElementResizing(ElementEventArgs e)
		{
			if (ElementResizing != null)
			{
				ElementResizing(this, e);
			}
		}

		// After Resize
		[Category("Element")]
		public event ElementEventHandler ElementResized;
		
		protected virtual void OnElementResized(ElementEventArgs e)
		{
			if (ElementResized != null)
			{
				ElementResized(this, e);
			}
		}
		#endregion

		#region Element Connect Events
		// connect handler
		public delegate void ElementConnectEventHandler(object sender, ElementConnectEventArgs e);

		// Before Connect
		[Category("Element")]
		public event ElementConnectEventHandler ElementConnecting;
		
		protected virtual void OnElementConnecting(ElementConnectEventArgs e)
		{
			if (ElementConnecting != null)
			{
				ElementConnecting(this, e);
			}
		}

		// After Connect
		[Category("Element")]
		public event ElementConnectEventHandler ElementConnected;
		
		protected virtual void OnElementConnected(ElementConnectEventArgs e)
		{
			if (ElementConnected != null)
			{
				ElementConnected(this, e);
			}
		}
		#endregion

		#region Element Selection Events
		// connect handler
		public delegate void ElementSelectionEventHandler(object sender, ElementSelectionEventArgs e);

		// Selection
		[Category("Element")]
		public event ElementSelectionEventHandler ElementSelection;
		
		protected virtual void OnElementSelection(ElementSelectionEventArgs e)
		{
			if (ElementSelection != null)
			{
				ElementSelection(this, e);
			}
		}

		#endregion

		#endregion

		#region Events Handling
		private void document_PropertyChanged(object sender, EventArgs e)
		{
			if (!IsChanging())
			{
				base.Invalidate();
			}
		}

		private void document_AppearancePropertyChanged(object sender, EventArgs e)
		{
			if (!IsChanging())
			{
				AddUndo();
				base.Invalidate();
			}
		}

		private void document_ElementPropertyChanged(object sender, EventArgs e)
		{
			changed = true;

			if (!IsChanging())
			{
				AddUndo();
				base.Invalidate();
			}
		}

		private void document_ElementSelection(object sender, ElementSelectionEventArgs e)
		{
			OnElementSelection(e);
		}
		#endregion

		#region Properties

     
		public Document Document
		{
			get
			{
				return document;
			}
		}

		public bool CanUndo
		{
			get
			{
				return undo.CanUndo;
			}
		}

		public bool CanRedo
		{
			get
			{
				return undo.CanRedo;
			}
		}


		private bool IsChanging()
		{
			return (
					((moveAction != null) && (moveAction.IsMoving)) //isDragging
					|| isAddLink || isMultiSelection || 
					((resizeAction != null) && (resizeAction.IsResizing)) //isResizing
					);
		}
		#endregion
		
		#region Draw Methods

		/// <summary>
		/// Graphic surface coordinates to graphic object coordinates.
		/// </summary>
		/// <param name="p">Graphic surface point.</param>
		/// <returns></returns>
		public Point Gsc2Goc(Point gsp)
		{
			float zoom = document.Zoom;
			gsp.X = (int) ((gsp.X - this.AutoScrollPosition.X) / zoom);
			gsp.Y = (int) ((gsp.Y - this.AutoScrollPosition.Y) / zoom);
			return gsp;
		}

		public Rectangle Gsc2Goc(Rectangle gsr)
		{
			float zoom = document.Zoom;
			gsr.X = (int) ((gsr.X - this.AutoScrollPosition.X) / zoom);
			gsr.Y = (int) ((gsr.Y - this.AutoScrollPosition.Y) / zoom);
			gsr.Width = (int) (gsr.Width / zoom);
			gsr.Height = (int) (gsr.Height / zoom);
			return gsr;
		}

		public Rectangle Goc2Gsc(Rectangle gsr)
		{
			float zoom = document.Zoom;
			gsr.X = (int) ((gsr.X + this.AutoScrollPosition.X) * zoom);
			gsr.Y = (int) ((gsr.Y + this.AutoScrollPosition.Y) * zoom);
			gsr.Width = (int) (gsr.Width * zoom);
			gsr.Height = (int) (gsr.Height * zoom);
			return gsr;
		}

		internal void DrawSelectionRectangle(Graphics g)
		{
			selectionArea.Draw(g);
		}
		#endregion

		#region Open/Save File
		public void Save(string fileName)
		{
			IFormatter formatter = new BinaryFormatter();
			Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
			formatter.Serialize(stream, document);
			stream.Close();
		}

		public void Open(string fileName)
		{
			IFormatter formatter = new BinaryFormatter();
			Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			document = (Document) formatter.Deserialize(stream);
			stream.Close();
			RecreateEventsHandlers();
		}
		#endregion

		#region Copy/Paste
		public void Copy()
		{
			if (document.SelectedElements.Count == 0) return;

			IFormatter formatter = new BinaryFormatter();
			Stream stream = new MemoryStream();
			formatter.Serialize(stream, document.SelectedElements.GetArray());
			DataObject data = new DataObject(DataFormats.GetFormat("Diagram.NET Element Collection").Name,
				stream);
			Clipboard.SetDataObject(data);
		}

		public void Paste()
		{
			const int pasteStep = 20;

			undo.Enabled = false;
			IDataObject iData = Clipboard.GetDataObject();
			DataFormats.Format format = DataFormats.GetFormat("Diagram.NET Element Collection");
			if (iData.GetDataPresent(format.Name))
			{
				IFormatter formatter = new BinaryFormatter();
				Stream stream = (MemoryStream) iData.GetData(format.Name);
				BaseElement[] elCol = (BaseElement[]) formatter.Deserialize(stream);
				stream.Close();

				foreach(BaseElement el in elCol)
				{
					el.Location = new Point(el.Location.X + pasteStep, el.Location.Y + pasteStep);
				}

				document.AddElements(elCol);
				document.ClearSelection();
				document.SelectElements(elCol);
			}
			undo.Enabled = true;
				
			AddUndo();
			EndGeneralAction();
		}

		public void Cut()
		{
			this.Copy();
			DeleteSelectedElements();
			EndGeneralAction();
		}
		#endregion

		#region Start/End Actions and General Functions
		
		#region General
		private void EndGeneralAction()
		{
			RestartInitValues();
			
			if (resizeAction != null) resizeAction.ShowResizeCorner(false);
		}
		
		private void RestartInitValues()
		{
			
			// Reinitialize status
			moveAction = null;

			isMultiSelection = false;
			isAddSelection = false;
			isAddLink = false;

			changed = false;

			connStart = null;
			
			selectionArea.FillColor1 = SystemColors.Control;
			selectionArea.BorderColor = SystemColors.Control;
			selectionArea.Visible = false;

			document.CalcWindow(true);
		}

		#endregion

		#region Selection
		private void StartSelectElements(BaseElement selectedElement, Point mousePoint)
		{
			// Vefiry if element is in selection
			if (!document.SelectedElements.Contains(selectedElement))
			{
				//Clear selection and add new element to selection
				document.ClearSelection();
				document.SelectElement(selectedElement);
			}

			changed = false;
			

			moveAction = new MoveAction();
			MoveAction.OnElementMovingDelegate onElementMovingDelegate = new Dalssoft.DiagramNet.MoveAction.OnElementMovingDelegate(OnElementMoving);
			moveAction.Start(mousePoint, document, onElementMovingDelegate);


			// Get Controllers
			controllers = new IController[document.SelectedElements.Count];
			for(int i = document.SelectedElements.Count - 1; i >= 0; i--)
			{
				if (document.SelectedElements[i] is IControllable)
				{
					// Get General Controller
					controllers[i] = ((IControllable) document.SelectedElements[i]).GetController();
				}
				else
				{
					controllers[i] = null;
				}
			}

			resizeAction = new ResizeAction();
			resizeAction.Select(document);
		}

		private void EndSelectElements(Rectangle selectionRectangle)
		{
			document.SelectElements(selectionRectangle);
		}
		#endregion		

		#region Resize
		private void StartResizeElement(Point mousePoint)
		{
			if ((resizeAction != null)
				&& ((document.Action == DesignerAction.Select)				
					|| ((document.Action == DesignerAction.Connect)
						&& (resizeAction.IsResizingLink))))
			{
				ResizeAction.OnElementResizingDelegate onElementResizingDelegate = new ResizeAction.OnElementResizingDelegate(OnElementResizing);
				resizeAction.Start(mousePoint, onElementResizingDelegate);
				if (!resizeAction.IsResizing)
					resizeAction = null;
			}
		}
		#endregion

		#region Link
		private void StartAddLink(ConnectorElement connStart, Point mousePoint)
		{
			if (document.Action == DesignerAction.Connect)
			{
				this.connStart = connStart;
				this.connEnd = new ConnectorElement(connStart.ParentElement);

				connEnd.Location = connStart.Location;
				IMoveController ctrl = (IMoveController) ((IControllable) connEnd).GetController();
				ctrl.Start(mousePoint);

				isAddLink = true;
				
				switch(document.LinkType)
				{
					case (LinkType.Straight):
						linkLine = new StraightLinkElement(connStart, connEnd);
						break;
					case (LinkType.RightAngle):
						linkLine = new RightAngleLinkElement(connStart, connEnd);
						break;
				}
				linkLine.Visible = true;
				linkLine.BorderColor = Color.FromArgb(150, Color.Black);
				linkLine.BorderWidth = 1;
				
				this.Invalidate(linkLine, true);
				
				OnElementConnecting(new ElementConnectEventArgs(connStart.ParentElement, null, linkLine));
			}
		}

		private void EndAddLink()
		{
			if (connEnd != linkLine.Connector2)
			{
				linkLine.Connector1.RemoveLink(linkLine);
				linkLine = document.AddLink(linkLine.Connector1, linkLine.Connector2);
				OnElementConnected(new ElementConnectEventArgs(linkLine.Connector1.ParentElement, linkLine.Connector2.ParentElement, linkLine));
			}

			connStart = null;
			connEnd = null;
			linkLine = null;
		}
		#endregion

		#region Add Element
		private void StartAddElement(Point mousePoint)
		{
			document.ClearSelection();

			//Change Selection Area Color
			selectionArea.FillColor1 = Color.LightSteelBlue;
			selectionArea.BorderColor = Color.WhiteSmoke;

			isAddSelection = true;
			selectionArea.Visible = true;
			selectionArea.Location = mousePoint;
			selectionArea.Size = new Size(0, 0);		
		}

		private void EndAddElement(Rectangle selectionRectangle)
		{
            BaseElement el;

            if (metododibujar == OpcionDibujo.FixedSize)
            {
                //IMPORTANTE, ha de fijarse el mismo tamaño de imagen en los siguiente sitios para conseguir la nitidez:
                // a) En el tamaño original de la imagen *.bmp
                // b) En el tamaño de la ImageList 
                // c) En la siguiente línea de Código
                Point localizacion = new Point((selectionRectangle.Location.X - 30), (selectionRectangle.Location.Y - 30));            
                Size tamaño = new Size(35, 35);
                Rectangle recluis = new Rectangle(localizacion, tamaño);

                switch (document.ElementType)
                {
                    case ElementType.Rectangle:
                        el = new RectangleElement(recluis);
                        break;
                    case ElementType.RectangleNode:
                        el = new RectangleNode(recluis, 1);
                        break;
                    case ElementType.Elipse1:
                        el = new ElipseElement(recluis);
                        break;
                    case ElementType.Image:
                        el = new ImageElement(recluis, TipodeEquipo, imagen);
                        break;
                    case ElementType.ImageNode:
                        el = new ImageNode(recluis, TipodeEquipo, imagen);
                        break;
                    case ElementType.ElipseNode1:
                        el = new ElipseNode(recluis, 1);
                        break;
                    case ElementType.Turbina:
                        el = new TurbinaElement(recluis);
                        break;
                    case ElementType.TurbinaNode:
                        el = new TurbinaNode(recluis, 9);
                        break;
                    case ElementType.CommentBox:
                        el = new CommentBoxElement(recluis);
                        break;
                    default:
                        el = new RectangleNode(recluis, 1);
                        break;
                }

                document.AddElement(el);
                document.Action = DesignerAction.Add;
            }

            else if(metododibujar==OpcionDibujo.DragandDropSize)
            {
                switch (document.ElementType)
                {
                    case ElementType.Linea:
                        el = new LineaElementResultados(selectionRectangle);
                        break;
                    case ElementType.Rectangle:
                        el = new RectangleElement(selectionRectangle);
                        break;
                    case ElementType.RectangleNode:
                        el = new RectangleNode(selectionRectangle, 1);
                        break;
                    case ElementType.Elipse1:
                        el = new ElipseElement(selectionRectangle);
                        break;
                    case ElementType.Image:
                        el = new ImageElement(selectionRectangle, TipodeEquipo, imagen);
                        break;
                    case ElementType.ImageNode:
                        el = new ImageNode(selectionRectangle, TipodeEquipo, imagen);
                        break;
                    case ElementType.ElipseNode1:
                        el = new ElipseNode(selectionRectangle, 1);
                        break;
                    case ElementType.Turbina:
                        el = new TurbinaElement(selectionRectangle);
                        break;
                    case ElementType.TurbinaResultadosNode:
                        el = new TurbinaResultadoNode(selectionRectangle,9);
                        break;
                    case ElementType.SeparadorHumedadNode:
                        el = new SeparadorHumedadResultadoNode(selectionRectangle, 1);
                        break;
                    case ElementType.ArcoResultados:
                        el = new ArcoElementResultados (selectionRectangle);
                        break;
                    case ElementType.TurbinaResultadosAltaNode:
                        el = new TurbinaAltaResultadoNode(selectionRectangle, 1);
                        break;
                    case ElementType.CondensadorSellosNode:
                        el = new CondensadorSellosResultadoNode(selectionRectangle, 1);
                        break;
                    case ElementType.DesaireadorNode:
                        el = new DesaireadorResultadoNode(selectionRectangle, 1);
                        break;
                    case ElementType.RectanguloRedondeado:
                        el = new RectanguloRedondeadoElementResultados(selectionRectangle);
                        break;
                    case ElementType.BombaResultadosNode:
                        el = new BombaResultadoNode(selectionRectangle, 9);
                        break;
                    case ElementType.ValvulaNode:
                        el = new ValvulaResultadoNode(selectionRectangle, 9);
                        break;
                    case ElementType.Rectangulo:
                        el = new RectanguloElementResultados(selectionRectangle);
                        break;
                    case ElementType.Circulo:
                        el = new CirculoElementResultados(selectionRectangle);
                        break;
                    case ElementType.CondensadorNode:
                        el = new CondensadorResultadoNode(selectionRectangle,1);
                        break;
                    case ElementType.Generador:
                        el = new GeneradorElementResultados(selectionRectangle);
                        break;
                    case ElementType.CalentadorNode:
                        el = new CalentadorResultadoNode(selectionRectangle, 1);
                        break;
                    case ElementType.TurbinaNode:
                        el = new TurbinaNode(selectionRectangle, 1);
                        break;
                    case ElementType.CommentBox:
                        el = new CommentBoxElement(selectionRectangle);
                        break;
                    default:
                        el = new RectangleNode(selectionRectangle, 1);
                        break;
                }

                document.AddElement(el);
                document.Action = DesignerAction.Add;
            }
		}
		#endregion

		#region Edit Label
		private void StartEditLabel()
		{
			isEditLabel = true;

			// Disable resize
			if (resizeAction != null)
			{	
				resizeAction.ShowResizeCorner(false);
				resizeAction = null;
			}
			
			editLabelAction = new EditLabelAction();
			editLabelAction.StartEdit(selectedElement, labelTextBox);
		}

		private void EndEditLabel()
		{
			if (editLabelAction != null)
			{
				editLabelAction.EndEdit();
				editLabelAction = null;
			}
			isEditLabel = false;
		}
		#endregion

		#region Delete
		private void DeleteElement(Point mousePoint)
		{
			document.DeleteElement(mousePoint);
			selectedElement = null;
			document.Action = DesignerAction.Select;		
		}

		private void DeleteSelectedElements()
		{
			document.DeleteSelectedElements();
		}
		#endregion

		#endregion

		#region Undo/Redo
		public void Undo()
		{
			document = (Document) undo.Undo();
			RecreateEventsHandlers();
			if (resizeAction != null) resizeAction.UpdateResizeCorner();
			base.Invalidate();
		}

		public void Redo()
		{
			document = (Document) undo.Redo();
			RecreateEventsHandlers();
			if (resizeAction != null) resizeAction.UpdateResizeCorner();
			base.Invalidate();
		}

		private void AddUndo()
		{
			undo.AddUndo(document);
		}
		#endregion

		private void RecreateEventsHandlers()
		{
			document.PropertyChanged += new EventHandler(document_PropertyChanged);
			document.AppearancePropertyChanged+=new EventHandler(document_AppearancePropertyChanged);
			document.ElementPropertyChanged += new EventHandler(document_ElementPropertyChanged);
			document.ElementSelection += new Document.ElementSelectionEventHandler(document_ElementSelection);
		}
	}
}
