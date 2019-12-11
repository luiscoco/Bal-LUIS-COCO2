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
    //Clase para Cálculo de Intercambiadores por Diferencias Finitas basándose en los Datos: Q(W/m2), número intervalos y AL (longitud de cada tramo) 
    // y condiciones iniciales:tci,thi,cpc,cph,pi,mc,mh
    public class HeatExchangerNominalDiferenciasFinitas
    {
        //General information
        
        //Configuración del Intercambiador:  
        //Type 1 - 0: Counter Current Flows Heat Exchanger
        //Type 2 - 1: Cocurrent Flows Heat Exchanger
        //Type 3 - 2: 2 Tube Passes - 1 Shell Passes Heat Exchanger
        //Type 4 - 3: Boiler
        //Type 5 - 4: Condenser
        public int configuracion = 0;

        //Definición Hot and Cold Fluids
        //Type 1 - 0: Tube Side Hot Fluid - Shell Side Cold Fluid
        //Type 2 - 1: Tube Side Cold Fluid - Shell Side Hot Fluid
        public int Hotfluid=0;

        public double L = 0;
        public double AZ = 0;
        public int ntimes = 0;
        public int ngeometric = 0;
        public double intervtiempo = 0;

        //Shell side information
        public double shelldia = 0;
        public double lateralAs = 0;
        public double crossAs = 0;
        public double cps = 0;
        public double rhos = 0;
        public double Fs = 0;
        public double ms = 0;
        public double Tos = 0;
        public double Pos = 0;
        public double rugosidadshell = 0;
        
        //Tube side information
        public double tubediaint = 0;
        public double thk = 0;
        public double tubediaext = 0;
        public double Ntube = 0;
        public double lateralAt = 0;
        public double crossAt = 0;
        public double cpt = 0;
        public double rhot = 0;
        public double Ft = 0;
        public double mt = 0;
        public double Tot = 0;
        public double Pot = 0;
        public double rugosidadtube = 0;

        //Total Heat Transfer Coefficient U(W/m2 K)
        public double HTCs = 0;
        public double HTCt = 0;
        public double HTCsd = 0;
        public double HTCtd = 0;
        public double k = 0;
        public double tubeslong = 0;
        public double Uo = 0;

        //Arrays auxiliares 
        public double[] time;
        public int i = 0;
        public int j = 0;
        public double[,] Tt;
        public double[,] Ts;
        public double[,] Twt;
        public double[,] Tws;
        public double initialtime = 0;
        public double finaltime = 0;
        public double resultadotubos = 0;
        public double resultadoshell = 0;

        //Heat Variables
        public double[,] qt;
        public double[,] qs;
        public double[] Qt;
        public double[] Qs;
        public double[,] U;

        //Fluid properties updated each iteration
        public Monofasicoliquido[,] shellfluid;
        public Monofasicoliquido[,] tubefluid;     
         
        //Wall Temperatures
        

        //Método de Integración
        //First Order=0; Second Order Heun=1; Second Order Ralston=2; Third Order=3; Fourth Order=4; Fifth Order=5; Fifth-Fourth Order=6 
        public int metodointegracion = 0;

        //Shell Cold Fluid, Tubes Hot Fluid
        //Cálculo de Temperatura(K)s en el Lado Tubos en cada uno de los Nodos(j1) y en cada uno de los intervalos de Tiempo(i1)
        public double caltubcontrac(int j1,int i1,double rhot1,double cpt1)
        {
            Tt[j1, i1] = Tt[j1, i1 - 1] + intervtiempo * (((Ft * (Tt[j1 - 1, i1 - 1]-Tt[j1,i1-1])) * (1 / (crossAt * AZ))) - (((Uo *(lateralAt/ngeometric)) / (AZ * crossAt * rhot1 * cpt1)) * (Tt[j1, i1 - 1] - Ts[ngeometric-j1+1, i1 - 1])));            
            resultadotubos=Tt[j1,i1];
            return resultadotubos;
        }

        //Shell Cold Fluid, Tubes Hot Fluid
        //Cálculo de Temperaturas(K) en el Lado Carcasa en cada uno de los Nodos(j2) y en cada uno de los intervalos de Tiempo(i2)
        public double calshellcontrac(int j2, int i2, double rhot2, double cpt2)
        {
            Ts[j2, i2] = Ts[j2, i2 - 1] + intervtiempo * (((Fs * (Ts[j2 - 1, i2 - 1] - Ts[j2, i2 - 1])) * (1 / (crossAs * AZ))) - (((Uo *(lateralAs/ngeometric)) / (AZ * crossAs * rhot2 * cpt2)) * (Ts[j2, i2 - 1] - Tt[ngeometric -j2 + 1, i2 - 1])));
            resultadoshell = Ts[j2, i2];
            return resultadoshell;
        }

        //Shell Cold Fluid, Tubes Hot Fluid
        //Cálculo de Temperatura(K)s en el Lado Tubos en cada uno de los Nodos(j1) y en cada uno de los intervalos de Tiempo(i1)
        public double caltubparalelos(int j1, int i1, double rhot1, double cpt1)
        {
            Tt[j1, i1] = Tt[j1, i1 - 1] + intervtiempo * (((Ft * (Tt[j1 - 1, i1 - 1] - Tt[j1, i1 - 1])) * (1 / (crossAt * AZ))) - (((Uo *(lateralAt/ngeometric)) / (AZ * crossAt * rhot1 * cpt1)) * (Tt[j1, i1 - 1] - Ts[j1, i1 - 1])));
            resultadotubos = Tt[j1, i1];
            return resultadotubos;
        }

        //Shell Cold Fluid, Tubes Hot Fluid
        //Cálculo de Temperaturas(K) en el Lado Carcasa en cada uno de los Nodos(j2) y en cada uno de los intervalos de Tiempo(i2)
        public double calshellparalelos(int j2, int i2, double rhot2, double cpt2)
        {
            Ts[j2, i2] = Ts[j2, i2 - 1] + intervtiempo * (((Fs * (Ts[j2 - 1, i2 - 1] - Ts[j2, i2 - 1])) * (1 / (crossAs * AZ))) - (((Uo * (lateralAs/ngeometric)) / (AZ * crossAs * rhot2 * cpt2)) * (Ts[j2, i2 - 1] - Tt[j2, i2 - 1])));
            resultadoshell = Ts[j2, i2];
            return resultadoshell;
        }


        //----------------------------------------------------------------------------------------------------------------------------------
        //Cálculo de Temperatura de Pared Tws lado Carcasa, y Fluido Frio por Tubos
        public double calTwtColdFluid(int j11, int i11, double Tt11, double ht11, double q11)
        {
            Twt[j11, i11] = (q11 + (ht11 * Tt11)) / ht11;
            return Twt[j11, i11];
        }
        
        //Cálculo de Temperatura de Pared Lado Tubos (Twt), y Fluido Caliente por Tubos
        public double calTwtHotFluid(int j8, int i8, double Tt8, double ht8, double q8)
        {
            Twt[j8, i8] = (-q8 + (ht8 * Tt8)) / ht8;
            return Twt[j8,i8];
        }       

        //Cálculo de Temperatura de Pared Lado Carcasa (Tws), y Fluido Caliente por Tubos
        public double calTwsHotFluid(int j10, int i10, double Twt10, double k10, double q10)
        {
            Tws[j10,i10]=(k10*Twt10-q10)/k10;
            return Tws[j10,i10];
        }

        //Cálculo de Temperatura de Pared Tws lado Carcasa, y Fluido Frio por Tubos
        public double calTwsColdFluid(int j9, int i9, double Twt9, double k9, double q9)
        {
            Tws[j9, i9] = (k9*Twt9+q9)/k9;
            return Tws[j9,i9];
        }       
        //----------------------------------------------------------------------------------------------------------------------------------
        
        //Cálculo del Calor(W/m2) en el Lado Tubos en cada uno de los Nodos(j3) y en cada uno de los intervalos de Tiempo(i3)
        public double calcalortuboscontracor(int j3, int i3, double temp1, double temp2, double U3)
        { 
            qt[j3,i3]=U3*(temp1-temp2);
            return qt[j3, i3];
        }
    
        //Cálculo del Calor(W/m2) en el Lado Carcasa en cada uno de los Nodos(j4) y en cada uno de los intervalos de Tiempo(i4)
        public double calcalorcarcasacontracor(int j4, int i4, double temp3, double temp4, double U4)
        {
            qs[j4,i4]=U4*(temp3-temp4);
            return qs[j4, i4];
        }

        //Cálculo del Calor(W/m2) en el Lado Tubos en cada uno de los Nodos(j3) y en cada uno de los intervalos de Tiempo(i3)
        public double calcalortubosparalelo(int j3, int i3, double temp1, double temp2, double U5)
        {
            qt[j3, i3] = U5 * (temp1 - temp2);
            return qt[j3, i3];
        }

        //Cálculo del Calor(W/m2) en el Lado Carcasa en cada uno de los Nodos(j4) y en cada uno de los intervalos de Tiempo(i4)
        public double calcalorcarcasaparalelo(int j4, int i4, double temp3, double temp4, double U6)
        {
            qs[j4, i4] = U6 * (temp3 - temp4);
            return qs[j4, i4];
        }

        //Cálculo del Coeficiente General de Transmisión de Calor (W/m2 K) entre el Fluido de Tubos y Fluido de Carcasa
        public double calculoU(int j5,int i5,double din11,double dout11,double ht11, double hs11,double hsd11,double htd11,double kw11)
        {
            U[j5, i5] = Math.Pow((1 / shellfluid[j5, i5].coefpeliculaDittusBoelter) + (1 / hsd11) + ((dout11 * Math.Log(dout11 / din11)) / (2 * kw11)) + ((dout11 / din11) * (1 / tubefluid[j5, i5].coefpeliculaDittusBoelter)) + ((dout11 / din11) * (1 / htd11)), -1);
            return U[j5,i5];
        }

        //Cálculo del Coeficiente General de Transmisión de Calor (W/m2 K) entre el Fluido de Tubos y Fluido de Carcasa
        public double calculoUo(double din11, double dout11, double ht11, double hs11, double hsd11, double htd11, double kw11)
        {
            Uo = Math.Pow((1 / HTCs) + (1 / hsd11) + ((dout11 * Math.Log(dout11 / din11)) / (2 * kw11)) + ((dout11 / din11) * (1 / HTCt)) + ((dout11 / din11) * (1 / htd11)), -1);
            return Uo;
        }

        //Calculo de Propiedades del Fluido de la Carcasa (ShellFluid)
        public Monofasicoliquido calculoShellfluid(int j6, int i6,double P,double T)
        {
            Monofasicoliquido temp = new Monofasicoliquido();
            //Data Input and Average Temperature Calculation
            temp.caudalmasico = ms/Ntube;
            //Presión en Bares
            temp.pressurein = P / 10;
            //Temperatura en Kelvin
            temp.temperaturein = T;           

            //Fluid Properties Calculation
            temp.densityin = temp.calculodensidad(temp.temperaturein, temp.pressurein);
            temp.viscosidaddinamica = temp.calculoviscosidaddinamica(temp.densityin, temp.temperaturein, temp.pressurein);
            temp.viscosidadcinematica = temp.calculoviscosidadcinematica(temp.viscosidaddinamica, temp.densityin);
            temp.calorespecifico = temp.calculocalorespisob(temp.pressurein, temp.temperaturein);
            temp.conductividadtermica = temp.calculoconductividad(temp.temperaturein, temp.densityin);

            //Fluid Velocity Calculation
            //Diámetro interior en m
            temp.diametrointerior = tubediaint+(2*thk);
            temp.diametrohidraulico = temp.calculodiametrohidraulico(temp.diametrointerior);
            temp.velocidad = temp.calculovelocidad(temp.caudalmasico, temp.areafluido, temp.densityin);

            //Non-dimensional variables Calculation
            temp.numreynold = temp.calculonumreynolds(temp.densityin, temp.velocidad, temp.diametrohidraulico, temp.viscosidaddinamica);
            temp.numeroPrandtl = temp.calculonumprandtl(temp.calorespecifico, temp.viscosidaddinamica, temp.conductividadtermica);
            temp.rugosidad = rugosidadshell;
            //temp.coefdarcy = temp.calculocoefdarcy(temp.numreynold, temp.rugosidad, temp.diametrohidraulico, temp.tipoflujo);

            //IMPORTANTE pendiente de programar el HTC según Petukhov y según Gnielinski

            temp.coefpeliculaDittusBoelter = temp.calculoHTCDittusBoelter(temp.numreynold, temp.numeroPrandtl, temp.conductividadtermica, temp.diametrohidraulico);
            shellfluid[j6, i6] = temp;
            return shellfluid[j6,i6];
        }

        //Calculo de Propiedades del Fluido de la Carcasa (ShellFluid)
        public double calculoShellfluid(double P, double T)
        {
            Monofasicoliquido temp = new Monofasicoliquido();
            //Data Input and Average Temperature Calculation
            temp.caudalmasico = ms/Ntube;
            //Presión en Bares
            temp.pressurein = P / 10;
            //Temperatura en Kelvin
            temp.temperaturein = T;

            //Fluid Properties Calculation
            temp.densityin = temp.calculodensidad(temp.temperaturein, temp.pressurein);
            temp.viscosidaddinamica = temp.calculoviscosidaddinamica(temp.densityin, temp.temperaturein, temp.pressurein);
            temp.viscosidadcinematica = temp.calculoviscosidadcinematica(temp.viscosidaddinamica, temp.densityin);
            temp.calorespecifico = temp.calculocalorespisob(temp.pressurein, temp.temperaturein);
            temp.conductividadtermica = temp.calculoconductividad(temp.temperaturein, temp.densityin);

            //Fluid Velocity Calculation
            //Diámetro interior en m
            temp.diametrointerior = tubediaint+(2*thk);
            temp.diametrohidraulico = temp.calculodiametrohidraulico(temp.diametrointerior);
            temp.velocidad = temp.calculovelocidad(temp.caudalmasico, temp.areafluido, temp.densityin);

            //Non-dimensional variables Calculation
            temp.numreynold = temp.calculonumreynolds(temp.densityin, temp.velocidad, temp.diametrohidraulico, temp.viscosidaddinamica);
            temp.numeroPrandtl = temp.calculonumprandtl(temp.calorespecifico, temp.viscosidaddinamica, temp.conductividadtermica);
            temp.rugosidad = rugosidadshell;
            //temp.coefdarcy = temp.calculocoefdarcy(temp.numreynold, temp.rugosidad, temp.diametrohidraulico, temp.tipoflujo);

            //IMPORTANTE pendiente de programar el HTC según Petukhov y según Gnielinski

            temp.coefpeliculaDittusBoelter = temp.calculoHTCDittusBoelter(temp.numreynold, temp.numeroPrandtl, temp.conductividadtermica, temp.diametrohidraulico);
            return temp.coefpeliculaDittusBoelter;
        }

        //Calculo de Propiedades del Fluido de la Carcasa (ShellFluid)
        public Monofasicoliquido calculoTubesfluid(int j7, int i7, double P1, double T1)
        {
            Monofasicoliquido temp1 = new Monofasicoliquido();

            //Data Input and Average Temperature Calculation
            temp1.caudalmasico = mt/Ntube;
            //Presión en Bares
            temp1.pressurein = P1 / 10;
            //Temperatura en Kelvin
            temp1.temperaturein = T1;

            //Fluid Properties Calculation
            temp1.densityin = temp1.calculodensidad(temp1.temperaturein, temp1.pressurein);
            temp1.viscosidaddinamica = temp1.calculoviscosidaddinamica(temp1.densityin, temp1.temperaturein, temp1.pressurein);
            temp1.viscosidadcinematica = temp1.calculoviscosidadcinematica(temp1.viscosidaddinamica, temp1.densityin);
            temp1.calorespecifico = temp1.calculocalorespisob(temp1.pressurein, temp1.temperaturein);
            temp1.conductividadtermica = temp1.calculoconductividad(temp1.temperaturein, temp1.densityin);

            //Fluid Velocity Calculation
            //Diámetro interior en m
            temp1.diametrointerior = tubediaint;
            temp1.diametrohidraulico = temp1.calculodiametrohidraulico(temp1.diametrointerior);
            temp1.velocidad = temp1.calculovelocidad(temp1.caudalmasico, temp1.areafluido, temp1.densityin);

            //Non-dimensional variables Calculation
            temp1.numreynold = temp1.calculonumreynolds(temp1.densityin, temp1.velocidad, temp1.diametrohidraulico, temp1.viscosidaddinamica);
            temp1.numeroPrandtl = temp1.calculonumprandtl(temp1.calorespecifico, temp1.viscosidaddinamica, temp1.conductividadtermica);
            temp1.rugosidad =rugosidadtube;
            //temp1.coefdarcy = temp1.calculocoefdarcy(temp1.numreynold, temp1.rugosidad, temp1.diametrohidraulico, temp1.tipoflujo);

            //IMPORTANTE pendiente de programar el HTC según Petukhov y según Gnielinski

            temp1.coefpeliculaDittusBoelter = temp1.calculoHTCDittusBoelter(temp1.numreynold, temp1.numeroPrandtl, temp1.conductividadtermica, temp1.diametrohidraulico);
            tubefluid[j7, i7] = temp1;
            return tubefluid[j7, i7];
        }

        //Calculo de Propiedades del Fluido de la Carcasa (ShellFluid)
        public double calculoTubesfluid(double P1, double T1)
        {
            Monofasicoliquido temp1 = new Monofasicoliquido();

            //Data Input and Average Temperature Calculation
            temp1.caudalmasico = mt/Ntube;
            //Presión en Bares
            temp1.pressurein = P1 / 10;
            //Temperatura en Kelvin
            temp1.temperaturein = T1;

            //Fluid Properties Calculation
            temp1.densityin = temp1.calculodensidad(temp1.temperaturein, temp1.pressurein);
            temp1.viscosidaddinamica = temp1.calculoviscosidaddinamica(temp1.densityin, temp1.temperaturein, temp1.pressurein);
            temp1.viscosidadcinematica = temp1.calculoviscosidadcinematica(temp1.viscosidaddinamica, temp1.densityin);
            temp1.calorespecifico = temp1.calculocalorespisob(temp1.pressurein, temp1.temperaturein);
            temp1.conductividadtermica = temp1.calculoconductividad(temp1.temperaturein, temp1.densityin);

            //Fluid Velocity Calculation
            //Diámetro interior en m
            temp1.diametrointerior = tubediaint;
            temp1.diametrohidraulico = temp1.calculodiametrohidraulico(temp1.diametrointerior);
            temp1.velocidad = temp1.calculovelocidad(temp1.caudalmasico, temp1.areafluido, temp1.densityin);

            //Non-dimensional variables Calculation
            temp1.numreynold = temp1.calculonumreynolds(temp1.densityin, temp1.velocidad, temp1.diametrohidraulico, temp1.viscosidaddinamica);
            temp1.numeroPrandtl = temp1.calculonumprandtl(temp1.calorespecifico, temp1.viscosidaddinamica, temp1.conductividadtermica);
            temp1.rugosidad = rugosidadtube;
            //temp1.coefdarcy = temp1.calculocoefdarcy(temp1.numreynold, temp1.rugosidad, temp1.diametrohidraulico, temp1.tipoflujo);

            //IMPORTANTE pendiente de programar el HTC según Petukhov y según Gnielinski

            temp1.coefpeliculaDittusBoelter = temp1.calculoHTCDittusBoelter(temp1.numreynold, temp1.numeroPrandtl, temp1.conductividadtermica, temp1.diametrohidraulico);
           
            return temp1.coefpeliculaDittusBoelter;
        }

        //Cálculo de Temperatura(K)s en el Lado Tubos en cada uno de los Nodos(j1) y en cada uno de los intervalos de Tiempo(i1) con U (variable con la temperatura)
        public double calculovariableintegracintubos(int j1, int i1, double rhot1, double cpt1)
        {
            Tt[j1, i1] = Tt[j1, i1 - 1] + intervtiempo * (((Ft * (Tt[j1 - 1, i1 - 1] - Tt[j1, i1 - 1])) * (1 / (crossAt * AZ))) - (((U[j1,i1] *lateralAt) / (AZ * crossAt * rhot1 * cpt1)) * (Tt[j1, i1 - 1] - Ts[ngeometric - j1 + 1, i1 - 1])));
            resultadotubos = Tt[j1, i1];
            return resultadotubos;
        }

        //Cálculo de Temperaturas(K) en el Lado Carcasa en cada uno de los Nodos(j2) y en cada uno de los intervalos de Tiempo(i2) con U (variable con la temperatura)
        public double calculovariableintegracionshell(int j2, int i2, double rhot2, double cpt2)
        {
            Ts[j2, i2] = Ts[j2, i2 - 1] + intervtiempo * (((Fs * (Ts[j2 - 1, i2 - 1] - Ts[j2, i2 - 1])) * (1 / (crossAs * AZ))) - (((U[j2,i2] * lateralAs) / (AZ * crossAs * rhot2 * cpt2)) * (Ts[j2, i2 - 1] - Tt[ngeometric - j2 + 1, i2 - 1])));
            resultadoshell = Ts[j2, i2];
            return resultadoshell;
        }

        //Cálculo del Coeficiente General de Transmisión de Calor (W/m2 K) entre el Fluido de Tubos y Fluido de Carcasa
        public double calculoUconstante(double din11, double dout11, double ht11, double hs11, double hsd11, double htd11, double kw11)
        {
            Uo = Math.Pow((1 / hs11) + (1 / hsd11) + ((dout11 * Math.Log(dout11 / din11)) / (2 * kw11)) + ((dout11 / din11) * (1 / ht11)) + ((dout11 / din11) * (1 / htd11)), -1);
            return Uo;
        }
    }
}