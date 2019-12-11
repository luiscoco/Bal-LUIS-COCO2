using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Microsoft.CSharp;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using System.IO;
using Drag_AND_Drop_between_Forms;

using CSharpScripter;
using CSharpScripter1;
using CSharpScripter2;

//Para acceder a las librerías dinámicas *.dll de FORTRAN
using System.Runtime.InteropServices;

//using System.Reflection;

namespace CompiladorLUISCOCO
{
	/// <summary>
	/// Zusammenfassung für Form1.
	/// </summary>
	public class FormMain : System.Windows.Forms.Form
    {
        public Aplicacion puntero = new Aplicacion();
		private System.Windows.Forms.Button btnCompile;
		private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.Button btnQuit;
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;
        private TextBox textBox1;
        private TextBox textBox2;
        private Label label1;
        private Label label2;

        public Double resultado = 0;
        public Double valora = 0;
        private Label label3;
        private TextBox textBox3;
        private Label label4;
        private TextBox textBox4;
        public Double valorb = 0;
        private Label label5;
        private TextBox textBox5;
        private Label label6;
        private TextBox textBox6;
        private Label label7;
        private TextBox textBox7;
        private Button button2;
        private ListBox listBox1;
        private Button button8;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private RadioButton radioButton3;
        private RadioButton radioButton4;

        public char comillas = '"';
        private Button button16;

        public int elementoseleccionado = 0;

        public string[] codigo;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private Button button13;
        private Button btnLoad;
        private TabPage tabPage2;
        private Button button1;
        private Button button3;
        private TabPage tabPage3;
        private Button button7;
        private Button button6;
        private Button button5;
        private Button button4;
        private TabPage tabPage4;
        private Button button11;
        private Button button9;
        private Button button10;
        private RichTextBox rtfCode;
        private RichTextBox richTextBox1;
        private RichTextBox richTextBox2;
        private RadioButton radioButton5;
        public string codigoleido;

        public FormMain(Aplicacion puntero1)
		{            
            puntero = puntero1;

			// Erforderlich für die Windows Form-Designerunterstützung
			//
			InitializeComponent();

			//
			// TODO: Fügen Sie den Konstruktorcode nach dem Aufruf von InitializeComponent hinzu
			//
		}

		/// <summary>
		/// Die verwendeten Ressourcen bereinigen.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Vom Windows Form-Designer generierter Code
		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
            this.btnCompile = new System.Windows.Forms.Button();
            this.btnExecute = new System.Windows.Forms.Button();
            this.btnQuit = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button8 = new System.Windows.Forms.Button();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.button16 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.button11 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.button13 = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.rtfCode = new System.Windows.Forms.RichTextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCompile
            // 
            this.btnCompile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCompile.Location = new System.Drawing.Point(851, 540);
            this.btnCompile.Name = "btnCompile";
            this.btnCompile.Size = new System.Drawing.Size(75, 35);
            this.btnCompile.TabIndex = 1;
            this.btnCompile.Text = "Compile";
            this.btnCompile.Click += new System.EventHandler(this.btnCompile_Click);
            // 
            // btnExecute
            // 
            this.btnExecute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExecute.Location = new System.Drawing.Point(933, 540);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(72, 35);
            this.btnExecute.TabIndex = 2;
            this.btnExecute.Text = "Execute";
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // btnQuit
            // 
            this.btnQuit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuit.Location = new System.Drawing.Point(933, 591);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(72, 32);
            this.btnQuit.TabIndex = 3;
            this.btnQuit.Text = "OK";
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(521, 25);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(43, 20);
            this.textBox1.TabIndex = 5;
            this.textBox1.Text = "2";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(521, 51);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(43, 20);
            this.textBox2.TabIndex = 6;
            this.textBox2.Text = "3";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(471, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Valor A:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(471, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Valor B:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(598, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Resultado:";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(662, 25);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(43, 20);
            this.textBox3.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(600, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Título:";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(652, 42);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(68, 20);
            this.textBox4.TabIndex = 12;
            this.textBox4.Text = "EJEMPLO 1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(819, 463);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Execute File *.DLL:";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(940, 460);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(100, 20);
            this.textBox5.TabIndex = 14;
            this.textBox5.Text = "TestClass.dll";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(819, 437);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(108, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Compile to File *.DLL:";
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(940, 434);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(100, 20);
            this.textBox6.TabIndex = 16;
            this.textBox6.Text = "TestClass.dll";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(600, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(81, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "Nº Ecuaciones:";
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(687, 10);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(33, 20);
            this.textBox7.TabIndex = 20;
            this.textBox7.Text = "2";
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.Location = new System.Drawing.Point(784, 371);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(78, 39);
            this.button2.TabIndex = 22;
            this.button2.Text = "List *.DLL Files";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.Location = new System.Drawing.Point(784, 102);
            this.listBox1.Name = "listBox1";
            this.listBox1.ScrollAlwaysVisible = true;
            this.listBox1.Size = new System.Drawing.Size(256, 251);
            this.listBox1.TabIndex = 23;
            this.listBox1.SelectedValueChanged += new System.EventHandler(this.listBox1_SelectedValueChanged);
            // 
            // button8
            // 
            this.button8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button8.Location = new System.Drawing.Point(852, 592);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(75, 32);
            this.button8.TabIndex = 30;
            this.button8.Text = "Cancel";
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(26, 34);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(141, 17);
            this.radioButton1.TabIndex = 36;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Linear Equations System";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(26, 56);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(164, 17);
            this.radioButton2.TabIndex = 37;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Non-Linear Equations System";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(280, 12);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(255, 17);
            this.radioButton3.TabIndex = 38;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "ODE Differential Equations System (Trent-Guidry)";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(26, 11);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(128, 17);
            this.radioButton4.TabIndex = 39;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "Interpolation Equation";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // button16
            // 
            this.button16.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button16.Location = new System.Drawing.Point(940, 371);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(100, 39);
            this.button16.TabIndex = 42;
            this.button16.Text = "Delete selected *.DLL File";
            this.button16.Click += new System.EventHandler(this.button16_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(26, 486);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(739, 147);
            this.tabControl1.TabIndex = 43;
            this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.button1);
            this.tabPage2.Controls.Add(this.button3);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(731, 121);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Sistema Ecuaciones Lineales";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(6, 61);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 38);
            this.button1.TabIndex = 57;
            this.button1.Text = "Linear Equations System Sample2";
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button3.Location = new System.Drawing.Point(6, 18);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 37);
            this.button3.TabIndex = 56;
            this.button3.Text = "Linear Equations System Sample1";
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage3.Controls.Add(this.button7);
            this.tabPage3.Controls.Add(this.button6);
            this.tabPage3.Controls.Add(this.button5);
            this.tabPage3.Controls.Add(this.button4);
            this.tabPage3.Controls.Add(this.textBox7);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.label4);
            this.tabPage3.Controls.Add(this.textBox4);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(731, 121);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Sistemas Ecuaciones No Lineales";
            // 
            // button7
            // 
            this.button7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button7.Location = new System.Drawing.Point(111, 10);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(102, 47);
            this.button7.TabIndex = 71;
            this.button7.Text = "Non-Linear Equations System Sample3";
            this.button7.Click += new System.EventHandler(this.button7_Click_1);
            // 
            // button6
            // 
            this.button6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button6.Location = new System.Drawing.Point(3, 64);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(102, 51);
            this.button6.TabIndex = 70;
            this.button6.Text = "Non-Linear Equations System Sample2";
            this.button6.Click += new System.EventHandler(this.button6_Click_1);
            // 
            // button5
            // 
            this.button5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button5.Location = new System.Drawing.Point(3, 9);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(102, 48);
            this.button5.TabIndex = 69;
            this.button5.Text = "Non-Linear Equations System Sample1";
            this.button5.Click += new System.EventHandler(this.button5_Click_1);
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button4.Location = new System.Drawing.Point(111, 64);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(102, 51);
            this.button4.TabIndex = 68;
            this.button4.Text = "Non-Linear Equations System Sample4";
            this.button4.Click += new System.EventHandler(this.button4_Click_1);
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage4.Controls.Add(this.button11);
            this.tabPage4.Controls.Add(this.button9);
            this.tabPage4.Controls.Add(this.button10);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(731, 121);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Sistema Ecuacioneas Diferenciales (ODE)";
            // 
            // button11
            // 
            this.button11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button11.Location = new System.Drawing.Point(143, 8);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(132, 47);
            this.button11.TabIndex = 68;
            this.button11.Text = "DOE System Sample 2 (DotNumerics Libraries)";
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button9
            // 
            this.button9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button9.Location = new System.Drawing.Point(12, 60);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(125, 47);
            this.button9.TabIndex = 67;
            this.button9.Text = "DOE System Sample 3 (DotNumerics Libraries)";
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button10
            // 
            this.button10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button10.Location = new System.Drawing.Point(12, 8);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(125, 47);
            this.button10.TabIndex = 66;
            this.button10.Text = "DOE System Sample1 (Trent-Guidry Libraries)";
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.button13);
            this.tabPage1.Controls.Add(this.btnLoad);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.textBox1);
            this.tabPage1.Controls.Add(this.textBox2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.textBox3);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(731, 121);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Ecuaciones para Interpolación";
            // 
            // button13
            // 
            this.button13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button13.Location = new System.Drawing.Point(100, 19);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(88, 50);
            this.button13.TabIndex = 53;
            this.button13.Text = "Interpolation Equation Sample2";
            this.button13.Click += new System.EventHandler(this.button13_Click_1);
            // 
            // btnLoad
            // 
            this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLoad.Location = new System.Drawing.Point(6, 19);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(88, 48);
            this.btnLoad.TabIndex = 42;
            this.btnLoad.Text = "Interpolation Equation Sample1";
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click_1);
            // 
            // rtfCode
            // 
            this.rtfCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rtfCode.BackColor = System.Drawing.Color.White;
            this.rtfCode.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtfCode.HideSelection = false;
            this.rtfCode.Location = new System.Drawing.Point(26, 102);
            this.rtfCode.Name = "rtfCode";
            this.rtfCode.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.rtfCode.Size = new System.Drawing.Size(742, 116);
            this.rtfCode.TabIndex = 44;
            this.rtfCode.Text = "";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.BackColor = System.Drawing.Color.White;
            this.richTextBox1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.ForeColor = System.Drawing.Color.Blue;
            this.richTextBox1.HideSelection = false;
            this.richTextBox1.Location = new System.Drawing.Point(26, 224);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.richTextBox1.Size = new System.Drawing.Size(742, 151);
            this.richTextBox1.TabIndex = 45;
            this.richTextBox1.Text = "";
            // 
            // richTextBox2
            // 
            this.richTextBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox2.BackColor = System.Drawing.Color.White;
            this.richTextBox2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox2.HideSelection = false;
            this.richTextBox2.Location = new System.Drawing.Point(26, 381);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.richTextBox2.Size = new System.Drawing.Size(742, 99);
            this.richTextBox2.TabIndex = 46;
            this.richTextBox2.Text = "";
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Location = new System.Drawing.Point(280, 34);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(258, 17);
            this.radioButton5.TabIndex = 47;
            this.radioButton5.TabStop = true;
            this.radioButton5.Text = "ODE Differential Equations System (DotNumerics)";
            this.radioButton5.UseVisualStyleBackColor = true;
            // 
            // FormMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(1052, 636);
            this.Controls.Add(this.radioButton5);
            this.Controls.Add(this.rtfCode);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.richTextBox2);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button16);
            this.Controls.Add(this.radioButton4);
            this.Controls.Add(this.radioButton3);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.btnQuit);
            this.Controls.Add(this.btnExecute);
            this.Controls.Add(this.btnCompile);
            this.Name = "FormMain";
            this.Text = "MATHEMATICAL EQUATIONS COMPILER";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// Der Haupteinstiegspunkt für die Anwendung.
		/// </summary>
		
