using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO; //Para lectura y escritura de archivos
using System.Globalization;

//Acceso a la Librería de Diagram.Net
using System.Collections;
using Dalssoft.DiagramNet;

using NumericalMethods; //Método numérico de Newton Raphson
//using NumericalMethods.FourthBlog;  //Método numérico de Newton Raphson

using DotNumerics;
using DotNumerics.LinearAlgebra;

using WindowsFormsApplication2; 

using Files_in_csharp; //Interface lectura archivos HBAL

using ClaseEquipos;

using TablasAgua1967; //Tablas de Agua-Vapor ASME 1967

using ZedGraphSample; //Ejemplo de Ploteo de Tablas definidas por el usuario

using System.Diagnostics;

using Drag_AND_Drop_between_Forms.DotNumerics;

using CSharpScripter;

using Dalssoft.TestForm;

using Dalssoft.DiagramNet;

//Tablas AGUA IAPWS 1997 
using Tablas_Vapor_ASME;

using System.Diagnostics;

using Excel = Microsoft.Office.Interop.Excel;

using System.Windows.Forms.DataVisualization.Charting;

using CompiladorLUISCOCO;

using CSharpScripter2;

using System.Runtime.InteropServices;

using DasslInterface;

using Monofasico;

using Bifasico;

using HeatExchangers;

namespace Drag_AND_Drop_between_Forms
{
    public partial class Aplicacion : Form
    {
        private bool changeDocumentProp = true;

        //Ruta del archivo *.res de resultados de HBAL
        public String rutaresultadoshbal;

        public Double numcorrientes = 0;
        public List<Parameter> p = new List<Parameter>();
        public List<Parameter> p1 = new List<Parameter>();
        public List<Parameter> p5 = new List<Parameter>();
        public Parameter ptemp = new Parameter(0.0, "luis");
        public List<Func<Double>> functions = new List<Func<Double>>();

        //puntero para acceso a las funciones de las Tablas de Agua-Vapor ASME 1967 
        public Class1 acceso = new Class1();
        
        //DATOS GENERALES 
        //Título del Archivo
        public String Titulo = "";
        //Nombre del Archivo de entrada de Datos *.dat
        public String NombreArchivo = "";
        //Número Total de Equipos
        public Double NumTotalEquipos = 0;
        //Número Total de Corrientes
        public Double NumTotalCorrientes = 0;
        //Número Máximo de Iteraciones
        public Double NumMaxIteraciones = 200;
        //Número Total de Tablas
        public Double NumTotalTablas = 0;
        //Error Máximo Admisible
        public Double ErrorMaxAdmisible = 0;
        //Datos Iniciales Buenos 
        public Double DatosIniciales = 0;
        //Factor de Iteraciones (EPS)
        public Double FactorIteraciones = 0;
        //Fichero Iteraciones Intermedias
        public Double FicheroIteraciones = 0;
        //Unidades
        //Sistema Britanico=0;Sistema Internacional=1;Sistema Métrico=2
        public Double unidades = 0;

        //Objeto para escritura de archivos
        StreamWriter fl;

        //Numero de equipos
        public Int16 numequipos = 0;

        //Tipo de equipo arrastrado
        public Int16 tipoequipodrag;

        //Numero de ecuaciones
        public Double numecuaciones;

        //Numero de varibales
        public Double numvariables;

        //Lista de Picture Box que representan a los equipos en el Diagrama
        //public ArrayList equipos = new ArrayList();

        //Lista de cadenas que guardan las ecuaciones del sistema
        public List<String> ecuaciones = new List<String>();

        //Lista de la Clase Equipos que guarda todos los datos introducidos por el usuario sobre los diferentes equipos creados
        public List<Equipos> equipos11 = new List<Equipos>();

        //Lista de cadenas para general el archivo de lectura de HBAL incluyendo los datos de entrada *.dat
        public List<String> Hbalfile = new List<String>();

        //Número de Condiciones Iniciales
        public Double numcondiciniciales = 0;

        //Lista de Matrices 2x2 Double para guardar las Tablas definidas por el usuario
        public List<Double[,]> listaTablas = new List<Double[,]>();

        //Lista de Número de Tablas
        public List<Double> listanumTablas = new List<Double>();

        //Lista de Número de Datos en cada Tabla
        public List<Double> listanumDatosenTabla = new List<Double>();

        //Lista Títulos de Tablas
        public List<String> listaTituloTabla = new List<String>();

        //Lista de Tipo de Iterpolación de Tablas
        public List<Double> listanumTipoInterpolacionTabla = new List<Double>();

        //Lista de Título EjeX de Tablas
        public List<String> listaTituloEjeXTabla = new List<String>();

        //Lista de Título EjeY de Tablas
        public List<String> listaTituloEjeYTabla = new List<String>();

        //Lista de Unidades EjeX de Tablas


        //Lista de Unidades EjeY de Tablas

        public Double[] caudalinicial = new Double[10000];
        public Double[] presioninicial = new Double[10000];
        public Double[] entalpiainicial = new Double[10000];
        public Double[] numcorrienteinicial = new Double[10000];


        //Incluir las CONDICIONES INICIALES (Si 1, No 0) para escritura de archivo de entrada de Datos de HBAL *.DAT
        public int incluircondicionesiniciales = 0;

        //Leidas CONDICIONES INICIALES del archivo de entrada de Datos de HBAL *.DAT
        public int leidascondicionesiniciales = 0;

        //Objeto para lle
        StreamReader fl1;

        //Matriz del SISTEMA: matriz Auxiliar para calcular la Jacobiana: filas(ecuaciones), columnas(variables/parámetros)
        public Double[,] matrizauxjacob = new Double[2, 2];

        //Resultados del Método Numérico de resolución del Sistema de Ecuaciones No Lineales (Descomposición LU,método Newton Rapson, Método Broyden, etc)
        public Double[, ,] jacobianXn;
        public Double[, ,] jacobianinversoXnmenos1;
        public Double[, ,] jacobianinversoXn;

        public Double[, ,] Xn;
        public Double[, ,] Xnmenos1;
        public Double[, ,] functionXn;

        //Marca de Segundos Cálculos. Es decir, no hace falta generar de nuevo ni ecuaciones ni parámetros ni leer las condiciones iniciales, sino solo hace falta mostrar el Motor de Cálculo
        public int marca = 0;

        //Variable que indica si queremos guardar las iteraciones intermedias: Si=1, No=0
        public int guardarintermedias4 = 0;

        //Variable Tipo de Cálculo realizado: Jacobiana Inversa (0), Jacobiana (1).
        public int tipocalculo = 0;

        //Marca de Ejemplo de Validación: Si 1, No 0
        public int ejemplovalidacion = 0;

        //Tipo de Elemento elegido para dibujar el Diagrama del Balance Térmico
        public int indiceimagen = 0;
        public int contadorindices = 0;
        public ListView.SelectedIndexCollection luis;
        public ListView.SelectedIndexCollection maria;

        //RESULTADOS DE LOS EQUIPOS
        //Lista Resultados de Equipos en cada Corrida de Cálculo
        public List<ClassCondicionContorno1> condiciones1 = new List<ClassCondicionContorno1>();
        public List<ClassDivisor2> divisores2 = new List<ClassDivisor2>();
        public List<ClassPerdidaCarga3> perdidas3 = new List<ClassPerdidaCarga3>();
        public List<ClassBomba4> bombas4 = new List<ClassBomba4>();
        public List<ClassMezclador5> mezcladores5 = new List<ClassMezclador5>();
        public List<ClassReactor6> reactores6 = new List<ClassReactor6>();
        public List<ClassCalentador7> calentadores7 = new List<ClassCalentador7>();
        public List<ClassCondensador8> condensadores8 = new List<ClassCondensador8>();
        public List<ClassTurbina9> turbinas9 = new List<ClassTurbina9>();
        public List<ClassTurbina10> turbinas10 = new List<ClassTurbina10>();
        public List<ClassTurbina11> turbinas11 = new List<ClassTurbina11>();
        public List<ClassSeparadorHumedad13> sephumedadlista13 = new List<ClassSeparadorHumedad13>();
        public List<ClassMSR14> msrlista14 = new List<ClassMSR14>();
        public List<ClassCondensador15> condensadorlista15 = new List<ClassCondensador15>();
        public List<ClassEnfriadorDrenajes16> enfriadorlista16 = new List<ClassEnfriadorDrenajes16>();
        public List<ClassAtemperador17> atemperadores17 = new List<ClassAtemperador17>();
        public List<ClassDesaireador18> desaireadores18 = new List<ClassDesaireador18>();
        public List<ClassValvula19> valvulas19 = new List<ClassValvula19>();
        public List<ClassDivisorEntalpiaFija20> divisoresentalpia20 = new List<ClassDivisorEntalpiaFija20>();
        public List<ClassTanqueVaporizacion21> tanquesvaporizacion21 = new List<ClassTanqueVaporizacion21>();
        public List<ClassIntercambiador22> intercambiadores22 = new List<ClassIntercambiador22>();

        //Matrices de Resultados de los Equipos de las diferentes Corridas de Cálculo
        //Estamos limitando el número de Equipos a 2000 y el número de setnumber (número de cálculos del análisis de sensibilidad) a 200
        public ClassCondicionContorno1[,] matrixcondicioncontorno1 = new ClassCondicionContorno1[2000, 200];
        public ClassDivisor2[,] matrixdivisor2 = new ClassDivisor2[2000, 200];
        public ClassPerdidaCarga3[,] matrixperdida3 = new ClassPerdidaCarga3[2000, 200];
        public ClassBomba4[,] matrixbomba4 = new ClassBomba4[2000, 200];
        public ClassMezclador5[,] matrixmezclador5=new ClassMezclador5[2000,200];
        public ClassReactor6[,] matrixreactor6=new ClassReactor6[2000,200];
        public ClassCalentador7[,] matrixcalentador7=new ClassCalentador7[2000,200];
        public ClassCondensador8[,] matrixcondensador8=new ClassCondensador8[2000,200];
        public ClassTurbina9[,] matrixturbina9=new ClassTurbina9[2000,200];
        public ClassTurbina10[,] matrixturbina10=new ClassTurbina10[2000,200];
        public ClassTurbina11[,] matrixturbina11=new ClassTurbina11[2000,200];
        public ClassSeparadorHumedad13[,] matrixseparador13=new ClassSeparadorHumedad13[2000,200];
        public ClassMSR14[,] matrixMSR14=new ClassMSR14[2000,200];
        public ClassCondensador15[,] matrixcondensador15=new ClassCondensador15[2000,200];
        public ClassEnfriadorDrenajes16[,] matrixenfriador16=new ClassEnfriadorDrenajes16[2000,200];
        public ClassAtemperador17[,] matrixatemperador17=new ClassAtemperador17[2000,200];
        public ClassDesaireador18[,] matrixdesaireador18=new ClassDesaireador18[2000,200];
        public ClassValvula19[,] matrixvalvula19=new ClassValvula19[2000,200];
        public ClassDivisorEntalpiaFija20[,] matrixdivisor20=new ClassDivisorEntalpiaFija20[2000,200];
        public ClassTanqueVaporizacion21[,] matrixtanque21=new ClassTanqueVaporizacion21[2000,200];
        public ClassIntercambiador22[,] matrixintercambiador22=new ClassIntercambiador22[2000,200];
        
        //Número de los diferentes Tipos de Equipos en el Modelo
        public int numtipo1 = 0;
        public int numtipo2 = 0;
        public int numtipo3 = 0;
        public int numtipo4 = 0; 
        public int numtipo5 = 0;
        public int numtipo6 = 0;
        public int numtipo7 = 0;
        public int numtipo8 = 0;
        public int numtipo9 = 0;
        public int numtipo10 = 0;
        public int numtipo11 = 0;
        public int numtipo12 = 0;
        public int numtipo13 = 0;
        public int numtipo14= 0;
        public int numtipo15 = 0;
        public int numtipo16 = 0;
        public int numtipo17 = 0;
        public int numtipo18 = 0;
        public int numtipo19 = 0;
        public int numtipo20 = 0;
        public int numtipo21 = 0;
        public int numtipo22 = 0;

        
        //Estamos limitando el número de Corrientes a 2000 y el número de setnumber (número de cálculos del análisis de sensibilidad) a 200
        public Parameter[,] listaresultadoscorrientes = new Parameter[2000, 200];
                
        //Número de Cálculo (Set Number)
        public int setnumber = 1;

        //Consola del Motor de Calculo: activada=1, desactivada =0. Desactivamos la Consola (vista de iteraciones intermedias en MS-DOS) porque para compilación en tiempo real no funciona bien y no sé porqué.
        public int consola = 1;

        //Indicador de Archivo cargado de HBAL (Si=1; No=0)
        public int archivocargado = 0;

        //Indicador de Borrar las Matrices de Resultados de Equipos o Mantener su contenido para el Nuevo Cálculo (Si=1; No=0)
        public int borrarmatrixequipos = 0;
        
        //Indicador de Tipo de Análisis: Estacionario (0), Transitorio(1)
        public int tipoanalisis = 0;

        //Indicador de Tipo de Análsis Estacionario: Sistema Ecuaciones No Lineales (0), Sistema Ecuaciones Lineales (1)
        public int tipoanalisisestacionario = 0;

        //Indicador de Tipo de Análsis Transitorio: TrentGuidry Library (0), DotNumerics Library (1)
        public int tipoanalisistransitorio = 0;

        //Solución de Ecuacione Diferenciales con la librería de TrentGuidry
        public double[][] dRes;

        //Solución de Ecuacione Diferenciales con la librería de DotNumerics
        public double[,] sol;

        //Puntero al cuadro de diálogo de las Tablas para acceder a su listbox y añadir los puntos de interpolación y el cálculo de interpolación
        public Tablaluis punterodialogotabla1;

        //Puntero al cuadro de diálogo de los Ejemplos del Solving Engine 
        public FormMain dialogoejemplos;

        //Ejecución de ODE: Copiamos el método de la Clase Compilada en Runtime para llamarla desde el Motor de Cálculo
        public Command2 luistest;

        public Aplicacion()
        {
            numecuaciones = 0;
            numvariables = 0;
            numequipos = 0;
            tipoequipodrag = 0;

            InitializeComponent();
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //Esta función es llamada cuando se carga la ventana de la Aplicación principal
        private void Form2_Load(object sender, EventArgs e)
        {
            //Añadir un número secuencial del cálculo realizado
            comboBox3.Items.Add(setnumber);

            About luiscocosoftware = new About();
            luiscocosoftware.ShowDialog();

            //Elegimos el tabPage5 (Heat Balance General Information) del Control tabControl2 de la Aplicación principal
            tabControl2.SelectedTab = tabPage5;
            
            listView4.AllowDrop = true;

            //DIGRAM.NET
            Edit_UpdateUndoRedoEnable();
                       
            //Enviamos los puntero de a la Aplicación Principal a los controles de dibujo "designer1" y "designer2"
            designer1.punteroaplicacion = this;
            designer2.punteroaplicacion = this;
            

            //Confirmamos que la Configuracion Regional de Windows tiene como Símbolo Decimal el PUNTO '.'
            System.Globalization.NumberFormatInfo infoformatodecimal = new NumberFormatInfo();
            if (infoformatodecimal.NumberDecimalSeparator == ",")
            {
                MessageBox.Show("Atención la Configuración Regional de Window utiliza como Simbolo Decimal la coma ',' este programa necesita que la cambie al punto '.' ");
            }
            //infoformatodecimal.NumberDecimalSeparator = ".";

            //MUY IMPORTANTE ACTIVAR ESTA PROPIEDAD PARA PERMITIR QUE LOS CONTROLES RECIBAN OBJETOS ARRASTRADOS (dragged objects)

            this.AllowDrop = true;
            //Número Máximo de Iteraciones
            NumMaxIteraciones = 20;
            //Error Máximo Admisible
            ErrorMaxAdmisible = 1E-5;
            //Datos Iniciales Buenos 
            DatosIniciales = 0;
            //Factor de Iteraciones (EPS)
            FactorIteraciones = 0.5;
            //Fichero Iteraciones Intermedias
            FicheroIteraciones = 1;
            //Unidades
            //Sistema Métrico=2
            unidades = 2;
            sistemaMétricoToolStripMenuItem_Click(sender, e);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        private void Form2_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Button)))
                e.Effect = DragDropEffects.Move;
            else e.Effect = DragDropEffects.None;
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //Drag and Drop Equipos sobrel el Form
        private void Form2_DragDrop(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent(typeof(Button)))
            {
                if (this.tipoequipodrag == 1)
                {
                    //this.Text = "Hola SOY LUIS COCO";

                    //this.equipos.Add(new PictureBox());
                    //((PictureBox)this.equipos[numequipos]).Location = new System.Drawing.Point(e.X, e.Y);
                    //((PictureBox)this.equipos[numequipos]).Name = "PictureBox" + Convert.ToString(numequipos);
                    //((PictureBox)this.equipos[0]).Size = new System.Drawing.Size(100, 50);
                    //((PictureBox)this.equipos[numequipos]).TabIndex = 2;
                    //((PictureBox)this.equipos[numequipos]).Image = System.Drawing.Bitmap.FromFile("C:\\Users\\luis\\Desktop\\Iconos PEPSE\\Fuente.bmp");
                    //((PictureBox)this.equipos[numequipos]).SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
                    //this.Controls.Add(((PictureBox)this.equipos[numequipos]));

                    //Incrementamos en una unidad el número de equipos
                    numequipos++;
                    //Llamada al cuadro de diálogo que crea las ecuaciones del equipo e inicializa sus valores
                    //Enviamos al cuadro de dialogo el puntero de la ventana principal para guardar los datos del cuadro de dialogo en variables de la aplicacion principal

                    Condcontorno Condcontorno1 = new Condcontorno(this, numecuaciones, numvariables, 0, 0);
                    Condcontorno1.Show();
                }

                else if (this.tipoequipodrag == 2)
                {
                    //Incrementamos en una unidad el número de equipos
                    numequipos++;
                    //Llamada al cuadro de diálogo que crea las ecuaciones del equipo e inicializa sus valores
                    //Enviamos al cuadro de dialogo el puntero de la ventana principal para guardar los datos del cuadro de dialogo en variables de la aplicacion principal
                    Divisor Divisor1 = new Divisor(this, numecuaciones, numvariables,0,0);
                    Divisor1.Show();
                }

                else if (this.tipoequipodrag == 3)
                {
                    //numequipos++;

                    //Llamada al cuadro de diálogo que crea las ecuaciones del equipo e inicializa sus valores
                    //Enviamos al cuadro de dialogo el puntero de la ventana principal para guardar los datos del cuadro de dialogo en variables de la aplicacion principal
                    //Perdidacarga Perdidacarga1 = new Perdidacarga(this, numecuaciones, numvariables);
                    //Perdidacarga1.Show();

                    ListaPerdidaCarga listperdcarga = new ListaPerdidaCarga(this);
                    listperdcarga.Show();
                }

                else if (this.tipoequipodrag == 4)
                {
                    //numequipos++;

                    //Llamada al cuadro de diálogo que crea las ecuaciones del equipo e inicializa sus valores
                    //Enviamos al cuadro de dialogo el puntero de la ventana principal para guardar los datos del cuadro de dialogo en variables de la aplicacion principal
                    //Bomba bomba1 = new Bomba(this, numecuaciones, numvariables);
                    //bomba1.Show();
                    ListaBomba listbomba = new ListaBomba(this);
                    listbomba.Show();
                }


                else if (this.tipoequipodrag == 13)
                {
                    numequipos++;

                    //Llamada al cuadro de diálogo que crea las ecuaciones del equipo e inicializa sus valores
                    //Enviamos al cuadro de dialogo el puntero de la ventana principal para guardar los datos del cuadro de dialogo en variables de la aplicacion principal
                    Sephumedad Sephumedad1 = new Sephumedad(this, numecuaciones, numvariables,0,0);
                    Sephumedad1.Show();
                }

                else if (this.tipoequipodrag == 5)
                {

                    numequipos++;

                    //Llamada al cuadro de diálogo que crea las ecuaciones del equipo e inicializa sus valores
                    //Enviamos al cuadro de dialogo el puntero de la ventana principal para guardar los datos del cuadro de dialogo en variables de la aplicacion principal
                    Mezclador Mezclador1 = new Mezclador(this, numecuaciones, numvariables,0,0);
                    Mezclador1.Show();
                }

                else if (this.tipoequipodrag == 9)
                {
                    //numequipos++;

                    //Llamada al cuadro de diálogo que crea las ecuaciones del equipo e inicializa sus valores
                    //Enviamos al cuadro de dialogo el puntero de la ventana principal para guardar los datos del cuadro de dialogo en variables de la aplicacion principal
                    //Turbina Turbina1 = new Turbina(this, numecuaciones, numvariables);
                    //Turbina1.Show();

                    ListaTurbina9 listaturbina9 = new ListaTurbina9(this);
                    listaturbina9.Show();
                }

                else if (this.tipoequipodrag == 14)
                {

                    numequipos++;

                    //Llamada al cuadro de diálogo que crea las ecuaciones del equipo e inicializa sus valores
                    //Enviamos al cuadro de dialogo el puntero de la ventana principal para guardar los datos del cuadro de dialogo en variables de la aplicacion principal
                    MSR MSR1 = new MSR(this, numecuaciones, numvariables,0,0);
                    MSR1.Show();
                }

                else if (this.tipoequipodrag == 7)
                {
                    numequipos++;

                    //Llamada al cuadro de diálogo que crea las ecuaciones del equipo e inicializa sus valores
                    //Enviamos al cuadro de dialogo el puntero de la ventana principal para guardar los datos del cuadro de dialogo en variables de la aplicacion principal
                    Calentador calentador1 = new Calentador(this, numecuaciones, numvariables,0,0);
                    calentador1.Show();
                }

                else if (this.tipoequipodrag == 10)
                {

                    numequipos++;

                    //Llamada al cuadro de diálogo que crea las ecuaciones del equipo e inicializa sus valores
                    //Enviamos al cuadro de dialogo el puntero de la ventana principal para guardar los datos del cuadro de dialogo en variables de la aplicacion principal
                    Turbina10 Turbina2 = new Turbina10(this, numecuaciones, numvariables,0,0);
                    Turbina2.Show();

                }

                else if (this.tipoequipodrag == 8)
                {

                    numequipos++;

                    //Llamada al cuadro de diálogo que crea las ecuaciones del equipo e inicializa sus valores
                    //Enviamos al cuadro de dialogo el puntero de la ventana principal para guardar los datos del cuadro de dialogo en variables de la aplicacion principal
                    Condensadorprincipal condensadorprincipal1 = new Condensadorprincipal(this, numecuaciones, numvariables);
                    condensadorprincipal1.Show();
                }

                else
                {
                    MessageBox.Show("Hola LUIS COCO Tipo de equipo no reconocido");
                }
            }

            else
            {
                MessageBox.Show("Hola LUIS COCO Tipo de control arrastrado no reconcido");
            }
        }


        //Opción del Menú de visualizar la Paleta de Controles
        private void paletaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Paletaequipos luis = new Paletaequipos(this);
            luis.Show();
        }

        //Opción Menu Condición Contorno
        private void condiciónContornoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //numequipos++;

            //Llamada al cuadro de diálogo que crea las ecuaciones del equipo e inicializa sus valores
            //Enviamos al cuadro de dialogo el puntero de la ventana principal para guardar los datos del cuadro de dialogo en variables de la aplicacion principal
            //Condcontorno cond1 = new Condcontorno(this, numecuaciones, numvariables);
            //cond1.Show();
            ListaCondContorno cond1 = new ListaCondContorno(this);
            cond1.Show();
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------

        private void formulaciónASME1967ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ASME1967 tablaasme = new ASME1967();
            tablaasme.Show();

        }

        private void sistemaBritánicoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sistemaBritánicoToolStripMenuItem.Checked = true;
            sistemaMétricoToolStripMenuItem.Checked = false;
            sitemaInternacionalToolStripMenuItem.Checked = false;

            unidades = 0;

        }

        private void sitemaInternacionalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sitemaInternacionalToolStripMenuItem.Checked = true;
            sistemaBritánicoToolStripMenuItem.Checked = false;
            sistemaMétricoToolStripMenuItem.Checked = false;

            unidades = 1;
        }

        private void sistemaMétricoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sistemaMétricoToolStripMenuItem.Checked = true;
            sitemaInternacionalToolStripMenuItem.Checked = false;
            sistemaBritánicoToolStripMenuItem.Checked = false;

            unidades = 2;
        }

        //Opción del Menú de RESOLUCIÓN MANUAL del Sistema de Ecuaciones
        public void resolverSistemaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Caso de haber Creado un EJEMPLO de Validación del Motor de Cálculo
            if (ejemplovalidacion == 1)
            {
                //Desactivamos la vista Consola (Iteraciones Intermedias en MS-DOS)
                consola = 0;

                Motorcalculo motorcalculo2 = new Motorcalculo();
                motorcalculo2.punteroaplicacion(this);
                motorcalculo2.ShowDialog();
                return;            
            }

            //No hace falta crear las ecuaciones, los parámetros y leer las condiciones iniciales. Y sólo es necesario mostrar el motor de cálculo
            else if (marca == 0)
            {
                //Activamos la vista Consola (Iteraciones Intermedias en MS-DOS)
                consola = 1;

                //Inicializamos para realizar cálculos sucesivos
                numcorrientes = 0;
                p.Clear();
                p1.Clear();
                functions.Clear();

                //Generar los parámeteros (variables) correpondiente a la lista de objetos de equipos11 generada por los form de cada tipo de equipo
                for (int t = 0; t < equipos11.Count; t++)
                {
                    generarparametros(equipos11[t].tipoequipo2, equipos11[t].aN1, equipos11[t].aN2, equipos11[t].aN3, equipos11[t].aN4, equipos11[t].aD1, equipos11[t].aD2, equipos11[t].aD3, equipos11[t].aD4, equipos11[t].aD5, equipos11[t].aD6, equipos11[t].aD7, equipos11[t].aD8, equipos11[t].aD9, equipos11[t].adicional11, equipos11[t].adicional12, equipos11[t].adicional13, equipos11[t].adicional14);
                }

                //Utilizamos el array de parametro p1 como array auxiliar para los criterios de busqueda de parametros
                p1 = p;


                //Generar las Condiciones Iniciales del Sistema para la Primera Iteración del Método de Newton Raphson

                //Tendremos que tener en cuenta las siguientes consideraciones:
                // a) Caso de haber leido todas las condiciones iniciales de un archivo HBAL (*.DAT)
                // b) Caso de haber leido parte de las condiciones iniciales de un archivo de HBAL (*.DAT)
                // c) Caso de no haber leido ninguna condición inicial de un archivo de HBAL (*.DAT)

                //Caso a) Haber leido todas las condiciones iniciales de un archivo HBAL (*.DAT)
                if ((leidascondicionesiniciales == 1) && (numcondiciniciales == numcorrientes))
                {
                    //ALGORITMO PARA ORDENAR LAS CONCIONES INICIALES
                    //De todas formas las Condiciones Iniclaes ya han sido ordenadas mediante el Algoritmo de LecturaHabal 
                    //Tenemos dos arrays: el numcorrienteinicial[a] y los parámetros del sistema p[j]
                    //Primero: Leer el número de numcorrienteinicial[a] con índice "a" (Bucle leyendo numcorrienteinicial[a])
                    //Segundo: Buscar el número de parámetro que coincida con el numcorrientinicial[a] (Bucle leyendo los parámetros para buscar el que coincida con el numcorrienteinicial[a])
                    //Tercero: Cuando coincidan el numcorrienteinicial[a] igual al número de parámetro, copiar el las condiciones iniciales en los valores del parámetro.

                    for (int a = 0; a < numcorrientes; a++)
                    {
                        for (int j = 0; j < p.Count - 2; j++)
                        {
                            //Leemos el Nombre del parámetro, por ejemplo: W1
                            String nom = p[j].Nombre;
                            //Calculaos la longitud del nombre del parámetro, por ejemplo: 2
                            int longitud = nom.Length;
                            //Introducimos en una variable 
                            String tem = nom.Substring(1, longitud - 1);
                            //Introducimos en la variable "tipoparametro" el nombre de parámetro: W
                            String tipoparametro = nom.Substring(0, 1);
                            //Introducimos  en la variable "numcorr" el número de corriente: 1
                            Double numcorr = Convert.ToDouble(tem);

                            //Chequeamos que el número de corriente del parámetro coincide con el número de corriente inicial
                            //Copiamos los valores inciales de la corriente (caudal,presión y entalpía) en los parámetros del programa 

                            if (numcorrienteinicial[a] == numcorr)
                            {
                                p[j].Value = caudalinicial[a];
                                p[j + 1].Value = presioninicial[a];
                                p[j + 2].Value = entalpiainicial[a];
                                j = p.Count;
                            }
                        }

                        //Copiamos las Condiciones iniciales leidas del Archivo de HBAL *.DAT en los Parámeteros Generados
                        //int cont = 0;
                        //int cont1 = 1;
                        //int cont2 = 2;

                        //for (int a = 0; a < numcorrientes; a++)
                        //{
                        //    p[a + cont].Value = caudalinicial[a];
                        //    p[a + cont1].Value = presioninicial[a];
                        //    p[a + cont2].Value = entalpiainicial[a];

                        //    cont = cont + 2;
                        //    cont1 = cont1 + 2;
                        //    cont2 = cont2 + 2;
                        //}

                        
                    }
                }

                //ESTUDIAR EL ERROR 
                //Caso b) Haber leido parte de las condiciones iniciales de un archivo de HBAL (*.DAT)
                else if ((leidascondicionesiniciales == 1) && (numcondiciniciales != numcorrientes))
                {
                    //ALGORITMO PARA ORDENAR LAS CONCIONES INICIALES
                    //De todas formas las Condiciones Iniclaes ya han sido ordenadas mediante el Algoritmo de LecturaHabal 
                    //Tenemos dos arrays: el numcorrienteinicial[a] y los parámetros del sistema p[j]
                    //Primero: Leer el número de numcorrienteinicial[a] con índice "a" (Bucle leyendo numcorrienteinicial[a])
                    //Segundo: Buscar el número de parámetro que coincida con el numcorrientinicial[a] (Bucle leyendo los parámetros para buscar el que coincida con el numcorrienteinicial[a])
                    //Tercero: Cuando coincidan el numcorrienteinicial[a] igual al número de parámetro, copiar el las condiciones iniciales en los valores del parámetro.

                    for (int a = 0; a < numcorrientes; a++)
                    {
                        for (int j = 0; j < p.Count - 2; j++)
                        {
                            //Leemos el Nombre del parámetro, por ejemplo: W1
                            String nom = p[j].Nombre;
                            //Calculaos la longitud del nombre del parámetro, por ejemplo: 2
                            int longitud = nom.Length;
                            //Introducimos en una variable 
                            String tem = nom.Substring(1, longitud - 1);
                            //Introducimos en la variable "tipoparametro" el nombre de parámetro: W
                            String tipoparametro = nom.Substring(0, 1);
                            //Introducimos  en la variable "numcorr" el número de corriente: 1
                            Double numcorr = Convert.ToDouble(tem);

                            //Chequeamos que el número de corriente del parámetro coincide con el número de corriente inicial
                            //Copiamos los valores inciales de la corriente (caudal,presión y entalpía) en los parámetros del programa 

                            if (numcorrienteinicial[a] == numcorr)
                            {
                                p[j].Value = caudalinicial[a];
                                p[j + 1].Value = presioninicial[a];
                                p[j + 2].Value = entalpiainicial[a];
                                j = p.Count;
                            }
                        }
                    }
                }


                //Caso c)No haber leido ninguna condición inicial de un archivo de HBAL (*.DAT)
                else
                {
                    for (int i = 0; i < equipos11.Count; i++)
                    {
                        //generarcondicionesiniciales(equipos11[i].tipoequipo2, equipos11[i].aN1, equipos11[i].aN2, equipos11[i].aN3, equipos11[i].aN4, equipos11[i].aD1, equipos11[i].aD2, equipos11[i].aD3, equipos11[i].aD4, equipos11[i].aD5, equipos11[i].aD6, equipos11[i].aD7, equipos11[i].aD8, equipos11[i].aD9, equipos11[i].adicional11, equipos11[i].adicional12, equipos11[i].adicional13, equipos11[i].adicional14);
                    }
                }

                int dimen = p.Count;

                Double[,] matrizauxjacob1 = new Double[dimen, dimen];

                matrizauxjacob = matrizauxjacob1;
                int contecuaciones = 0;

                listView1.Items.Clear();
                listView2.Items.Clear();
                listView3.Items.Clear();

                int temporal = 0;
                int ecuacfaltan = 0;
                int ecuacsobran = 0;

                //Generar Ecuaciones de cada uno de los equipos de la lista equipos11 cuando ya tenemos toda la lista de parámetros creadas y con los nombres de las corrientes asignados
                for (int i = 0; i < equipos11.Count; i++)
                {
                    contecuaciones = generarecuaciones(contecuaciones, equipos11[i].tipoequipo2, equipos11[i].aN1, equipos11[i].aN2, equipos11[i].aN3, equipos11[i].aN4, equipos11[i].aD1, equipos11[i].aD2, equipos11[i].aD3, equipos11[i].aD4, equipos11[i].aD5, equipos11[i].aD6, equipos11[i].aD7, equipos11[i].aD8, equipos11[i].aD9, equipos11[i].adicional11, equipos11[i].adicional12, equipos11[i].adicional13, equipos11[i].adicional14);

                    //Estudiamos el Primer elemento de la Lista
                    if (i == 0)
                    {
                        if ((contecuaciones - temporal) < 3)
                        {
                            //listView2.Items[i].BackColor = Color.Red;
                            listView2.Items.Add("Equipo: " + Convert.ToString(equipos11[i].numequipo2) + " Tipo: " + Convert.ToString(equipos11[i].tipoequipo2) + " Ecuac.: " + Convert.ToString(contecuaciones - temporal));
                            ecuacfaltan = ecuacfaltan + (contecuaciones - temporal);
                        }
                        else if ((contecuaciones - temporal) == 3)
                        {
                            //listView1.Items[i].BackColor = Color.White;
                            listView1.Items.Add("Equipo: " + Convert.ToString(equipos11[i].numequipo2) + " Tipo: " + Convert.ToString(equipos11[i].tipoequipo2) + " Ecuac.: " + Convert.ToString(contecuaciones - temporal));
                        }
                        else if ((contecuaciones - temporal) == 5)
                        {
                            listView2.Items.Add("Equipo: " + Convert.ToString(equipos11[i].numequipo2) + " Tipo: " + Convert.ToString(equipos11[i].tipoequipo2) + " Ecuac.: " + Convert.ToString(contecuaciones - temporal));
                            //listView1.Items[i].BackColor = Color.Cyan;
                            ecuacfaltan = ecuacfaltan + (contecuaciones - temporal);
                        }
                        else if ((contecuaciones - temporal) > 6)
                        {
                            listView3.Items.Add("Equipo: " + Convert.ToString(equipos11[i].numequipo2) + " Tipo: " + Convert.ToString(equipos11[i].tipoequipo2) + " Ecuac.: " + Convert.ToString(contecuaciones - temporal));
                            //listView1.Items[i].BackColor = Color.Green;
                            ecuacsobran = ecuacsobran + (contecuaciones - temporal);
                        }
                        else if ((contecuaciones - temporal) == 6)
                        {
                            listView1.Items.Add("Equipo: " + Convert.ToString(equipos11[i].numequipo2) + " Tipo: " + Convert.ToString(equipos11[i].tipoequipo2) + " Ecuac.: " + Convert.ToString(contecuaciones - temporal));
                            //listView1.Items[i].BackColor = Color.White;
                        }
                        else if ((contecuaciones - temporal) == 4)
                        {
                            listView3.Items.Add("Equipo: " + Convert.ToString(equipos11[i].numequipo2) + " Tipo: " + Convert.ToString(equipos11[i].tipoequipo2) + " Ecuac.: " + Convert.ToString(contecuaciones - temporal));
                            //listView1.Items[i].BackColor = Color.Green;
                            ecuacsobran = ecuacsobran + (contecuaciones - temporal);
                        }
                        temporal = contecuaciones;
                    }

                    //Estudiamos el resto de elementos de la Lista
                    else
                    {
                        if ((contecuaciones - temporal) < 3)
                        {
                            //listView2.Items[i].BackColor = Color.Red;
                            listView2.Items.Add("Equipo: " + Convert.ToString(equipos11[i].numequipo2) + " Tipo: " + Convert.ToString(equipos11[i].tipoequipo2) + " Ecuac.: " + Convert.ToString(contecuaciones - temporal));
                            ecuacfaltan = ecuacfaltan + (contecuaciones - temporal);
                        }
                        else if ((contecuaciones - temporal) == 3)
                        {
                            //listView1.Items[i].BackColor = Color.White;
                            listView1.Items.Add("Equipo: " + Convert.ToString(equipos11[i].numequipo2) + " Tipo: " + Convert.ToString(equipos11[i].tipoequipo2) + " Ecuac.: " + Convert.ToString(contecuaciones - temporal));
                        }
                        else if ((contecuaciones - temporal) == 5)
                        {
                            listView2.Items.Add("Equipo: " + Convert.ToString(equipos11[i].numequipo2) + " Tipo: " + Convert.ToString(equipos11[i].tipoequipo2) + " Ecuac.: " + Convert.ToString(contecuaciones - temporal));
                            //listView1.Items[i].BackColor = Color.Cyan;
                            ecuacfaltan = ecuacfaltan + (contecuaciones - temporal);
                        }
                        else if ((contecuaciones - temporal) > 6)
                        {
                            listView3.Items.Add("Equipo: " + Convert.ToString(equipos11[i].numequipo2) + " Tipo: " + Convert.ToString(equipos11[i].tipoequipo2) + " Ecuac.: " + Convert.ToString(contecuaciones - temporal));
                            //listView1.Items[i].BackColor = Color.Green;
                            ecuacsobran = ecuacsobran + (contecuaciones - temporal);
                        }
                        else if ((contecuaciones - temporal) == 6)
                        {
                            listView1.Items.Add("Equipo: " + Convert.ToString(equipos11[i].numequipo2) + " Tipo: " + Convert.ToString(equipos11[i].tipoequipo2) + " Ecuac.: " + Convert.ToString(contecuaciones - temporal));
                            //listView1.Items[i].BackColor = Color.White;
                        }
                        else if ((contecuaciones - temporal) == 4)
                        {
                            listView3.Items.Add("Equipo: " + Convert.ToString(equipos11[i].numequipo2) + " Tipo: " + Convert.ToString(equipos11[i].tipoequipo2) + " Ecuac.: " + Convert.ToString(contecuaciones - temporal));
                            //listView1.Items[i].BackColor = Color.Green;
                            ecuacsobran = ecuacsobran + (contecuaciones - temporal);
                        }

                        temporal = contecuaciones;
                    }

                    if (i == equipos11.Count - 1)
                    {
                        listView1.Items.Add("Number of Equations Generated: " + Convert.ToString(contecuaciones));
                        listView1.Items.Add("Number of System Variables: " + Convert.ToString(p.Count));
                        listView2.Items.Add("Number of Equations Generated: " + Convert.ToString(ecuacfaltan));
                        listView3.Items.Add("Number of Equations Generated: " + Convert.ToString(ecuacsobran));
                    }
                }

                matrizauxjacob1 = matrizauxjacob;

            }

            //Si no hace falta crear las ecuaciones, los parámetros y leer las condiciones iniciales. Y sólo es necesario mostrar el motor de cálculo
            else if (marca == 1)
            {
                //Activamos la vista Consola (Iteraciones Intermedias en MS-DOS)
                consola = 1;
            }

            //Añadimos las ecuaciones generadas por el Sistema al Control ListBox
            for (int j = 0; j < ecuaciones.Count; j++)
            {
                listBox5.Items.Add(Convert.ToString(ecuaciones[j]));
            }

            if (ErrorMaxAdmisible < 10e-9)
            {
                MessageBox.Show(this,"Error Máximo admisible fijado a un valor inferior a 10e-9, para facilitar la convergencia se fija a 10e-9, en todo caso el usuario puede modificar su valor en el siguiente cuadro de diálogo donde se fijan las opciones del Motor de Cálculo.","Recomendación del Error Máximo Admisible",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                ErrorMaxAdmisible = 10e-9;
            }

            Motorcalculo motorcalculo1 = new Motorcalculo();
            motorcalculo1.punteroaplicacion(this);
            motorcalculo1.ShowDialog();
            
            //Muestro las pestañas 1 y 8 de los controles Tab de la Aplicación principal
            tabControl1.SelectedTab = tabPage1;
            tabControl2.SelectedTab = tabPage8;
        }

        private void definiciónDeTablasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tablaluis grafica1 = new Tablaluis(this);
            grafica1.Show();
        }

        private void divisorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //numequipos++;

            //Llamada al cuadro de diálogo que crea las ecuaciones del equipo e inicializa sus valores
            //Enviamos al cuadro de dialogo el puntero de la ventana principal para guardar los datos del cuadro de dialogo en variables de la aplicacion principal
            //Divisor Divisor1 = new Divisor(this, numecuaciones, numvariables);
            //Divisor1.Show();

            ListaDivisor listdiv = new ListaDivisor(this);
            listdiv.Show();
        }

        private void perdidaDeCargaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //numequipos++;

            //Llamada al cuadro de diálogo que crea las ecuaciones del equipo e inicializa sus valores
            //Enviamos al cuadro de dialogo el puntero de la ventana principal para guardar los datos del cuadro de dialogo en variables de la aplicacion principal
            //Perdidacarga Perdidacarga1 = new Perdidacarga(this, numecuaciones, numvariables,0,0);
            //Perdidacarga1.Show();

            ListaPerdidaCarga listperd = new ListaPerdidaCarga(this);
            listperd.Show();
        }

        //Función para gestionar las Tablas2D definidas por el usuario
        //Primer argumento el número de Tabla utilizado y segundo argumento el valor de Xo enviado por el usuario
        //Segundo argumento el valor de Xo enviado por el Usuario

        public Double tabla(Double numeroTabla, Double Xo, Double tipoInterpolacion)
        {
            Double resultado = 0;

            Double[,] temp = new Double[1, 1];

            int indice = 0;
            int marca = 0;

            //Búsqueda de la Tabla en la que se va a realizar la Interpolación
            for (int a = 0; a < listaTablas.Count; a++)
            {
                if (numeroTabla == listanumTablas[a])
                {
                    indice = a;
                    marca = 1;
                    goto luis44;
                }
            }

            if (marca == 0)
            {
                MessageBox.Show("No se ha encontrado el número de Tabla: " + Convert.ToString(numeroTabla));
                return 0;
            }

        luis44:

            temp = listaTablas[indice];

            //Número Filas
            int numfilas = temp.GetLength(0);

            //Número Columnas. El número de columnas para las Tablas2D es de 2.
            int numcolumnas = temp.GetLength(1);

            //Comprobamos si el valor de Xo coincide con los valores de X en la Tabla.
            for (int i = 0; i < numfilas; i++)
            {
                //Atento esta función nos puede dar problemas con valores extremos del array o con el primer valor
                if ((temp[i, 0] == Xo))
                {
                    resultado = temp[i, 1];
                    //MessageBox.Show("Valor encontrado en la tabla. X=" + Convert.ToDouble(Xo) + " Y=" + Convert.ToDouble(temp[i, 1]));
                    return resultado;
                }
            }

            //TIPO INTERPOLACION 1D-LINEAL
            if (tipoInterpolacion == 1)
            {
                //Valores auxiliares para Interporlación 1d
                Double X1 = 0;
                Double X2 = 0;
                Double Y1 = 0;
                Double Y2 = 0;

                for (int i = 0; i < numfilas; i++)
                {

                    //Valor de Xo dentro del rango de valores de X en la Tabla (Interpolación)
                    if ((temp[i, 0] < Xo) && (temp[i + 1, 0] > Xo))
                    {
                        X1 = temp[i, 0];
                        Y1 = temp[i, 1];
                        X2 = temp[i + 1, 0];
                        Y2 = temp[i + 1, 1];

                        //MessageBox.Show("X1=" + Convert.ToString(X1) + " Y1=" + Convert.ToString(Y1) + " X2=" + Convert.ToString(X2) + " Y2=" + Convert.ToString(Y2));

                        //Enviamos los valores para calcular la interpolación 1D
                        resultado = Interpolacion1D(Xo, X1, Y1, X2, Y2);
                        
                     
                        //MessageBox.Show("Valor resultado de la Interpolación Lineal: X=" + Convert.ToDouble(Xo) + " Y=" + Convert.ToString(resultado));
                        return resultado;
                    }

                    //Valor de Xo fuera del rango de valores de X en la Tabla (Extrapolación)
                    else if (Xo > temp[numfilas - 1, 0])
                    {

                        MessageBox.Show("Valor fuera de tablas. Se han tomado los dos últimos valores de la tabla para interpolar. Posible error.");
                        X1 = temp[numfilas - 2, 0];
                        Y1 = temp[numfilas - 2, 1];
                        X2 = temp[numfilas - 1, 0];
                        Y2 = temp[numfilas - 1, 1];

                        //MessageBox.Show("X1=" + Convert.ToString(X1) + " Y1=" + Convert.ToString(Y1) + " X2=" + Convert.ToString(X2) + " Y2=" + Convert.ToString(Y2));

                        //Enviamos los valores para calcular la interpolación 1D
                        resultado = Interpolacion1D(Xo, X1, Y1, X2, Y2);

                        //MessageBox.Show("Valor resultado de la Interpolación Lineal: X=" + Convert.ToDouble(Xo) + " Y=" + Convert.ToString(resultado));
                        return resultado;
                    }
                }

                MessageBox.Show("Error no se ha encontrado valores para realizar la Interporlacion Lineal.");
                return 0;

            }

            //TIPO INTERPOLACION 2D-PARABÓLICA,CUADRÁTICA
            else if (tipoInterpolacion == 2)
            {
                //Valores auxiliares para Interporlación 2D
                Double X3 = 0;
                Double Y3 = 0;
                Double X4 = 0;
                Double Y4 = 0;
                Double X5 = 0;
                Double Y5 = 0;

                for (int i = 0; i < numfilas - 1; i++)
                {

                    //Atento esta función nos puede dar problemas con valores extremos del array o con el primer valor
                    if (((temp[i, 0] < Xo) && (temp[i + 1, 0] > Xo)))
                    {
                        //Si el valor de Xo está comprendido entre el penúltimo y el último valor de X en la TABLA
                        if ((Xo > temp[numfilas - 2, 0]) && (Xo < temp[numfilas - 1, 0]))
                        {
                            X3 = temp[numfilas - 3, 0];
                            Y3 = temp[numfilas - 3, 1];
                            X4 = temp[numfilas - 2, 0];
                            Y4 = temp[numfilas - 2, 1];
                            X5 = temp[numfilas - 1, 0];
                            Y5 = temp[numfilas - 1, 1];

                            //MessageBox.Show("X1=" + Convert.ToString(X3) + " Y1=" + Convert.ToString(Y3) + " X2=" + Convert.ToString(X4) + " Y2=" + Convert.ToString(Y4) + " X3=" + Convert.ToString(X5) + " Y3=" + Convert.ToString(Y5));
                            //Enviamos los valores para calcular la interpolación 2D
                            resultado = Interpolacion2D(Xo, X3, Y3, X4, Y4, X5, Y5);

                            //MessageBox.Show("Valor resultado de la Interpolación Lineal: X=" + Convert.ToDouble(Xo) + " Y=" + Convert.ToString(resultado));
                            return resultado;
                        }

                        //Si los valores no son superiores al penúltimo valor de la Tabla ni al último de la tabla
                        else
                        {
                            X3 = temp[i, 0];
                            Y3 = temp[i, 1];
                            X4 = temp[i + 1, 0];
                            Y4 = temp[i + 1, 1];
                            X5 = temp[i + 2, 0];
                            Y5 = temp[i + 2, 1];

                            //MessageBox.Show("X1=" + Convert.ToString(X3) + "Y1=" + Convert.ToString(Y3) + "X2=" + Convert.ToString(X4) + "Y2=" + Convert.ToString(Y4) + "X3=" + Convert.ToString(X5) + "Y3=" + Convert.ToString(Y5));
                            //Enviamos los valores para calcular la interpolación 2D
                            resultado = Interpolacion2D(Xo, X3, Y3, X4, Y4, X5, Y5);

                            //MessageBox.Show("Valor resultado de la Interpolación Lineal: X=" + Convert.ToDouble(Xo) + " Y=" + Convert.ToString(resultado));
                            return resultado;
                        }
                    }

                    //Si Xo queda fuera del rango de valores de la Tabla, es decir, superior al último valor de la tabla (Extrapolación)
                    else if (Xo > temp[numfilas - 1, 0])
                    {
                        //MessageBox.Show("Valor fuera de Tabla. Para interpolar se han tomado los tres últimos valores de la tabla.");
                        X3 = temp[numfilas - 3, 0];
                        Y3 = temp[numfilas - 3, 1];
                        X4 = temp[numfilas - 2, 0];
                        Y4 = temp[numfilas - 2, 1];
                        X5 = temp[numfilas - 1, 0];
                        Y5 = temp[numfilas - 1, 1];

                        //MessageBox.Show("X1=" + Convert.ToString(X3) + "Y1=" + Convert.ToString(Y3) + "X2=" + Convert.ToString(X4) + "Y2=" + Convert.ToString(Y4) + "X3=" + Convert.ToString(X5) + "Y3=" + Convert.ToString(Y5));
                        //Enviamos los valores para calcular la interpolación 1D
                        resultado = Interpolacion2D(Xo, X3, Y3, X4, Y4, X5, Y5);

                     
                        // MessageBox.Show("Valor resultado de la Interpolación Lineal: X=" + Convert.ToDouble(Xo) + " Y=" + Convert.ToString(resultado));
                        return resultado;
                    }
                }            

                return resultado;
            }


            //TIPO INTERPOLACION 3D-CUBICA
            else if (tipoInterpolacion == 3)
            {
                //Valores auxiliares para Interporlación 1d
                Double X6 = 0;
                Double Y6 = 0;
                Double X7 = 0;
                Double Y7 = 0;
                Double X8 = 0;
                Double Y8 = 0;
                Double X9 = 0;
                Double Y9 = 0;


                for (int i = 0; i < numfilas - 2; i++)
                {

                    //Atento esta función nos puede dar problemas con valores extremos del array o con el primer valor
                    if (((temp[i, 0] < Xo) && (temp[i + 1, 0] > Xo)))
                    {

                        if ((Xo > temp[numfilas - 3, 0]) && (Xo < temp[numfilas - 2, 0]))
                        {
                            X6 = temp[numfilas - 4, 0];
                            Y6 = temp[numfilas - 4, 1];
                            X7 = temp[numfilas - 3, 0];
                            Y7 = temp[numfilas - 3, 1];
                            X8 = temp[numfilas - 2, 0];
                            Y8 = temp[numfilas - 2, 1];
                            X9 = temp[numfilas - 1, 0];
                            Y9 = temp[numfilas - 1, 1];

                           // MessageBox.Show("X1=" + Convert.ToString(X6) + "Y1=" + Convert.ToString(Y6) + "X2=" + Convert.ToString(X7) + "Y2=" + Convert.ToString(Y7) + "X3=" + Convert.ToString(X8) + "Y3=" + Convert.ToString(Y8) + "X4=" + Convert.ToString(X9) + "Y4=" + Convert.ToString(Y9));
                            //Enviamos los valores para calcular la interpolación 3D
                            resultado = Interpolacion3D(Xo, X6, Y6, X7, Y7, X8, Y8, X9, Y9);

                           // MessageBox.Show("Valor resultado de la Interpolación Lineal: X=" + Convert.ToDouble(Xo) + " Y=" + Convert.ToString(resultado));
                            return resultado;
                        }

                        else
                        {
                            X6 = temp[i, 0];
                            Y6 = temp[i, 1];
                            X7 = temp[i + 1, 0];
                            Y7 = temp[i + 1, 1];
                            X8 = temp[i + 2, 0];
                            Y8 = temp[i + 2, 1];
                            X9 = temp[i + 3, 0];
                            Y9 = temp[i + 3, 1];

                            //MessageBox.Show("X1=" + Convert.ToString(X6) + "Y1=" + Convert.ToString(Y6) + "X2=" + Convert.ToString(X7) + "Y2=" + Convert.ToString(Y7) + "X3=" + Convert.ToString(X8) + "Y3=" + Convert.ToString(Y8));
                            //Enviamos los valores para calcular la interpolación 1D
                            resultado = Interpolacion3D(Xo, X6, Y6, X7, Y7, X8, Y8, X9, Y9);

                            
                            //MessageBox.Show("Valor resultado de la Interpolación Lineal: X=" + Convert.ToDouble(Xo) + " Y=" + Convert.ToString(resultado));
                            return resultado;
                        }

                    }

                    else if (Xo > temp[numfilas - 2, 0])
                    {
                       // MessageBox.Show("Valor fuera de Tabla. Para interpolar se han tomado los tres últimos valores de la tabla.");
                        X6 = temp[numfilas - 4, 0];
                        Y6 = temp[numfilas - 4, 1];
                        X7 = temp[numfilas - 3, 0];
                        Y7 = temp[numfilas - 3, 1];
                        X8 = temp[numfilas - 2, 0];
                        Y8 = temp[numfilas - 2, 1];
                        X9 = temp[numfilas - 1, 0];
                        Y9 = temp[numfilas - 1, 1];

                        //MessageBox.Show("X1=" + Convert.ToString(X6) + "Y1=" + Convert.ToString(Y6) + "X2=" + Convert.ToString(X7) + "Y2=" + Convert.ToString(Y7) + "X3=" + Convert.ToString(X8) + "Y3=" + Convert.ToString(Y8) + "X3=" + Convert.ToString(X9) + "Y3=" + Convert.ToString(Y9));
                        //Enviamos los valores para calcular la interpolación 1D
                        resultado = Interpolacion3D(Xo, X6, Y6, X7, Y7, X8, Y8, X9, Y9);

                     
                        //MessageBox.Show("Valor resultado de la Interpolación Lineal: X=" + Convert.ToDouble(Xo) + " Y=" + Convert.ToString(resultado));
                        return resultado;
                    }
                }

                return resultado;
            }

            //TIPO INTERPOLACION 2D-C según Minimos Cuadrados y Librería DotNumerics
            else if (tipoInterpolacion == 4)
            {
                //Valores auxiliares para Interporlación 2D
                Double X3 = 0;
                Double Y3 = 0;
                Double X4 = 0;
                Double Y4 = 0;
                Double X5 = 0;
                Double Y5 = 0;

                for (int i = 0; i < numfilas - 1; i++)
                {

                    //Atento esta función nos puede dar problemas con valores extremos del array o con el primer valor
                    if (((temp[i, 0] < Xo) && (temp[i + 1, 0] > Xo)))
                    {
                        //Si el valor de Xo está comprendido entre el penúltimo y el último valor de X en la TABLA
                        if ((Xo > temp[numfilas - 2, 0]) && (Xo < temp[numfilas - 1, 0]))
                        {
                            X3 = temp[numfilas - 3, 0];
                            Y3 = temp[numfilas - 3, 1];
                            X4 = temp[numfilas - 2, 0];
                            Y4 = temp[numfilas - 2, 1];
                            X5 = temp[numfilas - 1, 0];
                            Y5 = temp[numfilas - 1, 1];

                            //MessageBox.Show("X1=" + Convert.ToString(X3) + " Y1=" + Convert.ToString(Y3) + " X2=" + Convert.ToString(X4) + " Y2=" + Convert.ToString(Y4) + " X3=" + Convert.ToString(X5) + " Y3=" + Convert.ToString(Y5));
                            //Enviamos los valores para calcular la interpolación 2D
                            resultado = Interpolacion2Ddotnumerics(Xo, X3, Y3, X4, Y4, X5, Y5);

                            //MessageBox.Show("Valor resultado de la Interpolación Lineal: X=" + Convert.ToDouble(Xo) + " Y=" + Convert.ToString(resultado));
                            return resultado;
                        }

                        //Si los valores no son superiores al penúltimo valor de la Tabla ni al último de la tabla
                        else
                        {
                            X3 = temp[i, 0];
                            Y3 = temp[i, 1];
                            X4 = temp[i + 1, 0];
                            Y4 = temp[i + 1, 1];
                            X5 = temp[i + 2, 0];
                            Y5 = temp[i + 2, 1];

                            //MessageBox.Show("X1=" + Convert.ToString(X3) + "Y1=" + Convert.ToString(Y3) + "X2=" + Convert.ToString(X4) + "Y2=" + Convert.ToString(Y4) + "X3=" + Convert.ToString(X5) + "Y3=" + Convert.ToString(Y5));
                            //Enviamos los valores para calcular la interpolación 2D
                            resultado = Interpolacion2Ddotnumerics(Xo, X3, Y3, X4, Y4, X5, Y5);

                         
                            //MessageBox.Show("Valor resultado de la Interpolación Lineal: X=" + Convert.ToDouble(Xo) + " Y=" + Convert.ToString(resultado));
                            return resultado;
                        }
                    }

                    //Si Xo queda fuera del rango de valores de la Tabla, es decir, superior al último valor de la tabla (Extrapolación)
                    else if (Xo > temp[numfilas - 1, 0])
                    {
                        //MessageBox.Show("Valor fuera de Tabla. Para interpolar se han tomado los tres últimos valores de la tabla.");
                        X3 = temp[numfilas - 3, 0];
                        Y3 = temp[numfilas - 3, 1];
                        X4 = temp[numfilas - 2, 0];
                        Y4 = temp[numfilas - 2, 1];
                        X5 = temp[numfilas - 1, 0];
                        Y5 = temp[numfilas - 1, 1];

                        //MessageBox.Show("X1=" + Convert.ToString(X3) + "Y1=" + Convert.ToString(Y3) + "X2=" + Convert.ToString(X4) + "Y2=" + Convert.ToString(Y4) + "X3=" + Convert.ToString(X5) + "Y3=" + Convert.ToString(Y5));
                        //Enviamos los valores para calcular la interpolación 1D
                        resultado = Interpolacion2Ddotnumerics(Xo, X3, Y3, X4, Y4, X5, Y5);

                        // MessageBox.Show("Valor resultado de la Interpolación Lineal: X=" + Convert.ToDouble(Xo) + " Y=" + Convert.ToString(resultado));
                        return resultado;
                    }
                }
                return resultado;
            }

            //TIPO INTERPOLACION 3D-CUBICA según Minimos Cuadrados y Librería DotNumerics
            else if (tipoInterpolacion == 5)
            {
                //Valores auxiliares para Interporlación 3d
                Double X6 = 0;
                Double Y6 = 0;
                Double X7 = 0;
                Double Y7 = 0;
                Double X8 = 0;
                Double Y8 = 0;
                Double X9 = 0;
                Double Y9 = 0;


                for (int i = 0; i < numfilas - 2; i++)
                {

                    //Atento esta función nos puede dar problemas con valores extremos del array o con el primer valor
                    if (((temp[i, 0] < Xo) && (temp[i + 1, 0] > Xo)))
                    {

                        if ((Xo > temp[numfilas - 3, 0]) && (Xo < temp[numfilas - 2, 0]))
                        {
                            X6 = temp[numfilas - 4, 0];
                            Y6 = temp[numfilas - 4, 1];
                            X7 = temp[numfilas - 3, 0];
                            Y7 = temp[numfilas - 3, 1];
                            X8 = temp[numfilas - 2, 0];
                            Y8 = temp[numfilas - 2, 1];
                            X9 = temp[numfilas - 1, 0];
                            Y9 = temp[numfilas - 1, 1];

                            // MessageBox.Show("X1=" + Convert.ToString(X6) + "Y1=" + Convert.ToString(Y6) + "X2=" + Convert.ToString(X7) + "Y2=" + Convert.ToString(Y7) + "X3=" + Convert.ToString(X8) + "Y3=" + Convert.ToString(Y8) + "X4=" + Convert.ToString(X9) + "Y4=" + Convert.ToString(Y9));
                            //Enviamos los valores para calcular la interpolación 3D
                            resultado = Interpolacion3Ddotnumerics(Xo, X6, Y6, X7, Y7, X8, Y8, X9, Y9);

                      
                            // MessageBox.Show("Valor resultado de la Interpolación Lineal: X=" + Convert.ToDouble(Xo) + " Y=" + Convert.ToString(resultado));
                            return resultado;
                        }

                        else
                        {
                            X6 = temp[i, 0];
                            Y6 = temp[i, 1];
                            X7 = temp[i + 1, 0];
                            Y7 = temp[i + 1, 1];
                            X8 = temp[i + 2, 0];
                            Y8 = temp[i + 2, 1];
                            X9 = temp[i + 3, 0];
                            Y9 = temp[i + 3, 1];

                            //MessageBox.Show("X1=" + Convert.ToString(X6) + "Y1=" + Convert.ToString(Y6) + "X2=" + Convert.ToString(X7) + "Y2=" + Convert.ToString(Y7) + "X3=" + Convert.ToString(X8) + "Y3=" + Convert.ToString(Y8));
                            //Enviamos los valores para calcular la interpolación 1D
                            resultado = Interpolacion3Ddotnumerics(Xo, X6, Y6, X7, Y7, X8, Y8, X9, Y9);

                           

                            //MessageBox.Show("Valor resultado de la Interpolación Lineal: X=" + Convert.ToDouble(Xo) + " Y=" + Convert.ToString(resultado));
                            return resultado;
                        }

                    }

                    else if (Xo > temp[numfilas - 2, 0])
                    {
                        // MessageBox.Show("Valor fuera de Tabla. Para interpolar se han tomado los tres últimos valores de la tabla.");
                        X6 = temp[numfilas - 4, 0];
                        Y6 = temp[numfilas - 4, 1];
                        X7 = temp[numfilas - 3, 0];
                        Y7 = temp[numfilas - 3, 1];
                        X8 = temp[numfilas - 2, 0];
                        Y8 = temp[numfilas - 2, 1];
                        X9 = temp[numfilas - 1, 0];
                        Y9 = temp[numfilas - 1, 1];

                        //MessageBox.Show("X1=" + Convert.ToString(X6) + "Y1=" + Convert.ToString(Y6) + "X2=" + Convert.ToString(X7) + "Y2=" + Convert.ToString(Y7) + "X3=" + Convert.ToString(X8) + "Y3=" + Convert.ToString(Y8) + "X3=" + Convert.ToString(X9) + "Y3=" + Convert.ToString(Y9));
                        //Enviamos los valores para calcular la interpolación 3D
                        resultado = Interpolacion3Ddotnumerics(Xo, X6, Y6, X7, Y7, X8, Y8, X9, Y9);

                       
                        //MessageBox.Show("Valor resultado de la Interpolación Lineal: X=" + Convert.ToDouble(Xo) + " Y=" + Convert.ToString(resultado));
                        return resultado;
                    }
                }

                return resultado;
            }

            //TIPO INTERPOLACION 4D-CUADRÁTICA según Minimos Cuadrados y Librería DotNumerics
            else if (tipoInterpolacion == 6)
            {
                //Valores auxiliares para Interporlación 3d
                Double X6 = 0;
                Double Y6 = 0;
                Double X7 = 0;
                Double Y7 = 0;
                Double X8 = 0;
                Double Y8 = 0;
                Double X9 = 0;
                Double Y9 = 0;
                Double X10 = 0;
                Double Y10 = 0;


                for (int i = 0; i < numfilas - 2; i++)
                {

                    //Atento esta función nos puede dar problemas con valores extremos del array o con el primer valor
                    if (((temp[i, 0] < Xo) && (temp[i + 1, 0] > Xo)))
                    {

                        if ((Xo > temp[numfilas - 4, 0]) && (Xo < temp[numfilas - 3, 0]))
                        {
                            X6 = temp[numfilas - 5, 0];
                            Y6 = temp[numfilas - 5, 1];
                            X7 = temp[numfilas - 4, 0];
                            Y7 = temp[numfilas - 4, 1];
                            X8 = temp[numfilas - 3, 0];
                            Y8 = temp[numfilas - 3, 1];
                            X9 = temp[numfilas - 2, 0];
                            Y9 = temp[numfilas - 2, 1];
                            X10 = temp[numfilas - 1, 0];
                            Y10 = temp[numfilas - 1, 1];

                            // MessageBox.Show("X1=" + Convert.ToString(X6) + "Y1=" + Convert.ToString(Y6) + "X2=" + Convert.ToString(X7) + "Y2=" + Convert.ToString(Y7) + "X3=" + Convert.ToString(X8) + "Y3=" + Convert.ToString(Y8) + "X4=" + Convert.ToString(X9) + "Y4=" + Convert.ToString(Y9));
                            //Enviamos los valores para calcular la interpolación 3D
                            resultado = Interpolacion4Ddotnumerics(Xo, X6, Y6, X7, Y7, X8, Y8, X9, Y9, X10, Y10);

                            // MessageBox.Show("Valor resultado de la Interpolación Lineal: X=" + Convert.ToDouble(Xo) + " Y=" + Convert.ToString(resultado));
                            return resultado;
                        }

                        else
                        {
                            X6 = temp[i, 0];
                            Y6 = temp[i, 1];
                            X7 = temp[i + 1, 0];
                            Y7 = temp[i + 1, 1];
                            X8 = temp[i + 2, 0];
                            Y8 = temp[i + 2, 1];
                            X9 = temp[i + 3, 0];
                            Y9 = temp[i + 3, 1];
                            X10 = temp[i + 4, 0];
                            Y10 = temp[i + 4, 1];

                            //MessageBox.Show("X1=" + Convert.ToString(X6) + "Y1=" + Convert.ToString(Y6) + "X2=" + Convert.ToString(X7) + "Y2=" + Convert.ToString(Y7) + "X3=" + Convert.ToString(X8) + "Y3=" + Convert.ToString(Y8));
                            //Enviamos los valores para calcular la interpolación 1D
                            resultado = Interpolacion4Ddotnumerics(Xo, X6, Y6, X7, Y7, X8, Y8, X9, Y9, X10, Y10);

                            //MessageBox.Show("Valor resultado de la Interpolación Lineal: X=" + Convert.ToDouble(Xo) + " Y=" + Convert.ToString(resultado));
                            return resultado;
                        }

                    }

                    else if (Xo > temp[numfilas - 3, 0])
                    {
                        // MessageBox.Show("Valor fuera de Tabla. Para interpolar se han tomado los tres últimos valores de la tabla.");
                        X6 = temp[numfilas - 5, 0];
                        Y6 = temp[numfilas - 5, 1];
                        X7 = temp[numfilas - 4, 0];
                        Y7 = temp[numfilas - 4, 1];
                        X8 = temp[numfilas - 3, 0];
                        Y8 = temp[numfilas - 3, 1];
                        X9 = temp[numfilas - 2, 0];
                        Y9 = temp[numfilas - 2, 1];
                        X10 = temp[numfilas - 1, 0];
                        Y10 = temp[numfilas - 1, 1];

                        //MessageBox.Show("X1=" + Convert.ToString(X6) + "Y1=" + Convert.ToString(Y6) + "X2=" + Convert.ToString(X7) + "Y2=" + Convert.ToString(Y7) + "X3=" + Convert.ToString(X8) + "Y3=" + Convert.ToString(Y8) + "X3=" + Convert.ToString(X9) + "Y3=" + Convert.ToString(Y9));
                        //Enviamos los valores para calcular la interpolación 1D
                        resultado = Interpolacion4Ddotnumerics(Xo, X6, Y6, X7, Y7, X8, Y8, X9, Y9, X10, Y10);

                        //MessageBox.Show("Valor resultado de la Interpolación Lineal: X=" + Convert.ToDouble(Xo) + " Y=" + Convert.ToString(resultado));
                        return resultado;
                    }
                }

                return resultado;
            }


            //TIPO INTERPOLACION 5D-CUADRÁTICA según Minimos Cuadrados y Librería DotNumerics
            else if (tipoInterpolacion == 7)
            {
                //Valores auxiliares para Interporlación 3d
                Double X6 = 0;
                Double Y6 = 0;
                Double X7 = 0;
                Double Y7 = 0;
                Double X8 = 0;
                Double Y8 = 0;
                Double X9 = 0;
                Double Y9 = 0;
                Double X10 = 0;
                Double Y10 = 0;
                Double X11 = 0;
                Double Y11 = 0;


                for (int i = 0; i < numfilas - 2; i++)
                {

                    //Atento esta función nos puede dar problemas con valores extremos del array o con el primer valor
                    if (((temp[i, 0] < Xo) && (temp[i + 1, 0] > Xo)))
                    {

                        if ((Xo > temp[numfilas - 5, 0]) && (Xo < temp[numfilas - 4, 0]))
                        {
                            X6 = temp[numfilas - 6, 0];
                            Y6 = temp[numfilas - 6, 1];
                            X7 = temp[numfilas - 5, 0];
                            Y7 = temp[numfilas - 5, 1];
                            X8 = temp[numfilas - 4, 0];
                            Y8 = temp[numfilas - 4, 1];
                            X9 = temp[numfilas - 3, 0];
                            Y9 = temp[numfilas - 3, 1];
                            X10 = temp[numfilas - 2, 0];
                            Y10 = temp[numfilas - 2, 1];
                            X11 = temp[numfilas - 1, 0];
                            Y11 = temp[numfilas - 1, 1];

                            // MessageBox.Show("X1=" + Convert.ToString(X6) + "Y1=" + Convert.ToString(Y6) + "X2=" + Convert.ToString(X7) + "Y2=" + Convert.ToString(Y7) + "X3=" + Convert.ToString(X8) + "Y3=" + Convert.ToString(Y8) + "X4=" + Convert.ToString(X9) + "Y4=" + Convert.ToString(Y9));
                            //Enviamos los valores para calcular la interpolación 3D
                            resultado = Interpolacion5Ddotnumerics(Xo, X6, Y6, X7, Y7, X8, Y8, X9, Y9, X10, Y10, X11, Y11);

                            // MessageBox.Show("Valor resultado de la Interpolación Lineal: X=" + Convert.ToDouble(Xo) + " Y=" + Convert.ToString(resultado));
                            return resultado;
                        }

                        else
                        {
                            X6 = temp[i, 0];
                            Y6 = temp[i, 1];
                            X7 = temp[i + 1, 0];
                            Y7 = temp[i + 1, 1];
                            X8 = temp[i + 2, 0];
                            Y8 = temp[i + 2, 1];
                            X9 = temp[i + 3, 0];
                            Y9 = temp[i + 3, 1];
                            X10 = temp[i + 4, 0];
                            Y10 = temp[i + 4, 1];
                            X11 = temp[i + 5, 0];
                            Y11 = temp[i + 5, 1];

                            //MessageBox.Show("X1=" + Convert.ToString(X6) + "Y1=" + Convert.ToString(Y6) + "X2=" + Convert.ToString(X7) + "Y2=" + Convert.ToString(Y7) + "X3=" + Convert.ToString(X8) + "Y3=" + Convert.ToString(Y8));
                            //Enviamos los valores para calcular la interpolación 1D
                            resultado = Interpolacion5Ddotnumerics(Xo, X6, Y6, X7, Y7, X8, Y8, X9, Y9, X10, Y10, X11, Y11);

                            //MessageBox.Show("Valor resultado de la Interpolación Lineal: X=" + Convert.ToDouble(Xo) + " Y=" + Convert.ToString(resultado));
                            return resultado;
                        }

                    }

                    else if (Xo > temp[numfilas - 4, 0])
                    {
                        // MessageBox.Show("Valor fuera de Tabla. Para interpolar se han tomado los tres últimos valores de la tabla.");
                        X6 = temp[numfilas - 6, 0];
                        Y6 = temp[numfilas - 6, 1];
                        X7 = temp[numfilas - 5, 0];
                        Y7 = temp[numfilas - 5, 1];
                        X8 = temp[numfilas - 4, 0];
                        Y8 = temp[numfilas - 4, 1];
                        X9 = temp[numfilas - 3, 0];
                        Y9 = temp[numfilas - 3, 1];
                        X10 = temp[numfilas - 2, 0];
                        Y10 = temp[numfilas - 2, 1];
                        X11 = temp[numfilas - 1, 0];
                        Y11 = temp[numfilas - 1, 1];

                        //MessageBox.Show("X1=" + Convert.ToString(X6) + "Y1=" + Convert.ToString(Y6) + "X2=" + Convert.ToString(X7) + "Y2=" + Convert.ToString(Y7) + "X3=" + Convert.ToString(X8) + "Y3=" + Convert.ToString(Y8) + "X3=" + Convert.ToString(X9) + "Y3=" + Convert.ToString(Y9));
                        //Enviamos los valores para calcular la interpolación 1D
                        resultado = Interpolacion5Ddotnumerics(Xo, X6, Y6, X7, Y7, X8, Y8, X9, Y9, X10, Y10, X11, Y11);

                        //MessageBox.Show("Valor resultado de la Interpolación Lineal: X=" + Convert.ToDouble(Xo) + " Y=" + Convert.ToString(resultado));
                        return resultado;
                    }
                }

                return resultado;
            }

            else
            {
                //MessageBox.Show("Posible error no se ha interpolado ni tampoco se ha encontrado el valore en tablas.");
                return 0;
            }

            return 0;
        }


        public void recibirpunterotabla(Tablaluis punterodialogotabla)
        {
            punterodialogotabla1 = punterodialogotabla;      
        }


        //Interpolación LINEAL 1D
        public Double Interpolacion1D(Double X, Double Xo, Double fxo, Double X1, Double fx1)
        {
            Double fx = 0;
            fx = fxo + (((fx1 - fxo) * (X - Xo)) / (X1 - Xo));

            if (punterodialogotabla1 != null)
            {
                punterodialogotabla1.listBox1.Items.Clear();
                punterodialogotabla1.listBox1.Items.Add("Punto Xo: " + Convert.ToString(Xo) + "   fxo: " + Convert.ToString(fxo));
                punterodialogotabla1.listBox1.Items.Add("Punto X1: " + Convert.ToString(X1) + "   fx1: " + Convert.ToString(fx1));
            }
            return fx;
        }

        //Interpolación CUADRÁTICA 2D
        public Double Interpolacion2D(Double X, Double Xo, Double fxo, Double X1, Double fx1, Double X2, Double fx2)
        {
            Double fx = 0;

            Double bo = 0;
            Double b1 = 0;
            Double b2 = 0;

            Double ao = 0;
            Double a1 = 0;
            Double a2 = 0;

            bo = fxo;
            b1 = (fx1 - fxo) / (X1 - Xo);
            b2 = (((fx2 - fx1) / (X2 - X1)) - ((fx1 - fxo) / (X1 - Xo))) / (X2 - Xo);

            a2 = b2;
            a1 = b1 - b2 * Xo - b2 * X1;
            ao = bo - b1 * Xo + b2 * Xo * X1;

            if ((Xo < X) && (X < X1))
            {
                fx = (b2 * (X - Xo) * (X - X1)) + (b1 * (X - Xo)) + bo;

                if (punterodialogotabla1 != null)
                {
                    punterodialogotabla1.listBox1.Items.Clear();
                    punterodialogotabla1.listBox1.Items.Add("Punto Xo: " + Convert.ToString(Xo) + "   fxo: " + Convert.ToString(fxo));
                    punterodialogotabla1.listBox1.Items.Add("Punto X1: " + Convert.ToString(X1) + "   fx1: " + Convert.ToString(fx1));
                    punterodialogotabla1.listBox1.Items.Add("Punto X2: " + Convert.ToString(X2) + "   fx2: " + Convert.ToString(fx2));
                }
                return fx;
            }

            else if ((X1 < X) && (X < X2))
            {
                fx = (b2 * (X - X1) * (X - X2)) + (b1 * (X - X1)) + bo;

                if (punterodialogotabla1 != null)
                {
                    punterodialogotabla1.listBox1.Items.Clear();
                    punterodialogotabla1.listBox1.Items.Add("Punto Xo: " + Convert.ToString(Xo) + "   fxo: " + Convert.ToString(fxo));
                    punterodialogotabla1.listBox1.Items.Add("Punto X1: " + Convert.ToString(X1) + "   fx1: " + Convert.ToString(fx1));
                    punterodialogotabla1.listBox1.Items.Add("Punto X2: " + Convert.ToString(X2) + "   fx2: " + Convert.ToString(fx2));
                }
                    return fx;
            }

            else if (X2 < X)
            {
                //EXTRAPOLAR FUERA DE LA TABLA???
                //MessageBox.Show("ERROR. Valor fuera del rango de la Tabla. Se ha tomado un valor cero.");
                
                if (punterodialogotabla1 != null)
                {
                    punterodialogotabla1.listBox1.Items.Clear();
                    punterodialogotabla1.listBox1.Items.Add("Punto Xo: " + Convert.ToString(Xo) + "   fxo: " + Convert.ToString(fxo));
                    punterodialogotabla1.listBox1.Items.Add("Punto X1: " + Convert.ToString(X1) + "   fx1: " + Convert.ToString(fx1));
                    punterodialogotabla1.listBox1.Items.Add("Punto X2: " + Convert.ToString(X2) + "   fx2: " + Convert.ToString(fx2));
                }
                     return 0;
            }

            else
            {
                if (punterodialogotabla1 != null)
                {
                    punterodialogotabla1.listBox1.Items.Clear();
                    punterodialogotabla1.listBox1.Items.Add("Punto Xo: " + Convert.ToString(Xo) + "   fxo: " + Convert.ToString(fxo));
                    punterodialogotabla1.listBox1.Items.Add("Punto X1: " + Convert.ToString(X1) + "   fx1: " + Convert.ToString(fx1));
                    punterodialogotabla1.listBox1.Items.Add("Punto X2: " + Convert.ToString(X2) + "   fx2: " + Convert.ToString(fx2));
                }

                return 0;
            }
        }
        
        //Interpolación CÚBICA3D
        public Double Interpolacion3D(Double X, Double Xo, Double fxo, Double X1, Double fx1, Double X2, Double fx2, Double X3, Double fx3)
        {
            //Resulado de la Interpolación
            Double fx = 0;

            //Calculo de coeficientes f[Xi,Xj]
            Double fx1xo = 0;
            fx1xo = (fx1 - fxo) / (X1 - Xo);

            Double fx2x1 = 0;
            fx2x1 = (fx2 - fx1) / (X2 - X1);

            Double fx3x2 = 0;
            fx3x2 = (fx3 - fx2) / (X3 - X2);

            Double fx2x1xo = 0;
            fx2x1xo = (fx2x1 - fx1xo) / (X2 - Xo);

            Double fx3x2x1 = 0;
            fx3x2x1 = (fx3x2 - fx2x1) / (X3 - X1);

            Double fx3x2x1xo = 0;
            fx3x2x1xo = (fx3x2x1 - fx2x1xo) / (X3 - Xo);

            Double bo = 0;
            bo = fxo;

            Double b1 = 0;
            b1 = fx1xo;

            Double b2 = 0;
            b2 = fx2x1xo;

            Double b3 = 0;
            b3 = fx3x2x1xo;

            if (X < X3)
            {
                fx = bo + ((X - Xo) * b1) + ((X - Xo) * (X - X1) * b2) + ((X - Xo) * (X - X1) * (X - X2) * b3);

                if (punterodialogotabla1!=null)
                {
                punterodialogotabla1.listBox1.Items.Clear();
                punterodialogotabla1.listBox1.Items.Add("Punto Xo: " + Convert.ToString(Xo) + "   fxo: " + Convert.ToString(fxo));
                punterodialogotabla1.listBox1.Items.Add("Punto X1: " + Convert.ToString(X1) + "   fx1: " + Convert.ToString(fx1));
                punterodialogotabla1.listBox1.Items.Add("Punto X2: " + Convert.ToString(X2) + "   fx2: " + Convert.ToString(fx2));
                punterodialogotabla1.listBox1.Items.Add("Punto X3: " + Convert.ToString(X3) + "   fx3: " + Convert.ToString(fx3));
                }

                return fx;
            }

            else if (X3 < X)
            {
                //EXTRAPOLAR FUERA DE LA TABLA???
                //MessageBox.Show("ERROR. Valor fuera del rango de la Tabla. Se ha tomado un valor cero.");

                if (punterodialogotabla1 != null)
                {
                    punterodialogotabla1.listBox1.Items.Clear();
                    punterodialogotabla1.listBox1.Items.Add("Punto Xo: " + Convert.ToString(Xo) + "   fxo: " + Convert.ToString(fxo));
                    punterodialogotabla1.listBox1.Items.Add("Punto X1: " + Convert.ToString(X1) + "   fx1: " + Convert.ToString(fx1));
                    punterodialogotabla1.listBox1.Items.Add("Punto X2: " + Convert.ToString(X2) + "   fx2: " + Convert.ToString(fx2));
                    punterodialogotabla1.listBox1.Items.Add("Punto X3: " + Convert.ToString(X3) + "   fx3: " + Convert.ToString(fx3));
                }
                return 0;
            }

            else
            {
                if (punterodialogotabla1 != null)
                {
                    punterodialogotabla1.listBox1.Items.Clear();
                    punterodialogotabla1.listBox1.Items.Add("Punto Xo: " + Convert.ToString(Xo) + "   fxo: " + Convert.ToString(fxo));
                    punterodialogotabla1.listBox1.Items.Add("Punto X1: " + Convert.ToString(X1) + "   fx1: " + Convert.ToString(fx1));
                    punterodialogotabla1.listBox1.Items.Add("Punto X2: " + Convert.ToString(X2) + "   fx2: " + Convert.ToString(fx2));
                    punterodialogotabla1.listBox1.Items.Add("Punto X3: " + Convert.ToString(X3) + "   fx3: " + Convert.ToString(fx3));
                }
                return 0;
            }
        }

        //Interpolación CUADRÁTICA2D (Librería DotNumerics)
        public Double Interpolacion2Ddotnumerics(Double X, Double Xo, Double fxo, Double X1, Double fx1, Double X2, Double fx2)
        {
            Double resultado = 0;

            //Método de Interpolación mediante Mínimos Cuadrados utilizando la Librería DotNumerics.Net

            //Declaración de Variables
            LinearLeastSquares leastSquares = new LinearLeastSquares();

            Matrix A = new Matrix(3, 3);
            Matrix C = new Matrix(3, 1);
            Matrix B = new Matrix(3, 1);

            Double[] Xluis = new Double[3];
            double[] YModel = new Double[3];

            //Opción de Cálculo elegida: opción 1(COFSolve), opción 2(QRorLQSolve), opción 3(SVDdcSolve)
            double opcion = 1;

            //Inicializamos los valores de los Puntos conocidos con coordenadas Xi,Yi
            Xluis[0] = Xo;
            Xluis[1] = X1;
            Xluis[2] = X2;

            B[0, 0] = fxo;
            B[1, 0] = fx1;
            B[2, 0] = fx2;

            for (int i = 0; i < 3; i++)
            {
                //Inicializamos la matriz A con los coeficientes de lo
                //Esta matriz A define la forma del polinomio que estamos definiendo.
                A[i, 0] = 1;
                A[i, 1] = Xluis[i];
                A[i, 2] = Xluis[i] * Xluis[i];
            }

            //Elegimos el Método de Cálculo (método de resolución del Sistema de Ecuaciones lineales
            //Calculamos los coeficientes Ci del polinomio de interpolación
            if (opcion == 1)
            {
                C = leastSquares.COFSolve(A, B);
            }
            else if (opcion == 2)
            {
                C = leastSquares.QRorLQSolve(A, B);
            }
            else if (opcion == 3)
            {
                C = leastSquares.SVDdcSolve(A, B);
            }

            resultado = C[0, 0] + (C[1, 0] * X) + (C[2, 0] * X * X);

            if (punterodialogotabla1 != null)
            {
                punterodialogotabla1.listBox1.Items.Clear();
                punterodialogotabla1.listBox1.Items.Add("Punto Xo: " + Convert.ToString(Xo) + "   fxo: " + Convert.ToString(fxo));
                punterodialogotabla1.listBox1.Items.Add("Punto X1: " + Convert.ToString(X1) + "   fx1: " + Convert.ToString(fx1));
                punterodialogotabla1.listBox1.Items.Add("Punto X2: " + Convert.ToString(X2) + "   fx2: " + Convert.ToString(fx2));

                punterodialogotabla1.listBox1.Items.Add(Convert.ToString(C[0, 0]) + "+" + Convert.ToString(C[1, 0]) + "*" + Convert.ToString(X) + "+" + Convert.ToString(C[2, 0]) + "*" + Convert.ToString(X) + "*" + Convert.ToString(X));
            }
                 return resultado;
        }

        //Interpolación CÚBICA3D (Librería DotNumerics)
        public Double Interpolacion3Ddotnumerics(Double X, Double Xo, Double fxo, Double X1, Double fx1, Double X2, Double fx2, Double X3, Double fx3)
        {
            Double resultado = 0;

            //Método de Interpolación mediante Mínimos Cuadrados utilizando la Librería DotNumerics.Net

            //Declaración de Variables
            LinearLeastSquares leastSquares = new LinearLeastSquares();

            Matrix A = new Matrix(4, 4);
            Matrix C = new Matrix(4, 1);
            Matrix B = new Matrix(4,1);

            Double[] Xluis=new Double[4];
            double[] YModel = new Double[4];

            //Opción de Cálculo elegida: opción 1(COFSolve), opción 2(QRorLQSolve), opción 3(SVDdcSolve)
            double opcion = 1;

            //Inicializamos los valores de los Puntos conocidos con coordenadas Xi,Yi
            Xluis[0] = Xo;
            Xluis[1] = X1;
            Xluis[2] = X2;
            Xluis[3] = X3;

            B[0,0]  = fxo;
            B[1, 0] = fx1;
            B[2, 0] = fx2;
            B[3, 0] = fx3;
	 
            for (int i=0; i<4;i++)
           {            
	           //Inicializamos la matriz A con los coeficientes de lo
	           //Esta matriz A define la forma del polinomio que estamos definiendo.
	           A[i,0]=1;
	           A[i,1]=Xluis[i];
               A[i, 2]=Xluis[i] * Xluis[i];
               A[i, 3] = Xluis[i] * Xluis[i] * Xluis[i];  
           }

         //Elegimos el Método de Cálculo (método de resolución del Sistema de Ecuaciones lineales
         //Calculamos los coeficientes Ci del polinomio de interpolación
         if (opcion==1)
         {
            C = leastSquares.COFSolve(A, B);           
         }
         else if (opcion == 2)
         {
            C = leastSquares.QRorLQSolve(A, B);
         }
         else if (opcion == 3)
         {
            C = leastSquares.SVDdcSolve(A, B);
         }
                      
            resultado = C[0, 0] + (C[1, 0] * X) + (C[2, 0] * X * X) + (C[3, 0] * X * X * X);

            if (punterodialogotabla1 != null)
            {
                punterodialogotabla1.listBox1.Items.Clear();
                punterodialogotabla1.listBox1.Items.Add("Punto Xo: " + Convert.ToString(Xo) + "   fxo: " + Convert.ToString(fxo));
                punterodialogotabla1.listBox1.Items.Add("Punto X1: " + Convert.ToString(X1) + "   fx1: " + Convert.ToString(fx1));
                punterodialogotabla1.listBox1.Items.Add("Punto X2: " + Convert.ToString(X2) + "   fx2: " + Convert.ToString(fx2));
                punterodialogotabla1.listBox1.Items.Add("Punto X3: " + Convert.ToString(X3) + "   fx3: " + Convert.ToString(fx3));
            }

            punterodialogotabla1.listBox1.Items.Add(Convert.ToString(C[0, 0]) + "+" + Convert.ToString(C[1, 0]) + "*" + Convert.ToString(X) + "+" + Convert.ToString(C[2, 0]) + "*" + Convert.ToString(X) + "*" + Convert.ToString(X) + "+" + Convert.ToString(C[3, 0]) + "*" + Convert.ToString(X) + "*" + Convert.ToString(X) + "*" + Convert.ToString(X));
            return resultado;
        }

        //Interpolación CÚBICA4D (Librería DotNumerics)
        public Double Interpolacion4Ddotnumerics(Double X, Double Xo, Double fxo, Double X1, Double fx1, Double X2, Double fx2, Double X3, Double fx3, Double X4, Double fx4)
        {
            Double resultado = 0;

            //Método de Interpolación mediante Mínimos Cuadrados utilizando la Librería DotNumerics.Net

            //Declaración de Variables
            LinearLeastSquares leastSquares = new LinearLeastSquares();

            Matrix A = new Matrix(5, 5);
            Matrix C = new Matrix(5, 1);
            Matrix B = new Matrix(5, 1);

            Double[] Xluis = new Double[5];
            double[] YModel = new Double[5];

            //Opción de Cálculo elegida: opción 1(COFSolve), opción 2(QRorLQSolve), opción 3(SVDdcSolve)
            double opcion = 1;

            //Inicializamos los valores de los Puntos conocidos con coordenadas Xi,Yi
            Xluis[0] = Xo;
            Xluis[1] = X1;
            Xluis[2] = X2;
            Xluis[3] = X3;
            Xluis[4] = X4;

            B[0, 0] = fxo;
            B[1, 0] = fx1;
            B[2, 0] = fx2;
            B[3, 0] = fx3;
            B[4, 0] = fx4;

            for (int i = 0; i < 5; i++)
            {
                //Inicializamos la matriz A con los coeficientes de lo
                //Esta matriz A define la forma del polinomio que estamos definiendo.
                A[i, 0] = 1;
                A[i, 1] = Xluis[i];
                A[i, 2] = Xluis[i] * Xluis[i];
                A[i, 3] = Xluis[i] * Xluis[i] * Xluis[i];
                A[i, 4] = Xluis[i] * Xluis[i] * Xluis[i] * Xluis[i];
            }

            //Elegimos el Método de Cálculo (método de resolución del Sistema de Ecuaciones lineales
            //Calculamos los coeficientes Ci del polinomio de interpolación
            if (opcion == 1)
            {
                C = leastSquares.COFSolve(A, B);
            }
            else if (opcion == 2)
            {
                C = leastSquares.QRorLQSolve(A, B);
            }
            else if (opcion == 3)
            {
                C = leastSquares.SVDdcSolve(A, B);
            }

            resultado = C[0, 0] + (C[1, 0] * X) + (C[2, 0] * X * X) + (C[3, 0] * X * X * X) + (C[4, 0] * X * X * X* X);

            if (punterodialogotabla1 != null)
            {
                punterodialogotabla1.listBox1.Items.Clear();
                punterodialogotabla1.listBox1.Items.Add("Punto Xo: " + Convert.ToString(Xo) + "   fxo: " + Convert.ToString(fxo));
                punterodialogotabla1.listBox1.Items.Add("Punto X1: " + Convert.ToString(X1) + "   fx1: " + Convert.ToString(fx1));
                punterodialogotabla1.listBox1.Items.Add("Punto X2: " + Convert.ToString(X2) + "   fx2: " + Convert.ToString(fx2));
                punterodialogotabla1.listBox1.Items.Add("Punto X3: " + Convert.ToString(X3) + "   fx3: " + Convert.ToString(fx3));
                punterodialogotabla1.listBox1.Items.Add("Punto X4: " + Convert.ToString(X4) + "   fx4: " + Convert.ToString(fx4));

                punterodialogotabla1.listBox1.Items.Add(Convert.ToString(C[0, 0]) + "+" + Convert.ToString(C[1, 0]) + "*" + Convert.ToString(X) + "+" + Convert.ToString(C[2, 0]) + "*" + Convert.ToString(X) + "*" + Convert.ToString(X) + "+" + Convert.ToString(C[3, 0]) + "*" + Convert.ToString(X) + "*" + Convert.ToString(X) + "*" + Convert.ToString(X) + "+" + Convert.ToString(C[4, 0]) + "*" + Convert.ToString(X) + "*" + Convert.ToString(X) + "*" + Convert.ToString(X) + "*" + Convert.ToString(X));
            }
            
            return resultado;
        }

        //Interpolación CÚBICA5D (Librería DotNumerics)
        public Double Interpolacion5Ddotnumerics(Double X, Double Xo, Double fxo, Double X1, Double fx1, Double X2, Double fx2, Double X3, Double fx3, Double X4, Double fx4, Double X5, Double fx5)
        {
            Double resultado = 0;

            //Método de Interpolación mediante Mínimos Cuadrados utilizando la Librería DotNumerics.Net

            //Declaración de Variables
            LinearLeastSquares leastSquares = new LinearLeastSquares();

            Matrix A = new Matrix(6, 6);
            Matrix C = new Matrix(6, 1);
            Matrix B = new Matrix(6, 1);

            Double[] Xluis = new Double[6];
            double[] YModel = new Double[6];

            //Opción de Cálculo elegida: opción 1(COFSolve), opción 2(QRorLQSolve), opción 3(SVDdcSolve)
            double opcion = 1;

            //Inicializamos los valores de los Puntos conocidos con coordenadas Xi,Yi
            Xluis[0] = Xo;
            Xluis[1] = X1;
            Xluis[2] = X2;
            Xluis[3] = X3;
            Xluis[4] = X4;
            Xluis[5] = X5;

            B[0, 0] = fxo;
            B[1, 0] = fx1;
            B[2, 0] = fx2;
            B[3, 0] = fx3;
            B[4, 0] = fx4;
            B[5, 0] = fx5;

            for (int i = 0; i < 6; i++)
            {
                //Inicializamos la matriz A con los coeficientes de lo
                //Esta matriz A define la forma del polinomio que estamos definiendo.
                A[i, 0] = 1;
                A[i, 1] = Xluis[i];
                A[i, 2] = Xluis[i] * Xluis[i];
                A[i, 3] = Xluis[i] * Xluis[i] * Xluis[i];
                A[i, 4] = Xluis[i] * Xluis[i] * Xluis[i] * Xluis[i];
                A[i, 5] = Xluis[i] * Xluis[i] * Xluis[i] * Xluis[i] * Xluis[i];
            }

            //Elegimos el Método de Cálculo (método de resolución del Sistema de Ecuaciones lineales
            //Calculamos los coeficientes Ci del polinomio de interpolación
            if (opcion == 1)
            {
                C = leastSquares.COFSolve(A, B);
            }
            else if (opcion == 2)
            {
                C = leastSquares.QRorLQSolve(A, B);
            }
            else if (opcion == 3)
            {
                C = leastSquares.SVDdcSolve(A, B);
            }

            resultado = C[0, 0] + (C[1, 0] * X) + (C[2, 0] * X * X) + (C[3, 0] * X * X * X) + (C[4, 0] * X * X * X * X) + (C[5, 0] * X * X * X * X * X);

            if (punterodialogotabla1 != null)
            {
                punterodialogotabla1.listBox1.Items.Clear();
                punterodialogotabla1.listBox1.Items.Add("Punto Xo: " + Convert.ToString(Xo) + "   fxo: " + Convert.ToString(fxo));
                punterodialogotabla1.listBox1.Items.Add("Punto X1: " + Convert.ToString(X1) + "   fx1: " + Convert.ToString(fx1));
                punterodialogotabla1.listBox1.Items.Add("Punto X2: " + Convert.ToString(X2) + "   fx2: " + Convert.ToString(fx2));
                punterodialogotabla1.listBox1.Items.Add("Punto X3: " + Convert.ToString(X3) + "   fx3: " + Convert.ToString(fx3));
                punterodialogotabla1.listBox1.Items.Add("Punto X4: " + Convert.ToString(X4) + "   fx4: " + Convert.ToString(fx4));
                punterodialogotabla1.listBox1.Items.Add("Punto X5: " + Convert.ToString(X5) + "   fx5: " + Convert.ToString(fx5));

                punterodialogotabla1.listBox1.Items.Add(Convert.ToString(C[0, 0]) + "+" + Convert.ToString(C[1, 0]) + "*" + Convert.ToString(X) + "+" + Convert.ToString(C[2, 0]) + "*" + Convert.ToString(X) + "*" + Convert.ToString(X) + "+" + Convert.ToString(C[3, 0]) + "*" + Convert.ToString(X) + "*" + Convert.ToString(X) + "*" + Convert.ToString(X) + "+" + Convert.ToString(C[4, 0]) + "*" + Convert.ToString(X) + "*" + Convert.ToString(X) + "*" + Convert.ToString(X) + "*" + Convert.ToString(X) + "+" + Convert.ToString(C[5, 0]) + "*" + Convert.ToString(X) + "*" + Convert.ToString(X) + "*" + Convert.ToString(X) + "*" + Convert.ToString(X) + "*" + Convert.ToString(X));
            }
                 return resultado;
        }


        //Opción del Menú Nuevo equipo TIPO 5: MEZCLADOR
        private void mezcladorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //numequipos++;

            //Llamada al cuadro de diálogo que crea las ecuaciones del equipo e inicializa sus valores
            //Enviamos al cuadro de dialogo el puntero de la ventana principal para guardar los datos del cuadro de dialogo en variables de la aplicacion principal
            //Mezclador Mezclador1 = new Mezclador(this, numecuaciones, numvariables,0,0);
            //Mezclador1.Show();

            ListaMezclador listmezcl = new ListaMezclador(this);
            listmezcl.Show();
        }

        //Opción del Menú Nuevo equipo TIPO 13: SEPARADOR DE HUMEDAD
        private void separadorDeHumedadTipo13ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //numequipos++;

            //Llamada al cuadro de diálogo que crea las ecuaciones del equipo e inicializa sus valores
            //Enviamos al cuadro de dialogo el puntero de la ventana principal para guardar los datos del cuadro de dialogo en variables de la aplicacion principal
            //Sephumedad Sephumedad1 = new Sephumedad(this, numecuaciones, numvariables);
            //Sephumedad1.Show();

            ListaSepHumedad listsephum1 = new ListaSepHumedad(this);
            listsephum1.Show();

        }

        //Opción del Menú Nuevo equipo TIPO 9: TURBINA CON PÉRDIDAS EN EL ESCAPE
        private void turbinaTipo9ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //numequipos++;

            //Llamada al cuadro de diálogo que crea las ecuaciones del equipo e inicializa sus valores
            //Enviamos al cuadro de dialogo el puntero de la ventana principal para guardar los datos del cuadro de dialogo en variables de la aplicacion principal
            //Turbina Turbina1 = new Turbina(this, numecuaciones, numvariables);
            //Turbina1.Show();

            ListaTurbina9 listaturb9 = new ListaTurbina9(this);
            listaturb9.Show();
        }

        //Opción del Menú Nuevo equipo TIPO 10: TURBINA SIN PÉRDIDAS EN EL ESCAPE
        private void turbinaTipo10ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //numequipos++;

            //Llamada al cuadro de diálogo que crea las ecuaciones del equipo e inicializa sus valores
            //Enviamos al cuadro de dialogo el puntero de la ventana principal para guardar los datos del cuadro de dialogo en variables de la aplicacion principal
            //Turbina10 Turbina2 = new Turbina10(this, numecuaciones, numvariables);
            //Turbina2.Show();

             ListaTurbinas10 turb10 = new ListaTurbinas10(this);
             turb10.Show();

        }

        //Opción del Menú Nuevo equipo TIPO 8: CONDENSADOR
        private void condensadorTipo8ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            numequipos++;

            //Llamada al cuadro de diálogo que crea las ecuaciones del equipo e inicializa sus valores
            //Enviamos al cuadro de dialogo el puntero de la ventana principal para guardar los datos del cuadro de dialogo en variables de la aplicacion principal
            Condensadorprincipal condensadorprincipal1 = new Condensadorprincipal(this, numecuaciones, numvariables);
            condensadorprincipal1.Show();
        }

        //Opción del Menú Nuevo equipo TIPO 4: BOMBA
        private void bombaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //numequipos++;

            //Llamada al cuadro de diálogo que crea las ecuaciones del equipo e inicializa sus valores
            //Enviamos al cuadro de dialogo el puntero de la ventana principal para guardar los datos del cuadro de dialogo en variables de la aplicacion principal
            //Bomba bomba1 = new Bomba(this, numecuaciones, numvariables);
            //bomba1.Show();

            ListaBomba listbomba = new ListaBomba(this);
            listbomba.Show();
        }

        //Opción del Menú RESOLUCIÓN AUTOMÁTICA del Sistema de Ecuaciones
        public void resoluciónSistemaEcuacionesNoLinealesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Caso de haber Creado un EJEMPLO de Validación del Motor de Cálculo
            if (ejemplovalidacion == 1)
            {
                //Limitar el número de Iteraciones a 100
                if (NumMaxIteraciones > 100)
                {
                    NumMaxIteraciones = 100;
                }

                //Limitar el Error Máximo a 10e-8
                if (ErrorMaxAdmisible < 10e-8)
                {
                    ErrorMaxAdmisible = 10e-8;
                }

                //Desactivamos la Consola (vista de iteraciones intermedias en MS-DOS) del Motor de Cálculo
                consola = 0;

                Motorcalculo motorcalculo1 = new Motorcalculo();
                motorcalculo1.punteroaplicacion(this);
                motorcalculo1.Show();
                motorcalculo1.button1_Click_1(sender, e);
                motorcalculo1.Hide();

                //Llamamos a la opción del Menú NUEVO CÁLCULO
                //toolStripMenuItem11_Click(sender, e);
               
                return;
            }
            
            //Si hace falta crear las ecuaciones, los parámetros y leer las condiciones iniciales. Y sólo es necesario mostrar el motor de cálculo
            else if (marca == 0)
            {
                //Activamos la Consola del motor de cálculo
                consola = 1;

                //Inicializamos el número de corrientes y funciones para realizar cálculos sucesivos
                numcorrientes = 0;
                p.Clear();
                p1.Clear();
                functions.Clear();

                //Generar los parámeteros (variables) correpondiente a la lista de objetos de equipos11 generada por los form de cada tipo de equipo
                for (int t = 0; t < equipos11.Count; t++)
                {
                    generarparametros(equipos11[t].tipoequipo2, equipos11[t].aN1, equipos11[t].aN2, equipos11[t].aN3, equipos11[t].aN4, equipos11[t].aD1, equipos11[t].aD2, equipos11[t].aD3, equipos11[t].aD4, equipos11[t].aD5, equipos11[t].aD6, equipos11[t].aD7, equipos11[t].aD8, equipos11[t].aD9, equipos11[t].adicional11, equipos11[t].adicional12, equipos11[t].adicional13, equipos11[t].adicional14);
                }

                //Utilizamos el array de parametro p1 como array auxiliar para los criterios de busqueda de parametros
                p1 = p;
                
                //Generar las Condiciones Iniciales del Sistema para la Primera Iteración del Método de Newton Raphson

                //Tendremos que tener en cuenta las siguientes consideraciones:
                // a) Caso de haber leido todas las condiciones iniciales de un archivo HBAL (*.DAT)
                // b) Caso de haber leido parte de las condiciones iniciales de un archivo de HBAL (*.DAT)
                // c) Caso de no haber leido ninguna condición inicial de un archivo de HBAL (*.DAT)

                //Caso a) Haber leido todas las condiciones iniciales de un archivo HBAL (*.DAT)
                if ((leidascondicionesiniciales == 1) && (numcondiciniciales == numcorrientes))
                {
                    //ALGORITMO PARA ORDENAR LAS CONCIONES INICIALES
                    //De todas formas las Condiciones Iniclaes ya han sido ordenadas mediante el Algoritmo de LecturaHabal 
                    //Tenemos dos arrays: el numcorrienteinicial[a] y los parámetros del sistema p[j]
                    //Primero: Leer el número de numcorrienteinicial[a] con índice "a" (Bucle leyendo numcorrienteinicial[a])
                    //Segundo: Buscar el número de parámetro que coincida con el numcorrientinicial[a] (Bucle leyendo los parámetros para buscar el que coincida con el numcorrienteinicial[a])
                    //Tercero: Cuando coincidan el numcorrienteinicial[a] igual al número de parámetro, copiar el las condiciones iniciales en los valores del parámetro.

                    for (int a = 0; a <numcorrientes; a++)
                    { 
                        for (int j=0;j<p.Count-2;j++)
                        {
                            //Leemos el Nombre del parámetro, por ejemplo: W1
                            String nom = p[j].Nombre;
                            //Calculaos la longitud del nombre del parámetro, por ejemplo: 2
                            int longitud = nom.Length;
                            //Introducimos en una variable 
                            String tem = nom.Substring(1, longitud - 1);
                            //Introducimos en la variable "tipoparametro" el nombre de parámetro: W
                            String tipoparametro = nom.Substring(0, 1);
                            //Introducimos  en la variable "numcorr" el número de corriente: 1
                            Double numcorr = Convert.ToDouble(tem);

                            //Chequeamos que el número de corriente del parámetro coincide con el número de corriente inicial
                            //Copiamos los valores inciales de la corriente (caudal,presión y entalpía) en los parámetros del programa 

                            if (numcorrienteinicial[a]==numcorr)
                            {
                                p[j].Value = caudalinicial[a];
                                p[j+1].Value = presioninicial[a];
                                p[j+2].Value = entalpiainicial[a];
                                j = p.Count;
                            }
                        }                                                                    
                    }

                    //ALGORITMO ANTIGUO PARA COPIAR LAS CONDICIONES INICIALES
                    //Copiamos las Condiciones iniciales leidas del Archivo de HBAL *.DAT en los Parámeteros Generados
                    //int cont = 0;
                    //int cont1 = 1;
                    //int cont2 = 2;

                   //for (int a = 0; a < numcorrientes; a++)
                   // {
                   //     p[a + cont].Value = caudalinicial[a];
                   //     p[a + cont1].Value = presioninicial[a];
                   //     p[a + cont2].Value = entalpiainicial[a];

                   //     cont = cont + 2;
                   //     cont1 = cont1 + 2;
                  //      cont2 = cont2 + 2;
                   // }
                }

                //ESTUDIAR EL ERROR 
                //Caso b) Haber leido parte de las condiciones iniciales de un archivo de HBAL (*.DAT)
                else if ((leidascondicionesiniciales == 1) && (numcondiciniciales != numcorrientes))
                {
                   //ALGORITMO PARA ORDENAR LAS CONCIONES INICIALES
                    //De todas formas las Condiciones Iniclaes ya han sido ordenadas mediante el Algoritmo de LecturaHabal 
                    //Tenemos dos arrays: el numcorrienteinicial[a] y los parámetros del sistema p[j]
                    //Primero: Leer el número de numcorrienteinicial[a] con índice "a" (Bucle leyendo numcorrienteinicial[a])
                    //Segundo: Buscar el número de parámetro que coincida con el numcorrientinicial[a] (Bucle leyendo los parámetros para buscar el que coincida con el numcorrienteinicial[a])
                    //Tercero: Cuando coincidan el numcorrienteinicial[a] igual al número de parámetro, copiar el las condiciones iniciales en los valores del parámetro.

                    for (int a = 0; a < numcorrientes; a++)
                    {
                        for (int j = 0; j < p.Count - 2; j++)
                        {
                            //Leemos el Nombre del parámetro, por ejemplo: W1
                            String nom = p[j].Nombre;
                            //Calculaos la longitud del nombre del parámetro, por ejemplo: 2
                            int longitud = nom.Length;
                            //Introducimos en una variable 
                            String tem = nom.Substring(1, longitud - 1);
                            //Introducimos en la variable "tipoparametro" el nombre de parámetro: W
                            String tipoparametro = nom.Substring(0, 1);
                            //Introducimos  en la variable "numcorr" el número de corriente: 1
                            Double numcorr = Convert.ToDouble(tem);

                            //Chequeamos que el número de corriente del parámetro coincide con el número de corriente inicial
                            //Copiamos los valores inciales de la corriente (caudal,presión y entalpía) en los parámetros del programa 

                            if (numcorrienteinicial[a] == numcorr)
                            {
                                p[j].Value = caudalinicial[a];
                                p[j + 1].Value = presioninicial[a];
                                p[j + 2].Value = entalpiainicial[a];
                                j = p.Count;
                            }
                        }
                    }
                }

                //Caso c)No haber leido ninguna condición inicial de un archivo de HBAL (*.DAT)
                else
                {
                    for (int i = 0; i < equipos11.Count; i++)
                    {
                        //generarcondicionesiniciales(equipos11[i].tipoequipo2, equipos11[i].aN1, equipos11[i].aN2, equipos11[i].aN3, equipos11[i].aN4, equipos11[i].aD1, equipos11[i].aD2, equipos11[i].aD3, equipos11[i].aD4, equipos11[i].aD5, equipos11[i].aD6, equipos11[i].aD7, equipos11[i].aD8, equipos11[i].aD9, equipos11[i].adicional11, equipos11[i].adicional12, equipos11[i].adicional13, equipos11[i].adicional14);
                    }
                }

                int dimen = p.Count;

                Double[,] matrizauxjacob1 = new Double[dimen, dimen];

                matrizauxjacob = matrizauxjacob1;
                int contecuaciones = 0;

                int temporal = 0;
                int ecuacsobran = 0;
                int ecuacfaltan = 0;

                listView1.Items.Clear();
                listView2.Items.Clear();
                listView3.Items.Clear();

                //Generar Ecuaciones de cada uno de los equipos de la lista equipos11 cuando ya tenemos toda la lista de parámetros creadas y con los nombres de las corrientes asignados
                for (int i = 0; i < equipos11.Count; i++)
                {
                    contecuaciones = generarecuaciones(contecuaciones, equipos11[i].tipoequipo2, equipos11[i].aN1, equipos11[i].aN2, equipos11[i].aN3, equipos11[i].aN4, equipos11[i].aD1, equipos11[i].aD2, equipos11[i].aD3, equipos11[i].aD4, equipos11[i].aD5, equipos11[i].aD6, equipos11[i].aD7, equipos11[i].aD8, equipos11[i].aD9, equipos11[i].adicional11, equipos11[i].adicional12, equipos11[i].adicional13, equipos11[i].adicional14);
                   

//-------------------Rellenamos en el Tabpage15 del Tabcontrol 2 de la apliacion principal el número de ecuaciones generadas por cada equipo --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                    //Estudiamos el Primer elemento de la Lista
                    if (i == 0)
                    {
                        if ((contecuaciones - temporal) < 3)
                        {
                            //listView2.Items[i].BackColor = Color.Red;
                            listView2.Items.Add("Equipo: " + Convert.ToString(equipos11[i].numequipo2) + " Tipo: " + Convert.ToString(equipos11[i].tipoequipo2) + " Ecuac.: " + Convert.ToString(contecuaciones - temporal));
                            ecuacfaltan=ecuacfaltan+(contecuaciones-temporal);
                        }
                        else if ((contecuaciones - temporal) == 3)
                        {
                            //listView1.Items[i].BackColor = Color.White;
                            listView1.Items.Add("Equipo: " + Convert.ToString(equipos11[i].numequipo2) + " Tipo: " + Convert.ToString(equipos11[i].tipoequipo2) + " Ecuac.: " + Convert.ToString(contecuaciones - temporal));
                        }
                        else if ((contecuaciones - temporal) == 5)
                        {
                             listView2.Items.Add("Equipo: " + Convert.ToString(equipos11[i].numequipo2) + " Tipo: " + Convert.ToString(equipos11[i].tipoequipo2) + " Ecuac.: " + Convert.ToString(contecuaciones - temporal));
                            //listView1.Items[i].BackColor = Color.Cyan;
                             ecuacfaltan = ecuacfaltan + (contecuaciones - temporal);
                        }
                        else if ((contecuaciones - temporal) > 6)
                        {
                            listView3.Items.Add("Equipo: " + Convert.ToString(equipos11[i].numequipo2) + " Tipo: " + Convert.ToString(equipos11[i].tipoequipo2) + " Ecuac.: " + Convert.ToString(contecuaciones - temporal));
                            //listView1.Items[i].BackColor = Color.Green;
                            ecuacsobran = ecuacsobran + (contecuaciones - temporal);
                        }
                        else if ((contecuaciones - temporal) == 6)
                        {
                            listView1.Items.Add("Equipo: " + Convert.ToString(equipos11[i].numequipo2) + " Tipo: " + Convert.ToString(equipos11[i].tipoequipo2) + " Ecuac.: " + Convert.ToString(contecuaciones - temporal));
                            //listView1.Items[i].BackColor = Color.White;
                        }
                        else if ((contecuaciones - temporal) == 4)
                        {
                            listView3.Items.Add("Equipo: " + Convert.ToString(equipos11[i].numequipo2) + " Tipo: " + Convert.ToString(equipos11[i].tipoequipo2) + " Ecuac.: " + Convert.ToString(contecuaciones - temporal));
                            //listView1.Items[i].BackColor = Color.Green;
                            ecuacsobran = ecuacsobran + (contecuaciones - temporal);
                        }
                        temporal = contecuaciones;                     
                    }

                    //Estudiamos el resto de elementos de la Lista
                    else 
                    {
                        if ((contecuaciones - temporal) < 3)
                        {
                            //listView2.Items[i].BackColor = Color.Red;
                            listView2.Items.Add("Equipo: " + Convert.ToString(equipos11[i].numequipo2) + " Tipo: " + Convert.ToString(equipos11[i].tipoequipo2) + " Ecuac.: " + Convert.ToString(contecuaciones - temporal));
                            ecuacfaltan = ecuacfaltan + (contecuaciones - temporal);
                        }
                        else if ((contecuaciones - temporal) == 3)
                        {
                            //listView1.Items[i].BackColor = Color.White;
                            listView1.Items.Add("Equipo: " + Convert.ToString(equipos11[i].numequipo2) + " Tipo: " + Convert.ToString(equipos11[i].tipoequipo2) + " Ecuac.: " + Convert.ToString(contecuaciones - temporal));
                        }
                        else if ((contecuaciones - temporal) == 5)
                        {
                            listView2.Items.Add("Equipo: " + Convert.ToString(equipos11[i].numequipo2) + " Tipo: " + Convert.ToString(equipos11[i].tipoequipo2) + " Ecuac.: " + Convert.ToString(contecuaciones - temporal));
                            //listView1.Items[i].BackColor = Color.Cyan;
                            ecuacfaltan = ecuacfaltan + (contecuaciones - temporal);
                        }
                        else if ((contecuaciones - temporal) > 6)
                        {
                            listView3.Items.Add("Equipo: " + Convert.ToString(equipos11[i].numequipo2) + " Tipo: " + Convert.ToString(equipos11[i].tipoequipo2) + " Ecuac.: " + Convert.ToString(contecuaciones - temporal));
                            //listView1.Items[i].BackColor = Color.Green;
                            ecuacsobran = ecuacsobran + (contecuaciones - temporal);
                        }
                        else if ((contecuaciones - temporal) == 6)
                        {
                            listView1.Items.Add("Equipo: " + Convert.ToString(equipos11[i].numequipo2) + " Tipo: " + Convert.ToString(equipos11[i].tipoequipo2) + " Ecuac.: " + Convert.ToString(contecuaciones - temporal));
                            //listView1.Items[i].BackColor = Color.White;
                        }
                        else if ((contecuaciones - temporal) == 4)
                        {
                            listView3.Items.Add("Equipo: " + Convert.ToString(equipos11[i].numequipo2) + " Tipo: " + Convert.ToString(equipos11[i].tipoequipo2) + " Ecuac.: " + Convert.ToString(contecuaciones - temporal));
                            //listView1.Items[i].BackColor = Color.Green;
                            ecuacsobran = ecuacsobran + (contecuaciones - temporal);
                        }

                        temporal = contecuaciones;
                    }

                    if (i == equipos11.Count - 1)
                    {
                        listView1.Items.Add("Number of Equations Generated: " + Convert.ToString(contecuaciones));
                        listView1.Items.Add("Number of System Variables: " + Convert.ToString(p.Count));
                        listView2.Items.Add("Number of Equations Generated: " + Convert.ToString(ecuacfaltan));
                        listView3.Items.Add("Number of Equations Generated: " + Convert.ToString(ecuacsobran));
                    }

//---------------Fin de rellenar en el tabpage15 del tabcontrol2 de la aplicación, el número de ecuaciones generadas por cada equipo------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                }

                matrizauxjacob1 = matrizauxjacob;

                //Limitar el número de Iteraciones a 100
                if (NumMaxIteraciones > 100)
                {
                    MessageBox.Show(this, "El número máximo de iteraciones se ha limitado a 100 para acelerar la resolución del problema.", "Información del Número Máximo de iteraciones", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    NumMaxIteraciones = 100;
                }

                //Limitar el Error Máximo a 10e-8
                if (ErrorMaxAdmisible < 10e-8)
                {
                    //Mensaje para advertir que el Error Máximo admisible era menor de 10e-8 y por tanto se ha fijado a 10e-8
                    MessageBox.Show(this, "Error Máximo admisible fijado a un valor inferior a 10e-8, para facilitar la convergencia se fija a 10e-8, en todo caso el usuario puede modificar su valor en el siguiente cuadro de diálogo donde se fijan las opciones del Motor de Cálculo.", "Recomendación del Error Máximo Admisible", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    ErrorMaxAdmisible = 10e-8;
                }
            }

             //Si no hace falta crear las ecuaciones, los parámetros y leer las condiciones iniciales. Y sólo es necesario mostrar el motor de cálculo
            else if (marca == 1)
            {
                //Activamos la vista Consola (Iteraciones Intermedias en MS-DOS)
                consola = 1;
            }

            //Añadimos las ecuaciones generadas por el Sistema al Control ListBox
            for (int j = 0; j < ecuaciones.Count; j++)
            {
                listBox5.Items.Add(Convert.ToString(ecuaciones[j]));
            }

            Motorcalculo motorcalculo2 = new Motorcalculo();
            motorcalculo2.punteroaplicacion(this);
            motorcalculo2.Show();
            motorcalculo2.button1_Click_1(sender, e);
            motorcalculo2.Hide();

            //Inicializamos el número de equipos del modelo calculado
            numtipo1 = 0;
            numtipo2 = 0;
            numtipo3 = 0;
            numtipo4 = 0;
            numtipo5 = 0;
            numtipo6 = 0;
            numtipo7 = 0;
            numtipo8 = 0;
            numtipo9 = 0;
            numtipo10 = 0;
            numtipo11 = 0;
            numtipo12 = 0;
            numtipo13 = 0;
            numtipo14 = 0;
            numtipo15 = 0;
            numtipo16 = 0;
            numtipo17 = 0;
            numtipo18 = 0;
            numtipo19 = 0;
            numtipo20 = 0;
            numtipo21 = 0;
            numtipo22 = 0;

            //Guardar los Resultados de Corrientes y Equipos
            guardaresultadoscalculos();

            //Pulsamos la opción del Menú Principal Streams results
            visualizarResultadosToolStripMenuItem_Click(sender, e);

            //Muestro las pestañas 1 y 8 de los controles Tab de la Aplicación principal
            tabControl1.SelectedTab = tabPage1;
            tabControl2.SelectedTab = tabPage8;
        }

        //Opción del Menú Nuevo equipo TIPO 7: CALENTADOR
        private void calentadorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //numequipos++;

            //Llamada al cuadro de diálogo que crea las ecuaciones del equipo e inicializa sus valores
            //Enviamos al cuadro de dialogo el puntero de la ventana principal para guardar los datos del cuadro de dialogo en variables de la aplicacion principal
            //Calentador calentador1 = new Calentador(this, numecuaciones, numvariables);
            //calentador1.Show();
            ListaCalentador7 listcalentador7 = new ListaCalentador7(this);
            listcalentador7.Show();
        }

        //Opción del Menú Nuevo equipo TIPO 14: MSR
        private void mSRTipo14ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //numequipos++;

            //Llamada al cuadro de diálogo que crea las ecuaciones del equipo e inicializa sus valores
            //Enviamos al cuadro de dialogo el puntero de la ventana principal para guardar los datos del cuadro de dialogo en variables de la aplicacion principal
            //MSR MSR1 = new MSR(this, numecuaciones, numvariables);
            //MSR1.Show();

            ListaMSR listmsr = new ListaMSR(this);
            listmsr.Show();
        }

        //Ayuda del Manual HBAL
        private void manualHBALToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            Process.Start("C:\\Users\\LUISCOCO\\Desktop\\Bal LUIS\\Bal LUIS\\Bal LUIS\\Bal LUIS COCO\\Manual HBAL.pdf");
        }
        
        private void manualSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // ruta donde se hallan los PDFs (tal vez quieras incluir:
            Process.Start("C:\\Users\\LUISCOCO\\Desktop\\Bal LUIS\\Bal LUIS\\Bal LUIS\\Bal LUIS COCO\\Manual SBAL.pdf");
        }

        private void conversorUnidadesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // ruta donde se hallan los PDFs (tal vez quieras incluir:
            string ruta = "C:\\Users\\LUISCOCO\\Desktop\\Desarrollo Software LUIS COCO\\";

            // iniciamos la instancia de la clase process
            System.Diagnostics.Process proc = new System.Diagnostics.Process();

            // nombre del archivo a abrir
            proc.StartInfo.FileName = ruta + "Convert.exe";

            // arrancamos el proceso
            proc.Start();

            // liberamos recursos
            proc.Close();   // atención: close no termina el proceso
        }

        private void steamTablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // ruta donde se hallan los PDFs (tal vez quieras incluir:
            string ruta = "C:\\Users\\LUISCOCO\\Desktop\\Desarrollo Software LUIS COCO\\";

            // iniciamos la instancia de la clase process
            System.Diagnostics.Process proc = new System.Diagnostics.Process();

            // nombre del archivo a abrir
            proc.StartInfo.FileName = ruta + "Steam.exe";

            // arrancamos el proceso
            proc.Start();

            // liberamos recursos
            proc.Close();   // atención: close no termina el proceso
        }

        private void diagramasDeMollierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Diagrama_Mollier diagrama1 = new Diagrama_Mollier();
            diagrama1.Show();
        }

        //Opción del Menú LISTADO DE EQUIPOS
        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            Listados listado1 = new Listados(this);
            listado1.Show(this);
        }

        //Abrir Heat Balance Scheme (*.bmp)
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Elegimos las pestañas 5 y 1 de los controles Tab de la Aplicación principal
            tabControl1.SelectedTab = tabPage1;
            tabControl2.SelectedTab = tabPage5;
          
            
            // Create new SaveFileDialog object
            OpenFileDialog OpenFileDialog = new OpenFileDialog();

            // Default file extension
            OpenFileDialog.DefaultExt = "bmp";

            // Available file extensions
            OpenFileDialog.Filter = "Esquemas files (*.bmp)|*.bmp|All files (*.*)|*.*";

            // Adds a extension if the user does not
            OpenFileDialog.AddExtension = true;

            // Restores the selected directory, next time
            OpenFileDialog.RestoreDirectory = true;

            // Dialog title
            OpenFileDialog.Title = "Where do you want to save the file?";

            // Startup directory
            OpenFileDialog.InitialDirectory = @"C:/";

            // Show the dialog and process the result
            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.pictureBox2.Image = new Bitmap(OpenFileDialog.OpenFile());
                //this.pictureBox3.Image = new Bitmap(OpenFileDialog.OpenFile());
            }
            else
            {
                MessageBox.Show("You hit cancel or closed the dialog.");
            }

            OpenFileDialog.Dispose();
            OpenFileDialog = null;

            //Opción "ZOOM" del Menú Contextual de la imagen del Heat Balance 
            //Esta opción nos pemite encuadrar la imagen del Balance
            toolStripMenuItem15_Click(sender, e);

        }

        //Opción del Menú Nuevo equipo TIPO 20: DIVISOR ENTALPÍA FIJA
        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            //numequipos++;

            //Llamada al cuadro de diálogo que crea las ecuaciones del equipo e inicializa sus valores
            //Enviamos al cuadro de dialogo el puntero de la ventana principal para guardar los datos del cuadro de dialogo en variables de la aplicacion principal
            //Divisorentalpiafija divisorentalpiafija1 = new Divisorentalpiafija(this, numecuaciones, numvariables);
            //divisorentalpiafija1.Show();

            ListaDivisorEntalpiaFija diventalfija = new ListaDivisorEntalpiaFija(this);
            diventalfija.Show();
        }

        //Opción del Menú Nuevo equipo TIPO 19: VALVULA
        private void válvulaTipo19ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //numequipos++;

            //Llamada al cuadro de diálogo que crea las ecuaciones del equipo e inicializa sus valores
            //Enviamos al cuadro de dialogo el puntero de la ventana principal para guardar los datos del cuadro de dialogo en variables de la aplicacion principal
            //Valvula valvula1 = new Valvula(this, numecuaciones, numvariables,0,0);
            //valvula1.Show();
            ListaValvula19 listavalvula = new ListaValvula19(this);
            listavalvula.Show();
        }

        //Opción del Menú Nuevo equipo TIPO 8: CONDENSADOR
        private void condensadorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //numequipos++;

            //Llamada al cuadro de diálogo que crea las ecuaciones del equipo e inicializa sus valores
            //Enviamos al cuadro de dialogo el puntero de la ventana principal para guardar los datos del cuadro de dialogo en variables de la aplicacion principal
            //Condensador condensador1 = new Condensador(this, numecuaciones, numvariables,0,0);
            //condensador1.Show();

            ListaCondensador listcond15 = new ListaCondensador(this);
            listcond15.Show();
        }

        //Opción del Menú Nuevo equipo TIPO 21: TANQUE DE VAPORIZACIÓN
        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            //numequipos++;

            //Llamada al cuadro de diálogo que crea las ecuaciones del equipo e inicializa sus valores
            //Enviamos al cuadro de dialogo el puntero de la ventana principal para guardar los datos del cuadro de dialogo en variables de la aplicacion principal
            //TanqueVaporizacion tanque1 = new TanqueVaporizacion(this, numecuaciones, numvariables,0,0);
            //tanque1.Show();

            ListaFlashTank21 listflash = new ListaFlashTank21(this);
            listflash.Show();
        }

        //Opción de Cálculo del Condensador mediante la HEI
        private void calculoCondensadorHEIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            numequipos++;
            CalculoCondensador calccondensador = new CalculoCondensador();
            calccondensador.Show();
        }

        //Opción del Menú Nuevo equipo TIPO 11: TURBINA AUXILIAR
        private void turbinaAuxiliarTipo11ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //numequipos++;

            //Llamada al cuadro de diálogo que crea las ecuaciones del equipo e inicializa sus valores
            //Enviamos al cuadro de dialogo el puntero de la ventana principal para guardar los datos del cuadro de dialogo en variables de la aplicacion principal
            //TurbinaAuxiliar turbinaauxiliar1 = new TurbinaAuxiliar(this, numecuaciones, numvariables);
            //turbinaauxiliar1.Show();

            ListaTurbinaAuxiliar11 listTurbAuxi = new ListaTurbinaAuxiliar11(this);
            listTurbAuxi.Show();
        }

        public void lanzardera()
        {
            Process notePad = new Process();
            notePad.StartInfo.FileName = "C:\\Windows\\notepad.exe";
            notePad.StartInfo.Arguments = rutaresultadoshbal;
            notePad.Start();
        }

        //Opción del Menú de DATOS GENERALES de la Aplicación
        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            Datosgenerales datosgenerales1 = new Datosgenerales(this);
            datosgenerales1.Show();
        }


        public void crearcondicionesiniciales()
        {
            //Barrer todos los Equipos creados
            //Ver Tipo Equipo (dependiendo del tipo de equipo se generarán una u otro tipo de condiciones iniciales)
            //Ver Nombre Corrientes de Entrada y Salida

            //Buscar los parámeteros con los nombre de las Corrientes de Entrada y Salida
        }


        //Botón NUEVO CÁLCULO
        public void toolStripMenuItem11_Click(object sender, EventArgs e)
        {   
       
            //Inicializamos los Tipos de Análisis
            //Análisis Estacionario
            tipoanalisis = 0;
            //Análisis Estacionario de un Sistema de Ecuaciones No Lineales
            tipoanalisisestacionario = 0;
            
            //Incógnita que indica que hay que generar ecuaciones, parámetros y leer las condiciones iniciales
            marca = 0;

            //Inicializamos todas las Variables de la Aplicación
            rutaresultadoshbal = "";

            numcorrientes = 0;
            p.Clear();
            p1.Clear();
            functions.Clear();
            
            for (int g = 0; g < 10000; g++)
            {
                caudalinicial[g] = 0;
                presioninicial[g] = 0;
                entalpiainicial[g] = 0;
            }

            leidascondicionesiniciales = 0;

            Titulo = "";
            NombreArchivo = "";
            NumTotalEquipos = 0;
            NumTotalCorrientes = 0;
            NumMaxIteraciones = 200;
            NumTotalTablas = 0;
            ErrorMaxAdmisible = 1E-8;
            DatosIniciales = 0;
            FactorIteraciones = 0.5;
            FicheroIteraciones = 1;
            unidades = 2;

            numequipos = 0;
            numecuaciones = 0;
            numvariables = 0;
            equipos11.Clear();
            Hbalfile.Clear();
            listaTablas.Clear();
            listanumTablas.Clear();
            listanumDatosenTabla.Clear();

            //Limpiamos la Lista de Resultados de los Equipos
            condiciones1.Clear();
            divisores2.Clear();
            perdidas3.Clear();
            bombas4.Clear();
            mezcladores5.Clear();
            reactores6.Clear();
            calentadores7.Clear();
            condensadores8.Clear();
            turbinas9.Clear();
            turbinas10.Clear();
            turbinas11.Clear();
            sephumedadlista13.Clear();
            msrlista14.Clear();
            condensadorlista15.Clear();
            enfriadorlista16.Clear();
            atemperadores17.Clear();
            desaireadores18.Clear();
            valvulas19.Clear();
            divisoresentalpia20.Clear();
            tanquesvaporizacion21.Clear();
            intercambiadores22.Clear();

            if (borrarmatrixequipos == 1)
            {
                //Limpiamos las Matrices de Equipos y el SetNumber
                setnumber = 1;

                //Limpiamos el ComboBox3 que guarda el Número de Cálculos (setnumber)
                comboBox3.Items.Clear();
                //Inicializamos el ComoBox3 que guarda el Número de Cálculos (setnumber) al valor 1
                comboBox3.Items.Add(Convert.ToString(setnumber));

                matrixcondicioncontorno1 = null;
                matrixdivisor2 = null;
                matrixperdida3 = null;
                matrixbomba4 = null;
                matrixmezclador5 = null;
                matrixreactor6 = null;
                matrixcalentador7 = null;
                matrixcondensador8 = null;
                matrixturbina9 = null;
                matrixturbina10 = null;
                matrixturbina11 = null;
                matrixseparador13 = null;
                matrixMSR14 = null;
                matrixcondensador15 = null;
                matrixenfriador16 = null;
                matrixatemperador17 = null;
                matrixdesaireador18 = null;
                matrixvalvula19 = null;
                matrixdivisor20 = null;
                matrixtanque21 = null;
                matrixintercambiador22 = null;


                //Matrices de Resultados de los Equipos de las diferentes Corridas de Cálculo
                //Estamos limitando el número de Equipos a 2000 y el número de setnumber (número de cálculos del análisis de sensibilidad) a 200
                matrixcondicioncontorno1 = new ClassCondicionContorno1[2000, 200];
                matrixdivisor2 = new ClassDivisor2[2000, 200];
                matrixperdida3 = new ClassPerdidaCarga3[2000, 200];
                matrixbomba4 = new ClassBomba4[2000, 200];
                matrixmezclador5 = new ClassMezclador5[2000, 200];
                matrixreactor6 = new ClassReactor6[2000, 200];
                matrixcalentador7 = new ClassCalentador7[2000, 200];
                matrixcondensador8 = new ClassCondensador8[2000, 200];
                matrixturbina9 = new ClassTurbina9[2000, 200];
                matrixturbina10 = new ClassTurbina10[2000, 200];
                matrixturbina11 = new ClassTurbina11[2000, 200];
                matrixseparador13 = new ClassSeparadorHumedad13[2000, 200];
                matrixMSR14 = new ClassMSR14[2000, 200];
                matrixcondensador15 = new ClassCondensador15[2000, 200];
                matrixenfriador16 = new ClassEnfriadorDrenajes16[2000, 200];
                matrixatemperador17 = new ClassAtemperador17[2000, 200];
                matrixdesaireador18 = new ClassDesaireador18[2000, 200];
                matrixvalvula19 = new ClassValvula19[2000, 200];
                matrixdivisor20 = new ClassDivisorEntalpiaFija20[2000, 200];
                matrixtanque21 = new ClassTanqueVaporizacion21[2000, 200];
                matrixintercambiador22 = new ClassIntercambiador22[2000, 200];
                
                //Inicializamos el número de equipos del modelo calculado
                numtipo1 = 0;
                numtipo2 = 0;
                numtipo3 = 0;
                numtipo4 = 0;
                numtipo5 = 0;
                numtipo6 = 0;
                numtipo7 = 0;
                numtipo8 = 0;
                numtipo9 = 0;
                numtipo10 = 0;
                numtipo11 = 0;
                numtipo12 = 0;
                numtipo13 = 0;
                numtipo14 = 0;
                numtipo15 = 0;
                numtipo16 = 0;
                numtipo17 = 0;
                numtipo18 = 0;
                numtipo19 = 0;
                numtipo20 = 0;
                numtipo21 = 0;
                numtipo22 = 0;
            }
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            ListaCondContorno lista1 = new ListaCondContorno(this);
            lista1.Show();
        }

        //Opción del Menú Nuevo equipo TIPO 6: REACTOR
        private void calderaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //numequipos++;

            //Llamada al cuadro de diálogo que crea las ecuaciones del equipo e inicializa sus valores
            //Enviamos al cuadro de dialogo el puntero de la ventana principal para guardar los datos del cuadro de dialogo en variables de la aplicacion principal
            //Reactor reactor1 = new Reactor(this, numecuaciones, numvariables);
            //reactor1.Show();

            ListaReactor listreactor = new ListaReactor(this);
            listreactor.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            About about1 = new About();
            about1.Show();
        }

        private void splitContainer1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Button)))
            {
                if (this.tipoequipodrag == 1)
                {
                    numequipos++;

                    //Llamada al cuadro de diálogo que crea las ecuaciones del equipo e inicializa sus valores
                    //Enviamos al cuadro de dialogo el puntero de la ventana principal para guardar los datos del cuadro de dialogo en variables de la aplicacion principal
                    Condcontorno Condcontorno1 = new Condcontorno(this, numecuaciones, numvariables, 0, 0);
                    Condcontorno1.Show();
                }
            }
        }

        //Drag and Drop Tipo 1: CONDICIÓN DE CONTORNO
        private void button10_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Button)))
            {
                if (this.tipoequipodrag == 1)
                {

                    //this.equipos.Add(new PictureBox());
                    //((PictureBox)this.equipos[numequipos]).Location = new System.Drawing.Point(e.X, e.Y);
                    //((PictureBox)this.equipos[numequipos]).Name = "PictureBox" + Convert.ToString(numequipos);
                    //((PictureBox)this.equipos[0]).Size = new System.Drawing.Size(100, 50);
                    //((PictureBox)this.equipos[numequipos]).TabIndex = 2;
                    //((PictureBox)this.equipos[numequipos]).Image = System.Drawing.Bitmap.FromFile("C:\\Users\\luis\\Desktop\\Iconos PEPSE\\Fuente.bmp");
                    //((PictureBox)this.equipos[numequipos]).SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
                    //this.Controls.Add(((PictureBox)this.equipos[numequipos]));


                    numequipos++;

                    //Llamada al cuadro de diálogo que crea las ecuaciones del equipo e inicializa sus valores
                    //Enviamos al cuadro de dialogo el puntero de la ventana principal para guardar los datos del cuadro de dialogo en variables de la aplicacion principal
                    Condcontorno Condcontorno1 = new Condcontorno(this, numecuaciones, numvariables, 0, 0);
                    Condcontorno1.Show();
                }
            }
        }

        //Botón para ACTUALIZAR la vista de ARBOL TREEVIEW 
        public void actualizararbol()
        {
            treeView1.Nodes.Clear();
            treeView2.Nodes.Clear();
            treeView3.Nodes.Clear();

            TreeNode luis = new TreeNode();

            TreeNode node1 = treeView1.Nodes.Add("DATOS GENERALES");
            node1.Nodes.Add("Título del Archivo: " + Titulo);
            node1.Nodes.Add("Nº Total de Equipos: " + Convert.ToString(NumTotalEquipos));
            node1.Nodes.Add("Nº Total de Corrientes: " + Convert.ToString(NumTotalCorrientes));
            node1.Nodes.Add("Nº Total de Tablas: " + Convert.ToString(NumTotalTablas));
            node1.Nodes.Add("Nº Máximo de Iteraciones: " + Convert.ToString(NumMaxIteraciones));
            node1.Nodes.Add("Error Máximo Admisible: " + Convert.ToString(ErrorMaxAdmisible));
            node1.Nodes.Add("Factor de Iteraciones (EPS): " + Convert.ToString(FactorIteraciones));
            node1.Nodes.Add("Unidades: " + "Sistema Internacional");

            TreeNode node2 = treeView2.Nodes.Add("EQUIPOS");
            for (int i = 0; i < equipos11.Count; i++)
            {
                TreeNode node7 = node2.Nodes.Add("Número de Equipo: " + Convert.ToString(equipos11[i].numequipo2));
                TreeNode node8 = node7.Nodes.Add("Tipo de Equipo: " + Convert.ToString(equipos11[i].tipoequipo2));
                TreeNode node9 = node7.Nodes.Add("Corriente Entrada1: " + Convert.ToString(equipos11[i].aN1));
                TreeNode node10 = node7.Nodes.Add("Corriente Entrada2: " + Convert.ToString(equipos11[i].aN2));
                TreeNode node11 = node7.Nodes.Add("Corriente Salida1: " + Convert.ToString(equipos11[i].aN3));
                TreeNode node12 = node7.Nodes.Add("Corriente Salida2: " + Convert.ToString(equipos11[i].aN4));
            }

             TreeNode node211 = treeView3.Nodes.Add("EQUIPOS");
             for (int i = 0; i < equipos11.Count; i++)
             {
                 TreeNode node71 = node211.Nodes.Add("Número de Equipo: " + Convert.ToString(equipos11[i].numequipo2));
                 TreeNode node81 = node71.Nodes.Add("Tipo de Equipo: " + Convert.ToString(equipos11[i].tipoequipo2));
                 TreeNode node91 = node71.Nodes.Add("Corriente Entrada1: " + Convert.ToString(equipos11[i].aN1));
                 TreeNode node101 = node71.Nodes.Add("Corriente Entrada2: " + Convert.ToString(equipos11[i].aN2));
                 TreeNode node111 = node71.Nodes.Add("Corriente Salida1: " + Convert.ToString(equipos11[i].aN3));
                 TreeNode node121 = node71.Nodes.Add("Corriente Salida2: " + Convert.ToString(equipos11[i].aN4));

             }

            TreeNode node5 = treeView1.Nodes.Add("TABLAS");
            for (int j = 0; j < listaTablas.Count; j++)
            {
                TreeNode node14 = node5.Nodes.Add("TÍTULO DE LA TABLA: " + listaTituloTabla[j]);
                TreeNode node13 = node5.Nodes.Add("Número de Tabla: " + Convert.ToString(listanumTablas[j]));
                TreeNode node15 = node5.Nodes.Add("Título de EjeX: " + listaTituloEjeXTabla[j]);
                TreeNode node16 = node5.Nodes.Add("Título de EjeY: " + listaTituloEjeYTabla[j]);
                TreeNode node17 = node5.Nodes.Add("Tipo de Interpolación: " + Convert.ToString(listanumTipoInterpolacionTabla[j]));
            }

            TreeNode node20 = treeView1.Nodes.Add("ECUACIONES DEL MODELO");
            TreeNode node21 = node20.Nodes.Add("Nº Total de Ecuaciones del Modelo: " + Convert.ToString(numecuaciones));

            TreeNode node22 = treeView1.Nodes.Add("VARIABLES DEL MODELO");
            TreeNode node23 = node22.Nodes.Add("Nº Total de Varibales del Modelo: " + Convert.ToString(NumTotalCorrientes * 3));
            TreeNode node24 = node22.Nodes.Add("Valor de las Varibales del Modelo:");
            for (int j = 0; j < p.Count; j++)
            {
                TreeNode node25 = node24.Nodes.Add(p[j].Nombre + " :" + Convert.ToString(p[j].Value));
            }

            TreeNode node18 = treeView1.Nodes.Add("CONDICIONES INICIALES");
            TreeNode node19 = node18.Nodes.Add("Nº Condiciones Iniciales: " + Convert.ToString(numcondiciniciales));
            TreeNode node26 = node18.Nodes.Add("Valor de las Condiciones Iniciales:");
        }


        //Botón del Menú para VISUALIZAR RESULTADOS DE CORRIENTES
        public void visualizarResultadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Elegimos las pestañas 1 y 8 de los controles tab de la aplicación principal
            tabControl1.SelectedTab = tabPage1;
            tabControl2.SelectedTab = tabPage8;

            Double num = p.Count;

            for (int a = 0; a < num; a++)
            {
                p5.Add(ptemp);
            }

            //Limpiamos el contenido del ListBox del Tabpage "StreamResults" en el tabcontrol2 de la aplicación principal
            listBox1.Items.Clear();

            //Caso de haber Creado un EJEMPLO de Validación del Motor de Cálculo y ser el Análisis Tipo Estacionario
            if ((ejemplovalidacion == 1)&&(tipoanalisis==0))
            {
                listBox1.Items.Add("Validación del Motor de Cálculo");
                listBox1.Items.Add("");
                listBox1.Items.Add("Tipo Análisis Estacionario.");
                listBox1.Items.Add("");
                listBox1.Items.Add("");

                for (int i = 0; i < p.Count; i++)
                {
                    p5[i].Nombre = p[i].Nombre;
                    p5[i].Value = p[i].Value;
                    listBox1.Items.Add(p5[i].ToString());
                }

                ejemplovalidacion = 0;
            }

            //Caso de haber Creado un EJEMPLO de Validación del Motor de Cálculo y ser el Análisis Transitorio
            else if ((ejemplovalidacion == 1)&&(tipoanalisis==1))
            {
                this.chart2.Series.Clear();

                //Elegimos las pestañas 1 y 8 de los controles tab de la aplicación principal
                tabControl1.SelectedTab = tabPage21;
                tabControl2.SelectedTab = tabPage8;

                //Análisis de Transitorio tipo 0 (realizado con la librería de TrentGuidry)
                if (tipoanalisistransitorio == 0)
                {
                    listBox1.Items.Add("Validación del Motor de Cálculo.");
                    listBox1.Items.Add("");
                    listBox1.Items.Add("Tipo Análisis Transitorio.");
                    listBox1.Items.Add("");
                    listBox1.Items.Add("");                   

                    //El primer bucle barre el número de iteraciones=(tiempofinal-tiempoinicial)/stepsize
                    for (int i = 0; i < dRes.GetLength(0); i++)
                    {
                        //El segundo bucle recorre todos los parámetros
                        for (int j= 0; j < dRes[0].GetLength(0); j++)
                        {
                            listBox1.Items.Add(p[j].Nombre +"= "+ dRes[i][j].ToString());                           
                        }

                        listBox1.Items.Add("");
                    }

                    //Graficamos los resultados del Análisis TRANSITORIO 
                    //Tendrá que haber tantas series como funciones dependientes del tiempo                    
                    List < Series > graficas= new List<Series>();

                    //Hay tantas funciones dependientes como parámetros menos uno (que representa el tiempo)
                    for (int i = 0; i < dRes[0].GetLength(0)-1; i++)
                    {
                        Series serietemp = new Series();
                        serietemp.Name = "MyGraph" + Convert.ToString(i);
                        serietemp.Color = Color.Blue;
                        serietemp.Legend = "Legend1";
                        serietemp.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
                        serietemp.ChartArea = "ChartArea1";
                        serietemp.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

                        graficas.Add(serietemp);
                        serietemp = null;

                        this.chart2.Series.Add(graficas[i]);
                    }

                    //Primer Bucle que indica el número de intervalos de integración =tiempofinal-tiempoinicial/stepsize
                    for (int i = 0; i < dRes.GetLength(0); i++)
                    {
                        //Segundo Bucle que indica el número de funciones a graficar
                        for (int j = 0; j < dRes[0].GetLength(0)-1; j++)
                        {
                            chart2.Series["MyGraph" + Convert.ToString(j)].Points.AddXY(dRes[i][0], dRes[i][j+1]);
                        }
                    }

                    graficas = null;
                    
                    ejemplovalidacion = 0;
                }

                //Análsis de Transitorio tipo 1 (realizado con la librería de DotNumerics)
                else if (tipoanalisistransitorio == 1) 
                {
                    listBox1.Items.Add("Validación del Motor de Cálculo.");
                    listBox1.Items.Add("");
                    listBox1.Items.Add("Tipo Análisis Transitorio.");
                    listBox1.Items.Add("");
                    listBox1.Items.Add("");

                    //El primer bucle barre el número de iteraciones=(tiempofinal-tiempoinicial)/stepsize
                    for (int i = 0; i < sol.GetLength(0); i++)
                    {
                        //El segundo bucle recorre todos los parámetros
                        for (int j = 0; j < sol.GetLength(1); j++)
                        {
                            listBox1.Items.Add(p[j].Nombre + "= " + sol[i,j].ToString());
                        }

                        listBox1.Items.Add("");
                    }

                    //Graficamos los resultados del Análisis TRANSITORIO 
                    //Tendrá que haber tantas series como funciones dependientes del tiempo                    
                    List<Series> graficas = new List<Series>();

                    //Hay tantas funciones dependientes como parámetros menos uno (que representa el tiempo)
                    for (int i = 0; i < sol.GetLength(1) - 1; i++)
                    {
                        Series serietemp = new Series();
                        serietemp.Name = "MyGraph" + Convert.ToString(i);
                        serietemp.Color = Color.Blue;
                        serietemp.Legend = "Legend1";
                        serietemp.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
                        serietemp.ChartArea = "ChartArea1";
                        serietemp.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

                        graficas.Add(serietemp);
                        serietemp = null;

                        this.chart2.Series.Add(graficas[i]);
                    }

                    //Primer Bucle que indica el número de intervalos de integración =tiempofinal-tiempoinicial/stepsize
                    for (int i = 0; i < sol.GetLength(0); i++)
                    {
                        //Segundo Bucle que indica el número de funciones a graficar
                        for (int j = 0; j < sol.GetLength(1) - 1; j++)
                        {
                            chart2.Series["MyGraph" + Convert.ToString(j)].Points.AddXY(sol[i,0], sol[i,j + 1]);
                        }
                    }

                    graficas = null;

                    ejemplovalidacion = 0;
                }
            }

            //Caso de no estar ejecutando un ejemplo de validación del Motor de Cáculo
            else
            {
                //Unidades Sistema Internacional
                if (unidades == 1)
                {
                    listBox1.Items.Add("Unidades: W(Kgr/sg), P(kPa), H(Kj/Kgr)");
                    listBox1.Items.Add("");
                }

                //Unidades Métricas
                else if (unidades == 2)
                {
                    listBox1.Items.Add("Unidades: W(Kgr/sg), P(Bar), H(Kj/Kgr)");
                    listBox1.Items.Add("");
                }
                //Unidades Británicas

                else if (unidades == 0)
                {
                    listBox1.Items.Add("Unidades: W(Lb/sg), P(psia), H(BTU/Lb)");
                    listBox1.Items.Add("");
                }

                listBox1.Items.Add("Nº Variables:" + Convert.ToString(numvariables));
                listBox1.Items.Add("Nombre de las variables:");
                listBox1.Items.Add("");

                for (int i = 0; i < p.Count; i++)
                {
                    //Unidades
                    //Sistema Britanico=0;Sistema Internacional=1;Sistema Métrico=2

                    //Unidades Sistema Internacional
                    if (unidades == 1)
                    {
                        String primercaracter =listaresultadoscorrientes[i, setnumber].Nombre.Substring(0, 1);

                        if (primercaracter == "W")
                        {
                            p5[i].Value = listaresultadoscorrientes[i, setnumber].Value * (0.4536);
                        }
                        else if (primercaracter == "P")
                        {
                            p5[i].Value = listaresultadoscorrientes[i, setnumber].Value * (6.8947572);
                        }
                        else if (primercaracter == "H")
                        {
                            p5[i].Value = listaresultadoscorrientes[i, setnumber].Value * 2.326009;
                        }

                        p5[i].Nombre = listaresultadoscorrientes[i, setnumber].Nombre;
                        listBox1.Items.Add(p5[i].ToString());
                        //listBox1.Items.Add("---");
                        //listBox1.Items.Add(listaresultadoscorrientes[i, setnumber].ToString());    
                    }

                    //Unidades Sistema Métrico
                    else if (unidades == 2)
                    {
                        String primercaracter =listaresultadoscorrientes[i, setnumber].Nombre.Substring(0, 1);

                        if (primercaracter == "W")
                        {
                            (p5[i].Value) = ((listaresultadoscorrientes[i, setnumber].Value) * 0.4536);
                        }
                        else if (primercaracter == "P")
                        {
                            p5[i].Value = listaresultadoscorrientes[i, setnumber].Value * (6.8947572 / 100);
                        }
                        else if (primercaracter == "H")
                        {
                            p5[i].Value = listaresultadoscorrientes[i, setnumber].Value * 2.326009;
                        }

                        p5[i].Nombre = listaresultadoscorrientes[i, setnumber].Nombre;
                        listBox1.Items.Add(p5[i].ToString());
                        //listBox1.Items.Add("---");
                        //listBox1.Items.Add(listaresultadoscorrientes[i, setnumber].ToString());    
                    }

                    //Unidades Sistema Británico
                    else if (unidades == 0)
                    {
                        p5[i] = listaresultadoscorrientes[i, setnumber];
                        listBox1.Items.Add(p5[i].ToString());
                        //listBox1.Items.Add("---");
                        //listBox1.Items.Add(listaresultadoscorrientes[i, setnumber].ToString());                        
                    }
                }
            }

            p5.Clear();

            listBox1.Items.Add("");
        }

        //Opción del Menú Contextual "AutoSize" en Heat Balance Scheme
        private void toolStripMenuItem14_Click(object sender, EventArgs e)
        {
            pictureBox2.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox2.Refresh();

            //pictureBox3.SizeMode = PictureBoxSizeMode.AutoSize;
            //pictureBox3.Refresh();
        }

        //Opción del Menú Contextual "Zoom" en Heat Balance Scheme
        private void toolStripMenuItem15_Click(object sender, EventArgs e)
        {
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.Refresh();

            //pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            //pictureBox3.Refresh();
        }

        //Opción del Menú Contextual "StrechImage" en Heat Balance Scheme
        private void toolStripMenuItem16_Click(object sender, EventArgs e)
        {
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.Refresh();

            //pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            //pictureBox3.Refresh();
        }

        //Visualizar los RESULTADOS de los Equipos
        public void visualizarResultadosDeLosEquiposToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Limpiamos el contenido del control Lista "listBox2" de la Aplicación Principal
            listBox2.Items.Clear();

            //Elegimos las pestañas 1 y 6 de los controles tab de la aplicación principal
            tabControl1.SelectedTab = tabPage2;
            tabControl2.SelectedTab = tabPage8;

//----------Visualizamos los equipo tipo Condición Contorno 1 guardados en la "ClassCondicionContorno1"-----------------------------------------------------------------------------------------

            for (int u = 0; u <numtipo1; u++)
            {
                    listBox2.Items.Add("Boundary Condition Equipment, Type 1." + "Equipment Number: " + matrixcondicioncontorno1[u,setnumber].numequipo);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Stream Nº: " + matrixcondicioncontorno1[u,setnumber].numcorrentrada);
                    listBox2.Items.Add("Output Stream Nº: " + matrixcondicioncontorno1[u,setnumber].numcorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Flow: " + matrixcondicioncontorno1[u,setnumber].caudalcorrentrada);
                    listBox2.Items.Add("Output Flow: " + matrixcondicioncontorno1[u,setnumber].caudalcorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Pressure: " + matrixcondicioncontorno1[u,setnumber].presioncorrentrada);
                    listBox2.Items.Add("Output Pressure: " + matrixcondicioncontorno1[u,setnumber].presioncorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Enthalphy: " + matrixcondicioncontorno1[u,setnumber].entalpiacorrentrada);
                    listBox2.Items.Add("Output Enthalpy: " + matrixcondicioncontorno1[u,setnumber].entalpiacorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Entropy: " + matrixcondicioncontorno1[u,setnumber].entropiaentrada);
                    listBox2.Items.Add("Output Entropy: " + matrixcondicioncontorno1[u,setnumber].entropiasalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Temperature: " + matrixcondicioncontorno1[u,setnumber].temperaturaentrada);
                    listBox2.Items.Add("Output Temperature: " + matrixcondicioncontorno1[u,setnumber].temperaturasalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Volumen Específico: " + matrixcondicioncontorno1[u,setnumber].volumenespecificoentrada);
                    listBox2.Items.Add("Output Volumen Específico: " + matrixcondicioncontorno1[u,setnumber].volumenespecificosalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Título: " + matrixcondicioncontorno1[u,setnumber].tituloentrada);
                    listBox2.Items.Add("Output Título: " + matrixcondicioncontorno1[u,setnumber].titulosalida);
                    listBox2.Items.Add("");
                    listBox2.Items.Add("");
                    listBox2.Items.Add("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
             }
            
//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


//----------Visualizamos los equipo tipo Divisor 2 guardados en la "ClassDivisor2"-----------------------------------------------------------------------------------------
            for (int u = 0; u < numtipo2; u++)
            {               
                    listBox2.Items.Add("Divider Equipment, Type 2." + "Equipment Number: " + matrixdivisor2[u,setnumber].numequipo);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Stream Nº: " + matrixdivisor2[u,setnumber].numcorrentrada);
                    listBox2.Items.Add("Output1 Stream Nº: " + matrixdivisor2[u,setnumber].numcorrsalida1);
                    listBox2.Items.Add("Output2 Stream Nº: " + matrixdivisor2[u,setnumber].numcorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Flow: " + matrixdivisor2[u,setnumber].caudalcorrentrada);
                    listBox2.Items.Add("Output1 Flow: " + matrixdivisor2[u,setnumber].caudalcorrsalida1);
                    listBox2.Items.Add("Output2 Flow: " + matrixdivisor2[u,setnumber].caudalcorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Pressure: " + matrixdivisor2[u,setnumber].presioncorrentrada);
                    listBox2.Items.Add("Output1 Pressure: " + matrixdivisor2[u,setnumber].presioncorrsalida1);
                    listBox2.Items.Add("Output2 Pressure: " + matrixdivisor2[u,setnumber].presioncorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Enthalphy: " + matrixdivisor2[u,setnumber].entalpiacorrentrada);
                    listBox2.Items.Add("Output1 Enthalpy: " + matrixdivisor2[u,setnumber].entalpiacorrsalida1);
                    listBox2.Items.Add("Output2 Enthalpy: " + matrixdivisor2[u,setnumber].entalpiacorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Entropy: " + matrixdivisor2[u,setnumber].entropiaentrada);
                    listBox2.Items.Add("Output1 Entropy: " + matrixdivisor2[u,setnumber].entropiasalida1);
                    listBox2.Items.Add("Output2 Entropy: " + matrixdivisor2[u,setnumber].entropiasalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Temperature: " + matrixdivisor2[u,setnumber].temperaturaentrada);
                    listBox2.Items.Add("Output1 Temperature: " + matrixdivisor2[u,setnumber].temperaturasalida1);
                    listBox2.Items.Add("Output2 Temperature: " + matrixdivisor2[u,setnumber].temperaturasalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Volumen Específico: " + matrixdivisor2[u,setnumber].volumenespecificoentrada);
                    listBox2.Items.Add("Output1 Volumen Epecífico: " + matrixdivisor2[u,setnumber].volumenespecificosalida1);
                    listBox2.Items.Add("Output2 Volumen Específico: " + matrixdivisor2[u,setnumber].volumenespecificosalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Título: " + matrixdivisor2[u,setnumber].tituloentrada);
                    listBox2.Items.Add("Output1 Título: " + matrixdivisor2[u,setnumber].titulosalida1);
                    listBox2.Items.Add("Output2 Título: " + matrixdivisor2[u,setnumber].titulosalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Flow Input/Output1 ratio: " + matrixdivisor2[u,setnumber].porcentajesalida1+"%");
                    listBox2.Items.Add("Flow Input/Output2 ratio: " + matrixdivisor2[u,setnumber].porcentajesalida2+"%");
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Flow Factor Output1: " + matrixdivisor2[u,setnumber].factorflujosalida1);
                    listBox2.Items.Add("Flow Factor Output2: " + matrixdivisor2[u,setnumber].factorflujosalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("");
                    listBox2.Items.Add("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                    listBox2.Items.Add("");
                    listBox2.Items.Add("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            }
//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
     

//----------Visualizamos los equipo tipo Pérdida de Carga 3 guardados en la "ClassPerdidaCarga3"-----------------------------------------------------------------------------------------
            for (int u = 0; u < numtipo3; u++)
            {              
                    listBox2.Items.Add("Pressure Loss Equipment, Type 3." + "Equipment Number: " +  matrixperdida3[u,setnumber].numequipo);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Stream Nº: " +  matrixperdida3[u,setnumber].numcorrentrada);
                    listBox2.Items.Add("Output Stream Nº: " +  matrixperdida3[u,setnumber].numcorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Flow: " +  matrixperdida3[u,setnumber].caudalcorrentrada);
                    listBox2.Items.Add("Output Flow: " +  matrixperdida3[u,setnumber].caudalcorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Pressure: " +  matrixperdida3[u,setnumber].presioncorrentrada);
                    listBox2.Items.Add("Output Pressure: " +  matrixperdida3[u,setnumber].presioncorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Enthalphy: " +  matrixperdida3[u,setnumber].entalpiacorrentrada);
                    listBox2.Items.Add("Output Enthalpy: " +  matrixperdida3[u,setnumber].entalpiacorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Entropy: " +  matrixperdida3[u,setnumber].entropiaentrada);
                    listBox2.Items.Add("Output Entropy: " +  matrixperdida3[u,setnumber].entropiasalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Temperature: " +  matrixperdida3[u,setnumber].temperaturaentrada);
                    listBox2.Items.Add("Output Temperature: " +  matrixperdida3[u,setnumber].temperaturasalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Volumen Específico: " +  matrixperdida3[u,setnumber].volumenespecificoentrada);
                    listBox2.Items.Add("Output Volumen Específico: " +  matrixperdida3[u,setnumber].volumenespecificosalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Título: " +  matrixperdida3[u,setnumber].tituloentrada);
                    listBox2.Items.Add("Output Título: " +  matrixperdida3[u,setnumber].titulosalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            }
//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------     


//----------Visualizamos los equipo tipo Bomba 4 guardados en la "ClassBomba4"-----------------------------------------------------------------------------------------
            for (int u = 0; u < numtipo4; u++)
            {
                    listBox2.Items.Add("Pump Equipment, Type 4." + "Equipment Number: " +   matrixbomba4[u,setnumber].numequipo);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Stream Nº: " +   matrixbomba4[u,setnumber].numcorrentrada);
                    listBox2.Items.Add("Output Stream Nº: " +   matrixbomba4[u,setnumber].numcorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Flow: " +   matrixbomba4[u,setnumber].caudalcorrentrada);
                    listBox2.Items.Add("Output Flow: " +   matrixbomba4[u,setnumber].caudalcorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Pressure: " +   matrixbomba4[u,setnumber].presioncorrentrada);
                    listBox2.Items.Add("Output Pressure: " +   matrixbomba4[u,setnumber].presioncorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Enthalphy: " +   matrixbomba4[u,setnumber].entalpiacorrentrada);
                    listBox2.Items.Add("Output Enthalpy: " +   matrixbomba4[u,setnumber].entalpiacorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Entropy: " +   matrixbomba4[u,setnumber].entropiaentrada);
                    listBox2.Items.Add("Output Entropy: " +   matrixbomba4[u,setnumber].entropiasalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Temperature: " +   matrixbomba4[u,setnumber].temperaturaentrada);
                    listBox2.Items.Add("Output Temperature: " +   matrixbomba4[u,setnumber].temperaturasalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Volumen Específico: " +   matrixbomba4[u,setnumber].volumenespecificoentrada);
                    listBox2.Items.Add("Output Volumen Epecífico: " +   matrixbomba4[u,setnumber].volumenespecificosalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Título: " +   matrixbomba4[u,setnumber].tituloentrada);
                    listBox2.Items.Add("Output Título: " +   matrixbomba4[u,setnumber].titulosalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Efficiency: " +   matrixbomba4[u,setnumber].eficiencia);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Power: " +   matrixbomba4[u,setnumber].potencia);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("");
                    listBox2.Items.Add("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");       
            }
//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


//----------Visualizamos los equipo tipo Mezclador 5 guardados en la "ClassMezclador5"-----------------------------------------------------------------------------------------
            for (int u = 0; u < numtipo5; u++)
            {
                    listBox2.Items.Add("Mixer Equipment, Type 5." + "Equipment Number: " +   matrixmezclador5[u,setnumber].numequipo);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Stream Nº: " +   matrixmezclador5[u,setnumber].numcorrentrada1);
                    listBox2.Items.Add("Input2 Stream Nº: " +   matrixmezclador5[u,setnumber].numcorrentrada2);
                    listBox2.Items.Add("Output Stream Nº: " +   matrixmezclador5[u,setnumber].numcorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Flow: " +   matrixmezclador5[u,setnumber].caudalcorrentrada1);
                    listBox2.Items.Add("Input2 Flow: " +   matrixmezclador5[u,setnumber].caudalcorrentrada2);
                    listBox2.Items.Add("Output Flow: " +   matrixmezclador5[u,setnumber].caudalcorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Pressure: " +   matrixmezclador5[u,setnumber].presioncorrentrada1);
                    listBox2.Items.Add("Input2 Pressure: " +   matrixmezclador5[u,setnumber].presioncorrentrada2);
                    listBox2.Items.Add("Output Pressure: " +   matrixmezclador5[u,setnumber].presioncorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Enthalphy: " +   matrixmezclador5[u,setnumber].entalpiacorrentrada1);
                    listBox2.Items.Add("Input2 Enthalphy: " +   matrixmezclador5[u,setnumber].entalpiacorrentrada2);
                    listBox2.Items.Add("Output Enthalpy: " +   matrixmezclador5[u,setnumber].entalpiacorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Entropy: " +   matrixmezclador5[u,setnumber].entropiaentrada1);
                    listBox2.Items.Add("Input2 Entropy: " +   matrixmezclador5[u,setnumber].entropiaentrada2);
                    listBox2.Items.Add("Output Entropy: " +   matrixmezclador5[u,setnumber].entropiasalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Temperature: " +   matrixmezclador5[u,setnumber].temperaturaentrada1);
                    listBox2.Items.Add("Input2 Temperature: " +   matrixmezclador5[u,setnumber].temperaturaentrada2);
                    listBox2.Items.Add("Output Temperature: " +   matrixmezclador5[u,setnumber].temperaturasalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Volumen Específico: " +   matrixmezclador5[u,setnumber].volumenespecificoentrada1);
                    listBox2.Items.Add("Input2 Volumen Específico: " +   matrixmezclador5[u,setnumber].volumenespecificoentrada2);
                    listBox2.Items.Add("Output Volumen Específico: " +   matrixmezclador5[u,setnumber].volumenespecificosalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Título: " +   matrixmezclador5[u,setnumber].tituloentrada1);
                    listBox2.Items.Add("Input2 Título: " +   matrixmezclador5[u,setnumber].tituloentrada2);
                    listBox2.Items.Add("Output Título: " +   matrixmezclador5[u,setnumber].titulosalida);

                    listBox2.Items.Add("");
                    listBox2.Items.Add("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            }
//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


//----------Visualizamos los equipo tipo Reactor 6 guardados en la "ClassReactor6"-----------------------------------------------------------------------------------------
            for (int u = 0; u < numtipo6; u++)
            {              
                    listBox2.Items.Add("Reactor Equipment, Type 6." + "Equipment Number: " + matrixreactor6[u,setnumber].numequipo);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Stream Nº: " + matrixreactor6[u,setnumber].numcorrentrada);
                    listBox2.Items.Add("Output Stream Nº: " + matrixreactor6[u,setnumber].numcorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Flow: " + matrixreactor6[u,setnumber].caudalcorrentrada);
                    listBox2.Items.Add("Output Flow: " + matrixreactor6[u,setnumber].caudalcorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Pressure: " + matrixreactor6[u,setnumber].presioncorrentrada);
                    listBox2.Items.Add("Output Pressure: " + matrixreactor6[u,setnumber].presioncorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Enthalphy: " + matrixreactor6[u,setnumber].entalpiacorrentrada);
                    listBox2.Items.Add("Output Enthalpy: " + matrixreactor6[u,setnumber].entalpiacorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Entropy: " + matrixreactor6[u,setnumber].entropiaentrada);
                    listBox2.Items.Add("Output Entropy: " + matrixreactor6[u,setnumber].entropiasalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Temperature: " + matrixreactor6[u,setnumber].temperaturaentrada);
                    listBox2.Items.Add("Output Temperature: " + matrixreactor6[u,setnumber].temperaturasalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Volumen Específico: " + matrixreactor6[u,setnumber].volumenespecificoentrada);
                    listBox2.Items.Add("Output Volumen Epecífico: " + matrixreactor6[u,setnumber].volumenespecificosalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Título: " + matrixreactor6[u,setnumber].tituloentrada);
                    listBox2.Items.Add("Output Título: " + matrixreactor6[u,setnumber].titulosalida);
                    listBox2.Items.Add("");
                    
                    listBox2.Items.Add("");
                    listBox2.Items.Add("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            }
//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            
           
//----------Visualizamos los equipo tipo CalentadorAguaAlimentación 7 guardados en la "ClassCalentador7"-----------------------------------------------------------------------------------------
            for (int u = 0; u < numtipo7; u++)
            {             
                    listBox2.Items.Add("FeedWater Heater Equipment, Type 7." + "Equipment Number: " +  matrixcalentador7[u,setnumber].numequipo);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Stream Nº: " +  matrixcalentador7[u,setnumber].numcorrentrada1);
                    listBox2.Items.Add("Input2 Stream Nº: " +  matrixcalentador7[u,setnumber].numcorrentrada2);
                    listBox2.Items.Add("Output1 Stream Nº: " +  matrixcalentador7[u,setnumber].numcorrsalida1);
                    listBox2.Items.Add("Output2 Stream Nº: " +  matrixcalentador7[u,setnumber].numcorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Flow: " +  matrixcalentador7[u,setnumber].caudalcorrentrada1);
                    listBox2.Items.Add("Input2 Flow: " +  matrixcalentador7[u,setnumber].caudalcorrentrada2);
                    listBox2.Items.Add("Output1 Flow: " +  matrixcalentador7[u,setnumber].caudalcorrsalida1);
                    listBox2.Items.Add("Output2 Flow: " +  matrixcalentador7[u,setnumber].caudalcorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Pressure: " +  matrixcalentador7[u,setnumber].presioncorrentrada1);
                    listBox2.Items.Add("Input2 Pressure: " +  matrixcalentador7[u,setnumber].presioncorrentrada2);
                    listBox2.Items.Add("Output1 Pressure: " +  matrixcalentador7[u,setnumber].presioncorrsalida1);
                    listBox2.Items.Add("Output2 Pressure: " +  matrixcalentador7[u,setnumber].presioncorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Enthalphy: " +  matrixcalentador7[u,setnumber].entalpiacorrentrada1);
                    listBox2.Items.Add("Input2 Enthalphy: " +  matrixcalentador7[u,setnumber].entalpiacorrentrada2);
                    listBox2.Items.Add("Output1 Enthalpy: " +  matrixcalentador7[u,setnumber].entalpiacorrsalida1);
                    listBox2.Items.Add("Output2 Enthalpy: " +  matrixcalentador7[u,setnumber].entalpiacorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Entropy: " +  matrixcalentador7[u,setnumber].entropiaentrada1);
                    listBox2.Items.Add("Input2 Entropy: " +  matrixcalentador7[u,setnumber].entropiaentrada2);
                    listBox2.Items.Add("Output1 Entropy: " +  matrixcalentador7[u,setnumber].entropiasalida1);
                    listBox2.Items.Add("Output2 Entropy: " +  matrixcalentador7[u,setnumber].entropiasalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Temperature: " +  matrixcalentador7[u,setnumber].temperaturaentrada1);
                    listBox2.Items.Add("Input2 Temperature: " +  matrixcalentador7[u,setnumber].temperaturaentrada2);
                    listBox2.Items.Add("Output1 Temperature: " +  matrixcalentador7[u,setnumber].temperaturasalida1);
                    listBox2.Items.Add("Output2 Temperature: " +  matrixcalentador7[u,setnumber].temperaturasalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("TTD (Terminal Temperature Difference): " +  matrixcalentador7[u,setnumber].TTD);
                    listBox2.Items.Add("DCA (Drain Colling Approach): " +  matrixcalentador7[u,setnumber].DCA);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            }
//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


//----------Visualizamos los equipo tipo Condensador 8 guardados en la "ClassCondensador8"-----------------------------------------------------------------------------------------
            for (int u = 0; u < numtipo8; u++)
            {              
                    listBox2.Items.Add("Condenser Equipment, Type 8." + "Equipment Number: " +  matrixcondensador8[u,setnumber].numequipo);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Stream Nº: " +  matrixcondensador8[u,setnumber].numcorrentrada1);
                    listBox2.Items.Add("Input2 Stream Nº: " +  matrixcondensador8[u,setnumber].numcorrentrada2);
                    listBox2.Items.Add("Output Stream Nº: " +  matrixcondensador8[u,setnumber].numcorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Flow: " +  matrixcondensador8[u,setnumber].caudalcorrentrada1);
                    listBox2.Items.Add("Input2 Flow: " +  matrixcondensador8[u,setnumber].caudalcorrentrada2);
                    listBox2.Items.Add("Output Flow: " +  matrixcondensador8[u,setnumber].caudalcorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Pressure: " +  matrixcondensador8[u,setnumber].presioncorrentrada1);
                    listBox2.Items.Add("Input2 Pressure: " +  matrixcondensador8[u,setnumber].presioncorrentrada2);
                    listBox2.Items.Add("Output Pressure: " +  matrixcondensador8[u,setnumber].presioncorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Enthalphy: " +  matrixcondensador8[u,setnumber].entalpiacorrentrada1);
                    listBox2.Items.Add("Input2 Enthalphy: " +  matrixcondensador8[u,setnumber].entalpiacorrentrada2);
                    listBox2.Items.Add("Output Enthalpy: " +  matrixcondensador8[u,setnumber].entalpiacorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Entropy: " +  matrixcondensador8[u,setnumber].entropiaentrada1);
                    listBox2.Items.Add("Input2 Entropy: " +  matrixcondensador8[u,setnumber].entropiaentrada2);
                    listBox2.Items.Add("Output Entropy: " +  matrixcondensador8[u,setnumber].entropiasalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Temperature: " +  matrixcondensador8[u,setnumber].temperaturaentrada1);
                    listBox2.Items.Add("Input2 Temperature: " +  matrixcondensador8[u,setnumber].temperaturaentrada2);
                    listBox2.Items.Add("Output Temperature: " +  matrixcondensador8[u,setnumber].temperaturasalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Título: " +  matrixcondensador8[u,setnumber].tituloentrada1);
                    listBox2.Items.Add("Input2 Título: " +  matrixcondensador8[u,setnumber].tituloentrada2);
                    listBox2.Items.Add("Output Título: " +  matrixcondensador8[u,setnumber].titulosalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Volumen Específico: " +  matrixcondensador8[u,setnumber].volumenespecificoentrada1);
                    listBox2.Items.Add("Input2 Volumen Específico: " +  matrixcondensador8[u,setnumber].volumenespecificoentrada2);
                    listBox2.Items.Add("Output Volumen Específico: " +  matrixcondensador8[u,setnumber].volumenespecificosalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("");
                    listBox2.Items.Add("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            }
//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        
//----------Visualizamos los equipo tipo Turbina 9 guardados en la "ClassTurbina9"-----------------------------------------------------------------------------------------
            for (int u = 0; u < numtipo9; u++)
            {               
                    listBox2.Items.Add("Turbine Equipment, Type 9." + "Equipment Number: " +  matrixturbina9[u,setnumber].numequipo);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Stream Nº: " +  matrixturbina9[u,setnumber].numcorrentrada);
                    listBox2.Items.Add("Output Stream Nº: " +  matrixturbina9[u,setnumber].numcorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Flow: " +  matrixturbina9[u,setnumber].caudalcorrentrada);
                    listBox2.Items.Add("Output Flow: " +  matrixturbina9[u,setnumber].caudalcorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Pressure: " +  matrixturbina9[u,setnumber].presioncorrentrada);
                    listBox2.Items.Add("Output Pressure: " +  matrixturbina9[u,setnumber].presioncorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Enthalphy: " +  matrixturbina9[u,setnumber].entalpiacorrentrada);
                    listBox2.Items.Add("Output Enthalpy: " +  matrixturbina9[u,setnumber].entalpiacorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Entropy: " +  matrixturbina9[u,setnumber].entropiaentrada);
                    listBox2.Items.Add("Output Entropy: " +  matrixturbina9[u,setnumber].entropiasalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Temperature: " +  matrixturbina9[u,setnumber].temperaturaentrada);
                    listBox2.Items.Add("Output Temperature: " +  matrixturbina9[u,setnumber].temperaturasalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Título: " +  matrixturbina9[u,setnumber].tituloentrada);
                    listBox2.Items.Add("Output Título: " +  matrixturbina9[u,setnumber].titulosalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Volumen Específico: " +  matrixturbina9[u,setnumber].volumenespecificoentrada);
                    listBox2.Items.Add("Output Volumen Específico: " +  matrixturbina9[u,setnumber].volumenespecificosalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Pressures Relation: " +  matrixturbina9[u,setnumber].relacionpresiones);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Flow Factor: " +  matrixturbina9[u,setnumber].factorflujo);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Efficiency: " +  matrixturbina9[u,setnumber].eficiencia);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Power Generation: " +  matrixturbina9[u,setnumber].potencia);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("");
                    listBox2.Items.Add("");
                    listBox2.Items.Add("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            }
//--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


//----------Visualizamos los equipo tipo Turbina 10 guardados en la "ClassTurbina10"-----------------------------------------------------------------------------------------
            for (int u = 0; u < numtipo10; u++)
            {              
                    listBox2.Items.Add("Turbine Equipment, Type 10." + "Equipment Number: " +  matrixturbina10[u,setnumber].numequipo);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Stream Nº: " +  matrixturbina10[u,setnumber].numcorrentrada);
                    listBox2.Items.Add("Output Stream Nº: " +  matrixturbina10[u,setnumber].numcorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Flow: " +  matrixturbina10[u,setnumber].caudalcorrentrada);
                    listBox2.Items.Add("Output Flow: " +  matrixturbina10[u,setnumber].caudalcorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Pressure: " +  matrixturbina10[u,setnumber].presioncorrentrada);
                    listBox2.Items.Add("Output Pressure: " +  matrixturbina10[u,setnumber].presioncorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Enthalphy: " +  matrixturbina10[u,setnumber].entalpiacorrentrada);
                    listBox2.Items.Add("Output Enthalpy: " +  matrixturbina10[u,setnumber].entalpiacorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Entropy: " +  matrixturbina10[u,setnumber].entropiaentrada);
                    listBox2.Items.Add("Output Entropy: " +  matrixturbina10[u,setnumber].entropiasalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Temperature: " +  matrixturbina10[u,setnumber].temperaturaentrada);
                    listBox2.Items.Add("Output Temperature: " +  matrixturbina10[u,setnumber].temperaturasalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Título: " +  matrixturbina10[u,setnumber].tituloentrada);
                    listBox2.Items.Add("Output Título: " +  matrixturbina10[u,setnumber].titulosalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Volumen Específico: " +  matrixturbina10[u,setnumber].volumenespecificoentrada);
                    listBox2.Items.Add("Output Volumen Específico: " +  matrixturbina10[u,setnumber].volumenespecificosalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Pressures Relation: " +  matrixturbina10[u,setnumber].relacionpresiones);

                    listBox2.Items.Add("Flow Factor: " +  matrixturbina10[u,setnumber].factorflujo);

                    //listBox2.Items.Add("Power Generation: " +  matrixturbina10[u,setnumber].relacionpresiones);

                    listBox2.Items.Add("");
                    listBox2.Items.Add("");
                    listBox2.Items.Add("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            }
//--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


//----------Visualizamos los equipo tipo Turbina 11 guardados en la "ClassTurbina11"-----------------------------------------------------------------------------------------
            for (int u = 0; u < numtipo11; u++)
            {               
                    listBox2.Items.Add("Auxiliary Turbine Equipment, Type 11." + "Equipment Number: " +  matrixturbina11[u,setnumber].numequipo);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Stream Nº: " +  matrixturbina11[u,setnumber].numcorrentrada);
                    listBox2.Items.Add("Output Stream Nº: " +  matrixturbina11[u,setnumber].numcorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Flow: " +  matrixturbina11[u,setnumber].caudalcorrentrada);
                    listBox2.Items.Add("Output Flow: " +  matrixturbina11[u,setnumber].caudalcorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Pressure: " +  matrixturbina11[u,setnumber].presioncorrentrada);
                    listBox2.Items.Add("Output Pressure: " +  matrixturbina11[u,setnumber].presioncorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Enthalphy: " +  matrixturbina11[u,setnumber].entalpiacorrentrada);
                    listBox2.Items.Add("Output Enthalpy: " +  matrixturbina11[u,setnumber].entalpiacorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Entropy: " +  matrixturbina11[u,setnumber].entropiaentrada);
                    listBox2.Items.Add("Output Entropy: " +  matrixturbina11[u,setnumber].entropiasalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Temperature: " +  matrixturbina11[u,setnumber].temperaturaentrada);
                    listBox2.Items.Add("Output Temperature: " +  matrixturbina11[u,setnumber].temperaturasalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Título: " +  matrixturbina11[u,setnumber].tituloentrada);
                    listBox2.Items.Add("Output Título: " +  matrixturbina11[u,setnumber].titulosalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Volumen Específico: " +  matrixturbina11[u,setnumber].volumenespecificoentrada);
                    listBox2.Items.Add("Output Volumen Específico: " +  matrixturbina11[u,setnumber].volumenespecificosalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Pressures Relation: " +  matrixturbina11[u,setnumber].relacionpresiones);

                    listBox2.Items.Add("Flow Factor: " +  matrixturbina11[u,setnumber].relacionpresiones);

                    listBox2.Items.Add("Power Generation: " +  matrixturbina11[u,setnumber].relacionpresiones);

                    listBox2.Items.Add("");
                    listBox2.Items.Add("");
                    listBox2.Items.Add("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            }
//--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

            
//----------Visualizamos los equipo tipo Turbina 13 guardados en la "ClassSeparadorHumedad13"-----------------------------------------------------------------------------------------
            for (int u = 0; u < numtipo13; u++)
            {              
                    listBox2.Items.Add("Moisture Separator Equipment, Type 13." + "Equipment Number: " + matrixseparador13[u,setnumber].numequipo);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Stream Nº: " + matrixseparador13[u,setnumber].numcorrentrada);
                    listBox2.Items.Add("Output1 Stream Nº: " + matrixseparador13[u,setnumber].numcorrsalida1);
                    listBox2.Items.Add("Output2 Stream Nº: " + matrixseparador13[u,setnumber].numcorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Flow: " + matrixseparador13[u,setnumber].caudalcorrentrada);
                    listBox2.Items.Add("Output1 Flow: " + matrixseparador13[u,setnumber].caudalcorrsalida1);
                    listBox2.Items.Add("Output2 Flow: " + matrixseparador13[u,setnumber].caudalcorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Pressure: " + matrixseparador13[u,setnumber].presioncorrentrada);
                    listBox2.Items.Add("Output1 Pressure: " + matrixseparador13[u,setnumber].presioncorrsalida1);
                    listBox2.Items.Add("Output2 Pressure: " + matrixseparador13[u,setnumber].presioncorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Enthalphy: " + matrixseparador13[u,setnumber].entalpiacorrentrada);
                    listBox2.Items.Add("Output1 Enthalpy: " + matrixseparador13[u,setnumber].entalpiacorrsalida1);
                    listBox2.Items.Add("Output2 Enthalpy: " + matrixseparador13[u,setnumber].entalpiacorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Entropy: " + matrixseparador13[u,setnumber].entropiaentrada);
                    listBox2.Items.Add("Output1 Entropy: " + matrixseparador13[u,setnumber].entropiasalida1);
                    listBox2.Items.Add("Output2 Entropy: " + matrixseparador13[u,setnumber].entropiasalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Temperature: " + matrixseparador13[u,setnumber].temperaturaentrada);
                    listBox2.Items.Add("Output1 Temperature: " + matrixseparador13[u,setnumber].temperaturasalida1);
                    listBox2.Items.Add("Output2 Temperature: " + matrixseparador13[u,setnumber].temperaturasalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Volumen Específico: " + matrixseparador13[u,setnumber].volumenespecificoentrada);
                    listBox2.Items.Add("Output1 Volumen Específico: " + matrixseparador13[u,setnumber].volumenespecificosalida1);
                    listBox2.Items.Add("Output2 Volumen Específico: " + matrixseparador13[u,setnumber].volumenespecificosalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Título: " + matrixseparador13[u,setnumber].tituloentrada);
                    listBox2.Items.Add("Output1 Título: " + matrixseparador13[u,setnumber].titulosalida1);
                    listBox2.Items.Add("Output2 Título: " + matrixseparador13[u,setnumber].titulosalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("");
                    listBox2.Items.Add("");
                    listBox2.Items.Add("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            }
//--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------     
                    
            
//----------Visualizamos los equipo tipo MSR 14 guardados en la "ClassMSR14"-----------------------------------------------------------------------------------------
            for (int u = 0; u < numtipo14; u++)
            {            
                    listBox2.Items.Add("MSR Moisture Steam Reheater, Type 14." + "Equipment Number: " + matrixMSR14[u,setnumber].numequipo);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Stream Nº: " + matrixMSR14[u,setnumber].numcorrentrada1);
                    listBox2.Items.Add("Input2 Stream Nº: " + matrixMSR14[u,setnumber].numcorrentrada2);
                    listBox2.Items.Add("Output1 Stream Nº: " + matrixMSR14[u,setnumber].numcorrsalida1);
                    listBox2.Items.Add("Output2 Stream Nº: " + matrixMSR14[u,setnumber].numcorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Flow: " + matrixMSR14[u,setnumber].caudalcorrentrada1);
                    listBox2.Items.Add("Input2 Flow: " + matrixMSR14[u,setnumber].caudalcorrentrada2);
                    listBox2.Items.Add("Output1 Flow: " + matrixMSR14[u,setnumber].caudalcorrsalida1);
                    listBox2.Items.Add("Output2 Flow: " + matrixMSR14[u,setnumber].caudalcorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Pressure: " + matrixMSR14[u,setnumber].presioncorrentrada1);
                    listBox2.Items.Add("Input2 Pressure: " + matrixMSR14[u,setnumber].presioncorrentrada2);
                    listBox2.Items.Add("Output1 Pressure: " + matrixMSR14[u,setnumber].presioncorrsalida1);
                    listBox2.Items.Add("Output2 Pressure: " + matrixMSR14[u,setnumber].presioncorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Enthalphy: " + matrixMSR14[u,setnumber].entalpiacorrentrada1);
                    listBox2.Items.Add("Input2 Enthalphy: " + matrixMSR14[u,setnumber].entalpiacorrentrada2);
                    listBox2.Items.Add("Output1 Enthalpy: " + matrixMSR14[u,setnumber].entalpiacorrsalida1);
                    listBox2.Items.Add("Output2 Enthalpy: " + matrixMSR14[u,setnumber].entalpiacorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Entropy: " + matrixMSR14[u,setnumber].entropiaentrada1);
                    listBox2.Items.Add("Input2 Entropy: " + matrixMSR14[u,setnumber].entropiaentrada2);
                    listBox2.Items.Add("Output1 Entropy: " + matrixMSR14[u,setnumber].entropiasalida1);
                    listBox2.Items.Add("Output2 Entropy: " + matrixMSR14[u,setnumber].entropiasalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Temperature: " + matrixMSR14[u,setnumber].temperaturaentrada1);
                    listBox2.Items.Add("Input2 Temperature: " + matrixMSR14[u,setnumber].temperaturaentrada2);
                    listBox2.Items.Add("Output1 Temperature: " + matrixMSR14[u,setnumber].temperaturasalida1);
                    listBox2.Items.Add("Output2 Temperature: " + matrixMSR14[u,setnumber].temperaturasalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Volumen Específico: " + matrixMSR14[u,setnumber].volumenespecificoentrada1);
                    listBox2.Items.Add("Input2 Volumen Específico: " + matrixMSR14[u,setnumber].volumenespecificoentrada2);
                    listBox2.Items.Add("Output1 Volumen Específico: " + matrixMSR14[u,setnumber].volumenespecificosalida1);
                    listBox2.Items.Add("Output2 Volumen Específico: " + matrixMSR14[u,setnumber].volumenespecificosalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Título: " + matrixMSR14[u,setnumber].tituloentrada1);
                    listBox2.Items.Add("Input2 Título: " + matrixMSR14[u,setnumber].tituloentrada2);
                    listBox2.Items.Add("Output1 Título: " + matrixMSR14[u,setnumber].titulosalida1);
                    listBox2.Items.Add("Output2 Título: " + matrixMSR14[u,setnumber].titulosalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("TTD (Terminal Temperature Difference): " + matrixMSR14[u,setnumber].TTD);
                    listBox2.Items.Add("");
                    listBox2.Items.Add("");
                    listBox2.Items.Add("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            }
//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

           
//----------Visualizamos los equipo tipo Condensador 15 guardados en la "ClassCondensador15"-----------------------------------------------------------------------------------------
            for (int u = 0; u < numtipo15; u++)
            {                
                    listBox2.Items.Add("Gland Seal, Ejectors and Off-Gas Condensers, Type 15." + "Equipment Number: " + matrixcondensador15[u,setnumber].numequipo);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Stream Nº: " + matrixcondensador15[u,setnumber].numcorrentrada1);
                    listBox2.Items.Add("Input2 Stream Nº: " + matrixcondensador15[u,setnumber].numcorrentrada2);
                    listBox2.Items.Add("Output1 Stream Nº: " + matrixcondensador15[u,setnumber].numcorrsalida1);
                    listBox2.Items.Add("Output2 Stream Nº: " + matrixcondensador15[u,setnumber].numcorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Flow: " + matrixcondensador15[u,setnumber].caudalcorrentrada1);
                    listBox2.Items.Add("Input2 Flow: " + matrixcondensador15[u,setnumber].caudalcorrentrada2);
                    listBox2.Items.Add("Output1 Flow: " + matrixcondensador15[u,setnumber].caudalcorrsalida1);
                    listBox2.Items.Add("Output2 Flow: " + matrixcondensador15[u,setnumber].caudalcorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Pressure: " + matrixcondensador15[u,setnumber].presioncorrentrada1);
                    listBox2.Items.Add("Input2 Pressure: " + matrixcondensador15[u,setnumber].presioncorrentrada2);
                    listBox2.Items.Add("Output1 Pressure: " + matrixcondensador15[u,setnumber].presioncorrsalida1);
                    listBox2.Items.Add("Output2 Pressure: " + matrixcondensador15[u,setnumber].presioncorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Enthalphy: " + matrixcondensador15[u,setnumber].entalpiacorrentrada1);
                    listBox2.Items.Add("Input2 Enthalphy: " + matrixcondensador15[u,setnumber].entalpiacorrentrada2);
                    listBox2.Items.Add("Output1 Enthalpy: " + matrixcondensador15[u,setnumber].entalpiacorrsalida1);
                    listBox2.Items.Add("Output2 Enthalpy: " + matrixcondensador15[u,setnumber].entalpiacorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Entropy: " + matrixcondensador15[u,setnumber].entropiaentrada1);
                    listBox2.Items.Add("Input2 Entropy: " + matrixcondensador15[u,setnumber].entropiaentrada2);
                    listBox2.Items.Add("Output1 Entropy: " + matrixcondensador15[u,setnumber].entropiasalida1);
                    listBox2.Items.Add("Output2 Entropy: " + matrixcondensador15[u,setnumber].entropiasalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Temperature: " + matrixcondensador15[u,setnumber].temperaturaentrada1);
                    listBox2.Items.Add("Input2 Temperature: " + matrixcondensador15[u,setnumber].temperaturaentrada2);
                    listBox2.Items.Add("Output1 Temperature: " + matrixcondensador15[u,setnumber].temperaturasalida1);
                    listBox2.Items.Add("Output2 Temperature: " + matrixcondensador15[u,setnumber].temperaturasalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Volumen Específico: " + matrixcondensador15[u,setnumber].volumenespecificoentrada1);
                    listBox2.Items.Add("Input2 Volumen Específico: " + matrixcondensador15[u,setnumber].volumenespecificoentrada2);
                    listBox2.Items.Add("Output1 Volumen Específico: " + matrixcondensador15[u,setnumber].volumenespecificosalida1);
                    listBox2.Items.Add("Output2 Volumen Específico: " + matrixcondensador15[u,setnumber].volumenespecificosalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Título: " + matrixcondensador15[u,setnumber].tituloentrada1);
                    listBox2.Items.Add("Input2 Título: " + matrixcondensador15[u,setnumber].tituloentrada2);
                    listBox2.Items.Add("Output1 Título: " + matrixcondensador15[u,setnumber].titulosalida1);
                    listBox2.Items.Add("Output2 Título: " + matrixcondensador15[u,setnumber].titulosalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("");
                    listBox2.Items.Add("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            }
//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

            
//----------Visualizamos los equipo tipo Enfriador Drenajes 16 guardados en la "ClassEnfriadorDrenajes16"-----------------------------------------------------------------------------------------
            for (int u = 0; u < numtipo16; u++)
            {
                    listBox2.Items.Add("Drainage Cooling Equipment, Type 16." + "Equipment Number: " + matrixenfriador16[u,setnumber].numequipo);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Stream Nº: " + matrixenfriador16[u,setnumber].numcorrentrada1);
                    listBox2.Items.Add("Input2 Stream Nº: " + matrixenfriador16[u,setnumber].numcorrentrada2);
                    listBox2.Items.Add("Output1 Stream Nº: " + matrixenfriador16[u,setnumber].numcorrsalida1);
                    listBox2.Items.Add("Output2 Stream Nº: " + matrixenfriador16[u,setnumber].numcorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Flow: " + matrixenfriador16[u,setnumber].caudalcorrentrada1);
                    listBox2.Items.Add("Input2 Flow: " + matrixenfriador16[u,setnumber].caudalcorrentrada2);
                    listBox2.Items.Add("Output1 Flow: " + matrixenfriador16[u,setnumber].caudalcorrsalida1);
                    listBox2.Items.Add("Output2 Flow: " + matrixenfriador16[u,setnumber].caudalcorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Pressure: " + matrixenfriador16[u,setnumber].presioncorrentrada1);
                    listBox2.Items.Add("Input2 Pressure: " + matrixenfriador16[u,setnumber].presioncorrentrada2);
                    listBox2.Items.Add("Output1 Pressure: " + matrixenfriador16[u,setnumber].presioncorrsalida1);
                    listBox2.Items.Add("Output2 Pressure: " + matrixenfriador16[u,setnumber].presioncorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Enthalphy: " + matrixenfriador16[u,setnumber].entalpiacorrentrada1);
                    listBox2.Items.Add("Input2 Enthalphy: " + matrixenfriador16[u,setnumber].entalpiacorrentrada2);
                    listBox2.Items.Add("Output1 Enthalpy: " + matrixenfriador16[u,setnumber].entalpiacorrsalida1);
                    listBox2.Items.Add("Output2 Enthalpy: " + matrixenfriador16[u,setnumber].entalpiacorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Entropy: " + matrixenfriador16[u,setnumber].entropiaentrada1);
                    listBox2.Items.Add("Input2 Entropy: " + matrixenfriador16[u,setnumber].entropiaentrada2);
                    listBox2.Items.Add("Output1 Entropy: " + matrixenfriador16[u,setnumber].entropiasalida1);
                    listBox2.Items.Add("Output2 Entropy: " + matrixenfriador16[u,setnumber].entropiasalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Temperature: " + matrixenfriador16[u,setnumber].temperaturaentrada1);
                    listBox2.Items.Add("Input2 Temperature: " + matrixenfriador16[u,setnumber].temperaturaentrada2);
                    listBox2.Items.Add("Output1 Temperature: " + matrixenfriador16[u,setnumber].temperaturasalida1);
                    listBox2.Items.Add("Output2 Temperature: " + matrixenfriador16[u,setnumber].temperaturasalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("TTD (Terminal Temperature Difference): " + matrixenfriador16[u,setnumber].TTD);
                    listBox2.Items.Add("DCA (Drain Colling Approach): " + matrixenfriador16[u,setnumber].DCA);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Volumen Específico: " + matrixenfriador16[u,setnumber].volumenespecificoentrada1);
                    listBox2.Items.Add("Input2 Volumen Específico: " + matrixenfriador16[u,setnumber].volumenespecificoentrada2);
                    listBox2.Items.Add("Output1 Volumen Específico: " + matrixenfriador16[u,setnumber].volumenespecificosalida1);
                    listBox2.Items.Add("Output2 Volumen Específico: " + matrixenfriador16[u,setnumber].volumenespecificosalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Título: " + matrixenfriador16[u,setnumber].tituloentrada1);
                    listBox2.Items.Add("Input2 Título: " + matrixenfriador16[u,setnumber].tituloentrada2);
                    listBox2.Items.Add("Output1 Título: " + matrixenfriador16[u,setnumber].titulosalida1);
                    listBox2.Items.Add("Output2 Título: " + matrixenfriador16[u,setnumber].titulosalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("");
                    listBox2.Items.Add("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            }
//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


//----------Visualizamos los equipo tipo Atemperador 17 guardados en la "ClassAtemperador17"-----------------------------------------------------------------------------------------
            for (int u = 0; u < numtipo17; u++)
            {               
                    listBox2.Items.Add("Mixer Equipment, Type 5." + "Equipment Number: " + matrixatemperador17[u,setnumber].numequipo);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Stream Nº: " + matrixatemperador17[u,setnumber].numcorrentrada1);
                    listBox2.Items.Add("Input2 Stream Nº: " + matrixatemperador17[u,setnumber].numcorrentrada2);
                    listBox2.Items.Add("Output Stream Nº: " + matrixatemperador17[u,setnumber].numcorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Flow: " + matrixatemperador17[u,setnumber].caudalcorrentrada1);
                    listBox2.Items.Add("Input2 Flow: " + matrixatemperador17[u,setnumber].caudalcorrentrada2);
                    listBox2.Items.Add("Output Flow: " + matrixatemperador17[u,setnumber].caudalcorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Pressure: " + matrixatemperador17[u,setnumber].presioncorrentrada1);
                    listBox2.Items.Add("Input2 Pressure: " + matrixatemperador17[u,setnumber].presioncorrentrada2);
                    listBox2.Items.Add("Output Pressure: " + matrixatemperador17[u,setnumber].presioncorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Enthalphy: " + matrixatemperador17[u,setnumber].entalpiacorrentrada1);
                    listBox2.Items.Add("Input2 Enthalphy: " + matrixatemperador17[u,setnumber].entalpiacorrentrada2);
                    listBox2.Items.Add("Output Enthalpy: " + matrixatemperador17[u,setnumber].entalpiacorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Entropy: " + matrixatemperador17[u,setnumber].entropiaentrada1);
                    listBox2.Items.Add("Input2 Entropy: " + matrixatemperador17[u,setnumber].entropiaentrada2);
                    listBox2.Items.Add("Output Entropy: " + matrixatemperador17[u,setnumber].entropiasalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Temperature: " + matrixatemperador17[u,setnumber].temperaturaentrada1);
                    listBox2.Items.Add("Input2 Temperature: " + matrixatemperador17[u,setnumber].temperaturaentrada2);
                    listBox2.Items.Add("Output Temperature: " + matrixatemperador17[u,setnumber].temperaturasalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Volumen Específico: " + matrixatemperador17[u,setnumber].volumenespecificoentrada1);
                    listBox2.Items.Add("Input2 Volumen Específico: " + matrixatemperador17[u,setnumber].volumenespecificoentrada2);
                    listBox2.Items.Add("Output Volumen Específico: " + matrixatemperador17[u,setnumber].volumenespecificosalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Título: " + matrixatemperador17[u,setnumber].tituloentrada1);
                    listBox2.Items.Add("Input2 Título: " + matrixatemperador17[u,setnumber].tituloentrada2);
                    listBox2.Items.Add("Output Título: " + matrixatemperador17[u,setnumber].titulosalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("");
                    listBox2.Items.Add("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            }
//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        

//----------Visualizamos los equipo tipo Desaireador 18 guardados en la "ClassDesaireador"-----------------------------------------------------------------------------------------
            for (int u = 0; u < numtipo18; u++)
            {              
                    listBox2.Items.Add("Deaireator Equipment, Type 18." + "Equipment Number: " + matrixdesaireador18[u,setnumber].numequipo);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Stream Nº: " + matrixdesaireador18[u,setnumber].numcorrentrada1);
                    listBox2.Items.Add("Input2 Stream Nº: " + matrixdesaireador18[u,setnumber].numcorrentrada2);
                    listBox2.Items.Add("Output1 Stream Nº: " + matrixdesaireador18[u,setnumber].numcorrsalida1);
                    listBox2.Items.Add("Output2 Stream Nº: " + matrixdesaireador18[u,setnumber].numcorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Flow: " + matrixdesaireador18[u,setnumber].caudalcorrentrada1);
                    listBox2.Items.Add("Input2 Flow: " + matrixdesaireador18[u,setnumber].caudalcorrentrada2);
                    listBox2.Items.Add("Output1 Flow: " + matrixdesaireador18[u,setnumber].caudalcorrsalida1);
                    listBox2.Items.Add("Output2 Flow: " + matrixdesaireador18[u,setnumber].caudalcorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Pressure: " + matrixdesaireador18[u,setnumber].presioncorrentrada1);
                    listBox2.Items.Add("Input2 Pressure: " + matrixdesaireador18[u,setnumber].presioncorrentrada2);
                    listBox2.Items.Add("Output1 Pressure: " + matrixdesaireador18[u,setnumber].presioncorrsalida1);
                    listBox2.Items.Add("Output2 Pressure: " + matrixdesaireador18[u,setnumber].presioncorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Enthalphy: " + matrixdesaireador18[u,setnumber].entalpiacorrentrada1);
                    listBox2.Items.Add("Input2 Enthalphy: " + matrixdesaireador18[u,setnumber].entalpiacorrentrada2);
                    listBox2.Items.Add("Output1 Enthalpy: " + matrixdesaireador18[u,setnumber].entalpiacorrsalida1);
                    listBox2.Items.Add("Output2 Enthalpy: " + matrixdesaireador18[u,setnumber].entalpiacorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Entropy: " + matrixdesaireador18[u,setnumber].entropiaentrada1);
                    listBox2.Items.Add("Input2 Entropy: " + matrixdesaireador18[u,setnumber].entropiaentrada2);
                    listBox2.Items.Add("Output1 Entropy: " + matrixdesaireador18[u,setnumber].entropiasalida1);
                    listBox2.Items.Add("Output2 Entropy: " + matrixdesaireador18[u,setnumber].entropiasalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Temperature: " + matrixdesaireador18[u,setnumber].temperaturaentrada1);
                    listBox2.Items.Add("Input2 Temperature: " + matrixdesaireador18[u,setnumber].temperaturaentrada2);
                    listBox2.Items.Add("Output1 Temperature: " + matrixdesaireador18[u,setnumber].temperaturasalida1);
                    listBox2.Items.Add("Output2 Temperature: " + matrixdesaireador18[u,setnumber].temperaturasalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Título: " + matrixdesaireador18[u,setnumber].tituloentrada1);
                    listBox2.Items.Add("Input2 Título: " + matrixdesaireador18[u,setnumber].tituloentrada2);
                    listBox2.Items.Add("Output1 Título: " + matrixdesaireador18[u,setnumber].titulosalida1);
                    listBox2.Items.Add("Output2 Título: " + matrixdesaireador18[u,setnumber].titulosalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Volumen Específico: " + matrixdesaireador18[u,setnumber].volumenespecificoentrada1);
                    listBox2.Items.Add("Input2 Volumen Específico: " + matrixdesaireador18[u,setnumber].volumenespecificoentrada2);
                    listBox2.Items.Add("Output1 Volumen Específico: " + matrixdesaireador18[u,setnumber].volumenespecificosalida1);
                    listBox2.Items.Add("Output2 Volumen Específico: " + matrixdesaireador18[u,setnumber].volumenespecificosalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("TTD (Terminal Temperature Difference): " + matrixdesaireador18[u,setnumber].TTD);
                    listBox2.Items.Add("DCA (Drain Colling Approach): " + matrixdesaireador18[u,setnumber].DCA);
                    listBox2.Items.Add("");
                    listBox2.Items.Add("");
                    listBox2.Items.Add("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            }
//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
           

//----------Visualizamos los equipo tipo Válvula 19 guardados en la "ClassValvula"-----------------------------------------------------------------------------------------
            for (int u = 0; u < numtipo19; u++)
            {              
                    listBox2.Items.Add("Valve Equipment, Type 19." + "Equipment Number: " + matrixvalvula19[u,setnumber].numequipo);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Stream Nº: " + matrixvalvula19[u,setnumber].numcorrentrada);
                    listBox2.Items.Add("Output Stream Nº: " + matrixvalvula19[u,setnumber].numcorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Flow: " + matrixvalvula19[u,setnumber].caudalcorrentrada);
                    listBox2.Items.Add("Output Flow: " + matrixvalvula19[u,setnumber].caudalcorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Pressure: " + matrixvalvula19[u,setnumber].presioncorrentrada);
                    listBox2.Items.Add("Output Pressure: " + matrixvalvula19[u,setnumber].presioncorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Enthalphy: " + matrixvalvula19[u,setnumber].entalpiacorrentrada);
                    listBox2.Items.Add("Output Enthalpy: " + matrixvalvula19[u,setnumber].entalpiacorrsalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Entropy: " + matrixvalvula19[u,setnumber].entropiaentrada);
                    listBox2.Items.Add("Output Entropy: " + matrixvalvula19[u,setnumber].entropiasalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Temperature: " + matrixvalvula19[u,setnumber].temperaturaentrada);
                    listBox2.Items.Add("Output Temperature: " + matrixvalvula19[u,setnumber].temperaturasalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Specific Volumen: " + matrixvalvula19[u,setnumber].volumenespecificoentrada);
                    listBox2.Items.Add("Output Specific Volumen: " + matrixvalvula19[u,setnumber].volumenespecificosalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Título: " + matrixvalvula19[u,setnumber].tituloentrada);
                    listBox2.Items.Add("Output Título: " + matrixvalvula19[u,setnumber].titulosalida);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("");
                    listBox2.Items.Add("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            }
//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            

//----------Visualizamos los equipo tipo Divisor Entalpía Fija 20 guardados en la "ClassDivisorEntalpiaFija20"-----------------------------------------------------------------------------------------
            for (int u = 0; u <numtipo20; u++)
            {              
                    listBox2.Items.Add("Fixed Entalphy Divider, Type 20." + "Equipment Number: " + matrixdivisor20[u,setnumber].numequipo);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Stream Nº: " + matrixdivisor20[u,setnumber].numcorrentrada);
                    listBox2.Items.Add("Output1 Stream Nº: " + matrixdivisor20[u,setnumber].numcorrsalida1);
                    listBox2.Items.Add("Output2 Stream Nº: " + matrixdivisor20[u,setnumber].numcorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Flow: " + matrixdivisor20[u,setnumber].caudalcorrentrada);
                    listBox2.Items.Add("Output1 Flow: " + matrixdivisor20[u,setnumber].caudalcorrsalida1);
                    listBox2.Items.Add("Output2 Flow: " + matrixdivisor20[u,setnumber].caudalcorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Pressure: " + matrixdivisor20[u,setnumber].presioncorrentrada);
                    listBox2.Items.Add("Output1 Pressure: " + matrixdivisor20[u,setnumber].presioncorrsalida1);
                    listBox2.Items.Add("Output2 Pressure: " + matrixdivisor20[u,setnumber].presioncorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Enthalphy: " + matrixdivisor20[u,setnumber].entalpiacorrentrada);
                    listBox2.Items.Add("Output1 Enthalpy: " + matrixdivisor20[u,setnumber].entalpiacorrsalida1);
                    listBox2.Items.Add("Output2 Enthalpy: " + matrixdivisor20[u,setnumber].entalpiacorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Entropy: " + matrixdivisor20[u,setnumber].entropiaentrada);
                    listBox2.Items.Add("Output1 Entropy: " + matrixdivisor20[u,setnumber].entropiasalida1);
                    listBox2.Items.Add("Output2 Entropy: " + matrixdivisor20[u,setnumber].entropiasalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Temperature: " + matrixdivisor20[u,setnumber].temperaturaentrada);
                    listBox2.Items.Add("Output1 Temperature: " + matrixdivisor20[u,setnumber].temperaturasalida1);
                    listBox2.Items.Add("Output2 Temperature: " + matrixdivisor20[u,setnumber].temperaturasalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Specific Volumen: " + matrixdivisor20[u,setnumber].volumenespecificoentrada);
                    listBox2.Items.Add("Output1 Specific Volumen: " + matrixdivisor20[u,setnumber].volumenespecificosalida1);
                    listBox2.Items.Add("Output2 Specific Volumen: " + matrixdivisor20[u,setnumber].volumenespecificosalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Título: " + matrixdivisor20[u,setnumber].tituloentrada);
                    listBox2.Items.Add("Output1 Título: " + matrixdivisor20[u,setnumber].titulosalida1);
                    listBox2.Items.Add("Output2 Título: " + matrixdivisor20[u,setnumber].titulosalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("");
                    listBox2.Items.Add("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            }
//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
           

//----------Visualizamos los equipo tipo Tanque de Vaporización 21 guardados en la "ClassDivisorEntalpiaFija20"-----------------------------------------------------------------------------------------
            for (int u = 0; u <numtipo21; u++)
            {
                    listBox2.Items.Add("Flash Tank Equipment, Type 21." + "Equipment Number: " + matrixtanque21[u,setnumber].numequipo);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Stream Nº: " + matrixtanque21[u,setnumber].numcorrentrada);
                    listBox2.Items.Add("Output1 Stream Nº: " + matrixtanque21[u,setnumber].numcorrsalida1);
                    listBox2.Items.Add("Output2 Stream Nº: " + matrixtanque21[u,setnumber].numcorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Flow: " + matrixtanque21[u,setnumber].caudalcorrentrada);
                    listBox2.Items.Add("Output1 Flow: " + matrixtanque21[u,setnumber].caudalcorrsalida1);
                    listBox2.Items.Add("Output2 Flow: " + matrixtanque21[u,setnumber].caudalcorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Pressure: " + matrixtanque21[u,setnumber].presioncorrentrada);
                    listBox2.Items.Add("Output1 Pressure: " + matrixtanque21[u,setnumber].presioncorrsalida1);
                    listBox2.Items.Add("Output2 Pressure: " + matrixtanque21[u,setnumber].presioncorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Enthalphy: " + matrixtanque21[u,setnumber].entalpiacorrentrada);
                    listBox2.Items.Add("Output1 Enthalpy: " + matrixtanque21[u,setnumber].entalpiacorrsalida1);
                    listBox2.Items.Add("Output2 Enthalpy: " + matrixtanque21[u,setnumber].entalpiacorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Entropy: " + matrixtanque21[u,setnumber].entropiaentrada);
                    listBox2.Items.Add("Output1 Entropy: " + matrixtanque21[u,setnumber].entropiasalida1);
                    listBox2.Items.Add("Output2 Entropy: " + matrixtanque21[u,setnumber].entropiasalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Temperature: " + matrixtanque21[u,setnumber].temperaturaentrada);
                    listBox2.Items.Add("Output1 Temperature: " + matrixtanque21[u,setnumber].temperaturasalida1);
                    listBox2.Items.Add("Output2 Temperature: " + matrixtanque21[u,setnumber].temperaturasalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Specific Volumen: " + matrixtanque21[u,setnumber].volumenespecificoentrada);
                    listBox2.Items.Add("Output1 Specific Volumen: " + matrixtanque21[u,setnumber].volumenespecificosalida1);
                    listBox2.Items.Add("Output2 Specific Volumen: " + matrixtanque21[u,setnumber].volumenespecificosalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input Título: " + matrixtanque21[u,setnumber].tituloentrada);
                    listBox2.Items.Add("Output1 Título: " + matrixtanque21[u,setnumber].titulosalida1);
                    listBox2.Items.Add("Output2 Título: " + matrixtanque21[u,setnumber].titulosalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("");
                    listBox2.Items.Add("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            }
//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


//----------Visualizamos los equipo tipo Intercambiador 22 guardados en la "ClassIntercambiador22"-----------------------------------------------------------------------------------------
            for (int u = 0; u < numtipo22; u++)
            {               
                    listBox2.Items.Add("Heate Exchanger Equipment, Type 22." + "Equipment Number: " + matrixintercambiador22[u,setnumber].numequipo);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Stream Nº: " + matrixintercambiador22[u,setnumber].numcorrentrada1);
                    listBox2.Items.Add("Input2 Stream Nº: " + matrixintercambiador22[u,setnumber].numcorrentrada2);
                    listBox2.Items.Add("Output1 Stream Nº: " + matrixintercambiador22[u,setnumber].numcorrsalida1);
                    listBox2.Items.Add("Output2 Stream Nº: " + matrixintercambiador22[u,setnumber].numcorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Flow: " + matrixintercambiador22[u,setnumber].caudalcorrentrada1);
                    listBox2.Items.Add("Input2 Flow: " + matrixintercambiador22[u,setnumber].caudalcorrentrada2);
                    listBox2.Items.Add("Output1 Flow: " + matrixintercambiador22[u,setnumber].caudalcorrsalida1);
                    listBox2.Items.Add("Output2 Flow: " + matrixintercambiador22[u,setnumber].caudalcorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Pressure: " + matrixintercambiador22[u,setnumber].presioncorrentrada1);
                    listBox2.Items.Add("Input2 Pressure: " + matrixintercambiador22[u,setnumber].presioncorrentrada2);
                    listBox2.Items.Add("Output1 Pressure: " + matrixintercambiador22[u,setnumber].presioncorrsalida1);
                    listBox2.Items.Add("Output2 Pressure: " + matrixintercambiador22[u,setnumber].presioncorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Enthalphy: " + matrixintercambiador22[u,setnumber].entalpiacorrentrada1);
                    listBox2.Items.Add("Input2 Enthalphy: " + matrixintercambiador22[u,setnumber].entalpiacorrentrada2);
                    listBox2.Items.Add("Output1 Enthalpy: " + matrixintercambiador22[u,setnumber].entalpiacorrsalida1);
                    listBox2.Items.Add("Output2 Enthalpy: " + matrixintercambiador22[u,setnumber].entalpiacorrsalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Entropy: " + matrixintercambiador22[u,setnumber].entropiaentrada1);
                    listBox2.Items.Add("Input2 Entropy: " + matrixintercambiador22[u,setnumber].entropiaentrada2);
                    listBox2.Items.Add("Output1 Entropy: " + matrixintercambiador22[u,setnumber].entropiasalida1);
                    listBox2.Items.Add("Output2 Entropy: " + matrixintercambiador22[u,setnumber].entropiasalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Temperature: " + matrixintercambiador22[u,setnumber].temperaturaentrada1);
                    listBox2.Items.Add("Input2 Temperature: " + matrixintercambiador22[u,setnumber].temperaturaentrada2);
                    listBox2.Items.Add("Output1 Temperature: " + matrixintercambiador22[u,setnumber].temperaturasalida1);
                    listBox2.Items.Add("Output2 Temperature: " + matrixintercambiador22[u,setnumber].temperaturasalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Specific Volumen: " + matrixintercambiador22[u,setnumber].volumenespecificoentrada1);
                    listBox2.Items.Add("Input2 Specific Volumen: " + matrixintercambiador22[u,setnumber].volumenespecificoentrada2);
                    listBox2.Items.Add("Output1 Specific Volumen: " + matrixintercambiador22[u,setnumber].volumenespecificosalida1);
                    listBox2.Items.Add("Output2 Specific Volumen: " + matrixintercambiador22[u,setnumber].volumenespecificosalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("Input1 Título: " + matrixintercambiador22[u,setnumber].tituloentrada1);
                    listBox2.Items.Add("Input2 Título: " + matrixintercambiador22[u,setnumber].tituloentrada2);
                    listBox2.Items.Add("Output1 Título: " + matrixintercambiador22[u,setnumber].titulosalida1);
                    listBox2.Items.Add("Output2 Título: " + matrixintercambiador22[u,setnumber].titulosalida2);
                    listBox2.Items.Add("");

                    listBox2.Items.Add("TTD (Terminal Temperature Difference): " + matrixintercambiador22[u,setnumber].TTD);
                    listBox2.Items.Add("DCA (Drain Colling Approach): " + matrixintercambiador22[u,setnumber].DCA);
                    listBox2.Items.Add("");
                    listBox2.Items.Add("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            }
//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        }

        //SALIR de la Aplicación Principal
        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //Cuando pulsamos el Botón ENTER en el textBox1 actualizamos la anchura(Width) de las Columnas del Control dataGridView
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(System.ConsoleKey.Enter))
            {
                //Creamos las Columnas y una fila del Control DataGridView
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    dataGridView1.Columns[j].Width = Convert.ToInt16(textBox1.Text);
                }
            }
        }

        //Tamaño de las Columnas del Control dataGridView1 de la Aplicación principal
        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            //Creamos las Columnas y una fila del Control DataGridView
            for (int j = 0; j < dataGridView1.Columns.Count; j++)
            {
                if (textBox1.Text == "")
                {
                    textBox1.Text = "";
                    dataGridView1.Columns[j].Width = 25;
                }

                else if (Convert.ToInt16(textBox1.Text) <= 25)
                {
                    dataGridView1.Columns[j].Width = 25;
                }

                else
                {
                    dataGridView1.Columns[j].Width = Convert.ToInt16(textBox1.Text);
                }
            }
        }

        //Visualizar la Matriz Jacobiana Inversa
        public void visualizarJacobianaInversaxnmenos1()
        {
            if ((guardarintermedias4 == 1)&&(tipocalculo==0))
            {
                //Primero Inicializamos el Control dataGridView
                dataGridView4.Rows.Clear();
                dataGridView4.Columns.Clear();
               
                List<DataGridViewTextBoxColumn> listacolumnas = new List<DataGridViewTextBoxColumn>();

                int filas = jacobianinversoXnmenos1.GetLength(0);
                int columnas = jacobianinversoXnmenos1.GetLength(1);
                int numiteraciones = jacobianinversoXnmenos1.GetLength(2);

                int marca = 0;

                //Número Filas
                for (int i = 0; i < filas; i++)
                {
                    if (marca == 0)
                    {
                        //Creamos las Columnas y una fila del Control DataGridView
                        for (int j = 0; j < columnas; j++)
                        {
                            //Sólo para la primera fila es necesario añadir las Columnas al Control DataGrid. Después ya no hace falta.
                            if (marca == 0)
                            {
                                DataGridViewTextBoxColumn Columntemp = new DataGridViewTextBoxColumn();
                                Columntemp.HeaderText = p[j].Nombre.ToString();
                                Columntemp.Width = 30;
                                listacolumnas.Add(Columntemp);
                                dataGridView4.Columns.Add(listacolumnas[j]);
                            }
                        }

                        dataGridView4.Rows.Add();                       
                    }

                    else if (marca != 0)
                    {
                        dataGridView4.Rows.Add();
                       
                    }

                    int iteracion = 0;
                    iteracion = (int)numericUpDown1.Value;
                    //Número Columnas
                    for (int j = 0; j < columnas; j++)
                    {
                        dataGridView4.Rows[i].Cells[j].Value = jacobianinversoXnmenos1[i, j, iteracion];
                      
                        //A los elementos de la Diagonal Principal
                        if (i == j)
                        {
                            dataGridView4.Rows[i].Cells[j].Style.BackColor = Color.Yellow;
                           
                        }
                        else if (Convert.ToDouble(dataGridView4.Rows[i].Cells[j].Value) == 0)
                        {
                            dataGridView4.Rows[i].Cells[j].Style.BackColor = Color.Cyan;
                          
                        }

                        else if ((Convert.ToDouble(dataGridView4.Rows[i].Cells[j].Value) != 0) && (i != j))
                        {
                            dataGridView4.Rows[i].Cells[j].Style.BackColor = Color.White;                           
                        }
                    }

                    marca = 1;
                }

                //Primero Inicializamos el Control dataGridView
                dataGridView8.Rows.Clear();
                dataGridView8.Columns.Clear();

                List<DataGridViewTextBoxColumn> listacolumnas1 = new List<DataGridViewTextBoxColumn>();

                int filas1 = jacobianinversoXnmenos1.GetLength(0);
                int columnas1 = jacobianinversoXnmenos1.GetLength(1);
                int numiteraciones1 = jacobianinversoXnmenos1.GetLength(2);

                int marca1 = 0;

                //Número Filas
                for (int i = 0; i < filas1; i++)
                {
                    if (marca1 == 0)
                    {
                        //Creamos las Columnas y una fila del Control DataGridView
                        for (int j = 0; j < columnas1; j++)
                        {
                            //Sólo para la primera fila es necesario añadir las Columnas al Control DataGrid. Después ya no hace falta.
                            if (marca1 == 0)
                            {
                                DataGridViewTextBoxColumn Columntemp = new DataGridViewTextBoxColumn();
                                Columntemp.HeaderText = p[j].Nombre.ToString();
                                Columntemp.Width = 30;
                                listacolumnas1.Add(Columntemp);
                                dataGridView8.Columns.Add(listacolumnas1[j]);
                            }
                        }

                        dataGridView8.Rows.Add();
                    }

                    else if (marca1 != 0)
                    {
                        dataGridView8.Rows.Add();
                    }

                    int iteracion = 0;
                    iteracion = (int)numericUpDown2.Value;
                    //Número Columnas
                    for (int j = 0; j < columnas1; j++)
                    {
                        dataGridView8.Rows[i].Cells[j].Value = jacobianinversoXnmenos1[i, j, iteracion];

                        //A los elementos de la Diagonal Principal
                        if (i == j)
                        {
                            dataGridView8.Rows[i].Cells[j].Style.BackColor = Color.Yellow;
                        }
                        else if (Convert.ToDouble(dataGridView8.Rows[i].Cells[j].Value) == 0)
                        {
                            dataGridView8.Rows[i].Cells[j].Style.BackColor = Color.Cyan;
                        }

                        else if ((Convert.ToDouble(dataGridView8.Rows[i].Cells[j].Value) != 0) && (i != j))
                        {
                            dataGridView8.Rows[i].Cells[j].Style.BackColor = Color.White;
                        }
                    }

                    marca1 = 1;
                }
            }

            else
            {
                MessageBox.Show("You forget to checked Save Intermediate Numerica Algorithm Data in Solving Control Options. No data was Saved");
            }
        }

        //Visualizar la Matriz Jacobiana Inversa
        public void visualizarJacobianaInversaxn()
        {
            if ((guardarintermedias4 == 1) && (tipocalculo == 0))
            {
                //Primero Inicializamos el Control dataGridView
                dataGridView9.Rows.Clear();
                dataGridView9.Columns.Clear();

                List<DataGridViewTextBoxColumn> listacolumnas = new List<DataGridViewTextBoxColumn>();

                int filas = jacobianinversoXnmenos1.GetLength(0);
                int columnas = jacobianinversoXnmenos1.GetLength(1);
                int numiteraciones = jacobianinversoXnmenos1.GetLength(2);

                int marca = 0;

                //Número Filas
                for (int i = 0; i < filas; i++)
                {
                    if (marca == 0)
                    {
                        //Creamos las Columnas y una fila del Control DataGridView
                        for (int j = 0; j < columnas; j++)
                        {
                            //Sólo para la primera fila es necesario añadir las Columnas al Control DataGrid. Después ya no hace falta.
                            if (marca == 0)
                            {
                                DataGridViewTextBoxColumn Columntemp = new DataGridViewTextBoxColumn();
                                Columntemp.HeaderText = p[j].Nombre.ToString();
                                Columntemp.Width = 30;
                                listacolumnas.Add(Columntemp);
                                dataGridView9.Columns.Add(listacolumnas[j]);
                            }
                        }

                        dataGridView9.Rows.Add();
                    }

                    else if (marca != 0)
                    {
                        dataGridView9.Rows.Add();
                    }

                    int iteracion = 0;
                    iteracion = (int)numericUpDown1.Value;
                    //Número Columnas
                    for (int j = 0; j < columnas; j++)
                    {
                        dataGridView9.Rows[i].Cells[j].Value = jacobianinversoXnmenos1[i, j, iteracion];

                        //A los elementos de la Diagonal Principal
                        if (i == j)
                        {
                            dataGridView9.Rows[i].Cells[j].Style.BackColor = Color.Yellow;
                        }
                        else if (Convert.ToDouble(dataGridView4.Rows[i].Cells[j].Value) == 0)
                        {
                            dataGridView9.Rows[i].Cells[j].Style.BackColor = Color.Cyan;
                        }

                        else if ((Convert.ToDouble(dataGridView9.Rows[i].Cells[j].Value) != 0) && (i != j))
                        {
                            dataGridView9.Rows[i].Cells[j].Style.BackColor = Color.White;
                        }
                    }

                    marca = 1;
                }

            }

            else
            {

                MessageBox.Show("You forget to checked Save Intermediate Numerica Algorithm Data in Solving Control Options. No data was Saved");
            }
        }

        //Visualizar la matriz Xn
        public void visualizarxn()
        {
            if (guardarintermedias4 == 1)
            {
                //Primero Inicializamos el Control dataGridView
                dataGridView3.Rows.Clear();
                dataGridView3.Columns.Clear();

                List<DataGridViewTextBoxColumn> listacolumnas = new List<DataGridViewTextBoxColumn>();

                int filas = Xnmenos1.GetLength(0);
                int columnas = Xnmenos1.GetLength(1);
                int numiteraciones = Xnmenos1.GetLength(2);

                int marca = 0;

                //Número Filas
                for (int i = 0; i < filas; i++)
                {
                    if (marca == 0)
                    {
                        //Creamos las Columnas y una fila del Control DataGridView
                        for (int j = 0; j < columnas; j++)
                        {
                            //Sólo para la primera fila es necesario añadir las Columnas al Control DataGrid. Después ya no hace falta.
                            if (marca == 0)
                            {
                                DataGridViewTextBoxColumn Columntemp = new DataGridViewTextBoxColumn();
                                Columntemp.HeaderText = p[j].Nombre.ToString();
                                Columntemp.Width = 120;
                                listacolumnas.Add(Columntemp);
                                dataGridView3.Columns.Add(listacolumnas[j]);
                            }
                        }

                        dataGridView3.Rows.Add();
                    }

                    else if (marca != 0)
                    {
                        dataGridView3.Rows.Add();
                    }

                    int iteracion = 0;
                    iteracion = (int)numericUpDown1.Value;

                    //Número Columnas
                    for (int j = 0; j < columnas; j++)
                    {
                        dataGridView3.Rows[i].Cells[j].Value = Xnmenos1[i, j, iteracion];

                        //A los elementos de la Diagonal Principal
                        if (i == j)
                        {

                        }
                        else if (Convert.ToDouble(dataGridView3.Rows[i].Cells[j].Value) == 0)
                        {
                            dataGridView3.Rows[i].Cells[j].Style.BackColor = Color.Cyan;
                        }

                        else if ((Convert.ToDouble(dataGridView3.Rows[i].Cells[j].Value) != 0) && (i != j))
                        {
                            dataGridView3.Rows[i].Cells[j].Style.BackColor = Color.White;
                        }
                    }

                    marca = 1;
                }
            }

            else
            {

                MessageBox.Show("You forget to checked Save Intermediate Numerica Algorithm Data in Solving Control Options. No data was Saved");
            }
        }

        //Visualizar la Matrix de FXn
        public void visualizarfxn()
        {

            if (guardarintermedias4 == 1)
            {
                //Primero Inicializamos el Control dataGridView
                dataGridView5.Rows.Clear();
                dataGridView5.Columns.Clear();

                List<DataGridViewTextBoxColumn> listacolumnas = new List<DataGridViewTextBoxColumn>();

                int filas = Xnmenos1.GetLength(0);
                int columnas = Xnmenos1.GetLength(1);
                int numiteraciones = Xnmenos1.GetLength(2);

                int marca = 0;

                //Número Filas
                for (int i = 0; i < filas; i++)
                {
                    if (marca == 0)
                    {
                        //Creamos las Columnas y una fila del Control DataGridView
                        for (int j = 0; j < columnas; j++)
                        {
                            //Sólo para la primera fila es necesario añadir las Columnas al Control DataGrid. Después ya no hace falta.
                            if (marca == 0)
                            {
                                DataGridViewTextBoxColumn Columntemp = new DataGridViewTextBoxColumn();
                                Columntemp.HeaderText = p[j].Nombre.ToString();
                                Columntemp.Width = 120;
                                listacolumnas.Add(Columntemp);
                                dataGridView5.Columns.Add(listacolumnas[j]);
                            }
                        }

                        dataGridView5.Rows.Add();
                    }

                    else if (marca != 0)
                    {
                        dataGridView5.Rows.Add();
                    }

                    int iteracion = 0;
                    iteracion = (int)numericUpDown1.Value;

                    //Número Columnas
                    for (int j = 0; j < columnas; j++)
                    {
                        dataGridView5.Rows[i].Cells[j].Value = functionXn[i, j, iteracion];

                        //A los elementos de la Diagonal Principal
                        if (i == j)
                        {

                        }
                        else if (Convert.ToDouble(dataGridView5.Rows[i].Cells[j].Value) == 0)
                        {
                            dataGridView5.Rows[i].Cells[j].Style.BackColor = Color.Cyan;
                        }

                        else if ((Convert.ToDouble(dataGridView1.Rows[i].Cells[j].Value) != 0) && (i != j))
                        {
                            dataGridView5.Rows[i].Cells[j].Style.BackColor = Color.White;
                        }
                    }

                    marca = 1;
                }
            }

            else
            {

                MessageBox.Show("You forget to checked Save Intermediate Numerica Algorithm Data in Solving Control Options. No data was Saved");
            }
        }

        //Visualizar la Matriz Jacobiana
        public void visualizarjacobiana()
        {
            if ((guardarintermedias4 == 1)&&(tipocalculo==1))
            {
                //Primero Inicializamos el Control dataGridView
                dataGridView4.Rows.Clear();
                dataGridView4.Columns.Clear();

                List<DataGridViewTextBoxColumn> listacolumnas = new List<DataGridViewTextBoxColumn>();

                int filas = Xn.GetLength(0);
                int columnas = Xn.GetLength(1);
                int numiteraciones = Xn.GetLength(2);

                int marca = 0;

                //Número Filas
                for (int i = 0; i < filas; i++)
                {
                    if (marca == 0)
                    {

                        //Creamos las Columnas y una fila del Control DataGridView
                        for (int j = 0; j < columnas; j++)
                        {
                            //Sólo para la primera fila es necesario añadir las Columnas al Control DataGrid. Después ya no hace falta.
                            if (marca == 0)
                            {
                                DataGridViewTextBoxColumn Columntemp = new DataGridViewTextBoxColumn();
                                Columntemp.HeaderText = p[j].Nombre.ToString();
                                Columntemp.Width = 30;
                                listacolumnas.Add(Columntemp);
                                dataGridView4.Columns.Add(listacolumnas[j]);
                            }
                        }

                        dataGridView4.Rows.Add();
                    }

                    else if (marca != 0)
                    {
                        dataGridView4.Rows.Add();
                    }

                    int iteracion = 0;
                    iteracion = (int)numericUpDown1.Value;

                    //Número Columnas
                    for (int j = 0; j < columnas; j++)
                    {
                        dataGridView4.Rows[i].Cells[j].Value = jacobianXn[i, j, iteracion];

                        //A los elementos de la Diagonal Principal
                        if (i == j)
                        {
                            dataGridView4.Rows[i].Cells[j].Style.BackColor = Color.Yellow;
                        }
                        else if (Convert.ToDouble(dataGridView4.Rows[i].Cells[j].Value) == 0)
                        {
                            dataGridView4.Rows[i].Cells[j].Style.BackColor = Color.Cyan;
                        }

                        else if ((Convert.ToDouble(dataGridView4.Rows[i].Cells[j].Value) != 0) && (i != j))
                        {
                            dataGridView4.Rows[i].Cells[j].Style.BackColor = Color.White;
                        }
                    }

                    marca = 1;
                }
            }

            else
            {

                MessageBox.Show("You forget to checked Save Intermediate Numerica Algorithm Data in Solving Control Options. No data was Saved");
            }
        }

        //Cálculo del Número de Bandas Superiore e Inferiores de la Matriz Jacobiana
        public void CalculoBandas(ref int numbandasinferiores, ref int numbandassuperiores)
        {
            if (tipocalculo==1)
            {
                //Vamos a contar las Bandas Superiores e Inferiores de la Matriz jacobianXn 

                //Contamos primero las Bandas Inferiores de la Matriz jacobianXn
                for (int i = 1; i < matrizauxjacob.GetLength(0); i++)
                {
                    for (int j = 0; j < matrizauxjacob.GetLength(1); j++)
                    {
                        if ((i > j) && (matrizauxjacob[i, j] != 0))
                        {
                            //MessageBox.Show("Número de Banda Inferior: "+Convert.ToString(i-j));
                            if (numbandasinferiores < (i - j))
                            {
                                numbandasinferiores = i - j;
                            }
                        }
                    }
                }

                //Vamos a contar las Bandas Superiores e Inferiores de la Matriz jacobianXn 

                //Contamos primero las Bandas Inferiores de la Matriz jacobianXn
                for (int i = 0; i < matrizauxjacob.GetLength(0); i++)
                {
                    for (int j = 1; j < matrizauxjacob.GetLength(1); j++)
                    {
                        if ((i < j) && (matrizauxjacob[i, j] != 0))
                        {
                            //MessageBox.Show("Número de Banda Inferior: "+Convert.ToString(i-j));
                            if (numbandassuperiores < (j - i))
                            {
                                numbandassuperiores = j - i;
                            }
                        }
                    }
                }

                //MessageBox.Show("Nº de Bandas Inferiores: " + Convert.ToString(numbandasinferiores) + "  Nº de Bandas Superiores: " + Convert.ToString(numbandassuperiores));
                //MessageBox.Show("ml+mu+1: " + Convert.ToString(numbandasinferiores + numbandassuperiores + 1) + " y n= " + Convert.ToString(matrizauxjacob.GetLength(0)));
            }

            else if (tipocalculo == 0)
            {              
                //Vamos a contar las Bandas Superiores e Inferiores de la Matriz jacobianXn 
                
                //Contamos primero las Bandas Inferiores de la Matriz jacobianXn
                for (int i = 1; i < matrizauxjacob.GetLength(0); i++)
                {
                    for (int j = 0; j < matrizauxjacob.GetLength(1); j++)
                    {
                        if ((i > j) && (matrizauxjacob[i, j] != 0))
                        {
                            //MessageBox.Show("Número de Banda Inferior: "+Convert.ToString(i-j));
                            if (numbandasinferiores < (i - j))
                            {
                                numbandasinferiores = i - j;
                            }
                        }
                    }
                }

                //Vamos a contar las Bandas Superiores e Inferiores de la Matriz jacobianXn 

                //Contamos primero las Bandas Inferiores de la Matriz jacobianXn
                for (int i = 0; i < matrizauxjacob.GetLength(0); i++)
                {
                    for (int j = 1; j < matrizauxjacob.GetLength(1); j++)
                    {
                        if ((i < j) && (matrizauxjacob[i, j] != 0))
                        {
                            //MessageBox.Show("Número de Banda Inferior: "+Convert.ToString(i-j));
                            if (numbandassuperiores < (j - i))
                            {
                                numbandassuperiores = j - i;
                            }
                        }
                    }
                }

                //MessageBox.Show("Nº de Bandas Inferiores: " + Convert.ToString(numbandasinferiores) + "  Nº de Bandas Superiores: " + Convert.ToString(numbandassuperiores));
                //MessageBox.Show("ml+mu+1: " + Convert.ToString(numbandasinferiores + numbandassuperiores + 1) + " y n= " + Convert.ToString(matrizauxjacob.GetLength(0)));
            }

            else
            {

                //MessageBox.Show("You forget to checked Save Intermediate Numerica Algorithm Data in Solving Control Options. No data was Saved");
            }
        }

        //Ocultar el DataGridView de los Valores de las Tablas 
        private void button10_Click(object sender, EventArgs e)
        {
            dataGridView2.Show();
            chart1.Hide();
        }

        //Mostrar el DataGridView de los Valores de las Tablas 
        private void button11_Click(object sender, EventArgs e)
        {            
            dataGridView2.Hide();
            chart1.Show();           
        }


        //Numerical Algorithms Details TAB 0. Botón de SYSTEM MATRIX
        public void visualizarSystemMatrix()
        {
            //Primero Inicializamos el Control dataGridView
            dataGridView13.Rows.Clear();
            dataGridView13.Columns.Clear();

            List<DataGridViewTextBoxColumn> listacolumnas = new List<DataGridViewTextBoxColumn>();

            int filas = matrizauxjacob.GetLength(0);
            int columnas = matrizauxjacob.GetLength(1);

            int marca = 0;

            //Número Filas
            for (int i = 0; i < filas; i++)
            {
                if (marca == 0)
                {

                    //Creamos las Columnas y una fila del Control DataGridView
                    for (int j = 0; j < columnas; j++)
                    {
                        //Sólo para la primera fila es necesario añadir las Columnas al Control DataGrid. Después ya no hace falta.
                        if (marca == 0)
                        {
                            DataGridViewTextBoxColumn Columntemp = new DataGridViewTextBoxColumn();
                            Columntemp.HeaderText = p[j].Nombre.ToString();
                            Columntemp.Width = 30;
                            listacolumnas.Add(Columntemp);
                            dataGridView13.Columns.Add(listacolumnas[j]);
                        }
                    }

                    dataGridView13.Rows.Add();
                }

                else if (marca != 0)
                {
                    dataGridView13.Rows.Add();
                }

                //Número Columnas
                for (int j = 0; j < columnas; j++)
                {
                    dataGridView13.Rows[i].Cells[j].Value = matrizauxjacob[i, j];

                    //A los elementos de la Diagonal Principal
                    if (i == j)
                    {
                        dataGridView13.Rows[i].Cells[j].Style.BackColor = Color.Yellow;
                    }
                    else if (Convert.ToDouble(dataGridView13.Rows[i].Cells[j].Value) == 0)
                    {
                        dataGridView13.Rows[i].Cells[j].Style.BackColor = Color.Cyan;
                    }

                    else if ((Convert.ToDouble(dataGridView13.Rows[i].Cells[j].Value) != 0) && (i != j))
                    {
                        dataGridView13.Rows[i].Cells[j].Style.BackColor = Color.White;
                    }
                }

                marca = 1;
            }
        }

        //Botón para mostrar todas las ecuaciones del Sistema
        public void visualizarecuaciones()
        {
            listBox4.Items.Clear();
            listBox4.Items.Add("");
            //Creamos en tiempo real los TextBoxes para introducir las ecuaciones
            for (int i = 0; i <ecuaciones.Count; i++)
            {
                listBox4.Items.Add("Ecuación Nº" + Convert.ToString(i+1) + " :" + ecuaciones[i]);
                listBox4.Items.Add("");
            }
        }

        //Visualizar la Matrix Xn+1
        public void visualizarXnmasuno ()
        {
            if (guardarintermedias4 == 1)
            {
                //Primero Inicializamos el Control dataGridView
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();

                List<DataGridViewTextBoxColumn> listacolumnas = new List<DataGridViewTextBoxColumn>();

                int filas = Xn.GetLength(0);
                int columnas = Xn.GetLength(1);
                int numiteraciones = Xn.GetLength(2);

                int marca = 0;

                //Número Filas
                for (int i = 0; i < filas; i++)
                {
                    if (marca == 0)
                    {

                        //Creamos las Columnas y una fila del Control DataGridView
                        for (int j = 0; j < columnas; j++)
                        {
                            //Sólo para la primera fila es necesario añadir las Columnas al Control DataGrid. Después ya no hace falta.
                            if (marca == 0)
                            {
                                DataGridViewTextBoxColumn Columntemp = new DataGridViewTextBoxColumn();
                                Columntemp.HeaderText = p[j].Nombre.ToString();
                                Columntemp.Width = 120;
                                listacolumnas.Add(Columntemp);
                                dataGridView1.Columns.Add(listacolumnas[j]);
                            }
                        }

                        dataGridView1.Rows.Add();
                    }

                    else if (marca != 0)
                    {
                        dataGridView1.Rows.Add();
                    }

                    int iteracion = 0;
                    iteracion = (int)numericUpDown1.Value;

                    //Número Columnas
                    for (int j = 0; j < columnas; j++)
                    {
                        dataGridView1.Rows[i].Cells[j].Value = Xn[i, j, iteracion];

                        //A los elementos de la Diagonal Principal
                        if (i == j)
                        {

                        }
                        else if (Convert.ToDouble(dataGridView1.Rows[i].Cells[j].Value) == 0)
                        {
                            dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Cyan;
                        }

                        else if ((Convert.ToDouble(dataGridView1.Rows[i].Cells[j].Value) != 0) && (i != j))
                        {
                            dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.White;
                        }
                    }

                    marca = 1;
                }

            }

            else
            {

                MessageBox.Show("You forget to checked Save Intermediate Numerica Algorithm Data in Solving Control Options. No data was Saved");
            }
        }

        //Cada Vez que incrementamos el número de ITERACIONES refrescamos los valores de de las Matrices de Newton Raphson 
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {   
            if ((guardarintermedias4 == 1) && (tipocalculo == 0))
            {
                visualizarXnmasuno();
                visualizarxn();
                visualizarJacobianaInversaxnmenos1();
                visualizarJacobianaInversaxn();
                visualizarfxn();
            }

            else if ((guardarintermedias4 == 1) && (tipocalculo == 1))
            {
                visualizarXnmasuno();
                visualizarxn();
                visualizarjacobiana();
                visualizarfxn();            
            }
        }

        //Evento que se produce cuando se pulsa alguna de las Hojas del TabControl
        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            //Al pulsar el Tab 4 Numerical Methods Algorithm Data 1 actualizamos los valores de de las Matrices de Newton Raphson
            if (e.TabPage == tabPage4)
            {

                if ((guardarintermedias4 == 1) && (tipocalculo == 0))
                {
                    visualizarSystemMatrix();
                    visualizarecuaciones();

                    visualizarXnmasuno();
                    visualizarxn();
                    visualizarJacobianaInversaxnmenos1();
                    visualizarfxn();
                }

                else if ((guardarintermedias4 == 1) && (tipocalculo == 1))
                {
                    visualizarSystemMatrix();
                    visualizarecuaciones();

                    visualizarXnmasuno();
                    visualizarxn();
                    visualizarjacobiana();
                    visualizarfxn();
                }
            }
             
            //Tab del Modelo Entrada de Datos del Heat Balance
            else if (e.TabPage == tabPage17)
            {
                tabControl2.SelectedTab = tabPage18;
            }

            //Tab de Resultados Gráficos del Heat Balance
            else if (e.TabPage == tabPage19)
            {
                tabControl2.SelectedTab = tabPage11;
            }

            //Tab de Imagen BMP Modelo del Heat Balance
            else if (e.TabPage == tabPage1)
            {
                tabControl2.SelectedTab = tabPage5;
            }

             //Tab de Equipment Data Output
            else if (e.TabPage == tabPage2)
            {
                tabControl2.SelectedTab = tabPage8;
                visualizarResultadosDeLosEquiposToolStripMenuItem_Click(sender,e);
            }
        }

        
        
//-------------------------------------------------------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------------------------------------------------------
        //DIAGRAM .NET view

        #region Functions

        private void Edit_UpdateUndoRedoEnable()
        {
            if (tabControl1.SelectedTab == tabPage17)
            {
                undooption.Enabled = designer1.CanUndo;
                //btnUndo.Enabled = designer1.CanUndo;
                redooption.Enabled = designer1.CanRedo;
                //btnRedo.Enabled = designer1.CanRedo;
            }

            else if (tabControl1.SelectedTab == tabPage19)
            {
                undooption.Enabled = designer2.CanUndo;
                //btnUndo.Enabled = designer1.CanUndo;
                redooption.Enabled = designer2.CanRedo;
                //btnRedo.Enabled = designer1.CanRedo;
            }
        }

        private void Edit_Undo()
        {
            if (tabControl1.SelectedTab == tabPage17)
            {
                if (designer1.CanUndo)
                    designer1.Undo();

                Edit_UpdateUndoRedoEnable();
            }

            else if (tabControl1.SelectedTab == tabPage19)
            {
                if (designer2.CanUndo)
                    designer2.Undo();

                Edit_UpdateUndoRedoEnable();            
            }
        }

        private void Edit_Redo()
        {
            if (tabControl1.SelectedTab == tabPage17)
            {
                if (designer1.CanRedo)
                    designer1.Redo();

                Edit_UpdateUndoRedoEnable();
            }

            else if (tabControl1.SelectedTab == tabPage19)
            {
                if (designer2.CanRedo)
                    designer2.Redo();

                Edit_UpdateUndoRedoEnable();
            }
        }

        private void Action_None()
        {
            sizeoption.Checked = false;
            addoption.Checked = false;
            deleteoption.Checked = false;
            connectoption.Checked = false;

            //btnSize.Pushed = false;
            //btnAdd.Pushed = false;
            //btnDelete.Pushed = false;
            //btnConnect.Pushed = false;

            rectangleoption.Checked = false;
            mnuTbRectangle.Checked = false;
            ellipseoption.Checked = false;
            mnuTbElipse.Checked = false;
            rectangleNodeoption.Checked = false;
            mnuTbRectangleNode.Checked = false;
            ellipseNodeoption.Checked = false;
            mnuTbElipseNode.Checked = false;
        }

        private void Action_Size()
        {
            if (tabControl1.SelectedTab == tabPage17)
            {
                Action_None();
                sizeoption.Checked = true;
                //btnSize.Pushed = true;
                if (changeDocumentProp)
                    designer1.Document.Action = DesignerAction.Select;
            }

            else if (tabControl1.SelectedTab ==tabPage19)
            {
             Action_None();
                sizeoption.Checked = true;
                //btnSize.Pushed = true;
                if (changeDocumentProp)
                    designer2.Document.Action = DesignerAction.Select;
            }
        }

        private void Action_Add(ElementType e)
        {
            if (tabControl1.SelectedTab == tabPage17)
            {
                Action_None();
                //btnAdd.Pushed = true;
                switch (e)
                {
                    case ElementType.Rectangle:
                        rectangleoption.Checked = true;
                        mnuTbRectangle.Checked = true;
                        break;
                    case ElementType.Elipse1:
                        ellipseoption.Checked = true;
                        mnuTbElipse.Checked = true;
                        break;
                    case ElementType.Turbina:
                        turbinaoption.Checked = true;
                        mnuTbTurbina.Checked = true;
                        break;
                    case ElementType.RectangleNode:
                        rectangleNodeoption.Checked = true;
                        mnuTbRectangleNode.Checked = true;
                        break;
                    case ElementType.ElipseNode1:
                        ellipseNodeoption.Checked = true;
                        mnuTbElipseNode.Checked = true;
                        break;
                    case ElementType.TurbinaNode:
                        turbinanodeoption.Checked = true;
                        mnuTbTurbinaNode.Checked = true;
                        break;
                }

                if (changeDocumentProp)
                {
                    designer1.Document.Action = DesignerAction.Add;
                    designer1.Document.ElementType = e;
                }
            }

            else if (tabControl1.SelectedTab == tabPage19)
            {
                Action_None();
                //btnAdd.Pushed = true;
                switch (e)
                {
                    case ElementType.Rectangle:
                        rectangleoption.Checked = true;
                        mnuTbRectangle.Checked = true;
                        break;
                    case ElementType.Elipse1:
                        ellipseoption.Checked = true;
                        mnuTbElipse.Checked = true;
                        break;
                    case ElementType.Turbina:
                        turbinaoption.Checked = true;
                        mnuTbTurbina.Checked = true;
                        break;
                    case ElementType.RectangleNode:
                        rectangleNodeoption.Checked = true;
                        mnuTbRectangleNode.Checked = true;
                        break;
                    case ElementType.ElipseNode1:
                        ellipseNodeoption.Checked = true;
                        mnuTbElipseNode.Checked = true;
                        break;
                    case ElementType.TurbinaNode:
                        turbinanodeoption.Checked = true;
                        mnuTbTurbinaNode.Checked = true;
                        break;
                }

                if (changeDocumentProp)
                {
                    designer2.Document.Action = DesignerAction.Add;
                    designer2.Document.ElementType = e;
                }
            }
        }

        private void Action_Delete()
        {
            if (tabControl1.SelectedTab == tabPage17)
            {
                Action_None();
                deleteoption.Checked = true;
                //btnDelete.Pushed = true;
                if (changeDocumentProp)
                    designer1.Document.DeleteSelectedElements();
                Action_None();
            }

            else if (tabControl1.SelectedTab == tabPage19)
            {
                Action_None();
                deleteoption.Checked = true;
                //btnDelete.Pushed = true;
                if (changeDocumentProp)
                    designer2.Document.DeleteSelectedElements();
                Action_None();
            }
        }

        private void Action_Connect()
        {
            if (tabControl1.SelectedTab == tabPage17)
            {
                Action_None();
                connectoption.Checked = true;
                //btnConnect.Pushed = true;
                if (changeDocumentProp)
                    designer1.Document.Action = DesignerAction.Connect;
            }
            else if (tabControl1.SelectedTab == tabPage19)
            {
                Action_None();
                connectoption.Checked = true;
                //btnConnect.Pushed = true;
                if (changeDocumentProp)
                    designer2.Document.Action = DesignerAction.Connect;
            }
        }

        private void Action_Connector(LinkType lt)
        {
            if (tabControl1.SelectedTab == tabPage17)
            {
                Action_None();
                switch (lt)
                {
                    case LinkType.Straight:
                        mnuTbStraightLink.Checked = true;
                        mnuTbRightAngleLink.Checked = false;
                        break;
                    case LinkType.RightAngle:
                        mnuTbStraightLink.Checked = false;
                        mnuTbRightAngleLink.Checked = true;
                        break;
                }
                designer1.Document.LinkType = lt;
                Action_Connect();
            }

            else if (tabControl1.SelectedTab == tabPage19)
            {
                Action_None();
                switch (lt)
                {
                    case LinkType.Straight:
                        mnuTbStraightLink.Checked = true;
                        mnuTbRightAngleLink.Checked = false;
                        break;
                    case LinkType.RightAngle:
                        mnuTbStraightLink.Checked = false;
                        mnuTbRightAngleLink.Checked = true;
                        break;
                }
                designer2.Document.LinkType = lt;
                Action_Connect();
            }
            
            }

        private void Action_Zoom(float zoom)
        {
            if (tabControl1.SelectedTab == tabPage17)
            {
                designer1.Document.Zoom = zoom;
            }

            else if (tabControl1.SelectedTab == tabPage19)
            {
                designer2.Document.Zoom = zoom;
            }
        }

        private void File_Open()
        {
            if (tabControl1.SelectedTab == tabPage17)
            {
                if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    designer1.Open(openFileDialog1.FileName);
                }
            }

            else if (tabControl1.SelectedTab == tabPage19)
            {
                if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    designer2.Open(openFileDialog1.FileName);
                }
            }
        }

        private void File_Save()
        {
            if (tabControl1.SelectedTab == tabPage17)
            {
                if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    designer1.Save(saveFileDialog1.FileName);
                }
            }

            else if (tabControl1.SelectedTab == tabPage19)
            {
                if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    designer2.Save(saveFileDialog1.FileName);
                }
            }
        }

        private void Order_BringToFront()
        {
            if (tabControl1.SelectedTab == tabPage17)
            {
                if (designer1.Document.SelectedElements.Count == 1)
                {
                    designer1.Document.BringToFrontElement(designer1.Document.SelectedElements[0]);
                    designer1.Refresh();
                }
            }

            else if (tabControl1.SelectedTab == tabPage19)
            {
                if (designer2.Document.SelectedElements.Count == 1)
                {
                    designer2.Document.BringToFrontElement(designer2.Document.SelectedElements[0]);
                    designer2.Refresh();
                }
            }
        }

        private void Order_SendToBack()
        {
            if (tabControl1.SelectedTab == tabPage17)
            {
                if (designer1.Document.SelectedElements.Count == 1)
                {
                    designer1.Document.SendToBackElement(designer1.Document.SelectedElements[0]);
                    designer1.Refresh();
                }
            }

            else if (tabControl1.SelectedTab == tabPage19)
            {
                if (designer2.Document.SelectedElements.Count == 1)
                {
                    designer2.Document.SendToBackElement(designer2.Document.SelectedElements[0]);
                    designer2.Refresh();
                }
            }
        }

        private void Order_MoveUp()
        {
            if (tabControl1.SelectedTab == tabPage17)
            {
                if (designer1.Document.SelectedElements.Count == 1)
                {
                    designer1.Document.MoveUpElement(designer1.Document.SelectedElements[0]);
                    designer1.Refresh();
                }
            }

            else if (tabControl1.SelectedTab == tabPage19)
            {
                if (designer2.Document.SelectedElements.Count == 1)
                {
                    designer2.Document.MoveUpElement(designer2.Document.SelectedElements[0]);
                    designer2.Refresh();
                }
            }
        }

        private void Order_MoveDown()
        {
            if (tabControl1.SelectedTab == tabPage17)
            {
                if (designer1.Document.SelectedElements.Count == 1)
                {
                    designer1.Document.MoveDownElement(designer1.Document.SelectedElements[0]);
                    designer1.Refresh();
                }
            }

            else if (tabControl1.SelectedTab == tabPage19)
            {
                if (designer2.Document.SelectedElements.Count == 1)
                {
                    designer2.Document.MoveDownElement(designer2.Document.SelectedElements[0]);
                    designer2.Refresh();
                }
            }
        }

        private void Clipboard_Cut()
        {
            if (tabControl1.SelectedTab == tabPage17)
            {
                designer1.Cut();
            }

            else if (tabControl1.SelectedTab == tabPage19)
            {
                designer2.Cut();
            }
        }

        private void Clipboard_Copy()
        {
            if (tabControl1.SelectedTab == tabPage17)
            {
                designer1.Copy();
            }

            else if (tabControl1.SelectedTab == tabPage19)
            {
                designer2.Copy();
            }
        }

        private void Clipboard_Paste()
        {
            if (tabControl1.SelectedTab == tabPage17)
            {
                designer1.Paste();
            }

            else if (tabControl1.SelectedTab == tabPage19)
            {
                designer2.Paste();
            }
        }

        #endregion

       
        private void Document_PropertyChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage17)
            {
                changeDocumentProp = false;

                Action_None();

                switch (designer1.Document.Action)
                {
                    case DesignerAction.Select:
                        Action_Size();
                        break;
                    case DesignerAction.Delete:
                        Action_Delete();
                        break;
                    case DesignerAction.Connect:
                        Action_Connect();
                        break;
                    case DesignerAction.Add:
                        Action_Add(designer1.Document.ElementType);
                        break;
                }

                Edit_UpdateUndoRedoEnable();

                changeDocumentProp = true;
            }

            else if (tabControl1.SelectedTab == tabPage19)
            {
                changeDocumentProp = false;

                Action_None();

                switch (designer2.Document.Action)
                {
                    case DesignerAction.Select:
                        Action_Size();
                        break;
                    case DesignerAction.Delete:
                        Action_Delete();
                        break;
                    case DesignerAction.Connect:
                        Action_Connect();
                        break;
                    case DesignerAction.Add:
                        Action_Add(designer2.Document.ElementType);
                        break;
                }

                Edit_UpdateUndoRedoEnable();

                changeDocumentProp = true;
            }
        }



        //Opciones del MENU-----------------------------------------------------------------------------------------------------
        private void mnuZoom_10_Click(object sender, System.EventArgs e)
        {
            Action_Zoom(0.1f);
        }

        private void mnuZoom_25_Click(object sender, System.EventArgs e)
        {
            Action_Zoom(0.25f);
        }

        private void mnuZoom_50_Click(object sender, System.EventArgs e)
        {
            Action_Zoom(0.5f);
        }

        private void mnuZoom_75_Click(object sender, System.EventArgs e)
        {
            Action_Zoom(0.75f);
        }

        private void mnuZoom_100_Click(object sender, System.EventArgs e)
        {
            Action_Zoom(1f);
        }

        private void mnuZoom_150_Click(object sender, System.EventArgs e)
        {
            Action_Zoom(1.5f);
        }

        private void mnuZoom_200_Click(object sender, System.EventArgs e)
        {
            Action_Zoom(2.0f);
        }


        #region Events Handling
        private void designer1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage17)
            {
                AppendLog("designer1_MouseUp: {0}", e.ToString());

                propertyGrid10.SelectedObject = null;

                if (designer1.Document.SelectedElements.Count == 1)
                {
                    propertyGrid10.SelectedObject = designer1.Document.SelectedElements[0];
                }
                else if (designer1.Document.SelectedElements.Count > 1)
                {
                    propertyGrid10.SelectedObjects = designer1.Document.SelectedElements.GetArray();
                }
                else if (designer1.Document.SelectedElements.Count == 0)
                {
                    propertyGrid10.SelectedObject = designer1.Document;
                }
            }
        }

        //Evento cuando se levanta el botón del ratón y está seleccionado control de dibujo "designer1" asinamos al control "propertyGrid10" las propiedades de los objetos de dibujo seleccionados
        private void designer2_MouseUp(object sender, MouseEventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage19)
            {
                AppendLog("designer1_MouseUp: {0}", e.ToString());

                propertyGrid10.SelectedObject = null;

                if (designer2.Document.SelectedElements.Count == 1)
                {
                    propertyGrid10.SelectedObject = designer2.Document.SelectedElements[0];
                }
                else if (designer2.Document.SelectedElements.Count > 1)
                {
                    propertyGrid10.SelectedObjects = designer2.Document.SelectedElements.GetArray();
                }
                else if (designer2.Document.SelectedElements.Count == 0)
                {
                    propertyGrid10.SelectedObject = designer2.Document;
                }            
            }
        }           
        
        //Evento cuando se levanta el botón del ratón y está seleccionado control de dibujo "designer2" asinamos al control "propertyGrid10" las propiedades de los objetos de dibujo seleccionados
        private void designer1_ElementClick(object sender, Dalssoft.DiagramNet.ElementEventArgs e)
        {
            AppendLog("designer1_ElementClick: {0}", e.ToString());
        }

        private void designer1_ElementMouseDown(object sender, Dalssoft.DiagramNet.ElementMouseEventArgs e)
        {
            AppendLog("designer1_ElementMouseDown: {0}", e.ToString());
        }

        private void designer1_ElementMouseUp(object sender, Dalssoft.DiagramNet.ElementMouseEventArgs e)
        {
            AppendLog("designer1_ElementMouseUp: {0}", e.ToString());
        }

        private void designer1_ElementMoved(object sender, Dalssoft.DiagramNet.ElementEventArgs e)
        {
            AppendLog("designer1_ElementMoved: {0}", e.ToString());
        }

        private void designer1_ElementMoving(object sender, Dalssoft.DiagramNet.ElementEventArgs e)
        {
            AppendLog("designer1_ElementMoving: {0}", e.ToString());
        }

        private void designer1_ElementResized(object sender, Dalssoft.DiagramNet.ElementEventArgs e)
        {
            AppendLog("designer1_ElementResized: {0}", e.ToString());
        }

        private void designer1_ElementResizing(object sender, Dalssoft.DiagramNet.ElementEventArgs e)
        {
            AppendLog("designer1_ElementResizing: {0}", e.ToString());
        }

        private void designer1_ElementConnected(object sender, Dalssoft.DiagramNet.ElementConnectEventArgs e)
        {
            AppendLog("designer1_ElementConnected: {0}", e.ToString());
        }

        private void designer1_ElementConnecting(object sender, Dalssoft.DiagramNet.ElementConnectEventArgs e)
        {
            AppendLog("designer1_ElementConnecting: {0}", e.ToString());
        }

        private void designer1_ElementSelection(object sender, Dalssoft.DiagramNet.ElementSelectionEventArgs e)
        {
            AppendLog("designer1_ElementSelection: {0}", e.ToString());
        }

        #endregion

        private void AppendLog(string log)
        {
            AppendLog(log, "");
        }

        private void AppendLog(string log, params object[] args)
        {
            if (showDrawoption.Checked)
                txtLog.AppendText(String.Format(log, args) + "\r\n");
        }

//---------------------------------------------------------------------------------------------------------------------------------
        //MENU CONTEXTUAL (Right click Menu)

        //Crear un Elemento Rectángulo
        private void mnuTbRectangle_Click(object sender, System.EventArgs e)
        {
            Action_Add(ElementType.Rectangle);
        }

        //Crear un Elemento Elipse
        private void mnuTbElipse_Click(object sender, System.EventArgs e)
        {
            Action_Add(ElementType.Elipse1);
        }

        //Crear un Elemento Rectángulo Nudo
        private void mnuTbRectangleNode_Click(object sender, System.EventArgs e)
        {
            Action_Add(ElementType.RectangleNode);
        }

        //Crear un Elemento Elipse Nudo
        private void mnuTbElipseNode_Click(object sender, System.EventArgs e)
        {
            Action_Add(ElementType.ElipseNode1);
        }

        //Crear un Elemento Comentario
        private void TbCommentBox_Click(object sender, System.EventArgs e)
        {
            Action_Add(ElementType.CommentBox);
        }

        //Añadir un Elemento Turbin Nudo
        private void mnuTbTurbinaNode_Click(object sender, EventArgs e)
        {
            Action_Add(ElementType.TurbinaNode);
        }  


//------------------------------------------------------------------------------------------------------------------------------------
        //Nuevo Menú

        //OpenFile Option
        private void openoption_Click(object sender, EventArgs e)
        {
            File_Open();
        }

        //SaveFile Option
        private void saveoption_Click(object sender, EventArgs e)
        {
            File_Save();
        }
        
        //Add Rectangle Element
        private void rectangleoption_Click(object sender, EventArgs e)
        {
            Action_Add(ElementType.Rectangle);
        }

        //Add Ellipse Element
        private void ellipseoption_Click(object sender, EventArgs e)
        {
            Action_Add(ElementType.Elipse1);
        }

        //Add RectangleNode Element
        private void rectangleNodeoption_Click(object sender, EventArgs e)
        {
            Action_Add(ElementType.RectangleNode);
        }

        //Add EllipseNode Element
        private void ellipseNodeoption_Click(object sender, EventArgs e)
        {
            Action_Add(ElementType.ElipseNode1);
        }

        private void commentBoxoption_Click(object sender, EventArgs e)
        {
            Action_Add(ElementType.CommentBox);
        }

        //Straight Link Option
        private void straightLinkoption_Click(object sender, EventArgs e)
        {
            Action_Connector(LinkType.Straight);
        }

        //Right Angle Link Option
        private void rightAngleLinkoption_Click(object sender, EventArgs e)
        {
            Action_Connector(LinkType.RightAngle);
        }

        //Show Drawing Log Events Option
        private void showDrawoption_Click(object sender, EventArgs e)
        {
            showDrawoption.Checked = !showDrawoption.Checked;
            txtLog.Visible = showDrawoption.Checked;
        }

        //Size Option
        private void sizeoption_Click(object sender, EventArgs e)
        {
            Action_Size();
        }

        //Connect Option
        private void connectoption_Click(object sender, EventArgs e)
        {
            Action_Connect();
        }


        //Bring to Front
        private void bringToFrontoption_Click(object sender, EventArgs e)
        {
            Order_BringToFront();
        }

        //Send to Back
        private void sendToBackoption_Click(object sender, EventArgs e)
        {
            Order_SendToBack();
        }

        //Move Down
        private void moveDownoption_Click(object sender, EventArgs e)
        {
            Order_MoveDown();
        }

        //Move Up
        private void moveUpoption_Click(object sender, EventArgs e)
        {
            Order_MoveUp();
        }

        //Cut Option
        private void cutoption_Click(object sender, EventArgs e)
        {
            Clipboard_Cut();
        }

        //Copy Option 
        private void copyoption_Click(object sender, EventArgs e)
        {
            Clipboard_Copy();
        }

        //Paste Option
        private void pasteoption_Click(object sender, EventArgs e)
        {
            Clipboard_Paste();
        }

        //Delete Option
        private void deleteoption_Click(object sender, EventArgs e)
        {
            Action_Delete();
        }

        //Undo Option
        private void undooption_Click(object sender, EventArgs e)
        {
            Edit_Undo();
        }

        //Redo Option
        private void redooption_Click(object sender, EventArgs e)
        {
            Edit_Redo();
        }

        //Straight Link Context Menu
        private void straightlinkcontext_Click(object sender, EventArgs e)
        {
            Action_Connector(LinkType.Straight);
        }

        //Right Angle Link Context Menu
        private void rightanglelinkcontext_Click(object sender, EventArgs e)
        {
            Action_Connector(LinkType.RightAngle);
        }

        //Zoom Dinámico
        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            //Single valor=(Single)(hScrollBar1.Value/100);
            //Action_Zoom(valor);
        }

        //Zoom 10%
        private void toolStripMenuItem13_Click(object sender, EventArgs e)
        {
            Action_Zoom(0.1f);
        }

        //Zoom 25%
        private void toolStripMenuItem17_Click(object sender, EventArgs e)
        {
            Action_Zoom(0.25f);
        }

        //Zoom 50%
        private void toolStripMenuItem18_Click(object sender, EventArgs e)
        {
            Action_Zoom(0.50f);
        }

        //Zoom 75%
        private void toolStripMenuItem19_Click(object sender, EventArgs e)
        {
            Action_Zoom(0.75f);
        }

        //Zoom 100%
        private void toolStripMenuItem20_Click(object sender, EventArgs e)
        {
            Action_Zoom(1.0f);
        }

        //Zoom 150%
        private void toolStripMenuItem21_Click(object sender, EventArgs e)
        {
            Action_Zoom(1.5f);
        }

        //Zoom 200%
        private void toolStripMenuItem22_Click(object sender, EventArgs e)
        {
            Action_Zoom(2.0f);
        }

        //Zoom to Fit
        private void zoomToFitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Designer Control Width: "+Convert.ToString(designer1.Size.Width));
            //MessageBox.Show("Designer Canvas Width: " + Convert.ToString((float)designer1.HorizontalScroll.Maximum);

            float maxWidthScale = (float)designer1.Size.Width / (float)designer1.HorizontalScroll.Maximum;
            float maxHeightScale = (float)designer1.Size.Height / (float)designer1.VerticalScroll.Maximum;

            float scale = Math.Min(maxHeightScale, maxWidthScale);

            Action_Zoom(scale);
        }

        //Añadir un Elemento Turbina
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Action_Add(ElementType.Turbina);
        }

        //Añadir un Elemento TurbinaNode
        private void toolStripMenuItem23_Click(object sender, EventArgs e)
        {
            Action_Add(ElementType.TurbinaNode);
        }

        //Delete/Borrar un Elemento
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Action_Delete();
        }

        //Copy/Copiar un Elemento
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard_Copy();
        }

        //Paste/Pegar un Elemento
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard_Paste();
        }

        //Cut/Cortar un Elemento
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard_Cut();
        }

        //Size option for Contextual Menu
        private void toolStripMenuItem23_Click_1(object sender, EventArgs e)
        {
            Action_Size();
        }

        //Connect option for Contextual Menu
        private void toolStripMenuItem2_Click_1(object sender, EventArgs e)
        {
            Action_Connect();
        }

        //Añadir Elemento Turbina
        private void mnuTbTurbina_Click(object sender, EventArgs e)
        {
            Action_Add(ElementType.Turbina);
        }

        //Export Image (*.BMP)
        private void toolStripMenuItem24_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage17)
            {
                int nWidth = (int)designer1.HorizontalScroll.Maximum;
                int nHeight = (int)designer1.VerticalScroll.Maximum;

                if (nWidth < 625)
                {
                    nWidth = 675;
                }

                if (nHeight < 625)
                {
                    nHeight = 675;
                }

                Pen redPen = new Pen(Color.Red, 8);
                //Creamos un Bitmap donde vamos a dibujar todas las figuras de nuestro modelo
                Bitmap b1 = new Bitmap(nWidth, nHeight);
                Graphics g1 = Graphics.FromImage(b1);

                //Llamamos primero a Zoom to Fit
                //zoomToFitToolStripMenuItem_Click(sender, e);

                //Dibujar sobre el Bitmap "b1" los elementos guardados en la Clase Document en el Array "elements"
                for (int i = 0; i < designer1.Document.elements.Count; i++)
                {
                    designer1.Document.elements[i].Draw(g1);
                }

                //pictureBox3.Image = b1;

                saveFileDialog2.Filter = "bmp files (*.bmp)|*.bmp|All files (*.*)|*.*";
                saveFileDialog2.Title = "Export Model Image to *.BMP file";
                saveFileDialog2.InitialDirectory = "C:\\";

                if (saveFileDialog2.ShowDialog() == DialogResult.OK)
                {
                    String archivobmp = saveFileDialog2.FileName;
                    b1.Save(archivobmp);
                }
            }

            else if (tabControl1.SelectedTab == tabPage19)
            {
                int nWidth = (int)designer2.HorizontalScroll.Maximum;
                int nHeight = (int)designer2.VerticalScroll.Maximum;

                if (nWidth < 625)
                {
                    nWidth = 675;
                }

                if (nHeight < 625)
                {
                    nHeight = 675;
                }

                Pen redPen = new Pen(Color.Red, 8);
                //Creamos un Bitmap donde vamos a dibujar todas las figuras de nuestro modelo
                Bitmap b1 = new Bitmap(nWidth, nHeight);
                Graphics g1 = Graphics.FromImage(b1);

                //Llamamos primero a Zoom to Fit
                //zoomToFitToolStripMenuItem_Click(sender, e);

                //Dibujar sobre el Bitmap "b1" los elementos guardados en la Clase Document en el Array "elements"
                for (int i = 0; i < designer2.Document.elements.Count; i++)
                {
                    designer2.Document.elements[i].Draw(g1);
                }

                //pictureBox3.Image = b1;

                saveFileDialog2.Filter = "bmp files (*.bmp)|*.bmp|All files (*.*)|*.*";
                saveFileDialog2.Title = "Export Model Image to *.BMP file";
                saveFileDialog2.InitialDirectory = "C:\\";

                if (saveFileDialog2.ShowDialog() == DialogResult.OK)
                {
                    String archivobmp = saveFileDialog2.FileName;
                    b1.Save(archivobmp);
                }
            }
        }

        //Añadir un Elemento Imagen
        private void imageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Action_Add(ElementType.Image);
        }

        //Añadir un Elemento Imagen Nudo
        private void imageNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Action_Add(ElementType.ImageNode);
        }

        //AÑadir Elemento Imagen desde el Menu Contextual
        private void imageToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage17)
            {
                //Esta Variable viene por el Usuario cuando arrastramos un Elemento desde la ListView Control que contiene los Tipos de Equipos
                designer1.TipodeEquipo = 1;
                designer1.imagen = imageList2.Images[indiceimagen];
                Action_Add(ElementType.Image);
            }

            else if (tabControl1.SelectedTab == tabPage19)
            {
                //Esta Variable viene por el Usuario cuando arrastramos un Elemento desde la ListView Control que contiene los Tipos de Equipos
                designer2.TipodeEquipo = 1;
                designer2.imagen = imageList2.Images[indiceimagen];
                Action_Add(ElementType.Image);
            }
        }

        //Añadir Elemento Nodeo Imagen desde el Menu Contextual
        private void imageNodeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage17)
            {
                //Esta Variable viene por el Usuario cuando arrastramos un Elemento desde la ListView Control que contiene los Tipos de Equipos
                designer1.TipodeEquipo = 1;
                designer1.imagen = imageList2.Images[indiceimagen];
                Action_Add(ElementType.ImageNode);
            }

            else if (tabControl1.SelectedTab == tabPage19)
            {
                //Esta Variable viene por el Usuario cuando arrastramos un Elemento desde la ListView Control que contiene los Tipos de Equipos
                designer2.TipodeEquipo = 1;
                designer2.imagen = imageList2.Images[indiceimagen];
                Action_Add(ElementType.ImageNode);
            }
        }       

        //Evento que se produce cuando Cambia la Selección de un elemento del ListView4
        private void listView4_SelectedIndexChanged(object sender, EventArgs e)
        {
            luis = listView4.SelectedIndices;

            for (int i = 0; i < luis.Count; i++)
            {
                //MessageBox.Show("Número de Elementos: " + Convert.ToString(luis.Count));
                //MessageBox.Show("Indice del Elemento Seleccionado: " + luis[i].ToString());
                indiceimagen = (int)luis[i];
            }
        }

        //Evento que se produce cuando empezamos a Arrastrar un elemento del ListView 
        private void listView4_ItemDrag(object sender, ItemDragEventArgs e)
        {
            this.listView4.DoDragDrop(this.listView4.SelectedIndices, DragDropEffects.Copy);
        }

        //Evento que se produce cuando Arrastramos un elemento del ListView por encima del control ListView
        private void listView4_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        //Drag And Drop in the Designer Control "designer1"
        private void designer1_Load(object sender, EventArgs e)
        {
            designer1.AllowDrop = true;
        }

        //Drag And Drop in the Designer Control "designer2"
        private void designer2_Load(object sender, EventArgs e)
        {
            designer2.AllowDrop = true;
        }

        //Evento que se produce cuando se realiza DragDrop sobre el Control Designer
        private void designer1_DragDrop_1(object sender, DragEventArgs e)
        {
            maria = (ListView.SelectedIndexCollection)e.Data.GetData(typeof(ListView.SelectedIndexCollection));

            for (int i = 0; i < luis.Count; i++)
            {
                //MessageBox.Show("Número de Elementos: " + Convert.ToString(luis.Count));
                //MessageBox.Show("Indice del Elemento Seleccionado: " + luis[i].ToString());
                indiceimagen = (int)luis[i];
            }

            //Llamamos a la función del Menu para crear un Elemento Imagen Nodo
            imageNodeToolStripMenuItem1_Click(sender, e);
        }

        private void designer2_DragDrop(object sender, DragEventArgs e)
        {
            maria = (ListView.SelectedIndexCollection)e.Data.GetData(typeof(ListView.SelectedIndexCollection));

            for (int i = 0; i < luis.Count; i++)
            {
                //MessageBox.Show("Número de Elementos: " + Convert.ToString(luis.Count));
                //MessageBox.Show("Indice del Elemento Seleccionado: " + luis[i].ToString());
                indiceimagen = (int)luis[i];
            }

            //Llamamos a la función del Menu para crear un Elemento Imagen Nodo
            imageNodeToolStripMenuItem1_Click(sender, e);
        }

        private void designer1_DragEnter(object sender, DragEventArgs e)
        {
            //Bitmap b = new Bitmap(imageList2.Images[indiceimagen]);
            //IntPtr ptr = b.GetHicon();
            //Cursor c = new Cursor(ptr);
            //designer1.Cursor = c;

            designer1.dragdropluis = true;

            if (e.Data.GetDataPresent(typeof(ListView.SelectedIndexCollection)))
            {
                e.Effect = DragDropEffects.Copy;
            }

            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void designer2_DragEnter(object sender, DragEventArgs e)
        {
            //Bitmap b = new Bitmap(imageList2.Images[indiceimagen]);
            //IntPtr ptr = b.GetHicon();
            //Cursor c = new Cursor(ptr);
            //designer1.Cursor = c;

            designer2.dragdropluis = true;

            if (e.Data.GetDataPresent(typeof(ListView.SelectedIndexCollection)))
            {
                e.Effect = DragDropEffects.Copy;
            }

            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        //Mostrar el Cuadro de Diálogo de las Tablas de Vapor IAPWS 1997
        private void formulaciónAIPWSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RegionSelection tablasvapor97 = new RegionSelection();
            tablasvapor97.Show();
        }

       
        //Cambiamos el Valor de una Propiedad en el Control PropertyGrid de la Aplicación
        private void propertyGrid10_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage17)
            {
                GridItem elemento = e.ChangedItem;
                string nombre = elemento.Label;

                if (nombre == "ElementDrawingMethod")
                {
                    if (Convert.ToString(elemento.Value) == "FixedSize")
                    {
                        designer1.metododibujar = OpcionDibujo.FixedSize;
                    }
                    else if (Convert.ToString(elemento.Value) == "DragandDropSize")
                    {
                        designer1.metododibujar = OpcionDibujo.DragandDropSize;
                    }

                    //MessageBox.Show(nombre+" :  "+Convert.ToString(elemento.Value));
                }
            }

            else if (tabControl1.SelectedTab == tabPage19)
            {
                GridItem elemento = e.ChangedItem;
                string nombre = elemento.Label;

                if (nombre == "ElementDrawingMethod")
                {
                    if (Convert.ToString(elemento.Value) == "FixedSize")
                    {
                        designer2.metododibujar = OpcionDibujo.FixedSize;
                    }
                    else if (Convert.ToString(elemento.Value) == "DragandDropSize")
                    {
                        designer2.metododibujar = OpcionDibujo.DragandDropSize;
                    }

                    //MessageBox.Show(nombre+" :  "+Convert.ToString(elemento.Value));
                }
            }
        }

        //Dibujar una Turbina de Baja Resultados de Elementos
        private void button32_Click(object sender, EventArgs e)
        {
            designer2.Document.ElementDrawingMethod = OpcionDibujo.DragandDropSize;
            designer2.metododibujar = OpcionDibujo.DragandDropSize;

            Action_Add(ElementType.TurbinaResultadosNode);
        }

        //Dibujar una Turbina de Alta Resultados de Elementos
        private void button31_Click(object sender, EventArgs e)
        {
            designer2.Document.ElementDrawingMethod = OpcionDibujo.DragandDropSize;
            designer2.metododibujar = OpcionDibujo.DragandDropSize;

            Action_Add(ElementType.TurbinaResultadosAltaNode);
        }

        //Dibujar una Bomba Resultados de Elementos
        private void button38_Click(object sender, EventArgs e)
        {
            designer2.Document.ElementDrawingMethod = OpcionDibujo.DragandDropSize;
            designer2.metododibujar = OpcionDibujo.DragandDropSize;

            Action_Add(ElementType.BombaResultadosNode);
        }

        //Ejecutamos en el Menú la opción Drawing SIZE
        private void button27_Click_1(object sender, EventArgs e)
        {
            Action_Size();
        }

        //Ejecutamos el el Menú la opcción CONNECT
        private void button26_Click(object sender, EventArgs e)
        {
            Action_Connect();
        }

        //Dibujar Arco
        private void button40_Click(object sender, EventArgs e)
        {
            designer2.Document.ElementDrawingMethod = OpcionDibujo.DragandDropSize;
            designer2.metododibujar = OpcionDibujo.DragandDropSize;

            Action_Add(ElementType.ArcoResultados);
        }

        //Dibujar Separador Humedad
        private void button39_Click(object sender, EventArgs e)
        {
            designer2.Document.ElementDrawingMethod = OpcionDibujo.DragandDropSize;
            designer2.metododibujar = OpcionDibujo.DragandDropSize;

            Action_Add(ElementType.SeparadorHumedadNode);
        }

        //Dibujar Válvula
        private void button35_Click(object sender, EventArgs e)
        {
            designer2.Document.ElementDrawingMethod = OpcionDibujo.DragandDropSize;
            designer2.metododibujar = OpcionDibujo.DragandDropSize;

            Action_Add(ElementType.ValvulaNode);
        }

        //Dibujar Calentador
        private void button44_Click(object sender, EventArgs e)
        {
            designer2.Document.ElementDrawingMethod = OpcionDibujo.DragandDropSize;
            designer2.metododibujar = OpcionDibujo.DragandDropSize;

            Action_Add(ElementType.CalentadorNode);
        }

        //Dibujar Generador
        private void button43_Click(object sender, EventArgs e)
        {
            designer2.Document.ElementDrawingMethod = OpcionDibujo.DragandDropSize;
            designer2.metododibujar = OpcionDibujo.DragandDropSize;

            Action_Add(ElementType.Generador);
        }

        //Dibujar Condensador
        private void button37_Click(object sender, EventArgs e)
        {
            designer2.Document.ElementDrawingMethod = OpcionDibujo.DragandDropSize;
            designer2.metododibujar = OpcionDibujo.DragandDropSize;

            Action_Add(ElementType.CondensadorNode);
        }

        //Dibujar Condensador Sellos
        private void button42_Click(object sender, EventArgs e)
        {
            designer2.Document.ElementDrawingMethod = OpcionDibujo.DragandDropSize;
            designer2.metododibujar = OpcionDibujo.DragandDropSize;

            Action_Add(ElementType.CondensadorSellosNode);
        }

        //Dibujar Flecha
        private void button33_Click(object sender, EventArgs e)
        {
            designer2.Document.ElementDrawingMethod = OpcionDibujo.DragandDropSize;
            designer2.metododibujar = OpcionDibujo.DragandDropSize;           
        }

        //Dibujar Rectángulo
        private void button48_Click(object sender, EventArgs e)
        {
            designer2.Document.ElementDrawingMethod = OpcionDibujo.DragandDropSize;
            designer2.metododibujar = OpcionDibujo.DragandDropSize;

            Action_Add(ElementType.Rectangulo);
        }

        //Dibujar Círculo
        private void button50_Click(object sender, EventArgs e)
        {
            designer2.Document.ElementDrawingMethod = OpcionDibujo.DragandDropSize;
            designer2.metododibujar = OpcionDibujo.DragandDropSize;

            Action_Add(ElementType.Circulo);
        }

        //Dibujar Rectángulo Redondeado
        private void button45_Click(object sender, EventArgs e)
        {
            designer2.Document.ElementDrawingMethod = OpcionDibujo.DragandDropSize;
            designer2.metododibujar = OpcionDibujo.DragandDropSize;

            Action_Add(ElementType.RectanguloRedondeado);
        }

        //Dibujar Desaireador
        private void button51_Click(object sender, EventArgs e)
        {
            designer2.Document.ElementDrawingMethod = OpcionDibujo.DragandDropSize;
            designer2.metododibujar = OpcionDibujo.DragandDropSize;

            Action_Add(ElementType.DesaireadorNode);
        }

        //Dibujar Linea
        private void button49_Click(object sender, EventArgs e)
        {
            designer2.Document.ElementDrawingMethod = OpcionDibujo.DragandDropSize;
            designer2.metododibujar = OpcionDibujo.DragandDropSize;

            Action_Add(ElementType.Linea);
        }


        //Opción de Menú: Import - Open HBAL Input File (*.dat)
        private void hBALImportdatToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // La variable "archivocargado" nos indica si hemos cargado anteriormente un archivo
            if(archivocargado==1)
            {
                borrarmatrixequipos = 1;
                toolStripMenuItem11_Click(sender,e);
            }

            // Elegimos las pestañas 3 y 5 de los controles tab en la aplicación principal
            // tabControl1.SelectedTab = tabPage3;
            tabControl2.SelectedTab = tabPage5;
            
            // Inicializamos la marca que indica que es necesario leer los parámetros, generar ecucaciones y leer las condiciones iniciales
            marca = 0;            
            
            //-----------------------------------------INICIALIZAMOS VARIABLES de la Aplicación------------------------------------------------------------------------------------------
            
            // Variable que guarda el nombre de la Ruta de los Resultados de HBAL
            rutaresultadoshbal = "";

            // Variable que guarda el Nº de CORRIENTES
            numcorrientes = 0;
            
            // Variable que guarda el Nº de PARÁMETROS
            p.Clear();
            p1.Clear();
            
            // Variable que guarda las FUNCIONES 
            functions.Clear();

            // Variable que guarda las CONDICIONES INICIALES
            for (int g = 0; g < 10000; g++)
            {
                // Condiciones iniciales de CAUDAL
                caudalinicial[g] = 0;
                // Condiciones iniciales de PRESIÓN
                presioninicial[g] = 0;
                // Condiciones iniciales de ENTALPIA
                entalpiainicial[g] = 0;
            }

            // Variable que indica que hemos LEIDAS las CONDICIONES INICIALES
            leidascondicionesiniciales = 0;

            // Variable que guarda el TÍTULO del ARCHIVO
            Titulo = "";
            // Variable que guarda el NOMBRE del ARCHIVO
            NombreArchivo = "";
            // Variable que guarda el Nº EQUIPOS
            NumTotalEquipos = 0;
            // Variable que guarda el Nº CORRIENTES
            NumTotalCorrientes = 0;
            // Variable que guarda el Nº MAX. ITERACIONES
            NumMaxIteraciones = 20;
            // Variable que guarda el Nº TABLAS 
            NumTotalTablas = 0;            
            // Variable que guarda el ERROR MÁXIMO ADMISIBLE
            ErrorMaxAdmisible = 1E-5;
            // Variable que 
            DatosIniciales = 0;
            // Variable que guarda el FACTOR de ITERACIONES (EPS)
            FactorIteraciones = 0.5;
            // Variable del FICHERO de ITERACIONES INTERMEDIAS
            FicheroIteraciones = 1;
            // Variable que indica el tipo de UNIDADES consideradas
            unidades = 2;

            
            numequipos = 0;            
            numecuaciones = 0;            
            numvariables = 0;
            
            ecuaciones.Clear();
            
            // Variable que guarda los datos de Entrada de los EQUIPOS 
            equipos11.Clear();
            
            Hbalfile.Clear();
            
            // Variable que guarda las TABLAS 
            listaTablas.Clear();            
            // Variable que guarda él NÚMERO de TABLAS definido
            listanumTablas.Clear();
            // Variable que guarda 
            listanumDatosenTabla.Clear();
            
            // Inicialización de los CONTROLES de la APLICACIÓN PRINCIPAL
            // Inicialización del control lista en el Tab: "Streams Results"
            listBox1.Items.Clear();
            // 
            listBox2.Items.Clear();
            // Inicialización del control lista en el Tab: "Equipment Results" 
            listBox3.Items.Clear();
            //  
            listBox4.Items.Clear();
            // Inicialización del control lista en el Tab: "List of Equations"
            listBox5.Items.Clear();
            // Inicialización del control lista en el Tab: "Equipment Data Input"
            listBox6.Items.Clear();
            // 
            textBox14.Clear();

            // Inicialización del control lista en el Tab: "Number of Generated Equations", Number of equations equal than number of output variables
            listView1.Clear();
            // Inicialización del control lista en el Tab: "Number of Generated Equations", Number of equations less than number of output variables
            listView2.Clear();
            // Inicialización del control lista en el Tab: "Number of Generated Equations", Number of equations greater than number of output variables
            listView3.Clear();
            
            // Inicialización de los CONTROLES de ARBOL 
            // Inicializa el control arbol en el Tab: "Heat Balance General Information" 
            treeView1.Nodes.Clear();
            // Inicializa el control arbol en el Tab: "Equipment Data Input"
            treeView2.Nodes.Clear();
            // Inicializa el control arbol en el Tab: "Equipment Results"
            treeView3.Nodes.Clear();
            
            //------------------------------------------------------------------------------------------------------------------------------




            //----------------------------------La Clase OpenFileDialog nos facilita un cuadro de diálogo tipo para Abrir Archivos para su Lectura

            // Create new SaveFileDialog object
            OpenFileDialog OpenFileDialog1 = new OpenFileDialog();

            // Default file extension
            OpenFileDialog1.DefaultExt = "dat";

            // Available file extensions
            OpenFileDialog1.Filter = "HBal files (*.dat)|*.dat|All files (*.*)|*.*";

            // Adds a extension if the user does not
            OpenFileDialog1.AddExtension = true;

            // Restores the selected directory, next time
            OpenFileDialog1.RestoreDirectory = true;

            // Dialog title
            OpenFileDialog1.Title = "Diálogo para elegir el archivo de HBAL que queremos abrir.";

            // Startup directory
            OpenFileDialog1.InitialDirectory = @"C:\Users\LUISCOCO\Desktop\Validations Files\Validations Files";

            // Show the dialog and process the result
            if (OpenFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //En el StatusBar de la Aplicación principal mostraremos el directorio y el nombre del Archivo abierto.
                toolStripStatusLabel1.Text = "Working Direcgtory and File: " + OpenFileDialog1.FileName;

                //MessageBox.Show("You selected the file: " + OpenFileDialog .FileName);

                //La Clase StreamReader nos permite leer el contenido de un archivo.
                StreamReader tempfl = new StreamReader(OpenFileDialog1.FileName);
                fl1 = tempfl;

                //Llamamos al cuadro de Diálogo para Leer los datos en el fichero elegido de HBAL (*.DAT)
                //Enviamos al cuadro de Diálogo el puntero fl1 utilizado para lectura del archivo
                LecturaHbal lecturaHbal1 = new LecturaHbal(this, fl1);

                //Eliminamos el objeto OpenFileDialog1 y lo inicializamos con el valor null.
                OpenFileDialog1.Dispose();
                OpenFileDialog1 = null;

                //Mostramos el cuadro de Diálogo "lecturaHbal1"
                lecturaHbal1.Show();
            }

            //En caso de que pulsemos el botón de Cancel o cerremos el diálogo de Open File mostraremos un mensaje "You hit cancel or closed the dialog."
            else
            {
                MessageBox.Show("You hit cancel or closed the dialog.");
                fl1 = null;
                OpenFileDialog1.Dispose();
                OpenFileDialog1 = null;
            }

            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

            //Llamamos a la función de actualizar el control tree "arbol" en la Aplicación Principal con la información del archivo leido.
            actualizararbol();

            archivocargado = 1;
        }
//---------------------------------------------------------------------------------------------------------------------------------------------
        
        //Opción de Menú: Export - Write HBAL Input File (*.dat)
        private void hBALExportdatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Mostramos el cuadro de Diálogo para escribir las CONDICIONES INICIALES
            EscribirarchivoHbal escribir1 = new EscribirarchivoHbal(this);

            if (escribir1.ShowDialog(this) == DialogResult.OK)
            {

            }

            // Create new SaveFileDialog object
            SaveFileDialog SaveFileDialog = new SaveFileDialog();

            // Default file extension
            SaveFileDialog.DefaultExt = "dat";

            // Available file extensions
            SaveFileDialog.Filter = "HBal files (*.dat)|*.dat|All files (*.*)|*.*";

            // Adds a extension if the user does not
            SaveFileDialog.AddExtension = true;

            // Restores the selected directory, next time
            SaveFileDialog.RestoreDirectory = true;

            // Dialog title
            SaveFileDialog.Title = "Where do you want to save the file?";

            // Startup directory
            SaveFileDialog.InitialDirectory = @"C:/";

            // Show the dialog and process the result
            if (SaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                //SaveFileDialog.FileName=NombreArchivo;
                //MessageBox.Show("You selected the file: " + OpenFileDialog .FileName);
                StreamWriter tempfl = new StreamWriter(SaveFileDialog.FileName);
                fl = tempfl;
            }
            else
            {
                MessageBox.Show("You hit cancel or closed the dialog.");
                SaveFileDialog.Dispose();
                SaveFileDialog = null;
                return;
            }

            SaveFileDialog.Dispose();
            SaveFileDialog = null;


            //Hasta aquí hemos inicializado el objeto fl de la clase StreamWriter con el archivo en la ruta elegida.

            //CAMBIO DE UNIDADES  de los PARAMETROS D1 a D9 de los equipos antes de escribir un archivo de entrada de datos de HBAL.dat
            //El programa guarda todos los parámteros de los equipos D1 a D9 en unidades Británicas porque las Tablas ASME 1967 trabajan con unidades británicas

            //Creamos una Lista euipos55, para guardar los equipos con sus parámetros en Unidades del Sistema Internacional
            List<Equipos> equipos55 = new List<Equipos>();
            equipos55 = equipos11;

            for (int h = 0; h < equipos11.Count; h++)
            {

                //Cambio unidades de los equipos Tipo 1: Condicion de Contorno
                if (equipos11[h].tipoequipo2 == 1)
                {
                    equipos55[h].aD1 = equipos11[h].aD1 * 0.4536;
                    equipos55[h].aD2 = equipos11[h].aD2 * (6.8947572 / 100);
                    equipos55[h].aD3 = equipos11[h].aD3 * 2.326009;
                    if (equipos11[h].aD6 > 0)
                    {
                        //Presión de psia a Bar 
                        equipos55[h].aD6 = equipos11[h].aD6 * (6.8947572 / 100);
                    }

                    else if (equipos11[h].aD6 < 0)
                    {
                        //Convertir los grados ºF a ºC 
                        equipos55[h].aD6 = ((equipos11[h].aD6 - 32) * 5) / 9;
                    }
                }

                //Cambio unidades de los equipos Tipo 2: Divisor
                if (equipos11[h].tipoequipo2 == 2)
                {
                    //Caudal de Lb/sg a Kgr/sg
                    equipos55[h].aD1 = equipos11[h].aD1 * (0.4536);
                    //Conversión del Factor de Flujo de Unidades Británicas a Sistema Internacional
                    equipos55[h].aD3 = equipos11[h].aD3 / 2.316749697;
                }


                //Cambio unidades de los equipos Tipo 3:Pérdida de Carga
                if (equipos11[h].tipoequipo2 == 3)
                {
                    //Convertimos de mm a m y de mm a ft
                    equipos55[h].aD6 = (equipos11[h].aD6 * 1000) / 3.28083;
                    //Distancias de ft a m
                    equipos55[h].aD7 = equipos11[h].aD7 / 3.28083;
                    //Factor independiente de pérdida de carga
                    equipos55[h].aD1 = equipos11[h].aD1 * (6.8947572 / 100);
                    //Factor cuadrático de pérdida de carga
                    equipos55[h].aD3 = equipos11[h].aD3 / 2.984193609;
                    //Factor lineal de pérdida de carga
                    equipos55[h].aD2 = equipos11[h].aD2 / 6.578911309;
                }

                //Cambio unidades de los equipos Tipo 4:Bomba
                if (equipos11[h].tipoequipo2 == 4)
                {
                    //Presión psia a Bar
                    equipos55[h].aD5 = equipos11[h].aD5 * (6.8947572 / 100);
                    //Factor independiente de pérdida de carga
                    equipos55[h].aD1 = equipos11[h].aD1 * (6.8947572 / 100);
                    //Factor cuadrático de pérdida de carga
                    equipos55[h].aD3 = equipos11[h].aD3 / 2.984193609;
                    //Factor lineal de pérdida de carga
                    equipos55[h].aD2 = equipos11[h].aD2 / 6.578911309;
                }

                //Cambio unidades de los equipos Tipo 5:Mezclador
                if (equipos11[h].tipoequipo2 == 5)
                {
                    //Presión psia a Bar
                    equipos55[h].aD1 = equipos11[h].aD1 * (6.8947572 / 100);
                }

                //Cambio unidades de los equipos Tipo 6:Reactor
                if (equipos11[h].tipoequipo2 == 6)
                {
                    //Factor independiente de pérdida de carga
                    equipos55[h].aD1 = equipos11[h].aD1 * (6.8947572 / 100);
                    //Factor lineal de pérdida de carga
                    equipos55[h].aD2 = equipos11[h].aD2 / 6.578911309;
                    //Factor cuadrático de pérdida de carga
                    equipos55[h].aD3 = equipos11[h].aD3 / 2.984193609;
                    if (equipos11[h].aD5 > 0)
                    {
                        //Entalpia Btu/Lb a Kj/Kgr
                        equipos55[h].aD5 = equipos11[h].aD5 * 2.326009;
                    }
                    else if (equipos11[h].aD5 < 0)
                    {
                        //Convertir los grados ºC en ºF
                        //D5 = ((D5 * 9) / 5) + 32;
                    }
                }

                //Cambio unidades de los equipos Tipo 7:Calentador
                if (equipos11[h].tipoequipo2 == 7)
                {
                    //Convertir ºF a ºC los AT
                    equipos55[h].aD4 = ((equipos11[h].aD4 * 5.0) / 9);
                    equipos55[h].aD5 = ((equipos11[h].aD5 * 5.0) / 9);
                    //Convertir de psia a Bar
                    equipos55[h].aD1 = equipos11[h].aD1 * (6.8947572 / 100);
                    //Factor cuadrático de pérdida de carga de psia a Bar
                    equipos55[h].aD3 = equipos11[h].aD3 / 2.984193609;
                    //Factor lineal de pérdida de carga de psia a Bar
                    equipos55[h].aD2 = equipos11[h].aD2 / 6.578911309;
                }

                //Cambio unidades de los equipos Tipo 8:Condensador
                if (equipos11[h].tipoequipo2 == 8)
                {
                    //Presión psia a Bar
                    //Pv = Pv * (6.8947572 / 100);
                }

                //Cambio unidades de los equipos Tipo 9:Turbina
                if (equipos11[h].tipoequipo2 == 9)
                {
                    //Conversión del Factor de Flujo de Unidades de Británicas a Métricas
                    equipos55[h].aD3 = equipos11[h].aD3 / 2.316749697;
                }

                //Cambio unidades de los equipos Tipo 10:Turbina
                if (equipos11[h].tipoequipo2 == 10)
                {
                    //Conversión del Factor de Flujo de Unidades de Británicas a Métricas
                    equipos55[h].aD3 = equipos11[h].aD3 / 2.316749697;
                    //Presión psia a Bar
                    equipos55[h].aD2 = equipos11[h].aD2 * (6.8947572 / 100);
                }

                //Cambio unidades de los equipos Tipo 11:Turbina Auxiliar
                if (equipos11[h].tipoequipo2 == 11)
                {
                    //Presión de psia a Bar
                    equipos55[h].aD2 = equipos11[h].aD2 * (6.8947572 / 100);
                }

                //Cambio unidades de los equipos Tipo 13:Separador de Humedad
                if (equipos11[h].tipoequipo2 == 13)
                {
                }

                //Cambio unidades de los equipos Tipo 14:MSR
                if (equipos11[h].tipoequipo2 == 14)
                {
                    //Convertir el TTD de ºF a ºC
                    equipos55[h].aD5 = ((equipos11[h].aD5 * 5) / 9);
                    //Presión de psia a Bar
                    equipos55[h].aD1 = equipos11[h].aD1 * (6.8947572 / 100);
                    equipos55[h].aD7 = equipos11[h].aD7 * (6.8947572 / 100);
                    //Factor cuadrático de pérdida de carga
                    equipos55[h].aD3 = equipos11[h].aD3 / 2.984193609;
                    //Factor lineal de pérdida de carga
                    equipos55[h].aD2 = equipos11[h].aD2 / 6.578911309;
                    //Factor cuadrático de pérdida de carga
                    equipos55[h].aD9 = equipos11[h].aD9 / 2.984193609;
                    //Factor lineal de pérdida de carga
                    equipos55[h].aD8 = equipos11[h].aD8 / 6.578911309;
                }

                //Cambio unidades de los equipos Tipo 15:Condensador
                if (equipos11[h].tipoequipo2 == 15)
                {
                    //Factor independiente de pérdida de carga
                    equipos55[h].aD1 = equipos11[h].aD1 * (6.8947572 / 100);
                    //Factor cuadrático de pérdida de carga
                    equipos55[h].aD3 = equipos11[h].aD3 / 2.984193609;
                    //Factor lineal de pérdida de carga
                    equipos55[h].aD2 = equipos11[h].aD2 / 6.578911309;
                    //Conversión de psia a Bar
                    equipos55[h].aD7 = equipos11[h].aD7 * (6.8947572 / 100);
                }

                //Cambio unidades de los equipos Tipo 19:Válvula
                if (equipos11[h].tipoequipo2 == 19)
                {
                    //Conversión del CV
                    equipos55[h].aD6 = equipos11[h].aD6 * 1.727482106;
                    //Factor independiente de pérdida de carga
                    equipos55[h].aD1 = equipos11[h].aD1 * (6.8947572 / 100);
                    //Factor cuadrático de pérdida de carga
                    equipos55[h].aD3 = equipos11[h].aD3 / 2.984193609;
                    //Factor lineal de pérdida de carga
                    equipos55[h].aD2 = equipos11[h].aD2 / 6.578911309;
                }

                //Cambio unidades de los equipos Tipo 20:Divisor de Entalpía Fija
                if (equipos11[h].tipoequipo2 == 20)
                {
                    //Caudal de Lb/sg a Kgr/sg
                    equipos55[h].aD1 = equipos11[h].aD1 * (0.4536);
                    //Conversión de Entalpía de Británicas a Sistema Internacional (Btu/Lb a Kj/Kgr)
                    equipos55[h].aD2 = equipos11[h].aD2 * 2.326009;
                    //Conversión de Presión de Británicas a Sistema Internacional (psia a Bar)
                    equipos55[h].aD3 = equipos11[h].aD3 * (6.8947572 / 100);
                }

                //Cambio unidades de los equipos Tipo 21:Tanque Evaporizador
                if (equipos11[h].tipoequipo2 == 21)
                {
                    //Presión Bar a psia
                    equipos55[h].aD1 = equipos11[h].aD1 * (6.8947572 / 100);
                }

            }


            //Escritura de Archivo de HBAL *.DAT
            // ATENTO MODIFICAR equipos11 por equipos55 en todas las llamadas dentro del siguiente Bucle

            Hbalfile.Clear();

            //Linea PRIMERA del archivo: TITULO del archivo
            String Titulodelarchivo;
            Titulodelarchivo = Titulo;
            Hbalfile.Add(Titulodelarchivo);

            //Linea SEGUNDA del archivo:

            //Número Total de Equipos
            if (NumTotalEquipos == 0)
            {
                NumTotalEquipos = numequipos;
            }

            Double longitud155 = Convert.ToString(NumTotalEquipos).Length; ;
            String temporal155 = "";

            for (int hh = 0; hh < 5 - longitud155; hh++)
            {
                temporal155 = temporal155 + " ";
            }

            String equiposcadena = String.Concat(temporal155, Convert.ToString(NumTotalEquipos));

            //Número Total de Corrientes
            if (NumTotalCorrientes == 0)
            {
                NumTotalCorrientes = numcorrientes;
            }

            Double longitud156 = Convert.ToString(NumTotalCorrientes).Length; ;
            String temporal156 = "";

            for (int hh = 0; hh < 5 - longitud156; hh++)
            {
                temporal156 = temporal156 + " ";
            }

            String corrientescadena = String.Concat(temporal156, Convert.ToString(NumTotalCorrientes));

            //Número Máximo de Iteraciones
            Double longitud157 = Convert.ToString(NumMaxIteraciones).Length; ;
            String temporal157 = "";

            for (int hh = 0; hh < 5 - longitud157; hh++)
            {
                temporal157 = temporal157 + " ";
            }

            String iteracionescadena = String.Concat(temporal157, Convert.ToString(NumMaxIteraciones));

            //Número Máximo de Tablas
            Double longitud158 = Convert.ToString(NumTotalTablas).Length; ;
            String temporal158 = "";

            for (int hh = 0; hh < 5 - longitud158; hh++)
            {
                temporal158 = temporal158 + " ";
            }

            String tablascadena = String.Concat(temporal158, Convert.ToString(NumTotalTablas));

            //Error Máximo Admisible
            Double longitud159 = Convert.ToString(ErrorMaxAdmisible).Length; ;
            String temporal159 = "";

            for (int hh = 0; hh < 10 - longitud159; hh++)
            {
                temporal159 = temporal159 + " ";
            }

            String errormaximocadena = String.Concat(temporal159, Convert.ToString(ErrorMaxAdmisible));

            //Datos iniciales bueno (1 si, 0 no)
            Double longitud160 = Convert.ToString(DatosIniciales).Length; ;
            String temporal160 = "";

            for (int hh = 0; hh < 5 - longitud160; hh++)
            {
                temporal160 = temporal160 + " ";
            }

            String datosinicialescadena = String.Concat(temporal160, Convert.ToString(DatosIniciales));

            //????
            Double longitud161 = Convert.ToString(0).Length; ;
            String temporal161 = "";

            for (int hh = 0; hh < 5 - longitud161; hh++)
            {
                temporal161 = temporal161 + " ";
            }

            String pendientecadena = String.Concat(temporal161, Convert.ToString(0));

            //Factor de control de iteraciones (0,5 por defecto)
            Double longitud162 = Convert.ToString(FactorIteraciones).Length; ;
            String temporal162 = "";

            for (int hh = 0; hh < 5 - longitud162; hh++)
            {
                temporal162 = temporal162 + " ";
            }

            String factoriteracionescadena = String.Concat(temporal162, Convert.ToString(FactorIteraciones));

            //Fichero de iteraciones intermedias (1 si, 0 no)
            Double longitud163 = Convert.ToString(FicheroIteraciones).Length; ;
            String temporal163 = "";

            for (int hh = 0; hh < 5 - longitud163; hh++)
            {
                temporal163 = temporal163 + " ";
            }

            String interacionescadena = String.Concat(temporal163, Convert.ToString(FicheroIteraciones));

            //Unidades (1 Britanicas, 2 Métricas, 3 Sistema Internacional)
            Double longitud164 = Convert.ToString(unidades).Length; ;
            String temporal164 = "";

            for (int hh = 0; hh < 29 - longitud164; hh++)
            {
                temporal164 = temporal164 + " ";
            }

            // Hbal Británicas=1: Mi programa Británicas=0
            // Hbal Métricas=2: Mi programa Sistema Métrico=1
            // Hbal S.I.=3: Mi programa Sistema Internacional=2
            Double unidadestemp = 0;
            if (unidades == 0)
            {
                unidadestemp = 1;
            }

            else if (unidades == 1)
            {
                unidadestemp = 2;
            }

            else if (unidades == 2)
            {
                unidadestemp = 3;
            }

            String unidadescadena = String.Concat(temporal164, Convert.ToString(unidadestemp));


            String segundalinea = equiposcadena + corrientescadena + iteracionescadena + tablascadena + errormaximocadena + datosinicialescadena + pendientecadena + factoriteracionescadena + interacionescadena + unidadescadena;
            Hbalfile.Add(segundalinea);

            //Escribimos el resto de LINEAS del archivo
            for (int j = 0; j < equipos55.Count; j++)
            {


                //Creamos las dos líneas del archivo de entrada de HBAL
                //Enviamos las dos líneas del archivo HBAL de entrada de datos generada por el equipo Condición de Contorno que hemos creado
                //punteroaplicacion1.Hbalfile.Add("");
                //punteroaplicacion1.Hbalfile[(punteroaplicacion1.numequipos - 1)] = lineaprimera;
                //punteroaplicacion1.Hbalfile.Add("");
                //punteroaplicacion1.Hbalfile[(punteroaplicacion1.numequipos)] = lineasegunda;


                //Número de Equipo
                Double longitud = Convert.ToString(equipos55[j].numequipo2).Length;
                String temporal = "";

                for (int hh = 0; hh < 10 - longitud; hh++)
                {
                    temporal = temporal + " ";
                }

                String primerasubcadena = String.Concat(temporal, Convert.ToString(equipos55[j].numequipo2));
                //String primerasubcadena= temporal+Convert.ToString(numequipo);
                Int32 comprobacion = primerasubcadena.Length;


                //Tipo de Equipo
                Double longitud1 = Convert.ToString(equipos55[j].tipoequipo2).Length; ;
                String temporal1 = "";

                for (int hh = 0; hh < 10 - longitud1; hh++)
                {
                    temporal1 = temporal1 + " ";
                }

                String segundasubcadena = String.Concat(temporal1, Convert.ToString(equipos55[j].tipoequipo2));
                //String segundasubcadena = temporal1 +"1";
                Int32 comprobacion1 = segundasubcadena.Length;


                //Corriente 1
                Double longitud2 = Convert.ToString(equipos55[j].aN1).Length;
                String temporal2 = "";

                for (int hh = 0; hh < 10 - longitud2; hh++)
                {
                    temporal2 = temporal2 + " ";
                }

                String tercerasubcadena = String.Concat(temporal2, Convert.ToString(equipos55[j].aN1));
                //String tercerasubcadena = temporal2 + Convert.ToString(correntrada);
                Int32 comprobacion2 = tercerasubcadena.Length;


                //Corriente 2
                Double longitud3 = Convert.ToString(equipos55[j].aN2).Length; ;
                String temporal3 = "";

                for (int hh = 0; hh < 10 - longitud3; hh++)
                {
                    temporal3 = temporal3 + " ";
                }

                String cuartasubcadena = String.Concat(temporal3, Convert.ToString(equipos55[j].aN2));
                //String cuartasubcadena = temporal3 + Convert.ToString(0);
                Int32 comprobacion3 = cuartasubcadena.Length;


                //Corriente 3
                Double longitud4 = Convert.ToString(equipos55[j].aN3).Length;
                String temporal4 = "";

                for (int hh = 0; hh < 10 - longitud4; hh++)
                {
                    temporal4 = temporal4 + " ";
                }

                String quintasubcadena = String.Concat(temporal4, Convert.ToString(equipos55[j].aN3));
                //String quintasubcadena = temporal4 + Convert.ToString(corrsalida);
                Int32 comprobacion4 = quintasubcadena.Length;


                //Corriente 4
                Double longitud5 = Convert.ToString(equipos55[j].aN4).Length;
                String temporal5 = "";

                for (int hh = 0; hh < 10 - longitud5; hh++)
                {
                    temporal5 = temporal5 + " ";
                }

                String sextasubcadena = String.Concat(temporal5, Convert.ToString(equipos55[j].aN4));
                //String sextasubcadena = temporal5 + Convert.ToString(0);
                Int32 comprobacion5 = sextasubcadena.Length;

                //Indice de Dibujo
                String septimasubcadena = "    0";

                String lineaprimera;
                //Linea PRIMERA del archivo de HBAL incluyendo NºEquipo, Tipo Equipo, corriente N1,corriente N2, corriente N3, corriente N4 e indice del Dibujo
                lineaprimera = String.Concat(primerasubcadena, segundasubcadena, tercerasubcadena, cuartasubcadena, quintasubcadena, sextasubcadena, septimasubcadena);


                //Igual realizaremos la entrada de parameteros D1 hasta D9, teniendo en cuenta que cada uno de los campos ocupa en el archivo 8 caracteres en lugar de 10 caracteres por dato como ocupaba la linea anterior del archivo 
                //Codigo para escribir la "lineasegunda" para el envio al archivo de entrada de HBAL incluyendo los parámetros D1 al D9 del equipo condición de contorno generado.

                //D1
                Double longitud6 = 0;
                if ((equipos55[j].aD1 < 0.001) && (equipos55[j].aD1 != 0))
                {
                    longitud6 = equipos55[j].aD1.ToString("0.##e+0", CultureInfo.InvariantCulture).Length;
                }

                else if ((equipos55[j].aD1 < -0.001) && (equipos55[j].aD1 != 0))
                {
                    longitud6 = equipos55[j].aD1.ToString("0.##e+0", CultureInfo.InvariantCulture).Length;
                }

                else
                {
                    longitud6 = Convert.ToString(equipos55[j].aD1).Length;
                }

                String temporal6 = "";

                for (int hh = 0; hh < 8 - longitud6; hh++)
                {
                    temporal6 = temporal6 + " ";
                }

                String octavasubcadena = "";

                if ((equipos55[j].aD1 < 0.001) && (equipos55[j].aD1 != 0))
                {
                    octavasubcadena = String.Concat(equipos55[j].aD1.ToString("0.##e+0", CultureInfo.InvariantCulture), temporal6);
                }

                else if ((equipos55[j].aD1 < -0.001) && (equipos55[j].aD1 != 0))
                {
                    octavasubcadena = String.Concat(equipos55[j].aD1.ToString("0.##e+0", CultureInfo.InvariantCulture), temporal6);
                }

                else
                {
                    octavasubcadena = String.Concat(Convert.ToString(equipos55[j].aD1), temporal6);
                }

                Int32 comprobacion6 = octavasubcadena.Length;

                //D2
                Double longitud7 = 0;
                if ((equipos55[j].aD2 < 0.001) && (equipos55[j].aD2 != 0))
                {
                    longitud7 = equipos55[j].aD2.ToString("0.##e+0", CultureInfo.InvariantCulture).Length;
                }

                else if ((equipos55[j].aD2 < -0.001) && (equipos55[j].aD2 != 0))
                {
                    longitud7 = equipos55[j].aD2.ToString("0.##e+0", CultureInfo.InvariantCulture).Length;
                }

                else
                {
                    longitud7 = Convert.ToString(equipos55[j].aD2).Length;
                }

                String temporal7 = "";

                for (int hh = 0; hh < 8 - longitud7; hh++)
                {
                    temporal7 = temporal7 + " ";
                }

                String novenasubcadena = "";

                if ((equipos55[j].aD2 < 0.001) && (equipos55[j].aD2 != 0))
                {
                    novenasubcadena = String.Concat(equipos55[j].aD2.ToString("0.##e+0", CultureInfo.InvariantCulture), temporal7);
                }

                else if ((equipos55[j].aD2 < -0.001) && (equipos55[j].aD2 != 0))
                {
                    novenasubcadena = String.Concat(equipos55[j].aD2.ToString("0.##e+0", CultureInfo.InvariantCulture), temporal7);
                }

                else
                {
                    novenasubcadena = String.Concat(Convert.ToString(equipos55[j].aD2), temporal7);
                }

                Int32 comprobacion7 = novenasubcadena.Length;


                //D3
                Double longitud8 = 0;
                if ((equipos55[j].aD3 < 0.001) && (equipos55[j].aD3 != 0))
                {
                    longitud8 = equipos55[j].aD3.ToString("0.##e+0", CultureInfo.InvariantCulture).Length;
                }

                else if ((equipos55[j].aD3 < -0.001) && (equipos55[j].aD3 != 0))
                {
                    longitud8 = equipos55[j].aD3.ToString("0.##e+0", CultureInfo.InvariantCulture).Length;
                }

                else
                {
                    longitud8 = Convert.ToString(equipos55[j].aD3).Length;
                }

                String temporal8 = "";

                for (int hh = 0; hh < 8 - longitud8; hh++)
                {
                    temporal8 = temporal8 + " ";
                }

                String decimasubcadena = "";

                if ((equipos55[j].aD3 < 0.001) && (equipos55[j].aD3 != 0))
                {
                    decimasubcadena = String.Concat(equipos55[j].aD3.ToString("0.##e+0", CultureInfo.InvariantCulture), temporal8);
                }

                else if ((equipos55[j].aD3 < -0.001) && (equipos55[j].aD3 != 0))
                {
                    decimasubcadena = String.Concat(equipos55[j].aD3.ToString("0.##e+0", CultureInfo.InvariantCulture), temporal8);
                }

                else
                {
                    decimasubcadena = String.Concat(Convert.ToString(equipos55[j].aD3), temporal8);
                }

                Int32 comprobacion8 = decimasubcadena.Length;


                //D4
                Double longitud9 = 0;
                if ((equipos55[j].aD4 < 0.001) && (equipos55[j].aD4 != 0))
                {
                    longitud9 = equipos55[j].aD4.ToString("0.##e+0", CultureInfo.InvariantCulture).Length;
                }

                else if ((equipos55[j].aD4 < -0.001) && (equipos55[j].aD4 != 0))
                {
                    longitud9 = equipos55[j].aD4.ToString("0.##e+0", CultureInfo.InvariantCulture).Length;
                }

                else
                {
                    longitud9 = Convert.ToString(equipos55[j].aD4).Length;
                }

                String temporal9 = "";

                for (int hh = 0; hh < 8 - longitud9; hh++)
                {
                    temporal9 = temporal9 + " ";
                }

                String undecimasubcadena = "";

                if ((equipos55[j].aD4 < 0.001) && (equipos55[j].aD4 != 0))
                {
                    undecimasubcadena = String.Concat(equipos55[j].aD4.ToString("0.##e+0", CultureInfo.InvariantCulture), temporal9);
                }

                else if ((equipos55[j].aD4 < -0.001) && (equipos55[j].aD4 != 0))
                {
                    undecimasubcadena = String.Concat(equipos55[j].aD4.ToString("0.##e+0", CultureInfo.InvariantCulture), temporal9);
                }

                else
                {
                    undecimasubcadena = String.Concat(Convert.ToString(equipos55[j].aD4), temporal9);
                }

                Int32 comprobacion9 = undecimasubcadena.Length;


                //D5
                Double longitud10 = 0;
                if ((equipos55[j].aD5 < 0.001) && (equipos55[j].aD5 != 0))
                {
                    longitud10 = equipos55[j].aD5.ToString("0.##e+0", CultureInfo.InvariantCulture).Length;
                }

                else if ((equipos55[j].aD5 < -0.001) && (equipos55[j].aD5 != 0))
                {
                    longitud10 = equipos55[j].aD5.ToString("0.##e+0", CultureInfo.InvariantCulture).Length;
                }

                else
                {
                    longitud10 = Convert.ToString(equipos55[j].aD5).Length;
                }

                String temporal10 = "";

                for (int hh = 0; hh < 8 - longitud10; hh++)
                {
                    temporal10 = temporal10 + " ";
                }

                String duodecimasubcadena = "";

                if ((equipos55[j].aD5 < 0.001) && (equipos55[j].aD5 != 0))
                {
                    duodecimasubcadena = String.Concat(equipos55[j].aD5.ToString("0.##e+0", CultureInfo.InvariantCulture), temporal10);
                }

                else if ((equipos55[j].aD5 < -0.001) && (equipos55[j].aD5 != 0))
                {
                    duodecimasubcadena = String.Concat(equipos55[j].aD5.ToString("0.##e+0", CultureInfo.InvariantCulture), temporal10);
                }

                else
                {
                    duodecimasubcadena = String.Concat(Convert.ToString(equipos55[j].aD5), temporal10);
                }
                Int32 comprobacion10 = duodecimasubcadena.Length;

                //D6
                Double longitud11 = 0;
                if ((equipos55[j].aD6 < 0.001) && (equipos55[j].aD6 != 0))
                {
                    longitud11 = equipos55[j].aD6.ToString("0.##e+0", CultureInfo.InvariantCulture).Length;
                }

                else if ((equipos55[j].aD6 < -0.001) && (equipos55[j].aD6 != 0))
                {
                    longitud11 = equipos55[j].aD6.ToString("0.##e+0", CultureInfo.InvariantCulture).Length;
                }

                else
                {
                    longitud11 = Convert.ToString(equipos55[j].aD6).Length;
                }

                String temporal11 = "";

                for (int hh = 0; hh < 8 - longitud11; hh++)
                {
                    temporal11 = temporal11 + " ";
                }

                String trecesubcadena = "";

                if ((equipos55[j].aD6 < 0.001) && (equipos55[j].aD6 != 0))
                {
                    trecesubcadena = String.Concat(equipos55[j].aD6.ToString("0.##e+0", CultureInfo.InvariantCulture), temporal11);
                }

                else if ((equipos55[j].aD6 < -0.001) && (equipos55[j].aD6 != 0))
                {
                    trecesubcadena = String.Concat(equipos55[j].aD6.ToString("0.##e+0", CultureInfo.InvariantCulture), temporal11);
                }

                else
                {
                    trecesubcadena = String.Concat(Convert.ToString(equipos55[j].aD6), temporal11);
                }

                Int32 comprobacion11 = trecesubcadena.Length;

                //D7
                Double longitud12 = 0;
                if ((equipos55[j].aD7 < 0.001) && (equipos55[j].aD7 != 0))
                {
                    longitud12 = equipos55[j].aD7.ToString("0.##e+0", CultureInfo.InvariantCulture).Length;
                }

                else if ((equipos55[j].aD7 < -0.001) && (equipos55[j].aD7 != 0))
                {
                    longitud12 = equipos55[j].aD7.ToString("0.##e+0", CultureInfo.InvariantCulture).Length;
                }

                else
                {
                    longitud12 = Convert.ToString(equipos55[j].aD7).Length;
                }

                String temporal12 = "";

                for (int hh = 0; hh < 8 - longitud12; hh++)
                {
                    temporal12 = temporal12 + " ";
                }

                String catorcesubcadena = "";

                if ((equipos55[j].aD7 < 0.001) && (equipos55[j].aD7 != 0))
                {
                    catorcesubcadena = String.Concat(equipos55[j].aD7.ToString("0.##e+0", CultureInfo.InvariantCulture), temporal12);
                }

                else if ((equipos55[j].aD7 < -0.001) && (equipos55[j].aD7 != 0))
                {
                    catorcesubcadena = String.Concat(equipos55[j].aD7.ToString("0.##e+0", CultureInfo.InvariantCulture), temporal12);
                }

                else
                {
                    catorcesubcadena = String.Concat(Convert.ToString(equipos55[j].aD7), temporal12);

                }

                Int32 comprobacion12 = catorcesubcadena.Length;

                //D8
                Double longitud13 = 0;
                if ((equipos55[j].aD8 < 0.001) && (equipos55[j].aD8 != 0))
                {
                    longitud13 = equipos55[j].aD8.ToString("0.##e+0", CultureInfo.InvariantCulture).Length;
                }

                else if ((equipos55[j].aD8 < -0.001) && (equipos55[j].aD8 != 0))
                {
                    longitud13 = equipos55[j].aD8.ToString("0.##e+0", CultureInfo.InvariantCulture).Length;
                }

                else
                {
                    longitud13 = Convert.ToString(equipos55[j].aD8).Length;
                }

                String temporal13 = "";

                for (int hh = 0; hh < 8 - longitud13; hh++)
                {
                    temporal13 = temporal13 + " ";
                }

                String quincesubcadena = "";

                if ((equipos55[j].aD8 < 0.001) && (equipos55[j].aD8 != 0))
                {
                    quincesubcadena = String.Concat(equipos55[j].aD8.ToString("0.##e+0", CultureInfo.InvariantCulture), temporal13);
                }

                else if ((equipos55[j].aD8 < -0.001) && (equipos55[j].aD8 != 0))
                {
                    quincesubcadena = String.Concat(equipos55[j].aD8.ToString("0.##e+0", CultureInfo.InvariantCulture), temporal13);
                }

                else
                {
                    quincesubcadena = String.Concat(Convert.ToString(equipos55[j].aD8), temporal13);
                }

                Int32 comprobacion13 = quincesubcadena.Length;

                //D9
                Double longitud14 = 0;
                if ((equipos55[j].aD9 < 0.001) && (equipos55[j].aD9 != 0))
                {
                    longitud14 = equipos55[j].aD9.ToString("0.##e+0", CultureInfo.InvariantCulture).Length;
                }

                else if ((equipos55[j].aD9 < -0.001) && (equipos55[j].aD9 != 0))
                {
                    longitud14 = equipos55[j].aD9.ToString("0.##e+0", CultureInfo.InvariantCulture).Length;
                }

                else
                {
                    longitud14 = Convert.ToString(equipos55[j].aD9).Length;
                }

                String temporal14 = "";

                for (int hh = 0; hh < 8 - longitud14; hh++)
                {
                    temporal14 = temporal14 + " ";
                }

                String dieciseissubcadena = "";

                if ((equipos55[j].aD9 < 0.001) && (equipos55[j].aD9 != 0))
                {
                    dieciseissubcadena = String.Concat(equipos55[j].aD9.ToString("0.##e+0", CultureInfo.InvariantCulture), temporal14);
                }

                else if ((equipos55[j].aD9 < -0.001) && (equipos55[j].aD9 != 0))
                {
                    dieciseissubcadena = String.Concat(equipos55[j].aD9.ToString("0.##e+0", CultureInfo.InvariantCulture), temporal14);
                }

                else
                {
                    dieciseissubcadena = String.Concat(Convert.ToString(equipos55[j].aD9), temporal14);
                }

                Int32 comprobacion14 = dieciseissubcadena.Length;

                String lineasegunda;
                //Linea SEGUNDA del archivo de HBAL incluyendo los datos del equipo: D1, D2, D3, D4, D5, D6, D7, D8, D9
                lineasegunda = String.Concat(octavasubcadena, novenasubcadena, decimasubcadena, undecimasubcadena, duodecimasubcadena, trecesubcadena, catorcesubcadena, quincesubcadena, dieciseissubcadena);
                //Enviamos a la aplicación principal los datos de entrada del equipo generado.

                //Enviamos a la Aplicacion principal las lineas DOS del archivo HBAL generadas por el equipo Condición de Contorno
                Hbalfile.Add(lineaprimera);
                Hbalfile.Add(lineasegunda);
            }

            //Escribimos en el archivo de Datos de HBAL *.dat las CONDICIONES INICIALES
            int conta = 0;
            int conta1 = 1;
            int conta2 = 2;

            //Opción para incluir en el archivo de Datos de HBAL *.dat las CONDICIONES INICIALES
            // Si incluirlas = 1; No incluirlas = 0
            if (incluircondicionesiniciales == 1)
            {

                for (int i = 0; i < (numcondiciniciales - 1); i++)
                {
                    //Caudal Inicial

                    //Las condiciones iniciales hay que escribirlas en el archivo de Datos de HBAL *.dat en las unidades elegidas y por defecto el programa las expresa en Unidades Británicas
                    Double caudalunidades = 0;

                    //Unidades del Sistema Internacional
                    if (unidades == 2)
                    {
                        //Caudal de Lb/sg a Kgr/Sg
                        caudalunidades = p[i + conta].Value * 0.4536;
                    }

                    //Unidades del Sistema Métrico
                    else if (unidades == 1)
                    {
                        //Caudal de Lb/sg a Kgr/Sg
                        caudalunidades = p[i + conta].Value;
                    }

                    //Unidades del Sistema Británico
                    else if (unidades == 0)
                    {
                        caudalunidades = p[i + conta].Value;
                    }

                    Double longitud333 = 0;

                    longitud333 = caudalunidades.ToString("0.#######E+00", CultureInfo.InvariantCulture).Length;

                    String temporal333 = "";

                    for (int hh = 0; hh < 15 - longitud333; hh++)
                    {
                        temporal333 = temporal333 + " ";
                    }

                    String caudalinicial = "";


                    caudalinicial = String.Concat(temporal333, caudalunidades.ToString("0.#######E+00", CultureInfo.InvariantCulture));


                    //Presión Inicial

                    //Las condiciones iniciales hay que escribirlas en el archivo de Datos de HBAL *.dat en las unidades elegidas y por defecto el programa las expresa en Unidades Británicas
                    Double presionunidades = 0;

                    //Unidades del Sistema Internacional
                    if (unidades == 2)
                    {
                        //Caudal de psia a Bar
                        presionunidades = p[i + conta1].Value * (6.8947572 / 100);
                    }

                    //Unidades del Sistema Métrico
                    else if (unidades == 1)
                    {
                        //Caudal de psia a kPa
                        presionunidades = p[i + conta1].Value * 6.8947572;
                    }

                    //Unidades del Sistema Británico
                    else if (unidades == 0)
                    {
                        presionunidades = p[i + conta1].Value;
                    }

                    Double longitud334 = 0;

                    longitud334 = presionunidades.ToString("0.#######E+00", CultureInfo.InvariantCulture).Length;

                    String temporal334 = "";

                    for (int hh = 0; hh < 15 - longitud334; hh++)
                    {
                        temporal334 = temporal334 + " ";
                    }

                    String presioninicial = "";

                    presioninicial = String.Concat(temporal334, presionunidades.ToString("0.#######E+00", CultureInfo.InvariantCulture));


                    //Entalpia Inicial

                    Double entalpiaunidades = 0;

                    //Unidades del Sistema Internacional
                    if (unidades == 2)
                    {
                        //Caudal de BTU/Lb a Kj/Kgr
                        entalpiaunidades = p[i + conta2].Value * 2.326009;
                    }

                    //Unidades del Sistema Métrico
                    else if (unidades == 1)
                    {
                        //Caudal de BTU/Lb a Kj/Kgr
                        entalpiaunidades = p[i + conta2].Value * 2.326009;
                    }

                    //Unidades del Sistema Británico
                    else if (unidades == 0)
                    {
                        entalpiaunidades = p[i + conta2].Value;
                    }

                    Double longitud335 = 0;

                    longitud335 = entalpiaunidades.ToString("0.#######E+00", CultureInfo.InvariantCulture).Length;

                    String temporal335 = "";

                    for (int hh = 0; hh < 15 - longitud335; hh++)
                    {
                        temporal335 = temporal335 + " ";
                    }

                    String entalpiainicial = "";


                    entalpiainicial = String.Concat(temporal335, entalpiaunidades.ToString("0.#######E+00", CultureInfo.InvariantCulture));


                    String Nombrecorriente = p[i + conta].Nombre;
                    int longitud77 = Nombrecorriente.Length;
                    String Numcorriente = Nombrecorriente.Substring(1, longitud77 - 1);
                    Double longitud = Numcorriente.Length;
                    String temporal339 = "";

                    for (int hh = 0; hh < 15 - longitud; hh++)
                    {
                        temporal339 = temporal339 + " ";
                    }

                    String Numcorrientefinal = String.Concat(temporal339, Numcorriente);

                    conta = conta + 2;
                    conta1 = conta1 + 2;
                    conta2 = conta2 + 2;

                    String lineacondicion = caudalinicial + presioninicial + entalpiainicial + Numcorrientefinal;
                    Hbalfile.Add(lineacondicion);
                }

            }


            //Escribir las TABLAS en el archivo de entrada de Datos de HBAL (*.DAT)
            Double numtablas = listaTablas.Count;


            if (numtablas == 0)
            {
                MessageBox.Show("No se han almacenado Tablas en los datos de entrada.");
            }

            else if (numtablas != 0)
            {
                MessageBox.Show("Se han almacenado " + Convert.ToString(numtablas) + " Tablas en los datos de entrada.");
            }

            //Double numdatosencadatabla[i]=listaTablas[i].Length;

            //Si Incluir las CONDICIONES INICIALES
            if (incluircondicionesiniciales == 1)
            {
                //Ahora escribimos en el archivo la información introducida en la lista de cadenas Hbalfile
                for (int j = 0; j < (numequipos * 2) + 2 + (numcondiciniciales - 1); j++)
                {
                    fl.WriteLine(Hbalfile[j]);
                }
            }

            //No Incluir las CONDICIONES INICIALES
            else if (incluircondicionesiniciales == 0)
            {
                //Ahora escribimos en el archivo la información introducida en la lista de cadenas Hbalfile
                for (int j = 0; j < (numequipos * 2) + 2; j++)
                {
                    fl.WriteLine(Hbalfile[j]);
                }
            }

            fl.Close();

            MessageBox.Show("Archivo de Entrada de Datos de HBAL generado con éxito. Saludo LUIS COCO");
        }


        //Opción de Menú: Open HBAL Result File (*.res)
        private void hBALOpenresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Resultadoshbal resultadoshbal1 = new Resultadoshbal(this);
            resultadoshbal1.Show();
        }

        //Ayuda Numerical Methods: Newton Raphson - Broyden
        private void numericalMethodHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExplicacionMetodoCalculo exp1 = new ExplicacionMetodoCalculo();
            exp1.Show();
        }

        //Ayuda Numerical Methods: Factor Iteraciones Intermedias (EPS)
        private void iterationFactorEPSHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExplicacionFactorIteraciones exp2 = new ExplicacionFactorIteraciones();
            exp2.Show();
        }

        //Ejemplos de validación DotNumerics Library
        private void dotNumericsLibraryValidationExamplesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Prueba luis = new Prueba();
            luis.Show();
        }

        //Ejemplo de validación de Compilación C# en tiempo real
        private void runtimeCompilingCExampleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormMain prueba = new FormMain(this);
            dialogoejemplos = prueba.enviarpuntero();
            prueba.Show();
        }


        //Cuadro Dialogo sobre este Software
        private void aboutThisSoftwareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about1 = new About();
            about1.Show();
        }

        //Ayuda Water Steam Tables IAPWS IF97
        private void waterSteamTablesIAWPSIFC97HelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("C:\\Users\\LUISCOCO\\Desktop\\Bal LUIS\\Bal LUIS\\Bal LUIS\\Bal LUIS COCO\\IAPWS IF97.pdf");
        }

        //Validación del Motor de Cálculo
        private void solvingEngineValidationToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Ejemplos prueba1 = new Ejemplos(this, numecuaciones, numvariables);
            prueba1.Show();
        }


        //Opción del Menú Open Heat Balance Solution, es decir vamos a abrir un archivo *.dat (Heat Balace Data Input) y otro *.bmp (Heat Balance Scheme)
        private void openHeatBalanceSolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Elegimos las pestañas 5 y 1 de los controles Tab de la Aplicación principal
            //Heat Balance Scheme
            tabControl1.SelectedTab = tabPage1;
            //Heat Balance General Information
            tabControl2.SelectedTab = tabPage5;


            //Inicializamos la marca que indica que es necesario leer los parámetros, generar ecucaciones y leer las condiciones iniciales
            marca = 0;

            //Inicializamos todas las Variables de la Aplicación

            rutaresultadoshbal = "";

            numcorrientes = 0;
            p.Clear();
            p1.Clear();
            functions.Clear();
            for (int g = 0; g < 10000; g++)
            {
                caudalinicial[g] = 0;
                presioninicial[g] = 0;
                entalpiainicial[g] = 0;
            }
            leidascondicionesiniciales = 0;

            Titulo = "";
            NombreArchivo = "";
            NumTotalEquipos = 0;
            NumTotalCorrientes = 0;
            NumMaxIteraciones = 20;
            NumTotalTablas = 0;
            ErrorMaxAdmisible = 1E-5;
            DatosIniciales = 0;
            FactorIteraciones = 0.5;
            FicheroIteraciones = 1;
            unidades = 2;

            numequipos = 0;
            numecuaciones = 0;
            numvariables = 0;
            ecuaciones.Clear();
            equipos11.Clear();
            Hbalfile.Clear();
            listaTablas.Clear();
            listanumTablas.Clear();
            listanumDatosenTabla.Clear();

            listBox1.Items.Clear();
            listBox2.Items.Clear();
            textBox14.Clear();
            listBox4.Items.Clear();
            listBox5.Items.Clear();

            listView1.Clear();
            listView2.Clear();
            listView3.Clear();

            // Clear the TreeView each time the method is called.
            treeView1.Nodes.Clear();
            treeView2.Nodes.Clear();
            treeView3.Nodes.Clear();

            //------------------------------------------------------------------------------------------------------------------------------

            //PRIMERO: Abrimos el archivo de la Heat Balance Solution *.bal donde guardamos los path de los archivo *.dat y *.bmp
            // Create new OpenFileDialog object
            OpenFileDialog OpenFileDialog2 = new OpenFileDialog();
            string line;
            List<String> lineas = new List<String>();
            int numerolineasarchivo = 0;

            // Default file extension
            OpenFileDialog2.DefaultExt = "bal";

            // Available file extensions
            OpenFileDialog2.Filter = "APLICATION files (*.bal)|*.bal|All files (*.*)|*.*";

            // Adds a extension if the user does not
            OpenFileDialog2.AddExtension = true;

            // Restores the selected directory, next time
            OpenFileDialog2.RestoreDirectory = true;

            // Dialog title
            OpenFileDialog2.Title = "Which Heat Balance Solution would you like to open?";

            // Startup directory
            OpenFileDialog2.InitialDirectory = @"C:/";
           
             // Show the dialog and process the result
            if (OpenFileDialog2.ShowDialog() == DialogResult.OK)
            {
                //En el StatusBar de la Aplicación principal mostraremos el directorio y el nombre del Archivo abierto.
                toolStripStatusLabel1.Text = "Working Direcgtory and File: " + OpenFileDialog2.FileName;

                //La Clase StreamReader nos permite leer el contenido de un archivo.
                StreamReader fl = new StreamReader(OpenFileDialog2.FileName);

                while ((line = fl.ReadLine()) != null)
                {
                    lineas.Add(line);
                    numerolineasarchivo++;
                }

                if (numerolineasarchivo != 2)
                {
                    MessageBox.Show(this,"Posible Error se han leido menos de dos líneas del archivo Heat Balance Solution.","Error en el contenido del archivo Heat Balance Solution", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

            //En caso de que pulsemos el botón de Cancel o cerremos el diálogo de Open File mostraremos un mensaje "You hit cancel or closed the dialog."
            else
            {
                MessageBox.Show("You hit cancel or closed the dialog.");
                OpenFileDialog2.Dispose();
                OpenFileDialog2 = null;
            }

            OpenFileDialog2.Dispose();
            OpenFileDialog2 = null;

            //SEGUNDO: Abrimos la imagen del Heat Balance Scheme *.bmp, cuya ruta está guardad en la variable linea[0]
            OpenFileDialog openFileDialog3 = new OpenFileDialog();
            openFileDialog3.FileName=lineas[0];
            this.pictureBox2.Image = new Bitmap(openFileDialog3.OpenFile());
            //this.pictureBox3.Image = new Bitmap(openFileDialog3.OpenFile());

            //Opción "ZOOM" del Menú Contextual de la imagen del Heat Balance 
            //Esta opción nos pemite encuadrar la imagen del Balance
            toolStripMenuItem15_Click(sender, e);

            //TERCERO: Abrimos el archivo de entrada de datos Heat Balance Data Input *.dat, cuya ruta está guardada en la variable linea[1]
            //En el StatusBar de la Aplicación principal mostraremos el directorio y el nombre del Archivo abierto.

            toolStripStatusLabel1.Text = "Working Direcgtory and File: " + lineas[1];

            //MessageBox.Show("You selected the file: " + OpenFileDialog .FileName);

            //La Clase StreamReader nos permite leer el contenido de un archivo.
            StreamReader tempfl = new StreamReader(lineas[1]);

            //Llamamos al cuadro de Diálogo para Leer los datos en el fichero elegido de HBAL (*.DAT)
            //Enviamos al cuadro de Diálogo el puntero fl1 utilizado para lectura del archivo
            LecturaHbal lecturaHbal1 = new LecturaHbal(this, tempfl);

            //Mostramos el cuadro de Diálogo "lecturaHbal1"
            lecturaHbal1.Show();

            //Llamamos a la función de actualizar el control tree "arbol" en la Aplicación Principal con la información del archivo leido.
            actualizararbol();
        }

        //Opción del Menu para visualizar el Cuadro de Diálogo de datos de entrada del Desaireador 18
        private void desaireadorTipo18ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListaDesaireador listdesaireador = new ListaDesaireador(this);
            listdesaireador.Show();
        }

        //Opción del Menu para visualizar el Cuadro de Diálogo de datos de entrada del Intercambiador de Calor 22
        private void heatExchangerType22ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListaIntercambiador22 listaintercambiador = new ListaIntercambiador22(this);
            listaintercambiador.Show();
        }

        //Opción del Menu de Drainage Cooler Type 16
        private void enfriadorDeDrenajesTipo16ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListaDrainageCooling16 listdrenaje = new ListaDrainageCooling16(this);
            listdrenaje.Show();
        }

        private void desrecalentadorDeVarporAtemperadorTipo17ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //numequipos++;

            //Llamada al cuadro de diálogo que crea las ecuaciones del equipo e inicializa sus valores
            //Enviamos al cuadro de dialogo el puntero de la ventana principal para guardar los datos del cuadro de dialogo en variables de la aplicacion principal
            //Atemperador atemperador17 = new Atemperador(this, numecuaciones, numvariables,0,0);
            //atemperador17.Show();
            ListaAtemperador17 listatemperador = new ListaAtemperador17(this);
            listatemperador.Show();
        }

        //Leer los datos incluidos en el TextBox14 de la Aplicación principal
        private void button59_Click(object sender, EventArgs e)
        {
            //Cargamos el cursor Wait mientras esta función realiza la lectura del archivo
            this.Cursor = Cursors.WaitCursor;

            //Pulsar la Opción del Menú Nuevo Cálculo
            borrarmatrixequipos = 0;
            toolStripMenuItem11_Click(sender, e);

            int totalLines=textBox14.Lines.Length;

            List<String> lineas33 = new List<String>();

            foreach (string line22 in textBox14.Lines)
            {
                lineas33.Add(line22);
            }

            for (int j = 0; j < lineas33.Count; j++)
            {
                if ((lineas33[j] == "")&&(j<lineas33.Count-1))
                {
                    lineas33[j] = "0.";
                }
            }
            
            //char[] delimiters = new char[] { '\r', '\n' };

            //string[] lines = textBox14.Text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
         
            LecturaHbal luis = new LecturaHbal(this,null);

            luis.numlineasfichero = totalLines-1;

            for (int i = 0; i < totalLines-1; i=i+1)
            {
                luis.lineas.Add(lineas33[i]);
            }

            luis.button5_Click(sender,e);

            //Cargamos el cursor Wait mientras esta función realiza la lectura del archivo
            this.Cursor = Cursors.Arrow;
        }

        //Opción del Menú para visualizar los resultados de un Equipo en particular
        private void equipmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Elegimos las pestañas 1 y 10 de los controles tab de la aplicación principal
            tabControl1.SelectedTab = tabPage1;
            tabControl2.SelectedTab = tabPage10;

            actualizararbol();
        }

        //Resultados de la Potencia Generada por el Balance Térmico de la Central 
        private void heatBalancePowerReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Elegimos las pestañas 1 y 8 de los controles tab de la aplicación principal
            tabControl1.SelectedTab = tabPage2;
            tabControl2.SelectedTab = tabPage10;

            listBox2.Items.Clear();

            //Mostrar el Balance de Potencia Generada por las Turbinas de ALTA+BAJA y descontar las Pérdidas en el Generdor y la Potencia consumida por las Bombas
                        
        }

        //Datos de Entrada de los Equipos
        private void treeView2_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            listBox6.Items.Clear();

            TreeNode clickedNode = e.Node;
            Int16 numequipo = 0;

            if (clickedNode.Text == "EQUIPOS")
            {
                //MessageBox.Show(clickedNode.Text);
            }

            else 
            {
                int indice = 0;
                double numeroequipotemporal = 0;

                string temp = clickedNode.Text;
                int longitud = clickedNode.Text.Length;

                string numequipotemp = temp.Substring(17, longitud-17);
                numeroequipotemporal = Convert.ToInt16(numequipotemp);

                for (int j = 0; j < equipos11.Count; j++)
                {
                    if (equipos11[j].numequipo2 == numeroequipotemporal)
                    {
                        indice = j;
                    }
                }               

                //MessageBox.Show(numequipo);

                equipos11[indice].Number = equipos11[indice].numequipo2;
                equipos11[indice].Type = equipos11[indice].tipoequipo2;

                equipos11[indice].N1 = equipos11[indice].aN1;
                equipos11[indice].N2 = equipos11[indice].aN2;
                equipos11[indice].N3 = equipos11[indice].aN3;
                equipos11[indice].N4 = equipos11[indice].aN4;

                equipos11[indice].D1 = equipos11[indice].aD1;
                equipos11[indice].D2 = equipos11[indice].aD2;
                equipos11[indice].D3 = equipos11[indice].aD3;
                equipos11[indice].D4 = equipos11[indice].aD4;
                equipos11[indice].D5 = equipos11[indice].aD5;
                equipos11[indice].D6 = equipos11[indice].aD6;
                equipos11[indice].D7 = equipos11[indice].aD7;
                equipos11[indice].D8 = equipos11[indice].aD8;
                equipos11[indice].D9 = equipos11[indice].aD9;

                //System.Reflection.MemberInfo property = typeof(Equipos).GetProperty("D1");
                //var attribute1 = property.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                //MessageBox.Show(Convert.ToString(attribute1));          
                //propertyGrid1.SelectedObject = equipos11[indice];
                if (unidades == 2)
                {
                    listBox6.Items.Add("Sistema Internacional, Caudal Kg/Sg, Presión Bar, Entalpía Kj/Kgr");
                    listBox6.Items.Add("");
                    listBox6.Items.Add("Equipo Nº: " + equipos11[indice].Number);
                    listBox6.Items.Add("Tipo de Equipo: " + equipos11[indice].Type);
                    listBox6.Items.Add("");

                    if (equipos11[indice].Type == 1)
                    {
                        listBox6.Items.Add("Caudal Entrada(D1): " + Convert.ToString(equipos11[indice].D1 * 0.4536));
                        listBox6.Items.Add("Presión Entrada(D2): " + Convert.ToString(equipos11[indice].D2 * (6.8947572 / 100)));
                        listBox6.Items.Add("Entalpía Entrada(D3): " + Convert.ToString(equipos11[indice].D3 * 2.326009));
                        listBox6.Items.Add("No plante ecuación continuidad(D5): " + equipos11[indice].D5);

                        if (equipos11[indice].D6>0)
                        {
                            listBox6.Items.Add("Presión(positivo), Temperatura(negativo) (D6): " + Convert.ToString(equipos11[indice].D6 * (6.8947572 / 100)));
                        }

                        else if (equipos11[indice].D6<0)
                        {
                            listBox6.Items.Add("Presión(positivo), Temperatura(negativo) (D6): " + Convert.ToString(equipos11[indice].D6-273.0));
                        }

                        listBox6.Items.Add("Título (D7): " + equipos11[indice].D7);
                    }

                    else if (equipos11[indice].Type == 2)
                    {
                        listBox6.Items.Add("Caudal Entrada(D1): " + Convert.ToString(equipos11[indice].D1 * 0.4536));
                        listBox6.Items.Add("Fraccion de caudal N1 que sale por N3 (D2): " + equipos11[indice].D2);
                        listBox6.Items.Add("Factor de Flujo(D3): " + equipos11[indice].D3);
                    }

                    else if (equipos11[indice].Type == 3)
                    {
                        listBox6.Items.Add("Coeficiente Ecuación Cuadrática (D1): " + Convert.ToString(equipos11[indice].D1 * (6.8947572 / 100)));
                        listBox6.Items.Add("Coeficiente Ecuación Cuadrática (D2): " + Convert.ToString(equipos11[indice].D2 / 2.984193609));
                        listBox6.Items.Add("Coeficiente Ecuación Cuadrática (D3): " + Convert.ToString(equipos11[indice].D3 / 6.578911309));
                        listBox6.Items.Add("Factor Porcentaje Pérdida Carga (D4): " + equipos11[indice].D4);
                        listBox6.Items.Add("Factor fxL/D (D5): " + equipos11[indice].D5);
                        listBox6.Items.Add("Diámetro Interno (D6): " + Convert.ToString((equipos11[indice].D6* 1000) / 3.28083));
                        listBox6.Items.Add("Diferencia de Cotas (D7): " + Convert.ToString(equipos11[indice].D7 / 3.28083));
                        listBox6.Items.Add("Número Tuberías en Paralelo (D8): " + equipos11[indice].D8);
                        listBox6.Items.Add("Válvula Antirretorno - 1 Si - 0 No (D9): " + equipos11[indice].D9);
                    }

                    else if (equipos11[indice].Type == 4)
                    {
                        listBox6.Items.Add("Coeficiente Ecuación Cuadrática (D1): " +Convert.ToString(equipos11[indice].D1 * (6.8947572 / 100)));
                        listBox6.Items.Add("Coeficiente Ecuación Cuadrática (D2): " +Convert.ToString(equipos11[indice].D2 / 6.578911309));
                        listBox6.Items.Add("Coeficiente Ecuación Cuadrática (D3): " +Convert.ToString(equipos11[indice].D3 / 2.984193609));
                        listBox6.Items.Add("Rendimiento de la Bomba (D4): " + equipos11[indice].D4);
                        listBox6.Items.Add("Presión de Desacarga en N2 (D5): " +Convert.ToString(equipos11[indice].D5 * (6.8947572 / 100)));
                        listBox6.Items.Add("Número Bombas en Paralelo (D7): " + equipos11[indice].D7);
                        listBox6.Items.Add("Tabla TDH = f(caudal) (D8): " + equipos11[indice].D8);
                        listBox6.Items.Add("Cálculo TDH D9=1, cálculo presión descarga D9=0 (D9): " + equipos11[indice].D9);
                    }

                    else if (equipos11[indice].Type == 5)
                    {
                        listBox6.Items.Add("Definición de Presión de Salida en N3 (D1): " + Convert.ToString(equipos11[indice].D1 * (6.8947572 / 100)));
                        listBox6.Items.Add("Ecuación de equilibrio de presiones (D9): " + equipos11[indice].D9);
                    }

                    else if (equipos11[indice].Type == 6)
                    {
                        listBox6.Items.Add("Coeficiente Ecuación Cuadrática (D1): " + Convert.ToString(equipos11[indice].D1 * (6.8947572 / 100)));
                        listBox6.Items.Add("Coeficiente Ecuación Cuadrática (D2): " + Convert.ToString(equipos11[indice].D2 / 6.578911309));
                        listBox6.Items.Add("Coeficiente Ecuación Cuadrática (D3): " + Convert.ToString(equipos11[indice].D3 / 2.984193609));
                        if (equipos11[indice].D4 < 0)
                        {
                            //En este caso fijamos la presión P2=D4, por tanto, tenemos que realizar la conversión de Bar a PSI
                            listBox6.Items.Add("Factor Porcentaje Pérdida Carga (D4): " + Convert.ToString(equipos11[indice].D4* (6.8947572 / 100)));
                        }

                        if (equipos11[indice].D5 > 0)
                        {
                            //Entalpia Kj/Kgr a Btu/Lb
                            listBox6.Items.Add("Fija la Temperatura o Entalpía en corriente de Salida (D5): " + Convert.ToString(equipos11[indice].D5 * 2.326009));
                        }
                        else if (equipos11[indice].D5 < 0)
                        {
                            //Convertir los grados ºC en ºF
                             listBox6.Items.Add("Fija la Temperatura o Entalpía en corriente de Salida (D5): " + Convert.ToString((equipos11[indice].D5-32)*(5/9)));
                        }                       
                        listBox6.Items.Add("Calor aportado al Equipo (D6): " + Convert.ToString(equipos11[indice].D6 / 0.9486608));
                        listBox6.Items.Add("Rendimiento de Intercambio Térmico (D7): " + equipos11[indice].D7);
                        listBox6.Items.Add("Contabilización del Calor aportardo por el Equipo D9=1 Si, D9=0 No, (D9): " + equipos11[indice].D9);
                    }

                    else if (equipos11[indice].Type == 7)
                    {
                        listBox6.Items.Add("Coeficiente Ecuación Cuadrática (D1): " + Convert.ToString(equipos11[indice].D1 * (6.8947572 / 100)));
                        listBox6.Items.Add("Coeficiente Ecuación Cuadrática (D2): " + Convert.ToString(equipos11[indice].D2 / 6.578911309));
                        listBox6.Items.Add("Coeficiente Ecuación Cuadrática (D3): " + Convert.ToString(equipos11[indice].D3 / 2.984193609));
                        if (equipos11[indice].D4 > 500)
                        {
                            listBox6.Items.Add("DCA (D4): " + equipos11[indice].D4);
                        }
                        else if (equipos11[indice].D4 < 500)
                        {
                            listBox6.Items.Add("DCA (D4): " + Convert.ToString(equipos11[indice].D4*(5.0/9.0)));
                        }

                        //Convertir ºC a ºF
                        if (equipos11[indice].D5 > 500)
                        {
                            listBox6.Items.Add("TTD (D5): " + equipos11[indice].D5);
                        }
                        else if (equipos11[indice].D5 < 500)
                        {
                            listBox6.Items.Add("TTD (D5): " + Convert.ToString(equipos11[indice].D5*(5.0/9.0)));
                        }
                       
                        listBox6.Items.Add("Rendimiento Térmico (D6): " + equipos11[indice].D6);
                        listBox6.Items.Add("Igualdad e Presiones con Corriente de Cascada D7=1 Si, D7=0 No, (D7): " + equipos11[indice].D7);
                        listBox6.Items.Add("Número de Calentadores en Paralelo (D8): " + equipos11[indice].D8);
                        listBox6.Items.Add("Número de Corriente de Cascada N5, (D9): " + equipos11[indice].D9);
                    }

                    else if (equipos11[indice].Type == 8)
                    {
                        listBox6.Items.Add("Presión Vacion ó Coeficiente conversión m3/h a Tm/h + temperatura entrada agua circulación (D1): " + Convert.ToString(equipos11[indice].D1 * (6.8947572 / 100)));
                        listBox6.Items.Add("Diámetro Exterior Tubos (D2): " + equipos11[indice].D2);
                        listBox6.Items.Add("Galga o Espesor (D3): " + equipos11[indice].D3);
                        listBox6.Items.Add("Factor de Material de Tubos (D4): " + equipos11[indice].D4);
                        listBox6.Items.Add("Factor de Limpieza (D5): " + equipos11[indice].D5);
                        listBox6.Items.Add("Longitud efectiva de Tubos (D6): " + equipos11[indice].D6);
                        listBox6.Items.Add("Caudal de Agua de Circulación, (D7): " + equipos11[indice].D7);
                        listBox6.Items.Add("Superficie Efectiva Total (D8): " + equipos11[indice].D8);
                        listBox6.Items.Add("Producto del Número de Pasos por el Número de Presiones (D9): " + equipos11[indice].D9);
                    }

                    else if (equipos11[indice].Type == 9)
                    {
                        listBox6.Items.Add("Rendimiento Termodinámico (D1): " + equipos11[indice].D1);
                        listBox6.Items.Add("Factor de Flujo (D3): " + Convert.ToString(equipos11[indice].D3 / 2.316749697));
                        listBox6.Items.Add("Pérdidas de Entalpía en el Escape en función de la Velocidad o Caudal Volumétrico (D4): " + equipos11[indice].D4);
                        listBox6.Items.Add("Semisuma de las Calidades de Vapor entrada y salida escalón de vapor (D5): " + equipos11[indice].D5);
                        listBox6.Items.Add("Presión salida/ Presión entrada (D8): " + equipos11[indice].D8);
                        listBox6.Items.Add("Area Total de Escape (D9): " + equipos11[indice].D9);                       
                    }

                    else if (equipos11[indice].Type == 10)
                    {
                        listBox6.Items.Add("Rendimiento Termodinámico (D1): " + equipos11[indice].D1);
                        listBox6.Items.Add("Presión en el Escape (D2): " + Convert.ToString(equipos11[indice].D2 * (6.8947572 / 100)));
                        listBox6.Items.Add("Factor de Flujo (D3): " + Convert.ToString(equipos11[indice].D3 / 2.316749697));
                        listBox6.Items.Add("Semisuma de las Calidades de Vapor entrada y salida escalón de vapor (D5): " + equipos11[indice].D5);
                        listBox6.Items.Add("Presión salida/ Presión entrada (D8): " + equipos11[indice].D8);
                    }

                    else if (equipos11[indice].Type == 11)
                    {
                        listBox6.Items.Add("Rendimiento Termodinámico (D1): " + equipos11[indice].D1);
                        listBox6.Items.Add("Presión de Escape (D2): " + Convert.ToString(equipos11[indice].D2 * (6.8947572 / 100)));
                        listBox6.Items.Add("Potencia (D5): " + Convert.ToString(equipos11[indice].D5 / 0.9486608));
                    }

                    else if (equipos11[indice].Type == 13)
                    {
                        listBox6.Items.Add("Eficiencia del Separador de Humedad (D1): " + equipos11[indice].D1);
                        listBox6.Items.Add("Fracción Caudal Másico de entrada arrastrado (D2): " + equipos11[indice].D2);
                    }

                    else if (equipos11[indice].Type == 14)
                    {
                        listBox6.Items.Add("Coeficiente Ecuación Cuadrática (D1): " + Convert.ToString(equipos11[indice].D1 * (6.8947572 / 100)));
                        listBox6.Items.Add("Coeficiente Ecuación Cuadrática (D2): " + Convert.ToString(equipos11[indice].D2 / 6.578911309));
                        listBox6.Items.Add("Coeficiente Ecuación Cuadrática (D3): " + Convert.ToString(equipos11[indice].D3 / 2.984193609));
                        listBox6.Items.Add("Factor Porcentaje Pérdida Carga  (D4): " + equipos11[indice].D4);
                        listBox6.Items.Add("TTD (D5): " + Convert.ToString(equipos11[indice].D5*(5.0/9.0)));
                        listBox6.Items.Add("Rendimiento Térmico (D6): " + equipos11[indice].D6);
                        listBox6.Items.Add("Coeficiente Ecuación Cuadrática (D7): " + Convert.ToString(equipos11[indice].D7 * (6.8947572 / 100)));
                        listBox6.Items.Add("Coeficiente Ecuación Cuadrática (D8): " + Convert.ToString(equipos11[indice].D8 / 6.578911309));
                        listBox6.Items.Add("Factor Porcentaje Pérdida Carga (D9): " + Convert.ToString(equipos11[indice].D9 * 2.984193609));
                    }
 
                    else if (equipos11[indice].Type == 15)
                    {
                        listBox6.Items.Add("Coeficiente Ecuación Cuadrática (D1): " + Convert.ToString(equipos11[indice].D1 * (6.8947572 / 100)));
                        listBox6.Items.Add("Coeficiente Ecuación Cuadrática (D2): " + Convert.ToString(equipos11[indice].D2 / 6.578911309));
                        listBox6.Items.Add("Coeficiente Ecuación Cuadrática (D3): " + Convert.ToString(equipos11[indice].D3 / 2.984193609));
                        listBox6.Items.Add("Título a la Salida del Drenaje N4 (D5): " + equipos11[indice].D5);
                        listBox6.Items.Add("Rendimiento Térmico (D6): " + equipos11[indice].D6);
                        listBox6.Items.Add("Presión de Operación de Carcasa (D7): " + Convert.ToString(equipos11[indice].D7 * (6.8947572 / 100)));
                        listBox6.Items.Add("Número de Condensadores en Paralelo (D8): " + equipos11[indice].D8);
                    }

                    else if (equipos11[indice].Type == 16)
                    {
                        listBox6.Items.Add("Coeficiente Ecuación Cuadrática (D1): " + Convert.ToString(equipos11[indice].D1 * (6.8947572 / 100)));
                        listBox6.Items.Add("Coeficiente Ecuación Cuadrática (D2): " + Convert.ToString(equipos11[indice].D2 / 6.578911309));
                        listBox6.Items.Add("Coeficiente Ecuación Cuadrática (D3): " + Convert.ToString(equipos11[indice].D3 / 2.984193609));
                        //Convertir ºC a ºF
                        if (equipos11[indice].D4 > 500)
                        {
                            listBox6.Items.Add("DCA (D4): " + Convert.ToString(equipos11[indice].D4));
                        }
                        else if (equipos11[indice].D4 < 500)
                        {
                            listBox6.Items.Add("DCA (D4): " + Convert.ToString(equipos11[indice].D4*(5.0/9.0)));
                        }
                        listBox6.Items.Add("Rendimiento Térmico (D6): " + equipos11[indice].D6);
                        listBox6.Items.Add("Número de Calentadores en Paralelo (D8): " + equipos11[indice].D8);
                    }

//----------- A partir de aquí está pendiente incluir los factores de conversión de unidades del Sistema Británico al Sistema Internacional------------------------------------------------------------------------------------------------------------

                    else if (equipos11[indice].Type == 17)
                    {
                        listBox6.Items.Add("Coeficiente Ecuación Cuadrática (D1): " + Convert.ToString(equipos11[indice].D1 * (6.8947572 / 100)));
                        listBox6.Items.Add("Coeficiente Ecuación Cuadrática (D2): " + Convert.ToString(equipos11[indice].D2 / 6.578911309));
                        listBox6.Items.Add("Coeficiente Ecuación Cuadrática (D3): " + Convert.ToString(equipos11[indice].D3 / 2.984193609));
                        listBox6.Items.Add("Factor Porcentaje Pérdida Carga  (D4): " + equipos11[indice].D4);
                        listBox6.Items.Add("Incremento de Temperatura sobre la de saturación en la corriente N3 (D5): " + Convert.ToString(equipos11[indice].D5*(5.0/9.0)));
                        listBox6.Items.Add("Rendimiento Térmico (D6): " + equipos11[indice].D6);
                    }

                    else if (equipos11[indice].Type == 18)
                    {
                        listBox6.Items.Add("TTD (D5): " + Convert.ToString(equipos11[indice].D5*(5.0/9.0)));
                        listBox6.Items.Add("Rendimiento Térmico (D6): " + equipos11[indice].D6);
                        listBox6.Items.Add("Equilibrado de Presiones con corriente de agua de alimentación (D7): " + equipos11[indice].D7);
                        listBox6.Items.Add("Número de la corriente de cascada N5 (D9): " + equipos11[indice].D9);
                    }

                    else if (equipos11[indice].Type == 19)
                    {
                        listBox6.Items.Add("Coeficiente Ecuación Cuadrática (D1): " + Convert.ToString(equipos11[indice].D1 * (6.8947572 / 100)));
                        listBox6.Items.Add("Coeficiente Ecuación Cuadrática (D2): " + Convert.ToString(equipos11[indice].D2 / 6.578911309));
                        listBox6.Items.Add("Coeficiente Ecuación Cuadrática (D3): " + Convert.ToString(equipos11[indice].D3 / 2.984193609));
                        listBox6.Items.Add("CV de la Válvula (D4): " + equipos11[indice].D4);
                        listBox6.Items.Add("Factor de Caudal Crítico (D5): " + equipos11[indice].D5);
                        listBox6.Items.Add("CV Máximo (D6): " + equipos11[indice].D6);
                        listBox6.Items.Add("Coeficiente Ecuación Cuadrática (D7): " + Convert.ToString(equipos11[indice].D7 * (6.8947572 / 100)));
                        listBox6.Items.Add("Coeficiente Ecuación Cuadrática (D8): " + Convert.ToString(equipos11[indice].D8 / 6.578911309));
                        listBox6.Items.Add("Coeficiente Ecuación Cuadrática (D9): " + Convert.ToString(equipos11[indice].D9 / 2.984193609));
                    }

                    else if (equipos11[indice].Type == 20)
                    {
                        listBox6.Items.Add("Caudal por N3 (D1): " + Convert.ToString(equipos11[indice].D1 * (0.4536)));
                        listBox6.Items.Add("Entalpía en la Corriente N3 (D2): " + Convert.ToString(equipos11[indice].D2 * 2.326009));
                        listBox6.Items.Add("Define la presión en la corriente N3 (D3): " + Convert.ToString(equipos11[indice].D3 * (6.8947572 / 100)));
                    }

                    else if (equipos11[indice].Type == 21)
                    {
                        listBox6.Items.Add("Presión de Operación (D1): " + Convert.ToString(equipos11[indice].D1 * (6.8947572 / 100)));
                    }

                    else if (equipos11[indice].Type == 22)
                    {
                        listBox6.Items.Add("Coeficiente Ecuación Cuadrática (D1): " + Convert.ToString(equipos11[indice].D1 * (6.8947572 / 100)));
                        listBox6.Items.Add("Coeficiente Ecuación Cuadrática (D2): " + Convert.ToString(equipos11[indice].D2 / 6.578911309));
                        listBox6.Items.Add("Coeficiente Ecuación Cuadrática (D3): " + Convert.ToString(equipos11[indice].D3 / 2.984193609));
                        listBox6.Items.Add("Factor Porcentaje Pérdida Carga (D4): " + equipos11[indice].D4);
                        listBox6.Items.Add("Rendimiento Térmico (D5): " + equipos11[indice].D5);
                        listBox6.Items.Add("Coeficiente Ecuación Cuadrática (D6): " + Convert.ToString(equipos11[indice].D6 * (6.8947572 / 100)));
                        listBox6.Items.Add("Coeficiente Ecuación Cuadrática (D7): " + Convert.ToString(equipos11[indice].D7 / 6.578911309));
                        listBox6.Items.Add("Coeficiente Ecuación Cuadrática (D8): " + Convert.ToString(equipos11[indice].D8 / 2.984193609));
                        listBox6.Items.Add("Factor Porcentaje Pérdida Carga (D9): " + equipos11[indice].D9);
                    }
//--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

                }

                //CONTINUAR CON EL RESTO DE EQUIPOS
            }
        }

        //RESULTADO de los Equipos
        private void treeView3_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            listBox3.Items.Clear();

            TreeNode clickedNode = e.Node;

            Int16 numequipo = 0;

            ClassCondicionContorno1 condtemp = new ClassCondicionContorno1();
            condtemp.inicializar(0, 0, 0, 0, 0, 0);

            ClassDivisor2 divisortemp = new ClassDivisor2();
            divisortemp.inicializar(0, 0, 0, 0, 0, 0, 0, 0, 0);

            ClassPerdidaCarga3 perdidacargatemp = new ClassPerdidaCarga3();
            perdidacargatemp.inicializar(0, 0, 0, 0, 0, 0);

            ClassBomba4 bombatemp = new ClassBomba4();
            bombatemp.inicializar(0, 0, 0, 0, 0, 0);

            ClassMezclador5 mezcladortemp = new ClassMezclador5();
            mezcladortemp.inicializar(0, 0, 0, 0, 0, 0,0,0,0);

            ClassReactor6 reactortemp = new ClassReactor6();
            reactortemp.inicializar(0, 0, 0, 0, 0, 0);

            ClassCalentador7 calentadortemp = new ClassCalentador7();
            calentadortemp.inicializar(0, 0, 0, 0, 0, 0,0,0,0,0,0,0);

            ClassCondensador8 condensadortemp = new ClassCondensador8();
            condensadortemp.inicializar(0, 0, 0, 0, 0, 0,0,0,0);

            ClassTurbina9 turbina9temp = new ClassTurbina9();
            turbina9temp.inicializar(0, 0, 0, 0, 0, 0);

            ClassTurbina10 turbina10temp = new ClassTurbina10();
            turbina10temp.inicializar(0, 0, 0, 0, 0, 0);

            ClassTurbina11 turbina11temp = new ClassTurbina11();
            turbina11temp.inicializar(0, 0, 0, 0, 0, 0);

            ClassSeparadorHumedad13 separadorhumedadtemp = new ClassSeparadorHumedad13();
            separadorhumedadtemp.inicializar(0, 0, 0, 0, 0, 0,0,0,0);

            ClassMSR14 MSRtemp = new ClassMSR14();
            MSRtemp.inicializar(0, 0, 0, 0, 0, 0,0,0,0,0,0,0);

            ClassCondensador15 condensador15tmp = new ClassCondensador15();
            condensador15tmp.inicializar(0,0,0,0,0,0,0,0,0,0,0,0);

            ClassEnfriadorDrenajes16 enfriadortmp = new ClassEnfriadorDrenajes16();
            enfriadortmp.inicializar(0,0,0,0,0,0,0,0,0,0,0,0);

            ClassAtemperador17 atemperadortmp = new ClassAtemperador17();
            atemperadortmp.inicializar(0,0,0,0,0,0,0,0,0);

            ClassDesaireador18 desaireadortmp = new ClassDesaireador18();
            desaireadortmp.inicializar(0,0,0,0,0,0,0,0,0,0,0,0);

            ClassValvula19 valvulatmp = new ClassValvula19();
            valvulatmp.inicializar(0,0,0,0,0,0);

            ClassDivisorEntalpiaFija20 divientalpiatmp =new ClassDivisorEntalpiaFija20();
            divientalpiatmp.inicializar(0,0,0,0,0,0,0,0,0);

            ClassTanqueVaporizacion21 tanquevapotmp =new ClassTanqueVaporizacion21();
            tanquevapotmp.inicializar(0,0,0,0,0,0,0,0,0);

            ClassIntercambiador22 intercambiadortmp = new ClassIntercambiador22();
            intercambiadortmp.inicializar(0,0,0,0,0,0,0,0,0,0,0,0);

            if (clickedNode.Text == "EQUIPOS")
            {
                //MessageBox.Show(clickedNode.Text);
            }

            else
            {
                int indice = 0;
                double numeroequipotemporal = 0;

                string temp = clickedNode.Text;
                int longitud = clickedNode.Text.Length;

                string numequipotemp = temp.Substring(17, longitud - 17);
                numeroequipotemporal = Convert.ToInt16(numequipotemp);

                for (int j = 0; j < equipos11.Count; j++)
                {
                    if (equipos11[j].numequipo2 == numeroequipotemporal)
                    {
                        indice = j;
                    }
                }              

                //Unidades del Sistema Internacional
                if (unidades == 2)
                {
                    condtemp.unidades1 = 2;
                    divisortemp.unidades1 = 2;
                    perdidacargatemp.unidades1 = 2;
                    bombatemp.unidades1 = 2;
                    mezcladortemp.unidades1 = 2;
                    reactortemp.unidades1 = 2;
                    calentadortemp.unidades1 = 2;
                    condensadortemp.unidades1 = 2;
                    turbina9temp.unidades1 = 2;
                    turbina10temp.unidades1 = 2;
                    turbina11temp.unidades1 = 2;
                    separadorhumedadtemp.unidades1 = 2;
                    MSRtemp.unidades1 = 2;
                    condensador15tmp.unidades1 = 2;
                    enfriadortmp.unidades1 = 2;
                    atemperadortmp.unidades1 = 2;
                    desaireadortmp.unidades1 = 2;
                    valvulatmp.unidades1 = 2;
                    divientalpiatmp.unidades1 = 2;
                    tanquevapotmp.unidades1 = 2;
                    intercambiadortmp.unidades1 = 2;
                }
                
                if (equipos11[indice].tipoequipo2 == 1)
                {
                    for (int j = 0; j < numtipo1; j++)
                    {
                       if (matrixcondicioncontorno1[j, setnumber].numequipo == equipos11[indice].numequipo2)
                       {
                           condtemp.numcorrentrada = matrixcondicioncontorno1[j, setnumber].numcorrentrada;
                           condtemp.numcorrsalida = matrixcondicioncontorno1[j, setnumber].numcorrsalida;
                           condtemp.numequipo = matrixcondicioncontorno1[j, setnumber].numequipo;
                       }
                    }

                    //condtemp.numcorrentrada = equipos11[indice].aN1;
                    //condtemp.numcorrsalida = equipos11[indice].aN3;
                    //condtemp.numequipo = equipos11[indice].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 9 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = listaresultadoscorrientes[o, setnumber].Nombre;
                        int longitud1 = nom.Length;
                        String tem = nom.Substring(1, longitud1 - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr == condtemp.numcorrentrada)
                        {
                            if (tipoparametro == "W")
                            {
                                condtemp.caudalcorrentrada = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                condtemp.presioncorrentrada = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                condtemp.entalpiacorrentrada = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }

                        else if (numcorr == condtemp.numcorrsalida)
                        {
                            if (tipoparametro == "W")
                            {
                                condtemp.caudalcorrsalida = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                condtemp.presioncorrsalida = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                condtemp.entalpiacorrsalida = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }
                    }
                       condtemp.Calcular();
                }
                    

//-------------------------------  A PARTIR DE AQUI CONTINUAR CON LOS CAMBIOS REALIZADOS SOBRE LA CONDICIÓN DE CONTORNO --------------


                else if (equipos11[indice].tipoequipo2 == 2)
                {
                    for (int j = 0; j < numtipo2; j++)
                    {
                        if (matrixdivisor2[j, setnumber].numequipo == equipos11[indice].numequipo2)
                        {
                            divisortemp.numcorrentrada  = matrixdivisor2[j, setnumber].numcorrentrada;
							divisortemp.numcorrsalida1  = matrixdivisor2[j, setnumber].numcorrsalida1;
							divisortemp.numcorrsalida2  = matrixdivisor2[j, setnumber].numcorrsalida2;
							divisortemp.numequipo  = matrixdivisor2[j, setnumber].numequipo;
							   
                        }
                    }
                        //divisortemp.numcorrentrada = equipos11[indice].aN1;
                        //divisortemp.numcorrsalida1 = equipos11[indice].aN3;
                        //divisortemp.numcorrsalida2 = equipos11[indice].aN4;
                        //divisortemp.numequipo = equipos11[indice].numequipo2;

                        //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 9 y guardamos sus valores en la ClassTurbina9 
                        for (int o = 0; o < p.Count; o++)
                        {
                            String nom = listaresultadoscorrientes[o, setnumber].Nombre;
                            int longitud1 = nom.Length;
                            String tem = nom.Substring(1, longitud1 - 1);
                            String tipoparametro = nom.Substring(0, 1);
                            Double numcorr = Convert.ToDouble(tem);

                            if (numcorr == divisortemp.numcorrentrada)
                            {
                                if (tipoparametro == "W")
                                {
                                    divisortemp.caudalcorrentrada = listaresultadoscorrientes[o, setnumber].Value;
                                }

                                else if (tipoparametro == "P")
                                {
                                    divisortemp.presioncorrentrada = listaresultadoscorrientes[o, setnumber].Value;
                                }

                                else if (tipoparametro == "H")
                                {
                                    divisortemp.entalpiacorrentrada = listaresultadoscorrientes[o, setnumber].Value;
                                }
                            }

                            else if (numcorr == divisortemp.numcorrsalida1)
                            {
                                if (tipoparametro == "W")
                                {
                                    divisortemp.caudalcorrsalida1 = listaresultadoscorrientes[o, setnumber].Value;
                                }

                                else if (tipoparametro == "P")
                                {
                                    divisortemp.presioncorrsalida1 = listaresultadoscorrientes[o, setnumber].Value;
                                }

                                else if (tipoparametro == "H")
                                {
                                    divisortemp.entalpiacorrsalida1 = listaresultadoscorrientes[o, setnumber].Value;
                                }
                            }

                            else if (numcorr == divisortemp.numcorrsalida2)
                            {
                                if (tipoparametro == "W")
                                {
                                    divisortemp.caudalcorrsalida2 = listaresultadoscorrientes[o, setnumber].Value;
                                }

                                else if (tipoparametro == "P")
                                {
                                    divisortemp.presioncorrsalida2 = listaresultadoscorrientes[o, setnumber].Value;
                                }

                                else if (tipoparametro == "H")
                                {
                                    divisortemp.entalpiacorrsalida2 = listaresultadoscorrientes[o, setnumber].Value;
                                }
                            }
                        }

                        divisortemp.Calcular();                     
                }

                else if (equipos11[indice].tipoequipo2 == 3)
                {
                    for (int j = 0; j < numtipo3; j++)
                    {
                        if (matrixperdida3[j, setnumber].numequipo == equipos11[indice].numequipo2)
                        {
                            perdidacargatemp.numcorrentrada  = matrixperdida3[j, setnumber].numcorrentrada;
							perdidacargatemp.numcorrsalida  = matrixperdida3[j, setnumber].numcorrsalida;
							perdidacargatemp.numequipo  = matrixperdida3[j, setnumber].numequipo;
                        }
                    }                  
                   
                    //perdidacargatemp.numcorrentrada = equipos11[indice].aN1;
                    //perdidacargatemp.numcorrsalida = equipos11[indice].aN3;
                    //perdidacargatemp.numequipo = equipos11[indice].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 9 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = listaresultadoscorrientes[o, setnumber].Nombre;
                        int longitud1 = nom.Length;
                        String tem = nom.Substring(1, longitud1 - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr == perdidacargatemp.numcorrentrada)
                        {
                            if (tipoparametro == "W")
                            {
                                perdidacargatemp.caudalcorrentrada = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                perdidacargatemp.presioncorrentrada = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                perdidacargatemp.entalpiacorrentrada = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }

                        else if (numcorr == perdidacargatemp.numcorrsalida)
                        {
                            if (tipoparametro == "W")
                            {
                                perdidacargatemp.caudalcorrsalida = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                perdidacargatemp.presioncorrsalida = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                perdidacargatemp.entalpiacorrsalida = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }
                     }

                    perdidacargatemp.Calcular();
                }

                else if (equipos11[indice].tipoequipo2 == 4)
                {
                    for (int j = 0; j < numtipo4; j++)
                    {
                        if (matrixbomba4[j, setnumber].numequipo == equipos11[indice].numequipo2)
                        {
                           bombatemp.numcorrentrada  = matrixbomba4[j, setnumber].numcorrentrada;
						   bombatemp.numcorrsalida  = matrixbomba4[j, setnumber].numcorrsalida;
						   bombatemp.numequipo  = matrixbomba4[j, setnumber].numequipo;
                        }
                    }                   

                    //bombatemp.numcorrentrada = equipos11[indice].aN1;
                    //bombatemp.numcorrsalida = equipos11[indice].aN3;
                    //bombatemp.numequipo = equipos11[indice].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 9 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = listaresultadoscorrientes[o, setnumber].Nombre;
                        int longitud1 = nom.Length;
                        String tem = nom.Substring(1, longitud1 - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr == bombatemp.numcorrentrada)
                        {
                            if (tipoparametro == "W")
                            {
                                bombatemp.caudalcorrentrada = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                bombatemp.presioncorrentrada = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                bombatemp.entalpiacorrentrada = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }

                        else if (numcorr == bombatemp.numcorrsalida)
                        {
                            if (tipoparametro == "W")
                            {
                                bombatemp.caudalcorrsalida = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                bombatemp.presioncorrsalida = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                bombatemp.entalpiacorrsalida = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }
                    }
                    bombatemp.Calcular();
                }

                else if (equipos11[indice].tipoequipo2 == 5)
                {
                    for (int j = 0; j < numtipo5; j++)
                    {
                        if (matrixmezclador5[j, setnumber].numequipo == equipos11[indice].numequipo2)
                        {
                            mezcladortemp.numcorrentrada1 = matrixmezclador5[j, setnumber].numcorrentrada1;
							mezcladortemp.numcorrentrada2  = matrixmezclador5[j, setnumber].numcorrentrada2;
							mezcladortemp.numcorrsalida  = matrixmezclador5[j, setnumber].numcorrsalida;
							mezcladortemp.numequipo  = matrixmezclador5[j, setnumber].numequipo;			
                        }
                    }   
                   
                    //mezcladortemp.numcorrentrada1 = equipos11[indice].aN1;
                    //mezcladortemp.numcorrentrada2 = equipos11[indice].aN2;
                    //mezcladortemp.numcorrsalida = equipos11[indice].aN3;
                    //mezcladortemp.numequipo = equipos11[indice].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 9 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = listaresultadoscorrientes[o, setnumber].Nombre;
                        int longitud1 = nom.Length;
                        String tem = nom.Substring(1, longitud1 - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr == mezcladortemp.numcorrentrada1)
                        {
                            if (tipoparametro == "W")
                            {
                                mezcladortemp.caudalcorrentrada1 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                mezcladortemp.presioncorrentrada1 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                mezcladortemp.entalpiacorrentrada1 = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }

                        else if (numcorr == mezcladortemp.numcorrentrada2)
                        {
                            if (tipoparametro == "W")
                            {
                                mezcladortemp.caudalcorrentrada2 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                mezcladortemp.presioncorrentrada2 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                mezcladortemp.entalpiacorrentrada2 = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }

                        else if (numcorr == mezcladortemp.numcorrsalida)
                        {
                            if (tipoparametro == "W")
                            {
                                mezcladortemp.caudalcorrsalida = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                mezcladortemp.presioncorrsalida =listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                mezcladortemp.entalpiacorrsalida = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }
                    }
                    mezcladortemp.Calcular();
                }

                else if (equipos11[indice].tipoequipo2 == 6)
                {
                    for (int j = 0; j < numtipo6; j++)
                    {
                        if (matrixreactor6[j, setnumber].numequipo == equipos11[indice].numequipo2)
                        {
                            reactortemp.numcorrentrada  =matrixreactor6[j, setnumber].numcorrentrada;
							reactortemp.numcorrsalida  =matrixreactor6[j, setnumber].numcorrsalida;
							reactortemp.numequipo  =matrixreactor6[j, setnumber].numequipo;						
                        }
                    }  
                   
                    //reactortemp.numcorrentrada = equipos11[indice].aN1;
                    //reactortemp.numcorrsalida = equipos11[indice].aN3;
                    //reactortemp.numequipo = equipos11[indice].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 9 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = listaresultadoscorrientes[o, setnumber].Nombre;
                        int longitud1 = nom.Length;
                        String tem = nom.Substring(1, longitud1 - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr == reactortemp.numcorrentrada)
                        {
                            if (tipoparametro == "W")
                            {
                                reactortemp.caudalcorrentrada = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                reactortemp.presioncorrentrada = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                reactortemp.entalpiacorrentrada = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }

                        else if (numcorr == reactortemp.numcorrsalida)
                        {
                            if (tipoparametro == "W")
                            {
                                reactortemp.caudalcorrsalida = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                reactortemp.presioncorrsalida = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                reactortemp.entalpiacorrsalida = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }
                    }

                    reactortemp.Calcular();
                }

                else if (equipos11[indice].tipoequipo2 == 7)
                {
                    for (int j = 0; j < numtipo7; j++)
                    {
                        if (matrixcalentador7[j, setnumber].numequipo == equipos11[indice].numequipo2)
                        {
                            calentadortemp.numcorrentrada1  = matrixcalentador7[j, setnumber].numcorrentrada1;
							calentadortemp.numcorrentrada2 = matrixcalentador7[j, setnumber].numcorrentrada2;
							calentadortemp.numcorrsalida1  = matrixcalentador7[j, setnumber].numcorrsalida1;
							calentadortemp.numcorrsalida2 = matrixcalentador7[j, setnumber].numcorrsalida2;
							calentadortemp.numequipo = matrixcalentador7[j, setnumber].numequipo;		
                        }
                    }                  

                    //calentadortemp.numcorrentrada1 = equipos11[indice].aN1;
                    //calentadortemp.numcorrentrada2 = equipos11[indice].aN2;
                    //calentadortemp.numcorrsalida1 = equipos11[indice].aN3;
                    //calentadortemp.numcorrsalida2 = equipos11[indice].aN4;
                    //calentadortemp.numequipo = equipos11[indice].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 9 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = listaresultadoscorrientes[o, setnumber].Nombre;
                        int longitud1 = nom.Length;
                        String tem = nom.Substring(1, longitud1 - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr == calentadortemp.numcorrentrada1)
                        {
                            if (tipoparametro == "W")
                            {
                                calentadortemp.caudalcorrentrada1 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                calentadortemp.presioncorrentrada1 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                calentadortemp.entalpiacorrentrada1 = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }

                        else if (numcorr == calentadortemp.numcorrentrada2)
                        {
                            if (tipoparametro == "W")
                            {
                                calentadortemp.caudalcorrentrada2 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                calentadortemp.presioncorrentrada2 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                calentadortemp.entalpiacorrentrada2 = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }

                        else if (numcorr == calentadortemp.numcorrsalida1)
                        {
                            if (tipoparametro == "W")
                            {
                                calentadortemp.caudalcorrsalida1 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                calentadortemp.presioncorrsalida1 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                calentadortemp.entalpiacorrsalida1 = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }

                        else if (numcorr == calentadortemp.numcorrsalida2)
                        {
                            if (tipoparametro == "W")
                            {
                                calentadortemp.caudalcorrsalida2 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                calentadortemp.presioncorrsalida2 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                calentadortemp.entalpiacorrsalida2 = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }
                    }
                    calentadortemp.Calcular();
                }

                else if (equipos11[indice].tipoequipo2 == 8)
                {
                    for (int j = 0; j < numtipo8; j++)
                    {
                        if (matrixcondensador8[j, setnumber].numequipo == equipos11[indice].numequipo2)
                        {
                            condensadortemp.numcorrentrada1  = matrixcondensador8[j, setnumber].numcorrentrada1;
							condensadortemp.numcorrentrada2  = matrixcondensador8[j, setnumber].numcorrentrada2;
							condensadortemp.numcorrsalida = matrixcondensador8[j, setnumber].numcorrsalida;
							condensadortemp.numequipo  = matrixcondensador8[j, setnumber].numequipo;	
                        }
                    }   
                    
                    //condensadortemp.numcorrentrada1 = equipos11[indice].aN1;
                    //condensadortemp.numcorrentrada2 = equipos11[indice].aN2;
                    //condensadortemp.numcorrsalida = equipos11[indice].aN3;
                    //condensadortemp.numequipo = equipos11[indice].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 9 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = listaresultadoscorrientes[o, setnumber].Nombre;
                        int longitud1 = nom.Length;
                        String tem = nom.Substring(1, longitud1 - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr == condensadortemp.numcorrentrada1)
                        {
                            if (tipoparametro == "W")
                            {
                                condensadortemp.caudalcorrentrada1 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                condensadortemp.presioncorrentrada1 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                condensadortemp.entalpiacorrentrada1 = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }

                        else if (numcorr == condensadortemp.numcorrentrada2)
                        {
                            if (tipoparametro == "W")
                            {
                                condensadortemp.caudalcorrentrada2 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                condensadortemp.presioncorrentrada2 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                condensadortemp.entalpiacorrentrada2 = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }

                        else if (numcorr == condensadortemp.numcorrsalida)
                        {
                            if (tipoparametro == "W")
                            {
                                condensadortemp.caudalcorrsalida = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                condensadortemp.presioncorrsalida = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                condensadortemp.entalpiacorrsalida = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }
                    }
                    condensadortemp.Calcular();
                }

                else if (equipos11[indice].tipoequipo2 == 9)
                {
                    for (int j = 0; j < numtipo9; j++)
                    {
                        if ( matrixturbina9[j, setnumber].numequipo == equipos11[indice].numequipo2)
                        {
                            turbina9temp.numcorrentrada  =  matrixturbina9[j, setnumber].numcorrentrada;
							turbina9temp.numcorrsalida =  matrixturbina9[j, setnumber].numcorrsalida;
							turbina9temp.numequipo =  matrixturbina9[j, setnumber].numequipo;
                        }
                    } 

                    //turbina9temp.numcorrentrada = equipos11[indice].aN1;
                    //turbina9temp.numcorrsalida = equipos11[indice].aN3;
                    //turbina9temp.numequipo = equipos11[indice].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 9 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = listaresultadoscorrientes[o, setnumber].Nombre;
                        int longitud1 = nom.Length;
                        String tem = nom.Substring(1, longitud1 - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr == turbina9temp.numcorrentrada)
                        {
                            if (tipoparametro == "W")
                            {
                                turbina9temp.caudalcorrentrada = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                turbina9temp.presioncorrentrada = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                turbina9temp.entalpiacorrentrada = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }

                        else if (numcorr == turbina9temp.numcorrsalida)
                        {
                            if (tipoparametro == "W")
                            {
                                turbina9temp.caudalcorrsalida = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                turbina9temp.presioncorrsalida = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                turbina9temp.entalpiacorrsalida =listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }
                    }

                    turbina9temp.Calcular();
                }

                else if (equipos11[indice].tipoequipo2 == 10)
                {
                    for (int j = 0; j < numtipo10; j++)
                    {
                        if (matrixturbina10[j, setnumber].numequipo == equipos11[indice].numequipo2)
                        {
                            turbina10temp.numcorrentrada  = matrixturbina10[j, setnumber].numcorrentrada;
							turbina10temp.numcorrsalida = matrixturbina10[j, setnumber].numcorrsalida;
							turbina10temp.numequipo = matrixturbina10[j, setnumber].numequipo;
                        }
                    } 

                    // turbina10temp.numcorrentrada = equipos11[indice].aN1;
                    // turbina10temp.numcorrsalida = equipos11[indice].aN3;
                    // turbina10temp.numequipo = equipos11[indice].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 9 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = p[o].Nombre;
                        int longitud1 = nom.Length;
                        String tem = nom.Substring(1, longitud1 - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr ==  turbina10temp.numcorrentrada)
                        {
                            if (tipoparametro == "W")
                            {
                                 turbina10temp.caudalcorrentrada = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                 turbina10temp.presioncorrentrada = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                 turbina10temp.entalpiacorrentrada = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }

                        else if (numcorr ==  turbina10temp.numcorrsalida)
                        {
                            if (tipoparametro == "W")
                            {
                                 turbina10temp.caudalcorrsalida = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                 turbina10temp.presioncorrsalida = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                 turbina10temp.entalpiacorrsalida = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }
                    }

                     turbina10temp.Calcular();
                }

                else if (equipos11[indice].tipoequipo2 == 11)
                {
                    for (int j = 0; j < numtipo11; j++)
                    {
                        if (matrixturbina11[j, setnumber].numequipo == equipos11[indice].numequipo2)
                        {
                            turbina11temp.numcorrentrada  = matrixturbina11[j, setnumber].numcorrentrada;
							turbina11temp.numcorrsalida = matrixturbina11[j, setnumber].numcorrsalida;
							turbina11temp.numequipo  = matrixturbina11[j, setnumber].numequipo;
                        }
                    } 

                    //turbina11temp.numcorrentrada = equipos11[indice].aN1;
                    //turbina11temp.numcorrsalida = equipos11[indice].aN3;
                    //turbina11temp.numequipo = equipos11[indice].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 9 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = listaresultadoscorrientes[o, setnumber].Nombre;
                        int longitud1 = nom.Length;
                        String tem = nom.Substring(1, longitud1 - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr == turbina11temp.numcorrentrada)
                        {
                            if (tipoparametro == "W")
                            {
                                turbina11temp.caudalcorrentrada = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                turbina11temp.presioncorrentrada = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                turbina11temp.entalpiacorrentrada =listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }

                        else if (numcorr == turbina11temp.numcorrsalida)
                        {
                            if (tipoparametro == "W")
                            {
                                turbina11temp.caudalcorrsalida = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                turbina11temp.presioncorrsalida = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                turbina11temp.entalpiacorrsalida = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }
                    }

                    turbina11temp.Calcular();
                }

                else if (equipos11[indice].tipoequipo2 == 13)
                {
                    for (int j = 0; j < numtipo13; j++)
                    {
                        if (matrixseparador13[j, setnumber].numequipo == equipos11[indice].numequipo2)
                        {
                            separadorhumedadtemp.numcorrentrada  = matrixseparador13[j, setnumber].numcorrentrada;
							separadorhumedadtemp.numcorrsalida1  = matrixseparador13[j, setnumber].numcorrsalida1;
							separadorhumedadtemp.numcorrsalida2  = matrixseparador13[j, setnumber].numcorrsalida2;
							separadorhumedadtemp.numequipo  = matrixseparador13[j, setnumber].numequipo;
                        }
                    }                                     

                    //separadorhumedadtemp.numcorrentrada = equipos11[indice].aN1;
                    //separadorhumedadtemp.numcorrsalida1 = equipos11[indice].aN3;
                    //separadorhumedadtemp.numcorrsalida2 = equipos11[indice].aN4;
                    //separadorhumedadtemp.numequipo = equipos11[indice].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 9 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = listaresultadoscorrientes[o, setnumber].Nombre;
                        int longitud1 = nom.Length;
                        String tem = nom.Substring(1, longitud1 - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr == separadorhumedadtemp.numcorrentrada)
                        {
                            if (tipoparametro == "W")
                            {
                                separadorhumedadtemp.caudalcorrentrada = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                separadorhumedadtemp.presioncorrentrada = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                separadorhumedadtemp.entalpiacorrentrada = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }

                        else if (numcorr == separadorhumedadtemp.numcorrsalida1)
                        {
                            if (tipoparametro == "W")
                            {
                                separadorhumedadtemp.caudalcorrsalida1 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                separadorhumedadtemp.presioncorrsalida1 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                separadorhumedadtemp.entalpiacorrsalida1 = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }

                        else if (numcorr == separadorhumedadtemp.numcorrsalida2)
                        {
                            if (tipoparametro == "W")
                            {
                                separadorhumedadtemp.caudalcorrsalida2 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                separadorhumedadtemp.presioncorrsalida2 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                separadorhumedadtemp.entalpiacorrsalida2 = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }
                    }

                    separadorhumedadtemp.Calcular();
                }

                else if (equipos11[indice].tipoequipo2 == 14)
                {
                    for (int j = 0; j < numtipo14; j++)
                    {
                        if (matrixMSR14[j, setnumber].numequipo == equipos11[indice].numequipo2)
                        {
                            MSRtemp.numcorrentrada1  = matrixMSR14[j, setnumber].numcorrentrada1;
						    MSRtemp.numcorrentrada2 = matrixMSR14[j, setnumber].numcorrentrada2;
							MSRtemp.numcorrsalida1  = matrixMSR14[j, setnumber].numcorrsalida1;
							MSRtemp.numcorrsalida2  = matrixMSR14[j, setnumber].numcorrsalida2;
							MSRtemp.numequipo  = matrixMSR14[j, setnumber].numequipo;
                        }
                    }                   

                   //  MSRtemp.numcorrentrada1 = equipos11[indice].aN1;
                   //  MSRtemp.numcorrentrada2 = equipos11[indice].aN2;
                   //  MSRtemp.numcorrsalida1 = equipos11[indice].aN3;
                   //  MSRtemp.numcorrsalida2 = equipos11[indice].aN4;
                   //  MSRtemp.numequipo = equipos11[indice].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 9 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = listaresultadoscorrientes[o, setnumber].Nombre;
                        int longitud1 = nom.Length;
                        String tem = nom.Substring(1, longitud1 - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr ==  MSRtemp.numcorrentrada1)
                        {
                            if (tipoparametro == "W")
                            {
                                 MSRtemp.caudalcorrentrada1 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                 MSRtemp.presioncorrentrada1 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                 MSRtemp.entalpiacorrentrada1 = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }

                        else if (numcorr ==  MSRtemp.numcorrentrada2)
                        {
                            if (tipoparametro == "W")
                            {
                                 MSRtemp.caudalcorrentrada2 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                 MSRtemp.presioncorrentrada2 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                 MSRtemp.entalpiacorrentrada2 = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }

                        else if (numcorr ==  MSRtemp.numcorrsalida1)
                        {
                            if (tipoparametro == "W")
                            {
                                 MSRtemp.caudalcorrsalida1 =listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                 MSRtemp.presioncorrsalida1 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                 MSRtemp.entalpiacorrsalida1 = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }

                        else if (numcorr ==  MSRtemp.numcorrsalida2)
                        {
                            if (tipoparametro == "W")
                            {
                                 MSRtemp.caudalcorrsalida2 =listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                 MSRtemp.presioncorrsalida2 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                 MSRtemp.entalpiacorrsalida2 = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }
                    }
                     MSRtemp.Calcular();
                }

                else if (equipos11[indice].tipoequipo2 == 15)
                {
                    for (int j = 0; j < numtipo15; j++)
                    {
                        if (  matrixcondensador15[j, setnumber].numequipo == equipos11[indice].numequipo2)
                        {
                            condensador15tmp.numcorrentrada1  =   matrixcondensador15[j, setnumber].numcorrentrada1;
							condensador15tmp.numcorrentrada2  =   matrixcondensador15[j, setnumber].numcorrentrada2;
							condensador15tmp.numcorrsalida1  =   matrixcondensador15[j, setnumber].numcorrsalida1;
							condensador15tmp.numcorrsalida2=   matrixcondensador15[j, setnumber].numcorrsalida2;
							condensador15tmp.numequipo =   matrixcondensador15[j, setnumber].numequipo;
                        }
                    }

                    // condensador15tmp.numcorrentrada1 = equipos11[indice].aN1;
                    // condensador15tmp.numcorrentrada2 = equipos11[indice].aN2;
                    // condensador15tmp.numcorrsalida1 = equipos11[indice].aN3;
                    // condensador15tmp.numcorrsalida2 = equipos11[indice].aN4;
                    // condensador15tmp.numequipo = equipos11[indice].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 9 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = listaresultadoscorrientes[o, setnumber].Nombre;
                        int longitud1 = nom.Length;
                        String tem = nom.Substring(1, longitud1 - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr ==  condensador15tmp.numcorrentrada1)
                        {
                            if (tipoparametro == "W")
                            {
                                 condensador15tmp.caudalcorrentrada1 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                 condensador15tmp.presioncorrentrada1 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                 condensador15tmp.entalpiacorrentrada1 = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }

                        else if (numcorr ==  condensador15tmp.numcorrentrada2)
                        {
                            if (tipoparametro == "W")
                            {
                                 condensador15tmp.caudalcorrentrada2 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                 condensador15tmp.presioncorrentrada2 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                 condensador15tmp.entalpiacorrentrada2 = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }

                        else if (numcorr ==  condensador15tmp.numcorrsalida1)
                        {
                            if (tipoparametro == "W")
                            {
                                 condensador15tmp.caudalcorrsalida1 =listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                 condensador15tmp.presioncorrsalida1 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                 condensador15tmp.entalpiacorrsalida1 = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }

                        else if (numcorr ==  condensador15tmp.numcorrsalida2)
                        {
                            if (tipoparametro == "W")
                            {
                                 condensador15tmp.caudalcorrsalida2 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                 condensador15tmp.presioncorrsalida2 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                 condensador15tmp.entalpiacorrsalida2 = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }
                    }
                     condensador15tmp.Calcular();
                }

                else if (equipos11[indice].tipoequipo2 == 16)
                {
                    for (int j = 0; j < numtipo16; j++)
                    {
                        if (matrixenfriador16[j, setnumber].numequipo == equipos11[indice].numequipo2)
                        {
                            enfriadortmp.numcorrentrada1  = matrixenfriador16[j, setnumber].numcorrentrada1;
							enfriadortmp.numcorrentrada2 = matrixenfriador16[j, setnumber].numcorrentrada2;
							enfriadortmp.numcorrsalida1  = matrixenfriador16[j, setnumber].numcorrsalida1;
							enfriadortmp.numcorrsalida2  = matrixenfriador16[j, setnumber].numcorrsalida2;
						    enfriadortmp.numequipo  = matrixenfriador16[j, setnumber].numequipo;							
                        }
                    }
                    
                    //enfriadortmp.numcorrentrada1 = equipos11[indice].aN1;
                    //enfriadortmp.numcorrentrada2 = equipos11[indice].aN2;
                    //enfriadortmp.numcorrsalida1 = equipos11[indice].aN3;
                    //enfriadortmp.numcorrsalida2 = equipos11[indice].aN4;
                    //enfriadortmp.numequipo = equipos11[indice].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 9 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = listaresultadoscorrientes[o, setnumber].Nombre;
                        int longitud1 = nom.Length;
                        String tem = nom.Substring(1, longitud1 - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr == enfriadortmp.numcorrentrada1)
                        {
                            if (tipoparametro == "W")
                            {
                                enfriadortmp.caudalcorrentrada1 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                enfriadortmp.presioncorrentrada1 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                enfriadortmp.entalpiacorrentrada1 = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }

                        else if (numcorr == enfriadortmp.numcorrentrada2)
                        {
                            if (tipoparametro == "W")
                            {
                                enfriadortmp.caudalcorrentrada2 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                enfriadortmp.presioncorrentrada2 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                enfriadortmp.entalpiacorrentrada2 = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }

                        else if (numcorr == enfriadortmp.numcorrsalida1)
                        {
                            if (tipoparametro == "W")
                            {
                                enfriadortmp.caudalcorrsalida1 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                enfriadortmp.presioncorrsalida1 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                enfriadortmp.entalpiacorrsalida1 = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }

                        else if (numcorr == enfriadortmp.numcorrsalida2)
                        {
                            if (tipoparametro == "W")
                            {
                                enfriadortmp.caudalcorrsalida2 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                enfriadortmp.presioncorrsalida2 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                enfriadortmp.entalpiacorrsalida2 = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }
                    }
                    enfriadortmp.Calcular();
                }


                else if (equipos11[indice].tipoequipo2 == 17)
                {
                    for (int j = 0; j < numtipo17; j++)
                    {
                        if (matrixatemperador17[j, setnumber].numequipo == equipos11[indice].numequipo2)
                        {
                            atemperadortmp.numcorrentrada1 = matrixatemperador17[j, setnumber].numcorrentrada1;
							atemperadortmp.numcorrentrada2 = matrixatemperador17[j, setnumber].numcorrentrada2;
							atemperadortmp.numcorrsalida = matrixatemperador17[j, setnumber].numcorrsalida;
							atemperadortmp.numequipo  = matrixatemperador17[j, setnumber].numequipo;				
                        }
                    }
                    
                    //atemperadortmp.numcorrentrada1 = equipos11[indice].aN1;
                    //atemperadortmp.numcorrentrada2 = equipos11[indice].aN2;
                    //atemperadortmp.numcorrsalida = equipos11[indice].aN3;
                    //atemperadortmp.numequipo = equipos11[indice].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 9 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = listaresultadoscorrientes[o, setnumber].Nombre;
                        int longitud1 = nom.Length;
                        String tem = nom.Substring(1, longitud1 - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr == atemperadortmp.numcorrentrada1)
                        {
                            if (tipoparametro == "W")
                            {
                                atemperadortmp.caudalcorrentrada1 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                atemperadortmp.presioncorrentrada1 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                atemperadortmp.entalpiacorrentrada1 = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }

                        else if (numcorr == atemperadortmp.numcorrentrada2)
                        {
                            if (tipoparametro == "W")
                            {
                                atemperadortmp.caudalcorrentrada2 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                atemperadortmp.presioncorrentrada2 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                atemperadortmp.entalpiacorrentrada2 = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }

                        else if (numcorr == atemperadortmp.numcorrsalida)
                        {
                            if (tipoparametro == "W")
                            {
                                atemperadortmp.caudalcorrsalida = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                atemperadortmp.presioncorrsalida = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                atemperadortmp.entalpiacorrsalida = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }
                    }
                    atemperadortmp.Calcular();
                }

                else if (equipos11[indice].tipoequipo2 == 18)
                {
                    for (int j = 0; j < numtipo18; j++)
                    {
                        if (matrixdesaireador18[j, setnumber].numequipo == equipos11[indice].numequipo2)
                        {
                            desaireadortmp.numcorrentrada1  = matrixdesaireador18[j, setnumber].numcorrentrada1;
							desaireadortmp.numcorrentrada2  = matrixdesaireador18[j, setnumber].numcorrentrada2;
							desaireadortmp.numcorrsalida1  = matrixdesaireador18[j, setnumber].numcorrsalida1;
							desaireadortmp.numcorrsalida2  = matrixdesaireador18[j, setnumber].numcorrsalida2;
							desaireadortmp.numequipo = matrixdesaireador18[j, setnumber].numequipo;					
                        }
                    }
                    
                    //desaireadortmp.numcorrentrada1 = equipos11[indice].aN1;
                    //desaireadortmp.numcorrentrada2 = equipos11[indice].aN2;
                    //desaireadortmp.numcorrsalida1 = equipos11[indice].aN3;
                    //desaireadortmp.numcorrsalida2 = equipos11[indice].aN4;
                    //desaireadortmp.numequipo = equipos11[indice].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 9 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = listaresultadoscorrientes[o, setnumber].Nombre;
                        int longitud1 = nom.Length;
                        String tem = nom.Substring(1, longitud1 - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr == desaireadortmp.numcorrentrada1)
                        {
                            if (tipoparametro == "W")
                            {
                                desaireadortmp.caudalcorrentrada1 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                desaireadortmp.presioncorrentrada1 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                desaireadortmp.entalpiacorrentrada1 = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }

                        else if (numcorr == desaireadortmp.numcorrentrada2)
                        {
                            if (tipoparametro == "W")
                            {
                                desaireadortmp.caudalcorrentrada2 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                desaireadortmp.presioncorrentrada2 =listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                desaireadortmp.entalpiacorrentrada2 = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }

                        else if (numcorr == desaireadortmp.numcorrsalida1)
                        {
                            if (tipoparametro == "W")
                            {
                                desaireadortmp.caudalcorrsalida1 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                desaireadortmp.presioncorrsalida1 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                desaireadortmp.entalpiacorrsalida1 = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }

                        else if (numcorr == desaireadortmp.numcorrsalida2)
                        {
                            if (tipoparametro == "W")
                            {
                                desaireadortmp.caudalcorrsalida2 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                desaireadortmp.presioncorrsalida2 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                desaireadortmp.entalpiacorrsalida2 = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }
                    }
                    desaireadortmp.Calcular();
                }

                else if (equipos11[indice].tipoequipo2 == 19)
                {
                    for (int j = 0; j < numtipo19; j++)
                    {
                        if (matrixvalvula19[j, setnumber].numequipo == equipos11[indice].numequipo2)
                        {
                            valvulatmp.numcorrentrada  = matrixvalvula19[j, setnumber].numcorrentrada;
							valvulatmp.numcorrsalida  = matrixvalvula19[j, setnumber].numcorrsalida;
							valvulatmp.numequipo = matrixvalvula19[j, setnumber].numequipo;	
                        }
                    }
                  
                    //valvulatmp.numcorrentrada = equipos11[indice].aN1;
                    //valvulatmp.numcorrsalida = equipos11[indice].aN3;
                    //valvulatmp.numequipo = equipos11[indice].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 9 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = listaresultadoscorrientes[o, setnumber].Nombre;
                        int longitud1 = nom.Length;
                        String tem = nom.Substring(1, longitud1 - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr == valvulatmp.numcorrentrada)
                        {
                            if (tipoparametro == "W")
                            {
                                valvulatmp.caudalcorrentrada = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                valvulatmp.presioncorrentrada = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                valvulatmp.entalpiacorrentrada = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }

                        else if (numcorr == valvulatmp.numcorrsalida)
                        {
                            if (tipoparametro == "W")
                            {
                                valvulatmp.caudalcorrsalida = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                valvulatmp.presioncorrsalida =listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                valvulatmp.entalpiacorrsalida = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }
                    }
                    valvulatmp.Calcular();
                }

                else if (equipos11[indice].tipoequipo2 == 20)
                {
                    for (int j = 0; j < numtipo20; j++)
                    {
                        if (matrixdivisor20[j, setnumber].numequipo == equipos11[indice].numequipo2)
                        {
                            divientalpiatmp.numcorrentrada  = matrixdivisor20[j, setnumber].numcorrentrada;
							divientalpiatmp.numcorrsalida1  = matrixdivisor20[j, setnumber].numcorrsalida1;
							divientalpiatmp.numcorrsalida2 = matrixdivisor20[j, setnumber].numcorrsalida2;
							divientalpiatmp.numequipo  = matrixdivisor20[j, setnumber].numequipo;	
						}
                    }
                  
                     //divientalpiatmp.numcorrentrada = equipos11[indice].aN1;
                     //divientalpiatmp.numcorrsalida1 = equipos11[indice].aN3;
                     //divientalpiatmp.numcorrsalida2 = equipos11[indice].aN4;
                     //divientalpiatmp.numequipo = equipos11[indice].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 9 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom =listaresultadoscorrientes[o, setnumber].Nombre;
                        int longitud1 = nom.Length;
                        String tem = nom.Substring(1, longitud1 - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr ==  divientalpiatmp.numcorrentrada)
                        {
                            if (tipoparametro == "W")
                            {
                                 divientalpiatmp.caudalcorrentrada = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                 divientalpiatmp.presioncorrentrada = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                 divientalpiatmp.entalpiacorrentrada = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }

                        else if (numcorr ==  divientalpiatmp.numcorrsalida1)
                        {
                            if (tipoparametro == "W")
                            {
                                 divientalpiatmp.caudalcorrsalida1 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                 divientalpiatmp.presioncorrsalida1 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                 divientalpiatmp.entalpiacorrsalida1 = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }

                        else if (numcorr ==  divientalpiatmp.numcorrsalida2)
                        {
                            if (tipoparametro == "W")
                            {
                                 divientalpiatmp.caudalcorrsalida2 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                 divientalpiatmp.presioncorrsalida2 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                 divientalpiatmp.entalpiacorrsalida2 = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }
                    }

                     divientalpiatmp.Calcular();
                }

                else if (equipos11[indice].tipoequipo2 == 21)
                {
                    for (int j = 0; j < numtipo21; j++)
                    {
                        if (matrixtanque21[j, setnumber].numequipo == equipos11[indice].numequipo2)
                        {
                            tanquevapotmp.numcorrentrada  = matrixtanque21[j, setnumber].numcorrentrada;
							tanquevapotmp.numcorrsalida1 = matrixtanque21[j, setnumber].numcorrsalida1;
							tanquevapotmp.numcorrsalida2  = matrixtanque21[j, setnumber].numcorrsalida2;
							tanquevapotmp.numequipo = matrixtanque21[j, setnumber].numequipo;	
                        }
                    }
                    
                     //tanquevapotmp.numcorrentrada = equipos11[indice].aN1;
                     //tanquevapotmp.numcorrsalida1 = equipos11[indice].aN3;
                     //tanquevapotmp.numcorrsalida2 = equipos11[indice].aN4;
                     //tanquevapotmp.numequipo = equipos11[indice].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 9 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = listaresultadoscorrientes[o, setnumber].Nombre;
                        int longitud1 = nom.Length;
                        String tem = nom.Substring(1, longitud1 - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr ==  tanquevapotmp.numcorrentrada)
                        {
                            if (tipoparametro == "W")
                            {
                                 tanquevapotmp.caudalcorrentrada =listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                 tanquevapotmp.presioncorrentrada = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                 tanquevapotmp.entalpiacorrentrada = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }

                        else if (numcorr ==  tanquevapotmp.numcorrsalida1)
                        {
                            if (tipoparametro == "W")
                            {
                                 tanquevapotmp.caudalcorrsalida1 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                 tanquevapotmp.presioncorrsalida1 =listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                 tanquevapotmp.entalpiacorrsalida1 = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }

                        else if (numcorr ==  tanquevapotmp.numcorrsalida2)
                        {
                            if (tipoparametro == "W")
                            {
                                 tanquevapotmp.caudalcorrsalida2 =listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                 tanquevapotmp.presioncorrsalida2 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                 tanquevapotmp.entalpiacorrsalida2 = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }
                    }

                     tanquevapotmp.Calcular();
                }

                else if (equipos11[indice].tipoequipo2 == 22)
                {
                    for (int j = 0; j < numtipo22; j++)
                    {
                        if (matrixintercambiador22[j, setnumber].numequipo == equipos11[indice].numequipo2)
                        {
                            intercambiadortmp.numcorrentrada1  = matrixintercambiador22[j, setnumber].numcorrentrada1;
							intercambiadortmp.numcorrentrada2  = matrixintercambiador22[j, setnumber].numcorrentrada2;
							intercambiadortmp.numcorrsalida1  = matrixintercambiador22[j, setnumber].numcorrsalida1;
							intercambiadortmp.numcorrsalida2  = matrixintercambiador22[j, setnumber].numcorrsalida2;
							intercambiadortmp.numequipo  = matrixintercambiador22[j, setnumber].numequipo;						
                        }
                    }                    

                     //intercambiadortmp.numcorrentrada1 = equipos11[indice].aN1;
                     //intercambiadortmp.numcorrentrada2 = equipos11[indice].aN2;
                     //intercambiadortmp.numcorrsalida1 = equipos11[indice].aN3;
                     //intercambiadortmp.numcorrsalida2 = equipos11[indice].aN4;
                     //intercambiadortmp.numequipo = equipos11[indice].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 9 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = listaresultadoscorrientes[o, setnumber].Nombre;
                        int longitud1 = nom.Length;
                        String tem = nom.Substring(1, longitud1 - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr ==  intercambiadortmp.numcorrentrada1)
                        {
                            if (tipoparametro == "W")
                            {
                                 intercambiadortmp.caudalcorrentrada1 =listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                 intercambiadortmp.presioncorrentrada1 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                 intercambiadortmp.entalpiacorrentrada1 = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }

                        else if (numcorr ==  intercambiadortmp.numcorrentrada2)
                        {
                            if (tipoparametro == "W")
                            {
                                 intercambiadortmp.caudalcorrentrada2 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                 intercambiadortmp.presioncorrentrada2 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                 intercambiadortmp.entalpiacorrentrada2 = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }

                        else if (numcorr ==  intercambiadortmp.numcorrsalida1)
                        {
                            if (tipoparametro == "W")
                            {
                                 intercambiadortmp.caudalcorrsalida1 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                 intercambiadortmp.presioncorrsalida1 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                 intercambiadortmp.entalpiacorrsalida1 = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }

                        else if (numcorr ==  intercambiadortmp.numcorrsalida2)
                        {
                            if (tipoparametro == "W")
                            {
                                 intercambiadortmp.caudalcorrsalida2 =listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                 intercambiadortmp.presioncorrsalida2 = listaresultadoscorrientes[o, setnumber].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                 intercambiadortmp.entalpiacorrsalida2 = listaresultadoscorrientes[o, setnumber].Value;
                            }
                        }
                    }
                     intercambiadortmp.Calcular();
                }

                    //Analizamos el Tipo de Equipo de que se trata 
                    if (equipos11[indice].tipoequipo2 == 1)
                    {
                        listBox3.Items.Add("Boundary Condition Equipment, Type 1." + "Equipment Number: " + condtemp.numequipo);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Stream Nº: " + condtemp.numcorrentrada);
                        listBox3.Items.Add("Output Stream Nº: " + condtemp.numcorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Flow: " + condtemp.caudalcorrentrada);
                        listBox3.Items.Add("Output Flow: " + condtemp.caudalcorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Pressure: " + condtemp.presioncorrentrada);
                        listBox3.Items.Add("Output Pressure: " + condtemp.presioncorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Enthalphy: " + condtemp.entalpiacorrentrada);
                        listBox3.Items.Add("Output Enthalpy: " + condtemp.entalpiacorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Entropy: " + condtemp.entropiaentrada);
                        listBox3.Items.Add("Output Entropy: " + condtemp.entropiasalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Temperature: " + condtemp.temperaturaentrada);
                        listBox3.Items.Add("Output Temperature: " + condtemp.temperaturasalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Volumen Específico: " + condtemp.volumenespecificoentrada);
                        listBox3.Items.Add("Output Volumen Específico: " + condtemp.volumenespecificosalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Título: " + condtemp.tituloentrada);
                        listBox3.Items.Add("Output Título: " + condtemp.titulosalida);
                        listBox3.Items.Add("");
                        listBox3.Items.Add("");
                        listBox3.Items.Add("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                    }

                    //Analizamos el Tipo de Equipo de que se trata 
                    else if (equipos11[indice].tipoequipo2 == 2)
                    {
                        listBox3.Items.Add("Divider Equipment, Type 2." + "Equipment Number: " + divisortemp.numequipo);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Stream Nº: " + divisortemp.numcorrentrada);
                        listBox3.Items.Add("Output1 Stream Nº: " + divisortemp.numcorrsalida1);
                        listBox3.Items.Add("Output2 Stream Nº: " + divisortemp.numcorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Flow: " + divisortemp.caudalcorrentrada);
                        listBox3.Items.Add("Output1 Flow: " + divisortemp.caudalcorrsalida1);
                        listBox3.Items.Add("Output2 Flow: " + divisortemp.caudalcorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Pressure: " + divisortemp.presioncorrentrada);
                        listBox3.Items.Add("Output1 Pressure: " + divisortemp.presioncorrsalida1);
                        listBox3.Items.Add("Output2 Pressure: " + divisortemp.presioncorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Enthalphy: " + divisortemp.entalpiacorrentrada);
                        listBox3.Items.Add("Output1 Enthalpy: " + divisortemp.entalpiacorrsalida1);
                        listBox3.Items.Add("Output2 Enthalpy: " + divisortemp.entalpiacorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Entropy: " + divisortemp.entropiaentrada);
                        listBox3.Items.Add("Output1 Entropy: " + divisortemp.entropiasalida1);
                        listBox3.Items.Add("Output2 Entropy: " + divisortemp.entropiasalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Temperature: " + divisortemp.temperaturaentrada);
                        listBox3.Items.Add("Output1 Temperature: " + divisortemp.temperaturasalida1);
                        listBox3.Items.Add("Output2 Temperature: " + divisortemp.temperaturasalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Volumen Específico: " + divisortemp.volumenespecificoentrada);
                        listBox3.Items.Add("Output1 Volumen Epecífico: " + divisortemp.volumenespecificosalida1);
                        listBox3.Items.Add("Output2 Volumen Específico: " + divisortemp.volumenespecificosalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Título: " + divisortemp.tituloentrada);
                        listBox3.Items.Add("Output1 Título: " + divisortemp.titulosalida1);
                        listBox3.Items.Add("Output2 Título: " + divisortemp.titulosalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Flow Input/Output1 ratio: " + divisortemp.porcentajesalida1 + "%");
                        listBox3.Items.Add("Flow Input/Output2 ratio: " + divisortemp.porcentajesalida2 + "%");
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Flow Factor Output1: " + divisortemp.factorflujosalida1);
                        listBox3.Items.Add("Flow Factor Output2: " + divisortemp.factorflujosalida2);
                        listBox3.Items.Add("");
                    }

                    //Analizamos el Tipo de Equipo de que se trata 
                    else if (equipos11[indice].tipoequipo2 == 3)
                    {
                        listBox3.Items.Add("Pressure Loss Equipment, Type 3." + "Equipment Number: " + perdidacargatemp.numequipo);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Stream Nº: " +  perdidacargatemp.numcorrentrada);
                        listBox3.Items.Add("Output Stream Nº: " +  perdidacargatemp.numcorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Flow: " +  perdidacargatemp.caudalcorrentrada);
                        listBox3.Items.Add("Output Flow: " +  perdidacargatemp.caudalcorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Pressure: " +  perdidacargatemp.presioncorrentrada);
                        listBox3.Items.Add("Output Pressure: " +  perdidacargatemp.presioncorrsalida);
                        listBox3.Items.Add("");
                        listBox3.Items.Add("Pressure Drop: " + perdidacargatemp.AP); 
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Enthalphy: " +  perdidacargatemp.entalpiacorrentrada);
                        listBox3.Items.Add("Output Enthalpy: " +  perdidacargatemp.entalpiacorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Entropy: " +  perdidacargatemp.entropiaentrada);
                        listBox3.Items.Add("Output Entropy: " +  perdidacargatemp.entropiasalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Temperature: " +  perdidacargatemp.temperaturaentrada);
                        listBox3.Items.Add("Output Temperature: " +  perdidacargatemp.temperaturasalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Volumen Específico: " +  perdidacargatemp.volumenespecificoentrada);
                        listBox3.Items.Add("Output Volumen Específico: " +  perdidacargatemp.volumenespecificosalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Título: " +  perdidacargatemp.tituloentrada);
                        listBox3.Items.Add("Output Título: " +  perdidacargatemp.titulosalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("");
                        listBox3.Items.Add("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                    }

                    //Analizamos el Tipo de Equipo de que se trata 
                    else if (equipos11[indice].tipoequipo2 == 4)
                    {
                        listBox3.Items.Add("Pump Equipment, Type 4." + "Equipment Number: " + bombatemp.numequipo);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Stream Nº: " + bombatemp.numcorrentrada);
                        listBox3.Items.Add("Output Stream Nº: " + bombatemp.numcorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Flow: " + bombatemp.caudalcorrentrada);
                        listBox3.Items.Add("Output Flow: " + bombatemp.caudalcorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Pressure: " + bombatemp.presioncorrentrada);
                        listBox3.Items.Add("Output Pressure: " + bombatemp.presioncorrsalida);
                        listBox3.Items.Add("");
                        listBox3.Items.Add("Pressure Drop: " + bombatemp.AP);
                        listBox3.Items.Add("");
                        listBox3.Items.Add("TDH (m): " + bombatemp.TDH);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Enthalphy: " + bombatemp.entalpiacorrentrada);
                        listBox3.Items.Add("Output Enthalpy: " + bombatemp.entalpiacorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Entropy: " + bombatemp.entropiaentrada);
                        listBox3.Items.Add("Output Entropy: " + bombatemp.entropiasalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Temperature: " + bombatemp.temperaturaentrada);
                        listBox3.Items.Add("Output Temperature: " + bombatemp.temperaturasalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Volumen Específico: " + bombatemp.volumenespecificoentrada);
                        listBox3.Items.Add("Output Volumen Específico: " + bombatemp.volumenespecificosalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Título: " + bombatemp.tituloentrada);
                        listBox3.Items.Add("Output Título: " + bombatemp.titulosalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Pump Power: " + bombatemp.potencia);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Pump Efficiency: " + bombatemp.eficiencia);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("");
                        listBox3.Items.Add("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                    }

                    //Analizamos el Tipo de Equipo de que se trata 
                    else if (equipos11[indice].tipoequipo2 == 5)
                    {
                        listBox3.Items.Add("Mixer Equipment, Type 5." + "Equipment Number: " + mezcladortemp.numequipo);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Stream Nº: " + mezcladortemp.numcorrentrada1);
                        listBox3.Items.Add("Input2 Stream Nº: " + mezcladortemp.numcorrentrada2);
                        listBox3.Items.Add("Output Stream Nº: " + mezcladortemp.numcorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Flow: " + mezcladortemp.caudalcorrentrada1);
                        listBox3.Items.Add("Input2 Flow: " + mezcladortemp.caudalcorrentrada2);
                        listBox3.Items.Add("Output Flow: " + mezcladortemp.caudalcorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Pressure: " + mezcladortemp.presioncorrentrada1);
                        listBox3.Items.Add("Input2 Pressure: " + mezcladortemp.presioncorrentrada2);
                        listBox3.Items.Add("Output Pressure: " + mezcladortemp.presioncorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Enthalphy: " + mezcladortemp.entalpiacorrentrada1);
                        listBox3.Items.Add("Input2 Enthalpy: " + mezcladortemp.entalpiacorrentrada2);
                        listBox3.Items.Add("Output Enthalpy: " + mezcladortemp.entalpiacorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Entropy: " + mezcladortemp.entropiaentrada1);
                        listBox3.Items.Add("Input2 Entropy: " + mezcladortemp.entropiaentrada2);
                        listBox3.Items.Add("Output Entropy: " + mezcladortemp.entropiasalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Temperature: " + mezcladortemp.temperaturaentrada1);
                        listBox3.Items.Add("Input2 Temperature: " + mezcladortemp.temperaturaentrada2);
                        listBox3.Items.Add("Output Temperature: " + mezcladortemp.temperaturasalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Volumen Específico: " + mezcladortemp.volumenespecificoentrada1);
                        listBox3.Items.Add("Input2 Volumen Epecífico: " + mezcladortemp.volumenespecificoentrada2);
                        listBox3.Items.Add("Output Volumen Específico: " + mezcladortemp.volumenespecificosalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Título: " + mezcladortemp.tituloentrada1);
                        listBox3.Items.Add("Input2 Título: " + mezcladortemp.tituloentrada2);
                        listBox3.Items.Add("Output Título: " + mezcladortemp.titulosalida);
                        listBox3.Items.Add("");
                    }

                    //Analizamos el Tipo de Equipo de que se trata 
                    else if (equipos11[indice].tipoequipo2 == 6)
                    {
                        listBox3.Items.Add("Reactor Equipment, Type 6." + "Equipment Number: " + reactortemp.numequipo);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Stream Nº: " + reactortemp.numcorrentrada);
                        listBox3.Items.Add("Output Stream Nº: " + reactortemp.numcorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Flow: " + reactortemp.caudalcorrentrada);
                        listBox3.Items.Add("Output Flow: " + reactortemp.caudalcorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Pressure: " + reactortemp.presioncorrentrada);
                        listBox3.Items.Add("Output Pressure: " + reactortemp.presioncorrsalida);
                        listBox3.Items.Add("");
                        listBox3.Items.Add("Pressure Drop: " + reactortemp.AP);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Enthalphy: " + reactortemp.entalpiacorrentrada);
                        listBox3.Items.Add("Output Enthalpy: " + reactortemp.entalpiacorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Entropy: " + reactortemp.entropiaentrada);
                        listBox3.Items.Add("Output Entropy: " + reactortemp.entropiasalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Temperature: " + reactortemp.temperaturaentrada);
                        listBox3.Items.Add("Output Temperature: " + reactortemp.temperaturasalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Volumen Específico: " + reactortemp.volumenespecificoentrada);
                        listBox3.Items.Add("Output Volumen Específico: " + reactortemp.volumenespecificosalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Título: " + reactortemp.tituloentrada);
                        listBox3.Items.Add("Output Título: " + reactortemp.titulosalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("");
                        listBox3.Items.Add("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                    }

                    else if (equipos11[indice].tipoequipo2 == 7)
                    {
                        listBox3.Items.Add("FeedWaterHeater Equipment, Type 7." + "Equipment Number: " + calentadortemp.numequipo);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Stream Nº: " + calentadortemp.numcorrentrada1);
                        listBox3.Items.Add("Input2 Stream Nº: " + calentadortemp.numcorrentrada2);
                        listBox3.Items.Add("Output1 Stream Nº: " + calentadortemp.numcorrsalida1);
                        listBox3.Items.Add("Output2 Stream Nº: " + calentadortemp.numcorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Flow: " + calentadortemp.caudalcorrentrada1);
                        listBox3.Items.Add("Input2 Flow: " + calentadortemp.caudalcorrentrada2);
                        listBox3.Items.Add("Output1 Flow: " + calentadortemp.caudalcorrsalida1);
                        listBox3.Items.Add("Output2 Flow: " + calentadortemp.caudalcorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Pressure: " + calentadortemp.presioncorrentrada1);
                        listBox3.Items.Add("Input2 Pressure: " + calentadortemp.presioncorrentrada2);
                        listBox3.Items.Add("Output1 Pressure: " + calentadortemp.presioncorrsalida1);
                        listBox3.Items.Add("Output2 Pressure: " + calentadortemp.presioncorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Enthalphy: " + calentadortemp.entalpiacorrentrada1);
                        listBox3.Items.Add("Input2 Enthalphy: " + calentadortemp.entalpiacorrentrada2);
                        listBox3.Items.Add("Output1 Enthalpy: " + calentadortemp.entalpiacorrsalida1);
                        listBox3.Items.Add("Output2 Enthalpy: " + calentadortemp.entalpiacorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Entropy: " + calentadortemp.entropiaentrada1);
                        listBox3.Items.Add("Input2 Entropy: " + calentadortemp.entropiaentrada2);
                        listBox3.Items.Add("Output1 Entropy: " + calentadortemp.entropiasalida1);
                        listBox3.Items.Add("Output2 Entropy: " + calentadortemp.entropiasalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Temperature: " + calentadortemp.temperaturaentrada1);
                        listBox3.Items.Add("Input2 Temperature: " + calentadortemp.temperaturaentrada2);
                        listBox3.Items.Add("Output1 Temperature: " + calentadortemp.temperaturasalida1);
                        listBox3.Items.Add("Output2 Temperature: " + calentadortemp.temperaturasalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Volumen Específico: " + calentadortemp.volumenespecificoentrada1);
                        listBox3.Items.Add("Input2 Volumen Específico: " + calentadortemp.volumenespecificoentrada2);
                        listBox3.Items.Add("Output1 Volumen Específico: " + calentadortemp.volumenespecificosalida1);
                        listBox3.Items.Add("Output2 Volumen Específico: " + calentadortemp.volumenespecificosalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Título: " + calentadortemp.tituloentrada1);
                        listBox3.Items.Add("Input2 Título: " + calentadortemp.tituloentrada2);
                        listBox3.Items.Add("Output1 Título: " + calentadortemp.titulosalida1);
                        listBox3.Items.Add("Output2 Título: " + calentadortemp.titulosalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("TTD: " + calentadortemp.TTD);
                        listBox3.Items.Add("DCA: " + calentadortemp.DCA);

                        listBox3.Items.Add("");
                        listBox3.Items.Add("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                    }

                    else if (equipos11[indice].tipoequipo2 == 8)
                    {
                        listBox3.Items.Add("Main Condenser Equipment, Type 8." + "Equipment Number: " +condensadortemp.numequipo);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Stream Nº: " + condensadortemp.numcorrentrada1);
                        listBox3.Items.Add("Input2 Stream Nº: " + condensadortemp.numcorrentrada2);
                        listBox3.Items.Add("Output Stream Nº: " + condensadortemp.numcorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Flow: " + condensadortemp.caudalcorrentrada1);
                        listBox3.Items.Add("Input2 Flow: " + condensadortemp.caudalcorrentrada2);
                        listBox3.Items.Add("Output Flow: " + condensadortemp.caudalcorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Pressure: " + condensadortemp.presioncorrentrada1);
                        listBox3.Items.Add("Input2 Pressure: " + condensadortemp.presioncorrentrada2);
                        listBox3.Items.Add("Output Pressure: " + condensadortemp.presioncorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Enthalphy: " + condensadortemp.entalpiacorrentrada1);
                        listBox3.Items.Add("Input2 Enthalphy: " + condensadortemp.entalpiacorrentrada2);
                        listBox3.Items.Add("Output Enthalpy: " + condensadortemp.entalpiacorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Entropy: " + condensadortemp.entropiaentrada1);
                        listBox3.Items.Add("Input2 Entropy: " + condensadortemp.entropiaentrada2);
                        listBox3.Items.Add("Output Entropy: " + condensadortemp.entropiasalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Temperature: " + condensadortemp.temperaturaentrada1);
                        listBox3.Items.Add("Input2 Temperature: " + condensadortemp.temperaturaentrada2);
                        listBox3.Items.Add("Output Temperature: " + condensadortemp.temperaturasalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Volumen Específico: " + condensadortemp.volumenespecificoentrada1);
                        listBox3.Items.Add("Input2 Volumen Específico: " + condensadortemp.volumenespecificoentrada2);
                        listBox3.Items.Add("Output Volumen Específico: " + condensadortemp.volumenespecificosalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Título: " + condensadortemp.tituloentrada1);
                        listBox3.Items.Add("Input2 Título: " + condensadortemp.tituloentrada2);
                        listBox3.Items.Add("Output Título: " + condensadortemp.titulosalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("");
                        listBox3.Items.Add("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                    }

                    //Analizamos el Tipo de Equipo de que se trata 
                    else if (equipos11[indice].tipoequipo2 == 9)
                    {
                        listBox3.Items.Add("Turbine9 Equipment, Type 9." + "Equipment Number: " + turbina9temp.numequipo);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Stream Nº: " + turbina9temp.numcorrentrada);
                        listBox3.Items.Add("Output Stream Nº: " + turbina9temp.numcorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Flow: " + turbina9temp.caudalcorrentrada);
                        listBox3.Items.Add("Output Flow: " + turbina9temp.caudalcorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Pressure: " + turbina9temp.presioncorrentrada);
                        listBox3.Items.Add("Output Pressure: " + turbina9temp.presioncorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Enthalphy: " + turbina9temp.entalpiacorrentrada);
                        listBox3.Items.Add("Output Enthalpy: " + turbina9temp.entalpiacorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Entropy: " + turbina9temp.entropiaentrada);
                        listBox3.Items.Add("Output Entropy: " + turbina9temp.entropiasalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Temperature: " + turbina9temp.temperaturaentrada);
                        listBox3.Items.Add("Output Temperature: " + turbina9temp.temperaturasalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Volumen Específico: " + turbina9temp.volumenespecificoentrada);
                        listBox3.Items.Add("Output Volumen Específico: " + turbina9temp.volumenespecificosalida);
                        listBox3.Items.Add("");                     

                        listBox3.Items.Add("Input Título: " + turbina9temp.tituloentrada);
                        listBox3.Items.Add("Output Título: " + turbina9temp.titulosalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Eficiencia: " + turbina9temp.eficiencia);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Potencia: " + turbina9temp.potencia);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Relación Presiones: " + turbina9temp.relacionpresiones);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Factor Flujo: " + turbina9temp.factorflujo);
                        listBox3.Items.Add("");
                        
                        listBox3.Items.Add("");
                        listBox3.Items.Add("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                    }

                    //Analizamos el Tipo de Equipo de que se trata 
                    else if (equipos11[indice].tipoequipo2 == 10)
                    {
                        listBox3.Items.Add("Turbine10 Equipment, Type 10." + "Equipment Number: " + turbina10temp.numequipo);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Stream Nº: " + turbina10temp.numcorrentrada);
                        listBox3.Items.Add("Output Stream Nº: " + turbina10temp.numcorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Flow: " + turbina10temp.caudalcorrentrada);
                        listBox3.Items.Add("Output Flow: " + turbina10temp.caudalcorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Pressure: " + turbina10temp.presioncorrentrada);
                        listBox3.Items.Add("Output Pressure: " + turbina10temp.presioncorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Enthalphy: " + turbina10temp.entalpiacorrentrada);
                        listBox3.Items.Add("Output Enthalpy: " + turbina10temp.entalpiacorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Entropy: " + turbina10temp.entropiaentrada);
                        listBox3.Items.Add("Output Entropy: " + turbina10temp.entropiasalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Temperature: " + turbina10temp.temperaturaentrada);
                        listBox3.Items.Add("Output Temperature: " + turbina10temp.temperaturasalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Volumen Específico: " + turbina10temp.volumenespecificoentrada);
                        listBox3.Items.Add("Output Volumen Específico: " + turbina10temp.volumenespecificosalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Título: " + turbina10temp.tituloentrada);
                        listBox3.Items.Add("Output Título: " + turbina10temp.titulosalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Eficiencia: " + turbina10temp.eficiencia);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Potencia: " + turbina10temp.potencia);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Relación Presiones: " + turbina10temp.relacionpresiones);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Factor Flujo: " + turbina10temp.factorflujo);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("");
                        listBox3.Items.Add("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                    }

                    //Analizamos el Tipo de Equipo de que se trata 
                    else if (equipos11[indice].tipoequipo2 == 11)
                    {
                        listBox3.Items.Add("Auxiliary Turbine Equipment, Type 11." + "Equipment Number: " + turbina11temp.numequipo);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Stream Nº: " + turbina11temp.numcorrentrada);
                        listBox3.Items.Add("Output Stream Nº: " + turbina11temp.numcorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Flow: " + turbina11temp.caudalcorrentrada);
                        listBox3.Items.Add("Output Flow: " + turbina11temp.caudalcorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Pressure: " + turbina11temp.presioncorrentrada);
                        listBox3.Items.Add("Output Pressure: " + turbina11temp.presioncorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Enthalphy: " + turbina11temp.entalpiacorrentrada);
                        listBox3.Items.Add("Output Enthalpy: " + turbina11temp.entalpiacorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Entropy: " + turbina11temp.entropiaentrada);
                        listBox3.Items.Add("Output Entropy: " + turbina11temp.entropiasalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Temperature: " + turbina11temp.temperaturaentrada);
                        listBox3.Items.Add("Output Temperature: " + turbina11temp.temperaturasalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Volumen Específico: " + turbina11temp.volumenespecificoentrada);
                        listBox3.Items.Add("Output Volumen Específico: " + turbina11temp.volumenespecificosalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Título: " + turbina11temp.tituloentrada);
                        listBox3.Items.Add("Output Título: " + turbina11temp.titulosalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Eficiencia: " + turbina11temp.eficiencia);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Potencia: " + turbina11temp.potencia);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Relación Presiones: " + turbina11temp.relacionpresiones);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Factor Flujo: " + turbina11temp.factorflujo);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("");
                        listBox3.Items.Add("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                    }

                   //Analizamos el Tipo de Equipo de que se trata 
                    else if (equipos11[indice].tipoequipo2 == 13)
                    {
                        listBox3.Items.Add("Moisture Separator Equipment, Type 13." + "Equipment Number: " + separadorhumedadtemp.numequipo);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Stream Nº: " + separadorhumedadtemp.numcorrentrada);
                        listBox3.Items.Add("Output1 Stream Nº: " + separadorhumedadtemp.numcorrsalida1);
                        listBox3.Items.Add("Output2 Stream Nº: " + separadorhumedadtemp.numcorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Flow: " + separadorhumedadtemp.caudalcorrentrada);
                        listBox3.Items.Add("Output1 Flow: " + separadorhumedadtemp.caudalcorrsalida1);
                        listBox3.Items.Add("Output2 Flow: " + separadorhumedadtemp.caudalcorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Pressure: " + separadorhumedadtemp.presioncorrentrada);
                        listBox3.Items.Add("Output1 Pressure: " + separadorhumedadtemp.presioncorrsalida1);
                        listBox3.Items.Add("Output2 Pressure: " + separadorhumedadtemp.presioncorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Enthalphy: " + separadorhumedadtemp.entalpiacorrentrada);
                        listBox3.Items.Add("Output1 Enthalpy: " + separadorhumedadtemp.entalpiacorrsalida1);
                        listBox3.Items.Add("Output2 Enthalpy: " + separadorhumedadtemp.entalpiacorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Entropy: " + separadorhumedadtemp.entropiaentrada);
                        listBox3.Items.Add("Output1 Entropy: " + separadorhumedadtemp.entropiasalida1);
                        listBox3.Items.Add("Output2 Entropy: " + separadorhumedadtemp.entropiasalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Temperature: " + separadorhumedadtemp.temperaturaentrada);
                        listBox3.Items.Add("Output1 Temperature: " + separadorhumedadtemp.temperaturasalida1);
                        listBox3.Items.Add("Output2 Temperature: " + separadorhumedadtemp.temperaturasalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Volumen Específico: " + separadorhumedadtemp.volumenespecificoentrada);
                        listBox3.Items.Add("Output1 Volumen Epecífico: " + separadorhumedadtemp.volumenespecificosalida1);
                        listBox3.Items.Add("Output2 Volumen Específico: " + separadorhumedadtemp.volumenespecificosalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Título: " + separadorhumedadtemp.tituloentrada);
                        listBox3.Items.Add("Output1 Título: " + separadorhumedadtemp.titulosalida1);
                        listBox3.Items.Add("Output2 Título: " + separadorhumedadtemp.titulosalida2);
                        listBox3.Items.Add("");                    
                    }

                    else if (equipos11[indice].tipoequipo2 == 14)
                    {
                        listBox3.Items.Add("MSR Equipment, Type 14." + "Equipment Number: " + MSRtemp.numequipo);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Stream Nº: " + MSRtemp.numcorrentrada1);
                        listBox3.Items.Add("Input2 Stream Nº: " + MSRtemp.numcorrentrada2);
                        listBox3.Items.Add("Output1 Stream Nº: " + MSRtemp.numcorrsalida1);
                        listBox3.Items.Add("Output2 Stream Nº: " + MSRtemp.numcorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Flow: " + MSRtemp.caudalcorrentrada1);
                        listBox3.Items.Add("Input2 Flow: " + MSRtemp.caudalcorrentrada2);
                        listBox3.Items.Add("Output1 Flow: " + MSRtemp.caudalcorrsalida1);
                        listBox3.Items.Add("Output2 Flow: " + MSRtemp.caudalcorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Pressure: " + MSRtemp.presioncorrentrada1);
                        listBox3.Items.Add("Input2 Pressure: " + MSRtemp.presioncorrentrada2);
                        listBox3.Items.Add("Output1 Pressure: " + MSRtemp.presioncorrsalida1);
                        listBox3.Items.Add("Output2 Pressure: " + MSRtemp.presioncorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Enthalphy: " + MSRtemp.entalpiacorrentrada1);
                        listBox3.Items.Add("Input2 Enthalphy: " + MSRtemp.entalpiacorrentrada2);
                        listBox3.Items.Add("Output1 Enthalpy: " + MSRtemp.entalpiacorrsalida1);
                        listBox3.Items.Add("Output2 Enthalpy: " + MSRtemp.entalpiacorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Entropy: " + MSRtemp.entropiaentrada1);
                        listBox3.Items.Add("Input2 Entropy: " + MSRtemp.entropiaentrada2);
                        listBox3.Items.Add("Output1 Entropy: " + MSRtemp.entropiasalida1);
                        listBox3.Items.Add("Output2 Entropy: " + MSRtemp.entropiasalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Temperature: " + MSRtemp.temperaturaentrada1);
                        listBox3.Items.Add("Input2 Temperature: " + MSRtemp.temperaturaentrada2);
                        listBox3.Items.Add("Output1 Temperature: " + MSRtemp.temperaturasalida1);
                        listBox3.Items.Add("Output2 Temperature: " + MSRtemp.temperaturasalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Volumen Específico: " + MSRtemp.volumenespecificoentrada1);
                        listBox3.Items.Add("Input2 Volumen Específico: " + MSRtemp.volumenespecificoentrada2);
                        listBox3.Items.Add("Output1 Volumen Específico: " + MSRtemp.volumenespecificosalida1);
                        listBox3.Items.Add("Output2 Volumen Específico: " + MSRtemp.volumenespecificosalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Título: " + MSRtemp.tituloentrada1);
                        listBox3.Items.Add("Input2 Título: " + MSRtemp.tituloentrada2);
                        listBox3.Items.Add("Output1 Título: " + MSRtemp.titulosalida1);
                        listBox3.Items.Add("Output2 Título: " + MSRtemp.titulosalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("TTD: " + MSRtemp.TTD);
                       
                        listBox3.Items.Add("");
                        listBox3.Items.Add("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                    }

                    else if (equipos11[indice].tipoequipo2 == 15)
                    {
                        listBox3.Items.Add("Condensator Equipment, Type 15." + "Equipment Number: " +  condensador15tmp.numequipo);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Stream Nº: " +  condensador15tmp.numcorrentrada1);
                        listBox3.Items.Add("Input2 Stream Nº: " +  condensador15tmp.numcorrentrada2);
                        listBox3.Items.Add("Output1 Stream Nº: " +  condensador15tmp.numcorrsalida1);
                        listBox3.Items.Add("Output2 Stream Nº: " +  condensador15tmp.numcorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Flow: " +  condensador15tmp.caudalcorrentrada1);
                        listBox3.Items.Add("Input2 Flow: " +  condensador15tmp.caudalcorrentrada2);
                        listBox3.Items.Add("Output1 Flow: " +  condensador15tmp.caudalcorrsalida1);
                        listBox3.Items.Add("Output2 Flow: " +  condensador15tmp.caudalcorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Pressure: " +  condensador15tmp.presioncorrentrada1);
                        listBox3.Items.Add("Input2 Pressure: " +  condensador15tmp.presioncorrentrada2);
                        listBox3.Items.Add("Output1 Pressure: " +  condensador15tmp.presioncorrsalida1);
                        listBox3.Items.Add("Output2 Pressure: " +  condensador15tmp.presioncorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Enthalphy: " +  condensador15tmp.entalpiacorrentrada1);
                        listBox3.Items.Add("Input2 Enthalphy: " +  condensador15tmp.entalpiacorrentrada2);
                        listBox3.Items.Add("Output1 Enthalpy: " +  condensador15tmp.entalpiacorrsalida1);
                        listBox3.Items.Add("Output2 Enthalpy: " +  condensador15tmp.entalpiacorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Entropy: " +  condensador15tmp.entropiaentrada1);
                        listBox3.Items.Add("Input2 Entropy: " +  condensador15tmp.entropiaentrada2);
                        listBox3.Items.Add("Output1 Entropy: " +  condensador15tmp.entropiasalida1);
                        listBox3.Items.Add("Output2 Entropy: " +  condensador15tmp.entropiasalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Temperature: " +  condensador15tmp.temperaturaentrada1);
                        listBox3.Items.Add("Input2 Temperature: " +  condensador15tmp.temperaturaentrada2);
                        listBox3.Items.Add("Output1 Temperature: " +  condensador15tmp.temperaturasalida1);
                        listBox3.Items.Add("Output2 Temperature: " +  condensador15tmp.temperaturasalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Volumen Específico: " +  condensador15tmp.volumenespecificoentrada1);
                        listBox3.Items.Add("Input2 Volumen Específico: " +  condensador15tmp.volumenespecificoentrada2);
                        listBox3.Items.Add("Output1 Volumen Específico: " +  condensador15tmp.volumenespecificosalida1);
                        listBox3.Items.Add("Output2 Volumen Específico: " +  condensador15tmp.volumenespecificosalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Título: " +  condensador15tmp.tituloentrada1);
                        listBox3.Items.Add("Input2 Título: " +  condensador15tmp.tituloentrada2);
                        listBox3.Items.Add("Output1 Título: " +  condensador15tmp.titulosalida1);
                        listBox3.Items.Add("Output2 Título: " +  condensador15tmp.titulosalida2);
                        listBox3.Items.Add("");
                       
                        listBox3.Items.Add("");
                        listBox3.Items.Add("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                    }

                    else if (equipos11[indice].tipoequipo2 == 16)
                    {
                        listBox3.Items.Add("Drainage Cooling Equipment, Type 16." + "Equipment Number: " + enfriadortmp.numequipo);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Stream Nº: " + enfriadortmp.numcorrentrada1);
                        listBox3.Items.Add("Input2 Stream Nº: " + enfriadortmp.numcorrentrada2);
                        listBox3.Items.Add("Output1 Stream Nº: " + enfriadortmp.numcorrsalida1);
                        listBox3.Items.Add("Output2 Stream Nº: " + enfriadortmp.numcorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Flow: " + enfriadortmp.caudalcorrentrada1);
                        listBox3.Items.Add("Input2 Flow: " + enfriadortmp.caudalcorrentrada2);
                        listBox3.Items.Add("Output1 Flow: " + enfriadortmp.caudalcorrsalida1);
                        listBox3.Items.Add("Output2 Flow: " + enfriadortmp.caudalcorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Pressure: " + enfriadortmp.presioncorrentrada1);
                        listBox3.Items.Add("Input2 Pressure: " + enfriadortmp.presioncorrentrada2);
                        listBox3.Items.Add("Output1 Pressure: " + enfriadortmp.presioncorrsalida1);
                        listBox3.Items.Add("Output2 Pressure: " + enfriadortmp.presioncorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Enthalphy: " + enfriadortmp.entalpiacorrentrada1);
                        listBox3.Items.Add("Input2 Enthalphy: " + enfriadortmp.entalpiacorrentrada2);
                        listBox3.Items.Add("Output1 Enthalpy: " + enfriadortmp.entalpiacorrsalida1);
                        listBox3.Items.Add("Output2 Enthalpy: " + enfriadortmp.entalpiacorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Entropy: " + enfriadortmp.entropiaentrada1);
                        listBox3.Items.Add("Input2 Entropy: " + enfriadortmp.entropiaentrada2);
                        listBox3.Items.Add("Output1 Entropy: " + enfriadortmp.entropiasalida1);
                        listBox3.Items.Add("Output2 Entropy: " + enfriadortmp.entropiasalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Temperature: " + enfriadortmp.temperaturaentrada1);
                        listBox3.Items.Add("Input2 Temperature: " + enfriadortmp.temperaturaentrada2);
                        listBox3.Items.Add("Output1 Temperature: " + enfriadortmp.temperaturasalida1);
                        listBox3.Items.Add("Output2 Temperature: " + enfriadortmp.temperaturasalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Volumen Específico: " + enfriadortmp.volumenespecificoentrada1);
                        listBox3.Items.Add("Input2 Volumen Específico: " + enfriadortmp.volumenespecificoentrada2);
                        listBox3.Items.Add("Output1 Volumen Específico: " + enfriadortmp.volumenespecificosalida1);
                        listBox3.Items.Add("Output2 Volumen Específico: " + enfriadortmp.volumenespecificosalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Título: " + enfriadortmp.tituloentrada1);
                        listBox3.Items.Add("Input2 Título: " + enfriadortmp.tituloentrada2);
                        listBox3.Items.Add("Output1 Título: " + enfriadortmp.titulosalida1);
                        listBox3.Items.Add("Output2 Título: " + enfriadortmp.titulosalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("TTD: " + enfriadortmp.TTD);
                        listBox3.Items.Add("DCA: " + enfriadortmp.DCA);

                        listBox3.Items.Add("");
                        listBox3.Items.Add("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                    }

                    //Analizamos el Tipo de Equipo de que se trata 
                    else if (equipos11[indice].tipoequipo2 == 17)
                    {
                        listBox3.Items.Add("DesuperHeater Equipment, Type 17." + "Equipment Number: " + atemperadortmp.numequipo);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Stream Nº: " + atemperadortmp.numcorrentrada1);
                        listBox3.Items.Add("Input2 Stream Nº: " + atemperadortmp.numcorrentrada2);
                        listBox3.Items.Add("Output Stream Nº: " + atemperadortmp.numcorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Flow: " + atemperadortmp.caudalcorrentrada1);
                        listBox3.Items.Add("Input2 Flow: " + atemperadortmp.caudalcorrentrada2);
                        listBox3.Items.Add("Output Flow: " + atemperadortmp.caudalcorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Pressure: " + atemperadortmp.presioncorrentrada1);
                        listBox3.Items.Add("Input2 Pressure: " + atemperadortmp.presioncorrentrada2);
                        listBox3.Items.Add("Output Pressure: " + atemperadortmp.presioncorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Enthalphy: " + atemperadortmp.entalpiacorrentrada1);
                        listBox3.Items.Add("Input2 Enthalpy: " + atemperadortmp.entalpiacorrentrada2);
                        listBox3.Items.Add("Output Enthalpy: " + atemperadortmp.entalpiacorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Entropy: " + atemperadortmp.entropiaentrada1);
                        listBox3.Items.Add("Input2 Entropy: " + atemperadortmp.entropiaentrada2);
                        listBox3.Items.Add("Output Entropy: " + atemperadortmp.entropiasalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Temperature: " + atemperadortmp.temperaturaentrada1);
                        listBox3.Items.Add("Input2 Temperature: " + atemperadortmp.temperaturaentrada2);
                        listBox3.Items.Add("Output Temperature: " + atemperadortmp.temperaturasalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Volumen Específico: " + atemperadortmp.volumenespecificoentrada1);
                        listBox3.Items.Add("Input2 Volumen Epecífico: " + atemperadortmp.volumenespecificoentrada2);
                        listBox3.Items.Add("Output Volumen Específico: " + atemperadortmp.volumenespecificosalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Título: " + atemperadortmp.tituloentrada1);
                        listBox3.Items.Add("Input2 Título: " + atemperadortmp.tituloentrada2);
                        listBox3.Items.Add("Output Título: " + atemperadortmp.titulosalida);
                        listBox3.Items.Add("");
                    }

                    else if (equipos11[indice].tipoequipo2 == 18)
                    {
                        listBox3.Items.Add("Deaireator Equipment, Type 18." + "Equipment Number: " +  desaireadortmp.numequipo);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Stream Nº: " +  desaireadortmp.numcorrentrada1);
                        listBox3.Items.Add("Input2 Stream Nº: " +  desaireadortmp.numcorrentrada2);
                        listBox3.Items.Add("Output1 Stream Nº: " +  desaireadortmp.numcorrsalida1);
                        listBox3.Items.Add("Output2 Stream Nº: " +  desaireadortmp.numcorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Flow: " +  desaireadortmp.caudalcorrentrada1);
                        listBox3.Items.Add("Input2 Flow: " +  desaireadortmp.caudalcorrentrada2);
                        listBox3.Items.Add("Output1 Flow: " +  desaireadortmp.caudalcorrsalida1);
                        listBox3.Items.Add("Output2 Flow: " +  desaireadortmp.caudalcorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Pressure: " +  desaireadortmp.presioncorrentrada1);
                        listBox3.Items.Add("Input2 Pressure: " +  desaireadortmp.presioncorrentrada2);
                        listBox3.Items.Add("Output1 Pressure: " +  desaireadortmp.presioncorrsalida1);
                        listBox3.Items.Add("Output2 Pressure: " +  desaireadortmp.presioncorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Enthalphy: " +  desaireadortmp.entalpiacorrentrada1);
                        listBox3.Items.Add("Input2 Enthalphy: " +  desaireadortmp.entalpiacorrentrada2);
                        listBox3.Items.Add("Output1 Enthalpy: " +  desaireadortmp.entalpiacorrsalida1);
                        listBox3.Items.Add("Output2 Enthalpy: " +  desaireadortmp.entalpiacorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Entropy: " +  desaireadortmp.entropiaentrada1);
                        listBox3.Items.Add("Input2 Entropy: " +  desaireadortmp.entropiaentrada2);
                        listBox3.Items.Add("Output1 Entropy: " +  desaireadortmp.entropiasalida1);
                        listBox3.Items.Add("Output2 Entropy: " +  desaireadortmp.entropiasalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Temperature: " +  desaireadortmp.temperaturaentrada1);
                        listBox3.Items.Add("Input2 Temperature: " +  desaireadortmp.temperaturaentrada2);
                        listBox3.Items.Add("Output1 Temperature: " +  desaireadortmp.temperaturasalida1);
                        listBox3.Items.Add("Output2 Temperature: " +  desaireadortmp.temperaturasalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Volumen Específico: " +  desaireadortmp.volumenespecificoentrada1);
                        listBox3.Items.Add("Input2 Volumen Específico: " +  desaireadortmp.volumenespecificoentrada2);
                        listBox3.Items.Add("Output1 Volumen Específico: " +  desaireadortmp.volumenespecificosalida1);
                        listBox3.Items.Add("Output2 Volumen Específico: " +  desaireadortmp.volumenespecificosalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Título: " +  desaireadortmp.tituloentrada1);
                        listBox3.Items.Add("Input2 Título: " +  desaireadortmp.tituloentrada2);
                        listBox3.Items.Add("Output1 Título: " +  desaireadortmp.titulosalida1);
                        listBox3.Items.Add("Output2 Título: " +  desaireadortmp.titulosalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("TTD: " +  desaireadortmp.TTD);
                        listBox3.Items.Add("DCA: " +  desaireadortmp.DCA);

                        listBox3.Items.Add("");
                        listBox3.Items.Add("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                    }

                    //Analizamos el Tipo de Equipo de que se trata 
                    else if (equipos11[indice].tipoequipo2 == 19)
                    {
                        listBox3.Items.Add("Valve Equipment, Type 19." + "Equipment Number: " +  valvulatmp.numequipo);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Stream Nº: " +  valvulatmp.numcorrentrada);
                        listBox3.Items.Add("Output Stream Nº: " +  valvulatmp.numcorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Flow: " +  valvulatmp.caudalcorrentrada);
                        listBox3.Items.Add("Output Flow: " +  valvulatmp.caudalcorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Pressure: " +  valvulatmp.presioncorrentrada);
                        listBox3.Items.Add("Output Pressure: " +  valvulatmp.presioncorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Enthalphy: " +  valvulatmp.entalpiacorrentrada);
                        listBox3.Items.Add("Output Enthalpy: " +  valvulatmp.entalpiacorrsalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Entropy: " +  valvulatmp.entropiaentrada);
                        listBox3.Items.Add("Output Entropy: " +  valvulatmp.entropiasalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Temperature: " +  valvulatmp.temperaturaentrada);
                        listBox3.Items.Add("Output Temperature: " +  valvulatmp.temperaturasalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Volumen Específico: " +  valvulatmp.volumenespecificoentrada);
                        listBox3.Items.Add("Output Volumen Específico: " +  valvulatmp.volumenespecificosalida);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Título: " +  valvulatmp.tituloentrada);
                        listBox3.Items.Add("Output Título: " +  valvulatmp.titulosalida);
                        listBox3.Items.Add("");
                        listBox3.Items.Add("");
                        listBox3.Items.Add("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                    }

                    //Analizamos el Tipo de Equipo de que se trata 
                    else if (equipos11[indice].tipoequipo2 == 20)
                    {
                        listBox3.Items.Add("Fixed Entalpht Divider Equipment, Type 20." + "Equipment Number: " +  divientalpiatmp.numequipo);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Stream Nº: " +  divientalpiatmp.numcorrentrada);
                        listBox3.Items.Add("Output1 Stream Nº: " +  divientalpiatmp.numcorrsalida1);
                        listBox3.Items.Add("Output2 Stream Nº: " +  divientalpiatmp.numcorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Flow: " +  divientalpiatmp.caudalcorrentrada);
                        listBox3.Items.Add("Output1 Flow: " +  divientalpiatmp.caudalcorrsalida1);
                        listBox3.Items.Add("Output2 Flow: " +  divientalpiatmp.caudalcorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Pressure: " +  divientalpiatmp.presioncorrentrada);
                        listBox3.Items.Add("Output1 Pressure: " +  divientalpiatmp.presioncorrsalida1);
                        listBox3.Items.Add("Output2 Pressure: " +  divientalpiatmp.presioncorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Enthalphy: " +  divientalpiatmp.entalpiacorrentrada);
                        listBox3.Items.Add("Output1 Enthalpy: " +  divientalpiatmp.entalpiacorrsalida1);
                        listBox3.Items.Add("Output2 Enthalpy: " +  divientalpiatmp.entalpiacorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Entropy: " +  divientalpiatmp.entropiaentrada);
                        listBox3.Items.Add("Output1 Entropy: " +  divientalpiatmp.entropiasalida1);
                        listBox3.Items.Add("Output2 Entropy: " +  divientalpiatmp.entropiasalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Temperature: " +  divientalpiatmp.temperaturaentrada);
                        listBox3.Items.Add("Output1 Temperature: " +  divientalpiatmp.temperaturasalida1);
                        listBox3.Items.Add("Output2 Temperature: " +  divientalpiatmp.temperaturasalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Volumen Específico: " +  divientalpiatmp.volumenespecificoentrada);
                        listBox3.Items.Add("Output1 Volumen Epecífico: " +  divientalpiatmp.volumenespecificosalida1);
                        listBox3.Items.Add("Output2 Volumen Específico: " +  divientalpiatmp.volumenespecificosalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Título: " +  divientalpiatmp.tituloentrada);
                        listBox3.Items.Add("Output1 Título: " +  divientalpiatmp.titulosalida1);
                        listBox3.Items.Add("Output2 Título: " +  divientalpiatmp.titulosalida2);
                        listBox3.Items.Add("");
                    }

                    //Analizamos el Tipo de Equipo de que se trata 
                    else if (equipos11[indice].tipoequipo2 == 21)
                    {
                        listBox3.Items.Add("Flash Tank Equipment, Type 21." + "Equipment Number: " + tanquevapotmp.numequipo);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Stream Nº: " + tanquevapotmp.numcorrentrada);
                        listBox3.Items.Add("Output1 Stream Nº: " + tanquevapotmp.numcorrsalida1);
                        listBox3.Items.Add("Output2 Stream Nº: " + tanquevapotmp.numcorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Flow: " + tanquevapotmp.caudalcorrentrada);
                        listBox3.Items.Add("Output1 Flow: " + tanquevapotmp.caudalcorrsalida1);
                        listBox3.Items.Add("Output2 Flow: " + tanquevapotmp.caudalcorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Pressure: " + tanquevapotmp.presioncorrentrada);
                        listBox3.Items.Add("Output1 Pressure: " + tanquevapotmp.presioncorrsalida1);
                        listBox3.Items.Add("Output2 Pressure: " + tanquevapotmp.presioncorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Enthalphy: " + tanquevapotmp.entalpiacorrentrada);
                        listBox3.Items.Add("Output1 Enthalpy: " + tanquevapotmp.entalpiacorrsalida1);
                        listBox3.Items.Add("Output2 Enthalpy: " + tanquevapotmp.entalpiacorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Entropy: " + tanquevapotmp.entropiaentrada);
                        listBox3.Items.Add("Output1 Entropy: " + tanquevapotmp.entropiasalida1);
                        listBox3.Items.Add("Output2 Entropy: " + tanquevapotmp.entropiasalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Temperature: " + tanquevapotmp.temperaturaentrada);
                        listBox3.Items.Add("Output1 Temperature: " + tanquevapotmp.temperaturasalida1);
                        listBox3.Items.Add("Output2 Temperature: " + tanquevapotmp.temperaturasalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Volumen Específico: " + tanquevapotmp.volumenespecificoentrada);
                        listBox3.Items.Add("Output1 Volumen Epecífico: " + tanquevapotmp.volumenespecificosalida1);
                        listBox3.Items.Add("Output2 Volumen Específico: " + tanquevapotmp.volumenespecificosalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input Título: " + tanquevapotmp.tituloentrada);
                        listBox3.Items.Add("Output1 Título: " + tanquevapotmp.titulosalida1);
                        listBox3.Items.Add("Output2 Título: " + tanquevapotmp.titulosalida2);
                        listBox3.Items.Add("");
                    }

                    else if (equipos11[indice].tipoequipo2 == 22)
                    {
                        listBox3.Items.Add("Heat Exchanger Equipment, Type 22." + "Equipment Number: " + intercambiadortmp.numequipo);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Stream Nº: " + intercambiadortmp.numcorrentrada1);
                        listBox3.Items.Add("Input2 Stream Nº: " + intercambiadortmp.numcorrentrada2);
                        listBox3.Items.Add("Output1 Stream Nº: " + intercambiadortmp.numcorrsalida1);
                        listBox3.Items.Add("Output2 Stream Nº: " + intercambiadortmp.numcorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Unidades: Flow - W(Kgr/sg),Pressure - P(Bar),Entalphy - H(Kj/Kgr), Entropy - S(Kj/KgrºC), Temperature - (ºC)");
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Flow: " + intercambiadortmp.caudalcorrentrada1);
                        listBox3.Items.Add("Input2 Flow: " + intercambiadortmp.caudalcorrentrada2);
                        listBox3.Items.Add("Output1 Flow: " + intercambiadortmp.caudalcorrsalida1);
                        listBox3.Items.Add("Output2 Flow: " + intercambiadortmp.caudalcorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Pressure: " + intercambiadortmp.presioncorrentrada1);
                        listBox3.Items.Add("Input2 Pressure: " + intercambiadortmp.presioncorrentrada2);
                        listBox3.Items.Add("Output1 Pressure: " + intercambiadortmp.presioncorrsalida1);
                        listBox3.Items.Add("Output2 Pressure: " + intercambiadortmp.presioncorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Enthalphy: " + intercambiadortmp.entalpiacorrentrada1);
                        listBox3.Items.Add("Input2 Enthalphy: " + intercambiadortmp.entalpiacorrentrada2);
                        listBox3.Items.Add("Output1 Enthalpy: " + intercambiadortmp.entalpiacorrsalida1);
                        listBox3.Items.Add("Output2 Enthalpy: " + intercambiadortmp.entalpiacorrsalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Entropy: " + intercambiadortmp.entropiaentrada1);
                        listBox3.Items.Add("Input2 Entropy: " + intercambiadortmp.entropiaentrada2);
                        listBox3.Items.Add("Output1 Entropy: " + intercambiadortmp.entropiasalida1);
                        listBox3.Items.Add("Output2 Entropy: " + intercambiadortmp.entropiasalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Temperature: " + intercambiadortmp.temperaturaentrada1);
                        listBox3.Items.Add("Input2 Temperature: " + intercambiadortmp.temperaturaentrada2);
                        listBox3.Items.Add("Output1 Temperature: " + intercambiadortmp.temperaturasalida1);
                        listBox3.Items.Add("Output2 Temperature: " + intercambiadortmp.temperaturasalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Volumen Específico: " + intercambiadortmp.volumenespecificoentrada1);
                        listBox3.Items.Add("Input2 Volumen Específico: " + intercambiadortmp.volumenespecificoentrada2);
                        listBox3.Items.Add("Output1 Volumen Específico: " + intercambiadortmp.volumenespecificosalida1);
                        listBox3.Items.Add("Output2 Volumen Específico: " + intercambiadortmp.volumenespecificosalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("Input1 Título: " + intercambiadortmp.tituloentrada1);
                        listBox3.Items.Add("Input2 Título: " + intercambiadortmp.tituloentrada2);
                        listBox3.Items.Add("Output1 Título: " + intercambiadortmp.titulosalida1);
                        listBox3.Items.Add("Output2 Título: " + intercambiadortmp.titulosalida2);
                        listBox3.Items.Add("");

                        listBox3.Items.Add("TTD: " + intercambiadortmp.TTD);
                        listBox3.Items.Add("DCA: " + intercambiadortmp.DCA);

                        listBox3.Items.Add("");
                        listBox3.Items.Add("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                    }                                  

                    //CONTINUAR CON EL RESTO DE EQUIPOS MEDIANTE OTROS ELSE IF 
                }
            }

        //Prueba Ejemplo Interface con EXCEL
        private void interfaceConEXCELToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Path1="";
            OpenFileDialog openFileDialog22 = new OpenFileDialog();
            openFileDialog22.Filter = "xlsx files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            openFileDialog22.Title = "Selecciona un Archivo de EXCEL para Importar su contenido.";
            openFileDialog22.InitialDirectory = "C:\\Users\\LUISCOCO\\Desktop";

            if (openFileDialog22.ShowDialog() == DialogResult.OK)
            {
                Path1 = openFileDialog22.FileName;
            }

            try
            {
                // initialize the Excel Application class
                Excel.Application app = new Excel.Application();
                // create the workbook object by opening  the excel file.
                Excel.Workbook workBook = app.Workbooks.Open(Path1, 0, true, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                // Get The Active Worksheet Using Sheet Name Or Active Sheet
                Excel.Worksheet workSheet = (Excel.Worksheet)workBook.ActiveSheet;
                int index = 0;
                // This row,column index should be changed as per your need.
                // that is which cell in the excel you are interesting to read.
                object rowIndex = 2;
                object colIndex1 = 1;
                object colIndex2 = 2;
          
                while (((Excel.Range)workSheet.Cells[rowIndex, colIndex1]).Value2 != null)
                {
                    rowIndex = 2 + index;
                    string firstName = ((Excel.Range)workSheet.Cells[rowIndex, colIndex1]).Value2.ToString();
                    string lastName = ((Excel.Range)workSheet.Cells[rowIndex, colIndex2]).Value2.ToString();
                    textBox5.Text = firstName;
                    textBox15.Text = lastName;
                    Console.WriteLine("Name : {0},{1} ", firstName, lastName);
                    index++;
                }
            }

            catch (Exception ex)
            {
                //app.Quit();
                //Application.Exit();
                MessageBox.Show(ex.Message);                
            }        
        }

        //Análisis de Sensibilidad
        private void sensivityAnalysisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Analisis_Sensibilidad ansensibilidad = new Analisis_Sensibilidad(this);
            ansensibilidad.ShowDialog();
        }

        //Guardar resultados de Equipos y Corrientes de cada Cálculo (cada setnumber)
        public void guardaresultadoscalculos()
        {       
            //Guardar los Resultados de los Equipos Tipo 1 Condición de Contorno
            for (int u = 0; u < equipos11.Count; u++)
            {
                ClassCondicionContorno1 condtemp = new ClassCondicionContorno1();
                condtemp.inicializar(0, 0, 0, 0, 0, 0);

                //Unidades del Sistema Internacional
                if (unidades == 2)
                {
                    condtemp.unidades1 = 2;
                }

                if (equipos11[u].tipoequipo2 == 1)
                {
                    condtemp.numcorrentrada = equipos11[u].aN1;
                    condtemp.numcorrsalida = equipos11[u].aN3;
                    condtemp.numequipo = equipos11[u].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 9 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = p[o].Nombre;
                        int longitud = nom.Length;
                        String tem = nom.Substring(1, longitud - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr == condtemp.numcorrentrada)
                        {
                            if (tipoparametro == "W")
                            {
                                condtemp.caudalcorrentrada = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                condtemp.presioncorrentrada = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                condtemp.entalpiacorrentrada = p[o].Value;
                            }
                        }

                        else if (numcorr == condtemp.numcorrsalida)
                        {
                            if (tipoparametro == "W")
                            {
                                condtemp.caudalcorrsalida = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                condtemp.presioncorrsalida = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                condtemp.entalpiacorrsalida = p[o].Value;
                            }
                        }
                    }

                    condtemp.Calcular();
                    condiciones1.Add(condtemp);
                    numtipo1++;
                }
            }

            //Guardar los Resultados de los Equipos Tipo 2 Divisor
            for (int u = 0; u < equipos11.Count; u++)
            {
                ClassDivisor2 divisortemp = new ClassDivisor2();
                divisortemp.inicializar(0, 0, 0, 0, 0, 0, 0, 0, 0);

                //Unidades del Sistema Internacional
                if (unidades == 2)
                {
                    divisortemp.unidades1 = 2;
                }

                if (equipos11[u].tipoequipo2 == 2)
                {
                    divisortemp.numcorrentrada = equipos11[u].aN1;
                    divisortemp.numcorrsalida1 = equipos11[u].aN3;
                    divisortemp.numcorrsalida2 = equipos11[u].aN4;
                    divisortemp.numequipo = equipos11[u].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 9 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = p[o].Nombre;
                        int longitud = nom.Length;
                        String tem = nom.Substring(1, longitud - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr == divisortemp.numcorrentrada)
                        {
                            if (tipoparametro == "W")
                            {
                                divisortemp.caudalcorrentrada = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                divisortemp.presioncorrentrada = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                divisortemp.entalpiacorrentrada = p[o].Value;
                            }
                        }

                        else if (numcorr == divisortemp.numcorrsalida1)
                        {
                            if (tipoparametro == "W")
                            {
                                divisortemp.caudalcorrsalida1 = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                divisortemp.presioncorrsalida1 = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                divisortemp.entalpiacorrsalida1 = p[o].Value;
                            }
                        }

                        else if (numcorr == divisortemp.numcorrsalida2)
                        {
                            if (tipoparametro == "W")
                            {
                                divisortemp.caudalcorrsalida2 = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                divisortemp.presioncorrsalida2 = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                divisortemp.entalpiacorrsalida2 = p[o].Value;
                            }
                        }
                    }

                    divisortemp.Calcular();
                    divisores2.Add(divisortemp);
                    numtipo2++;
                }
            }


            //Guardar los Resultados de los Equipos Tipo 3 Pérdida de Carga
            for (int u = 0; u < equipos11.Count; u++)
            {
                ClassPerdidaCarga3 perdidatemp = new ClassPerdidaCarga3();
                perdidatemp.inicializar(0, 0, 0, 0, 0, 0);

                //Unidades del Sistema Internacional
                if (unidades == 2)
                {
                    perdidatemp.unidades1 = 2;
                }

                if (equipos11[u].tipoequipo2 == 3)
                {
                    perdidatemp.numcorrentrada = equipos11[u].aN1;
                    perdidatemp.numcorrsalida = equipos11[u].aN3;
                    perdidatemp.numequipo = equipos11[u].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 9 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = p[o].Nombre;
                        int longitud = nom.Length;
                        String tem = nom.Substring(1, longitud - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr == perdidatemp.numcorrentrada)
                        {
                            if (tipoparametro == "W")
                            {
                                perdidatemp.caudalcorrentrada = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                perdidatemp.presioncorrentrada = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                perdidatemp.entalpiacorrentrada = p[o].Value;
                            }
                        }

                        else if (numcorr == perdidatemp.numcorrsalida)
                        {
                            if (tipoparametro == "W")
                            {
                                perdidatemp.caudalcorrsalida = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                perdidatemp.presioncorrsalida = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                perdidatemp.entalpiacorrsalida = p[o].Value;
                            }

                        }
                    }

                    perdidatemp.Calcular();
                    perdidas3.Add(perdidatemp);
                    numtipo3++;
                }
            }


            //Guardar los Resultados de los Equipos Tipo 4 Bomba
            for (int u = 0; u < equipos11.Count; u++)
            {
                ClassBomba4 bombatemp = new ClassBomba4();
                bombatemp.inicializar(0, 0, 0, 0, 0, 0);

                //Unidades del Sistema Internacional
                if (unidades == 2)
                {
                    bombatemp.unidades1 = 2;
                }

                if (equipos11[u].tipoequipo2 == 4)
                {
                    bombatemp.numcorrentrada = equipos11[u].aN1;
                    bombatemp.numcorrsalida = equipos11[u].aN3;
                    bombatemp.numequipo = equipos11[u].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 9 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = p[o].Nombre;
                        int longitud = nom.Length;
                        String tem = nom.Substring(1, longitud - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr == bombatemp.numcorrentrada)
                        {
                            if (tipoparametro == "W")
                            {
                                bombatemp.caudalcorrentrada = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                bombatemp.presioncorrentrada = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                bombatemp.entalpiacorrentrada = p[o].Value;
                            }
                        }

                        else if (numcorr == bombatemp.numcorrsalida)
                        {
                            if (tipoparametro == "W")
                            {
                                bombatemp.caudalcorrsalida = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                bombatemp.presioncorrsalida = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                bombatemp.entalpiacorrsalida = p[o].Value;
                            }
                        }
                    }

                    bombatemp.Calcular();
                    bombas4.Add(bombatemp);
                    numtipo4++;
                }
            }


            //Guardar los Resultados de los Equipos Tipo 5 Mezclador
            for (int u = 0; u < equipos11.Count; u++)
            {
                ClassMezclador5 mezcladortemp = new ClassMezclador5();
                mezcladortemp.inicializar(0, 0, 0, 0, 0, 0, 0, 0, 0);

                //Unidades del Sistema Internacional
                if (unidades == 2)
                {
                    mezcladortemp.unidades1 = 2;
                }

                if (equipos11[u].tipoequipo2 == 5)
                {
                    mezcladortemp.numcorrentrada1 = equipos11[u].aN1;
                    mezcladortemp.numcorrentrada2 = equipos11[u].aN2;
                    mezcladortemp.numcorrsalida = equipos11[u].aN3;
                    mezcladortemp.numequipo = equipos11[u].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 9 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = p[o].Nombre;
                        int longitud = nom.Length;
                        String tem = nom.Substring(1, longitud - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr == mezcladortemp.numcorrentrada1)
                        {
                            if (tipoparametro == "W")
                            {
                                mezcladortemp.caudalcorrentrada1 = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                mezcladortemp.presioncorrentrada1 = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                mezcladortemp.entalpiacorrentrada1 = p[o].Value;
                            }
                        }

                        else if (numcorr == mezcladortemp.numcorrentrada2)
                        {
                            if (tipoparametro == "W")
                            {
                                mezcladortemp.caudalcorrentrada2 = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                mezcladortemp.presioncorrentrada2 = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                mezcladortemp.entalpiacorrentrada2 = p[o].Value;
                            }
                        }

                        else if (numcorr == mezcladortemp.numcorrsalida)
                        {
                            if (tipoparametro == "W")
                            {
                                mezcladortemp.caudalcorrsalida = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                mezcladortemp.presioncorrsalida = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                mezcladortemp.entalpiacorrsalida = p[o].Value;
                            }

                        }
                    }

                    mezcladortemp.Calcular();
                    mezcladores5.Add(mezcladortemp);
                    numtipo5++;
                }
            }


            //Guardar los Resultados de los Equipos Tipo 6 Reactor
            for (int u = 0; u < equipos11.Count; u++)
            {
                ClassReactor6 reactortemp = new ClassReactor6();
                reactortemp.inicializar(0, 0, 0, 0, 0, 0);

                //Unidades del Sistema Internacional
                if (unidades == 2)
                {
                    reactortemp.unidades1 = 2;
                }

                if (equipos11[u].tipoequipo2 == 6)
                {
                    reactortemp.numcorrentrada = equipos11[u].aN1;
                    reactortemp.numcorrsalida = equipos11[u].aN3;
                    reactortemp.numequipo = equipos11[u].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 9 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = p[o].Nombre;
                        int longitud = nom.Length;
                        String tem = nom.Substring(1, longitud - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr == reactortemp.numcorrentrada)
                        {
                            if (tipoparametro == "W")
                            {
                                reactortemp.caudalcorrentrada = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                reactortemp.presioncorrentrada = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                reactortemp.entalpiacorrentrada = p[o].Value;
                            }
                        }

                        else if (numcorr == reactortemp.numcorrsalida)
                        {
                            if (tipoparametro == "W")
                            {
                                reactortemp.caudalcorrsalida = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                reactortemp.presioncorrsalida = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                reactortemp.entalpiacorrsalida = p[o].Value;
                            }

                        }
                    }

                    reactortemp.Calcular();
                    reactores6.Add(reactortemp);
                    numtipo6++;
                }
            }


            //Guardar los Resultados de los Equipos Tipo 7 Calentador
            for (int u = 0; u < equipos11.Count; u++)
            {
                ClassCalentador7 calentadortemp = new ClassCalentador7();
                calentadortemp.inicializar(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

                //Unidades del Sistema Internacional
                if (unidades == 2)
                {
                    calentadortemp.unidades1 = 2;
                }

                if (equipos11[u].tipoequipo2 == 7)
                {
                    calentadortemp.numcorrentrada1 = equipos11[u].aN1;
                    calentadortemp.numcorrentrada2 = equipos11[u].aN2;
                    calentadortemp.numcorrsalida1 = equipos11[u].aN3;
                    calentadortemp.numcorrsalida2 = equipos11[u].aN4;
                    calentadortemp.numequipo = equipos11[u].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 7 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = p[o].Nombre;
                        int longitud = nom.Length;
                        String tem = nom.Substring(1, longitud - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr == calentadortemp.numcorrentrada1)
                        {
                            if (tipoparametro == "W")
                            {
                                calentadortemp.caudalcorrentrada1 = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                calentadortemp.presioncorrentrada1 = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                calentadortemp.entalpiacorrentrada1 = p[o].Value;
                            }
                        }

                        else if (numcorr == calentadortemp.numcorrentrada2)
                        {
                            if (tipoparametro == "W")
                            {
                                calentadortemp.caudalcorrentrada2 = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                calentadortemp.presioncorrentrada2 = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                calentadortemp.entalpiacorrentrada2 = p[o].Value;
                            }
                        }


                        else if (numcorr == calentadortemp.numcorrsalida1)
                        {
                            if (tipoparametro == "W")
                            {
                                calentadortemp.caudalcorrsalida1 = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                calentadortemp.presioncorrsalida1 = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                calentadortemp.entalpiacorrsalida1 = p[o].Value;
                            }
                        }

                        else if (numcorr == calentadortemp.numcorrsalida2)
                        {
                            if (tipoparametro == "W")
                            {
                                calentadortemp.caudalcorrsalida2 = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                calentadortemp.presioncorrsalida2 = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                calentadortemp.entalpiacorrsalida2 = p[o].Value;
                            }
                        }
                    }

                    calentadortemp.Calcular();
                    calentadores7.Add(calentadortemp);
                    numtipo7++;
                }
            }


            //Guardar los Resultados de los Equipos Tipo 8 Condensador Principal
            for (int u = 0; u < equipos11.Count; u++)
            {
                ClassCondensador8 condensadortemp = new ClassCondensador8();
                condensadortemp.inicializar(0, 0, 0, 0, 0, 0, 0, 0, 0);

                //Unidades del Sistema Internacional
                if (unidades == 2)
                {
                    condensadortemp.unidades1 = 2;
                }

                if (equipos11[u].tipoequipo2 == 8)
                {
                    condensadortemp.numcorrentrada1 = equipos11[u].aN1;
                    condensadortemp.numcorrentrada2 = equipos11[u].aN2;
                    condensadortemp.numcorrsalida = equipos11[u].aN3;
                    condensadortemp.numequipo = equipos11[u].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 9 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = p[o].Nombre;
                        int longitud = nom.Length;
                        String tem = nom.Substring(1, longitud - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr == condensadortemp.numcorrentrada1)
                        {
                            if (tipoparametro == "W")
                            {
                                condensadortemp.caudalcorrentrada1 = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                condensadortemp.presioncorrentrada1 = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                condensadortemp.entalpiacorrentrada1 = p[o].Value;
                            }
                        }

                        else if (numcorr == condensadortemp.numcorrentrada2)
                        {
                            if (tipoparametro == "W")
                            {
                                condensadortemp.caudalcorrentrada2 = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                condensadortemp.presioncorrentrada2 = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                condensadortemp.entalpiacorrentrada2 = p[o].Value;
                            }
                        }

                        else if (numcorr == condensadortemp.numcorrsalida)
                        {
                            if (tipoparametro == "W")
                            {
                                condensadortemp.caudalcorrsalida = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                condensadortemp.presioncorrsalida = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                condensadortemp.entalpiacorrsalida = p[o].Value;
                            }
                        }
                    }

                    condensadortemp.Calcular();
                    condensadores8.Add(condensadortemp);
                    numtipo8++;
                }
            }

            //Guardar los Resultados de los Equipos Tipo 9 Turbina con Pérdidas en el Escape
            for (int u = 0; u < equipos11.Count; u++)
            {
                ClassTurbina9 turtemp = new ClassTurbina9();
                turtemp.inicializar(0, 0, 0, 0, 0, 0);

                //Unidades del Sistema Internacional
                if (unidades == 2)
                {
                    turtemp.unidades1 = 2;
                }

                if (equipos11[u].tipoequipo2 == 9)
                {
                    turtemp.numcorrentrada = equipos11[u].aN1;
                    turtemp.numcorrsalida = equipos11[u].aN3;
                    turtemp.numequipo = equipos11[u].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 9 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = p[o].Nombre;
                        int longitud = nom.Length;
                        String tem = nom.Substring(1, longitud - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr == turtemp.numcorrentrada)
                        {
                            if (tipoparametro == "W")
                            {
                                turtemp.caudalcorrentrada = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                turtemp.presioncorrentrada = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                turtemp.entalpiacorrentrada = p[o].Value;
                            }
                        }

                        else if (numcorr == turtemp.numcorrsalida)
                        {
                            if (tipoparametro == "W")
                            {
                                turtemp.caudalcorrsalida = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                turtemp.presioncorrsalida = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                turtemp.entalpiacorrsalida = p[o].Value;
                            }

                        }
                    }

                    turtemp.Calcular();
                    turbinas9.Add(turtemp);
                    numtipo9++;
                }
            }


            //Guardar los Resultados de los Equipos Tipo 10 Turbina sin Pérdidas en el Escape
            for (int u = 0; u < equipos11.Count; u++)
            {
                ClassTurbina10 turtemp10 = new ClassTurbina10();
                turtemp10.inicializar(0, 0, 0, 0, 0, 0);

                //Unidades del Sistema Internacional
                if (unidades == 2)
                {
                    turtemp10.unidades1 = 2;
                }

                if (equipos11[u].tipoequipo2 == 10)
                {
                    turtemp10.numcorrentrada = equipos11[u].aN1;
                    turtemp10.numcorrsalida = equipos11[u].aN3;
                    turtemp10.numequipo = equipos11[u].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 9 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = p[o].Nombre;
                        int longitud = nom.Length;
                        String tem = nom.Substring(1, longitud - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr == turtemp10.numcorrentrada)
                        {
                            if (tipoparametro == "W")
                            {
                                turtemp10.caudalcorrentrada = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                turtemp10.presioncorrentrada = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                turtemp10.entalpiacorrentrada = p[o].Value;
                            }
                        }

                        else if (numcorr == turtemp10.numcorrsalida)
                        {
                            if (tipoparametro == "W")
                            {
                                turtemp10.caudalcorrsalida = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                turtemp10.presioncorrsalida = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                turtemp10.entalpiacorrsalida = p[o].Value;
                            }
                        }
                    }

                    turtemp10.Calcular();
                    turbinas10.Add(turtemp10);
                    numtipo10++;
                }
            }


            //Guardar los Resultados de los Equipos Tipo 11 Turbina Auxiliar
            for (int u = 0; u < equipos11.Count; u++)
            {
                ClassTurbina11 turtemp11 = new ClassTurbina11();
                turtemp11.inicializar(0, 0, 0, 0, 0, 0);

                //Unidades del Sistema Internacional
                if (unidades == 2)
                {
                    turtemp11.unidades1 = 2;
                }

                if (equipos11[u].tipoequipo2 == 11)
                {
                    turtemp11.numcorrentrada = equipos11[u].aN1;
                    turtemp11.numcorrsalida = equipos11[u].aN3;
                    turtemp11.numequipo = equipos11[u].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 9 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = p[o].Nombre;
                        int longitud = nom.Length;
                        String tem = nom.Substring(1, longitud - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr == turtemp11.numcorrentrada)
                        {
                            if (tipoparametro == "W")
                            {
                                turtemp11.caudalcorrentrada = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                turtemp11.presioncorrentrada = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                turtemp11.entalpiacorrentrada = p[o].Value;
                            }
                        }

                        else if (numcorr == turtemp11.numcorrsalida)
                        {
                            if (tipoparametro == "W")
                            {
                                turtemp11.caudalcorrsalida = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                turtemp11.presioncorrsalida = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                turtemp11.entalpiacorrsalida = p[o].Value;
                            }
                        }
                    }

                    turtemp11.Calcular();
                    turbinas11.Add(turtemp11);
                    numtipo11++;
                }
            }


            //Guardar los Resultados de los Equipos Tipo 12 Eje
                        
            //Guardar los Resultados de los Equipos Tipo 13 Separador de Humedad
            for (int u = 0; u < equipos11.Count; u++)
            {
                ClassSeparadorHumedad13 sephumedadtemp13 = new ClassSeparadorHumedad13();
                sephumedadtemp13.inicializar(0, 0, 0, 0, 0, 0, 0, 0, 0);

                //Unidades del Sistema Internacional
                if (unidades == 2)
                {
                    sephumedadtemp13.unidades1 = 2;
                }

                if (equipos11[u].tipoequipo2 == 13)
                {
                    sephumedadtemp13.numcorrentrada = equipos11[u].aN1;
                    sephumedadtemp13.numcorrsalida1 = equipos11[u].aN3;
                    sephumedadtemp13.numcorrsalida2 = equipos11[u].aN4;
                    sephumedadtemp13.numequipo = equipos11[u].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 9 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = p[o].Nombre;
                        int longitud = nom.Length;
                        String tem = nom.Substring(1, longitud - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr == sephumedadtemp13.numcorrentrada)
                        {
                            if (tipoparametro == "W")
                            {
                                sephumedadtemp13.caudalcorrentrada = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                sephumedadtemp13.presioncorrentrada = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                sephumedadtemp13.entalpiacorrentrada = p[o].Value;
                            }
                        }

                        else if (numcorr == sephumedadtemp13.numcorrsalida1)
                        {
                            if (tipoparametro == "W")
                            {
                                sephumedadtemp13.caudalcorrsalida1 = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                sephumedadtemp13.presioncorrsalida1 = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                sephumedadtemp13.entalpiacorrsalida1 = p[o].Value;
                            }
                        }

                        else if (numcorr == sephumedadtemp13.numcorrsalida2)
                        {
                            if (tipoparametro == "W")
                            {
                                sephumedadtemp13.caudalcorrsalida2 = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                sephumedadtemp13.presioncorrsalida2 = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                sephumedadtemp13.entalpiacorrsalida2 = p[o].Value;
                            }
                        }
                    }

                    sephumedadtemp13.Calcular();
                    sephumedadlista13.Add(sephumedadtemp13);
                    numtipo13++;
                }
            }
           

            //Guardar los Resultados de los Equipos Tipo 14 MSR
            for (int u = 0; u < equipos11.Count; u++)
            {
                ClassMSR14 msrtemp = new ClassMSR14();
                msrtemp.inicializar(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

                //Unidades del Sistema Internacional
                if (unidades == 2)
                {
                    msrtemp.unidades1 = 2;
                }

                if (equipos11[u].tipoequipo2 == 14)
                {
                    msrtemp.numcorrentrada1 = equipos11[u].aN1;
                    msrtemp.numcorrentrada2 = equipos11[u].aN2;
                    msrtemp.numcorrsalida1 = equipos11[u].aN3;
                    msrtemp.numcorrsalida2 = equipos11[u].aN4;
                    msrtemp.numequipo = equipos11[u].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 7 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = p[o].Nombre;
                        int longitud = nom.Length;
                        String tem = nom.Substring(1, longitud - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr == msrtemp.numcorrentrada1)
                        {
                            if (tipoparametro == "W")
                            {
                                msrtemp.caudalcorrentrada1 = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                msrtemp.presioncorrentrada1 = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                msrtemp.entalpiacorrentrada1 = p[o].Value;
                            }
                        }

                        else if (numcorr == msrtemp.numcorrentrada2)
                        {
                            if (tipoparametro == "W")
                            {
                                msrtemp.caudalcorrentrada2 = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                msrtemp.presioncorrentrada2 = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                msrtemp.entalpiacorrentrada2 = p[o].Value;
                            }
                        }


                        else if (numcorr == msrtemp.numcorrsalida1)
                        {
                            if (tipoparametro == "W")
                            {
                                msrtemp.caudalcorrsalida1 = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                msrtemp.presioncorrsalida1 = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                msrtemp.entalpiacorrsalida1 = p[o].Value;
                            }
                        }

                        else if (numcorr == msrtemp.numcorrsalida2)
                        {
                            if (tipoparametro == "W")
                            {
                                msrtemp.caudalcorrsalida2 = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                msrtemp.presioncorrsalida2 = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                msrtemp.entalpiacorrsalida2 = p[o].Value;
                            }
                        }
                    }

                    msrtemp.Calcular();
                    msrlista14.Add(msrtemp);
                    numtipo14++;
                }
            }
            

            //Guardar los Resultados de los Equipos Tipo 15 Condensador de Vapor de Sellos
            for (int u = 0; u < equipos11.Count; u++)
            {
                ClassCondensador15 condensador15temp = new ClassCondensador15();
                condensador15temp.inicializar(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

                //Unidades del Sistema Internacional
                if (unidades == 2)
                {
                    condensador15temp.unidades1 = 2;
                }

                if (equipos11[u].tipoequipo2 == 15)
                {
                    condensador15temp.numcorrentrada1 = equipos11[u].aN1;
                    condensador15temp.numcorrentrada2 = equipos11[u].aN2;
                    condensador15temp.numcorrsalida1 = equipos11[u].aN3;
                    condensador15temp.numcorrsalida2 = equipos11[u].aN4;
                    condensador15temp.numequipo = equipos11[u].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 7 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = p[o].Nombre;
                        int longitud = nom.Length;
                        String tem = nom.Substring(1, longitud - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr == condensador15temp.numcorrentrada1)
                        {
                            if (tipoparametro == "W")
                            {
                                condensador15temp.caudalcorrentrada1 = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                condensador15temp.presioncorrentrada1 = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                condensador15temp.entalpiacorrentrada1 = p[o].Value;
                            }
                        }

                        else if (numcorr == condensador15temp.numcorrentrada2)
                        {
                            if (tipoparametro == "W")
                            {
                                condensador15temp.caudalcorrentrada2 = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                condensador15temp.presioncorrentrada2 = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                condensador15temp.entalpiacorrentrada2 = p[o].Value;
                            }
                        }


                        else if (numcorr == condensador15temp.numcorrsalida1)
                        {
                            if (tipoparametro == "W")
                            {
                                condensador15temp.caudalcorrsalida1 = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                condensador15temp.presioncorrsalida1 = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                condensador15temp.entalpiacorrsalida1 = p[o].Value;
                            }
                        }

                        else if (numcorr == condensador15temp.numcorrsalida2)
                        {
                            if (tipoparametro == "W")
                            {
                                condensador15temp.caudalcorrsalida2 = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                condensador15temp.presioncorrsalida2 = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                condensador15temp.entalpiacorrsalida2 = p[o].Value;
                            }
                        }
                    }

                    condensador15temp.Calcular();
                    condensadorlista15.Add(condensador15temp);
                    numtipo15++;
                }
            }


            //Guardar los Resultados de los Equipos Tipo 16 Enfriador de Drenajes
            for (int u = 0; u < equipos11.Count; u++)
            {
                ClassEnfriadorDrenajes16 enfriador16temp = new ClassEnfriadorDrenajes16();
                enfriador16temp.inicializar(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

                //Unidades del Sistema Internacional
                if (unidades == 2)
                {
                    enfriador16temp.unidades1 = 2;
                }

                if (equipos11[u].tipoequipo2 == 16)
                {
                    enfriador16temp.numcorrentrada1 = equipos11[u].aN1;
                    enfriador16temp.numcorrentrada2 = equipos11[u].aN2;
                    enfriador16temp.numcorrsalida1 = equipos11[u].aN3;
                    enfriador16temp.numcorrsalida2 = equipos11[u].aN4;
                    enfriador16temp.numequipo = equipos11[u].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 7 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = p[o].Nombre;
                        int longitud = nom.Length;
                        String tem = nom.Substring(1, longitud - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr == enfriador16temp.numcorrentrada1)
                        {
                            if (tipoparametro == "W")
                            {
                                enfriador16temp.caudalcorrentrada1 = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                enfriador16temp.presioncorrentrada1 = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                enfriador16temp.entalpiacorrentrada1 = p[o].Value;
                            }
                        }

                        else if (numcorr == enfriador16temp.numcorrentrada2)
                        {
                            if (tipoparametro == "W")
                            {
                                enfriador16temp.caudalcorrentrada2 = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                enfriador16temp.presioncorrentrada2 = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                enfriador16temp.entalpiacorrentrada2 = p[o].Value;
                            }
                        }


                        else if (numcorr == enfriador16temp.numcorrsalida1)
                        {
                            if (tipoparametro == "W")
                            {
                                enfriador16temp.caudalcorrsalida1 = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                enfriador16temp.presioncorrsalida1 = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                enfriador16temp.entalpiacorrsalida1 = p[o].Value;
                            }
                        }

                        else if (numcorr == enfriador16temp.numcorrsalida2)
                        {
                            if (tipoparametro == "W")
                            {
                                enfriador16temp.caudalcorrsalida2 = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                enfriador16temp.presioncorrsalida2 = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                enfriador16temp.entalpiacorrsalida2 = p[o].Value;
                            }
                        }
                    }

                    enfriador16temp.Calcular();
                    enfriadorlista16.Add(enfriador16temp);
                    numtipo16++;
                }
            }


            //Guardar los Resultados de los Equipos Tipo 17 Atemperador
            for (int u = 0; u < equipos11.Count; u++)
            {
                ClassAtemperador17 atemperadortemp = new ClassAtemperador17();
                atemperadortemp.inicializar(0, 0, 0, 0, 0, 0, 0, 0, 0);

                //Unidades del Sistema Internacional
                if (unidades == 2)
                {
                    atemperadortemp.unidades1 = 2;
                }

                if (equipos11[u].tipoequipo2 == 17)
                {
                    atemperadortemp.numcorrentrada1 = equipos11[u].aN1;
                    atemperadortemp.numcorrentrada2 = equipos11[u].aN2;
                    atemperadortemp.numcorrsalida = equipos11[u].aN3;
                    atemperadortemp.numequipo = equipos11[u].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 9 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = p[o].Nombre;
                        int longitud = nom.Length;
                        String tem = nom.Substring(1, longitud - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr == atemperadortemp.numcorrentrada1)
                        {
                            if (tipoparametro == "W")
                            {
                                atemperadortemp.caudalcorrentrada1 = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                atemperadortemp.presioncorrentrada1 = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                atemperadortemp.entalpiacorrentrada1 = p[o].Value;
                            }
                        }

                        else if (numcorr == atemperadortemp.numcorrentrada2)
                        {
                            if (tipoparametro == "W")
                            {
                                atemperadortemp.caudalcorrentrada2 = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                atemperadortemp.presioncorrentrada2 = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                atemperadortemp.entalpiacorrentrada2 = p[o].Value;
                            }
                        }

                        else if (numcorr == atemperadortemp.numcorrsalida)
                        {
                            if (tipoparametro == "W")
                            {
                                atemperadortemp.caudalcorrsalida = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                atemperadortemp.presioncorrsalida = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                atemperadortemp.entalpiacorrsalida = p[o].Value;
                            }
                        }
                    }

                    atemperadortemp.Calcular();
                    atemperadores17.Add(atemperadortemp);
                    numtipo17++;
                }
            }


            //Guardar los Resultados de los Equipos Tipo 18 Desaireador
            for (int u = 0; u < equipos11.Count; u++)
            {
                ClassDesaireador18 desaireadortemp = new ClassDesaireador18();
                desaireadortemp.inicializar(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

                //Unidades del Sistema Internacional
                if (unidades == 2)
                {
                    desaireadortemp.unidades1 = 2;
                }

                if (equipos11[u].tipoequipo2 == 18)
                {
                    desaireadortemp.numcorrentrada1 = equipos11[u].aN1;
                    desaireadortemp.numcorrentrada2 = equipos11[u].aN2;
                    desaireadortemp.numcorrsalida1 = equipos11[u].aN3;
                    desaireadortemp.numcorrsalida2 = equipos11[u].aN4;
                    desaireadortemp.numequipo = equipos11[u].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 7 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = p[o].Nombre;
                        int longitud = nom.Length;
                        String tem = nom.Substring(1, longitud - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr == desaireadortemp.numcorrentrada1)
                        {
                            if (tipoparametro == "W")
                            {
                                desaireadortemp.caudalcorrentrada1 = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                desaireadortemp.presioncorrentrada1 = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                desaireadortemp.entalpiacorrentrada1 = p[o].Value;
                            }
                        }

                        else if (numcorr == desaireadortemp.numcorrentrada2)
                        {
                            if (tipoparametro == "W")
                            {
                                desaireadortemp.caudalcorrentrada2 = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                desaireadortemp.presioncorrentrada2 = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                desaireadortemp.entalpiacorrentrada2 = p[o].Value;
                            }
                        }


                        else if (numcorr == desaireadortemp.numcorrsalida1)
                        {
                            if (tipoparametro == "W")
                            {
                                desaireadortemp.caudalcorrsalida1 = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                desaireadortemp.presioncorrsalida1 = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                desaireadortemp.entalpiacorrsalida1 = p[o].Value;
                            }
                        }

                        else if (numcorr == desaireadortemp.numcorrsalida2)
                        {
                            if (tipoparametro == "W")
                            {
                                desaireadortemp.caudalcorrsalida2 = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                desaireadortemp.presioncorrsalida2 = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                desaireadortemp.entalpiacorrsalida2 = p[o].Value;
                            }
                        }
                    }

                    desaireadortemp.Calcular();
                    desaireadores18.Add(desaireadortemp);
                    numtipo18++;
                }
            }


            //Guardar los Resultados de los Equipos Tipo 19 Valvula
            for (int u = 0; u < equipos11.Count; u++)
            {
                ClassValvula19 valvulatemp = new ClassValvula19();
                valvulatemp.inicializar(0, 0, 0, 0, 0, 0);

                //Unidades del Sistema Internacional
                if (unidades == 2)
                {
                    valvulatemp.unidades1 = 2;
                }

                if (equipos11[u].tipoequipo2 == 19)
                {
                    valvulatemp.numcorrentrada = equipos11[u].aN1;
                    valvulatemp.numcorrsalida = equipos11[u].aN3;
                    valvulatemp.numequipo = equipos11[u].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 9 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = p[o].Nombre;
                        int longitud = nom.Length;
                        String tem = nom.Substring(1, longitud - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr == valvulatemp.numcorrentrada)
                        {
                            if (tipoparametro == "W")
                            {
                                valvulatemp.caudalcorrentrada = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                valvulatemp.presioncorrentrada = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                valvulatemp.entalpiacorrentrada = p[o].Value;
                            }
                        }

                        else if (numcorr == valvulatemp.numcorrsalida)
                        {
                            if (tipoparametro == "W")
                            {
                                valvulatemp.caudalcorrsalida = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                valvulatemp.presioncorrsalida = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                valvulatemp.entalpiacorrsalida = p[o].Value;
                            }
                        }
                    }

                    valvulatemp.Calcular();
                    valvulas19.Add(valvulatemp);
                    numtipo19++;
                }
            }


            //Guardar los Resultados de los Equipos Tipo 20 Divisor Entalpia fija
            for (int u = 0; u < equipos11.Count; u++)
            {
                ClassDivisorEntalpiaFija20 divisorentalpiatemp = new ClassDivisorEntalpiaFija20();
                divisorentalpiatemp.inicializar(0, 0, 0, 0, 0, 0, 0, 0, 0);

                //Unidades del Sistema Internacional
                if (unidades == 2)
                {
                    divisorentalpiatemp.unidades1 = 2;
                }

                if (equipos11[u].tipoequipo2 == 20)
                {
                    divisorentalpiatemp.numcorrentrada = equipos11[u].aN1;
                    divisorentalpiatemp.numcorrsalida1 = equipos11[u].aN3;
                    divisorentalpiatemp.numcorrsalida2 = equipos11[u].aN4;
                    divisorentalpiatemp.numequipo = equipos11[u].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 9 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = p[o].Nombre;
                        int longitud = nom.Length;
                        String tem = nom.Substring(1, longitud - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr == divisorentalpiatemp.numcorrentrada)
                        {
                            if (tipoparametro == "W")
                            {
                                divisorentalpiatemp.caudalcorrentrada = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                divisorentalpiatemp.presioncorrentrada = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                divisorentalpiatemp.entalpiacorrentrada = p[o].Value;
                            }
                        }

                        else if (numcorr == divisorentalpiatemp.numcorrsalida1)
                        {
                            if (tipoparametro == "W")
                            {
                                divisorentalpiatemp.caudalcorrsalida1 = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                divisorentalpiatemp.presioncorrsalida1 = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                divisorentalpiatemp.entalpiacorrsalida1 = p[o].Value;
                            }
                        }

                        else if (numcorr == divisorentalpiatemp.numcorrsalida2)
                        {
                            if (tipoparametro == "W")
                            {
                                divisorentalpiatemp.caudalcorrsalida2 = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                divisorentalpiatemp.presioncorrsalida2 = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                divisorentalpiatemp.entalpiacorrsalida2 = p[o].Value;
                            }
                        }
                    }

                    divisorentalpiatemp.Calcular();
                    divisoresentalpia20.Add(divisorentalpiatemp);
                    numtipo20++;
                }
            }


            //Guardar los Resultados de los Equipos Tipo 21 Flash Tank
            for (int u = 0; u < equipos11.Count; u++)
            {
                ClassTanqueVaporizacion21 tanquevaporizaciontemp = new ClassTanqueVaporizacion21();

                tanquevaporizaciontemp.inicializar(0, 0, 0, 0, 0, 0, 0, 0, 0);

                //Unidades del Sistema Internacional
                if (unidades == 2)
                {
                    tanquevaporizaciontemp.unidades1 = 2;
                }

                if (equipos11[u].tipoequipo2 == 21)
                {
                    tanquevaporizaciontemp.numcorrentrada = equipos11[u].aN1;
                    tanquevaporizaciontemp.numcorrsalida1 = equipos11[u].aN3;
                    tanquevaporizaciontemp.numcorrsalida2 = equipos11[u].aN4;
                    tanquevaporizaciontemp.numequipo = equipos11[u].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 9 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = p[o].Nombre;
                        int longitud = nom.Length;
                        String tem = nom.Substring(1, longitud - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr == tanquevaporizaciontemp.numcorrentrada)
                        {
                            if (tipoparametro == "W")
                            {
                                tanquevaporizaciontemp.caudalcorrentrada = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                tanquevaporizaciontemp.presioncorrentrada = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                tanquevaporizaciontemp.entalpiacorrentrada = p[o].Value;
                            }
                        }

                        else if (numcorr == tanquevaporizaciontemp.numcorrsalida1)
                        {
                            if (tipoparametro == "W")
                            {
                                tanquevaporizaciontemp.caudalcorrsalida1 = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                tanquevaporizaciontemp.presioncorrsalida1 = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                tanquevaporizaciontemp.entalpiacorrsalida1 = p[o].Value;
                            }
                        }

                        else if (numcorr == tanquevaporizaciontemp.numcorrsalida2)
                        {
                            if (tipoparametro == "W")
                            {
                                tanquevaporizaciontemp.caudalcorrsalida2 = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                tanquevaporizaciontemp.presioncorrsalida2 = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                tanquevaporizaciontemp.entalpiacorrsalida2 = p[o].Value;
                            }
                        }
                    }

                    tanquevaporizaciontemp.Calcular();
                    tanquesvaporizacion21.Add(tanquevaporizaciontemp);
                    numtipo21++;
                }
            }


            //Guardar los Resultados de los Equipos Tipo 22 Intercambiador
            for (int u = 0; u < equipos11.Count; u++)
            {
                ClassIntercambiador22 intercambiadortemp = new ClassIntercambiador22();
                intercambiadortemp.inicializar(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

                //Unidades del Sistema Internacional
                if (unidades == 2)
                {
                    intercambiadortemp.unidades1 = 2;
                }

                if (equipos11[u].tipoequipo2 == 22)
                {
                    intercambiadortemp.numcorrentrada1 = equipos11[u].aN1;
                    intercambiadortemp.numcorrentrada2 = equipos11[u].aN2;
                    intercambiadortemp.numcorrsalida1 = equipos11[u].aN3;
                    intercambiadortemp.numcorrsalida2 = equipos11[u].aN4;
                    intercambiadortemp.numequipo = equipos11[u].numequipo2;

                    //Rastreamos la lista de Parámetros con los número de corriente de los equipos Tipo 7 y guardamos sus valores en la ClassTurbina9 
                    for (int o = 0; o < p.Count; o++)
                    {
                        String nom = p[o].Nombre;
                        int longitud = nom.Length;
                        String tem = nom.Substring(1, longitud - 1);
                        String tipoparametro = nom.Substring(0, 1);
                        Double numcorr = Convert.ToDouble(tem);

                        if (numcorr == intercambiadortemp.numcorrentrada1)
                        {
                            if (tipoparametro == "W")
                            {
                                intercambiadortemp.caudalcorrentrada1 = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                intercambiadortemp.presioncorrentrada1 = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                intercambiadortemp.entalpiacorrentrada1 = p[o].Value;
                            }
                        }

                        else if (numcorr == intercambiadortemp.numcorrentrada2)
                        {
                            if (tipoparametro == "W")
                            {
                                intercambiadortemp.caudalcorrentrada2 = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                intercambiadortemp.presioncorrentrada2 = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                intercambiadortemp.entalpiacorrentrada2 = p[o].Value;
                            }
                        }


                        else if (numcorr == intercambiadortemp.numcorrsalida1)
                        {
                            if (tipoparametro == "W")
                            {
                                intercambiadortemp.caudalcorrsalida1 = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                intercambiadortemp.presioncorrsalida1 = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                intercambiadortemp.entalpiacorrsalida1 = p[o].Value;
                            }
                        }

                        else if (numcorr == intercambiadortemp.numcorrsalida2)
                        {
                            if (tipoparametro == "W")
                            {
                                intercambiadortemp.caudalcorrsalida2 = p[o].Value;
                            }

                            else if (tipoparametro == "P")
                            {
                                intercambiadortemp.presioncorrsalida2 = p[o].Value;
                            }

                            else if (tipoparametro == "H")
                            {
                                intercambiadortemp.entalpiacorrsalida2 = p[o].Value;
                            }
                        }
                    }

                    intercambiadortemp.Calcular();
                    intercambiadores22.Add(intercambiadortemp);
                    numtipo22++;
                }
            }

            //Guardar los Resultados de las Variables elegidas de los Equipos elegidos en un array para posteriormente Graficarlas.
            // "Set Number" (número del cálculo realizado), "o" (número de la condición de contorno tratada)
            //Guardar las matrix Equipo Tipo 1
            for (int o = 0; o < condiciones1.Count; o++)
            {
                matrixcondicioncontorno1[o, setnumber] = condiciones1[o];
            }

            //Guardar las matrix Equipo Tipo 2
            for (int o = 0; o < divisores2.Count; o++)
            {
                matrixdivisor2[o, setnumber] = divisores2[o];
            }

            //Guardar las matrix Equipo Tipo 3
            for (int o = 0; o < perdidas3.Count; o++)
            {
                matrixperdida3[o, setnumber] = perdidas3[o];
            }

            //Guardar las matrix Equipo Tipo 4
            for (int o = 0; o < bombas4.Count; o++)
            {
                matrixbomba4[o, setnumber] = bombas4[o];
            }

            //Guardar las matrix Equipo Tipo 5
            for (int o = 0; o < mezcladores5.Count; o++)
            {
                matrixmezclador5[o, setnumber] = mezcladores5[o];
            }

            //Guardar las matrix Equipo Tipo 6
            for (int o = 0; o < reactores6.Count; o++)
            {
                matrixreactor6[o, setnumber] = reactores6[o];
            }

            //Guardar las matrix Equipo Tipo 7
            for (int o = 0; o < calentadores7.Count; o++)
            {
                matrixcalentador7[o, setnumber] = calentadores7[o];
            }

            //Guardar las matrix Equipo Tipo 8
            for (int o = 0; o < condensadores8.Count; o++)
            {
                matrixcondensador8[o, setnumber] = condensadores8[o];
            }

            //Guardar las matrix Equipo Tipo 9
            for (int o = 0; o < turbinas9.Count; o++)
            {
                matrixturbina9[o, setnumber] = turbinas9[o];
            }

            //Guardar las matrix Equipo Tipo 10
            for (int o = 0; o < turbinas10.Count; o++)
            {
                matrixturbina10[o, setnumber] = turbinas10[o];
            }

            //Guardar las matrix Equipo Tipo 11
            for (int o = 0; o < turbinas11.Count; o++)
            {
                matrixturbina11[o, setnumber] = turbinas11[o];
            }

            //Guardar las matrix Equipo Tipo 13
            for (int o = 0; o < sephumedadlista13.Count; o++)
            {
                matrixseparador13[o, setnumber] = sephumedadlista13[o];
            }

            //Guardar las matrix Equipo Tipo 14
            for (int o = 0; o < msrlista14.Count; o++)
            {
                matrixMSR14[o, setnumber] = msrlista14[o];
            }

            //Guardar las matrix Equipo Tipo 15
            for (int o = 0; o < condensadorlista15.Count; o++)
            {
                matrixcondensador15[o, setnumber] = condensadorlista15[o];
            }

            //Guardar las matrix Equipo Tipo 16
            for (int o = 0; o < enfriadorlista16.Count; o++)
            {
                matrixenfriador16[o, setnumber] = enfriadorlista16[o];
            }

            //Guardar las matrix Equipo Tipo 17
            for (int o = 0; o <atemperadores17.Count; o++)
            {
                matrixatemperador17[o, setnumber] = atemperadores17[o];
            }

            //Guardar las matrix Equipo Tipo 18
            for (int o = 0; o < desaireadores18.Count; o++)
            {
                matrixdesaireador18[o, setnumber] = desaireadores18[o];
            }

            //Guardar las matrix Equipo Tipo 19
            for (int o = 0; o < valvulas19.Count; o++)
            {
                matrixvalvula19[o, setnumber] = valvulas19[o];
            }

            //Guardar las matrix Equipo Tipo 20
            for (int o = 0; o < divisoresentalpia20.Count; o++)
            {
                matrixdivisor20[o, setnumber] = divisoresentalpia20[o];
            }
           
            //Guardar las matrix Equipo Tipo 21
            for (int o = 0; o < tanquesvaporizacion21.Count; o++)
            {
                matrixtanque21[o, setnumber] = tanquesvaporizacion21[o];
            }

            //Guardar las matrix Equipo Tipo 22
            for (int o = 0; o < intercambiadores22.Count; o++)
            {
                matrixintercambiador22[o, setnumber] = intercambiadores22[o];
            }       

            //También copiamos la lista de resultado de las CORRIENTES en cada iteración
            for (int o = 0; o < p.Count; o++)
            {
                listaresultadoscorrientes[o, setnumber] = p[o];
                //MessageBox.Show(puntero.p[o].ToString());                   
            }
        }

        //Elección del setnumber (número de cálculo)
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            int a = comboBox3.SelectedIndex;
            setnumber = Convert.ToInt16(comboBox3.Items[a]);
            //MessageBox.Show(Convert.ToString(setnumber));

            //Pulsamos la opción del Menú Principal Streams Results
            visualizarResultadosToolStripMenuItem_Click(sender, e);

            //Pulsamos la opción del Menú Principal Model Output
            visualizarResultadosDeLosEquiposToolStripMenuItem_Click(sender,e);          
        }  

        public void newCalculationSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Incrementamos el valor del setnumber (número de cálculos realizados)
            setnumber++; 
 
            //Añadir a la lista de Número de Cálculo (setnumber)
            comboBox3.Items.Add(Convert.ToString(setnumber));

            //Activamos el Botón de Read Input File 
            button59.Enabled = true;

            //Limpiamos la Lista de Resultados de los Equipos
            condiciones1.Clear();
            divisores2.Clear();
            perdidas3.Clear();
            bombas4.Clear();
            mezcladores5.Clear();
            reactores6.Clear();
            calentadores7.Clear();
            condensadores8.Clear();
            turbinas9.Clear();
            turbinas10.Clear();
            turbinas11.Clear();
            sephumedadlista13.Clear();
            msrlista14.Clear();
            condensadorlista15.Clear();
            enfriadorlista16.Clear();
            atemperadores17.Clear();
            desaireadores18.Clear();
            valvulas19.Clear();
            divisoresentalpia20.Clear();
            tanquesvaporizacion21.Clear();
            intercambiadores22.Clear();
        }

        private void dasslSolvingEngineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string sourceDir = Directory.GetCurrentDirectory();
            //File.Delete(sourceDir + "\\wrapper.dll");
            //File.Delete(sourceDir + "\\wrapper4.dll");

            Form1Dassl dasslluis = new Form1Dassl();

            //dasslluis.nombrelibreria = "wrapper.dll";

            if (dasslluis.ShowDialog() == DialogResult.OK)
            {
                dasslluis.Dispose();
                dasslluis = null;
            }
        }

        //Opción del Menú para mostrar el cuadro de diálogo de Cálculo de HTC y Pérdida de Carga de Fluido Monofásico
        private void heatExchangeCalculationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MonofasicoFluidCal calculomonofasico = new MonofasicoFluidCal();
            calculomonofasico.ShowDialog();
        }

        //Opción del Menú para mostrar el cuadro de diálogo de Cálculo de la Pérdida de Carga de Fluido Bifásico
        private void twoPhasePressureDropsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BifasicoPressureDropCalc calculobifasico = new BifasicoPressureDropCalc();
            calculobifasico.ShowDialog();
        }
        
        //Opción del Menú para mostrar el cuadro de diálogo de Cálculo de Intercambiador de Calor Líquido-Líquido
        private void heatExchangerLiquidLiquidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HeatExchangerLiqLiq intercambiadorliqliq = new HeatExchangerLiqLiq();
            intercambiadorliqliq.ShowDialog();
        }

        //Opción del Menú para mostrar el cuadro de diálogo de Cálculo del HTC en Fluido Bifásico en Ebullición
        private void twoPhaseHeatTransferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BifasicoBoilingHeatCal bifasicoboilingHTC = new BifasicoBoilingHeatCal();
            bifasicoboilingHTC.ShowDialog();
        }

        //Opción del Menú para mostrar el cuadro de diálogo de Cálculo del HTC en Fluido Bifásico en Condensación
        private void twoPhaseCondensationHeatTransferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BifasicoCondensationHeatCal bifasicocondenserHTC = new BifasicoCondensationHeatCal();
            bifasicocondenserHTC.ShowDialog();
        }       
    } 
}
