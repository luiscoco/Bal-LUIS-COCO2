using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tablas_Vapor_ASMEBorder
{
    public partial class Region_Border : Form
    {
        public Region_Border()
        {
            InitializeComponent();
        }

        public Double B23p_T(Double T)
        {
          //Function B23p_T(ByVal T As Double) As Double
          //Release on the IAPWS Industrial Formulation 1997 for the Thermodynamic Properties of Water and Steam 1997
          //Section 4 Auxiliary Equation for the Boundary between Regions 2 and 3
          //Eq 5, Page 5
          return (348.05185628969 - 1.1671859879975 * T + 1.0192970039326E-03 * Math.Pow(T, 2));
        }

        public Double B23T_p(Double p)
        {
         //Function B23T_p(ByVal p As Double) As Double
         //Release on the IAPWS Industrial Formulation 1997 for the Thermodynamic Properties of Water and Steam 1997
         //Section 4 Auxiliary Equation for the Boundary between Regions 2 and 3
         //Eq 6, Page 6

            return (572.54459862746 + Math.Pow(((p - 13.91883977887) / 1.0192970039326E-03),0.5));
        }

        public Double p3sat_h(Double h)
        {
           //Function p3sat_h(ByVal h As Double) As Double
           //Revised Supplementary Release on Backward Equations for the Functions T(p,h), v(p,h) and T(p,s), v(p,s) for Region 3 of the IAPWS Industrial Formulation 1997 for the Thermodynamic Properties of Water and Steam 2004
           //Section 4 Boundary Equations psat(h) and psat(s) for the Saturation Lines of Region 3
           //Se pictures Page 17, Eq 10, Table 17, Page 18

            Double ps;
            
            Double[] Ii = new Double[] {0, 1, 1, 1, 1, 5, 7, 8, 14, 20, 22, 24, 28, 36};
            Double[] Ji =new Double[] {0, 1, 3, 4, 36, 3, 0, 24, 16, 16, 3, 18, 8, 24};
            Double[] ni = new Double[] { 0.600073641753024, -9.36203654849857, 24.6590798594147, -107.014222858224, -91582131580576.8, -8623.32011700662, -23.5837344740032, 2.52304969384128E+17, -3.89718771997719E+18, -3.33775713645296E+22, 35649946963.6328, -1.48547544720641E+26, 3.30611514838798E+18, 8.13641294467829E+37};
            
            h = h / 2600;
            ps = 0;

            for (int i = 0;i<=13;i++)
            {    
                ps = ps + ni[i] * Math.Pow((h - 1.02),Ii[i]) * Math.Pow((h - 0.608),Ji[i]);

            }

            return(ps * 22);
        }

        public Double p3sat_s(Double s)
        {
             //Function p3sat_s(ByVal s As Double) As Double

            Double sigma, p;

            Double[] Ii = new Double[] {0, 1, 1, 4, 12, 12, 16, 24, 28, 32};
            Double[] Ji =new Double[] {0, 1, 32, 7, 4, 14, 36, 10, 0, 18};
            Double[] ni = new Double[] { 0.639767553612785, -12.9727445396014, -2.24595125848403E+15, 1774667.41801846, 7170793495.71538, -3.78829107169011E+17, -9.55586736431328E+34, 1.87269814676188E+23, 119254746466.473, 1.10649277244882E+36};
            
            sigma = s / 5.2;
            p = 0;

            for(int i = 0;i<=9;i++)
            {

            p = p + ni[i] * Math.Pow((sigma - 1.03),Ii[i]) *Math.Pow((sigma - 0.699),Ji[i]);

            }

            return(p * 22);

        }

        public Double hB13_s(Double s)
        {
        
            //Function hB13_s(ByVal s As Double) As Double
            //Supplementary Release on Backward Equations ( ) , p h s for Region 3,
            //Chapter 4.5 page 23.

            Double sigma, eta;
  
            Double[] Ii = new Double[] {0, 1, 1, 3, 5, 6};
            Double[] Ji = new Double[] {0, -2, 2, -12, -4, -3};
            Double[] ni = new Double[] { 0.913965547600543, -4.30944856041991E-05, 60.3235694765419, 1.17518273082168E-18, 0.220000904781292, -69.0815545851641};
 
            sigma = s / 3.8;
            eta = 0;
  
            for(int i = 0;i<=5;i++)
            {
            eta = eta + ni[i] * Math.Pow((sigma - 0.884),Ii[i]) * Math.Pow((sigma - 0.864),Ji[i]);
            }
  
            return(eta * 1700);
        } 

       public Double TB23_hs(Double h, Double s)
       {
          //Function TB23_hs(ByVal h As Double, ByVal s As Double) As Double
          //Supplementary Release on Backward Equations ( ) , p h s for Region 3,
          //Chapter 4.6 page 25.

           Double sigma, eta, teta;
   
           Double[] Ii = new Double[] {-12, -10, -8, -4, -3, -2, -2, -2, -2, 0, 1, 1, 1, 3, 3, 5, 6, 6, 8, 8, 8, 12, 12, 14, 14};
           Double[] Ji = new Double[] {10, 8, 3, 4, 3, -6, 2, 3, 4, 0, -3, -2, 10, -2, -1, -5, -6, -3, -8, -2, -1, -12, -1, -12, 1};
           Double[] ni = new Double[] { 6.2909626082981E-04, -8.23453502583165E-04, 5.15446951519474E-08, -1.17565945784945, 3.48519684726192, -5.07837382408313E-12, -2.84637670005479, -2.36092263939673, 6.01492324973779, 1.48039650824546, 3.60075182221907E-04, -1.26700045009952E-02, -1221843.32521413, 0.149276502463272, 0.698733471798484, -2.52207040114321E-02, 1.47151930985213E-02, -1.08618917681849, -9.36875039816322E-04, 81.9877897570217, -182.041861521835, 2.61907376402688E-06, -29162.6417025961, 1.40660774926165E-05, 7832370.62349385 };
  
           sigma = s / 5.3;
           eta = h / 3000;
           teta = 0;

           for(int i = 0;i<=24;i++)
           {
               teta = teta + ni[i] * Math.Pow((eta - 0.727),Ii[i]) * Math.Pow((sigma - 0.864),Ji[i]);
           }

           return(teta * 900);

        }

       private void button1_Click(object sender, EventArgs e)
       {

       }
       




    }
}
