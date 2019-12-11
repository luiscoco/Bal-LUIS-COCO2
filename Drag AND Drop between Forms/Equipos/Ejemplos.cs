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
    public partial class Ejemplos : Form
    {
        public int numparnuevos = 0;

        //El parámetro auxiliar nos indica si se trata de un equipos tipo condición de contorno que genera 2 o 1 corriente
        //Si parametroauxiliar=1 solo genera una corriente la de salida y si parametroauxiliar=0 genera dos corrientes la de entrada y la de salida
        
        public Double correntrada, corrsalida, parametroauxiliar;
        public Double numequipo;

        public Double numecuaciones2;
        public Double numvariables2;

        //Lista de cadenas para gardar los nombres de las variables del sistema de ecuaciones
        List<String> variables1 = new List<String>();

        //Lista de cadenas String que guardan las ecuaciones del sistema
        List<String> ecuaciones1 = new List<String>();

        Aplicacion punteroaplicacion1;

        public Double numparametroscreados=0;

        int auxiliar = 0;

        public Ejemplos(Aplicacion punteroaplicion,Double numecuaciones1,Double numvariables1)
        {
            InitializeComponent();
            
            punteroaplicacion1 = punteroaplicion;

            numparametroscreados= (punteroaplicacion1.numcorrientes)*3;           
            
            correntrada = 0;
            corrsalida = 0;
            parametroauxiliar = 0;

            numequipo = 0;

            numecuaciones2 = numecuaciones1;

            numvariables2 = numvariables1;
        }


        //Boton Generar Ecuaciones
        private void button1_Click(object sender, EventArgs e)
        {
            //Llamamos a la opción del Menú de Nuevo Cálculo
            punteroaplicacion1.toolStripMenuItem11_Click(sender, e);

            correntrada = Convert.ToDouble(textBox7.Text);
            corrsalida = Convert.ToDouble(textBox8.Text);

            funcionauxiliar();
        }
        
        public void funcionauxiliar()
        {
                        
            Random random = new Random();

            //CREAMOS EL ARRAY DE PARAMETROS
            for (int v = 0; v < 2; v++)
            {
                //int randomNumber = random.Next(0, 2500);
                //Creamos la lista de parámetros generadas por este programa
                punteroaplicacion1.p.Add(punteroaplicacion1.ptemp);
                punteroaplicacion1.p[v] = new Parameter(1, 0.01, "");
            }
           
            //Asignamos las Condiciones Iniciales a las dos variables, en este caso W1=1 y W2=2
                punteroaplicacion1.p[0].Nombre = "W" + Convert.ToString(correntrada);
                punteroaplicacion1.p[0].Value = 1;
                punteroaplicacion1.p[1].Nombre = "W" + Convert.ToString(corrsalida);
                punteroaplicacion1.p[1].Value = 2;
                
            ecuaciones1=generaecucaiones(correntrada,corrsalida);

            listBox1.Items.Add("Nº Equipo:" + Convert.ToString(numequipo));

            for (int numecua = 0; numecua < auxiliar; numecua++)
            {
                listBox1.Items.Add(ecuaciones1[numecua]);
            }

            //listBox1.Items.Add("");

            button2.Enabled = true;
            button1.Enabled = false;
        }


        private List<String> generaecucaiones(Double correntrada, Double corrsalida)
        {
            //Lista de cadenas que guardan las ecuaciones del sistema
            List<String> ecuaciones2 = new List<String>();

            Parameter W1 = new Parameter();
            Parameter W2 = new Parameter();
            Parameter P1 = new Parameter();
            Parameter P2 = new Parameter();
            Parameter H1 = new Parameter();
            Parameter H2 = new Parameter();

            W1 = punteroaplicacion1.p.Find(p => p.Nombre == "W" + Convert.ToString(correntrada));
            W2 = punteroaplicacion1.p.Find(p => p.Nombre == "W" + Convert.ToString(corrsalida));
              
                    ecuaciones2.Add("");
                    ecuaciones2[auxiliar] = "W" + Convert.ToString(correntrada) + "+" + "2*W" + Convert.ToString(corrsalida) + "-2";
                    Func<Double> primeraecuacion = () => W1+2*W2-2;
                    punteroaplicacion1.functions.Add(primeraecuacion);
                    auxiliar++;

                    ecuaciones2.Add("");
                    ecuaciones2[auxiliar] = "(" + "W" + Convert.ToString(correntrada) + "*" + "W" + Convert.ToString(correntrada) + ")" + "+" + "(" + "4" + "*" + "W" + Convert.ToString(corrsalida) + "*" + "W" + Convert.ToString(corrsalida)+")"+"-"+"4";
                    Func<Double> segundaecuacion = () => (W1*W1)+(4*W2*W2)-4;
                    punteroaplicacion1.functions.Add(segundaecuacion);
                    auxiliar++;

                numecuaciones2 = auxiliar;
                numvariables2 = 2;

                punteroaplicacion1.ejemplovalidacion = 1;
           
            return (ecuaciones2);        
        }


        //Botón OK
        //Condcontorno este botón enviamos tanto las ecuaciones, los nombres de las variables y las lineas de código de Hbal a la aplicación general
        public void button2_Click(object sender, EventArgs e)
        {

            funcionauxiliar1();
           // punteroaplicacion1.matrizauxjacob.
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }


        //Función para enviar a la Aplicación Principal las ecuaciones y las variables creadas
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

            //Enviamos a la aplicación principal la ecuaciones generadas (array de cadenas string)
            punteroaplicacion1.numecuaciones = numecuaciones2+punteroaplicacion1.numecuaciones;

            //Enviamos a la aplicación principal los nombres de las variables que integran las ecuaciones generadas
            punteroaplicacion1.numvariables = numvariables2 + punteroaplicacion1.numvariables;

        }


        //Botón Cancel
        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
