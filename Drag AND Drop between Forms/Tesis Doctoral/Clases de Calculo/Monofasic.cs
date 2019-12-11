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

namespace Monofasico
{
    //Clase de Pérdida de Carga en Fluido Monofásico
    public class Monofasicoliquido
    {
        public double viscosidaddinamica = 0;
        public double viscosidadcinematica = 0;

        public RegionSelection selecregion = new RegionSelection();
        public Region1 region1 = new Region1();
        public Region2 region2 = new Region2();
        public Region_3 region3 = new Region_3();
        public Region_4 region4 = new Region_4();
        public Region5 region5 = new Region5();

        public double twall = 0;
        public double tin = 0;
        public double tout = 0;
        public double densityin = 0;
        public double temperaturein = 0;
        public double pressurein = 0;
        public double diametrointerior = 0;
        public double areafluido = 0;
        public double perimetrofluido = 0;
        public double diametrohidraulico = 0;
        public double numreynold = 0;
        public double velocidad = 0;
        public string tipoflujo = "";
        public double coefdarcy = 0;
        public double rugosidad = 0;
        public double caudalmasico = 0;
        public double calorespecifico = 0;
        public double conductividadtermica = 0;
        public double longitudtuberia = 0;
        public double perdidacargamonofasico = 0;
        public double numeroPrandtl = 0;
        public double numeroNusseltGnielinski = 0;
        public double numeroNusseltPetukhov = 0;
        public double numeroNusseltDittusBoelter = 0;

        public double coefpeliculaGnielinski = 0;
        public double coefpeliculaPetukhov = 0;
        public double coefpeliculaDittusBoelter = 0;

        public double calorGnielinski = 0;
        public double calorPetukhov = 0;
        public double calorDittusBoelter = 0;

        public double tempfluido = 0;

        public int opcionPetukhov = 0;

        public void Reset() 
        { 
        viscosidaddinamica = 0;
        viscosidadcinematica = 0;

        twall = 0;
        tin = 0;
        tout = 0;
        densityin = 0;
        temperaturein = 0;
        pressurein = 0;
        diametrointerior = 0;
        areafluido = 0;
        perimetrofluido = 0;
        diametrohidraulico = 0;
        numreynold = 0;
        velocidad = 0;
        tipoflujo = "";
        coefdarcy = 0;
        rugosidad = 0;
        caudalmasico = 0;
        calorespecifico = 0;
        conductividadtermica = 0;
        longitudtuberia = 0;
        perdidacargamonofasico = 0;
        numeroPrandtl = 0;
        numeroNusseltGnielinski = 0;
        numeroNusseltPetukhov = 0;
        numeroNusseltDittusBoelter = 0;

        coefpeliculaGnielinski = 0;
        coefpeliculaPetukhov = 0;
        coefpeliculaDittusBoelter = 0;

        calorGnielinski = 0;
        calorPetukhov = 0;
        calorDittusBoelter = 0;

        tempfluido = 0;

        opcionPetukhov = 0;            
        }
        
        //Cálculo de la DENSIDAD (kg/m3)
        public double calculodensidad(double temperaturein, double pressurein)
        {
            selecregion.region_pT(pressurein, temperaturein);
            
            if (selecregion.regionpT == 1)
            {
                densityin = 1 / region1.v1_pT(pressurein, temperaturein);
            }

            else if (selecregion.regionpT == 2)
            {
                densityin = 1 / region2.v2_pT(pressurein, temperaturein);
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
                densityin = 1 / region5.v5_pT(pressurein, temperaturein);
            }

            else
            {
                MessageBox.Show("Error al seleccionar la Región.");
            }

            return densityin;
        }

        //Cálculo del DIÁMETRO HIDRÁULICO Dh = 4xAcross/Pwet (m)
        public double calculodiametrohidraulico(double diametrointerior)
        {
            areafluido = (3.1416 * diametrointerior * diametrointerior) / 4;
            perimetrofluido = 3.1416 * diametrointerior;
            diametrohidraulico = (4 * areafluido) / perimetrofluido;
            return diametrohidraulico;
        }

