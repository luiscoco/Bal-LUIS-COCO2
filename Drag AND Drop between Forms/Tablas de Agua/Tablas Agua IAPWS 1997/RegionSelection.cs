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
using Tablas_Vapor_ASME4;
using Tablas_Vapor_ASME5;
using Tablas_Vapor_ASMEBorder;



namespace Tablas_Vapor_ASME
{
    public partial class RegionSelection : Form
    {
        const double MARIA = 28;

        public Double regionpT;
        public Double regionph;
        public Double regionps;

        Double h1, T1, p1, s1, v1, u1, Cp1, Cv1, w1,rho1;
        Double h2, T2, p2, s2, v2, u2, Cp2, Cv2, w2,rho2;
        Double h3, T3, p3, s3, v3, u3, Cp3, Cv3, w3, rho3;
        Double h4, T4, p4, s4;
        Double h5, T5, p5, s5, v5, u5, Cp5, Cv5, w5, rho5;

        public RegionSelection()
        {
            InitializeComponent();
        }

        private void RegionSelection_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Double p, T;

            p=Convert.ToDouble(textBox2.Text);
            T = Convert.ToDouble(textBox3.Text);
            region_pT(p, T);
            //textBox1.Text = Convert.ToString(regionpT);
            listBox1.Items.Add("Presión:" + Convert.ToString(p)+"  MPa"+"   "+"Temperatura:"+Convert.ToString(T)+"  K"+"    "+"Región:"+Convert.ToString(regionpT));
            listBox1.Items.Add("");

            if (regionpT == 1)
            {
                listBox1.Items.Add("Agua en estado SOLIDO");
                listBox1.Items.Add("");
                listBox1.Items.Add("H1:" + Convert.ToString(h1)+"  kJ/Kg");
                listBox1.Items.Add("");
                listBox1.Items.Add("s1:" + Convert.ToString(s1) + "  kJ/Kg K");
                listBox1.Items.Add("");
                listBox1.Items.Add("v1:" + Convert.ToString(v1)+"  m3/Kg");
                listBox1.Items.Add("");
                listBox1.Items.Add("u1:" + Convert.ToString(u1) + "  kJ/Kg");
                listBox1.Items.Add("");
                listBox1.Items.Add("w1:" + Convert.ToString(w1) + "  m/sg");
                listBox1.Items.Add("");
                listBox1.Items.Add("Cp1:" + Convert.ToString(Cp1) + "  kJ/Kg K");
                listBox1.Items.Add("");
                listBox1.Items.Add("Cv1:" + Convert.ToString(Cv1) + "  kJ/Kg K");
                listBox1.Items.Add("");
                listBox1.Items.Add("");
                listBox1.Items.Add("");
            }

            else if (regionpT==2)
            {
                listBox1.Items.Add("Agua en estado VAPOR");
                listBox1.Items.Add("");
                listBox1.Items.Add("H2:" + Convert.ToString(h2) + "  kJ/Kg");
                listBox1.Items.Add("");
                listBox1.Items.Add("s2:" + Convert.ToString(s2) + "  kJ/Kg K");
                listBox1.Items.Add("");
                listBox1.Items.Add("v2:" + Convert.ToString(v2) + "  m3/Kg");
                listBox1.Items.Add("");
                listBox1.Items.Add("u2:" + Convert.ToString(u2) + "  kJ/Kg");
                listBox1.Items.Add("");
                listBox1.Items.Add("w2:" + Convert.ToString(w2) + "  m/sg");
                listBox1.Items.Add("");
                listBox1.Items.Add("Cp2:" + Convert.ToString(Cp2) + "  kJ/Kg K");
                listBox1.Items.Add("");
                listBox1.Items.Add("Cv2:" + Convert.ToString(Cv2) + "  kJ/Kg K");
                listBox1.Items.Add("");
                listBox1.Items.Add("");
                listBox1.Items.Add("");
            }

            else if (regionpT==3)
            {
                listBox1.Items.Add("Agua en estado LIQUIDO");
                listBox1.Items.Add("");
                listBox1.Items.Add("H3:" + Convert.ToString(h3) + "  kJ/Kg");
                listBox1.Items.Add("");
                listBox1.Items.Add("s3:" + Convert.ToString(s3) + "  kJ/Kg K");
                listBox1.Items.Add("");
                listBox1.Items.Add("v3:" + Convert.ToString(v3) + "  m3/Kg");
                listBox1.Items.Add("");
                listBox1.Items.Add("u3:" + Convert.ToString(u3) + "  kJ/Kg");
                listBox1.Items.Add("");
                listBox1.Items.Add("w3:" + Convert.ToString(w3) + "  m/sg");
                listBox1.Items.Add("");
                listBox1.Items.Add("Cp3:" + Convert.ToString(Cp3) + "  kJ/Kg K");
                listBox1.Items.Add("");
                listBox1.Items.Add("Cv3:" + Convert.ToString(Cv3) + "  kJ/Kg K");
                listBox1.Items.Add("");
                listBox1.Items.Add("");
                listBox1.Items.Add("");
            }

