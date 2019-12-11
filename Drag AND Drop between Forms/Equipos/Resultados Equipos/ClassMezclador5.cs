using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NumericalMethods;

using TablasAgua1967;

namespace ClaseEquipos
{
    public class ClassMezclador5
    {
        //Identificación del Equipo
        public Double numequipo;

        //Unidades Datos de Entrada
        public Double unidades1 = 0;

        //Datos de Entrada
        public Double numcorrentrada1;
        public Double numcorrentrada2;
        public Double numcorrsalida;

        public Double caudalcorrentrada1;
        public Double caudalcorrentrada2;
        public Double caudalcorrsalida;

        public Double presioncorrentrada1;
        public Double presioncorrentrada2;
        public Double presioncorrsalida;

        public Double entalpiacorrentrada1;
        public Double entalpiacorrentrada2;
        public Double entalpiacorrsalida;

        //Resultados del Equipo
        public Double entropiaentrada1=0;
        public Double entropiaentrada2=0;
        public Double entropiasalida=0;

        public Double volumenespecificoentrada1=0;
        public Double volumenespecificoentrada2=0;
        public Double volumenespecificosalida=0;

        public Double temperaturaentrada1=0;
        public Double temperaturaentrada2=0;
        public Double temperaturasalida=0;

        public Double tituloentrada1 = 0;
        public Double tituloentrada2 = 0;
        public Double titulosalida = 0;

        //Objeto para acceder a las Tablas de Agua de 1967
        public Class1 acceso=new Class1();

        public void ClassMezclador()
        { 
        
        }

        public void inicializar(Double caudalcorrentrada10,Double caudalcorrentrada20,Double caudalcorrsalida10,Double presioncorrentrada10,Double presioncorrentrada20,Double presioncorrsalida10,Double entalpiacorrentrada10,Double entalpiacorrentrada20,Double entalpiacorrsalida10)
        {
            caudalcorrentrada1 = caudalcorrentrada10;
            caudalcorrentrada2 = caudalcorrentrada20;
            caudalcorrsalida = caudalcorrsalida10;

            presioncorrentrada1 = presioncorrentrada10;
            presioncorrentrada2 = presioncorrentrada20;
            presioncorrsalida = presioncorrsalida10;

            entalpiacorrentrada1 = entalpiacorrentrada10;
            entalpiacorrentrada2 = entalpiacorrentrada20;
            entalpiacorrsalida = entalpiacorrsalida10;
        }

