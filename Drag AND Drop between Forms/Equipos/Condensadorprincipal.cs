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
    public partial class Condensadorprincipal : Form
    {
        Double D1, D2, D3,D4,D5,D6,D7,D8,D9,Q,Pv;
        Double correntrada1,correntrada2, corrsalida;
        Double numequipo;

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

            numparametroscreados = (punteroaplicacion1.numcorrientes) * 3;

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
            Q = 0;
            Pv = 0;


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
            Pv=Convert.ToDouble(textBox14.Text);
            Q=Convert.ToDouble(textBox15.Text);

            correntrada1=Convert.ToDouble(textBox7.Text);
            correntrada2 = Convert.ToDouble(textBox10.Text);
            corrsalida = Convert.ToDouble(textBox8.Text);

            numequipo = Convert.ToDouble(textBox9.Text);

            //Conversión UNIDADES
            //Como las Tablas de Vapor de Agua ASME 1967 están en unidades británicas, siempre tenemos que convertir los datos de entrada a Unidades Británicas

            //Unidades Métricas
            if (punteroaplicacion1.unidades == 2)
            {
                //Presión Bar a psia
                Pv = Pv / (6.8947572 / 100);
            }

            //Unidades Sistema Internacional
            else if (punteroaplicacion1.unidades == 1)
            {
                //Presión kPa a psia
                Pv = Pv / (6.894757);
            }

            //Unidades Sistema Británico
            else if (punteroaplicacion1.unidades == 0)
            {

            }
            else
            {

            }

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

            ecuaciones1=generaecucaiones(D1,D2,D3,D4,D5,D6,D7,D8,D9,Q,Pv,correntrada1,correntrada2,corrsalida);

            listBox1.Items.Add("Nº Equipo:" + Convert.ToString(numequipo));

            for (int numecua = 0; numecua < 4; numecua++)
            {
                listBox1.Items.Add(ecuaciones1[numecua]);
            }

            listBox1.Items.Add("");

        }

        private List<String> generaecucaiones(Double D1, Double D2,Double D3,Double D4, Double D5, Double D6, Double D7,Double D8,Double D9,Double Q,Double Pv, Double correntrada1,Double correntrada2, Double corrsalida)
        {
            //Lista de cadenas que guardan las ecuaciones del sistema
            List<String> ecuaciones2 = new List<String>();
           
            if (checkBox1.Checked == true)
            {
                textBox10.Enabled = true;

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
                ecuaciones2[auxiliar] = "W" + Convert.ToString(correntrada1) + "+" + "W" + Convert.ToString(correntrada2) + "-" + "W" + Convert.ToString(corrsalida);
                Func<Double> primeraecuacion = () => W1 + W2 - W3;
                punteroaplicacion1.functions.Add(primeraecuacion);
                auxiliar++;


                ecuaciones2.Add("");
                ecuaciones2[auxiliar] = "P" + Convert.ToString(correntrada1) + "-" + "P" + Convert.ToString(corrsalida);
                Func<Double> segundaecuacion = () => P1 - P3;
                punteroaplicacion1.functions.Add(segundaecuacion);
                auxiliar++;

                String Q1;
                Q1 = "(" + "W" + Convert.ToString(correntrada1) + "*" + "H" + Convert.ToString(correntrada1) + ")" + "+" + "(" + "W" + Convert.ToString(correntrada2) + "*" + "H" + Convert.ToString(correntrada2) + ")" + "-" + "(" + "W" + Convert.ToString(corrsalida) + "*" + "H" + Convert.ToString(corrsalida) + ")";

                String tempPv;
                tempPv = Convert.ToString(D1) + "+" + "(" + Convert.ToString(D2) + "*" + Q1 + ")" + "+" + "(" + Convert.ToString(D3) + "*" + "(" + Q1 + ")" + "*" + "(" + Q1 + ")" + ")";

                if (Pv > 0)
                {
                    ecuaciones2.Add("");
                    ecuaciones2[auxiliar] = "P" + Convert.ToString(correntrada1) + "-" + "(" + tempPv + ")";
                    Func<Double> terceraecuacion = () => P1 - Pv;
                    punteroaplicacion1.functions.Add(terceraecuacion);
                    auxiliar++;
                }
                else if ((Pv == 0) && (Q != 0))
                {
                    ecuaciones2.Add("");
                    ecuaciones2[auxiliar] = "P" + Convert.ToString(correntrada1) + "-" + "(" + tempPv + ")";
                    Q = (W1 * H1) + (W2 * H2) - (W3 * H3);
                    Func<Double> terceraecuacion = () => P1 - (D1 + D2 * Q + D3 * Q * Q);
                    punteroaplicacion1.functions.Add(terceraecuacion);
                    auxiliar++;
                }

                ecuaciones2.Add("");
                ecuaciones2[auxiliar] = "H" + Convert.ToString(corrsalida) + "-" + "hsatp(P" + Convert.ToString(correntrada1) + ")";
                Func<Double> cuartaecuacion = () => H3 - punteroaplicacion1.acceso.hsatpliq(P1);
                punteroaplicacion1.functions.Add(cuartaecuacion);
                auxiliar++;


            }

            else if (checkBox1.Checked == false)
            {
                punteroaplicacion1.p[(int)numparametroscreados].Nombre = "W" + Convert.ToString(corrsalida);
                punteroaplicacion1.p[(int)numparametroscreados + 1].Nombre = "P" + Convert.ToString(corrsalida);
                punteroaplicacion1.p[(int)numparametroscreados + 2].Nombre = "H" + Convert.ToString(corrsalida);

                Parameter W1 = new Parameter();
                Parameter W2 = new Parameter();
                Parameter P1 = new Parameter();
                Parameter P2 = new Parameter();
                Parameter H1 = new Parameter();
                Parameter H2 = new Parameter();
                
                W1 = punteroaplicacion1.p.Find(p => p.Nombre == "W" + Convert.ToString(correntrada1));
                W2 = punteroaplicacion1.p.Find(p => p.Nombre == "W" + Convert.ToString(corrsalida));
                P1 = punteroaplicacion1.p.Find(p => p.Nombre == "P" + Convert.ToString(correntrada1));
                P2 = punteroaplicacion1.p.Find(p => p.Nombre == "P" + Convert.ToString(corrsalida));
                H1 = punteroaplicacion1.p.Find(p => p.Nombre == "H" + Convert.ToString(correntrada1));
                H2 = punteroaplicacion1.p.Find(p => p.Nombre == "H" + Convert.ToString(corrsalida));

                ecuaciones2.Add("");
                ecuaciones2[auxiliar] = "W" + Convert.ToString(correntrada1) +"-" + "W" + Convert.ToString(corrsalida);
                Func<Double> primeraecuacion = () => W1- W2;
                punteroaplicacion1.functions.Add(primeraecuacion);
                auxiliar++;


                ecuaciones2.Add("");
                ecuaciones2[auxiliar] = "P" + Convert.ToString(correntrada1) + "-" + "P" + Convert.ToString(corrsalida);
                Func<Double> segundaecuacion = () => P1 - P2;
                punteroaplicacion1.functions.Add(segundaecuacion);
                auxiliar++;

                String Q1;
                Q1 = "(" + "W" + Convert.ToString(correntrada1) + "*" + "H" + Convert.ToString(correntrada1) + ")" + "+" + "(" + "W" + Convert.ToString(correntrada2) + "*" + "H" + Convert.ToString(correntrada2) + ")" + "-" + "(" + "W" + Convert.ToString(corrsalida) + "*" + "H" + Convert.ToString(corrsalida) + ")";

                String tempPv;
                tempPv = Convert.ToString(D1) + "+" + "(" + Convert.ToString(D2) + "*" + Q1 + ")" + "+" + "(" + Convert.ToString(D3) + "*" + "(" + Q1 + ")" + "*" + "(" + Q1 + ")" + ")";

                if (Pv > 0)
                {
                    ecuaciones2.Add("");
                    ecuaciones2[auxiliar] = "P" + Convert.ToString(correntrada1) + "-" + "(" + tempPv + ")";
                    Func<Double> terceraecuacion = () => P1 - Pv;
                    punteroaplicacion1.functions.Add(terceraecuacion);
                    auxiliar++;
                }
                else if ((Pv == 0) && (Q != 0))
                {
                    ecuaciones2.Add("");
                    ecuaciones2[auxiliar] = "P" + Convert.ToString(correntrada1) + "-" + "(" + tempPv + ")";
                    Q = (W1 * H1) - (W2 * H2);
                    Func<Double> terceraecuacion = () => P1 - (D1 + D2 * Q + D3 * Q * Q);
                    punteroaplicacion1.functions.Add(terceraecuacion);
                    auxiliar++;
                }

                ecuaciones2.Add("");
                ecuaciones2[auxiliar] = "H" + Convert.ToString(corrsalida) + "-" + "hsatp(P" + Convert.ToString(correntrada1) + ")";
                Func<Double> cuartaecuacion = () => H2 - punteroaplicacion1.acceso.hsatpliq(P1);
                punteroaplicacion1.functions.Add(cuartaecuacion);
                auxiliar++;
            }

            variables1.Add("");
            variables1[0] = "W" + Convert.ToString(corrsalida);
            variables1.Add("");
            variables1[1] = "P" + Convert.ToString(corrsalida);
            variables1.Add("");
            variables1[2] = "H" + Convert.ToString(corrsalida);

            numecuaciones2 = auxiliar;
            numvariables2 = 3;
           
            return (ecuaciones2);
        }


        //Botón OK
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


        //Botón Cancel
        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

       
    }
}
