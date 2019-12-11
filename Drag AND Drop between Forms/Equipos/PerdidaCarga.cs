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
    public partial class Perdidacarga : Form
    {
        Double D1, D2, D3, D4, D5, D6, D7, D8;
        Double correntrada, corrsalida;
        Double numequipo;

        Double numecuaciones2;
        Double numvariables2;

        //Lista de cadenas para gardar los nombres de las variables del sistema de ecuaciones
        List<String> variables1 = new List<String>();

        //Lista de cadenas que guardan las ecuaciones del sistema
        List<String> ecuaciones1 = new List<String>();

        Aplicacion punteroaplicacion1;

        Double numparametroscreados = 0;

        public Perdidacarga(Aplicacion punteroaplicion,Double numecuaciones1,Double numvariables1)
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



            }
            else if (punteroaplicacion1.unidades == 2)
            {
                label17.Text = "m";
                label13.Text = "m";
                
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

            correntrada = 0;
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

            for (int v = (int)numparametroscreados; v < ((numparametroscreados) + 3); v++)
            {
                //int randomNumber = random.Next(0, 2500);
                //Creamos la lista de parámetros generadas por este programa
                punteroaplicacion1.p.Add(punteroaplicacion1.ptemp);
                punteroaplicacion1.p[v] = new Parameter(10, 0.01, "");
            }

            punteroaplicacion1.numcorrientes = punteroaplicacion1.numcorrientes + 1;

            D1=Convert.ToDouble(textBox1.Text);
            D2=Convert.ToDouble(textBox2.Text);
            D3=Convert.ToDouble(textBox3.Text);
            D4=Convert.ToDouble(textBox4.Text);
            D5=Convert.ToDouble(textBox5.Text);
            D6=Convert.ToDouble(textBox6.Text);
            D7 = Convert.ToDouble(textBox7.Text);
            D8 = Convert.ToDouble(textBox8.Text);

            correntrada=Convert.ToDouble(textBox10.Text);
            corrsalida = Convert.ToDouble(textBox11.Text);

            numequipo = Convert.ToDouble(textBox9.Text);
            //Conversión UNIDADES
            //Como las Tablas de Vapor de Agua ASME 1967 están en unidades británicas, siempre tenemos que convertir los datos de entrada a Unidades Británicas

            //Unidades Métricas
            if (punteroaplicacion1.unidades == 2)
            {
                D6 = D6 *3.28083;
                //Distancias de m a ft
                D7 = D7 * 3.28083;
                //Distancias de m a ft
                D1 = D1 / (6.8947572 / 100);
                //Factor cuadrático de pérdida de carga
                D3 = D3 * 2.984193609;
                //Factor lineal de pérdida de carga
                D2 = D2 * 6.578911309;
            }

            //Unidades Sistema Internacional
            else if (punteroaplicacion1.unidades == 1)
            {
                D6 = D6 * 3.28083;
                //Distancias de m a ft
                D7 = D7 * 3.28083;
                //Distancias de m a ft
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

            ecuaciones1=generaecucaiones(D1,D2,D3,D4,D5,D6,D7,D8,correntrada,corrsalida);

            listBox1.Items.Add("Nº Equipo:" + Convert.ToString(numequipo));

            for (int numecua = 0; numecua < 3; numecua++)
            {
                listBox1.Items.Add(ecuaciones1[numecua]);
            }

            listBox1.Items.Add("");

            button2.Enabled = true;

        }

        private List<String> generaecucaiones(Double D1, Double D2,Double D3,Double D4, Double D5, Double D6, Double D7,Double D8, Double correntrada, Double corrsalida)
        {
            String ap1,ap2;
            ap1 = "";
            ap2 = "";

            //Lista de cadenas que guardan las ecuaciones del sistema
            List<String> ecuaciones2 = new List<String>();

            punteroaplicacion1.p[(int)numparametroscreados].Nombre = "W" + Convert.ToString(corrsalida);
            punteroaplicacion1.p[(int)numparametroscreados].Value = punteroaplicacion1.p.Find(p => p.Nombre == "W" + Convert.ToString(correntrada)).Value;
            punteroaplicacion1.p[(int)numparametroscreados + 1].Nombre = "P" + Convert.ToString(corrsalida);

            punteroaplicacion1.p[(int)numparametroscreados + 2].Nombre = "H" + Convert.ToString(corrsalida);
            punteroaplicacion1.p[(int)numparametroscreados + 2].Value = punteroaplicacion1.p.Find(p => p.Nombre == "H" + Convert.ToString(correntrada)).Value;



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
            
            ecuaciones2.Add("");
            ecuaciones2[0] = "W" + Convert.ToString(correntrada) + "-" + "W" + Convert.ToString(corrsalida);
            Func<Double> primeraecuacion = () => W1 - W2;
            punteroaplicacion1.functions.Add(primeraecuacion);

            if ((D1>0)||(D2>0)||(D3>0)||(D4>0))
            {
                ecuaciones2.Add("");
                ap1= "("+Convert.ToString(D1) + "+" + "(" + "(" + Convert.ToString(D2) + "*" + "W" + Convert.ToString(correntrada) + ")" + "/" + Convert.ToString(D8) + ")" + "+" + "(" + Convert.ToString(D3) + "*" + "W" + Convert.ToString(correntrada) + "*" + "W" + Convert.ToString(correntrada) + ")" + "/" + "(" + Convert.ToString(D8) + "*" + Convert.ToString(D8) + ")"+"+"+"("+ Convert.ToString(D4) + "*"+ "P" + Convert.ToString(correntrada)+")"+")";
                Func<Double> segundaecuacion = () => P1 - P2-(D1+(D2*W1/D8)+((D3*W1*W1)/(D8*D8))+(D4*P1));
                punteroaplicacion1.functions.Add(segundaecuacion);

                if ((D5>0)||(D6>0)||(D7 > 0))
                {
                    //Llamada a la función de Tables de Vapor 
                    //String v;
                    //v = Convert.ToString(p,h);
                    //rho=1/v;
                    ap2 = "-" + "((" + Convert.ToString(D5) + "/"+"("+ Convert.ToString(D6) + "^4))*(" + "W" + Convert.ToString(correntrada) + "^2)*(16/(3.14159265^2))*(" + "vph(P"+Convert.ToString(correntrada)+",H"+Convert.ToString(correntrada)+")" + "^2)*(1/(2*9.80665))*((" + "33.9795" + "*9.80665)/100000))";
                    segundaecuacion = () => P1 - P2 - ((D1 + (D2 * W1 / D8) + ((D3 * W1 * W1) / (D8 * D8)) + (D4 * P1))) - (((D5 * W2 * W2) / (Math.PI * Math.PI / 16)) * (punteroaplicacion1.acceso.vph(P1, H1) * punteroaplicacion1.acceso.vph(P1, H1) / (D6 * D6 * D6 * D6)) * (1 / (2 * 32.15236)) * (0.006944444 / punteroaplicacion1.acceso.vph(P1, H1)));
                }
            }
            
            else if ((D5>0)||(D6>0)||(D7>0))
            {
                //Llamada a la función de Tables de Vapor 
                //String v;
                //v = Convert.ToString(p,h);
                //rho=1/v;
                
                ap2 = "((" + Convert.ToString(D5) + "/" +"("+ Convert.ToString(D6) + "^4))*(" + "W" + Convert.ToString(correntrada) + "^2)*(16/(3.14159265^2))*(" + "vph(P"+Convert.ToString(correntrada)+",H"+Convert.ToString(correntrada)+")"+ "^2)*(1/(2*9.80665))*((" + "33.9795" + "*9.80665)/100000))";
                Func<Double> segundaecuacion = () => P1 - P2 - (((D5 * W2 * W2) / (Math.PI * Math.PI / 16)) * (punteroaplicacion1.acceso.vph(P1, H1) * punteroaplicacion1.acceso.vph(P1, H1) / (D6 * D6 * D6 * D6)) * (1 / (2 * 32.15236)) * (0.006944444 / punteroaplicacion1.acceso.vph(P1, H1)));
                punteroaplicacion1.functions.Add(segundaecuacion);
            }

            else
            {

            }

            ecuaciones2.Add("");
            ecuaciones2[1] = "P" + Convert.ToString(correntrada) + "-" + "P" + Convert.ToString(corrsalida) + "-" + ap1 + ap2;

            ecuaciones2.Add("");
            ecuaciones2[2] = "H" + Convert.ToString(correntrada) + "-" + "H" + Convert.ToString(corrsalida);
            Func<Double> terceraecuacion = () => H1 - H2;
            punteroaplicacion1.functions.Add(terceraecuacion);

            variables1.Add("");
            variables1[0] = "W" + Convert.ToString(corrsalida);
            variables1.Add("");
            variables1[1] = "P" + Convert.ToString(corrsalida);
            variables1.Add("");
            variables1[2] = "H" + Convert.ToString(corrsalida);

            numecuaciones2 = 3;
            numvariables2 = 3;
           
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
