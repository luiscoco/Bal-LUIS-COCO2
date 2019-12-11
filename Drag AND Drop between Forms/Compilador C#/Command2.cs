using System;
using Drag_AND_Drop_between_Forms;


namespace CSharpScripter2
{  
        //Interface utilizado para Sistema de Ecuaciones Diferenciales
        public interface Command2
        {
        	void Execute2(Aplicacion puntero3);
            double[] ODEs(double t, double[] y);          
        }            
}
