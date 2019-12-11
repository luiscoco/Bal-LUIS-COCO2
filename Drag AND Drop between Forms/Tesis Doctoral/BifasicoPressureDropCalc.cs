using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Tablas_Vapor_ASME;
using Tablas_Vapor_ASME1;
using Tablas_Vapor_ASME2;
using Tablas_Vapor_ASME3;
using Tablas_Vapor_ASME4;
using Tablas_Vapor_ASME5;

using NumericalMethods;
using NumericalMethods.FourthBlog;

namespace Bifasico
{
    public partial class BifasicoPressureDropCalc : Form
    {
        public BifasicoPressureDrop bifasico = new BifasicoPressureDrop();
        
        
        public BifasicoPressureDropCalc ()
        {
            InitializeComponent();
        }
               
        //Cálculo Densidad Líquido Saturado
        private void button1_Click(object sender, EventArgs e)
        {
            //Primero cálculo de la Región 
            bifasico.temperaturein = Convert.ToDouble(textBox11.Text) + 273.15;
            bifasico.pressurein = Convert.ToDouble(textBox10.Text) / 10;
            bifasico.densityinliquido = bifasico.calculodensidadliquido(bifasico.temperaturein, bifasico.pressurein);
            textBox16.Text = Convert.ToString(bifasico.densityinliquido);
        }
                
        //Cálculo Densidad Vapor Saturado
        private void button4_Click(object sender, EventArgs e)
        {
            //Primero cálculo de la Región 
            bifasico.temperaturein = Convert.ToDouble(textBox11.Text) + 273.15;
            bifasico.pressurein = Convert.ToDouble(textBox10.Text) / 10;
            bifasico.densityinvapor = bifasico.calculodensidadvapor(bifasico.temperaturein, bifasico.pressurein);
            textBox4.Text = Convert.ToString(bifasico.densityinvapor);
        }               

        //Botón del Cálculo de la Viscosidad Dinámica Líquido Saturado
        private void button8_Click(object sender, EventArgs e)
        {
            bifasico.densityinliquido = Convert.ToDouble(textBox16.Text);
            bifasico.viscosidaddinamicaliquido = bifasico.calculoviscosidaddinamicaliquido(bifasico.densityinliquido, bifasico.temperaturein, bifasico.pressurein);
            
            //Cálculo de la VISCOSIDAD CINEMÁTICA Líquido Saturado
            bifasico.viscosidadcinematicaliquido = 1000000 * (bifasico.viscosidaddinamicaliquido / bifasico.densityinliquido);

            textBox2.Text = Convert.ToString(bifasico.viscosidadcinematicaliquido);
            textBox1.Text = Convert.ToString(bifasico.viscosidaddinamicaliquido);
        }
        
        //Cálculo Coeficiente Darcy Líquido Saturado
        private void button7_Click(object sender, EventArgs e)
        {
            bifasico.rugosidad = Convert.ToDouble(textBox12.Text) / 1000;
            bifasico.coefdarcyliquido = bifasico.calculocoefdarcyliq(bifasico.numreynoldliquido, bifasico.rugosidad, bifasico.diametrohidraulico, bifasico.tipoflujoLiquido);
            textBox13.Text = Convert.ToString(bifasico.coefdarcyliquido);
        }        

        //Cálculo del DIÁMETRO HIDRÁULICO
        private void button5_Click(object sender, EventArgs e)
        {
            bifasico.diametrointerior = Convert.ToDouble(textBox5.Text) / 1000;
            bifasico.diametrohidraulico = bifasico.calculodiametrohidraulico(bifasico.diametrointerior);
            textBox6.Text = Convert.ToString(bifasico.areafluido);
            textBox7.Text = Convert.ToString(bifasico.perimetrofluido);
            textBox8.Text = Convert.ToString(bifasico.diametrohidraulico);
        }       

        //Cálculo del Número de Reynolds Líquido Saturado
        private void button6_Click(object sender, EventArgs e)
        {
            bifasico.velocidad = Convert.ToDouble(textBox3.Text);
            bifasico.numreynoldliquido = bifasico.calculonumreynoldsliquido(bifasico.densityinliquido, bifasico.velocidad, bifasico.diametrohidraulico, bifasico.viscosidaddinamicaliquido);
            
            textBox9.Text = Convert.ToString(bifasico.numreynoldliquido);      
            textBox14.Text = bifasico.tipoflujoLiquido;
        }       

