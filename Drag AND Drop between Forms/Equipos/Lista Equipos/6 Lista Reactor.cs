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
    public partial class ListaReactor : Form
    {
        Aplicacion puntero1;

        public ListaReactor(Aplicacion puntero)
        {
            puntero1 = puntero;
            
            InitializeComponent();
        }

        //Función Load de la Lista de Condicones de Contorno
        private void ListaReactor_Load(object sender, EventArgs e)
        {
            LeerEquipos();
        }

        //Botón NEW de la Lista de Objetos Condición de Contorno
        private void button2_Click(object sender, EventArgs e)
        {
            puntero1.numequipos++;
            //Argumento 4 del constructor de la Clase Condcontorno :New =1
            Reactor reactor6 = new Reactor(puntero1, puntero1.numecuaciones, puntero1.numvariables,0,0);
            if (reactor6.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
               listBox1.Items.Add("Equipo Nº: "+Convert.ToString(puntero1.equipos11[puntero1.numequipos-1].numequipo2)+"   Tipo Equipo: " +Convert.ToString(6));
            }            
        }

        //Función para leer los Objetos de la lista de Equipos (equipos11) de la aplicación principal
        private void LeerEquipos()
        {
            for (int i = 0; i <puntero1.equipos11.Count; i++)
            {
                //IMPORTANTE: Modificar en Refactoring. Elegimos el Tipo de Equipo que queremos incluir en la lista de Equipos
                if (puntero1.equipos11[i].tipoequipo2 == 6)
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
                //Si el equipo esta selecciondo en la lista de equipos tipo 10, guardamos en la variable numeroequipo11 el número de equipo seleccionado en la lista
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

            Reactor reactor6=new Reactor(puntero1, puntero1.numecuaciones, puntero1.numvariables,1,indice);

            //Unidades
            //Sistema Britanico=0;Sistema Internacional=2;Sistema Métrico=1

            //Dependiendo de las unidades elegidas en la Aplicación principal se realiza una conversión 
            //de los valores de los parámetros (D1 a D9) guardados en el array de equipos "equipos11" para visualizarlo en el cuadro de diálogo en las unidades elegidas
            //Hay que tener en cuenta que dentro del array equipos11 siempre se guardan los parámetros (D1 al D9) en unidades del Sistema Británico porque son las utilizadas por las Tablas de Vapor ASME

            //Si las Unidades de la Aplicación son del Sistema Métrico
            if (puntero1.unidades == 1)
            {
                //Factor independiente de pérdida de carga
                reactor6.textBox1.Text =  Convert.ToString(puntero1.equipos11[indice].aD1 * (6.8947572 / 100));
                //Factor lineal de pérdida de carga
                reactor6.textBox2.Text =  Convert.ToString(puntero1.equipos11[indice].aD2 / 6.578911309);
                //Factor cuadrático de pérdida de carga
                reactor6.textBox3.Text =  Convert.ToString(puntero1.equipos11[indice].aD3/ 2.984193609);

                if (puntero1.equipos11[indice].aD4 < 0)
                {
                    //En este caso fijamos la presión P2=D4, por tanto, tenemos que realizar la conversión de PSI a Bar
                    reactor6.textBox4.Text = Convert.ToString(puntero1.equipos11[indice].aD4 * (6.8947572 / 100));
                }

                if (puntero1.equipos11[indice].aD5 > 0)
                {
                    //Entalpia Btu/Lb a Kj/Kg
                    reactor6.textBox5.Text = Convert.ToString(puntero1.equipos11[indice].aD5 * 2.326009);
                }
                else if (puntero1.equipos11[indice].aD5 < 0)
                {
                    //Convertir los grados ºC en ºF
                    reactor6.textBox5.Text = Convert.ToString(((puntero1.equipos11[indice].aD5-32)*5.0)/9.0);
                }

                //Convertimos el Calor aportado al ciclo de BTU/sg a Kw
                reactor6.textBox6.Text = Convert.ToString(puntero1.equipos11[indice].aD6 / 0.9486608);
                //Rendimiento Térmico
                reactor6.textBox7.Text = Convert.ToString(puntero1.equipos11[indice].aD7);
                //Contabilación del Calor aportado al final del Balance
                reactor6.textBox11.Text = Convert.ToString(puntero1.equipos11[indice].aD9);
            }

            //Si las Unidades de la Aplicación son del Sistema Internacional
            else if (puntero1.unidades == 2)
            {
                //Factor independiente de pérdida de carga
                reactor6.textBox1.Text = Convert.ToString(puntero1.equipos11[indice].aD1 * (6.8947572 / 100));
                //Factor lineal de pérdida de carga
                reactor6.textBox2.Text = Convert.ToString(puntero1.equipos11[indice].aD2 / 6.578911309);
                //Factor cuadrático de pérdida de carga
                reactor6.textBox3.Text = Convert.ToString(puntero1.equipos11[indice].aD3 / 2.984193609);

                if (puntero1.equipos11[indice].aD4 < 0)
                {
                    //En este caso fijamos la presión P2=D4, por tanto, tenemos que realizar la conversión de PSI a Bar
                    reactor6.textBox4.Text = Convert.ToString(puntero1.equipos11[indice].aD4 * (6.8947572 / 100));
                }

                if (puntero1.equipos11[indice].aD5 > 0)
                {
                    //Entalpia Btu/Lb a Kj/Kg
                    reactor6.textBox5.Text = Convert.ToString(puntero1.equipos11[indice].aD5 * 2.326009);
                }
                else if (puntero1.equipos11[indice].aD5 < 0)
                {
                    //Convertir los grados ºC en ºF
                    reactor6.textBox5.Text = Convert.ToString(((puntero1.equipos11[indice].aD5 - 32) * 5.0) / 9.0);
                }

                //Convertimos el Calor aportado al ciclo de BTU/sg a Kw
                reactor6.textBox6.Text = Convert.ToString(puntero1.equipos11[indice].aD6 / 0.9486608);
                //Rendimiento Térmico
                reactor6.textBox7.Text = Convert.ToString(puntero1.equipos11[indice].aD7);
                //Contabilación del Calor aportado al final del Balance
                reactor6.textBox11.Text = Convert.ToString(puntero1.equipos11[indice].aD9);
            }

            //Si las Unidades de la Aplicación son del Sistema Británico
            else if (puntero1.unidades == 0)
            {
                //Factor independiente de pérdida de carga
                reactor6.textBox1.Text = Convert.ToString(puntero1.equipos11[indice].aD1);
                //Factor lineal de pérdida de carga
                reactor6.textBox2.Text = Convert.ToString(puntero1.equipos11[indice].aD2);
                //Factor cuadrático de pérdida de carga
                reactor6.textBox3.Text = Convert.ToString(puntero1.equipos11[indice].aD3);
                //En este caso fijamos la presión P2=D4, PSI 
                reactor6.textBox4.Text = Convert.ToString(puntero1.equipos11[indice].aD4);
                //Entalpia Btu/Lb 
                reactor6.textBox5.Text = Convert.ToString(puntero1.equipos11[indice].aD5);
                //Convertimos el Calor aportado al ciclo de BTU/sg a Kw
                reactor6.textBox6.Text = Convert.ToString(puntero1.equipos11[indice].aD6);
                //Rendimiento Térmico
                reactor6.textBox7.Text = Convert.ToString(puntero1.equipos11[indice].aD7);
                //Contabilación del Calor aportado al final del Balance
                reactor6.textBox11.Text = Convert.ToString(puntero1.equipos11[indice].aD9);
            }

            reactor6.textBox10.Text = Convert.ToString(puntero1.equipos11[indice].aN1);
            reactor6.textBox8.Text = Convert.ToString(puntero1.equipos11[indice].aN3);
            reactor6.textBox9.Text = Convert.ToString(puntero1.equipos11[indice].numequipo2);

            reactor6.ShowDialog();

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
