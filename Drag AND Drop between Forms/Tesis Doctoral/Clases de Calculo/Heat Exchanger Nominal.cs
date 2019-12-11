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
    public class HeatExchangerNominal
    {
        //Tipo de Intercambiador (Condenser, Boiler, Preheater,Subcooler)
        public string Tipologia = "";

        //Configuración del Intercambiador (0:Contracorriente, 1:Paralelo y 2:Cruzado)
        public int configuracion = 0;

        //Caudales másicos de fluido frio y caliente
        public double mh = 0;
        public double mc = 0;

        //Temperaturas de entrada y salida de fluido caliente
        public double thi = 0;
        public double tho = 0;
        
        //Temperaturas de entrada y salida de fluido frio
        public double tci = 0;
        public double tco = 0;
        
        //Presiones de los fluidos frio y caliente
        public double pci = 0;
        public double pco = 0;
        public double phi = 0;
        public double pho = 0;

        //Calores especificos isobáricos de fluido frio y caliente
        public double cph = 0;
        public double cpc = 0;
        public double HTCh=0;
        public double HTCc=0;

        //Fouling factor del lado de fluido frio y caliente
        public double HTCfh = 0;
        public double HTCfc = 0;
        //Conductividad térmica del material de los tubos
        public double k = 0;

        //Diámetro interior y exterior de tubos y su espesor
        public double din=0;
        public double dout = 0;
        public double thk = 0;
        
        //Diferencia de temperaturas entre fluidos frio y caliente
        public double AT1 = 0;
        public double AT2 = 0;
        //Coeficiente Total de Transmisión de Calor
        public double U = 0;
        //Temperatura logarítmica media
        public double LMTD = 0;
        //Factor de Corrección según el tipo de Configuración 
        public double F = 0;
        public double R = 0;
        public double S = 0;
        //Area Total de intercambio de Calor
        public double Atotal=0;
        public double numtubos = 0;
        public double longitud = 0;

        //Calor Intercambiado
        //Qu=U*A*LMTD*F
        public double Qu=0;
        //Qh=mh*Cph*(thi-tho)
        public double Qh=0;
        //Qc=mc*Cpc*(tco-tci)
        public double Qc=0;
        //Qmax=Cmin*(thi-tci)
        public double Qmax=0;
        //Heat Density Qd(W/m2)
        public double Qd = 0;

        //Cálculo de la Eficiencia y NTU
        public double eficiencia = 0;
        public double NTU = 0;
        public double N = 0;
                
        //Cálculo de Cmin y Cmax y C
        public double Cmin = 0;
        public double Cmax = 0;
        public double C = 0;      
         
        public double calculoAT1(double tci1, double tco1, double thi1, double tho1)
        {
            if (configuracion == 0)
            { 
               AT1=thi1-tci1;
            }

            else if (configuracion == 1)
            { 
               AT1=thi1-tco1;
            }

            else if (configuracion==2)
            {
               AT1=thi1-tco1;            
            }

            return AT1;
        }

        public double calculoAT2(double tci2, double tco2, double thi2, double tho2)
        {
            if (configuracion == 0)
            { 
               AT2=tho2-tco2;
            }

            else if (configuracion ==1)
            { 
               AT2=tho2-tci2;
            }

            else if (configuracion==2)
            {
               AT2=tho2-tci2;            
            }

            return AT2;
        }

        public double calculoF(double T1, double T2, double t1, double t2)
        { 

          // R=Range of shell fluid/Range of tube fluid
          R=(T1-T2)/(t2-t1);

          // S=Range of tube fluid/Maximum temperature difference
          S=(t2-t1)/(T1-t1);

          if (configuracion == 0)
            {
                F = 1;
            }

            else if (configuracion == 1)
            {
                F = 1;
            }

            else if (configuracion==2)
            {
                double A=0;
                double B=0;
                A=2-S*(R+1-Math.Pow((Math.Pow(R,2)+1),0.5));
                B=2-S*(R+1-Math.Pow((Math.Pow(R,2)+1),0.5));
                
                //For 1 shell and 2 tubes pass heat exchanger
                F = (Math.Pow((Math.Pow(R, 2) + 1), 0.5) * Math.Log((1 - S))) / (1 - (R * S))/((R-1)*Math.Log(A/B));
            }

            return F;        
        }

        public double calculoLMTD(double AT1, double AT2)
        {

            LMTD=(AT2-AT1)/Math.Log(AT2/AT1);

            return LMTD;
        }

        public double calculoAtotal(double longitud,double numtubos,double diatubos)
        {
            Atotal = 3.1416 * diatubos * longitud * numtubos;
            return Atotal;
        }

        public double calculoU(double ho,double hod,double kw,double dout,double din,double hi,double hid)
        {
            U = Math.Pow((Math.Pow(ho, -1)) + (Math.Pow(hod, -1)) + ((dout / din) * Math.Pow(hi, -1)) + ((dout / din) * Math.Pow(hid, -1)) + (Math.Pow((2*kw)/(dout*Math.Log(dout/din)), -1)), -1);
            return U;
        }

        public double calculoQu(double A1,double U1,double LMTD1, double F1)
        {
            Qu = A1 * U1 * LMTD1 * F1;
            return Qu;
        }

        public double calculoArea(double Q11, double U11, double LMTD11, double F11)
        {
            Atotal=Q11/(U11*LMTD11*F11);
            return Atotal;
        }

        public double calculoQc(double mc1,double cpc1,double tci1,double tco1)
        {
            Qc = mc1 * cpc1 * (tco1 - tci1);
            return Qc;
        }

        public double calculoQh(double mh1,double cph1,double thi1,double tho1)
        {
            Qh = mh1 * cph1 * (thi1 - tho1);
            return Qh;
        }

        public double calculoCmin(double mh11,double cph11,double mc11,double cpc11)
        {
            if((mh11*cph11)<(mc11*cpc11))
            {
                Cmin = mh11 * cph11;
            }

            else if ((mh11 * cph11) >(mc11 * cpc11))
            {
                Cmin = mc11 * cpc11;
            }

            return Cmin;
        }

        public double calculoCmax(double mh22, double cph22, double mc22, double cpc22)
        {
            if ((mh22 * cph22) < (mc22 * cpc22))
            {
                Cmax = mc22 * cpc22;
            }

            else if ((mh22 * cph22) > (mc22 * cpc22))
            {
                Cmax = mh22 * cph22;
            }

            return Cmax;
        }

        public double calculoC(double Cmin1, double Cmax1)
        {
            C = Cmin1 / Cmax1;
            return C;
        }

        public double calculoNTU(double U11,double A11,double Cmin11)
        {
            NTU=(U11*A11)/Cmin11;
            N = NTU;
            return NTU;
        }

        public double calculoEficiencia(double C11,double N11)
        {
            if((configuracion==0)&&(Tipologia!="Boiler")&&(Tipologia!="Condenser"))
            {
                eficiencia=(1-Math.Exp(-N11*(1+C11)))/(1+C11);
            }

            else if ((configuracion == 1)&&(Tipologia!="Boiler")&&(Tipologia!="Condenser"))
            {
                eficiencia = (1 - Math.Exp(-N11 * (1 + C11))) / (1 - Math.Exp(-N11 * (1 - C11)));
            }

            else if ((Tipologia=="Boiler")||(Tipologia=="Condenser"))
            {
                eficiencia = 1 - Math.Exp(-N11);
            }

            return eficiencia;
        }

        public double calculoQmax(double Cmin11,double thi11,double tci11)
        {
            Qmax=Cmin11*(thi11-tci11);
            return Qmax;
        }

        //Cálculo Qd (W/m2)
        public double calculoQd(double Qc11,double Longitud11)
        {
            Qd = Qc11 / Longitud11;
            return Qd;
        }

    }
}