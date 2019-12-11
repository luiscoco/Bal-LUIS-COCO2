using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Drag_AND_Drop_between_Forms
{
    public partial class Listados : Form
    {
        public Aplicacion puntero1=new Aplicacion();

        public Listados(Aplicacion puntero)
        {
            puntero1 = puntero;

            InitializeComponent();
        }

        //Botón VER LISTA ECUACIONES
        private void button1_Click(object sender, EventArgs e)
        {
            Double num = puntero1.p.Count;

            for (int a = 0; a < num; a++)
            {
                puntero1.p5.Add(puntero1.ptemp);
            }

            listBox1.Items.Clear();

            listBox1.Items.Add("Nº Ecuaciones:" + Convert.ToString(puntero1.numecuaciones));
            listBox1.Items.Add("Lista de Ecuaciones del Sitema:");

            for (int i = 0; i < puntero1.numecuaciones; i++)
            {
                listBox1.Items.Add(puntero1.ecuaciones[i]);
            }

            listBox1.Items.Add("");


            //Unidades Sistema Internacional
            if (puntero1.unidades == 1)
            {
                listBox1.Items.Add("Unidades: W(Kgr/sg), P(kPa), H(Kj/Kgr)");
                listBox1.Items.Add("");
            }

            //Unidades Métricas
            else if (puntero1.unidades == 2)
            {
                listBox1.Items.Add("Unidades: W(Kgr/sg), P(Bar), H(Kj/Kgr)");
                listBox1.Items.Add("");
            }
            //Unidades Británicas

            else if (puntero1.unidades == 0)
            {
                listBox1.Items.Add("Unidades: W(Lb/sg), P(psia), H(BTU/Lb)");
                listBox1.Items.Add("");
            }


            listBox1.Items.Add("Nº Variables:" + Convert.ToString(puntero1.numvariables));
            listBox1.Items.Add("Nombre de las variables:");
            listBox1.Items.Add("");

            for (int i = 0; i < num; i++)
            {
                //Unidades
                //Sistema Britanico=0;Sistema Internacional=1;Sistema Métrico=2

                //Unidades Sistema Internacional
                if (puntero1.unidades == 1)
                {
                    String primercaracter = puntero1.p[i].Nombre.Substring(0, 1);

                    if (primercaracter == "W")
                    {
                        puntero1.p5[i].Value = puntero1.p[i].Value * (0.4536);
                    }
                    else if (primercaracter == "P")
                    {
                        puntero1.p5[i].Value = puntero1.p[i].Value * (6.8947572);
                    }
                    else if (primercaracter == "H")
                    {
                        puntero1.p5[i].Value = puntero1.p[i].Value * 2.326009;
                    }

                    puntero1.p5[i].Nombre= puntero1.p[i].Nombre;
                    listBox1.Items.Add(puntero1.p5[i].ToString());
                }

                //Unidades Sistema Métrico
                else if (puntero1.unidades == 2)
                {
                    String primercaracter = puntero1.p[i].Nombre.Substring(0, 1);

                    if (primercaracter == "W")
                    {
                        puntero1.p5[i].Value = puntero1.p[i].Value * (0.4536);
                    }
                    else if (primercaracter == "P")
                    {
                        puntero1.p5[i].Value = puntero1.p[i].Value * (6.8947572 / 100);
                    }
                    else if (primercaracter == "H")
                    {
                        puntero1.p5[i].Value = puntero1.p[i].Value * 2.326009;
                    }

                    puntero1.p5[i].Nombre= puntero1.p[i].Nombre;
                    listBox1.Items.Add(puntero1.p5[i].ToString());

                }

                //Unidades Sistema Británico
                else if (puntero1.unidades == 0)
                {
                    puntero1.p5[i] = puntero1.p[i];
                    listBox1.Items.Add(puntero1.p5[i].ToString());
                }
            }

            puntero1.p5.Clear();

            listBox1.Items.Add("");
        }

        //Botón VER UN SOLO EQUIPO
        private void button2_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();

            Int32 indice=Convert.ToInt32(textBox1.Text);
            indice = indice - 1;

            String cadena = "Nº Equipo: " + Convert.ToString(puntero1.equipos11[indice].numequipo2)+"  Tipo de Equipo: "+Convert.ToString(puntero1.equipos11[indice].tipoequipo2);
            String cadena1 = "Corriente N1: " + Convert.ToString(puntero1.equipos11[indice].aN1) + "  Corriente N2: " + Convert.ToString(puntero1.equipos11[indice].aN2) + "  Corriente N3: " + Convert.ToString(puntero1.equipos11[indice].aN3) + "  Corriente N4: " + Convert.ToString(puntero1.equipos11[indice].aN4);
            String cadena2= "D1:"+Convert.ToString(puntero1.equipos11[indice].aD1)+"  "+"D2:"+Convert.ToString(puntero1.equipos11[indice].aD2)+"  "+"D3:"+Convert.ToString(puntero1.equipos11[indice].aD3)+"  "+"D4:"+Convert.ToString(puntero1.equipos11[indice].aD4)+"  "+"D5:"+Convert.ToString(puntero1.equipos11[indice].aD5)+"  "+"D6:"+Convert.ToString(puntero1.equipos11[indice].aD6)+"  "+"D7:"+Convert.ToString(puntero1.equipos11[indice].aD7)+"  "+"D8:"+Convert.ToString(puntero1.equipos11[indice].aD8)+"  "+"D9:"+Convert.ToString(puntero1.equipos11[indice].aD9);
           
            listBox2.Items.Add(cadena);
            //listBox2.Items.Add("");
            listBox2.Items.Add(cadena1);
            //listBox2.Items.Add("");
            listBox2.Items.Add(cadena2);
            listBox2.Items.Add("");
            listBox2.Items.Add("");
            listBox2.Items.Add("");
            //equipos11[indice].aD1;
          

            //Actualizar el Arbol de Equipos de la Aplicación
            String cadena11="Corriente N1: " + Convert.ToString(puntero1.equipos11[indice].aN1);
            String cadena12="Corriente N2: " + Convert.ToString(puntero1.equipos11[indice].aN2);
            String cadena13 ="Corriente N3: " + Convert.ToString(puntero1.equipos11[indice].aN3);
            String cadena14 ="Corriente N4: " + Convert.ToString(puntero1.equipos11[indice].aN4);
           
            String cadena15="D1: " + Convert.ToString(puntero1.equipos11[indice].aD1);
            String cadena16="D2: " + Convert.ToString(puntero1.equipos11[indice].aD2);
            String cadena17 ="D3: " + Convert.ToString(puntero1.equipos11[indice].aD3);
            String cadena18 ="D4: " + Convert.ToString(puntero1.equipos11[indice].aD4);
            String cadena19="D5: " + Convert.ToString(puntero1.equipos11[indice].aD5);
            String cadena20="D6: " + Convert.ToString(puntero1.equipos11[indice].aD6);
            String cadena21 ="D7: " + Convert.ToString(puntero1.equipos11[indice].aD7);
            String cadena22 ="D8: " + Convert.ToString(puntero1.equipos11[indice].aD8);
            String cadena23 ="D9: " + Convert.ToString(puntero1.equipos11[indice].aD9);
        }

        //Botón VER TODOS LOS EQUIPOS
        private void button3_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();

            for (int h = 0; h < puntero1.numequipos; h++)
            {
                String cadena = "Nº Equipo: " + Convert.ToString(puntero1.equipos11[h].numequipo2) + "  Tipo de Equipo: " + Convert.ToString(puntero1.equipos11[h].tipoequipo2);
                String cadena1 = "Corriente N1: " + Convert.ToString(puntero1.equipos11[h].aN1) + "  Corriente N2: " + Convert.ToString(puntero1.equipos11[h].aN2) + "  Corriente N3: " + Convert.ToString(puntero1.equipos11[h].aN3) + "  Corriente N4: " + Convert.ToString(puntero1.equipos11[h].aN4);
                String cadena2 = "D1:" + Convert.ToString(puntero1.equipos11[h].aD1) + "  " + "D2:" + Convert.ToString(puntero1.equipos11[h].aD2) + "  " + "D3:" + Convert.ToString(puntero1.equipos11[h].aD3) + "  " + "D4:" + Convert.ToString(puntero1.equipos11[h].aD4) + "  " + "D5:" + Convert.ToString(puntero1.equipos11[h].aD5) + "  " + "D6:" + Convert.ToString(puntero1.equipos11[h].aD6) + "  " + "D7:" + Convert.ToString(puntero1.equipos11[h].aD7) + "  " + "D8:" + Convert.ToString(puntero1.equipos11[h].aD8) + "  " + "D9:" + Convert.ToString(puntero1.equipos11[h].aD9);

                listBox2.Items.Add(cadena);
                //listBox2.Items.Add("");
                listBox2.Items.Add(cadena1);
                //listBox2.Items.Add("");
                listBox2.Items.Add(cadena2);
                listBox2.Items.Add("");
                //equipos11[indice].aD1;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
