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
    public partial class BifasicoBoilingHeatCal : Form
    {
        public double viscosidaddinamica = 0;
        public double viscosidadcinematica = 0;

        public RegionSelection selecregion = new RegionSelection();
        public Region1 region1 = new Region1();
        public Region2 region2 = new Region2();
        public Region_3 region3 = new Region_3();
        public Region_4 region4 = new Region_4();
        public Region5 region5 = new Region5();

        public double densityinliquido = 0;
        public double densityinvapor = 0;
        public double temperaturein = 0;
        public double pressurein = 0;
        public double diametrointerior = 0;
        public double areafluido = 0;
        public double perimetrofluido = 0;
        public double diametrohidraulico = 0;
        public double numreynoldliquido = 0;
        public double numreynoldvapor = 0;
        public double velocidad = 0;
        public string tipoflujo = "";
        public double coefdarcyliquido = 0;
        public double coefdarcyvapor = 0;
        public double rugosidad = 0;
        public double caudalmasico = 0;
        public double calorespecifico = 0;
        public double conductividadtermica = 0;
        public double longitudtuberia = 0;
        public double perdidacargamonofasico = 0;
        public double numeroPrandtl = 0;
        public double numeroNusselt = 0;
        public double coefpelicula = 0;
        public double calor = 0;
        public double tempfluido = 0;
        public double temppared = 0;

        public BifasicoBoilingHeatCal()
        {
            InitializeComponent();
        }

        //Cálculo del HTC según Chen 1966 
        private void button1_Click_1(object sender, EventArgs e)
        {
            //Toma de datos del interface del usuario 




        }     

      }
}
