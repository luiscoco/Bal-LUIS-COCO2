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
    public partial class Resultadoshbal : Form
    {
        Aplicacion puntero1;

        public Resultadoshbal(Aplicacion puntero)
        {
            puntero1 = puntero;

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            puntero1.rutaresultadoshbal= textBox1.Text;
            this.Hide();
            puntero1.lanzardera();
        }
    }
}
