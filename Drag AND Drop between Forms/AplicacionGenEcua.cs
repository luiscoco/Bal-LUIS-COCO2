using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections;
using System.IO; //Para lectura y escritura de archivos
using System.Globalization;

using NumericalMethods; //Método numérico de Newton Raphson
using NumericalMethods.FourthBlog;  //Método numérico de Newton Raphson

using WindowsFormsApplication2;

using Files_in_csharp; //Interface lectura archivos HBAL

using ClaseEquipos;

using TablasAgua1967; //Tablas de Agua-Vapor ASME 1967

using ZedGraphSample; //Ejemplo de Ploteo de Tablas definidas por el usuario

using System.Diagnostics;

namespace Drag_AND_Drop_between_Forms
{
    public partial class Aplicacion : Form
    {
        
        private int generarecuaciones(int contecuaciones1 ,Double TipodeEquipo, Double N1, Double N2, Double N3, Double N4, Double D1, Double D2, Double D3, Double D4, Double D5, Double D6, Double D7, Double D8, Double D9, Double adicional1, Double adicional2, Double adicional3, Double adicional4)
        {
            //Creamos las ecuaciones del Equipo TIPO 1: CONDICIÓN DE CONTORNO
            if (TipodeEquipo == 1)
            {
                Parameter W1 = new Parameter();
                Parameter W2 = new Parameter();
                Parameter P1 = new Parameter();
                Parameter P2 = new Parameter();
                Parameter H1 = new Parameter();
                Parameter H2 = new Parameter();

                W1 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N1));
                W2 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N3));
                P1 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N1));
                P2 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N3));
                H1 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N1));
                H2 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N3));
                              
                //No hay corriente de entrada desde otro equipo, es el primer equipo del Sistema 
                if (adicional1!=1)
                {

                    if (D5 == 0)
                    {
                        Func<Double> primeraecuacion = () => W1 - W2;
                        functions.Add(primeraecuacion);

                        //Populamos la matrizauxjacob
                        for (int j = 0; j < p.Count; j++)
                        {
                            if ((p[j].Nombre == W1.Nombre)||(p[j].Nombre == W2.Nombre))
                            {
                                matrizauxjacob[contecuaciones1, j] = 1;
                            }
                        }
                        contecuaciones1++;
                    }

                    Func<Double> segundaecuacion = () => P1 - P2;
                    functions.Add(segundaecuacion);
                    //Populamos la matrizauxjacob
                    for (int j = 0; j < p.Count; j++)
                    {
                        if ((p[j].Nombre == P1.Nombre) || (p[j].Nombre == P2.Nombre))
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;
                            
                        }
                    }
                    contecuaciones1++;

                    Func<Double> terceraecuacion = () => H1 - H2;
                    functions.Add(terceraecuacion);

                    //Populamos la matrizauxjacob
                    for (int j = 0; j < p.Count; j++)
                    {
                        if ((p[j].Nombre == H1.Nombre) || (p[j].Nombre == H2.Nombre))
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;
                           
                        }
                    }
                    contecuaciones1++;


                    if (D1 > 0)
                    {
                        Func<Double> cuartaecuacion = () => W1 - D1;
                        //punteroaplicacion1.tablas(0,7,2)
                        functions.Add(cuartaecuacion);
                        //Populamos la matrizauxjacob
                        for (int j = 0; j < p.Count; j++)
                        {
                            if (p[j].Nombre == W1.Nombre)
                            {
                                matrizauxjacob[contecuaciones1, j] = 1;
                               
                            }
                        }
                        contecuaciones1++;
                    }
                    

                    if (D2 > 0)
                    {
                        Func<Double> quintaecuacion = () => P1 - D2;
                        functions.Add(quintaecuacion);
                        //Populamos la matrizauxjacob
                        for (int j = 0; j < p.Count; j++)
                        {
                            if (p[j].Nombre == P1.Nombre) 
                            {
                                matrizauxjacob[contecuaciones1, j] = 1;
                               
                            }
                        }
                        contecuaciones1++;
                    }
                   

                    if (D3 > 0)
                    {
                        Func<Double> sextaecuacion = () => H1 - D3;
                        functions.Add(sextaecuacion);
                        //Populamos la matrizauxjacob
                        for (int j = 0; j < p.Count; j++)
                        {
                            if (p[j].Nombre == H1.Nombre) 
                            {
                                matrizauxjacob[contecuaciones1, j] = 1;
                                
                            }
                        }
                        contecuaciones1++;
                    }
                    

                    else if (D6 > 0 && D3 == 0)
                    {
                        Func<Double> sextaecuacion = () => H1 - acceso.hpx(D6, D7);
                        functions.Add(sextaecuacion);

                        for (int j = 0; j < p.Count; j++)
                        {
                            if (p[j].Nombre == H1.Nombre)
                            {
                                matrizauxjacob[contecuaciones1, j] = 1;

                            }
                        }
                        contecuaciones1++;
                    }

                    else if (D6 < 0 && D3 == 0)
                    {
                        Func<Double> sextaecuacion = () => H1 - acceso.htx(-D6, D7);
                        functions.Add(sextaecuacion);

                        for (int j = 0; j < p.Count; j++)
                        {
                            if (p[j].Nombre == H1.Nombre)
                            {
                                matrizauxjacob[contecuaciones1, j] = 1;

                            }
                        }
                        contecuaciones1++;
                    }
                }

                //Corriente de entrada proveniente de otro equipo
                else if (adicional1== 1)
                {
                    Func<Double> primeraecuacion = () => W1 - W2;
                    //punteroaplicacion1.tablas(0,7,2)
                    functions.Add(primeraecuacion);

                    for (int j = 0; j < p.Count; j++)
                    {
                        if ((p[j].Nombre == W1.Nombre)||(p[j].Nombre == W2.Nombre))
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;

                        }
                    }
                    contecuaciones1++;


                    Func<Double> segundaecuacion = () => P1 - P2;
                    functions.Add(segundaecuacion);

                    for (int j = 0; j < p.Count; j++)
                    {
                        if ((p[j].Nombre == P1.Nombre) || (p[j].Nombre == P2.Nombre))
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;

                        }
                    }
                    contecuaciones1++;


                    Func<Double> terceraecuacion = () => H1 - H2;
                    functions.Add(terceraecuacion);

                    for (int j = 0; j < p.Count; j++)
                    {
                        if ((p[j].Nombre == H1.Nombre) || (p[j].Nombre == H2.Nombre))
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;

                        }
                    }
                    contecuaciones1++;

                    //Caudal diferente de cero W1=D1
                    if (D1 > 0)
                    {
                        Func<Double> cuartaecuacion = () => W1 - D1;
                        //punteroaplicacion1.tablas(0,7,2)
                        functions.Add(cuartaecuacion);

                        for (int j = 0; j < p.Count; j++)
                        {
                            if (p[j].Nombre == W1.Nombre) 
                            {
                                matrizauxjacob[contecuaciones1, j] = 1;

                            }
                        }
                        contecuaciones1++;
                    }

                    //Presión diferente de cero P1=D2
                    if (D2 > 0)
                    {
                        Func<Double> cuartaecuacion = () => P1 - D2;
                        //punteroaplicacion1.tablas(0,7,2)
                        functions.Add(cuartaecuacion);

                        for (int j = 0; j < p.Count; j++)
                        {
                            if (p[j].Nombre == P1.Nombre)
                            {
                                matrizauxjacob[contecuaciones1, j] = 1;

                            }
                        }
                        contecuaciones1++;

                    }

                    if (D3 > 0)
                    {
                        Func<Double> cuartaecuacion = () => H1 - D3;
                        //punteroaplicacion1.tablas(0,7,2)
                        functions.Add(cuartaecuacion);

                        for (int j = 0; j < p.Count; j++)
                        {
                            if (p[j].Nombre == H1.Nombre)
                            {
                                matrizauxjacob[contecuaciones1, j] = 1;

                            }
                        }
                        contecuaciones1++;
                    }
                }

                //Caso de que hayamos creado una condición de contorno Tipo 1 con todos sus parámetros D1 a D9 = 0
                else if ((D1 == 0) && (D2 == 0) && (D3 == 0) && (D4 == 0) && (D5 == 0) && (D6 == 0) && (D7 == 0) && (D8 == 0) && (D9 == 0))
                {
                    Func<Double> primeraecuacion = () => W1 - W2;
                    //punteroaplicacion1.tablas(0,7,2)
                    functions.Add(primeraecuacion);

                    for (int j = 0; j < p.Count; j++)
                    {
                        if ((p[j].Nombre == W1.Nombre) || (p[j].Nombre == W2.Nombre))
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;

                        }
                    }
                    contecuaciones1++;


                    Func<Double> segundaecuacion = () => P1 - P2;
                    functions.Add(segundaecuacion);

                    for (int j = 0; j < p.Count; j++)
                    {
                        if ((p[j].Nombre == P1.Nombre) || (p[j].Nombre == P2.Nombre))
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;

                        }
                    }
                    contecuaciones1++;


                    Func<Double> terceraecuacion = () => H1 - H2;
                    functions.Add(terceraecuacion);

                    for (int j = 0; j < p.Count; j++)
                    {
                        if ((p[j].Nombre == H1.Nombre) || (p[j].Nombre == H2.Nombre))
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;

                        }
                    }
                    contecuaciones1++;
                }
                return contecuaciones1;
            }
