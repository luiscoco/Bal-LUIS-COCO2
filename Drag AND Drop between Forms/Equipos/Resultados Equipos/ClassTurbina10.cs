using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NumericalMethods;

using TablasAgua1967;

namespace ClaseEquipos
{
    public class ClassTurbina10
    {
        //Identificación del Equipo
        public Double numequipo;

        //Unidades Datos de Entrada
        public Double unidades1 = 0;

        //Datos de Entrada
        public Double numcorrentrada;
        public Double numcorrsalida;
        public Double caudalcorrentrada;
        public Double caudalcorrsalida;
        public Double presioncorrentrada;
        public Double presioncorrsalida;
        public Double entalpiacorrentrada;
        public Double entalpiacorrsalida;

        //Resultados del Equipo
        public Double entropiaentrada=0;
        public Double entropiasalida=0;
        public Double volumenespecificoentrada=0;
        public Double volumenespecificosalida=0;
        public Double temperaturaentrada=0;
        public Double temperaturasalida=0;
        public Double tituloentrada = 0;
        public Double titulosalida = 0;
        public Double relacionpresiones=0;
        public Double factorflujo=0;
        public Double eficiencia=0;
        public Double potencia=0;

        //Objeto para acceder a las Tablas de Agua de 1967
        public Class1 acceso=new Class1();

        public void ClassTurbina()
        { 
        
        }

        public void inicializar(Double caudalcorrentrada1,Double caudalcorrsalida1,Double presioncorrentrada1,Double presioncorrsalida1,Double entalpiacorrentrada1,Double entalpiacorrsalida1)
        {
            caudalcorrentrada = caudalcorrentrada1;
            caudalcorrsalida = caudalcorrsalida1;
            presioncorrentrada = presioncorrentrada1;
            presioncorrsalida = presioncorrsalida1;
            entalpiacorrentrada = entalpiacorrentrada1;                    
        }


        public void Calcular()
        {     
            //Cambio de UNIDADES es necesario tranformar las UNIDADES al Sistema Británico para poder ser interpretadas por las Tablas de Vapor ASME 1967
            if(unidades1==2)
            {
                caudalcorrentrada = caudalcorrentrada * 0.4536;
                caudalcorrsalida=caudalcorrsalida * 0.4536;
               
                presioncorrentrada = presioncorrentrada * (6.8947572/100);
                presioncorrsalida = presioncorrsalida * (6.8947572/100);
                
                entalpiacorrentrada = entalpiacorrentrada * 2.326009;
                entalpiacorrsalida = entalpiacorrsalida * 2.326009;           
            }

            //Calculo de las ENTROPIAS de entrada y salida

            //Es necesario enviar a las funciones de las Tablas de Vapor las variables en Unidades Británicas
            entropiaentrada = acceso.sph(presioncorrentrada / (6.8947572 / 100), entalpiacorrentrada / 2.326009);
            //Una vez recibida los resultados de las funciones de las Tablas de Vapor es necesario pasar de Unidades Británicas a Unidades del Sistema Internacional para graficar los resultados en la Aplicación.
            entropiaentrada = entropiaentrada * 4.1868;

            //Es necesario enviar a las funciones de las Tablas de Vapor las variables en Unidades Británicas
            entropiasalida = acceso.sph(presioncorrsalida / (6.8947572 / 100), entalpiacorrsalida / 2.326009);
            //Una vez recibida los resultados de las funciones de las Tablas de Vapor es necesario pasar de Unidades Británicas a Unidades del Sistema Internacional para graficar los resultados en la Aplicación.
            entropiasalida = entropiasalida * 4.1868;


            //Cálculo de las TEMPERATURAS de entrada y salida (enviamos los datos a las funciones de las Tablas de Vapor en Unidades Británicas siempre)
            temperaturaentrada = acceso.tph(presioncorrentrada / (6.8947572 / 100), entalpiacorrentrada / 2.326009);
            //Una vez recibida los resultados de las funciones de las Tablas de Vapor es necesario pasar de Unidades Británicas a Unidades del Sistema Internacional para graficar los resultados en la Aplicación.
            temperaturaentrada = (temperaturaentrada - 32.0) * (5.0 / 9.0);


            //Cálculo de las TEMPERATURAS de entrada y salida  (enviamos los datos a las funciones de las Tablas de Vapor en Unidades Británicas siempre)
            temperaturasalida = acceso.tph(presioncorrsalida / (6.8947572 / 100), entalpiacorrsalida / 2.326009);
            //Una vez recibida los resultados de las funciones de las Tablas de Vapor es necesario pasar de Unidades Británicas a Unidades del Sistema Internacional para graficar los resultados en la Aplicación.
            temperaturasalida = (temperaturasalida - 32.0) * (5.0 / 9.0);


            //Cálculo del VOLUMEN ESPECÍFICO de entrada y salida  (enviamos los datos a las funciones de las Tablas de Vapor en Unidades Británicas siempre)
            volumenespecificoentrada = acceso.vph(presioncorrentrada / (6.8947572 / 100), entalpiacorrentrada / 2.326009);
            //Una vez recibida los resultados de las funciones de las Tablas de Vapor es necesario pasar de Unidades Británicas a Unidades del Sistema Internacional para graficar los resultados en la Aplicación.
            volumenespecificoentrada = volumenespecificoentrada / 16.018463535;

            //Cálculo del VOLUMEN ESPECÍFICO de entrada y salida  (enviamos los datos a las funciones de las Tablas de Vapor en Unidades Británicas siempre)
            volumenespecificosalida = acceso.vph(presioncorrsalida / (6.8947572 / 100), entalpiacorrsalida / 2.326009);
            //Una vez recibida los resultados de las funciones de las Tablas de Vapor es necesario pasar de Unidades Británicas a Unidades del Sistema Internacional para graficar los resultados en la Aplicación.
            volumenespecificosalida = volumenespecificosalida / 16.018463535;

            //Cálculo del TÍTULO de entrada y salida  (enviamos los datos a las funciones de las Tablas de Vapor en Unidades Británicas siempre)
            tituloentrada = acceso.xph(presioncorrentrada/ (6.8947572 / 100), entalpiacorrentrada / 2.326009);
            titulosalida = acceso.xph(presioncorrsalida / (6.8947572 / 100), entalpiacorrsalida / 2.326009);     

            //Cálculo RELACIÓN DE PRESIONES (PEntrada/PSalida)
            relacionpresiones = presioncorrentrada / presioncorrsalida;

            //Cálculo del FACTOR DE FLUJO
            factorflujo = caudalcorrentrada / (Math.Pow(presioncorrentrada / volumenespecificoentrada, 0.5));

            //Cálculo de la EFICIENCIA
            eficiencia = ((entalpiacorrentrada / 2.326009) - (entalpiacorrsalida / 2.326009)) / ((entalpiacorrentrada / 2.326009) - acceso.hps(presioncorrsalida / (6.8947572 / 100), (acceso.sph(presioncorrentrada / (6.8947572 / 100), entalpiacorrentrada / 2.326009))));
            //H2 - H1 + rendcorrejido(P1, H1, P2, H2, D1, D5) * (H1 - acceso.hps(P2, (acceso.sph(P1, H1))));

            //Cálculo de la POTENCIA
            potencia = (entalpiacorrentrada - entalpiacorrsalida) * caudalcorrentrada;       
        }        
    }
}
