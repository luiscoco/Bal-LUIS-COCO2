﻿using System;
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
    public partial class MSR : Form
    {
        public Double D1, D2, D3,D4, D5, D6, D7,D8,D9;
        public Double correntrada1,correntrada2, corrsalida1,corrsalida2;
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

        public MSR(Aplicacion punteroaplicion,Double numecuaciones1,Double numvariables1,Double ediciononuevo,int indice)
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
                label13.Text = "ºC";
            }
            else if (punteroaplicacion1.unidades == 2)
            {
                label13.Text = "ºC";
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
            corrsalida1 = 0;
            corrsalida2 = 0;

            numequipo = 0;

            numecuaciones2 = numecuaciones1;

            numvariables2 = numvariables1;
        
        }

        private void Condcontorno_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            D1=Convert.ToDouble(textBox1.Text);
            D2=Convert.ToDouble(textBox2.Text);
            D3=Convert.ToDouble(textBox3.Text);
            D4 = Convert.ToDouble(textBox4.Text);
            D5=Convert.ToDouble(textBox5.Text);
            D6=Convert.ToDouble(textBox6.Text);
            D7=Convert.ToDouble(textBox7.Text);
            D8 = Convert.ToDouble(textBox8.Text);
            D9 = Convert.ToDouble(textBox14.Text);

            correntrada1=Convert.ToDouble(textBox10.Text);
            correntrada2 = Convert.ToDouble(textBox11.Text);
            corrsalida1 = Convert.ToDouble(textBox12.Text);
            corrsalida2 = Convert.ToDouble(textBox13.Text);

            numequipo = Convert.ToDouble(textBox9.Text);

            funcionauxiliar();
        }


        public void funcionauxiliar()
        {
            //Conversión UNIDADES
            //Como las Tablas de Vapor de Agua ASME 1967 están en unidades británicas, siempre tenemos que convertir los datos de entrada a Unidades Británicas

            //CONVERSION DE UNIDADES DEL TTD
            //Unidades Sistema Internacional
            if (punteroaplicacion1.unidades == 2)
            {
                //Convertir el TTD de ºC a ºF
                D5 = ((D5 * 9) / 5);
                //Presión de Bar a psia
                D1 = D1 / (6.8947572 / 100);
                D7 = D7 / (6.8947572 / 100);
                //Factor cuadrático de pérdida de carga
                D3 = D3 * 2.984193609;
                //Factor lineal de pérdida de carga
                D2 = D2 * 6.578911309;
                //Factor cuadrático de pérdida de carga
                D9 = D9 * 2.984193609;
                //Factor lineal de pérdida de carga
                D8 = D8 * 6.578911309;
            }

            //Unidades Sistema Métrico
            else if (punteroaplicacion1.unidades == 1)
            {
                //Convertir el TTD de ºC a ºF
                D5 = ((D5 * 9) / 5);
                //Presión kPa a psia
                D1 = D1 / (6.894757);
                //Presión kPa a psia
                D7 = D7 / (6.894757);
            }

            //Unidades Sistema Británico
            else if (punteroaplicacion1.unidades == 0)
            {

            }
            else
            {

            }

            ecuaciones1=generaecucaiones(D1,D2,D3,D4,D5,D6,D7,D8,D9,correntrada1,correntrada2,corrsalida1,corrsalida2);

            listBox1.Items.Add("Nº Equipo:" + Convert.ToString(numequipo));

            for (int numecua = 0; numecua < auxiliar; numecua++)
            {
                listBox1.Items.Add(ecuaciones1[numecua]);
            }
            listBox1.Items.Add("");
        }

        private List<String> generaecucaiones(Double D1, Double D2,Double D3,Double D4, Double D5, Double D6, Double D7,Double D8, Double D9, Double correntrada1,Double correntrada2, Double corrsalida1,Double corrsalida2)
        {
            //Lista de cadenas que guardan las ecuaciones del sistema
            List<String> ecuaciones2 = new List<String>();

                      
            ecuaciones2.Add("");
            ecuaciones2[auxiliar] = "W" + Convert.ToString(correntrada1) + "-" + "W" + Convert.ToString(corrsalida1);
            auxiliar++;

            ecuaciones2.Add("");
            ecuaciones2[auxiliar] = "W" + Convert.ToString(correntrada2) + "-" + "W" + Convert.ToString(corrsalida2);
            auxiliar++;

            ecuaciones2.Add("");
            ecuaciones2[auxiliar] = "P" + Convert.ToString(correntrada1) + "-" + "P" + Convert.ToString(corrsalida1) + "-" + Convert.ToString(D1) + "-" + Convert.ToString(D2) + "*" + "W" + Convert.ToString(correntrada1) + "-" + Convert.ToString(D3) + "*" + "W" + Convert.ToString(correntrada1) + "*" + "W" + Convert.ToString(correntrada1) + "-" + Convert.ToString(D4) + "*" + "P" + Convert.ToString(correntrada1);
            auxiliar++;

            ecuaciones2.Add("");
            ecuaciones2[auxiliar] = "W" + Convert.ToString(correntrada1) + "-" + "W" + Convert.ToString(corrsalida1);
            auxiliar++;

            ecuaciones2.Add("");
            ecuaciones2[auxiliar] = "W" + Convert.ToString(correntrada1) + "-" + "W" + Convert.ToString(corrsalida1);
            auxiliar++;

            ecuaciones2.Add("");
            ecuaciones2[auxiliar] = "W" + Convert.ToString(correntrada1) + "-" + "W" + Convert.ToString(corrsalida1);
            auxiliar++;

            ecuaciones2.Add("");
            ecuaciones2[auxiliar] = "W" + Convert.ToString(correntrada1) + "-" + "W" + Convert.ToString(corrsalida1);
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

            //Creamos un NUEVO equipo Tipo MSR14 en la lista de equipos11 de la aplicación principal
            if (ediciononuevo1 == 0)
            {
                //Declaramos y creamos un objeto de la CLASE EQUIPOS para guardar los datos de entrada del usuario del nuevo equipo "condición de contorno" generado.
                Equipos msr14 = new Equipos();
                msr14.Inicializar(numequipo, 14, correntrada1, correntrada2, corrsalida1, corrsalida2, D1, D2, D3, D4, D5, D6, D7, D8, D9, 0, 0, 0, 0);
                //Copio los valores de la clase de entrada de datos en la aplicación principal
                punteroaplicacion1.equipos11.Add(msr14);

                //Incrementamos el Número Total de Equipos y Número Total de Corrientes en la aplicación principal
                punteroaplicacion1.NumTotalEquipos++;
                punteroaplicacion1.NumTotalCorrientes = punteroaplicacion1.NumTotalCorrientes + 2;  
            }

             //EDICIÓN de un Equipo ya creado en la lista de equipos11 de la aplicación principal
            else if (ediciononuevo1 == 1)
            {
                punteroaplicacion1.equipos11[indice1].numequipo2 = numequipo;
                punteroaplicacion1.equipos11[indice1].aN1 = correntrada1;               
                punteroaplicacion1.equipos11[indice1].aN2 = correntrada2;
                punteroaplicacion1.equipos11[indice1].aN3 = corrsalida1;
                punteroaplicacion1.equipos11[indice1].aN4 = corrsalida2;
                punteroaplicacion1.equipos11[indice1].aD1 = D1;
                punteroaplicacion1.equipos11[indice1].aD2 = D2;
                punteroaplicacion1.equipos11[indice1].aD3 = D3;
                punteroaplicacion1.equipos11[indice1].aD4 = D4;
                punteroaplicacion1.equipos11[indice1].aD5 = D5;
                punteroaplicacion1.equipos11[indice1].aD6 = D6;
                punteroaplicacion1.equipos11[indice1].aD7 = D7;
                punteroaplicacion1.equipos11[indice1].aD8 = D8;
                punteroaplicacion1.equipos11[indice1].aD9 = D9;

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
