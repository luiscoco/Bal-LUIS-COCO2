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
    public partial class Sephumedad : Form
    {
        Double D1, D2,entalpiasalida;
        Double correntrada, corrsalida1,corrsalida2;
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

        public Sephumedad(Aplicacion punteroaplicion,Double numecuaciones1,Double numvariables1)
        {
            InitializeComponent();           

            punteroaplicacion1 = punteroaplicion;
            numparametroscreados = (punteroaplicacion1.numcorrientes) * 3;

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
            Random random = new Random();

            for (int v = (int)numparametroscreados; v < ((numparametroscreados) + 6); v++)
            {
                //int randomNumber = random.Next(0, 2500);
                //Creamos la lista de parámetros generadas por este programa
                punteroaplicacion1.p.Add(punteroaplicacion1.ptemp);
                punteroaplicacion1.p[v] = new Parameter(10, 0.01, "");
            }

            punteroaplicacion1.numcorrientes = punteroaplicacion1.numcorrientes + 2;

            D1=Convert.ToDouble(textBox1.Text);
            D2=Convert.ToDouble(textBox2.Text);
            entalpiasalida = Convert.ToDouble(textBox6.Text);
                     
            correntrada=Convert.ToDouble(textBox3.Text);
            corrsalida1 = Convert.ToDouble(textBox4.Text);
            corrsalida2 = Convert.ToDouble(textBox5.Text);


            numequipo = Convert.ToDouble(textBox9.Text);

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

            if (checkBox1.Checked == false)
            {   
                punteroaplicacion1.p[(int)numparametroscreados].Nombre = "W" + Convert.ToString(corrsalida1);
                punteroaplicacion1.p[(int)numparametroscreados].Value = punteroaplicacion1.p.Find(p => p.Nombre == "W" + Convert.ToString(correntrada)).Value;
                punteroaplicacion1.p[(int)numparametroscreados + 1].Nombre = "W" + Convert.ToString(corrsalida2);

                //Por ser un proceso isobaro (la separación de humedad) la presión de la entrada es igual a la presión de las salidas
                punteroaplicacion1.p[(int)numparametroscreados + 2].Nombre = "P" + Convert.ToString(corrsalida1);
                punteroaplicacion1.p[(int)numparametroscreados + 2].Value = punteroaplicacion1.p.Find(p => p.Nombre == "P" + Convert.ToString(correntrada)).Value;
                punteroaplicacion1.p[(int)numparametroscreados + 3].Nombre = "P" + Convert.ToString(corrsalida2);
                punteroaplicacion1.p[(int)numparametroscreados + 3].Value = punteroaplicacion1.p.Find(p => p.Nombre == "P" + Convert.ToString(correntrada)).Value;
                //La entalpía del vapor secado es un poco superior a la del vapor húmedo
                punteroaplicacion1.p[(int)numparametroscreados + 4].Nombre = "H" + Convert.ToString(corrsalida1);
                punteroaplicacion1.p[(int)numparametroscreados + 4].Value = punteroaplicacion1.p.Find(p => p.Nombre == "H" + Convert.ToString(correntrada)).Value;
                
                punteroaplicacion1.p[(int)numparametroscreados + 5].Nombre = "H" + Convert.ToString(corrsalida2);
                punteroaplicacion1.p[(int)numparametroscreados + 5].Value = punteroaplicacion1.acceso.hsatpliq(punteroaplicacion1.p.Find(p => p.Nombre == "P" + Convert.ToString(correntrada)).Value);

                Parameter W1 = new Parameter();
                Parameter W2 = new Parameter();
                Parameter W3 = new Parameter();
                Parameter P1 = new Parameter();
                Parameter P2 = new Parameter();
                Parameter P3 = new Parameter();
                Parameter H1 = new Parameter();
                Parameter H2 = new Parameter();
                Parameter H3 = new Parameter();

                W1 = punteroaplicacion1.p.Find(p => p.Nombre == "W" + Convert.ToString(correntrada));
                W2 = punteroaplicacion1.p.Find(p => p.Nombre == "W" + Convert.ToString(corrsalida1));
                W3 = punteroaplicacion1.p.Find(p => p.Nombre == "W" + Convert.ToString(corrsalida2));
                P1 = punteroaplicacion1.p.Find(p => p.Nombre == "P" + Convert.ToString(correntrada));
                P2 = punteroaplicacion1.p.Find(p => p.Nombre == "P" + Convert.ToString(corrsalida1));
                P3 = punteroaplicacion1.p.Find(p => p.Nombre == "P" + Convert.ToString(corrsalida2));
                H1 = punteroaplicacion1.p.Find(p => p.Nombre == "H" + Convert.ToString(correntrada));
                H2 = punteroaplicacion1.p.Find(p => p.Nombre == "H" + Convert.ToString(corrsalida1));
                H3 = punteroaplicacion1.p.Find(p => p.Nombre == "H" + Convert.ToString(corrsalida2));

                ecuaciones2.Add("");
                ecuaciones2[auxiliar] = "W" + Convert.ToString(correntrada) + "-" + "W" + Convert.ToString(corrsalida1) + "-" + "W" + Convert.ToString(corrsalida2);
                Func<Double> primeraecuacion = () => W1 - W2 - W3;
                punteroaplicacion1.functions.Add(primeraecuacion);
                auxiliar++;

                ecuaciones2.Add("");
                //Pendiente hacer una llamada a Tablas de Vapor-Agua para calcular el titulo x(p1,H1)
                ecuaciones2[auxiliar] = "W" + Convert.ToString(corrsalida2) + "-" + "W" + Convert.ToString(correntrada) + "*" + "(" + Convert.ToString(D2) + "+" + "(" + "(" + "1" + "-" + "xph(P" + Convert.ToString(correntrada) + ",H" + Convert.ToString(correntrada) + ")" + ")" + "*" + Convert.ToString(D1) + ")" + ")";
                Func<Double> segundaecuacion = () => W3 - (W1 * (D2 + (D1 * (1 - (punteroaplicacion1.acceso.xph(P1, H1) / 100)))));
                punteroaplicacion1.functions.Add(segundaecuacion);
                auxiliar++;

                ecuaciones2.Add("");
                ecuaciones2[auxiliar] = "P" + Convert.ToString(corrsalida1) + "-" + "P" + Convert.ToString(correntrada);
                Func<Double> terceraecuacion = () => P2 - P1;
                punteroaplicacion1.functions.Add(terceraecuacion);
                auxiliar++;

                ecuaciones2.Add("");
                ecuaciones2[auxiliar] = "P" + Convert.ToString(corrsalida2) + "-" + "P" + Convert.ToString(correntrada);
                Func<Double> cuartaecuacion = () => P3 - P1;
                punteroaplicacion1.functions.Add(cuartaecuacion);
                auxiliar++;

                ecuaciones2.Add("");
                //Pendiente hacer una llamada a Tablas de Vapor-Agua para calcular el titulo x(p1,h1)
                //Pendiente hacer una llamada a Tablas de Vapor-Agua para calcular la entalpía Hsat(p1)
                ecuaciones2[auxiliar] = "(" + "W" + Convert.ToString(corrsalida2) + "*" + "H" + Convert.ToString(corrsalida2) + ")" + "-" + "(" + "(" + "1" + "-" + "xph(P" + Convert.ToString(correntrada) + ",H" + Convert.ToString(correntrada) + ")" + ")" + "*" + Convert.ToString(D1) + "*" + "W" + Convert.ToString(correntrada) + "*" + "hsatp(P" + Convert.ToString(correntrada) + ")" + ")" + "-" + "(" + Convert.ToString(D2) + "*" + "W" + Convert.ToString(correntrada) + "*" + "H" + Convert.ToString(corrsalida1) + ")";
                Func<Double> quintaecuacion = () => (W3 * H3) - ((1 - (punteroaplicacion1.acceso.xph(P1, H1) / 100)) * D1 * W1 * punteroaplicacion1.acceso.hsatpliq(P1)) - (D2 * W1 * H2);
                punteroaplicacion1.functions.Add(quintaecuacion);
                auxiliar++;                

                ecuaciones2.Add("");
                ecuaciones2[auxiliar] = "(" + "W" + Convert.ToString(correntrada) + "*" + "H" + Convert.ToString(correntrada) + ")" + "-" + "(" + "W" + Convert.ToString(corrsalida1) + "*" + "H" + Convert.ToString(corrsalida1) + ")" + "-" + "(" + "W" + Convert.ToString(corrsalida2) + "*" + "H" + Convert.ToString(corrsalida2) + ")";
                Func<Double> sextaecuacion = () => (W1 * H1) - (W2 * H2) - (W3 * H3);
                punteroaplicacion1.functions.Add(sextaecuacion);
                auxiliar++;
            }

            else if (checkBox1.Checked == true)
            {
                punteroaplicacion1.p[(int)numparametroscreados].Nombre = "W" + Convert.ToString(corrsalida1);
                punteroaplicacion1.p[(int)numparametroscreados + 1].Nombre = "W" + Convert.ToString(corrsalida2);

                punteroaplicacion1.p[(int)numparametroscreados + 2].Nombre = "P" + Convert.ToString(corrsalida1);
                punteroaplicacion1.p[(int)numparametroscreados + 3].Nombre = "P" + Convert.ToString(corrsalida2);

                punteroaplicacion1.p[(int)numparametroscreados + 4].Nombre = "Eficiencia";
                punteroaplicacion1.p[(int)numparametroscreados + 5].Nombre = "H" + Convert.ToString(corrsalida2);


                Parameter W1 = new Parameter();
                Parameter W2 = new Parameter();
                Parameter W3 = new Parameter();
                Parameter P1 = new Parameter();
                Parameter P2 = new Parameter();
                Parameter P3 = new Parameter();
                Parameter H1 = new Parameter();
                Parameter Eficiencia = new Parameter();
                Parameter H3 = new Parameter();

                W1 = punteroaplicacion1.p.Find(p => p.Nombre == "W" + Convert.ToString(correntrada));
                W2 = punteroaplicacion1.p.Find(p => p.Nombre == "W" + Convert.ToString(corrsalida1));
                W3 = punteroaplicacion1.p.Find(p => p.Nombre == "W" + Convert.ToString(corrsalida2));
                P1 = punteroaplicacion1.p.Find(p => p.Nombre == "P" + Convert.ToString(correntrada));
                P2 = punteroaplicacion1.p.Find(p => p.Nombre == "P" + Convert.ToString(corrsalida1));
                P3 = punteroaplicacion1.p.Find(p => p.Nombre == "P" + Convert.ToString(corrsalida2));
                H1 = punteroaplicacion1.p.Find(p => p.Nombre == "H" + Convert.ToString(correntrada));
                Eficiencia = punteroaplicacion1.p.Find(p => p.Nombre == "Eficiencia");
                H3 = punteroaplicacion1.p.Find(p => p.Nombre == "H" + Convert.ToString(corrsalida2));

                ecuaciones2.Add("");
                ecuaciones2[auxiliar] = "W" + Convert.ToString(correntrada) + "-" + "W" + Convert.ToString(corrsalida1) + "-" + "W" + Convert.ToString(corrsalida2);
                Func<Double> primeraecuacion = () => W1 - W2 - W3;
                punteroaplicacion1.functions.Add(primeraecuacion);
                auxiliar++;

                ecuaciones2.Add("");
                //Pendiente hacer una llamada a Tablas de Vapor-Agua para calcular el titulo x(p1,H1)
                ecuaciones2[auxiliar] = "W" + Convert.ToString(corrsalida2) + "-" + "W" + Convert.ToString(correntrada) + "*" + "(" + Convert.ToString(D2) + "+" + "(" + "(" + "1" + "-" + "xph(P" + Convert.ToString(correntrada) + ",H" + Convert.ToString(correntrada) + ")" + ")" + "*" + Convert.ToString(D1) + ")" + ")";
                Func<Double> segundaecuacion = () => W3 - (W1 * (D2 + (Eficiencia * (1 - (punteroaplicacion1.acceso.xph(P1, H1) / 100)))));
                punteroaplicacion1.functions.Add(segundaecuacion);
                auxiliar++;

                ecuaciones2.Add("");
                ecuaciones2[auxiliar] = "P" + Convert.ToString(corrsalida1) + "-" + "P" + Convert.ToString(correntrada);
                Func<Double> terceraecuacion = () => P2 - P1;
                punteroaplicacion1.functions.Add(terceraecuacion);
                auxiliar++;

                ecuaciones2.Add("");
                ecuaciones2[auxiliar] = "P" + Convert.ToString(corrsalida2) + "-" + "P" + Convert.ToString(correntrada);
                Func<Double> cuartaecuacion = () => P3 - P1;
                punteroaplicacion1.functions.Add(cuartaecuacion);
                auxiliar++;

                ecuaciones2.Add("");
                //Pendiente hacer una llamada a Tablas de Vapor-Agua para calcular el titulo x(p1,h1)
                //Pendiente hacer una llamada a Tablas de Vapor-Agua para calcular la entalpía Hsat(p1)
                ecuaciones2[auxiliar] = "(" + "W" + Convert.ToString(corrsalida2) + "*" + "H" + Convert.ToString(corrsalida2) + ")" + "-" + "(" + "(" + "1" + "-" + "xph(P" + Convert.ToString(correntrada) + ",H" + Convert.ToString(correntrada) + ")" + ")" + "*" + Convert.ToString(D1) + "*" + "W" + Convert.ToString(correntrada) + "*" + "hsatp(P" + Convert.ToString(correntrada) + ")" + ")" + "-" + "(" + Convert.ToString(D2) + "*" + "W" + Convert.ToString(correntrada) + "*" + "H" + Convert.ToString(corrsalida1) + ")";
                Func<Double> quintaecuacion = () => (W3 * H3) - ((1 - (punteroaplicacion1.acceso.xph(P1, H1) / 100)) * D1 * W1 * punteroaplicacion1.acceso.hsatpliq(P1)) - (D2 * W1 * entalpiasalida1);
                punteroaplicacion1.functions.Add(quintaecuacion);
                auxiliar++;

                ecuaciones2.Add("");
                ecuaciones2[auxiliar] = "(" + "W" + Convert.ToString(correntrada) + "*" + "H" + Convert.ToString(correntrada) + ")" + "-" + "(" + "W" + Convert.ToString(corrsalida1) + "*" + "H" + Convert.ToString(corrsalida1) + ")" + "-" + "(" + "W" + Convert.ToString(corrsalida2) + "*" + "H" + Convert.ToString(corrsalida2) + ")";
                Func<Double> sextaecuacion = () => (W1 * H1) - (W2 * entalpiasalida1) - (W3 * H3);
                punteroaplicacion1.functions.Add(sextaecuacion);
                auxiliar++;
            }

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
