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

namespace Drag_AND_Drop_between_Forms
{
    public partial class Mezclador : Form
    {
        Double D1,D9;
        Double correntrada1, correntrada2,corrsalida;
        Double numequipo;

        Double numecuaciones2;
        Double numvariables2;

        //Lista de cadenas para gardar los nombres de las variables del sistema de ecuaciones
        List<String> variables1 = new List<String>();

        //Lista de cadenas que guardan las ecuaciones del sistema
        List<String> ecuaciones1 = new List<String>();

        Aplicacion punteroaplicacion1;

        Double numparametroscreados = 0;

        public Mezclador(Aplicacion punteroaplicion,Double numecuaciones1,Double numvariables1)
        {
            InitializeComponent();           

            punteroaplicacion1 = punteroaplicion;
            numparametroscreados = (punteroaplicacion1.numcorrientes) * 3;


            //Inicializamos las etiquetas de las unidades de entrada en el cuadro de dialogo de toma de datos
            if (punteroaplicacion1.unidades == 0)
            {

            }
            else if (punteroaplicacion1.unidades == 1)
            {
                label17.Text = "kPa";

            }
            else if (punteroaplicacion1.unidades == 2)
            {
                label17.Text = "Bar";


            }
            else
            {

            }

            D1 = 0;
            D9 = 0;
                        
            correntrada1 = 0;
            correntrada2 = 0;
            corrsalida = 0;

            numequipo = 0;

            numecuaciones2 = numecuaciones1;

            numvariables2 = numvariables1;
        
        }

        private void Condcontorno_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random random = new Random();

            //CREAMOS EL ARRAY DE PARAMETROS
            for (int v = (int)numparametroscreados; v < ((numparametroscreados) + 3); v++)
            {
                int randomNumber = random.Next(0, 2500);

                //Creamos la lista de parámetros generadas por este programa
                punteroaplicacion1.p.Add(punteroaplicacion1.ptemp);
                punteroaplicacion1.p[v] = new Parameter(randomNumber, 0.01, "");
            }
            //INCREMENTAMOS EL NÚMERO DE CORRIENTES
            punteroaplicacion1.numcorrientes = punteroaplicacion1.numcorrientes + 1;

            D1=Convert.ToDouble(textBox1.Text);
            D9=Convert.ToDouble(textBox2.Text);
                     

            correntrada1=Convert.ToDouble(textBox3.Text);
            correntrada2 = Convert.ToDouble(textBox4.Text);
            corrsalida = Convert.ToDouble(textBox5.Text);


            numequipo = Convert.ToDouble(textBox9.Text);

            //Conversión UNIDADES
            //Como las Tablas de Vapor de Agua ASME 1967 están en unidades británicas, siempre tenemos que convertir los datos de entrada a Unidades Británicas

            //Unidades Métricas
            if (punteroaplicacion1.unidades == 2)
            {
                //Presión Bar a psia
                D1 = D1 / (6.8947572 / 100);
            }

            //Unidades Sistema Internacional
            else if (punteroaplicacion1.unidades == 1)
            {
                //Presión kPa a psia
                D1 = D1 / (6.894757);
            }

            //Unidades Sistema Británico
            else if (punteroaplicacion1.unidades == 0)
            {

            }
            else
            {

            }


            ecuaciones1=generaecucaiones(D1,D9,correntrada1,correntrada2,corrsalida);

            listBox1.Items.Add("Nº Equipo:" + Convert.ToString(numequipo));

            for (int numecua = 0; numecua < 3; numecua++)
            {
                listBox1.Items.Add(ecuaciones1[numecua]);
            }

            listBox1.Items.Add("");

            button2.Enabled = true;

        }