        //Botón del Cálculo de la Viscosidad Dinámica Vapor Saturado
        private void button2_Click(object sender, EventArgs e)
        {
            bifasico.densityinvapor = Convert.ToDouble(textBox4.Text);
            bifasico.viscosidaddinamicavapor = bifasico.calculoviscosidaddinamicavapor(bifasico.densityinvapor, bifasico.temperaturein, bifasico.pressurein);

            //Cálculo de la VISCOSIDAD CINEMÁTICA Vapor Saturado
            bifasico.viscosidadcinematicavapor = 1000000 * (bifasico.viscosidaddinamicavapor / bifasico.densityinvapor);

            textBox18.Text = Convert.ToString(bifasico.viscosidadcinematicavapor);
            textBox17.Text = Convert.ToString(bifasico.viscosidaddinamicavapor);
        }

        //Cálculo del Número de Reynolds Vapor Saturado
        private void button3_Click(object sender, EventArgs e)
        {
            bifasico.velocidad = Convert.ToDouble(textBox3.Text);
            bifasico.numreynoldvapor = bifasico.calculonumreynoldsvapor(bifasico.densityinvapor, bifasico.velocidad, bifasico.diametrohidraulico, bifasico.viscosidaddinamicavapor);

            textBox23.Text = Convert.ToString(bifasico.numreynoldvapor);
            textBox22.Text = bifasico.tipoflujoVapor;
        }

        //Calcular Tª Saturación
        private void button9_Click_1(object sender, EventArgs e)
        {
            bifasico.pressurein = Convert.ToDouble(textBox10.Text) / 10;
            bifasico.temperaturein = bifasico.calculotempsaturacion(bifasico.pressurein);
            textBox11.Text = Convert.ToString(bifasico.temperaturein);
        }

        //Calcular Presión Saturación
        private void button11_Click(object sender, EventArgs e)
        {
            bifasico.temperaturein = Convert.ToDouble(textBox11.Text) + 273.15;
            bifasico.pressurein = bifasico.calculopresionsaturacion(bifasico.temperaturein);
            textBox10.Text = Convert.ToString(bifasico.pressurein);
        }

        //Cálculo Velocidad Friedel
        private void button12_Click(object sender, EventArgs e)
        {
            bifasico.caudalmasico = Convert.ToDouble(textBox21.Text);
            bifasico.velocidad = bifasico.calculovelocidad(bifasico.caudalmasico, bifasico.areafluido, bifasico.densityintotal);
            textBox3.Text = Convert.ToString(bifasico.velocidad);
        }







        //---------------------------- Pressure Drop Friedel -----------------------------------------------------------------









        //Calcular la Tensión superficial del Líquido Saturado Friedel
        private void button13_Click(object sender, EventArgs e)
        {
            bifasico.surfacetension = bifasico.calculotensionsuperficial(bifasico.temperaturein);
            textBox15.Text = Convert.ToString(bifasico.surfacetension);
        }

        //Cálculo de la Densidad Total Friedel
        private void button14_Click(object sender, EventArgs e)
        {
            bifasico.titulo = Convert.ToDouble(textBox28.Text) / 100;
            bifasico.densityintotal = bifasico.calculodensidadtotalfriedel(bifasico.titulo, bifasico.densityinvapor, bifasico.densityinliquido);
            textBox19.Text = Convert.ToString(bifasico.densityintotal);
        }

        //Cálculo Coeficiente Darcy Vapor Saturado Friedel
        private void button10_Click(object sender, EventArgs e)
        {
            bifasico.rugosidad = Convert.ToDouble(textBox12.Text) / 1000;
            bifasico.coefdarcyvapor = bifasico.calculocoefdarcyvap(bifasico.numreynoldvapor, bifasico.rugosidad, bifasico.diametrohidraulico, bifasico.tipoflujoVapor);
            textBox24.Text = Convert.ToString(bifasico.coefdarcyvapor);
        }

        //Calcular A Friedel
        private void button16_Click(object sender, EventArgs e)
        {
            bifasico.A = bifasico.calculoA_Friedel(bifasico.titulo, bifasico.densityinliquido, bifasico.coefdarcyvapor, bifasico.densityinvapor, bifasico.coefdarcyliquido);
            textBox26.Text = Convert.ToString(bifasico.A);
        }

        //Calcular Nº Froude Friedel
        private void button17_Click(object sender, EventArgs e)
        {
            bifasico.Fr = bifasico.calculoFroudeFriedel(bifasico.diametrointerior, bifasico.densityintotal, bifasico.caudalmasico);
            textBox25.Text = Convert.ToString(bifasico.Fr);
        }









        //------------------------------ AQUI ME HE QUEDADO CREANDO LA CLASE BIFASICO PRESSURE DROP, CONTINUAR CREANDO MAS FUNCIONES EN LA MENCIONADA CLASE --------------------------------------------------------------------------------












        //Calcular F Friedel
        private void button18_Click(object sender, EventArgs e)
        {
            bifasico.F = Math.Pow(bifasico.titulo, 0.78) * Math.Pow((1 - bifasico.titulo), 0.224);
            textBox29.Text = Convert.ToString(bifasico.F);
        }

