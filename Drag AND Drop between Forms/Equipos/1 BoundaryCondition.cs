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
    public partial class Condcontorno : Form
    {
        public int numparnuevos = 0;

        public Double ediciononuevo1 = 0;

        //El parámetro auxiliar nos indica si se trata de un equipos tipo condición de contorno que genera 2 o 1 corriente
        //Si parametroauxiliar=1 solo genera una corriente la de salida y si parametroauxiliar=0 genera dos corrientes la de entrada y la de salida
        public Double D1, D2, D3, D5, D6, D7;
        public Double correntrada, corrsalida, parametroauxiliar;
        public Double numequipo;

        public Double numecuaciones2;
        public Double numvariables2;

        //Lista de cadenas String que guardan las ecuaciones del sistema
        List<String> ecuaciones1 = new List<String>();

        Aplicacion punteroaplicacion1;

        public Double numparametroscreados=0;

        int auxiliar = 0;

        //En el caso de que queramos EDITAR un equiop ya creado utilizamos estas dos variables para ayudarnos a buscar su indice en la lista de equipos11 de la aplicacion principal que contiene todos los equipos
        int indice1 = 0;
        int marca = 0;

        public Condcontorno(Aplicacion punteroaplicion,Double numecuaciones1,Double numvariables1,Double ediciononuevo,int indice)
        {
            InitializeComponent();

            //Estos dos parámetros se utilizan para EDICION o NUEVO equipos (indice y edicionnuevo)
            indice1 = indice;
            ediciononuevo1 = ediciononuevo;
            
            punteroaplicacion1 = punteroaplicion;

            //numparametroscreados= (punteroaplicacion1.numcorrientes)*3;

            
            //Inicializamos las etiquetas de las unidades de entrada en el cuadro de dialogo de toma de datos
            if (punteroaplicacion1.unidades == 0)
            {

            }
            else if (punteroaplicacion1.unidades == 1)
            {
                label11.Text = "Kgr/sg";
                label12.Text = "kPa";
                label13.Text = "Kj/Kgr";
                label14.Text = "Bar ó ºC";


            }
            else if (punteroaplicacion1.unidades == 2)
            {
                label11.Text = "Kgr/sg";
                label12.Text = "Bar";
                label13.Text = "Kj/Kgr";
                label14.Text = "Bar ó ºC";
            }
            else
            {

            }

            D1 = 0;
            D2 = 0;
            D3 = 0;
            D5 = 0;
            D6 = 0;
            D7 = 0;
            
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
            D1 = Convert.ToDouble(textBox1.Text);
            D2 = Convert.ToDouble(textBox2.Text);
            D3 = Convert.ToDouble(textBox3.Text);
            D5 = Convert.ToDouble(textBox4.Text);
            D6 = Convert.ToDouble(textBox5.Text);
            D7 = Convert.ToDouble(textBox6.Text);

            correntrada = Convert.ToDouble(textBox7.Text);
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
                //Caudal Kgr/sg a Lb/sg
                D1 = D1 / (0.4536);
                //Presión Bar a psia
                D2 = D2 / (6.8947572 / 100);
                //Entalpia Kj/Kgr a Btu/Lb
                D3 = D3 / 2.326009;
               
                if (D6 > 0)
                {
                    Double te = D6;
                    //Presión de Bar a psia
                    D6 = te / (6.8947572 / 100);
                }

                else if (D6 < 0)
                {
                    //Convertir los grados ºC en ºF
                    D6 = ((D6 * 9) / 5) + 32;
                }

            }

            //Unidades Sistema Métrico
            else if (punteroaplicacion1.unidades == 1)
            {
                //Caudal Kgr/sg a Lb/sg
                D1 = D1 / (0.4536);
                //Presión kPa a psia
                D2 = D2 / (6.894757);
                //Entalpia Kj/Kgr a Btu/Lb
                D3 = D3 / 2.326009;
                if (D6 > 0)
                {
                    Double te = D6;
                    //Presión de Bar a psia
                    D6 = te / (6.8947572 / 100);
                }

                else if (D6 < 0)
                {
                    //Convertir los grados ºC en ºF
                    D6 = ((D6 * 9) / 5) + 32;
                }
            }

            //Unidades Sistema Británico
            else if (punteroaplicacion1.unidades == 0)
            {

            }
            else
            {

            }

            ecuaciones1=generaecucaiones(D1,D2,D3,D5,D6,D7,correntrada,corrsalida);

            listBox1.Items.Add("Nº Equipo:" + Convert.ToString(numequipo));

            for (int numecua = 0; numecua < auxiliar; numecua++)
            {
                listBox1.Items.Add(ecuaciones1[numecua]);
            }

            //listBox1.Items.Add("");

            button2.Enabled = true;
            button1.Enabled = false;

        }


        private List<String> generaecucaiones(Double D1, Double D2,Double D3, Double D5, Double D6, Double D7, Double correntrada, Double corrsalida)
        {
            //Lista de cadenas que guardan las ecuaciones del sistema
            List<String> ecuaciones2 = new List<String>();

           
            //No hay corriente de entrada desde otro equipo, es el primer equipo del Sistema 
            if (checkBox1.Checked == false)
            {
              
                if (D5 == 0)
                {
                    ecuaciones2.Add("");
                    ecuaciones2[auxiliar] = "W" + Convert.ToString(correntrada) + "-" + "W" + Convert.ToString(corrsalida);
                    auxiliar++;
                }

                ecuaciones2.Add("");
                ecuaciones2[auxiliar] = "P" + Convert.ToString(correntrada) + "-" + "P" + Convert.ToString(corrsalida);
                auxiliar++;

                ecuaciones2.Add("");
                ecuaciones2[auxiliar] = "H" + Convert.ToString(correntrada) + "-" + "H" + Convert.ToString(corrsalida);
                auxiliar++;

                if (D1 > 0)
                {
                    ecuaciones2.Add("");
                    ecuaciones2[auxiliar] = "W" + Convert.ToString(correntrada) + "-" + Convert.ToString(D1);
                    auxiliar++;
                }

                if (D2 > 0)
                {
                    ecuaciones2.Add("");
                    ecuaciones2[auxiliar] = "P" + Convert.ToString(correntrada) + "-" + Convert.ToString(D2);
                    auxiliar++;
                }

                if (D3 > 0)
                {
                    ecuaciones2.Add("");
                    ecuaciones2[auxiliar] = "H" + Convert.ToString(correntrada) + "-" + Convert.ToString(D3);
                    auxiliar++;
                }

                else if (D6 > 0 && D3 == 0)
                {

                    //Llamada a la función h(p=D6,x=D7)
                    ecuaciones2.Add("");
                    ecuaciones2[auxiliar] = "H" + Convert.ToString(correntrada) + "-" + "hpx(" + Convert.ToString(D6) + "," + Convert.ToString(D7) + ")";
                    auxiliar++;
                }

                else if (D6 < 0 && D3 == 0)
                {

                    //Llamada a la función h(T=-D6,x=D7)
                    ecuaciones2.Add("");
                    ecuaciones2[auxiliar] = "H" + Convert.ToString(correntrada) + "-" + "htx(" + Convert.ToString(-D6) + "," + Convert.ToString(D7) + ")";
                    auxiliar++;
                }

                numecuaciones2 = auxiliar;
                numvariables2 = 6;
            }

            //Corriente de entrada proveniente de otro equipo
            else if (checkBox1.Checked == true)
            {
                ecuaciones2.Add("");
                ecuaciones2[auxiliar] = "W" + Convert.ToString(correntrada) + "-" + "W" + Convert.ToString(corrsalida);
                auxiliar++;

                ecuaciones2.Add("");
                ecuaciones2[auxiliar] = "P" + Convert.ToString(correntrada) + "-" + "P" + Convert.ToString(corrsalida);
                auxiliar++;

                ecuaciones2.Add("");
                ecuaciones2[auxiliar] = "H" + Convert.ToString(correntrada) + "-" + "H" + Convert.ToString(corrsalida);
                auxiliar++;

                //Caudal diferente de cero W1=D1
                if (D1>0)
                {
                    ecuaciones2.Add("");
                    ecuaciones2[auxiliar] = "W" + Convert.ToString(correntrada) + "-" + Convert.ToString(D1);
                    auxiliar++;

                    numecuaciones2 = auxiliar;
                    numvariables2 = 3;
                }

                //Presión diferente de cero P1=D2
                if (D2 > 0)
                {
                    ecuaciones2.Add("");
                    ecuaciones2[auxiliar] = "P" + Convert.ToString(correntrada) + "-" + Convert.ToString(D2);
                    auxiliar++;

                    numecuaciones2 = auxiliar;
                    numvariables2 = 3;
                }

                if (D3 > 0)
                {
                    ecuaciones2.Add("");
                    ecuaciones2[auxiliar] = "H" + Convert.ToString(correntrada) + "-" + Convert.ToString(D3);
                    auxiliar++;

                    numecuaciones2 = auxiliar;
                    numvariables2 = 3;
                }

                numecuaciones2 = auxiliar;
                numvariables2 = 3;
            }

            else
            {

            }

            //Habilitamos el Botón de OK porque ya hemos generado las ecuaciones y las variables
            button2.Enabled = true;


            //Enviamos a la aplicación general el array de string "ecuaciones2" incluyendo las ecuaciones generadas para su envio a la listbox1 que las grafica en este form


            return (ecuaciones2);

        }


        //Botón OK
        //Condcontorno este botón enviamos tanto las ecuaciones, los nombres de las variables y las lineas de código de Hbal a la aplicación general
        public void button2_Click(object sender, EventArgs e)
        {
            funcionauxiliar1();
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
            punteroaplicacion1.numvariables = numvariables2+punteroaplicacion1.numvariables;


            //Creamos un NUEVO equipo Tipo condición de contorno en la lista de equipos11 de la aplicación principal
            if (ediciononuevo1 == 0)
            {
                //Declaramos y creamos un objeto de la CLASE EQUIPOS para guardar los datos de entrada del usuario del nuevo equipo "condición de contorno" generado.
                Equipos condicioncontorno1 = new Equipos();
                //Hay otro equipo conectado se generar 3 parámetros de salida y sus correspondientes ecuaciones
                if (checkBox1.Checked == true)
                {
                    condicioncontorno1.Inicializar(numequipo, 1, correntrada, parametroauxiliar, corrsalida, 0, D1, D2, D3, 0, D5, D6, D7, 0, 0, 1, 0, 0, 0);
                    //Copio los valores de la clase de entrada de datos en la aplicación principal
                    punteroaplicacion1.equipos11.Add(condicioncontorno1);
                }
                //No hay otro equipos conectado se generan 6 parámetros y correspondientes écuaciones
                else if (checkBox1.Checked == false)
                {
                    condicioncontorno1.Inicializar(numequipo, 1, correntrada, parametroauxiliar, corrsalida, 0, D1, D2, D3, 0, D5, D6, D7, 0, 0, 0, 0, 0, 0);
                    //Copio los valores de la clase de entrada de datos en la aplicación principal
                    punteroaplicacion1.equipos11.Add(condicioncontorno1);
                }

                //Incrementamos el Número Total de Equipos y Número Total de Corrientes en la aplicación principal
                punteroaplicacion1.NumTotalEquipos++;

                if (parametroauxiliar == 0)
                {
                    punteroaplicacion1.NumTotalCorrientes = punteroaplicacion1.NumTotalCorrientes + 2;
                }

                else if (parametroauxiliar == 1)
                {
                    punteroaplicacion1.NumTotalCorrientes = punteroaplicacion1.NumTotalCorrientes + 1;
                }
            }

            //EDICIÓN de un Equipo ya creado en la lista de equipos11 de la aplicación principal
            else if (ediciononuevo1 == 1)
            {
            punteroaplicacion1.equipos11[indice1].numequipo2 = numequipo;
            punteroaplicacion1.equipos11[indice1].aN1= correntrada;
            punteroaplicacion1.equipos11[indice1].aN3 = corrsalida;
            punteroaplicacion1.equipos11[indice1].aD1 = D1;
            punteroaplicacion1.equipos11[indice1].aD2 = D2;
            punteroaplicacion1.equipos11[indice1].aD3 = D3;
            punteroaplicacion1.equipos11[indice1].aD5 = D5;
            punteroaplicacion1.equipos11[indice1].aD6 = D6;
            punteroaplicacion1.equipos11[indice1].aD7 = D7;

            //Esta marca indica que cuando realicemos cálculos sucesivos y hayamos editado un equipo es necesario generar de nuevo las ecuaciones y los parámetros de los equipos guardados en el array de objetos equipos11 de la aplicación principal
            punteroaplicacion1.marca = 0;

            }              
        }

        //Botón Cancel
        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
