using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsFormsApplication2;

namespace MINPACK
{
    public class Hybrid1
    {

        public int hybrd1run(Motorcalculo f1, int n,
double[] x, double[] fvec, double tol, double[] wa, int lwa,Double errormaximo,ref Double nummaxiteraciones, int ml,int mu)

//****************************************************************************80
        //
        //  Purpose:
        //
        //    HYBRD1 is a simplified interface to HYBRD.
        //
        //  Discussion:
        //
        //    The purpose of HYBRD1 is to find a zero of a system of
        //    N nonlinear functions in N variables by a modification
        //    of the Powell hybrid method.  This is done by using the
        //    more general nonlinear equation solver HYBRD.  The user
        //    must provide a subroutine which calculates the functions.
        //    The jacobian is then calculated by a forward-difference
        //    approximation.
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
        //       fcn is the name of the user-supplied subroutine which
        //         calculates the functions. fcn must be declared
        //         in an external statement in the user calling
        //         program, and should be written as follows.
        //
        //         subroutine fcn(n,x,fvec,iflag)
        //         integer n,iflag
        //         double precision x(n),fvec(n)
        //         ----------
        //         calculate the functions at x and
        //         return this vector in fvec.
        //         ---------
        //         return
        //         end
        //
        //         the value of iflag should not be changed by fcn unless
        //         the user wants to terminate execution of hybrd1.
        //         in this case set iflag to a negative integer.
        //
        //       n is a positive integer input variable set to the number
        //         of functions and variables.
        //
        //       x is an array of length n. on input x must contain
        //         an initial estimate of the solution vector. on output x
        //         contains the final estimate of the solution vector.
        //
        //       fvec is an output array of length n which contains
        //         the functions evaluated at the output x.
        //
        //       tol is a nonnegative input variable. termination occurs
        //         when the algorithm estimates that the relative error
        //         between x and the solution is at most tol.
        //
        //       info is an integer output variable. if the user has
        //         terminated execution, info is set to the (negative)
        //         value of iflag. see description of fcn. otherwise,
        //         info is set as follows.
        //
        //         info = 0   improper input parameters.
        //
        //         info = 1   algorithm estimates that the relative error
        //                    between x and the solution is at most tol.
        //
        //         info = 2   number of calls to fcn has reached or exceeded
        //                    200*(n+1).
        //
        //         info = 3   tol is too small. no further improvement in
        //                    the approximate solution x is possible.
        //
        //         info = 4   iteration is not making good progress.
        //
        //       wa is a work array of length lwa.
        //
        //       lwa is a positive integer input variable not less than
        //         (n*(3*n+13))/2.
        //
        {
            double epsfcn;

            //
            double factor = 100.0;
            int index;
            int info;
            int j;
            int lr;
            int maxfev;
           
            int mode;
           
            int nfev = 0;
            int nprint;
            double xtol;
            MINPACK.Hybrid hybridinstancia = new Hybrid();

            info = 0;
            //
            //  Check the input.
            //
            if (n <= 0)
            {
                return info;
            }

            //En caso de que el Error Máximo de X sea <= 0 nos devuelve un Error la función.
            if (tol <= 0.0)
            {
                return info;
            }
            if (lwa < (n * (3 * n + 13)) / 2)
            {
                return info;
            }
            //
            //  Call HYBRD.

            //Número máximo de ITERACIONES
            maxfev = (int)nummaxiteraciones;

            //Tolerancias del Error Absoluto
            tol = errormaximo;
            xtol = tol;


            //Número de Bandas Superiores e Inferiores de la Matriz Banda Jacobiana
            //Número de Bandas Inferiores
            //ml = n - 1;
            //Número de Bandas Superiores
            //mu = n - 1;
            
            //Faja 
            epsfcn = 0.0;
            
            mode = 2;
            
            for (j = 0; j < n; j++)
            {
                wa[j] = 1.0;
            }
            nprint = 0;
            lr = (n * (n + 1)) / 2;
            //index = 6 * n + lr;
            index = (n * (n + 1));

            //Variables auxiliares creadas por LUIS COCO para llamar a HYBRD
            double[] wa1 = new double[index];
            double[] wa2 = new double[n*n];
            double[] wa3 = new double[n];
            double[] wa4 = new double[2 * n];
            double[] wa5 = new double[3 * n];
            double[] wa6 = new double[4 * n];
            double[] wa7 = new double[5 * n];

           
            info = hybridinstancia.hybrd(f1, n, x, fvec, xtol, maxfev, ml, mu, epsfcn, wa, mode,
              factor, nprint,ref nfev, wa1, n, wa2, lr,
              wa3, wa4, wa5, wa6, wa7);

            nummaxiteraciones=(Double)nfev;

            if (info == 5)
            {
                info = 4;
            }
            return info;
        }

    }
}