		private void btnQuit_Click(object sender, System.EventArgs e)
		{
			//Application.Exit();
            this.Hide();
		}

        //Esta función envio a la Aplicación principal el puntero de este cuadro de diálogo de ejemplos de validación del motor de cálculo
        public FormMain enviarpuntero()
        {
            return this;
        }

        //Botón de COMPILAR
		private void btnCompile_Click(object sender, System.EventArgs e)
		{
            //Creamos el Objeto compilador de C# y sus parámetros
         	CSharpCodeProvider csp = new CSharpCodeProvider();
			ICodeCompiler cc = csp.CreateCompiler();
			CompilerParameters cp = new CompilerParameters();

            //Definimos la ruta donde se generará la librería *.DLL compilada
			cp.OutputAssembly = Application.StartupPath + "\\"+textBox6.Text;

            //Definimos las librerías que se referencian en la compilación
			cp.ReferencedAssemblies.Add("System.dll");
			cp.ReferencedAssemblies.Add("System.dll");
			cp.ReferencedAssemblies.Add("System.Data.dll");
			cp.ReferencedAssemblies.Add("System.Xml.dll");
			cp.ReferencedAssemblies.Add("mscorlib.dll");
			cp.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            cp.ReferencedAssemblies.Add("Drag AND Drop between Forms.exe");
						
			cp.WarningLevel = 3;

			cp.CompilerOptions = "/target:library /optimize";
			cp.GenerateExecutable = false;
			cp.GenerateInMemory = false;

			System.CodeDom.Compiler.TempFileCollection tfc = new TempFileCollection(Application.StartupPath, false);
			CompilerResults cr  = new CompilerResults(tfc);

            //Definimos el Código de C# que será compilado para generar la librería *.DLL
            string codigoacompilar = "";
            codigoacompilar = this.rtfCode.Text + this.richTextBox1.Text + this.richTextBox2.Text;
            cr = cc.CompileAssemblyFromSource(cp, codigoacompilar);
            
            //En caso de que existan errores de compilación se muestran en la Consola 
			if (cr.Errors.Count > 0) 
			{
				foreach (CompilerError ce in cr.Errors) 
				{
					Console.WriteLine(ce.ErrorNumber + ": " + ce.ErrorText);
				}
				MessageBox.Show(this, "Errors occoured", "Errors", MessageBoxButtons.OK, MessageBoxIcon.Error);
				this.btnExecute.Enabled = false;
			} 
			else 
			{
				this.btnExecute.Enabled = true;
			}
			
			System.Collections.Specialized.StringCollection sc = cr.Output;

			foreach (string s in sc) 
			{
				//Console.WriteLine(s);
			}

            //Listar las librerías *.dll
            button2_Click(sender, e);
		}

        //Botón EJECUTAR el Código Compilado
		private void btnExecute_Click(object sender, System.EventArgs e)
		{
            //Llamamos a la opción del Menú de Nuevo Cálculo
            puntero.toolStripMenuItem11_Click(sender, e);

			AppDomainSetup ads = new AppDomainSetup();
			ads.ShadowCopyFiles = "true";

			AppDomain.CurrentDomain.SetShadowCopyFiles();           

            AppDomain currentDomain = AppDomain.CurrentDomain;

            //MessageBox.Show("Current Domain:"+ currentDomain.FriendlyName);

			AppDomain newDomain = AppDomain.CreateDomain("newDomain");

            //MessageBox.Show("New Domain:"+ newDomain.FriendlyName);
            
            //Cargamos una de las librerías(*.DLL) compiladas
			byte[] rawAssembly = loadFile(textBox5.Text);

            Assembly assembly = newDomain.Load(rawAssembly);

            //El Objetivo de los RadioButton y en particular de estas opción "if" es elegir según la opción seleccionada un "Interface" diferente a implementar 
            //para poder disponer de funciones "Execute" con argumentos diferentes

            //Si hemos elegido la Opción de crear una Ecuación para Interpolación
            if (radioButton4.Checked == true)
            {
                //IMPORTANTE: se crea una instancia ("testClass") de la Clase compilada TestClass
                Command testClass = (Command)assembly.CreateInstance("CSharpScripter.TestClass");           

                valora = Convert.ToDouble(textBox1.Text);
                valorb = Convert.ToDouble(textBox2.Text);

                //Hacemos una llamada a una función "Execute" incluida en el objeto "testClass"
                resultado = testClass.Execute(valora, valorb, puntero);

                testClass = null;
            }

            //Si hemos elegido la Opción de crear Sistemas de Ecuaciones Lineales
            else if (radioButton1.Checked == true)
            {
                //IMPORTANTE: se crea una instancia ("testClass") de la Clase compilada TestClass
                Command1 testClass = (Command1)assembly.CreateInstance("CSharpScripter1.TestClass");

                //Hacemos una llamada a una función "Execute" incluida en el objeto "testClass"
                testClass.Execute1(puntero);

                testClass = null;
            }

            //Si hemos elegido la Opción de crear Sistemas de Ecuaciones No Lineales
            else if (radioButton2.Checked == true)
            {
                //IMPORTANTE: se crea una instancia ("testClass") de la Clase compilada TestClass
                Command1 testClass = (Command1)assembly.CreateInstance("CSharpScripter1.TestClass");

                //Hacemos una llamada a una función "Execute" incluida en el objeto "testClass"
                testClass.Execute1(puntero);

                testClass = null;
            }

            //Si hemos elegido la Opción de crear Sistemas de Ecuaciones Diferenciales (con librería DotNumerics.Net)
            else if (radioButton5.Checked == true)
            {
                //IMPORTANTE: se crea una instancia ("testClass") de la Clase compilada TestClass 
                Command2 testClass = (Command2)assembly.CreateInstance("CSharpScripter2.TestClass");

                puntero.luistest = testClass;

                //Hacemos una llamada a una función "Execute" incluida en el objeto "testClass"
                testClass.Execute2(puntero);

                testClass = null;
            }

             //Si hemos elegido la Opción de crear Sistemas de Ecuaciones Diferenciales (con librería Trent-Guidry)
            else if (radioButton3.Checked == true)
            {
                //IMPORTANTE: se crea una instancia ("testClass") de la Clase compilada TestClass
                Command1 testClass = (Command1)assembly.CreateInstance("CSharpScripter1.TestClass");            

                //Hacemos una llamada a una función "Execute" incluida en el objeto "testClass"
                testClass.Execute1(puntero);

                testClass = null;
            }
        
            //Resto de Casos 
            else
            {
                //IMPORTANTE: se crea una instancia ("testClass") de la Clase compilada TestClass
                Command testClass = (Command)assembly.CreateInstance("CSharpScripter.TestClass");

                valora = Convert.ToDouble(textBox1.Text);
                valorb = Convert.ToDouble(textBox2.Text);

                //Hacemos una llamada a una función "Execute" incluida en el objeto "testClass"
                resultado = testClass.Execute(valora, valorb, puntero);

                testClass = null;
            }
            
            rtfCode.Text = "";
            richTextBox1.Text = "";
            richTextBox2.Text = "";

            textBox3.Text = Convert.ToString(resultado);		
			
			AppDomain.Unload(newDomain);          
		}

        //Lectura de Archvio (*.DLL) que hemos elegido 
		private byte[] loadFile(string filename) 
		{
			FileStream fs = new FileStream(filename, FileMode.Open);
			byte[] buffer = new byte[(int) fs.Length];
			fs.Read(buffer, 0, buffer.Length);
			fs.Close();
			fs = null;
			return buffer;
		}

        //Cargamos las librerías (*.DLL) compiladas en la lista 
        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            DirectoryInfo di = new DirectoryInfo(Application.StartupPath + "\\");
            FileInfo[] rgFiles = di.GetFiles("*.dll");