        public void Calcular()
        {     
            //Cambio de UNIDADES es necesario tranformar las UNIDADES al Sistema Británico para poder ser interpretadas por las Tablas de Vapor ASME 1967
            if(unidades1==2)
            {
                caudalcorrentrada1 = caudalcorrentrada1 * 0.4536;
                caudalcorrentrada2 = caudalcorrentrada2 * 0.4536;
                caudalcorrsalida=caudalcorrsalida * 0.4536;
               
                presioncorrentrada1 = presioncorrentrada1 * (6.8947572/100);
                presioncorrentrada2 = presioncorrentrada2 * (6.8947572/100);
                presioncorrsalida = presioncorrsalida * (6.8947572/100);
                
                entalpiacorrentrada1 = entalpiacorrentrada1 * 2.326009;
                entalpiacorrentrada2 = entalpiacorrentrada2 * 2.326009;
                entalpiacorrsalida = entalpiacorrsalida * 2.326009;           
            }

            //Calculo de las ENTROPIAS de entrada y salida

            //Es necesario enviar a las funciones de las Tablas de Vapor las variables en Unidades Británicas
            entropiaentrada1 = acceso.sph(presioncorrentrada1 / (6.8947572 / 100), entalpiacorrentrada1 / 2.326009);
            //Una vez recibida los resultados de las funciones de las Tablas de Vapor es necesario pasar de Unidades Británicas a Unidades del Sistema Internacional para graficar los resultados en la Aplicación.
            entropiaentrada1 = entropiaentrada1 * 4.1868;

            //Es necesario enviar a las funciones de las Tablas de Vapor las variables en Unidades Británicas
            entropiaentrada2 = acceso.sph(presioncorrentrada2 / (6.8947572 / 100), entalpiacorrentrada2 / 2.326009);
            //Una vez recibida los resultados de las funciones de las Tablas de Vapor es necesario pasar de Unidades Británicas a Unidades del Sistema Internacional para graficar los resultados en la Aplicación.
            entropiaentrada2 = entropiaentrada2 * 4.1868;

            //Es necesario enviar a las funciones de las Tablas de Vapor las variables en Unidades Británicas
            entropiasalida = acceso.sph(presioncorrsalida / (6.8947572 / 100), entalpiacorrsalida / 2.326009);
            //Una vez recibida los resultados de las funciones de las Tablas de Vapor es necesario pasar de Unidades Británicas a Unidades del Sistema Internacional para graficar los resultados en la Aplicación.
            entropiasalida = entropiasalida * 4.1868;

            //Cálculo de las TEMPERATURAS de entrada y salida (enviamos los datos a las funciones de las Tablas de Vapor en Unidades Británicas siempre)
            temperaturaentrada1 = acceso.tph(presioncorrentrada1 / (6.8947572 / 100), entalpiacorrentrada1 / 2.326009);
            //Una vez recibida los resultados de las funciones de las Tablas de Vapor es necesario pasar de Unidades Británicas a Unidades del Sistema Internacional para graficar los resultados en la Aplicación.
            temperaturaentrada1 = (temperaturaentrada1 - 32.0) * (5.0 / 9.0);

            //Cálculo de las TEMPERATURAS de entrada y salida (enviamos los datos a las funciones de las Tablas de Vapor en Unidades Británicas siempre)
            temperaturaentrada2 = acceso.tph(presioncorrentrada2 / (6.8947572 / 100), entalpiacorrentrada2 / 2.326009);
            //Una vez recibida los resultados de las funciones de las Tablas de Vapor es necesario pasar de Unidades Británicas a Unidades del Sistema Internacional para graficar los resultados en la Aplicación.
            temperaturaentrada2 = (temperaturaentrada2 - 32.0) * (5.0 / 9.0);

            //Cálculo de las TEMPERATURAS de entrada y salida  (enviamos los datos a las funciones de las Tablas de Vapor en Unidades Británicas siempre)
            temperaturasalida = acceso.tph(presioncorrsalida / (6.8947572 / 100), entalpiacorrsalida / 2.326009);
            //Una vez recibida los resultados de las funciones de las Tablas de Vapor es necesario pasar de Unidades Británicas a Unidades del Sistema Internacional para graficar los resultados en la Aplicación.
            temperaturasalida = (temperaturasalida - 32.0) * (5.0 / 9.0);

            //Cálculo del VOLUMEN ESPECÍFICO de entrada y salida  (enviamos los datos a las funciones de las Tablas de Vapor en Unidades Británicas siempre)
            volumenespecificoentrada1 = acceso.vph(presioncorrentrada1 / (6.8947572 / 100), entalpiacorrentrada1 / 2.326009);
            //Una vez recibida los resultados de las funciones de las Tablas de Vapor es necesario pasar de Unidades Británicas a Unidades del Sistema Internacional para graficar los resultados en la Aplicación.
            volumenespecificoentrada1 = volumenespecificoentrada1 / 16.018463535;

            //Cálculo del VOLUMEN ESPECÍFICO de entrada y salida  (enviamos los datos a las funciones de las Tablas de Vapor en Unidades Británicas siempre)
            volumenespecificoentrada2 = acceso.vph(presioncorrentrada2 / (6.8947572 / 100), entalpiacorrentrada2 / 2.326009);
            //Una vez recibida los resultados de las funciones de las Tablas de Vapor es necesario pasar de Unidades Británicas a Unidades del Sistema Internacional para graficar los resultados en la Aplicación.
            volumenespecificoentrada2 = volumenespecificoentrada2 / 16.018463535;

            //Cálculo del VOLUMEN ESPECÍFICO de entrada y salida  (enviamos los datos a las funciones de las Tablas de Vapor en Unidades Británicas siempre)
            volumenespecificosalida = acceso.vph(presioncorrsalida / (6.8947572 / 100), entalpiacorrsalida / 2.326009);
            //Una vez recibida los resultados de las funciones de las Tablas de Vapor es necesario pasar de Unidades Británicas a Unidades del Sistema Internacional para graficar los resultados en la Aplicación.
            volumenespecificosalida = volumenespecificosalida / 16.018463535;

            //Cálculo del TÍTULO de entrada y salida  (enviamos los datos a las funciones de las Tablas de Vapor en Unidades Británicas siempre)
            tituloentrada1 = acceso.xph(presioncorrentrada1 / (6.8947572 / 100), entalpiacorrentrada1 / 2.326009);
            tituloentrada2 = acceso.xph(presioncorrentrada2 / (6.8947572 / 100), entalpiacorrentrada2 / 2.326009);
            titulosalida = acceso.xph(presioncorrsalida / (6.8947572 / 100), entalpiacorrsalida / 2.326009);   
        }        
    }
}