            else if (regionpT == 4)
            {
                listBox1.Items.Add("Agua en estado LIQUIDO");
                listBox1.Items.Add("");
                listBox1.Items.Add("H4:" + Convert.ToString(h4) + "  kJ/Kg");
                listBox1.Items.Add("");
                listBox1.Items.Add("s4:" + Convert.ToString(s4) + "  kJ/Kg K");
                listBox1.Items.Add("");
                listBox1.Items.Add("p4:" + Convert.ToString(p4) + "  MPa");
                listBox1.Items.Add("");
                listBox1.Items.Add("T4:" + Convert.ToString(T4) + "  K");
                listBox1.Items.Add("");
                listBox1.Items.Add("");
                listBox1.Items.Add("");
            }

            else if (regionpT == 5)
            {
                listBox1.Items.Add("Agua en estado SUPERCRÍTICO");
                listBox1.Items.Add("");
                listBox1.Items.Add("H5:" + Convert.ToString(h5) + "  kJ/Kg");
                listBox1.Items.Add("");
                listBox1.Items.Add("s5:" + Convert.ToString(s5) + "  kJ/Kg K");
                listBox1.Items.Add("");
                listBox1.Items.Add("v5:" + Convert.ToString(v5) + "  m3/Kg");
                listBox1.Items.Add("");
                listBox1.Items.Add("u5:" + Convert.ToString(u5) + "  kJ/Kg");
                listBox1.Items.Add("");
                listBox1.Items.Add("w5:" + Convert.ToString(w5) + "  m/sg");
                listBox1.Items.Add("");
                listBox1.Items.Add("Cp5:" + Convert.ToString(Cp5) + "  kJ/Kg K");
                listBox1.Items.Add("");
                listBox1.Items.Add("Cv5:" + Convert.ToString(Cv5) + "  kJ/Kg K");
                listBox1.Items.Add("");
                listBox1.Items.Add("");
                listBox1.Items.Add("");
            }

            else
            {
                listBox1.Items.Add("Error en los datos introducidos.");
                listBox1.Items.Add("");
                listBox1.Items.Add("");
                listBox1.Items.Add("");

                MessageBox.Show("Error fuera del Rango de las Tablas del Agua. LUIS COCO");
            }
        }

        //'***********************************************************************************************************
        //'*3 Region Selection
        //'***********************************************************************************************************

