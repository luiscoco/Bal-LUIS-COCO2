﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using NumericalMethods;
using NumericalMethods.FourthBlog;

using System.Collections;


namespace NumericalMethods
{
    public class Derivatives1
    {
        // _coefficients is the array of differential coefficients matrices.
        // The index corresponds to the position from the left edge
        // of the points.
        // I.e _coefficients[0] is for a matrix with three points in it corresponds to
        // the left most point.
        // The coefficients of the derivatives go down by row.  I.e. the first row
        // is the functional value, the second row is for the first derivative of the functional
        // value, the third row is the second derivative of the functional value.
        // The columns correspond to the points themselves.
        private Matrix[] _coefficients;


        private Derivatives1()
        {
        }

        public Derivatives1(int numberOfPoints)
            : this()
        {
            SolveCoefs(numberOfPoints);
        }

        public void SolveCoefs(int numberOfPoints)
        {
            _coefficients = new Matrix[numberOfPoints];
            for (int i = 0; i < numberOfPoints; i++)
            {
                Matrix deltsMatrix = new Matrix(numberOfPoints, numberOfPoints);
                for (int j = 0; j < numberOfPoints; j++)
                {
                    double delt = (double)(j - i);
                    double HTerm = 1.0;
                    for (int k = 0; k < numberOfPoints; k++)
                    {
                        deltsMatrix[j, k] = HTerm / Factorial(k);
                        HTerm *= delt;
                    }
                }
                _coefficients[i] = deltsMatrix.Invert();
                double numPointsFactorial = Factorial(numberOfPoints);
                for (int j = 0; j < numberOfPoints; j++)
                {
                    for (int k = 0; k < numberOfPoints; k++)
                    {
                        _coefficients[i][j, k] = (Math.Round(_coefficients[i][j, k] * numPointsFactorial)) / numPointsFactorial;
                    }
                }
            }
        }

        private static double Factorial(int value)
        {
            double result = 1.0;
            for (int i = 1; i <= value; i++)
            {
                result *= (double)i;
            }
            return result;
        }

        /// <summary>
        /// Computes the derivative of a function.
        /// </summary>
        /// <param name="points">Equally spaced function value points</param>
        /// <param name="order">The order of the derivative to take</param>
        /// <param name="variablePosition">The position in the array of function values to take the derivative at.</param>
        /// <param name="step">The x axis step size.</param>
        /// <returns></returns>
        
        public double ComputeDerivative(double[] points, int order, int variablePosition, double step)
        {
            Debug.Assert(points.Length == _coefficients.Length);
            Debug.Assert(order < _coefficients.Length);
            double result = 0.0;
            for (int i = 0; i < _coefficients.Length; i++)
            {
                result += _coefficients[variablePosition][order, i] * points[i];
            }
            result /= Math.Pow(step, order);
            return result;
        }

       

        public double ComputePartialDerivative(Func<Double> function,Parameter parameter, int order, double currentFunctionValue, List<Parameter> _parameters, double DerivativeStepSize)
        {                                           
            //Número de puntos considerados para cálcular los intervalos de derivación alrededor de xo
            //El número de puntos debe ser un número primo superior o igual a 3. Ejemplo:3,5,7 etc.
            int numberOfPoints = _coefficients.Length;

            double result = 0.0;
            double originalValue = parameter;

            //double[] points = new double[numberOfPoints];
            double[] points = new double[numberOfPoints];          

            //Tamaño del intervalo para calcular la derivada en un punto xo es decir calculo de la f'(xo)
            double derivativeStepSize = DerivativeStepSize;
            
            //Calculo de punto central donde está localizado el f(xo)
            //Para el caso de considerar 3 puntos caso por defecto el punto central es el 1 (considerando los Puntos:0,1,2)
            int centerPoint = (numberOfPoints - 1) / 2;
            
            for (int i = 0; i < numberOfPoints; i++)
            {
                if (i != centerPoint)
                {
                    parameter.Value = originalValue + ((double)(i - centerPoint)) * derivativeStepSize;
                   points[i] = function();
                }
                else
                {
                    
                    points[i] = currentFunctionValue;
                }
            }
            result = ComputeDerivative(points, order, centerPoint, derivativeStepSize);
            parameter.Value = originalValue;
            return result;
        }





        //ESTA FUNCIÓN NO SE USA
        
        public double ComputePartialDerivative(Func<double> function, Parameter parameter, int order)
        {
            int numberOfPoints = _coefficients.Length;
            double result = 0.0;
            double originalValue = parameter;
            double[] points = new double[numberOfPoints];
            double derivativeStepSize = parameter.DerivativeStepSize;
            int centerPoint = (numberOfPoints - 1) / 2;

            for (int i = 0; i < numberOfPoints; i++)
            {
                parameter.Value = originalValue + ((double)(i - centerPoint)) * derivativeStepSize;
                points[i] = function();
            }
            result = ComputeDerivative(points, order, centerPoint, derivativeStepSize);
            parameter.Value = originalValue;
            return result;
        }


        //ESTA FUNCIÓN NO SE USA

        public double[] ComputePartialDerivatives(Func<double> function, Parameter parameter, int[] derivativeOrders)
        {
            int numberOfPoints = _coefficients.Length;
            double[] result = new double[derivativeOrders.Length];
            double originalValue = parameter;
            double[] points = new double[numberOfPoints];
            double derivativeStepSize = parameter.DerivativeStepSize;
            int centerPoint = (numberOfPoints - 1) / 2;

            for (int i = 0; i < numberOfPoints; i++)
            {
                parameter.Value = originalValue + ((double)(i - centerPoint)) * derivativeStepSize;
                points[i] = function();
            }
            for (int i = 0; i < derivativeOrders.Length; i++)
            {
                result[i] = ComputeDerivative(points, derivativeOrders[i], centerPoint, derivativeStepSize);
            }
            parameter.Value = originalValue;
            return result;
        }


        //ESTA FUNCIÓN NO SE USA

        public double[] ComputePartialDerivatives(Func<double> function, Parameter parameter, int[] derivativeOrders, double currentFunctionValue)
        {
            int numberOfPoints = _coefficients.Length;
            double[] result = new double[derivativeOrders.Length];
            double originalValue = parameter;
            double[] points = new double[numberOfPoints];
            double derivativeStepSize = parameter.DerivativeStepSize;
            int centerPoint = (numberOfPoints - 1) / 2;

            for (int i = 0; i < numberOfPoints; i++)
            {
                if (i != centerPoint)
                {
                    parameter.Value = originalValue + ((double)(i - centerPoint)) * derivativeStepSize;
                    points[i] = function();
                }
                else
                {
                    points[i] = currentFunctionValue;
                }
            }
            for (int i = 0; i < derivativeOrders.Length; i++)
            {
                result[i] = ComputeDerivative(points, derivativeOrders[i], centerPoint, derivativeStepSize);
            }
            parameter.Value = originalValue;
            return result;
        }

    }
}
