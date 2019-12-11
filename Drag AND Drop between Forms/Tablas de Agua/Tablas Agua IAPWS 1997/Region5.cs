using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tablas_Vapor_ASME2;

namespace Tablas_Vapor_ASME5
{
    public partial class Region5 : Form
    {
        public Region5()
        {
            InitializeComponent();
        }

       
        public Double h5_pT(Double p, Double T)
        {   
          //*2.5 Functions for region 5
          //Function h5_pT(ByVal p As Double, ByVal T As Double) As Double
          //Release on the IAPWS Industrial Formulation 1997 for the Thermodynamic Properties of Water and Steam, September 1997
          //Basic Equation for Region 5
          //Eq 32,33, Page 36, Tables 37-41


          Double tau, gamma0_tau, gammar_tau;
          Double R = 0.461526;   //kJ/(kg K)
          Double h5_pT;
          h5_pT=0;

          Double[] Ji0 = new Double[] {0, 1, -3, -2, -1, 2};
          Double[] ni0 = new Double[] {-13.179983674201, 6.8540841634434, -0.024805148933466, 0.36901534980333, -3.1161318213925, -0.32961626538917};
          Double[] Iir = new Double[] {1, 1, 1, 2, 2, 3};
          Double[] Jir = new Double[] {1, 2, 3, 3, 9, 7};
          Double[] nir = new Double[] {0.15736404855259E-02, 0.90153761673944E-03, -0.50270077677648E-02, 0.22440037409485E-05, -0.41163275453471E-05, 0.37919454822955E-07};
        
          tau = 1000 / T;
          gamma0_tau = 0;

          for(int i = 0;i<=5;i++)
          {   
            gamma0_tau = gamma0_tau + ni0[i] * Ji0[i] * Math.Pow(tau,(Ji0[i] - 1));
          }

          gammar_tau = 0;

          for(int i = 0;i<=5;i++)
          {
            gammar_tau = gammar_tau + nir[i] * Math.Pow(p,Iir[i]) * Jir[i] * Math.Pow(tau,(Jir[i] - 1));
          }

          h5_pT = R * T * tau * (gamma0_tau + gammar_tau);
          
          return(h5_pT);
    
        }


        public Double v5_pT(Double p, Double T)
        {
            //Function v5_pT(ByVal p As Double, ByVal T As Double) As Double
            //Release on the IAPWS Industrial Formulation 1997 for the Thermodynamic Properties of Water and Steam, September 1997
            //Basic Equation for Region 5
            //Eq 32,33, Page 36, Tables 37-41

            Double tau, gamma0_pi, gammar_pi;

            Double R = 0.461526;   //kJ/(kg K)

            Double v5_pT;
            v5_pT=0;

            Double[] Ji0 = new Double[] { 0, 1, -3, -2, -1, 2 };
            Double[] ni0 = new Double[] { -13.179983674201, 6.8540841634434, -0.024805148933466, 0.36901534980333, -3.1161318213925, -0.32961626538917 };
            Double[] Iir = new Double[] { 1, 1, 1, 2, 2, 3 };
            Double[] Jir = new Double[] { 1, 2, 3, 3, 9, 7 };
            Double[] nir = new Double[] { 0.15736404855259E-02, 0.90153761673944E-03, -0.50270077677648E-02, 0.22440037409485E-05, -0.41163275453471E-05, 0.37919454822955E-07 };
        
           
            tau = 1000 / T;
            gamma0_pi = 1 / p;
            gammar_pi = 0;

            for(int i = 0;i<=5;i++)
            {
                 gammar_pi = gammar_pi + nir[i] * Iir[i] * Math.Pow(p,(Iir[i] - 1)) * Math.Pow(tau,Jir[i]);
            }

            v5_pT = R * T / p * p * (gamma0_pi + gammar_pi) / 1000;

            return(v5_pT);

          }



