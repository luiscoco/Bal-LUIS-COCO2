using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NumericalMethods;

using TablasAgua1967;

namespace ClaseEquipos
{
    public class ClassDivisor2
    {
        //Identificación del Equipo
        public Double numequipo;

        //Unidades Datos de Entrada
        public Double unidades1 = 0;

        //Datos de Entrada
        public Double numcorrentrada;
        public Double numcorrsalida1;
        public Double numcorrsalida2;

        public Double caudalcorrentrada;
        public Double caudalcorrsalida1;
        public Double caudalcorrsalida2;

        public Double presioncorrentrada;
        public Double presioncorrsalida1;
        public Double presioncorrsalida2;

        public Double entalpiacorrentrada;
        public Double entalpiacorrsalida1;
        public Double entalpiacorrsalida2;

        //Resultados del Equipo
        public Double entropiaentrada=0;
        public Double entropiasalida1=0;
        public Double entropiasalida2=0;

        public Double volumenespecificoentrada=0;
        public Double volumenespecificosalida1=0;
        public Double volumenespecificosalida2=0;

        public Double temperaturaentrada=0;
        public Double temperaturasalida1=0;
        public Double temperaturasalida2=0;

        public Double tituloentrada = 0;
        public Double titulosalida1 = 0;
        public Double titulosalida2 = 0;

        public Double porcentajesalida1 = 0;
        public Double porcentajesalida2 = 0;

        public Double factorflujosalida1 = 0;
        public Double factorflujosalida2 = 0;


        //Objeto para acceder a las Tablas de Agua de 1967
        public Class1 acceso=new Class1();

        public void ClassDivisor()
        { 
        
        }

        public void inicializar(Double caudalcorrentrada10, Double caudalcorrsalida10, Double caudalcorrsalida20, Double presioncorrentrada10, Double presioncorrsalida10, Double presioncorrsalida20, Double entalpiacorrentrada10, Double entalpiacorrsalida10, Double entalpiacorrsalida20)
        {
            caudalcorrentrada = caudalcorrentrada10;
            caudalcorrsalida1 = caudalcorrsalida10;
            caudalcorrsalida2 = caudalcorrsalida20;

            presioncorrentrada = presioncorrentrada10;
            presioncorrsalida1 = presioncorrsalida10;
            presioncorrsalida2 = presioncorrsalida20;

            entalpiacorrentrada = entalpiacorrentrada10;
            entalpiacorrsalida1 = entalpiacorrsalida20;
            entalpiacorrsalida2 = entalpiacorrsalida20;
        }


