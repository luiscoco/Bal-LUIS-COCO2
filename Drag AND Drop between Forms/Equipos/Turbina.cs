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
    public partial class Turbina : Form
    {
        Double D1, D3, D4, D5, D8, D9;
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

        int auxiliar = 0;

        public Turbina(Aplicacion punteroaplicion,Double numecuaciones1,Double numvariables1)
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

                label11.Text = "m2";

            }
            else if (punteroaplicacion1.unidades == 2)
            {
                label11.Text = "m2";
                
            }
            else
            {

            }
            D1 = 0;
            D3= 0;
            D4 = 0;
            D5 = 0;
            D8 = 0;
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
            Random random = new Random();
           
            //CREAMOS EL ARRAY DE PARAMETROS
            for (int v = (int)numparametroscreados; v < ((numparametroscreados) + 3); v++)
            {
                //int randomNumber = random.Next(0, 2500);
                //Creamos la lista de parámetros generadas por este programa
                punteroaplicacion1.p.Add(punteroaplicacion1.ptemp);
                punteroaplicacion1.p[v] = new Parameter(10, 0.01, "");
            }
            //INCREMENTAMOS EL NÚMERO DE CORRIENTES
            punteroaplicacion1.numcorrientes = punteroaplicacion1.numcorrientes + 1;

            D1=Convert.ToDouble(textBox1.Text);
            D3=Convert.ToDouble(textBox2.Text);
            D4=Convert.ToDouble(textBox3.Text);
            D5=Convert.ToDouble(textBox4.Text);
            D8=Convert.ToDouble(textBox5.Text);
            D9=Convert.ToDouble(textBox6.Text);

            correntrada=Convert.ToDouble(textBox7.Text);
            corrsalida = Convert.ToDouble(textBox8.Text);

            numequipo = Convert.ToDouble(textBox9.Text);
            //Conversión UNIDADES
            //Como las Tablas de Vapor de Agua ASME 1967 están en unidades británicas, siempre tenemos que convertir los datos de entrada a Unidades Británicas

            //Unidades Métricas (W Kgr/sg P Bar H Kj/Kgr)
            if (punteroaplicacion1.unidades == 2)
            {
               //Conversión del Factor de Flujo de Unidades Métricas a Británicas
               D3 = D3 * 2.316749697;
            }

            //Unidades Sistema Internacional (W Kgr/sg, P kPa, H Kj/Kgr)
            else if (punteroaplicacion1.unidades == 1)
            {
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


            ecuaciones1=generaecucaiones(D1,D3,D4,D5,D8,D9,correntrada,corrsalida);

            listBox1.Items.Add("Nº Equipo:" + Convert.ToString(numequipo));

            for (int numecua = 0; numecua < auxiliar; numecua++)
            {
                listBox1.Items.Add(ecuaciones1[numecua]);
            }

            listBox1.Items.Add("");

            button2.Enabled = true;
            
        }

        private List<String> generaecucaiones(Double D1, Double D3,Double D4, Double D5, Double D8, Double D9, Double correntrada, Double corrsalida)
        {
            //Lista de cadenas que guardan las ecuaciones del sistema
            List<String> ecuaciones2 = new List<String>();
            

            punteroaplicacion1.p[(int)numparametroscreados].Nombre = "W" + Convert.ToString(corrsalida);
            punteroaplicacion1.p[(int)numparametroscreados].Value=punteroaplicacion1.p.Find(p => p.Nombre == "W" + Convert.ToString(correntrada)).Value;
            punteroaplicacion1.p[(int)numparametroscreados + 1].Nombre = "P" + Convert.ToString(corrsalida);
            punteroaplicacion1.p[(int)numparametroscreados + 1].Value =punteroaplicacion1.p.Find(p => p.Nombre == "P" + Convert.ToString(correntrada)).Value-1;
            punteroaplicacion1.p[(int)numparametroscreados + 2].Nombre = "H" + Convert.ToString(corrsalida);
            punteroaplicacion1.p[(int)numparametroscreados + 2].Value = punteroaplicacion1.acceso.hsatpvap(punteroaplicacion1.p.Find(p => p.Nombre == "P" + Convert.ToString(corrsalida)).Value);

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
            ecuaciones2[auxiliar] = "W" + Convert.ToString(correntrada) + "-" + "W" + Convert.ToString(corrsalida);
            Func<Double> primeraecuacion = () => W1 - W2;
            punteroaplicacion1.functions.Add(primeraecuacion);
            auxiliar++;


            //Ecuación de rendimiento termodinámico
            if(D1!=0)
            {
                           
            String Hescape;
            Hescape="";
            //Las Pérdidas en el Escape 

            Hescape=Convert.ToString(D4);
            
                //Corrección del rendimiento termodinámico
                if(D5>0)
                {
                     ecuaciones2.Add("");
                     ecuaciones2[auxiliar]="H"+Convert.ToString(corrsalida)+"-"+"H"+Convert.ToString(correntrada)+"+"+Convert.ToString(D1)+"*"+"("+"H"+Convert.ToString(correntrada)+"-"+"H2isoentropica"+")"+"+"+ Hescape;
                     
                     //TABLA DE PERDIDAS EN EL ESCAPE
                     Double fac = 1;
                     Double elep = H1 - D1 * (H1 - punteroaplicacion1.acceso.hps(P2, (punteroaplicacion1.acceso.sph(P1, H1))));
                     Double X_ex = punteroaplicacion1.acceso.xph(W2,elep);
                     //Double Velocidadescape=W2/(punteroaplicacion1.acceso.rh);

                     //Pérdidas en el escape en función del Area de Escape
                     if((D9>0)&&(D9!=1))
                     {
                        Func<Double> terceraecuacion = () => H2-H1+rendcorrejido(P1,H1,P2,H2)*(H1-punteroaplicacion1.acceso.hps(P2,(punteroaplicacion1.acceso.sph(P1,H1))))+(fac*punteroaplicacion1.tabla(D4,D9,2));
                        punteroaplicacion1.functions.Add(terceraecuacion);
                        auxiliar++; 
                     }

                     //Pérdidas en el escape en función del Caudal Volumétrico a la Descarga
                     else if (D9==1)
                     {
                        fac = 0.87 * (0.35 + 0.65 * X_ex);
                        Func<Double> terceraecuacion = () => H2-H1+rendcorrejido(P1,H1,P2,H2)*(H1-punteroaplicacion1.acceso.hps(P2,(punteroaplicacion1.acceso.sph(P1,H1))))+(fac*X_ex*punteroaplicacion1.tabla(D4,W2,2));
                        punteroaplicacion1.functions.Add(terceraecuacion);
                        auxiliar++;    
                     }

                     //No se consideran las Pérdidas en el Escape
                     else if (D9 == 0)
                     {
                         Func<Double> terceraecuacion = () => H2 - H1 + rendcorrejido(P1, H1, P2, H2) * (H1 - punteroaplicacion1.acceso.hps(P2, (punteroaplicacion1.acceso.sph(P1, H1))));
                         punteroaplicacion1.functions.Add(terceraecuacion);
                         auxiliar++;
                     }
                }

                //No hay corrección del rendimiento termodinámico
                else
                {
                    ecuaciones2.Add("");
                    ecuaciones2[auxiliar] = "H" + Convert.ToString(corrsalida) + "-" + "H" + Convert.ToString(correntrada) + "+" + Convert.ToString(D1) + "*" + "(" + "H" + Convert.ToString(correntrada) + "-" + "H2isoentropica" + ")" + "+" + Hescape;
                    
                    //TABLA DE PERDIDAS EN EL ESCAPE
                    Double fac = 1;
                    Double elep = H1 - D1 * (H1 - punteroaplicacion1.acceso.hps(P2, (punteroaplicacion1.acceso.sph(P1, H1))));
                    Double X_ex = punteroaplicacion1.acceso.xph(W2, elep);

                    //Pérdidas en el escape en función del Area de Escape
                    if ((D9 > 0) && (D9 != 1))
                    {
                        Func<Double> terceraecuacion = () => H2 - H1 + D1* (H1 - punteroaplicacion1.acceso.hps(P2, (punteroaplicacion1.acceso.sph(P1, H1)))) + fac*(punteroaplicacion1.tabla(D4, D9, 2));
                        punteroaplicacion1.functions.Add(terceraecuacion);
                        auxiliar++;
                    }

                    //Pérdidas en el escape en función del Caudal Volumétrico a la Descarga
                    else if (D9 == 1)
                    {
                        fac = 0.87 * (0.35 + 0.65 *X_ex);
                        Func<Double> terceraecuacion = () => H2 - H1 + D1* (H1 - punteroaplicacion1.acceso.hps(P2, (punteroaplicacion1.acceso.sph(P1, H1)))) + fac*X_ex*(punteroaplicacion1.tabla(D4, W2, 2));
                        punteroaplicacion1.functions.Add(terceraecuacion);
                        auxiliar++;
                    }

                    //No se consideran las Pérdidas en el Escape
                    else if (D9 == 0)
                    {
                        Func<Double> terceraecuacion = () => H2 - H1 + D1 * (H1 - punteroaplicacion1.acceso.hps(P2, (punteroaplicacion1.acceso.sph(P1, H1))));
                        punteroaplicacion1.functions.Add(terceraecuacion);
                        auxiliar++;
                    }
                }
            }

            //Ecuación del Factor de Flujo
            if (D3 > 0)
            {
                if (D8> 0) //Corrección del Factor de Flujo mediante la relación de presiones Psalida/Pentrada
                {
                    ecuaciones2.Add("");
                    ecuaciones2[auxiliar] = "W" + Convert.ToString(correntrada) + "-" + "(" + Convert.ToString(D3) + "*" + "(" + "(" + "P" + Convert.ToString(correntrada) + "/" + "vph(P" + Convert.ToString(correntrada) + ",H" + Convert.ToString(correntrada) + ")" + ")" + "^" + "(1/2)" + ")" + ")";
                    Func<Double> segundaecuacion = () => W1 - (FactorFlujocorrejido(P1,P2,punteroaplicacion1.acceso.xph(P1,H1)) * (Math.Sqrt(P1 / (punteroaplicacion1.acceso.vph(P1, H1)))));
                    punteroaplicacion1.functions.Add(segundaecuacion);
                    auxiliar++;
                }
                else //No corrección del Factor de Flujo
                {
                    ecuaciones2.Add("");
                    ecuaciones2[auxiliar] = "W" + Convert.ToString(correntrada) + "-" + "(" + Convert.ToString(D3) + "*" + "(" + "(" + "P" + Convert.ToString(correntrada) + "/" + "vph(P" + Convert.ToString(correntrada) + ",H" + Convert.ToString(correntrada) + ")" + ")" + "^" + "(1/2)" + ")" + ")";
                    Func<Double> segundaecuacion = () => W1 - (D3 * (Math.Sqrt(P1 / (punteroaplicacion1.acceso.vph(P1, H1)))));
                    punteroaplicacion1.functions.Add(segundaecuacion);
                    auxiliar++;
                }
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

        //Función para calcular el Factor de Flujo correjido en función de la relación Psalida/Pentrada
        public Double FactorFlujocorrejido(Double P1, Double P2, Double X)
        {
            Double k = 1.3;
            Double factorflujocorrejido1 = 0;
            Double rp = P2 / P1;
            X = X / 100;

            if ((X > 0) && (X == 1))
            {
                k = 1.13;
            }

            else 
            {
                k = 1.3;            
            }

            Double FR1 = Math.Sqrt(Math.Abs(Math.Pow(rp, (2 / k)) - Math.Pow(rp, ((k + 1) / k))));
            Double FR2 = Math.Sqrt(Math.Abs(Math.Pow(D8, (2 / k)) - Math.Pow(D8, ((k + 1) / k))));
            factorflujocorrejido1=D3*(1-(Math.Abs(FR1-FR2)/FR1));
            return factorflujocorrejido1;
        }


        //Rendimiento correjido en función de la Calidad Media
        public Double rendcorrejido(Double P1,Double H1,Double P2,Double H2)
        {
            Double X = (punteroaplicacion1.acceso.xph(P1,H1)+punteroaplicacion1.acceso.xph(P2,H2))/2;

            Double rendcorregido1=D1*(1-((D5-(X/100))/D5));
            
            return rendcorregido1;
        }
        

        //Botón de OK
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

        //Botón de Cancel
        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        
    }
}
