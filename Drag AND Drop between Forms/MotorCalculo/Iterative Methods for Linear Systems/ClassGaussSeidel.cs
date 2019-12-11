using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using DotNumerics.LinearAlgebra;

namespace IterativeMethodsforLinearSystems
{

    //JACOBI Method for Linear Equation System Solving
    public class ClassGaussSeidel
    {        
        int filasa = 0;
        int columnasa = 0;
        Matrix a;

        int filasb=0;
        Matrix b;


        public Matrix GaussSeidel(Matrix x0,Matrix a1,int numberOfFunctions1, int numberOfParameter1,Matrix b1,int numberOfFunctions2,int columnasb2,int numiteraciones)
        {
            filasa = numberOfFunctions1;
            columnasa = numberOfParameter1;

            a = new Matrix(filasa,columnasa);

            a = a1;

            filasb = numberOfFunctions2;
            columnasb2 = 1;

            b = new Matrix(filasb,columnasb2);

            b = b1;

            Matrix L = new Matrix(filasa, columnasa);
            Matrix R = new Matrix(filasa, columnasa);
            Matrix U = new Matrix(filasa, columnasa);
            Matrix D = new Matrix(filasa, columnasa);

            Matrix C = new Matrix(filasa, columnasa);
            Matrix T = new Matrix(filasa, columnasa);

            Matrix x = new Matrix(filasa, 1);

            x = x0;

            //Comprobamos si la matriz a es diagonal dominante
            int prueba = matrixdiagonaldominante(a, filasa);
            
            if (prueba == 1)
            {
                MessageBox.Show("Matriz SI Diagonal Dominante");            
            }
            else if (prueba == 0)
            {
                MessageBox.Show("Matriz NO Diagonal Dominante. No se debe utilizar el Método de Jacobi. Utilizamos por defecto el método LU.");
                return x;
            }

         
            
            // Creamos la Matrix Diagonal D            
            for (int row = 0; row < a.RowCount; row++)
            {
                for (int colum = 0; colum < a.ColumnCount; colum++)
                {
                    if (row == colum)
                    {
                        D[row, colum] = a[row, colum];
                    }
                }
            }

            //Declaramos la Matriz R (que contiene todos los elementos de a excepto su diagonal principal) y la inicializamos

            for (int row = 0; row < a.RowCount; row++)
            {
                for (int colum = 0; colum < a.ColumnCount; colum++)
                {
                    if (row != colum)
                    {
                        R[row, colum] = a[row, colum];
                    }
                }
            }

            //Declaramos las Matrices L (lower triangural matrix)  

            for (int row = 0; row < R.RowCount; row++)
            {
                for (int colum = 0; colum < R.ColumnCount; colum++)
                {
                    if (row > colum)
                    {
                        L[row, colum] = R[row, colum];
                    }
                }
            }

            for (int row = 0; row < R.RowCount; row++)
            {
                for (int colum = 0; colum < R.ColumnCount; colum++)
                {
                    if (row < colum)
                    {
                        U[row, colum] = R[row, colum];
                    }
                }
            }
            
             //Obtain  T matrix : T= -(L^-1)*U
            T = (-1*L.Inverse()) * U;

            //Obtain  C matrix : C=(L^-1)*b
            C = (L.Inverse()) * b;

            for (int j = 0; j < numiteraciones; j++)    
            {
                 //Obtain first iteration solution
                 x = (T * x) + C;
            }        
        
            return x;
        }
        


        public int matrixdiagonaldominante(Matrix a1,int filas)
        {        
            double izq = 0;
            double derecho = 0;
            double[] suma = new double[filas];

            //Resultado de Matriz Estrictamente Diagonal Dominante (si=1; no=0)
            int resultado = 0;

            for (int row = 0; row < a1.RowCount; row++)
            {
                for (int colum = 0; colum < a1.ColumnCount; colum++)
                {
                    if (colum < row)
                    {
                        izq = Math.Abs(a1[row, colum]) + izq;
                    }

                    else if (colum > row)
                    {
                        derecho = Math.Abs(a1[row, colum]) + derecho;
                    }
                }

                suma[row] = izq + derecho;
                izq = 0;
                derecho = 0;
            }

            for (int row = 0; row < a1.RowCount; row++)
            {
                for (int colum = 0; colum < a1.ColumnCount; colum++)
                {
                    if (row == colum)
                    {
                        if (a1[row, colum] > suma[row])
                        {
                            resultado = 1;
                            //textBox4.Text = "Si Diagonal Dominante";
                            return resultado;
                        }

                        else
                        {
                            resultado = 0;
                            //MessageBox.Show("Matriz No Diagonal Dominante");
                            //textBox4.Text = "No Diagonal Dominante";
                            return resultado;
                        }
                    }
                }                
            }

            return 0;
        }
    }
}
