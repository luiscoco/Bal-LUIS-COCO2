using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Diagnostics;

//To Call Fortran *.DLL
using System.Runtime.InteropServices;

// needed to call external
// application (winAPI dll)
using System.Runtime.InteropServices;    // needed to call external
                                         // application (winAPI dll)

//Para utilizar el puntero a la Aplicación principal
using Drag_AND_Drop_between_Forms;

//Para utilizar la biblioteca de Matrices y el metodo de NewtonRaphson y objetos Parametros
using NumericalMethods;
using NumericalMethods.FourthBlog;

using MINPACK;

using DotNumerics;
using DotNumerics.ODE;
using DotNumerics.LinearAlgebra;




namespace WindowsFormsApplication2
{
    public partial class Motorcalculo : Form
    {
        //
        int pausa = 0;
        //Derivative Step Size 
        double derivativestepsize;
        //Error Relativo
        Double errorRelativo=1E-08;
        Double errorrelativomax = 0;
        //Error Absoluto
        Double errorabsolutomax = 0;
        //Errores Relativos
        Double[,] erroresRelativos1;
        //Factor de Iteraciones EPS
        Double EPS = 0;
        //Numero de Maximo de Iteraciones del método de Newton Raphson
        Double numeroiteraciones;

        //Numero de ecuaciones del sistema de ecuaciones
        int numeroecuaciones;
        
        //Para graficar los resultados después de solucionar el sistema de ecuaciones y evaluar el error
        //PENDIENTE DE IMPLEMENTACIÓN
        //List<Doouble> listaresultados = new List<Double>();

        //TextBoxes para guardar las ecuaciones
        private ArrayList textboxes=new ArrayList();

        //TextBoxes para guardar las condiciones inciales
        private ArrayList textboxes1 = new ArrayList();

        //TextBoxes para guardar los resultados
        private ArrayList textboxes2 = new ArrayList();

        //Labels2 lista de etiquetas de los resultados del sistema de ecuaciones
        private ArrayList labels2 = new ArrayList();

        private ArrayList textboxes3 = new ArrayList();
       
        //Labels2 lista de etiquetas de los resultados del sistema de ecuaciones
        private ArrayList labels3= new ArrayList();

        //Labels2 lista de etiquetas de los resultados del sistema de ecuaciones
        private ArrayList labels33 = new ArrayList();

        private ArrayList textboxes4 = new ArrayList();
        
        //Labels2 lista de etiquetas de los resultados del sistema de ecuaciones
        private ArrayList labels4 = new ArrayList();
               
        //Puntero a la Aplicacion principal
        public Aplicacion aplicacion1=new Aplicacion();

        //Lista de Tablas definidas por el Usuario
        List<Double[,]> ListaTablas1 = new List<Double[,]>();

        //Lista de Double para guardar las condiciones iniciales
         List<Double> condicionesiniciales = new List<Double>();

        //Variable que nos informa si vamos a considerar las condiciones iniciales de un cálculo anterior: si(1), no(0)
         Double cond = 0;

         List<Func<Double>> functions1 = new List<Func<Double>>();

         //Lista de parametros que guardan los valores y nombres de las variables del sistema
         List<Parameter> E = new List<Parameter>();

         //Parametro para inicializar la lista de parámetros que guardan las variables del sistema de ecuaciones
         Parameter Eprueba = new Parameter(1,0.01);

        //Opción de Cálculo
        string opcioncalculo1 = "Jacobiano Inverso";
        string opcioncalculo2 = "LU";

        //Contador del DataGridView para visualizar los resultados de Tiempo de los Métodos de Cálculo
        public int contadatagrid = 0;
        
        //Guardamos los resultados de los cálculos en el DataGridView
        string[] opcion1 = new string[500];
        string[] opcion2 = new string[500];
        Double[] tiempo = new Double[500];

        //Guardar Datos de las Iteraciones Intermedias: Yes=1, No=0.
        public int guardarintermedias = 0;
        
        //Guardamos el Número de Bandas de la Matriz Jacobiana
        public int numbandasinferiores = 0;
        public int numbandassuperiores = 0;

        //Elemento Seleccionado de las Lista de Condiciones iniciales
        int elementoseleccionado = 0;

        //Lista de parametros que guardan los valores y nombres de las variables del sistema
        List<Parameter> p5 = new List<Parameter>();


        public Motorcalculo()
        {
            InitializeComponent();
            //Incializamos el puntero local a la Aplicacion principal con el valor recibido en el argumento del constructor de la Clase del motor de calculo            
        }

        public void punteroaplicacion(Aplicacion aplicacion2)
        {
            aplicacion1 = aplicacion2;

            functions1 = aplicacion1.functions;
            E = aplicacion1.p;

            //Inicializamos el numero de ecuaciones generadas en la aplicación en el motor de calculo
            textBox9.Text = Convert.ToString(aplicacion1.numecuaciones);
            numeroecuaciones = Convert.ToInt16(textBox9.Text);
            
            //Inicializamos el Numero de iteraciones para ejecutar el metodo de NewtonRaphson
            numeroiteraciones = aplicacion1.NumMaxIteraciones;
            textBox5.Text = Convert.ToString(numeroiteraciones);
            
            //Inicializamos el Error Máximo Admisible
            textBox4.Text = Convert.ToString(aplicacion1.ErrorMaxAdmisible);
            textBox1.Text = Convert.ToString(aplicacion1.ErrorMaxAdmisible);

            //Inicializamos el Factor de Iteraciones (EPS)
            if (aplicacion1.FactorIteraciones != 0)
            {
                textBox7.Text = Convert.ToString(aplicacion1.FactorIteraciones);
            }
            else if (aplicacion1.FactorIteraciones == 0)
            {
                textBox7.Text = Convert.ToString(0.5);
            }
        }
        
        //Función FCN para evaluar Xn
        public int f03(int n, double[] x, double[] fvec, int iflag)
        {
            for (int y = 0; y < E.Count; y++)
            {
                E[y].Value = x[y];
            }

            for (int i = 0; i < functions1.Count; i++)
                {
                    fvec[i] = functions1[i]();                  
                }

            return 1;
        }
           
        //Evento que se produce cuando Cargamos el Cuadro de Diálogo del Motor de Cálculo
        //Inicializamos los controles Check Boxes
        private void Form1_Load(object sender, EventArgs e)
        {
            //Análisis tipo ESTACIONARIO
            if (aplicacion1.tipoanalisis == 0)
            {
                //Elegimos el tabPage1 (Análisis ESTACIONARIO)
                tabControl1.SelectedTab = tabPage1;

                //Desactivamos el botón de solución el Estado Transitorio
                button9.Enabled = false;
                //Desactivamos el botón de solución el Estado Estacionario
                button1.Enabled = true;

                //Elegimos por defecto el método de cálculo del cálculo del Jacobiano Inverso.
                checkBox2.Checked = true;

                checkBox3.Checked = false;
                checkBox4.Checked = false;

                //Elegimos por defecto la descomposición LU de A.
                checkBox7.Checked = true;

                checkBox11.Checked = false;
                checkBox8.Checked = false;
                checkBox6.Checked = false;
                checkBox5.Checked = false;

                checkBox12.Checked = false;
                checkBox13.Checked = false;
                checkBox14.Checked = false;
                checkBox15.Checked = false;
            }

            //Análisis tipo TRANSITORIO
            else if (aplicacion1.tipoanalisis == 1)
            {
                //Elegimos el tabPage4 (Análisis TRANSITORIO)
                tabControl1.SelectedTab = tabPage4;

                //Desactivamos el botón de solución el Estado Transitorio
                button9.Enabled = true;
                //Desactivamos el botón de solución el Estado Estacionario
                button1.Enabled = false;

                //Desactivamos todas las opciones de resolución de Ecuaciones Diferenciales excepto la opción de Runge-Kutta-Euler
                checkBox27.Checked = true;
                checkBox16.Checked = false;
                checkBox18.Checked = false;
                checkBox28.Checked = false;
                checkBox29.Checked = false;
                checkBox17.Checked = false;
                checkBox30.Checked = false;
                checkBox20.Checked = false;
                checkBox26.Checked = false;
                checkBox25.Checked = false;
                checkBox24.Checked = false;
                checkBox22.Checked = false;
                checkBox19.Checked = false;
                checkBox31.Checked = false;

                //Desactivamos botones no utilizados en los Análisis de Transitorios
                button4.Enabled = false;
                button5.Enabled = false;
                button8.Enabled = false;
                button3.Enabled = false;
                button7.Enabled = false;
            }

            //Cargamos las Condiciones Iniciales en el Tab de Modificar/Edit las Condiciones Iniciales 
            listBox6.Items.Clear();

            //int j = 0;
            //Cargamos las Condiciones Iniciales en el ListBox6 para poder Editarlas y Modificarlas
            //for (j = 0; j < E.Count; j++)
            //{
            //    listBox6.Items.Add(E[j].Nombre + ":" + Convert.ToString(E[j].Value));
            //}
                      
            //Inicializamos el Control listBox7 con los valores del array E 
            for (int i = 0; i < E.Count; i++)
            {
                listBox7.Items.Add(E[i].Nombre + ":" + Convert.ToString(E[i].Value));
            }
            //Convertimos las Unidades del array de parámetros E al p5 en el Sistema de Unidades elegido
            for (int i = 0; i < E.Count; i++)
            {               
                //Cambio de Unidades del Sistema Británico al Sistema de Unidades Internacional o Métrico
                //Sistema Britanico=0;Sistema Internacional=1;Sistema Métrico=2
                Double temporal = 0;
                String nombretemp = "";
                //Unidades Sistema Internacional
                if (aplicacion1.unidades == 1)
                {
                    String primercaracter = E[i].Nombre.Substring(0, 1);

                    if (primercaracter == "W")
                    {
                        temporal = E[i].Value * (0.4536);
                    }
                    else if (primercaracter == "P")
                    {
                        temporal = E[i].Value * (6.8947572);
                    }
                    else if (primercaracter == "H")
                    {
                        temporal = E[i].Value * 2.326009;
                    }

                    nombretemp = E[i].Nombre;
                    listBox6.Items.Add(nombretemp+ ":" + Convert.ToString(temporal));
                }

                //Unidades Sistema Métrico
                else if (aplicacion1.unidades == 2)
                {
                    String primercaracter = E[i].Nombre.Substring(0, 1);

                    if (primercaracter == "W")
                    {
                        temporal= ((E[i].Value) * 0.4536);
                    }
                    else if (primercaracter == "P")
                    {
                       temporal = E[i].Value * (6.8947572 / 100);
                    }
                    else if (primercaracter == "H")
                    {
                       temporal = E[i].Value * 2.326009;
                    }

                    nombretemp = E[i].Nombre;
                    listBox6.Items.Add(nombretemp + ":" + Convert.ToString(temporal));
                }

                //Unidades Sistema Británico
                else if (aplicacion1.unidades == 0)
                {
                   
                    listBox6.Items.Add(nombretemp + ":" + Convert.ToString(temporal));
                }
            }
        }