//-------------------------------------------------------------------------------------------------------------------------------
            //Creamos las ecuaciones del Equipo TIPO 2: DIVISOR
            else if (TipodeEquipo == 2)
            {
                Parameter W1 = new Parameter();
                Parameter W2 = new Parameter();
                Parameter W3 = new Parameter();
                Parameter P1 = new Parameter();
                Parameter P2 = new Parameter();
                Parameter P3 = new Parameter();
                Parameter H1 = new Parameter();
                Parameter H2 = new Parameter();
                Parameter H3 = new Parameter();

                W1 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N1));
                W2 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N3));
                W3 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N4));
                P1 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N1));
                P2 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N3));
                P3 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N4));
                H1 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N1));
                H2 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N3));
                H3 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N4));

                Func<Double> primeraecuacion = () => W1 - W2 - W3;
                functions.Add(primeraecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == W1.Nombre)||(p[j].Nombre == W2.Nombre)||(p[j].Nombre == W3.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;
                
                Func<Double> segundaecuacion = () => P1 - P2;
                functions.Add(segundaecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == P1.Nombre)||(p[j].Nombre == P2.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;
                
                Func<Double> terceraecuacion = () => H1 - H2;
                functions.Add(terceraecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == H1.Nombre)||(p[j].Nombre == H2.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;

                
                Func<Double> cuartaecuacion = () => P1 - P3;
                functions.Add(cuartaecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == P1.Nombre) || (p[j].Nombre == P3.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;
                
                Func<Double> quintaecuacion = () => H1 - H3;
                functions.Add(quintaecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == H1.Nombre) || (p[j].Nombre == H3.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;

                if ((D1 > 0) || (D2 > 0) || (D3 > 0))
                {
                    if ((D1 > 0) || (D2 > 0) && (D3 == 0))
                    {
                        Func<Double> sextaecuacion = () => W3 - D1 - (D2 * W1);
                        functions.Add(sextaecuacion);

                        for (int j = 0; j < p.Count; j++)
                        {
                            if ((p[j].Nombre == W3.Nombre) || (p[j].Nombre == W1.Nombre))
                            {
                                matrizauxjacob[contecuaciones1, j] = 1;

                            }
                        }
                        contecuaciones1++;
                    }

                    else if (D3 > 0 && D1 == 0 && D2 == 0)
                    {
                        Func<Double> sextaecuacion = () => W3 - (D3 * Math.Sqrt(P1 / acceso.vph(P1, H1)));
                        functions.Add(sextaecuacion);

                        for (int j = 0; j < p.Count; j++)
                        {
                            if ((p[j].Nombre == W3.Nombre) || (p[j].Nombre == P1.Nombre) || (p[j].Nombre == H1.Nombre))
                            {
                                matrizauxjacob[contecuaciones1, j] = 1;

                            }
                        }
                        contecuaciones1++;
                    }

                    else if (D1 < 0 && D2 == 0 && D3 == 0)
                    {
                        Func<Double> sextaecuacion = () => W3 - (-D1);
                        functions.Add(sextaecuacion);

                        for (int j = 0; j < p.Count; j++)
                        {
                            if (p[j].Nombre == W3.Nombre) 
                            {
                                matrizauxjacob[contecuaciones1, j] = 1;

                            }
                        }
                        contecuaciones1++;
                       
                    }
                    else
                    {

                    }
                }

                else
                {

                }

                return contecuaciones1;
            }
                
//-------------------------------------------------------------------------------------------------------------------------------
            //Creamos las ecuaciones del Equipo TIPO 3: PÉRDIDA DE CARGA
            else if (TipodeEquipo == 3)
            {
                Parameter W1 = new Parameter();
                Parameter W2 = new Parameter();
                Parameter P1 = new Parameter();
                Parameter P2 = new Parameter();
                Parameter H1 = new Parameter();
                Parameter H2 = new Parameter();

                W1 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N1));
                W2 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N3));
                P1 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N1));
                P2 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N3));
                H1 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N1));
                H2 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N3));

                Func<Double> primeraecuacion = () => W1 - W2;
                functions.Add(primeraecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == W1.Nombre) || (p[j].Nombre == W2.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;
                    }
                }
                contecuaciones1++;

                if ((D1 != 0) || (D2 > 0) || (D3 > 0) || (D4 > 0))
                {
                    Func<Double> segundaecuacion = () => P1 - P2 - (D1 + (D2 * W1 / D8) + ((D3 * W1 * W1) / (D8 * D8)) + (D4 * P1));
                    functions.Add(segundaecuacion);

                    for (int j = 0; j < p.Count; j++)
                    {
                        if ((p[j].Nombre == P1.Nombre) || (p[j].Nombre == P2.Nombre) || (p[j].Nombre == W1.Nombre))
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;
                        }
                    }
                    contecuaciones1++;
                }

                else if (((D1 != 0) || (D2 > 0) || (D3 > 0) || (D4 > 0)) && ((D5 > 0) || (D6 > 0) || (D7 > 0)))

                {
                    Func<Double> segundaecuacion = () => P1 - P2 - ((D1 + (D2 * W1 / D8) + ((D3 * W1 * W1) / (D8 * D8)) + (D4 * P1))) - (((D5 * W2 * W2) / (Math.PI * Math.PI / 16)) * (acceso.vmediaph(P1, H1,P2,H2) * acceso.vmediaph(P1, H1,P2,H2) / (D6 * D6 * D6 * D6)) * (1 / (2 * 32.15236)) * (0.006944444 / acceso.vmediaph(P1, H1,P2,H2)));
                    functions.Add(segundaecuacion);

                    for (int j = 0; j < p.Count; j++)
                    {
                        if ((p[j].Nombre == P1.Nombre) || (p[j].Nombre == P2.Nombre) || (p[j].Nombre == W1.Nombre) || (p[j].Nombre == W2.Nombre) || (p[j].Nombre == H1.Nombre))
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;

                        }
                    }
                    contecuaciones1++;
                }

                else if ((D5 > 0) || (D6 > 0) || (D7 > 0))
                {
                    Func<Double> segundaecuacion = () => P1 - P2 - (((D5 * W2 * W2) / (Math.PI * Math.PI / 16)) * (acceso.vmediaph(P1, H1,P2,H2) * acceso.vmediaph(P1, H1,P2,H2) / (D6 * D6 * D6 * D6)) * (1 / (2 * 32.15236)) * (0.006944444 / acceso.vmediaph(P1, H1,P2,H2)));
                    functions.Add(segundaecuacion);

                    for (int j = 0; j < p.Count; j++)
                    {
                        if ((p[j].Nombre == P1.Nombre) || (p[j].Nombre == P2.Nombre)  || (p[j].Nombre == W2.Nombre) || (p[j].Nombre == H1.Nombre))
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;

                        }
                    }
                    contecuaciones1++;
                }

                else
                {

                }

                Func<Double> terceraecuacion = () => H1 - H2;
                functions.Add(terceraecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == H1.Nombre) || (p[j].Nombre == H2.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;

                return contecuaciones1;
            }
//--------------------------------------------------------------------------------------------------------------------------------
            //--------------------------------------------------------------------------------------------------------------------------------
            //Creamos las ecuaciones del Equipo TIPO 4: BOMBA
            else if (TipodeEquipo == 4)
            {
                Double Cau = 0;

                Parameter W1 = new Parameter();
                Parameter W2 = new Parameter();
                Parameter P1 = new Parameter();
                Parameter P2 = new Parameter();
                Parameter H1 = new Parameter();
                Parameter H2 = new Parameter();

                W1 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N1));
                W2 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N3));
                P1 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N1));
                P2 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N3));
                H1 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N1));
                H2 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N3));


                //PRIMERA ECUACION: continuidad de caudal
                Func<Double> primeraecuacion = () => W1 - W2;
                functions.Add(primeraecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == W1.Nombre) || (p[j].Nombre == W2.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;


                //SEGUNDA ECUACION: balance energético
                //Potencia = Caudal (Lb/sg)* VolumenEspecifico (Ft3/Lb) * 32.174 (gravity aceleration) * Pressure (Lb/Ft2) = Lb x Ft /sg
                Double Potencia = 0;
                //Potencia: W1*0.4536(para pasar de Lb/sg a Kg/sg)
                //acceso.vmediaph(P1, H1, P2, H2)/16.01846) (para pasar el volumen específico de Ft3/Lb a M3/Kg
                //((P1 - P2) * 6.894757)) para pasar el incremento de presion de psi a kPa
                //Potencia= Kg/sg x m3/kg x kPa = KW. Para pasar de KW a BTU/sg 
                Potencia = (((W1 * 0.4536) * (acceso.vmediaph(P1, H1, P2, H2) / 16.01846) * ((P1 - P2) * 6.894757)) / 0.85) * 0.9486608;
                Func<Double> segundaecuacion = () => (W1 * (H2 - H1)) - ((((W1 * 0.4536) * (acceso.vmediaph(P1, H1, P2, H2) / 16.01846) * ((P2 - P1) * 6.894757)) / 0.85) * 0.9486608);
                functions.Add(segundaecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == W1.Nombre) || (p[j].Nombre == H1.Nombre) || (p[j].Nombre == H2.Nombre) || (p[j].Nombre == P2.Nombre) || (p[j].Nombre == P1.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;


                //TERCERA ECUACIÓN: Presion de Descarga
                //En las Tablas se dará el Caudal y el TDH de la Bomba. 
                //D4 es el rendimiento de la Bomba
                if ((D4 > 0) && (D9 != 0))
                {
                    if (unidades == 0)
                    {
                        //Convertir a Lb/sg a GPM: Lb/sg x Ft3/Lb = Ft3/sg; Ft3/sg * 60 * 7.48051948= GPM;  
                        Cau = (W1 * acceso.vph(P1, H1) * 3600 * 0.02831685) / D7;
                        //Convertimos los resultados de la tabla de Ft a m de H2O multiplicando por 0,3048
                        //Por comodidad hemos trabajado con kg/sg, kPa y m3/kg y luego hemos pasado a psi multiplicando por 0.1450377
                        Func<Double> terceraecuacion = () => P2 - (P1 * D9) - ((tabla(D8, ((W1 * acceso.vph(P1, H1) * 60 * 7.48051948) / D7), 3) * 0.3048 * (1 / (acceso.vph(P1, H1) / 16.01846)) * 9.80665 / 1000) * 0.1450377);
                        functions.Add(terceraecuacion);

                        for (int j = 0; j < p.Count; j++)
                        {
                            if ((p[j].Nombre == P2.Nombre) || (p[j].Nombre == P1.Nombre) || (p[j].Nombre == W1.Nombre))
                            {
                                matrizauxjacob[contecuaciones1, j] = 1;
                            }
                        }
                        contecuaciones1++;
                    }

                    else if (unidades == 1)
                    {
                        //Convertir a Lb/sg a m3/hr: Lb/sg x Ft3/Lb = Ft3/sg; Ft3/sg * 3600 * 0.02831685 = m3/hr;  
                        Cau = (W1 * acceso.vph(P1, H1) * 3600 * 0.02831685) / D7;
                        //Convertimos los resultados de la tabla de m de H2O a Ft de H2O multiplicando por 3.28083
                        Func<Double> terceraecuacion = () => P2 - (P1 * D9) - ((tabla(D8, ((W1 * acceso.vph(P1, H1) * 3600 * 0.02831685) / D7), 3) * (1 / (acceso.vph(P1, H1) / 16.01846)) * 9.80665 / 1000) * 0.1450377);
                        functions.Add(terceraecuacion);

                        for (int j = 0; j < p.Count; j++)
                        {
                            if ((p[j].Nombre == P2.Nombre) || (p[j].Nombre == P1.Nombre) || (p[j].Nombre == W1.Nombre) || (p[j].Nombre == H1.Nombre))
                            {
                                matrizauxjacob[contecuaciones1, j] = 1;

                            }
                        }
                        contecuaciones1++;
                    }

                    else if (unidades == 2)
                    {
                        //Convertir a Lb/sg a m3/hr: Lb/sg x Ft3/Lb = Ft3/sg; Ft3/sg * 3600 * 0.02831685 = m3/hr;  
                        Cau = (W1 * acceso.vph(P1, H1) * 3600 * 0.02831685) / D7;
                        //Convertimos los resultados de la tabla de m de H2O a Ft de H2O multiplicando por 3.28083
                        Func<Double> terceraecuacion = () => P2 - (P1 * D9) - ((tabla(D8, ((W1 * acceso.vph(P1, H1) * 3600 * 0.02831685) / D7), 3) * (1 / (acceso.vph(P1, H1) / 16.01846)) * 9.80665 / 1000) * 0.1450377);
                        functions.Add(terceraecuacion);

                        for (int j = 0; j < p.Count; j++)
                        {
                            if ((p[j].Nombre == P2.Nombre) || (p[j].Nombre == P1.Nombre) || (p[j].Nombre == W1.Nombre) || (p[j].Nombre == H1.Nombre))
                            {
                                matrizauxjacob[contecuaciones1, j] = 1;

                            }
                        }
                        contecuaciones1++;
                    }
                }

                // Calculo de la Presión de Descarga D5.
                else if ((D9 == 0) && (D5 > 0))
                {
                    Func<Double> terceraecuacion = () => P2 - D5;
                    functions.Add(terceraecuacion);

                    for (int j = 0; j < p.Count; j++)
                    {
                        if (p[j].Nombre == P2.Nombre)
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;

                        }
                    }
                    contecuaciones1++;
                }

                else if ((D1 > 0) || (D2 > 0) || (D3 > 0))
                {
                    Func<Double> terceraecuacion = () => P2 - (P1 * D9) - (D1 + D2 * (W1 / D7) + D3 * (W1 * W1 / D7));
                    functions.Add(terceraecuacion);

                    for (int j = 0; j < p.Count; j++)
                    {
                        if ((p[j].Nombre == P2.Nombre) || (p[j].Nombre == P1.Nombre) || (p[j].Nombre == W1.Nombre))
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;

                        }
                    }
                    contecuaciones1++;
                }

                //No se genera ecuación alguna en caso de que D1=D2=D3=D5=D8=0
                else if ((D1 == 0) && (D2 == 0) && (D3 == 0) && (D5 == 0) && (D8 == 0))
                { 
                
                
                }
            }
//--------------------------------------------------------------------------------------------------------------------------------
            //Creamos las ecuaciones del Equipo TIPO 5: MEZCLADOR
            else if (TipodeEquipo == 5)
            {
                Parameter W1 = new Parameter();
                Parameter W2 = new Parameter();
                Parameter W3 = new Parameter();
                Parameter P1 = new Parameter();
                Parameter P2 = new Parameter();
                Parameter P3 = new Parameter();
                Parameter H1 = new Parameter();
                Parameter H2 = new Parameter();
                Parameter H3 = new Parameter();

                W1 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N1));
                W2 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N2));
                W3 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N3));
                P1 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N1));
                P2 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N2));
                P3 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N3));
                H1 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N1));
                H2 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N2));
                H3 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N3));

                Func<Double> primeraecuacion = () => W3 - W1 - W2;
                functions.Add(primeraecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == W1.Nombre) || (p[j].Nombre == W2.Nombre) || (p[j].Nombre == W3.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;
                
                Func<Double> segundaecuacion = () => (H3 * (W1 + W2)) - (W1 * H1) - (W2 * H2);
                functions.Add(segundaecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == W1.Nombre) || (p[j].Nombre == W2.Nombre) || (p[j].Nombre == H3.Nombre) || (p[j].Nombre == H2.Nombre) || (p[j].Nombre == H1.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;
                

                if (D1 > 0)
                {
                    Func<Double> terceraecuacion = () => P3 - D1;
                    functions.Add(terceraecuacion);

                    for (int j = 0; j < p.Count; j++)
                    {
                        if (p[j].Nombre == P3.Nombre) 
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;

                        }
                    }
                    contecuaciones1++;
                }
                else if (D1 == 0)
                {
                    Func<Double> terceraecuacion = () => P3 - P1;
                    functions.Add(terceraecuacion);

                    for (int j = 0; j < p.Count; j++)
                    {
                        if ((p[j].Nombre == P3.Nombre)|| (p[j].Nombre == P1.Nombre))
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;

                        }
                    }
                    contecuaciones1++;
                }
                else
                {

                }

                if (D9 == 1)
                {
                    Func<Double> cuartaecuacion = () => P2 - P1;
                    functions.Add(cuartaecuacion);

                    for (int j = 0; j < p.Count; j++)
                    {
                        if ((p[j].Nombre == P2.Nombre) || (p[j].Nombre == P1.Nombre))
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;

                        }
                    }
                    contecuaciones1++;
                }

                else if (D9 != 1)
                {
                   
                }

                else
                {

                }

                return contecuaciones1;
            }
