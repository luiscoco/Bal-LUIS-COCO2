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
    public partial class Paletaequipos : Form
    {
        Aplicacion punteroaplicacion2;

        public Paletaequipos(Aplicacion punteroaplicacion1)
        {
            punteroaplicacion2 = punteroaplicacion1;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }



        private void button1_MouseMove(object sender, MouseEventArgs e)
        {
          

        }

        private void button10_MouseMove(object sender, MouseEventArgs e)
        {
            //Si el boton pulsado al arrastrar no es el izquierdo.
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            punteroaplicacion2.tipoequipodrag = 1;
            
            Button boton1 = button10;
            //Arrastra el boton desde el Form1
            button10.DoDragDrop(boton1, DragDropEffects.Move);
        }

        private void button6_MouseMove(object sender, MouseEventArgs e)
        {
            //Si el boton pulsado al arrastrar no es el izquierdo.
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            
            punteroaplicacion2.tipoequipodrag = 2;

            Button boton2 = button6;
            //Arrastra el boton desde el Form1
            button6.DoDragDrop(boton2, DragDropEffects.Move);
        }

        private void button17_MouseMove(object sender, MouseEventArgs e)
        {
            //Si el boton pulsado al arrastrar no es el izquierdo.
            if (e.Button != MouseButtons.Left)
            {
                return;
            }


            punteroaplicacion2.tipoequipodrag = 3;

            Button boton3 = button17;
            //Arrastra el boton desde el Form1
            button17.DoDragDrop(boton3, DragDropEffects.Move);
        }

        private void button8_MouseMove(object sender, MouseEventArgs e)
        {
            //Si el boton pulsado al arrastrar no es el izquierdo.
            if (e.Button != MouseButtons.Left)
            {
                return;
            }


            punteroaplicacion2.tipoequipodrag = 13;

            Button boton4 = button8;
            //Arrastra el boton desde el Form1
            button8.DoDragDrop(boton4, DragDropEffects.Move);
        }

        private void button7_MouseMove(object sender, MouseEventArgs e)
        {
            //Si el boton pulsado al arrastrar no es el izquierdo.
            if (e.Button != MouseButtons.Left)
            {
                return;
            }


            punteroaplicacion2.tipoequipodrag = 5;

            Button boton5 = button7;
            //Arrastra el boton desde el Form1
            button7.DoDragDrop(boton5, DragDropEffects.Move);
        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            //Si el boton pulsado al arrastrar no es el izquierdo.
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            punteroaplicacion2.tipoequipodrag = 9;

            Button boton6 = button1;
            //Arrastra el boton desde el Form1
            button1.DoDragDrop(boton6, DragDropEffects.Move);
        }

        private void button13_MouseMove(object sender, MouseEventArgs e)
        {
            //Si el boton pulsado al arrastrar no es el izquierdo.
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            punteroaplicacion2.tipoequipodrag = 14;

            Button boton7 = button13;
            //Arrastra el boton desde el Form1
            button13.DoDragDrop(boton7, DragDropEffects.Move);
        }

        private void button11_MouseMove(object sender, MouseEventArgs e)
        {
            //Si el boton pulsado al arrastrar no es el izquierdo.
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            punteroaplicacion2.tipoequipodrag = 7;

            Button boton8 = button11;
            //Arrastra el boton desde el Form1
            button8.DoDragDrop(boton8, DragDropEffects.Move);
        }

        private void button18_MouseMove(object sender, MouseEventArgs e)
        {
            //Si el boton pulsado al arrastrar no es el izquierdo.
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            punteroaplicacion2.tipoequipodrag = 10;

            Button boton9 = button18;
            //Arrastra el boton desde el Form1
            button9.DoDragDrop(boton9, DragDropEffects.Move);
        }

        private void button2_MouseMove(object sender, MouseEventArgs e)
        {
            //Si el boton pulsado al arrastrar no es el izquierdo.
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            punteroaplicacion2.tipoequipodrag = 8;

            Button boton10 = button2;
            //Arrastra el boton desde el Form1
            button10.DoDragDrop(boton10, DragDropEffects.Move);
        }

        private void button14_MouseMove(object sender, MouseEventArgs e)
        {
            //Si el boton pulsado al arrastrar no es el izquierdo.
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            punteroaplicacion2.tipoequipodrag = 4;

            Button boton11 = button14;
            //Arrastra el boton desde el Form1
            boton11.DoDragDrop(boton11, DragDropEffects.Move);
        }

        private void button10_Click(object sender, EventArgs e)
        {

        }
    }
}
