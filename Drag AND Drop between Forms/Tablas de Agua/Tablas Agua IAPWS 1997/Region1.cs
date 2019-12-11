using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tablas_Vapor_ASME1
{
    public partial class Region1 : Form
    {
        public Region1()
        {
            InitializeComponent();
        }

        // IAPWS IF 97 Calling functions                                                                          *

        //2.1 Functions for REGION 1

        public Double v1_pT(Double p, Double T)
        {
            //Function v1_pT(ByVal p As Double, ByVal T As Double) As Double

            //Release on the IAPWS Industrial Formulation 1997 for the Thermodynamic Properties of Water and Steam, September 1997
            //5 Equations for Region 1, Section. 5.1 Basic Equation
            //Eqution 7, Table 3, Page 6

            Double ps, tau, g;
            Double R = 0.461526; //kJ/(kg K)

            Double[] I1 = new Double[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 8, 8, 21, 23, 29, 30, 31, 32 };
            Double[] J1 = new Double[] { -2, -1, 0, 1, 2, 3, 4, 5, -9, -7, -1, 0, 1, 3, -3, 0, 1, 3, 17, -4, 0, 6, -5, -2, 10, -8, -11, -6, -29, -31, -38, -39, -40, -41 };
            Double[] n1 = new Double[] { 0.14632971213167, -0.84548187169114, -3.756360367204, 3.3855169168385, -0.95791963387872, 0.15772038513228, -0.016616417199501, 8.1214629983568E-04, 2.8319080123804E-04, -6.0706301565874E-04, -0.018990068218419, -0.032529748770505, -0.021841717175414, -5.283835796993E-05, -4.7184321073267E-04, -3.0001780793026E-04, 4.7661393906987E-05, -4.4141845330846E-06, -7.2694996297594E-16, -3.1679644845054E-05, -2.8270797985312E-06, -8.5205128120103E-10, -2.2425281908E-06, -6.5171222895601E-07, -1.4341729937924E-13, -4.0516996860117E-07, -1.2734301741641E-09, -1.7424871230634E-10, -6.8762131295531E-19, 1.4478307828521E-20, 2.6335781662795E-23, -1.1947622640071E-23, 1.8228094581404E-24, -9.3537087292458E-26 };


            ps = p / 16.53;
            tau = 1386 / T;
            g = 0.0;

            for (int i = 0; i <= 33; i++)
            {
                g = g - n1[i] * I1[i] * Math.Pow((7.1 - ps), (I1[i] - 1)) * Math.Pow((tau - 1.222), J1[i]);
            }
            
            return (R * T / p * ps * g / 1000);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Double p, T;

            p = 0;
            T = 0;

            p = Convert.ToDouble(textBox1.Text);
            T = Convert.ToDouble(textBox2.Text);

            textBox3.Text = Convert.ToString(v1_pT(p, T));
        }

        public Double h1_pT(Double p, Double T)
        {
            //Function h1_pT(ByVal p As Double, ByVal T As Double) As Double

            //Release on the IAPWS Industrial Formulation 1997 for the Thermodynamic Properties of Water and Steam, September 1997
            //5 Equations for Region 1, Section. 5.1 Basic Equation
            //Eqution 7, Table 3, Page 6

            Double ps, tau, g;

            Double R = 0.461526; // kJ/(kg K)

            Double[] I1 = new Double[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 8, 8, 21, 23, 29, 30, 31, 32 };
            Double[] J1 = new Double[] { -2, -1, 0, 1, 2, 3, 4, 5, -9, -7, -1, 0, 1, 3, -3, 0, 1, 3, 17, -4, 0, 6, -5, -2, 10, -8, -11, -6, -29, -31, -38, -39, -40, -41 };
            Double[] n1 = new Double[] { 0.14632971213167, -0.84548187169114, -3.756360367204, 3.3855169168385, -0.95791963387872, 0.15772038513228, -0.016616417199501, 8.1214629983568E-04, 2.8319080123804E-04, -6.0706301565874E-04, -0.018990068218419, -0.032529748770505, -0.021841717175414, -5.283835796993E-05, -4.7184321073267E-04, -3.0001780793026E-04, 4.7661393906987E-05, -4.4141845330846E-06, -7.2694996297594E-16, -3.1679644845054E-05, -2.8270797985312E-06, -8.5205128120103E-10, -2.2425281908E-06, -6.5171222895601E-07, -1.4341729937924E-13, -4.0516996860117E-07, -1.2734301741641E-09, -1.7424871230634E-10, -6.8762131295531E-19, 1.4478307828521E-20, 2.6335781662795E-23, -1.1947622640071E-23, 1.8228094581404E-24, -9.3537087292458E-26 };

            ps = p / 16.53;
            tau = 1386 / T;
            g = 0;

            for (int i = 0; i <= 33; i++)
            {
                g = g + (n1[i] * Math.Pow((7.1 - ps), I1[i]) * J1[i] * Math.Pow((tau - 1.222), (J1[i] - 1)));
            }

            return(R * T * tau * g);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Double p, T;

            p = 0;
            T = 0;

            p = Convert.ToDouble(textBox6.Text);
            T = Convert.ToDouble(textBox5.Text);

            textBox4.Text=Convert.ToString(h1_pT(p,T));
        }


        public Double u1_pT(Double p, Double T)
        {
            //Function u1_pT(ByVal p As Double, ByVal T As Double) As Double

            //Release on the IAPWS Industrial Formulation 1997 for the Thermodynamic Properties of Water and Steam, September 1997
            //5 Equations for Region 1, Section. 5.1 Basic Equation
            //Eqution 7, Table 3, Page 6

            Double tau, gt, gp;

            Double R = 0.461526; // kJ/(kg K)

            Double[] I1 = new Double[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 8, 8, 21, 23, 29, 30, 31, 32 };
            Double[] J1 = new Double[] { -2, -1, 0, 1, 2, 3, 4, 5, -9, -7, -1, 0, 1, 3, -3, 0, 1, 3, 17, -4, 0, 6, -5, -2, 10, -8, -11, -6, -29, -31, -38, -39, -40, -41 };
            Double[] n1 = new Double[] { 0.14632971213167, -0.84548187169114, -3.756360367204, 3.3855169168385, -0.95791963387872, 0.15772038513228, -0.016616417199501, 8.1214629983568E-04, 2.8319080123804E-04, -6.0706301565874E-04, -0.018990068218419, -0.032529748770505, -0.021841717175414, -5.283835796993E-05, -4.7184321073267E-04, -3.0001780793026E-04, 4.7661393906987E-05, -4.4141845330846E-06, -7.2694996297594E-16, -3.1679644845054E-05, -2.8270797985312E-06, -8.5205128120103E-10, -2.2425281908E-06, -6.5171222895601E-07, -1.4341729937924E-13, -4.0516996860117E-07, -1.2734301741641E-09, -1.7424871230634E-10, -6.8762131295531E-19, 1.4478307828521E-20, 2.6335781662795E-23, -1.1947622640071E-23, 1.8228094581404E-24, -9.3537087292458E-26 };

            p = p / 16.53;
            tau = 1386 / T;
            gt = 0;
            gp = 0;

            for (int i = 0; i <= 33; i++)
            {
                gp = gp - n1[i] * I1[i] * Math.Pow((7.1 - p), (I1[i] - 1)) * Math.Pow((tau - 1.222), J1[i]);
                gt = gt + (n1[i] * Math.Pow((7.1 - p), I1[i]) * J1[i] * Math.Pow((tau - 1.222), (J1[i] - 1)));
            }

           return (R * T * (tau * gt - p * gp));
        
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Double p, T;
            
            p = 0;
            T = 0;

            p = Convert.ToDouble(textBox12.Text);
            T = Convert.ToDouble(textBox11.Text);

            textBox10.Text= Convert.ToString(u1_pT(p,T));

        }

        public Double s1_pT(Double p, Double T)
        {
            //Function s1_pT(ByVal p As Double, ByVal T As Double) As Double

            //Release on the IAPWS Industrial Formulation 1997 for the Thermodynamic Properties of Water and Steam, September 1997
            //5 Equations for Region 1, Section. 5.1 Basic Equation
            //Equation 7, Table 3, Page 6

            Double g, gt;

            Double R = 0.461526; //kJ/(kg K)
            Double[] I1 = new Double[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 8, 8, 21, 23, 29, 30, 31, 32 };
            Double[] J1 = new Double[] { -2, -1, 0, 1, 2, 3, 4, 5, -9, -7, -1, 0, 1, 3, -3, 0, 1, 3, 17, -4, 0, 6, -5, -2, 10, -8, -11, -6, -29, -31, -38, -39, -40, -41 };
            Double[] n1 = new Double[] { 0.14632971213167, -0.84548187169114, -3.756360367204, 3.3855169168385, -0.95791963387872, 0.15772038513228, -0.016616417199501, 8.1214629983568E-04, 2.8319080123804E-04, -6.0706301565874E-04, -0.018990068218419, -0.032529748770505, -0.021841717175414, -5.283835796993E-05, -4.7184321073267E-04, -3.0001780793026E-04, 4.7661393906987E-05, -4.4141845330846E-06, -7.2694996297594E-16, -3.1679644845054E-05, -2.8270797985312E-06, -8.5205128120103E-10, -2.2425281908E-06, -6.5171222895601E-07, -1.4341729937924E-13, -4.0516996860117E-07, -1.2734301741641E-09, -1.7424871230634E-10, -6.8762131295531E-19, 1.4478307828521E-20, 2.6335781662795E-23, -1.1947622640071E-23, 1.8228094581404E-24, -9.3537087292458E-26 };

           

            p = p / 16.53;
            T = 1386 / T;
            g = 0;
            gt = 0;

            for (int i = 0; i <= 33; i++)
            {

                gt = gt + (n1[i] * Math.Pow((7.1 - p), I1[i]) * J1[i] * Math.Pow((T - 1.222), (J1[i] - 1)));
                g = g + n1[i] * Math.Pow((7.1 - p), I1[i]) * Math.Pow((T - 1.222), J1[i]);

            }

            
            return (R * T * gt - R * g);

        }


        private void button3_Click(object sender, EventArgs e)
        {
            Double p, T;
            p = 0;
            T = 0;
            p = Convert.ToDouble(textBox9.Text);
            T = Convert.ToDouble(textBox8.Text);
            textBox7.Text = Convert.ToString( s1_pT(p,T));
        }

        public Double Cp1_pT(Double p, Double T)
        {
            //Function Cp1_pT(ByVal p As Double, ByVal T As Double) As Double

            //Release on the IAPWS Industrial Formulation 1997 for the Thermodynamic Properties of Water and Steam, September 1997
            //5 Equations for Region 1, Section. 5.1 Basic Equation
            //Eqution 7, Table 3, Page 6

            Double Gtt;

            Double R = 0.461526; //kJ/(kg K)

            Double[] I1 = new Double[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 8, 8, 21, 23, 29, 30, 31, 32 };
            Double[] J1 = new Double[] { -2, -1, 0, 1, 2, 3, 4, 5, -9, -7, -1, 0, 1, 3, -3, 0, 1, 3, 17, -4, 0, 6, -5, -2, 10, -8, -11, -6, -29, -31, -38, -39, -40, -41 };
            Double[] n1 = new Double[] { 0.14632971213167, -0.84548187169114, -3.756360367204, 3.3855169168385, -0.95791963387872, 0.15772038513228, -0.016616417199501, 8.1214629983568E-04, 2.8319080123804E-04, -6.0706301565874E-04, -0.018990068218419, -0.032529748770505, -0.021841717175414, -5.283835796993E-05, -4.7184321073267E-04, -3.0001780793026E-04, 4.7661393906987E-05, -4.4141845330846E-06, -7.2694996297594E-16, -3.1679644845054E-05, -2.8270797985312E-06, -8.5205128120103E-10, -2.2425281908E-06, -6.5171222895601E-07, -1.4341729937924E-13, -4.0516996860117E-07, -1.2734301741641E-09, -1.7424871230634E-10, -6.8762131295531E-19, 1.4478307828521E-20, 2.6335781662795E-23, -1.1947622640071E-23, 1.8228094581404E-24, -9.3537087292458E-26 };

            p = p / 16.53;
            T = 1386 / T;
            Gtt = 0;

            for (int i = 0; i <= 33; i++)
            {

                Gtt = Gtt + (n1[i] * Math.Pow((7.1 - p), I1[i]) * J1[i] * (J1[i] - 1) * Math.Pow((T - 1.222), (J1[i] - 2)));

            }

            return (-R * Math.Pow(T, 2) * Gtt);

        }

        private void button9_Click(object sender, EventArgs e)
        {
            Double p,T;
            p=0;
            T=0;

            p = Convert.ToDouble(textBox27.Text);
            T = Convert.ToDouble(textBox26.Text);
            textBox25.Text = Convert.ToString(Cp1_pT(p,T));
        }

        public Double Cv1_pT(Double p, Double T)
        {

            //Function Cv1_pT(ByVal p As Double, ByVal T As Double) As Double

            //Release on the IAPWS Industrial Formulation 1997 for the Thermodynamic Properties of Water and Steam, September 1997
            //5 Equations for Region 1, Section. 5.1 Basic Equation
            //Eqution 7, Table 3, Page 6

            

            Double gp, gpp, gpt, Gtt;
            Double R = 0.461526; //kJ/(kg K)

            Double[] I1 = new Double[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 8, 8, 21, 23, 29, 30, 31, 32 };
            Double[] J1 = new Double[] { -2, -1, 0, 1, 2, 3, 4, 5, -9, -7, -1, 0, 1, 3, -3, 0, 1, 3, 17, -4, 0, 6, -5, -2, 10, -8, -11, -6, -29, -31, -38, -39, -40, -41 };
            Double[] n1 = new Double[] { 0.14632971213167, -0.84548187169114, -3.756360367204, 3.3855169168385, -0.95791963387872, 0.15772038513228, -0.016616417199501, 8.1214629983568E-04, 2.8319080123804E-04, -6.0706301565874E-04, -0.018990068218419, -0.032529748770505, -0.021841717175414, -5.283835796993E-05, -4.7184321073267E-04, -3.0001780793026E-04, 4.7661393906987E-05, -4.4141845330846E-06, -7.2694996297594E-16, -3.1679644845054E-05, -2.8270797985312E-06, -8.5205128120103E-10, -2.2425281908E-06, -6.5171222895601E-07, -1.4341729937924E-13, -4.0516996860117E-07, -1.2734301741641E-09, -1.7424871230634E-10, -6.8762131295531E-19, 1.4478307828521E-20, 2.6335781662795E-23, -1.1947622640071E-23, 1.8228094581404E-24, -9.3537087292458E-26 };

            p = p / 16.53;
            T = 1386 / T;

            gp = 0;
            gpp = 0;
            gpt = 0;
            Gtt = 0;

            for (int i = 0; i <= 33; i++)
            {
                gp = gp - n1[i] * I1[i] * Math.Pow((7.1 - p), (I1[i] - 1)) * Math.Pow((T - 1.222), J1[i]);
                gpp = gpp + n1[i] * I1[i] * (I1[i] - 1) * Math.Pow((7.1 - p), (I1[i] - 2)) * Math.Pow((T - 1.222), J1[i]);
                gpt = gpt - n1[i] * I1[i] * Math.Pow((7.1 - p), (I1[i] - 1)) * J1[i] * Math.Pow((T - 1.222), (J1[i] - 1));
                Gtt = Gtt + n1[i] * Math.Pow((7.1 - p), I1[i]) * J1[i] * (J1[i] - 1) * Math.Pow((T - 1.222), (J1[i] - 2));
            }

           return (R * (-(Math.Pow(T, 2) * Gtt) + Math.Pow((gp - T * gpt), 2) / gpp));

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Double p, T;
            p = 0;
            T = 0;

            p = Convert.ToDouble(textBox18.Text);
            T = Convert.ToDouble(textBox17.Text);

            textBox16.Text = Convert.ToString(Cv1_pT(p,T));
        }

        public Double w1_pT(Double p,Double T)
        {
            //Function w1_pT(ByVal p As Double, ByVal T As Double) As Double

            //Release on the IAPWS Industrial Formulation 1997 for the Thermodynamic Properties of Water and Steam, September 1997
            //5 Equations for Region 1, Section. 5.1 Basic Equation
            //Eqution 7, Table 3, Page 6


            Double gp, gpp, gpt, Gtt, tau;
            Double R = 0.461526; //kJ/(kg K)

            Double[] I1 = new Double[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 8, 8, 21, 23, 29, 30, 31, 32 };
            Double[] J1 = new Double[] { -2, -1, 0, 1, 2, 3, 4, 5, -9, -7, -1, 0, 1, 3, -3, 0, 1, 3, 17, -4, 0, 6, -5, -2, 10, -8, -11, -6, -29, -31, -38, -39, -40, -41 };
            Double[] n1 = new Double[] { 0.14632971213167, -0.84548187169114, -3.756360367204, 3.3855169168385, -0.95791963387872, 0.15772038513228, -0.016616417199501, 8.1214629983568E-04, 2.8319080123804E-04, -6.0706301565874E-04, -0.018990068218419, -0.032529748770505, -0.021841717175414, -5.283835796993E-05, -4.7184321073267E-04, -3.0001780793026E-04, 4.7661393906987E-05, -4.4141845330846E-06, -7.2694996297594E-16, -3.1679644845054E-05, -2.8270797985312E-06, -8.5205128120103E-10, -2.2425281908E-06, -6.5171222895601E-07, -1.4341729937924E-13, -4.0516996860117E-07, -1.2734301741641E-09, -1.7424871230634E-10, -6.8762131295531E-19, 1.4478307828521E-20, 2.6335781662795E-23, -1.1947622640071E-23, 1.8228094581404E-24, -9.3537087292458E-26 };

            p = p / 16.53;
            tau = 1386 / T;

            gp = 0;
            gpp = 0;
            gpt = 0;
            Gtt = 0;

            for (int i = 0; i <= 33; i++)
            {
                gp = gp - n1[i] * I1[i] * Math.Pow((7.1 - p), (I1[i] - 1)) * Math.Pow((tau - 1.222), J1[i]);
                gpp = gpp + n1[i] * I1[i] * (I1[i] - 1) * Math.Pow((7.1 - p), (I1[i] - 2)) * Math.Pow((tau - 1.222), J1[i]);
                gpt = gpt - n1[i] * I1[i] * Math.Pow((7.1 - p), (I1[i] - 1)) * J1[i] * Math.Pow((tau - 1.222), (J1[i] - 1));
                Gtt = Gtt + n1[i] * Math.Pow((7.1 - p), I1[i]) * J1[i] * (J1[i] - 1) * Math.Pow((tau - 1.222), (J1[i] - 2));
            }

            return (Math.Pow((1000 * R * T * Math.Pow(gp, 2) / (Math.Pow((gp - tau * gpt), 2) / (Math.Pow(tau, 2) * Gtt) - gpp)), 0.5));

        }
        
        private void button5_Click(object sender, EventArgs e)
        {
             Double p,T;
             p=0;
             T=0;
             p = Convert.ToDouble(textBox15.Text);
             T = Convert.ToDouble(textBox14.Text);
             textBox13.Text = Convert.ToString(w1_pT(p,T));
        
        }

        public Double T1_ph(Double p,Double h)
        {
            //Function T1_ph(ByVal p As Double, ByVal h As Double) As Double

            //Release on the IAPWS Industrial Formulation 1997 for the Thermodynamic Properties of Water and Steam, September 1997
            //5 Equations for Region 1, Section. 5.1 Basic Equation, 5.2.1 The Backward Equation T ( p,h )
            //Eqution 11, Table 6, Page 10

            Double T;

            Double[] I1 = new Double[] { 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 2, 2, 3, 3, 4, 5, 6 };
            Double[] J1 = new Double[] { 0, 1, 2, 6, 22, 32, 0, 1, 2, 3, 4, 10, 32, 10, 32, 10, 32, 32, 32, 32 };
            Double[] n1 = new Double[] { -238.72489924521, 404.21188637945, 113.49746881718, -5.8457616048039, -1.528548241314E-04, -1.0866707695377E-06, -13.391744872602, 43.211039183559, -54.010067170506, 30.535892203916, -6.5964749423638, 9.3965400878363E-03, 1.157364750534E-07, -2.5858641282073E-05, -4.0644363084799E-09, 6.6456186191635E-08, 8.0670734103027E-11, -9.3477771213947E-13, 5.8265442020601E-15, -1.5020185953503E-17 };

            h = h / 2500;
            T = 0;

            for (int i = 0; i <= 19; i++)
            {
                T = T + n1[i] * Math.Pow(p, I1[i]) * Math.Pow((h + 1), J1[i]);
            }

            return (T);
        
        }
      
        private void button8_Click(object sender, EventArgs e)
        {
           Double p,h;
           p=0;
           h=0;
           p = Convert.ToDouble(textBox24.Text);
           h = Convert.ToDouble(textBox23.Text);
           textBox22.Text = Convert.ToString(T1_ph(p,h));
        }


        public Double T1_ps(Double p, Double s)
        {
            //Function T1_ps(ByVal p As Double, ByVal s As Double) As Double

            //Release on the IAPWS Industrial Formulation 1997 for the Thermodynamic Properties of Water and Steam, September 1997
            //5 Equations for Region 1, Section. 5.1 Basic Equation, 5.2.2 The Backward Equation T ( p, s )
            //Eqution 13, Table 8, Page 11

            Double T1ps;

            Double[] I1 = new Double[] { 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 4 };
            Double[] J1 = new Double[] { 0, 1, 2, 3, 11, 31, 0, 1, 2, 3, 12, 31, 0, 1, 2, 9, 31, 10, 32, 32 };
            Double[] n1 = new Double[] { 174.78268058307, 34.806930892873, 6.5292584978455, 0.33039981775489, -1.9281382923196E-07, -2.4909197244573E-23, -0.26107636489332, 0.22592965981586, -0.064256463395226, 7.8876289270526E-03, 3.5672110607366E-10, 1.7332496994895E-24, 5.6608900654837E-04, -3.2635483139717E-04, 4.4778286690632E-05, -5.1322156908507E-10, -4.2522657042207E-26, 2.6400441360689E-13, 7.8124600459723E-29, -3.0732199903668E-31 };
            
            T1ps = 0;

            for (int i = 0; i <= 19; i++)
            {
                T1ps = T1ps + n1[i] * Math.Pow(p, I1[i]) * Math.Pow((s + 2), J1[i]);
            }

           return(T1ps);

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Double p, s;
            p = 0;
            s = 0;
            p = Convert.ToDouble(textBox21.Text);
            s = Convert.ToDouble(textBox20.Text);

            textBox19.Text = Convert.ToString(T1_ps(p,s));

        }

        public Double p1_hs(Double h, Double s)
        {

            //Function p1_hs(ByVal h As Double, ByVal s As Double) As Double

            //Supplementary Release on Backward Equations for Pressure as a Function of Enthalpy and Entropy p(h,s) to the IAPWS Industrial Formulation 1997 for the Thermodynamic Properties of Water and Steam
            //5 Backward Equation p(h,s) for Region 1
            //Eqution 1, Table 2, Page 5

            Double p;

            Double[] I1 = new Double[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 3, 4, 4, 5 };
            Double[] J1 = new Double[] { 0, 1, 2, 4, 5, 6, 8, 14, 0, 1, 4, 6, 0, 1, 10, 4, 1, 4, 0 };
            Double[] n1 = new Double[] { -0.691997014660582, -18.361254878756, -9.28332409297335, 65.9639569909906, -16.2060388912024, 450.620017338667, 854.68067822417, 6075.23214001162, 32.6487682621856, -26.9408844582931, -319.9478483343, -928.35430704332, 30.3634537455249, -65.0540422444146, -4309.9131651613, -747.512324096068, 730.000345529245, 1142.84032569021, -436.407041874559 };

            h = h / 3400;
            s = s / 7.6;
            p = 0;

            for (int i = 0; i <= 18; i++)
            {
                p = p + n1[i] * Math.Pow((h + 0.05), I1[i]) * Math.Pow((s + 0.05), J1[i]);
            }

            return(p * 100);
         

        }

        private void button10_Click(object sender, EventArgs e)
        {
            Double h, s;
            h = 0;
            s=0;

            h = Convert.ToDouble(textBox30.Text);
            s = Convert.ToDouble(textBox29.Text);

            textBox28.Text = Convert.ToString(p1_hs(h,s));
        }

        

      
   }
}
