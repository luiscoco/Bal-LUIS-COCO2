using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MINPACK
{
    public class chkder
    {
        //****************************************************************************80

        void chkderRUN(int m, int n, double[] x, double[] fvec, double[] fjac,
          int ldfjac, double[] xp, double[] fvecp, int mode, double[] err)

        //****************************************************************************80
        //
        //  Purpose:
        //
        //    CHKDER checks the gradients of M functions in N variables.
        //
        //  Discussion:
        //
        //    This subroutine checks the gradients of M nonlinear functions
        //    in N variables, evaluated at a point x, for consistency with
        //    the functions themselves. the user must call chkder twice,
        //    first with mode = 1 and then with mode = 2.
        //
        //    mode = 1: on input, x must contain the point of evaluation.
        //    on output, xp is set to a neighboring point.
        //
        //    mode = 2. on input, fvec must contain the functions and the
        //    rows of fjac must contain the gradients
        //    of the respective functions each evaluated
        //    at x, and fvecp must contain the functions
        //    evaluated at xp.
        //    on output, err contains measures of correctness of
        //    the respective gradients.
        //
        //    the subroutine does not perform reliably if cancellation or
        //    rounding errors cause a severe loss of significance in the
        //    evaluation of a function. therefore, none of the components
        //    of x should be unusually small (in particular, zero) or any
        //    other value which may cause loss of significance.
        //
        //  Licensing:
        //
        //    This code is distributed under the GNU LGPL license.
        //
        //  Modified:
        //
        //    05 April 2010
        //
        //  Author:
        //
        //    Original FORTRAN77 version by Jorge More, Burt Garbow, Ken Hillstrom.
        //    C++ version by John Burkardt.
        //
        //  Reference:
        //
        //    Jorge More, Burton Garbow, Kenneth Hillstrom,
        //    User Guide for MINPACK-1,
        //    Technical Report ANL-80-74,
        //    Argonne National Laboratory, 1980.
        //
        //  Parameters:
        //
        //       m is a positive integer input variable set to the number
        //         of functions.
        //
        //       n is a positive integer input variable set to the number
        //         of variables.
        //
        //       x is an input array of length n.
        //
        //       fvec is an array of length m. on input when mode = 2,
        //         fvec must contain the functions evaluated at x.
        //
        //       fjac is an m by n array. on input when mode = 2,
        //         the rows of fjac must contain the gradients of
        //         the respective functions evaluated at x.
        //
        //       ldfjac is a positive integer input parameter not less than m
        //         which specifies the leading dimension of the array fjac.
        //
        //       xp is an array of length n. on output when mode = 1,
        //         xp is set to a neighboring point of x.
        //
        //       fvecp is an array of length m. on input when mode = 2,
        //         fvecp must contain the functions evaluated at xp.
        //
        //       mode is an integer input variable set to 1 on the first call
        //         and 2 on the second. other values of mode are equivalent
        //         to mode = 1.
        //
        //       err is an array of length m. on output when mode = 2,
        //         err contains measures of correctness of the respective
        //         gradients. if there is no severe loss of significance,
        //         then if err(i) is 1.0 the i-th gradient is correct,
        //         while if err(i) is 0.0 the i-th gradient is incorrect.
        //         for values of err between 0.0 and 1.0, the categorization
        //         is less certain. in general, a value of err(i) greater
        //         than 0.5 indicates that the i-th gradient is probably
        //         correct, while a value of err(i) less than 0.5 indicates
        //         that the i-th gradient is probably incorrect.
        //
        {
            double eps;
            double epsf;
            double epslog;
            double epsmch;
            double factor = 100.0;
            int i;
            int j;
            double temp;

            Auxiliares aux = new Auxiliares();
            //
            //  EPSMCH is the machine precision.
            //
            epsmch = aux.r8_epsilon();
            //
            eps = Math.Sqrt(epsmch);
            //
            //  MODE = 1.
            //
            if (mode == 1)
            {
                for (j = 0; j < n; j++)
                {
                    if (x[j] == 0.0)
                    {
                        temp = eps;
                    }
                    else
                    {
                        temp = eps * aux.r8_abs(x[j]);
                    }
                    xp[j] = x[j] + temp;
                }
            }
            //
            //  MODE = 2.
            //
            else
            {
                epsf = factor * epsmch;
                epslog = Math.Log10(eps);
                for (i = 0; i < m; i++)
                {
                    err[i] = 0.0;
                }

                for (j = 0; j < n; j++)
                {
                    if (x[j] == 0.0)
                    {
                        temp = 1.0;
                    }
                    else
                    {
                        temp = aux.r8_abs(x[j]);
                    }
                    for (i = 0; i < m; i++)
                    {
                        err[i] = err[i] + temp * fjac[i + j * ldfjac];
                    }
                }

                for (i = 0; i < m; i++)
                {
                    temp = 1.0;
                    if (fvec[i] != 0.0 &&
                         fvecp[i] != 0.0 &&
                         epsf * aux.r8_abs(fvec[i]) <= aux.r8_abs(fvecp[i] - fvec[i]))
                    {
                        temp = eps * aux.r8_abs((fvecp[i] - fvec[i]) / eps - err[i])
                          / (aux.r8_abs(fvec[i]) + aux.r8_abs(fvecp[i]));

                        if (temp <= epsmch)
                        {
                            err[i] = 1.0;
                        }
                        else if (temp < eps)
                        {
                            err[i] = (Math.Log10(temp) - epslog) / epslog;
                        }
                        else
                        {
                            err[i] = 0.0;
                        }
                    }
                }
            }
            return;
        }



    }
}

