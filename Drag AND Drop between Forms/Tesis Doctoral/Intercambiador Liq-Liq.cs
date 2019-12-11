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

using Monofasico;

namespace HeatExchangers
{
    public partial class HeatExchangerLiqLiq : Form
    {
        public HeatExchangerPreDesign prediseño = new HeatExchangerPreDesign();

        public Monofasicoliquido hotfluid = new Monofasicoliquido();
        public Monofasicoliquido coldfluid = new Monofasicoliquido();

        public HeatExchangerNominal intercambiadornominal= new HeatExchangerNominal();
        public HETransitoryCondenser HEtransitorioCondensador = new HETransitoryCondenser();

        public HeatExchangerNominalDiferenciasFinitas intercambiadordifefinitas = new HeatExchangerNominalDiferenciasFinitas();
        
        public HeatExchangerLiqLiq()
        {
            InitializeComponent();
        }

        //Calcular Fluido Caliente (Hot Fluid)
        private void button4_Click(object sender, EventArgs e)
        {
            //Data Input and Average Temperature Calculation
            hotfluid.caudalmasico = Convert.ToDouble(textBox1.Text);
            hotfluid.pressurein = Convert.ToDouble(textBox2.Text)/10;            
            hotfluid.tin = Convert.ToDouble(textBox3.Text)+273.15;
            hotfluid.tout = Convert.ToDouble(textBox4.Text)+273.15;
            hotfluid.temperaturein=(hotfluid.tin+hotfluid.tout)/2;
            textBox5.Text = Convert.ToString(hotfluid.temperaturein - 273.15);

            //Fluid Properties Calculation
            hotfluid.densityin = hotfluid.calculodensidad(hotfluid.temperaturein, hotfluid.pressurein);
            textBox8.Text = Convert.ToString(hotfluid.densityin);
            hotfluid.viscosidaddinamica=hotfluid.calculoviscosidaddinamica(hotfluid.densityin, hotfluid.temperaturein, hotfluid.pressurein);
            textBox7.Text = Convert.ToString(hotfluid.viscosidaddinamica);
            hotfluid.viscosidadcinematica = hotfluid.calculoviscosidadcinematica(hotfluid.viscosidaddinamica, hotfluid.densityin);
            textBox6.Text = Convert.ToString(hotfluid.viscosidadcinematica);
            hotfluid.calorespecifico = hotfluid.calculocalorespisob(hotfluid.pressurein, hotfluid.temperaturein);
            textBox16.Text = Convert.ToString(hotfluid.calorespecifico);
            hotfluid.conductividadtermica = hotfluid.calculoconductividad(hotfluid.temperaturein, hotfluid.densityin);
            textBox23.Text = Convert.ToString(hotfluid.conductividadtermica);

            //Fluid Velocity Calculation
            hotfluid.diametrointerior = Convert.ToDouble(textBox13.Text)/1000;
            hotfluid.diametrohidraulico = hotfluid.calculodiametrohidraulico(hotfluid.diametrointerior);
            textBox12.Text= Convert.ToString(hotfluid.areafluido);
            textBox11.Text= Convert.ToString(hotfluid.perimetrofluido);
            textBox10.Text = Convert.ToString(hotfluid.diametrohidraulico);
            hotfluid.velocidad = hotfluid.calculovelocidad(hotfluid.caudalmasico, hotfluid.areafluido, hotfluid.densityin);
            textBox15.Text = Convert.ToString(hotfluid.velocidad);

            //Non-dimensional variables Calculation
            hotfluid.numreynold = hotfluid.calculonumreynolds(hotfluid.densityin, hotfluid.velocidad, hotfluid.diametrohidraulico, hotfluid.viscosidaddinamica);
            textBox9.Text = Convert.ToString(hotfluid.numreynold);
            hotfluid.numeroPrandtl = hotfluid.calculonumprandtl(hotfluid.calorespecifico, hotfluid.viscosidaddinamica, hotfluid.conductividadtermica);
            textBox22.Text = Convert.ToString(hotfluid.numeroPrandtl);
            textBox14.Text = Convert.ToString(hotfluid.tipoflujo);
            hotfluid.twall=Convert.ToDouble(textBox49.Text)+273.15;

            hotfluid.rugosidad = Convert.ToDouble(textBox51.Text)/1000;
            hotfluid.coefdarcy = hotfluid.calculocoefdarcy(hotfluid.numreynold, hotfluid.rugosidad, hotfluid.diametrohidraulico, hotfluid.tipoflujo);
            textBox52.Text = Convert.ToString(hotfluid.coefdarcy);

            hotfluid.numeroNusseltGnielinski = hotfluid.calculonusselgnielinski(hotfluid.pressurein, hotfluid.temperaturein, hotfluid.twall);
            textBox17.Text = Convert.ToString(hotfluid.numeroNusseltGnielinski);
            hotfluid.numeroNusseltPetukhov = hotfluid.calculonusselPetukhov(hotfluid.pressurein, hotfluid.temperaturein, hotfluid.twall, hotfluid.opcionPetukhov);
            textBox18.Text = Convert.ToString(hotfluid.numeroNusseltPetukhov);
            
            //IMPORTANTE DUDA!!!!!!!!!!!
            //Para calcular el HTC de Gnielinski y de Petukhov hemos utilizado el Diamtero Interior, sin embargo, para calcular el HTC de DittusBoelter hemos utilizado el Diámetro Hidráulico
            //preguntar esta DUDA en la diferencia de utilziación del Dinterior o Dhidraulico
            hotfluid.coefpeliculaGnielinski = hotfluid.calculoHTCGnielinski(hotfluid.conductividadtermica, hotfluid.numeroNusseltGnielinski, hotfluid.diametrointerior);
            textBox19.Text = Convert.ToString(hotfluid.coefpeliculaGnielinski);
            hotfluid.coefpeliculaPetukhov = hotfluid.calculoHTCPetukhov(hotfluid.conductividadtermica, hotfluid.numeroNusseltPetukhov, hotfluid.diametrointerior);         
            textBox21.Text = Convert.ToString(hotfluid.coefpeliculaPetukhov);
            hotfluid.coefpeliculaDittusBoelter = hotfluid.calculoHTCDittusBoelter(hotfluid.numreynold, hotfluid.numeroPrandtl, hotfluid.conductividadtermica, hotfluid.diametrohidraulico);
            textBox20.Text = Convert.ToString(hotfluid.coefpeliculaDittusBoelter);

            hotfluid.calorGnielinski = hotfluid.calculocalorGnielinski(hotfluid.coefpeliculaGnielinski, hotfluid.diametrohidraulico, hotfluid.twall, hotfluid.temperaturein);
            textBox48.Text = Convert.ToString(hotfluid.calorGnielinski);
            
            hotfluid.calorPetukhov = hotfluid.calculocalorPetukhov(hotfluid.coefpeliculaPetukhov, hotfluid.diametrohidraulico, hotfluid.twall, hotfluid.temperaturein);
            textBox50.Text = Convert.ToString(hotfluid.calorPetukhov);
            
            hotfluid.calorDittusBoelter = hotfluid.calculocalorDittusBoelter(hotfluid.coefpeliculaDittusBoelter, hotfluid.diametrohidraulico, hotfluid.twall, hotfluid.temperaturein);
            textBox53.Text = Convert.ToString(hotfluid.calorDittusBoelter);
        }

        //Asignamos la opción de la lista para el cálculo de Nusselt Petukhov HOT FLUID
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            hotfluid.opcionPetukhov = comboBox1.SelectedIndex;

            if (hotfluid.opcionPetukhov == 0)
            {
                //MessageBox.Show("Elegido el elemento número 0.");
            }

            else if (hotfluid.opcionPetukhov== 1)
            {
                //MessageBox.Show("Elegido el elemento número 1.");
            }

            else if (hotfluid.opcionPetukhov == 2)
            {
                //MessageBox.Show("Elegido el elemento número 2.");
            }
        }

