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
        public Double numeroValores = 0;
        public int numerofilas = 1;
        public int numerocolumnas = 2;
        Double[,] b  = new Double[1,1];
        Double[,] temp = new Double[1, 1];
        List<Double[,]> listaTablas=new List<Double[,]>();
        Aplicacion puntero1;
        Double ntabla = 0;

		public Tablaluis(Aplicacion puntero)
		{
            puntero1 = puntero;
			InitializeComponent();
		}

		private void Form1_Load( object sender, EventArgs e )
		{
            numeroTabla = puntero1.NumTotalTablas;

            if (numeroTabla > 0)
            {
                button4.Enabled = true;  
            }

            //Cargar los Número de las Tablas leeidas de un archivo de HBAL por el usuario
            comboBox1.Items.Clear();

            for (int gg = 0; gg < puntero1.listanumTablas.Count; gg++)
            {
                comboBox1.Items.Add(Convert.ToString(puntero1.listanumTablas[gg]));
            }

            //Cargar los Número de las Tablas leeidas de un archivo de HBAL por el usuario
            comboBox3.Items.Clear();

            for (int n = 0; n < puntero1.listanumTablas.Count; n++)
            {
                comboBox3.Items.Add(Convert.ToString(puntero1.listanumTablas[n]));
            }
            
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
        
    
        //Graficar la TABLA
        void graficartabla(Double numTabla)
        {
            int indicetabla = 0;
            int marca = 0;
            for (int j = 0; j < (int)puntero1.listanumTablas.Count; j++)
            {
                if (puntero1.listanumTablas[j] == numTabla)
                {
                    indicetabla = j;
                    marca = 1;
                    b = puntero1.listaTablas[indicetabla];
                    goto marias;
                }
            }

            if (marca == 0)
            {
                this.ClientSize = new System.Drawing.Size(700, 727);
                MessageBox.Show("La Tabla indicada número: " + Convert.ToString(ntabla) + " no se ha encontrada en las Tablas guardadas por el usuario");
                return;
            }

            marias:

            this.ClientSize = new System.Drawing.Size(1120, 641);

            if (zg1 != null)
            {
                zg1.Dispose();
            }     

            this.zg1 = new ZedGraph.ZedGraphControl();
            // zg1
            // 
            this.zg1.EditButtons = System.Windows.Forms.MouseButtons.Left;
            this.zg1.Location = new System.Drawing.Point(640, 36);
            this.zg1.Name = "zg1";
            this.zg1.PanModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)));
            this.zg1.ScrollGrace = 0D;
            this.zg1.ScrollMaxX = 0D;
            this.zg1.ScrollMaxY = 0D;
            this.zg1.ScrollMaxY2 = 0D;
            this.zg1.ScrollMinX = 0D;
            this.zg1.ScrollMinY = 0D;
            this.zg1.ScrollMinY2 = 0D;
            this.zg1.Size = new System.Drawing.Size(550, 399);
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
            myPane.Title.Text = puntero1.listaTituloTabla[indicetabla];
            myPane.XAxis.Title.Text = puntero1.listaTituloEjeXTabla[indicetabla];
            myPane.YAxis.Title.Text = puntero1.listaTituloEjeYTabla[indicetabla];

            // Make up some data points from the Sine function
            PointPairList list = new PointPairList();

            for (int i = 0; i < numerofilas - 1; i++)
            {
                double z = b[i, 1];
                double x = b[i, 0];
                list.Add(x, z);
            }

            // Generate a blue curve with circle symbols, and "My Curve 2" in the legend
            LineItem myCurve = myPane.AddCurve(puntero1.listaTituloTabla[indicetabla], list, Color.Blue,
                                    SymbolType.Circle);

            // Fill the area under the curve with a white-red gradient at 45 degrees
            //myCurve.Line.Fill = new Fill( Color.White, Color.Red, 45F );
            // Make the symbols opaque by filling them with white
            myCurve.Symbol.Fill = new Fill(Color.Green);

            // Fill the axis background with a color gradient
            myPane.Chart.Fill = new Fill(Color.White, Color.White, 45F);

            // Fill the pane background with a color gradient
            myPane.Fill = new Fill(Color.White, Color.FromArgb(220, 220, 255), 45F);

            // Calculate the Axis Scale Ranges
            zg1.AxisChange();

            //SetSize(); 

         
            button3.Enabled = true;
        }
      
      
        //Botón de OK
        private void button5_Click(object sender, EventArgs e)
        {
            puntero1.listaTablas = listaTablas;
            this.Hide();
        }

        //Ver Tabla en el TAB2
        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView2.Enabled = true;
            comboBox3.Items.Clear();

            for (int n = 0; n < puntero1.listanumTablas.Count; n++)
            {
                comboBox3.Items.Add(Convert.ToString(puntero1.listanumTablas[n]));
            }

            int indicetabla = 0;
            int marca = 0;
            //Buscamos el índice de la tabla que cumple con la condición de que su número de tabla coincide con el guardado en la lista de número de Tablas
            //La variable ntabla es actualizada con el Evento de cambio de selección de los elementos del ComboBox1 (ver función del evento más abajo en este fichero de código)
            for (int j = 0; j < (int)puntero1.listanumTablas.Count; j++)
            {
                if (puntero1.listanumTablas[j] == ntabla)
                {
                    indicetabla = j;
                    marca = 1;
                    goto marias;
                }
            }

            if (marca == 0)
            {
                this.ClientSize = new System.Drawing.Size(316, 641);
                MessageBox.Show("La Tabla indicada número: " + Convert.ToString(ntabla) + " no se ha encontrada en las Tablas guardadas por el usuario");
                return;
            }

        marias:
            int filas = puntero1.listaTablas[indicetabla].GetLength(0);
            int columnas = puntero1.listaTablas[indicetabla].GetLength(1);
            Double[,] c = new Double[filas, columnas];
            //  W1 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N1));
            c = puntero1.listaTablas[indicetabla];
            //Título de Tabla
            textBox8.Text = puntero1.listaTituloTabla[indicetabla];
            //Título Eje X de Tabla
            textBox7.Text = puntero1.listaTituloEjeXTabla[indicetabla];
            //Título Eje X de Tabla
            textBox6.Text = puntero1.listaTituloEjeYTabla[indicetabla];
            
            //Número Filas
            int luis = c.GetLength(0);
            numerofilas = luis;
            //Número Columnas
            int mariluz = c.GetLength(1);
            numerocolumnas = mariluz;

            //Número Filas
            for (int i = 0; i < luis; i++)
            {
                //Número Columnas
                for (int j = 0; j < mariluz; j++)
                {
                    dataGridView2.Rows.Add();
                    dataGridView2.Rows[i].Cells[j].Value = c[i, j];
                }
            }
        }


        //Ver Tabla en TAB1
        private void button4_Click_1(object sender, EventArgs e)
        {
            if (zg1 != null)
            {
                zg1.Dispose();
            }

            comboBox1.Items.Clear();
            for (int n = 0; n < puntero1.listanumTablas.Count; n++)
            {
                comboBox1.Items.Add(Convert.ToString(puntero1.listanumTablas[n]));
            }

            int indicetabla = 0;
            int marca = 0;
            //Buscamos el índice de la tabla que cumple con la condición de que su número de tabla coincide con el guardado en la lista de número de Tablas
            //La variable ntabla es actualizada con el Evento de cambio de selección de los elementos del ComboBox1 (ver función del evento más abajo en este fichero de código)
            for (int j = 0; j < (int)puntero1.listanumTablas.Count; j++)
            {
                if (puntero1.listanumTablas[j] == ntabla)
                {
                    indicetabla = j;
                    marca = 1;
                    goto marias;
                }
            }

            if (marca == 0)
            {
                this.ClientSize = new System.Drawing.Size(316, 641);
                MessageBox.Show("La Tabla indicada número: " + Convert.ToString(ntabla) + " no se ha encontrada en las Tablas guardadas por el usuario");
                return;
            }

        marias:
            int filas = puntero1.listaTablas[indicetabla].GetLength(0);
            int columnas = puntero1.listaTablas[indicetabla].GetLength(1);
            Double[,] c = new Double[filas, columnas];
            //  W1 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N1));
            c = puntero1.listaTablas[indicetabla];
            //Título de Tabla
            textBox3.Text = puntero1.listaTituloTabla[indicetabla];
            //Título Eje X de Tabla
            textBox4.Text = puntero1.listaTituloEjeXTabla[indicetabla];
            //Título Eje X de Tabla
            textBox5.Text = puntero1.listaTituloEjeYTabla[indicetabla];
            //Tipo de interpolación
            if (puntero1.listanumTipoInterpolacionTabla[indicetabla] == 1)
            {
                comboBox2.SelectedItem = "Lineal";
            }

            else if (puntero1.listanumTipoInterpolacionTabla[indicetabla] == 2)
            {
                comboBox2.SelectedItem = "Cuadrática";
            }

            else if (puntero1.listanumTipoInterpolacionTabla[indicetabla] == 3)
            {
                comboBox2.SelectedItem = "Cúbica";
            }

            //Número Filas
            int luis = c.GetLength(0);
            numerofilas = luis;
            //Número Columnas
            int mariluz = c.GetLength(1);
            numerocolumnas = mariluz;

            //Número Filas
            for (int i = 0; i < luis; i++)
            {
                //Número Columnas
                for (int j = 0; j < mariluz; j++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[j].Value = c[i, j];
                }
            }

            b = c;

            graficartabla(ntabla);
            //Activamos el botón Nueva Tabla
            button3.Enabled = false;
        }

        //Guardar Tabla en TAB1
        private void button1_Click_1(object sender, EventArgs e)
        {
            numerofilas = Convert.ToInt16(textBox2.Text);
            numerocolumnas = 2;

            Int16 numfilas = Convert.ToInt16(dataGridView1.RowCount);
            Int16 numcolumnas = Convert.ToInt16(dataGridView1.ColumnCount);

            //Comprobamos si el número de valores de la tabla es menor que el número de valores que hemos definido en el textBox2
            if (numfilas > numerofilas)
            {
                numerofilas = numfilas;
            }

            Double[,] a = new Double[numerofilas-1, numerocolumnas];

            for (int i = 0; i < numfilas-1; i++)
            {
                for (int j = 0; j < numcolumnas; j++)
                {
                    a[i, j] = Convert.ToDouble(dataGridView1.Rows[i].Cells[j].Value);
                }
            }

            b = a;
            listaTablas.Add(b);
            puntero1.listaTablas.Add(b);
            Double numTablatemp = Convert.ToDouble(textBox1.Text);
            puntero1.listanumTablas.Add(numTablatemp);
            Double numdatosTabla = Convert.ToDouble(textBox2.Text);
            puntero1.listanumDatosenTabla.Add(numdatosTabla);
            String Titulo;
            Titulo = textBox3.Text;
            puntero1.listaTituloTabla.Add(Titulo);
            String TituloejeX;
            TituloejeX = textBox4.Text;
            puntero1.listaTituloEjeXTabla.Add(TituloejeX);
            String TituloejeY;
            TituloejeY = textBox5.Text;
            puntero1.listaTituloEjeYTabla.Add(TituloejeY);

            String tipointerpolacion;

            tipointerpolacion = comboBox2.SelectedItem.ToString();

            if (tipointerpolacion == "Lineal")
            {
                puntero1.listanumTipoInterpolacionTabla.Add(1);
            }

            else if (tipointerpolacion == "Cuadrática")
            {
                puntero1.listanumTipoInterpolacionTabla.Add(2);
            }

            else if (tipointerpolacion == "Cúbica")
            {
                puntero1.listanumTipoInterpolacionTabla.Add(3);
            }

            button1.Enabled = false;

            button4.Enabled = false;

            //Cargar los Número de las Tablas creadas por el usuario
            comboBox1.Items.Clear();

            for (int n = 0; n < puntero1.listanumTablas.Count; n++)
            {
                comboBox1.Items.Add(Convert.ToString(puntero1.listanumTablas[n]));
            }

            //Cargar los Número de las Tablas creadas por el usuario
            comboBox3.Items.Clear();

            for (int n = 0; n < puntero1.listanumTablas.Count; n++)
            {
                comboBox3.Items.Add(Convert.ToString(puntero1.listanumTablas[n]));
            }

            graficartabla(numTablatemp);
        }

        //Nueva Tabla en TAB1
        private void button3_Click_1(object sender, EventArgs e)
        {
            zg1.Dispose();
            numeroTabla++;
            puntero1.NumTotalTablas++;
            textBox1.Text = Convert.ToString(numeroTabla);
            textBox2.Text = Convert.ToString(4);
            button1.Enabled = true;
            if (zg1 != null)
            {
                zg1.Dispose();
            }
            button3.Enabled = false;
            button1.Enabled = true;
            button4.Enabled = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int a = comboBox1.SelectedIndex;
            ntabla = Convert.ToDouble(comboBox1.Items[a]);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            int a = comboBox3.SelectedIndex;
            ntabla = Convert.ToDouble(comboBox3.Items[a]);
            button2.Enabled = true;
        }

        //Selección de Páginas en el Control tipo TAB
        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (tabControl1.SelectedTab==tabPage1)
            {                
                if (zg1 != null)
                {
                    zg1.Dispose();
                }

                this.ClientSize = new System.Drawing.Size(300, 400);                     
            }

            else if (tabControl1.SelectedTab == tabPage2)
            {
                if (zg1 != null)
                {
                    zg1.Dispose();
                }

                this.ClientSize = new System.Drawing.Size(300, 400);
            }

            else if (tabControl1.SelectedTab == tabPage3)
            {
                
                if (zg1 != null)
                {
                    zg1.Dispose();
                }

                this.ClientSize = new System.Drawing.Size(300, 400);
            }
        }


        //Botón de INTERPOLAR
        private void button6_Click(object sender, EventArgs e)
        {
            puntero1.recibirpunterotabla(this);

            if (comboBox4.SelectedIndex==0)
            {
            //Tipo de Interpolación LINEAL
            textBox10.Text=Convert.ToString(puntero1.tabla(ntabla, Convert.ToDouble(textBox9.Text), 1));
            }

            else if (comboBox4.SelectedIndex==1)
            {
            //Tipo de Interpolación CUADRÁTICA
            textBox10.Text=Convert.ToString(puntero1.tabla(ntabla, Convert.ToDouble(textBox9.Text), 2));
            }

            else if (comboBox4.SelectedIndex == 2)
            {
                //Tipo de Interpolación CÚBICA
                textBox10.Text = Convert.ToString(puntero1.tabla(ntabla, Convert.ToDouble(textBox9.Text), 3));
            }

            // el comboBox4.SelectedIndex == 4 es para el separador de items entre minimos cuadrados y interpolación en la lista de tipos de interpolación
            else if (comboBox4.SelectedIndex == 4)
            {
                //Tipo de Interpolación por Mínimos Cuadrados según DotNumerics: polinomio cuadrático
                textBox10.Text = Convert.ToString(puntero1.tabla(ntabla, Convert.ToDouble(textBox9.Text), 4));
            }

            // el comboBox4.SelectedIndex == 5 es para el separador de items entre minimos cuadrados y interpolación en la lista de tipos de interpolación
            else if (comboBox4.SelectedIndex == 5)
            {
                //Tipo de Interpolación por Mínimos Cuadrados según DotNumerics: polinomio cúbico
                textBox10.Text = Convert.ToString(puntero1.tabla(ntabla, Convert.ToDouble(textBox9.Text), 5));
            }

            // el comboBox4.SelectedIndex == 6 es para el separador de items entre minimos cuadrados y interpolación en la lista de tipos de interpolación
            else if (comboBox4.SelectedIndex == 6)
            {
                //Tipo de Interpolación por Mínimos Cuadrados según DotNumerics: polinomio grado 4
                textBox10.Text = Convert.ToString(puntero1.tabla(ntabla, Convert.ToDouble(textBox9.Text), 6));
            }

            // el comboBox4.SelectedIndex == 7 es para el separador de items entre minimos cuadrados y interpolación en la lista de tipos de interpolación
            else if (comboBox4.SelectedIndex == 7)
            {
                //Tipo de Interpolación por Mínimos Cuadrados según DotNumerics: polinomio grado 5
                textBox10.Text = Convert.ToString(puntero1.tabla(ntabla, Convert.ToDouble(textBox9.Text), 7));
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            puntero1.punterodialogotabla1 = null;
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            puntero1.punterodialogotabla1 = null;
            this.Hide();
        }
	}
}
