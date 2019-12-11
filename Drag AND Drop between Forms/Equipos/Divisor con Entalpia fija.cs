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
    public partial class Divisorentalpiafija : Form
    {
        Double D1, D2, D3;
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

        public Divisorentalpiafija(Aplicacion punteroaplicion,Double numecuaciones1,Double numvariables1)
        {
            InitializeComponent();           

            punteroaplicacion1 = punteroaplicion;
            numparametroscreados = (punteroaplicacion1.numcorrientes)*3;

            //Inicializamos las etiquetas de las unidades de entrada en el cuadro de dialogo de toma de datos
            if (punteroaplicacion1.unidades == 0)
            {

            }
            else if (punteroaplicacion1.unidades == 1)
            {
                label17.Text = "Kgr/sg";
                
            }
            else if (punteroaplicacion1.unidades == 2)
            {
                label17.Text = "Kgr/sg";
                

            }
            else
            {

            }

            D1 = 0;
            D2 = 0;
            D3 = 0;
            
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


        //Boton Generar Ecuaciones
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
            D3=Convert.ToDouble(textBox3.Text);
           

            correntrada=Convert.ToDouble(textBox7.Text);
            corrsalida1 = Convert.ToDouble(textBox8.Text);
            corrsalida2 = Convert.ToDouble(textBox4.Text);


            numequipo = Convert.ToDouble(textBox9.Text);

            //Conversión UNIDADES
            //Como las Tablas de Vapor de Agua ASME 1967 están en unidades británicas, siempre tenemos que convertir los datos de entrada a Unidades Británicas

            //Unidades Métricas
            if (punteroaplicacion1.unidades == 2)
            {
                //Caudal Kgr/sg a Lb/sg
                D1 = D1 / (0.4536);
                //Conversión del Factor de Flujo de Unidades del Sistema Métrico a Británicas
                D3 = D3 * 2.316749697;
            }

            //Unidades Sistema Internacional
            else if (punteroaplicacion1.unidades == 1)
            {
                //Caudal Kgr/sg a Lb/sg
                D1 = D1 / (0.4536);
                //Conversión del Factor de Flujo de Unidades del Sistema Internacional a Británicas
                D3 = D3 * 23.16749697;
            }

            //Unidades Sistema Británico
            else if (punteroaplicacion1.unidades == 0)
            {

            }
            else
            {

            }

            ecuaciones1=generaecucaiones(D1,D2,D3,correntrada,corrsalida1,corrsalida2);

            listBox1.Items.Add("Nº Equipo:" + Convert.ToString(numequipo));

            for (int numecua = 0; numecua < auxiliar; numecua++)
            {
                listBox1.Items.Add(ecuaciones1[numecua]);
            }

            listBox1.Items.Add("");

            button2.Enabled = true;
        }

        private List<String> generaecucaiones(Double D1, Double D2,Double D3, Double correntrada, Double corrsalida1,Double corrsalida2)
        {
            //Lista de cadenas que guardan las ecuaciones del sistema
            List<String> ecuaciones2 = new List<String>();

            punteroaplicacion1.p[(int)numparametroscreados].Nombre = "W" + Convert.ToString(corrsalida1);
            punteroaplicacion1.p[(int)numparametroscreados + 1].Nombre = "W" + Convert.ToString(corrsalida2);

            punteroaplicacion1.p[(int)numparametroscreados + 2].Nombre = "P" + Convert.ToString(corrsalida1);
            punteroaplicacion1.p[(int)numparametroscreados + 3].Nombre = "P" + Convert.ToString(corrsalida2);

            punteroaplicacion1.p[(int)numparametroscreados + 4].Nombre = "H" + Convert.ToString(corrsalida1);
            punteroaplicacion1.p[(int)numparametroscreados + 5].Nombre = "H" + Convert.ToString(corrsalida2);


            Parameter W1 = new Parameter();
            Parameter W2 = new Parameter();
            Parameter W3 = new Parameter();
            Parameter P1 = new Parameter();
            Parameter P2 = new Parameter();
            Parameter P3 = new Parameter();
            Parameter H1 = new Parameter();
            Parameter H2 = new Parameter();
            Parameter H3=  new Parameter();

            W1 = punteroaplicacion1.p.Find(p => p.Nombre == "W" + Convert.ToString(correntrada));
            W2 = punteroaplicacion1.p.Find(p => p.Nombre == "W" + Convert.ToString(corrsalida1));
            W3=  punteroaplicacion1.p.Find(p => p.Nombre == "W" + Convert.ToString(corrsalida2));
            P1 = punteroaplicacion1.p.Find(p => p.Nombre == "P" + Convert.ToString(correntrada));
            P2 = punteroaplicacion1.p.Find(p => p.Nombre == "P" + Convert.ToString(corrsalida1));
            P3 = punteroaplicacion1.p.Find(p => p.Nombre == "P" + Convert.ToString(corrsalida2));
            H1 = punteroaplicacion1.p.Find(p => p.Nombre == "H" + Convert.ToString(correntrada));
            H2 = punteroaplicacion1.p.Find(p => p.Nombre == "H" + Convert.ToString(corrsalida1));
            H3 = punteroaplicacion1.p.Find(p => p.Nombre == "H" + Convert.ToString(corrsalida2));

            ecuaciones2.Add("");
            ecuaciones2[auxiliar] = "W" + Convert.ToString(correntrada) + "-" + "W" + Convert.ToString(corrsalida1) + "-" + "W" + Convert.ToString(corrsalida2);
            Func<Double> primeraecuacion = () => W1 -W2-W3;
            punteroaplicacion1.functions.Add(primeraecuacion);
            auxiliar++;

            ecuaciones2.Add("");
            ecuaciones2[auxiliar] = "P" + Convert.ToString(correntrada) + "-" + "P" + Convert.ToString(corrsalida1);
            Func<Double> segundaecuacion = () => P1-P2;
            punteroaplicacion1.functions.Add(segundaecuacion);
            auxiliar++;

            ecuaciones2.Add("");
            ecuaciones2[auxiliar] = "H" + Convert.ToString(correntrada) + "-" + "H" + Convert.ToString(corrsalida1);
            Func<Double> terceraecuacion = () => H1-H2;
            punteroaplicacion1.functions.Add(terceraecuacion);
            auxiliar++;

            ecuaciones2.Add("");
            ecuaciones2[auxiliar] = "P" + Convert.ToString(correntrada) + "-" + "P" + Convert.ToString(corrsalida2);
            Func<Double> cuartaecuacion = () => P1-P3;
            punteroaplicacion1.functions.Add(cuartaecuacion);
            auxiliar++;

            ecuaciones2.Add("");
            ecuaciones2[auxiliar] = "H" + Convert.ToString(correntrada) + "-" + "H" + Convert.ToString(corrsalida2);
            Func<Double> quintaecuacion = () => H1-H3;
            punteroaplicacion1.functions.Add(quintaecuacion);
            auxiliar++;            


            if ((D1 > 0) || (D2 > 0) || (D3 > 0))
            {
                if ((D1 > 0) || (D2 > 0) && (D3==0))
                {
                    ecuaciones2.Add("");
                    ecuaciones2[auxiliar] = "W" + Convert.ToString(corrsalida2) + "-" + Convert.ToString(D1) + "-" +"("+Convert.ToString(D2) + "*" + "W" + Convert.ToString(correntrada)+")";
                    Func<Double> sextaecuacion = () => W3 - D1 - (D2 * W1);
                    punteroaplicacion1.functions.Add(sextaecuacion);
                    auxiliar++;
                }

                else if (D3 > 0 && D1==0 && D2==0)
                {
                    ecuaciones2.Add("");
                    ecuaciones2[auxiliar] = "W" + Convert.ToString(corrsalida2) + "-" + "(" + Convert.ToString(D3) + "*" +"sqrt("+"P"+Convert.ToString(correntrada)+"/"+"vph("+"P"+Convert.ToString(correntrada)+","+"H"+Convert.ToString(correntrada)+")"+")"+ ")";
                    //Pendiente actualizar la función v(P,H)
                    Func<Double> sextaecuacion = () => W3 - (D3*Math.Sqrt(P1/punteroaplicacion1.acceso.vph(P1,H1)));
                    punteroaplicacion1.functions.Add(sextaecuacion);
                    auxiliar++;
                }

                else if (D1 < 0 && D2==0 && D3==0)
                {
                    ecuaciones2.Add("");
                    ecuaciones2[auxiliar] = "W" + Convert.ToString(corrsalida2) + "-" + "("+Convert.ToString(-D1)+")";
                    Func<Double> sextaecuacion = () => W3 - (-D1);
                    punteroaplicacion1.functions.Add(sextaecuacion);
                    auxiliar++;
                }
                else
                {
                 
                }
            }

            else
            {

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
