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
    public partial class Reactor : Form
    {
        public Double D1, D2,D3, D4, D5,D6,D7, D9;
        public Double correntrada, corrsalida;
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

        public Reactor(Aplicacion punteroaplicion,Double numecuaciones1,Double numvariables1,Double ediciononuevo,int indice)
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

                //label11.Text = "m2";

            }
            else if (punteroaplicacion1.unidades == 2)
            {
                //label11.Text = "m2";
                
            }
            else
            {

            }
            D1 = 0;
            D2 = 0;
            D3= 0;
            D4 = 0;
            D5 = 0;
            D7 = 0;
            D9 = 0;

            correntrada = 0;
            corrsalida = 0;

            numequipo = 0;

            numecuaciones2 = numecuaciones1;

            numvariables2 = numvariables1;
        }

     
        //Botón de Generar Ecuaciones
        private void button1_Click(object sender, EventArgs e)
        {
            D1 = Convert.ToDouble(textBox1.Text);
            D2 = Convert.ToDouble(textBox2.Text);
            D3 = Convert.ToDouble(textBox3.Text);
            D4 = Convert.ToDouble(textBox4.Text);
            D5 = Convert.ToDouble(textBox5.Text);
            D6 = Convert.ToDouble(textBox6.Text);
            D7 = Convert.ToDouble(textBox7.Text);

            D9 = Convert.ToDouble(textBox11.Text);

            correntrada = Convert.ToDouble(textBox10.Text);
            corrsalida = Convert.ToDouble(textBox8.Text);

            numequipo = Convert.ToDouble(textBox9.Text);

            funcionauxiliar();
        }

            public void funcionauxiliar()
            {

            //Conversión UNIDADES
            //Como las Tablas de Vapor de Agua ASME 1967 están en unidades británicas, siempre tenemos que convertir los datos de entrada a Unidades Británicas

            //Unidades Sistema Internacional (W Kgr/sg P Bar H Kj/Kgr)
            if (punteroaplicacion1.unidades == 2)
            {
                //Factor independiente de pérdida de carga
                D1 = D1 / (6.8947572 / 100);
                //Factor lineal de pérdida de carga
                D2 = D2 * 6.578911309;
                //Factor cuadrático de pérdida de carga
                D3 = D3 * 2.984193609;

                if (D4 < 0)
                { 
                    //En este caso fijamos la presión P2=D4, por tanto, tenemos que realizar la conversión de Bar a PSI
                    D4 = D4 / (6.8947572 / 100);                
                }
                
                if (D5>0)
                {
                     //Entalpia Kj/Kgr a Btu/Lb
                     D5 = D5 / 2.326009;
                }
                else if (D5 < 0)
                {
                     //Convertir los grados ºC en ºF
                     D5 = ((D5 * 9.0) / 5.0) - 32;
                }

                //Convertimos el Calor aportado al ciclo de Kw a BTU/sg
                D6 = D6 * 0.9486608;
            }

            //Unidades Sistema Métrico (W Kgr/sg, P kPa, H Kj/Kgr)
            else if (punteroaplicacion1.unidades == 1)
            {
                //Presión kPa a psia
                D1 = D1 / (6.894757);
                //Factor lineal de pérdida de carga
                D2 = D2 * 6.578911309;
                //Factor cuadrático de pérdida de carga
                D3 = D3 * 2.984193609;
                if (D5 > 0)
                {
                    //Entalpia Kj/Kgr a Btu/Lb
                    D5 = D5 / 2.326009;
                }
                else if (D5 < 0)
                {
                    //Convertir los grados ºC en ºF
                    D5 = ((D5 * 9.0) / 5.0) + 32;
                }
            }

            //Unidades Sistema Británico
            else if (punteroaplicacion1.unidades == 0)
            {

            }
            else
            {

            }
          
            ecuaciones1=generaecucaiones(D1,D2,D3,D4,D5,D6,D7,D9,correntrada,corrsalida);

            listBox1.Items.Add("Nº Equipo:" + Convert.ToString(numequipo));

            for (int numecua = 0; numecua < auxiliar; numecua++)
            {
                listBox1.Items.Add(ecuaciones1[numecua]);
            }

            listBox1.Items.Add("");

            button2.Enabled = true;            
        }

        private List<String> generaecucaiones(Double D1, Double D2,Double D3,Double D4, Double D5,Double D6,Double D7, Double D9, Double correntrada, Double corrsalida)
        {
            //Lista de cadenas que guardan las ecuaciones del sistema
            List<String> ecuaciones2 = new List<String>();
           
            ecuaciones2.Add("");
            ecuaciones2[auxiliar] = "W" + Convert.ToString(correntrada) + "-" + "W" + Convert.ToString(corrsalida);
            auxiliar++;

            if(D4<0)
            {
            ecuaciones2.Add("");
            ecuaciones2[auxiliar] = "P" + Convert.ToString(correntrada) + "-" + "P" + Convert.ToString(corrsalida);
            auxiliar++;
            }

            else if(D4>0)
            {
                ecuaciones2.Add("");
                ecuaciones2[auxiliar] = "P" + Convert.ToString(correntrada) + "-" + "P" + Convert.ToString(corrsalida);
                auxiliar++;
            }

            if (D5 > 0) 
            {
                ecuaciones2.Add("");
                ecuaciones2[auxiliar] = "P" + Convert.ToString(correntrada) + "-" + "P" + Convert.ToString(corrsalida);
                auxiliar++;          
            }

            else if (D5 < 0)
            {
                ecuaciones2.Add("");
                ecuaciones2[auxiliar] = "P" + Convert.ToString(correntrada) + "-" + "P" + Convert.ToString(corrsalida);
                auxiliar++;    
            }

            else if (D6 != 0)
            {
                ecuaciones2.Add("");
                ecuaciones2[auxiliar] = "P" + Convert.ToString(correntrada) + "-" + "P" + Convert.ToString(corrsalida);
                auxiliar++;    
            }
           
            numecuaciones2 = auxiliar;
            numvariables2 = 3;
           
            return (ecuaciones2);
        }

        //Botón de OK
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

            //Creamos un NUEVO equipo Tipo condición de contorno en la lista de equipos11 de la aplicación principal
            if (ediciononuevo1 == 0)
            {
                //Declaramos y creamos un objeto de la CLASE EQUIPOS para guardar los datos de entrada del usuario del nuevo equipo "condición de contorno" generado.
                Equipos reactor6 = new Equipos();
                reactor6.Inicializar(numequipo, 6, correntrada, 0, corrsalida, 0, D1, D2, D3, D4, D5, D6, D7, 0, D9, 0, 0, 0, 0);
                //Copio los valores de la clase de entrada de datos en la aplicación principal
                punteroaplicacion1.equipos11.Add(reactor6);

                //Incrementamos el Número Total de Equipos y Número Total de Corrientes en la aplicación principal
                punteroaplicacion1.NumTotalEquipos++;
                punteroaplicacion1.NumTotalCorrientes = punteroaplicacion1.NumTotalCorrientes + 1;
            }

            //EDICIÓN de un Equipo ya creado en la lista de equipos11 de la aplicación principal
            else if (ediciononuevo1 == 1)
            {
                punteroaplicacion1.equipos11[indice1].numequipo2 = numequipo;
                punteroaplicacion1.equipos11[indice1].aN1 = correntrada;
                punteroaplicacion1.equipos11[indice1].aN3 = corrsalida;
                punteroaplicacion1.equipos11[indice1].aD1 = D1;
                punteroaplicacion1.equipos11[indice1].aD2 = D2;
                punteroaplicacion1.equipos11[indice1].aD3 = D3;
                punteroaplicacion1.equipos11[indice1].aD4 = D4;
                punteroaplicacion1.equipos11[indice1].aD5 = D5;
                punteroaplicacion1.equipos11[indice1].aD6 = D6;
                punteroaplicacion1.equipos11[indice1].aD7 = D7;
                punteroaplicacion1.equipos11[indice1].aD9 = D9;

                //Esta marca indica que cuando realicemos cálculos sucesivos y hayamos editado un equipo es necesario generar de nuevo las ecuaciones y los parámetros de los equipos guardados en el array de objetos equipos11 de la aplicación principal
                punteroaplicacion1.marca = 0;
            }   
        }

        //Botón de Cancel
        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}