        //Pulsamos el Botón de OK en el diálogo del Motor de Cálculo
        private void button2_Click(object sender, EventArgs e)
        {
            //Si se trata de una Análisis ESTACIONARIO
            if (aplicacion1.tipoanalisis == 0)
            {
                //Copiar las CONDICIONES INICIALES
                button3_Click(sender, e);

                //Inicializamos el número de equipos del modelo calculado
                aplicacion1.numtipo1 = 0;
                aplicacion1.numtipo2 = 0;
                aplicacion1.numtipo3 = 0;
                aplicacion1.numtipo4 = 0;
                aplicacion1.numtipo5 = 0;
                aplicacion1.numtipo6 = 0;
                aplicacion1.numtipo7 = 0;
                aplicacion1.numtipo8 = 0;
                aplicacion1.numtipo9 = 0;
                aplicacion1.numtipo10 = 0;
                aplicacion1.numtipo11 = 0;
                aplicacion1.numtipo12 = 0;
                aplicacion1.numtipo13 = 0;
                aplicacion1.numtipo14 = 0;
                aplicacion1.numtipo15 = 0;
                aplicacion1.numtipo16 = 0;
                aplicacion1.numtipo17 = 0;
                aplicacion1.numtipo18 = 0;
                aplicacion1.numtipo19 = 0;
                aplicacion1.numtipo20 = 0;
                aplicacion1.numtipo21 = 0;
                aplicacion1.numtipo22 = 0;

                //Guardar los Resultados de Corrientes y Equipos
                aplicacion1.guardaresultadoscalculos();

                //Inicializar a cero todas las varibles del motor de cálculo.
                //Exportar los RESULTADOS a la aplicación principal en las unidades elegidas en la aplicación 
                aplicacion1.visualizarResultadosToolStripMenuItem_Click(sender, e);

                this.Hide();

                aplicacion1.actualizararbol();

                //Pulsamos la Opción del Menú de Nuevo Cálculo para dejar todo preparado para un siguiente cálculo
                aplicacion1.toolStripMenuItem11_Click(sender, e);
            }

            //Si se trata de una Análisis TRANSITORIO   
            else if (aplicacion1.tipoanalisis == 1)
            {
                //Pulsamos la Opción del Menú de Nuevo Cálculo para dejar todo preparado para un siguiente cálculo
                aplicacion1.toolStripMenuItem11_Click(sender, e);

                this.Hide();
            }
        }



        //Copiar CONDICIONES INICIALES
        private void button3_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();

            int j = 0;
            //Creamos en tiempo real los TextBoxes para introducir los valores iniciales de las variables
            for (j = 0; j < E.Count; j++)
            {
                listBox2.Items.Add(E[j].Nombre + " :" + Convert.ToString(E[j].Value));
            }

            //Copiamos la lista de los resultados de los parámeteros E en la lista de los parámeteros de la aplicación p
            aplicacion1.p = E;
            aplicacion1.numcondiciniciales=E.Count;

            int cont = 0;
            int cont1 = 1;
            int cont2 = 2;

            for (int a = 0; a < E.Count/3; a++)
            {
                aplicacion1.caudalinicial[a] = E[cont];
                aplicacion1.presioninicial[a] = E[cont1];
                aplicacion1.entalpiainicial[a] = E[cont2];
                cont = cont + 3;
                cont1 = cont1 + 3;
                cont2 = cont2 + 3;
            }

            aplicacion1.leidascondicionesiniciales = 1;

            //Cargamos las Condiciones Iniciales en el Tab de Modificar/Edit las Condiciones Iniciales 
            listBox6.Items.Clear();

            int g = 0;
            //Creamos en tiempo real los TextBoxes para introducir los valores iniciales de las variables
            for (g = 0; g < E.Count; g++)
            {
                listBox6.Items.Add(E[g].Nombre + " :" + Convert.ToString(E[g].Value));
            }
        }