        //Calcular H Friedel
        private void button19_Click(object sender, EventArgs e)
        {
            double relaciondensidad = 0;
            relaciondensidad = (bifasico.densityinliquido / bifasico.densityinvapor);
            double relacionviscosidades = 0;
            relacionviscosidades = (bifasico.viscosidaddinamicavapor / bifasico.viscosidaddinamicaliquido);

            bifasico.H = Math.Pow((bifasico.densityinliquido / bifasico.densityinvapor), 0.91) * Math.Pow((bifasico.viscosidaddinamicavapor / bifasico.viscosidaddinamicaliquido), 0.19) * Math.Pow((1 - (bifasico.viscosidaddinamicavapor / bifasico.viscosidaddinamicaliquido)), 0.7);
            textBox30.Text = Convert.ToString(bifasico.H);
        }

        //Calcular Weber Friedel
        private void button20_Click(object sender, EventArgs e)
        {
            bifasico.We = ((Math.Pow(bifasico.caudalmasico, 2) * bifasico.diametrointerior) / (bifasico.surfacetension * bifasico.densityintotal));
            textBox31.Text = Convert.ToString(bifasico.We);
        }

        //Calcular APLfriedel
        private void button21_Click(object sender, EventArgs e)
        {
            bifasico.L = Convert.ToDouble(textBox32.Text);
            bifasico.APLfriedel = 4 * bifasico.coefdarcyliquido * (bifasico.L / bifasico.diametrointerior) * Math.Pow(bifasico.caudalmasico, 2) * (1 / (2 * bifasico.densityinliquido));
            textBox20.Text = Convert.ToString(bifasico.APLfriedel);
        }


        //Cálculo de APfricción Friedel
        private void button22_Click(object sender, EventArgs e)
        {
            bifasico.APfricFriedel = bifasico.APLfriedel * bifasico.Friedel;
            textBox33.Text = Convert.ToString(bifasico.APfricFriedel);
        }

        //Cálculo del parámetro de Friedel
        private void button15_Click(object sender, EventArgs e)
        {
            bifasico.Friedel = bifasico.A + ((3.24 * bifasico.F * bifasico.H) / (Math.Pow(bifasico.Fr, 0.045) * Math.Pow(bifasico.We, 0.035)));
            textBox27.Text = Convert.ToString(bifasico.Friedel);
        }






//---------------------------- Pressure Drop Lockhart Martinelli -----------------------------------------------------------------







        //Cálculo de APG LockHart Martinelli
        private void button45_Click(object sender, EventArgs e)
        {
            bifasico.L = Convert.ToDouble(textBox59.Text);
            bifasico.APG = 4 * bifasico.coefdarcyvapor * (bifasico.L / bifasico.diametrointerior) * Math.Pow(bifasico.caudalmasico, 2) * (Math.Pow(1 - bifasico.titulo, 2)) * (1 / (2 * bifasico.densityinvapor));
            textBox67.Text = Convert.ToString(bifasico.APG); 
        }

        //Cálculo del parámetro de Martinelli
        private void button46_Click(object sender, EventArgs e)
        {
            bifasico.Martinelli = Math.Pow(((1 - bifasico.titulo) / bifasico.titulo), 0.9) * Math.Pow((bifasico.densityinvapor / bifasico.densityinliquido), 0.5) * Math.Pow((bifasico.viscosidaddinamicaliquido / bifasico.viscosidaddinamicavapor), 0.1);
            textBox68.Text = Convert.ToString(bifasico.Martinelli);
        }

        //Cálculo de APL LockHart Martinelli
        private void button38_Click(object sender, EventArgs e)
        {
            bifasico.L = Convert.ToDouble(textBox59.Text);
            bifasico.APL = 4 * bifasico.coefdarcyliquido * (bifasico.L / bifasico.diametrointerior) * Math.Pow(bifasico.caudalmasico, 2) * (Math.Pow(bifasico.titulo, 2)) * (1 / (2 * bifasico.densityinliquido));
            textBox60.Text = Convert.ToString(bifasico.APL);
        }

        //Cálculo de Fhi Líquido LockHart Martinelli
        private void button44_Click(object sender, EventArgs e)
        {
            if ((bifasico.tipoflujoLiquido == "Turbulento") && (bifasico.tipoflujoVapor == "Turbulento"))
            {
                bifasico.C = 20;
            }

            else if ((bifasico.tipoflujoLiquido == "Laminar") && (bifasico.tipoflujoVapor == "Turbulento"))
            {
                bifasico.C = 12;
            }

            else if ((bifasico.tipoflujoLiquido == "Turbulento") && (bifasico.tipoflujoVapor == "Laminar"))
            {
                bifasico.C = 10;
            }

            else if ((bifasico.tipoflujoLiquido == "Laminar") && (bifasico.tipoflujoVapor == "Laminar"))
            {
                bifasico.C = 5;
            }

            bifasico.FhiLiquido = 1 + (bifasico.C / bifasico.Martinelli) + (1 / Math.Pow(bifasico.Martinelli, 2));
            textBox66.Text = Convert.ToString(bifasico.FhiLiquido);
        }

