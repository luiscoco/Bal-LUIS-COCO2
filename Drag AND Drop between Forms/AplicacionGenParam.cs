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
        Double numparametroscreados = 0;
        Double numparnuevos = 0;

        private void generarparametros(Double TipodeEquipo, Double N1, Double N2, Double N3, Double N4, Double D1, Double D2, Double D3, Double D4, Double D5, Double D6, Double D7, Double D8, Double D9,Double adicional1,Double adicional2, Double adicional3, Double adicional4)
        {

//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            //Generar parámeteros para Equipo Tipo 1: Condición de Contorno
            if (TipodeEquipo == 1)
            {
                //Comprobamos si el Equipo Condición de Contorno tiene conectado un equipo o no.
                //Si tiene conectado un equipo temporal1 y temporal2 será igual a 1. De los contrario temporal1 y temporal2 será igual a 0.
                Double temporal1 = 0;
                Double temporal2 = 0;

                //Detectamos si hay algún equipo conectado a este equipo
                for (int u = 0; u <equipos11.Count; u++)
                { 
                    if ((N1==equipos11[u].aN3)&&(N3!=equipos11[u].aN3)&&(N1!=0))
                    {
                       temporal1 = 1;
                    }

                    else if ((N2==equipos11[u].aN4)&&(N3!=equipos11[u].aN3)&&(N2!=0))
                    {
                       temporal2 = 1;
                    }                
                }

                //Actualizamos el número de parámetros creados
                if (p.Count == 0)
                {
                    numparametroscreados = 0;
                }

                else if (p.Count != 0)
                {
                    numparametroscreados = (numcorrientes) * 3;
                }

                //En este caso hay un equipo conectado a la Condición de Contorno y sólo tenemos que crear UNA Corriente Más
                if ((temporal1 == 1) || (temporal2 == 1)||(adicional1==1)||((D1==0)&&(D2==0)&&(D3==0)&&(D4==0)&&(D5==0)&&(D6==0)&&(D7==0)&&(D8==0)&&(D9==0)))
                {
                    numparnuevos = numparnuevos + 3;

                    //CREAMOS EL ARRAY DE PARAMETROS
                    for (int v = (int)numparametroscreados; v < ((numparametroscreados) + 3); v++)
                    {
                        //Creamos la lista de parámetros generadas por este programa
                        p.Add(ptemp);
                        p[v] = new Parameter(100, 0.01, "");
                    }

                    p[(int)numparametroscreados].Nombre = "W" + Convert.ToString(N3);                   
                    p[(int)numparametroscreados + 1].Nombre = "P" + Convert.ToString(N3);                    
                    p[(int)numparametroscreados + 2].Nombre = "H" + Convert.ToString(N3);
                    
                    //INCREMENTAMOS EL NÚMERO DE CORRIENTES
                    numcorrientes = numcorrientes + 1;
                }

                //En este caso hay un equipo conectado a la Condición de Contorno y sólo tenemos que crear DOS Corrientes más
                else if ((temporal1 != 1) && (temporal2 != 1)&&(adicional1!=1))
                {
                    numparnuevos = numparnuevos + 6;

                    //CREAMOS EL ARRAY DE PARAMETROS
                    for (int v = (int)numparametroscreados; v < ((numparametroscreados) + 6); v++)
                    {
                        //Creamos la lista de parámetros generadas por este programa
                        p.Add(ptemp);
                        p[v] = new Parameter(100, 0.01, "");
                    }

                    p[(int)numparametroscreados].Nombre = "W" + Convert.ToString(N1);
                    p[(int)numparametroscreados + 1].Nombre = "P" + Convert.ToString(N1);
                    p[(int)numparametroscreados + 2].Nombre = "H" + Convert.ToString(N1);

                    p[(int)numparametroscreados + 3].Nombre = "W" + Convert.ToString(N3);                          
                    p[(int)numparametroscreados + 4].Nombre = "P" + Convert.ToString(N3);               
                    p[(int)numparametroscreados + 5].Nombre = "H" + Convert.ToString(N3);
                    
                    //INCREMENTAMOS EL NÚMERO DE CORRIENTES
                    numcorrientes = numcorrientes + 2;
                }
            }
//---------------------------------------------------------------------------------------------------------------------------
            //Generar parámeteros para Equipo Tipo 2: Divisor
            else if (TipodeEquipo == 2)
            {
                numparametroscreados = (numcorrientes) * 3;

                numparnuevos = numparnuevos + 6;

                //CREAMOS EL ARRAY DE PARAMETROS
                for (int v = (int)numparametroscreados; v < ((numparametroscreados) + 6); v++)
                {
                    //Creamos la lista de parámetros generadas por este programa
                    p.Add(ptemp);
                    p[v] = new Parameter(100, 0.01, "");
                }

                p[(int)numparametroscreados].Nombre = "W" + Convert.ToString(N3);
                p[(int)numparametroscreados + 1].Nombre = "P" + Convert.ToString(N3);
                p[(int)numparametroscreados + 2].Nombre = "H" + Convert.ToString(N3);

                p[(int)numparametroscreados + 3].Nombre = "W" + Convert.ToString(N4);
                p[(int)numparametroscreados + 4].Nombre = "P" + Convert.ToString(N4);
                p[(int)numparametroscreados + 5].Nombre = "H" + Convert.ToString(N4);

                //INCREMENTAMOS EL NÚMERO DE CORRIENTES
                numcorrientes = numcorrientes + 2;
            }
//--------------------------------------------------------------------------------------------------------------------------
            //Generar parámeteros para Equipo Tipo 3: Pérdida de Carga
            else if (TipodeEquipo == 3)
            {
                numparametroscreados = (numcorrientes) * 3;

                numparnuevos = numparnuevos + 3;

                //CREAMOS EL ARRAY DE PARAMETROS
                for (int v = (int)numparametroscreados; v < ((numparametroscreados) + 3); v++)
                {
                    //Creamos la lista de parámetros generadas por este programa
                    p.Add(ptemp);
                    p[v] = new Parameter(100, 0.01, "");
                }

                p[(int)numparametroscreados].Nombre = "W" + Convert.ToString(N3);
                p[(int)numparametroscreados + 1].Nombre = "P" + Convert.ToString(N3);
                p[(int)numparametroscreados + 2].Nombre = "H" + Convert.ToString(N3);

                //INCREMENTAMOS EL NÚMERO DE CORRIENTES
                numcorrientes = numcorrientes + 1;
            }
//--------------------------------------------------------------------------------------------------------------------------
            //Generar parámeteros para Equipo Tipo 4: Bomba
            else if (TipodeEquipo == 4)
            {
                numparametroscreados = (numcorrientes) * 3;

                numparnuevos = numparnuevos + 3;

                //CREAMOS EL ARRAY DE PARAMETROS
                for (int v = (int)numparametroscreados; v < ((numparametroscreados) + 3); v++)
                {
                    //Creamos la lista de parámetros generadas por este programa
                    p.Add(ptemp);
                    p[v] = new Parameter(100, 0.01, "");
                }

                p[(int)numparametroscreados].Nombre = "W" + Convert.ToString(N3);
                p[(int)numparametroscreados + 1].Nombre = "P" + Convert.ToString(N3);
                p[(int)numparametroscreados + 2].Nombre = "H" + Convert.ToString(N3);

                //INCREMENTAMOS EL NÚMERO DE CORRIENTES
                numcorrientes = numcorrientes + 1;
            }
//--------------------------------------------------------------------------------------------------------------------------
            //Generar parámeteros para Equipo Tipo 5: Mezclador
            else if (TipodeEquipo == 5)
            {
                numparametroscreados = (numcorrientes) * 3;

                numparnuevos = numparnuevos + 3;

                //CREAMOS EL ARRAY DE PARAMETROS
                for (int v = (int)numparametroscreados; v < ((numparametroscreados) + 3); v++)
                {
                    //Creamos la lista de parámetros generadas por este programa
                    p.Add(ptemp);
                    p[v] = new Parameter(100, 0.01, "");
                }

                p[(int)numparametroscreados].Nombre = "W" + Convert.ToString(N3);
                p[(int)numparametroscreados + 1].Nombre = "P" + Convert.ToString(N3);
                p[(int)numparametroscreados + 2].Nombre = "H" + Convert.ToString(N3);

                //INCREMENTAMOS EL NÚMERO DE CORRIENTES
                numcorrientes = numcorrientes + 1;
            }
//--------------------------------------------------------------------------------------------------------------------------
            //Generar parámeteros para Equipo Tipo 6: Reactor
            else if (TipodeEquipo == 6)
            {
                numparametroscreados = (numcorrientes) * 3;

                numparnuevos = numparnuevos + 3;

                //CREAMOS EL ARRAY DE PARAMETROS
                for (int v = (int)numparametroscreados; v < ((numparametroscreados) + 3); v++)
                {
                    //Creamos la lista de parámetros generadas por este programa
                    p.Add(ptemp);
                    p[v] = new Parameter(100, 0.01, "");
                }

                p[(int)numparametroscreados].Nombre = "W" + Convert.ToString(N3);
                p[(int)numparametroscreados + 1].Nombre = "P" + Convert.ToString(N3);
                p[(int)numparametroscreados + 2].Nombre = "H" + Convert.ToString(N3);

                //INCREMENTAMOS EL NÚMERO DE CORRIENTES
                numcorrientes = numcorrientes + 1;
            }
//---------------------------------------------------------------------------------------------------------------------------
            //Generar parámeteros para Equipo Tipo 7: Calentador
            else if (TipodeEquipo == 7)
            {
                numparametroscreados = (numcorrientes) * 3;

                numparnuevos = numparnuevos + 6;

                //CREAMOS EL ARRAY DE PARAMETROS
                for (int v = (int)numparametroscreados; v < ((numparametroscreados) + 6); v++)
                {
                    //Creamos la lista de parámetros generadas por este programa
                    p.Add(ptemp);
                    p[v] = new Parameter(10, 0.01, "");
                }

                p[(int)numparametroscreados].Nombre = "W" + Convert.ToString(N3);
                p[(int)numparametroscreados + 1].Nombre = "P" + Convert.ToString(N3);
                p[(int)numparametroscreados + 2].Nombre = "H" + Convert.ToString(N3);

                p[(int)numparametroscreados + 3].Nombre = "W" + Convert.ToString(N4);
                p[(int)numparametroscreados + 4].Nombre = "P" + Convert.ToString(N4);
                p[(int)numparametroscreados + 5].Nombre = "H" + Convert.ToString(N4);

                //INCREMENTAMOS EL NÚMERO DE CORRIENTES
                numcorrientes = numcorrientes + 2;
            }
//---------------------------------------------------------------------------------------------------------------------------
            //Generar parámeteros para Equipo Tipo 8: Condensador
            else if (TipodeEquipo == 8)
            {
                numparametroscreados = (numcorrientes) * 3;

                numparnuevos = numparnuevos + 3;

                //CREAMOS EL ARRAY DE PARAMETROS
                for (int v = (int)numparametroscreados; v < ((numparametroscreados) + 3); v++)
                {
                    //Creamos la lista de parámetros generadas por este programa
                    p.Add(ptemp);
                    p[v] = new Parameter(100, 0.01, "");
                }

                p[(int)numparametroscreados].Nombre = "W" + Convert.ToString(N3);
                p[(int)numparametroscreados + 1].Nombre = "P" + Convert.ToString(N3);
                p[(int)numparametroscreados + 2].Nombre = "H" + Convert.ToString(N3);

                //INCREMENTAMOS EL NÚMERO DE CORRIENTES
                numcorrientes = numcorrientes + 1;
            }

//---------------------------------------------------------------------------------------------------------------------------
            //Generar parámeteros para Equipo Tipo 9: Turbina con Pérdidas en el Escape
            else if (TipodeEquipo == 9)
            {
                numparametroscreados = (numcorrientes) * 3;

                numparnuevos = numparnuevos + 3;

                //CREAMOS EL ARRAY DE PARAMETROS
                for (int v = (int)numparametroscreados; v < ((numparametroscreados) + 3); v++)
                {
                    //Creamos la lista de parámetros generadas por este programa
                    p.Add(ptemp);
                    p[v] = new Parameter(100, 0.01, "");
                }

                p[(int)numparametroscreados].Nombre = "W" + Convert.ToString(N3);
                p[(int)numparametroscreados + 1].Nombre = "P" + Convert.ToString(N3);
                p[(int)numparametroscreados + 2].Nombre = "H" + Convert.ToString(N3);

                //INCREMENTAMOS EL NÚMERO DE CORRIENTES
                numcorrientes = numcorrientes + 1;
            }

//-------------------------------------------------------------------------------------------------------------------------------------
            //Generar parámeteros para Equipo Tipo 10: Turbina Sin Pérdidas en el Escape
            else if (TipodeEquipo == 10)
            {
                numparametroscreados = (numcorrientes) * 3;

                numparnuevos = numparnuevos + 3;

                //CREAMOS EL ARRAY DE PARAMETROS
                for (int v = (int)numparametroscreados; v < ((numparametroscreados) + 3); v++)
                {
                    //Creamos la lista de parámetros generadas por este programa
                    p.Add(ptemp);
                    p[v] = new Parameter(100, 0.01, "");
                }

                p[(int)numparametroscreados].Nombre = "W" + Convert.ToString(N3);
                p[(int)numparametroscreados + 1].Nombre = "P" + Convert.ToString(N3);
                p[(int)numparametroscreados + 2].Nombre = "H" + Convert.ToString(N3);

                //INCREMENTAMOS EL NÚMERO DE CORRIENTES
                numcorrientes = numcorrientes + 1;
            }
//---------------------------------------------------------------------------------------------------------------------------
            //Generar parámeteros para Equipo Tipo 11: Turbina Auxiliar
            else if (TipodeEquipo == 11)
            {
                numparametroscreados = (numcorrientes) * 3;

                numparnuevos = numparnuevos + 3;

                //CREAMOS EL ARRAY DE PARAMETROS
                for (int v = (int)numparametroscreados; v < ((numparametroscreados) + 3); v++)
                {
                    //Creamos la lista de parámetros generadas por este programa
                    p.Add(ptemp);
                    p[v] = new Parameter(100, 0.01, "");
                }

                p[(int)numparametroscreados].Nombre = "W" + Convert.ToString(N3);
                p[(int)numparametroscreados + 1].Nombre = "P" + Convert.ToString(N3);
                p[(int)numparametroscreados + 2].Nombre = "H" + Convert.ToString(N3);

                //INCREMENTAMOS EL NÚMERO DE CORRIENTES
                numcorrientes = numcorrientes + 1;
            }
//---------------------------------------------------------------------------------------------------------------------------
            //Generar parámeteros para Equipo Tipo 13: Separador Humedad
            else if (TipodeEquipo == 13)
            {
                numparametroscreados = (numcorrientes) * 3;

                numparnuevos = numparnuevos + 6;

                //CREAMOS EL ARRAY DE PARAMETROS
                for (int v = (int)numparametroscreados; v < ((numparametroscreados) + 6); v++)
                {
                    //Creamos la lista de parámetros generadas por este programa
                    p.Add(ptemp);
                    p[v] = new Parameter(100, 0.01, "");
                }

                p[(int)numparametroscreados].Nombre = "W" + Convert.ToString(N3);
                p[(int)numparametroscreados + 1].Nombre = "P" + Convert.ToString(N3);
                p[(int)numparametroscreados + 2].Nombre = "H" + Convert.ToString(N3);

                p[(int)numparametroscreados + 3].Nombre = "W" + Convert.ToString(N4);
                p[(int)numparametroscreados + 4].Nombre = "P" + Convert.ToString(N4);
                p[(int)numparametroscreados + 5].Nombre = "H" + Convert.ToString(N4);

                //INCREMENTAMOS EL NÚMERO DE CORRIENTES
                numcorrientes = numcorrientes + 2;
            }
//---------------------------------------------------------------------------------------------------------------------------
            //Generar parámeteros para Equipo Tipo 14: MSR
            else if (TipodeEquipo == 14)
            {
                numparametroscreados = (numcorrientes) * 3;

                numparnuevos = numparnuevos + 6;

                //CREAMOS EL ARRAY DE PARAMETROS
                for (int v = (int)numparametroscreados; v < ((numparametroscreados) + 6); v++)
                {
                    //Creamos la lista de parámetros generadas por este programa
                    p.Add(ptemp);
                    p[v] = new Parameter(10, 0.01, "");
                }

                p[(int)numparametroscreados].Nombre = "W" + Convert.ToString(N3);
                p[(int)numparametroscreados + 1].Nombre = "P" + Convert.ToString(N3);
                p[(int)numparametroscreados + 2].Nombre = "H" + Convert.ToString(N3);

                p[(int)numparametroscreados + 3].Nombre = "W" + Convert.ToString(N4);
                p[(int)numparametroscreados + 4].Nombre = "P" + Convert.ToString(N4);
                p[(int)numparametroscreados + 5].Nombre = "H" + Convert.ToString(N4);

                //INCREMENTAMOS EL NÚMERO DE CORRIENTES
                numcorrientes = numcorrientes + 2;
            }
//---------------------------------------------------------------------------------------------------------------------------
            //Generar parámeteros para Equipo Tipo 15: Condensador de Eyectores, de Off-Gas y de Vapor de Sellos
            else if (TipodeEquipo == 15)
            {
                numparametroscreados = (numcorrientes) * 3;

                numparnuevos = numparnuevos + 6;

                //CREAMOS EL ARRAY DE PARAMETROS
                for (int v = (int)numparametroscreados; v < ((numparametroscreados) + 6); v++)
                {
                    //Creamos la lista de parámetros generadas por este programa
                    p.Add(ptemp);
                    p[v] = new Parameter(10, 0.01, "");
                }

                p[(int)numparametroscreados].Nombre = "W" + Convert.ToString(N3);
                p[(int)numparametroscreados + 1].Nombre = "P" + Convert.ToString(N3);
                p[(int)numparametroscreados + 2].Nombre = "H" + Convert.ToString(N3);

                p[(int)numparametroscreados + 3].Nombre = "W" + Convert.ToString(N4);
                p[(int)numparametroscreados + 4].Nombre = "P" + Convert.ToString(N4);
                p[(int)numparametroscreados + 5].Nombre = "H" + Convert.ToString(N4);

                //INCREMENTAMOS EL NÚMERO DE CORRIENTES
                numcorrientes = numcorrientes + 2;
            }
//---------------------------------------------------------------------------------------------------------------------------
            //Generar parámeteros para Equipo Tipo 16: Enfriador de Drenajes
            else if (TipodeEquipo == 16)
            {
                numparametroscreados = (numcorrientes) * 3;

                numparnuevos = numparnuevos + 6;

                //CREAMOS EL ARRAY DE PARAMETROS
                for (int v = (int)numparametroscreados; v < ((numparametroscreados) + 6); v++)
                {
                    //Creamos la lista de parámetros generadas por este programa
                    p.Add(ptemp);
                    p[v] = new Parameter(10, 0.01, "");
                }

                p[(int)numparametroscreados].Nombre = "W" + Convert.ToString(N3);
                p[(int)numparametroscreados + 1].Nombre = "P" + Convert.ToString(N3);
                p[(int)numparametroscreados + 2].Nombre = "H" + Convert.ToString(N3);

                p[(int)numparametroscreados + 3].Nombre = "W" + Convert.ToString(N4);
                p[(int)numparametroscreados + 4].Nombre = "P" + Convert.ToString(N4);
                p[(int)numparametroscreados + 5].Nombre = "H" + Convert.ToString(N4);

                //INCREMENTAMOS EL NÚMERO DE CORRIENTES
                numcorrientes = numcorrientes + 2;
            }

//--------------------------------------------------------------------------------------------------------------------------
            //Generar parámeteros para Equipo Tipo 17: Atemperador-Desuperheater
            else if (TipodeEquipo == 17)
            {
                numparametroscreados = (numcorrientes) * 3;

                numparnuevos = numparnuevos + 3;

                //CREAMOS EL ARRAY DE PARAMETROS
                for (int v = (int)numparametroscreados; v < ((numparametroscreados) + 3); v++)
                {
                    //Creamos la lista de parámetros generadas por este programa
                    p.Add(ptemp);
                    p[v] = new Parameter(100, 0.01, "");
                }

                p[(int)numparametroscreados].Nombre = "W" + Convert.ToString(N3);
                p[(int)numparametroscreados + 1].Nombre = "P" + Convert.ToString(N3);
                p[(int)numparametroscreados + 2].Nombre = "H" + Convert.ToString(N3);

                //INCREMENTAMOS EL NÚMERO DE CORRIENTES
                numcorrientes = numcorrientes + 1;
            }
//---------------------------------------------------------------------------------------------------------------------------
            //Generar parámeteros para Equipo Tipo 18: Desaireador
            else if (TipodeEquipo == 18)
            {
                numparametroscreados = (numcorrientes) * 3;

                numparnuevos = numparnuevos + 6;

                //CREAMOS EL ARRAY DE PARAMETROS
                for (int v = (int)numparametroscreados; v < ((numparametroscreados) + 6); v++)
                {
                    //Creamos la lista de parámetros generadas por este programa
                    p.Add(ptemp);
                    p[v] = new Parameter(10, 0.01, "");
                }

                p[(int)numparametroscreados].Nombre = "W" + Convert.ToString(N3);
                p[(int)numparametroscreados + 1].Nombre = "P" + Convert.ToString(N3);
                p[(int)numparametroscreados + 2].Nombre = "H" + Convert.ToString(N3);

                p[(int)numparametroscreados + 3].Nombre = "W" + Convert.ToString(N4);
                p[(int)numparametroscreados + 4].Nombre = "P" + Convert.ToString(N4);
                p[(int)numparametroscreados + 5].Nombre = "H" + Convert.ToString(N4);

                //INCREMENTAMOS EL NÚMERO DE CORRIENTES
                numcorrientes = numcorrientes + 2;
            }

//---------------------------------------------------------------------------------------------------------------------------
            //Generar parámeteros para Equipo Tipo 19: Válvula
            else if (TipodeEquipo == 19)
            {
                numparametroscreados = (numcorrientes) * 3;

                numparnuevos = numparnuevos + 3;

                //CREAMOS EL ARRAY DE PARAMETROS
                for (int v = (int)numparametroscreados; v < ((numparametroscreados) + 3); v++)
                {
                    //Creamos la lista de parámetros generadas por este programa
                    p.Add(ptemp);
                    p[v] = new Parameter(100, 0.01, "");
                }

                p[(int)numparametroscreados].Nombre = "W" + Convert.ToString(N3);
                p[(int)numparametroscreados + 1].Nombre = "P" + Convert.ToString(N3);
                p[(int)numparametroscreados + 2].Nombre = "H" + Convert.ToString(N3);

                //INCREMENTAMOS EL NÚMERO DE CORRIENTES
                numcorrientes = numcorrientes + 1;
            }
//---------------------------------------------------------------------------------------------------------------------------
            //Generar parámeteros para Equipo Tipo 20:Divisor de Entalpía Fija
            else if (TipodeEquipo == 20)
            {
                numparametroscreados = (numcorrientes) * 3;

                numparnuevos = numparnuevos + 6;

                //CREAMOS EL ARRAY DE PARAMETROS
                for (int v = (int)numparametroscreados; v < ((numparametroscreados) + 6); v++)
                {
                    //Creamos la lista de parámetros generadas por este programa
                    p.Add(ptemp);
                    p[v] = new Parameter(10, 0.01, "");
                }

                p[(int)numparametroscreados].Nombre = "W" + Convert.ToString(N3);
                p[(int)numparametroscreados + 1].Nombre = "P" + Convert.ToString(N3);
                p[(int)numparametroscreados + 2].Nombre = "H" + Convert.ToString(N3);

                p[(int)numparametroscreados + 3].Nombre = "W" + Convert.ToString(N4);
                p[(int)numparametroscreados + 4].Nombre = "P" + Convert.ToString(N4);
                p[(int)numparametroscreados + 5].Nombre = "H" + Convert.ToString(N4);

                //INCREMENTAMOS EL NÚMERO DE CORRIENTES
                numcorrientes = numcorrientes + 2;
            }
//---------------------------------------------------------------------------------------------------------------------------
            //Generar parámeteros para Equipo Tipo 21: Tanque de Vaporización
            else if (TipodeEquipo == 21)
            {
                numparametroscreados = (numcorrientes) * 3;

                numparnuevos = numparnuevos + 6;

                //CREAMOS EL ARRAY DE PARAMETROS
                for (int v = (int)numparametroscreados; v < ((numparametroscreados) + 6); v++)
                {
                    //Creamos la lista de parámetros generadas por este programa
                    p.Add(ptemp);
                    p[v] = new Parameter(10, 0.01, "");
                }

                p[(int)numparametroscreados].Nombre = "W" + Convert.ToString(N3);
                p[(int)numparametroscreados + 1].Nombre = "P" + Convert.ToString(N3);
                p[(int)numparametroscreados + 2].Nombre = "H" + Convert.ToString(N3);

                p[(int)numparametroscreados + 3].Nombre = "W" + Convert.ToString(N4);
                p[(int)numparametroscreados + 4].Nombre = "P" + Convert.ToString(N4);
                p[(int)numparametroscreados + 5].Nombre = "H" + Convert.ToString(N4);

                //INCREMENTAMOS EL NÚMERO DE CORRIENTES
                numcorrientes = numcorrientes + 2;
            }
//---------------------------------------------------------------------------------------------------------------------------
            //Generar parámeteros para Equipo Tipo 22: Intercambiador
            else if (TipodeEquipo ==22)
            {
                numparametroscreados = (numcorrientes) * 3;

                numparnuevos = numparnuevos + 6;

                //CREAMOS EL ARRAY DE PARAMETROS
                for (int v = (int)numparametroscreados; v < ((numparametroscreados) + 6); v++)
                {
                    //Creamos la lista de parámetros generadas por este programa
                    p.Add(ptemp);
                    p[v] = new Parameter(10, 0.01, "");
                }

                p[(int)numparametroscreados].Nombre = "W" + Convert.ToString(N3);
                p[(int)numparametroscreados + 1].Nombre = "P" + Convert.ToString(N3);
                p[(int)numparametroscreados + 2].Nombre = "H" + Convert.ToString(N3);

                p[(int)numparametroscreados + 3].Nombre = "W" + Convert.ToString(N4);
                p[(int)numparametroscreados + 4].Nombre = "P" + Convert.ToString(N4);
                p[(int)numparametroscreados + 5].Nombre = "H" + Convert.ToString(N4);

                //INCREMENTAMOS EL NÚMERO DE CORRIENTES
                numcorrientes = numcorrientes + 2;
            }

        }


    }
}

