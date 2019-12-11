using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tablas_Vapor_ASME1;
using Tablas_Vapor_ASME2;
using Tablas_Vapor_ASME3;
using Tablas_Vapor_ASMEBorder;

namespace Tablas_Vapor_ASME4
{
    public partial class Region_4 : Form
    {
        public Region_4()
        {
            InitializeComponent();
            
        }
        
        public Double p4_T(Double T)
        {

            //Function p4_T(ByVal T As Double) As Double
            //Release on the IAPWS Industrial Formulation 1997 for the Thermodynamic Properties of Water and Steam, September 1997
            //Section 8.1 The Saturation-Pressure Equation
            //Eq 30, Page 33
            
            Double teta, a, b, c;

            
            teta = T - 0.23855557567849 / (T - 650.17534844798);

            a = Math.Pow(teta, 2) + 1167.0521452767 * teta - 724213.16703206;
            b = -17.073846940092 * Math.Pow(teta, 2) + 12020.82470247 * teta - 3232555.0322333;
            c = 14.91510861353 * Math.Pow(teta, 2) - 4823.2657361591 * teta + 405113.40542057;

            return(Math.Pow((2 * c / (-b + Math.Pow((Math.Pow(b, 2) - 4 * a * c), 0.5))), 4));

        
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Double T;
            T = 0;

            T = Convert.ToDouble(textBox1.Text);
            textBox3.Text = Convert.ToString(p4_T(T));

        }

