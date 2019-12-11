using System;

namespace Dalssoft.DiagramNet
{
    //Opción de Dibujo de Equipos: tamaño imagen fija ó arrastrar y soltar 
    public enum OpcionDibujo
    {
        FixedSize,
        DragandDropSize
    }

	internal enum CornerPosition: int
	{
		Nothing = -1,
		BottomCenter = 0,
		BottomLeft = 1,
		BottomRight = 2,
		MiddleCenter = 3,
		MiddleLeft = 4,
		MiddleRight = 5,
		TopCenter = 6,
		TopLeft = 7,
		TopRight = 8,
		Undefined = 99
	}

    internal enum CornerPositionforlines : int
    {
        Nothing = -1,
        TopLeft = 0,
        BottomRight = 1,       
        Undefined = 99
    }

	public enum CardinalDirection
	{
		Nothing,
		North,
		South,
		East,
		West
	}

	public enum Orientation
	{
		Horizontal,
		Vertical
	}

	public enum ElementType
	{
		Rectangle,
		RectangleNode,
		Elipse1,
		ElipseNode1,
		CommentBox,
        Image,
        ImageNode,
        Turbina,
        TurbinaNode,
        TurbinaResultadosNode,
        TurbinaResultadosAltaNode,
        BombaResultadosNode,
        ArcoResultados,
        SeparadorHumedadNode,
        ValvulaNode,
        CalentadorNode,
        Generador,
        CondensadorNode,
        CondensadorSellosNode,
        Rectangulo,
        Circulo,
        RectanguloRedondeado,
        DesaireadorNode,
        Linea
	}

	public enum LinkType
	{
		Straight,
		RightAngle
	}

	internal enum LabelEditDirection
	{
		UpDown,
		Both
	}

}