        //Cálculo de Fhi Vapor LockHart Martinelli
        private void button47_Click(object sender, EventArgs e)
        {
            if ((bifasico.tipoflujoLiquido == "Turbulento") && (bifasico.tipoflujoVapor == "Turbulento"))
            {
                bifasico.C = 20;
            }

            else if ((bifasico.tipoflujoLiquido == "Laminar") && (bifasico.tipoflujoVapor == "Turbulento"))
            {
                bifasico.C = 12;
            }

            else if ((bifasico.tipoflujoLiquido == "Turbulento") && (bifasico.tipoflujoVapor == "Laminar"))
            {
                bifasico.C = 10;
            }

            else if ((bifasico.tipoflujoLiquido == "Laminar") && (bifasico.tipoflujoVapor == "Laminar"))
            {
                bifasico.C = 5;
            }

            bifasico.FhiVapor = 1 + (bifasico.C * bifasico.Martinelli) + (Math.Pow(bifasico.Martinelli, 2));
            textBox69.Text = Convert.ToString(bifasico.FhiVapor);
        }

        //Cálculo de APfriccionLiquido LockHart Martinelli
        private void button48_Click(object sender, EventArgs e)
        {
            bifasico.APfricLiquido = bifasico.FhiLiquido * bifasico.APL;
            textBox70.Text = Convert.ToString(bifasico.APfricLiquido);
        }

        //Cálculo APfriccionVapor LockHart Martinelli
        private void button37_Click(object sender, EventArgs e)
        {
            bifasico.APfricVapor = bifasico.FhiVapor * bifasico.APG;
            textBox58.Text = Convert.ToString(bifasico.APfricVapor);
        }

        //Cálculos APFricción LockHart Martinelli
        private void button49_Click(object sender, EventArgs e)
        {
            if (bifasico.numreynoldliquido > 4000)
            {
                bifasico.APfricLockhartMartinelli = bifasico.APfricLiquido;
                textBox71.Text = Convert.ToString(bifasico.APfricLockhartMartinelli);
            }

            else if (bifasico.numreynoldliquido < 4000)
            {
                bifasico.APfricLockhartMartinelli = bifasico.APfricVapor;
                textBox71.Text = Convert.ToString(bifasico.APfricLockhartMartinelli);
            }
        }

        //Cálculo Temperatura Saturación LockHart Martinelli
        private void button25_Click(object sender, EventArgs e)
        {
            bifasico.pressurein = Convert.ToDouble(textBox43.Text) / 10;
            bifasico.temperaturein = bifasico.region4.T4_p(bifasico.pressurein) - 273.15;
            textBox42.Text = Convert.ToString(bifasico.temperaturein);
        }

        //Cálculo Presión Saturación LockHart Martinelli
        private void button24_Click(object sender, EventArgs e)
        {
            bifasico.temperaturein = Convert.ToDouble(textBox42.Text) + 273.15;
            bifasico.pressurein = bifasico.region4.p4_T(bifasico.temperaturein) * 10;
            textBox43.Text = Convert.ToString(bifasico.pressurein);
        }

        //Cálculo Densidad Líquido Saturado LockHart Martinelli
        private void button27_Click(object sender, EventArgs e)
        {
            //Primero cálculo de la Región 
            bifasico.temperaturein = Convert.ToDouble(textBox42.Text) + 273.15;
            bifasico.pressurein = Convert.ToDouble(textBox43.Text) / 10;
            bifasico.densityinliquido = bifasico.calculodensidadliquido(bifasico.temperaturein, bifasico.pressurein);
            textBox38.Text = Convert.ToString(bifasico.densityinliquido);
        }

        //Cálculo Densidad Vapor Saturado LockHart Martinelli
        private void button29_Click(object sender, EventArgs e)
        {
            //Primero cálculo de la Región 
            bifasico.temperaturein = Convert.ToDouble(textBox42.Text) + 273.15;
            bifasico.pressurein = Convert.ToDouble(textBox43.Text) / 10;
            bifasico.densityinvapor = bifasico.calculodensidadvapor(bifasico.temperaturein, bifasico.pressurein);
            textBox44.Text = Convert.ToString(bifasico.densityinvapor);
        }