        public void Calcular()
        {     
            //Cambio de UNIDADES es necesario tranformar las UNIDADES al Sistema Británico para poder ser interpretadas por las Tablas de Vapor ASME 1967
            if(unidades1==2)
            {
                caudalcorrentrada = caudalcorrentrada * 0.4536;
                caudalcorrsalida1 = caudalcorrsalida1 * 0.4536;
                caudalcorrsalida2 = caudalcorrsalida2 * 0.4536;
               
                presioncorrentrada = presioncorrentrada * (6.8947572/100);
                presioncorrsalida1 = presioncorrsalida1 * (6.8947572/100);
                presioncorrsalida2 = presioncorrsalida2 * (6.8947572/100);
                
                entalpiacorrentrada = entalpiacorrentrada * 2.326009;
                entalpiacorrsalida1 = entalpiacorrsalida1 * 2.326009;
                entalpiacorrsalida2 = entalpiacorrsalida2 * 2.326009;  
            }

            //Calculo de las ENTROPIAS de entrada y salida

            //Es necesario enviar a las funciones de las Tablas de Vapor las variables en Unidades Británicas
            entropiaentrada = acceso.sph(presioncorrentrada / (6.8947572 / 100), entalpiacorrentrada / 2.326009);
            //Una vez recibida los resultados de las funciones de las Tablas de Vapor es necesario pasar de Unidades Británicas a Unidades del Sistema Internacional para graficar los resultados en la Aplicación.
            entropiaentrada = entropiaentrada * 4.1868;

            //Es necesario enviar a las funciones de las Tablas de Vapor las variables en Unidades Británicas
            entropiasalida1 = acceso.sph(presioncorrsalida1 / (6.8947572 / 100), entalpiacorrsalida1 / 2.326009);
            //Una vez recibida los resultados de las funciones de las Tablas de Vapor es necesario pasar de Unidades Británicas a Unidades del Sistema Internacional para graficar los resultados en la Aplicación.
            entropiasalida1 = entropiasalida1 * 4.1868;

            //Es necesario enviar a las funciones de las Tablas de Vapor las variables en Unidades Británicas
            entropiasalida2 = acceso.sph(presioncorrsalida2 / (6.8947572 / 100), entalpiacorrsalida2 / 2.326009);
            //Una vez recibida los resultados de las funciones de las Tablas de Vapor es necesario pasar de Unidades Británicas a Unidades del Sistema Internacional para graficar los resultados en la Aplicación.
            entropiasalida2 = entropiasalida2 * 4.1868;


            //Cálculo de las TEMPERATURAS de entrada y salida (enviamos los datos a las funciones de las Tablas de Vapor en Unidades Británicas siempre)
            temperaturaentrada = acceso.tph(presioncorrentrada / (6.8947572 / 100), entalpiacorrentrada / 2.326009);
            //Una vez recibida los resultados de las funciones de las Tablas de Vapor es necesario pasar de Unidades Británicas a Unidades del Sistema Internacional para graficar los resultados en la Aplicación.
            temperaturaentrada = (temperaturaentrada - 32.0) * (5.0 / 9.0);


            //Cálculo de las TEMPERATURAS de entrada y salida  (enviamos los datos a las funciones de las Tablas de Vapor en Unidades Británicas siempre)
            temperaturasalida1 = acceso.tph(presioncorrsalida1 / (6.8947572 / 100), entalpiacorrsalida1 / 2.326009);
            //Una vez recibida los resultados de las funciones de las Tablas de Vapor es necesario pasar de Unidades Británicas a Unidades del Sistema Internacional para graficar los resultados en la Aplicación.
            temperaturasalida1 = (temperaturasalida1 - 32.0) * (5.0 / 9.0);

            //Cálculo de las TEMPERATURAS de entrada y salida  (enviamos los datos a las funciones de las Tablas de Vapor en Unidades Británicas siempre)
            temperaturasalida2 = acceso.tph(presioncorrsalida2 / (6.8947572 / 100), entalpiacorrsalida2 / 2.326009);
            //Una vez recibida los resultados de las funciones de las Tablas de Vapor es necesario pasar de Unidades Británicas a Unidades del Sistema Internacional para graficar los resultados en la Aplicación.
            temperaturasalida2 = (temperaturasalida2 - 32.0) * (5.0 / 9.0);


            //Cálculo del VOLUMEN ESPECÍFICO de entrada y salida  (enviamos los datos a las funciones de las Tablas de Vapor en Unidades Británicas siempre)
            volumenespecificoentrada = acceso.vph(presioncorrentrada / (6.8947572 / 100), entalpiacorrentrada / 2.326009);
            //Una vez recibida los resultados de las funciones de las Tablas de Vapor es necesario pasar de Unidades Británicas a Unidades del Sistema Internacional para graficar los resultados en la Aplicación.
            volumenespecificoentrada = volumenespecificoentrada / 16.018463535;

            //Cálculo del VOLUMEN ESPECÍFICO de entrada y salida  (enviamos los datos a las funciones de las Tablas de Vapor en Unidades Británicas siempre)
            volumenespecificosalida1 = acceso.vph(presioncorrsalida1 / (6.8947572 / 100), entalpiacorrsalida1 / 2.326009);
            //Una vez recibida los resultados de las funciones de las Tablas de Vapor es necesario pasar de Unidades Británicas a Unidades del Sistema Internacional para graficar los resultados en la Aplicación.
            volumenespecificosalida1 = volumenespecificosalida1 / 16.018463535;

            //Cálculo del VOLUMEN ESPECÍFICO de entrada y salida  (enviamos los datos a las funciones de las Tablas de Vapor en Unidades Británicas siempre)
            volumenespecificosalida2 = acceso.vph(presioncorrsalida2 / (6.8947572 / 100), entalpiacorrsalida2 / 2.326009);
            //Una vez recibida los resultados de las funciones de las Tablas de Vapor es necesario pasar de Unidades Británicas a Unidades del Sistema Internacional para graficar los resultados en la Aplicación.
            volumenespecificosalida2 = volumenespecificosalida2 / 16.018463535;

            //Cálculo del TÍTULO de entrada y salida  (enviamos los datos a las funciones de las Tablas de Vapor en Unidades Británicas siempre)
            tituloentrada = acceso.xph(presioncorrentrada / (6.8947572 / 100), entalpiacorrentrada / 2.326009);
            titulosalida1 = acceso.xph(presioncorrsalida1 / (6.8947572 / 100), entalpiacorrsalida1 / 2.326009);
            titulosalida2 = acceso.xph(presioncorrsalida2 / (6.8947572 / 100), entalpiacorrsalida2 / 2.326009);

            //Cálculo del PORCENTAJE de caudal de salida1 y salida2 
            porcentajesalida1 = (caudalcorrsalida1 / caudalcorrentrada)*100;
            porcentajesalida2 = (caudalcorrsalida2 / caudalcorrentrada)*100;

            //Cálculo del FACTOR de FLUJO
            factorflujosalida1 = caudalcorrsalida1/(Math.Pow(presioncorrentrada/volumenespecificoentrada,0.5));
            factorflujosalida2 = caudalcorrsalida2 / (Math.Pow(presioncorrentrada / volumenespecificoentrada, 0.5));
        }        
    }
}