//--------------------------------------------------------------------------------------------------------------------------------
            //Creamos las ecuaciones del Equipo TIPO 6: REACTOR
            else if (TipodeEquipo == 6)
            {
                Parameter W1 = new Parameter();
                Parameter W2 = new Parameter();
                Parameter P1 = new Parameter();
                Parameter P2 = new Parameter();
                Parameter H1 = new Parameter();
                Parameter H2 = new Parameter();

                W1 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N1));
                W2 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N3));
                P1 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N1));
                P2 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N3));
                H1 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N1));
                H2 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N3));

                Func<Double> primeraecuacion = () => W1 - W2;
                functions.Add(primeraecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == W1.Nombre) || (p[j].Nombre == W2.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;

                if (D4 < 0)
                {                    
                    Func<Double> segundaecuacion = () => P2 + D4;
                    functions.Add(segundaecuacion);

                    for (int j = 0; j < p.Count; j++)
                    {
                        if (p[j].Nombre == P2.Nombre)
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;

                        }
                    }
                    contecuaciones1++;
                }

                else if ((D4 >= 0)||(D1>0)||(D2>0)||(D3>0))
                {                    
                    Func<Double> segundaecuacion = () => P1 - P2 - D1 - (D2 * W1) - (D3 * W1 * W1) - (D4 * P1);
                    functions.Add(segundaecuacion);

                    for (int j = 0; j < p.Count; j++)
                    {
                        if ((p[j].Nombre == P1.Nombre) || (p[j].Nombre == P2.Nombre) || (p[j].Nombre == W1.Nombre))
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;

                        }
                    }
                    contecuaciones1++;
                }

                if (D5 > 0)
                {
                    Func<Double> terceraecuacion = () => H2 - D5;
                    functions.Add(terceraecuacion);

                    for (int j = 0; j < p.Count; j++)
                    {
                        if (p[j].Nombre == H2.Nombre) 
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;

                        }
                    }
                    contecuaciones1++;
                }

                else if (D5 < 0)
                {
                    Func<Double> terceraecuacion = () => acceso.tph(Math.Abs(P2), H2) + D5;
                    functions.Add(terceraecuacion);

                    for (int j = 0; j < p.Count; j++)
                    {
                        if ((p[j].Nombre == P2.Nombre) || (p[j].Nombre == H2.Nombre))
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;

                        }
                    }
                    contecuaciones1++;
                }

                else if (D6 != 0)
                {
                    Func<Double> terceraecuacion = () => (W1 * ((H2 - H1) / D7)) - D6;
                    functions.Add(terceraecuacion);

                    for (int j = 0; j < p.Count; j++)
                    {
                        if ((p[j].Nombre == W1.Nombre) || (p[j].Nombre == H2.Nombre) || (p[j].Nombre == H1.Nombre))
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;

                        }
                    }
                    contecuaciones1++;
                }

                return contecuaciones1;
            }
//--------------------------------------------------------------------------------------------------------------------------------
            //Creamos las ecuaciones del Equipo TIPO 7: CALENTADOR
            else if (TipodeEquipo == 7)
            {
                Parameter W1 = new Parameter();
                Parameter W2 = new Parameter();
                Parameter W3 = new Parameter();
                Parameter W4 = new Parameter();
                Parameter W5 = new Parameter();

                Parameter P1 = new Parameter();
                Parameter P2 = new Parameter();
                Parameter P3 = new Parameter();
                Parameter P4 = new Parameter();
                Parameter P5 = new Parameter();

                Parameter H1 = new Parameter();
                Parameter H2 = new Parameter();
                Parameter H3 = new Parameter();
                Parameter H4 = new Parameter();
                Parameter H5 = new Parameter();
                
                W1 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N1));
                W2 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N2));
                if (D9 != 0)
                {
                    W5 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(D9));
                }
                W3 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N3));
                W4 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N4));

                P1 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N1));
                P2 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N2));
                if (D9 != 0)
                {
                    P5 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(D9));
                }
                P3 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N3));
                P4 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N4));

                H1 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N1));
                H2 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N2));
                if (D9 != 0)
                {
                    H5 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(D9));
                }
                H3 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N3));
                H4 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N4));

                //PRIMERA ECUACIÓN: Continuidad de Caudal Lado Tubos
                Func<Double> primeraecuacion = () => W1 - W3;
                functions.Add(primeraecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == W1.Nombre) || (p[j].Nombre == W3.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;

                //SEGUNDA ECUACIÓN: Continuidad de Caudal Lado Carcasa
                if (D9 != 0)
                {
                    Func<Double> segundaecuacion = () => W2 + W5 - W4;
                    functions.Add(segundaecuacion);
                }

                else if (D9 == 0)
                {
                    Func<Double> segundaecuacion = () => W2 - W4;
                    functions.Add(segundaecuacion);
                }             
                
                for (int j = 0; j < p.Count; j++)
                {
                    if (D9!=0)
                    { 
                       if ((p[j].Nombre == W2.Nombre) || (p[j].Nombre == W5.Nombre) || (p[j].Nombre == W4.Nombre))
                       {
                          matrizauxjacob[contecuaciones1, j] = 1;
                       }
                    }

                    else if (D9 ==0)
                    {
                        if ((p[j].Nombre == W2.Nombre) || (p[j].Nombre == W4.Nombre))
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;
                        }
                    } 
                }

                contecuaciones1++;

                //TERCERA ECUACIÓN: Igualdad Presiones Lado Carcasa
                Func<Double> terceraecuacion = () => P4 - P2;
                functions.Add(terceraecuacion);


                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == P4.Nombre) || (p[j].Nombre == P2.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;

                //CUARTA ECUACIÓN: Pérdida de Carga Lado Tubos
                Func<Double> cuartaecuacion = () => P1 - P3 - (D1 + (D2 * W1) + (D3 * W1 * W1));
                functions.Add(cuartaecuacion);


                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == P1.Nombre) || (p[j].Nombre == P3.Nombre) || (p[j].Nombre == W1.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;

                //QUINTA ECUACIÓN:Balance Energético
                if (D9 != 0)
                {
                    Func<Double> quintaecuacion = () => (W1 * (H3 - H1)) - (W2 * (H2 - H4) * D6) - (D6 * W5 * (H5 - H4));
                    functions.Add(quintaecuacion);
                }
                else if (D9 == 0)
                {
                    Func<Double> quintaecuacion = () => (W1 * (H3 - H1)) - (W2 * (H2 - H4) * D6);
                    functions.Add(quintaecuacion);
                }

                for (int j = 0; j < p.Count; j++)
                {
                    if (D9 != 0)
                    {

                        if ((p[j].Nombre == W1.Nombre) || (p[j].Nombre == H3.Nombre) || (p[j].Nombre == H1.Nombre) || (p[j].Nombre == W2.Nombre) || (p[j].Nombre == H2.Nombre) || (p[j].Nombre == H4.Nombre) || (p[j].Nombre == W5.Nombre) || (p[j].Nombre == H5.Nombre))
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;

                        }
                    }

                    else if (D9 == 0)
                    {
                        if ((p[j].Nombre == W1.Nombre) || (p[j].Nombre == H3.Nombre) || (p[j].Nombre == H1.Nombre) || (p[j].Nombre == W2.Nombre) || (p[j].Nombre == H2.Nombre) || (p[j].Nombre == H4.Nombre))
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;

                        }

                    }
                }
                contecuaciones1++;

                //SEXTA ECUACIÓN: Cálculo del TTD
                //Definición de TTD mediante un valor introducido por el usuario
                if (D5 <= 500)
                {
                    Func<Double> sextaecuacion = () => acceso.tph(P3, H3) - acceso.tsl(P2) + D5;
                    functions.Add(sextaecuacion);

                    for (int j = 0; j < p.Count; j++)
                    {
                        if ((p[j].Nombre == P3.Nombre) || (p[j].Nombre == H3.Nombre) || (p[j].Nombre == P2.Nombre))
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;

                        }
                    }
                    contecuaciones1++;

                }

                //Definición del TTD mediante un valor tomado desde una TABLA con número mayor de 500
                else if (D5 > 500)
                {
                    //La definición del TTD mediante una TABLA hace necesario establecer tres casos dependiendo de la UNIDADES de la TABLA
                    //Sistema de Unidades Británico
                    if (unidades == 0)
                    {
                        //Definición de la Tabla: Caudal (Lb/Hr) & TTD(ºF)
                        //Transformamos las Lb/sg a Lb/Hr para entrar en la Tabla
                        Func<Double> sextaecuacion = () => acceso.tph(P3, H3) - acceso.tsl(P2) + tabla(D5, W1 * 3600, 2);
                        functions.Add(sextaecuacion);

                        for (int j = 0; j < p.Count; j++)
                        {
                            if ((p[j].Nombre == P3.Nombre) || (p[j].Nombre == H3.Nombre) || (p[j].Nombre == P2.Nombre)|| (p[j].Nombre == W1.Nombre))
                            {
                                matrizauxjacob[contecuaciones1, j] = 1;

                            }
                        }
                        contecuaciones1++;

                    }
                    //Sistema de Unidades Métrico
                    else if (unidades == 1)
                    {
                        MessageBox.Show("Opción todavía no progamada. LUIS COCO");
                    }
                    //Sistema de Unidades Internacional
                    else if (unidades == 2)
                    {
                        //Definición de la Tabla: Caudal (Kg/sg) & TTD(ºC)
                        //Tranformamos las Lb/sg en Kgr/sg para entrar en la Tabla. Y convertimos el TTD desde ºC a ºF.
                        //El TTD es un Incremento de Temperatura por lo que para transformarlo de ºC a ºF sólo hay que multiplicar por 9/5
                        Func<Double> sextaecuacion = () => acceso.tph(P3, H3) - acceso.tsl(P2) + (((tabla(D5, W1 * 0.4536, 3) * 9) / 5));
                        functions.Add(sextaecuacion);

                         for (int j = 0; j < p.Count; j++)
                        {
                            if ((p[j].Nombre == P3.Nombre) || (p[j].Nombre == H3.Nombre) || (p[j].Nombre == P2.Nombre)|| (p[j].Nombre == W1.Nombre))
                            {
                                matrizauxjacob[contecuaciones1, j] = 1;

                            }
                        }
                        contecuaciones1++;
                    }
                }

                //SEPTIMA ECUACIÓN: Cálculo del DCA
                //Caso de CALENTADOR HÚMEDO (con DCA>0)
                if (D4 > 0)
                {
                    //Definición de DCA mediante un valor introducido por el usuario
                    if (D4 <= 500)
                    {
                        Func<Double> septimaecuacion = () => acceso.tph(P4, H4) - acceso.tph(P1, H1) - D4;
                        functions.Add(septimaecuacion);

                         for (int j = 0; j < p.Count; j++)
                        {
                            if ((p[j].Nombre == P4.Nombre) || (p[j].Nombre == H4.Nombre) || (p[j].Nombre == P1.Nombre)|| (p[j].Nombre == H1.Nombre))
                            {
                                matrizauxjacob[contecuaciones1, j] = 1;

                            }
                        }
                        contecuaciones1++;
                    }

                    else if (D4 > 500)
                    {
                        //La definición del DCA mediante una TABLA hace necesario establecer tres casos dependiendo de la UNIDADES de la TABLA
                        //Sistema de Unidades Británico
                        if (unidades == 0)
                        {
                            //Definición de la Tabla: Caudal (Lb/Hr) & TTD(ºF)
                            //Transformamos las Lb/sg a Lb/Hr para entrar en la Tabla
                            Func<Double> septimaecuacion = () => acceso.tph(P4, H4) - acceso.tph(P1, H1) - tabla(D4, W1 * 3600, 2);
                            functions.Add(septimaecuacion);

                        for (int j = 0; j < p.Count; j++)
                        {
                            if ((p[j].Nombre == P4.Nombre) || (p[j].Nombre == H4.Nombre) || (p[j].Nombre == P1.Nombre)|| (p[j].Nombre == H1.Nombre)|| (p[j].Nombre == W1.Nombre))
                            {
                                matrizauxjacob[contecuaciones1, j] = 1;

                            }
                        }
                        contecuaciones1++;

                        }
                        //Sistema de Unidades Métrico
                        else if (unidades == 1)
                        {
                            MessageBox.Show("Opción todavía no progamada. LUIS COCO");
                        }
                        else if (unidades == 2)
                        {
                            //Definición de la Tabla: Caudal (Kg/sg) & TTD(ºC)
                            //Tranformamos las Lb/sg en Kgr/sg para entrar en la Tabla. Y convertimos el TTD desde ºC a ºF.
                            //El TTD es un Incremento de Temperatura por lo que para transformarlo de ºC a ºF sólo hay que multiplicar por 9/5
                            Func<Double> septimaecuacion = () => acceso.tph(P4, H4) - acceso.tph(P1, H1) - (((tabla(D4, W1 * 0.4536, 3) * 9) / 5));
                            functions.Add(septimaecuacion);

                        for (int j = 0; j < p.Count; j++)
                        {
                            if ((p[j].Nombre == P4.Nombre) || (p[j].Nombre == H4.Nombre) || (p[j].Nombre == P1.Nombre)|| (p[j].Nombre == H1.Nombre)|| (p[j].Nombre == W1.Nombre))
                            {
                                matrizauxjacob[contecuaciones1, j] = 1;

                            }
                        }
                        contecuaciones1++;
                        }
                    }
                }

                //Caso de CALENTADOR SECO (con DCA=0)
                else if (D4 == 0)
                {
                    Func<Double> septimaecuacion = () => acceso.tph(P4, H4) - acceso.tsl(P2);
                    functions.Add(septimaecuacion);

                     for (int j = 0; j < p.Count; j++)
                        {
                            if ((p[j].Nombre == P4.Nombre) || (p[j].Nombre == H4.Nombre) || (p[j].Nombre == P1.Nombre)|| (p[j].Nombre == H1.Nombre))
                            {
                                matrizauxjacob[contecuaciones1, j] = 1;

                            }
                        }
                        contecuaciones1++;
                }

                //OCTAVA ECUACIÓN: Igualamos las presiones de la corriente de cascada P5 con la de corriente de entrada en carcasa P2
                if (D7 == 1)
                {
                    if (D9 != 0)
                    {
                        Func<Double> octavaecuacion = () => P5 - P2;
                        functions.Add(octavaecuacion);

                        for (int j = 0; j < p.Count; j++)
                        {
                            if (D9 != 0)
                            {

                                if ((p[j].Nombre == P5.Nombre) || (p[j].Nombre == P2.Nombre))
                                {
                                    matrizauxjacob[contecuaciones1, j] = 1;

                                }
                            }
                        }

                        contecuaciones1++;
                    }
                    else if (D9 == 0)
                    { 
                    
                    
                    }                       
                }
            }
