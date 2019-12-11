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
        int numparnuevos = 0;

        Double D1, D2, D3, D5, D6, D7;
        Double correntrada, corrsalida;
        Double numequipo;

        Double numecuaciones2;
        Double numvariables2;

        //Lista de cadenas para gardar los nombres de las variables del sistema de ecuaciones
        List<String> variables1 = new List<String>();

        //Lista de cadenas que guardan las ecuaciones del sistema
        List<String> ecuaciones1 = new List<String>();

        //Dos líneas del archivo de entrada de HBAL creadas por el equipo Condición de Contorno
        String lineaprimera="";
        String lineasegunda ="";

        Aplicacion punteroaplicacion1;

        Double numparametroscreados=0;

        int auxiliar = 0;

        public Condcontorno(Aplicacion punteroaplicion,Double numecuaciones1,Double numvariables1)
        {
            InitializeComponent();
            
            punteroaplicacion1 = punteroaplicion;

            numparametroscreados= (punteroaplicacion1.numcorrientes)*3;
            

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

            //Conversión UNIDADES
            //Como las Tablas de Vapor de Agua ASME 1967 están en unidades británicas, siempre tenemos que convertir los datos de entrada a Unidades Británicas

            //Unidades Métricas
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

            //Unidades Sistema Internacional
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

            if (checkBox1.Checked==true)
            {
                numparnuevos=numparnuevos+3;

            }
           
            else if (checkBox1.Checked==false)
            {
                //
                numparnuevos = numparnuevos + 6;            
            }

            Random random = new Random();

            //CREAMOS EL ARRAY DE PARAMETROS
            for (int v = (int)numparametroscreados; v < ((numparametroscreados) + numparnuevos); v++)
            {
                //int randomNumber = random.Next(0, 2500);
                //Creamos la lista de parámetros generadas por este programa
                punteroaplicacion1.p.Add(punteroaplicacion1.ptemp);
                punteroaplicacion1.p[v] = new Parameter(10, 0.01, "");
            }
           

            //Si no hay corriente de entrada proveniente de otro equipo, el equipo es de Input Source del Sistema
            if (checkBox1.Checked == false)
            {
                punteroaplicacion1.p[(int)numparametroscreados].Nombre = "W" + Convert.ToString(correntrada);
                punteroaplicacion1.p[(int)numparametroscreados].Value = D1;
                punteroaplicacion1.p[(int)numparametroscreados + 1].Nombre = "W" + Convert.ToString(corrsalida);
                punteroaplicacion1.p[(int)numparametroscreados + 1].Value = D1;

                punteroaplicacion1.p[(int)numparametroscreados + 2].Nombre = "P" + Convert.ToString(correntrada);
                punteroaplicacion1.p[(int)numparametroscreados + 2].Value = D2;
                punteroaplicacion1.p[(int)numparametroscreados + 3].Nombre = "P" + Convert.ToString(corrsalida);
                punteroaplicacion1.p[(int)numparametroscreados + 3].Value = D2;
                punteroaplicacion1.p[(int)numparametroscreados + 4].Nombre = "H" + Convert.ToString(correntrada);
                punteroaplicacion1.p[(int)numparametroscreados + 4].Value = D3;
                punteroaplicacion1.p[(int)numparametroscreados + 5].Nombre = "H" + Convert.ToString(corrsalida);
                punteroaplicacion1.p[(int)numparametroscreados + 5].Value = D3;

                //INCREMENTAMOS EL NÚMERO DE CORRIENTES
                punteroaplicacion1.numcorrientes = punteroaplicacion1.numcorrientes + 2;
            }

            //Si hay corriente de entrada proveniente de otro equipo
            else if (checkBox1.Checked == true)
            {
                
                if ((D1 != 0) || (D2 != 0) || (D3 != 0))
                {
                    punteroaplicacion1.p[(int)numparametroscreados].Nombre = "W" + Convert.ToString(corrsalida);
                    punteroaplicacion1.p[(int)numparametroscreados].Value = D1;
                    punteroaplicacion1.p[(int)numparametroscreados + 1].Nombre = "P" + Convert.ToString(corrsalida);
                    punteroaplicacion1.p[(int)numparametroscreados + 1].Value = D2;
                    punteroaplicacion1.p[(int)numparametroscreados + 2].Nombre = "H" + Convert.ToString(corrsalida);
                    punteroaplicacion1.p[(int)numparametroscreados + 2].Value = D3;

                    //INCREMENTAMOS EL NÚMERO DE CORRIENTES
                    punteroaplicacion1.numcorrientes = punteroaplicacion1.numcorrientes + 1;
                }

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

            Parameter W1 = new Parameter();
            Parameter W2 = new Parameter();
            Parameter P1 = new Parameter();
            Parameter P2 = new Parameter();
            Parameter H1 = new Parameter();
            Parameter H2 = new Parameter();

            W1 = punteroaplicacion1.p.Find(p => p.Nombre == "W" + Convert.ToString(correntrada));
            W2 = punteroaplicacion1.p.Find(p => p.Nombre == "W" + Convert.ToString(corrsalida));
            P1 = punteroaplicacion1.p.Find(p => p.Nombre == "P" + Convert.ToString(correntrada));
            P2 = punteroaplicacion1.p.Find(p => p.Nombre == "P" + Convert.ToString(corrsalida));
            H1 = punteroaplicacion1.p.Find(p => p.Nombre == "H" + Convert.ToString(correntrada));
            H2 = punteroaplicacion1.p.Find(p => p.Nombre == "H" + Convert.ToString(corrsalida));

            //No hay corriente de entrada desde otro equipo, es el primer equipo del Sistema 
            if (checkBox1.Checked == false)
            {
                //Creamos los nombre de las variables de las ecuaicones generadas para enviarlas al motor de cálculo de Newton Raphson
                variables1.Add("");
                variables1[0] = "W" + Convert.ToString(correntrada);
                variables1.Add("");
                variables1[1] = "P" + Convert.ToString(correntrada);
                variables1.Add("");
                variables1[2] = "H" + Convert.ToString(correntrada);
                variables1.Add("");
                variables1[3] = "W" + Convert.ToString(corrsalida);
                variables1.Add("");
                variables1[4] = "P" + Convert.ToString(corrsalida);
                variables1.Add("");
                variables1[5] = "H" + Convert.ToString(corrsalida);

                if (D5 == 0)
                {
                    ecuaciones2.Add("");
                    ecuaciones2[auxiliar] = "W" + Convert.ToString(correntrada) + "-" + "W" + Convert.ToString(corrsalida);
                    Func<Double> primeraecuacion = () => W1 - W2;
                    punteroaplicacion1.functions.Add(primeraecuacion);
                    auxiliar++;
                }

                ecuaciones2.Add("");
                ecuaciones2[auxiliar] = "P" + Convert.ToString(correntrada) + "-" + "P" + Convert.ToString(corrsalida);
                Func<Double> segundaecuacion = () => P1 - P2;
                punteroaplicacion1.functions.Add(segundaecuacion);
                auxiliar++;

                ecuaciones2.Add("");
                ecuaciones2[auxiliar] = "H" + Convert.ToString(correntrada) + "-" + "H" + Convert.ToString(corrsalida);
                Func<Double> terceraecuacion = () => H1 - H2;
                punteroaplicacion1.functions.Add(terceraecuacion);
                auxiliar++;

                if (D1 > 0)
                {
                    ecuaciones2.Add("");
                    ecuaciones2[auxiliar] = "W" + Convert.ToString(correntrada) + "-" + Convert.ToString(D1);
                    Func<Double> cuartaecuacion = () => W1 - D1;
                    //punteroaplicacion1.tablas(0,7,2)
                    punteroaplicacion1.functions.Add(cuartaecuacion);
                    auxiliar++;
                }

                if (D2 > 0)
                {
                    ecuaciones2.Add("");
                    ecuaciones2[auxiliar] = "P" + Convert.ToString(correntrada) + "-" + Convert.ToString(D2);
                    Func<Double> quintaecuacion = () => P1 - D2;
                    punteroaplicacion1.functions.Add(quintaecuacion);
                    auxiliar++;
                }

                if (D3 > 0)
                {
                    ecuaciones2.Add("");
                    ecuaciones2[auxiliar] = "H" + Convert.ToString(correntrada) + "-" + Convert.ToString(D3);
                    Func<Double> sextaecuacion = () => H1 - D3;
                    punteroaplicacion1.functions.Add(sextaecuacion);
                    auxiliar++;
                }

                else if (D6 > 0 && D3 == 0)
                {

                    //Llamada a la función h(p=D6,x=D7)
                    ecuaciones2.Add("");
                    ecuaciones2[auxiliar] = "H" + Convert.ToString(correntrada) + "-" + "hpx(" + Convert.ToString(D6) + "," + Convert.ToString(D7) + ")";
                    Func<Double> sextaecuacion = () => H1 - punteroaplicacion1.acceso.hpx(D6, D7);
                    punteroaplicacion1.functions.Add(sextaecuacion);
                    auxiliar++;
                }

                else if (D6 < 0 && D3 == 0)
                {

                    //Llamada a la función h(T=-D6,x=D7)
                    ecuaciones2.Add("");
                    ecuaciones2[auxiliar] = "H" + Convert.ToString(correntrada) + "-" + "htx(" + Convert.ToString(-D6) + "," + Convert.ToString(D7) + ")";
                    Func<Double> sextaecuacion = () => H1 - punteroaplicacion1.acceso.htx(-D6, D7);
                    punteroaplicacion1.functions.Add(sextaecuacion);
                    auxiliar++;
                }

                numecuaciones2 = auxiliar;
                numvariables2 = 6;
            }

            //Corriente de entrada proveniente de otro equipo
            else if (checkBox1.Checked == true)
            {
                //Creamos los nombre de las variables de las ecuaicones generadas para enviarlas al motor de cálculo de Newton Raphson
                variables1.Add("");
                variables1[0] = "W" + Convert.ToString(corrsalida);
                variables1.Add("");
                variables1[1] = "P" + Convert.ToString(corrsalida);
                variables1.Add("");
                variables1[2] = "H" + Convert.ToString(corrsalida);
                variables1.Add("");

                ecuaciones2.Add("");
                ecuaciones2[auxiliar] = "W" + Convert.ToString(correntrada) + "-" + "W" + Convert.ToString(corrsalida);
                Func<Double> primeraecuacion = () => W1 - W2;
                //punteroaplicacion1.tablas(0,7,2)
                punteroaplicacion1.functions.Add(primeraecuacion);
                auxiliar++;

                ecuaciones2.Add("");
                ecuaciones2[auxiliar] = "P" + Convert.ToString(correntrada) + "-" + "P" + Convert.ToString(corrsalida);
                Func<Double> segundaecuacion = () => P1 - P2;
                punteroaplicacion1.functions.Add(segundaecuacion);
                auxiliar++;

                ecuaciones2.Add("");
                ecuaciones2[auxiliar] = "H" + Convert.ToString(correntrada) + "-" + "H" + Convert.ToString(corrsalida);
                Func<Double> terceraecuacion = () => H1 - H2;
                punteroaplicacion1.functions.Add(terceraecuacion);
                auxiliar++;

                //Caudal diferente de cero W1=D1
                if (D1>0)
                {
                    ecuaciones2.Add("");
                    ecuaciones2[auxiliar] = "W" + Convert.ToString(correntrada) + "-" + Convert.ToString(D1);
                    Func<Double> cuartaecuacion = () => W1 - D1;
                    //punteroaplicacion1.tablas(0,7,2)
                    punteroaplicacion1.functions.Add(cuartaecuacion);
                    auxiliar++;

                    numecuaciones2 = auxiliar;
                    numvariables2 = 3;
                }

                //Presión diferente de cero P1=D2
                if (D2 > 0)
                {
                    ecuaciones2.Add("");
                    ecuaciones2[auxiliar] = "P" + Convert.ToString(correntrada) + "-" + Convert.ToString(D2);
                    Func<Double> cuartaecuacion = () => P1 - D2;
                    //punteroaplicacion1.tablas(0,7,2)
                    punteroaplicacion1.functions.Add(cuartaecuacion);
                    auxiliar++;

                    numecuaciones2 = auxiliar;
                    numvariables2 = 3;
                }

                if (D3 > 0)
                {
                    ecuaciones2.Add("");
                    ecuaciones2[auxiliar] = "H" + Convert.ToString(correntrada) + "-" + Convert.ToString(D3);
                    Func<Double> cuartaecuacion = () => H1 - D3;
                    //punteroaplicacion1.tablas(0,7,2)
                    punteroaplicacion1.functions.Add(cuartaecuacion);
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
        private void button2_Click(object sender, EventArgs e)
        {
            int j;
            j = 0;

            //Declaramos y creamos un objeto de la clase Equipos para guardar los datos de entrada del usuario del nuevo equipo "condición de contorno" generado.
            Equipos condicioncontorno1 = new Equipos();


            //Incicializamos los miembros del objeto de la clase Equipos creado anteriormente.

            condicioncontorno1.numequipo2 = numequipo;

            condicioncontorno1.tipoequipo2 = 1;

            condicioncontorno1.aD1 = D1;
            condicioncontorno1.aD2 = D2;
            condicioncontorno1.aD3 = D3;
            condicioncontorno1.aD4 = 0;
            condicioncontorno1.aD5 = D5;
            condicioncontorno1.aD6 = D6;
            condicioncontorno1.aD7 = D7;
            condicioncontorno1.aD8 = 0;
            condicioncontorno1.aD9 = 0;

            condicioncontorno1.aN1 = correntrada;
            condicioncontorno1.aN2 = 0;
            condicioncontorno1.aN3 = corrsalida;
            condicioncontorno1.aN4 = 0;

            
            //Creamos las dos líneas del archivo de entrada de HBAL

            //Número de Equipo
            Double longitud= Convert.ToString(numequipo).Length;
            String temporal="";

            for (int hh = 0; hh < 10 - longitud; hh++)
            {
                temporal = temporal + " ";
            }

            String primerasubcadena = String.Concat(temporal, Convert.ToString(numequipo));
           //String primerasubcadena= temporal+Convert.ToString(numequipo);
            Int32 comprobacion = primerasubcadena.Length;


            //Tipo de Equipo
            Double longitud1 = 1;
            String temporal1 = "";

            for (int hh = 0; hh < 10 - longitud1; hh++)
            {
                temporal1 = temporal1 + " ";
            }

            String segundasubcadena = String.Concat(temporal1,"1");
            //String segundasubcadena = temporal1 +"1";
            Int32 comprobacion1 = segundasubcadena.Length;


            //Corriente 1
            Double longitud2 = Convert.ToString(correntrada).Length;
            String temporal2 = "";

            for (int hh = 0; hh < 10 - longitud2; hh++)
            {
                temporal2 = temporal2 + " ";
            }

            String tercerasubcadena = String.Concat(temporal2, Convert.ToString(correntrada));
            //String tercerasubcadena = temporal2 + Convert.ToString(correntrada);
            Int32 comprobacion2 = tercerasubcadena.Length;


            //Corriente 2
            Double longitud3 = 1;
            String temporal3 = "";

            for (int hh = 0; hh < 10 - longitud3; hh++)
            {
                temporal3 = temporal3 + " ";
            }

            String cuartasubcadena = String.Concat(temporal3,"0");
            //String cuartasubcadena = temporal3 + Convert.ToString(0);
            Int32 comprobacion3 = cuartasubcadena.Length;


            //Corriente 3
            Double longitud4 = Convert.ToString(corrsalida).Length;
            String temporal4 = "";

            for (int hh = 0; hh < 10 - longitud4; hh++)
            {
                temporal4 = temporal4 + " ";
            }

            String quintasubcadena = String.Concat(temporal4, Convert.ToString(corrsalida));
            //String quintasubcadena = temporal4 + Convert.ToString(corrsalida);
            Int32 comprobacion4 = quintasubcadena.Length;


            //Corriente 4
            Double longitud5 = 1;
            String temporal5 = "";

            for (int hh = 0; hh < 10 - longitud5; hh++)
            {
                temporal5 = temporal5 + " ";
            }

            String sextasubcadena = String.Concat(temporal5,0);
            //String sextasubcadena = temporal5 + Convert.ToString(0);
            Int32 comprobacion5 = sextasubcadena.Length;

            String septimasubcadena = "    0";

            lineaprimera = String.Concat(primerasubcadena,segundasubcadena,tercerasubcadena,cuartasubcadena,quintasubcadena,sextasubcadena,septimasubcadena);


            //Igual realizaremos la entrada de parameteros D1 hasta D9, teniendo en cuenta que cada uno de los campos ocupa en el archivo 8 caracteres en lugar de 10 caracteres por dato como ocupaba la linea anterior del archivo 
            //Codigo para escribir la "lineasegunda" para el envio al archivo de entrada de HBAL incluyendo los parámetros D1 al D9 del equipo condición de contorno generado.

            //D1
            Double longitud6 = Convert.ToString(D1).Length;
            String temporal6 = "";

            for (int hh = 0; hh < 8 - longitud6; hh++)
            {
                temporal6 = temporal6 + " ";
            }

            String octavasubcadena = String.Concat(Convert.ToString(D1),temporal6);
            //String primerasubcadena= temporal+Convert.ToString(numequipo);
            Int32 comprobacion6 = octavasubcadena.Length;

            //D2
            Double longitud7 = Convert.ToString(D2).Length;
            String temporal7 = "";

            for (int hh = 0; hh < 8 - longitud7; hh++)
            {
                temporal7 = temporal7 + " ";
            }

            String novenasubcadena = String.Concat(Convert.ToString(D2),temporal7);
            //String primerasubcadena= temporal+Convert.ToString(numequipo);
            Int32 comprobacion7 = novenasubcadena.Length;

            //D3
            Double longitud8 = Convert.ToString(D3).Length;
            String temporal8 = "";

            for (int hh = 0; hh < 8 - longitud8; hh++)
            {
                temporal8 = temporal8 + " ";
            }

            String decimasubcadena = String.Concat(Convert.ToString(D3),temporal8);
            //String primerasubcadena= temporal+Convert.ToString(numequipo);
            Int32 comprobacion8 = decimasubcadena.Length;


            //D4
            Double longitud9 = Convert.ToString("0.").Length;
            String temporal9 = "";

            for (int hh = 0; hh < 8 - longitud9; hh++)
            {
                temporal9 = temporal9 + " ";
            }

            String undecimasubcadena = String.Concat(Convert.ToString("0."),temporal9);
            //String primerasubcadena= temporal+Convert.ToString(numequipo);
            Int32 comprobacion9 = undecimasubcadena.Length;

            //D5
            Double longitud10 = Convert.ToString(D5).Length;
            String temporal10 = "";

            for (int hh = 0; hh < 8 - longitud10; hh++)
            {
                temporal10 = temporal10 + " ";
            }

            String duodecimasubcadena = String.Concat(Convert.ToString(D5),temporal10);
            //String primerasubcadena= temporal+Convert.ToString(numequipo);
            Int32 comprobacion10 = duodecimasubcadena.Length;

            //D6
            Double longitud11 = Convert.ToString(D6).Length;
            String temporal11 = "";

            for (int hh = 0; hh < 8 - longitud11; hh++)
            {
                temporal11 = temporal11 + " ";
            }

            String trecesubcadena = String.Concat(Convert.ToString(D6),temporal11);
            //String primerasubcadena= temporal+Convert.ToString(numequipo);
            Int32 comprobacion11 = trecesubcadena.Length;

            //D7
            Double longitud12 = Convert.ToString(D7).Length;
            String temporal12 = "";

            for (int hh = 0; hh < 8 - longitud12; hh++)
            {
                temporal12 = temporal12 + " ";
            }

            String catorcesubcadena = String.Concat(Convert.ToString(D7),temporal12);
            //String primerasubcadena= temporal+Convert.ToString(numequipo);
            Int32 comprobacion12 = catorcesubcadena.Length;

            //D8
            Double longitud13 = Convert.ToString("0.").Length;
            String temporal13 = "";

            for (int hh = 0; hh < 8 - longitud13; hh++)
            {
                temporal13 = temporal13 + " ";
            }

            String quincesubcadena = String.Concat(Convert.ToString("0."),temporal13);
            //String primerasubcadena= temporal+Convert.ToString(numequipo);
            Int32 comprobacion13 = quincesubcadena.Length;

            //D9
            Double longitud14 = Convert.ToString("0.").Length;
            String temporal14 = "";

            for (int hh = 0; hh < 8 - longitud14; hh++)
            {
                temporal14 = temporal14 + " ";
            }

            String dieciseissubcadena = String.Concat(Convert.ToString("0."),temporal14);
            //String primerasubcadena= temporal+Convert.ToString(numequipo);
            Int32 comprobacion14 = dieciseissubcadena.Length;

            lineasegunda = String.Concat(octavasubcadena, novenasubcadena, decimasubcadena, undecimasubcadena, duodecimasubcadena, trecesubcadena, catorcesubcadena, quincesubcadena,dieciseissubcadena);


            for (int i = (int)punteroaplicacion1.numecuaciones; i < numecuaciones2 + (int)punteroaplicacion1.numecuaciones; i++)
            {
                    punteroaplicacion1.ecuaciones.Add("");
                    punteroaplicacion1.ecuaciones[i] = ecuaciones1[j];
                    j++;
            }

            int f;
            f = 0;

            for (int i = (int)punteroaplicacion1.numvariables; i < numvariables2 + (int)punteroaplicacion1.numvariables; i++)
            {
                punteroaplicacion1.variables.Add("");
                punteroaplicacion1.variables[i] = variables1[f];
                f++;
            }
            
            //Enviamos a la aplicación principal la ecuaciones generadas (array de cadenas string)
            punteroaplicacion1.numecuaciones = numecuaciones2+punteroaplicacion1.numecuaciones;

            //Enviamos a la aplicación principal los nombres de las variables que integran las ecuaciones generadas
            punteroaplicacion1.numvariables = numvariables2+punteroaplicacion1.numvariables;

            //Enviamos a la aplicación principal los datos de entrada del equipo generado.
            punteroaplicacion1.equipos11.Add(condicioncontorno1);

            //Enviamos las dos líneas del archivo HBAL de entrada de datos generada por el equipo Condición de Contorno que hemos creado
            //punteroaplicacion1.Hbalfile.Add("");
            //punteroaplicacion1.Hbalfile[(punteroaplicacion1.numequipos - 1)] = lineaprimera;
            //punteroaplicacion1.Hbalfile.Add("");
            //punteroaplicacion1.Hbalfile[(punteroaplicacion1.numequipos)] = lineasegunda;
                                   
            this.Hide();
        }


        //Botón Cancel
        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

       
    }
}
