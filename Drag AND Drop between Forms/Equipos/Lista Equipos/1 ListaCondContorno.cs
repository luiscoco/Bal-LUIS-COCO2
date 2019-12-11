using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ClaseEquipos;

namespace Drag_AND_Drop_between_Forms
{
    public partial class ListaCondContorno : Form
    {
        Aplicacion puntero1;

        public ListaCondContorno(Aplicacion puntero)
        {
            puntero1 = puntero;
            
            InitializeComponent();
        }

        //Función Load de la Lista de Condicones de Contorno
        private void ListaCondContorno_Load(object sender, EventArgs e)
        {
            LeerEquipos();
        }

        //Botón NEW de la Lista de Objetos Condición de Contorno
        private void button2_Click(object sender, EventArgs e)
        {
            puntero1.numequipos++;
            //Argumento 4 del constructor de la Clase Condcontorno :New =1
            Condcontorno condcontorno1 = new Condcontorno(puntero1, puntero1.numecuaciones, puntero1.numvariables,0,0);
            if (condcontorno1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
               listBox1.Items.Add("Equipo Nº: "+Convert.ToString(puntero1.equipos11[puntero1.numequipos-1].numequipo2)+"   Tipo Equipo: " +Convert.ToString(1));
            }            
        }

        //Función para leer los Objetos de la lista de Equipos (equipos11) de la aplicación principal
        private void LeerEquipos()
        {
            for (int i = 0; i <puntero1.equipos11.Count; i++)
            {
                if (puntero1.equipos11[i].tipoequipo2 == 1)
                {
                    listBox1.Items.Add("Equipo Nº: " + Convert.ToString(puntero1.equipos11[i].numequipo2) + "   Tipo Equipo: " + Convert.ToString(puntero1.equipos11[i].tipoequipo2));
                }
            } 
        }

        //Botón de OK
        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        
        //Botón de CANCEL
        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        //Botón EDIT (editar un objeto de la Clase equipo11)
        private void button1_Click(object sender, EventArgs e)
        {
            String elemento;
            Int32 numeroequipo11=0;

            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                if (listBox1.GetSelected(i) == true)
                {
                    elemento = listBox1.Items[i].ToString();
                    numeroequipo11 = Convert.ToInt32(elemento.Substring(10, 4));
                }
            }

            int indice=0;
            int marca = 0;

            for (int j = 0; j < puntero1.equipos11.Count;j++)
            {
                if (puntero1.equipos11[j].numequipo2 == numeroequipo11)
                {
                    indice = j;
                    marca = 1;
                    goto maria;
                }
            }

            if (marca == 0)
            {
                MessageBox.Show("Error no se ha encontrado el número de Equipo en la lista de Equipos.");
            }

            maria:

            Condcontorno cond1=new Condcontorno(puntero1, puntero1.numecuaciones, puntero1.numvariables,1,indice);

            //Unidades
            //Sistema Britanico=0;Sistema Internacional=2;Sistema Métrico=1

            //Dependiendo de las unidades elegidas en la Aplicación principal se realiza una conversión 
            //de los valores de los parámetros (D1 a D9) guardados en el array de equipos "equipos11" para visualizarlo en el cuadro de diálogo en las unidades elegidas
            //Hay que tener en cuenta que dentro del array equipos11 siempre se guardan los parámetros (D1 al D9) en unidades del Sistema Británico porque son las utilizadas por las Tablas de Vapor ASME

            //Si las Unidades de la Aplicación son del Sistema Métrico

            if (puntero1.unidades == 1)
            {
                //Caudal  Lb/sg a Kgr/sg
                cond1.textBox1.Text = Convert.ToString(puntero1.equipos11[indice].aD1 * 0.4536);

                //Presión  psia a Bar
                cond1.textBox2.Text = Convert.ToString(puntero1.equipos11[indice].aD2 * (6.8947572 / 100));

                //Entalpia  Btu/Lb a Kj/Kgr
                cond1.textBox3.Text = Convert.ToString(puntero1.equipos11[indice].aD3 * 2.326009);

                cond1.textBox4.Text = Convert.ToString(puntero1.equipos11[indice].aD5);

                if (puntero1.equipos11[indice].aD6> 0)
                {
                    Double te = puntero1.equipos11[indice].aD6;
                    //Presión de psia a Bar
                    puntero1.equipos11[indice].aD6 = te * (6.8947572 / 100);
                    cond1.textBox5.Text = Convert.ToString(puntero1.equipos11[indice].aD6);
                }

                else if (puntero1.equipos11[indice].aD6 < 0)
                {
                    //Convertir los grados ºF en ºC
                    cond1.textBox5.Text = Convert.ToString((puntero1.equipos11[indice].aD6 - 32) * (5 / 9));
                }

                cond1.textBox6.Text = Convert.ToString(puntero1.equipos11[indice].aD7);
            }