//--------------------------------------------------------------------------------------------------------------------------------
            //Creamos las ecuaciones del Equipo TIPO 8: CONDENSADOR PRINCIPAL
            else if (TipodeEquipo == 8)
            {
                //Cálculo tradicional del Condensador fijando su presión de vacio. Pv= D1 + D2xQ + D3xQXQ
                if (D9==0)
                {
                    Parameter W1 = new Parameter();
                    Parameter W2 = new Parameter();
                    Parameter W3 = new Parameter();
                    Parameter P1 = new Parameter();
                    Parameter P2 = new Parameter();
                    Parameter P3 = new Parameter();
                    Parameter H1 = new Parameter();
                    Parameter H2 = new Parameter();
                    Parameter H3 = new Parameter();

                    W1 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N1));
                    W2 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N2));
                    W3 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N3));
                    P1 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N1));
                    P2 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N2));
                    P3 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N3));
                    H1 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N1));
                    H2 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N2));
                    H3 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N3));

                    Func<Double> primeraecuacion = () => W1 + W2 - W3;
                    functions.Add(primeraecuacion);

                    for (int j = 0; j < p.Count; j++)
                    {
                        if ((p[j].Nombre == W1.Nombre) || (p[j].Nombre == W2.Nombre) || (p[j].Nombre == W3.Nombre))
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;
                        }
                    }
                    contecuaciones1++;
                    
                    Func<Double> segundaecuacion = () => P1 - P3;
                    functions.Add(segundaecuacion);

                    for (int j = 0; j < p.Count; j++)
                    {
                        if ((p[j].Nombre == P1.Nombre) || (p[j].Nombre == P3.Nombre))
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;
                        }
                    }
                    contecuaciones1++;

                    //Siendo D4=Q(calor rechazado), la presión de vacio Pv vendría definida por la siguiente fórmula.
                    //Pv=D1+D2*Q+D3*Q*Q
                    Double Pv = D1 + D2 * D4 + D3 * D4 * D4;
                    if ((D1>0)||(D2>0)||(D3>0)||(D4>0))
                    {
                        Func<Double> terceraecuacion = () => P1 - Pv;
                        functions.Add(terceraecuacion);

                        for (int j = 0; j < p.Count; j++)
                        {
                            if (p[j].Nombre == P1.Nombre)
                            {
                                matrizauxjacob[contecuaciones1, j] = 1;
                            }
                        }
                        contecuaciones1++;
                    }

                    Func<Double> cuartaecuacion = () => H3 - acceso.hsatpliq(P1);
                    functions.Add(cuartaecuacion);

                    for (int j = 0; j < p.Count; j++)
                    {
                        if ((p[j].Nombre == H3.Nombre) || (p[j].Nombre == P1.Nombre))
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;
                        }
                    }
                    contecuaciones1++;
                }

                //Cálculo del Condensador según el método de la HEI.
                else if (D9>0)
                {
                    //PENDIENTE IMPLEMENTAR CALCULO DEL CONDENSADOR SEGÚN LA HEI.
                    MessageBox.Show("Pendiente implementar cálculo del Condensador de acuerdo a la HEI.");                   
                }

                return contecuaciones1;
            }
