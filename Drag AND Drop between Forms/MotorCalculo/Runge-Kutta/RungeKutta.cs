//Created by Trent F Guidry
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace NumericalMethods
{
    public class RungeKuttaBase
    {
        public RungeKuttaBase()
        {
        }

        public virtual double[][] Integrate(Parameter[] rungeKuttaParameters, Parameter x, Func<double>[] rungeKuttaFunctions, double xEnd, double step)
        {
            throw new NotImplementedException();
        }
    }

    public class Euler : RungeKuttaBase
    {
        public Euler()
            : base()
        {
        }

        public override double[][] Integrate(Parameter[] rungeKuttaParameters, Parameter x, Func<double>[] rungeKuttaFunctions, double xEnd, double step)
        {
            double xStart = x;
            double stepSize = (xEnd - xStart) / step;
            int integerStepSize = (int)stepSize;
            if (stepSize > (double)integerStepSize) { integerStepSize++; }
            integerStepSize++;
            Collection<double[]> returnCollection = new Collection<double[]>();
            double[] row = new double[rungeKuttaParameters.Length + 1];
            row[0] = xStart;
            for (int i = 0; i < rungeKuttaParameters.Length; i++)
            {
                row[i + 1] = rungeKuttaParameters[i];
            }
            returnCollection.Add(row);
            double currentX = x;
            double[] dCurrentFs = new double[rungeKuttaParameters.Length];
            double currentStep = step;
            while (currentX < xEnd)
            {
                if (currentX + currentStep > xEnd) { currentStep = xEnd - currentX; }
                x.Value = currentX;
                for (int j = 0; j < rungeKuttaParameters.Length; j++)
                {
                    dCurrentFs[j] = rungeKuttaFunctions[j]();
                }
                row = new double[rungeKuttaParameters.Length + 1];
                for (int j = 0; j < rungeKuttaParameters.Length; j++)
                {
                    rungeKuttaParameters[j].Value = rungeKuttaParameters[j] + dCurrentFs[j] * currentStep;
                    row[j + 1] = rungeKuttaParameters[j];
                }
                currentX += currentStep;
                row[0] = currentX;
                returnCollection.Add(row);
            }
            return returnCollection.ToArray();
        }
    }

    public class RungeKutta2Heun : RungeKuttaBase
    {
        public RungeKutta2Heun()
            : base()
        {
        }

        public override double[][] Integrate(Parameter[] rungeKuttaParameters, Parameter x, Func<double>[] rungeKuttaFunctions, double xEnd, double step)
        {
            Debug.Assert(rungeKuttaParameters.Length == rungeKuttaFunctions.Length);
            double xStart = x;
            double stepSize = (xEnd - xStart) / step;
            int integerStepSize = (int)stepSize;
            if (stepSize > (double)integerStepSize) { integerStepSize++; }
            integerStepSize++;
            Collection<double[]> returnCollection = new Collection<double[]>();
            double[] row = new double[rungeKuttaParameters.Length + 1];
            row[0] = x;
            for (int i = 0; i < rungeKuttaParameters.Length; i++)
            {
                row[i + 1] = rungeKuttaParameters[i];
            }
            returnCollection.Add(row);

            double currentX = xStart;
            double[,] ks = new double[2, rungeKuttaParameters.Length];
            double x0 = xStart;
            double[] y0 = new double[rungeKuttaParameters.Length];
            double currentStep = step;
            while (currentX < xEnd)
            {
                if (currentX + currentStep > xEnd) { currentStep = xEnd - currentX; }
                x0 = currentX;
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    y0[i] = rungeKuttaParameters[i];
                }

                //k0s
                x.Value = x0;
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    ks[0, i] = rungeKuttaFunctions[i]();
                }

                //k1s
                x.Value = x0 + currentStep;
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    rungeKuttaParameters[i].Value = y0[i] + ks[0, i] * currentStep;
                }
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    ks[1, i] = rungeKuttaFunctions[i]();
                }

                //Final
                currentX += currentStep;
                row = new double[rungeKuttaParameters.Length + 1];
                row[0] = currentX;
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    rungeKuttaParameters[i].Value = y0[i] + (ks[0, i] / 2.0 + ks[1, i] / 2.0) * currentStep;
                    row[i + 1] = rungeKuttaParameters[i];
                }
                returnCollection.Add(row);
            }
            return returnCollection.ToArray();
        }
    }

    public class RungeKutta2ImprovedPolygon : RungeKuttaBase
    {
        public RungeKutta2ImprovedPolygon()
            : base()
        {
        }

        public override double[][] Integrate(Parameter[] rungeKuttaParameters, Parameter x, Func<double>[] rungeKuttaFunctions, double xEnd, double step)
        {
            Debug.Assert(rungeKuttaParameters.Length == rungeKuttaFunctions.Length);
            double xStart = x;
            double stepSize = (xEnd - xStart) / step;
            int integerStepSize = (int)stepSize;
            if (stepSize > (double)integerStepSize) { integerStepSize++; }
            integerStepSize++;
            Collection<double[]> returnCollection = new Collection<double[]>();
            double[] row = new double[rungeKuttaParameters.Length + 1];
            row[0] = x;
            for (int i = 0; i < rungeKuttaParameters.Length; i++)
            {
                row[i + 1] = rungeKuttaParameters[i];
            }
            returnCollection.Add(row);

            double currentX = xStart;
            double[,] ks = new double[2, rungeKuttaParameters.Length];
            double x0 = xStart;
            double[] y0 = new double[rungeKuttaParameters.Length];
            double currentStep = step;
            while (currentX < xEnd)
            {
                if (currentX + currentStep > xEnd) { currentStep = xEnd - currentX; }
                x0 = currentX;
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    y0[i] = rungeKuttaParameters[i];
                }

                //k0s
                x.Value = x0;
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    ks[0, i] = rungeKuttaFunctions[i]();
                }

                //k1s
                x.Value = x0 + currentStep / 2.0;
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    rungeKuttaParameters[i].Value = y0[i] + ks[0, i] * currentStep / 2.0;
                }
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    ks[1, i] = rungeKuttaFunctions[i]();
                }

                //Final
                currentX += currentStep;
                row = new double[rungeKuttaParameters.Length + 1];
                row[0] = currentX;
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    rungeKuttaParameters[i].Value = y0[i] + ks[1, i] * currentStep;
                    row[i + 1] = rungeKuttaParameters[i];
                }
                returnCollection.Add(row);
            }
            return returnCollection.ToArray();
        }
    }

    public class RungeKutta2Ralston : RungeKuttaBase
    {
        public RungeKutta2Ralston()
            : base()
        {
        }

        public override double[][] Integrate(Parameter[] rungeKuttaParameters, Parameter x, Func<double>[] rungeKuttaFunctions, double xEnd, double step)
        {
            Debug.Assert(rungeKuttaParameters.Length == rungeKuttaFunctions.Length);
            double xStart = x;
            double stepSize = (xEnd - xStart) / step;
            int integerStepSize = (int)stepSize;
            if (stepSize > (double)integerStepSize) { integerStepSize++; }
            integerStepSize++;
            Collection<double[]> returnCollection = new Collection<double[]>();
            double[] row = new double[rungeKuttaParameters.Length + 1];
            row[0] = x;
            for (int i = 0; i < rungeKuttaParameters.Length; i++)
            {
                row[i + 1] = rungeKuttaParameters[i];
            }
            returnCollection.Add(row);

            double currentX = xStart;
            double[,] ks = new double[2, rungeKuttaParameters.Length];
            double x0 = xStart;
            double[] y0 = new double[rungeKuttaParameters.Length];
            double currentStep = step;
            while (currentX < xEnd)
            {
                if (currentX + currentStep > xEnd) { currentStep = xEnd - currentX; }
                x0 = currentX;
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    y0[i] = rungeKuttaParameters[i];
                }
                //k0s
                x.Value = x0;
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    ks[0, i] = rungeKuttaFunctions[i]();
                }
                //k1s
                x.Value = x0 + currentStep * 3.0 / 4.0;
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    rungeKuttaParameters[i].Value = y0[i] + ks[0, i] * currentStep * 3.0 / 4.0;
                }
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    ks[1, i] = rungeKuttaFunctions[i]();
                }

                //Final
                currentX += currentStep;
                row = new double[rungeKuttaParameters.Length + 1];
                row[0] = currentX;
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    rungeKuttaParameters[i].Value = y0[i] + (ks[0, i] * 1.0 / 3.0 + ks[1, i] * 2.0 / 3.0) * currentStep;
                    row[i + 1] = rungeKuttaParameters[i];
                }
                returnCollection.Add(row);
            }
            return returnCollection.ToArray();
        }
    }

    public class RungeKutta3 : RungeKuttaBase
    {
        public RungeKutta3()
            : base()
        {
        }

        public override double[][] Integrate(Parameter[] rungeKuttaParameters, Parameter x, Func<double>[] rungeKuttaFunctions, double xEnd, double step)
        {
            Debug.Assert(rungeKuttaParameters.Length == rungeKuttaFunctions.Length);
            double xStart = x;
            double stepSize = (xEnd - xStart) / step;
            int integerStepSize = (int)stepSize;
            if (stepSize > (double)integerStepSize) { integerStepSize++; }
            integerStepSize++;
            Collection<double[]> returnCollection = new Collection<double[]>();
            double[] row = new double[rungeKuttaParameters.Length + 1];
            row[0] = x;
            for (int i = 0; i < rungeKuttaParameters.Length; i++)
            {
                row[i + 1] = rungeKuttaParameters[i];
            }
            returnCollection.Add(row);

            double currentX = xStart;
            double[,] ks = new double[3, rungeKuttaParameters.Length];
            double x0 = xStart;
            double[] y0 = new double[rungeKuttaParameters.Length];
            double currentStep = step;
            while (currentX < xEnd)
            {
                if (currentX + currentStep > xEnd) { currentStep = xEnd - currentX; }
                x0 = currentX;
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    y0[i] = rungeKuttaParameters[i];
                }

                //k0s
                x.Value = x0;
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    ks[0, i] = rungeKuttaFunctions[i]();
                }

                //k1s
                x.Value = x0 + currentStep / 2.0;
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    rungeKuttaParameters[i].Value = y0[i] + ks[0, i] * currentStep / 2.0;
                }
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    ks[1, i] = rungeKuttaFunctions[i]();
                }

                //k2s
                x.Value = x0 + currentStep;
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    rungeKuttaParameters[i].Value = y0[i] + (-ks[0, i] + 2.0 * ks[1, i]) * currentStep;
                }
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    ks[2, i] = rungeKuttaFunctions[i]();
                }

                //Final
                currentX += currentStep;
                row = new double[rungeKuttaParameters.Length + 1];
                row[0] = currentX;
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    rungeKuttaParameters[i].Value = y0[i] + ((ks[0, i] + 4.0 * ks[1, i] + ks[2, i]) / 6.0) * currentStep;
                    row[i + 1] = rungeKuttaParameters[i];
                }
                returnCollection.Add(row);
            }
            return returnCollection.ToArray();
        }
    }

    public class RungeKutta4 : RungeKuttaBase
    {
        public RungeKutta4()
            : base()
        {
        }

        public override double[][] Integrate(Parameter[] rungeKuttaParameters, Parameter x, Func<double>[] rungeKuttaFunctions, double xEnd, double step)
        {
            Debug.Assert(rungeKuttaParameters.Length == rungeKuttaFunctions.Length);
            double xStart = x;
            double stepSize = (xEnd - xStart) / step;
            int integerStepSize = (int)stepSize;
            if (stepSize > (double)integerStepSize) { integerStepSize++; }
            integerStepSize++;
            Collection<double[]> returnCollection = new Collection<double[]>();
            double[] row = new double[rungeKuttaParameters.Length + 1];
            row[0] = x;
            for (int i = 0; i < rungeKuttaParameters.Length; i++)
            {
                row[i + 1] = rungeKuttaParameters[i];
            }
            returnCollection.Add(row);

            double currentX = xStart;
            double[,] ks = new double[4, rungeKuttaParameters.Length];
            double x0 = xStart;
            double[] y0 = new double[rungeKuttaParameters.Length];
            double currentStep = step;
            while (currentX < xEnd)
            {
                if (currentX + currentStep > xEnd) { currentStep = xEnd - currentX; }
                x0 = currentX;
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    y0[i] = rungeKuttaParameters[i];
                }

                //k0s
                x.Value = x0;
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    ks[0, i] = rungeKuttaFunctions[i]();
                }

                //k1s
                x.Value = x0 + currentStep / 2.0;
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    rungeKuttaParameters[i].Value = y0[i] + ks[0, i] * currentStep / 2.0;
                }
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    ks[1, i] = rungeKuttaFunctions[i]();
                }

                //k2s
                x.Value = x0 + currentStep / 2.0;
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    rungeKuttaParameters[i].Value = y0[i] + ks[1, i] * currentStep / 2.0;
                }
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    ks[2, i] = rungeKuttaFunctions[i]();
                }

                //k3s
                x.Value = x0 + currentStep;
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    rungeKuttaParameters[i].Value = y0[i] + ks[2, i] * currentStep;
                }
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    ks[3, i] = rungeKuttaFunctions[i]();
                }

                //Final
                currentX += currentStep;
                row = new double[rungeKuttaParameters.Length + 1];
                row[0] = currentX;
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    rungeKuttaParameters[i].Value = y0[i] + ((ks[0, i] + 2.0 * ks[1, i] + 2.0 * ks[2, i] + ks[3, i]) / 6.0) * currentStep;
                    row[i + 1] = rungeKuttaParameters[i];
                }
                returnCollection.Add(row);
            }
            return returnCollection.ToArray();
        }
    }

    public class RungeKutta5Butcher : RungeKuttaBase
    {
        public RungeKutta5Butcher()
            : base()
        {
        }

        public override double[][] Integrate(Parameter[] rungeKuttaParameters, Parameter x, Func<double>[] rungeKuttaFunctions, double xEnd, double step)
        {
            Debug.Assert(rungeKuttaParameters.Length == rungeKuttaFunctions.Length);
            double xStart = x;
            double stepSize = (xEnd - xStart) / step;
            int integerStepSize = (int)stepSize;
            if (stepSize > (double)integerStepSize) { integerStepSize++; }
            integerStepSize++;
            Collection<double[]> returnCollection = new Collection<double[]>();
            double[] row = new double[rungeKuttaParameters.Length + 1];
            row[0] = x;
            for (int i = 0; i < rungeKuttaParameters.Length; i++)
            {
                row[i + 1] = rungeKuttaParameters[i];
            }
            returnCollection.Add(row);

            double currentX = xStart;
            double[,] ks = new double[6, rungeKuttaParameters.Length];
            double x0 = xStart;
            double[] y0 = new double[rungeKuttaParameters.Length];
            double currentStep = step;
            while (currentX < xEnd)
            {
                if (currentX + currentStep > xEnd) { currentStep = xEnd - currentX; }
                x0 = currentX;
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    y0[i] = rungeKuttaParameters[i];
                }

                //k0s
                x.Value = x0;
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    ks[0, i] = rungeKuttaFunctions[i]();
                }

                //k1s
                x.Value = x0 + currentStep / 4.0;
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    rungeKuttaParameters[i].Value = y0[i] + (ks[0, i] / 4.0) * currentStep;
                }
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    ks[1, i] = rungeKuttaFunctions[i]();
                }

                //k2s
                x.Value = x0 + currentStep / 4.0;
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    rungeKuttaParameters[i].Value = y0[i] + (ks[0, i] / 8.0 + ks[1, i] / 8.0) * currentStep;
                }
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    ks[2, i] = rungeKuttaFunctions[i]();
                }

                //k3s
                x.Value = x0 + currentStep / 2.0;
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    rungeKuttaParameters[i].Value = y0[i] + (-ks[1, i] / 2.0 + ks[2, i]) * currentStep;
                }
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    ks[3, i] = rungeKuttaFunctions[i]();
                }

                //k4s
                x.Value = x0 + currentStep * 3.0 / 4.0;
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    rungeKuttaParameters[i].Value = y0[i] + (ks[0, i] * 3.0 / 16.0 + ks[3, i] * 9.0 / 16.0) * currentStep;
                }
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    ks[4, i] = rungeKuttaFunctions[i]();
                }

                //k5s
                x.Value = x0 + currentStep;
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    rungeKuttaParameters[i].Value = y0[i] + (-ks[0, i] * 3.0 / 7.0 + ks[1, i] * 2.0 / 7.0 + ks[2, i] * 12.0 / 7.0 - ks[3, i] * 12.0 / 7.0 + ks[4, i] * 8.0 / 7.0) * currentStep;
                }
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    ks[5, i] = rungeKuttaFunctions[i]();
                }

                //Final
                currentX += currentStep;
                row = new double[rungeKuttaParameters.Length + 1];
                row[0] = currentX;
                for (int i = 0; i < rungeKuttaParameters.Length; i++)
                {
                    rungeKuttaParameters[i].Value = y0[i] + ((7.0 * ks[0, i] + 32.0 * ks[2, i] + 12.0 * ks[3, i] + 32.0 * ks[4, i] + 7.0 * ks[5, i]) / 90.0) * currentStep;
                    row[i + 1] = rungeKuttaParameters[i];
                }
                returnCollection.Add(row);
            }
            return returnCollection.ToArray();
        }
    }

    public class RungeKutta54Fehlberg : RungeKuttaBase
    {
        private double[] _maximumNumberOfErrors;
        private double _maxAllErrors = double.NaN;
        private bool _useFifthOrderEstimate = true;

        public RungeKutta54Fehlberg()
            : base()
        {
        }

        public RungeKutta54Fehlberg(bool useFifthOrderEstimate)
        {
            _useFifthOrderEstimate = useFifthOrderEstimate;
        }


        public RungeKutta54Fehlberg(double maxAllErrors)
        {
            _maxAllErrors = maxAllErrors;
        }

        public RungeKutta54Fehlberg(double maxAllErrors, bool useFifthOrderEstimate)
        {
            _maxAllErrors = maxAllErrors;
            _useFifthOrderEstimate = useFifthOrderEstimate;
        }

        public RungeKutta54Fehlberg(double[] maxErrors)
        {
            _maximumNumberOfErrors = maxErrors;
        }

        public RungeKutta54Fehlberg(double[] maxErrors, bool useFifthOrderEstimate)
        {
            _maximumNumberOfErrors = maxErrors;
            _useFifthOrderEstimate = useFifthOrderEstimate;
        }

        public override double[][] Integrate(Parameter[] rungeKuttaParameters, Parameter x, Func<double>[] rungeKuttaFunctions, double xEnd, double step)
        {
            Debug.Assert(rungeKuttaParameters.Length == rungeKuttaFunctions.Length);
            if (_maximumNumberOfErrors == null && !double.IsNaN(_maxAllErrors))
            {
                _maximumNumberOfErrors = new double[rungeKuttaParameters.Length];
                for (int i = 0; i < _maximumNumberOfErrors.Length; i++) { _maximumNumberOfErrors[i] = _maxAllErrors; }
            }

            if (_maximumNumberOfErrors != null) { Debug.Assert(_maximumNumberOfErrors.Length == rungeKuttaParameters.Length); }

            double[] dErrors = new double[rungeKuttaParameters.Length];
            double[] ys = new double[rungeKuttaParameters.Length];
            double[] zs = new double[rungeKuttaParameters.Length];

            double xStart = x;
            double stepSize = (xEnd - xStart) / step;
            int integerStepSize = (int)stepSize;
            if (stepSize > (double)integerStepSize) { integerStepSize++; }
            integerStepSize++;
            Collection<double[]> returnCollection = new Collection<double[]>();
            double[] row = new double[rungeKuttaParameters.Length + 1];
            row[0] = x;
            for (int i = 0; i < rungeKuttaParameters.Length; i++)
            {
                row[i + 1] = rungeKuttaParameters[i];
            }
            returnCollection.Add(row);

            double currentX = xStart;
            double[,] ks = new double[6, rungeKuttaParameters.Length];

            double x0 = xStart;
            double[] y0 = new double[rungeKuttaParameters.Length];
            double currentStep = step;
            while (currentX < xEnd)
            {
                if (currentX + currentStep > xEnd) { currentStep = xEnd - currentX; }
                x0 = currentX;
                for (int j = 0; j < rungeKuttaParameters.Length; j++)
                {
                    y0[j] = rungeKuttaParameters[j];
                }

                //k0s
                x.Value = x0;
                for (int j = 0; j < rungeKuttaParameters.Length; j++)
                {
                    ks[0, j] = rungeKuttaFunctions[j]();
                }

                //k1s
                x.Value = x0 + currentStep / 4.0;
                for (int j = 0; j < rungeKuttaParameters.Length; j++)
                {
                    rungeKuttaParameters[j].Value = y0[j] + ks[0, j] * currentStep / 4.0;
                }
                for (int j = 0; j < rungeKuttaParameters.Length; j++)
                {
                    ks[1, j] = rungeKuttaFunctions[j]();
                }

                //k2s
                x.Value = x0 + currentStep * 3.0 / 8.0;
                for (int j = 0; j < rungeKuttaParameters.Length; j++)
                {
                    rungeKuttaParameters[j].Value = y0[j] + (ks[0, j] * 3.0 / 32.0 + ks[1, j] * 9.0 / 32.0) * currentStep;
                }
                for (int j = 0; j < rungeKuttaParameters.Length; j++)
                {
                    ks[2, j] = rungeKuttaFunctions[j]();
                }

                //k3s
                x.Value = x0 + currentStep * 12.0 / 13.0;
                for (int j = 0; j < rungeKuttaParameters.Length; j++)
                {
                    rungeKuttaParameters[j].Value = y0[j] + (ks[0, j] * 1932.0 / 2197.0 - ks[1, j] * 7200.0 / 2197.0 + ks[2, j] * 7296.0 / 2197.0) * currentStep;
                }
                for (int j = 0; j < rungeKuttaParameters.Length; j++)
                {
                    ks[3, j] = rungeKuttaFunctions[j]();
                }

                //k4s
                x.Value = x0 + currentStep;
                for (int j = 0; j < rungeKuttaParameters.Length; j++)
                {
                    rungeKuttaParameters[j].Value = y0[j] + (ks[0, j] * 439.0 / 216.0 - ks[1, j] * 8.0 + ks[2, j] * 3680.0 / 513.0 - ks[3, j] * 845.0 / 4104.0) * currentStep;
                }
                for (int j = 0; j < rungeKuttaParameters.Length; j++)
                {
                    ks[4, j] = rungeKuttaFunctions[j]();
                }

                //k5s
                x.Value = x0 + currentStep / 2.0;
                for (int j = 0; j < rungeKuttaParameters.Length; j++)
                {
                    rungeKuttaParameters[j].Value = y0[j] + (-ks[0, j] * 8.0 / 27.0 + ks[1, j] * 2.0 - ks[2, j] * 3544.0 / 2565.0 + ks[3, j] * 1859.0 / 4104.0 - ks[4, j] * 11.0 / 40.0) * currentStep;
                }
                for (int j = 0; j < rungeKuttaParameters.Length; j++)
                {
                    ks[5, j] = rungeKuttaFunctions[j]();
                }

                //Calculate estimated errors.
                for (int j = 0; j < rungeKuttaParameters.Length; j++)
                {
                    ys[j] = y0[j] + (ks[0, j] * 25.0 / 216.0 + ks[2, j] * 1408.0 / 2565.0 + ks[3, j] * 2197.0 / 4104.0 - ks[4, j] * 1.0 / 5.0) * currentStep;
                    zs[j] = y0[j] + (ks[0, j] * 16.0 / 135.0 + ks[2, j] * 6656.0 / 12825.0 + ks[3, j] * 28561.0 / 56430.0 - ks[4, j] * 9.0 / 50.0 + ks[5, j] * 2.0 / 55.0) * currentStep;
                    dErrors[j] = Math.Abs(ys[j] - zs[j]);
                }
                bool bErrorExceeded = false;
                double minErrorRatio = double.PositiveInfinity;
                if (_maximumNumberOfErrors != null)
                {
                    for (int j = 0; j < rungeKuttaParameters.Length; j++)
                    {
                        if (dErrors[j] > _maximumNumberOfErrors[j])
                        {
                            bErrorExceeded = true;
                        }
                        if (minErrorRatio > Math.Abs(_maximumNumberOfErrors[j] / (2.0 * (ys[j] - zs[j]))))
                        {
                            minErrorRatio = Math.Abs(_maximumNumberOfErrors[j] / (2.0 * (ys[j] - zs[j])));
                        }
                    }
                }

                if (!bErrorExceeded)
                {
                    //Final
                    currentX += currentStep;
                    row = new double[rungeKuttaParameters.Length + 1];
                    row[0] = currentX;
                    for (int j = 0; j < rungeKuttaParameters.Length; j++)
                    {
                        rungeKuttaParameters[j].Value = (_useFifthOrderEstimate) ? zs[j] : ys[j];
                        row[j + 1] = rungeKuttaParameters[j];
                    }
                    returnCollection.Add(row);
                }
                else
                {
                    x.Value = x0;
                    for (int j = 0; j < rungeKuttaParameters.Length; j++)
                    {
                        rungeKuttaParameters[j].Value = y0[j];
                    }
                }
                if (_maximumNumberOfErrors != null)
                {
                    double newStep = currentStep * Math.Pow(minErrorRatio * currentStep, 0.25);
                    if (!bErrorExceeded || newStep < currentStep)
                    {
                        currentStep *= Math.Pow(minErrorRatio * currentStep, 0.25);
                    }
                    else
                    {
                        currentStep /= 2.0;
                    }
                    if (currentStep > step) { currentStep = step; }
                }
            }
            return returnCollection.ToArray();
        }
    }
}
