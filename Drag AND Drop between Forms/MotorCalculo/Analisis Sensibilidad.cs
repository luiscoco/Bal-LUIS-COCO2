using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ClaseEquiposSensibilidad;
using ClaseEquipos;

namespace Drag_AND_Drop_between_Forms
{
    public partial class Analisis_Sensibilidad : Form
    {
        //Puntero a la Aplicación Principal
        public Aplicacion puntero;
        
        //Número de los Equipos a los que se les realiza Análisis de Sensibilidad
        public Int16[] sensibilidadequipos=new Int16[100];
        
        //Número de Equipos a los que se les realiza Análisis de Sensibilidad
        public Int16 contadorequipos = 0;

        //Lista de Equipos a los cuales se les va a realziar la prueba de sensiblidad
        public List< EquiposSensibilidad> listaequiposensibilidad=new List<EquiposSensibilidad> ();
        public EquiposSensibilidad equiposensibilidadtemp = new EquiposSensibilidad();

        public Analisis_Sensibilidad(Aplicacion puntero1)
        {
            puntero = puntero1;

            InitializeComponent();
        }

        //Botón CANCEL
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        //Botón OK
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        //Botón ADD. Añadir al Control ListBox1 el Tipo de Equipo y las Variables a estudiar su Sensibilidad
        private void button3_Click(object sender, EventArgs e)
        {            
            try
            {   
                equiposensibilidadtemp.numequipo2 = Convert.ToDouble(textBox1.Text);

                sensibilidadequipos[contadorequipos] = Convert.ToInt16(textBox1.Text);

                listBox1.Items.Add("Equipment Number: " + textBox1.Text);
                listBox1.Items.Add("Equipment Type: " + textBox30.Text);

                if ((checkBox1.Checked == true) && (Convert.ToDouble(textBox3.Text) != 0))
                {
                    listBox1.Items.Add("Range From: " + textBox3.Text +  "  Increment: " + textBox2.Text);
                    equiposensibilidadtemp.aD1 = true;
                    equiposensibilidadtemp.fromD1 = Convert.ToDouble(textBox3.Text);
                    equiposensibilidadtemp.incrementD1 = Convert.ToDouble(textBox2.Text);
                }

                if ((checkBox2.Checked == true) && (Convert.ToDouble(textBox6.Text) != 0))
                {
                    listBox1.Items.Add("Range From: " + textBox6.Text +"  Increment: " + textBox7.Text);
                    equiposensibilidadtemp.aD2 = true;
                    equiposensibilidadtemp.fromD2 = Convert.ToDouble(textBox6.Text);
                    equiposensibilidadtemp.incrementD2 = Convert.ToDouble(textBox7.Text);
                }

                if ((checkBox4.Checked == true) && (Convert.ToDouble(textBox12.Text) != 0))
                {
                    listBox1.Items.Add("Range From: " + textBox12.Text +  "  Increment: " + textBox13.Text);
                    equiposensibilidadtemp.aD3 = true;
                    equiposensibilidadtemp.fromD3 = Convert.ToDouble(textBox12.Text);
                    equiposensibilidadtemp.incrementD3 = Convert.ToDouble(textBox13.Text);
                }

                if ((checkBox3.Checked == true) && (Convert.ToDouble(textBox9.Text) != 0))
                {
                    listBox1.Items.Add("Range From: " + textBox9.Text +  "  Increment: " + textBox10.Text);
                    equiposensibilidadtemp.aD4 = true;
                    equiposensibilidadtemp.fromD4 = Convert.ToDouble(textBox9.Text);
                    equiposensibilidadtemp.incrementD4 = Convert.ToDouble(textBox10.Text);
                }

                if ((checkBox8.Checked == true) && (Convert.ToDouble(textBox24.Text) != 0))
                {
                    listBox1.Items.Add("Range From: " + textBox24.Text +  "  Increment: " + textBox25.Text);
                    equiposensibilidadtemp.aD5 = true;
                    equiposensibilidadtemp.fromD5 = Convert.ToDouble(textBox24.Text);
                    equiposensibilidadtemp.incrementD5 = Convert.ToDouble(textBox25.Text);
                }

                if ((checkBox7.Checked == true) && (Convert.ToDouble(textBox21.Text) != 0))
                {
                    listBox1.Items.Add("Range From: " + textBox21.Text +  "  Increment: " + textBox22.Text);
                    equiposensibilidadtemp.aD6 = true;
                    equiposensibilidadtemp.fromD6 = Convert.ToDouble(textBox21.Text);
                    equiposensibilidadtemp.incrementD6 = Convert.ToDouble(textBox22.Text);
                }

                if ((checkBox6.Checked == true) && (Convert.ToDouble(textBox18.Text) != 0))
                {
                    listBox1.Items.Add("Range From: " + textBox18.Text+ "  Increment: " + textBox19.Text);
                    equiposensibilidadtemp.aD7 = true;
                    equiposensibilidadtemp.fromD7 = Convert.ToDouble(textBox18.Text);
                    equiposensibilidadtemp.incrementD7 = Convert.ToDouble(textBox19.Text);
                }

                if ((checkBox5.Checked == true) && (Convert.ToDouble(textBox15.Text) != 0))
                {
                    listBox1.Items.Add("Range From: " + textBox15.Text +  "  Increment: " + textBox16.Text);
                    equiposensibilidadtemp.aD8 = true;
                    equiposensibilidadtemp.fromD8 = Convert.ToDouble(textBox15.Text);
                    equiposensibilidadtemp.incrementD8 = Convert.ToDouble(textBox16.Text);
                }

                if ((checkBox9.Checked == true) && (Convert.ToDouble(textBox27.Text) != 0))
                {
                    listBox1.Items.Add("Range From: " + textBox27.Text + "  Increment: " + textBox28.Text);
                    equiposensibilidadtemp.aD9 = true;
                    equiposensibilidadtemp.fromD9 = Convert.ToDouble(textBox27.Text);
                    equiposensibilidadtemp.incrementD9 = Convert.ToDouble(textBox28.Text);
                }

                listBox1.Items.Add("");

                listaequiposensibilidad.Add(equiposensibilidadtemp);
                contadorequipos++;

                //Activamos el Botón ADD del cuadro de diálogo de Análisis de Sensibilidad
                button4.Enabled = true;
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error :" + ex);
            }
        }

