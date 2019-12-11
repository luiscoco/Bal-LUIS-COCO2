using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;

//Para acceder a las librerías dinámicas *.dll de FORTRAN
using System.Runtime.InteropServices;

//Para ejecutar un archivo *.bat
using System.Diagnostics;

using System.Reflection;

using CSharpScripter;

namespace DasslInterface
{
    public partial class Form1Dassl : Form
    {
        public string ruta = System.Windows.Forms.Application.StartupPath;

        int numeq = 0;

        public double[] Y;

        public double[] Yexacta;
        public double[] ERO;
        public double[] ER;
        public double[] YT1;
        public double[] YT2;

        public double[] YPRIME;
        
        public double[] RTOL;
        public double[] ATOL;

        public double RTOL1;
        public double ATOL1;

        public string nomfuncion;

        public double Tin;
        
        public double Tout;

        public int[] INFO;
        
        public double DTOUT;
        
        public int NOUT;
        
        public int IDID;

        public double[] RWORK;

        public int LRW;

        public int LIW;

        public int[] IWORK;

        public double[] RPAR;

        public int[] IPAR;

        public string fileread;

        public IntPtr pDll;
        public IntPtr pDll1;

        public IntPtr pAddressOfFunctionToCall;
        public IntPtr pAddressOfFunctionToCall1; 

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void luis1_(ref int NEQ, ref double T, double[] Y, double[] YPRIME, ref double TOUT, int[] INFO, double[] RTOL, double[] ATOL, ref int IDID, double[] rwork, ref int lrw, int[] iwork, ref int liw, double[] rpar, int[] ipar);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void luis_(ref int NEQ, ref double T, double[] Y, double[] YPRIME, ref double TOUT, int[] INFO, double[] RTOL, double[] ATOL, ref int IDID, double[] rwork, ref int lrw, int[] iwork, ref int liw, double[] rpar, int[] ipar, double[] Yexacta);


        //Wrapper del algoritmo de DASSL
        
        //[DllImport("wrapper.dll", EntryPoint = "luis1_", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        //public static extern void luis1_(ref int NEQ, ref double T, double[] Y, double[] YPRIME, ref double TOUT, int[] INFO, double[] RTOL, double[] ATOL, ref int IDID, double[] rwork, ref int lrw, int[] iwork, ref int liw, double[] rpar, int[] ipar);

        //[DllImport("wrapper4.dll", EntryPoint = "luis_", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        //public static extern void luis_(ref int NEQ, ref double T, double[] Y, double[] YPRIME, ref double TOUT, int[] INFO, double[] RTOL, double[] ATOL, ref int IDID, double[] rwork, ref int lrw, int[] iwork, ref int liw, double[] rpar, int[] ipar, double[] Yexacta);

        public Form1Dassl()
        {
            InitializeComponent();       
        }        

        //
        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView3.Enabled = false;
            dataGridView4.Enabled = false;
            textBox10.Text = ruta+"\\";
            richTextBox2.Text = "";

            string sourceDir = Directory.GetCurrentDirectory();
            File.Delete(sourceDir + "\\wrapper.dll");
            File.Delete(sourceDir + "\\wrapper4.dll");
        }

