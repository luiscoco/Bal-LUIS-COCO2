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

namespace HeatExchangers
{
    //Parámetros de Pre-Diseño de un Intercambiador de Calor
    public class HeatExchangerPreDesign
    {      
        public RegionSelection selecregion = new RegionSelection();
        public Region1 region1 = new Region1();
        public Region2 region2 = new Region2();
        public Region_3 region3 = new Region_3();
        public Region_4 region4 = new Region_4();
        public Region5 region5 = new Region5();

        //Tipología de Intercambiador: 0-Heat Exchanger with two Fluids, 1-Boiler, 2-Condenser
        public int tipologiaintercambiador = 0;

        public double mh = 0;
        public double mc = 0;

        public double thi = 0;
        public double tho = 0;
        public double tci = 0;
        public double tco = 0;
        public double tavghot = 0;
        public double tavgcold = 0;

        public double presionhotin = 0;
        public double presioncoldin = 0;

        public double cpc = 0;
        public double cph = 0;

        public double psat = 0;
        public double tsat = 0;

        public double hliqsat = 0;
        public double hvapsat = 0;
        public double entalpiavaporizacion = 0;

        public double numtubos = 0;
        public double diatubos = 0;
        public double areatubos=0;
        public double areashell = 0;
        public double densidadhot = 0;
        public double densidadcold = 0;
        public double velocidadtubos = 0;
        public double velocidadshell = 0;

        public double Qc = 0;
        public double Qh = 0;

        public double calculotho(double thi,double tci,double tco,double mc,double mh,double cpc, double cph)
        {           
            //Tipología de Intercambiador: 0-Heat Exchanger with two Fluids, 1-Boile/-Condenser

            if (tipologiaintercambiador == 0)
            {
                tho = thi - ((mc * cpc * (tco - tci)) / (mh * cph));
                return tho;
            }

            else if (tipologiaintercambiador == 1)
            {
                entalpiavaporizacion = calculoentalpiavaporizacion(tci+273.15);
                tho = thi - ((mc * entalpiavaporizacion) / (mh * cph));
                return tho;
            }

            else 
            {
                MessageBox.Show("Error in Tipology selection.");
                return 0;
            }
        }

        public double calculomh(double thi, double tci, double tco, double mc, double tho, double cpc, double cph)
        {
            //Tipología de Intercambiador: 0-Heat Exchanger with two Fluids, 1-Boiler, 2-Condenser

            if (tipologiaintercambiador == 0)
            {
               mh=(mc*cpc*(tco-tci))/(cph*(thi-tho));
               return mh;                       
            }

            else if (tipologiaintercambiador == 1)
            {
                entalpiavaporizacion = calculoentalpiavaporizacion(tci+273.15);
                mh = (mc *entalpiavaporizacion) / (cph * (thi - tho));
                return mh; 
            }

            else 
            {
                MessageBox.Show("Error in Tipology selection.");
                return 0;
            }
        }

        public double calculoentalpiavaporizacion(double temperaturasaturacion)
        {
            psat=region4.p4_T(temperaturasaturacion);
            hliqsat = region4.h4L_p(psat);
            hvapsat = region4.h4V_p(psat);
            entalpiavaporizacion = hvapsat - hliqsat;
            return entalpiavaporizacion;
        }

        public double calculoQc(double mc11,double tci11, double tco11, double cpc11)
        {
            Qc=mc11*cpc11*(tco11-tci11);
            return Qc;
        }

        public double calculoQh(double mh11, double thi11, double tho11, double cph11)
        {
            Qh = mh11 * cph11 * (thi11-tho11);
            return Qh;
        }

        public double calculovelocidadtubos(double numtubos11,double diatubos11,double mc11,double densidad11)
        {
            velocidadtubos = (mc11*1/densidad11)/(numtubos11*((Math.PI * diatubos11 * diatubos11) / 4));
            return velocidadtubos;
        }

        public double calculovelocidadshell(double diatubos22,double mc22,double densidad22)
        {
            velocidadshell = (mc22 * 1 / densidad22) / ((Math.PI * diatubos22 * diatubos22) / 4);
            return velocidadshell;
        }

        public double calculotempaveragecold(double t1, double t2)
        {
            tavgcold = (t1 + t2) / 2;
            return tavgcold;
        }

        public double calculotempaveragehot(double t1, double t2)
        {
            tavghot = (t1 + t2) / 2;
            return tavghot;
        }

        public double calculodensidadcold(double presion33, double temperatura33)
        {
            selecregion.region_pT(presion33, temperatura33);

            //Cálculo de la Densidad

            if (selecregion.regionpT == 1)
            {
                densidadcold = 1 / region1.v1_pT(presion33, temperatura33);
            }

            else if (selecregion.regionpT == 2)
            {
                densidadcold = 1 / region2.v2_pT(presion33, temperatura33);
            }

            else if (selecregion.regionpT == 3)
            {
                MessageBox.Show("Region 3: no hay datos para el cálculo de a Densidad.");
            }

            else if (selecregion.regionpT == 4)
            {
                MessageBox.Show("Region 4: no hay datos para el cálculo de a Densidad.");
            }

            else if (selecregion.regionpT == 5)
            {
                densidadcold = 1 / region5.v5_pT(presion33, temperatura33);
            }

            else
            {
                MessageBox.Show("Error al seleccionar la Región.");
            }
        
            return densidadcold;
        }

        public double calculodensidadhot(double presion22, double temperatura22)
        {
            selecregion.region_pT(presion22, temperatura22);

            //Cálculo de la Densidad

            if (selecregion.regionpT == 1)
            {
                densidadhot = 1 / region1.v1_pT(presion22, temperatura22);
            }

            else if (selecregion.regionpT == 2)
            {
                densidadhot = 1 / region2.v2_pT(presion22, temperatura22);
            }

            else if (selecregion.regionpT == 3)
            {
                MessageBox.Show("Region 3: no hay datos para el cálculo de a Densidad.");
            }

            else if (selecregion.regionpT == 4)
            {
                MessageBox.Show("Region 4: no hay datos para el cálculo de a Densidad.");
            }

            else if (selecregion.regionpT == 5)
            {
                densidadhot = 1 / region5.v5_pT(presion22, temperatura22);
            }

            else
            {
                MessageBox.Show("Error al seleccionar la Región.");
            }
        
            return densidadhot;
        }
    }
}