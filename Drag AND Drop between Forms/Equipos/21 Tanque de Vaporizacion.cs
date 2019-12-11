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
    public partial class TanqueVaporizacion : Form
    {
        public Double D1;
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

        public TanqueVaporizacion(Aplicacion punteroaplicion, Double numecuaciones1, Double numvariables1, Double ediciononuevo, int indice)
        {
            InitializeComponent();           

            punteroaplicacion1 = punteroaplicion;

            //Estos dos parámetros se utilizan para EDICION o NUEVO equipos (indice y edicionnuevo)
            indice1 = indice;
            ediciononuevo1 = ediciononuevo;
         
            //Inicializamos las etiquetas de las unidades de entrada en el cuadro de dialogo de toma de datos
            if (punteroaplicacion1.unidades == 0)
            {

            }
            else if (punteroaplicacion1.unidades == 1)
            {
                label17.Text = "Bar";
                
            }
            else if (punteroaplicacion1.unidades == 2)
            {
                label17.Text = "kPa";
                

            }
            else
            {

            }

            D1 = 0;
            
            correntrada = 0;
            corrsalida1 = 0;
            corrsalida2 = 0;

            numequipo = 0;

            numecuaciones2 = numecuaciones1;

            numvariables2 = numvariables1;
        
        }

        //Boton Generar Ecuaciones
        private void button1_Click(object sender, EventArgs e)
        {
            D1=Convert.ToDouble(textBox1.Text);          

            correntrada=Convert.ToDouble(textBox7.Text);
            corrsalida1 = Convert.ToDouble(textBox8.Text);
            corrsalida2 = Convert.ToDouble(textBox4.Text);

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
            }

            //Unidades Sistema Métrico
            else if (punteroaplicacion1.unidades == 1)
            {
                //Presión kPa a psia
                D1 = D1 / (6.8947572);
            }

            //Unidades Sistema Británico
            else if (punteroaplicacion1.unidades == 0)
            {

            }
            else
            {

            }

            ecuaciones1=generaecucaiones(D1,correntrada,corrsalida1,corrsalida2);

            listBox1.Items.Add("Nº Equipo:" + Convert.ToString(numequipo));

            for (int numecua = 0; numecua < auxiliar; numecua++)
            {
                listBox1.Items.Add(ecuaciones1[numecua]);
            }

            listBox1.Items.Add("");

            button2.Enabled = true;
        }

        private List<String> generaecucaiones(Double D1,Double correntrada, Double corrsalida1,Double corrsalida2)
        {
            //Lista de cadenas que guardan las ecuaciones del sistema
            List<String> ecuaciones2 = new List<String>();

            ecuaciones2.Add("");
            ecuaciones2[auxiliar] = "W" + Convert.ToString(correntrada) + "-" + "W" + Convert.ToString(corrsalida1) + "-" + "W" + Convert.ToString(corrsalida2);
            auxiliar++;

            ecuaciones2.Add("");
            ecuaciones2[auxiliar] = "P" + Convert.ToString(correntrada) + "-" + "P" + Convert.ToString(corrsalida1);
            auxiliar++;

            ecuaciones2.Add("");
            ecuaciones2[auxiliar] = "H" + Convert.ToString(correntrada) + "-" + "H" + Convert.ToString(corrsalida1);
            auxiliar++;

            ecuaciones2.Add("");
            ecuaciones2[auxiliar] = "P" + Convert.ToString(correntrada) + "-" + "P" + Convert.ToString(corrsalida2);
            auxiliar++;

            ecuaciones2.Add("");
            ecuaciones2[auxiliar] = "H" + Convert.ToString(correntrada) + "-" + "H" + Convert.ToString(corrsalida2);
            auxiliar++;

            ecuaciones2.Add("");
            ecuaciones2[auxiliar] = "H" + Convert.ToString(correntrada) + "-" + "H" + Convert.ToString(corrsalida2);
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
                Equipos flashtank21 = new Equipos();
                flashtank21.Inicializar(numequipo, 21, correntrada, 0, corrsalida1, corrsalida2, D1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                //Copio los valores de la clase de entrada de datos en la aplicación principal
                punteroaplicacion1.equipos11.Add(flashtank21);

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
