using System;
using Drag_AND_Drop_between_Forms;


namespace CSharpScripter
{  
        //Interface utilizado para Ecuaciones de Interpolación
        public interface Command
        {
        	Double Execute(Double a, Double b, Aplicacion puntero3);       
        }            
}
