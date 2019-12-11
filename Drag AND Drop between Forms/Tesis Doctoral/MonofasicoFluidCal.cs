using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Monofasico
{
    public partial class MonofasicoFluidCal : Form
    {
        Monofasicoliquido monofasico = new Monofasicoliquido();

        public MonofasicoFluidCal()
        {
            InitializeComponent();
        }

        //Cálculo de todos los Resultados del Cuadro de Diálogo
        private void button3_Click(object sender, EventArgs e)
        {   
            //Cálculamos la temperatura media
            monofasico.tempfluido = Convert.ToDouble(textBox19.Text);
            monofasico.twall = Convert.ToDouble(textBox18.Text);

            monofasico.temperaturein = (monofasico.tempfluido + monofasico.twall) / 2;

            textBox11.Text = Convert.ToString(monofasico.temperaturein);
            
            //Llamadas a las diferentes funciones de forma secuencial para calcular los resultados intermedios

            // 1. Cálculo de la Densidad.
            button4_Click(sender,e);

            // 2. Cálculo de la Viscosidad.
            button8_Click(sender,e);

            // 3. Cálculo del Diámetro Hidráulico.
            button5_Click_1(sender, e);
            
            // 4. Cálculo de la Velocidad.
            button9_Click_1(sender, e);

            // 5. Cálculo del Nº Reynold.
            button6_Click_1(sender, e);

            // 6. Cálculo del Coeficiente de Darcy f.
            button7_Click(sender, e);

            // 7. 

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Hide();
        }       

        //Botón de Cálculo de la DENSIDAD
        private void button4_Click(object sender, EventArgs e)
        {
            //Primero cálculo de la Región 
            monofasico.temperaturein = Convert.ToDouble(textBox11.Text) + 273.15;
            monofasico.pressurein = Convert.ToDouble(textBox10.Text) / 10;
            monofasico.densityin = monofasico.calculodensidad(monofasico.temperaturein, monofasico.pressurein);
            textBox4.Text = Convert.ToString(monofasico.densityin);
        }       
       
        //Cálculo del Coeficiente de Darcy "f"
        private void button7_Click(object sender, EventArgs e)
        {
            monofasico.rugosidad = Convert.ToDouble(textBox12.Text) / 1000;
            monofasico.coefdarcy = monofasico.calculocoefdarcy(monofasico.numreynold, monofasico.rugosidad, monofasico.diametrohidraulico, monofasico.tipoflujo);
            textBox13.Text= Convert.ToString(monofasico.coefdarcy);
        }       

        //Botón del Cálculo de la Viscosidad Dinámica
        private void button8_Click(object sender, EventArgs e)
        {
            monofasico.viscosidaddinamica = monofasico.calculoviscosidaddinamica(monofasico.densityin, monofasico.temperaturein, monofasico.pressurein);
            //Cálculo de la VISCOSIDAD CINEMÁTICA
            monofasico.viscosidadcinematica = 1000000 * (monofasico.viscosidaddinamica / monofasico.densityin);

            textBox2.Text = Convert.ToString(monofasico.viscosidadcinematica);
            textBox1.Text = Convert.ToString(monofasico.viscosidaddinamica);
        }             
        
        //Cálculo de la PÉRDIDA DE CARGA EN MONOFÁSICO
        private void button13_Click(object sender, EventArgs e)
        {
            monofasico.longitudtuberia = Convert.ToDouble(textBox24.Text);
            monofasico.perdidacargamonofasico = monofasico.coefdarcy * (monofasico.longitudtuberia / monofasico.diametrointerior) * (Math.Pow(monofasico.velocidad, 2) / (2 * 9.80665));
            textBox25.Text = Convert.ToString(monofasico.perdidacargamonofasico);
        }       
  
        //Botón del Cálculo del Diámetro Hidráulico
        private void button5_Click_1(object sender, EventArgs e)
        {
            monofasico.diametrointerior = Convert.ToDouble(textBox5.Text) / 1000;
            monofasico.diametrohidraulico = monofasico.calculodiametrohidraulico(monofasico.diametrointerior);
            textBox6.Text = Convert.ToString(monofasico.areafluido);
            textBox7.Text = Convert.ToString(monofasico.perimetrofluido);
            textBox8.Text = Convert.ToString(monofasico.diametrohidraulico);
        }

        //Botón de cálculo de la Velocidad
        private void button9_Click_1(object sender, EventArgs e)
        {
            monofasico.caudalmasico = Convert.ToDouble(textBox21.Text);
            monofasico.velocidad = monofasico.calculovelocidad(monofasico.caudalmasico, monofasico.areafluido, monofasico.densityin);
            textBox3.Text = Convert.ToString(monofasico.velocidad);
        }

        //Botón para el Cálculo del Número de Reynolds
        private void button6_Click_1(object sender, EventArgs e)
        {
            monofasico.velocidad = Convert.ToDouble(textBox3.Text);
            monofasico.numreynold = monofasico.calculonumreynolds(monofasico.densityin, monofasico.velocidad, monofasico.diametrohidraulico, monofasico.viscosidaddinamica);
            textBox9.Text = Convert.ToString(monofasico.numreynold);
            textBox14.Text = monofasico.tipoflujo;
        }

        //Botón del Cálculo del Calor Específico Isobárico Cp
        private void button11_Click_1(object sender, EventArgs e)
        {
            //Primero cálculo de la Región 
            monofasico.temperaturein = Convert.ToDouble(textBox11.Text) + 273.15;
            monofasico.pressurein = Convert.ToDouble(textBox10.Text) / 10;
            monofasico.calorespecifico = monofasico.calculocalorespisob(monofasico.pressurein, monofasico.temperaturein);
            textBox15.Text = Convert.ToString(monofasico.calorespecifico);
        }

        //Botón de Cálculo de la Conductividad Térmica K
        private void button12_Click_1(object sender, EventArgs e)
        {
            double temperature = 0;
            temperature = Convert.ToDouble(textBox11.Text) + 273.15;
            double rho = 0;
            rho = Convert.ToDouble(textBox4.Text);

            monofasico.conductividadtermica = monofasico.calculoconductividad(temperature, rho);

            textBox23.Text = Convert.ToString(monofasico.conductividadtermica);
        }

        //Botón de Cálculo de Número de Prandtl
        private void button10_Click_1(object sender, EventArgs e)
        {
            monofasico.numeroPrandtl = monofasico.calculonumprandtl(monofasico.calorespecifico,monofasico.viscosidaddinamica, monofasico.conductividadtermica);
            textBox22.Text = Convert.ToString(monofasico.numeroPrandtl);
        }

        //Botón Cálculo de Número de Nusselt Gnielinski
        private void button14_Click_1(object sender, EventArgs e)
        {
            //Cálculo del Número de Prandtl1
            double temperatura1 = Convert.ToDouble(textBox19.Text) + 273.15;
            double temperatura2 = Convert.ToDouble(textBox18.Text) + 273.15;
            double presion = Convert.ToDouble(textBox10.Text) / 10;
            monofasico.numeroNusseltGnielinski = monofasico.calculonusselgnielinski(presion, temperatura1, temperatura2);

            textBox16.Text = Convert.ToString(monofasico.numeroNusseltGnielinski);
        }

        //Botón para cálculo del  Numero de Nusselt Petukhov
        private void button17_Click_1(object sender, EventArgs e)
        {
            //Cálculo del Número de Prandtl1
            double temperatura1 = Convert.ToDouble(textBox19.Text) + 273.15;
            double temperatura2 = Convert.ToDouble(textBox18.Text) + 273.15;
            double presion = Convert.ToDouble(textBox10.Text) / 10;
            int n = Convert.ToInt16(textBox26.Text);
            monofasico.numeroNusseltPetukhov = monofasico.calculonusselPetukhov(presion, temperatura1, temperatura2, n);

            textBox16.Text = Convert.ToString(monofasico.numeroNusseltPetukhov);
        }

        //Cálculo Número de Nusselt de Dittus Boelter
        private void button3_Click_1(object sender, EventArgs e)
        {
          monofasico.coefpeliculaDittusBoelter = monofasico.calculoHTCDittusBoelter(monofasico.numreynold, monofasico.numeroPrandtl, monofasico.conductividadtermica, monofasico.diametrohidraulico);
          monofasico.numeroNusseltDittusBoelter = monofasico.coefpeliculaDittusBoelter * monofasico.diametrohidraulico / monofasico.conductividadtermica;
          textBox16.Text = Convert.ToString(monofasico.numeroNusseltDittusBoelter);
        }

        //Cálculo del Coeficiente de Película h(W/m2 K)
        private void button15_Click_1(object sender, EventArgs e)
        {
            //Coeficiente de Película HTC Gnielinski
            if (checkBox1.Checked == true)
            {
                monofasico.coefpeliculaGnielinski = monofasico.calculoHTCGnielinski(monofasico.conductividadtermica, monofasico.numeroNusseltGnielinski, monofasico.diametrointerior);
                textBox17.Text = Convert.ToString(monofasico.coefpeliculaGnielinski);
            }

            //Coeficiente de Película HTC Petukhov
            else if (checkBox2.Checked == true)
            {
                monofasico.coefpeliculaPetukhov = monofasico.calculoHTCPetukhov(monofasico.conductividadtermica, monofasico.numeroNusseltPetukhov, monofasico.diametrointerior);
                textBox17.Text = Convert.ToString(monofasico.coefpeliculaPetukhov);
            }

            //Coeficiente de Película HTC Dittus-Boelter
            else if (checkBox3.Checked == true)
            {
                monofasico.coefpeliculaDittusBoelter = monofasico.calculoHTCDittusBoelter(monofasico.numreynold, monofasico.numeroPrandtl, monofasico.conductividadtermica, monofasico.diametrohidraulico);
                textBox17.Text = Convert.ToString(monofasico.coefpeliculaDittusBoelter);
            }
        }

        //Cálculo del calor q(W/m)
        private void button16_Click_1(object sender, EventArgs e)
        {
            monofasico.twall = Convert.ToDouble(textBox18.Text);
            monofasico.tempfluido = Convert.ToDouble(textBox19.Text);

            if (checkBox1.Checked == true)
            {
                //Calor Q= h * D * pi * (Tpared - Tfluido)
                monofasico.calorGnielinski = monofasico.calculocalorGnielinski(monofasico.coefpeliculaGnielinski, monofasico.diametrohidraulico, monofasico.twall, monofasico.tempfluido);
                textBox20.Text = Convert.ToString(monofasico.calorGnielinski);
            }

            else if (checkBox2.Checked == true)
            {
                //Calor Q= h * D * pi * (Tpared - Tfluido)
                monofasico.calorPetukhov = monofasico.calculocalorPetukhov(monofasico.coefpeliculaPetukhov, monofasico.diametrohidraulico, monofasico.twall, monofasico.tempfluido);
                textBox20.Text = Convert.ToString(monofasico.calorPetukhov);
            }

            else if (checkBox3.Checked == true)
            {
                //Calor Q= h * D * pi * (Tpared - Tfluido)
                monofasico.calorDittusBoelter = monofasico.calculocalorDittusBoelter(monofasico.coefpeliculaDittusBoelter, monofasico.diametrohidraulico, monofasico.twall, monofasico.tempfluido);
                textBox20.Text = Convert.ToString(monofasico.calorDittusBoelter);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                checkBox2.Checked = false;
                checkBox3.Checked = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                checkBox1.Checked = false;
                checkBox3.Checked = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                checkBox1.Checked = false;
                checkBox2.Checked = false;
            }
        }

        //Inicializar el Cálculo, ponemos todas las variables a cero.
        private void button18_Click(object sender, EventArgs e)
        {
            monofasico.Reset();
        }        
    }
}
