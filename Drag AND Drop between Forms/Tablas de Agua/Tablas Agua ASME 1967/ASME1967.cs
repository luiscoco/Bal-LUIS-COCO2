using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TablasAgua1967
{
    public partial class ASME1967 : Form
    {
        public ASME1967()
        {
            InitializeComponent();
        }

        //Dato entrada Temperatura Saturación
        private void button1_Click(object sender, EventArgs e)
        {
            Class1 luis = new Class1();
            Double temperatura = Convert.ToDouble(textBox1.Text);
            Double presion = luis.psl(Convert.ToDouble(textBox1.Text),1);
            textBox2.Text = Convert.ToString(presion);
        }

        //Dato entrada Presión Saturación
        private void button2_Click(object sender, EventArgs e)
        {
            Class1 luis = new Class1();
            Double presion = Convert.ToDouble(textBox4.Text);
            Double temperatura = luis.tsl(Convert.ToDouble(textBox4.Text));
            textBox3.Text = Convert.ToString(temperatura);
            Double calidad = 100;
            Double entalpia = luis.shvptx(presion, temperatura, calidad, 2);
            textBox14.Text = Convert.ToString(entalpia);    
            Double entropia=luis.shvptx(presion, temperatura, calidad, 1);
            textBox6.Text = Convert.ToString(entropia);
        }

        //Dato entrada Presión y Entalpía
        private void button4_Click(object sender, EventArgs e)
        {

            Class1 luis2 = new Class1(); 
            Double presion = Convert.ToDouble(textBox7.Text);
            Double entalpia = Convert.ToDouble(textBox10.Text);
            Double calidad = luis2.xpshv(presion,entalpia,2);
            textBox11.Text = Convert.ToString(calidad);
            Double temperatura = luis2.tpshvx(presion,entalpia,calidad,2,1);
            textBox12.Text = Convert.ToString(temperatura);
            Double entropia = luis2.shvptx(presion,temperatura,calidad,1);
            textBox13.Text = Convert.ToString(entropia);
            Double volumenespecifico = luis2.shvptx(presion,temperatura,calidad,3);
            textBox21.Text = Convert.ToString(volumenespecifico);
        }

        //Dato entrada Presión y Entropía
        private void button3_Click(object sender, EventArgs e)
        {
            Class1 luis2 = new Class1();
            Double presion = Convert.ToDouble(textBox20.Text);
            Double entropia = Convert.ToDouble(textBox19.Text);
            Double calidad = luis2.xpshv(presion, entropia, 1);
            textBox18.Text = Convert.ToString(calidad);
            Double temperatura = luis2.tpshvx(presion, entropia, calidad, 1, 1);
            textBox17.Text = Convert.ToString(temperatura);
            Double entalpia = luis2.shvptx(presion, temperatura, calidad, 2);
            textBox16.Text = Convert.ToString(entalpia);

        }

        //Dato entrada Presión y Temperatura
        private void button5_Click(object sender, EventArgs e)
        {
            Class1 luis2 = new Class1();
            Double presion = Convert.ToDouble(textBox26.Text);
            Double temperatura = Convert.ToDouble(textBox25.Text);
            Double calidad = luis2.x_pt(presion, temperatura);
            textBox24.Text = Convert.ToString(calidad);
            Double entropia=luis2.shvptx(presion,temperatura,calidad,1);
            textBox23.Text = Convert.ToString(entropia);
            Double entalpia1 = luis2.shvptx(presion,temperatura,calidad,2);
            Double entalpia2 = luis2.shvptx(presion,temperatura+0.001,calidad,2);
            Double cp=(entalpia2-entalpia1)/(0.001);
            textBox22.Text = Convert.ToString(entalpia1);
            textBox60.Text = Convert.ToString(cp);
            Double voluesp = luis2.shvptx(presion,temperatura,calidad,3);
            textBox61.Text = Convert.ToString(voluesp);

        }

        //Datos entrada Temperatura y Entalpía
        private void button6_Click(object sender, EventArgs e)
        {
            Class1 luis2 = new Class1();
            Double temperatura = Convert.ToDouble(textBox31.Text);
            Double entalpia = Convert.ToDouble(textBox30.Text);
            Double calidad = luis2.xtshv(temperatura, entalpia, 2);
            textBox29.Text = Convert.ToString(calidad);
            Double presion = luis2.tpshvx(temperatura, entalpia, calidad, 2, 2);
            textBox27.Text = Convert.ToString(presion);
            Double entropia = luis2.shvptx(presion,temperatura,calidad,1);
            textBox28.Text = Convert.ToString(entropia);
        }

        //Datos entrada Temperatura y Entropia
        private void button7_Click(object sender, EventArgs e)
        {
            Class1 luis2 = new Class1();
            Double temperatura=Convert.ToDouble(textBox36.Text);
            Double entropia = Convert.ToDouble(textBox35.Text);
            Double calidad = luis2.xtshv(temperatura,entropia,1);
            textBox34.Text = Convert.ToString(calidad);
            Double presion = luis2.tpshvx(temperatura, entropia, calidad, 1, 2);
            textBox32.Text = Convert.ToString(presion);
            Double entalpia = luis2.shvptx(presion,temperatura,calidad,2);
            textBox33.Text = Convert.ToString(entalpia);
        }

        //Datos entrada Entalpía y Entropía
        private void button8_Click(object sender, EventArgs e)
        {
            Class1 luis2 = new Class1();
            Double entalpia = Convert.ToDouble(textBox41.Text);
            Double entropia = Convert.ToDouble(textBox40.Text);
            Double presion = luis2.phs(entalpia,entropia);
            textBox37.Text = Convert.ToString(presion);
            Double calidad = luis2.xpshv(presion,entalpia,2);
            textBox39.Text = Convert.ToString(calidad);
            Double temperatura = luis2.tpshvx(presion,entalpia,calidad,2,1);
            textBox38.Text = Convert.ToString(temperatura);
        }

        //Datos de entrada Presión y Título
        private void button10_Click(object sender, EventArgs e)
        {
            Class1 luis2=new Class1();
            Double presion = Convert.ToDouble(textBox51.Text);
            Double calidad = Convert.ToDouble(textBox50.Text);
            Double temperaturasaturacion = luis2.tsl(presion);

            //Liquido saturado
            Double calidadliquidosaturado = 0;
            Double entalpialiquido = luis2.shvptx(presion,temperaturasaturacion,calidadliquidosaturado,2);
            textBox53.Text = Convert.ToString(entalpialiquido);
            Double entropialiquido = luis2.shvptx(presion, temperaturasaturacion, calidadliquidosaturado, 1);
            textBox55.Text = Convert.ToString(entropialiquido);

            //Vapor saturado
            Double calidadvaporsaturado = 100;
            Double entalpiavapor = luis2.shvptx(presion,temperaturasaturacion,calidadvaporsaturado,2);
            textBox52.Text = Convert.ToString(entalpiavapor);
            Double entropiavapor = luis2.shvptx(presion, temperaturasaturacion, calidadvaporsaturado, 1);
            textBox54.Text = Convert.ToString(entropiavapor);

            //Mezcla liquido-vapor
            Double entalpia=(entalpialiquido*(1-calidad))+(calidad*entalpiavapor);
            textBox48.Text = Convert.ToString(entalpia);
            Double temperatura = luis2.tpshvx(presion, entalpia, calidad, 2, 1);
            textBox49.Text = Convert.ToString(temperatura);
            Double entropia = (entropialiquido * (1 - calidad)) + (calidad * entropiavapor);
            textBox47.Text = Convert.ToString(entropia);
        }

        //Datos de entrada Temperatura y Título
        private void button9_Click(object sender, EventArgs e)
        {
            Class1 luis2 = new Class1();
            Double temperatura = Convert.ToDouble(textBox46.Text);
            Double calidad = Convert.ToDouble(textBox45.Text);
            Double presionsaturacion = luis2.psl(temperatura,1);

            //Liquido saturado
            Double calidadliquidosaturado = 0;
            Double entalpialiquido = luis2.shvptx(presionsaturacion, temperatura, calidadliquidosaturado, 2);
            textBox59.Text = Convert.ToString(entalpialiquido);
            Double entropialiquido = luis2.shvptx(presionsaturacion, temperatura, calidadliquidosaturado, 1);
            textBox57.Text = Convert.ToString(entropialiquido);

            //Vapor saturado
            Double calidadvaporsaturado = 100;
            Double entalpiavapor = luis2.shvptx(presionsaturacion, temperatura, calidadvaporsaturado, 2);
            textBox58.Text = Convert.ToString(entalpiavapor);
            Double entropiavapor = luis2.shvptx(presionsaturacion, temperatura, calidadvaporsaturado, 1);
            textBox56.Text = Convert.ToString(entropiavapor);

            //Mezcla liquido-vapor
            Double entalpia = (entalpialiquido * (1 - calidad)) + (calidad * entalpiavapor);
            textBox43.Text = Convert.ToString(entalpia);
            Double presion = luis2.tpshvx(temperatura,entalpia,calidad,2,2);
            textBox42.Text = Convert.ToString(presion);
            Double entropia = (entropialiquido * (1-calidad))+(calidad*entropiavapor);
            textBox44.Text = Convert.ToString(entropia);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Class1 luis2 = new Class1();
            Double presion=Convert.ToDouble(textBox66.Text);
            Double titulo=Convert.ToDouble(textBox65.Text);

            Double entalpia=luis2.hpx(presion, titulo);
            Double entropia = luis2.sph(presion, entalpia);
            
            textBox63.Text = Convert.ToString(entalpia);
            textBox62.Text=Convert.ToString(entropia);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Class1 luis2 = new Class1();

            Double presion1 = Convert.ToDouble(textBox69.Text);
            Double entalpia1 = Convert.ToDouble(textBox68.Text);

            Double presion2 = Convert.ToDouble(textBox67.Text);
            Double entalpia2 = Convert.ToDouble(textBox64.Text);

            Double vmedio = luis2.vmediaph(presion1, presion2, entalpia1, entalpia2);
            textBox70.Text=Convert.ToString(vmedio);
        }


        //Cálculo de la Viscosidad Dinámica y Cinemática en función de la Temperatura y Densidad del Agua
        private void button13_Click(object sender, EventArgs e)
        {
            Class1 luis2 = new Class1();

            Double temperatura = Convert.ToDouble(textBox72.Text);
            Double densidad = Convert.ToDouble(textBox71.Text);

            Double visdinamica = luis2.vistv(temperatura, densidad);
            Double viscinematica = visdinamica / densidad;

            textBox74.Text = Convert.ToString(visdinamica);
            textBox73.Text = Convert.ToString(viscinematica);
        }

       

     
    }
}
