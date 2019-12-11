using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsFormsApplication2;

namespace MINPACK
{
    /// <summary>
    /// For further information view the MinPack documentation.
    /// </summary>
    public class Hybrid
    {
public int hybrd (Motorcalculo f1,
  int n, double[] x,
  double[] fvec, double xtol, int maxfev, int ml, int mu, double epsfcn,
  double[] diag, int mode, double factor, int nprint, ref int nfev,
  double[] fjac, int ldfjac, double[] r, int lr, double[] qtf, double[] wa1,
  double[] wa2, double[] wa3, double[] wa4 )

//****************************************************************************80
//
//  Purpose:
//
//    HYBRD finds a zero of a system of N nonlinear equations.
//
//  Discussion:
//
//    The purpose of hybrd is to find a zero of a system of
//    n nonlinear functions in n variables by a modification
//    of the powell hybrid method. the user must provide a
//    subroutine which calculates the functions. the jacobian is
//    then calculated by a forward-difference approximation.
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
//         the user wants to terminate execution of hybrd.
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
//       xtol is a nonnegative input variable. termination
//         occurs when the relative error between two consecutive
//         iterates is at most xtol.
//
//       maxfev is a positive integer input variable. termination
//         occurs when the number of calls to fcn is at least maxfev
//         by the end of an iteration.
//
//       ml is a nonnegative integer input variable which specifies
//         the number of subdiagonals within the band of the
//         jacobian matrix. if the jacobian is not banded, set
//         ml to at least n - 1.
//
//       mu is a nonnegative integer input variable which specifies
//         the number of superdiagonals within the band of the
//         jacobian matrix. if the jacobian is not banded, set
//         mu to at least n - 1.
//
//       epsfcn is an input variable used in determining a suitable
//         step length for the forward-difference approximation. this
//         approximation assumes that the relative errors in the
//         functions are of the order of epsfcn. if epsfcn is less
//         than the machine precision, it is assumed that the relative
//         errors in the functions are of the order of the machine
//         precision.
//
//       diag is an array of length n. if mode = 1 (see
//         below), diag is internally set. if mode = 2, diag
//         must contain positive entries that serve as
//         multiplicative scale factors for the variables.
//
//       mode is an integer input variable. if mode = 1, the
//         variables will be scaled internally. if mode = 2,
//         the scaling is specified by the input diag. other
//         values of mode are equivalent to mode = 1.
//
//       factor is a positive input variable used in determining the
//         initial step bound. this bound is set to the product of
//         factor and the euclidean norm of diag*x if nonzero, or else
//         to factor itself. in most cases factor should lie in the
//         interval (.1,100.). 100. is a generally recommended value.
//
//       nprint is an integer input variable that enables controlled
//         printing of iterates if it is positive. in this case,
//         fcn is called with iflag = 0 at the beginning of the first
//         iteration and every nprint iterations thereafter and
//         immediately prior to return, with x and fvec available
//         for printing. if nprint is not positive, no special calls
//         of fcn with iflag = 0 are made.
//
//       info is an integer output variable. if the user has
//         terminated execution, info is set to the (negative)
//         value of iflag. see description of fcn. otherwise,
//         info is set as follows.
//
//         info = 0   improper input parameters.
//
//         info = 1   relative error between two consecutive iterates
//                    is at most xtol.
//
//         info = 2   number of calls to fcn has reached or exceeded
//                    maxfev.
//
//         info = 3   xtol is too small. no further improvement in
//                    the approximate solution x is possible.
//
//         info = 4   iteration is not making good progress, as
//                    measured by the improvement from the last
//                    five jacobian evaluations.
//
//         info = 5   iteration is not making good progress, as
//                    measured by the improvement from the last
//                    ten iterations.
//
//       nfev is an integer output variable set to the number of
//         calls to fcn.
//
//       fjac is an output n by n array which contains the
//         orthogonal matrix q produced by the qr factorization
//         of the final approximate jacobian.
//
//       ldfjac is a positive integer input variable not less than n
//         which specifies the leading dimension of the array fjac.
//
//       r is an output array of length lr which contains the
//         upper triangular matrix produced by the qr factorization
//         of the final approximate jacobian, stored rowwise.
//
//       lr is a positive integer input variable not less than
//         (n*(n+1))/2.
//
//       qtf is an output array of length n which contains
//         the vector (q transpose)*fvec.
//
//       wa1, wa2, wa3, and wa4 are work arrays of length n.
//
{
  double actred;
  double delta=0;
  double epsmch;
  double fnorm;
  double fnorm1;
  int i;
  int iflag;
  int info;
  int iter;
  int[] iwa = new int[n];
  int j;
  bool jeval;
  int jm1;
  int l;
  int msum;
  int ncfail;
  int ncsuc;
  int nslow1;
  int nslow2;
  double p001 = 0.001;
  double p0001 = 0.0001;
  double p1 = 0.1;
  double p5 = 0.5;
  double pnorm;
  double prered;
  double ratio;
  bool sing;
  double sum;
  double temp;
  double xnorm=0;
  MINPACK.Auxiliares auxiliares=new Auxiliares();
  MINPACK.EnormClass norma=new EnormClass();
  MINPACK.r1mpyq r1mpyqinstancia=new r1mpyq();
  MINPACK.qrfac qrfacinstancia=new qrfac();
  MINPACK.fdjac1 fdjac1instancia = new fdjac1();
  MINPACK.dogleg dogleginstancia = new dogleg();
  MINPACK.r1updt r1updtinstancia = new r1updt();
  MINPACK.qform qforminstancia = new qform();
  
//
//  Certain loops in this function were kept closer to their original FORTRAN77
//  format, to avoid confusing issues with the array index L.  These loops are
//  marked "DO NOT ADJUST", although they certainly could be adjusted (carefully)
//  once the initial translated code is tested.
//

//
//  EPSMCH is the machine precision.
//
  epsmch = auxiliares.r8_epsilon ( );

  info = 0;
  iflag = 0;
  nfev = 0;
//
//  Check the input parameters.
//
  if ( n <= 0 )
  {
    info = 0;
    return info;
  }
  if ( xtol < 0.0 )
  {
    info = 0;
    return info;
  }
  if ( maxfev <= 0 )
  {
    info = 0;
    return info;
  }
  if ( ml < 0 )
  {
    info = 0;
    return info;
  }
  if ( mu < 0 )
  {
    info = 0;
    return info;
  }
  if ( factor <= 0.0 )
  {
    info = 0;
    return info;
  }
  if ( ldfjac < n )
  {
    info = 0;
    return info;
  }
  if ( lr < ( n * ( n + 1 ) ) / 2 )
  {
    info = 0;
    return info;
  }
  if ( mode == 2 )
  {
    for ( j = 0; j < n; j++ )
    {
      if ( diag[j] <= 0.0 )
      {
        info = 0;
        return info;
      }
    }
  }
//
//  Evaluate the function at the starting point and calculate its norm.
//
  iflag = 1;
  f1.f03( n, x, fvec,iflag );
  nfev = 1;
  if ( iflag < 0 )
  {
    info = iflag;
    return info;
  }

  fnorm = norma.Enorm ( n, fvec );
//
//  Determine the number of calls to FCN needed to compute the jacobian matrix.
//
  msum = auxiliares.i4_min ( ml + mu + 1, n );
//
//  Initialize iteration counter and monitors.
//
  iter = 1;
  ncsuc = 0;
  ncfail = 0;
  nslow1 = 0;
  nslow2 = 0;
//
//  Beginning of the outer loop.
//
  for ( ; ; )
  {
    jeval = true;
//
//  Calculate the jacobian matrix.
//
    iflag = 2;
    fdjac1instancia.fdjac1arun(f1, n, x, fvec, fjac, ldfjac, iflag, ml, mu, epsfcn, wa1, wa2);

    nfev = nfev + msum;
    if ( iflag < 0 )
    {
      info = iflag;
      return info;
    }
//
//  Compute the QR factorization of the jacobian.
//
    qrfacinstancia.qrfacrun(n, n, fjac, ldfjac, false, iwa, 1, wa1, wa2, wa3);
//
//  On the first iteration and if MODE is 1, scale according
//  to the norms of the columns of the initial jacobian.
//
    if ( iter == 1 )
    {
      if ( mode == 1 )
      {
        for ( j = 0; j < n; j++ )
        {
          if ( wa2[j] != 0.0 )
          {
            diag[j] = wa2[j];
          }
          else
          {
            diag[j] = 1.0;
          }
        }
      }
//
//  On the first iteration, calculate the norm of the scaled X
//  and initialize the step bound DELTA.
//
      for ( j = 0; j < n; j++ )
      {
        wa3[j] = diag[j] * x[j];
      }
      xnorm = norma.Enorm ( n, wa3 );

      if ( xnorm == 0.0 )
      {
        delta = factor;
      }
      else
      {
        delta = factor * xnorm;
      }
    }
//
//  Form Q' * FVEC and store in QTF.
//
    for ( i = 0; i < n; i++ )
    {
      qtf[i] = fvec[i];
    }
    for ( j = 0; j < n; j++ )
    {
      if ( fjac[j+j*ldfjac] != 0.0 )
      {
        sum = 0.0;
        for ( i = j; i < n; i++ )
        {
          sum = sum + fjac[i+j*ldfjac] * qtf[i];
        }
        temp = - sum / fjac[j+j*ldfjac];
        for ( i = j; i < n; i++ )
        {
          qtf[i] = qtf[i] + fjac[i+j*ldfjac] * temp;
        }
      }
    }
//
//  Copy the triangular factor of the QR factorization into R.
//
//  DO NOT ADJUST THIS LOOP, BECAUSE OF L.
//
    sing = false;
    for ( j = 1; j <= n; j++ )
    {
      l = j;
      for ( i = 1; i <= j - 1; i++ )
      {
        r[l-1] = fjac[(i-1)+(j-1)*ldfjac];
        l = l + n - i;
      }
      r[l-1] = wa1[j-1];
      if ( wa1[j-1] == 0.0 )
      {
        sing = true;
      }
    }
//
//  Accumulate the orthogonal factor in FJAC.
//
    qforminstancia.qformrun(n, n, fjac, ldfjac, wa1);
//
//  Rescale if necessary.
//
    if ( mode == 1 )
    {
      for ( j = 0; j < n; j++ )
      {
        diag[j] = auxiliares.r8_max ( diag[j], wa2[j] );
      }
    }
//
//  Beginning of the inner loop.
//
    for ( ; ; )
    {
//
//  If requested, call FCN to enable printing of iterates.
//
      if ( 0 < nprint )
      {
        if ( ( iter - 1 ) % nprint == 0 )
        {
          iflag = 0;
          f1.f03(n,x,fvec,iflag);
          if ( iflag < 0 )
          {
            info = iflag;
            return info;
          }
        }
      }
//
//  Determine the direction P.
//
      dogleginstancia.doglegrun(n, r, lr, diag, qtf, delta, wa1, wa2, wa3);
//
//  Store the direction P and X + P.  Calculate the norm of P.
//
      for ( j = 0; j < n; j++ )
      {
        wa1[j] = - wa1[j];
        wa2[j] = x[j] + wa1[j];
        wa3[j] = diag[j] * wa1[j];
      }
      pnorm = norma.Enorm ( n, wa3 );
//
//  On the first iteration, adjust the initial step bound.
//
      if ( iter == 1 )
      {
        delta = auxiliares.r8_min ( delta, pnorm );
      }
//
//  Evaluate the function at X + P and calculate its norm.
//
      iflag = 1;
      f1.f03(n,wa2,wa4,iflag);
      nfev = nfev + 1;
      if ( iflag < 0 )
      {
        info = iflag;
        return info;
      }
      fnorm1 = norma.Enorm ( n, wa4 );
//
//  Compute the scaled actual reduction.
//
      if ( fnorm1 < fnorm )
      {
        actred = 1.0 - ( fnorm1 / fnorm ) * ( fnorm1 / fnorm );
      }
      else
      {
        actred = - 1.0;
      }
//
//  Compute the scaled predicted reduction.
//
//  DO NOT ADJUST THIS LOOP, BECAUSE OF L.
//
      l = 1;
      for ( i = 1; i <= n; i++ )
      {
        sum = 0.0;
        for ( j = i; j <= n; j++ )
        {
          sum = sum + r[l-1] * wa1[j-1];
          l = l + 1;
        }
        wa3[i-1] = qtf[i-1] + sum;
      }
      temp = norma.Enorm ( n, wa3 );

      if ( temp < fnorm )
      {
        prered = 1.0 - ( temp / fnorm ) * ( temp / fnorm );
      }
      else
      {
        prered = 0.0;
      }
//
//  Compute the ratio of the actual to the predicted reduction.
//
      if ( 0.0 < prered )
      {
        ratio = actred / prered;
      }
      else
      {
        ratio = 0.0;
      }
//
//  Update the step bound.
//
      if ( ratio < p1 )
      {
        ncsuc = 0;
        ncfail = ncfail + 1;
        delta = p5 * delta;
      }
      else
      {
        ncfail = 0;
        ncsuc = ncsuc + 1;
        if ( p5 <= ratio || 1 < ncsuc )
        {
          delta = auxiliares.r8_max ( delta, pnorm / p5 );
        }
        if ( auxiliares.r8_abs ( ratio - 1.0 ) <= p1 )
        {
          delta = pnorm / p5;
        }
      }
//
//  On successful iteration, update X, FVEC, and their norms.
//
      if ( p0001 <= ratio )
      {
        for ( j = 0; j < n; j++ )
        {
          x[j] = wa2[j];
          wa2[j] = diag[j] * x[j];
          fvec[j] = wa4[j];
        }
        xnorm = norma.Enorm ( n, wa2 );
        fnorm = fnorm1;
        iter = iter + 1;
      }
//
//  Determine the progress of the iteration.
//
      nslow1 = nslow1 + 1;
      if ( p001 <= actred )
      {
        nslow1 = 0;
      }
      if ( jeval )
      {
        nslow2 = nslow2 + 1;
      }
      if ( p1 <= actred )
      {
        nslow2 = 0;
      }
//
//  Test for convergence.
//
      if ( delta <= xtol * xnorm || fnorm == 0.0 )
      {
        info = 1;
        return info;
      }
//
//  Tests for termination and stringent tolerances.
//
      if ( maxfev <= nfev )
      {
        info = 2;
        return info;
      }
      if ( p1 * auxiliares.r8_max ( p1 * delta, pnorm ) <= epsmch * xnorm )
      {
        info = 3;
        return info;
      }
      if ( nslow2 == 5 )
      {
        info = 4;
        return info;
      }
      if ( nslow1 == 10 )
      {
        info = 5;
        return info;
      }
//
//  Criterion for recalculating jacobian approximation by forward differences.
//
      if ( ncfail == 2 )
      {
        break;
      }
//
//  Calculate the rank one modification to the jacobian
//  and update QTF if necessary.
//
      for ( j = 0; j < n; j++ )
      {
        sum = 0.0;
        for ( i = 0; i < n; i++ )
        {
          sum = sum + fjac[i+j*ldfjac] * wa4[i];
        }
        wa2[j] = ( sum - wa3[j] ) / pnorm;
        wa1[j] = diag[j] * ( ( diag[j] * wa1[j] ) / pnorm );
        if ( p0001 <= ratio )
        {
          qtf[j] = sum;
        }
      }
//
//  Compute the QR factorization of the updated jacobian.
//
      sing = r1updtinstancia.r1updtrun(n, n, r, lr, wa1, wa2, wa3);
      r1mpyqinstancia.r1mpyqrun(n, n, fjac, ldfjac, wa2, wa3);
      r1mpyqinstancia.r1mpyqrun(1, n, qtf, 1, wa2, wa3);

      jeval = false;
    }
//
//  End of the inner loop.
//
  }
//
//  End of the outer loop.
//
}

    }


    

}