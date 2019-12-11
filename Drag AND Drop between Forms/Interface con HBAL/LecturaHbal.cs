//Incluir los cabeceros de las siguientes librerias. Es similar a utlizar "include<stdio.h>"
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//Para utilizar las funciones de lectura de Archivos
using System.IO;
using System.Globalization;

//Clase que contiene la definición de los diferentes Equipos
using ClaseEquipos;

using Drag_AND_Drop_between_Forms;



namespace Files_in_csharp
{

//--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public partial class LecturaHbal : Form
    {
        //Puntero a la Aplicación principal
        Aplicacion puntero1=new Aplicacion();

        //Puntero para lectura del archivo que hemos abierto con la función lecturaArchivosHBALToolStripMenuItem_Click de la Aplicación principal
        StreamReader fl;

        //Guardamos todas las líneas del Fichero que estamos leyendo en este array de String llamado "lineas"
        //Después iremos interpretando todos los datos que tiene cada una de las líneas del fichero leido
        public List<String> lineas = new List<String>();

        //
        public int numlineasfichero = 0;
        public Double numerolineas=0;

        //Estas variables generales incluyen los datos mas relevantes del sistema que vamos a simular
        //Número de Equipos
        Double numeroequipos=0;
        //Número de Corrientes
        Double numcorrientes = 0;
        //Número de Iteraciones
        Double numiteraciones = 0;
        //Número de Tablas
        Double numtablas = 0;
        //Error máximo Admisible
        Double errormaximo = 0;
        //Si incluimos o no las Condiciones iniciales
        Double datosinciales = 0;
        //Factor de Iteraciones (EPS-faja)
        Double factoriteraciones = 0.5;
        
        Double pendiente = 0;
        //Sistema de Unidades utilizado
        Double unidades = 2;
        //Si incluimos o no el Fichero de Iteraciones Intermedias. Esta opción está pendiente de programar
        Double ficheroiterintermedias = 1;

        //Creamos una lista de la Clase "equipos" donde guardaremos los Datos de Entrada de los equipos
        public List<Equipos> equipos = new List<Equipos>();

        //Número del Equipos
        Double numequipo=0;

        //Tipo del Equipo
        Double tipoequipo=0;
    
        //Estas variables guardan los número de las corrientes de entrada y salida de los equipos.
        Double N1=0;
        Double N2=0;
        Double N3=0;
        Double N4=0;

        Double[] D = new Double[9];

        //Guardamos las condiciones iniciales (caudal inicial, presión inicial, entalpía inicial y número de corrientes inciales).
        Double[] caudalinicial1 = new Double[10000];
        Double[] presioninicial1 = new Double[10000];
        Double[] entalpiainicial1 = new Double[10000];
        Double[] numcorrienteinicial1 = new Double[10000];

        //Guardamos los Títulos de las Tablas y los Datos de las Tablas  en los arrays "titulostablas1" y "listaTablas1"
        List<String> titulostablas1 = new List<String>();
        List<Double[,]> listaTablas1 = new List<Double[,]>();

        //Array de Corrientes de Salida
        Double[] listacorrsalida = new Double[10000];
        //Contador número de corrsalida
        Int32 corrientesdesalida = 0;

        //Array de Corrientes de Entrada
        Double[] listacorrentrada = new Double[10000];


//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        //CONSTRUCTOR de la Clase LecturaHbal a la cual le envio dos punteros desde la Aplicación Principal desde donde creo el Objeto de la Clase LecturaHbal
        public LecturaHbal(Aplicacion punteroaplicacion,StreamReader fl2)
        {
            //Recibimos el puntero "punteroaplicacion" para acceder a las variables de la Aplicación que guardará los datos del archivo que vamos a leer en esta función
            puntero1 = punteroaplicacion;
            //Recibimos el puntero para lectura del fichero desde la función lecturaArchivosHBALToolStripMenuItem_Click de la Aplicacion princiapl. 
            fl = fl2;

            //Inicializamos los controles y resto de componentes de este cuadro de diálogo
            InitializeComponent();
        }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        //Botón de PROCESAR el Archivo de entrada de datos de HBAL (*.DAT)
        public void button5_Click(object sender, EventArgs e)
        {
            //Cargamos el cursor Wait mientras esta función realiza la lectura del archivo
            this.Cursor = Cursors.WaitCursor;

            String temp="";

            //Leemos la LINEA 0 del Archivo donde se guarda el Título del Archivo
            temp = lineas[0];
            //Guardamos el Título del Archivo en la variable Titulo de la aplicacion principal a la cual accedemos a traves del punpero p1 que apunta a la Aplicación principal
            puntero1.Titulo=temp;

            if (temp =="")
            {
                MessageBox.Show("Hola LUIS COCO Línea Vacia.");
            }

                String numequipos=""; 
                String ll = "";
                int count = 0;

            //Leemos la  LINEA 1 del Archivo de HBAL*.DAT(donde se informa del NUMERO DE QUIPOS, NUMERO CORRIENTES,NUMERO DE TABLAS, ERRO MAXIMO ADMISIBLE, ETC)
              temp = lineas[1];

            //Bucle que lee todos los caracteres en la LINEA 1 del archivo leido
                foreach (char c in temp)
                {
                    //Convertimos la variable c de tipo char en una variable c1 tipo String
                    String c1 = c.ToString();

                    //Vamos creando una cadena de caracteres llamada "ll" que almacena los caracteres leidos en el bucle
                    ll = ll+c1;

                    //Contador utilizado para saber el número de caracteres de la LINEA1 quemos leido
                    count++;
                    
                    //Cuando el número de caracteres leidos es de seis 6 sabemos que hemos leido el Número de Equipos 
                    //Primero leemos el NÚMERO DE EQUIPOS
                    if (count == 6)
                    {
                        numequipos = ll;
                        char[] charsToTrim = { '*', ' ', '\'' };           
                        string result = ll.Trim(charsToTrim);
                        //MessageBox.Show("Nº de Equipos:"+result);
                        //Guardamos el número de equipos leidos en las variables "numeroequipos" declarada en el cabecero de esta clase
                        //y en la variable de la Aplicación principal NumTotalEquipos.
                        numeroequipos = Convert.ToDouble(result);
                        if (numeroequipos <= 0)
                        {
                            MessageBox.Show("Error Equipments Number <= 0.");
                        }

                        //El valor del NumTotalCorrientes es incrementado cada vez que creamos un equipo nuevo en la funcionauxiliar1 del Form del Equipo
                        //puntero1.NumTotalEquipos = (short)numeroequipos;
                        ll = "";
                    }

                    //Segundo leemos el NÚMERO DE CORRIENTES
                    else if (count == 11)
                    {
                        char[] charsToTrim = { '*', ' ', '\'' };
                        string result = ll.Trim(charsToTrim);
                        //MessageBox.Show("Nº de Corrientes:" + result);
                        numcorrientes = Convert.ToDouble(result);
                        if (numcorrientes <= 0)
                        {
                            MessageBox.Show("Error Streams Number <= 0.");
                        }

                        //El valor del NumTotalCorrientes es incrementado cada vez que creamos un equipo nuevo en la funcionauxiliar1 del Form del Equipo
                        //puntero1.NumTotalCorrientes = numcorrientes;
                        ll = "";
                     }

                    //Tercero leemos el NÚMERO DE ITERACIONES
                     else if (count == 17)
                     {
                        char[] charsToTrim = { '*', ' ', '\'' };
                        string result = ll.Trim(charsToTrim);
                        //MessageBox.Show("Nº de Iteraciones:" + result);
                        numiteraciones = Convert.ToDouble(result);
                        if (numiteraciones <= 0)
                        {
                            MessageBox.Show("Error Iteration Number <= 0.");
                        }
                        puntero1.NumMaxIteraciones = numiteraciones;
                        ll = "";
                    }

                    //Cuarto leemos el NÚMERO DE TABLAS
                    else if (count == 21)
                    {
                        char[] charsToTrim = { '*', ' ', '\'' };
                        string result = ll.Trim(charsToTrim);
                        //MessageBox.Show("Nº de Tablas:" + result);
                        numtablas = Convert.ToDouble(result);
                        if (numtablas < 0)
                        {
                            MessageBox.Show("Error Tables Number < 0.");
                        }
                        puntero1.NumTotalTablas = numtablas;
                        ll = "";
                     }

                    //Quinto leemos la precisión (ERROR MÁXIMO ADMISIBLE)
                     else if (count == 31)
                     {
                        char[] charsToTrim = { '*', ' ', '\'' };
                        string result = ll.Trim(charsToTrim);
                        //MessageBox.Show("Precisión:" + result);
                        errormaximo = Convert.ToDouble(result);
                        if (errormaximo <= 0)
                        {
                            MessageBox.Show("Error Max.Error <= 0.");
                        }
                        puntero1.ErrorMaxAdmisible = errormaximo;
                        ll = "";
                     }


                    //Sexto: DATOS INICIALES BUENOS (1 si, 0 no)
                    else if (count == 36)
                    {
                        char[] charsToTrim = { '*', ' ', '\'' };
                        string result = ll.Trim(charsToTrim);
                        //MessageBox.Show("Precisión:" + result);
                        datosinciales = Convert.ToDouble(result);
                        if (datosinciales < 0)
                        {
                            MessageBox.Show("Error datosinciales <= 0.");
                        }
                        puntero1.DatosIniciales = datosinciales;
                        ll = "";
                    }

                     //Septimo: PENDIENTE????
                    else if (count == 41)
                    {
                        char[] charsToTrim = { '*', ' ', '\'' };
                        string result = ll.Trim(charsToTrim);
                        //MessageBox.Show("Precisión:" + result);
                        pendiente = Convert.ToDouble(result);
                        ll = "";
                    }

                    //Octavo: FACTOR CONTROL DE ITERACIONES (EPS "faja de iteraciones"  con valor de 0,5 por defecto)
                    else if (count == 46)
                    {
                        char[] charsToTrim = { '*', ' ', '\'' };
                        string result = ll.Trim(charsToTrim);
                        //MessageBox.Show("Precisión:" + result);
                        factoriteraciones = Convert.ToDouble(result);
                        if (factoriteraciones <= 0)
                        {
                            MessageBox.Show("Error Factor Iteraciones <= 0.");
                        }
                        puntero1.FactorIteraciones = factoriteraciones;
                        ll = "";                        
                    }

                    //Noveno: FICHERO DE ITERACIONES INTERMEDIAS (1 si, 0 no)
                    else if (count == 51)
                    {
                        char[] charsToTrim = { '*', ' ', '\'' };
                        string result = ll.Trim(charsToTrim);
                        //MessageBox.Show("Precisión:" + result);
                        ficheroiterintermedias = Convert.ToDouble(result);
                        //if ((ficheroiterintermedias != 0) && (ficheroiterintermedias != 1))
                        //{
                        //    MessageBox.Show("Error Streams Number != 0 and !=1");
                        //}
                        puntero1.FicheroIteraciones = ficheroiterintermedias;
                        ll = "";
                    }
                        
                    //Decimo: UNIDADES (1 Britanicas, 2 Métricas, 3 Sistema Internacional)
                    else if (count == 80)
                    {
                        char[] charsToTrim = { '*', ' ', '\'' };
                        string result = ll.Trim(charsToTrim);
                        //MessageBox.Show("Precisión:" + result);
                        unidades = Convert.ToDouble(result);
                        ll = "";

                        //La designación de Unidades en mi aplicación es diferente a la de HBAL
                        // Hbal Británicas=1: Mi programa Británicas=0
                        // Hbal Métricas=2: Mi programa Sistema Métrico=1
                        // Hbal S.I.=3: Mi programa Sistema Internacional=2
                        if (unidades == 1)
                        {
                            unidades = 0;
                        }
                        else if (unidades == 2)
                        {
                            unidades = 1;
                        }
                        else if (unidades == 3)
                        {
                            unidades = 2;
                        }

                        else
                        {
                            MessageBox.Show("LUIS COCO error en el valor de designación de Unidades. Valores permitidos(Británicas:1, Métricas:2 y S.I.:3");
                        }

                        puntero1.unidades = unidades;
                    }

                    else
                    {

                    }
                }
                
                //Lectura del resto de líneas del archivo de entrada de datos *.dat. Leemos tantas líneas como número de equipos tenga el fichero
                //PRINCIPIO FOR del numero equipos
                //Inicializamos el bucle en 2 porque las dos primeras líneas del archivo son el Títulos del Archivo (LINEA 0) y los Datos Generales del Archivo (LINEA 1)
                //
                //En cada una de las iteraciones del bucle incrementamos la variable "a" en dos unidades porque leemos las dos líneas que definen los datos de entrada de cada Equipo
                //es decir leemos la línea donde se definen las corrientes de entrada y salida del equipo (N1, N2, N3, N4, etc)
                //y posteriormente leemos los datos que definen el equipo (D1, D2, D3, D4, D5, D7, etc)
                for (int a=2;a<=(int)(2*numeroequipos);a=a+2)
                {
                    //PRINCIPIO FOR del numero equipos
                    //Leemos el resto: LINEA 3 (Numero de Equipo, Tipo de Equipo, N1, N2, N3, N4)
                    temp = lineas[a];

                    //Leemos resto de líneas del archivo
                                      

                    //Ejemplo de función para leer una subcadena dentro de otra cadena de caracteres
                    //Comando utilizaco ej: string s3 = "Visual C# Express";
                    //                      MessageBox.Show(s3.Substring(7, 2));
                    //Esta función Substring primero indica en que caracter empezamos a leer (en el 7) y el número de caracteres a leer (2 caracteres)
                    //En este caso la cadena s3="C#"

                    //Este bucle lee la primera linea de definición de cada equipo
                    //Este bucle comienza en el caracter 0 e itera hasta leer el caracter 50 que es el último caracter de la primera linea de definición del equipo
                    //Vamos leyendo las corrientes de cada equipo leyendo 10 caracteres y guardándolos en la string luis1
                    //Cadena temporal que guarda los datos leidos de la primera linea de definición de cada equipo 
                    String luis1 = "";
                    int lonvalidacion = 0;
                    lonvalidacion = temp.Length;
                    if (lonvalidacion < 50)
                    {
                        MessageBox.Show("Posible Error en la cadena:"+temp);
                    }
                    for (int i = 0; i <= 50; i = i + 10)
                    {
                        luis1 = temp.Substring(0 + i, 10);

                        //Leemos el NÚMERO DE EQUIPO
                        if (i == 0)
                        {
                            //MessageBox.Show("Equipo Número:" + luis1);
                            numequipo = Convert.ToDouble(luis1);
                            if (numequipo==0)
                            {
                                MessageBox.Show("Posible Error Falta el Número de Equipo en la línea:"+temp);
                            }
                        }

                        //Leemos el TIPO DE EQUIPO
                        else if (i == 10)
                        {
                            //MessageBox.Show("Tipo de Equipo:" + luis1);
                            tipoequipo = Convert.ToDouble(luis1);
                            if (numequipo == 0)
                            {
                                MessageBox.Show("Posible Error Falta el Tipo de Equipo en la línea:" + temp);
                            }
                        }

                        // Leemos la corriente N1
                        else if (i == 20)
                        {
                            //MessageBox.Show("Corriente número 1:" + luis1);
                            N1 = Convert.ToDouble(luis1);

                        }

                        //Leemos la corriente N2
                        else if (i == 30)
                        {
                            //MessageBox.Show("Corriente número 2:" + luis1);
                            N2 = Convert.ToDouble(luis1);
                        }

                        //Leemos la correinte N3
                        else if (i == 40)
                        {
                            //MessageBox.Show("Corriente número 3:" + luis1);
                            N3 = Convert.ToDouble(luis1);
                        }

                        //Leemos la corriente N4
                        else if (i == 50)
                        {
                            //MessageBox.Show("Corriente número 4:" + luis1);
                            N4 = Convert.ToDouble(luis1);
                        }
                    }

                    //if (temp.Length <= 60)
                    //{
                    //    MessageBox.Show("Falta por introducir en el fichero un Índice del Dibujo.");
                    //    this.Hide();
                    //    return;                    
                    //}
                    
                    //luis1 = temp.Substring(60, 5);
                   
                    //MessageBox.Show("Índice del Dibujo:" + luis1);

                    //Leemos las línas que contienen los datos de los Equipos:D1,D2,D3,D4,D5,D6,D7,D8,D9
                    ll = "";
                    count = 0;

                    //Leemos la LINEA 4 con los datos de entrada de los EQUIPOS:D1,D2,D3,D4,D5,D6,D7,D8,D9
                    //Guardamos en la variable temp los datos de la LINEA 4  del archivo 
                    temp = lineas[a + 1];

                    int numd = 0;
                    //Leemos el número de caracteres de la LINEA4
                    Double numcaracteres = temp.Length;
                    //Sabemos que cada 8 caracteres están definidos los datos D1, D2, D3, etc de cada equipo
                    Double mariluz = numcaracteres / 8;

                    //Problema cuando 0,X es > que 0,5 redondea hacia arriba

                    Int16 mariluz1 =(Int16)(numcaracteres/8);

                    Double partedecimal1 = mariluz - mariluz1;

                    Double final = 8 * partedecimal1;

                    Int16 final1 = Convert.ToInt16(final);

                    Double resto = numcaracteres - (8 * partedecimal1);

                    String result1 = "";

                    Int32 h;

                    h = 0;

                    do
                    {
                        if (h < (Int32)resto)
                        {
                            //Leemos el Parámetro D[0]=D1
                            if (h == 0)
                            {   
                                result1 = temp.Substring((0 + h), 8);

                                //if (result1=="")
                                //{
                                //    MessageBox.Show("Error in D1 parameter in Equipment Number: "+Convert.ToString(numequipo));
                                //}

                                int ee = 0;
                                int ed = 0;

                                ee = result1.IndexOf("e-");
                                ed = result1.IndexOf("e+");

                                if ((ee > 0) || (ed > 0))
                                {
                                    D[numd] = Convert.ToDouble(result1);
                                    numd = 1;
                                }

                                else
                                {

                                    char[] charsToTrim = { '*', ' ', '\'' };
                                    string result2 = result1.Trim(charsToTrim);
                                    //MessageBox.Show("D" + Convert.ToString(numd) + ":" + result2);
                                    if (result2 == "")
                                    {
                                        D[0] = 0;
                                    }
                                    else
                                    {
                                        int menos = result2.IndexOf('-');
                                        int mas = result2.IndexOf('+');

                                        if ((menos > 1)||(mas>1))
                                        {
                                            result2 = result2.Replace("+", "E+").Replace(@"-", "E-");
                                            D[0] = Convert.ToDouble(result2);
                                        }

                                        else
                                        {
                                            D[0] = Convert.ToDouble(result2);
                                        }
                                    }
                                    numd = 1;
                                }
                            }

                            //Leemos el Parámetro D[1]=D2
                            else if (h == 8)
                            {
                                result1 = temp.Substring((0 + h), 8);
                                int ee = 0;
                                int ed = 0;

                                ee = result1.IndexOf("e-");
                                ed = result1.IndexOf("e+");

                                if ((ee > 0) || (ed > 0))
                                {
                                    D[numd] = Convert.ToDouble(result1);
                                    numd = 2;
                                }

                                else
                                {
                                    char[] charsToTrim = { '*', ' ', '\'' };
                                    string result2 = result1.Trim(charsToTrim);
                                    //MessageBox.Show("D" + Convert.ToString(numd) + ":" + result2);
                                    if (result2 == "")
                                    {
                                        D[1] = 0;
                                    }
                                    else
                                    {

                                        int menos = result2.IndexOf('-');
                                        int mas = result2.IndexOf('+');
                                        
                                        if ((menos > 1)||(mas>1))
                                        {
                                            result2 = result2.Replace("+", "E+").Replace(@"-", "E-");
                                            D[1] = Convert.ToDouble(result2);
                                        }

                                        else
                                        {
                                            D[1] = Convert.ToDouble(result2);
                                        }

                                    }

                                    numd = 2;
                                }
                            }

                            //Leemos el Parámetro D[2]=D3
                            else if (h == 16)
                            {
                                 result1 = temp.Substring((0 + h),8);
                                 int ee = 0;
                                 int ed = 0;

                                 ee = result1.IndexOf("e-");
                                 ed = result1.IndexOf("e+");

                                 if ((ee > 0) || (ed > 0))
                                 {
                                     D[numd] = Convert.ToDouble(result1);
                                     numd = 3;
                                 }

                                 else
                                 {

                                     char[] charsToTrim = { '*', ' ', '\'' };
                                     string result2 = result1.Trim(charsToTrim);

                                     //MessageBox.Show("D" + Convert.ToString(numd) + ":" + result2);

                                     if (result2 == "")
                                     {
                                         D[2] = 0;
                                     }

                                     else
                                     {
                                         int menos = result2.IndexOf('-');
                                         int mas = result2.IndexOf('+');

                                         if ((menos > 1)||(mas>1))
                                         {
                                             result2 = result2.Replace("+", "E+").Replace(@"-", "E-");
                                             D[2] = Convert.ToDouble(result2);
                                         }

                                         else
                                         {

                                             D[2] = Convert.ToDouble(result2);
                                         }
                                     }

                                     numd = 3;
                                   }

                            }

                            //Leemos el Parámetro D[3]=D4
                            else if (h == 24)
                            {
                                 result1 = temp.Substring((0 + h),8);
                                 int ee = 0;
                                 int ed = 0;

                                 ee = result1.IndexOf("e-");
                                 ed = result1.IndexOf("e+");

                                 if ((ee > 0) || (ed > 0))
                                 {
                                     D[numd] = Convert.ToDouble(result1);
                                     numd = 4;
                                 }

                                 else
                                 {

                                     char[] charsToTrim = { '*', ' ', '\'' };
                                     string result2 = result1.Trim(charsToTrim);
                                     //MessageBox.Show("D" + Convert.ToString(numd) + ":" + result2);
                                     if (result2 == "")
                                     {
                                         D[3] = 0;
                                     }
                                     else
                                     {
                                         int menos = result2.IndexOf('-');
                                         int mas = result2.IndexOf('+');

                                         if ((menos > 1) || (mas > 1))
                                         {
                                             result2 = result2.Replace("+", "E+").Replace(@"-", "E-");
                                             D[3] = Convert.ToDouble(result2);
                                         }

                                         else
                                         {
                                             D[3] = Convert.ToDouble(result2);
                                         }

                                     }
                                     numd = 4;

                                  }
                            }

                            //Leemos el Parámetro D[4]=D5
                            else if (h == 32)
                            {

                                 result1 = temp.Substring((0 + h),8);
                                 int ee = 0;
                                 int ed = 0;

                                 ee = result1.IndexOf("e-");
                                 ed = result1.IndexOf("e+");

                                 if ((ee > 0) || (ed > 0))
                                 {
                                     D[numd] = Convert.ToDouble(result1);
                                     numd = 5;
                                 }

                                 else
                                 {

                                     char[] charsToTrim = { '*', ' ', '\'' };
                                     string result2 = result1.Trim(charsToTrim);
                                     //MessageBox.Show("D" + Convert.ToString(numd) + ":" + result2);
                                     if (result2 == "")
                                     {
                                         D[4] = 0;
                                     }
                                     else
                                     {
                                         int menos = result2.IndexOf('-');
                                         int mas = result2.IndexOf('+');

                                         if ((menos > 1) || (mas > 1))
                                         {
                                             result2 = result2.Replace("+", "E+").Replace(@"-", "E-");
                                             D[4] = Convert.ToDouble(result2);
                                         }

                                         else
                                         {
                                             D[4] = Convert.ToDouble(result2);
                                         }

                                     }
                                     numd = 5;

                                 }
                            }

                            //Leemos el Parámetro D[5]=D6
                            else if (h == 40)
                            {
                                 result1 = temp.Substring((0 + h),8);
                                 int ee = 0;
                                 int ed = 0;

                                 ee = result1.IndexOf("e-");
                                 ed = result1.IndexOf("e+");

                                 if ((ee > 0) || (ed > 0))
                                 {
                                     D[numd] = Convert.ToDouble(result1);
                                     numd = 6;
                                 }

                                 else
                                 {

                                     char[] charsToTrim = { '*', ' ', '\'' };
                                     string result2 = result1.Trim(charsToTrim);
                                     //MessageBox.Show("D" + Convert.ToString(numd) + ":" + result2);
                                     if (result2 == "")
                                     {
                                         D[5] = 0;
                                     }
                                     else
                                     {
                                         int menos = result2.IndexOf('-');
                                         int mas = result2.IndexOf('+');

                                         if ((menos > 1)||(mas>1))
                                         {
                                             result2 = result2.Replace("+", "E+").Replace(@"-", "E-");
                                             D[5] = Convert.ToDouble(result2);
                                         }

                                         else
                                         {
                                             D[5] = Convert.ToDouble(result2);
                                         }

                                     }
                                     numd = 6;

                                 }
                            }

                            //Leemos el Parámetro D[6]=D7
                            else if (h == 48)
                            {

                                 result1 = temp.Substring((0 + h),8);
                                 int ee = 0;
                                 int ed = 0;

                                 ee = result1.IndexOf("e-");
                                 ed = result1.IndexOf("e+");

                                 if ((ee > 0) || (ed > 0))
                                 {
                                     D[numd] = Convert.ToDouble(result1);
                                     numd = 7;
                                 }

                                 else
                                 {

                                     char[] charsToTrim = { '*', ' ', '\'' };
                                     string result2 = result1.Trim(charsToTrim);
                                     //MessageBox.Show("D" + Convert.ToString(numd) + ":" + result2);
                                     if (result2 == "")
                                     {
                                         D[6] = 0;
                                     }
                                     else
                                     {

                                         int menos = result2.IndexOf('-');

                                         if (menos > 1)
                                         {
                                             result2 = result2.Replace("+", "E+").Replace(@"-", "E-");
                                             D[6] = Convert.ToDouble(result2);
                                         }

                                         else
                                         {
                                             D[6] = Convert.ToDouble(result2);
                                         }

                                     }
                                     numd = 7;

                                 }
                            }

                            //Leemos el Parámetro D[7]=D8
                            else if (h == 56)
                            {
                                 result1 = temp.Substring((0 + h),8);
                                 int ee = 0;
                                 int ed = 0;

                                 ee = result1.IndexOf("e-");
                                 ed = result1.IndexOf("e+");

                                 if ((ee > 0) || (ed > 0))
                                 {
                                     D[numd] = Convert.ToDouble(result1);
                                     numd = 8;
                                 }

                                 else
                                 {
                                     char[] charsToTrim = { '*', ' ', '\'' };
                                     string result2 = result1.Trim(charsToTrim);
                                     //MessageBox.Show("D" + Convert.ToString(numd) + ":" + result2);
                                     if (result2 == "")
                                     {
                                         D[7] = 0;
                                     }
                                     else
                                     {

                                         int menos = result2.IndexOf('-');

                                         if (menos > 1)
                                         {
                                             result2 = result2.Replace("+", "E+").Replace(@"-", "E-");
                                             D[7] = Convert.ToDouble(result2);
                                         }

                                         else
                                         {
                                             D[7] = Convert.ToDouble(result2);
                                         }

                                     }
                                     numd = 8;

                                 }
                            }

                            //Leemos el Parámetro D[8]=D9
                            else if (h == 64)
                            {

                                 result1 = temp.Substring((0 + h),8);
                                 int ee = 0;
                                 int ed = 0;

                                 ee = result1.IndexOf("e-");
                                 ed = result1.IndexOf("e+");

                                 if ((ee > 0) || (ed > 0))
                                 {
                                     D[numd] = Convert.ToDouble(result1);
                                 }

                                 else
                                 {

                                     char[] charsToTrim = { '*', ' ', '\'' };
                                     string result2 = result1.Trim(charsToTrim);
                                     //MessageBox.Show("D" + Convert.ToString(numd) + ":" + result2);
                                     if (result2 == "")
                                     {
                                         D[8] = 0;
                                     }
                                     else
                                     {
                                         int menos = result2.IndexOf('-');

                                         if (menos > 1)
                                         {
                                             result2 = result2.Replace("+", "E+").Replace(@"-", "E-");
                                             D[8] = Convert.ToDouble(result2);
                                         }

                                         else
                                         {
                                             D[8] = Convert.ToDouble(result2);
                                         }

                                     }
                                

                                 }
                            }

                            else
                            {
                              
                            }
                        }

                        //Comprabión de que se ha escrito con la notación exponencia e- o e+
                        else if (h >= (Int32)resto)
                        {
                            result1 = temp.Substring((0 + h), final1);
                            int ee = 0;
                            int ed = 0;

                            ee = result1.IndexOf("e-");
                            ed = result1.IndexOf("e+");

                            if ((ee > 0) || (ed > 0))
                            {
                                D[numd] = Convert.ToDouble(result1);
                            }

                            else
                            {

                            char[] charsToTrim = {'*',' ','\''};
                            string result2 = result1.Trim(charsToTrim);
                            //MessageBox.Show("D" + Convert.ToString(numd) + ":" + result2);

                            if (result2 == "")
                            {
                                D[numd] = 0;
                            }
                            else
                            {
                                int menos = result2.IndexOf('-');
                                int mas = result2.IndexOf('+');

                                if ((menos > 1)||(mas>1))
                                {
                                    result2 = result2.Replace("+", "E+").Replace(@"-", "E-");
                                    D[numd] = Convert.ToDouble(result2);
                                }

                                else
                                {
                                    D[numd] = Convert.ToDouble(result2);
                                }
                             }

                             }

                        }

                        else
                        {

                        }
                        
                        //Contador de 8 caracteres. Cada uno de los datos D1, D2, D3, D4 etc viene definido por 8 caracteres.
                        h = h + 8;

                    } while (h < (Int32)numcaracteres);
                    

                    //Llamada a los Forms de los equipos para crear los objetos tipo Equipos en la aplicación principal pertencientes a la lista equipos11
                    //Equipo Tipo 1 Condición de Contorno
                    if (tipoequipo == 1)
                    {
                        //Creamos un objeto "con1" de la Clase Condcontorno (Form para adquisición de Datos de los equipos Tipo 1 Condición de Contorno) 
                        //Enviamos al constructor de la Clase Condcontorno el puntero (puntero1) a la Aplicación Principal, el número de ecuaciones y el número de variables de la Aplicación Principal
                        Condcontorno con1 = new Condcontorno(puntero1,puntero1.numecuaciones,puntero1.numvariables,0,0);
                        
                        //CASO DE CONDICIÓN DE CONTORNO COMO EQUIPO PRIMERO SIN NINGUN EQUIPO CONECTADO A EL CREA DOS CORRIENTES
                        if (N2!=1)
                        {
                        con1.numequipo = numequipo;
                        con1.D1 = D[0];
                        con1.D2 = D[1];
                        con1.D3 = D[2];
                        con1.D5 = D[4];
                        con1.D6 = D[5];
                        con1.D7 = D[6];
                        con1.correntrada = N1;
                        con1.corrsalida = N3;
                        con1.funcionauxiliar();
                        con1.funcionauxiliar1();

                        listacorrsalida[corrientesdesalida]=N1;
                        corrientesdesalida++;
                        listacorrsalida[corrientesdesalida] = N3;
                        corrientesdesalida++;
                        }

                        //CASO DE CREACCION DE SOLO UNA CORRIENTE Y NO DOS, CUANDO EL EQUIPO NO ES EL PRIMERO Y ESTÁ CONECTADO A OTRO ANTERIOR
                        else if(N2==1)
                        {
                        con1.numequipo = numequipo;
                        con1.D1 = D[0];
                        con1.D2 = D[1];
                        con1.D3 = D[2];
                        con1.D5 = D[4];
                        con1.D6 = D[5];
                        con1.D7 = D[6];
                        con1.correntrada = N1;
                        con1.parametroauxiliar = 1;
                        con1.corrsalida = N3;
                        con1.checkBox1.Checked = true;
                        con1.funcionauxiliar();
                        con1.funcionauxiliar1();

                        listacorrsalida[corrientesdesalida] = N3;
                        corrientesdesalida++;
                        }
                    }

                    //Creamos las ecuaciones correspondientes al Divisor (Equipo Tipo 2)
                    else if (tipoequipo == 2)
                    {
                        Divisor div1 = new Divisor(puntero1, puntero1.numecuaciones, puntero1.numvariables,0,0);
                        div1.numequipo = numequipo;
                        div1.D1 = D[0];
                        div1.D2 = D[1];
                        div1.D3 = D[2];

                        div1.correntrada = N1;
                        div1.corrsalida1 = N3;
                        div1.corrsalida2 = N4;
                        div1.funcionauxiliar();
                        div1.funcionauxiliar1();

                        listacorrsalida[corrientesdesalida] = N3;
                        corrientesdesalida++;
                        listacorrsalida[corrientesdesalida] = N4;
                        corrientesdesalida++;
                    }

                    //Creamos las ecuaciones correspondientes a la Pérdida de Carga (Equipo Tipo 3)
                    else if (tipoequipo == 3)
                    {
                        Perdidacarga per1 = new Perdidacarga(puntero1, puntero1.numecuaciones, puntero1.numvariables,0,0);
                        per1.numequipo = numequipo;
                        per1.D1 = D[0];
                        per1.D2 = D[1];
                        per1.D3 = D[2];
                        per1.D4 = D[3];
                        per1.D5 = D[4];
                        per1.D6 = D[5];
                        per1.D7 = D[6];
                        //El valor D8 define el número de tuberías en paralelo que define el equipo pérdida de carga
                        if (D[7] == 0)
                        {
                            per1.D8 = 1;
                        }
                        else
                        {
                            per1.D8 = D[7];
                        }
                        per1.correntrada = N1;
                        per1.corrsalida = N3;
                        per1.funcionauxiliar();
                        per1.funcionauxiliar1();

                        listacorrsalida[corrientesdesalida] = N3;
                        corrientesdesalida++;
                    }

                    //Creamos las ecuaciones correspondientes al equipo BOMBA (Equipo Tipo 4)
                    else if (tipoequipo == 4)
                    {
                        Bomba per1 = new Bomba(puntero1, puntero1.numecuaciones, puntero1.numvariables,0,0);
                        per1.numequipo = numequipo;
                        per1.D1 = D[0];
                        per1.D2 = D[1];
                        per1.D3 = D[2];
                        per1.D4 = D[3];
                        per1.D5 = D[4];
                        per1.D7 = D[6];
                        per1.D8 = D[7];
                        per1.D9 = D[8];
                       
                        per1.correntrada = N1;
                        per1.corrsalida = N3;
                        per1.funcionauxiliar();
                        per1.funcionauxiliar1();

                        listacorrsalida[corrientesdesalida] = N3;
                        corrientesdesalida++;
                    }

                    //Creamos las ecuaciones correspondientes al Mezclador (Equipo Tipo 5)
                    else if (tipoequipo == 5)
                    {
                        Mezclador mezclador1 = new Mezclador(puntero1, puntero1.numecuaciones, puntero1.numvariables,0,0);
                        mezclador1.numequipo = numequipo;
                        mezclador1.D1 = D[0];
                        mezclador1.D9 = D[8];
                        mezclador1.correntrada1 = N1;
                        mezclador1.correntrada2 = N2;
                        mezclador1.corrsalida = N3;
                        mezclador1.funcionauxiliar();
                        mezclador1.funcionauxiliar1();

                        listacorrsalida[corrientesdesalida] = N3;
                        corrientesdesalida++;
                    }

                    //Creamos las ecuaciones correspondientes al Reactor (Equipo Tipo 6)
                    else if (tipoequipo == 6)
                    {
                        Reactor reactor1 = new Reactor(puntero1, puntero1.numecuaciones, puntero1.numvariables,0,0);
                        reactor1.numequipo = numequipo;
                        reactor1.D1 = D[0];
                        reactor1.D2 = D[1];
                        reactor1.D3 = D[2];
                        reactor1.D4 = D[3];
                        reactor1.D5 = D[4];
                        reactor1.D6 = D[5];
                        reactor1.D7 = D[6];
                        reactor1.D9 = D[8];
                        reactor1.correntrada = N1;
                        reactor1.corrsalida = N3;
                        reactor1.funcionauxiliar();
                        reactor1.funcionauxiliar1();

                        listacorrsalida[corrientesdesalida] = N3;
                        corrientesdesalida++;
                    }

                    //Creamos las ecuaciones correspondientes al Calentador (Equipo Tipo 7)
                    else if (tipoequipo == 7)
                    {
                        Calentador calentador1 = new Calentador(puntero1, puntero1.numecuaciones, puntero1.numvariables,0,0);
                        calentador1.numequipo = numequipo;
                        calentador1.D1 = D[0];
                        calentador1.D2 = D[1];
                        calentador1.D3 = D[2];
                        calentador1.D4 = D[3];
                        calentador1.D5 = D[4];
                        calentador1.D6 = D[5];
                        calentador1.D7 = D[6];
                        if (D[7] == 0)
                        {
                            calentador1.D8 = 1;
                        }
                        else
                        {
                            calentador1.D8 = D[7];
                        }
                        calentador1.D9 = D[8];
                        calentador1.correntrada1 = N1;
                        calentador1.correntrada2 = N2;
                        calentador1.correntrada3 = calentador1.D9;
                        calentador1.corrsalida1 =  N3;
                        calentador1.corrsalida2 = N4;
                        calentador1.funcionauxiliar();
                        calentador1.funcionauxiliar1();

                        listacorrsalida[corrientesdesalida] = N3;
                        corrientesdesalida++;
                        listacorrsalida[corrientesdesalida] = N4;
                        corrientesdesalida++;
                    }

                     //Creamos las ecuaciones correspondientes al Condensador Principal (Equipo Tipo 8)
                    else if (tipoequipo == 8)
                    {
                        Condensadorprincipal condprin1 = new Condensadorprincipal(puntero1, puntero1.numecuaciones, puntero1.numvariables);
                        
                        //No se aplica la HEI sólo el cálculo del calor rechazado
                        if (condprin1.D9==0)
                        {
                            condprin1.numequipo = numequipo;
                            condprin1.D1=D[0];
                            condprin1.D2 = D[1];
                            condprin1.D3 = D[2];
                            condprin1.correntrada1 = N1;
                            condprin1.correntrada2 = N2;
                            condprin1.corrsalida = N3;
                            condprin1.funcionauxiliar();
                            condprin1.funcionauxiliar1();

                            listacorrsalida[corrientesdesalida] = N3;
                            corrientesdesalida++;
                        }

                        //Los cálculos se realizan con base a la HEI
                        else if (condprin1.D9 != 0)
                        {
                            condprin1.numequipo = numequipo;
                            condprin1.D1 = D[0];
                            condprin1.D2 = D[1];
                            condprin1.D3 = D[2];
                            condprin1.D4 = D[3];
                            condprin1.D5 = D[4];
                            condprin1.D6 = D[5];
                            condprin1.D7 = D[6];
                            condprin1.D8 = D[7];
                            condprin1.D9 = D[8];
                            condprin1.correntrada1 = N1;
                            condprin1.correntrada2 = N2;
                            condprin1.corrsalida = N3;
                            condprin1.funcionauxiliar();
                            condprin1.funcionauxiliar1();

                            listacorrsalida[corrientesdesalida] = N3;
                            corrientesdesalida++;
                        }
                    }

                    //Creamos las ecuaciones correspondientes a la Turbina (Equipo Tipo 9)
                    else if (tipoequipo == 9)
                    {
                        Turbina turbina1 = new Turbina(puntero1, puntero1.numecuaciones, puntero1.numvariables,0,0);
                        turbina1.numequipo = numequipo;
                        turbina1.D1 = D[0];
                        turbina1.D3 = D[2];
                        turbina1.D4 = D[3];
                        turbina1.D5 = D[4];
                        turbina1.D8 = D[7];
                        turbina1.D9 = D[8];
                        turbina1.correntrada = N1;
                        turbina1.corrsalida = N3;
                        turbina1.funcionauxiliar();
                        turbina1.funcionauxiliar1();

                        listacorrsalida[corrientesdesalida] = N3;
                        corrientesdesalida++;
                    }

                    //Creamos las ecuaciones correspondientes a la Turbina (Equipo Tipo 10)
                    else if (tipoequipo == 10)
                    {
                        Turbina10 turbina10 = new Turbina10(puntero1, puntero1.numecuaciones, puntero1.numvariables,0,0);
                        turbina10.numequipo = numequipo;
                        turbina10.D1 = D[0];
                        turbina10.D2 = D[1];
                        turbina10.D3 = D[2];
                        turbina10.D5 = D[4];
                        turbina10.D8 = D[7];
                        turbina10.correntrada = N1;
                        turbina10.corrsalida = N3;
                        turbina10.funcionauxiliar();
                        turbina10.funcionauxiliar1();

                        listacorrsalida[corrientesdesalida] = N3;
                        corrientesdesalida++;
                    }

                    //Creamos las ecuaciones correspondientes a la Turbina Auxiliar (Equipo Tipo 11)
                    else if (tipoequipo == 11)
                    {
                        TurbinaAuxiliar turbinaAuxiliar1 = new TurbinaAuxiliar(puntero1, puntero1.numecuaciones, puntero1.numvariables,0,0);
                        turbinaAuxiliar1.numequipo = numequipo;
                        turbinaAuxiliar1.D1 = D[0];
                        turbinaAuxiliar1.D2 = D[1];
                        turbinaAuxiliar1.D5 = D[4];
                        turbinaAuxiliar1.correntrada = N1;
                        turbinaAuxiliar1.corrsalida = N3;
                        turbinaAuxiliar1.funcionauxiliar();
                        turbinaAuxiliar1.funcionauxiliar1();

                        listacorrsalida[corrientesdesalida] = N3;
                        corrientesdesalida++;
                    }

                    //Creamos las ecuaciones correspondientes al Separador de Humedad (Equipo Tipo 13)
                    else if (tipoequipo == 13)
                    {
                        Sephumedad sephumedad1 = new Sephumedad(puntero1, puntero1.numecuaciones, puntero1.numvariables,0,0);
                        sephumedad1.numequipo = numequipo;
                        sephumedad1.D1 = D[0];
                        sephumedad1.D2 = D[1];
                        sephumedad1.correntrada = N1;
                        sephumedad1.corrsalida1 = N3;
                        sephumedad1.corrsalida2 = N4;
                        sephumedad1.funcionauxiliar();
                        sephumedad1.funcionauxiliar1();

                        listacorrsalida[corrientesdesalida] = N3;
                        corrientesdesalida++;
                        listacorrsalida[corrientesdesalida] = N4;
                        corrientesdesalida++;
                    }

                    //Creamos las ecuaciones correspondientes al MSR (Equipo Tipo 14)
                    else if (tipoequipo == 14)
                    {
                        MSR msr1 = new MSR(puntero1, puntero1.numecuaciones, puntero1.numvariables,0,0);
                        msr1.numequipo = numequipo;
                        msr1.D1 = D[0];
                        msr1.D2 = D[1];
                        msr1.D3 = D[2];
                        msr1.D4 = D[3];
                        msr1.D5 = D[4];
                        msr1.D6 = D[5];
                        msr1.D7 = D[6];
                        msr1.D8 = D[7];
                        msr1.D9 = D[8];
                        msr1.correntrada1 = N1;
                        msr1.correntrada2 = N2;
                        msr1.corrsalida1 = N3;
                        msr1.corrsalida2 = N4;
                        msr1.funcionauxiliar();
                        msr1.funcionauxiliar1();

                        listacorrsalida[corrientesdesalida] = N3;
                        corrientesdesalida++;
                        listacorrsalida[corrientesdesalida] = N4;
                        corrientesdesalida++;
                    }

                    //Creamos las ecuaciones correspondientes al Condensador (Equipo Tipo 15)
                    else if (tipoequipo == 15)
                    {
                        Condensador condensador1 = new Condensador(puntero1, puntero1.numecuaciones, puntero1.numvariables,0,0);
                        condensador1.numequipo = numequipo;
                        condensador1.D1 = D[0];
                        condensador1.D2 = D[1];
                        condensador1.D3 = D[2];
                        condensador1.D5 = D[4];
                        condensador1.D6 = D[5];
                        condensador1.D7 = D[6];
                        if (D[7] == 0)
                        {
                            condensador1.D8 = 1;
                        }
                        else
                        {
                            condensador1.D8 = D[7];
                        }
                        condensador1.correntrada1 = N1;
                        condensador1.correntrada2 = N2;
                        condensador1.corrsalida1 = N3;
                        condensador1.corrsalida2 = N4;
                        condensador1.funcionauxiliar();
                        condensador1.funcionauxiliar1();

                        listacorrsalida[corrientesdesalida] = N3;
                        corrientesdesalida++;
                        listacorrsalida[corrientesdesalida] = N4;
                        corrientesdesalida++;
                    }

                    //Creamos las ecuaciones correspondientes al Enfriador de Drenajes (Equipo Tipo 16)
                    else if (tipoequipo == 16)
                    {
                        EnfriadorDrenajes enfriador1 = new EnfriadorDrenajes(puntero1, puntero1.numecuaciones, puntero1.numvariables,0,0);
                        enfriador1.numequipo = numequipo;
                        enfriador1.D1 = D[0];
                        enfriador1.D2 = D[1];
                        enfriador1.D3 = D[2];
                        enfriador1.D4 = D[3];                        
                        enfriador1.D6 = D[5];                        
                        enfriador1.D8 = D[7];                        

                        enfriador1.correntrada1 = N1;
                        enfriador1.correntrada2 = N2;
                        enfriador1.corrsalida1 = N3;
                        enfriador1.corrsalida2 = N4;

                        enfriador1.funcionauxiliar();
                        enfriador1.funcionauxiliar1();

                        listacorrsalida[corrientesdesalida] = N3;
                        corrientesdesalida++;
                        listacorrsalida[corrientesdesalida] = N4;
                        corrientesdesalida++;
                    }

                    //Creamos las ecuaciones correspondientes al Desaireador(Equipo Tipo 18)
                    else if (tipoequipo == 18)
                    {
                        Desaireador des1 = new Desaireador(puntero1, puntero1.numecuaciones, puntero1.numvariables,0,0);
                        des1.numequipo = numequipo;
                        des1.D5 = D[4];
                        des1.D6 = D[5];
                        des1.D7 = D[6];
                        des1.D9 = D[8];
                        des1.correntrada1 = N1;
                        des1.correntrada2 = N2;
                        des1.corrsalida1 = N3;
                        des1.corrsalida2 = N4;
                        des1.funcionauxiliar();
                        des1.funcionauxiliar1();

                        listacorrsalida[corrientesdesalida] = N3;
                        corrientesdesalida++;
                        listacorrsalida[corrientesdesalida] = N4;
                        corrientesdesalida++;
                    }

                    //Creamos las ecuaciones correspondientes a la Válvula (Equipo Tipo 19)
                    else if (tipoequipo == 19)
                    {
                        Valvula valvula1 = new Valvula(puntero1, puntero1.numecuaciones, puntero1.numvariables,0,0);
                        valvula1.numequipo = numequipo;
                        valvula1.D1 = D[0];
                        valvula1.D2 = D[1];
                        valvula1.D3 = D[2];
                        valvula1.D5 = D[4];
                        valvula1.D6 = D[5];
                        valvula1.D7 = D[6];
                        valvula1.D8 = D[7];
                        valvula1.D9 = D[8];
                        valvula1.correntrada = N1;
                        valvula1.corrsalida = N3;
                        valvula1.funcionauxiliar();
                        valvula1.funcionauxiliar1();

                        listacorrsalida[corrientesdesalida] = N3;
                        corrientesdesalida++;
                    }

                    //Creamos las ecuaciones correspondientes al Divisor de Entalpía Fija (Equipo Tipo 20)
                    else if (tipoequipo == 20)
                    {
                        Divisorentalpiafija divental1 = new Divisorentalpiafija(puntero1, puntero1.numecuaciones, puntero1.numvariables,0,0);
                        divental1.numequipo = numequipo;
                        divental1.D1 = D[0];
                        divental1.D2 = D[1];
                        divental1.D3 = D[2];
                        divental1.correntrada = N1;
                        divental1.corrsalida1 = N3;
                        divental1.corrsalida2 = N4;
                        divental1.funcionauxiliar();
                        divental1.funcionauxiliar1();

                        listacorrsalida[corrientesdesalida] = N3;
                        corrientesdesalida++;
                        listacorrsalida[corrientesdesalida] = N4;
                        corrientesdesalida++;
                    }

                    //Creamos las ecuaciones correspondientes al Tanque de Evaporización Instantanea (Equipo Tipo 21)
                    else if (tipoequipo == 21)
                    {
                        TanqueVaporizacion tanquevap1 = new TanqueVaporizacion(puntero1, puntero1.numecuaciones, puntero1.numvariables,0,0);
                        tanquevap1.numequipo = numequipo;
                        tanquevap1.D1 = D[0];
                        tanquevap1.correntrada = N1;
                        tanquevap1.corrsalida1 = N3;
                        tanquevap1.corrsalida2 = N4;
                        tanquevap1.funcionauxiliar();
                        tanquevap1.funcionauxiliar1();

                        listacorrsalida[corrientesdesalida] = N3;
                        corrientesdesalida++;
                        listacorrsalida[corrientesdesalida] = N4;
                        corrientesdesalida++;
                    }

                    //Creamos las ecuaciones correspondientes al Intercambiador (Equipo Tipo 22)
                    else if (tipoequipo == 22)
                    {
                        Intercambiador Intercambiador1 = new Intercambiador(puntero1, puntero1.numecuaciones, puntero1.numvariables,0,0);
                        Intercambiador1.numequipo = numequipo;
                        Intercambiador1.D1 = D[0];
                        Intercambiador1.D2 = D[1];
                        Intercambiador1.D3 = D[2];
                        Intercambiador1.D4 = D[3];
                        Intercambiador1.D5 = D[4];
                        Intercambiador1.D6 = D[5];
                        Intercambiador1.D7 = D[6];
                        Intercambiador1.D8 = D[7];
                        Intercambiador1.D9 = D[8];

                        Intercambiador1.correntrada1 = N1;
                        Intercambiador1.correntrada2 = N2;
                        Intercambiador1.corrsalida1 = N3;
                        Intercambiador1.corrsalida2 = N4;

                        Intercambiador1.funcionauxiliar();
                        Intercambiador1.funcionauxiliar1();

                        listacorrsalida[corrientesdesalida] = N3;
                        corrientesdesalida++;
                        listacorrsalida[corrientesdesalida] = N4;
                        corrientesdesalida++;
                    }

                    //Añadimos un objeto PictureBox del Equipo leido desde el Archivo de Entrada de Datos desde HBAL
                    //puntero1.equipos.Add(new PictureBox());


                    //Contador del Número de Equipos de la Aplicación Principal
                    puntero1.numequipos++;


                    //Enviamos el fichero leido de Hbal a la Aplicacion Principal
                    puntero1.Hbalfile.Add("");
                    //Añadimos la linea PRIMERA donde se incluye el Título del Archivo
                    puntero1.Hbalfile[0]=lineas[0];
                    puntero1.Hbalfile.Add("");
                    //Añadimos la linea SEGUNDA donde se incluye la informacin:NºEquipos, NºCorrientes, Error Máximo, Faja,etc.
                    puntero1.Hbalfile[1]=lineas[1];
                    //Añadimos el RESTO de LINEAS a donde se incluye la informacin:Nº Equipos, corrientes y D1 a D9.
                    for (int b = 2; b <= (int)(2*numeroequipos); b++)
                    {
                        puntero1.Hbalfile.Add("");
                        puntero1.Hbalfile[b] = lineas[b];
                    }

                    //Double prueba = equipos[0].aD1;
                    // FINAL del FOR del numero equipos
                    D[0] = 0;
                    D[1] = 0;
                    D[2] = 0;
                    D[3] = 0;
                    D[4] = 0;
                    D[5] = 0;
                    D[6] = 0;
                    D[7] = 0;
                    D[8] = 0;
                }
              

               //Ahora procesamos las TABLAS y las CONDICIONES INICIALES


                //Leemos las TABLAS.
                // Si la variable "numtablas" indica que el número de Tablas es mayor que cero leemos las TABLAS 
                if (numtablas != 0)
                {
                    //Si los datos iniciales no son buenos NO leemos las CONDICIONES INICIALES
                    if (datosinciales == 0)
                    {
                            //Variables locales para guardar los datos de las TABLAS leidos desde el archivo
                            //
                            String luis110 = "";
                            String luis111 = "";
                            //Número de la Tabla
                            Double numtabla=0;
                            //Número de Datos x e y de la Tabla
                            Double numdatosx=0;
                            Double numdatosy=0;
                            //
                            int contadorlineas=2;
                            //Número de lineas de datos
                            int numlineasdatos=0;

                            
                            //Bucle para leer los datos de todas las TABLAS
                            for (int h = 0; h <numtablas; h++)
                            {
                                //Leemos el Títulos de la Tabla
                                titulostablas1.Add("");
                                titulostablas1[h] = lineas[(int)(2 * numeroequipos) + contadorlineas];
                                //Guardamos en las variables de la Aplicación Principal el Título de la Tabla, Título del EjeX y Título del Eje Y de la TABLA
                                puntero1.listaTituloTabla.Add(titulostablas1[h]);
                                puntero1.listaTituloEjeXTabla.Add("Valores Eje X");
                                puntero1.listaTituloEjeYTabla.Add("Valores Eje Y");
                                //Ponemos por defecto Interpolación Tipo Cúbica:3
                                puntero1.listanumTipoInterpolacionTabla.Add(3);

                                //Leemos los Datos Generales Tabla:Nº Tabla, Nº Datos
                                temp = lineas[(int)(2 * numeroequipos) + contadorlineas + 1];                              
                                for (int iw = 0; iw <= 15; iw = iw + 6)
                                {
                                    if (iw <= 6)
                                    {
                                        luis110 = temp.Substring(0 + iw, 6);
                                    }
                                    else if (iw > 6)
                                    {
                                        luis111 = temp.Substring(0 + iw, 3);
                                    }

                                    //Leemos el Número de Tabla
                                    if (iw == 0)
                                    {
                                        //MessageBox.Show("Equipo Número:" + luis1);
                                        numtabla = Convert.ToDouble(luis110);
                                        puntero1.listanumTablas.Add(numtabla);
                                    }

                                    //Leemos el Número de DatosX de la Tabla
                                    else if (iw == 6)
                                    {
                                        //MessageBox.Show("Tipo de Equipo:" + luis1);
                                        numdatosx = Convert.ToDouble(luis110);                                       
                                        puntero1.listanumDatosenTabla.Add(numdatosx);
                                    }


                                    //Leemos el Número de DatosY de la Tabla
                                    else if (iw == 12)
                                    {
                                        //MessageBox.Show("Tipo de Equipo:" + luis1);
                                        numdatosy = Convert.ToDouble(luis111);
                                    }
                                }
                                
                                if ((numdatosx / 8)<=1)
                                {
                                    numlineasdatos = 1;
                                }
                                else if ((1 < numdatosx / 8) && (numdatosx / 8 <= 2))
                                {
                                    numlineasdatos = 2;
                                }

                                else if ((2 < numdatosx / 8) && (numdatosx / 8 <= 3))
                                {
                                    numlineasdatos = 3;

                                }

                                else if ((3 < numdatosx / 8) && (numdatosx / 8 <= 4))
                                {
                                    numlineasdatos = 4;
                                }

                                else if ((4 < numdatosx / 8) && (numdatosx / 8 <= 5))
                                {
                                    numlineasdatos = 5;
                                }

                                else 
                                {
                                    MessageBox.Show("Error LUIS. El número de datos introducidos en la tabla supera los 40 datos permitidos y es de:"+Convert.ToDouble(numdatosx*8));
                                }

                                //Leemos los datos de la Tabla
                                //Bucle para leer las lineas de datos

                                String temporal="";
                                String temporal22 = "";
                                int longitud = 0;
                                int bucle = 0;
                                int conta = 0;

                                Double[,] datos = new Double[(int)numdatosx + 1, 2];
                                
                                for (int hu = 2; hu <numlineasdatos+2; hu++)
                                {
                                    temp = lineas[(int)(2 * numeroequipos) + contadorlineas + hu];
                                    longitud = temp.Length;
                                    bucle = longitud / 8;

                                    int ina = 0;

                                    //Bucle para leer los datos de cada linea, que están separados 8 espacios
                                    for (int ie = 0; ie <=bucle; ie++)
                                    {
                                        if (bucle < 1)
                                        {
                                            temporal = temp.Substring(0 + ina, longitud);
                                            datos[conta,0] = Convert.ToDouble(temporal);
                                        }

                                        else if (ina <8*bucle)
                                        {
                                            temporal = temp.Substring(0 + ina, 8);
                                            datos[conta,0] = Convert.ToDouble(temporal);
                                        }
                                        else if (ina>=8*bucle)
                                        {
                                            temporal22 = temp.Substring(0 + ina, longitud-(8*bucle));
                                            datos[conta,0] = Convert.ToDouble(temporal22);
                                        }
                                                                                
                                        conta++;
                                        ina = ina + 8;
                                    }
                                }

                                String temporal33 = "";
                                String temporal44 = "";
                                int longitud11 = 0;
                                int bucle1 = 0;
                                int conta1 = 0;

                                for (int hu1 = 2; hu1 < numlineasdatos+2; hu1++)
                                {
                                    temp = lineas[(int)(2 * numeroequipos) + contadorlineas + hu1 + numlineasdatos];
                                    longitud11 = temp.Length;
                                    bucle1 = longitud11 / 8;

                                    int ina = 0;

                                    //Bucle para leer los datos de cada linea, que están separados 8 espacios
                                    for (int ie = 0; ie <= bucle1; ie++)
                                    {
                                        if (bucle1 < 1)
                                        {
                                            temporal = temp.Substring(0 + ina, longitud11);
                                            datos[conta1,1] = Convert.ToDouble(temporal);
                                        }

                                        else if (ina < 8 * bucle1)
                                        {
                                            if (ina > 0)
                                            {
                                                temporal33 = temp.Substring(0 + ina, 8);
                                                datos[conta1,1] = Convert.ToDouble(temporal33);
                                            }

                                            else if ((ina == 0)&&(conta1==0))
                                            {
                                                datos[conta1,1] = 0;
                                            }

                                            else if ((ina == 0) && (conta1 != 0))
                                            {
                                                temporal33 = temp.Substring(0 + ina, 8);
                                                datos[conta1,1] = Convert.ToDouble(temporal33);
                                            }
                                        }
                                        else if (ina >= 8 * bucle1)
                                        {
                                            temporal44 = temp.Substring(0 + ina, longitud11 - (8 * bucle1));
                                            datos[conta1,1] = Convert.ToDouble(temporal44);
                                        }

                                        conta1++;
                                        ina = ina + 8;
                                    }
                                }

                                contadorlineas = contadorlineas + (2 * numlineasdatos) + 2;
                                puntero1.listaTablas.Add(datos);
                               
                            }
                        
                         }

                    //En este caso tenemos que leer las TABLAS y las CONDICIONES INICIALES
                    else if (datosinciales == 1)
                    {
                        
                        String luis110 = "";
                        String luis111 = "";
                        Double numtabla = 0;
                        Double numdatosx = 0;
                        Double numdatosy = 0;
                        int contadorlineas = 2;
                        int numlineasdatos = 0;

                        //Bucle para leer los datos de todas las TABLAS
                        for (int h = 0; h < numtablas; h++)
                        {
                            //Leemos el Títulos de la Tabla
                            titulostablas1.Add("");
                            titulostablas1[h] = lineas[(int)(2 * numeroequipos) + contadorlineas];
                            puntero1.listaTituloTabla.Add(titulostablas1[h]);
                            puntero1.listaTituloEjeXTabla.Add("Valores Eje X");
                            puntero1.listaTituloEjeYTabla.Add("Valores Eje Y");
                            //Ponemos por defecto Interpolación Tipo Cúbica:3
                            puntero1.listanumTipoInterpolacionTabla.Add(3);

                            //Leemos los Datos Generales Tabla:Nº Tabla, Nº Datos
                            temp = lineas[(int)(2 * numeroequipos) + contadorlineas + 1];
                            for (int iw = 0; iw <= 15; iw = iw + 6)
                            {
                                if (iw <= 6)
                                {
                                    luis110 = temp.Substring(0 + iw, 6);
                                }
                                else if (iw > 6)
                                {
                                    luis111 = temp.Substring(0 + iw, 3);
                                }

                                //Leemos el Número de Tabla
                                if (iw == 0)
                                {
                                    //MessageBox.Show("Equipo Número:" + luis1);
                                    numtabla = Convert.ToDouble(luis110);
                                    puntero1.listanumTablas.Add(numtabla);
                                }

                                //Leemos el Número de DatosX de la Tabla
                                else if (iw == 6)
                                {
                                    //MessageBox.Show("Tipo de Equipo:" + luis1);
                                    numdatosx = Convert.ToDouble(luis110);
                                    puntero1.listanumDatosenTabla.Add(numdatosx);
                                }


                                //Leemos el Número de DatosY de la Tabla
                                else if (iw == 12)
                                {
                                    //MessageBox.Show("Tipo de Equipo:" + luis1);
                                    numdatosy = Convert.ToDouble(luis111);
                                }
                            }

                            if ((numdatosx / 8) <= 1)
                            {
                                numlineasdatos = 1;
                            }
                            else if ((1 < numdatosx / 8) && (numdatosx / 8 <= 2))
                            {
                                numlineasdatos = 2;
                            }

                            else if ((2 < numdatosx / 8) && (numdatosx / 8 <= 3))
                            {
                                numlineasdatos = 3;

                            }

                            else if ((3 < numdatosx / 8) && (numdatosx / 8 <= 4))
                            {
                                numlineasdatos = 4;
                            }

                            else if ((4 < numdatosx / 8) && (numdatosx / 8 <= 5))
                            {
                                numlineasdatos = 5;
                            }

                            else
                            {
                                MessageBox.Show("Error LUIS. El número de datos introducidos en la tabla supera los 40 datos permitidos y es de:" + Convert.ToDouble(numdatosx * 8));
                            }

                            //Leemos los datos de la Tabla
                            //Bucle para leer las lineas de datos

                            String temporal = "";
                            String temporal22 = "";
                            int longitud = 0;
                            int bucle = 0;
                            int conta = 0;

                            Double[,] datos = new Double[(int)numdatosx + 1, 2];

                            for (int hu = 2; hu < numlineasdatos + 2; hu++)
                            {
                                temp = lineas[(int)(2 * numeroequipos) + contadorlineas + hu];
                                longitud = temp.Length;
                                bucle = longitud / 8;

                                int ina = 0;

                                //Bucle para leer los datos de cada linea, que están separados 8 espacios
                                for (int ie = 0; ie <= bucle; ie++)
                                {
                                    if (bucle < 1)
                                    {
                                        temporal = temp.Substring(0 + ina, longitud);
                                        datos[conta, 0] = Convert.ToDouble(temporal);
                                    }

                                    else if (ina < 8 * bucle)
                                    {
                                        temporal = temp.Substring(0 + ina, 8);
                                        datos[conta, 0] = Convert.ToDouble(temporal);
                                    }
                                    else if (ina >= 8 * bucle)
                                    {
                                        temporal22 = temp.Substring(0 + ina, longitud - (8 * bucle));
                                        datos[conta, 0] = Convert.ToDouble(temporal22);
                                    }

                                    conta++;
                                    ina = ina + 8;
                                }
                            }

                            String temporal33 = "";
                            String temporal44 = "";
                            int longitud11 = 0;
                            int bucle1 = 0;
                            int conta1 = 0;

                            for (int hu1 = 2; hu1 < numlineasdatos + 2; hu1++)
                            {
                                temp = lineas[(int)(2 * numeroequipos) + contadorlineas + hu1 + numlineasdatos];
                                longitud11 = temp.Length;
                                bucle1 = longitud11 / 8;

                                int ina = 0;

                                //Bucle para leer los datos de cada linea, que están separados 8 espacios
                                for (int ie = 0; ie <= bucle1; ie++)
                                {
                                    if (bucle1 < 1)
                                    {
                                        temporal = temp.Substring(0 + ina, longitud11);
                                        datos[conta1, 1] = Convert.ToDouble(temporal);
                                    }

                                    else if (ina < 8 * bucle1)
                                    {
                                        if (ina > 0)
                                        {
                                            temporal33 = temp.Substring(0 + ina, 8);
                                            datos[conta1, 1] = Convert.ToDouble(temporal33);
                                        }

                                        else if ((ina == 0) && (conta1 == 0))
                                        {
                                            datos[conta1, 1] = 0;
                                        }

                                        else if ((ina == 0) && (conta1 != 0))
                                        {
                                            temporal33 = temp.Substring(0 + ina, 8);
                                            datos[conta1, 1] = Convert.ToDouble(temporal33);
                                        }
                                    }
                                    else if (ina >= 8 * bucle1)
                                    {
                                        temporal44 = temp.Substring(0 + ina, longitud11 - (8 * bucle1));
                                        datos[conta1, 1] = Convert.ToDouble(temporal44);
                                    }

                                    conta1++;
                                    ina = ina + 8;
                                }
                            }

                            contadorlineas = contadorlineas + (2 * numlineasdatos) + 2;
                            puntero1.listaTablas.Add(datos);

                        }
                        
                        //Leemos las CONDICIONES INICIALES
                        if (datosinciales == 1)
                        {
                            //Para leer las Condiciones Iniciales

                            //Falta convertir las UNIDADES de las condiciones iniciales a las Unidades Británicas que es el Sistema de Unidades que utiliza las Tablas de Vapor ASME de 1967                   

                            String luis22 = "";
                            Double[] caudalinicial = new Double[(int)numcorrientes];
                            Double[] presioninicial = new Double[(int)numcorrientes];
                            Double[] entalpiainicial = new Double[(int)numcorrientes];
                            Double[] numcorrienteinicial = new Double[(int)numcorrientes];
                            int contador = 0;

                            //Bucle para contar el número de CONDICIONES INICIALES (número de corrientes de las que se incluyen sus condiciones iniciales)
                            int numerocondiciones = 0;
                            int contadorcondiciones = 0;

                            for (int n = (2 * (int)numeroequipos) + contadorlineas+1; n <= numlineasfichero; n++)
                            {
                                temp = lineas[n - 1];

                                if (temp == "")
                                {
                                    contadorcondiciones = numerocondiciones;
                                    goto luis;
                                }
                                numerocondiciones++;
                            }

                            contadorcondiciones = numerocondiciones;

                        luis:

                            //Bucle para leer las CONDICIONES INICIALES
                            //for (int n = (2 * (int)numeroequipos) + 3; n <= (int)numcorrientes + (2 * (int)numeroequipos) + 2; n++)
                            for (int n = (2 * (int)numeroequipos) + contadorlineas + 1; n <= contadorcondiciones + (2 * (int)numeroequipos) + contadorlineas; n++)
                            {
                                temp = lineas[n - 1];

                                for (int i = 0; i <= 50; i = i + 15)
                                {
                                    luis22 = temp.Substring(0 + i, 15);

                                    if (i == 0)
                                    {
                                        //Leemos el CAUDAL INICIAL
                                        if (unidades == 0)
                                        {
                                            //MessageBox.Show("Equipo Número:" + luis1);
                                            caudalinicial[contador] = Convert.ToDouble(luis22);
                                        }
                                        else if (unidades == 1)
                                        {
                                            //Pasamos de unidades del Sistema Métrico a unidades Británicas (de Kg/sg a Lb/sg)
                                            caudalinicial[contador] = Convert.ToDouble(luis22) / (0.4536);
                                        }

                                        else if (unidades == 2)
                                        {
                                            //Pasamos de unidades del Sistema Internacional a unidades Británicas (de Kgr/sg a Lb/sg)
                                            caudalinicial[contador] = Convert.ToDouble(luis22) / (0.4536);
                                        }
                                    }

                                    //LEEMOS LA PRESION INICIAL
                                    else if (i == 15)
                                    {
                                        if (unidades == 0)
                                        {
                                            //MessageBox.Show("Tipo de Equipo:" + luis1);
                                            presioninicial[contador] = Convert.ToDouble(luis22);
                                        }

                                        else if (unidades == 1)
                                        {
                                            //Pasamos de unidades del Sistema Internacional a unidades Británicas (de kPa a psia)
                                            presioninicial[contador] = Convert.ToDouble(luis22) / (6.8947572);
                                        }

                                        else if (unidades == 2)
                                        {
                                            //Pasamos de unidades del Sistema Internacional a unidades Británicas (de Bar a psia)
                                            presioninicial[contador] = Convert.ToDouble(luis22) / (6.8947572 / 100);
                                        }
                                    }

                                    // Leemos la ENTALPIA INICIAL
                                    else if (i == 30)
                                    {
                                        if (unidades == 0)
                                        {
                                            //MessageBox.Show("Corriente número 1:" + luis1);
                                            entalpiainicial[contador] = Convert.ToDouble(luis22);
                                        }

                                        else if (unidades == 1)
                                        {
                                            //Pasamos de unidades del Sistema Métrico al Británico (de kcal/Kgr a BTU/Lb)
                                            entalpiainicial[contador] = Convert.ToDouble(luis22) / 2.326009;
                                        }

                                        else if (unidades == 2)
                                        {
                                            //Pasamos de unidades del Sistema Internacional al Británico (de Kj/Kgr a BTU/Lb)
                                            entalpiainicial[contador] = Convert.ToDouble(luis22) / 2.326009;
                                        }
                                    }

                                    //Leemos el NÚMERO de CORRIENTES
                                    else if (i == 45)
                                    {
                                        //MessageBox.Show("Corriente número 1:" + luis1);
                                        numcorrienteinicial[contador] = Convert.ToDouble(luis22);
                                    }
                                }

                                contador++;
                            }


                            //Ordenamos las CONDICIONES INICIALES

                            for (int j = 0; j < contadorcondiciones; j++)
                            {
                                for (int i = 0; i < contadorcondiciones; i++)
                                {
                                    if (listacorrsalida[j] == numcorrienteinicial[i])
                                    {
                                        numcorrienteinicial1[j] = numcorrienteinicial[i];
                                        caudalinicial1[j] = caudalinicial[i];
                                        presioninicial1[j] = presioninicial[i];
                                        entalpiainicial1[j] = entalpiainicial[i];
                                    }
                                }
                            }

                            //Copiamos las condiciones iniciales en la Aplicación principal

                            for (int a = 0; a < contadorcondiciones; a++)
                            {
                                puntero1.numcorrienteinicial[a] = numcorrienteinicial1[a];
                                puntero1.caudalinicial[a]= caudalinicial1[a];
                                puntero1.presioninicial[a] = presioninicial1[a];
                                puntero1.entalpiainicial[a] = entalpiainicial1[a];
                            }

                            puntero1.numcondiciniciales = contadorcondiciones;
                            puntero1.leidascondicionesiniciales = 1;
                        }                   
                    }
                                           
                  }
                

                //Si el Número de TABLAS es cero sólo leemos las CONDICIONES INICIALES
                else if (numtablas == 0)
                {
                    //Si los datos iniciales son buenos SOLO leemos las CONDICIONES INICIALES
                    if (datosinciales == 1)
                    {
                        //Falta convertir las UNIDADES de las condiciones iniciales a las Unidades Británicas que es el Sistema de Unidades que utiliza las Tablas de Vapor ASME de 1967                   

                        String luis22 = "";
                        Double[] caudalinicial = new Double[(int)numcorrientes];
                        Double[] presioninicial = new Double[(int)numcorrientes];
                        Double[] entalpiainicial = new Double[(int)numcorrientes];
                        Double[] numcorrienteinicial = new Double[(int)numcorrientes];
                        int contador = 0;

                        //Bucle para contar el número de CONDICIONES INICIALES (número de corrientes de las que se incluyen sus condiciones iniciales)

                        int numerocondiciones = 0;
                        int contadorcondiciones = 0;

                        for (int n = (2 * (int)numeroequipos) + 3; n <=numlineasfichero; n++)
                        {
                            temp = lineas[n - 1];

                            if (temp == "")
                            {
                                contadorcondiciones=numerocondiciones;
                                goto luis;
                            }
                            numerocondiciones++;
                        }

                        contadorcondiciones = numerocondiciones;
                        
                        luis:

                        //Bucle para leer las CONDICIONES INICIALES
                        //for (int n = (2 * (int)numeroequipos) + 3; n <= (int)numcorrientes + (2 * (int)numeroequipos) + 2; n++)
                        for (int n = (2 * (int)numeroequipos) + 3; n <= contadorcondiciones + (2 * (int)numeroequipos) + 2; n++)
                        {
                            temp = lineas[n - 1];

                            for (int i = 0; i <= 50; i = i + 15)
                            {
                                luis22 = temp.Substring(0 + i, 15);

                               
                                if (i == 0)
                                {
                                    //Leemos el CAUDAL INICIAL
                                    if (unidades == 0)
                                    {
                                        //MessageBox.Show("Equipo Número:" + luis1);
                                        caudalinicial[contador] = Convert.ToDouble(luis22);
                                    }
                                    else if (unidades == 1)
                                    {
                                        //Pasamos de unidades del Sistema Métrico a unidades Británicas (de Kg/sg a Lb/sg)
                                        caudalinicial[contador] = Convert.ToDouble(luis22)/ (0.4536);
                                    }

                                    else if (unidades == 2)
                                    {
                                        //Pasamos de unidades del Sistema Internacional a unidades Británicas (de Kgr/sg a Lb/sg)
                                        caudalinicial[contador] = Convert.ToDouble(luis22)/ (0.4536);
                                    }

                                }

                                //LEEMOS LA PRESION INICIAL
                                else if (i == 15)
                                {
                                    if (unidades == 0)
                                    {
                                        //MessageBox.Show("Tipo de Equipo:" + luis1);
                                        presioninicial[contador] = Convert.ToDouble(luis22);
                                    }

                                    else if (unidades == 1)
                                    {
                                        //Pasamos de unidades del Sistema Internacional a unidades Británicas (de kPa a psia)
                                        presioninicial[contador] = Convert.ToDouble(luis22)/ (6.8947572);
                                    }

                                    else if (unidades == 2)
                                    {
                                        //Pasamos de unidades del Sistema Internacional a unidades Británicas (de Bar a psia)
                                        presioninicial[contador] = Convert.ToDouble(luis22)/ (6.8947572 / 100);
                                    }
                                }

                                // Leemos la ENTALPIA INICIAL
                                else if (i == 30)
                                {
                                    if (unidades == 0)
                                    {
                                        //MessageBox.Show("Corriente número 1:" + luis1);
                                        entalpiainicial[contador] = Convert.ToDouble(luis22);
                                    }

                                    else if (unidades == 1)
                                    {
                                        //Pasamos de unidades del Sistema Métrico al Británico (de kcal/Kgr a BTU/Lb)
                                        entalpiainicial[contador] = Convert.ToDouble(luis22)/ 2.326009;
                                    }

                                    else if (unidades == 2)
                                    {
                                        //Pasamos de unidades del Sistema Internacional al Británico (de Kj/Kgr a BTU/Lb)
                                        entalpiainicial[contador] = Convert.ToDouble(luis22)/ 2.326009;
                                    }
                                }

                                //Leemos el NÚMERO de CORRIENTES
                                else if (i == 45)
                                {
                                    //MessageBox.Show("Corriente número 1:" + luis1);
                                    numcorrienteinicial[contador] = Convert.ToDouble(luis22);
                                }
                            }

                            contador++;
                        }


                        //Ordenamos las CONDICIONES INICIALES

                        for (int j = 0; j < contadorcondiciones; j++)
                        {
                              for (int i = 0; i < contadorcondiciones; i++)
                              {
                                  if (listacorrsalida[j]==numcorrienteinicial[i])
                                  {
                                      numcorrienteinicial1[j] = numcorrienteinicial[i]; 
                                      caudalinicial1[j]=caudalinicial[i];
                                      presioninicial1[j]=presioninicial[i];
                                      entalpiainicial1[j] = entalpiainicial[i];
                                  }
                              }
                        }
                                         
                        //Copiamos las Condiciones iniciales en la Aplicación Principal
                        for (int a = 0; a < contadorcondiciones; a++)
                        {
                          puntero1.numcorrienteinicial[a] = numcorrienteinicial1[a];
                          puntero1.caudalinicial[a] = caudalinicial1[a];
                          puntero1.presioninicial[a] = presioninicial1[a];
                          puntero1.entalpiainicial[a] = entalpiainicial1[a];
                        }

                        puntero1.numcondiciniciales = contadorcondiciones;
                        puntero1.leidascondicionesiniciales = 1;
                    }
                }

                this.Cursor = Cursors.Arrow;
                //MessageBox.Show("Archivo Procesado.");
        }

//--------------------------------------------------------------------------------------------------------------------------------------------------------
      
        //Botón de OK
        private void button1_Click_1(object sender, EventArgs e)
        {
            //Este función sirve para ocultar el cuadro de diálogo LecturaHbal cuando pulsamos el botón OK de dicho cuadro de diálogo
            this.Hide();
        }



//--------------------------------------------------------------------------------------------------------------------------------------------------------



        //Función que se ejecuta cuando CARGAMOS el Form LecturaHbal
        private void LecturaHbal_Load(object sender, EventArgs e)
        {
            if (fl != null)
            {
                //Limpiamos los Controles Listas antes de abrir otro archivo
                //El control listBox1 se encuentra localizado en el cuadro de dialogo LecturaHbal 
                //El control listBox3 se encuentra localizado en la ventana principal de la Aplicación
                listBox1.Items.Clear();
                //puntero1.listBox3.Items.Clear();

                char[] luis = new char[55];

                string line;

                List<String> lineas11 = new List<String>();
                int contalineas11 = 0;

                while ((line = fl.ReadLine()) != null)
                {
                    listBox1.Items.Add(line);
                    //puntero1.listBox3.Items.Add(line);
                    lineas.Add("");
                    lineas[numlineasfichero] = line;
                    numlineasfichero++;

                    lineas11.Add(line + "\r\n");

                    contalineas11++;
                }

                for (int i = 0; i < contalineas11; i++)
                {
                    puntero1.textBox14.AppendText(lineas11[i]);
                }

                listBox1.Items.Add("Número de líneas del archivo:" + Convert.ToString(numlineasfichero - 1));
                puntero1.textBox14.AppendText("Número de líneas del archivo:" + Convert.ToString(numlineasfichero - 1));

                //Inicializamos el valor la variable local "numerolineas" del cuadro de dialogo LecturaHbal 
                numerolineas = (numlineasfichero - 1);

                //Cerramos el archivo una vez leido mediante el puntero fl recibido desde la función 
                fl.Close();

                button1.Enabled = true;

                //Llamamos al Botón de PROCESADO de Archivo. Es decir, llamamos a la función button5_Click. 
                //La función button5_Click está ubicada en ese fichero mas arriba. 
                button5_Click(sender, e);
            }
        }      
    }
}
