using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NumericalMethods;
using NumericalMethods.FourthBlog;

using ClaseEquipos;

namespace Drag_AND_Drop_between_Forms
{
    public partial class Condensadorprincipal : Form
    {
        public Double D1, D2, D3,D4,D5,D6,D7,D8,D9;
        public Double correntrada1,correntrada2, corrsalida;
        public Double numequipo;

        Double numecuaciones2;
        Double numvariables2;

        //Lista de cadenas para gardar los nombres de las variables del sistema de ecuaciones
        List<String> variables1 = new List<String>();

        //Lista de cadenas que guardan las ecuaciones del sistema
        List<String> ecuaciones1 = new List<String>();

        Aplicacion punteroaplicacion1;

        Double numparametroscreados = 0;

        int auxiliar = 0;

        public Condensadorprincipal(Aplicacion punteroaplicion,Double numecuaciones1,Double numvariables1)
        {
            InitializeComponent();           

            punteroaplicacion1 = punteroaplicion;

            //Inicializamos las etiquetas de las unidades de entrada en el cuadro de dialogo de toma de datos
            if (punteroaplicacion1.unidades == 0)
            {

            }
            else if (punteroaplicacion1.unidades == 1)
            {
                label18.Text = "kPa";
                label19.Text = "Kj/Hr";
            }
            else if (punteroaplicacion1.unidades == 2)
            {
                label18.Text = "Bar";
                label19.Text = "Kcal/Hr";
            }
            else
            {

            }

            D1 = 0;
            D2 = 0;
            D3 = 0;
            D4 = 0;
            D5 = 0;
            D6 = 0;
            D7 = 0;
            D8 = 0;
            D9 = 0;

            correntrada1 = 0;
            correntrada2 = 0;
            corrsalida = 0;

            numequipo = 0;

            numecuaciones2 = numecuaciones1;

            numvariables2 = numvariables1;
        }


        //Boton Generar Ecuaciones
        private void button1_Click(object sender, EventArgs e)
        {
            D1=Convert.ToDouble(textBox1.Text);
            D2=Convert.ToDouble(textBox2.Text);
            D3=Convert.ToDouble(textBox3.Text);
            D4 =Convert.ToDouble(textBox4.Text);
            D5=Convert.ToDouble(textBox13.Text);
            D6=Convert.ToDouble(textBox5.Text);
            D7=Convert.ToDouble(textBox6.Text);
            D6 =Convert.ToDouble(textBox12.Text);
            D7 =Convert.ToDouble(textBox11.Text);

            correntrada1=Convert.ToDouble(textBox7.Text);
            correntrada2 = Convert.ToDouble(textBox10.Text);
            corrsalida = Convert.ToDouble(textBox8.Text);

            numequipo = Convert.ToDouble(textBox9.Text);

            funcionauxiliar();
        }

        public void funcionauxiliar()
        {

            //Conversión UNIDADES
            //Como las Tablas de Vapor de Agua ASME 1967 están en unidades británicas, siempre tenemos que convertir los datos de entrada a Unidades Británicas

            //Unidades Sistema Internacional
            if (punteroaplicacion1.unidades == 2)
            {
                //Presión Bar a psia
                D1 = D1 / (6.8947572 / 100);

                //PENDIENTE DEFINIR EL CAMBIO DE UNIDADES DE D2, D3 Y D4
            }

            //Unidades Sistema Métrico
            else if (punteroaplicacion1.unidades == 1)
            {
                //Presión kPa a psia
                D1 = D1 / (6.894757);

                //PENDIENTE DEFINIR EL CAMBIO DE UNIDADES DE D2, D3 Y D4
            }

            //Unidades Sistema Británico
            else if (punteroaplicacion1.unidades == 0)
            {

            }
            else
            {

            }

            ecuaciones1=generaecucaiones(D1,D2,D3,D4,D5,D6,D7,D8,D9,correntrada1,correntrada2,corrsalida);

            listBox1.Items.Add("Nº Equipo:" + Convert.ToString(numequipo));

            for (int numecua = 0; numecua < 4; numecua++)
            {
                listBox1.Items.Add(ecuaciones1[numecua]);
            }

            listBox1.Items.Add("");

        }