        //Función NO TERMINADA para Parar los Cálculos después de presionar una tecla el usuario
        private void Motorcalculo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Escape))
            {
                pausa = 1;
            }
        }

        //Evento que se produce al Mostrar el cuadro de diálogo del Motor de Cálculo
        private void Motorcalculo_Shown(object sender, EventArgs e)
        {
            numeroecuaciones = functions1.Count;

            if (aplicacion1.ecuaciones.Count != E.Count)
            {
                //MessageBox.Show("Error el número de Ecuaciones (representativas del modelo) es diferente al número de Variables del Modelo.");
                return;
            }

            //Creamos en tiempo real los TextBoxes para introducir las ecuaciones
            for (int i = 0; i < (E.Count-1); i++)
            {
                listBox1.Items.Add("Ecuación Nº" + Convert.ToString(i) + " :" + aplicacion1.ecuaciones[i]);
            }

            //Creamos en tiempo real los TextBoxes para introducir las ecuaciones
            for (int h = 0; h < (E.Count); h++)
            {
                listBox2.Items.Add(E[h].Nombre + " :" + Convert.ToString(E[h].Value));
            }          
        }

        //Opción de resolución de los Sistemas de ecuaciones No Lineales,cálculo del Jacobiano Inverso por Broyden.
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox9.Checked = false;
            }
        }

        //Opción de resolución de los Sistemas de ecuaciones No Lineales, cálculo del Jacobiano por Broyden.
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                checkBox2.Checked = false;
                checkBox4.Checked = false;
                checkBox9.Checked = false;
            }
        }

        //Opción de resolución de los Sistemas de ecuaciones No Lineales mediante la Librería MINPACK (Hybrid1, Hybrid)
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked == true)
            {
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox9.Checked = false;

                checkBox5.Checked = false;
                checkBox6.Checked = false;
                checkBox7.Checked = false;
                checkBox8.Checked = false;
                checkBox11.Checked = false;

                checkBox12.Checked = false;
                checkBox13.Checked = false;
                checkBox14.Checked = false;
                checkBox15.Checked = false;
            }
        }

        //Opción de resolución de los Sistemas de ecuaciones Lineales mediante la descomposición PLU de A
        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked == true)
            {
                checkBox5.Checked = false;
                checkBox6.Checked = false;
                checkBox8.Checked = false;
                checkBox11.Checked = false;

                checkBox12.Checked = false;
                checkBox13.Checked = false;
                checkBox14.Checked = false;
                checkBox15.Checked = false;

                checkBox4.Checked = false;
                checkBox9.Checked = false;
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked == true)
            {
                checkBox5.Checked = false;
                checkBox7.Checked = false;
                checkBox8.Checked = false;
                checkBox11.Checked = false;

                checkBox12.Checked = false;
                checkBox13.Checked = false;
                checkBox14.Checked = false;
                checkBox15.Checked = false;

                checkBox4.Checked = false;
                checkBox9.Checked = false;
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked == true)
            {
                checkBox7.Checked = false;
                checkBox6.Checked = false;
                checkBox8.Checked = false;
                checkBox11.Checked = false;

                checkBox12.Checked = false;
                checkBox13.Checked = false;
                checkBox14.Checked = false;
                checkBox15.Checked = false;

                checkBox4.Checked = false;
                checkBox9.Checked = false;
            }
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox8.Checked == true)
            {
                checkBox5.Checked = false;
                checkBox6.Checked = false;
                checkBox7.Checked = false;
                checkBox11.Checked = false;

                checkBox12.Checked = false;
                checkBox13.Checked = false;
                checkBox14.Checked = false;
                checkBox15.Checked = false;

                checkBox4.Checked = false;
                checkBox9.Checked = false;
            }
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox11.Checked == true)
            {
                checkBox5.Checked = false;
                checkBox6.Checked = false;
                checkBox7.Checked = false;
                checkBox8.Checked = false;

                checkBox12.Checked = false;
                checkBox13.Checked = false;
                checkBox14.Checked = false;
                checkBox15.Checked = false;

                checkBox4.Checked = false;
                checkBox9.Checked = false;
            }
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox12.Checked == true)
            {
                checkBox5.Checked = false;
                checkBox6.Checked = false;
                checkBox7.Checked = false;
                checkBox8.Checked = false;

                checkBox11.Checked = false;
                checkBox13.Checked = false;
                checkBox14.Checked = false;
                checkBox15.Checked = false;

                checkBox4.Checked = false;
                checkBox9.Checked = false;
            }

        }


        //CheckBox para elegir la Opción de Guardar los Cálculos de las Iteraciones Intermedias
        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            if (this.numeroecuaciones > 50)
            {
                MessageBox.Show("This option is not advisable for number of equations above 50. Calculation process will be slow down. Please Uncheck this option");
            }

            if (this.numeroiteraciones > 100)
            {
                MessageBox.Show("With this option checked for storage reasons Maximum Iteration Number will be set to 100.");

                textBox5.Text = "100";
                numeroiteraciones = 100;
            }

            //Opción de Guardar los Datos de las Iteraciones Intermedias
            if (checkBox10.Checked == true)
            {
                guardarintermedias = 1;
                aplicacion1.guardarintermedias4 = 1;
            }

            else if (checkBox10.Checked == false)
            {
                guardarintermedias = 0;
                aplicacion1.guardarintermedias4 = 0;
            }
        }

        //Botón STATIONARY SOLVING ENGNIGE 
        public void button1_Click_1(object sender, EventArgs e)
        {

//---------------------------------------------------Tipo de Análisis ESTACIONARIO -----------------------------------------------------------------------------------------------------------------------------------
            if (aplicacion1.tipoanalisis == 0)
            {
                //Start the stopwatch 
                Stopwatch sw = Stopwatch.StartNew();

                //Fijamos la opción de cálculo del SISTEMAS DE ECUACIONES NO LINEALES
                if (checkBox2.Checked == true)
                {
                    opcioncalculo1 = "Jacobiano Inverso";
                    aplicacion1.tipocalculo = 0;
                }

                else if (checkBox3.Checked == true)
                {
                    opcioncalculo1 = "Jacobiano";
                    aplicacion1.tipocalculo = 1;
                }

                else if (checkBox4.Checked == true)
                {
                    opcioncalculo1 = "MINPACK";
                }

                //Fijamos la opción de cálculo del Sistema de Ecuaciones Lineal
                if (checkBox7.Checked == true)
                {
                    opcioncalculo2 = "LU";
                }

                else if (checkBox6.Checked == true)
                {
                    opcioncalculo2 = "QR";
                }

                else if (checkBox5.Checked == true)
                {
                    opcioncalculo2 = "Orthogonal";
                }
                else if (checkBox8.Checked == true)
                {
                    opcioncalculo2 = "SVD";
                }
                else if (checkBox11.Checked == true)
                {
                    opcioncalculo2 = "BANDA";
                }
                else if (checkBox12.Checked == true)
                {
                    opcioncalculo2 = "JACOBI";
                }
                else if (checkBox13.Checked == true)
                {
                    opcioncalculo2 = "GAUSS-SEIDEL";
                }
                else if (checkBox14.Checked == true)
                {
                    opcioncalculo2 = "SOR";
                }
                else if (checkBox15.Checked == true)
                {
                    opcioncalculo2 = "CONJUGATED GRADIENT";
                }


//-------------------------------- Método de Cálculo de NEWTOON RAPHSON y BROYDEN -------------------------------------------------------------------------

                if ((opcioncalculo1 == "Jacobiano Inverso") || (opcioncalculo1 == "Jacobiano"))
                {

                    this.Cursor = Cursors.WaitCursor;

                    //Copiamos de la aplicación principal las ecuaciones y las incognitas
                    functions1 = aplicacion1.functions;
                    E = aplicacion1.p;

                    //Contador de Número de iteraciones
                    int contador;
                    contador = 1;

                    //Comprobamos que el número de ecuaciones es igual al número de incognitas
                    if (functions1.Count != E.Count)
                    {
                        MessageBox.Show("ERROR!!!. Número de Ecuaciones diferente al número de Incognitas, revise el modelo.");
                        return;
                    }

                    //Capturamos el usuario el error absoluto maximo permitido
                    //precision = Convert.ToDouble(textBox1.Text);

                    //Incializamos el textBox del número de Iteraciones
                    if (textBox5.Text == "0")
                    {
                        textBox5.Text = "20";
                    }

                    //Introducimos los valores iniciales de las variables si no se ha inicializado anteriormente
                    for (int j = 0; j < E.Count; j++)
                    {
                        if (E[j].Value == 0)
                        {
                            E[j].Value = 10;
                        }
                    }

                    numeroiteraciones = Convert.ToInt16(textBox5.Text);

                    EPS = Convert.ToDouble(textBox7.Text);

                    derivativestepsize = Convert.ToDouble(textBox6.Text);

                    //Opción para Guardar los Datos de las Iteraciones Intermedias
                    if (checkBox10.Checked == true)
                    {
                        guardarintermedias = 1;
                        aplicacion1.guardarintermedias4 = 1;
                    }

                    //Opción para NO Guardar los Datos de las Iteraciones Intermedias (por Defecto NO se guardan las iteraciones intermedid
                    else if (checkBox10.Checked == false)
                    {
                        guardarintermedias = 0;
                        aplicacion1.guardarintermedias4 = 0;
                    }

                    if (checkBox11.Checked == true)
                    {
                        //Llamamos a la función CalculoBandas() para calcular el número de Bandas Superiores e Inferiores de la Matriz del Sistema (matrizauxjacob)
                        aplicacion1.CalculoBandas(ref numbandasinferiores, ref numbandassuperiores);
                    }
                    //Creamos un objeto de la Clase NewtonRaphson y lo inicializamos
                    NewtonRaphson1 nr = new NewtonRaphson1(numbandasinferiores, numbandassuperiores, guardarintermedias, numeroiteraciones, aplicacion1, opcioncalculo1, opcioncalculo2, aplicacion1.matrizauxjacob, E, functions1, EPS, derivativestepsize);

                    //Etiqueta para volver al principio del bucle de iteración de cálculo
                    //luis:

                    Double[] valorE = new Double[10000];
                    Double[] funcionesE = new Double[10000];

                    //Bucle de iteración del método de Newton Raphson
                    for (int b = 0; b < numeroiteraciones; b++)
                    {
                    luis:

                        errorrelativomax = 0;
                        errorabsolutomax = 0;

                        if (aplicacion1.consola == 1)
                        {
                            //Activamos la consola para visualizar los resultados intermedios de los cálculos
                            Win32.AllocConsole();
                            Console.ForegroundColor = ConsoleColor.White;
                            //Escribimos el número de iteración de los resultados intermedios
                            Console.WriteLine(Convert.ToString("Iteracción Número: " + Convert.ToString(contador)));
                        }

                        //Llamada al objeto nr de la clase NewtonRaphson para realizar una iteracción
                        erroresRelativos1 = nr.Iterate(b);

                        //Comprobacón 
                        if (erroresRelativos1[0, 0] == 999999999999)
                        {
                            MessageBox.Show("Matriz SINGULAR. Cálculo Abortado LUIS COCO.");
                            goto salida;
                        }

                        for (int g = 0; g < E.Count; g++)
                        {
                            valorE[g] = E[g].Value;

                            funcionesE[g] = functions1[g]();

                            if (checkBox1.Checked == true)
                            {
                                if (aplicacion1.consola == 1)
                                {
                                    //Escribimos en la consola los nombre y valores de los parametros
                                    Console.WriteLine(Convert.ToString(E[g].Nombre + ":" + valorE[g]));
                                    //Escribimos en la consola el Error de la funciones/ecuaciones
                                    Console.WriteLine(Convert.ToString("Error " + "E" + "(x)" + ":" + Convert.ToString(funcionesE[g])));
                                }
                            }

                            if (E[g].Value > Convert.ToDouble(textBox2.Text) || E[g].Value < Convert.ToDouble(textBox3.Text))
                            {
                                MessageBox.Show("Hola LUIS COCO:Error de No Convergencia.", "Hola LUIS COCO:Error No Convergencia del Método", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                goto salida;
                            }

                            //Fijamos el número máximo de Iteraciones permitido
                            if (contador > numeroiteraciones)
                            {
                                MessageBox.Show("Superado Límite Máximo de Iteraciones. Número Iteraciones:" + Convert.ToString(contador), "Error superado Límite Iteraciones del Método.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                goto salida;
                            }
                        }

                        contador++;

                        //Comprobamos que los errores relativos siempre son menores que el valor introducido en el textBox4
                        for (int a = 0; a < E.Count; a++)
                        {
                            double temp = 0;
                            double temp1 = 0;

                            if (Math.Abs(erroresRelativos1[a, 0]) > (Convert.ToDouble(textBox4.Text)))
                            {
                                temp = Math.Abs(erroresRelativos1[a, 0]);

                                if (temp > errorrelativomax)
                                {
                                    errorrelativomax = temp;
                                }
                            }

                            if (Math.Abs(funcionesE[a]) > (Convert.ToDouble(textBox1.Text)))
                            {
                                temp1 = Math.Abs(funcionesE[a]);

                                if (temp1 > errorabsolutomax)
                                {
                                    errorabsolutomax = temp1;
                                }
                            }
                        }

                        if (errorrelativomax > (Convert.ToDouble(textBox4.Text)))
                        {
                            b++;
                            goto luis;
                        }

                        else if (errorabsolutomax > (Convert.ToDouble(textBox1.Text)))
                        {
                            b++;
                            goto luis;
                        }

                        else
                        {
                            goto salida;
                        }
                    }

                salida:

                    if (aplicacion1.consola == 1)
                    {
                        //Esperamos que el usuario pulse una tecla del teclado para finalizar/ocultar la consola
                        Console.WriteLine("Pulse una tecla para Continuar...");
                        Console.ReadKey();
                        Console.Beep();
                        //Finalizamos/cerramos la consola
                        Win32.FreeConsole();
                    }

                    listBox3.Items.Clear();
                    listBox4.Items.Clear();
                    listBox5.Items.Clear();

                    for (int g = 0; g < E.Count; g++)
                    {
                        //Escribimos en los TextBoxes los valores de las variables
                        listBox3.Items.Add(E[g].Nombre + " :" + Convert.ToString(E[g].Value));

                        //Calculamos los Errores Absolutos 
                        listBox4.Items.Add("Error " + E[g].Nombre + " :" + Convert.ToString(functions1[g]()));

                        //Calculamos los Errores Relativos 
                        listBox5.Items.Add("Error " + E[g].Nombre + " :" + Convert.ToString(erroresRelativos1[g, 0]));
                    }

                    textBox5.Text = Convert.ToString(contador);

                    label6.ForeColor = Color.Red;
                    label6.Text = "Número FINAL de iteraciones:";
                    label6.Location = new System.Drawing.Point(232, 45);

                    //Marca que indica que ya hemos creado los parámetros, ecuaciones y leido las condiciones iniciales
                    aplicacion1.marca = 1;


                    this.Cursor = Cursors.Arrow;

                    //Activamos el Botón de OK Button2
                    button2.Enabled = true;

                    sw.Stop();

                    dataGridView1.Rows.Clear();
                    opcion1[contadatagrid] = opcioncalculo1;
                    opcion2[contadatagrid] = opcioncalculo2;
                    tiempo[contadatagrid] = Convert.ToDouble(sw.Elapsed.TotalMilliseconds);

                    for (int i = 0; i < contadatagrid + 1; i++)
                    {
                        dataGridView1.Rows.Add();
                    }
                    //Populamos el DataGridView
                    for (int h = 1; h < dataGridView1.Rows.Count; h++)
                    {
                        dataGridView1.Rows[h].Cells[0].Value = opcion1[h - 1];
                        dataGridView1.Rows[h].Cells[1].Value = opcion2[h - 1];
                        dataGridView1.Rows[h].Cells[2].Value = tiempo[h - 1];
                    }
                    contadatagrid++;

                    //MessageBox.Show("Time used (float): {0} ms" + Convert.ToString(sw.Elapsed.TotalMilliseconds));
                    //MessageBox.Show("Time used (rounded): {0} ms" + Convert.ToString(sw.ElapsedMilliseconds));
                    
                    //En caso de que estemos trabajando con el Ejemplo de Validación del Motor de Cálculo
                    //tenemos que pulsar el Botón de OK del Motor de Cálculo que llama a la función de visualizar los resultados de las corrientes
                    if (aplicacion1.ejemplovalidacion == 1)
                    {
                        //Pulsamos el Botón de OK del Motor de Cálculo
                        button2_Click(sender, e);
                    }

                    else
                    {
                        //Ponemos a Cero la Señal de ejemplo de Validación del Motor de Cálculo
                        aplicacion1.ejemplovalidacion = 0;
                    }
                }
//-------------------------------- FIN del Método de Cálculo de NEWTOON RAPHSON y BROYDEN -------------------------------------------------------------------------




//----------------------------------------- Método de Cálculo de la Librería MINPACK ------------------------------------------------------------------------------ 

                else if (opcioncalculo1 == "MINPACK")
                {
                    numeroiteraciones = Convert.ToDouble(textBox5.Text);
                    errorabsolutomax = Convert.ToDouble(textBox1.Text);

                    this.Cursor = Cursors.WaitCursor;
                    numeroecuaciones = functions1.Count;
                    double[] fvec = new double[numeroecuaciones];
                    int lwa;
                    lwa = (numeroecuaciones * (3 * numeroecuaciones + 13)) / 2;
                    double[] wa = new double[lwa];
                    double[] x = new double[numeroecuaciones];
                    int iflag;
                    int info;
                    double tol = 1e-8;

                    Hybrid1 hybrid1instancia = new Hybrid1();

                    for (int y = 0; y < numeroecuaciones; y++)
                    {
                        x[y] = E[y].Value;
                    }

                    iflag = 1;

                    //Llamamos a la función CalculoBandas() para calcular el número de Bandas Superiores e Inferiores de la Matriz del Sistema (matrizauxjacob)
                    aplicacion1.CalculoBandas(ref numbandasinferiores, ref numbandassuperiores);

                    f03(numeroecuaciones, x, fvec, iflag);

                    info = hybrid1instancia.hybrd1run(this, numeroecuaciones, x, fvec, tol, wa, lwa, errorabsolutomax, ref numeroiteraciones, numbandasinferiores, numbandassuperiores);

                    textBox5.Text = Convert.ToString(numeroiteraciones);

                    for (int y = 0; y < E.Count; y++)
                    {
                        E[y].Value = x[y];
                    }

                    listBox3.Items.Clear();
                    listBox4.Items.Clear();
                    listBox5.Items.Clear();

                    for (int g = 0; g < E.Count; g++)
                    {
                        //Escribimos en los TextBoxes los valores de las variables
                        listBox3.Items.Add(E[g].Nombre + " :" + Convert.ToString(E[g].Value));

                        //Calculamos los Errores Absolutos 
                        listBox4.Items.Add("Error " + E[g].Nombre + " :" + Convert.ToString(fvec[g]));
                    }

                    //textBox5.Text = Convert.ToString(contador);

                    label6.ForeColor = Color.Red;
                    label6.Text = "Número FINAL de iteraciones:";
                    label6.Location = new System.Drawing.Point(232, 45);

                    this.Cursor = Cursors.Arrow;

                    //Activamos el Botón de OK Button2
                    button2.Enabled = true;

                    sw.Stop();

                    dataGridView1.Rows.Clear();
                    opcion1[contadatagrid] = opcioncalculo1;
                    opcion2[contadatagrid] = opcioncalculo2;
                    tiempo[contadatagrid] = Convert.ToDouble(sw.Elapsed.TotalMilliseconds);

                    for (int i = 0; i < contadatagrid + 1; i++)
                    {
                        dataGridView1.Rows.Add();
                    }
                    //Populamos el DataGridView
                    for (int h = 1; h < dataGridView1.Rows.Count; h++)
                    {
                        dataGridView1.Rows[h].Cells[0].Value = opcion1[h - 1];
                        dataGridView1.Rows[h].Cells[1].Value = opcion2[h - 1];
                        dataGridView1.Rows[h].Cells[2].Value = tiempo[h - 1];
                    }
                    contadatagrid++;

                    //MessageBox.Show("Time used (float): {0} ms" + Convert.ToString(sw.Elapsed.TotalMilliseconds));
                    //MessageBox.Show("Time used (rounded): {0} ms" + Convert.ToString(sw.ElapsedMilliseconds));

                    return;
                }
//---------------------------------------- Fin del Método de Cálculo de la Librería MINPACK -----------------------------------------------------------------------------------------------------------------------------




//--------------------------------------- Método de LU para Sistema Ecuaciones Lineales -------------------------------------------------------------------------------------------------------------------------------------            
                else if ((checkBox2.Checked == false) && (checkBox3.Checked == false) && (checkBox4.Checked == false) && (checkBox9.Checked == false))
                {

                    MessageBox.Show("Método de Cálculo para solucionar Sistema de Ecuaciones Lineales.");

                    //DEGSV método PLU
                    if (checkBox7.Checked == true)
                    {
                        LinearEquations leq1 = new LinearEquations();
                        //Ax = leq.Solve(jacobianX0, functionX0);  
                    }

                    //DGBSV método PLU con matriz Banda
                    else if (checkBox11.Checked == true)
                    {
                        LinearEquations leq2 = new LinearEquations();
                        //Ax = leq.Solve(jacobianaBandaX0, functionX0);                
                    }

                    //DGELS método QR o LQ
                    else if (checkBox6.Checked == true)
                    {
                        LinearLeastSquares leastSquares = new LinearLeastSquares();
                        //Ax = leastSquares.QRorLQSolve(jacobianX0, functionX0);  
                    }

                    this.Cursor = Cursors.Arrow;

                    //Activamos el Botón de OK Button2
                    button2.Enabled = true;
                }
//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

            }
        }




        //Botón para REFRESCAR la Lista de Condiciones Iniciales listBox6
        private void button5_Click(object sender, EventArgs e)
        {
            //Cargamos las Condiciones Iniciales en el Tab de Modificar/Edit las Condiciones Iniciales 
            listBox6.Items.Clear();
            listBox7.Items.Clear();

            int j = 0;
            //Creamos en tiempo real los TextBoxes para introducir los valores iniciales de las variables
            for (j = 0; j < E.Count; j++)
            {
                listBox7.Items.Add(E[j].Nombre + ":" + Convert.ToString(E[j].Value));
            }

            //Convertimos las Unidades del array de parámetros E al p5 en el Sistema de Unidades elegido
            for (int i = 0; i < E.Count; i++)
            {
                //Cambio de Unidades del Sistema Británico al Sistema de Unidades Internacional o Métrico
                //Sistema Britanico=0;Sistema Internacional=1;Sistema Métrico=2
                Double temporal = 0;
                String nombretemp = "";
                //Unidades Sistema Internacional
                if (aplicacion1.unidades == 1)
                {
                    String primercaracter = E[i].Nombre.Substring(0, 1);

                    if (primercaracter == "W")
                    {
                        temporal = E[i].Value * (0.4536);
                    }
                    else if (primercaracter == "P")
                    {
                        temporal = E[i].Value * (6.8947572);
                    }
                    else if (primercaracter == "H")
                    {
                        temporal = E[i].Value * 2.326009;
                    }

                    nombretemp = E[i].Nombre;
                    listBox6.Items.Add(nombretemp + ":" + Convert.ToString(temporal));
                }

                //Unidades Sistema Métrico
                else if (aplicacion1.unidades == 2)
                {
                    String primercaracter = E[i].Nombre.Substring(0, 1);

                    if (primercaracter == "W")
                    {
                        temporal = ((E[i].Value) * 0.4536);
                    }
                    else if (primercaracter == "P")
                    {
                        temporal = E[i].Value * (6.8947572 / 100);
                    }
                    else if (primercaracter == "H")
                    {
                        temporal = E[i].Value * 2.326009;
                    }

                    nombretemp = E[i].Nombre;
                    listBox6.Items.Add(nombretemp + ":" + Convert.ToString(temporal));
                }

                //Unidades Sistema Británico
                else if (aplicacion1.unidades == 0)
                {

                    listBox6.Items.Add(nombretemp + ":" + Convert.ToString(temporal));
                }
            }

        }

        //Evento de SELECCIÓN de un Elemento de las Condiciones Iniciales del ListBox6 
        private void listBox6_SelectedValueChanged(object sender, EventArgs e)
        {

            elementoseleccionado = 0;
           
            for (int a=0;a<listBox6.Items.Count;a++)
            {
                if (listBox6.SelectedItem == null)
                {
                    return;
                }
                if (listBox6.GetSelected(a) == true)
                {
                    elementoseleccionado = a;
                }              
            }

            textBox8.Text =Convert.ToString(listBox6.Items[elementoseleccionado]);

            //MessageBox.Show("Elemento Seleccionado de la ListBox número:  " + Convert.ToString(elementoseleccionado));
        }


        //Botón para GUARDAR el cambio de la Condición Inicial Editada 
        private void button4_Click(object sender, EventArgs e)
        {
            Double temporal = 0;
            String nombre = "";

            string MainString = textBox8.Text;
            string SearchString = ":";
            int FirstChr = MainString.IndexOf(SearchString);
            //SHOWS START POSITION OF STRING 
            //MessageBox.Show("Caracter : encontrado en la posición: " + FirstChr);

            int longitud=MainString.Length;

            string valor=MainString.Substring((FirstChr+1), longitud - (FirstChr+1));
            Double valorparametro = Convert.ToDouble(valor);
            //MessageBox.Show("Valor:"+valor);
            
            string nombreparametro = MainString.Substring(0, FirstChr);
            //MessageBox.Show("Nombre del Parámetro:" + nombreparametro);

            listBox6.Items[elementoseleccionado] = textBox8.Text;
            
            //Actualizamos el Valor y el Nombre del Parámetro Editado
            for (int n = 0; n < E.Count; n++)
            {
                if (E[n].Nombre == nombreparametro)
                {
                    //Unidades Sistema Internacional
                    if (aplicacion1.unidades == 1)
                    {
                        //PENDIENTE DE PROGRAMAR
                    }

                    //Unidades Sistema Métrico
                    else if (aplicacion1.unidades == 2)
                    {
                        String primercaracter = E[n].Nombre.Substring(0, 1);

                        if (primercaracter == "W")
                        {
                            E[n].Value = valorparametro / 0.4536;
                        }
                        else if (primercaracter == "P")
                        {
                            E[n].Value = valorparametro / (6.8947572 / 100);
                        }
                        else if (primercaracter == "H")
                        {
                            E[n].Value = valorparametro / 2.326009;
                        }

                        E[n].Nombre = nombreparametro;
                    }

                    //Unidades Sistema Británico
                    else if (aplicacion1.unidades == 0)
                    {

                    }
                }            
            }
           
          //Llamamos a la función REFRESCAR 
          button5_Click(sender, e);    
           
        }

        //Botón para mostrar las Ecuaciones del Sistema
        private void button7_Click(object sender, EventArgs e)
        {
            //Creamos en tiempo real los TextBoxes para introducir las ecuaciones
            for (int i = 0; i < (E.Count - 1); i++)
            {
                listBox1.Items.Add("Ecuación Nº" + Convert.ToString(i) + " :" + aplicacion1.ecuaciones[i]);
                //listBox2.Items.Add(E[i].ToString() + " :" + Convert.ToString(E[i].Value));
            }

            //Creamos en tiempo real los TextBoxes para introducir las ecuaciones
            for (int h = 0; h < (E.Count-1); h++)
            {
                listBox2.Items.Add(E[h].Nombre + " :" + Convert.ToString(E[h].Value));
            } 
        }

        //Cuando pulsamos ENTER en el textBox8 ejecutamos la función de GUARDAR el valor de la Condición Inicial Modificada
        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(System.ConsoleKey.Enter))
            {
                button4_Click(sender,e);
            }
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox13.Checked == true)
            {
                checkBox5.Checked = false;
                checkBox6.Checked = false;
                checkBox7.Checked = false;
                checkBox8.Checked = false;

                checkBox12.Checked = false;
                checkBox11.Checked = false;
                checkBox14.Checked = false;
                checkBox15.Checked = false;

                checkBox4.Checked = false;
                checkBox9.Checked = false;
            }
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox14.Checked == true)
            {
                checkBox5.Checked = false;
                checkBox6.Checked = false;
                checkBox7.Checked = false;
                checkBox8.Checked = false;

                checkBox12.Checked = false;
                checkBox13.Checked = false;
                checkBox11.Checked = false;
                checkBox15.Checked = false;

                checkBox4.Checked = false;
                checkBox9.Checked = false;
            }
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox15.Checked == true)
            {
                checkBox5.Checked = false;
                checkBox6.Checked = false;
                checkBox7.Checked = false;
                checkBox8.Checked = false;

                checkBox12.Checked = false;
                checkBox13.Checked = false;
                checkBox14.Checked = false;
                checkBox11.Checked = false;

                checkBox4.Checked = false;
                checkBox9.Checked = false;
            }
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox9.Checked == true)
            {
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;

                checkBox5.Checked = false;
                checkBox6.Checked = false;
                checkBox7.Checked = false;
                checkBox8.Checked = false;
                checkBox11.Checked = false;

                checkBox12.Checked = false;
                checkBox13.Checked = false;
                checkBox14.Checked = false;
                checkBox15.Checked = false;
            }
        }


        //Botón TRANSITORY SOLVING ENGNIGE
        private void button9_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
