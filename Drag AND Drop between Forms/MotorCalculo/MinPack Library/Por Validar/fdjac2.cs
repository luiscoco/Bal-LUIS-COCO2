using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsFormsApplication2;

namespace MINPACK
{
    public class fdjac2
    {
        //****************************************************************************80

        void fdjac2run(Motorcalculo f1,
  int m, int n, double[] x, double[] fvec, double[] fjac, int ldfjac,
  int iflag, double epsfcn, double[] wa )

//****************************************************************************80
//
//  Purpose:
//
//    FDJAC2 estimates an M by N Jacobian matrix using forward differences.
//
//  Discussion:
//
//    This subroutine computes a forward-difference approximation
//    to the m by n jacobian matrix associated with a specified
//    problem of m functions in n variables.
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
//         subroutine fcn(m,n,x,fvec,iflag)
//         integer m,n,iflag
//         double precision x(n),fvec(m)
//         ----------
//         calculate the functions at x and
//         return this vector in fvec.
//         ----------
//         return
//         end
//
//         the value of iflag should not be changed by fcn unless
//         the user wants to terminate execution of fdjac2.
//         in this case set iflag to a negative integer.
//
//       m is a positive integer input variable set to the number
//         of functions.
//
//       n is a positive integer input variable set to the number
//         of variables. n must not exceed m.
//
//       x is an input array of length n.
//
//       fvec is an input array of length m which must contain the
//         functions evaluated at x.
//
//       fjac is an output m by n array which contains the
//         approximation to the jacobian matrix evaluated at x.
//
//       ldfjac is a positive integer input variable not less than m
//         which specifies the leading dimension of the array fjac.
//
//       iflag is an integer variable which can be used to terminate
//         the execution of fdjac2. see description of fcn.
//
//       epsfcn is an input variable used in determining a suitable
//         step length for the forward-difference approximation. this
//         approximation assumes that the relative errors in the
//         functions are of the order of epsfcn. if epsfcn is less
//         than the machine precision, it is assumed that the relative
//         errors in the functions are of the order of the machine
//         precision.
//
//       wa is a work array of length m.
//
{
  double eps;
  double epsmch;
  double h;
  int i;
  int j;
  double temp;

  Auxiliares aux = new Auxiliares();
  
//
//  EPSMCH is the machine precision.
//
  epsmch = aux.r8_epsilon ( );
  eps = Math.Sqrt ( aux.r8_max ( epsfcn, epsmch ) );

  for ( j = 0; j < n; j++ )
  {
    temp = x[j];
    if ( temp == 0.0 )
    {
      h = eps;
    }
    else
    {
      h = eps * aux.r8_abs ( temp );
    }
    x[j] = temp + h;
    

    //ERROR PENDIENTE DE SOLUCIONAR en este caso enviamos tanto M como N y no solo N como en el caso de fdjac1
    //f1.f03( m, n, x, wa, iflag );

    if ( iflag < 0 )
    {
      break;
    }
    x[j] = temp;
    for ( i = 0; i < m; i++ )
    {
      fjac[i+j*ldfjac] = ( wa[i] - fvec[i] ) / h;
    }
  }
  return;
}
//

    }
}