        //Botón de OK
        private void button2_Click(object sender, EventArgs e)
        {         
            this.Hide();
            this.Dispose();
        }

      
        //Botón de RUN DASSL 
        private void button7_Click_1(object sender, EventArgs e)
        {
            if (checkBox15.Checked == false)
            {
                pDll = NativeMethods.LoadLibrary("wrapper.dll");
                pAddressOfFunctionToCall = NativeMethods.GetProcAddress(pDll, "luis1_");               
            }

            else if (checkBox15.Checked == true)
            {
                pDll1 = NativeMethods.LoadLibrary("wrapper4.dll");
                pAddressOfFunctionToCall1 = NativeMethods.GetProcAddress(pDll1, "luis_");              
            }
                                                                     
            //Leemos el nombre de la Función RES y JAC
            nomfuncion = textBox1.Text;

            // Leemos las Condiciones Iniciales de Y[0],Y[1],...,etc
            numeq = Convert.ToInt32(textBox2.Text);

            // Leemos el Tiempo Inicial Tin y Tiempo Final Tout
            Tin = Convert.ToDouble(textBox3.Text);

            //Incremento de tiempo cada intervalo
            DTOUT = Convert.ToDouble(textBox12.Text);

            //Número de Intervalos de tiempo calculados
            NOUT = Convert.ToInt32(textBox11.Text);

            Tout = Tin+DTOUT;           

            //Inidicador de ejecución de Dassl
            IDID = 77;         

            INFO = new int[15];

            for (int i = 0; i < 15; i++)
            {
                INFO[i] = 0;
            }  

            int numfilas = Convert.ToInt16(dataGridView1.RowCount);
            int numcolumnas = Convert.ToInt16(dataGridView1.ColumnCount);

            //Comprobamos si el número de valores de la tabla es menor que el número de valores que hemos definido en el textBox2
            if (numfilas > numeq)
            {
                numfilas = numeq;
            }
            else if (numfilas < numeq)
            {
                MessageBox.Show("Error!!!: [Y] initial conditions vector size is less than DAE System equations number.");
                return;
            }

            Y = new double[numfilas];

            if(checkBox15.Checked==true)
            {
               Yexacta = new double[numfilas];
            }

            for (int i = 0; i < numfilas; i++)
            {
                Y[i] = Convert.ToDouble(dataGridView1.Rows[i].Cells[0].Value);

                if (checkBox15.Checked == true)
                {
                    Yexacta[i] = 0;
                }
            }

            //Leemos las Condiciones Iniciales de Y'[0],Y'[1],...,etc
            numeq = Convert.ToInt16(textBox2.Text);

            int numfilas1 = Convert.ToInt16(dataGridView2.RowCount);
            int numcolumnas1 = Convert.ToInt16(dataGridView2.ColumnCount);

            //Comprobamos si el número de valores de la tabla es menor que el número de valores que hemos definido en el textBox2
            if (numfilas1 > numeq)
            {
                numfilas1 = numeq;
            }

            else if (numfilas1 < numeq)
            {
                MessageBox.Show("Error!!!: [Y'] initial conditions vector size is less than DAE System equations number.");
                return;
            }

            YPRIME = new double[numfilas];

            for (int i = 0; i < numfilas; i++)
            {
                YPRIME[i] = Convert.ToDouble(dataGridView2.Rows[i].Cells[0].Value);
            }

            //Leemos los valores de los Errores Relativos (RTOL) si se trata de un Vector 
            if (checkBox2.Checked == true)
            {
                dataGridView3.Enabled = true;

                numeq = Convert.ToInt16(textBox2.Text);

                int numfilas2 = Convert.ToInt16(dataGridView3.RowCount);
                int numcolumnas2 = Convert.ToInt16(dataGridView3.ColumnCount);

                //QUINTO:Comprobamos si el número de valores de la tabla es menor que el número de valores que hemos definido en el textBox2
                if (numfilas2 > numeq)
                {
                    numfilas2 = numeq;
                }

                RTOL = new double[numfilas];

                for (int i = 0; i < numfilas; i++)
                {
                    RTOL[i] = Convert.ToDouble(dataGridView3.Rows[i].Cells[0].Value);
                }

                ATOL = new double[numfilas];

                for (int i = 0; i < numfilas; i++)
                {
                    ATOL[i] = Convert.ToDouble(dataGridView4.Rows[i].Cells[0].Value);
                }

            }

            else if (checkBox2.Checked == false)
            {
                RTOL = new double[1];
                ATOL = new double[1];

                RTOL1 = Convert.ToDouble(textBox7.Text);
                RTOL[0] = RTOL1;

                ATOL1 = Convert.ToDouble(textBox8.Text);
                ATOL[0] = ATOL1;
            }              
                  
            //Longitud (LRW) del array de trabajo de números reales RWORK:
            //LRW .GE. 40+(MAXORD+4)*NEQ+NEQ**2
            //for the full (dense) JACOBIAN case (when INFO(6)=0), or

            //LRW .GE. 40+(MAXORD+4)*NEQ+(2*ML+MU+1)*NEQ
            //for the banded user-defined JACOBIAN case

            //(when INFO(5)=1 and INFO(6)=1),

            //or LRW .GE. 40+(MAXORD+4)*NEQ+(2*ML+MU+1)*NEQ+2*(NEQ/(ML+MU+1)+1)
            //for the banded finite-difference-generated JACOBIAN case
            //(when INFO(5)=0 and INFO(6)=1)

            if(INFO[5]==0)
            {
                if(INFO[8]==0)
                {
                   LRW = 40 + ((5 + 4) * numeq) +(numeq * numeq);   
                }

                else if (INFO[8] == 1)
                {
                    LRW = 40 + ((Convert.ToInt16(textBox9.Text) + 4) * numeq) + (numeq * numeq);   
                }
            }

            else if((INFO[4]==1)&&(INFO[5]==1))
            {
                if (INFO[8] == 0)
                {
                    LRW = 40 + ((5 + 4) * numeq) +(((2*IWORK[0])+IWORK[1]+1) * numeq);
                }

                else if (INFO[8] == 1)
                {
                    LRW = 40 + ((Convert.ToInt16(textBox9.Text) + 4) * numeq) + (((2 * IWORK[0]) + IWORK[1] + 1) * numeq);
                }           
            }

            else if ((INFO[4] == 0) && (INFO[5] == 1))
            {
                if (INFO[8] == 0)
                {
                    LRW = 40 + ((5 + 4) * numeq) + (((2 * IWORK[0]) + IWORK[1] + 1) * numeq)+(2*(numeq/(IWORK[0]+IWORK[1]+1)));
                }

                else if (INFO[8] == 1)
                {
                    LRW = 40 + ((Convert.ToInt16(textBox9.Text) + 4) * numeq) + (((2 * IWORK[0]) + IWORK[1] + 1) * numeq) + (2 * (numeq / (IWORK[0] + IWORK[1] + 1)));
                }
            }

            //Array de trabajo de números reales
            RWORK = new double[LRW];                  

            //Longitud del array de trabajo de números enteros
            // LIW >= 20+NEQ, en este caso tenemos 2 ecuaciones diferenciales por tanto LIW como mínimo será 22
            LIW = 20 + numeq;

            //Arrays de trabajo de números enteros
            IWORK = new int[LIW];

            //Arrays de intercambio de datos de números reales
            RPAR = new double[1];
            RPAR[0] = 0.1;

            //Arrays de intercambio de datos de números enteros
            IPAR = new int[1];
            IPAR[0] = 11;

            //Variables auxiliares 
            double NERR = 0;       

            //Opción de INFO(1) Enables the program to initialize itself.
            if (checkBox1.Checked == true)
            {
                INFO[0] = 1;
            }
            else if (checkBox1.Checked == false)
            {
                INFO[0] = 0;
            }

            //Opción de INFO(2) ATOL y RTOL Scalars(unchecked) or Vectors (checked)
            if (checkBox2.Checked == true)
            {
                INFO[1] = 1;
            }
            else if (checkBox2.Checked == false)
            {
                INFO[1] = 0;
            }

            //Opción de INFO(3) Solution at Intermediate step or at Tout.
            if (checkBox3.Checked == true)
            {
                INFO[2] = 1;

            }
            else if (checkBox3.Checked == false)
            {
                INFO[2] = 0;
            }

            //Opción de INFO(4)
            if (checkBox4.Checked == true)
            {
                INFO[3] = 1;
            }
            else if (checkBox4.Checked == false)
            {
                INFO[3] = 0;
            }

            //Opción de INFO(5) Code evaluates partial derivatives matrix 
            if (checkBox5.Checked == true)
            {
                if (richTextBox3.Text == "")
                {
                    MessageBox.Show("Warning!: No JAC Subroutine has been defined in first step.");
                    return;
                }

                INFO[4] = 1;
            }
            else if (checkBox5.Checked == false)
            {
                INFO[4] = 0;
            }

            //Opción de INFO(6) Solve using dense or banded partial derivatives matrix
            if (checkBox6.Checked == true)
            {
                INFO[5] = 1;
            }
            else if (checkBox6.Checked == false)
            {
                INFO[5] = 0;
            }

            //Opción de INFO(7) Specify Maximum Stepsize
            if (checkBox7.Checked == true)
            {
                INFO[6] = 1;
                RWORK[1] = Convert.ToDouble(textBox5.Text);
            }

            else if (checkBox7.Checked == false)
            {
                INFO[6] = 0;              
            }

            //Opción de INFO(8) Specify Initial Stepsize
            if (checkBox8.Checked == true)
            {
                INFO[7] = 1;
                RWORK[2]=Convert.ToDouble(textBox6.Text);
            }

            else if (checkBox8.Checked == false)
            {
                INFO[7] = 0;
            }

            //Opción de INFO(9) Maximum Order Default Value
            if (checkBox9.Checked == true)
            {
                INFO[8] = 1;
                IWORK[2] = (int)Convert.ToInt16(textBox9.Text);
            }

            else if (checkBox9.Checked == false)
            {
                INFO[8] = 0;
            }

            //Opción de INFO(10) Solutions to your equations will be always Nonnegative
            if (checkBox10.Checked == true)
            {
                INFO[9] = 1;               
            }
            else if (checkBox10.Checked == false)
            {
                INFO[9] = 0;
            }

            //Opción de INFO(11) Are the Initial T,Y,Yprime consistent?
            if (checkBox11.Checked == true)
            {
                INFO[10] = 1;
            }
            else if (checkBox11.Checked == false)
            {
                INFO[10] = 0;
            }


            //Creamos en Runtime las columnas del DataGrid5 para visualizar los resultados 
            for (int i = 0; i < Y.Count()+1; i++)
            {               
               DataGridViewTextBoxColumn temp = new DataGridViewTextBoxColumn();
               DataGridViewTextBoxColumn temp1 = new DataGridViewTextBoxColumn();

               if (i == 0)
               {
                   temp.HeaderText = "T";
                   temp.Name = "T";
                   temp.Width = 50;
                   temp.Visible = true;
               }
               else if (i != 0)
               {
                   temp.HeaderText = "Y[" + Convert.ToString(i) + "]";
                   temp.Name = "Y[" + Convert.ToString(i) + "]";
                   temp.Width = 50;
                   temp.Visible = true;

                   if (checkBox15.Checked == true)
                   {                       
                       temp1.HeaderText = "YT[" + Convert.ToString(i) + "]";
                       temp1.Name = "YT[" + Convert.ToString(i) + "]";
                       temp1.Width = 50;
                       temp1.Visible = true;                   
                   }
               }
                   dataGridView5.Columns.Add(temp);

                   if ((checkBox15.Checked == true)&&(i!=0))
                   {
                       dataGridView5.Columns.Add(temp1);
                   }
            }

            if (checkBox15.Checked == true)
            {
                DataGridViewTextBoxColumn temp2 = new DataGridViewTextBoxColumn();
                temp2.HeaderText = "ERROR";
                temp2.Name = "ERROR";
                temp2.Width = 50;
                temp2.Visible = true;
                dataGridView5.Columns.Add(temp2);
            }                                               
                //Creamos una matriz de double donde guardaremos los resultados del Algoritmo de DASSL
                double[,] resultados=new double[NOUT,Y.Count()+1];

                //Creamos una matriz de double donde guardaremos los resultados del Algoritmo de DASSL
                double[,] resultadosEXACTOS = new double[NOUT, Y.Count() + 1];

            //Máximo error de todas las corridas del Algoritmo Dassl
                ERO = new double[NOUT]; 
            //Máximo error de todas las funciones Y[] en cada una de las corridas del Algoritmo Dassl
                ER=new double[Y.Count()];

                //Corremos el algoritmo de Dassl NOUT veces
                for (int i1 = 0; i1 < NOUT; i1++)
                {
                    if (checkBox15.Checked == true)
                    {
                        luis_ luis_ = (luis_)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall1, typeof(luis_));
                        //Llamada al Wrapper "luis " de la función del algoritmo de DASSL 
                        luis_(ref numeq, ref Tin, Y, YPRIME, ref Tout, INFO, RTOL, ATOL, ref IDID, RWORK, ref LRW, IWORK, ref LIW, RPAR, IPAR, Yexacta);
                    }

                    else if (checkBox15.Checked == false)
                    {
                        luis1_ luis1_ = (luis1_)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(luis1_));
                        //Llamada al Wrapper "luis " de la función del algoritmo de DASSL 
                        luis1_(ref numeq, ref Tin, Y, YPRIME, ref Tout, INFO, RTOL, ATOL, ref IDID, RWORK, ref LRW, IWORK, ref LIW, RPAR, IPAR);
                    }
                    //Guardamos los resultados de cada una de las corridas de DASSL
                    resultados[i1,0] = Tin;
                    
