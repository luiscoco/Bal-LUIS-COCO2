C     Esta zona de código con la llamada a la función dassl.f no de de ser modificada ni enseñada al usuario 

      subroutine luis (NEQ,T,Y,YPRIME,TOUT,INFO,RTOL,ATOL,IDID,
     1     RWORK,LRW,IWORK,LIW,RPAR,IPAR,YT)

!GCC$ ATTRIBUTES DLLEXPORT :: LUIS

      IMPLICIT DOUBLE PRECISION(A-H,O-Z)
      EXTERNAL DRES,DYT
      INTEGER  NEQ, INFO(15), IDID, LRW, IWORK(*), LIW, IPAR(*)
      DOUBLE PRECISION T, Y(*),YT(*),YPRIME(*), TOUT, RTOL(*), ATOL(*),
     *  RWORK(*),RPAR(*)
      CALL DDASSL(DRES,NEQ,T,Y,YPRIME,TOUT,INFO,RTOL,ATOL,IDID,
     1  RWORK,LRW,IWORK,LIW,RPAR,IPAR,DJAC)

      CALL DYT(T,YT)

      END



C----------- Función DRES para definir el del DAE (Differential Algebraic Equations System) -------
      SUBROUTINE DRES(T,Y,YPRIME,DELTA,IRES,RPAR,IPAR)
      IMPLICIT DOUBLE PRECISION(A-H,O-Z)
       DIMENSION Y(2),YPRIME(1),DELTA(2)
       DELTA(1) = YPRIME(1) + 10.0D0*Y(1)
       DELTA(2) = Y(2) + Y(1) - 1.0D0

      RETURN
      END





C----------- Función Analítica del DAE (Differential Algebraic Equations System)-----
      SUBROUTINE DYT(T,YT)
      IMPLICIT DOUBLE PRECISION(A-H,O-Z)
      DIMENSION YT(2)
      YT(1) = Exp(-10.0 * T) 
      YT(2) = 1.0 - YT(1)

      RETURN
      END