        private List<String> generaecucaiones(Double D1, Double D2,Double D3,Double D4, Double D5, Double D6, Double D7,Double D8,Double D9, Double correntrada1,Double correntrada2, Double corrsalida)
        {
            //Lista de cadenas que guardan las ecuaciones del sistema
            List<String> ecuaciones2 = new List<String>();
           
            //Cálculo tradicional del Condensador fijando su presión de vacio. Pv= D1 + D2xQ + D3xQXQ
            if (D9==0)
            {
                textBox10.Enabled = true;

                ecuaciones2.Add("");
                ecuaciones2[auxiliar] = "W" + Convert.ToString(correntrada1) + "+" + "W" + Convert.ToString(correntrada2) + "-" + "W" + Convert.ToString(corrsalida);
                auxiliar++;

                ecuaciones2.Add("");
                ecuaciones2[auxiliar] = "P" + Convert.ToString(correntrada1) + "-" + "P" + Convert.ToString(corrsalida);
                auxiliar++;

                String Q1;
                Q1 = "(" + "W" + Convert.ToString(correntrada1) + "*" + "H" + Convert.ToString(correntrada1) + ")" + "+" + "(" + "W" + Convert.ToString(correntrada2) + "*" + "H" + Convert.ToString(correntrada2) + ")" + "-" + "(" + "W" + Convert.ToString(corrsalida) + "*" + "H" + Convert.ToString(corrsalida) + ")";

                String tempPv;
                tempPv = Convert.ToString(D1) + "+" + "(" + Convert.ToString(D2) + "*" + Q1 + ")" + "+" + "(" + Convert.ToString(D3) + "*" + "(" + Q1 + ")" + "*" + "(" + Q1 + ")" + ")";

                if ((D1 > 0)||(D2 > 0)||(D3 > 0)||(D4 > 0))
                {
                    ecuaciones2.Add("");
                    ecuaciones2[auxiliar] = "P" + Convert.ToString(correntrada1) + "-" + "(" + tempPv + ")";
                    auxiliar++;
                }

                ecuaciones2.Add("");
                ecuaciones2[auxiliar] = "H" + Convert.ToString(corrsalida) + "-" + "hsatp(P" + Convert.ToString(correntrada1) + ")";
                auxiliar++;
            }

            //Cálculo del Condensador de acuerdo a la HEI. 
            else if (D9>0)
            {
                //PENDIENTE IMPLEMENTAR LAS ECUACIONES

            }

            numecuaciones2 = auxiliar;
            numvariables2 = 3;
           
            return (ecuaciones2);
        }


        //Botón OK
        private void button2_Click(object sender, EventArgs e)
        {
            funcionauxiliar1();  
     
            this.Hide();
        }

        public void funcionauxiliar1()
        {
            int j;
            j = 0;

            for (int i = (int)punteroaplicacion1.numecuaciones; i < numecuaciones2 + (int)punteroaplicacion1.numecuaciones; i++)
            {
                punteroaplicacion1.ecuaciones.Add("");
                punteroaplicacion1.ecuaciones[i] = ecuaciones1[j];
                j++;
            }

            punteroaplicacion1.numecuaciones = numecuaciones2 + punteroaplicacion1.numecuaciones;
            punteroaplicacion1.numvariables = numvariables2 + punteroaplicacion1.numvariables;

            //Declaramos y creamos un objeto de la CLASE EQUIPOS para guardar los datos de entrada del usuario del nuevo equipo "condición de contorno" generado.
            Equipos condensador8 = new Equipos();
            condensador8.Inicializar(numequipo, 8, correntrada1, correntrada2, corrsalida, 0, D1, D2, D3, 0, 0, 0, 0, 0, 0,0,0,0,0);
            //Copio los valores de la clase de entrada de datos en la aplicación principal
            punteroaplicacion1.equipos11.Add(condensador8);

            //Incrementamos el Número Total de Equipos y Número Total de Corrientes en la aplicación principal
            punteroaplicacion1.NumTotalEquipos++;
            punteroaplicacion1.NumTotalCorrientes = punteroaplicacion1.NumTotalCorrientes + 1;   
        }

        //Botón Cancel
        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
