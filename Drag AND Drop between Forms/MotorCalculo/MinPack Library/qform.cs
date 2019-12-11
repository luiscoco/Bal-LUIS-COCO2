using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MINPACK
{
    public class qform
    {

        //****************************************************************************80

        public void qformrun(int m, int n, double[] q, int ldq, double[] wa)

        //****************************************************************************80
        //
        //  Purpose:
        //
        //    QFORM constructs the standard form of Q from its factored form.
        //
        //  Discussion:
        //
        //    This subroutine proceeds from the computed QR factorization of
        //    an M by N matrix A to accumulate the M by M orthogonal matrix
        //    Q from its factored form.
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
        //         of rows of a and the order of q.
        //
        //       n is a positive integer input variable set to the number
        //         of columns of a.
        //
        //       q is an m by m array. on input the full lower trapezoid in
        //         the first min(m,n) columns of q contains the factored form.
        //         on output q has been accumulated into a square matrix.
        //
        //       ldq is a positive integer input variable not less than m
        //         which specifies the leading dimension of the array q.
        //
        //       wa is a work array of length m.
        //
        {
            int i;
            int j;
            int jm1;
            int k;
            int l;
            int minmn;
            int np1;
            double sum;
            double temp;
            Auxiliares aux = new Auxiliares();
            //
            //  Zero out upper triangle of Q in the first min(M,N) columns.
            //
            minmn = aux.i4_min(m, n);

            for (j = 1; j < minmn; j++)
            {
                for (i = 0; i <= j - 1; i++)
                {
                    q[i + j * ldq] = 0.0;
                }
            }
            //
            //  Initialize remaining columns to those of the identity matrix.
            //
            for (j = n; j < m; j++)
            {
                for (i = 0; i < m; i++)
                {
                    q[i + j * ldq] = 0.0;
                }
                q[j + j * ldq] = 1.0;
            }
            //
            //  Accumulate Q from its factored form.
            //
            for (k = minmn - 1; 0 <= k; k--)
            {
                for (i = k; i < m; i++)
                {
                    wa[i] = q[i + k * ldq];
                    q[i + k * ldq] = 0.0;
                }
                q[k + k * ldq] = 1.0;

                if (wa[k] != 0.0)
                {
                    for (j = k; j < m; j++)
                    {
                        sum = 0.0;
                        for (i = k; i < m; i++)
                        {
                            sum = sum + q[i + j * ldq] * wa[i];
                        }
                        temp = sum / wa[k];
                        for (i = k; i < m; i++)
                        {
                            q[i + j * ldq] = q[i + j * ldq] - temp * wa[i];
                        }
                    }
                }
            }
            return;
        }


    }
}