//--------------------------------------------------------------------------------------------------------------------------------
//Creamos las ecuaciones del Equipo TIPO 9: TURBINA CON PÉRDIDAS EN EL ESCAPE
            else if (TipodeEquipo == 9)
            {
                Parameter W1 = new Parameter();
                Parameter W2 = new Parameter();
                Parameter P1 = new Parameter();
                Parameter P2 = new Parameter();
                Parameter H1 = new Parameter();
                Parameter H2 = new Parameter();

                W1 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N1));
                W2 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N3));
                P1 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N1));
                P2 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N3));
                H1 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N1));
                H2 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N3));

                //ECUACION PRIMERA: ecuación de continuidad de caudal a través del componente.
                Func<Double> primeraecuacion = () => W1 - W2;
                functions.Add(primeraecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == W1.Nombre) || (p[j].Nombre == W2.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;


                //Ecuación de rendimiento termodinámico (D1)
                if (D1 != 0)
                {
                    //Las Pérdidas en el Escape (D4) o nº de la Tabla que la define
                    String Hescape;
                    Hescape = "";
                    Hescape = Convert.ToString(D4);

                    //Corrección del rendimiento termodinámico por la Semisuma de Calidades de entrada y salida (D5)
                    if (D5 > 0)
                    {
                        //CÁLCULO DE PÉRDIDAS EN EL ESCAPE
                        //Definición del factor de corrección fac:
                        // a) HBAL: fac=1 (cuando la tabla viene dada en función del caudal volumétrico, es decir, D9=1)
                        // a) Exhaust Loss= fac*Calidad ELEP*Exhaust(valor de Tabla)
                        // b) ECOSIM PRO: fac= 0.87*(0.35+(0.65*Calidad ELEP))
                        // b) Exhaust loss= fac*Calidad ELEP*Exhaust(valor de Tabla)
                        // c) GENERAL ELECTRIC: fac= 0.87*(1-0.01*M)*(1-0.0065M). Donde M es la humedad a la P y H del ELEP
                        // c) Exhaust loss= fac*Exhaust(valor de Tabla)
                        // En este programa en una primera versión vamos a considerar el caso a) para poder validarlo con el HBAL

                        Double fac = 1;
                        Double elep = H1 - D1 * (H1 - acceso.hps(P2, (acceso.sph(P1, H1))));
                        Double X_ex = acceso.xph(P2, H1 - D1 * (H1 - acceso.hps(P2, (acceso.sph(P1, H1))))) / 100;
                        Double vespc = acceso.vph(P2, H1 - D1 * (H1 - acceso.hps(P2, (acceso.sph(P1, H1))))) * 0.062430234;
                        //Double resultabla = tabla(D4, ((W2 * 0.4536) * 0.062430234 * acceso.vph(P2, H1 - D1 * (H1 - acceso.hps(P2, (acceso.sph(P1, H1)))))), 2) / 2.326009;

                        //Double Velocidadescape=W2/(punteroaplicacion1.acceso.rh);

                        //Pérdidas en el escape en función del Area de Escape (D9=!1)
                        if ((D9 > 0) && (D9 != 1))
                        {
                            Func<Double> terceraecuacion = () => H2 - H1 + rendcorrejido(P1, H1, P2, H2, D1, D5) * (H1 - acceso.hps(P2, (acceso.sph(P1, H1)))) + (fac * tabla(D4, D9, 2));
                            functions.Add(terceraecuacion);

                            for (int j = 0; j < p.Count; j++)
                            {
                                if ((p[j].Nombre == H2.Nombre) || (p[j].Nombre == H1.Nombre) || (p[j].Nombre == P1.Nombre) || (p[j].Nombre == P2.Nombre))
                                {
                                    matrizauxjacob[contecuaciones1, j] = 1;

                                }
                            }
                            contecuaciones1++;
                        }

                        //Pérdidas en el escape en función del Caudal Volumétrico a la Descarga (D9=1)
                        //VALIDACIÓN CON HBAL
                        else if (D9 == 1)
                        {
                            //Debido a las Unidades en que están definidas las TABLAS es necesario la formulación de deferentes ecuaciones.
                            //Sistema de Unidades Internacional
                            if (unidades == 2)
                            {
                                //Tabla: Caudal Volumétrico (m3/sg) & Pérdidas en el Escape (kj/kg)
                                //Factor 0.4536 pasamos Lb/sg a Kg/sg;Factor 0.062430234 pasamos el volu.espe de Ft3/Lb a M3/kg;Factor 2.326009 pasamos de kj/kg a BTU/kg;
                                Func<Double> terceraecuacion = () => H2 - H1 + rendcorrejido(P1, H1, P2, H2, D1, D5) * (H1 - acceso.hps(P2, (acceso.sph(P1, H1)))) - (fac * (acceso.xph(P2, H1 - D1 * (H1 - acceso.hps(P2, (acceso.sph(P1, H1))))) / 100) * (tabla(D4, ((W2 * 0.4536) * 0.062430234 * acceso.vph(P2, H1 - D1 * (H1 - acceso.hps(P2, (acceso.sph(P1, H1)))))), 2) / 2.326009));
                                functions.Add(terceraecuacion);

                                for (int j = 0; j < p.Count; j++)
                                {
                                    if ((p[j].Nombre == H2.Nombre) || (p[j].Nombre == H1.Nombre) || (p[j].Nombre == P1.Nombre) || (p[j].Nombre == P2.Nombre) || (p[j].Nombre == W2.Nombre))
                                    {
                                        matrizauxjacob[contecuaciones1, j] = 1;

                                    }
                                }
                                contecuaciones1++;
                            }
                            //Sistema de Unidades Británicas
                            //Tabla: Caudal Volumétrico () & Pérdidas en el Escape (BTU/Lb)
                            //PENDIENTE pasar de Lb/sg a Ft3/Lb o GPM??? dependiente de la definición de la tabla
                            else if (unidades == 0)
                            {
                                //Func<Double> terceraecuacion = () => H2 - H1 + rendcorrejido(P1, H1, P2, H2, D1, D5) * (H1 - acceso.hps(P2, (acceso.sph(P1, H1)))) - (fac * (acceso.xph(P2, H1 - D1 * (H1 - acceso.hps(P2, (acceso.sph(P1, H1))))) / 100) * (tabla(D4, ((W2 * 0.4536) * 0.062430234 * acceso.vph(P2, H1 - D1 * (H1 - acceso.hps(P2, (acceso.sph(P1, H1)))))), 2) / 2.326009));
                                //functions.Add(terceraecuacion);                            
                            }

                            else if (unidades == 1)
                            {
                                MessageBox.Show("Todavía no me ha dado tiempo a programar esta opción. LUIS COCO");
                            }
                        }

                        //No se consideran las Pérdidas en el Escape (D9=0)
                        else if (D9 == 0)
                        {
                            Func<Double> terceraecuacion = () => H2 - H1 + rendcorrejido(P1, H1, P2, H2, D1, D5) * (H1 - acceso.hps(P2, (acceso.sph(P1, H1))));
                            functions.Add(terceraecuacion);

                            for (int j = 0; j < p.Count; j++)
                            {
                                if ((p[j].Nombre == H2.Nombre) || (p[j].Nombre == H1.Nombre) || (p[j].Nombre == P1.Nombre) || (p[j].Nombre == P2.Nombre))
                                {
                                    matrizauxjacob[contecuaciones1, j] = 1;

                                }
                            }
                            contecuaciones1++;
                        }
                    }

                    //No hay corrección del rendimiento termodinámico
                    else
                    {
                        //TABLA DE PERDIDAS EN EL ESCAPE
                        Double fac = 1;
                        Double elep = H1 - D1 * (H1 - acceso.hps(P2, (acceso.sph(P1, H1))));
                        Double X_ex = acceso.xph(P2, H1 - D1 * (H1 - acceso.hps(P2, (acceso.sph(P1, H1))))) / 100;
                        Double vespc = acceso.vph(P2, H1 - D1 * (H1 - acceso.hps(P2, (acceso.sph(P1, H1))))) * 0.062430234;
                        //Double resultabla = tabla(D4, ((W2 * 0.4536) * 0.062430234 * acceso.vph(P2, H1 - D1 * (H1 - acceso.hps(P2, (acceso.sph(P1, H1)))))), 2) / 2.326009;

                        //Pérdidas en el escape en función del Area de Escape
                        if ((D9 > 0) && (D9 != 1))
                        {
                            Func<Double> terceraecuacion = () => H2 - H1 + D1 * (H1 - acceso.hps(P2, (acceso.sph(P1, H1)))) + (fac * (tabla(D4, D9, 2)));
                            functions.Add(terceraecuacion);

                            for (int j = 0; j < p.Count; j++)
                            {
                                if ((p[j].Nombre == H2.Nombre) || (p[j].Nombre == H1.Nombre) || (p[j].Nombre == P1.Nombre) || (p[j].Nombre == P2.Nombre))
                                {
                                    matrizauxjacob[contecuaciones1, j] = 1;

                                }
                            }
                            contecuaciones1++;
                        }

                        //Pérdidas en el escape en función del Caudal Volumétrico a la Descarga
                        else if (D9 == 1)
                        {
                            //Debido a las Unidades en que están definidas las TABLAS es necesario la formulación de deferentes ecuaciones.
                            //Sistema de Unidades Internacional
                            if (unidades == 2)
                            {
                                //Tabla: Caudal Volumétrico (m3/sg) & Pérdidas en el Escape (kj/kg)
                                //Factor 0.4536 pasamos Lb/sg a Kg/sg;Factor 0.062430234 pasamos el volu.espe de Ft3/Lb a M3/kg;Factor 2.326009 pasamos de kj/kg a BTU/kg;
                                Func<Double> terceraecuacion = () => H2 - H1 + D1 * (H1 - acceso.hps(P2, (acceso.sph(P1, H1)))) - (fac * (acceso.xph(P2, H1 - D1 * (H1 - acceso.hps(P2, (acceso.sph(P1, H1))))) / 100) * (tabla(D4, ((W2 * 0.4536) * 0.062430234 * acceso.vph(P2, H1 - D1 * (H1 - acceso.hps(P2, (acceso.sph(P1, H1)))))), 2) / 2.326009));
                                functions.Add(terceraecuacion);

                                for (int j = 0; j < p.Count; j++)
                                {
                                    if ((p[j].Nombre == H2.Nombre) || (p[j].Nombre == H1.Nombre) || (p[j].Nombre == P1.Nombre) || (p[j].Nombre == P2.Nombre) || (p[j].Nombre == W2.Nombre))
                                    {
                                        matrizauxjacob[contecuaciones1, j] = 1;

                                    }
                                }
                                contecuaciones1++;
                            }
                            //Sistema de Unidades Británicas
                            //Tabla: Caudal Volumétrico () & Pérdidas en el Escape (BTU/Lb)
                            //PENDIENTE pasar de Lb/sg a Ft3/Lb o GPM??? dependiente de la definición de la tabla
                            else if (unidades == 0)
                            {
                                //Func<Double> terceraecuacion = () => H2 - H1 + rendcorrejido(P1, H1, P2, H2, D1, D5) * (H1 - acceso.hps(P2, (acceso.sph(P1, H1)))) - (fac * (acceso.xph(P2, H1 - D1 * (H1 - acceso.hps(P2, (acceso.sph(P1, H1))))) / 100) * (tabla(D4, ((W2 * 0.4536) * 0.062430234 * acceso.vph(P2, H1 - D1 * (H1 - acceso.hps(P2, (acceso.sph(P1, H1)))))), 2) / 2.326009));
                                //functions.Add(terceraecuacion);                            
                            }

                            else if (unidades == 1)
                            {
                                MessageBox.Show("Todavía no me ha dado tiempo a programar esta opción. LUIS COCO");
                            }
                        }

                        //No se consideran las Pérdidas en el Escape
                        else if (D9 == 0)
                        {
                            Func<Double> terceraecuacion = () => H2 - H1 + D1 * (H1 - acceso.hps(P2, (acceso.sph(P1, H1))));
                            functions.Add(terceraecuacion);

                            for (int j = 0; j < p.Count; j++)
                            {
                                if ((p[j].Nombre == H2.Nombre) || (p[j].Nombre == H1.Nombre) || (p[j].Nombre == P1.Nombre) || (p[j].Nombre == P2.Nombre))
                                {
                                    matrizauxjacob[contecuaciones1, j] = 1;

                                }
                            }
                            contecuaciones1++;
                        }
                    }
                }

                //Ecuación del Factor de Flujo
                if (D3 > 0)
                {
                    if (D8 > 0) //Corrección del Factor de Flujo mediante la relación de presiones Psalida/Pentrada (D8)
                    {
                        Func<Double> segundaecuacion = () => W1 - (FactorFlujocorrejido(P1, P2, acceso.xph(P1, H1), D3, D8) * (Math.Sqrt(P1 / (acceso.vph(P1, H1)))));
                        functions.Add(segundaecuacion);

                        for (int j = 0; j < p.Count; j++)
                        {
                            if ((p[j].Nombre == W1.Nombre) || (p[j].Nombre == H1.Nombre) || (p[j].Nombre == P1.Nombre) || (p[j].Nombre == P2.Nombre))
                            {
                                matrizauxjacob[contecuaciones1, j] = 1;

                            }
                        }
                        contecuaciones1++;
                    }
                    else //No corrección del Factor de Flujo
                    {
                        Func<Double> segundaecuacion = () => W1 - (D3 * (Math.Sqrt(P1 / (acceso.vph(P1, H1)))));
                        functions.Add(segundaecuacion);

                        for (int j = 0; j < p.Count; j++)
                        {
                            if ((p[j].Nombre == W1.Nombre) || (p[j].Nombre == H1.Nombre) || (p[j].Nombre == P1.Nombre))
                            {
                                matrizauxjacob[contecuaciones1, j] = 1;

                            }
                        }
                        contecuaciones1++;
                    }
                }
            }
//-------------------------------------------------------------------------------------------------------------------------------
            //Creamos las ecuaciones del Equipo TIPO 10: TURBINA SIN PÉRDIDAS EN EL ESCAPE
            else if (TipodeEquipo == 10)
            {
                Parameter W1 = new Parameter();
                Parameter W2 = new Parameter();
                Parameter P1 = new Parameter();
                Parameter P2 = new Parameter();
                Parameter H1 = new Parameter();
                Parameter H2 = new Parameter();

                W1 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N1));
                W2 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N3));
                P1 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N1));
                P2 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N3));
                H1 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N1));
                H2 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N3));

                Func<Double> primeraecuacion = () => W1 - W2;
                functions.Add(primeraecuacion);

                 for (int j = 0; j < p.Count; j++)
                        {
                            if ((p[j].Nombre == W1.Nombre) || (p[j].Nombre == W2.Nombre))
                            {
                                matrizauxjacob[contecuaciones1, j] = 1;

                            }
                        }
                        contecuaciones1++;

                //Ecuación de rendimiento termodinámico
                if (D1 != 0)
                {
                    //Corrección del rendimiento termodinámico
                    if (D5 > 0)
                    {
                        //No se consideran las Pérdidas en el Escape
                        Func<Double> terceraecuacion = () => H2 - H1 + rendcorrejido(P1, H1, P2, H2, D1, D5) * (H1 - acceso.hps(P2, (acceso.sph(P1, H1))));
                        functions.Add(terceraecuacion);

                        for (int j = 0; j < p.Count; j++)
                        {
                            if ((p[j].Nombre == H1.Nombre) || (p[j].Nombre == H2.Nombre)|| (p[j].Nombre == P2.Nombre)|| (p[j].Nombre == P1.Nombre))
                            {
                                matrizauxjacob[contecuaciones1, j] = 1;

                            }
                        }
                        contecuaciones1++;
                    }

                    //No hay corrección del rendimiento termodinámico
                    else
                    {

                        Func<Double> terceraecuacion = () => H2 - H1 + D1 * (H1 - acceso.hps(P2, (acceso.sph(P1, H1))));
                        functions.Add(terceraecuacion);

                         for (int j = 0; j < p.Count; j++)
                        {
                            if ((p[j].Nombre == H1.Nombre) || (p[j].Nombre == H2.Nombre)|| (p[j].Nombre == P2.Nombre)|| (p[j].Nombre == P1.Nombre))
                            {
                                matrizauxjacob[contecuaciones1, j] = 1;

                            }
                        }
                        contecuaciones1++;
                    }

                }

                //Ecuación del Factor de Flujo
                if (D3 > 0)
                {
                    if (D8 > 0) //Corrección del Factor de Flujo mediante la relación de presiones Psalida/Pentrada
                    {

                        Func<Double> segundaecuacion = () => W1 - (FactorFlujocorrejido(P1, P2, acceso.xph(P1, H1), D3, D8) * (Math.Sqrt(P1 / (acceso.vph(P1, H1)))));
                        functions.Add(segundaecuacion);

                         for (int j = 0; j < p.Count; j++)
                        {
                            if ((p[j].Nombre == W1.Nombre) || (p[j].Nombre == P2.Nombre)|| (p[j].Nombre == P1.Nombre)|| (p[j].Nombre == H1.Nombre))
                            {
                                matrizauxjacob[contecuaciones1, j] = 1;

                            }
                        }
                        contecuaciones1++;
                    }
                    else //No corrección del Factor de Flujo
                    {
                        Func<Double> segundaecuacion = () => W1 - (D3 * (Math.Sqrt(P1 / (acceso.vph(P1, H1)))));
                        functions.Add(segundaecuacion);

                         for (int j = 0; j < p.Count; j++)
                        {
                            if ((p[j].Nombre == W1.Nombre) || (p[j].Nombre == P1.Nombre)||(p[j].Nombre == H1.Nombre))
                            {
                                matrizauxjacob[contecuaciones1, j] = 1;

                            }
                        }
                        contecuaciones1++;
                    }
                }

                Func<Double> cuartaecuacion = () => P2 - D2;
                functions.Add(cuartaecuacion);

                 for (int j = 0; j < p.Count; j++)
                 {
                            if ((p[j].Nombre == P2.Nombre))
                            {
                                matrizauxjacob[contecuaciones1, j] = 1;

                            }
                 }
                        contecuaciones1++;

                return contecuaciones1;
            }
