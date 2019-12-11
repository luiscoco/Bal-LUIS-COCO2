using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DotNumerics;
using DotNumerics.LinearAlgebra;
using DotNumerics.LinearAlgebra.CSLapack;

using MINPACK;

using System.Runtime.InteropServices;

using System.Diagnostics;

using System.IO;

namespace Drag_AND_Drop_between_Forms.DotNumerics
{
    public partial class Prueba : Form
    {
        Matrix A = new Matrix(75, 75);

        Matrix B = new Matrix(75, 75);

        DGETRF luisprueba = new DGETRF();
                 
        Random ran = new Random();

        Double contador=0;


        public delegate void DRES1(ref double T, IntPtr Y, IntPtr YPRIME, IntPtr DELTA, ref double IRES, double[] RPAR, int[] IPAR);
        public delegate void DJAC1(ref double T, IntPtr Y, IntPtr YPRIME, IntPtr PD, ref double CJ, double[] RPAR, int[] IPAR);
        public delegate void fun1(ref int n,IntPtr x,IntPtr fvec, ref int iflag);
             

        //[DllImport("test22.dll",EntryPoint="sub1_",CallingConvention=CallingConvention.Cdecl,CharSet=CharSet.Auto)]
        //public static extern void sub1_(ref double A, ref double B, ref double C);

        //[DllImport("test22.dll", EntryPoint = "sub2_", CallingConvention = CallingConvention.Cdecl,CharSet=CharSet.Auto)]
        //public static extern void sub2_(ref double A2, ref double B2, ref double C2);