        public void region_pT(Double p, Double T)
        {

            //'*3.1 Regions as a function of pT
            //Function region_pT(ByVal p As Double, ByVal T As Double) As Integer

            Double ps;
            Region1 luis1 = new Region1();
            Region2 luis2 = new Region2();
            Region_3 luis3 = new Region_3();
            Region_Border luisb = new Region_Border();
            Region_4 luis4 = new Region_4();
            Region5 luis5 = new Region5();
            

            if ((T > 1073.15) & (p < 50) & (T < 2273.15) & (p > 0.000611))
            {
                //REGION 5
                regionpT = 5;

                h5 = luis5.h5_pT(p, T);
                s5 = luis5.s5_pT(p, T);
                v5 = luis5.v5_pT(p, T);
                u5 = luis5.u5_pT(p, T);
                Cp5 = luis5.Cp5_pT(p, T);
                Cv5= luis5.Cv5_pT(p, T);
                w5 = luis5.w5_pT(p, T);
                rho5 = 1 / (luis5.v5_pT(p, T));
                return;                
            }

            else if ((T <= 1073.15) & (T > 273.15) & (p <= 100) & (p > 0.000611))
            {
                //REGIONES 2,3 y 4
                if (T > 623.15)
                {
                    if (p > luisb.B23p_T(T))
                    {
                        //REGION 3
                        regionpT = 3;
                        v3 = (luis3.v3_ph(p, luis3.h3_pT(p, T)));
                        rho3 = 1 / v3;
                        h3 = luis3.h3_pT(p, T);
                        s3 = luis3.s3_rhoT(rho3,T);
                        w3 = luis3.w3_rhoT(rho3,T);
                        u3 = luis3.u3_rhoT(rho3,T);
                        Cp3 = luis3.Cp3_rhoT(rho3,T);
                        Cv3 = luis3.Cv3_rhoT(rho3,T);

                        if (T < 647.096)
                        {
                            ps = luis4.p4_T(T);
                            if ((Math.Abs(p - ps)) < 0.001)
                            {
                                //REGION 4
                                regionpT = 4;
                                                                 
                                T4 = luis4.p4_T(p);
                                p4 = luis4.T4_p(T);
                                s4 = luis4.p4_s(p);
                                h4 = luis4.h4_s(s4);
                                return;
                            }
                        }
                     }
                        else
                        {
                            //REGION 2
                            regionpT = 2;

                            h2 = luis2.h2_pT(p, T);
                            s2 = luis2.s2_pT(p, T);
                            v2 = luis2.v2_pT(p, T);
                            u2 = luis2.u2_pT(p, T);
                            Cp2 = luis2.Cp2_pT(p, T);
                            Cv2 = luis2.Cv2_pT(p, T);
                            w2 = luis2.w2_pT(p, T);
                            rho2=1/(luis2.v2_pT(p,T));

                            return;
                        }
                   }

                   //REGIONES 1,4,2
                    else
                    {
                        ps = luis4.p4_T(T);

                        if ((Math.Abs(p - ps)) < 0.001)
                        {
                            //REGION 4
                            regionpT = 4;

                            T4=luis4.p4_T(p);
                            p4=luis4.T4_p(T);
                            s4=luis4.p4_s(p);
                            h4=luis4.h4_s(s4);
                            return;
                        }
                        else if (p > ps)
                        {
                            //REGION 1
                            regionpT = 1;

                            h1=luis1.h1_pT(p,T);
                            s1 = luis1.s1_pT(p,T);
                            v1 = luis1.v1_pT(p, T);
                            u1 = luis1.u1_pT(p,T);
                            Cp1 = luis1.Cp1_pT(p, T);
                            Cv1=luis1.Cv1_pT(p,T);
                            w1 = luis1.w1_pT(p, T);
                            rho1 = 1 / (luis1.v1_pT(p, T));

                            return;
                        }
                        else
                        {
                            //REGION 2
                            regionpT = 2;

                            h2 = luis2.h2_pT(p,T);
                            s2 = luis2.s2_pT(p, T);
                            v2 = luis2.v2_pT(p, T);
                            u2 = luis2.u2_pT(p, T);
                            Cp2 = luis2.Cp2_pT(p, T);
                            Cv2 = luis2.Cv2_pT(p, T);
                            w2 = luis2.w2_pT(p, T);
                            rho2 = 1 / (luis2.v2_pT(p, T));

                            return;
                        }
                    }
            }
            else
            {
                //ERROR REGION 0
                MessageBox.Show("Error Fuera de los Rangos de las Tablas del Agua. Hola LUIS COCO");
                regionpT = 0; //'**Error, Outside valid area
                return;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Region1 luis1 = new Region1();
            luis1.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Region2 luis2 = new Region2();
            luis2.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Region_3 luis3 = new Region_3();
            luis3.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Region_4 luis4 = new Region_4();
            luis4.Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Region5 luis5 = new Region5();
            luis5.Show();

        }

        private void button11_Click(object sender, EventArgs e)
        {
            Region_Border luisb = new Region_Border();
            luisb.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            Double p, H;

            p = Convert.ToDouble(textBox5.Text);
            H = Convert.ToDouble(textBox4.Text);
            region_pH(p, H);
            //textBox6.Text = Convert.ToString(regionph);
            listBox1.Items.Add("Presión:" + Convert.ToString(p) + "   " + "Entalpía:" + Convert.ToString(H) + "    " + "Región:" + Convert.ToString(regionph));
        }



        public void region_pH(Double p, Double H)
        {
           // '***********************************************************************************************************
           // '*3.2 Region s as a function of ph
           //Function region_ph(ByVal p, ByVal h) As Integer

           Region1 luis1=new Region1();
           Region2 luis2=new Region2();
           Region_3 luis3=new Region_3();
           Region_Border luisb=new Region_Border();
           Region_4 luis4 = new Region_4();
           Region5 luis5=new Region5();
           Double hL, hV, h_45, h_5u, Ts;

           //'Check if outside pressure limits
           if ((p < 0.000611657) || (p > 100))
           {
             //ERROR REGION 0
             MessageBox.Show("Error Fuera de los Rangos de las Tablas del Agua. Hola LUIS COCO");
             regionph= 0;
             return;
           }
 
           //Check if outside low h.
           if (H < (0.963 * p + 2.2)) //'Linear adaption to h1_pt()+2 to speed up calcualations.
           {
             if (H < luis1.h1_pT(p, 273.15))
             {
               //ERROR REGION 0
               MessageBox.Show("Error Fuera de los Rangos de las Tablas del Agua. Hola LUIS COCO");
               regionph = 0;
               return;
             }
           }
 
if (p < 16.5292) //Bellow region 3,Check  region 1,4,2,5
{
   //Check REGION 1
   Ts = luis4.T4_p(p);
   hL = 109.6635 * Math.Log(p) + 40.3481 * p + 734.58;   //Approximate function for hL_p

   if (Math.Abs(H - hL) < 100) //If approximate is not god enough use real function
   {
      hL = luis1.h1_pT(p, Ts);
   }
   if (H <= hL)
   {
     regionph = 1;
     return;
   }

   //Check REGION 4
   hV = 45.1768 * Math.Log(p) - 20.158 * p + 2804.4; //Approximate function for hV_p
   if (Math.Abs(H - hV) < 50) //If approximate is not god enough use real function
   {
       hV = luis2.h2_pT(p, Ts);
   }

   if (H < hV)
   {
     regionph = 4;
     return;
   }
   

   //Check upper limit of REGION 2 Quick Test
   if (H < 4000)
   {
     regionph = 2;
   }
   
  //Check REGION 2 (Real value)
   h_45 = luis2.h2_pT(p, 1073.15);
   if (H <= h_45)
   {
     regionph = 2;
     return;
   }
   
  //Check REGION 5
   if (p > 10)
   {
     regionph = 0;
     return;
   }
   
   h_5u = luis5.h5_pT(p, 2273.15);

   if (H < h_5u)
   {
      regionph = 5;
      return;
   }

   regionph = 0;
   return;
}


else //For p>16.5292
{
   //Check if in REGION1
   if (H < luis1.h1_pT(p, 623.15))
   {  
     regionph = 1;
     return;
   }

   //Check if in REGION 3 or 4 (Bellow REGION 2)
   if (H<luis2.h2_pT(p,luisb.B23T_p(p)))
   {
     //REGION 3 or 4
     if (p >luisb.p3sat_h(H))
     {
       regionph = 3;
       return;
     }
     else
     {
       regionph = 4;
       return;
     }
   }

  //Check if REGION 2
  if (H < luis2.h2_pT(p, 1073.15))
  { 
      regionph = 2;
      return;
  }
   
 regionph = 0;
 return;

}  
        }




        public void region_ps(Double p, Double s)
        {
           //***********************************************************************************************************
           //*3.3 Regions as a function of ps
           //Function region_ps(ByVal p As Double, ByVal s) As Integer

            Double ss;
            Region1 luis1 = new Region1();
            Region2 luis2 = new Region2();
            Region_3 luis3=new Region_3();
            Region_Border luisb = new Region_Border();
            Region_4 luis4 = new Region_4();
            Region5 luis5 = new Region5();

  //Limites de validez de las Tablas de Vapor
  if ((p < 0.000611657)|| (p > 100) || (s < 0) || (s > luis5.s5_pT(p, 2273.15))) 
  {
   regionps = 0;
  }
  
  //Check REGION 5
  if (s > luis2.s2_pT(p, 1073.15))
  {
    if (p <= 10)
    {
      regionps = 5;
      return;
    }  
    else
    {
      regionps = 0;
      return;
    }
  }
  
  //Check REGION 2
  if (p > 16.529)
  {
    ss = luis2.s2_pT(p, luisb.B23T_p(p));  //Between 5.047 and 5.261. Use to speed up!
  }
  else
  {
    ss = luis2.s2_pT(p, luis4.T4_p(p));
  }
  
  if (s > ss)
  {
      regionps = 2;
      return;
  }
  
  //Check REGION 3
  ss = luis1.s1_pT(p, 623.15);

  if ((p > 16.529) & (s > ss))
  {
    if (p > luisb.p3sat_s(s))
    { 
      regionps = 3;
      return;
    } 
    else
    {  
      regionps = 4;
      return;
    }
  }
  
  //Check REGION 4 (Not inside region 3)
  if ((p < 16.529) & (s > luis1.s1_pT(p, luis4.T4_p(p))))
  {
    regionps = 4;
    return;
  }
  
  //Check REGION 1
  if ((p > 0.000611657) & (s > luis1.s1_pT(p, 273.15))) 
  {
    regionps = 1;
    return;
  }
  
  regionps = 1;
  return;
 
        }


        private void button4_Click(object sender, EventArgs e)
        {
            Double p, s;

            p = Convert.ToDouble(textBox11.Text);
            s = Convert.ToDouble(textBox10.Text);
            region_ps(p, s);
            //textBox12.Text = Convert.ToString(regionps);
            listBox1.Items.Add("Presión:" + Convert.ToString(p) + "   " + "Entropía:" + Convert.ToString(s) + "    " + "Región:" + Convert.ToString(regionps));
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

    }
}