        //Run Sensivity Analysis
        private void button4_Click(object sender, EventArgs e)
        {
            
//----------Inicio del Bucle del Número de cálculos a Ejecutar en el Análisis de Sensibilidad
            Int16 numeroiteraciones =Convert.ToInt16(textBox29.Text);

            for (int j = 0; j < numeroiteraciones; j++)
            {              
                //Modificar-Editar los Datos de Entrada de los Equipos elegidos con el Incremento elegido.
                for (int k=0;k<contadorequipos;k++)
                {
                    for (int i=0;i<puntero.equipos11.Count;i++)
                    {
                        if(puntero.equipos11[i].numequipo2==sensibilidadequipos[k])
                        {
                            //MessageBox.Show("Equipo número:"+Convert.ToString(puntero.equipos11[i].numequipo2));
                            if (listaequiposensibilidad[k].aD1 == true)
                            {
                                puntero.equipos11[i].aD1 = listaequiposensibilidad[k].fromD1 + listaequiposensibilidad[k].incrementD1 * j;
                                //MessageBox.Show("D1:" + Convert.ToString(puntero.equipos11[i].aD1));                         
                            }

                            if (listaequiposensibilidad[k].aD2 == true)
                            {
                                puntero.equipos11[i].aD2 = listaequiposensibilidad[k].fromD2 + listaequiposensibilidad[k].incrementD2 * j;
                                //MessageBox.Show("D2:" + Convert.ToString(puntero.equipos11[i].aD2));
                            }

                            if (listaequiposensibilidad[k].aD3 == true)
                            {
                                puntero.equipos11[i].aD3 = listaequiposensibilidad[k].fromD3 + listaequiposensibilidad[k].incrementD3 * j;
                                //MessageBox.Show("D3:" + Convert.ToString(puntero.equipos11[i].aD3));
                            }

                            if (listaequiposensibilidad[k].aD4 == true)
                            {
                                puntero.equipos11[i].aD4 = listaequiposensibilidad[k].fromD4 + listaequiposensibilidad[k].incrementD4 * j;
                                //MessageBox.Show("D4:" + Convert.ToString(puntero.equipos11[i].aD4));
                            }

                            if (listaequiposensibilidad[k].aD5 == true)
                            {
                                puntero.equipos11[i].aD5 = listaequiposensibilidad[k].fromD5 + listaequiposensibilidad[k].incrementD5 * j;
                                //MessageBox.Show("D5:" + Convert.ToString(puntero.equipos11[i].aD5));
                            }

                            if (listaequiposensibilidad[k].aD6 == true)
                            {
                                puntero.equipos11[i].aD6 = listaequiposensibilidad[k].fromD6 + listaequiposensibilidad[k].incrementD6 * j;
                                //MessageBox.Show("D6:" + Convert.ToString(puntero.equipos11[i].aD6));
                            }

                            if (listaequiposensibilidad[k].aD7 == true)
                            {
                                puntero.equipos11[i].aD7 = listaequiposensibilidad[k].fromD7 + listaequiposensibilidad[k].incrementD7 * j;
                                //MessageBox.Show("D7:" + Convert.ToString(puntero.equipos11[i].aD7));
                            }

                            if (listaequiposensibilidad[k].aD8 == true)
                            {
                                puntero.equipos11[i].aD8 = listaequiposensibilidad[k].fromD8 + listaequiposensibilidad[k].incrementD8 * j;
                                //MessageBox.Show("D8:" + Convert.ToString(puntero.equipos11[i].aD8));
                            }

                            if (listaequiposensibilidad[k].aD9 == true)
                            {
                                puntero.equipos11[i].aD9 = listaequiposensibilidad[k].fromD9 + listaequiposensibilidad[k].incrementD9 * j;
                                //MessageBox.Show("D9:" + Convert.ToString(puntero.equipos11[i].aD9));
                            }
                        }
                    }
                }              
                
                //Llamada al Motor de Cálculo para Ejecutar el Primer Cálculo Automático
                puntero.resoluciónSistemaEcuacionesNoLinealesToolStripMenuItem_Click(sender, e);

                //Cada vez que acabamos el cálculo inicializamos las corrientes y las funciones las condiciones iniciales ya se han inicializado con los valores resultados de las corrientes cuando se pulsa automáticamente 
                //el button4 de OK del motor de cálculos que llama a su vez al botón de Copiar las Condiciones Iniciales.
                puntero.marca = 0;  

                if (j != numeroiteraciones-1)
                {

                    //Llamamos a la Función de Nuevo Cálculo (set number)
                    puntero.newCalculationSetToolStripMenuItem_Click(sender, e);
                }                         
            }

//----------------------------------------- Fin de Bucle del Número de cálculos en el Análisis de Sensibilidad -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        }

        //Después de presionar ENTER al introducir el Número de Equipo a Editar
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(System.ConsoleKey.Enter))
            {
                for (int g = 0; g < puntero.equipos11.Count; g++)
                {
                    if (puntero.equipos11[g].numequipo2 == Convert.ToDouble(textBox1.Text))
                    {
                        textBox30.Text = Convert.ToString(puntero.equipos11[g].tipoequipo2);
                    }
                }

                for (int h = 0; h < puntero.equipos11.Count;h++)
                {
                    if (puntero.equipos11[h].numequipo2==Convert.ToDouble(textBox1.Text))
                    {
                        textBox3.Text = Convert.ToString(puntero.equipos11[h].aD1);
                    }

                    if (puntero.equipos11[h].numequipo2 == Convert.ToDouble(textBox1.Text))
                    {
                        textBox6.Text = Convert.ToString(puntero.equipos11[h].aD2);
                    }

                    if (puntero.equipos11[h].numequipo2 == Convert.ToDouble(textBox1.Text))
                    {
                        textBox12.Text = Convert.ToString(puntero.equipos11[h].aD3);
                    }

                    if (puntero.equipos11[h].numequipo2 == Convert.ToDouble(textBox1.Text))
                    {
                        textBox9.Text = Convert.ToString(puntero.equipos11[h].aD4);
                    }

                    if (puntero.equipos11[h].numequipo2 == Convert.ToDouble(textBox1.Text))
                    {
                        textBox24.Text = Convert.ToString(puntero.equipos11[h].aD5);
                    }

                    if (puntero.equipos11[h].numequipo2 == Convert.ToDouble(textBox1.Text))
                    {
                        textBox21.Text = Convert.ToString(puntero.equipos11[h].aD6);
                    }

                    if (puntero.equipos11[h].numequipo2 == Convert.ToDouble(textBox1.Text))
                    {
                        textBox18.Text = Convert.ToString(puntero.equipos11[h].aD7);
                    }

                    if (puntero.equipos11[h].numequipo2 == Convert.ToDouble(textBox1.Text))
                    {
                        textBox15.Text = Convert.ToString(puntero.equipos11[h].aD8);
                    }

                    if (puntero.equipos11[h].numequipo2 == Convert.ToDouble(textBox1.Text))
                    {
                        textBox27.Text = Convert.ToString(puntero.equipos11[h].aD9);
                    }
                }
            }
        }
    }
}
