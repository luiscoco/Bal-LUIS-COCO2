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
    //Clase de Intercambiadores de Calor (Heat Exchangers)
    public class HETransitory
    {
        //Tipo de Intercambiador (Condenser, Boiler, Preheater,Subcooler)
        public string Tipologia = "";

        //Configuración del Intercambiador (0:Contracorriente, 1:Paralelo y 2:Cruzado)
        public int configuracion = 0;

        //Caudales másicos de fluido frio y caliente 
        public double mh = 0;
        public double mc = 0;

        //Temperaturas de entrada y salida de fluido caliente (ºC)
        public double thi = 0;
        public double tho = 0;
        
        //Temperaturas de entrada y salida de fluido frio (ºC)
        public double tci = 0;
        public double tco = 0;

        //Temperatura media (ºC)
        public double tavg = 0;
        
        //Presiones de los fluidos frio y caliente (bar)
        public double pci = 0;
        public double pco = 0;
        public double phi = 0;
        public double pho = 0;

        //Densidad de los fluidos frio y caliente (Kg/m3)
        public double rhoc = 0;
        public double rhoh = 0;

        //Calor Específico Isobárico Cp (Kj/Kg K)
        public double cpc = 0;
        public double cph = 0;
        //Length L(m)
        public double L = 0;

        //Cross Sectional S (m2)
        public double S = 0;

        //Fluid Velocity (m/sg)
        public double v = 0;

        //Fluid Mass inside Heat Exchanger M
        public double M = 0;

        //Transit Time tf(sg)
        public double tf = 0;

        //Constant g=2/tf=2m'/M
        public double g = 0;

        //Nominal Heat Q(W)
        public double Q=0;

        //Time t
        public double t = 0;

        //Time dependant Fluid out Temperature T(t)
        public double Tt = 0;

        public double calculotavg(double ti, double to)
        {
            tavg=(ti+to)/2;
            return tavg;
        }

        public double calculotf(double L, double v)
        {
            tf = L / v;
            return tf;
        }

        public double calculog(double tf11)
        {
            g = 2 / tf11;
            return g;
        }

        public double calculoTt(double Q11,double mc11,double cpc11,double t11,double tf11,double tci11)
        {
            Tt = ((Q11 / (mc11 * cpc11)) * (1 - Math.Exp(-2 * t11 / tf11)))+tci11;
            return Tt;
        }

    }
}