            foreach (FileInfo fi in rgFiles)
            {
                listBox1.Items.Add(fi.Name);
            }
        }

     
        //Non Linear Equations System Sample5
        private void button14_Click(object sender, EventArgs e)
        {
            this.rtfCode.Clear();
            this.richTextBox1.Clear();
            this.richTextBox2.Clear();

            //Llamamos a la opción del Menú de Nuevo Cálculo
            puntero.toolStripMenuItem11_Click(sender, e);

            this.rtfCode.Text += "using System.Collections.Generic;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.ComponentModel;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Data;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System;" + System.Environment.NewLine;
            //this.rtfCode.Text += "using System.Linq;" + System.Environment.NewLine;
            //this.rtfCode.Text += "using System.Text;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Collections;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Xml;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.IO;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Windows.Forms;" + System.Environment.NewLine;
            this.rtfCode.Text += "using Drag_AND_Drop_between_Forms;" + System.Environment.NewLine;
            this.rtfCode.Text += "using NumericalMethods;" + System.Environment.NewLine;
            this.rtfCode.Text += "using NumericalMethods.FourthBlog;" + System.Environment.NewLine;
            this.rtfCode.Text += "using ClaseEquipos;" + System.Environment.NewLine;

            this.rtfCode.Text += System.Environment.NewLine;
            this.rtfCode.Text += "namespace CSharpScripter" + System.Environment.NewLine;
            this.rtfCode.Text += "{" + System.Environment.NewLine;
            this.rtfCode.Text += "	public class TestClass : CSharpScripter.Command" + System.Environment.NewLine;
            this.rtfCode.Text += "	{" + System.Environment.NewLine;
            this.rtfCode.Text += "Aplicacion puntero4;" + System.Environment.NewLine;
            this.rtfCode.Text += "		public TestClass()" + System.Environment.NewLine;
            this.rtfCode.Text += "		{" + System.Environment.NewLine;
            this.rtfCode.Text += "		}" + System.Environment.NewLine;
            this.rtfCode.Text += System.Environment.NewLine;
            this.rtfCode.Text += "		public Double Execute(Double a, Double b, Aplicacion puntero3) " + System.Environment.NewLine;
            this.rtfCode.Text += "		{" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4=puntero3;" + System.Environment.NewLine;

            int necuaciones = 0;
            necuaciones = Convert.ToInt16(textBox7.Text);

            //CREAMOS EL ARRAY DE PARAMETROS
            this.rtfCode.Text += " for (int v = 0; v < 10; v++)" + System.Environment.NewLine;
            this.rtfCode.Text += "{" + System.Environment.NewLine;
            //Creamos la lista de parámetros generadas por este programa
            this.rtfCode.Text += "puntero4.p.Add(puntero4.ptemp);" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p[v] = new Parameter(1,0.01," + comillas + comillas + ");" + System.Environment.NewLine;
            this.rtfCode.Text += "}" + System.Environment.NewLine;

            //Asignamos las Condiciones Iniciales a las dos variables, en este caso Xi....Xn
            this.rtfCode.Text += "puntero4.p[0].Nombre = " + comillas + "X1" + comillas + ";" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p[1].Nombre = " + comillas + "X2" + comillas + ";" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p[2].Nombre = " + comillas + "X3" + comillas + ";" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p[3].Nombre = " + comillas + "X4" + comillas + ";" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p[4].Nombre = " + comillas + "X5" + comillas + ";" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p[5].Nombre = " + comillas + "X6" + comillas + ";" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p[6].Nombre = " + comillas + "X7" + comillas + ";" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p[7].Nombre = " + comillas + "X8" + comillas + ";" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p[8].Nombre = " + comillas + "X9" + comillas + ";" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p[9].Nombre = " + comillas + "X10" + comillas + ";" + System.Environment.NewLine;

            this.rtfCode.Text += "List < String > ecuaciones2 = new List<String>();" + System.Environment.NewLine;

            this.rtfCode.Text += "Parameter X1= new Parameter();" + System.Environment.NewLine;
            this.rtfCode.Text += "X1=puntero4.p.Find(p =>p.Nombre ==" + comillas + "X1" + comillas + ");" + System.Environment.NewLine;
            this.rtfCode.Text += "Parameter X2= new Parameter();" + System.Environment.NewLine;
            this.rtfCode.Text += "X2=puntero4.p.Find(p =>p.Nombre ==" + comillas + "X2" + comillas + ");" + System.Environment.NewLine;
            this.rtfCode.Text += "Parameter X3= new Parameter();" + System.Environment.NewLine;
            this.rtfCode.Text += "X3=puntero4.p.Find(p =>p.Nombre ==" + comillas + "X3" + comillas + ");" + System.Environment.NewLine;
            this.rtfCode.Text += "Parameter X4= new Parameter();" + System.Environment.NewLine;
            this.rtfCode.Text += "X4=puntero4.p.Find(p =>p.Nombre ==" + comillas + "X4" + comillas + ");" + System.Environment.NewLine;
            this.rtfCode.Text += "Parameter X5= new Parameter();" + System.Environment.NewLine;
            this.rtfCode.Text += "X5=puntero4.p.Find(p =>p.Nombre ==" + comillas + "X5" + comillas + ");" + System.Environment.NewLine;
            this.rtfCode.Text += "Parameter X6= new Parameter();" + System.Environment.NewLine;
            this.rtfCode.Text += "X6=puntero4.p.Find(p =>p.Nombre ==" + comillas + "X6" + comillas + ");" + System.Environment.NewLine;
            this.rtfCode.Text += "Parameter X7= new Parameter();" + System.Environment.NewLine;
            this.rtfCode.Text += "X7=puntero4.p.Find(p =>p.Nombre ==" + comillas + "X7" + comillas + ");" + System.Environment.NewLine;
            this.rtfCode.Text += "Parameter X8= new Parameter();" + System.Environment.NewLine;
            this.rtfCode.Text += "X8=puntero4.p.Find(p =>p.Nombre ==" + comillas + "X8" + comillas + ");" + System.Environment.NewLine;
            this.rtfCode.Text += "Parameter X9= new Parameter();" + System.Environment.NewLine;
            this.rtfCode.Text += "X9=puntero4.p.Find(p =>p.Nombre ==" + comillas + "X9" + comillas + ");" + System.Environment.NewLine;
            this.rtfCode.Text += "Parameter X10= new Parameter();" + System.Environment.NewLine;
            this.rtfCode.Text += "X10=puntero4.p.Find(p =>p.Nombre ==" + comillas + "X10" + comillas + ");" + System.Environment.NewLine;
            
            this.richTextBox1.Text += "" + System.Environment.NewLine;
            this.richTextBox1.Text += "" + System.Environment.NewLine;

            this.richTextBox1.Text += "Func <Double> Ecuacion1 = () => X2+(2*X6)+X9+(2*X10)-(10e-5);" + System.Environment.NewLine;
            this.richTextBox1.Text += "Func <Double> Ecuacion2 = () => X3+X8-(3*(10e-5));" + System.Environment.NewLine;
            this.richTextBox1.Text += "Func <Double> Ecuacion3 = () => X1+X3+(2*X5)+(2*X8)+X9+X10-(5*(10e-5));" + System.Environment.NewLine;
            this.richTextBox1.Text += "Func <Double> Ecuacion4 = () => X4+(2*X7)-(10e-5);" + System.Environment.NewLine;
            this.richTextBox1.Text += "Func <Double> Ecuacion5 = () => (0.5140437*(10e-7))*X5-(X1*X1);" + System.Environment.NewLine;
            this.richTextBox1.Text += "Func <Double> Ecuacion6 = () => (0.1006932*(10e-6))*X6-(2*X2*X2);" + System.Environment.NewLine;
            this.richTextBox1.Text += "Func <Double> Ecuacion7 = () => (0.7816278*(10e-15))*X7-(X4*X4);" + System.Environment.NewLine;
            this.richTextBox1.Text += "Func <Double> Ecuacion8 = () => (0.1496236*(10e-6))*X8-(X1*X3);" + System.Environment.NewLine;
            this.richTextBox1.Text += "Func <Double> Ecuacion9 = () => (0.6194411*(10e-7))*X9-(X1*X2);" + System.Environment.NewLine;
            this.richTextBox1.Text += "Func <Double> Ecuacion10 = () =>(0.2089296*(10e-14))*X10-(X1*X2*X2);" + System.Environment.NewLine;

            this.richTextBox1.Text += "" + System.Environment.NewLine;
            this.richTextBox1.Text += "" + System.Environment.NewLine;

            this.richTextBox1.Text += "puntero4.p[0].Value = 0.16;" + System.Environment.NewLine;
            this.richTextBox1.Text += "puntero4.p[1].Value = 0.1;" + System.Environment.NewLine;
            this.richTextBox1.Text += "puntero4.p[2].Value = 1;" + System.Environment.NewLine;
            this.richTextBox1.Text += "puntero4.p[3].Value = 1;" + System.Environment.NewLine;
            this.richTextBox1.Text += "puntero4.p[4].Value = 1;" + System.Environment.NewLine;
            this.richTextBox1.Text += "puntero4.p[5].Value = 1;" + System.Environment.NewLine;
            this.richTextBox1.Text += "puntero4.p[6].Value = 1;" + System.Environment.NewLine;
            this.richTextBox1.Text += "puntero4.p[7].Value = 1;" + System.Environment.NewLine;
            this.richTextBox1.Text += "puntero4.p[8].Value = 1;" + System.Environment.NewLine;
            this.richTextBox1.Text += "puntero4.p[9].Value = 0.12;" + System.Environment.NewLine;

            this.richTextBox2.Text += "puntero4.functions.Add(Ecuacion1);" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.functions.Add(Ecuacion2);" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.functions.Add(Ecuacion3);" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.functions.Add(Ecuacion4);" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.functions.Add(Ecuacion5);" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.functions.Add(Ecuacion6);" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.functions.Add(Ecuacion7);" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.functions.Add(Ecuacion8);" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.functions.Add(Ecuacion9);" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.functions.Add(Ecuacion10);" + System.Environment.NewLine;

            this.richTextBox2.Text += "puntero4.ejemplovalidacion = 1;" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.tipoanalisis=0;" + System.Environment.NewLine;

            this.richTextBox2.Text += "puntero4.numecuaciones = 10" + "+ puntero4.numecuaciones;" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.numvariables = 10" + "+ puntero4.numvariables;" + System.Environment.NewLine;

            this.richTextBox2.Text += "return 1.0;" + System.Environment.NewLine;

            //this.rtfCode.Text += "MessageBox.Show(\"This is a testmessage\");" + System.Environment.NewLine;
            this.richTextBox2.Text += "		}" + System.Environment.NewLine;
            this.richTextBox2.Text += "	}" + System.Environment.NewLine;
            this.richTextBox2.Text += "}" + System.Environment.NewLine;
        }

        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            elementoseleccionado = 0;

            for (int a = 0; a < listBox1.Items.Count; a++)
            {
                if (listBox1.SelectedItem == null)
                {
                    return;
                }
                if (listBox1.GetSelected(a) == true)
                {
                    elementoseleccionado = a;
                }
            }

            textBox5.Text= Convert.ToString(listBox1.Items[elementoseleccionado]);
            textBox6.Text = Convert.ToString(listBox1.Items[elementoseleccionado]);
        }

        //Borrar el archivo *.DLL seleccionado en la Lista
        private void button16_Click(object sender, EventArgs e)
        {       
            string sourceDir = Application.StartupPath + "\\";
            File.Delete(sourceDir+Convert.ToString(listBox1.Items[elementoseleccionado]));

            //Pulsamos el botón "list *.DLL Files"
            button2_Click(sender,e);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        //Interpolation Equation Sample1
        private void btnLoad_Click_1(object sender, EventArgs e)
        {
            this.rtfCode.Clear();
            this.richTextBox1.Clear();
            this.richTextBox2.Clear();

            this.rtfCode.Text += "using System;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Collections;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Xml;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.IO;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Windows.Forms;" + System.Environment.NewLine;
            this.rtfCode.Text += "using Drag_AND_Drop_between_Forms;" + System.Environment.NewLine;
            this.rtfCode.Text += System.Environment.NewLine;
            this.rtfCode.Text += "namespace CSharpScripter" + System.Environment.NewLine;
            this.rtfCode.Text += "{" + System.Environment.NewLine;
            this.rtfCode.Text += "	public class TestClass : CSharpScripter.Command" + System.Environment.NewLine;
            this.rtfCode.Text += "	{" + System.Environment.NewLine;
            this.rtfCode.Text += "Aplicacion puntero4;" + System.Environment.NewLine;
            this.rtfCode.Text += "		public TestClass()" + System.Environment.NewLine;
            this.rtfCode.Text += "		{" + System.Environment.NewLine;
            this.rtfCode.Text += "		}" + System.Environment.NewLine;
            this.rtfCode.Text += System.Environment.NewLine;

            this.rtfCode.Text += "		public Double Execute(Double a, Double b, Aplicacion puntero3) " + System.Environment.NewLine;
            this.rtfCode.Text += "		{" + System.Environment.NewLine;
            this.rtfCode.Text += "Double c=0;" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4=puntero3;" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.Titulo=" + comillas + textBox4.Text + comillas + ";" + System.Environment.NewLine;
            this.richTextBox1.Text += "" + System.Environment.NewLine;
            this.richTextBox1.Text += "" + System.Environment.NewLine;
            this.richTextBox1.Text += "c=Math.Pow(a,b);" + System.Environment.NewLine;
            this.richTextBox2.Text += "return c;" + System.Environment.NewLine;
            //this.rtfCode.Text += "			MessageBox.Show(\"This is a testmessage\");" + System.Environment.NewLine;
            this.richTextBox2.Text += "		}" + System.Environment.NewLine;

            this.richTextBox2.Text += "	}" + System.Environment.NewLine;
            this.richTextBox2.Text += "}" + System.Environment.NewLine;
        }

        //Interpolation Equation Sample2
        private void button13_Click_1(object sender, EventArgs e)
        {
            this.rtfCode.Clear();
            this.richTextBox1.Clear();
            this.richTextBox2.Clear();

            this.rtfCode.Text += "using System;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Collections;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Xml;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.IO;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Windows.Forms;" + System.Environment.NewLine;
            this.rtfCode.Text += "using Drag_AND_Drop_between_Forms;" + System.Environment.NewLine;
            this.rtfCode.Text += System.Environment.NewLine;
            this.rtfCode.Text += "namespace CSharpScripter" + System.Environment.NewLine;
            this.rtfCode.Text += "{" + System.Environment.NewLine;
            this.rtfCode.Text += "	public class TestClass : CSharpScripter.Command" + System.Environment.NewLine;
            this.rtfCode.Text += "	{" + System.Environment.NewLine;
            this.rtfCode.Text += "Aplicacion puntero4;" + System.Environment.NewLine;
            this.rtfCode.Text += "		public TestClass()" + System.Environment.NewLine;
            this.rtfCode.Text += "		{" + System.Environment.NewLine;
            this.rtfCode.Text += "		}" + System.Environment.NewLine;
            this.rtfCode.Text += System.Environment.NewLine;
            this.rtfCode.Text += "		public Double Execute(Double a, Double b, Aplicacion puntero3) " + System.Environment.NewLine;
            this.rtfCode.Text += "		{" + System.Environment.NewLine;
            this.rtfCode.Text += "Double c=0;" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4=puntero3;" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.Titulo=" + comillas + textBox4.Text + comillas + ";" + System.Environment.NewLine;
            this.richTextBox1.Text += "" + System.Environment.NewLine;
            this.richTextBox1.Text += "" + System.Environment.NewLine;
            this.richTextBox1.Text += "c=11+(b*a)+(b*a*a)+(b*a*a*a)+(b*a*a*a*a);" + System.Environment.NewLine;
            this.richTextBox2.Text += "return c;" + System.Environment.NewLine;
            //this.rtfCode.Text += "			MessageBox.Show(\"This is a testmessage\");" + System.Environment.NewLine;
            this.richTextBox2.Text += "		}" + System.Environment.NewLine;
            this.richTextBox2.Text += "	}" + System.Environment.NewLine;
            this.richTextBox2.Text += "}" + System.Environment.NewLine;
        }

        //Linear Equations System Sample1
        private void button3_Click_1(object sender, EventArgs e)
        {
            this.rtfCode.Clear();
            this.richTextBox1.Clear();
            this.richTextBox2.Clear();

            //Llamamos a la opción del Menú de Nuevo Cálculo
            puntero.toolStripMenuItem11_Click(sender, e);

            this.rtfCode.Text += "using System.Collections.Generic;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.ComponentModel;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Data;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System;" + System.Environment.NewLine;
            //this.rtfCode.Text += "using System.Linq;" + System.Environment.NewLine;
            //this.rtfCode.Text += "using System.Text;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Collections;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Xml;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.IO;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Windows.Forms;" + System.Environment.NewLine;
            this.rtfCode.Text += "using Drag_AND_Drop_between_Forms;" + System.Environment.NewLine;
            this.rtfCode.Text += "using NumericalMethods;" + System.Environment.NewLine;
            this.rtfCode.Text += "using NumericalMethods.FourthBlog;" + System.Environment.NewLine;
            this.rtfCode.Text += "using ClaseEquipos;" + System.Environment.NewLine;

            this.rtfCode.Text += System.Environment.NewLine;
            this.rtfCode.Text += "namespace CSharpScripter1" + System.Environment.NewLine;
            this.rtfCode.Text += "{" + System.Environment.NewLine;
            this.rtfCode.Text += "	public class TestClass : CSharpScripter1.Command1" + System.Environment.NewLine;
            this.rtfCode.Text += "	{" + System.Environment.NewLine;
            this.rtfCode.Text += "Aplicacion puntero4;" + System.Environment.NewLine;
            this.rtfCode.Text += "		public TestClass()" + System.Environment.NewLine;
            this.rtfCode.Text += "		{" + System.Environment.NewLine;
            this.rtfCode.Text += "		}" + System.Environment.NewLine;
            this.rtfCode.Text += System.Environment.NewLine;
            this.rtfCode.Text += "		public void Execute1(Aplicacion puntero3) " + System.Environment.NewLine;
            this.rtfCode.Text += "		{" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4=puntero3;" + System.Environment.NewLine;

            int necuaciones = 0;
            necuaciones = Convert.ToInt16(textBox7.Text);

            //CREAMOS EL ARRAY DE PARAMETROS
            this.rtfCode.Text += "for (int v = 0; v <4; v++)" + System.Environment.NewLine;
            this.rtfCode.Text += "{" + System.Environment.NewLine;
            //Creamos la lista de parámetros generadas por este programa
            this.rtfCode.Text += "puntero4.p.Add(puntero4.ptemp);" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p[v] = new Parameter(1,0.01," + comillas + comillas + ");" + System.Environment.NewLine;
            this.rtfCode.Text += "}" + System.Environment.NewLine;

            //Asignamos las Condiciones Iniciales a las dos variables, en este caso Xi....Xn
            this.rtfCode.Text += "puntero4.p[0].Nombre = " + comillas + "a0" + comillas + ";" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p[1].Nombre = " + comillas + "a1" + comillas + ";" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p[2].Nombre = " + comillas + "a2" + comillas + ";" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p[3].Nombre = " + comillas + "a3" + comillas + ";" + System.Environment.NewLine;

            this.rtfCode.Text += "List < String > ecuaciones2 = new List<String>();" + System.Environment.NewLine;

            this.rtfCode.Text += "Parameter a0= new Parameter();" + System.Environment.NewLine;
            this.rtfCode.Text += "a0=puntero4.p.Find(p =>p.Nombre ==" + comillas + "a0" + comillas + ");" + System.Environment.NewLine;
            this.rtfCode.Text += "Parameter a1= new Parameter();" + System.Environment.NewLine;
            this.rtfCode.Text += "a1=puntero4.p.Find(p =>p.Nombre ==" + comillas + "a1" + comillas + ");" + System.Environment.NewLine;
            this.rtfCode.Text += "Parameter a2= new Parameter();" + System.Environment.NewLine;
            this.rtfCode.Text += "a2=puntero4.p.Find(p =>p.Nombre ==" + comillas + "a2" + comillas + ");" + System.Environment.NewLine;
            this.rtfCode.Text += "Parameter a3= new Parameter();" + System.Environment.NewLine;
            this.rtfCode.Text += "a3=puntero4.p.Find(p =>p.Nombre ==" + comillas + "a3" + comillas + ");" + System.Environment.NewLine;

            this.richTextBox1.Text += "" + System.Environment.NewLine;
            this.richTextBox1.Text += "" + System.Environment.NewLine;

            this.richTextBox1.Text += "Func <Double> Ecuacion1 = () => a0-(3*a1)+(9*a2)-(27*a3)+2;" + System.Environment.NewLine;
            this.richTextBox1.Text += "Func <Double> Ecuacion2 = () => a0-a1+a2-a3-2;" + System.Environment.NewLine;
            this.richTextBox1.Text += "Func <Double> Ecuacion3 = () => a0+a1+a2+a3-5;" + System.Environment.NewLine;
            this.richTextBox1.Text += "Func <Double> Ecuacion4 = () => a0+(2*a1)+(4*a2)+(8*a3)-1;" + System.Environment.NewLine;

            this.richTextBox1.Text += "" + System.Environment.NewLine;
            this.richTextBox1.Text += "" + System.Environment.NewLine;

            this.richTextBox1.Text += "puntero4.p[0].Value = 0;" + System.Environment.NewLine;
            this.richTextBox1.Text += "puntero4.p[1].Value = 0;" + System.Environment.NewLine;
            this.richTextBox1.Text += "puntero4.p[2].Value = 0;" + System.Environment.NewLine;
            this.richTextBox1.Text += "puntero4.p[3].Value = 0;" + System.Environment.NewLine;

            this.richTextBox2.Text += "puntero4.functions.Add(Ecuacion1);" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.functions.Add(Ecuacion2);" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.functions.Add(Ecuacion3);" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.functions.Add(Ecuacion4);" + System.Environment.NewLine;

            this.richTextBox2.Text += "puntero4.ejemplovalidacion = 1;" + System.Environment.NewLine;
            this.richTextBox2.Text += "//Análisis Tipo Estacionario (tipoanalisis=0)" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.tipoanalisis=0;" + System.Environment.NewLine;

            this.richTextBox2.Text += "puntero4.numecuaciones = 4" + "+ puntero4.numecuaciones;" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.numvariables = 4" + "+ puntero4.numvariables;" + System.Environment.NewLine;

            this.richTextBox2.Text += "		}" + System.Environment.NewLine;
            this.richTextBox2.Text += "	}" + System.Environment.NewLine;
            this.richTextBox2.Text += "}" + System.Environment.NewLine;
        }

        //Linear Equations System Sample2
        private void button1_Click_1(object sender, EventArgs e)
        {
            this.rtfCode.Clear();
            this.richTextBox1.Clear();
            this.richTextBox2.Clear();

            //Llamamos a la opción del Menú de Nuevo Cálculo
            puntero.toolStripMenuItem11_Click(sender, e);

            this.rtfCode.Text += "using System.Collections.Generic;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.ComponentModel;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Data;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System;" + System.Environment.NewLine;
            //this.rtfCode.Text += "using System.Linq;" + System.Environment.NewLine;
            //this.rtfCode.Text += "using System.Text;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Collections;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Xml;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.IO;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Windows.Forms;" + System.Environment.NewLine;
            this.rtfCode.Text += "using Drag_AND_Drop_between_Forms;" + System.Environment.NewLine;
            this.rtfCode.Text += "using NumericalMethods;" + System.Environment.NewLine;
            this.rtfCode.Text += "using NumericalMethods.FourthBlog;" + System.Environment.NewLine;
            this.rtfCode.Text += "using ClaseEquipos;" + System.Environment.NewLine;

            this.rtfCode.Text += System.Environment.NewLine;
            this.rtfCode.Text += "namespace CSharpScripter1" + System.Environment.NewLine;
            this.rtfCode.Text += "{" + System.Environment.NewLine;
            this.rtfCode.Text += "	public class TestClass : CSharpScripter1.Command1" + System.Environment.NewLine;
            this.rtfCode.Text += "	{" + System.Environment.NewLine;
            this.rtfCode.Text += "Aplicacion puntero4;" + System.Environment.NewLine;
            this.rtfCode.Text += "		public TestClass()" + System.Environment.NewLine;
            this.rtfCode.Text += "		{" + System.Environment.NewLine;
            this.rtfCode.Text += "		}" + System.Environment.NewLine;
            this.rtfCode.Text += System.Environment.NewLine;
            this.rtfCode.Text += "		public void Execute1(Aplicacion puntero3) " + System.Environment.NewLine;
            this.rtfCode.Text += "		{" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4=puntero3;" + System.Environment.NewLine;

            int necuaciones = 0;
            necuaciones = Convert.ToInt16(textBox7.Text);

            //CREAMOS EL ARRAY DE PARAMETROS
            this.rtfCode.Text += " for (int v = 0; v <3; v++)" + System.Environment.NewLine;
            this.rtfCode.Text += "{" + System.Environment.NewLine;
            //Creamos la lista de parámetros generadas por este programa
            this.rtfCode.Text += "puntero4.p.Add(puntero4.ptemp);" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p[v] = new Parameter(1,0.01," + comillas + comillas + ");" + System.Environment.NewLine;
            this.rtfCode.Text += "}" + System.Environment.NewLine;

            //Asignamos las Condiciones Iniciales a las dos variables, en este caso Xi....Xn
            this.rtfCode.Text += "puntero4.p[0].Nombre = " + comillas + "X1" + comillas + ";" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p[1].Nombre = " + comillas + "X2" + comillas + ";" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p[2].Nombre = " + comillas + "X3" + comillas + ";" + System.Environment.NewLine;

            this.rtfCode.Text += "List < String > ecuaciones2 = new List<String>();" + System.Environment.NewLine;

            this.rtfCode.Text += "Parameter X1= new Parameter();" + System.Environment.NewLine;
            this.rtfCode.Text += "X1=puntero4.p.Find(p =>p.Nombre ==" + comillas + "X1" + comillas + ");" + System.Environment.NewLine;
            this.rtfCode.Text += "Parameter X2= new Parameter();" + System.Environment.NewLine;
            this.rtfCode.Text += "X2=puntero4.p.Find(p =>p.Nombre ==" + comillas + "X2" + comillas + ");" + System.Environment.NewLine;
            this.rtfCode.Text += "Parameter X3= new Parameter();" + System.Environment.NewLine;
            this.rtfCode.Text += "X3=puntero4.p.Find(p =>p.Nombre ==" + comillas + "X3" + comillas + ");" + System.Environment.NewLine;

            this.richTextBox1.Text += "" + System.Environment.NewLine;
            this.richTextBox1.Text += "" + System.Environment.NewLine;

            this.richTextBox1.Text += "Func <Double> Ecuacion1 = () => 10*X1-7*X2-7;" + System.Environment.NewLine;
            this.richTextBox1.Text += "Func <Double> Ecuacion2 = () => -3*X1+2*X2+6*X3-4;" + System.Environment.NewLine;
            this.richTextBox1.Text += "Func <Double> Ecuacion3 = () => 5*X1-X2+5*X3-6;" + System.Environment.NewLine;

            this.richTextBox1.Text += "" + System.Environment.NewLine;
            this.richTextBox1.Text += "" + System.Environment.NewLine;

            this.richTextBox1.Text += "puntero4.p[0].Value = 0.1;" + System.Environment.NewLine;
            this.richTextBox1.Text += "puntero4.p[1].Value = 0.2;" + System.Environment.NewLine;
            this.richTextBox1.Text += "puntero4.p[2].Value = 0.3;" + System.Environment.NewLine;

            this.richTextBox2.Text += "puntero4.functions.Add(Ecuacion1);" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.functions.Add(Ecuacion2);" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.functions.Add(Ecuacion3);" + System.Environment.NewLine;

            this.richTextBox2.Text += "puntero4.ejemplovalidacion = 1;" + System.Environment.NewLine;
            this.richTextBox2.Text += "//Análisis Tipo Estacionario (tipoanalisis=0)" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.tipoanalisis=0;" + System.Environment.NewLine;

            this.richTextBox2.Text += "puntero4.numecuaciones = 3" + "+ puntero4.numecuaciones;" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.numvariables = 3" + "+ puntero4.numvariables;" + System.Environment.NewLine;

            this.richTextBox2.Text += "		}" + System.Environment.NewLine;
            this.richTextBox2.Text += "	}" + System.Environment.NewLine;
            this.richTextBox2.Text += "}" + System.Environment.NewLine;
        }

        //Non Linear Equations System Sample1
        private void button5_Click_1(object sender, EventArgs e)
        {
            this.rtfCode.Clear();
            this.richTextBox1.Clear();
            this.richTextBox2.Clear();

            //Llamamos a la opción del Menú de Nuevo Cálculo
            puntero.toolStripMenuItem11_Click(sender, e);

            this.rtfCode.Text += "using System.Collections.Generic;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.ComponentModel;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Data;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System;" + System.Environment.NewLine;
            //this.rtfCode.Text += "using System.Linq;" + System.Environment.NewLine;
            //this.rtfCode.Text += "using System.Text;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Collections;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Xml;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.IO;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Windows.Forms;" + System.Environment.NewLine;
            this.rtfCode.Text += "using Drag_AND_Drop_between_Forms;" + System.Environment.NewLine;
            this.rtfCode.Text += "using NumericalMethods;" + System.Environment.NewLine;
            this.rtfCode.Text += "using NumericalMethods.FourthBlog;" + System.Environment.NewLine;
            this.rtfCode.Text += "using ClaseEquipos;" + System.Environment.NewLine;

            this.rtfCode.Text += System.Environment.NewLine;
            this.rtfCode.Text += "namespace CSharpScripter1" + System.Environment.NewLine;
            this.rtfCode.Text += "{" + System.Environment.NewLine;
            this.rtfCode.Text += "	public class TestClass : CSharpScripter1.Command1" + System.Environment.NewLine;
            this.rtfCode.Text += "	{" + System.Environment.NewLine;
            this.rtfCode.Text += "Aplicacion puntero4;" + System.Environment.NewLine;
            this.rtfCode.Text += "		public TestClass()" + System.Environment.NewLine;
            this.rtfCode.Text += "		{" + System.Environment.NewLine;
            this.rtfCode.Text += "		}" + System.Environment.NewLine;
            this.rtfCode.Text += System.Environment.NewLine;
            this.rtfCode.Text += "		public void Execute1(Aplicacion puntero3) " + System.Environment.NewLine;
            this.rtfCode.Text += "		{" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4=puntero3;" + System.Environment.NewLine;

            int necuaciones = 0;
            necuaciones = Convert.ToInt16(textBox7.Text);

            //CREAMOS EL ARRAY DE PARAMETROS
            this.rtfCode.Text += " for (int v = 0; v < 2; v++)" + System.Environment.NewLine;
            this.rtfCode.Text += "{" + System.Environment.NewLine;
            //Creamos la lista de parámetros generadas por este programa
            this.rtfCode.Text += "puntero4.p.Add(puntero4.ptemp);" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p[v] = new Parameter(1,0.01," + comillas + comillas + ");" + System.Environment.NewLine;
            this.rtfCode.Text += "}" + System.Environment.NewLine;

            //Asignamos las Condiciones Iniciales a las dos variables, en este caso Xi....Xn
            this.rtfCode.Text += "puntero4.p[0].Nombre = " + comillas + "X1" + comillas + ";" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p[1].Nombre = " + comillas + "X2" + comillas + ";" + System.Environment.NewLine;

            this.rtfCode.Text += "List < String > ecuaciones2 = new List<String>();" + System.Environment.NewLine;

            this.rtfCode.Text += "Parameter X1= new Parameter();" + System.Environment.NewLine;
            this.rtfCode.Text += "X1=puntero4.p.Find(p =>p.Nombre ==" + comillas + "X1" + comillas + ");" + System.Environment.NewLine;
            this.rtfCode.Text += "Parameter X2= new Parameter();" + System.Environment.NewLine;
            this.rtfCode.Text += "X2=puntero4.p.Find(p =>p.Nombre ==" + comillas + "X2" + comillas + ");" + System.Environment.NewLine;

            this.richTextBox1.Text += "" + System.Environment.NewLine;
            this.richTextBox1.Text += "" + System.Environment.NewLine;

            this.richTextBox1.Text += "Func <Double> Ecuacion1 = () => Math.Cos(2*X1)-Math.Cos(2*X2)-0.4;" + System.Environment.NewLine;
            this.richTextBox1.Text += "Func <Double> Ecuacion2 = () => (2*(X2-X1))+Math.Sin(2*X2)-Math.Sin(2*X1)-1.2;" + System.Environment.NewLine;

            this.richTextBox1.Text += "" + System.Environment.NewLine;
            this.richTextBox1.Text += "" + System.Environment.NewLine;

            this.richTextBox1.Text += "puntero4.p[0].Value = 0.2;" + System.Environment.NewLine;
            this.richTextBox1.Text += "puntero4.p[1].Value = 0.3;" + System.Environment.NewLine;

            this.richTextBox2.Text += "puntero4.functions.Add(Ecuacion1);" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.functions.Add(Ecuacion2);" + System.Environment.NewLine;

            this.richTextBox2.Text += "puntero4.ejemplovalidacion = 1;" + System.Environment.NewLine;
            this.richTextBox2.Text += "//Análisis Tipo Estacionario (tipoanalisis=0)" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.tipoanalisis=0;" + System.Environment.NewLine;

            this.richTextBox2.Text += "puntero4.numecuaciones = 2" + "+ puntero4.numecuaciones;" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.numvariables = 2" + "+ puntero4.numvariables;" + System.Environment.NewLine;

            this.richTextBox2.Text += "		}" + System.Environment.NewLine;
            this.richTextBox2.Text += "	}" + System.Environment.NewLine;
            this.richTextBox2.Text += "}" + System.Environment.NewLine;
        }

        //Non Linear Equations System Sample2
        private void button6_Click_1(object sender, EventArgs e)
        {
            this.rtfCode.Clear();
            this.richTextBox1.Clear();
            this.richTextBox2.Clear();

            //Llamamos a la opción del Menú de Nuevo Cálculo
            puntero.toolStripMenuItem11_Click(sender, e);

            this.rtfCode.Text += "using System.Collections.Generic;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.ComponentModel;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Data;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System;" + System.Environment.NewLine;
            //this.rtfCode.Text += "using System.Linq;" + System.Environment.NewLine;
            //this.rtfCode.Text += "using System.Text;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Collections;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Xml;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.IO;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Windows.Forms;" + System.Environment.NewLine;
            this.rtfCode.Text += "using Drag_AND_Drop_between_Forms;" + System.Environment.NewLine;
            this.rtfCode.Text += "using NumericalMethods;" + System.Environment.NewLine;
            this.rtfCode.Text += "using NumericalMethods.FourthBlog;" + System.Environment.NewLine;
            this.rtfCode.Text += "using ClaseEquipos;" + System.Environment.NewLine;

            this.rtfCode.Text += System.Environment.NewLine;
            this.rtfCode.Text += "namespace CSharpScripter1" + System.Environment.NewLine;
            this.rtfCode.Text += "{" + System.Environment.NewLine;
            this.rtfCode.Text += "	public class TestClass : CSharpScripter1.Command1" + System.Environment.NewLine;
            this.rtfCode.Text += "	{" + System.Environment.NewLine;
            this.rtfCode.Text += "Aplicacion puntero4;" + System.Environment.NewLine;
            this.rtfCode.Text += "		public TestClass()" + System.Environment.NewLine;
            this.rtfCode.Text += "		{" + System.Environment.NewLine;
            this.rtfCode.Text += "		}" + System.Environment.NewLine;
            this.rtfCode.Text += System.Environment.NewLine;
            this.rtfCode.Text += "		public void Execute1(Aplicacion puntero3) " + System.Environment.NewLine;
            this.rtfCode.Text += "		{" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4=puntero3;" + System.Environment.NewLine;

            int necuaciones = 0;
            necuaciones = Convert.ToInt16(textBox7.Text);

            //CREAMOS EL ARRAY DE PARAMETROS
            this.rtfCode.Text += " for (int v = 0; v < 2; v++)" + System.Environment.NewLine;
            this.rtfCode.Text += "{" + System.Environment.NewLine;
            //Creamos la lista de parámetros generadas por este programa
            this.rtfCode.Text += "puntero4.p.Add(puntero4.ptemp);" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p[v] = new Parameter(1,0.01," + comillas + comillas + ");" + System.Environment.NewLine;
            this.rtfCode.Text += "}" + System.Environment.NewLine;

            //Asignamos las Condiciones Iniciales a las dos variables, en este caso Xi....Xn
            this.rtfCode.Text += "puntero4.p[0].Nombre = " + comillas + "X1" + comillas + ";" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p[1].Nombre = " + comillas + "X2" + comillas + ";" + System.Environment.NewLine;

            this.rtfCode.Text += "List < String > ecuaciones2 = new List<String>();" + System.Environment.NewLine;

            this.rtfCode.Text += "Parameter X1= new Parameter();" + System.Environment.NewLine;
            this.rtfCode.Text += "X1=puntero4.p.Find(p =>p.Nombre ==" + comillas + "X1" + comillas + ");" + System.Environment.NewLine;
            this.rtfCode.Text += "Parameter X2= new Parameter();" + System.Environment.NewLine;
            this.rtfCode.Text += "X2=puntero4.p.Find(p =>p.Nombre ==" + comillas + "X2" + comillas + ");" + System.Environment.NewLine;

            this.richTextBox1.Text += "" + System.Environment.NewLine;
            this.richTextBox1.Text += "" + System.Environment.NewLine;

            this.richTextBox1.Text += "Func <Double> Ecuacion1 = () => Math.Exp(X1)+(X1*X2)-1;" + System.Environment.NewLine;
            this.richTextBox1.Text += "Func <Double> Ecuacion2 = () => Math.Sin(X1*X2)+X1+X2-1;" + System.Environment.NewLine;

            this.richTextBox1.Text += "" + System.Environment.NewLine;
            this.richTextBox1.Text += "" + System.Environment.NewLine;

            this.richTextBox1.Text += "puntero4.p[0].Value = 0.1;" + System.Environment.NewLine;
            this.richTextBox1.Text += "puntero4.p[1].Value = 0.1;" + System.Environment.NewLine;

            this.richTextBox2.Text += "puntero4.functions.Add(Ecuacion1);" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.functions.Add(Ecuacion2);" + System.Environment.NewLine;

            this.richTextBox2.Text += "puntero4.ejemplovalidacion = 1;" + System.Environment.NewLine;
            this.richTextBox2.Text += "//Análisis Tipo Estacionario (tipoanalisis=0)" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.tipoanalisis=0;" + System.Environment.NewLine;

            this.richTextBox2.Text += "puntero4.numecuaciones = 2" + "+ puntero4.numecuaciones;" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.numvariables = 2" + "+ puntero4.numvariables;" + System.Environment.NewLine;

            this.richTextBox2.Text += "		}" + System.Environment.NewLine;
            this.richTextBox2.Text += "	}" + System.Environment.NewLine;
            this.richTextBox2.Text += "}" + System.Environment.NewLine;
        }

        //Non Linear Equations System Sample3
        private void button7_Click_1(object sender, EventArgs e)
        {
            this.rtfCode.Clear();
            this.richTextBox1.Clear();
            this.richTextBox2.Clear();

            //Llamamos a la opción del Menú de Nuevo Cálculo
            puntero.toolStripMenuItem11_Click(sender, e);

            this.rtfCode.Text += "using System.Collections.Generic;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.ComponentModel;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Data;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System;" + System.Environment.NewLine;
            //this.rtfCode.Text += "using System.Linq;" + System.Environment.NewLine;
            //this.rtfCode.Text += "using System.Text;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Collections;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Xml;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.IO;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Windows.Forms;" + System.Environment.NewLine;
            this.rtfCode.Text += "using Drag_AND_Drop_between_Forms;" + System.Environment.NewLine;
            this.rtfCode.Text += "using NumericalMethods;" + System.Environment.NewLine;
            this.rtfCode.Text += "using NumericalMethods.FourthBlog;" + System.Environment.NewLine;
            this.rtfCode.Text += "using ClaseEquipos;" + System.Environment.NewLine;

            this.rtfCode.Text += System.Environment.NewLine;
            this.rtfCode.Text += "namespace CSharpScripter1" + System.Environment.NewLine;
            this.rtfCode.Text += "{" + System.Environment.NewLine;
            this.rtfCode.Text += "	public class TestClass : CSharpScripter1.Command1" + System.Environment.NewLine;
            this.rtfCode.Text += "	{" + System.Environment.NewLine;
            this.rtfCode.Text += "Aplicacion puntero4;" + System.Environment.NewLine;
            this.rtfCode.Text += "		public TestClass()" + System.Environment.NewLine;
            this.rtfCode.Text += "		{" + System.Environment.NewLine;
            this.rtfCode.Text += "		}" + System.Environment.NewLine;
            this.rtfCode.Text += System.Environment.NewLine;
            this.rtfCode.Text += "		public void Execute1(Aplicacion puntero3) " + System.Environment.NewLine;
            this.rtfCode.Text += "		{" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4=puntero3;" + System.Environment.NewLine;

            int necuaciones = 0;
            necuaciones = Convert.ToInt16(textBox7.Text);

            //CREAMOS EL ARRAY DE PARAMETROS
            this.rtfCode.Text += " for (int v = 0; v < " + Convert.ToString(necuaciones) + "; v++)" + System.Environment.NewLine;
            this.rtfCode.Text += "{" + System.Environment.NewLine;
            //Creamos la lista de parámetros generadas por este programa
            this.rtfCode.Text += "puntero4.p.Add(puntero4.ptemp);" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p[v] = new Parameter(1,0.01," + comillas + comillas + ");" + System.Environment.NewLine;
            this.rtfCode.Text += "}" + System.Environment.NewLine;

            //Asignamos las Condiciones Iniciales a las dos variables, en este caso Xi....Xn
            for (int j = 1; j <= necuaciones; j++)
            {
                this.rtfCode.Text += "puntero4.p[" + Convert.ToString(j - 1) + "].Nombre = " + comillas + "X" + comillas + "+" + "Convert.ToString(" + Convert.ToString(j) + ");" + System.Environment.NewLine;
            }

            this.rtfCode.Text += "List < String > ecuaciones2 = new List<String>();" + System.Environment.NewLine;

            for (int n = 1; n <= necuaciones; n++)
            {
                this.rtfCode.Text += "Parameter X" + Convert.ToString(n) + "= new Parameter();" + System.Environment.NewLine;
                this.rtfCode.Text += "X" + Convert.ToString(n) + "=puntero4.p.Find(p =>p.Nombre ==" + comillas + "X" + comillas + "+" + "Convert.ToString(" + Convert.ToString(n) + "));" + System.Environment.NewLine;
            }

            for (int i = 1; i <= necuaciones; i++)
            {
                this.richTextBox1.Text += "Func <Double> Ecuacion" + Convert.ToString(i) + " = () => "+Convert.ToString(Math.Pow(-i,i))+"*"+"X1 + 2 * X2 - 2+"+Convert.ToString(i*5)+";" + System.Environment.NewLine;
            }

            for (int i = 1; i <= necuaciones; i++)
            {
                this.richTextBox1.Text += "puntero4.p[" + Convert.ToString(i - 1) + "].Value = 1.0;" + System.Environment.NewLine;
                this.richTextBox2.Text += "puntero4.functions.Add(Ecuacion" + Convert.ToString(i) + ");" + System.Environment.NewLine;
            }

            this.richTextBox2.Text += "puntero4.ejemplovalidacion = 1;" + System.Environment.NewLine;
            this.richTextBox2.Text += "//Análisis Tipo Estacionario (tipoanalisis=0)" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.tipoanalisis=0;" + System.Environment.NewLine;

            this.richTextBox2.Text += "puntero4.numecuaciones = " + Convert.ToString(necuaciones) + "+ puntero4.numecuaciones;" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.numvariables = " + Convert.ToString(necuaciones) + "+ puntero4.numvariables;" + System.Environment.NewLine;

            this.richTextBox2.Text += "		}" + System.Environment.NewLine;
            this.richTextBox2.Text += "	}" + System.Environment.NewLine;
            this.richTextBox2.Text += "}" + System.Environment.NewLine;
        }

        //Non Linear Equations System Sample4
        private void button4_Click_1(object sender, EventArgs e)
        {
            this.rtfCode.Clear();
            this.richTextBox1.Clear();
            this.richTextBox2.Clear();

            //Llamamos a la opción del Menú de Nuevo Cálculo
            puntero.toolStripMenuItem11_Click(sender, e);

            this.rtfCode.Text += "using System.Collections.Generic;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.ComponentModel;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Data;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System;" + System.Environment.NewLine;
            //this.rtfCode.Text += "using System.Linq;" + System.Environment.NewLine;
            //this.rtfCode.Text += "using System.Text;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Collections;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Xml;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.IO;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Windows.Forms;" + System.Environment.NewLine;
            this.rtfCode.Text += "using Drag_AND_Drop_between_Forms;" + System.Environment.NewLine;
            this.rtfCode.Text += "using NumericalMethods;" + System.Environment.NewLine;
            this.rtfCode.Text += "using NumericalMethods.FourthBlog;" + System.Environment.NewLine;
            this.rtfCode.Text += "using ClaseEquipos;" + System.Environment.NewLine;

            this.rtfCode.Text += System.Environment.NewLine;
            this.rtfCode.Text += "namespace CSharpScripter1" + System.Environment.NewLine;
            this.rtfCode.Text += "{" + System.Environment.NewLine;
            this.rtfCode.Text += "	public class TestClass : CSharpScripter1.Command1" + System.Environment.NewLine;
            this.rtfCode.Text += "	{" + System.Environment.NewLine;
            this.rtfCode.Text += "Aplicacion puntero4;" + System.Environment.NewLine;
            this.rtfCode.Text += "		public TestClass()" + System.Environment.NewLine;
            this.rtfCode.Text += "		{" + System.Environment.NewLine;
            this.rtfCode.Text += "		}" + System.Environment.NewLine;
            this.rtfCode.Text += System.Environment.NewLine;
            this.rtfCode.Text += "		public void Execute1(Aplicacion puntero3) " + System.Environment.NewLine;
            this.rtfCode.Text += "		{" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4=puntero3;" + System.Environment.NewLine;

            int necuaciones = 0;
            necuaciones = Convert.ToInt16(textBox7.Text);

            //CREAMOS EL ARRAY DE PARAMETROS
            this.rtfCode.Text += " for (int v = 0; v < 6; v++)" + System.Environment.NewLine;
            this.rtfCode.Text += "{" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p.Add(puntero4.ptemp);" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p[v] = new Parameter(1,0.01," + comillas + comillas + ");" + System.Environment.NewLine;
            this.rtfCode.Text += "}" + System.Environment.NewLine;

            //Asignamos las Condiciones Iniciales a las dos variables, en este caso Xi....Xn
            this.rtfCode.Text += "puntero4.p[0].Nombre = " + comillas + "X1" + comillas + ";" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p[1].Nombre = " + comillas + "X2" + comillas + ";" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p[2].Nombre = " + comillas + "X3" + comillas + ";" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p[3].Nombre = " + comillas + "X4" + comillas + ";" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p[4].Nombre = " + comillas + "X5" + comillas + ";" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p[5].Nombre = " + comillas + "X6" + comillas + ";" + System.Environment.NewLine;

            this.rtfCode.Text += "List < String > ecuaciones2 = new List<String>();" + System.Environment.NewLine;

            this.rtfCode.Text += "Parameter X1= new Parameter();" + System.Environment.NewLine;
            this.rtfCode.Text += "X1=puntero4.p.Find(p =>p.Nombre ==" + comillas + "X1" + comillas + ");" + System.Environment.NewLine;
            this.rtfCode.Text += "Parameter X2= new Parameter();" + System.Environment.NewLine;
            this.rtfCode.Text += "X2=puntero4.p.Find(p =>p.Nombre ==" + comillas + "X2" + comillas + ");" + System.Environment.NewLine;
            this.rtfCode.Text += "Parameter X3= new Parameter();" + System.Environment.NewLine;
            this.rtfCode.Text += "X3=puntero4.p.Find(p =>p.Nombre ==" + comillas + "X3" + comillas + ");" + System.Environment.NewLine;
            this.rtfCode.Text += "Parameter X4= new Parameter();" + System.Environment.NewLine;
            this.rtfCode.Text += "X4=puntero4.p.Find(p =>p.Nombre ==" + comillas + "X4" + comillas + ");" + System.Environment.NewLine;
            this.rtfCode.Text += "Parameter X5= new Parameter();" + System.Environment.NewLine;
            this.rtfCode.Text += "X5=puntero4.p.Find(p =>p.Nombre ==" + comillas + "X5" + comillas + ");" + System.Environment.NewLine;
            this.rtfCode.Text += "Parameter X6= new Parameter();" + System.Environment.NewLine;
            this.rtfCode.Text += "X6=puntero4.p.Find(p =>p.Nombre ==" + comillas + "X6" + comillas + ");" + System.Environment.NewLine;

            this.richTextBox1.Text += "" + System.Environment.NewLine;
            this.richTextBox1.Text += "" + System.Environment.NewLine;

            this.richTextBox1.Text += "Func <Double> Ecuacion1 = () => (X1*X1)+(X3*X3)-1;" + System.Environment.NewLine;
            this.richTextBox1.Text += "Func <Double> Ecuacion2 = () => (X2*X2)+(X4*X4)-1;" + System.Environment.NewLine;
            this.richTextBox1.Text += "Func <Double> Ecuacion3 = () => (X5*X3*X3*X3)+(X6*X4*X4*X4);" + System.Environment.NewLine;
            this.richTextBox1.Text += "Func <Double> Ecuacion4 = () => (X5*X1*X1*X1)+(X6*X2*X2*X2);" + System.Environment.NewLine;
            this.richTextBox1.Text += "Func <Double> Ecuacion5 = () => (X5*X1*X3*X3*X3)+(X6*X4*X4*X2);" + System.Environment.NewLine;
            this.richTextBox1.Text += "Func <Double> Ecuacion6 = () => (X5*X1*X1*X3)+(X6*X2*X2*X4);" + System.Environment.NewLine;

            this.richTextBox1.Text += "" + System.Environment.NewLine;
            this.richTextBox1.Text += "" + System.Environment.NewLine;

            this.richTextBox1.Text += "puntero4.p[0].Value = -1;" + System.Environment.NewLine;
            this.richTextBox1.Text += "puntero4.p[1].Value = -1;" + System.Environment.NewLine;
            this.richTextBox1.Text += "puntero4.p[2].Value = 0.6;" + System.Environment.NewLine;
            this.richTextBox1.Text += "puntero4.p[3].Value = -0.2;" + System.Environment.NewLine;
            this.richTextBox1.Text += "puntero4.p[4].Value = 0;" + System.Environment.NewLine;
            this.richTextBox1.Text += "puntero4.p[5].Value = 0;" + System.Environment.NewLine;

            this.richTextBox2.Text += "puntero4.functions.Add(Ecuacion1);" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.functions.Add(Ecuacion2);" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.functions.Add(Ecuacion3);" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.functions.Add(Ecuacion4);" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.functions.Add(Ecuacion5);" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.functions.Add(Ecuacion6);" + System.Environment.NewLine;

            this.richTextBox2.Text += "puntero4.ejemplovalidacion = 1;" + System.Environment.NewLine;
            this.richTextBox2.Text += "//Análisis Tipo Estacionario (tipoanalisis=0)" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.tipoanalisis=0;" + System.Environment.NewLine;

            this.richTextBox2.Text += "puntero4.numecuaciones = 6" + "+ puntero4.numecuaciones;" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.numvariables = 6" + "+ puntero4.numvariables;" + System.Environment.NewLine;

            this.richTextBox2.Text += "		}" + System.Environment.NewLine;
            this.richTextBox2.Text += "	}" + System.Environment.NewLine;
            this.richTextBox2.Text += "}" + System.Environment.NewLine;
        }


        //Diferential Equations System (ODE) Sample1 Librería TrentGuidry
        private void button10_Click(object sender, EventArgs e)
        {
            this.rtfCode.Clear();
            this.richTextBox1.Clear();
            this.richTextBox2.Clear();

            //Llamamos a la opción del Menú de Nuevo Cálculo
            puntero.toolStripMenuItem11_Click(sender, e);

            this.rtfCode.Text += "using System.Collections.Generic;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.ComponentModel;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Data;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Collections;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Xml;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.IO;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Windows.Forms;" + System.Environment.NewLine;
            this.rtfCode.Text += "using Drag_AND_Drop_between_Forms;" + System.Environment.NewLine;
            this.rtfCode.Text += "using ClaseEquipos;" + System.Environment.NewLine;
            this.rtfCode.Text += System.Environment.NewLine;
            this.rtfCode.Text += "using NumericalMethods;" + System.Environment.NewLine;
            this.rtfCode.Text += "using NumericalMethods.FourthBlog;" + System.Environment.NewLine;
           

            this.rtfCode.Text += System.Environment.NewLine;
            this.rtfCode.Text += "namespace CSharpScripter1" + System.Environment.NewLine;
            this.rtfCode.Text += "{" + System.Environment.NewLine;

            this.rtfCode.Text += "	public class TestClass : CSharpScripter1.Command1" + System.Environment.NewLine;
            this.rtfCode.Text += "	{" + System.Environment.NewLine;

            this.rtfCode.Text += "Aplicacion puntero4;" + System.Environment.NewLine;
         
            this.rtfCode.Text += "		public TestClass()" + System.Environment.NewLine;
            this.rtfCode.Text += "		{" + System.Environment.NewLine;
            this.rtfCode.Text += "		}" + System.Environment.NewLine;
            this.rtfCode.Text += System.Environment.NewLine;

            this.rtfCode.Text += "		public void Execute1(Aplicacion puntero3) " + System.Environment.NewLine;
            this.rtfCode.Text += "		{" + System.Environment.NewLine;
            this.rtfCode.Text += "        puntero4=puntero3;" + System.Environment.NewLine;

            //CREAMOS EL ARRAY DE PARAMETROS
            this.rtfCode.Text += " for (int v = 0; v < 3; v++)" + System.Environment.NewLine;
            this.rtfCode.Text += "{" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p.Add(puntero4.ptemp);" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p[v] = new Parameter(1,0.01," + comillas + comillas + ");" + System.Environment.NewLine;
            this.rtfCode.Text += "}" + System.Environment.NewLine;

            this.richTextBox1.Text += System.Environment.NewLine;

            //Asignamos las Condiciones Iniciales a las dos variables, en este caso Xi....Xn
            this.rtfCode.Text += "puntero4.p[0].Nombre = " + comillas + "tiempo" + comillas + ";" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p[1].Nombre = " + comillas + "X1" + comillas + ";" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p[2].Nombre = " + comillas + "X2" + comillas + ";" + System.Environment.NewLine;

            this.richTextBox1.Text += System.Environment.NewLine;

            this.rtfCode.Text += "Parameter tiempo= new Parameter();" + System.Environment.NewLine;
            this.rtfCode.Text += "tiempo=puntero4.p.Find(p =>p.Nombre ==" + comillas + "tiempo" + comillas + ");" + System.Environment.NewLine;
            this.rtfCode.Text += "Parameter X1= new Parameter();" + System.Environment.NewLine;
            this.rtfCode.Text += "X1=puntero4.p.Find(p =>p.Nombre ==" + comillas + "X1" + comillas + ");" + System.Environment.NewLine;
            this.rtfCode.Text += "Parameter X2= new Parameter();" + System.Environment.NewLine;
            this.rtfCode.Text += "X2=puntero4.p.Find(p =>p.Nombre ==" + comillas + "X2" + comillas + ");" + System.Environment.NewLine;

            this.richTextBox1.Text += "//EJEMPLO VALIDACIÓN RESOLUCIÓN SISTEMA ODEs CON LIBRERÍA TRENT-GUIDRY" + System.Environment.NewLine;
            this.richTextBox1.Text += "//DEFINICIÓN DE LAS ECUACIONES DIFERENCIALES EXPLÍCITAS	" + System.Environment.NewLine;
            this.richTextBox1.Text += "// x1’ = x1 – 2x2   x1(0) = 0" + System.Environment.NewLine;
            this.richTextBox1.Text += "// x2’ = 2x1 + x2   x2(0) = 4" + System.Environment.NewLine;

            this.richTextBox1.Text += System.Environment.NewLine;

            this.richTextBox1.Text += "puntero4.p[0].Value = 0.0;" + System.Environment.NewLine;
            this.richTextBox1.Text += "puntero4.p[1].Value = 0.0;" + System.Environment.NewLine;
            this.richTextBox1.Text += "puntero4.p[2].Value = 4.0;" + System.Environment.NewLine;

            this.richTextBox1.Text += System.Environment.NewLine;

            this.richTextBox1.Text += "Func <Double> Ecuacion1 = () => X1-2.0*X2;"+ System.Environment.NewLine;
            this.richTextBox1.Text += "Func <Double> Ecuacion2 = () => 2.0*X1+X2;" + System.Environment.NewLine;

            this.richTextBox1.Text += System.Environment.NewLine;

            this.richTextBox2.Text += "puntero4.functions.Add(Ecuacion1);" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.functions.Add(Ecuacion2);" + System.Environment.NewLine;

            this.richTextBox2.Text += System.Environment.NewLine;

            this.richTextBox2.Text += "puntero4.ejemplovalidacion = 1;" + System.Environment.NewLine;
            this.richTextBox2.Text += "//Análisis Tipo Transitorio (tipoanalisis=1)" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.tipoanalisis=1;" + System.Environment.NewLine;
            this.richTextBox2.Text += "//Análisis Transitorio con Librerías TrentGuidry (tipoanalisistransitorio=0)" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.tipoanalisistransitorio=0;" + System.Environment.NewLine;

            this.richTextBox2.Text += System.Environment.NewLine;
            this.richTextBox2.Text += "  }" + System.Environment.NewLine;
            this.richTextBox2.Text += "	}" + System.Environment.NewLine;
            this.richTextBox2.Text += "}" + System.Environment.NewLine;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
             //Al pulsar el Tab 2 Sistema de Ecuaciones Lineales
            if (e.TabPage == tabPage2)
            {
                radioButton4.Checked = false;
                radioButton1.Checked = true;
                radioButton2.Checked = false;
                radioButton3.Checked = false;
            }

            //Al pulsar el Tab 2 Sistema de Ecuaciones No Lineales
            else if (e.TabPage == tabPage3)
            {
                radioButton4.Checked = false;
                radioButton1.Checked = false;
                radioButton2.Checked = true;
                radioButton3.Checked = false;
            }

            //Al pulsar el Tab 4 Sistemas Diferencial Equations
            else if (e.TabPage == tabPage4)
            {
                radioButton4.Checked = false;
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                radioButton3.Checked = true;
            }

            //Al pulsar el Tab 1 Ecuaciones Interpolación
            else if (e.TabPage == tabPage1)
            {
                radioButton4.Checked = true;
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                radioButton3.Checked = false;
            }

        }

        //Diferential Equations System (ODE) Sample2 Librería DotNumerics
        private void button11_Click(object sender, EventArgs e)
        {
            this.rtfCode.Clear();
            this.richTextBox1.Clear();
            this.richTextBox2.Clear();

            //Llamamos a la opción del Menú de Nuevo Cálculo
            puntero.toolStripMenuItem11_Click(sender, e);

            this.rtfCode.Text += "using System.Collections.Generic;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.ComponentModel;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Data;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Collections;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Xml;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.IO;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Windows.Forms;" + System.Environment.NewLine;
            this.rtfCode.Text += "using Drag_AND_Drop_between_Forms;" + System.Environment.NewLine;
            this.rtfCode.Text += "using ClaseEquipos;" + System.Environment.NewLine;

            this.rtfCode.Text += "using NumericalMethods;" + System.Environment.NewLine;
            this.rtfCode.Text += "using NumericalMethods.FourthBlog;" + System.Environment.NewLine;

            this.rtfCode.Text += System.Environment.NewLine;
            this.rtfCode.Text += "namespace CSharpScripter2" + System.Environment.NewLine;
            this.rtfCode.Text += "{" + System.Environment.NewLine;

            this.rtfCode.Text += "	public class TestClass : CSharpScripter2.Command2" + System.Environment.NewLine;
            this.rtfCode.Text += "	{" + System.Environment.NewLine;

            this.rtfCode.Text += "Aplicacion puntero4;" + System.Environment.NewLine;
            this.rtfCode.Text += "//Declaración del array de double donde guardaremos los ecuaciones diferenciales explícitas" + System.Environment.NewLine;
            this.rtfCode.Text += "double[] yprime = new double[3];" + System.Environment.NewLine;

            this.rtfCode.Text += "		public TestClass()" + System.Environment.NewLine;
            this.rtfCode.Text += "		{" + System.Environment.NewLine;
            this.rtfCode.Text += "		}" + System.Environment.NewLine;
            this.rtfCode.Text += System.Environment.NewLine;

            this.rtfCode.Text += "		public void Execute2(Aplicacion puntero3) " + System.Environment.NewLine;
            this.rtfCode.Text += "		{" + System.Environment.NewLine;
            this.rtfCode.Text += "       puntero4=puntero3;" + System.Environment.NewLine;

            //CREAMOS EL ARRAY DE PARAMETROS
            this.rtfCode.Text += " for (int v = 0; v < 4; v++)" + System.Environment.NewLine;
            this.rtfCode.Text += "{" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p.Add(puntero4.ptemp);" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p[v] = new Parameter(1,0.01," + comillas + comillas + ");" + System.Environment.NewLine;
            this.rtfCode.Text += "}" + System.Environment.NewLine;

            this.richTextBox1.Text += System.Environment.NewLine;

            //Asignamos las Condiciones Iniciales a las dos variables, en este caso Xi....Xn
            this.rtfCode.Text += "puntero4.p[0].Nombre = " + comillas + "tiempo" + comillas + ";" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p[1].Nombre = " + comillas + "X1" + comillas + ";" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p[2].Nombre = " + comillas + "X2" + comillas + ";" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p[3].Nombre = " + comillas + "X3" + comillas + ";" + System.Environment.NewLine;

            this.richTextBox1.Text += System.Environment.NewLine;

            this.rtfCode.Text += "Parameter tiempo= new Parameter();" + System.Environment.NewLine;
            this.rtfCode.Text += "tiempo=puntero4.p.Find(p =>p.Nombre ==" + comillas + "tiempo" + comillas + ");" + System.Environment.NewLine;
            this.rtfCode.Text += "Parameter X1= new Parameter();" + System.Environment.NewLine;
            this.rtfCode.Text += "X1=puntero4.p.Find(p =>p.Nombre ==" + comillas + "X1" + comillas + ");" + System.Environment.NewLine;
            this.rtfCode.Text += "Parameter X2= new Parameter();" + System.Environment.NewLine;
            this.rtfCode.Text += "X2=puntero4.p.Find(p =>p.Nombre ==" + comillas + "X2" + comillas + ");" + System.Environment.NewLine;
            this.rtfCode.Text += "Parameter X3= new Parameter();" + System.Environment.NewLine;
            this.rtfCode.Text += "X3=puntero4.p.Find(p =>p.Nombre ==" + comillas + "X3" + comillas + ");" + System.Environment.NewLine;

            this.richTextBox1.Text += System.Environment.NewLine;

            this.richTextBox1.Text += "puntero4.p[0].Value = 1.0;" + System.Environment.NewLine;
            this.richTextBox1.Text += "puntero4.p[1].Value = 1.0;" + System.Environment.NewLine;
            this.richTextBox1.Text += "puntero4.p[2].Value = 1.0;" + System.Environment.NewLine;
            this.richTextBox1.Text += "puntero4.p[3].Value = 1.0;" + System.Environment.NewLine;

            this.richTextBox1.Text += System.Environment.NewLine;

            this.richTextBox2.Text += "puntero4.ejemplovalidacion = 1;" + System.Environment.NewLine;
            this.richTextBox2.Text += "//Análisis Tipo Transitorio (tipoanalisis=1)" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.tipoanalisis=1;" + System.Environment.NewLine;
            this.richTextBox2.Text += "//Análisis Transitorio con Librerías DotNumerics (tipoanalisistransitorio=1)" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.tipoanalisistransitorio=1;" + System.Environment.NewLine;
            
            this.richTextBox2.Text += System.Environment.NewLine;

            this.richTextBox2.Text += " }" + System.Environment.NewLine;

            this.richTextBox2.Text += "		public  double[] ODEs(double t, double[] y) " + System.Environment.NewLine;

            this.richTextBox2.Text += "{" + System.Environment.NewLine;
            this.richTextBox2.Text += "     yprime[0] = y[1] * y[2];" + System.Environment.NewLine;
            this.richTextBox2.Text += "     yprime[1] = -y[0] * y[2];" + System.Environment.NewLine;
            this.richTextBox2.Text += "     yprime[2] = -0.51 * y[0] * y[1];" + System.Environment.NewLine;

            this.richTextBox2.Text += "   return yprime;" + System.Environment.NewLine;
            this.richTextBox2.Text += "}" + System.Environment.NewLine;

            this.richTextBox2.Text += "	}" + System.Environment.NewLine;
            this.richTextBox2.Text += "}" + System.Environment.NewLine;
        }


        //Diferential Equations System (ODE) Sample3 Librería DotNumerics
        private void button9_Click(object sender, EventArgs e)
        {
            this.rtfCode.Clear();
            this.richTextBox1.Clear();
            this.richTextBox2.Clear();

            //Llamamos a la opción del Menú de Nuevo Cálculo
            puntero.toolStripMenuItem11_Click(sender, e);

            this.rtfCode.Text += "using System.Collections.Generic;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.ComponentModel;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Data;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Collections;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Xml;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.IO;" + System.Environment.NewLine;
            this.rtfCode.Text += "using System.Windows.Forms;" + System.Environment.NewLine;
            this.rtfCode.Text += "using Drag_AND_Drop_between_Forms;" + System.Environment.NewLine;
            this.rtfCode.Text += "using ClaseEquipos;" + System.Environment.NewLine;

            this.rtfCode.Text += "using NumericalMethods;" + System.Environment.NewLine;
            this.rtfCode.Text += "using NumericalMethods.FourthBlog;" + System.Environment.NewLine;

            this.rtfCode.Text += System.Environment.NewLine;
            this.rtfCode.Text += "namespace CSharpScripter2" + System.Environment.NewLine;
            this.rtfCode.Text += "{" + System.Environment.NewLine;

            this.rtfCode.Text += "	public class TestClass : CSharpScripter2.Command2" + System.Environment.NewLine;
            this.rtfCode.Text += "	{" + System.Environment.NewLine;

            this.rtfCode.Text += "Aplicacion puntero4;" + System.Environment.NewLine;
            this.rtfCode.Text += "//Declaración del array de double donde guardaremos los ecuaciones diferenciales explícitas" + System.Environment.NewLine;
            this.rtfCode.Text += "double[] yprime = new double[2];" + System.Environment.NewLine;
            this.rtfCode.Text += "double rpar = 1.0E-6;" + System.Environment.NewLine;

            this.rtfCode.Text += "		public TestClass()" + System.Environment.NewLine;
            this.rtfCode.Text += "		{" + System.Environment.NewLine;
            this.rtfCode.Text += "		}" + System.Environment.NewLine;
            this.rtfCode.Text += System.Environment.NewLine;

            this.rtfCode.Text += "		public void Execute2(Aplicacion puntero3) " + System.Environment.NewLine;
            this.rtfCode.Text += "		{" + System.Environment.NewLine;
            this.rtfCode.Text += "       puntero4=puntero3;" + System.Environment.NewLine;

            //CREAMOS EL ARRAY DE PARAMETROS
            this.rtfCode.Text += " for (int v = 0; v < 3; v++)" + System.Environment.NewLine;
            this.rtfCode.Text += "{" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p.Add(puntero4.ptemp);" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p[v] = new Parameter(1,0.01," + comillas + comillas + ");" + System.Environment.NewLine;
            this.rtfCode.Text += "}" + System.Environment.NewLine;

            this.richTextBox1.Text += System.Environment.NewLine;

            //Asignamos las Condiciones Iniciales a las dos variables, en este caso Xi....Xn
            this.rtfCode.Text += "puntero4.p[0].Nombre = " + comillas + "tiempo" + comillas + ";" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p[1].Nombre = " + comillas + "X1" + comillas + ";" + System.Environment.NewLine;
            this.rtfCode.Text += "puntero4.p[2].Nombre = " + comillas + "X2" + comillas + ";" + System.Environment.NewLine;

            this.richTextBox1.Text += System.Environment.NewLine;

            this.rtfCode.Text += "Parameter tiempo= new Parameter();" + System.Environment.NewLine;
            this.rtfCode.Text += "tiempo=puntero4.p.Find(p =>p.Nombre ==" + comillas + "tiempo" + comillas + ");" + System.Environment.NewLine;
            this.rtfCode.Text += "Parameter X1= new Parameter();" + System.Environment.NewLine;
            this.rtfCode.Text += "X1=puntero4.p.Find(p =>p.Nombre ==" + comillas + "X1" + comillas + ");" + System.Environment.NewLine;
            this.rtfCode.Text += "Parameter X2= new Parameter();" + System.Environment.NewLine;
            this.rtfCode.Text += "X2=puntero4.p.Find(p =>p.Nombre ==" + comillas + "X2" + comillas + ");" + System.Environment.NewLine;
           
            this.richTextBox1.Text += System.Environment.NewLine;

            this.richTextBox1.Text += "puntero4.p[0].Value = 0.0;" + System.Environment.NewLine;
            this.richTextBox1.Text += "puntero4.p[1].Value = 2.0E0;" + System.Environment.NewLine;
            this.richTextBox1.Text += "puntero4.p[2].Value = -0.66;" + System.Environment.NewLine;

            this.richTextBox1.Text += System.Environment.NewLine;

            this.richTextBox2.Text += "puntero4.ejemplovalidacion = 1;" + System.Environment.NewLine;
            this.richTextBox2.Text += "//Análisis Tipo Transitorio (tipoanalisis=1)" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.tipoanalisis=1;" + System.Environment.NewLine;
            this.richTextBox2.Text += "//Análisis Transitorio con Librerías DotNumerics (tipoanalisistransitorio=1)" + System.Environment.NewLine;
            this.richTextBox2.Text += "puntero4.tipoanalisistransitorio=1;" + System.Environment.NewLine;

            this.richTextBox2.Text += System.Environment.NewLine;

            this.richTextBox2.Text += " }" + System.Environment.NewLine;

            this.richTextBox2.Text += "		public  double[] ODEs(double t, double[] y) " + System.Environment.NewLine;

            this.richTextBox2.Text += "{" + System.Environment.NewLine;
            this.richTextBox2.Text += "     yprime[0] = y[1];" + System.Environment.NewLine;
            this.richTextBox2.Text += "     yprime[1] = ((1 - Math.Pow(y[0], 2)) * y[1] - y[0]) / rpar;" + System.Environment.NewLine;

            this.richTextBox2.Text += "   return yprime;" + System.Environment.NewLine;
            this.richTextBox2.Text += "}" + System.Environment.NewLine;

            this.richTextBox2.Text += "	}" + System.Environment.NewLine;
            this.richTextBox2.Text += "}" + System.Environment.NewLine;
        }
	}
}
