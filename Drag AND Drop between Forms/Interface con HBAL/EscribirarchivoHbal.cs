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
    public partial class EscribirarchivoHbal : Form
    {
        Aplicacion puntero1;

        public EscribirarchivoHbal(Aplicacion puntero)
        {
            puntero1 = puntero;
            InitializeComponent();
        }

        //Botón de OK
        private void button1_Click(object sender, EventArgs e)
        {
            puntero1.Titulo = textBox1.Text;
            puntero1.NombreArchivo = textBox2.Text;
            puntero1.NumMaxIteraciones= Convert.ToDouble(textBox5.Text);
            puntero1.ErrorMaxAdmisible=Convert.ToDouble(textBox7.Text);
            puntero1.FactorIteraciones=Convert.ToDouble(textBox9.Text);

            //Opción para escribir las CONDICIONES INICIALES en el archivo
            if (this.checkBox1.Checked == true)
            {
                puntero1.incluircondicionesiniciales = 1;
            }
            else if (this.checkBox1.Checked == false)
            {
                puntero1.incluircondicionesiniciales = 0;
            }

            this.DialogResult = DialogResult.OK;
            this.Hide();
        }

        //Botón de CANCEL
        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Hide();
        }

        private void EscribirarchivoHbal_Load(object sender, EventArgs e)
        {
            textBox3.Text = Convert.ToString(puntero1.NumTotalEquipos);
            textBox4.Text = Convert.ToString(puntero1.NumTotalCorrientes);
            textBox6.Text = Convert.ToString(puntero1.NumTotalTablas);
            textBox1.Text = puntero1.Titulo;
            textBox2.Text = puntero1.NombreArchivo;
            textBox5.Text = Convert.ToString(puntero1.NumMaxIteraciones);
            textBox7.Text = Convert.ToString(puntero1.ErrorMaxAdmisible);
            textBox9.Text = Convert.ToString(puntero1.FactorIteraciones);
            textBox11.Text = Convert.ToString(puntero1.unidades);
        }

    }
}