        //Cálculo del NÚMERO DE REYNOLDS
        public double calculonumreynolds(double densityin, double velocidad, double diametrohidraulico, double viscosidaddinamica)
        {
            numreynold = (densityin * velocidad * diametrohidraulico) / viscosidaddinamica;

            if (numreynold > 4000)
            {
                tipoflujo = "Turbulento";                
            }

            else if (numreynold < 2300)
            {
                tipoflujo = "Laminar";            
            }

            else if ((2300 <numreynold) && (numreynold < 4000))
            {
               tipoflujo = "Transitorio";
            }

            return numreynold;
        }

        //Cálculo de la VISCOSIDAD CINEMÁTICA
        public double calculoviscosidadcinematica(double viscosidaddinamica, double densityin)
        {
            viscosidadcinematica = 1000000 * (viscosidaddinamica /densityin);
            return viscosidadcinematica;
        }

        //Cálculo de la VISCOSIDAD DINÁMICA - IAPWS formulation 1985, Revised 2003 (Pa.sg)
        public double calculoviscosidaddinamica(double densityin, double temperaturein, double pressurein)
        {
            //Variables de referencia
            //Temperatura 647.096 K
            double reftemperature = 647.226;
            //Densidad 322.0 Kg/m3
            double refdensity = 317.763;
            //Presión 22.064 MPa
            double refpressure = 22.115;        

            //Variables adimensionales
            //Temperature T
            double temperature = 0;
            temperature = temperaturein / reftemperature;
            //Density rho
            double density = 0;
            density = densityin / refdensity;
            //Pressure P
            double pressure = 0;
            pressure = pressurein / refpressure;

            double[] ho = new double[6];
            ho[0] = 0.5132047;
            ho[1] = 0.3205656;
            ho[2] = 0;
            ho[3] = 0;
            ho[4] = -0.7782567;
            ho[5] = 0.1885447;

            double[] h1 = new double[6];
            h1[0] = 0.2151778;
            h1[1] = 0.7317883;
            h1[2] = 1.241044;
            h1[3] = 1.476783;
            h1[4] = 0;
            h1[5] = 0;

            double[] h2 = new double[6];
            h2[0] = -0.2818107;
            h2[1] = -1.070786;
            h2[2] = -1.263184;
            h2[3] = 0;
            h2[4] = 0;
            h2[5] = 0;

            double[] h3 = new double[6];
            h3[0] = 0.1778064;
            h3[1] = 0.460504;
            h3[2] = 0.2340379;
            h3[3] = -0.4924179;
            h3[4] = 0;
            h3[5] = 0;

            double[] h4 = new double[6];
            h4[0] = -0.0417661;
            h4[1] = 0;
            h4[2] = 0;
            h4[3] = 0.1600435;
            h4[4] = 0;
            h4[5] = 0;

            double[] h5 = new double[6];
            h5[0] = 0;
            h5[1] = -0.01578386;
            h5[2] = 0;
            h5[3] = 0;
            h5[4] = 0;
            h5[5] = 0;

            double[] h6 = new double[6];
            h6[0] = 0;
            h6[1] = 0;
            h6[2] = 0;
            h6[3] = -0.003629481;
            h6[4] = 0;
            h6[5] = 0;

            //Cálculo de uo(temperature)
            double uo = 0;
            uo = Math.Pow(temperature, 0.5) / (1 + (0.978197 / temperature) + (0.579829 / (Math.Pow(temperature, 2))) - (0.202354 / (Math.Pow(temperature, 3))));

            //Cálculo de u1(temperature)
            double u1 = 0;

            double sum = 0;

            for (int i = 0; i <= 5; i++)
            {
                sum = sum + (ho[i] * Math.Pow((1 / temperature - 1), i)) + (h1[i] * Math.Pow((1 / temperature - 1), i) * Math.Pow((density - 1), 1)) + (h2[i] * Math.Pow((1 / temperature - 1), i) * Math.Pow((density - 1), 2)) + (h3[i] * Math.Pow((1 / temperature - 1), i) * Math.Pow((density - 1), 3)) + (h4[i] * Math.Pow((1 / temperature - 1), i) * Math.Pow((density - 1), 4)) + (h5[i] * Math.Pow((1 / temperature - 1), i) * Math.Pow((density - 1), 5)) + (h6[i] * Math.Pow((1 / temperature - 1), i) * Math.Pow((density - 1), 6));
            }

            u1 = Math.Exp(density * sum);

            viscosidaddinamica = uo * u1 * 0.000055071;

            //Cálculo de la VISCOSIDAD (IAPWS formulation Revised 2008)
            //PENDIENTE IMPLEMENTAR

            return viscosidaddinamica;
        }

