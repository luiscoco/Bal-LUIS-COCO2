using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MINPACK
{
    public class Auxiliares
    {

        public double r8_epsilon()

//****************************************************************************80
        //
        //  Purpose:
        //
        //    R8_EPSILON returns the R8 roundoff unit.
        //
        //  Discussion:
        //
        //    The roundoff unit is a number R which is a power of 2 with the
        //    property that, to the precision of the computer's arithmetic,
        //      1 < 1 + R
        //    but
        //      1 = ( 1 + R / 2 )
        //
        //  Licensing:
        //
        //    This code is distributed under the GNU LGPL license.
        //
        //  Modified:
        //
        //    01 July 2004
        //
        //  Author:
        //
        //    John Burkardt
        //
        //  Parameters:
        //
        //    Output, double R8_EPSILON, the R8 round-off unit.
        //
        {
            double value;

            value = 1.0;

            while (1.0 < (double)(1.0 + value))
            {
                value = value / 2.0;
            }

            value = 2.0 * value;

            return value;
        }


        public double r8_max(double x, double y)

//****************************************************************************80
        //
        //  Purpose:
        //
        //    R8_MAX returns the maximum of two R8's.
        //
        //  Licensing:
        //
        //    This code is distributed under the GNU LGPL license.
        //
        //  Modified:
        //
        //    18 August 2004
        //
        //  Author:
        //
        //    John Burkardt
        //
        //  Parameters:
        //
        //    Input, double X, Y, the quantities to compare.
        //
        //    Output, double R8_MAX, the maximum of X and Y.
        //
        {
            double value;

            if (y < x)
            {
                value = x;
            }
            else
            {
                value = y;
            }
            return value;
        }


        //****************************************************************************80

        public int i4_min(int i1, int i2)

        //****************************************************************************80
        //
        //  Purpose:
        //
        //    I4_MIN returns the minimum of two I4's.
        //
        //  Licensing:
        //
        //    This code is distributed under the GNU LGPL license.
        //
        //  Modified:
        //
        //    13 October 1998
        //
        //  Author:
        //
        //    John Burkardt
        //
        //  Parameters:
        //
        //    Input, int I1, I2, two integers to be compared.
        //
        //    Output, int I4_MIN, the smaller of I1 and I2.
        //
        {
            int value;

            if (i1 < i2)
            {
                value = i1;
            }
            else
            {
                value = i2;
            }
            return value;
        }


        //****************************************************************************80

        public double r8_min(double x, double y)

        //****************************************************************************80
        //
        //  Purpose:
        //
        //    R8_MIN returns the minimum of two R8's.
        //
        //  Licensing:
        //
        //    This code is distributed under the GNU LGPL license.
        //
        //  Modified:
        //
        //    31 August 2004
        //
        //  Author:
        //
        //    John Burkardt
        //
        //  Parameters:
        //
        //    Input, double X, Y, the quantities to compare.
        //
        //    Output, double R8_MIN, the minimum of X and Y.
        //
        {
            double value;

            if (y < x)
            {
                value = y;
            }
            else
            {
                value = x;
            }
            return value;
        }


        //****************************************************************************80

        public double r8_abs(double x)

        //****************************************************************************80
        //
        //  Purpose:
        //
        //    R8_ABS returns the absolute value of an R8.
        //
        //  Licensing:
        //
        //    This code is distributed under the GNU LGPL license.
        //
        //  Modified:
        //
        //    14 November 2006
        //
        //  Author:
        //
        //    John Burkardt
        //
        //  Parameters:
        //
        //    Input, double X, the quantity whose absolute value is desired.
        //
        //    Output, double R8_ABS, the absolute value of X.
        //
        {
            double value;

            if (0.0 <= x)
            {
                value = x;
            }
            else
            {
                value = -x;
            }
            return value;
        }







        //****************************************************************************80

        public double r8_huge()

        //****************************************************************************80
        //
        //  Purpose:
        //
        //    R8_HUGE returns a "huge" R8.
        //
        //  Discussion:
        //
        //    The value returned by this function is NOT required to be the
        //    maximum representable R8.  This value varies from machine to machine,
        //    from compiler to compiler, and may cause problems when being printed.
        //    We simply want a "very large" but non-infinite number.
        //
        //  Licensing:
        //
        //    This code is distributed under the GNU LGPL license.
        //
        //  Modified:
        //
        //    06 October 2007
        //
        //  Author:
        //
        //    John Burkardt
        //
        //  Parameters:
        //
        //    Output, double R8_HUGE, a "huge" R8 value.
        //
        {
            double value;

            value = 1.0E+30;

            return value;
        }


        //****************************************************************************80

        void r8vec_print(int n, double[] a, string title)

        //****************************************************************************80
        //
        //  Purpose:
        //
        //    R8VEC_PRINT prints an R8VEC.
        //
        //  Discussion:
        //
        //    An R8VEC is a vector of R8's.
        //
        //  Licensing:
        //
        //    This code is distributed under the GNU LGPL license.
        //
        //  Modified:
        //
        //    16 August 2004
        //
        //  Author:
        //
        //    John Burkardt
        //
        //  Parameters:
        //
        //    Input, int N, the number of components of the vector.
        //
        //    Input, double A[N], the vector to be printed.
        //
        //    Input, string TITLE, a title.
        //
        {
            int i;


            //cout << "\n";
            //cout << title << "\n";
            //cout << "\n";
            //for ( i = 0; i < n; i++ )
            //{
            //cout << "  " << setw(8)  << i
            //     << "  " << setw(14) << a[i]  << "\n";
            //}

            return;
        }






    }
}