        public Double T4_p(Double p)
        {
            //Function T4_p(ByVal p As Double) As Double
            //Release on the IAPWS Industrial Formulation 1997 for the Thermodynamic Properties of Water and Steam, September 1997
            //Section 8.2 The Saturation-Temperature Equation
            //Eq 31, Page 34Pow(           

            Double beta, e1, f, g, d;

            beta = Math.Pow(p, 0.25);

            e1 = Math.Pow(beta, 2) - 17.073846940092 * beta + 14.91510861353;
            f = 1167.0521452767 * Math.Pow(beta, 2) + 12020.82470247 * beta - 4823.2657361591;
            g = -724213.16703206 * Math.Pow(beta, 2) - 3232555.0322333 * beta + 405113.40542057;
            d = 2 * g / (-f - Math.Pow((Math.Pow(f, 2) - 4 * e1 * g), 0.5));

           return((650.17534844798 + d - Math.Pow((Math.Pow((650.17534844798 + d), 2) - 4 * (-0.23855557567849 + 650.17534844798 * d)), 0.5)) / 2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Double p;
            p=0;

            p= Convert.ToDouble(textBox6.Text);
            textBox4.Text = Convert.ToString(T4_p(p));
        }


        public Double h4_s(Double s)
        {

            //Function h4_s(ByVal s As Double) As Double
            //Supplementary Release on Backward Equations ( ) , p h s for Region 3,Equations as a Function of h and s for the Region Boundaries, and an Equation( ) sat , T hs for Region 4 of the IAPWS Industrial Formulation 1997 for the Thermodynamic Properties of Water and Steam
            //4 Equations for Region Boundaries Given Enthalpy and Entropy
            //Se picture page 14

           
            Double eta, sigma, sigma1, sigma2;


            if ((s > -0.0001545495919) & (s <= 3.77828134))
            {
                //hL1_s
                //Eq 3,Table 9,Page 16

                Double[] Ii1 = new Double[] { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 4, 5, 5, 7, 8, 12, 12, 14, 14, 16, 20, 20, 22, 24, 28, 32, 32 };
                Double[] Ji1 = new Double[] { 14, 36, 3, 16, 0, 5, 4, 36, 4, 16, 24, 18, 24, 1, 4, 2, 4, 1, 22, 10, 12, 28, 8, 3, 0, 6, 8 };
                Double[] ni1 = new Double[] { 0.332171191705237, 6.11217706323496E-04, -8.82092478906822, -0.45562819254325, -2.63483840850452E-05, -22.3949661148062, -4.28398660164013, -0.616679338856916, -14.682303110404, 284.523138727299, -113.398503195444, 1156.71380760859, 395.551267359325, -1.54891257229285, 19.4486637751291, -3.57915139457043, -3.35369414148819, -0.66442679633246, 32332.1885383934, 3317.66744667084, -22350.1257931087, 5739538.75852936, 173.226193407919, -3.63968822121321E-02, 8.34596332878346E-07, 5.03611916682674, 65.5444787064505 };

                sigma = s / 3.8;

                eta = 0;

                for (int i = 0; i <= 26; i++)
                {
                    eta = eta + ni1[i] * Math.Pow((sigma - 1.09), Ii1[i]) * Math.Pow((sigma + 0.0000366), Ji1[i]);
                }

               return(eta * 1700);

            }

            else if ((s > 3.77828134) & (s <= 4.41202148223476))
            {

                //hL3_s
                //Eq 4,Table 10,Page 16

                Double[] Ii2 = new Double[] { 0, 0, 0, 0, 2, 3, 4, 4, 5, 5, 6, 7, 7, 7, 10, 10, 10, 32, 32 };
                Double[] Ji2 = new Double[] { 1, 4, 10, 16, 1, 36, 3, 16, 20, 36, 4, 2, 28, 32, 14, 32, 36, 0, 6 };
                Double[] ni2 = new Double[] { 0.822673364673336, 0.181977213534479, -0.011200026031362, -7.46778287048033E-04, -0.179046263257381, 4.24220110836657E-02, -0.341355823438768, -2.09881740853565, -8.22477343323596, -4.99684082076008, 0.191413958471069, 5.81062241093136E-02, -1655.05498701029, 1588.70443421201, -85.0623535172818, -31771.4386511207, -94589.0406632871, -1.3927384708869E-06, 0.63105253224098 };

                sigma = s / 3.8;

                eta = 0;

                for (int i = 0; i <= 18; i++)
                {

                    eta = eta + ni2[i] * Math.Pow((sigma - 1.09), Ii2[i]) * Math.Pow((sigma + 0.0000366), Ji2[i]);

                }

                return(eta * 1700);

            }

            else if ((s > 4.41202148223476) & (s <= 5.85))
            {

                //Section 4.4 Equations ( ) 2ab " h s and ( ) 2c3b "h s for the Saturated Vapor Line
                //Page 19, Eq 5
                //V2c3b_s(s)

                Double[] Ii3 = new Double[] { 0, 0, 0, 1, 1, 5, 6, 7, 8, 8, 12, 16, 22, 22, 24, 36 };
                Double[] Ji3 = new Double[] { 0, 3, 4, 0, 12, 36, 12, 16, 2, 20, 32, 36, 2, 32, 7, 20 };
                Double[] ni3 = new Double[] { 1.04351280732769, -2.27807912708513, 1.80535256723202, 0.420440834792042, -105721.24483466, 4.36911607493884E+24, -328032702839.753, -6.7868676080427E+15, 7439.57464645363, -3.56896445355761E+19, 1.67590585186801E+31, -3.55028625419105E+37, 396611982166.538, -4.14716268484468E+40, 3.59080103867382E+18, -1.16994334851995E+40 };

                sigma = s / 5.9;

                eta = 0;

                for (int i = 0; i <= 15; i++)
                {
                    eta = eta + ni3[i] * Math.Pow((sigma - 1.02), Ii3[i]) * Math.Pow((sigma - 0.726), Ji3[i]);
                }

                return(Math.Pow(eta, 4) * 2800);

            }

            else if ((s > 5.85) & (s < 9.155759395))
            {
                //Section 4.4 Equations ( ) 2ab " h s and ( ) 2c3b "h s for the Saturated Vapor Line
                //Page 20, Eq 6

                Double[] Ii4 = new Double[] { 1, 1, 2, 2, 4, 4, 7, 8, 8, 10, 12, 12, 18, 20, 24, 28, 28, 28, 28, 28, 32, 32, 32, 32, 32, 36, 36, 36, 36, 36 };
                Double[] Ji4 = new Double[] { 8, 24, 4, 32, 1, 2, 7, 5, 12, 1, 0, 7, 10, 12, 32, 8, 12, 20, 22, 24, 2, 7, 12, 14, 24, 10, 12, 20, 22, 28 };
                Double[] ni4 = new Double[] { -524.581170928788, -9269472.18142218, -237.385107491666, 21077015581.2776, -23.9494562010986, 221.802480294197, -5104725.33393438, 1249813.96109147, 2000084369.96201, -815.158509791035, -157.612685637523, -11420042233.2791, 6.62364680776872E+15, -2.27622818296144E+18, -1.71048081348406E+31, 6.60788766938091E+15, 1.66320055886021E+22, -2.18003784381501E+29, -7.87276140295618E+29, 1.51062329700346E+31, 7957321.70300541, 1.31957647355347E+15, -3.2509706829914E+23, -4.18600611419248E+25, 2.97478906557467E+34, -9.53588761745473E+19, 1.66957699620939E+24, -1.75407764869978E+32, 3.47581490626396E+34, -7.10971318427851E+38 };

                sigma1 = s / 5.21;

                sigma2 = s / 9.2;

                eta = 0;

                for (int i = 0; i <= 29; i++)
                {

                    eta = eta + ni4[i] * Math.Pow((1 / sigma1 - 0.513), Ii4[i]) * Math.Pow((sigma2 - 0.524), Ji4[i]);

                }

                return(Math.Exp(eta) * 2800);
            }

            else
            {
                return (0);
                //textBox9.Text  = CVErr(xlErrValue);

            }

        }

        private void button3_Click(object sender, EventArgs e)
        {

            Double s;
            s=0;

            s = Convert.ToDouble(textBox7.Text);

            textBox9.Text = Convert.ToString(h4_s(s));
           
        }

        public Double p4_s(Double s)
        {
            //Function p4_s(ByVal s As Double) As Double
            //Uses h4_s and p_hs for the diffrent regions to determine p4_s
            Double hsat;
            Double p4_s;
            p4_s=0;

            Region1 luis1 = new Region1();
            Region2 luis2 = new Region2();
            Region_3 luis3 = new Region_3();
            Region_Border luisborder = new Region_Border();

            hsat = h4_s(s);
 
            if ((s > -0.0001545495919) & (s <= 3.77828134))
            {          
                    p4_s = luis1.p1_hs(hsat, s);    
                    return(p4_s);
                    
            }
            else if ((s > 3.77828134) & (s <= 5.210887663))
            {        
                   //IMPORTANTE PENDIENTE !!!!

                MessageBox.Show("HOLA mensaje LUIS COCO: Error Falta la Función entre s>3.77828134 y s <= 5.210887663");
                   // p4_s = luis3.p3_hs(hsat, s);
                  
                return(0);
            } 
            else if ((s > 5.210887663) & (s < 9.155759395))
            {
                    p4_s = luis2.p2_hs(hsat, s);
                    return(p4_s);
            }
            else
            {       
                    //p4_s = CVErr(xlErrValue);
                MessageBox.Show("HOLA mensaje LUIS COCO: Error Falta la Función entre s>9.155759395");
                    return(0);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Double s;
            s=0;

            s = Convert.ToDouble(textBox2.Text);
            textBox5.Text = Convert.ToString(p4_s(s));

        }

        public Double h4L_p(Double p)
        {
            //Function h4L_p(ByVal p As Double) As Double

            Double Low_Bound, High_Bound, hs, ps, Ts;
            Region1 luis1=new Region1();
            Region2 luis2=new Region2();
            Region_3 luis3=new Region_3();
            Region_Border luisborder = new Region_Border();

            Double h4L_p;
            h4L_p=0;


            if ((p > 0.000611657) & (p < 22.06395))
            {
                Ts = T4_p(p);

                if (p < 16.529)
                {
                    h4L_p = luis1.h1_pT(p, Ts);
                    return(h4L_p);
                }

                else
                {
                    //Iterate to find the the backward solution of p3sat_h
                    Low_Bound = 1670.858218;
                    High_Bound = 2087.23500164864;

                    do
                    {
                        hs = (Low_Bound + High_Bound) / 2;

                        ps = luisborder.p3sat_h(hs);
                        
                        if (ps > p)
                        {
                            High_Bound = hs;
                        }

                        else
                        {
                            Low_Bound = hs;
                        }

                    }
                    while (Math.Abs(p - ps) > 0.00001);

                    h4L_p = hs;

                    return (h4L_p);

                }

            }
            else
            {
                //h4L_p = CVErr(xlErrValue);
                MessageBox.Show("HOla LUIS error fuera de rango");
                return (0);
            }
        }

        public Double h4V_p(Double p)
        {        
           //Function h4V_p(ByVal p As Double) As Double
            Double Low_Bound, High_Bound, hs, ps, Ts;
            Region2 luis2=new Region2();
            Region_3 luis3=new Region_3();
            Region_Border luisb=new Region_Border();
            Region_4 luis4=new Region_4();
            Double h4V_p;
            h4V_p=0;

            if ((p > 0.000611657) & (p < 22.06395))
            {
               Ts = luis4.T4_p(p);
               if (p < 16.529)
               {
               h4V_p = luis2.h2_pT(p, Ts);
               return(h4V_p);
               }
               else
               {
                //Iterate to find the the backward solution of p3sat_h
                Low_Bound = 2087.23500164864;
                High_Bound = 2563.592004 + 5;  //5 added to extrapolate to ensure even the border ==350°C solved.
  
                do
                {
                  hs = (Low_Bound + High_Bound) / 2;
                  ps = luisb.p3sat_h(hs);
                  if (ps < p)
                  { 
                     High_Bound = hs;
                  }
                  else
                  {
                     Low_Bound = hs;
                  }
                }
                while(Math.Abs(p - ps) > 0.000001);

               h4V_p = hs;
               return(h4V_p);
  
               }
               
            }
                else

                {
                   // h4V_p = CVErr(xlErrValue)
                    MessageBox.Show("HOla LUIS error fuera de rango");
                    return (0);
                }

        }

      

        public Double x4_ph(Double p, Double h)
        {
 
          //Function x4_ph(ByVal p As Double, ByVal h As Double) As Double
          //Calculate vapour fraction from hL and hV for given p
 
            Double h4v, h4l;
            Double  x4_ph;

            h4v = h4V_p(p);
            h4l = h4L_p(p);

            if(h > h4v)
            {
            x4_ph = 1;
            return(x4_ph);
            } 

            else if (h < h4l)
            {    
            x4_ph = 0;
            return(x4_ph);
            }

            else
            {
            x4_ph = (h - h4l) / (h4v - h4l);
            return(x4_ph);
            }
        }

        public Double x4_ps(Double p, Double s)
        {

            //Function x4_ps(ByVal p As Double, ByVal s As Double) As Double
              Double x4_ps;
              x4_ps=0;
             
              Double ssV, ssL;
              Region1 luis1=new Region1();
              Region2 luis2=new Region2();
              Region_3 luis3=new Region_3();

              if (p < 16.529)
              {
                 ssV = luis2.s2_pT(p, T4_p(p));
                 ssL = luis1.s1_pT(p, T4_p(p));
              }
              else
              { 
                 ssV = luis3.s3_rhoT(1 / (luis3.v3_ph(p, h4V_p(p))), T4_p(p));
                 ssL = luis3.s3_rhoT(1 / (luis3.v3_ph(p, h4L_p(p))), T4_p(p));
              }

              if (s < ssL)
              {
                 x4_ps = 0;
                 return(x4_ps);
              }

              else if (s > ssV) 
              {
                 x4_ps = 1;
                 return(x4_ps);
              }

              else
              {
                 x4_ps = (s - ssL) / (ssV - ssL);
                 return(x4_ps);
              }

        } 

        public Double T4_hs(Double h,Double s)
        {

          //Function T4_hs(ByVal h As Double, ByVal s As Double) As Double
          //Supplementary Release on Backward Equations ( ) , p h s for Region 3,
          //Chapter 5.3 page 30.
          //The if 97 function is only valid for part of region4. Use iteration outsida.

           Double hL, Ts, ss, p, sigma, eta, teta, High_Bound, Low_Bound, PL, s4V, v4V, s4L, v4L, xs;
           //MUY IMPORTANTE POSIBLE ERROR EN INICIALIZAR LA PL =0
            PL=0;
           Double T4_hs;
           T4_hs=0;

           Region1 luis1 = new Region1();
           Region2 luis2 = new Region2();
           Region_3 luis3 = new Region_3();
           Region_Border luisb = new Region_Border();

           Double[] Ii = new Double[] {0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 3, 3, 3, 3, 4, 4, 5, 5, 5, 5, 6, 6, 6, 8, 10, 10, 12, 14, 14, 16, 16, 18, 18, 18, 20, 28};
           Double[] Ji =new Double[] {0, 3, 12, 0, 1, 2, 5, 0, 5, 8, 0, 2, 3, 4, 0, 1, 1, 2, 4, 16, 6, 8, 22, 1, 20, 36, 24, 1, 28, 12, 32, 14, 22, 36, 24, 36};
           Double[] ni = new Double[] {0.179882673606601, -0.267507455199603, 1.162767226126, 0.147545428713616, -0.512871635973248, 0.421333567697984, 0.56374952218987, 0.429274443819153, -3.3570455214214, 10.8890916499278, -0.248483390456012, 0.30415322190639, -0.494819763939905, 1.07551674933261, 7.33888415457688E-02, 1.40170545411085E-02, -0.106110975998808, 1.68324361811875E-02, 1.25028363714877, 1013.16840309509, -1.51791558000712, 52.4277865990866, 23049.5545563912, 2.49459806365456E-02, 2107964.67412137, 366836848.613065, -144814105.365163, -1.7927637300359E-03, 4899556021.00459, 471.262212070518, -82929439019.8652, -1715.45662263191, 3557776.82973575, 586062760258.436, -12988763.5078195, 31724744937.1057};
 
           if ((s > 5.210887825) & (s < 9.15546555571324))
           {
             sigma = s / 9.2;
             eta = h / 2800;
             teta = 0;
 
             for(int i = 0;i<=35;i++)
             {
             teta = teta + ni[i] * Math.Pow((eta - 0.119),Ii[i])* Math.Pow((sigma - 1.07),Ji[i]);
             }
                 
             T4_hs = teta * 550;
             return(T4_hs);
           }

           else
           {
             //Function psat_h
  
             if ((s > -0.0001545495919) & (s <= 3.77828134))
             {
               Low_Bound = 0.000611;
               High_Bound = 165.291642526045;

               do 
               {
                 PL = (Low_Bound + High_Bound) / 2;
                 Ts = T4_p(PL);
                 hL = luis1.h1_pT(PL, Ts);
                 if (hL > h)
                 {
                   High_Bound = PL;
                 }
                 else
                 { 
                   Low_Bound = PL;
                 }
               } 
               while((Math.Abs(hL - h) > 0.00001) & (Math.Abs(High_Bound - Low_Bound) > 0.0001));

              } 

              else if((s > 3.77828134) & (s <= 4.41202148223476))
              { 
                PL = luisb.p3sat_h(h);
              }

              else if ((s > 4.41202148223476) & (s<= 5.210887663))
              {
                PL = luisb.p3sat_h(h);
              }

                Low_Bound = 0.000611;
                //MUY IMPORTANTE posible error.Comprobar el valor de PL inicializada a 0
                High_Bound = PL;
    
    do 
      {  
        
      p = (Low_Bound + High_Bound) / 2;
      
      //Calculate s4_ph
      Ts = T4_p(p);
      xs = x4_ph(p, h);

      if (p < 16.529)
      {  
        s4V = luis2.s2_pT(p, Ts);
        s4L = luis1.s1_pT(p, Ts);
      }

      else
      {
        v4V = luis3.v3_ph(p, h4V_p(p));
        s4V = luis3.s3_rhoT(1 / v4V, Ts);
        v4L = luis3.v3_ph(p, h4L_p(p));
        s4L = luis3.s3_rhoT(1 / v4L, Ts);
      }

      ss = (xs * s4V + (1 - xs) * s4L);
      
      if (ss < s)
      {
        High_Bound = p;
      }

      else
      {
        Low_Bound = p;
      }
      
    }

    while ((Math.Abs(s - ss) > 0.000001) & (Math.Abs(High_Bound - Low_Bound) > 0.0000001));

    T4_hs = T4_p(p);
    return(T4_hs);

           }

 }




    }

}

    