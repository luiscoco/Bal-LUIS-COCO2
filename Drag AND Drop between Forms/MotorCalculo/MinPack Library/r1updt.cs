using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MINPACK
{
    public class r1updt
    {
        //****************************************************************************80

        public bool r1updtrun(int m, int n, double[] s, int ls, double[] u, double[] v,
          double[] w)

        //****************************************************************************80
        //
        //  Purpose:
        //
        //    R1UPDT updates the Q factor after a rank one update of the matrix.
        //
        //  Discussion:
        //
        //    Given an m by n lower trapezoidal matrix s, an m-vector u,
        //    and an n-vector v, the problem is to determine an
        //    orthogonal matrix q such that
        //
        //      (s + u*v') * q
        //
        //    is again lower trapezoidal.
        //
        //    This subroutine determines q as the product of 2*(n - 1)
        //    transformations
        //
        //      gv(n-1)*...*gv(1)*gw(1)*...*gw(n-1)
        //
        //    where gv(i), gw(i) are givens rotations in the (i,n) plane
        //    which eliminate elements in the i-th and n-th planes,
        //    respectively. q itself is not accumulated, rather the
        //    information to recover the gv, gw rotations is returned.
        //
        //  Licensing:
        //
        //    This code is distributed under the GNU LGPL license.
        //
        //  Modified:
        //
        //    08 April 2010
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
        //         of rows of s.
        //
        //       n is a positive integer input variable set to the number
        //         of columns of s. n must not exceed m.
        //
        //       s is an array of length ls. on input s must contain the lower
        //         trapezoidal matrix s stored by columns. on output s contains
        //         the lower trapezoidal matrix produced as described above.
        //
        //       ls is a positive integer input variable not less than
        //         (n*(2*m-n+1))/2.
        //
        //       u is an input array of length m which must contain the
        //         vector u.
        //
        //       v is an array of length n. on input v must contain the vector
        //         v. on output v(i) contains the information necessary to
        //         recover the givens rotation gv(i) described above.
        //
        //       w is an output array of length m. w(i) contains information
        //         necessary to recover the givens rotation gw(i) described
        //         above.
        //
        //       sing is a logical output variable. sing is set true if any
        //         of the diagonal elements of the output s are zero. otherwise
        //         sing is set false.
        //
        {
            double cotan;
            double cs;
            double giant;
            int i;
            int j;
            int jj;
            int l;
            int nmj;
            int nm1;
            double p25 = 0.25;
            double p5 = 0.5;
            double sn;
            bool sing;
            double tan;
            double tau;
            double temp;
            Auxiliares aux = new Auxiliares();

            //
            //  Because of the computation of the pointer JJ, this function was
            //  converted from FORTRAN77 to C++ in a conservative way.  All computations
            //  are the same, and only array indexing is adjusted.
            //
            //  GIANT is the largest magnitude.
            //
            giant = aux.r8_huge();
            //
            //  Initialize the diagonal element pointer.
            //
            jj = (n * (2 * m - n + 1)) / 2 - (m - n);
            //
            //  Move the nontrivial part of the last column of S into W.
            //
            l = jj;
            for (i = n; i <= m; i++)
            {
                w[i - 1] = s[l - 1];
                l = l + 1;
            }
            //
            //  Rotate the vector V into a multiple of the N-th unit vector
            //  in such a way that a spike is introduced into W.
            //
            nm1 = n - 1;

            for (j = n - 1; 1 <= j; j--)
            {
                jj = jj - (m - j + 1);
                w[j - 1] = 0.0;

                if (v[j - 1] != 0.0)
                {
                    //
                    //  Determine a Givens rotation which eliminates the J-th element of V.
                    //
                    if (aux.r8_abs(v[n - 1]) < aux.r8_abs(v[j - 1]))
                    {
                        cotan = v[n - 1] / v[j - 1];
                        sn = p5 / Math.Sqrt(p25 + p25 * cotan * cotan);
                        cs = sn * cotan;
                        tau = 1.0;
                        if (1.0 < aux.r8_abs(cs) * giant)
                        {
                            tau = 1.0 / cs;
                        }
                    }
                    else
                    {
                        tan = v[j - 1] / v[n - 1];
                        cs = p5 / Math.Sqrt(p25 + p25 * tan * tan);
                        sn = cs * tan;
                        tau = sn;
                    }
                    //
                    //  Apply the transformation to V and store the information
                    //  necessary to recover the Givens rotation.
                    //
                    v[n - 1] = sn * v[j - 1] + cs * v[n - 1];
                    v[j - 1] = tau;
                    //
                    //  Apply the transformation to S and extend the spike in W.
                    //
                    l = jj;
                    for (i = j; i <= m; i++)
                    {
                        temp = cs * s[l - 1] - sn * w[i - 1];
                        w[i - 1] = sn * s[l - 1] + cs * w[i - 1];
                        s[l - 1] = temp;
                        l = l + 1;
                    }
                }
            }
            //
            //  Add the spike from the rank 1 update to W.
            //
            for (i = 1; i <= m; i++)
            {
                w[i - 1] = w[i - 1] + v[n - 1] * u[i - 1];
            }
            //
            //  Eliminate the spike.
            //
            sing = false;

            for (j = 1; j <= nm1; j++)
            {
                //
                //  Determine a Givens rotation which eliminates the
                //  J-th element of the spike.
                //
                if (w[j - 1] != 0.0)
                {

                    if (aux.r8_abs(s[jj - 1]) < aux.r8_abs(w[j - 1]))
                    {
                        cotan = s[jj - 1] / w[j - 1];
                        sn = p5 / Math.Sqrt(p25 + p25 * cotan * cotan);
                        cs = sn * cotan;
                        tau = 1.0;
                        if (1.0 < aux.r8_abs(cs) * giant)
                        {
                            tau = 1.0 / cs;
                        }
                    }
                    else
                    {
                        tan = w[j - 1] / s[jj - 1];
                        cs = p5 / Math.Sqrt(p25 + p25 * tan * tan);
                        sn = cs * tan;
                        tau = sn;
                    }
                    //
                    //  Apply the transformation to s and reduce the spike in w.
                    //
                    l = jj;

                    for (i = j; i <= m; i++)
                    {
                        temp = cs * s[l - 1] + sn * w[i - 1];
                        w[i - 1] = -sn * s[l - 1] + cs * w[i - 1];
                        s[l - 1] = temp;
                        l = l + 1;
                    }
                    //
                    //  Store the information necessary to recover the givens rotation.
                    //
                    w[j - 1] = tau;
                }
                //
                //  Test for zero diagonal elements in the output s.
                //
                if (s[jj - 1] == 0.0)
                {
                    sing = true;
                }
                jj = jj + (m - j + 1);
            }
            //
            //  Move W back into the last column of the output S.
            //
            l = jj;
            for (i = n; i <= m; i++)
            {
                s[l - 1] = w[i - 1];
                l = l + 1;
            }
            if (s[jj - 1] == 0.0)
            {
                sing = true;
            }
            return sing;
        }




    }
}