            //Si las Unidades de la Aplicación son del Sistema Internacional
            else if (puntero1.unidades == 2)
            {
                //Caudal  Lb/sg a Kgr/sg
                cond1.textBox1.Text = Convert.ToString(puntero1.equipos11[indice].aD1 * 0.4536);
               
                //Presión psia a Bar
                cond1.textBox2.Text = Convert.ToString(puntero1.equipos11[indice].aD2 * (6.8947572 / 100));

                //Entalpia Btu/Lb a Kj/Kgr 
                cond1.textBox3.Text = Convert.ToString(puntero1.equipos11[indice].aD3 * 2.326009);

                cond1.textBox4.Text = Convert.ToString(puntero1.equipos11[indice].aD5);

                if (puntero1.equipos11[indice].aD6> 0)
                {
                    Double te = puntero1.equipos11[indice].aD6;
                    //Presión de  psia a Bar
                    puntero1.equipos11[indice].aD6 = te * (6.8947572 / 100);
                    cond1.textBox5.Text = Convert.ToString(puntero1.equipos11[indice].aD6);
                }

                else if (puntero1.equipos11[indice].aD6 < 0)
                {
                    //Convertir los grados ºF en ºC
                    cond1.textBox5.Text = Convert.ToString((puntero1.equipos11[indice].aD6-32)*(5/9));
                }

                cond1.textBox6.Text = Convert.ToString(puntero1.equipos11[indice].aD7);
            }

            //Si las Unidades de la Aplicación son del Sistema Británico
            else if (puntero1.unidades == 0)
            {
                //Caudal Lb/sg
                cond1.textBox1.Text = Convert.ToString(puntero1.equipos11[indice].aD1);

                //Presión psia
                cond1.textBox2.Text = Convert.ToString(puntero1.equipos11[indice].aD2);

                //Entalpia Btu/Lb
                cond1.textBox3.Text = Convert.ToString(puntero1.equipos11[indice].aD3);

                cond1.textBox4.Text = Convert.ToString(puntero1.equipos11[indice].aD5);

                if ((puntero1.equipos11[indice].aD6 > 0)||(puntero1.equipos11[indice].aD6 < 0))
                {
                    cond1.textBox5.Text = Convert.ToString(puntero1.equipos11[indice].aD6);
                }

                cond1.textBox6.Text = Convert.ToString(puntero1.equipos11[indice].aD7);
            }

            cond1.textBox7.Text=Convert.ToString(puntero1.equipos11[indice].aN1);
            cond1.textBox8.Text = Convert.ToString(puntero1.equipos11[indice].aN3);
            cond1.textBox9.Text = Convert.ToString(puntero1.equipos11[indice].numequipo2);

            cond1.ShowDialog();

            //Leemos la lista de Equipos ya actualizada
            listBox1.Items.Clear();
            LeerEquipos();
        }

        //Botón DELETE (eliminar un objeto de la Clase equipo11)
        private void button3_Click(object sender, EventArgs e)
        {
            String elemento;
            Int32 numeroequipo11=0;

            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                if (listBox1.GetSelected(i) == true)
                {
                    elemento = listBox1.Items[i].ToString();
                    numeroequipo11 = Convert.ToInt32(elemento.Substring(10, 4));
                }
            }

            int indice=0;
            int marca = 0;

            for (int j = 0; j < puntero1.equipos11.Count;j++)
            {
                if (puntero1.equipos11[j].numequipo2 == numeroequipo11)
                {
                    indice = j;
                    marca = 1;
                    goto maria;
                }
            }

            if (marca == 0)
            {
                MessageBox.Show("Error no se ha encontrado el número de Equipo en la lista de Equipos.");
            }

            maria:

            puntero1.equipos11.RemoveAt(indice);

            //Leemos la lista de Equipos ya actualizada
            listBox1.Items.Clear();
            LeerEquipos();
        }
    }
}