        //Cálculo de la Viscosidad Dinámica del Líquido Saturado LockHart Martinelli
        private void button28_Click(object sender, EventArgs e)
        {
            bifasico.densityinliquido = Convert.ToDouble(textBox38.Text);
            bifasico.viscosidaddinamicaliquido = bifasico.calculoviscosidaddinamicaliquido(bifasico.densityinliquido, bifasico.temperaturein, bifasico.pressurein);
            //Cálculo de la VISCOSIDAD CINEMÁTICA
            bifasico.viscosidadcinematicaliquido = 1000000 * (bifasico.viscosidaddinamicaliquido / bifasico.densityinliquido);

            textBox39.Text = Convert.ToString(bifasico.viscosidadcinematicaliquido);
            textBox40.Text = Convert.ToString(bifasico.viscosidaddinamicaliquido);
        }

        //Cálculo de la Viscosidad Dinámica del Vapor Saturado LockHart Martinelli
        private void button26_Click(object sender, EventArgs e)
        {
            bifasico.densityinvapor = Convert.ToDouble(textBox44.Text);
            bifasico.viscosidaddinamicavapor = bifasico.calculoviscosidaddinamicavapor(bifasico.densityinvapor, bifasico.temperaturein, bifasico.pressurein);
            //Cálculo de la VISCOSIDAD CINEMÁTICA
            bifasico.viscosidadcinematicavapor = 1000000 * (bifasico.viscosidaddinamicavapor / bifasico.densityinvapor);

            textBox36.Text = Convert.ToString(bifasico.viscosidadcinematicavapor);
            textBox37.Text = Convert.ToString(bifasico.viscosidaddinamicavapor);
        }

        //Cálculo Tensión Superficial LockHart Martinelli
        private void button23_Click(object sender, EventArgs e)
        {
            bifasico.surfacetension = bifasico.calculotensionsuperficial(bifasico.temperaturein);
            textBox34.Text = Convert.ToString(bifasico.surfacetension);
        }

        //Cálculo del Diámetro Hidráulico LockHart Martinelli
        private void button35_Click(object sender, EventArgs e)
        {
            bifasico.diametrointerior = Convert.ToDouble(textBox54.Text) / 1000;
            bifasico.diametrohidraulico = bifasico.calculodiametrohidraulico(bifasico.diametrointerior);
            textBox53.Text = Convert.ToString(bifasico.areafluido);
            textBox52.Text = Convert.ToString(bifasico.perimetrofluido);
            textBox51.Text = Convert.ToString(bifasico.diametrohidraulico);
        }

        //Cálculo de la Densidad Total LockHart Martinelli
        private void button30_Click(object sender, EventArgs e)
        {
            bifasico.titulo = Convert.ToDouble(textBox35.Text) / 100;
            bifasico.densityintotal = Math.Pow(((bifasico.titulo / bifasico.densityinvapor) + ((1 - bifasico.titulo) / bifasico.densityinliquido)), -1);
            textBox45.Text = Convert.ToString(bifasico.densityintotal);
        }

        //Cálculo de la Velocidad LockHart Martinelli
        private void button31_Click(object sender, EventArgs e)
        {
            bifasico.caudalmasico = Convert.ToDouble(textBox41.Text);
            bifasico.velocidad = bifasico.calculovelocidad(bifasico.caudalmasico, bifasico.areafluido, bifasico.densityintotal);
            textBox55.Text = Convert.ToString(bifasico.velocidad);
        }

        //Cálculo del Número de Reynold Liquido Saturado LockHart Martinelli
        private void button34_Click(object sender, EventArgs e)
        {
            bifasico.velocidad = Convert.ToDouble(textBox55.Text);
            bifasico.numreynoldliquido = bifasico.calculonumreynoldsliquido(bifasico.densityinliquido, bifasico.velocidad, bifasico.diametrohidraulico, bifasico.viscosidaddinamicaliquido);
            textBox50.Text = Convert.ToString(bifasico.numreynoldliquido);

            if (bifasico.numreynoldliquido > 4000)
            {
                bifasico.tipoflujoLiquido = "Turbulento";
                textBox49.Text = bifasico.tipoflujoLiquido;
            }

            else if (bifasico.numreynoldliquido < 2300)
            {
                bifasico.tipoflujoLiquido = "Laminar";
                textBox49.Text = bifasico.tipoflujoLiquido;
            }

            else if ((2300 < bifasico.numreynoldliquido) && (bifasico.numreynoldliquido < 4000))
            {
                bifasico.tipoflujoLiquido = "Transitorio";
                textBox49.Text = bifasico.tipoflujoLiquido;
            }
        }

