using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//using NumericalMethods;
//using NumericalMethods.FourthBlog;

using DotNumerics;
using DotNumerics.LinearAlgebra;

//Para utilizar el puntero a la Aplicación principal
using Drag_AND_Drop_between_Forms;

using IterativeMethodsforLinearSystems;

namespace NumericalMethods
{
    public class NewtonRaphson1
    {
        public double DerivativeStepSize1;

        public Matrix Ax;

        public Matrix A;

        public Matrix B;

        public Matrix C;

        public Matrix D;

        public Matrix E;

        public Matrix F;

        private Matrix X0;

        public Matrix X1;

        public Matrix Xnmas1;

        public Matrix Xn;

        public Matrix Xnmenos1;

        private Matrix jacobianX0;

        private Matrix jacobianinversoXn;

        private Matrix jacobianinversoXnmenos1;

        private Matrix jacobianXn;

        private Matrix jacobianXnmenos1;
        
        private Matrix functionX0;

        private Matrix functionXn;

        private Matrix functionXnmenos1;

        private Matrix incrementofunctionXn;

        private Matrix incrementofunctionXntrapose;

        private Matrix incrementoXn;

        private Matrix incrementoXntraspose;
        
        private Derivatives1 derivatives;

        private List<Parameter> parameters1 = new List<Parameter>();

        Int16 numberOfFunctions;

       // Double precision1;

        List <Double> erroresRelativos=new List<Double>();

        List<Func<Double>> functions2 = new List<Func<Double>>();

        Double EPS1;

        //Número de Iteración
        int b = 0;

        Double[,] matrizauxjacob3 = new Double[2,2];

        //Opción de cálculo para resolver Sistema Ecuaciones No Lineales
        string opcioncalculo3 = "";

        //Opción de cálculo para resolver Sistema Ecuaciones Lineales
        string opcioncalculo4 = "";

        //Puntero a la Aplicación Principal para enviarle los resultados del método de Newton Raphson
        Aplicacion aplicacion3;

        double numeroiteraciones3;

        //Guardar las iteraciones intermedias
        int guardarintermedias3 = 0;

        //Matriz Banda Jacobiana 
        public BandMatrix jacobianaBandaX0;

        //Número de Bandas de la Matriz del Sistema (.matrizauxjacob)
        int numbandasinferiores3 = 0;
        int numbandassuperiores3 = 0;

