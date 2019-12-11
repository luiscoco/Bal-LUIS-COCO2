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
    public partial class MSR : Form
    {
        Double D1, D2, D3,D4, D5, D6, D7,D8,D9,X4;
        Double correntrada1,correntrada2, corrsalida1,corrsalida2;
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

        public MSR(Aplicacion punteroaplicion,Double numecuaciones1,Double numvariables1)
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
            X4 = 0;

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
            X4 = Convert.ToDouble(textBox15.Text);

            correntrada1=Convert.ToDouble(textBox10.Text);
            correntrada2 = Convert.ToDouble(textBox11.Text);
            corrsalida1 = Convert.ToDouble(textBox12.Text);
            corrsalida2 = Convert.ToDouble(textBox13.Text);

            numequipo = Convert.ToDouble(textBox9.Text);

            //Conversión UNIDADES
            //Como las Tablas de Vapor de Agua ASME 1967 están en unidades británicas, siempre tenemos que convertir los datos de entrada a Unidades Británicas

            //CONVERSION DE UNIDADES DEL TTD
            //Unidades Métricas
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

            //Unidades Sistema Internacional
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

            Random random = new Random();

            //CREAMOS EL ARRAY DE PARAMETROS
            for (int v = (int)numparametroscreados; v < ((numparametroscreados) + 6); v++)
            {
                //int randomNumber = random.Next(0, 2500);
                //Creamos la lista de parámetros generadas por este programa
                punteroaplicacion1.p.Add(punteroaplicacion1.ptemp);
                punteroaplicacion1.p[v] = new Parameter(10, 0.01, "");
            }

            //INCREMENTAMOS EL NÚMERO DE CORRIENTES
            punteroaplicacion1.numcorrientes = punteroaplicacion1.numcorrientes + 2;

            punteroaplicacion1.p[(int)numparametroscreados].Nombre = "W" + Convert.ToString(corrsalida1);
            punteroaplicacion1.p[(int)numparametroscreados].Value=punteroaplicacion1.p.Find(p => p.Nombre == "W" + Convert.ToString(correntrada1)).Value;
            punteroaplicacion1.p[(int)numparametroscreados + 1].Nombre = "W" + Convert.ToString(corrsalida2);
            //punteroaplicacion1.p[(int)numparametroscreados + 1].Value = 0.1*punteroaplicacion1.p.Find(p => p.Nombre == "W" + Convert.ToString(correntrada1)).Value;
            punteroaplicacion1.p[(int)numparametroscreados + 1].Value = punteroaplicacion1.p.Find(p => p.Nombre == "W" + Convert.ToString(correntrada2)).Value;
            punteroaplicacion1.p[(int)numparametroscreados + 2].Nombre = "P" + Convert.ToString(corrsalida1);
            punteroaplicacion1.p[(int)numparametroscreados + 2].Value = punteroaplicacion1.p.Find(p => p.Nombre == "P" + Convert.ToString(correntrada1)).Value;
            punteroaplicacion1.p[(int)numparametroscreados + 3].Nombre = "P" + Convert.ToString(corrsalida2);
            punteroaplicacion1.p[(int)numparametroscreados + 3].Value = punteroaplicacion1.p.Find(p => p.Nombre == "P" + Convert.ToString(correntrada2)).Value;
            punteroaplicacion1.p[(int)numparametroscreados + 4].Nombre = "H" + Convert.ToString(corrsalida1);
            //punteroaplicacion1.p[(int)numparametroscreados + 4].Value = 0.5 * punteroaplicacion1.p.Find(p => p.Nombre == "H" + Convert.ToString(correntrada1)).Value;
            punteroaplicacion1.p[(int)numparametroscreados + 5].Nombre = "H" + Convert.ToString(corrsalida2);
            //punteroaplicacion1.p[(int)numparametroscreados + 5].Value = 0.5 * punteroaplicacion1.p.Find(p => p.Nombre == "H" + Convert.ToString(correntrada2)).Value;

            ecuaciones1=generaecucaiones(D1,D2,D3,D4,D5,D6,D7,D8,D9,X4,correntrada1,correntrada2,corrsalida1,corrsalida2);

            listBox1.Items.Add("Nº Equipo:" + Convert.ToString(numequipo));

            for (int numecua = 0; numecua < auxiliar; numecua++)
            {
                listBox1.Items.Add(ecuaciones1[numecua]);
            }
            listBox1.Items.Add("");
        }

        private List<String> generaecucaiones(Double D1, Double D2,Double D3,Double D4, Double D5, Double D6, Double D7,Double D8, Double D9,Double X4, Double correntrada1,Double correntrada2, Double corrsalida1,Double corrsalida2)
        {
            //Lista de cadenas que guardan las ecuaciones del sistema
            List<String> ecuaciones2 = new List<String>();

            Parameter W1 = new Parameter();
            Parameter W2 = new Parameter();
            Parameter W3 = new Parameter();
            Parameter W4 = new Parameter();

            Parameter P1 = new Parameter();
            Parameter P2 = new Parameter();
            Parameter P3 = new Parameter();
            Parameter P4 = new Parameter();

            Parameter H1 = new Parameter();
            Parameter H2 = new Parameter();
            Parameter H3 = new Parameter();
            Parameter H4 = new Parameter();


            W1 = punteroaplicacion1.p.Find(p => p.Nombre == "W" + Convert.ToString(correntrada1));
            W2 = punteroaplicacion1.p.Find(p => p.Nombre == "W" + Convert.ToString(correntrada2));
            W3 = punteroaplicacion1.p.Find(p => p.Nombre == "W" + Convert.ToString(corrsalida1));
            W4 = punteroaplicacion1.p.Find(p => p.Nombre == "W" + Convert.ToString(corrsalida2));

            P1 = punteroaplicacion1.p.Find(p => p.Nombre == "P" + Convert.ToString(correntrada1));
            P2 = punteroaplicacion1.p.Find(p => p.Nombre == "P" + Convert.ToString(correntrada2));
            P3 = punteroaplicacion1.p.Find(p => p.Nombre == "P" + Convert.ToString(corrsalida1));
            P4 = punteroaplicacion1.p.Find(p => p.Nombre == "P" + Convert.ToString(corrsalida2));

            H1 = punteroaplicacion1.p.Find(p => p.Nombre == "H" + Convert.ToString(correntrada1));
            H2 = punteroaplicacion1.p.Find(p => p.Nombre == "H" + Convert.ToString(correntrada2));
            H3 = punteroaplicacion1.p.Find(p => p.Nombre == "H" + Convert.ToString(corrsalida1));
            H4 = punteroaplicacion1.p.Find(p => p.Nombre == "H" + Convert.ToString(corrsalida2));

           
            ecuaciones2.Add("");
            ecuaciones2[auxiliar] = "W" + Convert.ToString(correntrada1) + "-" + "W" + Convert.ToString(corrsalida1);
            Func<Double> primeraecuacion = () => W1 - W3;
            punteroaplicacion1.functions.Add(primeraecuacion);
            auxiliar++;

            ecuaciones2.Add("");
            ecuaciones2[auxiliar] = "W" + Convert.ToString(correntrada2) + "-" + "W" + Convert.ToString(corrsalida2);
            Func<Double> segundaecuacion = () => W2 - W4;
            punteroaplicacion1.functions.Add(segundaecuacion);
            auxiliar++;

            ecuaciones2.Add("");
            ecuaciones2[auxiliar] = "P" + Convert.ToString(correntrada1) + "-" + "P" + Convert.ToString(corrsalida1) + "-" + Convert.ToString(D1) + "-" + Convert.ToString(D2) + "*" + "W" + Convert.ToString(correntrada1) + "-" + Convert.ToString(D3) + "*" + "W" + Convert.ToString(correntrada1) + "*" + "W" + Convert.ToString(correntrada1) + "-" + Convert.ToString(D4) + "*" + "P" + Convert.ToString(correntrada1);
            Func<Double> terceraecuacion = () => P1-P3-D1-(D2*W1)-(D3*W1*W1)-(D4*P1);
            punteroaplicacion1.functions.Add(terceraecuacion);
            auxiliar++;

            ecuaciones2.Add("");
            ecuaciones2[auxiliar] = "W" + Convert.ToString(correntrada1) + "-" + "W" + Convert.ToString(corrsalida1);
            Func<Double> cuartaecuacion = () => P2-P4-D7-(D8*W2*W2)-(D9*P2);
            punteroaplicacion1.functions.Add(cuartaecuacion);
            auxiliar++;

            ecuaciones2.Add("");
            ecuaciones2[auxiliar] = "W" + Convert.ToString(correntrada1) + "-" + "W" + Convert.ToString(corrsalida1);
            Func<Double> quintaecuacion = () => W1*(H3-H1)-W2*(H2-H4)*D6;
            punteroaplicacion1.functions.Add(quintaecuacion);
            auxiliar++;

            ecuaciones2.Add("");
            ecuaciones2[auxiliar] = "W" + Convert.ToString(correntrada1) + "-" + "W" + Convert.ToString(corrsalida1);
            Func<Double> sextaecuacion = () => H4-punteroaplicacion1.acceso.hpx(P4,X4);
            punteroaplicacion1.functions.Add(sextaecuacion);
            auxiliar++;

            ecuaciones2.Add("");
            ecuaciones2[auxiliar] = "W" + Convert.ToString(correntrada1) + "-" + "W" + Convert.ToString(corrsalida1);
            Func<Double> septimaecuacion = () => H3 - punteroaplicacion1.acceso.hpt(P3, (punteroaplicacion1.acceso.tph(P2, H2) - D5));
            punteroaplicacion1.functions.Add(septimaecuacion);
            auxiliar++;

            variables1.Add("");
            variables1[0] = "W" + Convert.ToString(corrsalida1);
            variables1.Add("");
            variables1[1] = "P" + Convert.ToString(corrsalida1);
            variables1.Add("");
            variables1[2] = "H" + Convert.ToString(corrsalida1);

            variables1.Add("");
            variables1[3] = "W" + Convert.ToString(corrsalida2);
            variables1.Add("");
            variables1[4] = "P" + Convert.ToString(corrsalida2);
            variables1.Add("");
            variables1[5] = "H" + Convert.ToString(corrsalida2);

            numecuaciones2 = auxiliar;
            numvariables2 = 6;
           
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
