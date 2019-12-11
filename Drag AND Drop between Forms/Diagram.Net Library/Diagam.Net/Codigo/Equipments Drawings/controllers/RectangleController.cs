using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Dalssoft.DiagramNet
{
	/// <summary>
	/// This class is the controller for RectangleElement
	/// </summary>
	internal class RectangleController: IController, IMoveController, IResizeController
	{
		//parent element
		protected BaseElement el;
		
		//Move vars.
		protected Point dragOffset = new Point(0);
		protected bool isDragging = false;
		protected bool canMove = true;
		
		//Resize vars.
		protected const int selCornerSize = 3;
        //Array de elementos rectangulo que representan los "rectangulitos" para las operaciones de Resize
        //En este caso definimos 9 rectangulitos 
		protected RectangleElement[] selectionCorner = new RectangleElement[9];
        //Definimos un Enum que contiene la posici�n de los Retangulitos
		protected CornerPosition selCorner = CornerPosition.Nothing;
		protected bool canResize = true;

        //Constructor de la Clase Rectangle Controller que recibe como argumento un objeto de la clase BaseElement
		public RectangleController(BaseElement element)
		{
			el = element;

			//Create corners. Inicializamos los rectangulitos con el tama�o elegido anteriormente "selCornerSize=3"
			for(int i = 0; i < selectionCorner.Length; i++)
			{
				selectionCorner[i] = new RectangleElement(0, 0, selCornerSize * 2, selCornerSize * 2);
				selectionCorner[i].BorderColor = Color.Black;
				selectionCorner[i].FillColor1 = Color.White;
				selectionCorner[i].FillColor2 = Color.Empty;
			}
		}
		
        //Implementamos las funciones de la Interface "IController"
		#region IController Members

		public BaseElement OwnerElement
		{
			get
			{
				return el;
			}
		}

        //Funciones que detectan se hemos clickado dentro del rect�ngulo del Elemento (recibe como argumento un punto,donde hemos clickado)
		public virtual bool HitTest(Point p)
		{
			GraphicsPath gp = new GraphicsPath();
			Matrix mtx = new Matrix();

			Point elLocation = el.Location;
			Size elSize = el.Size;
			gp.AddRectangle(new Rectangle(elLocation.X,
				elLocation.Y,
				elSize.Width,
				elSize.Height));
			gp.Transform(mtx);

			return gp.IsVisible(p);
		}

        //Funciones que detectan se hemos clickado dentro del rect�ngulo del Elemento (recibe como argumento un rectangulo, el rectangulo que hemos seleccionado)
		public virtual bool HitTest(Rectangle r)
		{
			GraphicsPath gp = new GraphicsPath();
			Matrix mtx = new Matrix();

			Point elLocation = el.Location;
			Size elSize = el.Size;
			gp.AddRectangle(new Rectangle(elLocation.X,
				elLocation.Y,
				elSize.Width,
				elSize.Height));
			gp.Transform(mtx);
			Rectangle retGp = Rectangle.Round(gp.GetBounds());
			return r.Contains (retGp);
		}

        //Dibujamos el Rect�ngulo de Selecci�n 
		public virtual void DrawSelection(Graphics g)
		{
			int border = 3;

			Point elLocation = el.Location;
			Size elSize = el.Size;

			Rectangle r = BaseElement.GetUnsignedRectangle(
				new Rectangle(
				elLocation.X - border, elLocation.Y - border,
				elSize.Width + (border * 2), elSize.Height + (border * 2)));

			HatchBrush brush = new HatchBrush(HatchStyle.SmallCheckerBoard, Color.LightGray, Color.Transparent);
			Pen p = new Pen(brush, border);
			g.DrawRectangle(p, r);
			
			p.Dispose();
			brush.Dispose();
		}

		#endregion

        //Implementamos las funciones de la Interface "IMoverController"
		#region IMoveController Members

        //Esta funci�n recibe como argumento el punto donde hemos clickado y devuelve el punto hasta donde hemos arrastrado
		void IMoveController.Start(Point posStart)
		{
			Point elLocation = el.Location;
			dragOffset.X = elLocation.X - posStart.X;
			dragOffset.Y = elLocation.Y - posStart.Y;
			isDragging = true;
		}

        //Esta funci�n asigna la nueva posici�n del elemento movido 
		void IMoveController.Move(Point posCurrent)
		{
			if (isDragging)
			{
				Point dragPointEl = posCurrent;
				dragPointEl.Offset(dragOffset.X, dragOffset.Y) ;
				if (dragPointEl.X < 0) dragPointEl.X = 0;
				if (dragPointEl.Y < 0) dragPointEl.Y = 0;

				el.Location = dragPointEl;
			}
		}

		void IMoveController.End()
		{
			isDragging = false;
		}

		bool IMoveController.IsMoving
		{
			get
			{
				return isDragging;
			}
		}

		bool IMoveController.CanMove
		{
			get
			{
				return canMove;
			}
		}

		#endregion

        //Implementamos las funciones de la Interface "IResizeController"
		#region IResizeController Members

		public RectangleElement[] Corners
		{
			get
			{
				return selectionCorner;
			}			
		}

        //Esta funci�n actualiza la posici�n de los Rect�ngulos de Selecci�n
		void IResizeController.UpdateCornersPos()
		{
			// Update selection corner rectangle
			Rectangle rec = new Rectangle(el.Location, el.Size);
          
            selectionCorner[(int)CornerPosition.TopLeft].Location = new Point(rec.Location.X - selCornerSize, rec.Location.Y - selCornerSize);
            selectionCorner[(int)CornerPosition.TopRight].Location = new Point(rec.Location.X + rec.Size.Width - selCornerSize, rec.Location.Y - selCornerSize);
            selectionCorner[(int)CornerPosition.TopCenter].Location = new Point(rec.Location.X + rec.Size.Width / 2 - selCornerSize, rec.Location.Y - selCornerSize);

            selectionCorner[(int)CornerPosition.BottomLeft].Location = new Point(rec.Location.X - selCornerSize, rec.Location.Y + rec.Size.Height - selCornerSize);
            selectionCorner[(int)CornerPosition.BottomRight].Location = new Point(rec.Location.X + rec.Size.Width - selCornerSize, rec.Location.Y + rec.Size.Height - selCornerSize);
            selectionCorner[(int)CornerPosition.BottomCenter].Location = new Point(rec.Location.X + rec.Size.Width / 2 - selCornerSize, rec.Location.Y + rec.Size.Height - selCornerSize);

            selectionCorner[(int)CornerPosition.MiddleLeft].Location = new Point(rec.Location.X - selCornerSize, rec.Location.Y + rec.Size.Height / 2 - selCornerSize);
            selectionCorner[(int)CornerPosition.MiddleCenter].Location = new Point(rec.Location.X + rec.Size.Width / 2 - selCornerSize, rec.Location.Y + rec.Size.Height / 2 - selCornerSize);
            selectionCorner[(int)CornerPosition.MiddleRight].Location = new Point(rec.Location.X + rec.Size.Width - selCornerSize, rec.Location.Y + rec.Size.Height / 2 - selCornerSize);     
        }

        //Esta funci�n detecta dentro de que Rectangulito de Selecci�n hemos clickado
		CornerPosition IResizeController.HitTestCorner(Point p)
		{
			for(int i = 0; i < selectionCorner.Length; i++)
			{
				IController ctrl = ((IControllable) selectionCorner[i]).GetController();
				if (ctrl.HitTest(p))
					return (CornerPosition) i;
			}
			return CornerPosition.Nothing;
		}

        //Esta funci�n calcula el punto final hasta donde relizamos el Resize
		void IResizeController.Start(Point posStart, CornerPosition corner)
		{
			selCorner = corner;
			dragOffset.X = selectionCorner[(int) selCorner].Location.X - posStart.X;
			dragOffset.Y = selectionCorner[(int) selCorner].Location.Y - posStart.Y;
		}

        //Esta funci�n cambia de tama�o el objeto el del cual hemos realizado el Resize
		void IResizeController.Resize(Point posCurrent)
		{
			RectangleElement corner = selectionCorner[(int) selCorner];
			Point loc;

			Point dragPointEl = posCurrent;
			dragPointEl.Offset(dragOffset.X, dragOffset.Y);
			if (dragPointEl.X < 0) dragPointEl.X = 0;
			if (dragPointEl.Y < 0) dragPointEl.Y = 0;

			switch (selCorner)
			{
				case CornerPosition.TopLeft:
					corner.Location = dragPointEl;
					loc = new Point(corner.Location.X + corner.Size.Width / 2, corner.Location.Y + corner.Size.Height / 2);
					el.Size = new Size(el.Size.Width + (el.Location.X - loc.X),
						el.Size.Height + (el.Location.Y - loc.Y));
					el.Location = loc;
					break;

				case CornerPosition.TopCenter:
					corner.Location = new Point(corner.Location.X, dragPointEl.Y);
					loc = new Point(corner.Location.X + corner.Size.Width / 2, corner.Location.Y + corner.Size.Height / 2);
					el.Size = new Size(el.Size.Width,
						el.Size.Height + (el.Location.Y - loc.Y));
					el.Location = new Point(el.Location.X, loc.Y);
					break;

				case CornerPosition.TopRight:
					corner.Location = dragPointEl;
					loc = new Point(corner.Location.X + corner.Size.Width / 2, corner.Location.Y + corner.Size.Height / 2);
					el.Size = new Size(loc.X - el.Location.X,
						el.Size.Height - (loc.Y - el.Location.Y));
					el.Location = new Point(el.Location.X, loc.Y);
					break;

				case CornerPosition.MiddleLeft:
					corner.Location = new Point(dragPointEl.X, corner.Location.Y);
					loc = new Point(corner.Location.X + corner.Size.Width / 2, corner.Location.Y + corner.Size.Height / 2);
					el.Size = new Size(el.Size.Width + (el.Location.X - loc.X),
						el.Size.Height);
					el.Location = new Point(loc.X, el.Location.Y);
					break;

				case CornerPosition.MiddleRight:
					corner.Location = new Point(dragPointEl.X, corner.Location.Y);
					loc = new Point(corner.Location.X + corner.Size.Width / 2, corner.Location.Y + corner.Size.Height / 2);
					el.Size = new Size(loc.X - el.Location.X,
						el.Size.Height);
					break;
							
				case CornerPosition.BottomLeft:
					corner.Location = dragPointEl;
					loc = new Point(corner.Location.X + corner.Size.Width / 2, corner.Location.Y + corner.Size.Height / 2);
					el.Size = new Size(el.Size.Width - (loc.X - el.Location.X),
						loc.Y - el.Location.Y);
					el.Location = new Point(loc.X, el.Location.Y);
					break;

				case CornerPosition.BottomCenter:
					corner.Location = new Point(corner.Location.X, dragPointEl.Y);
					loc = new Point(corner.Location.X + corner.Size.Width / 2, corner.Location.Y + corner.Size.Height / 2);
					el.Size = new Size(el.Size.Width,
						loc.Y - el.Location.Y);
					break;
							
				case CornerPosition.BottomRight:
					corner.Location = dragPointEl;
					loc = new Point(corner.Location.X + corner.Size.Width / 2, corner.Location.Y + corner.Size.Height / 2);
					el.Size = new Size(loc.X - el.Location.X,
						loc.Y - el.Location.Y);
					break;
			}
		}

		void IResizeController.End(Point posEnd)
		{
			if ((el.Size.Height < 0) || (el.Size.Width < 0))
			{
				Rectangle urec = el.GetUnsignedRectangle();
				el.Location = urec.Location;
				el.Size = urec.Size;
			}
			selCorner = CornerPosition.Nothing;
			dragOffset = Point.Empty;
		}
		
		bool IResizeController.IsResizing
		{
			get
			{
				return (selCorner != CornerPosition.Nothing);
			}
		}

		bool IResizeController.CanResize
		{
			get
			{
				return canResize;
			}
		}

		#endregion
	}
}
