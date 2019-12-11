using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Dalssoft.TestForm
{
	/// <summary>
	/// Summary description for About.
	/// </summary>
	public class About : System.Windows.Forms.Form
	{
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Label lblVersion;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.LinkLabel linkLabel2;
		private System.ComponentModel.IContainer components;

		public About()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(About));
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.lblVersion = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.linkLabel2 = new System.Windows.Forms.LinkLabel();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(232, 8);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(80, 80);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
			// 
			// linkLabel1
			// 
			this.linkLabel1.Location = new System.Drawing.Point(16, 72);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(176, 16);
			this.linkLabel1.TabIndex = 1;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "Diagram.NET Official Website";
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// lblVersion
			// 
			this.lblVersion.Location = new System.Drawing.Point(16, 56);
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.Size = new System.Drawing.Size(160, 16);
			this.lblVersion.TabIndex = 3;
			this.lblVersion.Click += new System.EventHandler(this.lblVersion_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(160, 16);
			this.label2.TabIndex = 4;
			this.label2.Text = "Under Shared Source Licence.";
			// 
			// button1
			// 
			this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1.Location = new System.Drawing.Point(232, 200);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(80, 24);
			this.button1.TabIndex = 5;
			this.button1.Text = "&Ok";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(16, 8);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(160, 24);
			this.label3.TabIndex = 6;
			this.label3.Text = "Diagram.NET";
			// 
			// textBox1
			// 
			this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox1.Location = new System.Drawing.Point(16, 120);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.Size = new System.Drawing.Size(296, 72);
			this.textBox1.TabIndex = 7;
			this.textBox1.Text = "Contributors:\r\n\r\nDavid Lojudice Sobrinho\t\tOwner/Programmer\r\nDaniel Jos� Miguel Ro" +
				"drigues\t\tTester (0.3x / 0.4)";
			// 
			// linkLabel2
			// 
			this.linkLabel2.Location = new System.Drawing.Point(16, 88);
			this.linkLabel2.Name = "linkLabel2";
			this.linkLabel2.Size = new System.Drawing.Size(176, 16);
			this.linkLabel2.TabIndex = 8;
			this.linkLabel2.TabStop = true;
			this.linkLabel2.Text = "Diagram.NET GotDotNet Website";
			this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
			// 
			// About
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(322, 232);
			this.Controls.Add(this.linkLabel2);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.lblVersion);
			this.Controls.Add(this.linkLabel1);
			this.Controls.Add(this.pictureBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "About";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "About Diagram.NET";
			this.Load += new System.EventHandler(this.About_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void VisitLink()
		{
			try
			{
				// Change the color of the link text by setting LinkVisited 
				// to true.
				linkLabel1.LinkVisited = true;
				//Call the Process.Start method to open the default browser 
				//with a URL:
				System.Diagnostics.Process.Start("http://www.dalssoft.com/diagram");
			}
			catch
			{
				MessageBox.Show("Unable to open link that was clicked.");			
			}
		}

		private void VisitLinkGotDotNet()
		{
			try
			{
				// Change the color of the link text by setting LinkVisited 
				// to true.
				linkLabel1.LinkVisited = true;
				//Call the Process.Start method to open the default browser 
				//with a URL:
				System.Diagnostics.Process.Start("http://workspaces.gotdotnet.com/diagram");
			}
			catch
			{
				MessageBox.Show("Unable to open link that was clicked.");			
			}
		}

		private void About_Load(object sender, System.EventArgs e)
		{
			
			try
			{
				System.Reflection.Assembly diagramDLL = typeof(DiagramNet.Designer).Assembly;
				Version currentVersion = diagramDLL.GetName().Version;
				string codeBase = diagramDLL.CodeBase;
				this.toolTip1.SetToolTip(this.lblVersion, "Path: " + codeBase);
				lblVersion.Text = "Version " + currentVersion.ToString();
			}
			catch
			{
				lblVersion.Text = "Error: Unable to find DLL version.";
			}
		}

		private void pictureBox1_Click(object sender, System.EventArgs e)
		{
			VisitLink();
		}

		private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			VisitLink();
		}

		private void lblVersion_Click(object sender, System.EventArgs e)
		{
		
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void linkLabel2_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			VisitLinkGotDotNet();
		}



	}
}
