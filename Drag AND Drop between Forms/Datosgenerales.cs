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
    public partial class Datosgenerales : Form
    {
        Aplicacion puntero1;

        public Datosgenerales(Aplicacion puntero)
        {
            puntero1 = puntero;
            
            InitializeComponent();
        }

        //Botón de OK
        private void button1_Click(object sender, EventArgs e)
        {
            //Toma de Datosgenerales del cuadro de diálogo y envío de Datos a la Aplicación principal
          
            //Titulo del Archivo
            puntero1.Titulo = textBox1.Text;
            //Nombre del Archivo
            puntero1.NombreArchivo =textBox2.Text;
            //Número Total de Equipos
            puntero1.NumTotalEquipos =Convert.ToDouble(textBox3.Text);
            //Número Total de Corrientes
            puntero1.NumTotalCorrientes =Convert.ToDouble(textBox4.Text);
            //Número de Iteraciones Máximas
            puntero1.NumMaxIteraciones = Convert.ToDouble(textBox5.Text);
            //Número Total de Tablas
            puntero1.NumTotalTablas =Convert.ToDouble(textBox6.Text);
            //Error Máximo Admisible
            puntero1.ErrorMaxAdmisible = Convert.ToDouble(textBox7.Text);
            //Datos Inciales Buenos
            puntero1.DatosIniciales = Convert.ToDouble(textBox8.Text);
            //Factor de Iteraciones (EPS)
            puntero1.FactorIteraciones = Convert.ToDouble(textBox9.Text);
            //Fichero de Iteraciones Intermedias
            puntero1.FicheroIteraciones =Convert.ToDouble(textBox10.Text);
            //Unidades
            puntero1.unidades = Convert.ToDouble(textBox11.Text);

            this.Hide();
        }

        //Botón de Cancel
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void Datosgenerales_Load(object sender, EventArgs e)
        {
            //Titulo del Archivo
            textBox1.Text=puntero1.Titulo;
            //Nombre del Archivo
            textBox2.Text=puntero1.NombreArchivo;
            //Número Total de Equipos
            textBox3.Text=Convert.ToString(puntero1.NumTotalEquipos);
            //Número Total de Corrientes
            textBox4.Text=Convert.ToString(puntero1.NumTotalCorrientes);
            //Número de Iteraciones Máximas
            textBox5.Text=Convert.ToString(puntero1.NumMaxIteraciones);
            //Número Total de Tablas
            textBox6.Text=Convert.ToString(puntero1.NumTotalTablas);
            //Error Máximo Admisible
            textBox7.Text=Convert.ToString(puntero1.ErrorMaxAdmisible);
            //Datos Inciales Buenos
            textBox8.Text=Convert.ToString(puntero1.DatosIniciales);
            //Factor de Iteraciones (EPS)
            textBox9.Text=Convert.ToString(puntero1.FactorIteraciones);
            //Fichero de Iteraciones Intermedias
            textBox10.Text=Convert.ToString(puntero1.FicheroIteraciones);
            //Unidades
            textBox11.Text=Convert.ToString(puntero1.unidades);
        }   

    }
}