        private List<String> generaecucaiones(Double D1, Double D9,Double correntrada1, Double correntrada2,Double corrsalida)
        {
            //Lista de cadenas que guardan las ecuaciones del sistema
            List<String> ecuaciones2 = new List<String>();

            //Creamos las variables
            punteroaplicacion1.p[(int)numparametroscreados].Nombre = "W" + Convert.ToString(corrsalida);

            punteroaplicacion1.p[(int)numparametroscreados + 1].Nombre = "P" + Convert.ToString(corrsalida);

            punteroaplicacion1.p[(int)numparametroscreados + 2].Nombre = "H" + Convert.ToString(corrsalida);


            Parameter W1 = new Parameter();
            Parameter W2 = new Parameter();
            Parameter W3 = new Parameter();
            Parameter P1 = new Parameter();
            Parameter P2 = new Parameter();
            Parameter P3 = new Parameter();
            Parameter H1 = new Parameter();
            Parameter H2 = new Parameter();
            Parameter H3 = new Parameter();

            W1 = punteroaplicacion1.p.Find(p => p.Nombre == "W" + Convert.ToString(correntrada1));
            W2 = punteroaplicacion1.p.Find(p => p.Nombre == "W" + Convert.ToString(correntrada2));
            W3 = punteroaplicacion1.p.Find(p => p.Nombre == "W" + Convert.ToString(corrsalida));
            P1 = punteroaplicacion1.p.Find(p => p.Nombre == "P" + Convert.ToString(correntrada1));
            P2 = punteroaplicacion1.p.Find(p => p.Nombre == "P" + Convert.ToString(correntrada2));
            P3 = punteroaplicacion1.p.Find(p => p.Nombre == "P" + Convert.ToString(corrsalida));
            H1 = punteroaplicacion1.p.Find(p => p.Nombre == "H" + Convert.ToString(correntrada1));
            H2 = punteroaplicacion1.p.Find(p => p.Nombre == "H" + Convert.ToString(correntrada2));
            H3 = punteroaplicacion1.p.Find(p => p.Nombre == "H" + Convert.ToString(corrsalida));

            ecuaciones2.Add("");
            ecuaciones2[0] = "W" + Convert.ToString(corrsalida) + "-" + "W" + Convert.ToString(correntrada1) + "-" + "W" + Convert.ToString(correntrada2);
            Func<Double> primeraecuacion = () => W3 - W1- W2;
            punteroaplicacion1.functions.Add(primeraecuacion);
            
            ecuaciones2.Add("");
            ecuaciones2[1] = "H" + Convert.ToString(corrsalida) + "*" + "(" + "W" + Convert.ToString(correntrada1) + "+" + "W" + Convert.ToString(correntrada2) + ")" + "-" + "W" + Convert.ToString(correntrada1) + "*" + "H" + Convert.ToString(correntrada1) + "-" + "W" + Convert.ToString(correntrada1) + "*" + "H" + Convert.ToString(correntrada2);
            Func<Double> segundaecuacion = () => (H3*(W1+W2))-(W1*H1)-(W2*H2);
            punteroaplicacion1.functions.Add(segundaecuacion);


            if (D1>0)
            {
                ecuaciones2.Add("");
                ecuaciones2[2] = "P" + Convert.ToString(corrsalida) + "-" + Convert.ToString(D1);
                Func<Double> terceraecuacion = () => P3 - D1;
                punteroaplicacion1.functions.Add(terceraecuacion);
            }
            else if (D1 == 0)
            {
                ecuaciones2.Add("");
                ecuaciones2[2] = "P" + Convert.ToString(corrsalida) + "-" + "P" + Convert.ToString(correntrada1);
                Func<Double> terceraecuacion = () => P3 - P1;
                punteroaplicacion1.functions.Add(terceraecuacion);
            }
            else
            {

            }

            if (D9 == 1)
            {
                ecuaciones2.Add("");
                ecuaciones2[3] = "P" + Convert.ToString(correntrada2) + "-" + "P" + Convert.ToString(correntrada1);
                Func<Double> cuartaecuacion = () => P2 - P1;
                punteroaplicacion1.functions.Add(cuartaecuacion);


                numecuaciones2 = 4;
                numvariables2 = 4;

                variables1.Add("");
                variables1[0] = "W" + Convert.ToString(corrsalida);
                variables1.Add("");
                variables1[1] = "P" + Convert.ToString(corrsalida);
                variables1.Add("");
                variables1[2] = "H" + Convert.ToString(corrsalida);
                variables1.Add("");
                variables1[2] = "P" + Convert.ToString(correntrada2);
            }

            else if (D9 != 1)
            {
                numecuaciones2 = 3;
                numvariables2 = 3;

                variables1.Add("");
                variables1[0] = "W" + Convert.ToString(corrsalida);
                variables1.Add("");
                variables1[1] = "P" + Convert.ToString(corrsalida);
                variables1.Add("");
                variables1[2] = "H" + Convert.ToString(corrsalida);
            }

            else
            {

            }           
           
            return (ecuaciones2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int j;
            j = 0;

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
            
            punteroaplicacion1.numecuaciones = numecuaciones2+punteroaplicacion1.numecuaciones;
            punteroaplicacion1.numvariables = numvariables2+punteroaplicacion1.numvariables;
                                   
            this.Hide();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