        public NewtonRaphson1(int numbandasinferiores2, int numbandassuperiores2, int guardarintermedias2, double numeroiteraciones2, Aplicacion aplicacion2, string opcioncalculo1, string opcioncalculo2, Double[,] matrizauxjacob2, List<Parameter> parameters, int numberOfDerivativePoints, List<Func<Double>> functions, Double EPS, double derivativestepsize)
        {
            numbandasinferiores3 = numbandasinferiores2;

            numbandassuperiores3 = numbandassuperiores2;

            guardarintermedias3 = guardarintermedias2;

            numeroiteraciones3 = numeroiteraciones2;
            
            aplicacion3 = aplicacion2;

            opcioncalculo3 = opcioncalculo1;

            opcioncalculo4 = opcioncalculo2;

            matrizauxjacob3=matrizauxjacob2;

            DerivativeStepSize1 = derivativestepsize;

            EPS1 = EPS;

            functions2 = functions;

            //precision1 = precision;
            
            parameters1 = parameters;

            numberOfFunctions = (short)functions2.Count;

            int numberOfParameters = parameters1.Count;

            Debug.Assert(numberOfParameters == numberOfFunctions);

            //Importante al crear un nuevo objeto de la Clase Derivates le enviamos el número de puntos (en este caso 3) para calcular la derivada 
            derivatives = new Derivatives1(numberOfDerivativePoints);

            jacobianX0 = new Matrix(numberOfFunctions, numberOfParameters);
            jacobianXn = new Matrix(numberOfFunctions, numberOfParameters);
            jacobianXnmenos1 = new Matrix(numberOfFunctions, numberOfParameters);
            jacobianaBandaX0 = new BandMatrix(numberOfFunctions, numberOfParameters, numbandasinferiores3+1, numbandassuperiores3+1);

            X0 = new Matrix(numberOfFunctions, 1);
            X1 = new Matrix(numberOfFunctions, 1);
            Xn=new Matrix(numberOfFunctions, 1);
            Xnmas1 = new Matrix(numberOfFunctions, 1);
            Xnmenos1 = new Matrix(numberOfFunctions, 1);

            incrementoXn = new Matrix(numberOfFunctions, 1);
            incrementofunctionXn = new Matrix(numberOfFunctions, 1);
            incrementofunctionXntrapose = new Matrix(1, numberOfParameters);
            incrementoXntraspose = new Matrix(1, numberOfParameters);

            functionX0 = new Matrix(numberOfFunctions, 1);
            functionXn = new Matrix(numberOfFunctions, 1);
            functionXnmenos1 = new Matrix(numberOfFunctions, 1);

            A = new Matrix(numberOfFunctions, 1);
            B = new Matrix(1, 1);
            C = new Matrix(numberOfFunctions, 1);
            D = new Matrix(1, numberOfParameters);
            E = new Matrix(1, numberOfParameters);
            F = new Matrix(numberOfFunctions, numberOfParameters);

            //Siguiente paso enviar los valores inciales de las variables
            //Bucle para asignar los valores inciales alas variables

            //Si hemos elegido la opción CheckBox de Guardar Iteracciones intermedias enviamos a las variables de la Aplicación principal los resultados de las variables locales de esta clase Newton Raphson
            if (guardarintermedias3 == 1)
            {
                if (opcioncalculo3 == "Jacobiano")
                {
                    aplicacion3.jacobianXn = new Double[jacobianXn.RowCount, jacobianXn.ColumnCount, (int)numeroiteraciones3];
                }
                if (opcioncalculo3 == "Jacobiano Inverso")
                {
                    aplicacion3.jacobianinversoXnmenos1 = new Double[jacobianXnmenos1.RowCount, jacobianXnmenos1.ColumnCount, (int)numeroiteraciones3];
                    aplicacion3.jacobianinversoXn = new Double[jacobianXn.RowCount, jacobianXn.ColumnCount, (int)numeroiteraciones3];
                }
                aplicacion3.Xn = new Double[Xn.RowCount, 1, (int)numeroiteraciones3];
                aplicacion3.Xnmenos1 = new Double[Xnmenos1.RowCount, 1, (int)numeroiteraciones3];
                aplicacion3.functionXn = new Double[functionXn.RowCount, 1, (int)numeroiteraciones3];
            }
         }

        // Primero se llama a este Constructor de la Clase NewtonRaphson, que a su vez llama al constructor sobrecargado enviándole además el numberOfDerivativePoints=3
        //El 3 indica el numberOfDerivativePoints por defecto. 
        public NewtonRaphson1(int numbandasinferiores2, int numbandassuperiores2, int guardarintermedias2, double numeroiteraciones2, Aplicacion aplicacion2, string opcioncalculo1, string opcioncalculo2, Double[,] matrizauxjacob2, List<Parameter> parameters, List<Func<Double>> functions, Double EPS, double derivativestepsize)
            : this(numbandasinferiores2,numbandassuperiores2,guardarintermedias2, numeroiteraciones2, aplicacion2, opcioncalculo1, opcioncalculo2, matrizauxjacob2, parameters, 3, functions, EPS, derivativestepsize)
        {
        }
        