//-------------------------------------------------------------------------------------------------------------------------------
            //Creamos las ecuaciones del Equipo TIPO 11: TURBINA AUXILIAR
            else if (TipodeEquipo == 11)
            {
                Parameter W1 = new Parameter();
                Parameter W2 = new Parameter();

                Parameter P1 = new Parameter();
                Parameter P2 = new Parameter();

                Parameter H1 = new Parameter();
                Parameter H2 = new Parameter();

                W1 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N1));
                W2 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N3));

                P1 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N1));
                P2 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N3));

                H1 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N1));
                H2 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N3));
                
                Func<Double> primeraecuacion = () => W1 - W2;
                functions.Add(primeraecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == W1.Nombre) || (p[j].Nombre == W2.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;
                    }
                }
                contecuaciones1++;


                if (D2 > 0)
                {
                    Func<Double> segundaecuacion = () => P2 - D2;
                    functions.Add(segundaecuacion);

                    for (int j = 0; j < p.Count; j++)
                    {
                        if (p[j].Nombre == P2.Nombre)
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;
                        }
                    }
                    contecuaciones1++;

                }

                if (D1 != 0)
                {
                    Func<Double> terceraecuacion = () => H2 - H1 + D1 * (H1 - acceso.hps(P2, (acceso.sph(P1, H1))));
                    functions.Add(terceraecuacion);

                    for (int j = 0; j < p.Count; j++)
                    {
                        if ((p[j].Nombre == H1.Nombre) || (p[j].Nombre == H2.Nombre) || (p[j].Nombre == P1.Nombre) || (p[j].Nombre == P2.Nombre))
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;
                        }
                    }
                    contecuaciones1++;

                }

                if (D5 != 0)
                {
                    Func<Double> cuartaecuacion = () => W1 * (H1 - H2) - D5;
                    functions.Add(cuartaecuacion);

                    for (int j = 0; j < p.Count; j++)
                    {
                        if ((p[j].Nombre == H1.Nombre) || (p[j].Nombre == H2.Nombre) || (p[j].Nombre == W1.Nombre))
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;
                        }
                    }
                    contecuaciones1++;
                }

                return contecuaciones1;
            }

//-------------------------------------------------------------------------------------------------------------------------------
            //Creamos las ecuaciones del Equipo Tipo 13: SEPARADOR DE HUMEDAD
            else if (TipodeEquipo == 13)
            {
                Parameter W1 = new Parameter();
                Parameter W2 = new Parameter();
                Parameter W3 = new Parameter();
                Parameter P1 = new Parameter();
                Parameter P2 = new Parameter();
                Parameter P3 = new Parameter();
                Parameter H1 = new Parameter();
                Parameter H2 = new Parameter();
                Parameter H3 = new Parameter();

                W1 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N1));
                W2 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N3));
                W3 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N4));
                P1 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N1));
                P2 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N3));
                P3 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N4));
                H1 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N1));
                H2 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N3));
                H3 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N4));

                Func<Double> primeraecuacion = () => W1 - W2 - W3;
                functions.Add(primeraecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == W1.Nombre) || (p[j].Nombre == W2.Nombre) || (p[j].Nombre == W3.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;
                    }
                }
                contecuaciones1++;
                
                Func<Double> segundaecuacion = () => W3 - (W1 * (D2 + (D1 * (1 - (acceso.xph(P1, H1) / 100)))));
                functions.Add(segundaecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == W3.Nombre) || (p[j].Nombre == W1.Nombre) || (p[j].Nombre == H1.Nombre) || (p[j].Nombre == P1.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;
                    }
                }
                contecuaciones1++;
                
                Func<Double> terceraecuacion = () => P2 - P1;
                functions.Add(terceraecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == P2.Nombre) || (p[j].Nombre == P1.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;
                    }
                }
                contecuaciones1++;
               
                Func<Double> cuartaecuacion = () => P3 - P1;
                functions.Add(cuartaecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == P3.Nombre) || (p[j].Nombre == P1.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;
                    }
                }
                contecuaciones1++;
               

                Func<Double> quintaecuacion = () => (W3 * H3) - ((1 - (acceso.xph(P1, H1) / 100)) * D1 * W1 * acceso.hsatpliq(P1)) - (D2 * W1 * H2);
                functions.Add(quintaecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == W3.Nombre) || (p[j].Nombre == H3.Nombre) || (p[j].Nombre == P1.Nombre) || (p[j].Nombre == H1.Nombre) || (p[j].Nombre == W1.Nombre) || (p[j].Nombre == H2.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;
                    }
                }
                contecuaciones1++;
               
                
                Func<Double> sextaecuacion = () => (W1 * H1) - (W2 * H2) - (W3 * H3);
                functions.Add(sextaecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == W1.Nombre) || (p[j].Nombre == H1.Nombre) || (p[j].Nombre == W2.Nombre) || (p[j].Nombre == H2.Nombre) || (p[j].Nombre == W3.Nombre) || (p[j].Nombre == H3.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;
                    }
                }
                contecuaciones1++;

                return contecuaciones1;
            }
//-------------------------------------------------------------------------------------------------------------------------------
            //Creamos las ecuaciones del Equipo TIPO 14: MSR
            else if (TipodeEquipo == 14)
            {
                Parameter W1 = new Parameter();
                Parameter W2 = new Parameter();
                Parameter W3 = new Parameter();
                Parameter W4 = new Parameter();

                Parameter P1 = new Parameter();
                Parameter P2 = new Parameter();
                Parameter P3 = new Parameter();
                Parameter P4 = new Parameter();

                Parameter H1 = new Parameter();
                Parameter H2 = new Parameter();
                Parameter H3 = new Parameter();
                Parameter H4 = new Parameter();


                W1 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N1));
                W2 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N2));
                W3 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N3));
                W4 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N4));

                P1 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N1));
                P2 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N2));
                P3 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N3));
                P4 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N4));

                H1 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N1));
                H2 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N2));
                H3 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N3));
                H4 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N4));

                Func<Double> primeraecuacion = () => W1 - W3;
                functions.Add(primeraecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == W1.Nombre) || (p[j].Nombre == W3.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;
                    }
                }
                contecuaciones1++;
                              
                Func<Double> segundaecuacion = () => W2 - W4;
                functions.Add(segundaecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == W2.Nombre) || (p[j].Nombre == W4.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;
                    }
                }
                contecuaciones1++;
                              
                Func<Double> terceraecuacion = () => P1 - P3 - D1 - (D2 * W1) - (D3 * W1 * W1) - (D4 * P1);
                functions.Add(terceraecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == P1.Nombre) || (p[j].Nombre == P3.Nombre) || (p[j].Nombre == W1.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;
                    }
                }
                contecuaciones1++;
                            
                Func<Double> cuartaecuacion = () => P2 - P4 - D7 - (D8 * W2 * W2) - (D9 * P2);
                functions.Add(cuartaecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == P2.Nombre) || (p[j].Nombre == P4.Nombre) || (p[j].Nombre == W2.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;
                    }
                }
                contecuaciones1++;
                          
                Func<Double> quintaecuacion = () => W1 * (H3 - H1) - W2 * (H2 - H4) * D6;
                functions.Add(quintaecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == W1.Nombre) || (p[j].Nombre == H3.Nombre) || (p[j].Nombre == H1.Nombre) || (p[j].Nombre == H2.Nombre) || (p[j].Nombre == H4.Nombre) || (p[j].Nombre == W2.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;
                    }
                }
                contecuaciones1++;
                            
                //PENDIENTE: definir una título X4 a la salida distinto de cero. Lo define HBAL con la 2ª y 3ª cifra decimal de D5.
                Func<Double> sextaecuacion = () => H4 - acceso.hpx(P4, 0);
                functions.Add(sextaecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == H4.Nombre) || (p[j].Nombre == P4.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;
                    }
                }
                contecuaciones1++;
                               
                //PENDIENTE: si D5>90 la parte entera indica el número de la tabla donde se define el TTD.
                Func<Double> septimaecuacion = () => H3 - acceso.hpt(P3, (acceso.tph(P2, H2) - D5));
                functions.Add(septimaecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == H3.Nombre) || (p[j].Nombre == P3.Nombre) || (p[j].Nombre == P2.Nombre) || (p[j].Nombre == H2.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;
                    }
                }
                contecuaciones1++;

                return contecuaciones1;
            }

//-------------------------------------------------------------------------------------------------------------------------------
//Creamos las ecuaciones del Equipo TIPO 15: CONDENSADOR DE VAPOR DE SELLOS, OFF-GAS Y EYECTORES
            else if (TipodeEquipo == 15)
            {
                Parameter W1 = new Parameter();
                Parameter W2 = new Parameter();
                Parameter W3 = new Parameter();
                Parameter W4 = new Parameter();
                Parameter P1 = new Parameter();
                Parameter P2 = new Parameter();
                Parameter P3 = new Parameter();
                Parameter P4 = new Parameter();
                Parameter H1 = new Parameter();
                Parameter H2 = new Parameter();
                Parameter H3 = new Parameter();
                Parameter H4 = new Parameter();

                W1 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N1));
                W2 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N2));
                W3 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N3));
                W4 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N4));
                P1 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N1));
                P2 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N2));
                P3 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N3));
                P4 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N4));
                H1 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N1));
                H2 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N2));
                H3 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N3));
                H4 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N4));

                Func<Double> primeraecuacion = () => W1 - W3;
                functions.Add(primeraecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == W1.Nombre) || (p[j].Nombre == W3.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;
                
                Func<Double> segundaecuacion = () => W2 - W4;
                functions.Add(segundaecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == W2.Nombre) || (p[j].Nombre == W4.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;

                if (D7 == 0)
                {
                    Func<Double> terceraecuacion = () => P2 - P4;
                    functions.Add(terceraecuacion);

                    for (int j = 0; j < p.Count; j++)
                    {
                        if ((p[j].Nombre == P2.Nombre) || (p[j].Nombre == P4.Nombre))
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;

                        }
                    }
                    contecuaciones1++;
                }
                else if (D7 > 0)
                {
                    Func<Double> terceraecuacion = () => P4 - D7;
                    functions.Add(terceraecuacion);

                    for (int j = 0; j < p.Count; j++)
                    {
                        if (p[j].Nombre == P4.Nombre)
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;

                        }
                    }
                    contecuaciones1++;
                }
                
                Func<Double> cuartaecuacion = () => P1 - P3 - D1 - (D2 * W1 / D8) - ((D3 * W1 * W1) / (D8 * D8));
                functions.Add(cuartaecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == P1.Nombre) || (p[j].Nombre == P3.Nombre) || (p[j].Nombre == W1.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;
               
                Func<Double> quintaecuacion = () => W1 * (H3 - H1) - W2 * (H2 - H4) * D6;
                functions.Add(quintaecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == W1.Nombre) || (p[j].Nombre == H3.Nombre) || (p[j].Nombre == H1.Nombre) || (p[j].Nombre == W2.Nombre) || (p[j].Nombre == H2.Nombre) || (p[j].Nombre == H4.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;
                               
                Func<Double> sextaecuacion = () => H4 - acceso.hpx(P4, D5);
                functions.Add(sextaecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == H4.Nombre) || (p[j].Nombre == P4.Nombre) )

                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;

                return contecuaciones1;
            }
