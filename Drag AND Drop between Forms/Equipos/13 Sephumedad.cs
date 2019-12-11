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
    public partial class Sephumedad : Form
    {
        public Double D1, D2,entalpiasalida;
        public Double correntrada, corrsalida1,corrsalida2;
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

        //En el caso de que queramos EDITAR un equiop ya creado utilizamos estas dos variables para ayudarnos a buscar su indice en la lista de equipos11 de la aplicacion principal que contiene todos los equipos
        int indice1 = 0;
        public Double ediciononuevo1 = 0;

        public Sephumedad(Aplicacion punteroaplicion, Double numecuaciones1, Double numvariables1, Double ediciononuevo, int indice)
        {
            InitializeComponent();           

            punteroaplicacion1 = punteroaplicion;

            //Estos dos parámetros se utilizan para EDICION o NUEVO equipos (indice y edicionnuevo)
            indice1 = indice;
            ediciononuevo1 = ediciononuevo;
     
            D1 = 0;
            D2 = 0;
            entalpiasalida = 0;
                        
            correntrada = 0;
            corrsalida1 = 0;
            corrsalida2 = 0;

            numequipo = 0;

            numecuaciones2 = numecuaciones1;

            numvariables2 = numvariables1;        
        }

        private void Condcontorno_Load(object sender, EventArgs e)
        {

        }


        //BOTÓN Generar Ecuaciones
        private void button1_Click(object sender, EventArgs e)
        {
            D1 = Convert.ToDouble(textBox1.Text);
            D2 = Convert.ToDouble(textBox2.Text);
            entalpiasalida = Convert.ToDouble(textBox6.Text);

            correntrada = Convert.ToDouble(textBox3.Text);
            corrsalida1 = Convert.ToDouble(textBox4.Text);
            corrsalida2 = Convert.ToDouble(textBox5.Text);

            numequipo = Convert.ToDouble(textBox9.Text);

            funcionauxiliar();
        }

            public void funcionauxiliar()
            {

            ecuaciones1=generaecucaiones(D1,D2,correntrada,corrsalida1,corrsalida2,entalpiasalida);

            listBox1.Items.Add("Nº Equipo:" + Convert.ToString(numequipo));

            for (int numecua = 0; numecua < auxiliar; numecua++)
            {
                listBox1.Items.Add(ecuaciones1[numecua]);
            }

            listBox1.Items.Add("");

            button2.Enabled = true;

        }

        private List<String> generaecucaiones(Double D1, Double D2, Double correntrada, Double corrsalida1,Double corrsalida2,Double entalpiasalida1)
        {
            //Lista de cadenas que guardan las ecuaciones del sistema
            List<String> ecuaciones2 = new List<String>();
  
                ecuaciones2.Add("");
                ecuaciones2[auxiliar] = "W" + Convert.ToString(correntrada) + "-" + "W" + Convert.ToString(corrsalida1) + "-" + "W" + Convert.ToString(corrsalida2);
                auxiliar++;

                ecuaciones2.Add("");
                ecuaciones2[auxiliar] = "W" + Convert.ToString(corrsalida2) + "-" + "W" + Convert.ToString(correntrada) + "*" + "(" + Convert.ToString(D2) + "+" + "(" + "(" + "1" + "-" + "xph(P" + Convert.ToString(correntrada) + ",H" + Convert.ToString(correntrada) + ")" + ")" + "*" + Convert.ToString(D1) + ")" + ")";
                auxiliar++;

                ecuaciones2.Add("");
                ecuaciones2[auxiliar] = "P" + Convert.ToString(corrsalida1) + "-" + "P" + Convert.ToString(correntrada);
                auxiliar++;

                ecuaciones2.Add("");
                ecuaciones2[auxiliar] = "P" + Convert.ToString(corrsalida2) + "-" + "P" + Convert.ToString(correntrada);
                auxiliar++;

                ecuaciones2.Add("");
                ecuaciones2[auxiliar] = "(" + "W" + Convert.ToString(corrsalida2) + "*" + "H" + Convert.ToString(corrsalida2) + ")" + "-" + "(" + "(" + "1" + "-" + "xph(P" + Convert.ToString(correntrada) + ",H" + Convert.ToString(correntrada) + ")" + ")" + "*" + Convert.ToString(D1) + "*" + "W" + Convert.ToString(correntrada) + "*" + "hsatp(P" + Convert.ToString(correntrada) + ")" + ")" + "-" + "(" + Convert.ToString(D2) + "*" + "W" + Convert.ToString(correntrada) + "*" + "H" + Convert.ToString(corrsalida1) + ")";
                auxiliar++;                

                ecuaciones2.Add("");
                ecuaciones2[auxiliar] = "(" + "W" + Convert.ToString(correntrada) + "*" + "H" + Convert.ToString(correntrada) + ")" + "-" + "(" + "W" + Convert.ToString(corrsalida1) + "*" + "H" + Convert.ToString(corrsalida1) + ")" + "-" + "(" + "W" + Convert.ToString(corrsalida2) + "*" + "H" + Convert.ToString(corrsalida2) + ")";
                auxiliar++;
          
            numecuaciones2 = auxiliar;
            numvariables2 = 6;
           
            return (ecuaciones2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            funcionauxiliar1();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
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

            //Creamos un NUEVO equipo Tipo Turbina10 en la lista de equipos11 de la aplicación principal
            if (ediciononuevo1 == 0)
            {
                //Declaramos y creamos un objeto de la CLASE EQUIPOS para guardar los datos de entrada del usuario del nuevo equipo "condición de contorno" generado.
                Equipos sephumedad13 = new Equipos();
                sephumedad13.Inicializar(numequipo, 13, correntrada, 0, corrsalida1, corrsalida2, D1, D2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                //Copio los valores de la clase de entrada de datos en la aplicación principal
                punteroaplicacion1.equipos11.Add(sephumedad13);

                //Incrementamos el Número Total de Equipos y Número Total de Corrientes en la aplicación principal
                punteroaplicacion1.NumTotalEquipos++;
                punteroaplicacion1.NumTotalCorrientes = punteroaplicacion1.NumTotalCorrientes + 2;   
            }

            //EDICIÓN de un Equipo ya creado en la lista de equipos11 de la aplicación principal
            else if (ediciononuevo1 == 1)
            {
                punteroaplicacion1.equipos11[indice1].numequipo2 = numequipo;
                punteroaplicacion1.equipos11[indice1].aN1 = correntrada;
                punteroaplicacion1.equipos11[indice1].aN3 = corrsalida1;
                punteroaplicacion1.equipos11[indice1].aN4 = corrsalida2;
                punteroaplicacion1.equipos11[indice1].aD1 = D1;
                punteroaplicacion1.equipos11[indice1].aD2 = D2;

                //Esta marca indica que cuando realicemos cálculos sucesivos y hayamos editado un equipo es necesario generar de nuevo las ecuaciones y los parámetros de los equipos guardados en el array de objetos equipos11 de la aplicación principal
                punteroaplicacion1.marca = 0;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
