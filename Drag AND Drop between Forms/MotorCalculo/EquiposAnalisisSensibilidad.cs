using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NumericalMethods;

using System.ComponentModel;


namespace ClaseEquiposSensibilidad
{
   public class EquiposSensibilidad
   {
        public Double numequipo2;

        //Parámetros 
        public Boolean aD1;
        public Double fromD1;
        public Double toD1;
        public Double incrementD1;

      
        public Boolean aD2;
        public Double fromD2;
        public Double toD2;
        public Double incrementD2;

       
        public Boolean aD3;
        public Double fromD3;
        public Double toD3;
        public Double incrementD3;

     
        public Boolean aD4;
        public Double fromD4;
        public Double toD4;
        public Double incrementD4;

      
        public Boolean aD5;
        public Double fromD5;
        public Double toD5;
        public Double incrementD5;

     
        public Boolean aD6;
        public Double fromD6;
        public Double toD6;
        public Double incrementD6;

    
        public Boolean aD7;
        public Double fromD7;
        public Double toD7;
        public Double incrementD7;

     
        public Boolean aD8;
        public Double fromD8;
        public Double toD8;
        public Double incrementD8;

     
        public Boolean aD9;
        public Double fromD9;
        public Double toD9;
        public Double incrementD9;

       
       public void Equipos1()
       {                
       
       }

       public void Inicializar(Double numequipo1,Boolean bD1, Boolean bD2, Boolean bD3, Boolean bD4, Boolean bD5, Boolean bD6, Boolean bD7, Boolean bD8, Boolean bD9)
       {
            numequipo2 = numequipo1;

            //Parámetros 
            aD1 = bD1;
            aD2 = bD2;
            aD3 = bD3;
            aD4 = bD4;
            aD5 = bD5;
            aD6 = bD6;
            aD7 = bD7;
            aD8 = bD8;
            aD9 = bD9;  
       }

      public void Destructor()
      {
       
      }

      public void GenerarParametros()
      { 
      
      }

      public void GenerarEcuaciones()
      { 
      
      }

    }
}
