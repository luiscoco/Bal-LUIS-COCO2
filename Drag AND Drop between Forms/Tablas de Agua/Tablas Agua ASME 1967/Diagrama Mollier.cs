using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TablasAgua1967;

using ZedGraph;

namespace Drag_AND_Drop_between_Forms
{
    public partial class Diagrama_Mollier : Form
    {
        public Diagrama_Mollier()
        {
            InitializeComponent();
        }

        private void Diagrama_Mollier_Load(object sender, EventArgs e)
        {

            this.button1_Click(sender,e);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Class1 luis = new Class1();

            if (zg1 != null)
            {
                zg1.Dispose();
            }

            this.zg1 = new ZedGraph.ZedGraphControl();

            // zg1
            // 
            this.zg1.EditButtons = System.Windows.Forms.MouseButtons.Left;
            this.zg1.Location = new System.Drawing.Point(65, 39);
            this.zg1.Name = "zg1";
            this.zg1.PanModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)));
            this.zg1.ScrollGrace = 0D;
            this.zg1.ScrollMaxX = 0D;
            this.zg1.ScrollMaxY = 0D;
            this.zg1.ScrollMaxY2 = 0D;
            this.zg1.ScrollMinX = 0D;
            this.zg1.ScrollMinY = 0D;
            this.zg1.ScrollMinY2 = 0D;
            this.zg1.Size = new System.Drawing.Size(800, 550);
            this.zg1.TabIndex = 0;
            // 

            // 
            GraphPane myPane = zg1.GraphPane;

            this.Controls.Add(this.zg1);

            // Set the titles and axis labels
            myPane.Title.Text = "Diagrama de Mollier. Autor:Luis Coco";
            myPane.XAxis.Title.Text = "Valores de Entropia - BTU/LBxF ";
            myPane.YAxis.Title.Text = "Valores de Temperatura - F";
            myPane.Title.FontSpec.Size = 8;
            myPane.YAxis.Title.FontSpec.Size = 8;
            myPane.XAxis.Title.FontSpec.Size = 8;
            myPane.XAxis.Scale.FontSpec.Size = 8;
            myPane.YAxis.Scale.FontSpec.Size = 8;
            myPane.XAxis.Scale.Max = 2.2;
            myPane.YAxis.Scale.Max = 700;
            myPane.Legend.IsVisible = false;

            //Tamaño de los puntos(círculitos) en las gráficas
            Symbol.Default.Size = 2;

            // Make up some data points from the Sine function
            PointPairList list = new PointPairList();
            PointPairList list1 = new PointPairList();
            PointPairList list2 = new PointPairList();
            PointPairList list3 = new PointPairList();
            PointPairList list4 = new PointPairList();
            PointPairList list5 = new PointPairList();
            PointPairList list6 = new PointPairList();
            PointPairList list7 = new PointPairList();
            PointPairList list8 = new PointPairList();

            PointPairList list9 = new PointPairList();
            PointPairList list10 = new PointPairList();
            PointPairList list11 = new PointPairList();
            PointPairList list12 = new PointPairList();


            //LINEAS CON TITULO CONSTANTE X=CTE.
            //Vapor saturado con X=100
            Double entropiavapor = 0;
            Double calidadvaporsaturado = 100;
            Double presionsaturacion = 0;
            for (int temperatura = 45; temperatura < 680; temperatura = temperatura + 15)
            {
                presionsaturacion = luis.psl(temperatura, 1);
                entropiavapor = luis.shvptx(presionsaturacion, temperatura, calidadvaporsaturado, 1);
                list.Add(entropiavapor, temperatura);
            }

            //Liquido saturado con X=0
            Double calidadliquidosaturado = 0;
            Double entropialiquido = 0;
            Double presionsaturacion1 = 0;
            for (int temperatura1 = 45; temperatura1 < 680; temperatura1 = temperatura1 + 15)
            {
                presionsaturacion1 = luis.psl(temperatura1, 1);
                entropialiquido = luis.shvptx(presionsaturacion1, temperatura1, calidadliquidosaturado, 1);
                list4.Add(entropialiquido, temperatura1);
            }

            //Linea de titulo constante con X=80
            Double entropiavapor3 = 0;
            Double calidadvaporsaturado3 = Convert.ToDouble(textBox1.Text);
            Double presionsaturacion3 = 0;
            for (int temperatura = 45; temperatura < 670; temperatura = temperatura + 15)
            {
                presionsaturacion3 = luis.psl(temperatura, 1);
                entropiavapor3 = luis.shvptx(presionsaturacion3, temperatura, calidadvaporsaturado3, 1);
                list5.Add(entropiavapor3, temperatura);
            }

            Double presionsaturacion47 = luis.psl(100, 1);
            Double entropialiquido47 = luis.shvptx(presionsaturacion47, 100, calidadvaporsaturado3, 1);
            // Add a text item to decorate the graph
            TextObj text57 = new TextObj("X: " + Convert.ToString(calidadvaporsaturado3), (float)entropialiquido47, 100);
            // Align the text such that the Bottom-Center is at (175, 80) in user scale coordinates
            text57.Location.AlignH = AlignH.Center;
            text57.Location.AlignV = AlignV.Bottom;
            text57.FontSpec.Size = 7;
            text57.FontSpec.Fill = new Fill(Color.Green, Color.White, 45F);
            text57.FontSpec.StringAlignment = StringAlignment.Near;
            myPane.GraphObjList.Add(text57);

            //Liquido de titulos constante con X=60
            Double calidadliquidosaturado5 = 60;
            Double entropialiquido5 = 0;
            Double presionsaturacion5 = 0;
            for (int temperatura1 = 45; temperatura1 < 670; temperatura1 = temperatura1 + 15)
            {
                presionsaturacion5 = luis.psl(temperatura1, 1);
                entropialiquido5 = luis.shvptx(presionsaturacion5, temperatura1, calidadliquidosaturado5, 1);
                list7.Add(entropialiquido5, temperatura1);
            }

            Double presionsaturacion46 = luis.psl(100, 1);
            Double entropialiquido46 = luis.shvptx(presionsaturacion46, 100, calidadliquidosaturado5, 1);
            // Add a text item to decorate the graph
            TextObj text56 = new TextObj("X: " + Convert.ToString(calidadliquidosaturado5), (float)entropialiquido46, 100);
            // Align the text such that the Bottom-Center is at (175, 80) in user scale coordinates
            text56.Location.AlignH = AlignH.Center;
            text56.Location.AlignV = AlignV.Bottom;
            text56.FontSpec.Size = 7;
            text56.FontSpec.Fill = new Fill(Color.Green, Color.White, 45F);
            text56.FontSpec.StringAlignment = StringAlignment.Near;
            myPane.GraphObjList.Add(text56);


            //Liquido de titulos constante con X=20
            Double calidadliquidosaturado4 = 20;
            Double entropialiquido4 = 0;
            Double presionsaturacion4 = 0;
            for (int temperatura1 = 45; temperatura1 < 670; temperatura1 = temperatura1 + 15)
            {
                presionsaturacion4 = luis.psl(temperatura1, 1);
                entropialiquido4 = luis.shvptx(presionsaturacion4, temperatura1, calidadliquidosaturado4, 1);
                list6.Add(entropialiquido4, temperatura1);
            }

            Double presionsaturacion45 = luis.psl(100, 1);
            Double entropialiquido45 = luis.shvptx(presionsaturacion45, 100, calidadliquidosaturado4, 1);
            // Add a text item to decorate the graph
            TextObj text55 = new TextObj("X: " + Convert.ToString(calidadliquidosaturado4), (float)entropialiquido45, 100);
            // Align the text such that the Bottom-Center is at (175, 80) in user scale coordinates
            text55.Location.AlignH = AlignH.Center;
            text55.Location.AlignV = AlignV.Bottom;
            text55.FontSpec.Size = 7;
            text55.FontSpec.Fill = new Fill(Color.Green, Color.White, 45F);
            text55.FontSpec.StringAlignment = StringAlignment.Near;
            myPane.GraphObjList.Add(text55);


            //Liquido de titulos constante con X=40
            Double calidadliquidosaturado7 = 40;
            Double entropialiquido7 = 0;
            Double presionsaturacion7 = 0;
            for (int temperatura1 = 45; temperatura1 < 670; temperatura1 = temperatura1 + 15)
            {
                presionsaturacion7 = luis.psl(temperatura1, 1);
                entropialiquido7 = luis.shvptx(presionsaturacion7, temperatura1, calidadliquidosaturado7, 1);
                list8.Add(entropialiquido7, temperatura1);
            }

            Double presionsaturacion55 = luis.psl(100, 1);
            Double entropialiquido55 = luis.shvptx(presionsaturacion55, 100, calidadliquidosaturado7, 1);
            // Add a text item to decorate the graph
            TextObj text65 = new TextObj("X: " + Convert.ToString(calidadliquidosaturado7), (float)entropialiquido55, 100);
            // Align the text such that the Bottom-Center is at (175, 80) in user scale coordinates
            text65.Location.AlignH = AlignH.Center;
            text65.Location.AlignV = AlignV.Bottom;
            text65.FontSpec.Size = 7;
            text65.FontSpec.Fill = new Fill(Color.Green, Color.White, 45F);
            text65.FontSpec.StringAlignment = StringAlignment.Near;
            myPane.GraphObjList.Add(text65);


            //LINEAS CON PRESION CONSTANTE P=CTE.
            //Presion P=20 psia
            Double presion = 2390;
            Double calidad = 0;
            Double temperatura2 = 0;

            for (Double entropia1 = 0; entropia1 < 1.5; entropia1 = entropia1 + 0.025)
            {
                calidad = luis.xpshv(presion, entropia1, 1);
                temperatura2 = luis.tpshvx(presion, entropia1, calidad, 1, 1);
                list1.Add(entropia1, temperatura2);
            }

            Double temperaturasaturacion33 = luis.tsl(presion);
            Double entropia33 = luis.shvptx(presion, temperaturasaturacion33, 100, 1);
            temperatura2 = luis.tpshvx(presion, entropia33, 100, 1, 1);
            // Add a text item to decorate the graph
            TextObj text = new TextObj("P: " + Convert.ToString(presion) + " psia", (float)entropia33, (float)temperatura2);
            // Align the text such that the Bottom-Center is at (175, 80) in user scale coordinates
            text.Location.AlignH = AlignH.Center;
            text.Location.AlignV = AlignV.Bottom;
            text.FontSpec.Size = 7;
            text.FontSpec.Fill = new Fill(Color.Red, Color.White, 45F);
            text.FontSpec.StringAlignment = StringAlignment.Near;
            myPane.GraphObjList.Add(text);


            //Presion P=80 psia
            Double presion1 = Convert.ToDouble(textBox2.Text);
            Double calidad1 = 0;
            Double temperatura3 = 0;

            for (Double entropia1 = 0; entropia1 < 2; entropia1 = entropia1 + 0.025)
            {
                calidad1 = luis.xpshv(presion1, entropia1, 1);
                temperatura3 = luis.tpshvx(presion1, entropia1, calidad1, 1, 1);
                list2.Add(entropia1, temperatura3);
            }

            Double temperaturasaturacion34 = luis.tsl(presion1);
            Double entropia34 = luis.shvptx(presion1, temperaturasaturacion34, 100, 1);
            temperatura3 = luis.tpshvx(presion1, entropia34, 100, 1, 1);
            // Add a text item to decorate the graph
            TextObj text34 = new TextObj("P: " + Convert.ToString(presion1) + " psia", (float)entropia34, (float)temperatura3);
            // Align the text such that the Bottom-Center is at (175, 80) in user scale coordinates
            text34.Location.AlignH = AlignH.Center;
            text34.Location.AlignV = AlignV.Bottom;
            text34.FontSpec.Size = 7;
            text34.FontSpec.Fill = new Fill(Color.Red, Color.White, 45F);
            text34.FontSpec.StringAlignment = StringAlignment.Near;
            myPane.GraphObjList.Add(text34);


            //Presion P=320 psia
            Double presion2 = 320;
            Double calidad2 = 0;
            Double temperatura4 = 0;

            for (Double entropia1 = 0; entropia1 < 1.75; entropia1 = entropia1 + 0.025)
            {
                calidad2 = luis.xpshv(presion2, entropia1, 1);
                temperatura4 = luis.tpshvx(presion2, entropia1, calidad2, 1, 1);
                list3.Add(entropia1, temperatura4);
            }

            Double temperaturasaturacion35 = luis.tsl(presion2);
            Double entropia35 = luis.shvptx(presion2, temperaturasaturacion35, 100, 1);
            temperatura4 = luis.tpshvx(presion2, entropia35, 100, 1, 1);
            // Add a text item to decorate the graph
            TextObj text35 = new TextObj("P: " + Convert.ToString(presion2) + " psia", (float)entropia35, (float)temperatura4);
            // Align the text such that the Bottom-Center is at (175, 80) in user scale coordinates
            text35.Location.AlignH = AlignH.Center;
            text35.Location.AlignV = AlignV.Bottom;
            text35.FontSpec.Size = 7;
            text35.FontSpec.Fill = new Fill(Color.Red, Color.White, 45F);
            text35.FontSpec.StringAlignment = StringAlignment.Near;
            myPane.GraphObjList.Add(text35);


            //LINEAS CON PRESION CONSTANTE H=CTE.
            //Presion H=1000 BTU/LB
            Double entalpiaa = Convert.ToDouble(textBox3.Text);
            Double calidad22 = 0;
            Double temperatura433 = 0;
            Double presion88 = 0;
            Double entropiaa = 0;

            for (temperatura433 = 45; temperatura433 < 660; temperatura433= temperatura433 + 15)
            {
                calidad22 = luis.xtshv(temperatura433, entalpiaa, 2);
                presion88 = luis.tpshvx(temperatura433, entalpiaa, calidad22, 2, 2);
                entropiaa = luis.shvptx(presion88, temperatura433, calidad22, 1);
                list9.Add(entropiaa, temperatura433);
            }

            Double calidad555 = luis.xtshv(150, entalpiaa, 2);
            Double presion555 = luis.tpshvx(150, entalpiaa, calidad555, 2, 2);
            Double entropia555 = luis.shvptx(presion555, 150, calidad555, 1);
            // Add a text item to decorate the graph
            TextObj text351 = new TextObj("H: " + Convert.ToString(entalpiaa) + " BTU/Lb", (float)entropia555, (float)150);
            // Align the text such that the Bottom-Center is at (175, 80) in user scale coordinates
            text351.Location.AlignH = AlignH.Center;
            text351.Location.AlignV = AlignV.Bottom;
            text351.FontSpec.Size = 7;
            text351.FontSpec.Fill = new Fill(Color.BlueViolet, Color.White, 45F);
            text351.FontSpec.StringAlignment = StringAlignment.Near;
            myPane.GraphObjList.Add(text351);




            // Generate a blue curve with circle symbols, and "My Curve 2" in the legend
            LineItem myCurve99 = myPane.AddCurve("Diagrama de Mollier", list9, Color.BlueViolet,
                                    SymbolType.Circle);
            // Fill the area under the curve with a white-red gradient at 45 degrees
            //myCurve.Line.Fill = new Fill( Color.White, Color.Red, 45F );
            // Make the symbols opaque by filling them with white
            myCurve99.Symbol.Fill = new Fill(Color.BlueViolet);


            // Generate a blue curve with circle symbols, and "My Curve 2" in the legend
            LineItem myCurve = myPane.AddCurve("Diagrama de Mollier", list, Color.Black,
                                    SymbolType.Circle);
            // Fill the area under the curve with a white-red gradient at 45 degrees
            //myCurve.Line.Fill = new Fill( Color.White, Color.Red, 45F );
            // Make the symbols opaque by filling them with white
            myCurve.Symbol.Fill = new Fill(Color.Black);

            // Generate a blue curve with circle symbols, and "My Curve 2" in the legend
            LineItem myCurve4 = myPane.AddCurve("Diagrama de Mollier", list4, Color.Black,
                                    SymbolType.Circle);
            // Fill the area under the curve with a white-red gradient at 45 degrees
            //myCurve.Line.Fill = new Fill( Color.White, Color.Red, 45F );
            // Make the symbols opaque by filling them with white
            myCurve4.Symbol.Fill = new Fill(Color.Black);

            // Generate a blue curve with circle symbols, and "My Curve 2" in the legend
            LineItem myCurve5 = myPane.AddCurve("Diagrama de Mollier", list5, Color.Green,
                                    SymbolType.Circle);
            // Fill the area under the curve with a white-red gradient at 45 degrees
            //myCurve.Line.Fill = new Fill( Color.White, Color.Red, 45F );
            // Make the symbols opaque by filling them with white
            myCurve5.Symbol.Fill = new Fill(Color.Green);

            // Generate a blue curve with circle symbols, and "My Curve 2" in the legend
            LineItem myCurve6 = myPane.AddCurve("Diagrama de Mollier", list6, Color.Green,
                                    SymbolType.Circle);
            // Fill the area under the curve with a white-red gradient at 45 degrees
            //myCurve.Line.Fill = new Fill( Color.White, Color.Red, 45F );
            // Make the symbols opaque by filling them with white
            myCurve6.Symbol.Fill = new Fill(Color.Green);

            // Generate a blue curve with circle symbols, and "My Curve 2" in the legend
            LineItem myCurve7 = myPane.AddCurve("Diagrama de Mollier", list7, Color.Green,
                                    SymbolType.Circle);
            // Fill the area under the curve with a white-red gradient at 45 degrees
            //myCurve.Line.Fill = new Fill( Color.White, Color.Red, 45F );
            // Make the symbols opaque by filling them with white
            myCurve7.Symbol.Fill = new Fill(Color.Green);

            // Generate a blue curve with circle symbols, and "My Curve 2" in the legend
            LineItem myCurve17 = myPane.AddCurve("Diagrama de Mollier", list8, Color.Green,
                                    SymbolType.Circle);
            // Fill the area under the curve with a white-red gradient at 45 degrees
            //myCurve.Line.Fill = new Fill( Color.White, Color.Red, 45F );
            // Make the symbols opaque by filling them with white
            myCurve17.Symbol.Fill = new Fill(Color.Green);

            // Generate a blue curve with circle symbols, and "My Curve 2" in the legend
            LineItem myCurve1 = myPane.AddCurve("Diagrama de Mollier", list1, Color.Red,
                                    SymbolType.Circle);

            // Fill the area under the curve with a white-red gradient at 45 degrees
            //myCurve.Line.Fill = new Fill( Color.White, Color.Red, 45F );
            // Make the symbols opaque by filling them with white
            myCurve1.Symbol.Fill = new Fill(Color.Red);

            // Generate a blue curve with circle symbols, and "My Curve 2" in the legend
            LineItem myCurve2 = myPane.AddCurve("Diagrama de Mollier", list2, Color.Red,
                                    SymbolType.Circle);

            // Fill the area under the curve with a white-red gradient at 45 degrees
            //myCurve.Line.Fill = new Fill( Color.White, Color.Red, 45F );
            // Make the symbols opaque by filling them with white
            myCurve2.Symbol.Fill = new Fill(Color.Red);

            // Generate a blue curve with circle symbols, and "My Curve 2" in the legend
            LineItem myCurve3 = myPane.AddCurve("Diagrama de Mollier", list3, Color.Red,
                                    SymbolType.Circle);

            // Fill the area under the curve with a white-red gradient at 45 degrees
            //myCurve.Line.Fill = new Fill( Color.White, Color.Red, 45F );
            // Make the symbols opaque by filling them with white
            myCurve3.Symbol.Fill = new Fill(Color.Red);


            // Fill the axis background with a color gradient
            myPane.Chart.Fill = new Fill(Color.White, Color.LightGoldenrodYellow, 45F);

            // Fill the pane background with a color gradient
            myPane.Fill = new Fill(Color.White, Color.FromArgb(220, 220, 255), 45F);

            // Add an arrow pointer for the above text item
            //ArrowObj arrow = new ArrowObj(Color.Black, 1F, 1F, 10F, 10F, 10F);
            //arrow.Location.CoordinateFrame = CoordType.AxisXYScale;
            //myPane.GraphObjList.Add(arrow);

            // Calculate the Axis Scale Ranges
            zg1.AxisChange();

            zg1.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top);
        }
    }
}
