using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MINPACK
{
    public class dogleg
    {

        //****************************************************************************80

        public void doglegrun(int n, double[] r, int lr, double[] diag, double[] qtb,
          double delta, double[] x, double[] wa1, double[] wa2)

        //****************************************************************************80
        //
        //  Purpose:
        //
        //    DOGLEG combines Gauss-Newton and gradient for a minimizing step.
        //
        //  Discussion:
        //
        //    Given an m by n matrix a, an n by n nonsingular diagonal
        //    matrix d, an m-vector b, and a positive number delta, the
        //    problem is to determine the convex combination x of the
        //    gauss-newton and scaled gradient directions that minimizes
        //    (a*x - b) in the least squares sense, subject to the
        //    restriction that the euclidean norm of d*x be at most delta.
        //
        //    This subroutine completes the solution of the problem
        //    if it is provided with the necessary information from the
        //    qr factorization of a. that is, if a = q*r, where q has
        //    orthogonal columns and r is an upper triangular matrix,
        //    then dogleg expects the full upper triangle of r and
        //    the first n components of (q transpose)*b.
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
        //       n is a positive integer input variable set to the order of r.
        //
        //       r is an input array of length lr which must contain the upper
        //         triangular matrix r stored by rows.
        //
        //       lr is a positive integer input variable not less than
        //         (n*(n+1))/2.
        //
        //       diag is an input array of length n which must contain the
        //         diagonal elements of the matrix d.
        //
        //       qtb is an input array of length n which must contain the first
        //         n elements of the vector (q transpose)*b.
        //
        //       delta is a positive input variable which specifies an upper
        //         bound on the euclidean norm of d*x.
        //
        //       x is an output array of length n which contains the desired
        //         convex combination of the gauss-newton direction and the
        //         scaled gradient direction.
        //
        //       wa1 and wa2 are work arrays of length n.
        //
        {
            double alpha;
            double bnorm;
            double epsmch;
            double gnorm;
            int i;
            int j;
            int jj;
            int jp1;
            int k;
            int l;
            double qnorm;
            double sgnorm;
            double sum;
            double temp;
            Auxiliares aux = new Auxiliares();
            EnormClass norma = new EnormClass();

            //
            //  EPSMCH is the machine precision.
            //
            epsmch = aux.r8_epsilon();
            //
            //  Calculate the Gauss-Newton direction.
            //
            jj = (n * (n + 1)) / 2 + 1;

            for (k = 1; k <= n; k++)
            {
                j = n - k + 1;
                jp1 = j + 1;
                jj = jj - k;
                l = jj + 1;
                sum = 0.0;
                for (i = jp1; i <= n; i++)
                {
                    sum = sum + r[l - 1] * x[i - 1];
                    l = l + 1;
                }
                temp = r[jj - 1];
                if (temp == 0.0)
                {
                    l = j;
                    for (i = 1; i <= j; i++)
                    {
                        temp = aux.r8_max(temp, aux.r8_abs(r[l - 1]));
                        l = l + n - i;
                    }
                    temp = epsmch * temp;
                    if (temp == 0.0)
                    {
                        temp = epsmch;
                    }
                }
                x[j - 1] = (qtb[j - 1] - sum) / temp;
            }
            //
            //  Test whether the Gauss-Newton direction is acceptable.
            //
            for (j = 0; j < n; j++)
            {
                wa1[j] = 0.0;
                wa2[j] = diag[j] * x[j];
            }
            qnorm = norma.Enorm(n, wa2);

            if (qnorm <= delta)
            {
                return;
            }
            //
            //  The Gauss-Newton direction is not acceptable.
            //  Calculate the scaled gradient direction.
            //
            l = 0;
            for (j = 0; j < n; j++)
            {
                temp = qtb[j];
                for (i = j; i < n; i++)
                {
                    wa1[i] = wa1[i] + r[l] * temp;
                    l = l + 1;
                }
                wa1[j] = wa1[j] / diag[j];
            }
            //
            //  Calculate the norm of the scaled gradient and test for
            //  the special case in which the scaled gradient is zero.
            //
            gnorm = norma.Enorm(n, wa1);
            sgnorm = 0.0;
            alpha = delta / qnorm;
            //
            //  Calculate the point along the scaled gradient
            //  at which the quadratic is minimized.
            //
            if (gnorm != 0.0)
            {
                for (j = 0; j < n; j++)
                {
                    wa1[j] = (wa1[j] / gnorm) / diag[j];
                }
                l = 0;
                for (j = 0; j < n; j++)
                {
                    sum = 0.0;
                    for (i = j; i < n; i++)
                    {
                        sum = sum + r[l] * wa1[i];
                        l = l + 1;
                    }
                    wa2[j] = sum;
                }
                temp = norma.Enorm(n, wa2);
                sgnorm = (gnorm / temp) / temp;
                alpha = 0.0;
                //
                //  If the scaled gradient direction is not acceptable,
                //  calculate the point along the dogleg at which the quadratic is minimized.
                //
                if (sgnorm < delta)
                {
                    bnorm = norma.Enorm(n, qtb);
                    temp = (bnorm / gnorm) * (bnorm / qnorm) * (sgnorm / delta);
                    temp = temp - (delta / qnorm) * (sgnorm / delta) * (sgnorm / delta)
                      + Math.Sqrt(Math.Pow(temp - (delta / qnorm), 2)
                      + (1.0 - (delta / qnorm) * (delta / qnorm))
                      * (1.0 - (sgnorm / delta) * (sgnorm / delta)));
                    alpha = ((delta / qnorm)
                      * (1.0 - (sgnorm / delta) * (sgnorm / delta))) / temp;
                }
            }
            //
            //  Form appropriate convex combination of the Gauss-Newton
            //  direction and the scaled gradient direction.
            //
            temp = (1.0 - alpha) * aux.r8_min(sgnorm, delta);
            for (j = 0; j < n; j++)
            {
                x[j] = temp * wa1[j] + alpha * x[j];
            }
            return;
        }

    }
}