        //Cálculo de la VELOCIDAD DEL FLUIDO (m/sg)
        public double calculovelocidad(double caudalmasico, double areafluido, double densityin)
        {
            velocidad = caudalmasico / (areafluido * densityin);
            return velocidad;
        }

        //Cálculo del CALOR ESPECÍFICO ISOBÁRICO Cp (kJ/kg K)
        public double calculocalorespisob(double pressurein, double temperaturein)
        {
            selecregion.region_pT(pressurein, temperaturein);

            if (selecregion.regionpT == 1)
            {
                calorespecifico = region1.Cp1_pT(pressurein, temperaturein);
            }

            else if (selecregion.regionpT == 2)
            {
                calorespecifico = region2.Cp2_pT(pressurein, temperaturein);
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
                calorespecifico = region5.Cp5_pT(pressurein, temperaturein);
            }
            else
            {
                MessageBox.Show("Error al seleccionar la Región.");
            }

            return calorespecifico;
        }

        //Cálculo de la CONDUCTIVIDAD TÉRMICA K (W/m K)
        public double calculoconductividad(double temperature, double rho)
        {
            double tc0 = 0;
            double tc1 = 0;
            double tc2 = 0;
            double dT = 0;
            double Q = 0;
            double s = 0;

            temperature = temperature / 647.26;
            rho = rho / 317.7;

            tc0 = Math.Pow(temperature, 0.5) * (0.0102811 + 0.0299621 * temperature + 0.0156146 * Math.Pow(temperature, 2) - 0.00422464 * Math.Pow(temperature, 3));
            tc1 = -0.39707 + (0.400302 * rho) + 1.06 * Math.Exp(-0.171587 * Math.Pow((rho + 2.39219), 2));
            dT = Math.Abs(temperature - 1) + 0.00308976;
            Q = 2 + 0.0822994 / (Math.Pow(dT, (3.0 / 5.0)));

            if (temperature >= 1)
            {
                s = 1 / dT;
            }

            else
            {
                s = 10.0932 / Math.Pow(dT, (3.0 / 5.0));
            }

            tc2 = (((0.0701309 / (Math.Pow(temperature, 10))) + 0.011852) * Math.Pow(rho, (9.0 / 5.0)) * Math.Exp(0.642857 * (1 - Math.Pow(rho, (14.0 / 5.0))))) + (0.00169937 * s * Math.Pow(rho, Q) * Math.Exp((Q / (1 + Q)) * (1 - Math.Pow(rho, (1 + Q))))) - (1.02 * Math.Exp(-4.11717 * Math.Pow(temperature, (3.0 / 2.0)) - (6.17937 / Math.Pow(rho, 5))));

            conductividadtermica = tc0 + tc1 + tc2;

            return conductividadtermica;
        }

        //Cálculo del Número de PRANDTL
        public double calculonumprandtl(double calorespecifico, double viscosidaddinamica, double conductividadtermica)
        {
            numeroPrandtl = (calorespecifico * 1000 * viscosidaddinamica) / conductividadtermica;
            return numeroPrandtl;
        }