        //Cálculo del Número de Reynold Vapor Saturado LockHart Martinelli
        private void button33_Click(object sender, EventArgs e)
        {
            bifasico.velocidad = Convert.ToDouble(textBox55.Text);
            bifasico.numreynoldvapor = bifasico.calculonumreynoldsvapor(bifasico.densityinvapor, bifasico.velocidad, bifasico.diametrohidraulico, bifasico.viscosidaddinamicavapor);
            textBox48.Text = Convert.ToString(bifasico.numreynoldvapor);

            if (bifasico.numreynoldvapor > 4000)
            {
                bifasico.tipoflujoVapor = "Turbulento";
                textBox47.Text = bifasico.tipoflujoVapor;
            }

            else if (bifasico.numreynoldvapor < 2300)
            {
                bifasico.tipoflujoVapor = "Laminar";
                textBox47.Text = bifasico.tipoflujoVapor;
            }

            else if ((2300 < bifasico.numreynoldvapor) && (bifasico.numreynoldvapor < 4000))
            {
                bifasico.tipoflujoVapor = "Transitorio";
                textBox47.Text = bifasico.tipoflujoVapor;
            }
        }

        //Cálculo Coeficiente Darcy Líquido Saturado LockHart Martinelli
        private void button36_Click(object sender, EventArgs e)
        {
            if (bifasico.tipoflujoLiquido == "Laminar")
            {
                bifasico.coefdarcyliquido = 64 / bifasico.numreynoldliquido;

                bifasico.coefdarcyliquido = Math.Pow((1.82 * Math.Log10(bifasico.numreynoldliquido) - 1.64), -2);
            }

            else if (bifasico.tipoflujoLiquido == "Turbulento")
            {
                bifasico.rugosidad = Convert.ToDouble(textBox57.Text) / 1000;

                Parameter fl = new Parameter(0.01, "CoefDarcyLiquido");
                Parameter[] farray = new Parameter[1];
                farray[0] = fl;

                Func<double> ColebrookL = () => (-2 * Math.Log10(((bifasico.rugosidad / bifasico.diametrohidraulico) / 3.7) + (2.51 / (bifasico.numreynoldliquido * Math.Sqrt(fl))))) - (1 / Math.Sqrt(fl));
                Func<double>[] Colebrookarray = new Func<double>[1];
                Colebrookarray[0] = ColebrookL;

                NewtonRaphson motornewton = new NewtonRaphson(farray, Colebrookarray);

                //Bucle de iteración del método de Newton Raphson
                for (int b = 0; b < 100; b++)
                {
                    motornewton.Iterate();
                }

                bifasico.coefdarcyliquido = farray[0].Value;
            }

            else if (bifasico.tipoflujoLiquido == "Transitorio")
            {

            }

            else
            {
                MessageBox.Show("Error en el Cálculo del coeficient fl de Darcy.");
                textBox56.Text = "0";
            }

            textBox56.Text = Convert.ToString(bifasico.coefdarcyliquido);
        }

        //Cálculo Coeficiente Darcy Vapor Saturado LockHart Martinelli
        private void button32_Click(object sender, EventArgs e)
        {
            if (bifasico.tipoflujoVapor == "Laminar")
            {
                bifasico.coefdarcyvapor = 64 / bifasico.numreynoldvapor;

                bifasico.coefdarcyvapor = Math.Pow((1.82 * Math.Log10(bifasico.numreynoldvapor) - 1.64), -2);
            }

            else if (bifasico.tipoflujoVapor == "Turbulento")
            {
                bifasico.rugosidad = Convert.ToDouble(textBox57.Text) / 1000;

                Parameter fv = new Parameter(0.01, "CoefDarcyVapor");
                Parameter[] farray = new Parameter[1];
                farray[0] = fv;

                Func<double> ColebrookV = () => (-2 * Math.Log10(((bifasico.rugosidad / bifasico.diametrohidraulico) / 3.7) + (2.51 / (bifasico.numreynoldvapor * Math.Sqrt(fv))))) - (1 / Math.Sqrt(fv));
                Func<double>[] Colebrookarray = new Func<double>[1];
                Colebrookarray[0] = ColebrookV;

                NewtonRaphson motornewton = new NewtonRaphson(farray, Colebrookarray);

                //Bucle de iteración del método de Newton Raphson
                for (int b = 0; b < 100; b++)
                {
                    motornewton.Iterate();
                }

                bifasico.coefdarcyvapor = farray[0].Value;
            }

            else if (bifasico.tipoflujoVapor == "Transitorio")
            {

            }

            else
            {
                MessageBox.Show("Error en el Cálculo del coeficient fg de Darcy.");
                textBox46.Text = "0";
            }

            textBox46.Text = Convert.ToString(bifasico.coefdarcyvapor);
        }

        //Calcular PR LockHart Martinelli 1
        private void button68_Click(object sender, EventArgs e)
        {
            bifasico.PR = Math.Log(Math.Pow(bifasico.APL / bifasico.APG, 0.5));
            textBox101.Text = Convert.ToString(bifasico.PR); 
        }