        //Calculate Heat Exchanger AREA
        private void button5_Click(object sender, EventArgs e)
        {
            intercambiadornominal.thi = Convert.ToDouble(textBox76.Text);
            intercambiadornominal.tho = Convert.ToDouble(textBox75.Text);
            intercambiadornominal.tci = Convert.ToDouble(textBox72.Text);
            intercambiadornominal.tco = Convert.ToDouble(textBox73.Text);

            intercambiadornominal.HTCc = Convert.ToDouble(textBox77.Text);
            intercambiadornominal.HTCh = Convert.ToDouble(textBox78.Text);

            intercambiadornominal.HTCfh = Convert.ToDouble(textBox61.Text);
            intercambiadornominal.HTCfc = Convert.ToDouble(textBox62.Text);

            intercambiadornominal.k = Convert.ToDouble(textBox68.Text);

            intercambiadornominal.din=Convert.ToDouble(textBox80.Text);
            intercambiadornominal.dout = Convert.ToDouble(textBox79.Text);

            intercambiadornominal.mc=Convert.ToDouble(textBox74.Text);
            intercambiadornominal.mh=Convert.ToDouble(textBox71.Text);

            intercambiadornominal.cpc = Convert.ToDouble(textBox117.Text);
            intercambiadornominal.cph = Convert.ToDouble(textBox118.Text);

            //Cálculo de AT1 y de AT2  
            intercambiadornominal.AT1 = intercambiadornominal.calculoAT1(intercambiadornominal.tci, intercambiadornominal.tco, intercambiadornominal.thi, intercambiadornominal.tho);
            textBox69.Text = Convert.ToString(intercambiadornominal.AT1);
            intercambiadornominal.AT2 = intercambiadornominal.calculoAT2(intercambiadornominal.tci, intercambiadornominal.tco, intercambiadornominal.thi, intercambiadornominal.tho);
            textBox70.Text = Convert.ToString(intercambiadornominal.AT2);

            //Cálculo de LMTD
            intercambiadornominal.LMTD = intercambiadornominal.calculoLMTD(intercambiadornominal.AT1, intercambiadornominal.AT2);
            textBox59.Text = Convert.ToString(intercambiadornominal.LMTD);

            //Cálculo de F
            intercambiadornominal.F = intercambiadornominal.calculoF(intercambiadornominal.tci, intercambiadornominal.tco, intercambiadornominal.thi, intercambiadornominal.tho);
            textBox60.Text = Convert.ToString(intercambiadornominal.F);

            //Cálculo de U (h1, h2, hf1, hf2, K, din, dout)
            intercambiadornominal.U = intercambiadornominal.calculoU(intercambiadornominal.HTCh, intercambiadornominal.HTCfh, intercambiadornominal.k, intercambiadornominal.dout, intercambiadornominal.din, intercambiadornominal.HTCc, intercambiadornominal.HTCfc);
            textBox64.Text = Convert.ToString(intercambiadornominal.U);

            //Cálculo de Qc
            intercambiadornominal.Qc=intercambiadornominal.calculoQc(intercambiadornominal.mc,intercambiadornominal.cpc,intercambiadornominal.tci,intercambiadornominal.tco);
            textBox67.Text = Convert.ToString(intercambiadornominal.Qc);

            //Cálculo de Qh
            intercambiadornominal.Qh = intercambiadornominal.calculoQh(intercambiadornominal.mh, intercambiadornominal.cph, intercambiadornominal.thi, intercambiadornominal.tho);
            textBox81.Text = Convert.ToString(intercambiadornominal.Qh);      

            //Cálculo del AreaTotal de Intercambio
            intercambiadornominal.Atotal = intercambiadornominal.calculoArea(intercambiadornominal.Qc, intercambiadornominal.U, intercambiadornominal.LMTD, intercambiadornominal.F);
            textBox66.Text = Convert.ToString(intercambiadornominal.Atotal);

            //Cálculo de Qu
            intercambiadornominal.Qu = intercambiadornominal.calculoQu(intercambiadornominal.Atotal, intercambiadornominal.U, intercambiadornominal.LMTD, intercambiadornominal.F);
            textBox82.Text = Convert.ToString(intercambiadornominal.Qu);      

            //Cálculo de Cmin, Cmax,C
            intercambiadornominal.Cmin = intercambiadornominal.calculoCmin(intercambiadornominal.mh, intercambiadornominal.cph, intercambiadornominal.mc, intercambiadornominal.cpc);
            textBox82.Text = Convert.ToString(intercambiadornominal.Cmin);   
            intercambiadornominal.Cmax = intercambiadornominal.calculoCmax(intercambiadornominal.mh, intercambiadornominal.cph, intercambiadornominal.mc, intercambiadornominal.cpc);
            textBox82.Text = Convert.ToString(intercambiadornominal.Cmax);   
            intercambiadornominal.C = intercambiadornominal.calculoC(intercambiadornominal.Cmin, intercambiadornominal.Cmax);
            textBox82.Text = Convert.ToString(intercambiadornominal.C);   

            //Cálculo de Qmax
            intercambiadornominal.Qmax = intercambiadornominal.calculoQmax(intercambiadornominal.Cmin, intercambiadornominal.thi, intercambiadornominal.tci);
            textBox82.Text = Convert.ToString(intercambiadornominal.Qmax);   

            //Cálculo de NTU,N
            intercambiadornominal.NTU = intercambiadornominal.calculoNTU(intercambiadornominal.U, intercambiadornominal.Atotal, intercambiadornominal.Cmin);
            textBox82.Text = Convert.ToString(intercambiadornominal.NTU);   
            intercambiadornominal.N = intercambiadornominal.NTU;
            textBox82.Text = Convert.ToString(intercambiadornominal.N);   

            //Cálculo de Eficiencia
            intercambiadornominal.eficiencia = intercambiadornominal.calculoEficiencia(intercambiadornominal.C, intercambiadornominal.N);
            textBox82.Text = Convert.ToString(intercambiadornominal.eficiencia);   
        }
        
        //Elección de la Configuración para calcular F del intercambiadornominal
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            intercambiadornominal.configuracion = comboBox3.SelectedIndex;

            if (intercambiadornominal.configuracion == 0)
            {
                //MessageBox.Show("Elegido el elemento número 0. Cocurrent.");
            }

            else if (intercambiadornominal.configuracion == 1)
            {
                //MessageBox.Show("Elegido el elemento número 1. Counter current.");
            }