        //Cálculo del COEFICIENTE DE DARCY f
        public double calculocoefdarcy(double numreynold1, double rugosidad1, double diametrohidraulico1, string tipoflujo1)
        {
            if (tipoflujo1 == "Laminar")
            {
                coefdarcy = 64 / numreynold1;

                coefdarcy = Math.Pow((1.82 * Math.Log10(numreynold1) - 1.64), -2);
            }

            else if (tipoflujo1 == "Turbulento")
            {
                Parameter f = new Parameter(0.01, "CoefDarcy");
                Parameter[] farray = new Parameter[1];
                farray[0] = f;

                Func<double> Colebrook = () => (-2 * Math.Log10(((rugosidad1 / diametrohidraulico1) / 3.7) + (2.51 / (numreynold1 * Math.Sqrt(f))))) - (1 / Math.Sqrt(f));
                Func<double>[] Colebrookarray = new Func<double>[1];
                Colebrookarray[0] = Colebrook;

                NewtonRaphson motornewton = new NewtonRaphson(farray, Colebrookarray);

                //Bucle de iteración del método de Newton Raphson
                for (int b = 0; b < 50; b++)
                {
                    motornewton.Iterate();
                }

                coefdarcy = farray[0].Value;
            }

            else if (tipoflujo1 == "Transitorio")
            {

            }

            else
            {
                MessageBox.Show("Error en el Cálculo del coeficient f de Darcy.");
            }

            return coefdarcy;
        }

        //Cálculo del Número de NUSSELT GNIELINSKI
        //Temperatura1: Temperatura del Fluido
        //Temperatura2: Temperatura de Pared
        public double calculonusselgnielinski(double presion, double temperatura1, double temperatura2)
        {
            //Cálculo del Número de Prandtl1 - Temperatura de Líquido (ºC)
            double calorespecifico1 = 0;
            double viscosidaddinamica1 = 0;
            double conductividadtermica1 = 0;
            double densidad1 = 0;
            double prandtl1 = 0;

            calorespecifico1 = calculocalorespisob(presion, temperatura1);
            densidad1 = calculodensidad(temperatura1, presion);
            viscosidaddinamica1 = calculoviscosidaddinamica(densidad1, temperatura1, presion);
            conductividadtermica1 = calculoconductividad(temperatura1, densidad1);
            prandtl1 = calculonumprandtl(calorespecifico1, viscosidaddinamica1, conductividadtermica1);

            //Cálculo del Número de Prandtl2 - Temperatura de Pared (ºC)
            double calorespecifico2 = 0;
            double viscosidaddinamica2 = 0;
            double conductividadtermica2 = 0;
            double densidad2 = 0;
            double prandtl2 = 0;

            calorespecifico2 = calculocalorespisob(presion, temperatura2);
            densidad2 = calculodensidad(temperatura2, presion);
            viscosidaddinamica2 = calculoviscosidaddinamica(densidad2, temperatura2, presion);
            conductividadtermica2 = calculoconductividad(temperatura2, densidad2);
            prandtl2 = calculonumprandtl(calorespecifico2, viscosidaddinamica2, conductividadtermica2);

            conductividadtermica = conductividadtermica1;

            //Fórmula de Gnielinski correlation
            numeroNusseltGnielinski = (((coefdarcy / 8.0) * (numreynold - 1000) * prandtl1) / (1 + (12.7 * Math.Pow((coefdarcy / 8.0), 0.5) * (Math.Pow(prandtl1, (2.0 / 3.0)) - 1)))) * Math.Pow((prandtl1 / prandtl2), 0.11);

            return numeroNusseltGnielinski;
        }

