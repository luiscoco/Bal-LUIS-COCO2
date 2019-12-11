using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MINPACK
{
    public class r1mpyq
    {


        //****************************************************************************80

        public void r1mpyqrun(int m, int n, double[] a, int lda, double[] v, double[] w)

        //****************************************************************************80
        //
        //  Purpose:
        //
        //    R1MPYQ multiplies an M by N matrix A by the Q factor.
        //
        //  Discussion:
        //
        //    Given an m by n matrix a, this subroutine computes a*q where
        //    q is the product of 2*(n - 1) transformations
        //
        //      gv(n-1)*...*gv(1)*gw(1)*...*gw(n-1)
        //
        //    and gv(i), gw(i) are givens rotations in the (i,n) plane which
        //    eliminate elements in the i-th and n-th planes, respectively.
        //    q itself is not given, rather the information to recover the
        //    gv, gw rotations is supplied.
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
        //         of rows of a.
        //
        //       n is a positive integer input variable set to the number
        //         of columns of a.
        //
        //       a is an m by n array. on input a must contain the matrix
        //         to be postmultiplied by the orthogonal matrix q
        //         described above. on output a*q has replaced a.
        //
        //       lda is a positive integer input variable not less than m
        //         which specifies the leading dimension of the array a.
        //
        //       v is an input array of length n. v(i) must contain the
        //         information necessary to recover the givens rotation gv(i)
        //         described above.
        //
        //       w is an input array of length n. w(i) must contain the
        //         information necessary to recover the givens rotation gw(i)
        //         described above.
        //
        {
            double c;
            int i;
            int j;
            double s;
            double temp;

            Auxiliares aux = new Auxiliares(); ;
            //
            //  Apply the first set of Givens rotations to A.
            //
            for (j = n - 2; 0 <= j; j--)
            {
                if (1.0 < aux.r8_abs(v[j]))
                {
                    c = 1.0 / v[j];
                    s = Math.Sqrt(1.0 - c * c);
                }
                else
                {
                    s = v[j];
                    c = Math.Sqrt(1.0 - s * s);
                }
                for (i = 0; i < m; i++)
                {
                    temp = c * a[i + j * lda] - s * a[i + (n - 1) * lda];
                    a[i + (n - 1) * lda] = s * a[i + j * lda] + c * a[i + (n - 1) * lda];
                    a[i + j * lda] = temp;
                }
            }
            //
            //  Apply the second set of Givens rotations to A.
            //
            for (j = 0; j < n - 1; j++)
            {
                if (1.0 < aux.r8_abs(w[j]))
                {
                    c = 1.0 / w[j];
                    s = Math.Sqrt(1.0 - c * c);
                }
                else
                {
                    s = w[j];
                    c = Math.Sqrt(1.0 - s * s);
                }
                for (i = 0; i < m; i++)
                {
                    temp = c * a[i + j * lda] + s * a[i + (n - 1) * lda];
                    a[i + (n - 1) * lda] = -s * a[i + j * lda] + c * a[i + (n - 1) * lda];
                    a[i + j * lda] = temp;
                }
            }

            return;
        }

    }
}
