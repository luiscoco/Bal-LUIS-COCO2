//Created by Trent F Guidry
using System;
using System.Collections.ObjectModel;

namespace NumericalMethods
{
    public class Parameter
    {
        private bool _isSolvedFor = true;
        private double _value;
        private string _nombre;
        private double _derivativeStep = 1e-2;
        private DerivativeStepType _derivativeStepType = DerivativeStepType.Relative;

        public Parameter()
            : base()
        {
        }

        public Parameter(double value,string nombre)
            : this()
        {
            _nombre = nombre;
            _value = value;
        }

        public Parameter(double value)
            : this()
        {
            
            _value = value;
        }

        public Parameter(double value, double derivativeStep)
            : this(value)
        {
            _derivativeStep = derivativeStep;
        }

        public Parameter(double value, double derivativeStep,string nombre)
            : this(value,derivativeStep)
        {
            _derivativeStep = derivativeStep;
            _value = value;
            _nombre = nombre;
        }

        public Parameter(double value, double derivativeStep, DerivativeStepType stepSizeType)
            : this(value, derivativeStep)
        {
            _derivativeStepType = stepSizeType;
        }

        public Parameter(double value, double derivativeStep, DerivativeStepType stepSizeType, bool isSolvedFor)
            : this(value, derivativeStep, stepSizeType)
        {
            _isSolvedFor = isSolvedFor;
        }

        public Parameter(bool isSolvedFor)
            : this()
        {
            _isSolvedFor = isSolvedFor;
        }

        public Parameter(Parameter clone)
        {
            _isSolvedFor = clone.IsSolvedFor;
            _value = clone.Value;
            _nombre = clone.Nombre;
            _derivativeStep = clone.DerivativeStep;
            _derivativeStepType = clone.DerivativeStepType;
        }

        public bool IsSolvedFor
        {
            get { return _isSolvedFor; }
            set { _isSolvedFor = value; }
        }

        public double Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }

        }

        public double DerivativeStep
        {
            get { return _derivativeStep; }
            set { _derivativeStep = value; }
        }

        public double DerivativeStepSize
        {
            get
            {
                double derivativeStepSize;
                if (_derivativeStepType == DerivativeStepType.Absolute)
                {
                    derivativeStepSize = _derivativeStep;
                }
                else
                {
                    if (_value != 0.0)
                    {
                        derivativeStepSize = _derivativeStep * Math.Abs(_value);
                    }
                    else
                    {
                        derivativeStepSize = _derivativeStep;
                    }
                }
                return derivativeStepSize;
            }
        }

        public DerivativeStepType DerivativeStepType
        {
            get { return _derivativeStepType; }
            set { _derivativeStepType = value; }
        }

        public static implicit operator double(Parameter p)
        {
            return p.Value;
        }

        public override string ToString()
        {
            return Nombre.ToString()+":" + Value.ToString();
        }
    }

    public enum DerivativeStepType
    {
        Relative,
        Absolute
    }

    public class ParameterCollection : Collection<Parameter>
    {
        public ParameterCollection()
            : base()
        {
        }
    }

}