        //Cálculo del  Numero de NUSSELT PETUKHOV
        //Temperatura1: Temperatura del Fluido
        //Temperatura2: Temperatura de Pared
        public double calculonusselPetukhov(double presion, double temperatura1, double temperatura2, int opcion)
        {
            double viscosidaddinamica1 = 0;
            double densidad1 = 0;

            densidad1 = calculodensidad(temperatura1, presion);
            viscosidaddinamica1 = calculoviscosidaddinamica(densidad1, temperatura1, presion);

            double viscosidaddinamica2 = 0;
            double densidad2 = 0;

            densidad2 = calculodensidad(temperatura2, presion);
            viscosidaddinamica2 = calculoviscosidaddinamica(densidad2, temperatura2, presion);

            double n = 0;

            //Para PRECALENTAMIENTO del Agua
            if ((temperatura2 > temperatura1) && (opcion == 0))
            {
                n = 0.11;
            }
            //Para SUBENFIAMIENTO del Agua
            else if ((temperatura2 < temperatura1) && (opcion == 1))
            {
                MessageBox.Show("Atención¡¡¡: Elegida la opción de Subenfriamiento del Fluido: Tpared<Tfluido");
                n = 0.25;
            }

            //Para SOBRECALENTAMIENTO del Vapor
            else if (opcion == 2)
            {
                n = 0;
            }

            //Fórmula de Gnielinski correlation
            numeroNusseltPetukhov = (((coefdarcy / 8.0) * numreynold * numeroPrandtl) / (1.07 + (12.7 * Math.Pow((coefdarcy / 8.0), 0.5) * (Math.Pow(numeroPrandtl, (2.0 / 3.0)) - 1)))) * Math.Pow((viscosidaddinamica1 / viscosidaddinamica2), n);

            return numeroNusseltPetukhov;
        }

        //Cálculo del HTC según DITTUS-BOELTER
        public double calculoHTCDittusBoelter(double numreynold1, double numprandtl1, double conductividad1, double diahidraulico1)
        {
            numreynold1 = numreynold;
            numprandtl1 = numeroPrandtl;
            conductividad1 = conductividadtermica;
            diahidraulico1 = diametrohidraulico;

            coefpeliculaDittusBoelter = 0.023 * Math.Pow(numreynold1, 0.8) * Math.Pow(numprandtl1, 0.4) * (conductividad1 / diahidraulico1);

            return coefpeliculaDittusBoelter;
        }

        //Cálculo del HTC según GNIELINSKI
        public double calculoHTCGnielinski (double conductividad1,double NusseltGnielinski1,double diametro1)
        {
          coefpeliculaGnielinski = conductividad1 * NusseltGnielinski1 / diametro1;
          return coefpeliculaGnielinski;
        }

        //Cálculo del HTC según PETUKHOV
        public double calculoHTCPetukhov(double conductividad1, double NusseltGnielinski1, double diametro1)
        {
            coefpeliculaPetukhov = conductividad1 * NusseltGnielinski1 / diametro1;
            return coefpeliculaPetukhov;
        }
               
        //Cálculo del Calor según el HTC de GNIELINSKI
        public double calculocalorGnielinski(double coefpelicula1,double diametrohidraulico1,double temppared1,double tempfluido1)
        {
            calorGnielinski = coefpelicula1 * diametrohidraulico1 * 3.1416 * (temppared1 - tempfluido1);
            return calorGnielinski;
        }

        //Cálculo del Calor según el HTC de PETUKHOV
        public double calculocalorPetukhov(double coefpelicula1, double diametrohidraulico1, double temppared1, double tempfluido1)
        {
           calorPetukhov = coefpelicula1 * diametrohidraulico1 * 3.1416 * (temppared1 - tempfluido1);
            return calorPetukhov;
        }

        //Cálculo del Calor según el HTC de DITTUS-BOELTER
        public double calculocalorDittusBoelter(double coefpelicula1, double diametrohidraulico1, double temppared1, double tempfluido1)
        {
           calorDittusBoelter = coefpelicula1 * diametrohidraulico1 * 3.1416 * (temppared1 - tempfluido1);
            return calorDittusBoelter;
        }
    }
}