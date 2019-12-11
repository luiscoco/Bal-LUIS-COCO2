using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ZedGraph;

//Espacio de trabajo de la Aplicación Principal
using Drag_AND_Drop_between_Forms;


namespace ZedGraphSample
{
	public partial class Tablaluis: Form
	{
        public Double numeroTabla = 0;
        public Int16 numerofilas = 1;
        public Int16 numerocolumnas = 2;
        Double[,] b  = new Double[1,1];
        Double[,] temp = new Double[1, 1];
        List<Double[,]> listaTablas=new List<Double[,]>();
        Aplicacion puntero1;

		public Tablaluis(Aplicacion puntero)
		{
            puntero1 = puntero;
			InitializeComponent();
		}

		private void Form1_Load( object sender, EventArgs e )
		{
            textBox1.Text = Convert.ToString(numeroTabla);
            
		}

		private void Form1_Resize( object sender, EventArgs e )
		{
			//SetSize();
		}

		private void SetSize()
		{
			//zg1.Location = new Point( 10, 10 );
			// Leave a small margin around the outside of the control
			//zg1.Size = new Size( this.ClientRectangle.Width - 40, this.ClientRectangle.Height - 40 );
		}
        
        //BOTÓN Guardar Tabla
        private void button1_Click(object sender, EventArgs e)
        {

            numerofilas=Convert.ToInt16 (textBox2.Text);
            numerocolumnas=2;

            Int16 numfilas = Convert.ToInt16(dataGridView1.RowCount);
            Int16 numcolumnas = Convert.ToInt16(dataGridView1.ColumnCount);
           
            if (numfilas > numerofilas)
            {
                numerofilas = numfilas;
            }

            Double[,] a = new Double[numerofilas, numerocolumnas];

            for (int i = 0; i <numfilas; i++)
            {
                for (int j = 0; j <numcolumnas; j++)
                {
                    a[i,j] = Convert.ToDouble(dataGridView1.Rows[i].Cells[j].Value);
                }

            }

            b = a;
            listaTablas.Add(b);
            button1.Enabled=false;
            button2.Enabled = true;
            button4.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        //BOTON de Graficar Tabla
        private void button2_Click_1(object sender, EventArgs e)
        {
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
            this.zg1.Size = new System.Drawing.Size(539, 336);
            this.zg1.TabIndex = 0;
            // 
            // xval
            // 
            this.xval.HeaderText = "Valores de X:";
            this.xval.Name = "xval";
            // 
            // yval
            // 
            this.yval.HeaderText = "Valores de Y:";
            this.yval.Name = "yval";
            // 
            GraphPane myPane = zg1.GraphPane;

            this.Controls.Add(this.zg1);

            // Set the titles and axis labels
            myPane.Title.Text = "Gráfico de la Tabla definida por el Usuario";
            myPane.XAxis.Title.Text = "Valores de X";
            myPane.YAxis.Title.Text = "Valores de Y";

            // Make up some data points from the Sine function
            PointPairList list = new PointPairList();

            for (int i = 0; i < numerofilas - 1; i++)
            {
                double z = b[i, 1];
                double x = b[i, 0];
                list.Add(x, z);
            }

            // Generate a blue curve with circle symbols, and "My Curve 2" in the legend
            LineItem myCurve = myPane.AddCurve("Curva LUIS COCO1", list, Color.Blue,
                                    SymbolType.Circle);

            // Fill the area under the curve with a white-red gradient at 45 degrees
            //myCurve.Line.Fill = new Fill( Color.White, Color.Red, 45F );
            // Make the symbols opaque by filling them with white
            myCurve.Symbol.Fill = new Fill(Color.Green);


            // Fill the axis background with a color gradient
            myPane.Chart.Fill = new Fill(Color.White, Color.LightGoldenrodYellow, 45F);

            // Fill the pane background with a color gradient
            myPane.Fill = new Fill(Color.White, Color.FromArgb(220, 220, 255), 45F);

            // Calculate the Axis Scale Ranges
            zg1.AxisChange();

            //SetSize(); 

            button2.Enabled = false;
            button3.Enabled = true;
        }


        //BOTON de Nueva Tabla
        private void button3_Click(object sender, EventArgs e)
        {
            zg1.Dispose();
            numeroTabla++;
            textBox1.Text = Convert.ToString(numeroTabla);
            textBox2.Text = Convert.ToString(4);
            button1.Enabled = true;
            zg1.Dispose();
            button3.Enabled = false;
            button1.Enabled = true;
            button4.Enabled = true;
        }

        
        //BOTON de Ver Tabla
        private void button4_Click(object sender, EventArgs e)
        {
            zg1.Dispose();
            Int16 ntabla = 0;
            ntabla=Convert.ToInt16(textBox3.Text);
            Double[,] c = listaTablas[ntabla];
            //Número Filas
            int luis=c.GetLength(0);
            //Número Columnas
            int mariluz = c.GetLength(1);

            for (int i = 0; i < luis; i++)
            {
                for (int j = 0; j < mariluz; j++)
                {
                    dataGridView1.Rows[i].Cells[j].Value=c[i, j];
                }
            }

            b = c;

            this.button2_Click_1(sender,e);
            button3.Enabled = false;

        }

        private void button5_Click(object sender, EventArgs e)
        {
            puntero1.listaTablas = listaTablas;
            this.Hide();
        }
	}
}
