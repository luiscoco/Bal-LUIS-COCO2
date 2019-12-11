using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NumericalMethods;

using System.ComponentModel;


namespace ClaseEquipos
{
   public class Equipos
   {
        public Double numequipo2;

        public Double tipoequipo2;

        //Nº de Corrientes
        public Double aN1;
        public Double aN2;
        public Double aN3;
        public Double aN4;

        //Parámetros 
        public Double aD1;
        public string aD1description;
        public Double aD2;
        public string aD2description;
        public Double aD3;
        public string aD3description;
        public Double aD4;
        public string aD4description;
        public Double aD5;
        public string aD5description;
        public Double aD6;
        public string aD6description;
        public Double aD7;
        public string aD7description;
        public Double aD8;
        public string aD8description;
        public Double aD9;
        public string aD9description;

       //Adicionales
        public Double adicional11;
        public Double adicional12;
        public Double adicional13;
        public Double adicional14;

        //Resultados: Caudal W, Presión P y Entalpía H, de cada una de las corrientes del equipo
        public Double WN1=0;
        public Double PN1=0;
        public Double HN1=0;
        public Double WN2=0;
        public Double PN2=0;
        public Double HN2=0;
        public Double WN3=0;
        public Double PN3=0;
        public Double HN3=0;
        public Double WN4=0;
        public Double PN4=0;
        public Double HN4 = 0;

        //Número de Equipo
        [CategoryAttribute("Equipment Main Data: "),DescriptionAttribute("Equipment Number.")]
        [DisplayName()]

        public Double Number
        {
            get
            {
                return numequipo2;
            }

            set
            {
                numequipo2 = value;
            }
        }

        //Tipo de Equipo
        [CategoryAttribute("Equipment Main Data:"), DescriptionAttribute("Equipment Type.")]
        [DisplayName("Equipment Type: ")]

        public Double Type
        {
            get
            {
                return tipoequipo2;
            }

            set
            {
                tipoequipo2 = value;
            }
        }

        //Corriente de Entrada N1
        [CategoryAttribute("Equipment Streams:"), DescriptionAttribute("Input Stream N1.")]
        [DisplayName("Input Stream N1: ")]

        public Double N1
        {
            get
            {
                return aN1;
            }

            set
            {
                aN1 = value;
            }
        }

        //Corriente de Entrada N2
        [CategoryAttribute("Equipment Streams:"), DescriptionAttribute("Input Stream N2.")]
        [DisplayName("Input Stream N2: ")]

        public Double N2
        {
            get
            {
                return aN2;
            }

            set
            {
                aN2 = value;
            }
        }

        //Corriente de Salida N3
        [CategoryAttribute("Equipment Streams:"), DescriptionAttribute("Output Stream N3.")]
        [DisplayName("Output Stream N3: ")]

        public Double N3
        {
            get
            {
                return aN3;
            }

            set
            {
                aN3 = value;
            }
        }

        //Corriente de Salida N4
        [CategoryAttribute("Equipment Streams:"), DescriptionAttribute("Output Stream N4.")]
        [DisplayName("Output Stream N4: ")]

        public Double N4
        {
            get
            {
                return aN4;
            }

            set
            {
                aN4 = value;
            }
        }

        //Parámetro D1
        [CategoryAttribute("Equipment Parameters:"), DescriptionAttribute("Equipment Input Parameter D1.")]

        public Double D1
        {
            get
            {
                return aD1;
            }

            set
            {
                aD1 = value;
            }
        }

        //Parámetro D2
        [CategoryAttribute("Equipment Parameters:"), DescriptionAttribute("Equipment Input Parameter D2.")]

        public Double D2
        {
            get
            {
                return aD2;
            }

            set
            {
                aD2 = value;
            }
        }

        //Parámetro D3
        [CategoryAttribute("Equipment Parameters:"), DescriptionAttribute("Equipment Input Parameter D3.")]

        public Double D3
        {
            get
            {
                return aD3;
            }

            set
            {
                aD3 = value;
            }
        }

        //Parámetro D4
        [CategoryAttribute("Equipment Parameters:"), DescriptionAttribute("Equipment Input Parameter D4.")]

        public Double D4
        {
            get
            {
                return aD4;
            }

            set
            {
                aD4 = value;
            }
        }

        //Parámetro D5
        [CategoryAttribute("Equipment Parameters:"), DescriptionAttribute("Equipment Input Parameter D5.")]

        public Double D5
        {
            get
            {
                return aD5;
            }

            set
            {
                aD5 = value;
            }
        }

        //Parámetro D6
        [CategoryAttribute("Equipment Parameters:"), DescriptionAttribute("Equipment Input Parameter D6.")]

        public Double D6
        {
            get
            {
                return aD6;
            }

            set
            {
                aD6 = value;
            }
        }


        //Parámetro D7
        [CategoryAttribute("Equipment Parameters:"), DescriptionAttribute("Equipment Input Parameter D7.")]

        public Double D7
        {
            get
            {
                return aD7;
            }

            set
            {
                aD7 = value;
            }
        }


        //Parámetro D8
        [CategoryAttribute("Equipment Parameters:"), DescriptionAttribute("Equipment Input Parameter D8.")]

        public Double D8
        {
            get
            {
                return aD8;
            }

            set
            {
                aD8 = value;
            }
        }


        //Parámetro D9
        [CategoryAttribute("Equipment Parameters:"), DescriptionAttribute("Equipment Input Parameter D9.")]

        public Double D9
        {
            get
            {
                return aD9;
            }

            set
            {
                aD9 = value;
            }
        }


       public void Equipos1()
       {
                
       
       }

        public void Inicializar(Double numequipo1, Double tipoequipo1, Double bN1, Double bN2, Double bN3, Double bN4, Double bD1, Double bD2, Double bD3, Double bD4, Double bD5, Double bD6, Double bD7, Double bD8, Double bD9,Double adicional1,Double adicional2, Double adicional3, Double adicional4)
        {
            numequipo2 = numequipo1;
            
            tipoequipo2 = tipoequipo1;

            //Nº de Corrientes
            aN1 = bN1;
            aN2 = bN2;
            aN3 = bN3;
            aN4 = bN4;

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

            //Adicionales
            adicional11 = adicional1;
            adicional12 = adicional2;
            adicional13 = adicional3;
            adicional14 = adicional4;     
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
