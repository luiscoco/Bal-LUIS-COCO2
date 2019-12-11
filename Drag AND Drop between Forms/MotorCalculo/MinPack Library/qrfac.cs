using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MINPACK
{
    class qrfac
    {
        //****************************************************************************80

        public void qrfacrun(int m, int n, double[] a, int lda, bool pivot, int[] ipvt,
          int lipvt, double[] rdiag, double[] acnorm, double[] wa)

        //****************************************************************************80
        //
        //  Purpose:
        //
        //    QRFAC computes the QR factorization of an M by N matrix.
        //
        //  Discussion:
        //
        //    this subroutine uses householder transformations with column
        //    pivoting (optional) to compute a qr factorization of the
        //    m by n matrix a. that is, qrfac determines an orthogonal
        //    matrix q, a permutation matrix p, and an upper trapezoidal
        //    matrix r with diagonal elements of nonincreasing magnitude,
        //    such that a*p = q*r. the householder transformation for
        //    column k, k = 1,2,...,min(m,n), is of the form
        //
        //      i - (1/u(k))*u*u'
        //
        //    where u has zeros in the first k-1 positions. the form of
        //    this transformation and the method of pivoting first
        //    appeared in the corresponding linpack subroutine.
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
        //       a is an m by n array. on input a contains the matrix for
        //         which the qr factorization is to be computed. on output
        //         the strict upper trapezoidal part of a contains the strict
        //         upper trapezoidal part of r, and the lower trapezoidal
        //         part of a contains a factored form of q (the non-trivial
        //         elements of the u vectors described above).
        //
        //       lda is a positive integer input variable not less than m
        //         which specifies the leading dimension of the array a.
        //
        //       pivot is a logical input variable. if pivot is set true,
        //         then column pivoting is enforced. if pivot is set false,
        //         then no column pivoting is done.
        //
        //       ipvt is an integer output array of length lipvt. ipvt
        //         defines the permutation matrix p such that a*p = q*r.
        //         column j of p is column ipvt(j) of the identity matrix.
        //         if pivot is false, ipvt is not referenced.
        //
        //       lipvt is a positive integer input variable. if pivot is false,
        //         then lipvt may be as small as 1. if pivot is true, then
        //         lipvt must be at least n.
        //
        //       rdiag is an output array of length n which contains the
        //         diagonal elements of r.
        //
        //       acnorm is an output array of length n which contains the
        //         norms of the corresponding columns of the input matrix a.
        //         if this information is not needed, then acnorm can coincide
        //         with rdiag.
        //
        //       wa is a work array of length n. if pivot is false, then wa
        //         can coincide with rdiag.
        //
        {
            double ajnorm;
            double epsmch;
            int i;
            int j;
            int jp1;
            int k;
            int kmax;
            int minmn;
            double p05 = 0.05;
            double sum;
            double temp;
            EnormClass norma = new EnormClass();
            Auxiliares aux = new Auxiliares();
            double d1;


            //
            //  EPSMCH is the machine precision.
            //
            //epsmch = r8_epsilon();
            epsmch = 2.220446049250313e-16;

            //
            //  Compute the initial column norms and initialize several arrays.
            //
            double[,] luistemp = new double[m, n];
            int contaluis = 0;

            for (int col = 0; col < n; col++)
            {
                for (int filas = 0; filas < m; filas++)
                {
                    luistemp[filas, col] = a[contaluis];
                    contaluis++;
                }
            }


            for (j = 0; j < n; j++)
            {
                acnorm[j] = norma.Rownorm(m, j, 0, luistemp);
                rdiag[j] = acnorm[j];
                wa[j] = rdiag[j];
                if (pivot)
                {
                    ipvt[j] = j;
                }
            }
            //
            //  Reduce A to R with Householder transformations.
            //
            minmn = aux.i4_min(m, n);

            for (j = 0; j < minmn; j++)
            {
                if (pivot)
                {
                    //
                    //  Bring the column of largest norm into the pivot position.
                    //
                    kmax = j;
                    for (k = j; k < n; k++)
                    {
                        if (rdiag[kmax] < rdiag[k])
                        {
                            kmax = k;
                        }
                    }
                    if (kmax != j)
                    {
                        for (i = 0; i < m; i++)
                        {
                            temp = a[i + j * lda];
                            a[i + j * lda] = a[i + kmax * lda];
                            a[i + kmax * lda] = temp;
                        }
                        rdiag[kmax] = rdiag[j];
                        wa[kmax] = wa[j];
                        k = ipvt[j];
                        ipvt[j] = ipvt[kmax];
                        ipvt[kmax] = k;
                    }
                }
                //
                //  Compute the Householder transformation to reduce the
                //  J-th column of A to a multiple of the J-th unit vector.
                //
                contaluis = 0;
                for (int col = 0; col < n; col++)
                {
                    for (int filas = 0; filas < m; filas++)
                    {
                        luistemp[filas, col] = a[contaluis];
                        contaluis++;
                    }
                }

                ajnorm = norma.Rownorm(m, j, j, luistemp);

                if (ajnorm != 0.0)
                {
                    if (a[j + j * lda] < 0.0)
                    {
                        ajnorm = -ajnorm;
                    }
                    for (i = j; i < m; i++)
                    {
                        a[i + j * lda] = a[i + j * lda] / ajnorm;
                    }
                    a[j + j * lda] = a[j + j * lda] + 1.0;
                    //
                    //  Apply the transformation to the remaining columns and update the norms.
                    //
                    jp1 = j + 1;

                    if (n > jp1)
                    {
                        for (k = jp1; k < n; k++)
                        {
                            sum = 0.0;
                            for (i = j; i < m; i++)
                            {
                                sum = sum + a[i + j * lda] * a[i + k * lda];
                            }
                            temp = sum / a[j + j * lda];
                            for (i = j; i < m; i++)
                            {
                                a[i + k * lda] = a[i + k * lda] - temp * a[i + j * lda];
                            }
                            if (pivot && rdiag[k] != 0.0)
                            {
                                temp = a[j + k * lda] / rdiag[k];
                                rdiag[k] = rdiag[k] * Math.Sqrt(aux.r8_max(0.0, 1.0 - temp * temp));
                                if (p05 * (rdiag[k] / wa[k]) * (rdiag[k] / wa[k]) <= epsmch)
                                {

                                    contaluis = 0;
                                    for (int col = 0; col < n; col++)
                                    {
                                        for (int filas = 0; filas < m; filas++)
                                        {
                                            luistemp[filas, col] = a[contaluis];
                                            contaluis++;
                                        }
                                    }

                                    rdiag[k] = norma.Rownorm(m, k, jp1, luistemp);

                                    wa[k] = rdiag[k];
                                }
                            }
                        }
                    }
                }
                rdiag[j] = -ajnorm;
            }
            return;
        }


    }
}
