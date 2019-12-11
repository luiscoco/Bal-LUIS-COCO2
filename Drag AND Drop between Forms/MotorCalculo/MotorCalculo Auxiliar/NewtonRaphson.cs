using System;
using System.Diagnostics;
using NumericalMethods.FourthBlog;

namespace NumericalMethods
{
    public class NewtonRaphson
    {
        private Matrix _jacobian;
        private Matrix _functionMatrix;
        private Matrix _x0;
        private Derivatives _derivatives;
        private Parameter[] _parameters;
        private Func<double>[] _functions;

        public NewtonRaphson(Parameter[] parameters, Func<double>[] functions, int numberOfDerivativePoints)
        {
            _parameters = parameters;
            _functions = functions;
            int numberOfFunctions = _functions.Length;
            int numberOfParameters = _parameters.Length;

            Debug.Assert(numberOfParameters == numberOfFunctions);

            _derivatives = new Derivatives(numberOfDerivativePoints);

            _jacobian = new Matrix(numberOfFunctions, numberOfParameters);
            _functionMatrix = new Matrix(numberOfFunctions, 1);
            _x0 = new Matrix(numberOfFunctions, 1);
        }

        public NewtonRaphson(Parameter[] parameters, Func<double>[] functions)
            : this(parameters, functions, 3)
        {
        }

        public void Iterate()
        {
            int numberOfFunctions = _functions.Length;
            int numberOfParameters = _parameters.Length;
            for (int i = 0; i < numberOfFunctions; i++)
            {
                _functionMatrix[i, 0] = _functions[i]();
                _x0[i, 0] = _parameters[i];
            }

            for (int i = 0; i < numberOfFunctions; i++)
            {
                for (int j = 0; j < numberOfParameters; j++)
                {
                    _jacobian[i, j] = _derivatives.ComputePartialDerivative(_functions[i], _parameters[j], 1, _functionMatrix[i, 0]);
                }
            }

            Matrix newXs = _x0 - _jacobian.SolveFor(_functionMatrix);

            for (int i = 0; i < numberOfFunctions; i++)
            {
                _parameters[i].Value = newXs[i, 0];
            }
        }
    }
}