//-------------------------------------------------------------------------------------------------------------------------------
            //Creamos las ecuaciones del Equipo TIPO 16: ENFRIADOR DE DRENAJES
            else if (TipodeEquipo == 16)
            {
                Parameter W1 = new Parameter();
                Parameter W2 = new Parameter();
                Parameter W3 = new Parameter();
                Parameter W4 = new Parameter();
               
                Parameter P1 = new Parameter();
                Parameter P2 = new Parameter();
                Parameter P3 = new Parameter();
                Parameter P4 = new Parameter();
                
                Parameter H1 = new Parameter();
                Parameter H2 = new Parameter();
                Parameter H3 = new Parameter();
                Parameter H4 = new Parameter();
               

                W1 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N1));
                W2 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N2));              
                W3 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N3));
                W4 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N4));

                P1 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N1));
                P2 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N2));               
                P3 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N3));
                P4 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N4));

                H1 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N1));
                H2 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N2));               
                H3 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N3));
                H4 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N4));

                //PRIMERA ECUACIÓN: Continuidad de Caudal Lado Tubos
                Func<Double> primeraecuacion = () => W1 - W3;
                functions.Add(primeraecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == W1.Nombre) || (p[j].Nombre == W3.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;

                //SEGUNDA ECUACIÓN: Continuidad de Caudal Lado Carcasa
                Func<Double> segundaecuacion = () => W2- W4;
                functions.Add(segundaecuacion);


                for (int j = 0; j < p.Count; j++)
                {
                       if ((p[j].Nombre == W2.Nombre) || (p[j].Nombre == W4.Nombre))
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;
                        }
                }

                contecuaciones1++;

                //TERCERA ECUACIÓN: Igualdad Presiones Lado Carcasa
                Func<Double> terceraecuacion = () => P4 - P2;
                functions.Add(terceraecuacion);


                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == P4.Nombre) || (p[j].Nombre == P2.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;

                //CUARTA ECUACIÓN: Pérdida de Carga Lado Tubos
                Func<Double> cuartaecuacion = () => P1 - P3 - (D1 + (D2 * W1) + (D3 * W1 * W1));
                functions.Add(cuartaecuacion);


                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == P1.Nombre) || (p[j].Nombre == P3.Nombre) || (p[j].Nombre == W1.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;

                //QUINTA ECUACIÓN:Balance Energético
                Func<Double> quintaecuacion = () => (W1 * (H3 - H1)) - (W2 * (H2 - H4) * D6);
                functions.Add(quintaecuacion);


                for (int j = 0; j < p.Count; j++)
                {
                       if ((p[j].Nombre == W1.Nombre) || (p[j].Nombre == H3.Nombre) || (p[j].Nombre == H1.Nombre) || (p[j].Nombre == W2.Nombre) || (p[j].Nombre == H2.Nombre) || (p[j].Nombre == H4.Nombre))
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;

                        }
                }
                contecuaciones1++;

                
                //SEXTA ECUACIÓN: Cálculo del DCA
                //Caso de CALENTADOR HÚMEDO (con DCA>0)
                if (D4 > 0)
                {
                    //Definición de DCA mediante un valor introducido por el usuario
                    if (D4 <= 500)
                    {
                        Func<Double> sextaecuacion = () => acceso.tph(P4, H4) - acceso.tph(P1, H1) - D4;
                        functions.Add(sextaecuacion);

                        for (int j = 0; j < p.Count; j++)
                        {
                            if ((p[j].Nombre == P4.Nombre) || (p[j].Nombre == H4.Nombre) || (p[j].Nombre == P1.Nombre) || (p[j].Nombre == H1.Nombre))
                            {
                                matrizauxjacob[contecuaciones1, j] = 1;

                            }
                        }
                        contecuaciones1++;
                    }

                    else if (D4 > 500)
                    {
                        //La definición del DCA mediante una TABLA hace necesario establecer tres casos dependiendo de la UNIDADES de la TABLA
                        //Sistema de Unidades Británico
                        if (unidades == 0)
                        {
                            //Definición de la Tabla: Caudal (Lb/Hr) & TTD(ºF)
                            //Transformamos las Lb/sg a Lb/Hr para entrar en la Tabla
                            Func<Double> septimaecuacion = () => acceso.tph(P4, H4) - acceso.tph(P1, H1) - tabla(D4, W1 * 3600, 2);
                            functions.Add(septimaecuacion);

                            for (int j = 0; j < p.Count; j++)
                            {
                                if ((p[j].Nombre == P4.Nombre) || (p[j].Nombre == H4.Nombre) || (p[j].Nombre == P1.Nombre) || (p[j].Nombre == H1.Nombre) || (p[j].Nombre == W1.Nombre))
                                {
                                    matrizauxjacob[contecuaciones1, j] = 1;

                                }
                            }
                            contecuaciones1++;

                        }
                        //Sistema de Unidades Métrico
                        else if (unidades == 1)
                        {
                            MessageBox.Show("Opción todavía no progamada. LUIS COCO");
                        }
                        else if (unidades == 2)
                        {
                            //Definición de la Tabla: Caudal (Kg/sg) & DCA(ºC)
                            //Tranformamos las Lb/sg en Kgr/sg para entrar en la Tabla. Y convertimos el DCA desde ºC a ºF.
                            //El DCA es un Incremento de Temperatura por lo que para transformarlo de ºC a ºF sólo hay que multiplicar por 9/5
                            Func<Double> septimaecuacion = () => acceso.tph(P4, H4) - acceso.tph(P1, H1) - (((tabla(D4, W1 * 0.4536, 3) * 9) / 5));
                            functions.Add(septimaecuacion);

                            for (int j = 0; j < p.Count; j++)
                            {
                                if ((p[j].Nombre == P4.Nombre) || (p[j].Nombre == H4.Nombre) || (p[j].Nombre == P1.Nombre) || (p[j].Nombre == H1.Nombre) || (p[j].Nombre == W1.Nombre))
                                {
                                    matrizauxjacob[contecuaciones1, j] = 1;
                                }
                            }
                            contecuaciones1++;
                        }
                    }
                }
            }
//--------------------------------------------------------------------------------------------------------------------------------
            //Creamos las ecuaciones del Equipo TIPO 17: ATEMPERADOR
            else if (TipodeEquipo == 17)
            {
                Parameter W1 = new Parameter();
                Parameter W2 = new Parameter();
                Parameter W3 = new Parameter();
                Parameter P1 = new Parameter();
                Parameter P2 = new Parameter();
                Parameter P3 = new Parameter();
                Parameter H1 = new Parameter();
                Parameter H2 = new Parameter();
                Parameter H3 = new Parameter();

                W1 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N1));
                W2 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N2));
                W3 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N3));
                P1 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N1));
                P2 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N2));
                P3 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N3));
                H1 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N1));
                H2 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N2));
                H3 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N3));

                Func<Double> primeraecuacion = () => W3 - W1 - W2;
                functions.Add(primeraecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == W1.Nombre) || (p[j].Nombre == W2.Nombre) || (p[j].Nombre == W3.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;

                Func<Double> segundaecuacion = () => P1-(P3-D1)-(D2*W1)-(D3*W1*W1)-(D4*P1);
                functions.Add(segundaecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == W1.Nombre)||(p[j].Nombre == P1.Nombre)||(p[j].Nombre == P3.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }

                contecuaciones1++;


                Func<Double> terceraecuacion = () => (W1*(H1-H3))-(W2*(H3-H2)*D6);
                functions.Add(terceraecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == W1.Nombre)||(p[j].Nombre == H1.Nombre)||(p[j].Nombre == H3.Nombre)||(p[j].Nombre == W2.Nombre)||(p[j].Nombre == H2.Nombre))
                    {
                       matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                
                contecuaciones1++;


                Func<Double> cuartaecuacion = () => acceso.tph(P3, H3) - acceso.tsl(P3) - D5;
                functions.Add(cuartaecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == P3.Nombre) || (p[j].Nombre == H3.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
               
                contecuaciones1++;
   

                return contecuaciones1;
            }
//-------------------------------------------------------------------------------------------------------------------------------
            //Creamos las ecuaciones del Equipo TIPO 18: DESAIREADOR
 else if (TipodeEquipo == 18)
            {
                Parameter W1 = new Parameter();
                Parameter W2 = new Parameter();
                Parameter W3 = new Parameter();
                Parameter W4 = new Parameter();
                Parameter W5 = new Parameter();

                Parameter P1 = new Parameter();
                Parameter P2 = new Parameter();
                Parameter P3 = new Parameter();
                Parameter P4 = new Parameter();
                Parameter P5 = new Parameter();

                Parameter H1 = new Parameter();
                Parameter H2 = new Parameter();
                Parameter H3 = new Parameter();
                Parameter H4 = new Parameter();
                Parameter H5 = new Parameter();

                W1 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N1));
                W2 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N2));
                W5 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(D9));
                W3 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N3));
                W4 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N4));
                
                P1 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N1));
                P2 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N2));
                P5 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(D9));
                P3 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N3));
                P4 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N4));
                
                H1 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N1));
                H2 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N2));
                H5 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(D9));
                H3 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N3));
                H4 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N4));

                //PRIMERA ECUACIÓN: Continuidad de Caudal Lado Tubos
                Func<Double> primeraecuacion = () => W3+W4-W1-W2-W5;
                functions.Add(primeraecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == W3.Nombre) || (p[j].Nombre == W4.Nombre) || (p[j].Nombre == W1.Nombre) || (p[j].Nombre == W2.Nombre) || (p[j].Nombre == W5.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;
				
		        //SEGUNDA ECUACIÓN:
                //Sistema Británico
                Func<Double> segundaecuacion = () => W4 - ((5e-6) * (P2 * W1));
                functions.Add(segundaecuacion);
            
                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == W4.Nombre) || (p[j].Nombre == P2.Nombre) || (p[j].Nombre == W1.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;
                
		        //TERCERA ECUACIÓN:
		        Func<Double> terceraecuacion = () => P4-P2;
		        functions.Add(terceraecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == P4.Nombre) || (p[j].Nombre == P2.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;
				
		        //CUARTA ECUACIÓN:
		        Func<Double> cuartaecuacion = () => P3-P2;
		        functions.Add(cuartaecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == P3.Nombre) || (p[j].Nombre == P2.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;
            
		        //QUINTA ECUACIÓN:
		        //Confirmar que el T´ítulo X=1 no es X=100
		        Func<Double> quintaecuacion = () => H4-acceso.hpx(P2,100);
		        functions.Add(quintaecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == H4.Nombre) || (p[j].Nombre == P2.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;
		
                //SEXTA ECUACIÓN:
		        //Si D5<500 el valor de TTD viene dado por D5 
		        if (D5<500)
		        {
                    Func<Double> sextaecuacion = () =>acceso.tph(P3,H3)-acceso.tsl(P2)+D5;
		            functions.Add(sextaecuacion);

                    for (int j = 0; j < p.Count; j++)
                    {
                        if ((p[j].Nombre == P3.Nombre) || (p[j].Nombre == P2.Nombre) || (p[j].Nombre == H3.Nombre))
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;

                        }
                    }
                    contecuaciones1++;
		        }

		        //Si D5>500 se accede a una TABLA con número D5
		        //Es necesario formular las tres opciones de Sistema de Unidades para entrar en las Tablas.
                else if (D5>500)
                {				
		            if (unidades ==0)
		            {				
		                Func<Double> sextaecuacion = () =>acceso.tph(P3,H3)-acceso.tsl(P2)+tabla(D5,W2,2);
		                functions.Add(sextaecuacion);

                        for (int j = 0; j < p.Count; j++)
                        {
                            if ((p[j].Nombre == P3.Nombre) || (p[j].Nombre == W2.Nombre) || (p[j].Nombre == H3.Nombre) || (p[j].Nombre == P2.Nombre))
                            {
                                matrizauxjacob[contecuaciones1, j] = 1;

                            }
                        }
                        contecuaciones1++;
                    }				
				   
		            else if (unidades==1)
		            {
                        Func<Double> sextaecuacion = () => acceso.tph(P3, H3) - acceso.tsl(P2) + tabla(D5, (W2*0.4536*3600)/1000, 2);
		                functions.Add(sextaecuacion);

                        for (int j = 0; j < p.Count; j++)
                        {
                            if ((p[j].Nombre == P3.Nombre) || (p[j].Nombre == W2.Nombre) || (p[j].Nombre == H3.Nombre) || (p[j].Nombre == P2.Nombre))
                            {
                                matrizauxjacob[contecuaciones1, j] = 1;

                            }
                        }
                        contecuaciones1++;
		            }
				
		            else if (unidades==2)
		            {
                        Func<Double> sextaecuacion = () => acceso.tph(P3, H3) - acceso.tsl(P2) + tabla(D5, W2*0.4536, 2);
		                functions.Add(sextaecuacion);

                        for (int j = 0; j < p.Count; j++)
                        {
                            if ((p[j].Nombre == P3.Nombre) || (p[j].Nombre == W2.Nombre) || (p[j].Nombre == H3.Nombre) || (p[j].Nombre == P2.Nombre))
                            {
                                matrizauxjacob[contecuaciones1, j] = 1;

                            }
                        }
                        contecuaciones1++;
		            }
		        }	
				
		        //SEPTIMA ECUACIÓN
        	    Func<Double> septimaecuacion = () =>((W1*H1+W2*H2)*D6)-(W4*H4)-(W3*H3)+((W5*H5)*D6);
		        functions.Add(septimaecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == W1.Nombre) || (p[j].Nombre == H1.Nombre) || (p[j].Nombre == W2.Nombre) || (p[j].Nombre == H2.Nombre) || (p[j].Nombre == W4.Nombre) || (p[j].Nombre == H4.Nombre) || (p[j].Nombre == W3.Nombre) || (p[j].Nombre == H3.Nombre) || (p[j].Nombre == W5.Nombre)||(p[j].Nombre == H5.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;
			   
		        //OCTAVA Y NOVENA ECUACIÓN
		        if ((D7>0)&&(D7<1))
		        {
        	       Func<Double> octavaecuacion = () => P2-P1;
		           functions.Add(octavaecuacion);

                   for (int j = 0; j < p.Count; j++)
                   {
                       if ((p[j].Nombre == P2.Nombre)||(p[j].Nombre == P1.Nombre))
                       {
                           matrizauxjacob[contecuaciones1, j] = 1;

                       }
                   }
                   contecuaciones1++;
	       	    }
				
		        else if ((D7>0)&&(D7>1))
     		    {
		           Func<Double> novenaecuacion = () => P2-P5;
		           functions.Add(novenaecuacion);

                   for (int j = 0; j < p.Count; j++)
                   {
                       if (p[j].Nombre == P2.Nombre)
                       {
                           matrizauxjacob[contecuaciones1, j] = 1;

                       }
                   }
                   contecuaciones1++;
		        }

                return contecuaciones1;
	    }
   
//-------------------------------------------------------------------------------------------------------------------------------
            //Creamos las ecuaciones del Equipo TIPO 19: VÁLVULA
            else if (TipodeEquipo == 19)
            {
                Parameter W1 = new Parameter();
                Parameter W2 = new Parameter();
                Parameter P1 = new Parameter();
                Parameter P2 = new Parameter();
                Parameter H1 = new Parameter();
                Parameter H2 = new Parameter();

                W1 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N1));
                W2 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N3));
                P1 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N1));
                P2 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N3));
                H1 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N1));
                H2 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N3));

                
                Func<Double> primeraecuacion = () => W1 - W2;
                functions.Add(primeraecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == W1.Nombre) || (p[j].Nombre == W2.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;

                if ((D1 == 0) && (D2 == 0) && (D3 == 0) && (D4 == 0))
                {
                    
                }

                else if ((D4 == 0) && ((D1 != 0) || (D2 != 0) || (D3 != 0)))
                {                    
                    Func<Double> segundaecuacion = () => W1 - D1 - (D2 * P2) - (D3 * P2 * P2);
                    functions.Add(segundaecuacion);

                    for (int j = 0; j < p.Count; j++)
                    {
                        if ((p[j].Nombre == W1.Nombre) || (p[j].Nombre == P2.Nombre))
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;

                        }
                    }
                    contecuaciones1++;
                }

                else if ((D4 != 0) && (D1 == 0) && (D2 == 0) && (D3 == 0))
                {
                    Func<Double> segundaecuacion = () => ((W1 * 448.8312 * acceso.vph(P1, H1)) / Math.Sqrt(P1 - P2)) - D4;
                    functions.Add(segundaecuacion);

                    for (int j = 0; j < p.Count; j++)
                    {
                        if ((p[j].Nombre == W1.Nombre) || (p[j].Nombre == P1.Nombre) || (p[j].Nombre == H1.Nombre) || (p[j].Nombre == P2.Nombre))
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;

                        }
                    }
                    contecuaciones1++;
                }

                if ((D7 == 0) && (D8 == 0) && (D9 == 0))
                {
                    Func<Double> terceraecuacion = () => H1 - H2;
                    functions.Add(terceraecuacion);

                    for (int j = 0; j < p.Count; j++)
                    {
                        if ((p[j].Nombre == H1.Nombre) || (p[j].Nombre == H2.Nombre))
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;

                        }
                    }
                    contecuaciones1++;
                }

                else if ((D7 != 0) || (D8 != 0) || (D9 != 0))
                {
                    Func<Double> terceraecuacion = () => W1 - D7 - (H2 * D8) - (D9 * H2 * H2);
                    functions.Add(terceraecuacion);

                    for (int j = 0; j < p.Count; j++)
                    {
                        if ((p[j].Nombre == W1.Nombre) || (p[j].Nombre == H2.Nombre))
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;

                        }
                    }
                    contecuaciones1++;
                }

                return contecuaciones1;
            }