         public Double u5_pT(Double p, Double T)
         {

            //Function u5_pT(ByVal p As Double, ByVal T As Double) As Double
            //Release on the IAPWS Industrial Formulation 1997 for the Thermodynamic Properties of Water and Steam, September 1997
            //Basic Equation for Region 5
            //Eq 32,33, Page 36, Tables 37-41

            Double tau, gamma0_pi, gammar_pi, gamma0_tau, gammar_tau;
            Double R = 0.461526;   //kJ/(kg K)

            Double  u5_pT;
            u5_pT=0;

            Double[] Ji0 = new Double[] { 0, 1, -3, -2, -1, 2 };
            Double[] ni0 = new Double[] { -13.179983674201, 6.8540841634434, -0.024805148933466, 0.36901534980333, -3.1161318213925, -0.32961626538917 };
            Double[] Iir = new Double[] { 1, 1, 1, 2, 2, 3 };
            Double[] Jir = new Double[] { 1, 2, 3, 3, 9, 7 };
            Double[] nir = new Double[] { 0.15736404855259E-02, 0.90153761673944E-03, -0.50270077677648E-02, 0.22440037409485E-05, -0.41163275453471E-05, 0.37919454822955E-07 };
        
            tau = 1000 / T;
            gamma0_pi = 1 / p;
            gamma0_tau = 0;

            for(int i = 0;i<=5;i++)
            {
                gamma0_tau = gamma0_tau + ni0[i] * Ji0[i] *Math.Pow(tau,(Ji0[i] - 1));
            }
            
            gammar_pi = 0;
            gammar_tau = 0;

            for(int i = 0;i<=5;i++)
            {
                gammar_pi = gammar_pi + nir[i] * Iir[i] * Math.Pow(p,(Iir[i] - 1)) *Math.Pow(tau,Jir[i]);
                gammar_tau = gammar_tau + nir[i] * Math.Pow(p,Iir[i]) * Jir[i] * Math.Pow(tau,(Jir[i] - 1));

            }

            u5_pT = R * T * (tau * (gamma0_tau + gammar_tau) - p * (gamma0_pi + gammar_pi));

            return(u5_pT);

        }


public Double Cp5_pT(Double p, Double T)
{

//Function Cp5_pT(ByVal p As Double, ByVal T As Double) As Double
//Release on the IAPWS Industrial Formulation 1997 for the Thermodynamic Properties of Water and Steam, September 1997
//Basic Equation for Region 5
//Eq 32,33, Page 36, Tables 37-41

Double tau, gamma0_tautau, gammar_tautau;
Double R = 0.461526;   //kJ/(kg K)

Double Cp5_pT;
Cp5_pT=0;

Double[] Ji0 = new Double[] { 0, 1, -3, -2, -1, 2 };
Double[] ni0 = new Double[] { -13.179983674201, 6.8540841634434, -0.024805148933466, 0.36901534980333, -3.1161318213925, -0.32961626538917 };
Double[] Iir = new Double[] { 1, 1, 1, 2, 2, 3 };
Double[] Jir = new Double[] { 1, 2, 3, 3, 9, 7 };
Double[] nir = new Double[] { 0.15736404855259E-02, 0.90153761673944E-03, -0.50270077677648E-02, 0.22440037409485E-05, -0.41163275453471E-05, 0.37919454822955E-07 };
        
tau = 1000 / T;
gamma0_tautau = 0;

for(int i = 0;i<=5;i++)
{
gamma0_tautau = gamma0_tautau + ni0[i] * Ji0[i] * (Ji0[i] - 1) * Math.Pow(tau,(Ji0[i] - 2));
}

gammar_tautau = 0;

for(int i = 0;i<=5;i++)
{
gammar_tautau = gammar_tautau + nir[i] * Math.Pow(p,Iir[i]) * Jir[i] * (Jir[i] - 1) *Math.Pow(tau,(Jir[i] - 2));
}

Cp5_pT = -R * Math.Pow(tau,2) * (gamma0_tautau + gammar_tautau);

return(Cp5_pT);

}


public Double s5_pT(Double p, Double T)
{

//Function s5_pT(ByVal p As Double, ByVal T As Double) As Double
//Release on the IAPWS Industrial Formulation 1997 for the Thermodynamic Properties of Water and Steam, September 1997
//Basic Equation for Region 5
//Eq 32,33, Page 36, Tables 37-41

Double tau, gamma0, gamma0_tau, gammar, gammar_tau;
Double R = 0.461526;   //kJ/(kg K)

Double s5_pT;
s5_pT=0;

Double[] Ji0 = new Double[] { 0, 1, -3, -2, -1, 2 };
Double[] ni0 = new Double[] { -13.179983674201, 6.8540841634434, -0.024805148933466, 0.36901534980333, -3.1161318213925, -0.32961626538917 };
Double[] Iir = new Double[] { 1, 1, 1, 2, 2, 3 };
Double[] Jir = new Double[] { 1, 2, 3, 3, 9, 7 };
Double[] nir = new Double[] { 0.15736404855259E-02, 0.90153761673944E-03, -0.50270077677648E-02, 0.22440037409485E-05, -0.41163275453471E-05, 0.37919454822955E-07 };
        
tau = 1000 / T;
gamma0 = Math.Log(p);
gamma0_tau = 0;

for(int i = 0;i<=5;i++)
{
  gamma0_tau = gamma0_tau + ni0[i] * Ji0[i] * Math.Pow(tau,(Ji0[i] - 1));
  gamma0 = gamma0 + ni0[i] * Math.Pow(tau,Ji0[i]);
}

gammar = 0;
gammar_tau = 0;

for(int i = 0;i<=5;i++)
{
  gammar = gammar + nir[i] * Math.Pow(p,Iir[i]) * Math.Pow(tau,Jir[i]);
  gammar_tau = gammar_tau + nir[i] * Math.Pow(p,Iir[i]) * Jir[i] * Math.Pow(tau,(Jir[i] - 1));
}

s5_pT = R * (tau * (gamma0_tau + gammar_tau) - (gamma0 + gammar));

return(s5_pT);

}


public Double Cv5_pT(Double p, Double T)
{

//Function Cv5_pT(ByVal p As Double, ByVal T As Double) As Double
//Release on the IAPWS Industrial Formulation 1997 for the Thermodynamic Properties of Water and Steam, September 1997
//Basic Equation for Region 5
//Eq 32,33, Page 36, Tables 37-41

Double tau, gamma0_tautau, gammar_pi, gammar_pitau, gammar_pipi, gammar_tautau;
Double R = 0.461526;   //kJ/(kg K)

Double Cv5_pT;
Cv5_pT=0;

Double[] Ji0 = new Double[] { 0, 1, -3, -2, -1, 2 };
Double[] ni0 = new Double[] { -13.179983674201, 6.8540841634434, -0.024805148933466, 0.36901534980333, -3.1161318213925, -0.32961626538917 };
Double[] Iir = new Double[] { 1, 1, 1, 2, 2, 3 };
Double[] Jir = new Double[] { 1, 2, 3, 3, 9, 7 };
Double[] nir = new Double[] { 0.15736404855259E-02, 0.90153761673944E-03, -0.50270077677648E-02, 0.22440037409485E-05, -0.41163275453471E-05, 0.37919454822955E-07 };
        
tau = 1000 / T;
gamma0_tautau = 0;

for(int i = 0;i<=5;i++)
{
  gamma0_tautau = gamma0_tautau + ni0[i] * (Ji0[i] - 1) * Ji0[i] * Math.Pow(tau,(Ji0[i] - 2));
}


gammar_pi = 0;
gammar_pitau = 0;
gammar_pipi = 0;
gammar_tautau = 0;

for(int i = 0;i<=5;i++)
{
  gammar_pi = gammar_pi + nir[i] * Iir[i] * Math.Pow(p,(Iir[i] - 1)) * Math.Pow(tau,Jir[i]);
  gammar_pitau = gammar_pitau + nir[i] * Iir[i] * Math.Pow(p,(Iir[i] - 1)) * Jir[i] * Math.Pow(tau,(Jir[i] - 1));
  gammar_pipi = gammar_pipi + nir[i] * Iir[i] * (Iir[i] - 1) * Math.Pow(p,(Iir[i] - 2)) * Math.Pow(tau,Jir[i]);
  gammar_tautau = gammar_tautau + nir[i] * Math.Pow(p,Iir[i]) * Jir[i] * (Jir[i] - 1) * Math.Pow(tau,(Jir[i] - 2));

}

Cv5_pT = R * (-(Math.Pow(tau,2) * (gamma0_tautau + gammar_tautau)) - Math.Pow((1 + p * gammar_pi - tau * p * gammar_pitau),2) / (1 - Math.Pow(p,2) * gammar_pipi));

return(Cv5_pT);
    
}

        
public Double w5_pT(Double p, Double T)
{
//Function w5_pT(ByVal p As Double, ByVal T As Double) As Double
//Release on the IAPWS Industrial Formulation 1997 for the Thermodynamic Properties of Water and Steam, September 1997
//Basic Equation for Region 5
//Eq 32,33, Page 36, Tables 37-41

Double tau, gamma0_tautau, gammar_pi, gammar_pitau, gammar_pipi, gammar_tautau;
Double R = 0.461526;   //kJ/(kg K)

Double w5_pT;
w5_pT=0;

Double[] Ji0 = new Double[] { 0, 1, -3, -2, -1, 2 };
Double[] ni0 = new Double[] { -13.179983674201, 6.8540841634434, -0.024805148933466, 0.36901534980333, -3.1161318213925, -0.32961626538917 };
Double[] Iir = new Double[] { 1, 1, 1, 2, 2, 3 };
Double[] Jir = new Double[] { 1, 2, 3, 3, 9, 7 };
Double[] nir = new Double[] { 0.15736404855259E-02, 0.90153761673944E-03, -0.50270077677648E-02, 0.22440037409485E-05, -0.41163275453471E-05, 0.37919454822955E-07 };
        
tau = 1000 / T;
gamma0_tautau = 0;

for(int i = 0;i<=5;i++)
{
gamma0_tautau = gamma0_tautau + ni0[i] * (Ji0[i] - 1) * Ji0[i] * Math.Pow(tau,(Ji0[i] - 2));
}

gammar_pi = 0;
gammar_pitau = 0;
gammar_pipi = 0;
gammar_tautau = 0;

for(int i = 0;i<=5;i++)
{
  gammar_pi = gammar_pi + nir[i] * Iir[i] * Math.Pow(p,(Iir[i] - 1)) * Math.Pow(tau,Jir[i]);
  gammar_pitau = gammar_pitau + nir[i] * Iir[i] * Math.Pow(p,(Iir[i] - 1)) * Jir[i] * Math.Pow(tau,(Jir[i] - 1));
  gammar_pipi = gammar_pipi + nir[i] * Iir[i] * (Iir[i] - 1) * Math.Pow(p,(Iir[i] - 2)) * Math.Pow(tau,Jir[i]);
  gammar_tautau = gammar_tautau + nir[i] *Math.Pow(p,Iir[i]) * Jir[i] * (Jir[i] - 1) * Math.Pow(tau,(Jir[i] - 2));
}

w5_pT = Math.Pow((1000 * R * T * (1 + 2 * p * gammar_pi + Math.Pow(p,2) * Math.Pow(gammar_pi,2)) / ((1 -Math.Pow(p,2) * gammar_pipi) + Math.Pow((1 + p * gammar_pi - tau * p * gammar_pitau),2) / (Math.Pow(tau,2) * (gamma0_tautau + gammar_tautau)))),0.5);

return(w5_pT);

}



public Double T5_ph(Double p, Double h)
{

    //Function T5_ph(ByVal p As Double, ByVal h As Double) As Double
    //Solve with half interval method
    
    Double Low_Bound, High_Bound, Ts, hs;

    Double T5_ph;
    T5_ph=0;

    Low_Bound = 1073.15;
    High_Bound = 2273.15;

    do 
    {
      Ts = (Low_Bound + High_Bound) / 2;
      hs = h5_pT(p, Ts);
      
      if (hs > h)
      { 
        High_Bound = Ts;
      } 
      else
      {
        Low_Bound = Ts;
      }
    }
    while((Math.Abs(h - hs)) > 0.00001);
    
    T5_ph = Ts;

    return(T5_ph);
}


public Double T5_ps(Double p, Double s)
{
    //Function T5_ps(ByVal p As Double, ByVal s As Double) As Double
    //Solve with half interval method
    
    Double Low_Bound, High_Bound, Ts, ss;

    Double T5_ps;
    T5_ps=0;

    Low_Bound = 1073.15;
    High_Bound = 2273.15;
    
    do 
    {
      Ts = (Low_Bound + High_Bound) / 2;
      ss = s5_pT(p, Ts);
      
      if (ss > s)
      {
      High_Bound = Ts;
      }
      else
      {
      Low_Bound = Ts;
      }
      
    }
    while ((Math.Abs(s - ss)) > 0.00001);
   
    T5_ps = Ts;
    return(T5_ps);

}


public Double T5_prho(Double p, Double rho)
{
  //Function T5_prho(ByVal p As Double, ByVal rho As Double) As Double
  //Solve by iteration. Observe that fo low temperatures this equation has 2 solutions.
  //Solve with half interval method
  
    Double Low_Bound, High_Bound, Ts, rhos;
    Region2 luis2 = new Region2();

    Double T5_prho;
    T5_prho = 0;

    Low_Bound = 1073.15;
    High_Bound = 2073.15;

    do 
    {
    Ts = (Low_Bound + High_Bound) / 2;
    rhos = 1 / luis2.v2_pT(p, Ts);
    
    if (rhos < rho)
    {
    High_Bound = Ts;
    }
    else
    {
    Low_Bound = Ts;
    }
    
    }
    while ((Math.Abs(rho - rhos)) > 0.000001);

    T5_prho = Ts;

    return(T5_prho);
 }

private void button1_Click(object sender, EventArgs e)
{
    Double p,T,H;
    p=Convert.ToDouble(textBox1.Text);
    T = Convert.ToDouble(textBox2.Text);
    H=h5_pT(p, T);
    textBox3.Text=Convert.ToString(H);

}

private void button2_Click(object sender, EventArgs e)
{
    Double p, T, v;
    p = Convert.ToDouble(textBox6.Text);
    T = Convert.ToDouble(textBox5.Text);
    v = v5_pT(p, T);
    textBox4.Text = Convert.ToString(v);
}

private void button4_Click(object sender, EventArgs e)
{
    Double p, T, u;
    p = Convert.ToDouble(textBox12.Text);
    T = Convert.ToDouble(textBox11.Text);
    u = u5_pT(p, T);
    textBox10.Text = Convert.ToString(u);

}

private void button3_Click(object sender, EventArgs e)
{
    Double p, T, Cp;
    p = Convert.ToDouble(textBox9.Text);
    T = Convert.ToDouble(textBox8.Text);
    Cp = Cp5_pT(p, T);
    textBox7.Text = Convert.ToString(Cp);

}

private void button8_Click(object sender, EventArgs e)
{
    Double p, T, s;
    p = Convert.ToDouble(textBox24.Text);
    T = Convert.ToDouble(textBox23.Text);
    s = s5_pT(p, T);
    textBox22.Text = Convert.ToString(s);
}

private void button7_Click(object sender, EventArgs e)
{
    Double p, T, Cv;
    p = Convert.ToDouble(textBox21.Text);
    T = Convert.ToDouble(textBox20.Text);
    Cv = Cv5_pT(p, T);
    textBox19.Text = Convert.ToString(Cv);

}

private void button6_Click(object sender, EventArgs e)
{
    Double p, T, w;
    p = Convert.ToDouble(textBox18.Text);
    T = Convert.ToDouble(textBox17.Text);
    w = w5_pT(p, T);
    textBox16.Text = Convert.ToString(w);

}

private void button5_Click(object sender, EventArgs e)
{
    Double p, T, H;
    p = Convert.ToDouble(textBox15.Text);
    H = Convert.ToDouble(textBox14.Text);
    T = T5_ph(p, H);
    textBox13.Text = Convert.ToString(T);

}

private void button9_Click(object sender, EventArgs e)
{
    Double p, T, s;
    p = Convert.ToDouble(textBox27.Text);
    s = Convert.ToDouble(textBox26.Text);
    T = T5_ps(p, s);
    textBox25.Text = Convert.ToString(T);
}

private void button10_Click(object sender, EventArgs e)
{
    Double p, rho, T;
    p = Convert.ToDouble(textBox30.Text);
    rho = Convert.ToDouble(textBox29.Text);
    T = T5_prho(p, rho);
    textBox28.Text = Convert.ToString(T);
}


    }
}
