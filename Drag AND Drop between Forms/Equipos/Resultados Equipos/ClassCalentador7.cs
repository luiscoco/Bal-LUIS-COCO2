using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NumericalMethods;

using TablasAgua1967;

namespace ClaseEquipos
{
    public class ClassCalentador7
    {
        //Identificación del Equipo
        public Double numequipo;

        //Unidades Datos de Entrada
        public Double unidades1 = 0;

        //Datos de Entrada
        public Double numcorrentrada1;
        public Double numcorrsalida1;
        public Double numcorrentrada2;
        public Double numcorrsalida2;

        public Double caudalcorrentrada1;
        public Double caudalcorrsalida1;
        public Double caudalcorrentrada2;
        public Double caudalcorrsalida2;

        public Double presioncorrentrada1;
        public Double presioncorrsalida1;
        public Double presioncorrentrada2;
        public Double presioncorrsalida2;

        public Double entalpiacorrentrada1;
        public Double entalpiacorrsalida1;
        public Double entalpiacorrentrada2;
        public Double entalpiacorrsalida2;

        //Resultados del Equipo
        public Double entropiaentrada1=0;
        public Double entropiasalida1=0;
        public Double entropiaentrada2 = 0;
        public Double entropiasalida2 = 0;

        public Double volumenespecificoentrada1=0;
        public Double volumenespecificosalida1=0;
        public Double volumenespecificoentrada2 = 0;
        public Double volumenespecificosalida2 = 0;

        public Double temperaturaentrada1=0;
        public Double temperaturasalida1=0;
        public Double temperaturaentrada2 = 0;
        public Double temperaturasalida2 = 0;

        public Double tituloentrada1 = 0;
        public Double titulosalida1 = 0;
        public Double tituloentrada2 = 0;
        public Double titulosalida2 = 0;

        public Double TTD = 0;
        public Double DCA = 0;

        //Objeto para acceder a las Tablas de Agua de 1967
        public Class1 acceso=new Class1();

        public void ClassCalentador()
        { 
        
        }

        public void inicializar(Double caudalcorrentrada10, Double caudalcorrentrada20, Double caudalcorrsalida10, Double caudalcorrsalida20, Double presioncorrentrada10, Double presioncorrentrada20, Double presioncorrsalida10, Double presioncorrsalida20, Double entalpiacorrentrada10, Double entalpiacorrentrada20, Double entalpiacorrsalida10, Double entalpiacorrsalida20)
        {
            caudalcorrentrada1 = caudalcorrentrada10;
            caudalcorrentrada2 = caudalcorrentrada20;

            caudalcorrsalida1 = caudalcorrsalida10;
            caudalcorrsalida2 = caudalcorrsalida20;

            presioncorrentrada1 = presioncorrentrada10;
            presioncorrentrada2 = presioncorrentrada20;

            presioncorrsalida1 = presioncorrsalida10;
            presioncorrsalida2 = presioncorrsalida20;
            
            entalpiacorrentrada1 = entalpiacorrentrada10;
            entalpiacorrentrada2 = entalpiacorrentrada20;        
        }