            else if (intercambiadornominal.configuracion == 2)
            {
                //MessageBox.Show("Elegido el elemento número 2. Shell and Tube.");
            }
        }

        //Asignamos la opción de la lista para el cálculo de Nusselt Petukhov COLD FLUID
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            coldfluid.opcionPetukhov = comboBox2.SelectedIndex;

            if (coldfluid.opcionPetukhov == 0)
            {
                //MessageBox.Show("Elegido el elemento número 0.");
            }

            else if (coldfluid.opcionPetukhov == 1)
            {
                //MessageBox.Show("Elegido el elemento número 1.");
            }

            else if (coldfluid.opcionPetukhov == 2)
            {
                //MessageBox.Show("Elegido el elemento número 2.");
            }
        }

        //Cálculo del Tho (cálculos del predimensionamiento)
        private void button6_Click(object sender, EventArgs e)
        {
            prediseño.mc = Convert.ToDouble(textBox89.Text);
            prediseño.mh = Convert.ToDouble(textBox85.Text);

            prediseño.thi=Convert.ToDouble(textBox86.Text);
            prediseño.tco=Convert.ToDouble(textBox87.Text);
            prediseño.tci = Convert.ToDouble(textBox88.Text);

            prediseño.cpc = Convert.ToDouble(textBox95.Text);
            prediseño.cph = Convert.ToDouble(textBox96.Text);

            prediseño.tho=prediseño.calculotho(prediseño.thi, prediseño.tci, prediseño.tco, prediseño.mc, prediseño.mh, prediseño.cpc, prediseño.cph);
            textBox84.Text = Convert.ToString(prediseño.tho);
        }

        //Cálculo del mh (cálculos del predimensionamiento)
        private void button7_Click(object sender, EventArgs e)
        {
            prediseño.mc = Convert.ToDouble(textBox92.Text);            

            prediseño.thi = Convert.ToDouble(textBox94.Text);
            prediseño.tho  = Convert.ToDouble(textBox99.Text);

            prediseño.tco = Convert.ToDouble(textBox90.Text);
            prediseño.tci = Convert.ToDouble(textBox91.Text);

            prediseño.cpc = Convert.ToDouble(textBox97.Text);
            prediseño.cph = Convert.ToDouble(textBox98.Text);

            prediseño.mh = prediseño.calculomh(prediseño.thi, prediseño.tci, prediseño.tco, prediseño.mc, prediseño.tho, prediseño.cpc, prediseño.cph);
            textBox93.Text = Convert.ToString(prediseño.mh);
        }

        //Selección del Tipología de intercambiadornominal
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            prediseño.tipologiaintercambiador = comboBox4.SelectedIndex;

            if (prediseño.tipologiaintercambiador == 0)
            {
                //MessageBox.Show("Elegido el elemento número 0.");
            }

            else if (prediseño.tipologiaintercambiador == 1)
            {
                //MessageBox.Show("Elegido el elemento número 1.");
            }

            else if (prediseño.tipologiaintercambiador== 2)
            {
                //MessageBox.Show("Elegido el elemento número 2.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        //Inicializamos los valores de los comboBox
        private void HeatExchangerLiqLiq_Load(object sender, EventArgs e)
        {                  
            comboBox4.SelectedText = "0 - Two Fluids Heat Exchanger";
            comboBox5.SelectedText = "0 - Two Fluids Heat Exchanger";
            comboBox7.SelectedItem = "Type 1 - Counter Current Flows Heat Exchanger";
            comboBox8.SelectedItem = "Type 1 - Tube Side Hot Fluid - Shell Side Cold Fluid";
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            //Al pulsar el Tab 4 Numerical Methods Algorithm Data 1 actualizamos los valores de de las Matrices de Newton Raphson
            if (e.TabPage == tabPage1)
            {
                comboBox1.SelectedItem = "Twall>Tfluid,   n = 0.11";            
            }

            else if (e.TabPage == tabPage3)
            {
                comboBox2.SelectedItem = "Twall>Tfluid,   n = 0.11";
            }

            else if (e.TabPage == tabPage2)
            {
                comboBox3.SelectedItem = "Cocurrent";
            }

            else if (e.TabPage == tabPage8)
            {
                comboBox6.SelectedItem = "0- First Order";
            }
        }

        //Calcular Fluido Frio (Cold Fluid)
        private void button3_Click_1(object sender, EventArgs e)
        {
            //Data Input and Average Temperature Calculation
            coldfluid.caudalmasico = Convert.ToDouble(textBox58.Text);
            coldfluid.pressurein = Convert.ToDouble(textBox57.Text) / 10;
            coldfluid.tin = Convert.ToDouble(textBox56.Text) + 273.15;
            coldfluid.tout = Convert.ToDouble(textBox55.Text) + 273.15;
            coldfluid.temperaturein = (coldfluid.tin + coldfluid.tout) / 2;
            textBox54.Text = Convert.ToString(coldfluid.temperaturein - 273.15);

            //Fluid Properties Calculation
            coldfluid.densityin = coldfluid.calculodensidad(coldfluid.temperaturein, coldfluid.pressurein);
            textBox45.Text = Convert.ToString(coldfluid.densityin);
            coldfluid.viscosidaddinamica = coldfluid.calculoviscosidaddinamica(coldfluid.densityin, coldfluid.temperaturein, coldfluid.pressurein);
            textBox46.Text = Convert.ToString(coldfluid.viscosidaddinamica);
            coldfluid.viscosidadcinematica = coldfluid.calculoviscosidadcinematica(coldfluid.viscosidaddinamica, coldfluid.densityin);
            textBox47.Text = Convert.ToString(coldfluid.viscosidadcinematica);
            coldfluid.calorespecifico = coldfluid.calculocalorespisob(coldfluid.pressurein, coldfluid.temperaturein);
            textBox44.Text = Convert.ToString(coldfluid.calorespecifico);
            coldfluid.conductividadtermica = coldfluid.calculoconductividad(coldfluid.temperaturein, coldfluid.densityin);
            textBox43.Text = Convert.ToString(coldfluid.conductividadtermica);

            //Fluid Velocity Calculation
            coldfluid.diametrointerior = Convert.ToDouble(textBox39.Text) / 1000;
            coldfluid.diametrohidraulico = coldfluid.calculodiametrohidraulico(coldfluid.diametrointerior);
            textBox41.Text = Convert.ToString(coldfluid.areafluido);
            textBox42.Text = Convert.ToString(coldfluid.perimetrofluido);
            textBox40.Text = Convert.ToString(coldfluid.diametrohidraulico);
            coldfluid.velocidad = coldfluid.calculovelocidad(coldfluid.caudalmasico, coldfluid.areafluido, coldfluid.densityin);
            textBox38.Text = Convert.ToString(coldfluid.velocidad);

            //Non-dimensional variables Calculation
            coldfluid.numreynold = coldfluid.calculonumreynolds(coldfluid.densityin, coldfluid.velocidad, coldfluid.diametrohidraulico, coldfluid.viscosidaddinamica);
            textBox36.Text = Convert.ToString(coldfluid.numreynold);
            coldfluid.numeroPrandtl = coldfluid.calculonumprandtl(coldfluid.calorespecifico, coldfluid.viscosidaddinamica, coldfluid.conductividadtermica);
            textBox34.Text = Convert.ToString(coldfluid.numeroPrandtl);
            textBox37.Text = Convert.ToString(coldfluid.tipoflujo);
            coldfluid.twall = Convert.ToDouble(textBox29.Text) + 273.15;

            coldfluid.rugosidad = Convert.ToDouble(textBox28.Text) / 1000;
            coldfluid.coefdarcy = coldfluid.calculocoefdarcy(coldfluid.numreynold, coldfluid.rugosidad, coldfluid.diametrohidraulico, coldfluid.tipoflujo);
            textBox27.Text = Convert.ToString(coldfluid.coefdarcy);

            coldfluid.numeroNusseltGnielinski = coldfluid.calculonusselgnielinski(coldfluid.pressurein, coldfluid.temperaturein, coldfluid.twall);
            textBox35.Text = Convert.ToString(coldfluid.numeroNusseltGnielinski);
            coldfluid.numeroNusseltPetukhov = coldfluid.calculonusselPetukhov(coldfluid.pressurein, coldfluid.temperaturein, coldfluid.twall, coldfluid.opcionPetukhov);
            textBox33.Text = Convert.ToString(coldfluid.numeroNusseltPetukhov);

            //IMPORTANTE DUDA!!!!!!!!!!!
            //Para calcular el HTC de Gnielinski y de Petukhov hemos utilizado el Diamtero Interior, sin embargo, para calcular el HTC de DittusBoelter hemos utilizado el Diámetro Hidráulico
            //preguntar esta DUDA en la diferencia de utilziación del Dinterior o Dhidraulico
            coldfluid.coefpeliculaGnielinski = coldfluid.calculoHTCGnielinski(coldfluid.conductividadtermica, coldfluid.numeroNusseltGnielinski, coldfluid.diametrointerior);
            textBox32.Text = Convert.ToString(coldfluid.coefpeliculaGnielinski);
            coldfluid.coefpeliculaPetukhov = coldfluid.calculoHTCPetukhov(coldfluid.conductividadtermica, coldfluid.numeroNusseltPetukhov, coldfluid.diametrointerior);
            textBox30.Text = Convert.ToString(coldfluid.coefpeliculaPetukhov);
            coldfluid.coefpeliculaDittusBoelter = coldfluid.calculoHTCDittusBoelter(coldfluid.numreynold, coldfluid.numeroPrandtl, coldfluid.conductividadtermica, coldfluid.diametrohidraulico);
            textBox31.Text = Convert.ToString(coldfluid.coefpeliculaDittusBoelter);

            coldfluid.calorGnielinski = coldfluid.calculocalorGnielinski(coldfluid.coefpeliculaGnielinski, coldfluid.diametrohidraulico, coldfluid.twall, coldfluid.temperaturein);
            textBox26.Text = Convert.ToString(coldfluid.calorGnielinski);

            coldfluid.calorPetukhov = coldfluid.calculocalorPetukhov(coldfluid.coefpeliculaPetukhov, coldfluid.diametrohidraulico, coldfluid.twall, coldfluid.temperaturein);
            textBox25.Text = Convert.ToString(coldfluid.calorPetukhov);

            coldfluid.calorDittusBoelter = coldfluid.calculocalorDittusBoelter(coldfluid.coefpeliculaDittusBoelter, coldfluid.diametrohidraulico, coldfluid.twall, coldfluid.temperaturein);
            textBox24.Text = Convert.ToString(coldfluid.calorDittusBoelter);
        }

        //Cálculo de Qc y Qh en fase de Prediseño
        private void button13_Click(object sender, EventArgs e)
        {
            //Cálcjlo de Qc
            prediseño.Qc = prediseño.calculoQc(prediseño.mc,prediseño.tci,prediseño.tco,prediseño.cpc);
            textBox120.Text=Convert.ToString(prediseño.Qc);

            //Cálculo de Qh
            prediseño.Qh = prediseño.calculoQh(prediseño.mh, prediseño.thi, prediseño.tho, prediseño.cph);
            textBox119.Text = Convert.ToString(prediseño.Qh);
        }


        //Cálculo Transitorio de un intercambiadornominal de Calor 
        private void button15_Click(object sender, EventArgs e)
        {
            //Toma de Datos del Usuario
            HEtransitorioCondensador .mc = Convert.ToDouble(textBox133.Text);
            HEtransitorioCondensador .tci = Convert.ToDouble(textBox121.Text);
            HEtransitorioCondensador .tco = Convert.ToDouble(textBox134.Text);
            HEtransitorioCondensador .tavg = HEtransitorioCondensador .calculotavg(HEtransitorioCondensador .tci, HEtransitorioCondensador .tco);
            textBox124.Text = Convert.ToString(HEtransitorioCondensador .tavg);
            HEtransitorioCondensador .cpc=Convert.ToDouble(textBox125.Text);
            HEtransitorioCondensador .rhoc = Convert.ToDouble(textBox126.Text);
            HEtransitorioCondensador .L = Convert.ToDouble(textBox123.Text);
            HEtransitorioCondensador .S = Convert.ToDouble(textBox127.Text);
            HEtransitorioCondensador .v = Convert.ToDouble(textBox128.Text);
            HEtransitorioCondensador .M = Convert.ToDouble(textBox135.Text);
            HEtransitorioCondensador .tf = HEtransitorioCondensador .calculotf(HEtransitorioCondensador .L, HEtransitorioCondensador .v);
            textBox129.Text = Convert.ToString(HEtransitorioCondensador .tf);
            HEtransitorioCondensador .g = HEtransitorioCondensador .calculog(HEtransitorioCondensador .tf);
            textBox130.Text = Convert.ToString(HEtransitorioCondensador .g);
            HEtransitorioCondensador .Q = Convert.ToDouble(textBox130.Text);
            HEtransitorioCondensador .t = Convert.ToDouble(textBox131.Text);
            HEtransitorioCondensador .Tt = HEtransitorioCondensador .calculoTt(HEtransitorioCondensador .Q, HEtransitorioCondensador .mc, HEtransitorioCondensador .cpc, HEtransitorioCondensador .t, HEtransitorioCondensador .tf, HEtransitorioCondensador .tci);
            textBox122.Text=Convert.ToString(HEtransitorioCondensador .Tt); 
        }

        //Cálculo del Intercambiador por Diferencias Finitas
        private void button16_Click(object sender, EventArgs e)
        {
            //Toma de datos a través del Interface de Usuario
            intercambiadordifefinitas.intervtiempo=Convert.ToDouble(textBox164.Text);            
            intercambiadordifefinitas.initialtime=Convert.ToDouble(textBox168.Text);
            intercambiadordifefinitas.finaltime = Convert.ToDouble(textBox167.Text);

            intercambiadordifefinitas.ntimes = 1+Convert.ToInt16((intercambiadordifefinitas.finaltime - intercambiadordifefinitas.initialtime) / intercambiadordifefinitas.intervtiempo);
            textBox162.Text = Convert.ToString(intercambiadordifefinitas.ntimes);
            
            intercambiadordifefinitas.AZ = Convert.ToDouble(textBox163.Text);
            intercambiadordifefinitas.ngeometric = Convert.ToInt16(textBox161.Text);
            intercambiadordifefinitas.L = Convert.ToDouble(textBox169.Text);
            intercambiadordifefinitas.metodointegracion = comboBox6.SelectedIndex;

            intercambiadordifefinitas.tubediaint= Convert.ToDouble(textBox159.Text);
            intercambiadordifefinitas.thk = Convert.ToDouble(textBox171.Text);
            intercambiadordifefinitas.tubediaext = intercambiadordifefinitas.tubediaint - (2 * intercambiadordifefinitas.thk);
            intercambiadordifefinitas.Ntube = Convert.ToInt16(textBox160.Text);
            intercambiadordifefinitas.lateralAt = Convert.ToDouble(textBox158.Text);
            intercambiadordifefinitas.crossAt = Convert.ToDouble(textBox175.Text);
            intercambiadordifefinitas.cpt = Convert.ToDouble(textBox157.Text);
            intercambiadordifefinitas.rhot = Convert.ToDouble(textBox156.Text);
            intercambiadordifefinitas.Ft = Convert.ToDouble(textBox155.Text);
            intercambiadordifefinitas.mt = intercambiadordifefinitas.Ft * intercambiadordifefinitas.rhot;
            intercambiadordifefinitas.Tot = Convert.ToDouble(textBox154.Text);
            intercambiadordifefinitas.Pot = Convert.ToDouble(textBox165.Text);
            intercambiadordifefinitas.rugosidadtube = Convert.ToDouble(textBox146.Text);

            intercambiadordifefinitas.shelldia = Convert.ToDouble(textBox148.Text);
            intercambiadordifefinitas.lateralAs = Convert.ToDouble(textBox149.Text);
            intercambiadordifefinitas.crossAs = Convert.ToDouble(textBox176.Text);
            intercambiadordifefinitas.cps = Convert.ToDouble(textBox150.Text);
            intercambiadordifefinitas.rhos = Convert.ToDouble(textBox151.Text);
            intercambiadordifefinitas.Fs = Convert.ToDouble(textBox152.Text);
            intercambiadordifefinitas.ms = intercambiadordifefinitas.Fs * intercambiadordifefinitas.rhos;
            intercambiadordifefinitas.Tos = Convert.ToDouble(textBox153.Text);
            intercambiadordifefinitas.Pos = Convert.ToDouble(textBox166.Text);
            intercambiadordifefinitas.rugosidadshell = Convert.ToDouble(textBox174.Text);

            intercambiadordifefinitas.HTCs = Convert.ToDouble(textBox137.Text);
            intercambiadordifefinitas.HTCt= Convert.ToDouble(textBox144.Text);
            intercambiadordifefinitas.HTCsd = Convert.ToDouble(textBox173.Text);
            intercambiadordifefinitas.HTCtd = Convert.ToDouble(textBox172.Text);

            intercambiadordifefinitas.k = Convert.ToDouble(textBox145.Text);
            intercambiadordifefinitas.cpt = Convert.ToDouble(textBox157.Text);

            intercambiadordifefinitas.Uo = Convert.ToDouble(textBox147.Text);

            //Integración
            intercambiadordifefinitas.j=1;
            intercambiadordifefinitas.i=1;

            intercambiadordifefinitas.Tt=new double[intercambiadordifefinitas.ngeometric+1,intercambiadordifefinitas.ntimes];
            intercambiadordifefinitas.Ts=new double[intercambiadordifefinitas.ngeometric+1,intercambiadordifefinitas.ntimes];
            intercambiadordifefinitas.Twt = new double[intercambiadordifefinitas.ngeometric + 1, intercambiadordifefinitas.ntimes];
            intercambiadordifefinitas.Tws = new double[intercambiadordifefinitas.ngeometric + 1, intercambiadordifefinitas.ntimes];
            intercambiadordifefinitas.qt = new double[intercambiadordifefinitas.ngeometric+1, intercambiadordifefinitas.ntimes];
            intercambiadordifefinitas.qs = new double[intercambiadordifefinitas.ngeometric+1, intercambiadordifefinitas.ntimes];
            intercambiadordifefinitas.Qt = new double[intercambiadordifefinitas.ntimes];
            intercambiadordifefinitas.Qs = new double[intercambiadordifefinitas.ntimes];
            intercambiadordifefinitas.U = new double[intercambiadordifefinitas.ngeometric + 1, intercambiadordifefinitas.ntimes];   
            intercambiadordifefinitas.time=new double[intercambiadordifefinitas.ntimes];

            intercambiadordifefinitas.shellfluid = new Monofasicoliquido[intercambiadordifefinitas.ngeometric + 1, intercambiadordifefinitas.ntimes];
            intercambiadordifefinitas.tubefluid = new Monofasicoliquido[intercambiadordifefinitas.ngeometric + 1, intercambiadordifefinitas.ntimes];

            intercambiadordifefinitas.Tot=Convert.ToDouble(textBox154.Text);

            for (int j = 0; j <= intercambiadordifefinitas.ngeometric; j++)
            {
                intercambiadordifefinitas.Tt[j, 0] = intercambiadordifefinitas.Tot;                
            }

            intercambiadordifefinitas.Tos = Convert.ToDouble(textBox153.Text);

            for (int j = 0; j <= intercambiadordifefinitas.ngeometric; j++)
            {
                intercambiadordifefinitas.Ts[j, 0] = intercambiadordifefinitas.Tos;
            }
             
            for (int i=0;i<intercambiadordifefinitas.ntimes;i++)
            {
                intercambiadordifefinitas.Tt[0, i] = intercambiadordifefinitas.Tot;
                intercambiadordifefinitas.Ts[0, i] = intercambiadordifefinitas.Tos;
            }

            //Calculo de las Temperaturas (K) en cada Nodo y para cada intervalo de Tiempo (U constante e introducida por el Usuario)

            //intercambiadordifefinitas.Uo = intercambiadordifefinitas.calculoUo(intercambiadordifefinitas.tubediaint, intercambiadordifefinitas.tubediaext, intercambiadordifefinitas.HTCt, intercambiadordifefinitas.HTCs, intercambiadordifefinitas.HTCsd, intercambiadordifefinitas.HTCtd, intercambiadordifefinitas.k);
            
            for (int i = 1; i < intercambiadordifefinitas.ntimes; i++)
            {
                for (int j = 1; j <= intercambiadordifefinitas.ngeometric; j++)
                {
                    //Calcular las Temperaturas considerando U constante

                    //Intercambiador con Flujos a Contracorriente
                    if (comboBox7.SelectedIndex == 0)
                    {
                        intercambiadordifefinitas.Tt[j, i] = intercambiadordifefinitas.caltubcontrac(j, i, intercambiadordifefinitas.rhot, intercambiadordifefinitas.cpt);
                        intercambiadordifefinitas.Ts[j, i] = intercambiadordifefinitas.calshellcontrac(j, i, intercambiadordifefinitas.rhos, intercambiadordifefinitas.cps);
                    }

                    //Intercambiador con Flujos en Paralelo
                    else if (comboBox7.SelectedIndex == 1)
                    {
                        intercambiadordifefinitas.Tt[j, i] = intercambiadordifefinitas.caltubparalelos(j, i, intercambiadordifefinitas.rhot, intercambiadordifefinitas.cpt);
                        intercambiadordifefinitas.Ts[j, i] = intercambiadordifefinitas.calshellparalelos(j, i, intercambiadordifefinitas.rhos, intercambiadordifefinitas.cps);                       
                    }

                    //Intercambiador Boiler
                    else if (comboBox7.SelectedIndex == 3)
                    {
                        //Tube Side Hot Fluid: Tt=Tsaturación Ts=variable 
                        if (comboBox8.SelectedIndex == 0)
                        {
                            intercambiadordifefinitas.Tt[j, i] = intercambiadordifefinitas.Tot;
                            intercambiadordifefinitas.Ts[j, i] = intercambiadordifefinitas.calshellparalelos(j, i, intercambiadordifefinitas.rhos, intercambiadordifefinitas.cps);                       
                        }

                        //Tube Side Cold Fluid: Ts=Tsaturación Tt=variable
                        else if (comboBox8.SelectedIndex == 1)
                        {
                            intercambiadordifefinitas.Tt[j, i] = intercambiadordifefinitas.caltubparalelos(j, i, intercambiadordifefinitas.rhot, intercambiadordifefinitas.cpt);
                            intercambiadordifefinitas.Ts[j, i] = intercambiadordifefinitas.Tos;
                        }
                    }

                    //Intercambiador Condenser
                    else if (comboBox7.SelectedIndex == 4)
                    {
                        //Tube Side Hot Fluid: Tt=variable Ts=Tsaturación
                        if (comboBox8.SelectedIndex == 0)
                        {
                            intercambiadordifefinitas.Tt[j, i] = intercambiadordifefinitas.caltubparalelos(j, i, intercambiadordifefinitas.rhot, intercambiadordifefinitas.cpt);
                            intercambiadordifefinitas.Ts[j, i] = intercambiadordifefinitas.Tos;                         
                        }

                        //Tube Side Cold Fluid: Ts=variable Tt=Tsaturación
                        else if (comboBox8.SelectedIndex == 1)
                        {                           
                            intercambiadordifefinitas.Tt[j, i] = intercambiadordifefinitas.Tot;
                            intercambiadordifefinitas.Ts[j, i] = intercambiadordifefinitas.calshellparalelos(j, i, intercambiadordifefinitas.rhos, intercambiadordifefinitas.cps);                       
                        }
                    }
                }            
            }
            
           //Calcular las Temperaturas considerando (U variable según las Temperaturas calculadas)
           if ((checkBox1.Checked == false) && (checkBox2.Checked == true))
           {
              for (int i = 1; i < intercambiadordifefinitas.ntimes; i++)
              {
                  for (int j = 1; j <= intercambiadordifefinitas.ngeometric; j++)
                  {                    
                        intercambiadordifefinitas.shellfluid[j, i] = intercambiadordifefinitas.calculoShellfluid(j, i, intercambiadordifefinitas.Pos, intercambiadordifefinitas.Ts[j, i]);
                        intercambiadordifefinitas.tubefluid[j, i] = intercambiadordifefinitas.calculoTubesfluid(j, i, intercambiadordifefinitas.Pot, intercambiadordifefinitas.Tt[j, i]);          
                        intercambiadordifefinitas.U[j, i] = intercambiadordifefinitas.calculoU(j, i, intercambiadordifefinitas.tubediaint, intercambiadordifefinitas.tubediaext, intercambiadordifefinitas.HTCt, intercambiadordifefinitas.HTCs, intercambiadordifefinitas.HTCsd, intercambiadordifefinitas.HTCtd, intercambiadordifefinitas.k);
                        intercambiadordifefinitas.Tt[j, i] = intercambiadordifefinitas.calculovariableintegracintubos(j, i, intercambiadordifefinitas.rhot, intercambiadordifefinitas.cpt);
                        intercambiadordifefinitas.Ts[j, i] = intercambiadordifefinitas.calculovariableintegracionshell(j, i, intercambiadordifefinitas.rhos, intercambiadordifefinitas.cps);
                  }
              }
           }

            //Calculo de Calor Intercambiado qs,qt,Qs,Qt
            for (int i = 0; i < intercambiadordifefinitas.ntimes; i++)
            {
                for (int j = 0; j <= intercambiadordifefinitas.ngeometric; j++)
                {
                    if (comboBox7.SelectedIndex == 0)
                    {
                       intercambiadordifefinitas.qt[j, i] = intercambiadordifefinitas.calcalortuboscontracor(j, i, intercambiadordifefinitas.Tt[j, i], intercambiadordifefinitas.Ts[intercambiadordifefinitas.ngeometric-j,i], intercambiadordifefinitas.Uo);
                       intercambiadordifefinitas.qs[j, i] = intercambiadordifefinitas.calcalorcarcasacontracor(j, i, intercambiadordifefinitas.Ts[j, i], intercambiadordifefinitas.Tt[intercambiadordifefinitas.ngeometric-j,i], intercambiadordifefinitas.Uo);
                    }

                    else if (comboBox7.SelectedIndex == 1)
                    {
                        intercambiadordifefinitas.qt[j, i] = intercambiadordifefinitas.calcalortubosparalelo(j, i, intercambiadordifefinitas.Tt[j, i], intercambiadordifefinitas.Ts[j, i], intercambiadordifefinitas.Uo);
                        intercambiadordifefinitas.qs[j, i] = intercambiadordifefinitas.calcalorcarcasaparalelo(j, i, intercambiadordifefinitas.Ts[j, i], intercambiadordifefinitas.Tt[j, i], intercambiadordifefinitas.Uo);
                    }
                }
            }

            //Cálculo de las Temperaturas de Pared Tws y Twt
            for (int i = 0; i < intercambiadordifefinitas.ntimes; i++)
            {
                for (int j = 0; j <= intercambiadordifefinitas.ngeometric; j++)
                {
                    //Calcular las Temperaturas considerando U constante

                    //Tube Side Hot Fluid
                    if (comboBox8.SelectedIndex == 0)
                    {
                        intercambiadordifefinitas.Twt[j, i] = intercambiadordifefinitas.calTwtHotFluid(j,i,intercambiadordifefinitas.Tt[j,i],intercambiadordifefinitas.HTCt,intercambiadordifefinitas.qt[j,i]);
                        intercambiadordifefinitas.Tws[j, i] = intercambiadordifefinitas.calTwsHotFluid(j, i, intercambiadordifefinitas.Twt[j, i], intercambiadordifefinitas.k, intercambiadordifefinitas.qt[j, i]);
                    }

                    //Tube Side Cold Fluid
                    else if (comboBox8.SelectedIndex == 1)
                    {
                        intercambiadordifefinitas.Twt[j, i] = intercambiadordifefinitas.calTwtColdFluid(j, i, intercambiadordifefinitas.Tt[j, i], intercambiadordifefinitas.HTCt, intercambiadordifefinitas.qt[j, i]);
                        intercambiadordifefinitas.Tws[j, i] = intercambiadordifefinitas.calTwsColdFluid(j, i, intercambiadordifefinitas.Twt[j, i], intercambiadordifefinitas.k, intercambiadordifefinitas.qt[j, i]);
                    }
                }
            }
            
            double sumaQt = 0;
            double sumaQs = 0;

            //Cálculo del Calor Total (Qt y Qs) del Lado Tubos y Lado Carcasa para cada intervalo de Tiempo sumando el calor intercambiado entre cada Nodo del modelo geométrico.
            for (int i = 0; i < intercambiadordifefinitas.ntimes; i++)
            {
                for (int j = 0; j <= intercambiadordifefinitas.ngeometric; j++)
                {
                    sumaQt = sumaQt + intercambiadordifefinitas.qt[j, i];
                    sumaQs = sumaQs + intercambiadordifefinitas.qs[j, i];
                }
                
                intercambiadordifefinitas.Qt[i]=sumaQt;
                intercambiadordifefinitas.Qs[i] = sumaQs;

                sumaQt = 0;
                sumaQs = 0;
            }           
        }

        //Botón NEXT: Asignamos los valores de Prediseño a los valores iniciales del coldfluid y del hotfluid
        private void button9_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage7;

            coldfluid.caudalmasico = prediseño.mc;
            hotfluid.caudalmasico = prediseño.mh;

            coldfluid.tin = prediseño.tci;
            coldfluid.tout = prediseño.tco;

            hotfluid.tin = prediseño.thi;
            hotfluid.tout = prediseño.tho;

            coldfluid.calorespecifico = prediseño.cpc;
            hotfluid.calorespecifico = prediseño.cph;

            textBox109.Text = Convert.ToString(coldfluid.caudalmasico);
            textBox140.Text = Convert.ToString(coldfluid.pressurein);

            textBox110.Text = Convert.ToString(hotfluid.caudalmasico);
            textBox142.Text = Convert.ToString(hotfluid.pressurein);
        }

        //Cálculo de las Velocidades del Fluido Caliente y del Fluido Frio
        private void button8_Click(object sender, EventArgs e)
        {
            prediseño.tavghot = prediseño.calculotempaveragehot(prediseño.thi, prediseño.tho);
            textBox141.Text = Convert.ToString(prediseño.tavghot);
            prediseño.tavgcold = prediseño.calculotempaveragecold(prediseño.tci, prediseño.tco);
            textBox143.Text = Convert.ToString(prediseño.tavgcold);
            prediseño.presioncoldin=Convert.ToDouble(textBox140.Text);
            prediseño.presionhotin = Convert.ToDouble(textBox142.Text);
            prediseño.densidadcold = prediseño.calculodensidadcold(prediseño.presioncoldin, prediseño.tavgcold);
            textBox138.Text = Convert.ToString(prediseño.densidadcold);
            prediseño.densidadhot = prediseño.calculodensidadhot(prediseño.presionhotin, prediseño.tavghot);
            textBox139.Text= Convert.ToString(prediseño.densidadhot);
            prediseño.diatubos = Convert.ToDouble(textBox101.Text);
            prediseño.areashell = Convert.ToDouble(textBox104.Text);
            prediseño.numtubos = Convert.ToDouble(textBox100.Text);
            prediseño.velocidadtubos = prediseño.calculovelocidadtubos(prediseño.numtubos, prediseño.diatubos, prediseño.mc, prediseño.densidadcold);
            textBox102.Text = Convert.ToString(prediseño.velocidadtubos);
            prediseño.velocidadshell = prediseño.calculovelocidadshell(prediseño.diatubos, prediseño.mc, prediseño.densidadhot);
            textBox103.Text = Convert.ToString(prediseño.velocidadshell);
        }

        //Botón de Next 
        private void button18_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage1;
            
            //Inicializamos los textBoxes con los valores del Prediseño
            textBox1.Text = Convert.ToString(hotfluid.caudalmasico);
            textBox3.Text = Convert.ToString(hotfluid.tin);
            textBox4.Text = Convert.ToString(hotfluid.tout);

            textBox58.Text = Convert.ToString(coldfluid.caudalmasico);
            textBox56.Text = Convert.ToString(coldfluid.tin);
            textBox55.Text = Convert.ToString(coldfluid.tout);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage3;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage5;
        }

        //View Results. Botón para visualizar los resultados en el Control DataGridView
        private void button19_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            DataGridViewTextBoxColumn temp2 = new DataGridViewTextBoxColumn();

            temp2.HeaderText = "Nº time interval";
            temp2.Name = "timeinterval";
            temp2.Width = 50;
            temp2.Visible = true;

            dataGridView1.Columns.Add(temp2);

            DataGridViewTextBoxColumn temp1 = new DataGridViewTextBoxColumn();

            temp1.HeaderText = "Time (sg)";
            temp1.Name = "time";
            temp1.Width = 50;
            temp1.Visible = true;

            dataGridView1.Columns.Add(temp1);

            //Crear Columnas para introducir los valores de las Temperaturas
            if (checkBox3.Checked == true)
            {
                for (int j = 0; j <= intercambiadordifefinitas.ngeometric; j++)
                {
                    DataGridViewTextBoxColumn temp = new DataGridViewTextBoxColumn();

                    temp.HeaderText = "Tt" + Convert.ToString(j) + " (K)";
                    temp.Name = "Tt" + Convert.ToString(j);
                    temp.Width = 50;
                    temp.Visible = true;

                    dataGridView1.Columns.Add(temp);
                }

                for (int j = 0; j <= intercambiadordifefinitas.ngeometric; j++)
                {
                    DataGridViewTextBoxColumn temp = new DataGridViewTextBoxColumn();

                    temp.HeaderText = "Ts" + Convert.ToString(j) + " (K)";
                    temp.Name = "Ts" + Convert.ToString(j);
                    temp.Width = 50;
                    temp.Visible = true;

                    dataGridView1.Columns.Add(temp);
                }
            }

            //Crear Columnas para introducir los valores de los Calores Intercambiados
            if (checkBox4.Checked == true)
            {
                for (int j = 0; j <= intercambiadordifefinitas.ngeometric; j++)
                {
                    DataGridViewTextBoxColumn temp = new DataGridViewTextBoxColumn();

                    temp.HeaderText = "qt" + Convert.ToString(j) + " (W/m2)";
                    temp.Name = "qt" + Convert.ToString(j);
                    temp.Width = 50;
                    temp.Visible = true;

                    dataGridView1.Columns.Add(temp);
                }

                DataGridViewTextBoxColumn temp22 = new DataGridViewTextBoxColumn();

                temp22.HeaderText = "Qt" + " (W/m2)";
                temp22.Name = "Qt";
                temp22.Width = 50;
                temp22.Visible = true;

                dataGridView1.Columns.Add(temp22);

                for (int j = 0; j <= intercambiadordifefinitas.ngeometric; j++)
                {
                    DataGridViewTextBoxColumn temp = new DataGridViewTextBoxColumn();

                    temp.HeaderText = "qs" + Convert.ToString(j) + " (W/m2)";
                    temp.Name = "qs" + Convert.ToString(j);
                    temp.Width = 50;
                    temp.Visible = true;

                    dataGridView1.Columns.Add(temp);
                }
                             
                DataGridViewTextBoxColumn temp33 = new DataGridViewTextBoxColumn();

                temp33.HeaderText = "Qs"+ " (W/m2)";
                temp33.Name = "Qs";
                temp33.Width = 50;
                temp33.Visible = true;

                dataGridView1.Columns.Add(temp33);
            }
            
            for (int i = 0; i < intercambiadordifefinitas.ntimes; i++)
            {
                dataGridView1.Rows.Add();            
            }

            for (int i = 0; i < intercambiadordifefinitas.ntimes; i++)
            {
                intercambiadordifefinitas.time[i] = intercambiadordifefinitas.initialtime + intercambiadordifefinitas.intervtiempo * i;
                dataGridView1.Rows[i].Cells[0].Value = i;
                dataGridView1.Rows[i].Cells[1].Value = intercambiadordifefinitas.time[i];
            }
            
            //Visualizar Temperaturas y No Calores Intercambiados
            if ((checkBox3.Checked == true)&&(checkBox4.Checked == false))
            {
                //Flujo Contracorriente o Flujo en Paralelo
                if ((comboBox7.SelectedIndex == 0) || (comboBox7.SelectedIndex == 1))
                {
                    //Número Filas
                    for (int i = 0; i < intercambiadordifefinitas.ntimes; i++)
                    {
                        //Número Columnas
                        for (int j = 0; j <= intercambiadordifefinitas.ngeometric; j++)
                        {
                            dataGridView1.Rows[i].Cells[j + 2].Value = intercambiadordifefinitas.Tt[j, i];
                        }
                    }
                }

                //Intercambiador Tipo Boiler
                else if (comboBox7.SelectedIndex == 3)
                {
                    //Tube Side Hot Fluid: Tt=Tsaturación Ts=variable 
                    if (comboBox8.SelectedIndex == 0)
                    {
                        //Número Filas
                        for (int i = 0; i < intercambiadordifefinitas.ntimes; i++)
                        {
                            //Número Columnas
                            for (int j = 0; j <= intercambiadordifefinitas.ngeometric; j++)
                            {
                                dataGridView1.Rows[i].Cells[j + 2].Value = intercambiadordifefinitas.Tot;
                                dataGridView1.Rows[i].Cells[j + 2 + intercambiadordifefinitas.ngeometric + 1].Value = intercambiadordifefinitas.Ts[j, i];
                            }
                        }
                    }

                    //Tube Side Cold Fluid: Ts=Tsaturación Tt=variable
                    else if (comboBox8.SelectedIndex == 1)
                    {
                        //Número Filas
                        for (int i = 0; i < intercambiadordifefinitas.ntimes; i++)
                        {
                            //Número Columnas
                            for (int j = 0; j <= intercambiadordifefinitas.ngeometric; j++)
                            {
                                dataGridView1.Rows[i].Cells[j + 2].Value = intercambiadordifefinitas.Tt[j, i];
                                dataGridView1.Rows[i].Cells[j + 2 + intercambiadordifefinitas.ngeometric + 1].Value = intercambiadordifefinitas.Tos;
                            }
                        }
                    }
                }

                //Intercambiador Tipo Condenser
                else if (comboBox7.SelectedIndex == 4)
                {
                    //Tube Side Hot Fluid: Tt=variable Ts=Tsaturación
                    if (comboBox8.SelectedIndex == 0)
                    {
                        //Número Filas
                        for (int i = 0; i < intercambiadordifefinitas.ntimes; i++)
                        {
                            //Número Columnas
                            for (int j = 0; j <= intercambiadordifefinitas.ngeometric; j++)
                            {
                               dataGridView1.Rows[i].Cells[j + 2].Value = intercambiadordifefinitas.Tt[j,i];
                               dataGridView1.Rows[i].Cells[j + 2 + intercambiadordifefinitas.ngeometric + 1].Value  = intercambiadordifefinitas.Tos;
                            }
                        }
                    }

                    //Tube Side Cold Fluid: Ts=variable Tt=Tsaturación
                    else if (comboBox8.SelectedIndex == 1)
                    {
                        //Número Filas
                        for (int i = 0; i < intercambiadordifefinitas.ntimes; i++)
                        {
                            //Número Columnas
                            for (int j = 0; j <= intercambiadordifefinitas.ngeometric; j++)
                            {
                                dataGridView1.Rows[i].Cells[j + 2].Value = intercambiadordifefinitas.Tot;
                                dataGridView1.Rows[i].Cells[j + 2 + intercambiadordifefinitas.ngeometric + 1].Value = intercambiadordifefinitas.Ts[j,i];
                            }
                        }
                    }
                }

                //Flujo en Contracorriente
                if (comboBox7.SelectedIndex == 0)
                {
                    //Número Filas
                    for (int i = 0; i < intercambiadordifefinitas.ntimes; i++)
                    {
                        //Número Columnas
                        for (int j = 0; j <= intercambiadordifefinitas.ngeometric; j++)
                        {
                            dataGridView1.Rows[i].Cells[j + 2 + intercambiadordifefinitas.ngeometric + 1].Value = intercambiadordifefinitas.Ts[intercambiadordifefinitas.ngeometric-j, i];
                        }
                    }
                }

                //Flujo en Paralelo
                else if (comboBox7.SelectedIndex == 1)
                {
                    //Número Filas
                    for (int i = 0; i < intercambiadordifefinitas.ntimes; i++)
                    {
                        //Número Columnas
                        for (int j = 0; j <= intercambiadordifefinitas.ngeometric; j++)
                        {
                            dataGridView1.Rows[i].Cells[j + 2 + intercambiadordifefinitas.ngeometric + 1].Value = intercambiadordifefinitas.Ts[j, i];
                        }
                    }                
                }

                //Flujo en Paralelo
                else if (comboBox7.SelectedIndex == 1)
                {
                    //Número Filas
                    for (int i = 0; i < intercambiadordifefinitas.ntimes; i++)
                    {
                        //Número Columnas
                        for (int j = 0; j <= intercambiadordifefinitas.ngeometric; j++)
                        {
                            dataGridView1.Rows[i].Cells[j + 2 + intercambiadordifefinitas.ngeometric + 1].Value = intercambiadordifefinitas.Ts[j, i];
                        }
                    }
                }
             }

            //Visualizar Calores Intercambiados y Temperaturas
            if ((checkBox3.Checked == true) && (checkBox4.Checked == true))
            {
                if ((comboBox7.SelectedIndex == 0) || (comboBox7.SelectedIndex == 1))
                {
                    //Número Filas
                    for (int i = 0; i < intercambiadordifefinitas.ntimes; i++)
                    {
                        //Número Columnas
                        for (int j = 0; j <= intercambiadordifefinitas.ngeometric; j++)
                        {
                            dataGridView1.Rows[i].Cells[j + 2].Value = intercambiadordifefinitas.Tt[j, i];
                        }
                    }
                }
               
                //Flujo en Paralelo
                if (comboBox7.SelectedIndex == 1)
                {
                    //Número Filas
                    for (int i = 0; i < intercambiadordifefinitas.ntimes; i++)
                    {
                        //Número Columnas
                        for (int j = 0; j <= intercambiadordifefinitas.ngeometric; j++)
                        {
                            dataGridView1.Rows[i].Cells[j + 3 + (2 * intercambiadordifefinitas.ngeometric) + 1].Value = intercambiadordifefinitas.Ts[j, i];
                        }
                    }
                }
                else if (comboBox7.SelectedIndex == 0)
                {
                    //Número Filas
                    for (int i = 0; i < intercambiadordifefinitas.ntimes; i++)
                    {
                        //Número Columnas
                        for (int j = 0; j <= intercambiadordifefinitas.ngeometric; j++)
                        {
                            dataGridView1.Rows[i].Cells[j + 2 + intercambiadordifefinitas.ngeometric + 1].Value = intercambiadordifefinitas.Ts[intercambiadordifefinitas.ngeometric - j, i];
                        }
                    }                
                }

                //Número Filas
                for (int i = 0; i < intercambiadordifefinitas.ntimes; i++)
                {
                    //Número Columnas
                    for (int j = 0; j <= intercambiadordifefinitas.ngeometric; j++)
                    {
                        dataGridView1.Rows[i].Cells[j + 3 + (2 * intercambiadordifefinitas.ngeometric) + 1].Value = intercambiadordifefinitas.qt[j, i];
                    }
                }

                //Número Filas
                for (int i = 0; i < intercambiadordifefinitas.ntimes; i++)
                {
                    dataGridView1.Rows[i].Cells[1+intercambiadordifefinitas.ngeometric + 3 + (2 * intercambiadordifefinitas.ngeometric) + 1].Value = intercambiadordifefinitas.Qt[i];
                }

                //Número Filas
                for (int i = 0; i < intercambiadordifefinitas.ntimes; i++)
                {
                    //Número Columnas
                    for (int j = 0; j <= intercambiadordifefinitas.ngeometric; j++)
                    {
                        dataGridView1.Rows[i].Cells[1+j + 4 + (3 * intercambiadordifefinitas.ngeometric) + 1].Value = intercambiadordifefinitas.qs[j, i];
                    }
                }

                //Número Filas
                for (int i = 0; i < intercambiadordifefinitas.ntimes; i++)
                {
                    dataGridView1.Rows[i].Cells[3 + intercambiadordifefinitas.ngeometric + 3 + (3 * intercambiadordifefinitas.ngeometric) + 1].Value = intercambiadordifefinitas.Qs[i];
                }
            }

            //Visualizar Calores Intercambiados y No Temperaturas
            if ((checkBox3.Checked == false) && (checkBox4.Checked == true))
            {
                //Número Filas
                for (int i = 0; i < intercambiadordifefinitas.ntimes; i++)
                {
                    //Número Columnas
                    for (int j = 0; j <= intercambiadordifefinitas.ngeometric; j++)
                    {
                        dataGridView1.Rows[i].Cells[j + 2].Value = intercambiadordifefinitas.qt[j, i];
                    }
                }

                for (int i = 0; i < intercambiadordifefinitas.ntimes; i++)
                {
                    dataGridView1.Rows[i].Cells[3 + intercambiadordifefinitas.ngeometric].Value = intercambiadordifefinitas.Qt[i];
                }

                //Número Filas
                for (int i = 0; i < intercambiadordifefinitas.ntimes; i++)
                {
                    //Número Columnas
                    for (int j = 0; j <= intercambiadordifefinitas.ngeometric; j++)
                    {
                        dataGridView1.Rows[i].Cells[j + 3 + intercambiadordifefinitas.ngeometric + 1].Value = intercambiadordifefinitas.qs[j, i];
                    }
                }

                for (int i = 0; i < intercambiadordifefinitas.ntimes; i++)
                {
                    dataGridView1.Rows[i].Cells[5 + (2*intercambiadordifefinitas.ngeometric)].Value = intercambiadordifefinitas.Qs[i];
                }
            }
            
            button20_Click(sender, e);
        }

        //Graph Results. Botón para visualizar los Resultados en el Control Graph.
        private void button20_Click(object sender, EventArgs e)
        {
            chart1.Series["Series1"].Points.Clear();
            chart1.Series["Series2"].Points.Clear();

            int intervalo = 0;
            intervalo=Convert.ToInt16(textBox170.Text);

            chart1.ChartAreas[0].AxisY.Minimum = 200;
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Maximum = intercambiadordifefinitas.ngeometric;

            //Flujo en contracorriente
            if (comboBox7.SelectedIndex == 0)
            {
                for (int j = 0; j <= intercambiadordifefinitas.ngeometric; j++)
                {
                    chart1.Series["Series1"].Points.AddXY(j, intercambiadordifefinitas.Tt[j, intervalo]);
                    chart1.Series["Series2"].Points.AddXY(j, intercambiadordifefinitas.Ts[intercambiadordifefinitas.ngeometric-j, intervalo]);
                }
            }

            //Flujo en paralelo
            else if (comboBox7.SelectedIndex == 1)
            {
                for (int j = 0; j <= intercambiadordifefinitas.ngeometric; j++)
                {
                    chart1.Series["Series1"].Points.AddXY(j, intercambiadordifefinitas.Tt[j, intervalo]);
                    chart1.Series["Series2"].Points.AddXY(j, intercambiadordifefinitas.Ts[j, intervalo]);
                }            
            }

            //Boiler
            else if (comboBox7.SelectedIndex == 3)
            {
                for (int j = 0; j <= intercambiadordifefinitas.ngeometric; j++)
                {
                    chart1.Series["Series1"].Points.AddXY(j, intercambiadordifefinitas.Tt[j, intervalo]);
                    chart1.Series["Series2"].Points.AddXY(j, intercambiadordifefinitas.Ts[j, intervalo]);
                }  
            }

            //Condenser
            else if (comboBox7.SelectedIndex == 4)
            {
                for (int j = 0; j <= intercambiadordifefinitas.ngeometric; j++)
                {
                    chart1.Series["Series1"].Points.AddXY(j, intercambiadordifefinitas.Tt[j, intervalo]);
                    chart1.Series["Series2"].Points.AddXY(j, intercambiadordifefinitas.Ts[j, intervalo]);
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                checkBox2.Checked = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                checkBox1.Checked = false;
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
        }
               

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Configuración del Intercambiador:  
            //Type 1 - 0: Counter Current Flows Heat Exchanger
            //Type 2 - 1: Cocurrent Flows Heat Exchanger
            //Type 3 - 2: 2 Tube Passes - 1 Shell Passes Heat Exchanger
            //Type 4 - 3: Boiler
            //Type 5 - 4: Condenser      

            intercambiadordifefinitas.configuracion= comboBox7.SelectedIndex;

            if (intercambiadordifefinitas.configuracion == 0)
            {
                //MessageBox.Show("Elegido el elemento número 0. Counter Cocurrent.");
            }

            else if (intercambiadordifefinitas.configuracion == 1)
            {
                //MessageBox.Show("Elegido el elemento número 1. Cocurrent.");
            }

            else if ( intercambiadordifefinitas.configuracion == 2)
            {
                //MessageBox.Show("Elegido el elemento número 2 Tube Passes - 1 Shell Passes.");
            }

            else if (intercambiadordifefinitas.configuracion == 3)
            {
                //MessageBox.Show("Elegido el elemento número 3 Boiler.");
            }

            else if (intercambiadordifefinitas.configuracion == 4)
            {
                //MessageBox.Show("Elegido el elemento número 4 Condenser.");
            }
        }

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
             //Definición Hot and Cold Fluids
             //Type 1 - 0: Tube Side Hot Fluid - Shell Side Cold Fluid
             //Type 2 - 1: Tube Side Cold Fluid - Shell Side Hot Fluid
         
            intercambiadordifefinitas. Hotfluid= comboBox8.SelectedIndex;

            if (intercambiadordifefinitas. Hotfluid== 0)
            {
                //MessageBox.Show("Elegido el elemento número 0. Tube Side Hot Fluid - Shell Side Cold Fluid.");
            }

            else if (intercambiadordifefinitas. Hotfluid == 1)
            {
                //MessageBox.Show("Elegido el elemento número 1. Tube Side Cold Fluid - Shell Side Hot Fluid.");
            }
        }

        //Cálculo de U(W/m2 K) Coeficiente Total de Transmisión de calor entre Fluido de Carcasa y Fluido de Tubos
        private void button22_Click(object sender, EventArgs e)
        {
            intercambiadordifefinitas.HTCs = Convert.ToDouble(textBox137.Text);
            intercambiadordifefinitas.HTCt = Convert.ToDouble(textBox144.Text);
            intercambiadordifefinitas.HTCsd = Convert.ToDouble(textBox173.Text);
            intercambiadordifefinitas.HTCtd = Convert.ToDouble(textBox172.Text);
            intercambiadordifefinitas.k = Convert.ToDouble(textBox145.Text);
            intercambiadordifefinitas.tubediaint = Convert.ToDouble(textBox159.Text);
            intercambiadordifefinitas.thk = Convert.ToDouble(textBox171.Text);
            intercambiadordifefinitas.tubediaext = intercambiadordifefinitas.tubediaint - (2 * intercambiadordifefinitas.thk);

            intercambiadordifefinitas.Uo = intercambiadordifefinitas.calculoUconstante(intercambiadordifefinitas.tubediaint, intercambiadordifefinitas.tubediaext, intercambiadordifefinitas.HTCt, intercambiadordifefinitas.HTCs, intercambiadordifefinitas.HTCsd, intercambiadordifefinitas.HTCtd, intercambiadordifefinitas.k);
            textBox147.Text = Convert.ToString(intercambiadordifefinitas.Uo);
        }

        //Cálculo del Coeficiente de Transmisión de Calor Lado Carcasa (HTCs)
        private void button23_Click(object sender, EventArgs e)
        {
            intercambiadordifefinitas.shelldia = Convert.ToDouble(textBox148.Text);
            intercambiadordifefinitas.rhos = Convert.ToDouble(textBox151.Text);
            intercambiadordifefinitas.Fs = Convert.ToDouble(textBox152.Text);
            intercambiadordifefinitas.ms = intercambiadordifefinitas.Fs * intercambiadordifefinitas.rhos;
            intercambiadordifefinitas.Tos = Convert.ToDouble(textBox153.Text);
            intercambiadordifefinitas.Pos = Convert.ToDouble(textBox166.Text);
            intercambiadordifefinitas.HTCs = intercambiadordifefinitas.calculoShellfluid(intercambiadordifefinitas.Pos, intercambiadordifefinitas.Tos);
            textBox137.Text = Convert.ToString(intercambiadordifefinitas.HTCs);
        }

        //Cálculo del Coeficiente de Transmisión de Calor Lado Tubos (HTCt)
        private void button24_Click(object sender, EventArgs e)
        {
            intercambiadordifefinitas.tubediaint = Convert.ToDouble(textBox159.Text);
            intercambiadordifefinitas.rhot = Convert.ToDouble(textBox156.Text);
            intercambiadordifefinitas.Ft = Convert.ToDouble(textBox155.Text);
            intercambiadordifefinitas.mt = intercambiadordifefinitas.Ft * intercambiadordifefinitas.rhot;
            intercambiadordifefinitas.Tot = Convert.ToDouble(textBox154.Text);
            intercambiadordifefinitas.Pot = Convert.ToDouble(textBox165.Text);
            intercambiadordifefinitas.HTCt = intercambiadordifefinitas.calculoTubesfluid(intercambiadordifefinitas.Pot, intercambiadordifefinitas.Tot);
            textBox144.Text = Convert.ToString(intercambiadordifefinitas.HTCt);
        }

        //Tubes Lateral Surface Area At(m2)
        private void button25_Click(object sender, EventArgs e)
        {
            intercambiadordifefinitas.tubediaint = Convert.ToDouble(textBox159.Text);
            intercambiadordifefinitas.ngeometric = Convert.ToInt16(textBox161.Text);
            intercambiadordifefinitas.AZ = Convert.ToDouble(textBox163.Text);
            intercambiadordifefinitas.Ntube = Convert.ToInt16(textBox160.Text);
            intercambiadordifefinitas.lateralAt = intercambiadordifefinitas.Ntube * Math.PI * intercambiadordifefinitas.tubediaint * intercambiadordifefinitas.ngeometric * intercambiadordifefinitas.AZ;
            textBox158.Text = Convert.ToString(intercambiadordifefinitas.lateralAt);
        }

        //Shell Lateral Surface Area As(m2)
        private void button26_Click(object sender, EventArgs e)
        {
            intercambiadordifefinitas.ngeometric = Convert.ToInt16(textBox161.Text);
            intercambiadordifefinitas.AZ = Convert.ToDouble(textBox163.Text);
            intercambiadordifefinitas.thk = Convert.ToDouble(textBox171.Text);
            intercambiadordifefinitas.tubediaint = Convert.ToDouble(textBox159.Text);
            intercambiadordifefinitas.tubediaext = intercambiadordifefinitas.tubediaint + (2 * intercambiadordifefinitas.thk);
            //IMPORTANTE !!!: MODIFICAR POR EL DIÁMETRO HIDRÁULICO DEL CANAL ENTRE LOS CUATRO TUBOS.
            intercambiadordifefinitas.shelldia = intercambiadordifefinitas.tubediaext;
            textBox148.Text = Convert.ToString(intercambiadordifefinitas.tubediaext);
            intercambiadordifefinitas.Ntube = Convert.ToInt16(textBox160.Text);           
            intercambiadordifefinitas.lateralAs = intercambiadordifefinitas.Ntube * Math.PI * intercambiadordifefinitas.tubediaext* intercambiadordifefinitas.ngeometric * intercambiadordifefinitas.AZ;
            textBox149.Text = Convert.ToString(intercambiadordifefinitas.lateralAs);
        }

        //Tubes Cross Surface Area At(m2)
        private void button27_Click(object sender, EventArgs e)
        {
            intercambiadordifefinitas.tubediaint = Convert.ToDouble(textBox159.Text);
            intercambiadordifefinitas.Ntube = Convert.ToInt16(textBox160.Text);
            intercambiadordifefinitas.crossAt = intercambiadordifefinitas.Ntube * Math.PI * (intercambiadordifefinitas.tubediaint * intercambiadordifefinitas.tubediaint/4);
            textBox175.Text = Convert.ToString(intercambiadordifefinitas.crossAt);
        }

        //Shell Cross Surface Area As(m2)
        private void button28_Click(object sender, EventArgs e)
        {
            intercambiadordifefinitas.thk = Convert.ToDouble(textBox171.Text);
            intercambiadordifefinitas.tubediaint = Convert.ToDouble(textBox159.Text);
            intercambiadordifefinitas.Ntube = Convert.ToInt16(textBox160.Text);   
            intercambiadordifefinitas.tubediaext = intercambiadordifefinitas.tubediaint + (2 * intercambiadordifefinitas.thk);
            intercambiadordifefinitas.crossAs = intercambiadordifefinitas.Ntube * Math.PI * intercambiadordifefinitas.tubediaext * intercambiadordifefinitas.tubediaext / 4;
            textBox176.Text = Convert.ToString(intercambiadordifefinitas.crossAs);
        }     
    }
}