                    for (int i = 1; i < Y.Count()+1; i++)
                    {                                             
                        resultados[i1,i] = Y[i-1];

                        if (checkBox15.Checked == true)
                        {
                            resultadosEXACTOS[i1, i] = Yexacta[i - 1];
                        }
                    }                              
                                 
                    //Si el valor de IDID es negativo el Algoritmo de Dassl ha dado error al ejecutarlo
                    if (IDID < 0) goto LABEL175;

                    if (checkBox15.Checked == true)
                    {
                        for (int i = 0; i < Y.Count(); i++)
                        {
                            ER[i] = Math.Abs(Yexacta[i] - Y[i]);                          
                        }
                    }

                    listBox1.Items.Add("Dassl Run Nº "+Convert.ToString(i1+1)+" ; T:"+Convert.ToString(Tin));
                    listBox1.Items.Add("IDID Value: " + Convert.ToString(IDID));
                    listBox1.Items.Add("Step Size: " + Convert.ToString(RWORK[6]));
                    listBox1.Items.Add("Number of Steps: "+Convert.ToString(IWORK[10]));
                    listBox1.Items.Add("Number of call RES so far: " + Convert.ToString(IWORK[11]));
                    listBox1.Items.Add("Number of call JAC so far: " + Convert.ToString(IWORK[12]));
                    listBox1.Items.Add("Order of the method on last step: " + Convert.ToString(IWORK[7]));
                    listBox1.Items.Add("Order of the method on next step: "+Convert.ToString(IWORK[6]));
                    