        //Función de Iteraciones del Método de Newton-Raphson
        public Double[,] Iterate(int b1)
        {
            int numberOfParameters = parameters1.Count;

            Double[,] errores = new Double[numberOfFunctions, 1];

            b = b1;

            Double[,] errores2 = new Double[numberOfFunctions, 1];


//---------------------------------- Final de la PRIMERA ITERACIÓN -------------------------------------------------------------------------------------------------------------------------------------------------
//Si es la Primera Iteración, es decir, si b=0
            if (b==0)
            {
                //Calculamos F(X0) en base a las Condiciones Iniciales X0
                for (int i = 0; i < numberOfFunctions; i++)
                {
                    functionX0[i, 0] = functions2[i]();
                    X0[i, 0] = parameters1[i];
                }
               
                //Calculamos la matriz Jacobiana J(X0) de las condiciones iniciales X0
                for (int i = 0; i < numberOfFunctions; i++)
                {
                    for (int j = 0; j < numberOfParameters; j++)
                    {

                        if (aplicacion3.ejemplovalidacion == 1)
                        {
                            jacobianX0[i, j] = derivatives.ComputePartialDerivative(functions2[i], parameters1[j], 1, functionX0[i, 0], parameters1, DerivativeStepSize1);
                        }

                        else if (aplicacion3.ejemplovalidacion !=1)
                        {

                            //No cálculamos todos los elementos de la matriz Jacobiana sino sólo los elementos de la matriz Jacobiana diferente de Cero.
                            if (matrizauxjacob3[i, j] == 0)
                            {
                                if (opcioncalculo4 == "BANDA")
                                {
                                    jacobianaBandaX0[i, j] = 0;
                                }

                                else
                                {
                                    jacobianX0[i, j] = 0;
                                }
                            }

                            else if (matrizauxjacob3[i, j] != 0)
                            {
                                //El 1 en los argumentos de la función ComputePartialDerivative indica que las derivadas a calcular son de primer orden.
                                //Envimos como argumentos las funciones _functions y las variables de las ecuaciones _parameters                            
                                if (opcioncalculo4 == "BANDA")
                                {
                                    jacobianaBandaX0[i, j] = derivatives.ComputePartialDerivative(functions2[i], parameters1[j], 1, functionX0[i, 0], parameters1, DerivativeStepSize1);
                                }
                                else
                                {
                                    jacobianX0[i, j] = derivatives.ComputePartialDerivative(functions2[i], parameters1[j], 1, functionX0[i, 0], parameters1, DerivativeStepSize1);
                                }
                            }
                        }                      
                    }
                }

                //Resolvemos el Sistema de Ecuaciones Lineales J(X0) * AX = F(X0) para hallar AX = Xn+1 - Xn. 
                //El Sistema de Ecuaciones Lineales es del tipo A * X = B, donde A = J(X0); X = AX y B = F(X0)
                //Utilizamos la librería DOTNUMERICS, traducción a C# de la librería LAPACK

                //Resolución del Sistema de Ecuaciones Lineales mediante la Descomposición LU de PA
                //Función utilizada de la librería LAPACK: DGESV
                //Posible mejora utilizar DGESVX
                if (opcioncalculo4 == "LU")
                {
                    LinearEquations leq = new LinearEquations();
                    Ax = leq.Solve(jacobianX0, functionX0);  
                }

                //Resolución del Sistema de Ecuaciones Lineales mediante la Descomposición QR de A 
                //Función utilizada de la librería LAPACK: DGELS
                else if (opcioncalculo4 == "QR")
                {
                    LinearLeastSquares leastSquares = new LinearLeastSquares();
                    Ax = leastSquares.QRorLQSolve(jacobianX0, functionX0);  
                }

                //Resolución del Sistema de Ecuaciones Lineales mediante "Complete Orthogonal Factorization"
                //Función utilizada de la librería LAPACK: DLAMCH
                else if (opcioncalculo4 == "Orthogonal")
                {
                    LinearLeastSquares leastSquares = new LinearLeastSquares();
                    Ax = leastSquares.COFSolve(jacobianX0, functionX0);  
                }

                //Resolución del Sistema de Ecuaciones Lineales mediante la descomposción "Singular Value Descomposition (SVD)"
                //Función utilizada de la librería LAPACK: DGELSD
                else if (opcioncalculo4 == "SVD")
                {
                    LinearLeastSquares leastSquares = new LinearLeastSquares();
                    Ax = leastSquares.SVDdcSolve(jacobianX0, functionX0);  
                }

                //Resolución del Sistema de Ecuaciones Lineales teniendo en cuenta que la 
                //Función utilizada de la librería LAPACK: DGBSV
                else if (opcioncalculo4 == "BANDA")
                {
                    LinearEquations leq = new LinearEquations();
                    Ax = leq.Solve(jacobianaBandaX0, functionX0);
                }

                //Resolución del Sistema de Ecuaciones Lineales mediante el método de JACOBI
                else if (opcioncalculo4 == "JACOBI")
                {
                    ClassJacobi leq = new ClassJacobi();
                    Ax = leq.Jacobi(X0,jacobianX0, numberOfFunctions, numberOfParameters, functionX0, numberOfFunctions, 1,100);
                   
                    if (Ax == X0)
                    {
                        LinearEquations leq1 = new LinearEquations();
                        Ax = leq1.Solve(jacobianX0, functionX0);  
                    }
                }

                //Resolución del Sistema de Ecuaciones Lineales mediante el método de GAUSS-SEIDEL
                else if (opcioncalculo4 == "GAUSS-SEIDEL")
                {
                    MessageBox.Show("Error no se ha elegido una opción para resolución del Sistema de Ecuaciones Lineales. Por defecto hemos elegido el método LU.");
                    LinearEquations leq = new LinearEquations();
                    Ax = leq.Solve(jacobianX0, functionX0);
                }

                //Resolución del Sistema de Ecuaciones Lineales mediante el método de SOR
                else if (opcioncalculo4 == "SOR")
                {
                    MessageBox.Show("Error no se ha elegido una opción para resolución del Sistema de Ecuaciones Lineales. Por defecto hemos elegido el método LU.");
                    LinearEquations leq = new LinearEquations();
                    Ax = leq.Solve(jacobianX0, functionX0);
                }

                //Resolución del Sistema de Ecuaciones Lineales mediante el método de CONJUGATED GRADIENT
                else if (opcioncalculo4 == "CONJUGATED GRADIENT")
                {
                    MessageBox.Show("Error no se ha elegido una opción para resolución del Sistema de Ecuaciones Lineales. Por defecto hemos elegido el método LU.");
                    LinearEquations leq = new LinearEquations();
                    Ax = leq.Solve(jacobianX0, functionX0);
                }

                else
                {
                    MessageBox.Show("Error no se ha elegido una opción para resolución del Sistema de Ecuaciones Lineales. Por defecto hemos elegido el método LU.");
                    LinearEquations leq = new LinearEquations();
                    Ax = leq.Solve(jacobianX0, functionX0);
                }

                //Calculamos Xn+1 en base a X0 (condiciones iniciales)
                X1 = X0 - Ax; 
                //Asignamos a Xn-1 = X0
                Xnmenos1 = X0;
                //Asignamos a Xn = X1
                Xn = X1;
                //Asignamos a Xn+1 = X1
                Xnmas1 = X1;

                //Elegimos la opción de cálculo del Método de BROYDEN para cálculo del Jacobiano mediante Diferencias Finitas
                if (opcioncalculo3 == "Jacobiano")
                {
                    if (opcioncalculo4 == "BANDA")
                    {
                        jacobianXnmenos1 = jacobianaBandaX0;
                    }

                    else
                    {
                        jacobianXnmenos1 = jacobianX0;
                    }                    
                }

                else if (opcioncalculo3 == "Jacobiano Inverso")
                {
                    if (opcioncalculo4 == "BANDA")
                    {
                        jacobianinversoXnmenos1 = jacobianaBandaX0.Inverse();
                    }

                    else
                    {
                        jacobianinversoXnmenos1 = jacobianX0.Inverse();

                    }
                 }

                functionXnmenos1 = functionX0;
            }
//---------------------------------- Final de la PRIMERA ITERACIÓN -------------------------------------------------------------------------------------------------------------------------------------------------






//--------------------------------- ITERACIONES siguientes a la PRIMERA --------------------------------------------------------------------------------------------------------------------------------------------------
            else if (b>0)
            {
                for (int i = 0; i < numberOfFunctions; i++)
                {
                    functionXn[i, 0] = functions2[i]();
                    incrementofunctionXn[i, 0] = functionXn[i, 0] - functionXnmenos1[i, 0];
                    incrementoXn[i, 0] = Xn[i, 0] - Xnmenos1[i, 0];
                }

                incrementofunctionXntrapose = incrementofunctionXn.Transpose();
                incrementoXntraspose = incrementoXn.Transpose();

                if (opcioncalculo3 == "Jacobiano")
                {
                    //  nxn * nx1 = nx1
                    A = jacobianXnmenos1 * incrementoXn;
                    //  (1xn  *  nxn) = 1xn; (1xn) *  (nx1) = (1x1)
                    B = incrementofunctionXn - A;
                    //  (nx1) - (nx1)  = nx1
                    C = B * incrementoXntraspose;
                    // (1x1) * (nx1) = (nx1)             
                    D = incrementoXntraspose * incrementoXn;
                    //  (1xn) * (nxn)  = 1xn
                    E = C *(1/ D[0, 0]);
                    jacobianXn = jacobianXnmenos1 + E;

                    if (opcioncalculo4 == "LU")
                    {
                        LinearEquations leq = new LinearEquations();
                        Ax = leq.Solve(jacobianXn, functionXn);
                    }

                      //Resolución del Sistema de Ecuaciones Lineales mediante la Descomposición QR de A 
                    //Función utilizada de la librería LAPACK: DGELS
                    else if (opcioncalculo4 == "QR")
                    {
                        LinearLeastSquares leastSquares = new LinearLeastSquares();
                        Ax = leastSquares.QRorLQSolve(jacobianXn, functionXn);
                    }

                    //Resolución del Sistema de Ecuaciones Lineales mediante "Complete Orthogonal Factorization"
                    //Función utilizada de la librería LAPACK: DLAMCH
                    else if (opcioncalculo4 == "Orthogonal")
                    {
                        LinearLeastSquares leastSquares = new LinearLeastSquares();
                        Ax = leastSquares.COFSolve(jacobianXn, functionXn);
                    }

                    //Resolución del Sistema de Ecuaciones Lineales mediante la descomposción "Singular Value Descomposition (SVD)"
                    //Función utilizada de la librería LAPACK: DGELSD
                    else if (opcioncalculo4 == "SVD")
                    {
                        LinearLeastSquares leastSquares = new LinearLeastSquares();
                        Ax = leastSquares.SVDdcSolve(jacobianXn, functionXn);
                    }

                    //Resolución del Sistema de Ecuaciones Lineales teniendo en cuenta que la 
                    //Función utilizada de la librería LAPACK: DGBSV
                    else if (opcioncalculo4 == "BANDA")
                    {
                        LinearEquations leq = new LinearEquations();
                        Ax = leq.Solve(jacobianXn, functionXn);
                    }

                    //Resolución del Sistema de Ecuaciones Lineales mediante el método de JACOBI
                    else if (opcioncalculo4 == "JACOBI")
                    {
                        MessageBox.Show("Error no se ha elegido una opción para resolución del Sistema de Ecuaciones Lineales. Por defecto hemos elegido el método LU.");
                        LinearEquations leq = new LinearEquations();
                        Ax = leq.Solve(jacobianXn, functionXn);
                    }

                    //Resolución del Sistema de Ecuaciones Lineales mediante el método de GAUSS-SEIDEL
                    else if (opcioncalculo4 == "GAUSS-SEIDEL")
                    {
                        MessageBox.Show("Error no se ha elegido una opción para resolución del Sistema de Ecuaciones Lineales. Por defecto hemos elegido el método LU.");
                        LinearEquations leq = new LinearEquations();
                        Ax = leq.Solve(jacobianXn, functionXn);
                    }

                    //Resolución del Sistema de Ecuaciones Lineales mediante el método de SOR
                    else if (opcioncalculo4 == "SOR")
                    {
                        MessageBox.Show("Error no se ha elegido una opción para resolución del Sistema de Ecuaciones Lineales. Por defecto hemos elegido el método LU.");
                        LinearEquations leq = new LinearEquations();
                        Ax = leq.Solve(jacobianXn, functionXn);
                    }

                    //Resolución del Sistema de Ecuaciones Lineales mediante el método de CONJUGATED GRADIENT
                    else if (opcioncalculo4 == "CONJUGATED GRADIENT")
                    {
                        MessageBox.Show("Error no se ha elegido una opción para resolución del Sistema de Ecuaciones Lineales. Por defecto hemos elegido el método LU.");
                        LinearEquations leq = new LinearEquations();
                        Ax = leq.Solve(jacobianXn, functionXn);
                    }

                    else 
                    {
                        MessageBox.Show("Error no se ha elegido una opción para resolución del Sistema de Ecuaciones Lineales. Por defecto hemos elegido el método LU.");
                        LinearEquations leq = new LinearEquations();
                        Ax = leq.Solve(jacobianXn, functionXn);
                    }
                }

                else if (opcioncalculo3 == "Jacobiano Inverso")
                {
                    //  nxn * nx1 = nx1
                    A = jacobianinversoXnmenos1 * incrementofunctionXn;
                    //  (1xn  *  nxn) = 1xn; (1xn) *  (nx1) = (1x1)
                    B = incrementoXntraspose * jacobianinversoXnmenos1 * incrementofunctionXn;
                    //  (nx1) - (nx1)  = nx1
                    C = incrementoXn - A;
                    //  (1xn) * (nxn)  = 1xn
                    D = incrementoXntraspose * jacobianinversoXnmenos1;
                    // (1x1) * (nx1) = (nx1)             
                    E = (1 / B[0, 0]) * C;
                    // (nx1) * (1xn) = (nxn)
                    F = E * D;
                    //
                    jacobianinversoXn = jacobianinversoXnmenos1 + F;
                    Ax = jacobianinversoXn * functionXn;
                }

                // (nx1) = (nx1) - (nxn) * (nx1)
                Xnmas1 = Xn - Ax;
            }
//--------------------------------- Fin de ITERACIONES siguientes a la PRIMERA --------------------------------------------------------------------------------------------------------------------------------------------------

 



//---------------------------------------  Algoritmo de la Faja (incremento de X después de las sucesivas iteracines, también denominado el EPS) -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            for (int k = 0; k < numberOfFunctions; k++)
            {
                //Calculamos los errores relativos error[i]=Axi/xi
                errores[k, 0] = (-Ax[k, 0]) / Xn[k, 0];

                if (errores[k, 0] <= 0.1)
                {
                    parameters1[k].Value = Xnmas1[k, 0];
                }

                else if ((0.1 < errores[k, 0]) || (errores[k, 0] <= 1))
                {
                    Ax[k, 0] = Ax[k, 0] * EPS1 / errores[k, 0];
                    Xnmas1[k, 0] = Xn[k, 0] - Ax[k, 0];
                    parameters1[k].Value = Xnmas1[k, 0];
                }

                else if (errores[k, 0] > 1)
                {
                    Double F22;
                    F22 = EPS1 - (((1 - EPS1) / 0.9) * (errores[k, 0] - 1));
                    Ax[k, 0] = Ax[k, 0] * F22;
                    Xnmas1[k, 0] = Xn[k, 0] - Ax[k, 0];
                    parameters1[k].Value = Xnmas1[k, 0];
                }
            }
//---------------------------------------  Fin del Algoritmo de la Faja (EPS) -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------





//--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            if (b == 0)
            { 
            
            }
            else if (b!= 0)
            {
                for (int i = 0; i < numberOfFunctions; i++)
                {
                    Xnmenos1[i, 0] = Xn[i, 0];
                    functionXnmenos1[i, 0] = functionXn[i, 0];
                    Xn[i, 0] = Xnmas1[i, 0];

                    if (opcioncalculo3 == "Jacobiano")
                    {
                        jacobianXnmenos1 = jacobianXn;

                        if (guardarintermedias3 == 1)
                        {
                            for (int filas = 0; filas < jacobianXn.RowCount; filas++)
                            {
                                for (int col = 0; col < jacobianXn.ColumnCount; col++)
                                {
                                    aplicacion3.jacobianXn[filas, col, b] = jacobianXn[filas, 0];
                                    aplicacion3.Xn[filas, 0, b] = Xn[filas, col];
                                    aplicacion3.Xnmenos1[filas,0,b]=Xnmenos1[filas,0];
                                    aplicacion3.functionXn[filas, 0, b] = functionXn[filas, 0];
                                }
                            }
                        }
                    }

                    else if (opcioncalculo3 == "Jacobiano Inverso")
                    {
                        jacobianinversoXnmenos1 = jacobianinversoXn;

                        if (guardarintermedias3 == 1)
                        {
                            for (int filas = 0; filas < jacobianXn.RowCount; filas++)
                            {
                                for (int col = 0; col < jacobianXn.ColumnCount; col++)
                                {
                                    aplicacion3.jacobianinversoXnmenos1[filas, col, b] = jacobianinversoXnmenos1[filas, col];
                                    aplicacion3.jacobianinversoXn[filas, col, b] = jacobianinversoXn[filas, col];
                                    aplicacion3.Xn[filas, 0, b] = Xn[filas, 0];
                                    aplicacion3.Xnmenos1[filas, 0, b] = Xnmenos1[filas, 0];
                                    aplicacion3.functionXn[filas, 0, b] = functionXn[filas, 0];
                                }
                            }
                        }
                    }
                }
            }

            return errores;
        }
    }
}