        public void Calcular()
        {     
            //Cambio de UNIDADES es necesario tranformar las UNIDADES al Sistema Británico para poder ser interpretadas por las Tablas de Vapor ASME 1967
            if(unidades1==2)
            {
                caudalcorrentrada1 = caudalcorrentrada1 * 0.4536;
                caudalcorrentrada2 = caudalcorrentrada2 * 0.4536;

                caudalcorrsalida1=caudalcorrsalida1 * 0.4536;
                caudalcorrsalida2 = caudalcorrsalida2 * 0.4536;

                presioncorrentrada1 = presioncorrentrada1 * (6.8947572/100);
                presioncorrentrada2 = presioncorrentrada2 * (6.8947572 / 100);
                
                presioncorrsalida1 = presioncorrsalida1 * (6.8947572/100);
                presioncorrsalida2 = presioncorrsalida2 * (6.8947572 / 100);

                entalpiacorrentrada1 = entalpiacorrentrada1 * 2.326009;
                entalpiacorrentrada2 = entalpiacorrentrada2 * 2.326009;
                
                entalpiacorrsalida1 = entalpiacorrsalida1 * 2.326009;
                entalpiacorrsalida2 = entalpiacorrsalida2 * 2.326009;
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
            entropiasalida1 = acceso.sph(presioncorrsalida1 / (6.8947572 / 100), entalpiacorrsalida1 / 2.326009);
            //Una vez recibida los resultados de las funciones de las Tablas de Vapor es necesario pasar de Unidades Británicas a Unidades del Sistema Internacional para graficar los resultados en la Aplicación.
            entropiasalida1 = entropiasalida1 * 4.1868;

            //Es necesario enviar a las funciones de las Tablas de Vapor las variables en Unidades Británicas
            entropiasalida2 = acceso.sph(presioncorrsalida2 / (6.8947572 / 100), entalpiacorrsalida2 / 2.326009);
            //Una vez recibida los resultados de las funciones de las Tablas de Vapor es necesario pasar de Unidades Británicas a Unidades del Sistema Internacional para graficar los resultados en la Aplicación.
            entropiasalida2 = entropiasalida2 * 4.1868;

            //Cálculo de las TEMPERATURAS de entrada y salida (enviamos los datos a las funciones de las Tablas de Vapor en Unidades Británicas siempre)
            temperaturaentrada1 = acceso.tph(presioncorrentrada1 / (6.8947572 / 100), entalpiacorrentrada1 / 2.326009);
            //Una vez recibida los resultados de las funciones de las Tablas de Vapor es necesario pasar de Unidades Británicas a Unidades del Sistema Internacional para graficar los resultados en la Aplicación.
            temperaturaentrada1 = (temperaturaentrada1 - 32.0) * (5.0 / 9.0);

            //Cálculo de las TEMPERATURAS de entrada y salida (enviamos los datos a las funciones de las Tablas de Vapor en Unidades Británicas siempre)
            temperaturaentrada2 = acceso.tph(presioncorrentrada2 / (6.8947572 / 100), entalpiacorrentrada2 / 2.326009);
            //Una vez recibida los resultados de las funciones de las Tablas de Vapor es necesario pasar de Unidades Británicas a Unidades del Sistema Internacional para graficar los resultados en la Aplicación.
            temperaturaentrada2 = (temperaturaentrada2 - 32.0) * (5.0 / 9.0);

            //Cálculo de las TEMPERATURAS de entrada y salida  (enviamos los datos a las funciones de las Tablas de Vapor en Unidades Británicas siempre)
            temperaturasalida1 = acceso.tph(presioncorrsalida1 / (6.8947572 / 100), entalpiacorrsalida1 / 2.326009);
            //Una vez recibida los resultados de las funciones de las Tablas de Vapor es necesario pasar de Unidades Británicas a Unidades del Sistema Internacional para graficar los resultados en la Aplicación.
            temperaturasalida1 = (temperaturasalida1 - 32.0) * (5.0 / 9.0);

            //Cálculo de las TEMPERATURAS de entrada y salida  (enviamos los datos a las funciones de las Tablas de Vapor en Unidades Británicas siempre)
            temperaturasalida2 = acceso.tph(presioncorrsalida2 / (6.8947572 / 100), entalpiacorrsalida2 / 2.326009);
            //Una vez recibida los resultados de las funciones de las Tablas de Vapor es necesario pasar de Unidades Británicas a Unidades del Sistema Internacional para graficar los resultados en la Aplicación.
            temperaturasalida2 = (temperaturasalida2 - 32.0) * (5.0 / 9.0);

            //Cálculo del VOLUMEN ESPECÍFICO de entrada y salida  (enviamos los datos a las funciones de las Tablas de Vapor en Unidades Británicas siempre)
            volumenespecificoentrada1 = acceso.vph(presioncorrentrada1 / (6.8947572 / 100), entalpiacorrentrada1 / 2.326009);
            //Una vez recibida los resultados de las funciones de las Tablas de Vapor es necesario pasar de Unidades Británicas a Unidades del Sistema Internacional para graficar los resultados en la Aplicación.
            volumenespecificoentrada1 = volumenespecificoentrada1 / 16.018463535;

            //Cálculo del VOLUMEN ESPECÍFICO de entrada y salida  (enviamos los datos a las funciones de las Tablas de Vapor en Unidades Británicas siempre)
            volumenespecificoentrada2 = acceso.vph(presioncorrentrada2 / (6.8947572 / 100), entalpiacorrentrada2 / 2.326009);
            //Una vez recibida los resultados de las funciones de las Tablas de Vapor es necesario pasar de Unidades Británicas a Unidades del Sistema Internacional para graficar los resultados en la Aplicación.
            volumenespecificoentrada2 = volumenespecificoentrada2 / 16.018463535;

            //Cálculo del VOLUMEN ESPECÍFICO de entrada y salida  (enviamos los datos a las funciones de las Tablas de Vapor en Unidades Británicas siempre)
            volumenespecificosalida1 = acceso.vph(presioncorrsalida1 / (6.8947572 / 100), entalpiacorrsalida1 / 2.326009);
            //Una vez recibida los resultados de las funciones de las Tablas de Vapor es necesario pasar de Unidades Británicas a Unidades del Sistema Internacional para graficar los resultados en la Aplicación.
            volumenespecificosalida1 = volumenespecificosalida1 / 16.018463535;

            //Cálculo del VOLUMEN ESPECÍFICO de entrada y salida  (enviamos los datos a las funciones de las Tablas de Vapor en Unidades Británicas siempre)
            volumenespecificosalida2 = acceso.vph(presioncorrsalida2 / (6.8947572 / 100), entalpiacorrsalida2 / 2.326009);
            //Una vez recibida los resultados de las funciones de las Tablas de Vapor es necesario pasar de Unidades Británicas a Unidades del Sistema Internacional para graficar los resultados en la Aplicación.
            volumenespecificosalida2 = volumenespecificosalida2 / 16.018463535;

            //Cálculo del TÍTULO de entrada y salida  (enviamos los datos a las funciones de las Tablas de Vapor en Unidades Británicas siempre)
            tituloentrada1 = acceso.xph(presioncorrentrada1 / (6.8947572 / 100), entalpiacorrentrada1 / 2.326009);
            tituloentrada2 = acceso.xph(presioncorrentrada2 / (6.8947572 / 100), entalpiacorrentrada2 / 2.326009);
            titulosalida1 = acceso.xph(presioncorrsalida1 / (6.8947572 / 100), entalpiacorrsalida1 / 2.326009);
            titulosalida2 = acceso.xph(presioncorrsalida2 / (6.8947572 / 100), entalpiacorrsalida2 / 2.326009);

            //TTD 
            TTD = temperaturaentrada2 - temperaturasalida1;

            //DCA
            DCA = temperaturasalida2 - temperaturaentrada1;
        }        
    }
}