        [DllImport("bucle.dll", EntryPoint = "sub1_", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern void sub1_(ref double A, ref double B, ref double C);

        [DllImport("bucle.dll", EntryPoint = "sub2_", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern void sub2_(ref double A2, ref double B2, ref double C2);

        //[DllImport("daxpyfinal.dll", EntryPoint = "daxpy_", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        //public static extern void daxpy_(ref int n,ref double da, double[] luisx,ref int incx, double[] luisy,ref int incy);

        [DllImport("daxpy.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern void daxpy_(ref int n, ref double da, double[] luisx, ref int incx, double[] luisy, ref int incy);

        [DllImport("dgefa.dll",CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern void dgefa_(double[,] a,ref int lda,ref int n,int[] ipvt,ref int info);

        [DllImport("dgesl.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern void dgesl_(double[,] a, ref int lda, ref int n, int[] ipvt,double[] b, ref int job);

        [DllImport("chkder.dll", EntryPoint = "chkder_",CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern void chkder_(ref int m,ref int n,double[] x, double[] fvec, double[,] fjac,ref int ldfjac,double[] xp,double[] fvecp,double[] err);

        [DllImport("fcnluis.dll", EntryPoint = "fcn_", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern void fcn_(ref int m,ref int n, double[] x, double[] fvec, double[,] fjac,ref int ldfjac,ref int iflag);

        //[DllImport("hybrd.dll", EntryPoint = "hybrd_", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        //public static extern void hybrd_(fun1 funprueba,ref int n, double[] x, double[] fvec, ref double xtol, ref int maxfev, ref int ml, ref int mu
        //,ref double epsfcn, double[] diag,ref int mode, ref double factor,ref int nprint, ref int info, ref int nfev, double[,]fjac, 
        // ref int ldfjac,double[] r,ref int lr,double[] qtf,double[] wa1,double[] wa2,double[] wa3, double[] wa4);

       // [DllImport("luisfun.dll", EntryPoint = "fcn_", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        //public static extern void fcn1_(ref int n, IntPtr x, IntPtr fvec, ref int iflag);

        [DllImport("hybrdirene.dll", EntryPoint = "luis_", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern void luis_(ref int a,double[,] irene,double[] resultado);

        [DllImport("dpmpar.dll", EntryPoint = "dpmpar_", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern void dpmpar_(ref int a);

        //[DllImport("dasslluis.dll", EntryPoint = "luis_", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        //public static extern void luis_(ref double a, ref double T, double[] Y);

        //Wrapper del algoritmo de DASSL
        [DllImport("eje.dll", EntryPoint = "luis1_", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern void luis1_(ref int NEQ, ref double T, double[] Y,double[] YPRIME,ref double TOUT,int[] INFO,double[] RTOL,double[] ATOL,ref int IDID,double[]rwork,ref int lrw, int[] iwork,ref int liw,double[] rpar,int[] ipar);
       

        [DllImport("eje3.dll", EntryPoint = "luis2_", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern void luis2_(ref int NEQ, ref double T, double[] Y,double[] YPRIME,ref double TOUT,int[] INFO,double[] RTOL,double[] ATOL,ref int IDID, double[] rwork,ref int lrw, int[] iwork,ref int liw,double[] rpar,int[] ipar);
    

        public static void fcn1_(ref int n,IntPtr x,IntPtr fvec, ref int iflag)
        {

            //Copiamos los valores apuntados por el puntero (dirección del primer elementod el vector), en el arugumento de la función, a un vector temporal de trabajo de la función
            double[] X = new double[n];
            Marshal.Copy(x, X, 0, n);
            
           double[] FVEC= new double[n];
            Marshal.Copy(fvec, FVEC, 0, n); 
           
            double one, temp, temp1, temp2, three, two, zero;
            zero = 0;
            one = 1;
            two = 2;
            three = 3;

            if (iflag != 0) goto label5;

            return;

        label5:

            for (int i = 0; i < n;i++ )
            {
                temp = (three - two * X[i]) * X[i];
                temp1 = zero;
                if (i != 0) temp1 = X[i - 1];
                temp2 = zero;
                if(i!=n-1) temp2=X[i+1];

                FVEC[i] = temp - temp1 - two * temp2 + one;
            }

        //Copiamos la dirección del vector de trabajo de la función al puntero 
        Marshal.Copy(X, 0, x, n);
        Marshal.Copy(FVEC, 0, fvec, n);
            
             return;       
        
        }

        public static void dres1luis(ref double T, IntPtr Y, IntPtr YPRIME, IntPtr DELTA, ref double IRES, double[] RPAR, int[] IPAR)
        {
             double[] y = new double[2];
             Marshal.Copy(Y, y, 0, 2);

             double[] delta = new double[2];
             Marshal.Copy(DELTA, delta, 0, 2);

             double[] yprime = new double[2];
             Marshal.Copy(YPRIME, yprime, 0, 2);

             delta[0] = yprime[0] + 10.0*y[0];
             delta[1] = y[1] + y[0] - 1.0;
             
              Marshal.Copy(y, 0, Y, 2);
              Marshal.Copy(delta, 0, DELTA, 2);
              Marshal.Copy(yprime, 0, YPRIME, 2);

              y = null;
              delta = null;
              yprime = null;

             return;
        }

        public static void djac1luis(ref double T, IntPtr Y, IntPtr YPRIME, IntPtr PD, ref double CJ, double[] RPAR, int[] IPAR)
        {
            double[] temp = new double[2*2];
            Marshal.Copy(PD, temp, 0, 2);

            double[,] pd=new double[2,2];

            pd[0,0] = CJ + 10.0;
            pd[0,1] = 0.0;
            pd[1,0] = 1.0;
            pd[1,1] = 1.0;

            for (int i = 0; i < 2; i++)
            {


            }

            return;
        }
        
        //public MinPackFunction f1(int a, int b, double[] c, double[] d, ref int e);
        Func<int, int, double[], double[],int > f1 = (a, b, c, d) =>
        {
            int m, n;
            int iflag = 1;
            m = a;
            n = b;
            Double [] x =new Double[n];
            Double [] fvec =new Double[m];
           
            x=c;
            fvec=d;

            int i;
            Double tmp1, tmp2, tmp3;
            Double [] y=new Double[15];
            y[0] = 0.14;
            y[1] = 0.18;
            y[2] = 0.22;
            y[3] = 0.25;
            y[4] = 0.29;
            y[5] = 0.32;
            y[6] = 0.35;
            y[7] = 0.39;
            y[8] = 0.37;
            y[9] = 0.58;
            y[10] = 0.73;
            y[11] = 0.96;
            y[12] = 1.34;
            y[13] = 2.1;
            y[14] = 4.39;

            if (iflag != 0)
            {
                goto cinco;
            }

            else if (iflag==0)
            {
                return 99;
            }

            cinco:

            for (int j = 1; j < 15; j++)
            {
                tmp1 = j;
                tmp2 = 16 - j;
                tmp3 = tmp1;
                if (j > 8)
                {
                    tmp3 = tmp2;
                }

                fvec[j] = y[j] - (x[0] + tmp1 / (x[1] * tmp2 + x[2] * tmp3));
            }

            return 1;
        };           
                    
       
        public Prueba()
        {
            InitializeComponent();

            for (int j = 0; j < 10; j++)
            {
                for (int i = 0; i < 10; i++)
                {
                    A[i, j] = ran.Next(100);
                }
            }
        }

        //Resolución de un Sistema de Ecuaciones Ax=B de 1000 Ecuaciones con 1000 incognitas.
        private void button1_Click(object sender, EventArgs e)
        {
            Matrix A = new Matrix(75, 75);
            Matrix B = new Matrix(75, 1);

            Random ran = new Random();

            for (int j = 0; j < 75; j++)
            {
                for (int i = 0; i < 75; i++)
                {
                    A[i, j] = ran.Next(75);
                }
            }

            for (int i = 0; i <75; i++)
            {
                B[i, 0] = ran.Next(75);
            }


            //LinearEquations leq = new LinearEquations();
            //Matrix X = leq.Solve(A, B);
            LinearLeastSquares leastSquares = new LinearLeastSquares();
            Matrix X = leastSquares.SVDdcSolve(A, B); 
            
            Matrix AXmB = A * X - B;

            visualizar(X);
        }

        void visualizar(Matrix A)
        {
            //Primero Inicializamos el Control dataGridView
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            List<DataGridViewTextBoxColumn> listacolumnas = new List<DataGridViewTextBoxColumn>();

            int filas = A.RowCount;
            int columnas = A.ColumnCount;

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
                            Columntemp.HeaderText = "C" + Convert.ToString(j);
                            Columntemp.Width = 75;
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

                //Número Columnas
                for (int j = 0; j < columnas; j++)
                {
                    dataGridView1.Rows[i].Cells[j].Value = A[i, j];

                    //A los elementos de la Diagonal Principal
                    if (i == j)
                    {
                        dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Yellow;
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






        //Descomponer A =LU 
        private void button2_Click(object sender, EventArgs e)
        {            
            int info=0;

            int[] intt=new int[100];
            Double[] sol= new Double[100];
            int K = 0;

            //Copiamos los elementos de la matriz a factorizar A en el Array "sol"
            for (int j = 0; j < 10; j++)
            {
                for (int i = 0; i < 10; i++)
                {
                    sol[K]=A[i, j];
                    K++;
                }
            }

            //Enviamos a la función de factorización el array con los elementos de A llamado "sol"
            //En el mismo array "sol" recibiremos los factores de la descomposición LU y tendremos que copiarlos a la matriz B para visualizarlos
            //También quedaría por sobreescribir los elementos de la diagonal principal con elementos unitarios: "1"
            this.luisprueba.Run(10, 10,ref sol, 0, 10, ref intt,0, ref info);

            K=0;

                for (int j = 0; j < 10; j++)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        B[i, j] = sol[K];
                        K++;
                    }
                }
        
            visualizar(B);          
        }
        
        //Visualizar A
        private void button3_Click(object sender, EventArgs e)
        {       
            visualizar(A);
        }






        //Cálculo de la Norma Euclidea de la Matriz A
        //La función EnormClass procede de la Librería MINPACK
        private void button4_Click(object sender, EventArgs e)
        {
            EnormClass prueba = new EnormClass();

            Double[] sol = new Double[100];

            int K=0;

            for (int j = 0; j < 10; j++)
            {
                for (int i = 0; i < 10; i++)
                {                 
                    sol[K]=A[i,j];
                        K++;
                }
            }

           Double resultado= prueba.Enorm(10,sol);
           textBox1.Text = Convert.ToString(resultado);
        }





        private void button6_Click(object sender, EventArgs e)
        {
           double a = 1, b = 2, c = 3, x1 = 0, x2 = 0;

           sub1_(ref a, ref b, ref x1);
           MessageBox.Show("Result 1 : " + x1);

           sub2_(ref a, ref b, ref x2);
           MessageBox.Show("Result 2 : " + x2);    
        }






        //Validación DASSL Sample1: Ejecutar el Algoritmo de DASSL (Dassl.dll dynamic library call)
        private void button7_Click(object sender, EventArgs e)
        {
            //Sistema de Ecuaciones Diferenciales
            // y1'+10*y1=0;
            // y2+y1=1

            //Número de ecuaciones del Sistema de Ecuaciones Diferenciales Algebraicas (DAE)
            int NEQ = 2;       

            //Número de Intervalos de tiempo calculados
            int NOUT = 10;         
            
            //Tiempo inicial
            double T = 0;

            //Tiempo final para inicializar la segunda corrida del Algoritmo de Dassl
            double TOUT1 = 1;
            
            //Incremento de tiempo cada intervalo
            double DTOUT = 1;       

            //Condiciones iniciales de las y
            double[] Y = new double[2];
            Y[0] = 1;
            Y[1] = 0;   
                      
            //Delta=F(x,y,y'), es decir, el error de cada ecuación implícita antes de ser solucionada
            double[] DELTA = new double[2];
           
            //Condiciones iniciales de las y'
            double[] YPRIME = new double[2];
            YPRIME[0] = -10;
            YPRIME[1] = 10;

            //Inicialización del Tiempo Final calculado 
            double TOUT = 1;

            //Opciones de ejecución del Programa. Estas variables deberán ser elegidas por el Usuario en un Cuadro de Diálogo 
            //explicando bien el significado de cada una de las opciones elegidas
            int[] INFO = new int[15];
       
            //Error Relativo permitido
            double[] RTOL = new double[2];
            RTOL[0] = 0;

            //Error Absoluto permitido
            double[] ATOL = new double[2];
            ATOL[0] = 0.1e-05;
            ATOL[1] = 0.1e-05;

            //Indicador de los resultadod de la ejecución del algoritmo
            int IDID = 58;

            //Array de trabajo de números reales
            double[] RWORK = new double[550];

            //Longitud del array de trabajo de números reales
            int LRW = 550;

            //LRW .GE. 40+(MAXORD+4)*NEQ+NEQ**2
            //for the full (dense) JACOBIAN case (when INFO(6)=0), or

            //LRW .GE. 40+(MAXORD+4)*NEQ+(2*ML+MU+1)*NEQ
            //for the banded user-defined JACOBIAN case
            
            //(when INFO(5)=1 and INFO(6)=1),
            
            //or LRW .GE. 40+(MAXORD+4)*NEQ+(2*ML+MU+1)*NEQ+2*(NEQ/(ML+MU+1)+1)
            //for the banded finite-difference-generated JACOBIAN case
            //(when INFO(5)=0 and INFO(6)=1)           
            

            //Arrays de trabajo de números enteros
            int[] IWORK = new int[22];

            //Longitud del array de trabajo de números enteros
            // LIW >= 20+NEQ, en este caso tenemos 2 ecuaciones diferenciales por tanto LIW como mínimo será 22
            int LIW = 22;

            //Arrays de intercambio de datos de números reales
            double[] RPAR = new double[1];
            RPAR[0] = 0.1;

            //Arrays de intercambio de datos de números enteros
            int[] IPAR = new int[1];
            IPAR[0] = 11;

            //Variable de prueba de Luis Coco
            double s = 25;

            //Variables auxiliares 
            double LUN = 6;
            double KPRINT = 3;
            double IPASS = 1;
            double NERR = 0;
            double ERO = 0;
            double HU = 0;
            double NQU = 0;
            double NST = 0;
            double NFE = 0;
            double NJE = 0;
            double YT1 = 0;
            double YT2 = 0;
            double ER1 = 0;
            double ER2 = 0;
            double ER = 0;         

            //Este bucle es para correr dos veces el Algoritmo de Dassl. La primera calculando el Jacobiano de forma automática y la segunda calculándolo 
            //mediante la función definida por el usuaio DJAC1
            for (int a1 = 1; a1 <= 2; a1++)
            {

                for (int i = 0; i < 15;i++ )
                {
                    INFO[i] = 0;
                }
             
                //ATOL y RTOL son vectores INFO[1]=1
                //ATOL y RTOL son escalares INFO[1]=0
                INFO[1]=1;

                //En la segunda corrida del Algorimo de DASSL se calcula el Jacobiano con la función suministrada por el usuario
                if (a1==2) INFO[4]=1;
                    
                //Se inicializa TOUT al comienzo de la segunda corrida del Algoritmo de DASSL 
                TOUT=TOUT1;       
                
                for (int i1 = 1; i1 <= NOUT; i1++)
                {
                    //Llamada al Wrapper "luis " de la función del algoritmo de DASSL 
                    luis1_(ref NEQ, ref T, Y, YPRIME, ref TOUT, INFO,RTOL,ATOL, ref IDID, RWORK, ref LRW, IWORK, ref LIW, RPAR, IPAR);

                    MessageBox.Show("Resultado de T: " + Convert.ToString(T));
                    for (int i2 = 0; i2 <Y.Count() ;i2++) 
                    {
                       MessageBox.Show("Resultado de Y"+Convert.ToString(i2)+" : "+Convert.ToString(Y[i2]));
                    }

                    // RWORK(7)--Which contains the stepsize used
                    //  on the last successful step.
                    HU = RWORK[6];
                    MessageBox.Show("Resultado de H: " + Convert.ToString(HU));
                    NQU = IWORK[7];
                    MessageBox.Show("Resultado de NQ: " + Convert.ToString(NQU));

                    //Si el valor de IDID es negativo el Algoritmo de Dassl ha dado error al ejecutarlo
                    if (IDID < 0) goto LABEL175;
                    
                    //Sistema de Ecuaciones Diferenciales
                    // y1'+10*y1=0;
                    // y2+y1=1
                    //Solución Analítica              
                    YT1 = Math.Exp(-10.0*T);
                    YT2 = 1.0 - YT1;
                    //Comparación de los resultados de DASSL guardados en Y[0] e Y[1], con la solución analítica de la ecuación guardada en YT1 y YT2
                    ER1 = Math.Abs(YT1 - Y[0]);
                    ER2 = Math.Abs(YT2 - Y[1]);
                    ER = Math.Max(ER1,ER2)/ATOL[0];
                    ERO = Math.Max(ERO, ER);

                     MessageBox.Show("Error ERO: " + Convert.ToString(ERO));

                    //Si el error es menor de 1000 vamos a la LABEL170
                     if (ER < 1000) goto LABEL170;

                     NERR = NERR + 1;
                    
                   //Incrementamos el tiempo en un intervalo DOUT para calcular el siguiente valor del tiempo 
                   LABEL170: TOUT = TOUT + DTOUT;              
               
                }

            LABEL175:

               if (IDID < 0) NERR = NERR + 1;
                //IWORK(11)--Which contains the number of steps taken so far.
                NST = IWORK[10];
                //IWORK(12)--Which contains the number of calls to RES so far.
                NFE = IWORK[11];
                //IWORK(12)--Which contains the number of evaluations of the matrix of partial derivatives needed so far.
                NJE = IWORK[12];

                MessageBox.Show("Number Of Steps: "+Convert.ToString(NST));
                MessageBox.Show("Number Of F-S: " + Convert.ToString(NFE));
                MessageBox.Show("Number Of J-S: " + Convert.ToString(NJE));

                MessageBox.Show("Error Overrun: " + Convert.ToString(ERO));
               
              }
              //Fin del bucle es para correr dos veces el Algoritmo de Dassl. 

            int incx, incy, n;

            n = 5;

            incx = 1;
            incy = 1;

            double da = 5;

            double[] luisx = new double[5];
            luisx[0] = 15;
            luisx[1] = 2;
            luisx[2] = 3;
            luisx[3] = 4;
            luisx[4] = 5;

            double[] luisy = new double[5];
            luisy[0] = 5;
            luisy[1] = 4;
            luisy[2] = 3;
            luisy[3] = 2;
            luisy[4] = 1;

            MessageBox.Show("n: " + Convert.ToString(n) + " ; " + "da: " + Convert.ToString(da) + "luisx: " + Convert.ToString(luisx[0]) + "incx: " + Convert.ToString(incx) + "luisy: " + Convert.ToString(luisy[0]));

            daxpy_(ref n, ref da, luisx, ref incx, luisy,ref incy);

            MessageBox.Show("n: " + Convert.ToString(n) + " ; " + "da: " + Convert.ToString(da) + "luisx: " + Convert.ToString(luisx[0]) + "incx: " + Convert.ToString(incx) + "luisy: " + Convert.ToString(luisy[0]));

            double[,] a = new double[3,3];
            a[0, 0] = 1;
            a[0, 1] = 2;
            a[0, 2] = 3;
            a[1, 0] = 4;
            a[1, 1] = 5;
            a[1, 2] = 6;
            a[2, 0] = 7;
            a[2, 1] = 8;
            a[2, 2] = 8;

            double[] b=new double[3];
            b[0] = 7;
            b[1] = 8;
            b[2] = 9;

            int n1 = 3;
            int lda = 3;

            int[] ipvt=new int[3];
            ipvt[0] = 1;
            ipvt[1] = 2;
            ipvt[2] = 3;

            int info = 0;
            int job = 0;

            //Descomposición LU de A (funciónd de la librería LINPACK)
            dgefa_(a,ref lda,ref n1,ipvt,ref info);

            //Resolución de un Sistema de Ecuaciones Lineales Ax=B (función de la librería LINPACK)
            dgesl_(a, ref lda, ref n1, ipvt, b, ref job);
        }






        // Llamada a la Librería de Fortran *.DLL MINPACK, en particular a la función CHKDER  
        // La funcion fucn_.dll se ha compilado desde código Fortran siguiendo estos pasos:
        // a)g95 -c chkder.f dpmpar.f func.f
        // para crear chkder.o dpmpar.o func.o
        // b)g95 -shared -mrtd -o fcn_.dll chkder.o dpmpar.o func.o
        // para crear fcn_.dll

        //Igual se ha hecho con chkder.dll sólo cambiando el nombre de la dll y la función de llamada chkder_
        //el símbolo _ siginifa un espacio en blando después del nombre de la función en código fortran (ver chkder.f)
        private void button8_Click(object sender, EventArgs e)
        {
            int m, n, ldfjac, mode, nwrite,iflag;
            
            double[] x=new double[3];
            double[] fvec = new double[15];
            double[,] fjac = new double[15,3];
            double[] xp=new double[3];
            double[] fvecp=new double[15];
            double[] err=new double[15];

            m = 15;
            n = 3;

            x[0] = 9.2e-1;
            x[1] = 1.3e-1;
            x[2] = 5.4e-1;

            ldfjac = 15;

            mode = 1;
            chkder_(ref m, ref n, x, fvec, fjac, ref ldfjac, xp, fvecp, err);
            mode = 2;
            iflag = 1;
            fcn_(ref m, ref n,  x, fvec, fjac, ref ldfjac, ref iflag);
            iflag = 2;
            fcn_(ref m, ref n, x, fvec, fjac, ref ldfjac, ref iflag);
            iflag = 1;
            fcn_(ref m, ref n, xp, fvecp, fjac, ref ldfjac, ref iflag);
            chkder_(ref m, ref n, x, fvec, fjac, ref ldfjac, xp, fvecp, err);

            for (int i= 0; i < m; i++)
            {             
               fvecp[i]=fvecp[i]-fvec[i];               
            }

            for (int i = 0; i < m; i++)
            {
                MessageBox.Show("FVEC: " + Convert.ToString(fvec[i]));
                MessageBox.Show("FVECP-FVEC: " + Convert.ToString(fvecp[i]));
                MessageBox.Show("ERR: " + Convert.ToString(err[i]));
            }
        }





        //Call HYBRD.dll a Fortran dynamic link library
        //Método de compilación en Fortran:       
        private void button9_Click(object sender, EventArgs e)
        {

            int n, maxfev, ml, mu, mode, nprint, info, nfev, ldfjac, lr, nwrite;
            double xtol,epsfcn,factor,fnorm;
            
            double[] x = new double[9];
            double[] fvec = new double[9];
            double[] diag = new double[9];
            double[,] fjac = new double[9,9];
            double[]  r= new double[9];
            double[] qtf = new double[9];

            double[] wa1= new double[9];
            double[] wa2 = new double[9];
            double[] wa3 = new double[9];
            double[] wa4 = new double[9];

            double enorm,dpmpar;

            n = 9;

            for (int i = 0; i < 9; i++)
            {
                x[i] = -1;          
            }

            ldfjac = 9;
            lr = 45;
            
            xtol = Math.Sqrt(2.22044604926e-16);

            maxfev=2000;

            ml=1;
            mu=1;
            epsfcn=0;
            mode=2;

            for (int j=0;j<9;j++)
            {
               diag[j]=1;            
            }

            nwrite = 6;

            factor = 100;

            nprint = 0;

            //fun1 luis = new fun1(fcn1_);

            nfev = 1;

            info = 1;

            //hybrd_(luis,ref n,x,fvec,ref xtol,ref maxfev,ref ml,ref mu,ref epsfcn,diag,
            //    ref mode,ref factor,ref nprint,ref info,
            //    ref nfev,fjac,ref ldfjac,r,ref lr,qtf,wa1,wa2,wa3,wa4);     
            int b = 25;
            double[,] irene = new double[9, 9];
            double[] resultado = new double[9];

            //Lamada al WRAPPER "luis " de la función HYBRD de la librería MINPACK para resolución de Sistema de Ecuaciones No Lineales. Ejemplo de validación de la documentación original de la librería
            luis_(ref b,irene,resultado);

            //Mostramos los resultados obtenidos 
            for (int i = 0;i<(int)resultado.Count();i++)
            {
                MessageBox.Show("Resultado "+ Convert.ToString(i)+" : "+Convert.ToString(resultado[i]));
            }
        }

        //Compilar una Dll de Fortran en Runtime 
        private void button10_Click(object sender, EventArgs e)
        {          
            string dir1 = @"C:\MinGw\bin\";
            Directory.SetCurrentDirectory(dir1);
            Process p = new System.Diagnostics.Process();
            // Contenido del archivo "compilar.bat". En este archivo se indica el directorio donde queremos escribir la librería dinámica *.DLL de Fortran
            // g95 -c hybrdirene.f
            // g95 -shared -mrtd -o C:\Users\LUISCOCO\Desktop\prueba.dll hybrdirene.o            
            
            //p.StartInfo.Arguments = "argument1 argument2 argument3";
            p.StartInfo.FileName = "compilarluis.bat";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;                     

            p.Start();   
            p.WaitForExit();

            string AppPath = System.Windows.Forms.Application.StartupPath;
            Directory.SetCurrentDirectory(AppPath);
        }

        //Validación DASSL Sample3: Ejecutar el Algoritmo de DASSL (Dassl.dll dynamic library call)
        private void button11_Click(object sender, EventArgs e)
        {
            //Número de ecuaciones del Sistema de Ecuaciones Diferenciales Algebraicas (DAE)
            int NEQ = 20;            

            //Tiempo inicial
            double T = 0;

            //Inicialización del Tiempo Final calculado 
            double TOUT = 0.03; 

            //Incremento de tiempo cada intervalo
            double DTOUT = 1;
            //double DTOUT = 0.1;

            //Condiciones iniciales de las y
            double[] Y = new double[45];         
            Y[0]=-0.0617138900;
            Y[1]=0.0;
            Y[2]=0.452279;
            Y[3]=0.22266839;
            Y[4]=0.4873649;
            Y[5]=-0.222668;
            Y[6]=1.230547;
            for (int i=7;i<20;i++)
            {
               Y[i]=0.00;
            }
            Y[14]=98.5668;
            Y[15]=-6.1226883;

            //Delta=F(x,y,y'), es decir, el error de cada ecuación implícita antes de ser solucionada
            double[] DELTA = new double[45];

            //Condiciones iniciales de las y'
            double[] YPRIME = new double[45];
                  
            //Opciones de ejecución del Programa. Estas variables deberán ser elegidas por el Usuario en un Cuadro de Diálogo 
            //explicando bien el significado de cada una de las opciones elegidas
            int[] INFO = new int[15];
            INFO[0] = 0;
            //INFO[1] = 0;
            //INFO[2] = 0;
            INFO[3] = 0;
            INFO[4] = 0;
            INFO[5] = 0;
            INFO[6] = 0;
            INFO[7] = 0;
            INFO[8] = 0;
            INFO[9] = 0;
            INFO[10] = 0;
            INFO[11] = 0;
            INFO[12] = 0;
            INFO[13] = 0;
            INFO[14] = 0;

            INFO[1] = 1;
            INFO[2] = 1;

            //Error Relativo permitido
            double[] RTOL = new double[45];

            //Error Absoluto permitido
            double[] ATOL = new double[45];

            for (int i = 0; i < 7; i++)
            { 
              RTOL[i]=0.005;
              ATOL[i] = 0.005;
            }

            for (int i = 7; i < 20; i++)
            {
                RTOL[i] = 1;
                ATOL[i] = 1;
            }

            //Indicador de los resultadod de la ejecución del algoritmo
            int IDID = 0;

            //Array de trabajo de números reales
            double[] RWORK = new double[1055];

            //Longitud del array de trabajo de números reales
            int LRW = 1055;

            //Arrays de trabajo de números enteros
            int[] IWORK = new int[1000];

            //Longitud del array de trabajo de números enteros
            int LIW = 1000;

            //Arrays de intercambio de datos de números reales
            double[] RPAR = new double[1];
            RPAR[0] = 0.1;

            //Arrays de intercambio de datos de números enteros
            int[] IPAR = new int[1];
            IPAR[0] = 11;

            //Variables auxiliares 
            double LUN = 6;
            double KPRINT = 3;
            double IPASS = 1;
            double NERR = 0;
            double ERO = 0;
            double HU = 0;
            double NQU = 0;
            double NST = 0;
            double NFE = 0;
            double NJE = 0;
            double YT1 = 0;
            double YT2 = 0;
            double ER1 = 0;
            double ER2 = 0;
            double ER = 0;

                    
                //Llamada al Wrapper "luis " de la función del algoritmo de DASSL 
     LABEL22:   luis2_(ref NEQ, ref T, Y, YPRIME, ref TOUT, INFO, RTOL, ATOL, ref IDID, RWORK, ref LRW, IWORK, ref LIW, RPAR, IPAR);

                 //  MessageBox.Show("Resultado de T: " + Convert.ToString(T));

                for (int i2 = 0; i2 < Y.Count(); i2++)
                {
                 //  MessageBox.Show("Resultado de Y" + Convert.ToString(i2) + " : " + Convert.ToString(Y[i2]));
                }

                HU = RWORK[6];
                 //  MessageBox.Show("Resultado de H: " + Convert.ToString(HU));

                NQU = IWORK[7];
                 //  MessageBox.Show("Resultado de NQ: " + Convert.ToString(NQU));

                //Si el valor de IDID es negativo el Algoritmo de Dassl ha dado error al ejecutarlo
                if (IDID < 0) goto LABEL175;
                if (T >= TOUT) goto LABEL175;
                if (IDID >= 1) goto LABEL22;

        LABEL175:

            if (IDID < 0) NERR = NERR + 1;
            NST = IWORK[10];
            NFE = IWORK[11];
            NJE = IWORK[12];

            MessageBox.Show("Number Of Steps: " + Convert.ToString(NST));
            MessageBox.Show("Number Of F-S: " + Convert.ToString(NFE));
            MessageBox.Show("Number Of J-S: " + Convert.ToString(NJE));          
        }

        //Validación de Dassl Sample2
        private void button12_Click(object sender, EventArgs e)
        {
            //Número de ecuaciones del Sistema de Ecuaciones Diferenciales Algebraicas (DAE)
            int NEQ = 20;

            //Número de Intervalos de tiempo calculados
            int NOUT = 10;

            //Tiempo inicial
            double T = 0;

            //Inicialización del Tiempo Final calculado 
            double TOUT = 0.03;

            //Incremento de tiempo cada intervalo
            double DTOUT = 1;
            //double DTOUT = 0.1;

            //Condiciones iniciales de las y
            double[] Y = new double[20];
            Y[0] = -0.0617138900;
            Y[1] = 0.0;
            Y[2] = 0.452279;
            Y[3] = 0.22266839;
            Y[4] = 0.4873649;
            Y[5] = -0.222668;
            Y[6] = 1.230547;
            for (int i = 7; i < 20; i++)
            {
                Y[i] = 0.00;
            }
            Y[14] = 98.5668;
            Y[15] = -6.1226883;

            //Delta=F(x,y,y'), es decir, el error de cada ecuación implícita antes de ser solucionada
            double[] DELTA = new double[20];

            //Condiciones iniciales de las y'
            double[] YPRIME = new double[20];

            //Opciones de ejecución del Programa. Estas variables deberán ser elegidas por el Usuario en un Cuadro de Diálogo 
            //explicando bien el significado de cada una de las opciones elegidas
            int[] INFO = new int[15];
            INFO[0] = 0;
            //INFO[1] = 0;
            //INFO[2] = 0;
            INFO[3] = 0;
            INFO[4] = 0;
            INFO[5] = 0;
            INFO[6] = 0;
            INFO[7] = 0;
            INFO[8] = 0;
            INFO[9] = 0;
            INFO[10] = 0;
            INFO[11] = 0;
            INFO[12] = 0;
            INFO[13] = 0;
            INFO[14] = 0;

            INFO[1] = 1;
            INFO[2] = 1;

            //Error Relativo permitido
            double[] RTOL = new double[20];

            //Error Absoluto permitido
            double[] ATOL = new double[20];

            for (int i = 0; i < 7; i++)
            {
                RTOL[i] = 0.005;
                ATOL[i] = 0.005;
            }

            for (int i = 7; i < 20; i++)
            {
                RTOL[i] = 1;
                ATOL[i] = 1;
            }

            //Indicador de los resultadod de la ejecución del algoritmo
            int IDID = 0;

            //Array de trabajo de números reales
            double[] RWORK = new double[1055];

            //Longitud del array de trabajo de números reales
            int LRW = 1055;

            //Arrays de trabajo de números enteros
            int[] IWORK = new int[1000];

            //Longitud del array de trabajo de números enteros
            int LIW = 1000;

            //Arrays de intercambio de datos de números reales
            double[] RPAR = new double[1];
            RPAR[0] = 0.1;

            //Arrays de intercambio de datos de números enteros
            int[] IPAR = new int[1];
            IPAR[0] = 11;

            //Variables auxiliares 
            double LUN = 6;
            double KPRINT = 3;
            double IPASS = 1;
            double NERR = 0;
            double ERO = 0;
            double HU = 0;
            double NQU = 0;
            double NST = 0;
            double NFE = 0;
            double NJE = 0;
            double YT1 = 0;
            double YT2 = 0;
            double ER1 = 0;
            double ER2 = 0;
            double ER = 0;

            for (int i1 = 1; i1 <= NOUT; i1++)
            {
                //Llamada al Wrapper "luis " de la función del algoritmo de DASSL 
                luis2_(ref NEQ, ref T, Y, YPRIME, ref TOUT, INFO, RTOL, ATOL, ref IDID, RWORK, ref LRW, IWORK, ref LIW, RPAR, IPAR);

                MessageBox.Show("Resultado de T: " + Convert.ToString(T));
                for (int i2 = 0; i2 < Y.Count(); i2++)
                {
                    MessageBox.Show("Resultado de Y" + Convert.ToString(i2) + " : " + Convert.ToString(Y[i2]));
                }

                HU = RWORK[6];
                MessageBox.Show("Resultado de H: " + Convert.ToString(HU));
                NQU = IWORK[7];
                MessageBox.Show("Resultado de NQ: " + Convert.ToString(NQU));

                //Si el valor de IDID es negativo el Algoritmo de Dassl ha dado error al ejecutarlo
                if (IDID < 0) goto LABEL175;

                NERR = NERR + 1;

                //Incrementamos el tiempo en un intervalo DOUT para calcular el siguiente valor del tiempo 
                TOUT = TOUT + DTOUT;
            }

        LABEL175:

            if (IDID < 0) NERR = NERR + 1;
            NST = IWORK[10];
            NFE = IWORK[11];
            NJE = IWORK[12];

            MessageBox.Show("Number Of Steps: " + Convert.ToString(NST));
            MessageBox.Show("Number Of F-S: " + Convert.ToString(NFE));
            MessageBox.Show("Number Of J-S: " + Convert.ToString(NJE));       
        }  
   }
}