                    listBox1.Items.Add("");                                            

                    //Incrementamos el tiempo en un intervalo DOUT para calcular el siguiente valor del tiempo 
                    Tout = Tout + DTOUT;

                    //Guardamos el máximo error de corrida del Algoritmo de Dassl 
                    double errortemp = 0;
                    for (int i = 0; i < ER.Count(); i++)
                    {
                        if (ER[i] > errortemp)
                        {
                            errortemp = ER[i];
                        }
                    }

                    ERO[i1] = errortemp;
                }
                
             
                //Mostramos los valores guardados en la matriz resultados en el control gridView5
                for (int i = 0; i < NOUT; i++)
                {
                    dataGridView5.Rows.Add();

                    if (checkBox15.Checked == false)
                    {
                        for (int j = 0; j < Y.Count() + 1; j++)
                        {
                            dataGridView5.Rows[i].Cells[j].Value = resultados[i, j];
                        }
                    }

                    else if(checkBox15.Checked == true)
                    {
                        int cont = 0;
                        int cont1 = 0;
                        for (int j = 0; j < Y.Count() + 1; j++)
                        {
                            if (j < 2)
                            {
                                dataGridView5.Rows[i].Cells[j].Value = resultados[i, j];
                            }

                            else if(j>=2)
                            {
                                dataGridView5.Rows[i].Cells[j+1+cont].Value = resultados[i, j];
                                cont++;
                            }
                        }

                        for (int j = 0; j < Y.Count(); j++)
                        {                       
                                dataGridView5.Rows[i].Cells[j+2+cont1].Value = resultadosEXACTOS[i, j+1];
                                cont1++;
                        }

                         dataGridView5.Rows[i].Cells[2*Y.Count()+1].Value=ERO[i];
                    }
                 }
                if (checkBox15.Checked == false)
                {
                    bool result = NativeMethods.FreeLibrary(pDll);
                }

                else if (checkBox15.Checked == true)
                {
                    bool result1 = NativeMethods.FreeLibrary(pDll1);
                }

                return;

            LABEL175:if (IDID < 0) NERR = NERR + 1; MessageBox.Show("ERROR in Dassl Algorithm Rurn, IDID returned negative number.");        
        }

        private void checkBox13_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox13.Checked == true)
            {
                checkBox12.Checked = false;
                richTextBox3.ReadOnly = false;
            }

            else if(checkBox13.Checked==false)
            {
                
            }
        }

        private void checkBox12_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox12.Checked == true)
            {
                checkBox13.Checked = false;
                richTextBox3.ReadOnly = true;
            }

            else if (checkBox12.Checked == false)
            {

            }
        }

        //Botón de RESET all the variables and calculations
        private void button9_Click(object sender, EventArgs e)
        {           
            //Borrar los ficheros de ejecuciones anteriores: wrapper.f, wrapper4.f wrapper.o wrapper4.o wrapper.dll wrapper4.dll
            string sourceDir = Directory.GetCurrentDirectory();        
            File.Delete(sourceDir +"\\wrapper.f");
            File.Delete(sourceDir + "\\wrapper4.f");
            File.Delete(sourceDir + "\\wrapper.o");
            File.Delete(sourceDir + "\\wrapper4.o");
            File.Delete(sourceDir + "\\wrapper.dll");
            File.Delete(sourceDir + "\\wrapper4.dll");
            File.Delete(sourceDir + "\\compile.bat");
            File.Delete(sourceDir + "\\compile4.bat");



            //Controls Initialization
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();
            dataGridView4.Rows.Clear();
            dataGridView5.Rows.Clear();

            dataGridView5.Columns.Clear();

            listBox1.Items.Clear();

            richTextBox1.Text = "";
            richTextBox2.Text = "";
            richTextBox3.Text = "";
            richTextBox4.Text = "";

            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
            checkBox7.Checked = false;
            checkBox8.Checked = false;
            checkBox9.Checked = false;
            checkBox10.Checked = false;
            checkBox10.Checked = false;

            textBox5.Text = "";
            textBox6.Text = "";
            textBox9.Text = "";

            //Variables Initialization       
            Y=null;
            YPRIME=null;        
            RTOL=null;
            ATOL=null;
            RTOL1=0;
            ATOL1=0;
            nomfuncion="";
            Tin=0;        
            Tout=0;
            INFO = null;        
            DTOUT=0;      
            NOUT=0;        
            IDID=0;
            RWORK=null;
            LRW=0;
            LIW=0;
            IWORK=null;
            RPAR=null;
            IPAR=null;
            fileread ="";

            tabControl1.SelectedTab = tabPage1;
        }

        //Validation Sample 1
        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox3.Text = "";
            this.richTextBox3.Text = "      DIMENSION Y(1),YPRIME(1),PD(2,2)\n      PD(1,1) = CJ + 10.0D0\n      PD(1,2) " +
               "= 0.0D0\n      PD(2,1) = 1.0D0\n      PD(2,2) = 1.0D0";

            richTextBox1.Text = "";
            this.richTextBox1.Text = "       DIMENSION Y(2),YPRIME(1),DELTA(2)\n       DELTA(1) = YPRIME(1) + 10.0D0*Y(1" +
               ")\n       DELTA(2) = Y(2) + Y(1) - 1.0D0";

            richTextBox4.Text = "";
            this.richTextBox4.Text = "      DIMENSION YT(2)\n      YT(1) = Exp(-10.0 * T) \n      YT(2) = 1.0 - YT(1)";

            richTextBox2.Text = "";

            dataGridView1.Rows.Add();
            dataGridView1.Rows[0].Cells[0].Value = 1;
            dataGridView1.Rows.Add();
            dataGridView1.Rows[1].Cells[0].Value = 0;
            
            
            dataGridView2.Rows.Add();
            dataGridView2.Rows[0].Cells[0].Value = -10;
            dataGridView2.Rows.Add();
            dataGridView2.Rows[1].Cells[0].Value = 10;
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Enabled == true)
            {
                textBox5.Enabled = true;
            }

            else if (checkBox7.Enabled == false)
            {
                textBox5.Enabled = false;
            }
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox8.Enabled == true)
            {
                textBox6.Enabled = true;
            }

            else if (checkBox8.Enabled == false)
            {
                textBox6.Enabled = false;
            }
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox9.Enabled == true)
            {
                textBox9.Enabled = true;
            }

            else if (checkBox9.Enabled == false)
            {
                textBox9.Enabled = false;
            }
        }

        //Cuando pulsemos el Botón de APPLY el código de DRES y DJAC en los richtexBox1 y richtextBox3 se unirán entre sí para dar lugar al wrapper.f
        private void button8_Click_1(object sender, EventArgs e)
        {
            richTextBox2.Text = "";

            if ((checkBox12.Checked == true) && (checkBox15.Checked == false))
            {
               //Este es el Código Standard de nuestro Wrapper de C# para el Algoritmo de DASSL en Fortran           
               richTextBox2.Text += "C     Esta zona de código con la llamada a la función dassl.f no de de ser modificada ni enseñada al usuario " + System.Environment.NewLine;
               richTextBox2.Text += System.Environment.NewLine;
               richTextBox2.Text += "      subroutine luis1 (NEQ,T,Y,YPRIME,TOUT,INFO,RTOL,ATOL,IDID," + System.Environment.NewLine;
               richTextBox2.Text += "     1     RWORK,LRW,IWORK,LIW,RPAR,IPAR)" + System.Environment.NewLine;
               richTextBox2.Text += System.Environment.NewLine;
               richTextBox2.Text += "!GCC$ ATTRIBUTES DLLEXPORT :: LUIS1" + System.Environment.NewLine;
               richTextBox2.Text += System.Environment.NewLine;
               richTextBox2.Text += "      IMPLICIT DOUBLE PRECISION(A-H,O-Z)" + System.Environment.NewLine;
               richTextBox2.Text += "      EXTERNAL DRES" + System.Environment.NewLine;
               richTextBox2.Text += "      INTEGER  NEQ, INFO(15), IDID, LRW, IWORK(*), LIW, IPAR(*)" + System.Environment.NewLine;
               richTextBox2.Text += "      DOUBLE PRECISION T, Y(*), YPRIME(*), TOUT, RTOL(*), ATOL(*)," + System.Environment.NewLine;
               richTextBox2.Text += "     *  RWORK(*),RPAR(*)" + System.Environment.NewLine;
               richTextBox2.Text += System.Environment.NewLine;
               richTextBox2.Text += "      CALL DDASSL(DRES,NEQ,T,Y,YPRIME,TOUT,INFO,RTOL,ATOL,IDID," + System.Environment.NewLine;
               richTextBox2.Text += "     1  RWORK,LRW,IWORK,LIW,RPAR,IPAR,DJAC)" + System.Environment.NewLine;
               richTextBox2.Text += System.Environment.NewLine;
               richTextBox2.Text += "      END" + System.Environment.NewLine;
            }

            else if ((checkBox12.Checked == false) && (checkBox15.Checked == false))
            {
                //Este es el Código Standard de nuestro Wrapper de C# para el Algoritmo de DASSL en Fortran
                richTextBox2.Text +="C     Esta zona de código con la llamada a la función dassl.f no de de ser modificada ni enseñada al usuario "+ System.Environment.NewLine;
                richTextBox2.Text +=System.Environment.NewLine;
                richTextBox2.Text += "      subroutine luis1 (NEQ,T,Y,YPRIME,TOUT,INFO,RTOL,ATOL,IDID,"+ System.Environment.NewLine;
                richTextBox2.Text += "     1     RWORK,LRW,IWORK,LIW,RPAR,IPAR)"+ System.Environment.NewLine;
                richTextBox2.Text += System.Environment.NewLine;
                richTextBox2.Text += "!GCC$ ATTRIBUTES DLLEXPORT :: LUIS1"+ System.Environment.NewLine;
                richTextBox2.Text +=System.Environment.NewLine;
                richTextBox2.Text += "      IMPLICIT DOUBLE PRECISION(A-H,O-Z)"+ System.Environment.NewLine;
                richTextBox2.Text += "      EXTERNAL DRES,DJAC"+ System.Environment.NewLine;
                richTextBox2.Text += "      INTEGER  NEQ, INFO(15), IDID, LRW, IWORK(*), LIW, IPAR(*)"+ System.Environment.NewLine;
                richTextBox2.Text += "      DOUBLE PRECISION T, Y(*), YPRIME(*), TOUT, RTOL(*), ATOL(*),"+ System.Environment.NewLine;
                richTextBox2.Text += "     *  RWORK(*),RPAR(*)"+ System.Environment.NewLine;
                richTextBox2.Text +=System.Environment.NewLine;
                richTextBox2.Text += "      CALL DDASSL(DRES,NEQ,T,Y,YPRIME,TOUT,INFO,RTOL,ATOL,IDID,"+ System.Environment.NewLine;
                richTextBox2.Text += "     1  RWORK,LRW,IWORK,LIW,RPAR,IPAR,DJAC)"+ System.Environment.NewLine;
                richTextBox2.Text += System.Environment.NewLine;
                richTextBox2.Text += "      END";
            }

            else if ((checkBox12.Checked == false) && (checkBox15.Checked == true))
            {
                //Este es el Código Standard de nuestro Wrapper de C# para el Algoritmo de DASSL en Fortran
               richTextBox2.Text +="C     Esta zona de código con la llamada a la función dassl.f no de de ser modificada ni enseñada al usuario "+ System.Environment.NewLine;
               richTextBox2.Text += System.Environment.NewLine;
               richTextBox2.Text +="      subroutine luis (NEQ,T,Y,YPRIME,TOUT,INFO,RTOL,ATOL,IDID,"+ System.Environment.NewLine;
               richTextBox2.Text +="     1     RWORK,LRW,IWORK,LIW,RPAR,IPAR,YT)"+ System.Environment.NewLine;
               richTextBox2.Text += System.Environment.NewLine;
               richTextBox2.Text +="!GCC$ ATTRIBUTES DLLEXPORT :: LUIS"+ System.Environment.NewLine;
               richTextBox2.Text += System.Environment.NewLine;
               richTextBox2.Text +="      IMPLICIT DOUBLE PRECISION(A-H,O-Z)"+ System.Environment.NewLine;
               richTextBox2.Text +="      EXTERNAL DRES,DJAC,DYT"+ System.Environment.NewLine;
               richTextBox2.Text +="      INTEGER  NEQ, INFO(15), IDID, LRW, IWORK(*), LIW, IPAR(*)"+ System.Environment.NewLine;
               richTextBox2.Text +="      DOUBLE PRECISION T, Y(*),YT(*),YPRIME(*), TOUT, RTOL(*), ATOL(*),"+ System.Environment.NewLine;
               richTextBox2.Text +="     *  RWORK(*),RPAR(*)"+ System.Environment.NewLine;
               richTextBox2.Text += System.Environment.NewLine;
               richTextBox2.Text +="      CALL DDASSL(DRES,NEQ,T,Y,YPRIME,TOUT,INFO,RTOL,ATOL,IDID,"+ System.Environment.NewLine;
               richTextBox2.Text +="     1  RWORK,LRW,IWORK,LIW,RPAR,IPAR,DJAC)"+ System.Environment.NewLine;
               richTextBox2.Text += System.Environment.NewLine;
               richTextBox2.Text +="      CALL DYT(T,YT)"+ System.Environment.NewLine;
               richTextBox2.Text += System.Environment.NewLine;
               richTextBox2.Text +="      END"+ System.Environment.NewLine;
            }

            else if ((checkBox12.Checked == true) && (checkBox15.Checked == true))
            {
                //Este es el Código Standard de nuestro Wrapper de C# para el Algoritmo de DASSL en Fortran
                richTextBox2.Text += "C     Esta zona de código con la llamada a la función dassl.f no de de ser modificada ni enseñada al usuario " + System.Environment.NewLine;
               richTextBox2.Text += System.Environment.NewLine;
               richTextBox2.Text += "      subroutine luis (NEQ,T,Y,YPRIME,TOUT,INFO,RTOL,ATOL,IDID," + System.Environment.NewLine;
               richTextBox2.Text += "     1     RWORK,LRW,IWORK,LIW,RPAR,IPAR,YT)" + System.Environment.NewLine;
               richTextBox2.Text += System.Environment.NewLine;
               richTextBox2.Text += "!GCC$ ATTRIBUTES DLLEXPORT :: LUIS" + System.Environment.NewLine;
               richTextBox2.Text += System.Environment.NewLine;
               richTextBox2.Text += "      IMPLICIT DOUBLE PRECISION(A-H,O-Z)" + System.Environment.NewLine;
               richTextBox2.Text += "      EXTERNAL DRES,DYT" + System.Environment.NewLine;
               richTextBox2.Text += "      INTEGER  NEQ, INFO(15), IDID, LRW, IWORK(*), LIW, IPAR(*)" + System.Environment.NewLine;
               richTextBox2.Text += "      DOUBLE PRECISION T, Y(*),YT(*),YPRIME(*), TOUT, RTOL(*), ATOL(*)," + System.Environment.NewLine;
               richTextBox2.Text +="     *  RWORK(*),RPAR(*)";
               richTextBox2.Text += System.Environment.NewLine;
               richTextBox2.Text += "      CALL DDASSL(DRES,NEQ,T,Y,YPRIME,TOUT,INFO,RTOL,ATOL,IDID," + System.Environment.NewLine;
               richTextBox2.Text += "     1  RWORK,LRW,IWORK,LIW,RPAR,IPAR,DJAC)" + System.Environment.NewLine;
               richTextBox2.Text += System.Environment.NewLine;
               richTextBox2.Text += "      CALL DYT(T,YT)" + System.Environment.NewLine;
               richTextBox2.Text += System.Environment.NewLine;
               richTextBox2.Text += "      END" + System.Environment.NewLine;
            }

           richTextBox2.Text += System.Environment.NewLine;
           richTextBox2.Text += System.Environment.NewLine;
           richTextBox2.Text += System.Environment.NewLine;

           richTextBox2.Text += "C----------- Función DRES para definir el del DAE (Differential Algebraic Equations System) -------" + System.Environment.NewLine;
           richTextBox2.Text += "      SUBROUTINE DRES(T,Y,YPRIME,DELTA,IRES,RPAR,IPAR)" + System.Environment.NewLine;
           richTextBox2.Text += "      IMPLICIT DOUBLE PRECISION(A-H,O-Z)" + System.Environment.NewLine;
           richTextBox2.Text += richTextBox1.Text + System.Environment.NewLine;
           richTextBox2.Text += System.Environment.NewLine;
           richTextBox2.Text += "      RETURN" + System.Environment.NewLine;
           richTextBox2.Text += "      END" + System.Environment.NewLine;

           richTextBox2.Text += System.Environment.NewLine;
           richTextBox2.Text += System.Environment.NewLine;
           richTextBox2.Text += System.Environment.NewLine;

            if (checkBox12.Checked == false)
            {
                richTextBox2.Text += "C----------- Función DJAC para definir el JACOBIANO del DAE (Differential Algebraic Equations System)-----" + System.Environment.NewLine;
               richTextBox2.Text += "      SUBROUTINE DJAC(T,Y,YPRIME,PD,CJ,RPAR,IPAR)" + System.Environment.NewLine;
               richTextBox2.Text += "      IMPLICIT DOUBLE PRECISION(A-H,O-Z)" + System.Environment.NewLine;
               richTextBox2.Text += richTextBox3.Text + System.Environment.NewLine;
               richTextBox2.Text += System.Environment.NewLine;
               richTextBox2.Text += "      RETURN" + System.Environment.NewLine;
               richTextBox2.Text += "      END" + System.Environment.NewLine;
            }

            if (checkBox15.Checked == true)
            {
               richTextBox2.Text += System.Environment.NewLine;
               richTextBox2.Text += System.Environment.NewLine;
               richTextBox2.Text += "C----------- Función Analítica del DAE (Differential Algebraic Equations System)-----" + System.Environment.NewLine;
               richTextBox2.Text += "      SUBROUTINE DYT(T,YT)" + System.Environment.NewLine;
               richTextBox2.Text += "      IMPLICIT DOUBLE PRECISION(A-H,O-Z)" + System.Environment.NewLine;
               richTextBox2.Text += richTextBox4.Text + System.Environment.NewLine;
               richTextBox2.Text += System.Environment.NewLine;
               richTextBox2.Text += "      RETURN" + System.Environment.NewLine;
               richTextBox2.Text += "      END" + System.Environment.NewLine;
            }

            tabControl1.SelectedTab = tabPage7;
        }

        //Botón de Next Step del primer paso
        private void button10_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage6;
        }

        //Botón de Next Step del tercer paso
        //Botón para Compilar wrapper.f para obtener wrapper.dll. La compilación es la siguiente:
        //g95 -c wrapper.f
        //para obtener wrapper.o
        //g95 -shared -mrtd -o wrapper.dll daux.o dlinpk.o ddassl.o wrapper.o
        //para obtener wrapper.dll
        //Un tema importante es localizar la ruta del compilador g95 y la ruta de origen y destino de los archivo compilados.
        private void button11_Click(object sender, EventArgs e)
        {
            Directory.SetCurrentDirectory(ruta);

            StreamWriter fl;
            //Captura contenidos del richtextBox1 y richtextBox3 y unirlo al cógido standard del wrapper.f

            //Escribir el archivo final wrapper.f en la ruta requerida para su posterior compilación
            // Create new SaveFileDialog object
            //SaveFileDialog SaveFileDialog = new SaveFileDialog();

            // Default file extension
            //SaveFileDialog.DefaultExt = "f";

            // Available file extensions
            //SaveFileDialog.Filter = "Dassl C# Wrapper files (*.f)|*.f|All files (*.*)|*.*";

            // Adds a extension if the user does not
            //SaveFileDialog.AddExtension = true;

            // Restores the selected directory, next time
            //SaveFileDialog.RestoreDirectory = true;

            // Dialog title
            //SaveFileDialog.Title = "Where do you want to save you C# wrapper.f file?. Programmed by: Luis Coco Enríquez";

            // Startup directory
            //SaveFileDialog.InitialDirectory = @"C:/";
            // Show the dialog and process the result
            //if (SaveFileDialog.ShowDialog() == DialogResult.OK)
            //{
            //SaveFileDialog.FileName=NombreArchivo;
            //MessageBox.Show("You selected the file: " + OpenFileDialog .FileName);
            //    fl = new StreamWriter(SaveFileDialog.FileName);
            //}
            //else
            //{
            //    MessageBox.Show("You hit cancel or closed the dialog.");
            //    SaveFileDialog.Dispose();
            //    SaveFileDialog = null;
            //    return;
            //}

            //SaveFileDialog.Dispose();
            //SaveFileDialog = null;

            string wrappername = "";

            if ((checkBox12.Checked == true) && (checkBox15.Checked == false))
            {
                wrappername = "wrapper.f";
            }

            else if ((checkBox12.Checked == false) && (checkBox15.Checked == false))
            {
                wrappername = "wrapper.f";
            }

            else if ((checkBox12.Checked == false) && (checkBox15.Checked == true))
            {
                wrappername = "wrapper4.f";
            }

            else if ((checkBox12.Checked == true) && (checkBox15.Checked == true))
            {
                wrappername = "wrapper4.f";
            }

            fl = new StreamWriter(ruta + "\\" + wrappername);

            //MessageBox.Show("File wrapper.f created incluiding DAE System and Jacobian matrix user input." + "  Path:" + ruta + "\\" + "wrapper.f");     
                       
            //Este es el Código Standard de nuestro Wrapper de C# para el Algoritmo de DASSL en Fortran           
            fl.Write(richTextBox2.Text);
          
            fl.Close();

            StreamReader fl3;

            if ((checkBox12.Checked == true) && (checkBox15.Checked == false))
            {
                fl3 = new StreamReader(ruta + "\\" + "wrapper.f");
            }

            else if ((checkBox12.Checked == false) && (checkBox15.Checked == false))
            {
                fl3 = new StreamReader(ruta + "\\" + "wrapper.f");
            }

            else if ((checkBox12.Checked == false) && (checkBox15.Checked == true))
            {
                fl3 = new StreamReader(ruta + "\\" + "wrapper4.f");
            }

            else if ((checkBox12.Checked == true) && (checkBox15.Checked == true))
            {
                fl3 = new StreamReader(ruta + "\\" + "wrapper4.f");
            }

            else
            {
                MessageBox.Show("ERROR: It has been written wrappen.f");
                fl3 = new StreamReader(ruta + "\\" + "wrapper.f");
            }

            fl3.Close();      

            Directory.SetCurrentDirectory(ruta);
            //Creamos el archivo compilar.bat que contiene la líneas de compilación del archivo wrapper.f para obtener wrapper.dll
                        
            if ((checkBox12.Checked == true) && (checkBox15.Checked == false))
            {
                StreamWriter fl1;
                fl1 = new StreamWriter(ruta + "\\" + "compile.bat");
                fl1.WriteLine("g95 -c wrapper.f");
                fl1.WriteLine("g95 -shared -mrtd -o wrapper.dll wrapper.o dassl.dll");
                fl1.Close();
            }

            else if ((checkBox12.Checked == false) && (checkBox15.Checked == false))
            {
                StreamWriter fl1;
                fl1 = new StreamWriter(ruta + "\\" + "compile.bat");
                fl1.WriteLine("g95 -c wrapper.f");
                fl1.WriteLine("g95 -shared -mrtd -o wrapper.dll wrapper.o dassl.dll");
                fl1.Close();
            }

            else if ((checkBox12.Checked == false) && (checkBox15.Checked == true))
            {
                StreamWriter fl1;
                fl1 = new StreamWriter(ruta + "\\" + "compile4.bat");
                fl1.WriteLine("g95 -c wrapper4.f");
                fl1.WriteLine("g95 -shared -mrtd -o wrapper4.dll wrapper4.o dassl.dll");
                fl1.Close();
            }

            else if ((checkBox12.Checked == true) && (checkBox15.Checked == true))
            {
                StreamWriter fl1;
                fl1 = new StreamWriter(ruta + "\\" + "compile4.bat");
                fl1.WriteLine("g95 -c wrapper4.f");
                fl1.WriteLine("g95 -shared -mrtd -o wrapper4.dll wrapper4.o dassl.dll");
                fl1.Close();
            }

            else
            {
                StreamWriter fl1;
                fl1 = new StreamWriter(ruta + "\\" + "compile.bat");
                fl1.WriteLine("g95 -c wrapper.f");
                fl1.WriteLine("g95 -shared -mrtd -o wrapper.dll wrapper.o dassl.dll");
                fl1.Close();
            }                      
            
            if ((checkBox12.Checked == true) && (checkBox15.Checked == false))
            {
                Process p = new System.Diagnostics.Process();
                //p.StartInfo.Arguments = "argument1 argument2 argument3";
                p.StartInfo.FileName = "compile.bat";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                p.WaitForExit();
            }

            else if ((checkBox12.Checked == false) && (checkBox15.Checked == false))
            {
                Process p = new System.Diagnostics.Process();
                //p.StartInfo.Arguments = "argument1 argument2 argument3";
                p.StartInfo.FileName = "compile.bat";
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                p.WaitForExit();
            }

            else if ((checkBox12.Checked == false) && (checkBox15.Checked == true))
            {
                Process p = new System.Diagnostics.Process();
                //p.StartInfo.Arguments = "argument1 argument2 argument3";
                p.StartInfo.FileName = "compile4.bat";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                p.WaitForExit();      
            }

            else if ((checkBox12.Checked == true) && (checkBox15.Checked == true))
            {
                Process p = new System.Diagnostics.Process();
                //p.StartInfo.Arguments = "argument1 argument2 argument3";
                p.StartInfo.FileName = "compile4.bat";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                p.WaitForExit();
            }

            else
            {
                Process p = new System.Diagnostics.Process();
                //p.StartInfo.Arguments = "argument1 argument2 argument3";
                p.StartInfo.FileName = "compile.bat";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = false;
                p.Start();
                p.WaitForExit();          
            }                   

            //MessageBox.Show("File wrapper.dll created, containing DASSL Fortran code, DAE System and Jacobian matrix." + "  Path:" + ruta + "\\" + "wrapper.dll");

            tabControl1.SelectedTab = tabPage2;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage4;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage8;
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            if (textBox12.Text == "")
            {

            }
            else
            {
                textBox4.Text = Convert.ToString(Convert.ToDouble(textBox12.Text) + Convert.ToDouble(textBox3.Text));
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {

            }
            else
            {
                textBox4.Text = Convert.ToString(Convert.ToDouble(textBox12.Text) + Convert.ToDouble(textBox3.Text));
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage3;
        }

        private void button4_Click(object sender, EventArgs e)
        { 
            richTextBox1.Text = "";
            richTextBox2.Text = "";
            richTextBox3.Text = "";
            richTextBox4.Text = "";

            this.richTextBox1.Text = "      DIMENSION Y(2),YPRIME(1),DELTA(2)\n      DELTA(1) = YPRIME(1) - Y(1) + 2*Y(2" +
              ")\n      DELTA(2) = YPRIME(2) - 2*Y(1) - Y(2) ";

            this.richTextBox4.Text = "      DIMENSION YT(2)\n      YT(1) = -4*Exp(T)*sin(2*T)\n      YT(2) = 4*Exp(T)*cos" +
             "(2*T)";

            dataGridView1.Rows.Add();
            dataGridView1.Rows[0].Cells[0].Value = 0;
            dataGridView1.Rows.Add();
            dataGridView1.Rows[1].Cells[0].Value = 4;


            dataGridView2.Rows.Add();
            dataGridView2.Rows[0].Cells[0].Value = -8;
            dataGridView2.Rows.Add();
            dataGridView2.Rows[1].Cells[0].Value = 4;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            richTextBox2.Text = "";
            richTextBox3.Text = "";
            richTextBox4.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }


    static class NativeMethods
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);


        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);
    }
}