//-------------------------------------------------------------------------------------------------------------------------------
            //Creamos las ecuaciones del Equipo TIPO 20: DIVISOR DE ENTALPÍA FIJA
            else if (TipodeEquipo == 20)
            {
                Parameter W1 = new Parameter();
                Parameter W2 = new Parameter();
                Parameter W3 = new Parameter();
                Parameter P1 = new Parameter();
                Parameter P2 = new Parameter();
                Parameter P3 = new Parameter();
                Parameter H1 = new Parameter();
                Parameter H2 = new Parameter();
                Parameter H3 = new Parameter();

                W1 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N1));
                W2 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N3));
                W3 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N4));
                P1 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N1));
                P2 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N3));
                P3 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N4));
                H1 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N1));
                H2 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N3));
                H3 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N4));

                Func<Double> primeraecuacion = () => W1 - W2 - W3;
                functions.Add(primeraecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == W1.Nombre) || (p[j].Nombre == W2.Nombre) || (p[j].Nombre == W3.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;
                                
                Func<Double> segundaecuacion = () => W3 - D1;
                functions.Add(segundaecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if (p[j].Nombre == W3.Nombre) 
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;
                
                Func<Double> terceraecuacion = () => P1 - P2;
                functions.Add(terceraecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == P1.Nombre) || (p[j].Nombre == P2.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;
                
                if (D3 == 0)
                {
                    Func<Double> cuartaecuacion = () => P1 - P3;
                    functions.Add(cuartaecuacion);

                    for (int j = 0; j < p.Count; j++)
                    {
                        if ((p[j].Nombre == P1.Nombre) || (p[j].Nombre == P3.Nombre))
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;

                        }
                    }
                    contecuaciones1++;
                }

                else if (D3 != 0)
                {                    
                    Func<Double> cuartaecuacion = () => P1 - D3;
                    functions.Add(cuartaecuacion);

                    for (int j = 0; j < p.Count; j++)
                    {
                        if ((p[j].Nombre == P1.Nombre))
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;

                        }
                    }
                    contecuaciones1++;
                }
                                
                Func<Double> quintaecuacion = () => H3 - D2;
                functions.Add(quintaecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == H3.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;
                                
                Func<Double> sextaecuacion = () => (W1 * H1) - (W2 * H2) - (W3 * H3);
                functions.Add(sextaecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == W1.Nombre) || (p[j].Nombre == H1.Nombre) || (p[j].Nombre == W2.Nombre) || (p[j].Nombre == H2.Nombre) || (p[j].Nombre == W3.Nombre) || (p[j].Nombre == H3.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;

                return contecuaciones1;
            }

//-------------------------------------------------------------------------------------------------------------------------------
            //Creamos las ecuaciones del Equipo TIPO 21: TANQUE DE VAPORIZACIÓN
            else if (TipodeEquipo == 21)
            {
                Parameter W1 = new Parameter();
                Parameter W2 = new Parameter();
                Parameter W3 = new Parameter();
                Parameter P1 = new Parameter();
                Parameter P2 = new Parameter();
                Parameter P3 = new Parameter();
                Parameter H1 = new Parameter();
                Parameter H2 = new Parameter();
                Parameter H3 = new Parameter();

                W1 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N1));
                W2 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N3));
                W3 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N4));
                P1 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N1));
                P2 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N3));
                P3 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N4));
                H1 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N1));
                H2 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N3));
                H3 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N4));
               
                Func<Double> primeraecuacion = () => W1 - W2 - W3;
                functions.Add(primeraecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == W1.Nombre)||(p[j].Nombre == W2.Nombre)||(p[j].Nombre == W3.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;
                                          
                Func<Double> segundaecuacion = () => P2 - D1;
                functions.Add(segundaecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if (p[j].Nombre == P2.Nombre)
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;
                               
                Func<Double> terceraecuacion = () => P3 - D1;
                functions.Add(terceraecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if (p[j].Nombre == P3.Nombre)
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;

                Func<Double> cuartaecuacion = () => H3 - acceso.hsatpliq(D1);
                functions.Add(cuartaecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if (p[j].Nombre == H3.Nombre)
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;

                Func<Double> quintaecuacion = () => H2 - acceso.hsatpvap(D1);
                functions.Add(quintaecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if (p[j].Nombre == H2.Nombre)
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;
                
                Func<Double> sextaecuacion = () => (W1 * H1) - (W2 * H2) - (W3 * H3);
                functions.Add(sextaecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == W1.Nombre)||(p[j].Nombre == H1.Nombre)||(p[j].Nombre == W2.Nombre)||(p[j].Nombre == H2.Nombre)||(p[j].Nombre == W3.Nombre)||(p[j].Nombre == H3.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;
            }

//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            //Creamos las ecuaciones del Equipo TIPO 22: INTERCAMBIADOR
            else if (TipodeEquipo == 22)
            {
                Parameter W1 = new Parameter();
                Parameter W2 = new Parameter();
                Parameter W3 = new Parameter();
                Parameter W4 = new Parameter();
               
                Parameter P1 = new Parameter();
                Parameter P2 = new Parameter();
                Parameter P3 = new Parameter();
                Parameter P4 = new Parameter();
              
                Parameter H1 = new Parameter();
                Parameter H2 = new Parameter();
                Parameter H3 = new Parameter();
                Parameter H4 = new Parameter();
               
                W1 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N1));
                W2 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N2));
                W3 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N3));
                W4 = p.Find(p1 => p1.Nombre == "W" + Convert.ToString(N4));

                P1 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N1));
                P2 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N2));
                P3 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N3));
                P4 = p.Find(p1 => p1.Nombre == "P" + Convert.ToString(N4));

                H1 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N1));
                H2 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N2));
                H3 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N3));
                H4 = p.Find(p1 => p1.Nombre == "H" + Convert.ToString(N4));

                //PRIMERA ECUACIÓN: Continuidad de Caudal Lado Tubos
                Func<Double> primeraecuacion = () => W1 - W3;
                functions.Add(primeraecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == W1.Nombre) || (p[j].Nombre == W3.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;

                //SEGUNDA ECUACIÓN: Continuidad de Caudal Lado Carcasa
                Func<Double> segundaecuacion = () => W2 - W4;
                functions.Add(segundaecuacion);


                for (int j = 0; j < p.Count; j++)
                {                    
                        if ((p[j].Nombre == W2.Nombre) || (p[j].Nombre == W4.Nombre))
                        {
                            matrizauxjacob[contecuaciones1, j] = 1;
                        }
                }

                contecuaciones1++;              

                //CUARTA ECUACIÓN: Pérdida de Carga Lado Tubos
                Func<Double> cuartaecuacion = () => P1 - P3 - (D1 + (D2 * W1) + (D3 * W1 * W1)-(D4*P1));
                functions.Add(cuartaecuacion);


                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == P1.Nombre) || (p[j].Nombre == P3.Nombre) || (p[j].Nombre == W1.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;


                //QUINTA ECUACIÓN: Pérdida de Carga Lado Tubos
                Func<Double> quintaecuacion = () => P2 - P4 - (D6 + (D7 * W1) + (D8 * W1 * W1) - (D9 * P1));
                functions.Add(quintaecuacion);


                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == P1.Nombre) || (p[j].Nombre == P2.Nombre) || (p[j].Nombre == P4.Nombre) || (p[j].Nombre == W1.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;

                //SEXTA ECUACIÓN:Balance Energético
                Func<Double> sextaecuacion = () => (W1 * (H3 - H1)) - (W2 * (H2 - H4) * D5);
                functions.Add(sextaecuacion);

                for (int j = 0; j < p.Count; j++)
                {
                    if ((p[j].Nombre == W1.Nombre) || (p[j].Nombre == H3.Nombre) || (p[j].Nombre == H1.Nombre) || (p[j].Nombre == W2.Nombre) || (p[j].Nombre == H2.Nombre) || (p[j].Nombre == H4.Nombre))
                    {
                        matrizauxjacob[contecuaciones1, j] = 1;

                    }
                }
                contecuaciones1++;                        
            }
            
            return contecuaciones1;
        }
        
        //Función para calcular el FACTOR DE FLUJO correjido en función de la relación Psalida/Pentrada
        public Double FactorFlujocorrejido(Double P1, Double P2, Double X, Double D3, Double D8)
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
            factorflujocorrejido1 = D3 * (1 - (Math.Abs(FR1 - FR2) / FR1));
            return factorflujocorrejido1;
        }

        //RENDIMIENTO TERMODINÁMICO correjido en función de la CALIDAD MEDIA
        public Double rendcorrejido(Double P1, Double H1, Double P2, Double H2, Double D1, Double D5)
        {
            Double X = (acceso.xph(P1, H1) + acceso.xph(P2, H2)) / 2;

            Double rendcorregido1 = D1 * (1 - ((D5 - (X / 100)) / D5));

            return rendcorregido1;
        }

       
    }
}