        //Calcular Fhi Liquid LockHart Martinelli 1
        private void button70_Click(object sender, EventArgs e)
        {
            if ((bifasico.numreynoldliquido > 2100) && (bifasico.numreynoldvapor > 2100))
            {
                bifasico.FhiLiquido1 = 1.44 - 0.508 * bifasico.PR + 0.0579 * Math.Pow(bifasico.PR, 2) - 0.000376 * Math.Pow(bifasico.PR, 3) - 0.000444 * Math.Pow(bifasico.PR, 4);               
            }

            else if ((bifasico.numreynoldliquido > 2100) && (bifasico.numreynoldvapor < 2100))
            {
                bifasico.FhiLiquido1 = 1.25 - 0.458 * bifasico.PR + 0.067 * Math.Pow(bifasico.PR, 2) - 0.00213 * Math.Pow(bifasico.PR, 3) - 0.000585 * Math.Pow(bifasico.PR, 4);               
            }

            else if ((bifasico.numreynoldliquido < 2100) && (bifasico.numreynoldvapor > 2100))
            {
                bifasico.FhiLiquido1 = 1.24 - 0.484 * bifasico.PR + 0.072 * Math.Pow(bifasico.PR, 2) - 0.00127 * Math.Pow(bifasico.PR, 3) - 0.00071 * Math.Pow(bifasico.PR, 4);               
            }

            else if ((bifasico.numreynoldliquido < 2100) && (bifasico.numreynoldvapor < 2100))
            {
                bifasico.FhiLiquido1 = 0.979 - 0.444 * bifasico.PR + 0.096 * Math.Pow(bifasico.PR, 2) - 0.00245 * Math.Pow(bifasico.PR, 3) - 0.00144 * Math.Pow(bifasico.PR, 4);             
            }

            textBox103.Text = Convert.ToString(bifasico.FhiLiquido1); 
        }

        //Calcular Fhi Vapor LockHart Martinelli 1
        private void button69_Click(object sender, EventArgs e)
        {
            if ((bifasico.numreynoldliquido > 2100) && (bifasico.numreynoldvapor > 2100))
            {
                bifasico.FhiVapor1 = 1.44 + 0.492 * bifasico.PR + 0.0577 * Math.Pow(bifasico.PR, 2) - 0.000352 * Math.Pow(bifasico.PR, 3) - 0.000432 * Math.Pow(bifasico.PR, 4);
            }

            else if ((bifasico.numreynoldliquido > 2100) && (bifasico.numreynoldvapor < 2100))
            {
                bifasico.FhiVapor1 = 1.25 + 0.542 * bifasico.PR + 0.067 * Math.Pow(bifasico.PR, 2) - 0.00212 * Math.Pow(bifasico.PR, 3) - 0.000583 * Math.Pow(bifasico.PR, 4);
            }

            else if ((bifasico.numreynoldliquido < 2100) && (bifasico.numreynoldvapor > 2100))
            {
                bifasico.FhiVapor1 = 1.24 + 0.516 * bifasico.PR + 0.072 * Math.Pow(bifasico.PR, 2) - 0.00126 * Math.Pow(bifasico.PR, 3) - 0.000706 * Math.Pow(bifasico.PR, 4);
            }

            else if ((bifasico.numreynoldliquido < 2100) && (bifasico.numreynoldvapor < 2100))
            {
                bifasico.FhiVapor1 = 0.979 + 0.555 * bifasico.PR + 0.096 * Math.Pow(bifasico.PR, 2) - 0.00244 * Math.Pow(bifasico.PR, 3) - 0.00144 * Math.Pow(bifasico.PR, 4);
            }

            textBox102.Text = Convert.ToString(bifasico.FhiVapor1); 
        }

        //Cálculo de APL1 LockHart Martinelli 1
        private void button71_Click(object sender, EventArgs e)
        {
            bifasico.APL1 = Math.Pow(Math.Exp(bifasico.FhiLiquido1), 2) * bifasico.APL;
            textBox105.Text = Convert.ToString(bifasico.APL1);
        }

        //Cálculo de APG1 LockHart Martinelli 1
        private void button72_Click(object sender, EventArgs e)
        {
            bifasico.APG1 = Math.Pow(Math.Exp(bifasico.FhiVapor1), 2) * bifasico.APG;
            textBox104.Text = Convert.ToString(bifasico.APG1);
        }

        //Cálculo de AP1 Fricción Lockhart Martinelli 1
        private void button73_Click(object sender, EventArgs e)
        {
            bifasico.APfricLockhartMartinelli1 = Math.Max(bifasico.APL1, bifasico.APG1);
            textBox106.Text = Convert.ToString(bifasico.APfricLockhartMartinelli1);
        }



//---------------------------- Pressure Drop Correlation Selection -----------------------------------------------------------------