//---------------------------------------------------Tipo de Análisis TRANSITORIO -----------------------------------------------------------------------------------------------------------------------------------               
//------------------------------------------Resolución de SISTEMAS ECUACIONES DIFERENCIALES -------------------------------------------------------------------------------------------------------------------------------
            if (aplicacion1.tipoanalisis == 1)
            {        
                // Resolución de Sistema de Ecuaciones Diferenciales con la Librería TrentGuidry
                if (aplicacion1.tipoanalisistransitorio==0)
                {

                //Leemos el STEPSIZE desde el textBox13 del cuadro de diálogo del motor de cálculo
                double stepsize = 0;
                stepsize = (double)Convert.ToDouble(textBox13.Text);

                Parameter tiempo=aplicacion1.p.Find(p =>p.Nombre =="tiempo");
                                
                //Copiamos todos los parámetros de la aplicación principal guardados en "p", excepto el parámetro "tiempo" 
                int numparametrosmenostiempo = aplicacion1.p.Count - 1;
                int contador = 0;
                Parameter[] parameters1 = new Parameter[numparametrosmenostiempo];
                for (int i = 0; i < aplicacion1.p.Count; i++)
                {
                    if (aplicacion1.p[i].Nombre!= "tiempo")
                    { 
                        parameters1[contador]=aplicacion1.p[i];
                        contador++;
                    }                
                }

                //Copiamos las funciones de la aplicación principal en un array de funciones double
                int numfunciones = aplicacion1.functions.Count;
                textBox11.Text = Convert.ToString(numfunciones);
                Func<double>[] functions1 = new Func<double>[numfunciones];
                for (int i = 0; i < aplicacion1.functions.Count; i++)
                {
                    functions1[i]=aplicacion1.functions[i];
                }
               
                //Definimos el TIEMPO FINAL del intervalo de integración
                double tiempofinal = Convert.ToDouble(textBox14.Text);

                //Definimos el TIEMPO INICIAL del intervalo de integración
                double tiempoinicial = Convert.ToDouble(textBox12.Text);
                for (int i = 0; i < aplicacion1.p.Count; i++)
                {
                    if (aplicacion1.p[i].Nombre == "tiempo")
                    {
                        aplicacion1.p[i].Value = tiempoinicial;
                    }
                }                            
                

                //Runge-Kutta Orden Primero (Método de Euler)
                if (checkBox27.Checked == true)
                {
                    //Confirmamos que hemos elegido un tipo de analisis transitorio con la librería de TrentGuidry
                    aplicacion1.tipoanalisistransitorio = 0;

                    //Creamos un objeto RungeKuttaBase llamando al constructor Euler de la librería NumericalMethods.FourthBlog
                    RungeKuttaBase ode1 = new Euler();
                    double[][] dRes1 = ode1.Integrate(parameters1, tiempo, functions1, tiempofinal, stepsize);

                    //Definimos el número de iteraciones realizadas= (tiempofinal-tiempoinicial)/stepsize
                    textBox10.Text = Convert.ToString(dRes1.GetLength(0));

                    //Copiamos los resultados del Sistema de Ecuaciones Diferenciales en la vector de la aplicación principal dRes[][]
                    aplicacion1.dRes = dRes1;

                    //Llamada a la Opción del Menú de VISUALIZAR RESULTADOS DE CORRIENTES
                    aplicacion1.visualizarResultadosToolStripMenuItem_Click(sender,e);

                    //Liberamos memoria
                    ode1 = null;
                    dRes1 = null;
                }
                
                //Runge-Kutta Heun Segundo Orden
                else if (checkBox20.Checked == true)
                {
                    //Confirmamos que hemos elegido un tipo de analisis transitorio con la librería de TrentGuidry
                    aplicacion1.tipoanalisistransitorio = 0;

                    //Creamos un objeto RungeKuttaBase llamando al constructor Euler de la librería NumericalMethods.FourthBlog
                    RungeKuttaBase ode2 = new RungeKutta2Heun();
                    double[][] dRes2 = ode2.Integrate(parameters1, tiempo, functions1, tiempofinal, stepsize);

                    //Definimos el número de iteraciones realizadas= (tiempofinal-tiempoinicial)/stepsize
                    textBox10.Text = Convert.ToString(dRes2.GetLength(0));

                    //Copiamos los resultados del Sistema de Ecuaciones Diferenciales en la vector de la aplicación principal dRes[][]
                    aplicacion1.dRes = dRes2;

                    //Llamada a la Opción del Menú de VISUALIZAR RESULTADOS DE CORRIENTES
                    aplicacion1.visualizarResultadosToolStripMenuItem_Click(sender, e);

                    //Liberamos memoria
                    ode2 = null;
                    dRes2 = null;
                }

                //Runge-Kutta Ralston Segundo Orden
                else if (checkBox26.Checked == true)
                {
                    //Confirmamos que hemos elegido un tipo de analisis transitorio con la librería de TrentGuidry
                    aplicacion1.tipoanalisistransitorio = 0;

                    //Creamos un objeto RungeKuttaBase llamando al constructor Euler de la librería NumericalMethods.FourthBlog
                    RungeKuttaBase ode3 = new RungeKutta2Ralston();
                    double[][] dRes3 = ode3.Integrate(parameters1, tiempo, functions1, tiempofinal, stepsize);

                    //Definimos el número de iteraciones realizadas= (tiempofinal-tiempoinicial)/stepsize
                    textBox10.Text = Convert.ToString(dRes3.GetLength(0));

                    //Copiamos los resultados del Sistema de Ecuaciones Diferenciales en la vector de la aplicación principal dRes[][]
                    aplicacion1.dRes = dRes3;

                    //Llamada a la Opción del Menú de VISUALIZAR RESULTADOS DE CORRIENTES
                    aplicacion1.visualizarResultadosToolStripMenuItem_Click(sender, e);

                    //Liberamos memoria
                    ode3 = null;
                    dRes3 = null;
                }

                //Runge-Kutta Tercer Orden
                else if (checkBox25.Checked == true)
                {
                    //Confirmamos que hemos elegido un tipo de analisis transitorio con la librería de TrentGuidry
                    aplicacion1.tipoanalisistransitorio = 0;

                    //Creamos un objeto RungeKuttaBase llamando al constructor Euler de la librería NumericalMethods.FourthBlog
                    RungeKuttaBase ode4 = new RungeKutta3();
                    double[][] dRes4 = ode4.Integrate(parameters1, tiempo, functions1, tiempofinal, stepsize);

                    //Definimos el número de iteraciones realizadas= (tiempofinal-tiempoinicial)/stepsize
                    textBox10.Text = Convert.ToString(dRes4.GetLength(0));

                    //Copiamos los resultados del Sistema de Ecuaciones Diferenciales en la vector de la aplicación principal dRes[][]
                    aplicacion1.dRes = dRes4;

                    //Llamada a la Opción del Menú de VISUALIZAR RESULTADOS DE CORRIENTES
                    aplicacion1.visualizarResultadosToolStripMenuItem_Click(sender, e);

                    //Liberamos memoria
                    ode4 = null;
                    dRes4 = null;
                }

                //Runge-Kutta Cuarto Orden
                else if (checkBox24.Checked == true)
                {
                    //Confirmamos que hemos elegido un tipo de analisis transitorio con la librería de TrentGuidry
                    aplicacion1.tipoanalisistransitorio = 0;

                    //Creamos un objeto RungeKuttaBase llamando al constructor Euler de la librería NumericalMethods.FourthBlog
                    RungeKuttaBase ode5 = new RungeKutta4();
                    double[][] dRes5 = ode5.Integrate(parameters1, tiempo, functions1, tiempofinal, stepsize);

                    //Definimos el número de iteraciones realizadas= (tiempofinal-tiempoinicial)/stepsize
                    textBox10.Text = Convert.ToString(dRes5.GetLength(0));

                    //Copiamos los resultados del Sistema de Ecuaciones Diferenciales en la vector de la aplicación principal dRes[][]
                    aplicacion1.dRes = dRes5;

                    //Llamada a la Opción del Menú de VISUALIZAR RESULTADOS DE CORRIENTES
                    aplicacion1.visualizarResultadosToolStripMenuItem_Click(sender, e);

                    //Liberamos memoria
                    ode5 = null;
                    dRes5 = null;
                }

                //Runge-Kutta Butcher Quinto Orden
                else if (checkBox22.Checked == true)
                {
                    //Confirmamos que hemos elegido un tipo de analisis transitorio con la librería de TrentGuidry
                    aplicacion1.tipoanalisistransitorio = 0;

                    //Creamos un objeto RungeKuttaBase llamando al constructor Euler de la librería NumericalMethods.FourthBlog
                    RungeKuttaBase ode6 = new RungeKutta5Butcher();
                    double[][] dRes6 = ode6.Integrate(parameters1, tiempo, functions1, tiempofinal, stepsize);

                    //Definimos el número de iteraciones realizadas= (tiempofinal-tiempoinicial)/stepsize
                    textBox10.Text = Convert.ToString(dRes6.GetLength(0));

                    //Copiamos los resultados del Sistema de Ecuaciones Diferenciales en la vector de la aplicación principal dRes[][]
                    aplicacion1.dRes = dRes6;

                    //Llamada a la Opción del Menú de VISUALIZAR RESULTADOS DE CORRIENTES
                    aplicacion1.visualizarResultadosToolStripMenuItem_Click(sender, e);

                    //Liberamos memoria
                    ode6 = null;
                    dRes6 = null;
                }

                //Runge-Kutta Fehlberg Cuarto y Quinto Orden
                else if (checkBox19.Checked == true)
                {
                    //Confirmamos que hemos elegido un tipo de analisis transitorio con la librería de TrentGuidry
                    aplicacion1.tipoanalisistransitorio = 0;

                    //Creamos un objeto RungeKuttaBase llamando al constructor Euler de la librería NumericalMethods.FourthBlog
                    RungeKuttaBase ode7 = new RungeKutta54Fehlberg();
                    double[][] dRes7 = ode7.Integrate(parameters1, tiempo, functions1, tiempofinal, stepsize);

                    //Definimos el número de iteraciones realizadas= (tiempofinal-tiempoinicial)/stepsize
                    textBox10.Text = Convert.ToString(dRes7.GetLength(0));

                    //Copiamos los resultados del Sistema de Ecuaciones Diferenciales en la vector de la aplicación principal dRes[][]
                    aplicacion1.dRes = dRes7;

                    //Llamada a la Opción del Menú de VISUALIZAR RESULTADOS DE CORRIENTES
                    aplicacion1.visualizarResultadosToolStripMenuItem_Click(sender, e);

                    //Liberamos memoria
                    ode7 = null;
                    dRes7 = null;
                }

                }

                // Resolución de Sistema de Ecuaciones Diferenciales con la Librería DotNumerics.Net
                else if (aplicacion1.tipoanalisistransitorio==1)
                {

                    //Leemos el STEPSIZE desde el textBox13 del cuadro de diálogo del motor de cálculo
                    double stepsize = 0;
                    stepsize = (double)Convert.ToDouble(textBox13.Text);

                    //Definimos el TIEMPO FINAL del intervalo de integración
                    double tiempofinal = Convert.ToDouble(textBox14.Text);

                    //Definimos el TIEMPO INICIAL del intervalo de integración
                    double tiempoinicial = Convert.ToDouble(textBox12.Text);
                    for (int i = 0; i < aplicacion1.p.Count; i++)
                    {
                        if (aplicacion1.p[i].Nombre == "tiempo")
                        {
                            aplicacion1.p[i].Value = tiempoinicial;
                        }
                    }

                //Runge-Kutta Fehlberg Cuarto y Quinto Orden (DotNumerics)
                if (checkBox16.Checked == true)
                {                                   
                    //Declaración del objeto de la Clase OdeExplicitRungeKutta45 de la Librería DotNumerics
                    OdeExplicitRungeKutta45 odeRK1 = new OdeExplicitRungeKutta45();

                    //Asigamos las ecuaciones diferenciales explícitas 
                    OdeFunction fun = new OdeFunction(aplicacion1.luistest.ODEs);
                    
                    //Definimos las Condiciones Iniciales de las variables. Importante son 3 variables
                    int numparametros=aplicacion1.p.Count-1;
                    double[] y0 = new double[numparametros];
                    for (int i = 0; i < aplicacion1.p.Count-1; i++)
                    {
                        y0[i] = aplicacion1.p[i];
                    }

                    //Inicializamos los valores de las ecuaciones diferenciales. Importante son el número de parámetros menos uno (restamos el tiempo)
                    odeRK1.InitializeODEs(fun, aplicacion1.p.Count-1);
                                      
                    //Resolvemos el sistema de ecuaciones diferenciales explícitas
                    double[,] sol = odeRK1.Solve(y0, tiempoinicial, stepsize, tiempofinal);

                    //Definimos el número de iteraciones realizadas= (tiempofinal-tiempoinicial)/stepsize
                    textBox10.Text = Convert.ToString(sol.GetLength(0));

                    //Copiamos los resultados del Sistema de Ecuaciones Diferenciales en la vector de la aplicación principal dRes[][]
                    aplicacion1.sol = sol;

                    //Copiar a la lista de parámetros de la Aplicacion Principal los resultados 
                    //El primer bucle barre todas la iteraciones
                    for (int i = 0; i < sol.GetLength(0); i++)
                    { 
                          //El segundo bucle barre todos los parámetros
                          for (int j = 0; j < sol.GetLength(1); j++)
                          {
                              aplicacion1.p[j].Value = sol[i, j];
                          }                    
                    }

                    //Llamada a la Opción del Menú de VISUALIZAR RESULTADOS DE CORRIENTES
                    aplicacion1.visualizarResultadosToolStripMenuItem_Click(sender, e);

                    //Liberamos memoria
                    sol = null;
                    odeRK1 = null;
                    fun = null;
                }

                //Runge-Kutta Implícitas 5 Orden(DotNumerics)
                else if (checkBox17.Checked == true)
                {
                    //Declaración del objeto de la Clase OdeAdamsMoulton de la Librería DotNumerics
                    OdeImplicitRungeKutta5 odeImplic = new OdeImplicitRungeKutta5();

                    //Asigamos las ecuaciones diferenciales explícitas 
                    OdeFunction fun = new OdeFunction(aplicacion1.luistest.ODEs);

                    //Definimos las Condiciones Iniciales de las variables. Importante son 3 variables
                    int numparametros = aplicacion1.p.Count - 1;
                    double[] y0 = new double[numparametros];
                    for (int i = 0; i < aplicacion1.p.Count - 1; i++)
                    {
                        y0[i] = aplicacion1.p[i];
                    }

                    //Inicializamos los valores de las ecuaciones diferenciales. Importante son el número de parámetros menos uno (restamos el tiempo)
                    odeImplic.InitializeODEs(fun, aplicacion1.p.Count - 1);

                    //Resolvemos el sistema de ecuaciones diferenciales explícitas
                    double[,] sol = odeImplic.Solve(y0, tiempoinicial, stepsize, tiempofinal);

                    //Definimos el número de iteraciones realizadas= (tiempofinal-tiempoinicial)/stepsize
                    textBox10.Text = Convert.ToString(sol.GetLength(0));

                    //Copiamos los resultados del Sistema de Ecuaciones Diferenciales en la vector de la aplicación principal dRes[][]
                    aplicacion1.sol = sol;

                    //Copiar a la lista de parámetros de la Aplicacion Principal los resultados 
                    //El primer bucle barre todas la iteraciones
                    for (int i = 0; i < sol.GetLength(0); i++)
                    {
                        //El segundo bucle barre todos los parámetros
                        for (int j = 0; j < sol.GetLength(1); j++)
                        {
                            aplicacion1.p[j].Value = sol[i, j];
                        }
                    }

                    //Llamada a la Opción del Menú de VISUALIZAR RESULTADOS DE CORRIENTES
                    aplicacion1.visualizarResultadosToolStripMenuItem_Click(sender, e);

                    //Liberamos memoria
                    sol = null;
                    odeImplic = null;
                    fun = null;
                }

                //Runge-Kutta AdamsMoulton (DotNumerics)
                else if (checkBox18.Checked == true)
                {
                    //Declaración del objeto de la Clase OdeAdamsMoulton de la Librería DotNumerics
                    OdeAdamsMoulton odeAdams = new OdeAdamsMoulton();

                    //Asigamos las ecuaciones diferenciales explícitas 
                    OdeFunction fun = new OdeFunction(aplicacion1.luistest.ODEs);

                    //Definimos las Condiciones Iniciales de las variables. Importante son 3 variables
                    int numparametros = aplicacion1.p.Count - 1;
                    double[] y0 = new double[numparametros];
                    for (int i = 0; i < aplicacion1.p.Count - 1; i++)
                    {
                        y0[i] = aplicacion1.p[i];
                    }

                    //Inicializamos los valores de las ecuaciones diferenciales. Importante son el número de parámetros menos uno (restamos el tiempo)
                    odeAdams.InitializeODEs(fun, aplicacion1.p.Count - 1);

                    //Resolvemos el sistema de ecuaciones diferenciales explícitas
                    double[,] sol = odeAdams.Solve(y0, tiempoinicial, stepsize, tiempofinal);

                    //Definimos el número de iteraciones realizadas= (tiempofinal-tiempoinicial)/stepsize
                    textBox10.Text = Convert.ToString(sol.GetLength(0));

                    //Copiamos los resultados del Sistema de Ecuaciones Diferenciales en la vector de la aplicación principal dRes[][]
                    aplicacion1.sol = sol;

                    //Copiar a la lista de parámetros de la Aplicacion Principal los resultados 
                    //El primer bucle barre todas la iteraciones
                    for (int i = 0; i < sol.GetLength(0); i++)
                    {
                        //El segundo bucle barre todos los parámetros
                        for (int j = 0; j < sol.GetLength(1); j++)
                        {
                            aplicacion1.p[j].Value = sol[i, j];
                        }
                    }

                    //Llamada a la Opción del Menú de VISUALIZAR RESULTADOS DE CORRIENTES
                    aplicacion1.visualizarResultadosToolStripMenuItem_Click(sender, e);

                    //Liberamos memoria
                    sol = null;
                    odeAdams = null;
                    fun = null;
                }

                //Runge-Kutta GearsBDF (DotNumerics)
                else if (checkBox28.Checked == true)
                {
                    //Declaración del objeto de la Clase OdeAdamsMoulton de la Librería DotNumerics
                    OdeGearsBDF odeGearsBDF = new OdeGearsBDF();

                    //Asigamos las ecuaciones diferenciales explícitas 
                    OdeFunction fun = new OdeFunction(aplicacion1.luistest.ODEs);

                    //Definimos las Condiciones Iniciales de las variables. Importante son 3 variables
                    int numparametros = aplicacion1.p.Count - 1;
                    double[] y0 = new double[numparametros];
                    for (int i = 0; i < aplicacion1.p.Count - 1; i++)
                    {
                        y0[i] = aplicacion1.p[i];
                    }

                    //Inicializamos los valores de las ecuaciones diferenciales. Importante son el número de parámetros menos uno (restamos el tiempo)
                    odeGearsBDF.InitializeODEs(fun, aplicacion1.p.Count - 1);
                  
                    //Resolvemos el sistema de ecuaciones diferenciales explícitas
                    double[,] sol = odeGearsBDF.Solve(y0, tiempoinicial, stepsize, tiempofinal);
                

                    //Definimos el número de iteraciones realizadas= (tiempofinal-tiempoinicial)/stepsize
                    textBox10.Text = Convert.ToString(sol.GetLength(0));

                    //Copiamos los resultados del Sistema de Ecuaciones Diferenciales en la vector de la aplicación principal dRes[][]
                    aplicacion1.sol = sol;

                    //Copiar a la lista de parámetros de la Aplicacion Principal los resultados 
                    //El primer bucle barre todas la iteraciones
                    for (int i = 0; i < sol.GetLength(0); i++)
                    {
                        //El segundo bucle barre todos los parámetros
                        for (int j = 0; j < sol.GetLength(1); j++)
                        {
                            aplicacion1.p[j].Value = sol[i, j];
                        }
                    }

                    //Llamada a la Opción del Menú de VISUALIZAR RESULTADOS DE CORRIENTES
                    aplicacion1.visualizarResultadosToolStripMenuItem_Click(sender, e);

                    //Liberamos memoria
                    sol = null;
                    odeGearsBDF = null;
                    fun = null;
                }











                }
                this.Cursor = Cursors.Arrow;

                //Activamos el Botón de OK Button2
                button2.Enabled = true;
            }
        }



        private void checkBox27_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox27.Checked == true)
            {
                checkBox16.Checked = false;
                checkBox18.Checked = false;
                checkBox28.Checked = false;
                checkBox29.Checked = false;
                checkBox17.Checked = false;
                checkBox30.Checked = false;
                checkBox20.Checked = false;
                checkBox26.Checked = false;
                checkBox25.Checked = false;
                checkBox24.Checked = false;
                checkBox22.Checked = false;
                checkBox19.Checked = false;
                checkBox31.Checked = false;
            }
        }

        private void checkBox20_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox20.Checked == true)
            {
                checkBox16.Checked = false;
                checkBox18.Checked = false;
                checkBox28.Checked = false;
                checkBox29.Checked = false;
                checkBox17.Checked = false;
                checkBox30.Checked = false;
                checkBox27.Checked = false;
                checkBox26.Checked = false;
                checkBox25.Checked = false;
                checkBox24.Checked = false;
                checkBox22.Checked = false;
                checkBox19.Checked = false;
                checkBox31.Checked = false;
            }
        }

        private void checkBox26_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox26.Checked == true)
            {
                checkBox16.Checked = false;
                checkBox18.Checked = false;
                checkBox28.Checked = false;
                checkBox29.Checked = false;
                checkBox17.Checked = false;
                checkBox30.Checked = false;
                checkBox27.Checked = false;
                checkBox20.Checked = false;
                checkBox25.Checked = false;
                checkBox24.Checked = false;
                checkBox22.Checked = false;
                checkBox19.Checked = false;
                checkBox31.Checked = false;
            }
        }

        private void checkBox25_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox25.Checked == true)
            {
                checkBox16.Checked = false;
                checkBox18.Checked = false;
                checkBox28.Checked = false;
                checkBox29.Checked = false;
                checkBox17.Checked = false;
                checkBox30.Checked = false;
                checkBox27.Checked = false;
                checkBox26.Checked = false;
                checkBox20.Checked = false;
                checkBox24.Checked = false;
                checkBox22.Checked = false;
                checkBox19.Checked = false;
                checkBox31.Checked = false;
            }
        }

        private void checkBox24_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox24.Checked == true)
            {
                checkBox16.Checked = false;
                checkBox18.Checked = false;
                checkBox28.Checked = false;
                checkBox29.Checked = false;
                checkBox17.Checked = false;
                checkBox30.Checked = false;
                checkBox27.Checked = false;
                checkBox26.Checked = false;
                checkBox25.Checked = false;
                checkBox20.Checked = false;
                checkBox22.Checked = false;
                checkBox19.Checked = false;
                checkBox31.Checked = false;
            }
        }

        private void checkBox22_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox22.Checked == true)
            {
                checkBox16.Checked = false;
                checkBox18.Checked = false;
                checkBox28.Checked = false;
                checkBox29.Checked = false;
                checkBox17.Checked = false;
                checkBox30.Checked = false;
                checkBox27.Checked = false;
                checkBox26.Checked = false;
                checkBox25.Checked = false;
                checkBox24.Checked = false;
                checkBox20.Checked = false;
                checkBox19.Checked = false;
                checkBox31.Checked = false;
            }
        }

        private void checkBox19_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox19.Checked == true)
            {
                checkBox16.Checked = false;
                checkBox18.Checked = false;
                checkBox28.Checked = false;
                checkBox29.Checked = false;
                checkBox17.Checked = false;
                checkBox30.Checked = false;
                checkBox27.Checked = false;
                checkBox26.Checked = false;
                checkBox25.Checked = false;
                checkBox24.Checked = false;
                checkBox22.Checked = false;
                checkBox20.Checked = false;
                checkBox31.Checked = false;
            }
        }

        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox16.Checked == true)
            {
                checkBox20.Checked = false;
                checkBox18.Checked = false;
                checkBox28.Checked = false;
                checkBox29.Checked = false;
                checkBox17.Checked = false;
                checkBox30.Checked = false;
                checkBox27.Checked = false;
                checkBox26.Checked = false;
                checkBox25.Checked = false;
                checkBox24.Checked = false;
                checkBox22.Checked = false;
                checkBox19.Checked = false;
                checkBox31.Checked = false;
            }
        }

        private void checkBox18_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox18.Checked == true)
            {
                checkBox16.Checked = false;
                checkBox20.Checked = false;
                checkBox28.Checked = false;
                checkBox29.Checked = false;
                checkBox17.Checked = false;
                checkBox30.Checked = false;
                checkBox27.Checked = false;
                checkBox26.Checked = false;
                checkBox25.Checked = false;
                checkBox24.Checked = false;
                checkBox22.Checked = false;
                checkBox19.Checked = false;
                checkBox31.Checked = false;
            }
        }

        private void checkBox28_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox28.Checked == true)
            {
                checkBox16.Checked = false;
                checkBox18.Checked = false;
                checkBox20.Checked = false;
                checkBox29.Checked = false;
                checkBox17.Checked = false;
                checkBox30.Checked = false;
                checkBox27.Checked = false;
                checkBox26.Checked = false;
                checkBox25.Checked = false;
                checkBox24.Checked = false;
                checkBox22.Checked = false;
                checkBox19.Checked = false;
                checkBox31.Checked = false;
            }
        }

        private void checkBox29_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox29.Checked == true)
            {
                checkBox16.Checked = false;
                checkBox18.Checked = false;
                checkBox28.Checked = false;
                checkBox20.Checked = false;
                checkBox17.Checked = false;
                checkBox30.Checked = false;
                checkBox27.Checked = false;
                checkBox26.Checked = false;
                checkBox25.Checked = false;
                checkBox24.Checked = false;
                checkBox22.Checked = false;
                checkBox19.Checked = false;
                checkBox31.Checked = false;
            }
        }

        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox17.Checked == true)
            {
                checkBox16.Checked = false;
                checkBox18.Checked = false;
                checkBox28.Checked = false;
                checkBox29.Checked = false;
                checkBox20.Checked = false;
                checkBox30.Checked = false;
                checkBox27.Checked = false;
                checkBox26.Checked = false;
                checkBox25.Checked = false;
                checkBox24.Checked = false;
                checkBox22.Checked = false;
                checkBox19.Checked = false;
                checkBox31.Checked = false;
            }
        }

        private void checkBox30_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox30.Checked == true)
            {
                checkBox16.Checked = false;
                checkBox18.Checked = false;
                checkBox28.Checked = false;
                checkBox29.Checked = false;
                checkBox17.Checked = false;
                checkBox20.Checked = false;
                checkBox27.Checked = false;
                checkBox26.Checked = false;
                checkBox25.Checked = false;
                checkBox24.Checked = false;
                checkBox22.Checked = false;
                checkBox19.Checked = false;
                checkBox31.Checked = false;
            }
        }

        private void checkBox31_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox31.Checked == true)
            {
                checkBox16.Checked = false;
                checkBox18.Checked = false;
                checkBox28.Checked = false;
                checkBox29.Checked = false;
                checkBox17.Checked = false;
                checkBox30.Checked = false;
                checkBox27.Checked = false;
                checkBox26.Checked = false;
                checkBox25.Checked = false;
                checkBox24.Checked = false;
                checkBox22.Checked = false;
                checkBox19.Checked = false;
                checkBox20.Checked = false;
            }
        }






     }

    public class Win32
    {
        [DllImport("kernel32.dll")]
        public static extern Boolean AllocConsole();
        [DllImport("kernel32.dll")]
        public static extern Boolean FreeConsole();
    }

}

