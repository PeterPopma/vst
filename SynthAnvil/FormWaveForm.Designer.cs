
namespace SynthAnvil
{
    partial class FormWaveForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBoxSine = new System.Windows.Forms.PictureBox();
            this.pictureBoxSquare = new System.Windows.Forms.PictureBox();
            this.pictureBoxSawtooth = new System.Windows.Forms.PictureBox();
            this.pictureBoxRandom = new System.Windows.Forms.PictureBox();
            this.pictureBoxTriangle = new System.Windows.Forms.PictureBox();
            this.pictureBoxCustom = new System.Windows.Forms.PictureBox();
            this.pictureBoxWav = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSquare)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSawtooth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRandom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTriangle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCustom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWav)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBoxSine
            // 
            this.pictureBoxSine.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxSine.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxSine.Image = global::SynthAnvil.Properties.Resources.sine;
            this.pictureBoxSine.InitialImage = null;
            this.pictureBoxSine.Location = new System.Drawing.Point(22, 21);
            this.pictureBoxSine.Name = "pictureBoxSine";
            this.pictureBoxSine.Size = new System.Drawing.Size(70, 70);
            this.pictureBoxSine.TabIndex = 92;
            this.pictureBoxSine.TabStop = false;
            this.pictureBoxSine.Click += new System.EventHandler(this.pictureBoxSine_Click);
            // 
            // pictureBoxSquare
            // 
            this.pictureBoxSquare.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxSquare.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxSquare.Image = global::SynthAnvil.Properties.Resources.square;
            this.pictureBoxSquare.InitialImage = null;
            this.pictureBoxSquare.Location = new System.Drawing.Point(129, 21);
            this.pictureBoxSquare.Name = "pictureBoxSquare";
            this.pictureBoxSquare.Size = new System.Drawing.Size(70, 70);
            this.pictureBoxSquare.TabIndex = 93;
            this.pictureBoxSquare.TabStop = false;
            this.pictureBoxSquare.Click += new System.EventHandler(this.pictureBoxSquare_Click);
            // 
            // pictureBoxSawtooth
            // 
            this.pictureBoxSawtooth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxSawtooth.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxSawtooth.Image = global::SynthAnvil.Properties.Resources.sawtooth;
            this.pictureBoxSawtooth.InitialImage = null;
            this.pictureBoxSawtooth.Location = new System.Drawing.Point(336, 21);
            this.pictureBoxSawtooth.Name = "pictureBoxSawtooth";
            this.pictureBoxSawtooth.Size = new System.Drawing.Size(70, 70);
            this.pictureBoxSawtooth.TabIndex = 94;
            this.pictureBoxSawtooth.TabStop = false;
            this.pictureBoxSawtooth.Click += new System.EventHandler(this.pictureBoxSawtooth_Click);
            // 
            // pictureBoxRandom
            // 
            this.pictureBoxRandom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxRandom.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxRandom.Image = global::SynthAnvil.Properties.Resources.random;
            this.pictureBoxRandom.InitialImage = null;
            this.pictureBoxRandom.Location = new System.Drawing.Point(27, 125);
            this.pictureBoxRandom.Name = "pictureBoxRandom";
            this.pictureBoxRandom.Size = new System.Drawing.Size(70, 70);
            this.pictureBoxRandom.TabIndex = 95;
            this.pictureBoxRandom.TabStop = false;
            this.pictureBoxRandom.Click += new System.EventHandler(this.pictureBoxRandom_Click);
            // 
            // pictureBoxTriangle
            // 
            this.pictureBoxTriangle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxTriangle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxTriangle.Image = global::SynthAnvil.Properties.Resources.triangle;
            this.pictureBoxTriangle.InitialImage = null;
            this.pictureBoxTriangle.Location = new System.Drawing.Point(234, 21);
            this.pictureBoxTriangle.Name = "pictureBoxTriangle";
            this.pictureBoxTriangle.Size = new System.Drawing.Size(70, 70);
            this.pictureBoxTriangle.TabIndex = 96;
            this.pictureBoxTriangle.TabStop = false;
            this.pictureBoxTriangle.Click += new System.EventHandler(this.pictureBoxTriangle_Click);
            // 
            // pictureBoxCustom
            // 
            this.pictureBoxCustom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxCustom.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxCustom.Image = global::SynthAnvil.Properties.Resources.custom;
            this.pictureBoxCustom.InitialImage = null;
            this.pictureBoxCustom.Location = new System.Drawing.Point(134, 125);
            this.pictureBoxCustom.Name = "pictureBoxCustom";
            this.pictureBoxCustom.Size = new System.Drawing.Size(70, 70);
            this.pictureBoxCustom.TabIndex = 97;
            this.pictureBoxCustom.TabStop = false;
            this.pictureBoxCustom.Click += new System.EventHandler(this.pictureBoxCustom_Click);
            // 
            // pictureBoxWav
            // 
            this.pictureBoxWav.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxWav.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxWav.Image = global::SynthAnvil.Properties.Resources.wav;
            this.pictureBoxWav.InitialImage = null;
            this.pictureBoxWav.Location = new System.Drawing.Point(239, 125);
            this.pictureBoxWav.Name = "pictureBoxWav";
            this.pictureBoxWav.Size = new System.Drawing.Size(70, 70);
            this.pictureBoxWav.TabIndex = 98;
            this.pictureBoxWav.TabStop = false;
            this.pictureBoxWav.Click += new System.EventHandler(this.pictureBoxWav_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.pictureBoxTriangle);
            this.panel1.Controls.Add(this.pictureBoxSquare);
            this.panel1.Controls.Add(this.pictureBoxSine);
            this.panel1.Controls.Add(this.pictureBoxSawtooth);
            this.panel1.ForeColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(428, 210);
            this.panel1.TabIndex = 99;
            // 
            // FormWaveForm
            // 
            this.ClientSize = new System.Drawing.Size(436, 222);
            this.Controls.Add(this.pictureBoxWav);
            this.Controls.Add(this.pictureBoxCustom);
            this.Controls.Add(this.pictureBoxRandom);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormWaveForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSquare)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSawtooth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRandom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTriangle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCustom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWav)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxSine;
        private System.Windows.Forms.PictureBox pictureBoxSquare;
        private System.Windows.Forms.PictureBox pictureBoxSawtooth;
        private System.Windows.Forms.PictureBox pictureBoxRandom;
        private System.Windows.Forms.PictureBox pictureBoxTriangle;
        private System.Windows.Forms.PictureBox pictureBoxCustom;
        private System.Windows.Forms.PictureBox pictureBoxWav;
        private System.Windows.Forms.Panel panel1;
    }
}