        //Calcular Tª Saturación Pressure Drop Method Selection
        private void button41_Click(object sender, EventArgs e)
        {
            bifasico.pressurein = Convert.ToDouble(textBox76.Text) / 10;
            bifasico.temperaturein = bifasico.calculotempsaturacion(bifasico.pressurein);
            textBox75.Text = Convert.ToString(bifasico.temperaturein);
        }

        //Calcular Presión Saturación Pressure Drop Method Selection 
        private void button40_Click(object sender, EventArgs e)
        {
            bifasico.temperaturein = Convert.ToDouble(textBox75.Text) + 273.15;
            bifasico.pressurein = bifasico.calculopresionsaturacion(bifasico.temperaturein);
            textBox76.Text = Convert.ToString(bifasico.pressurein);
        }

        //Calcular Densidad Líquido Saturado Drop Method Selection
        private void button43_Click(object sender, EventArgs e)
        {
            bifasico.temperaturein = Convert.ToDouble(textBox75.Text) + 273.15;
            bifasico.pressurein = Convert.ToDouble(textBox76.Text) / 10;
            bifasico.densityinliquido = bifasico.calculodensidadliquido(bifasico.temperaturein, bifasico.pressurein);
            textBox65.Text = Convert.ToString(bifasico.densityinliquido);
        }

        //Calcular Densidad Vapor Saturado Drop Method Selection
        private void button51_Click(object sender, EventArgs e)
        {
            bifasico.temperaturein = Convert.ToDouble(textBox75.Text) + 273.15;
            bifasico.pressurein = Convert.ToDouble(textBox76.Text) / 10;
            bifasico.densityinvapor = bifasico.calculodensidadvapor(bifasico.temperaturein, bifasico.pressurein);
            textBox77.Text = Convert.ToString(bifasico.densityinvapor);
        }

        //Calcular Viscosidad Líquido Saturado Drop Method Selection
        private void button50_Click(object sender, EventArgs e)
        {
            bifasico.densityinliquido = Convert.ToDouble(textBox65.Text);
            bifasico.viscosidaddinamicaliquido = bifasico.calculoviscosidaddinamicaliquido(bifasico.densityinliquido, bifasico.temperaturein, bifasico.pressurein);
            //Cálculo de la VISCOSIDAD CINEMÁTICA
            bifasico.viscosidadcinematicaliquido = 1000000 * (bifasico.viscosidaddinamicaliquido / bifasico.densityinliquido);

            textBox73.Text = Convert.ToString(bifasico.viscosidaddinamicaliquido);
        }

        //Calcular Viscosidad Vapor Saturado Drop Method Selection
        private void button42_Click(object sender, EventArgs e)
        {
            bifasico.densityinvapor = Convert.ToDouble(textBox77.Text);
            bifasico.viscosidaddinamicavapor = bifasico.calculoviscosidaddinamicavapor(bifasico.densityinvapor, bifasico.temperaturein, bifasico.pressurein);
            //Cálculo de la VISCOSIDAD CINEMÁTICA
            bifasico.viscosidadcinematicavapor = 1000000 * (bifasico.viscosidaddinamicavapor / bifasico.densityinvapor);

            textBox64.Text = Convert.ToString(bifasico.viscosidaddinamicavapor);
        }

        //Calcular Relación de Viscosidades Dinámicas Drop Method Selection
        private void button39_Click(object sender, EventArgs e)
        {
            bifasico.relacionviscosidades = bifasico.viscosidaddinamicaliquido / bifasico.viscosidaddinamicavapor;
            textBox61.Text = Convert.ToString(bifasico.relacionviscosidades);

            if (bifasico.relacionviscosidades < 1000)
            {
                label70.Text = "Pressure Drop Selection: FRIEDEL";
            }

            else if ((bifasico.relacionviscosidades > 1000) && (bifasico.caudalmasico / bifasico.areafluido > 20.5))
            {
                label70.Text = "Pressure Drop Selection: CHISHOLM-BAROCZY";
            }

            else if ((bifasico.relacionviscosidades > 1000) && (bifasico.caudalmasico / bifasico.areafluido < 20.5))
            {
                label70.Text = "Pressure Drop Selection: LOCKHART-MARTINELLI";
            }

        }

        //Calcular Área Transversal del Fluido
        private void button52_Click(object sender, EventArgs e)
        {
            bifasico.diametrointerior = Convert.ToDouble(textBox63.Text) / 1000;
            bifasico.areafluido = (3.1416 * bifasico.diametrointerior * bifasico.diametrointerior) / 4;
            textBox62.Text = Convert.ToString(bifasico.areafluido);
            textBox72.Text = Convert.ToString(bifasico.caudalmasico / bifasico.areafluido);
        }
        


      }
}
