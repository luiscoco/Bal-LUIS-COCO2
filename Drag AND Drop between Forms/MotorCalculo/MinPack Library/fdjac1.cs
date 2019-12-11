using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsFormsApplication2;

namespace MINPACK
{
    public class fdjac1
    {
        //****************************************************************************80

        public void fdjac1arun(Motorcalculo f1,
          int n, double[] x, double[] fvec, double[] fjac, int ldfjac, int iflag,
          int ml, int mu, double epsfcn, double[] wa1, double[] wa2)

        //****************************************************************************80
        //
        //  Purpose:
        //
        //    FDJAC1 estimates an N by N Jacobian matrix using forward differences.
        //
        //  Discussion:
        //
        //    This subroutine computes a forward-difference approximation
        //    to the n by n jacobian matrix associated with a specified
        //    problem of n functions in n variables. if the jacobian has
        //    a banded form, then function evaluations are saved by only
        //    approximating the nonzero terms.
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
        //         ----------
        //         return
        //         end
        //
        //         the value of iflag should not be changed by fcn unless
        //         the user wants to terminate execution of fdjac1.
        //         in this case set iflag to a negative integer.
        //
        //       n is a positive integer input variable set to the number
        //         of functions and variables.
        //
        //       x is an input array of length n.
        //
        //       fvec is an input array of length n which must contain the
        //         functions evaluated at x.
        //
        //       fjac is an output n by n array which contains the
        //         approximation to the jacobian matrix evaluated at x.
        //
        //       ldfjac is a positive integer input variable not less than n
        //         which specifies the leading dimension of the array fjac.
        //
        //       iflag is an integer variable which can be used to terminate
        //         the execution of fdjac1. see description of fcn.
        //
        //       ml is a nonnegative integer input variable which specifies
        //         the number of subdiagonals within the band of the
        //         jacobian matrix. if the jacobian is not banded, set
        //         ml to at least n - 1.
        //
        //       epsfcn is an input variable used in determining a suitable
        //         step length for the forward-difference approximation. this
        //         approximation assumes that the relative errors in the
        //         functions are of the order of epsfcn. if epsfcn is less
        //         than the machine precision, it is assumed that the relative
        //         errors in the functions are of the order of the machine
        //         precision.
        //
        //       mu is a nonnegative integer input variable which specifies
        //         the number of superdiagonals within the band of the
        //         jacobian matrix. if the jacobian is not banded, set
        //         mu to at least n - 1.
        //
        //       wa1 and wa2 are work arrays of length n. if ml + mu + 1 is at
        //         least n, then the jacobian is considered dense, and wa2 is
        //         not referenced.
        {
            double eps;
            double epsmch;
            double h;
            int i;
            int j;
            int k;
            int msum;
            double temp;
            Auxiliares aux = new Auxiliares();
            //
            //  EPSMCH is the machine precision.
            //
            epsmch = aux.r8_epsilon();

            eps = Math.Sqrt(aux.r8_max(epsfcn, epsmch));
            msum = ml + mu + 1;
            //
            //  Computation of dense approximate jacobian.
            //
            if (n <= msum)
            {
                for (j = 0; j < n; j++)
                {
                    temp = x[j];
                    h = eps * aux.r8_abs(temp);
                    if (h == 0.0)
                    {
                        h = eps;
                    }
                    x[j] = temp + h;
                    f1.f03(n, x, wa1, iflag);
                    if (iflag < 0)
                    {
                        break;
                    }
                    x[j] = temp;
                    for (i = 0; i < n; i++)
                    {
                        fjac[i + (j * ldfjac)] = (wa1[i] - fvec[i]) / h;
                    }
                }
            }
            //
            //  Computation of banded approximate jacobian.
            //
            else
            {
                for (k = 0; k < msum; k++)
                {
                    for (j = k; j < n; j = j + msum)
                    {
                        wa2[j] = x[j];
                        h = eps * aux.r8_abs(wa2[j]);
                        if (h == 0.0)
                        {
                            h = eps;
                        }
                        x[j] = wa2[j] + h;
                    }
                    f1.f03(n, x, wa1, iflag);
                    if (iflag < 0)
                    {
                        break;
                    }
                    for (j = k; j < n; j = j + msum)
                    {
                        x[j] = wa2[j];
                        h = eps * aux.r8_abs(wa2[j]);
                        if (h == 0.0)
                        {
                            h = eps;
                        }
                        for (i = 0; i < n; i++)
                        {
                            if (j - mu <= i && i <= j + ml)
                            {
                                fjac[i + j * ldfjac] = (wa1[i] - fvec[i]) / h;
                            }
                            else
                            {
                                fjac[i + j * ldfjac] = 0.0;
                            }
                        }
                    }
                }
            }
            return;
        }


    }
}
