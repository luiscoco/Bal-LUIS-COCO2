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
        private void generarcondicionesiniciales(Double TipodeEquipo, Double N1, Double N2, Double N3, Double N4, Double D1, Double D2, Double D3, Double D4, Double D5, Double D6, Double D7, Double D8, Double D9, Double adicional1, Double adicional2, Double adicional3, Double adicional4)
        {
            //Equipo Tipo 1 CONDICION DE CONTORNO
            if (TipodeEquipo == 1)
            { 
            
            
            }

            //Equipo Tipo 2 DIVISOR
            else if (TipodeEquipo==2)
            {
            
            
            }


        }
    }

}

