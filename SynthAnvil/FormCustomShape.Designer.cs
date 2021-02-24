
namespace SynthAnvil
{
    partial class FormCustomShape
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
            this.pictureBoxCustomWave = new System.Windows.Forms.PictureBox();
            this.pictureBoxSine = new System.Windows.Forms.PictureBox();
            this.pictureBoxSquare = new System.Windows.Forms.PictureBox();
            this.pictureBoxTriangle = new System.Windows.Forms.PictureBox();
            this.pictureBoxSawtooth = new System.Windows.Forms.PictureBox();
            this.buttonCancel = new SynthAnvil.CustomControls.GradientButton();
            this.buttonApply = new SynthAnvil.CustomControls.GradientButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCustomWave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSquare)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTriangle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSawtooth)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxCustomWave
            // 
            this.pictureBoxCustomWave.Location = new System.Drawing.Point(96, 3);
            this.pictureBoxCustomWave.Name = "pictureBoxCustomWave";
            this.pictureBoxCustomWave.Size = new System.Drawing.Size(1000, 500);
            this.pictureBoxCustomWave.TabIndex = 0;
            this.pictureBoxCustomWave.TabStop = false;
            this.pictureBoxCustomWave.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxCustomWave_MouseDown);
            this.pictureBoxCustomWave.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxCustomWave_MouseMove);
            this.pictureBoxCustomWave.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxCustomWave_MouseUp);
            // 
            // pictureBoxSine
            // 
            this.pictureBoxSine.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxSine.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxSine.Image = global::SynthAnvil.Properties.Resources.sine;
            this.pictureBoxSine.InitialImage = null;
            this.pictureBoxSine.Location = new System.Drawing.Point(12, 24);
            this.pictureBoxSine.Name = "pictureBoxSine";
            this.pictureBoxSine.Size = new System.Drawing.Size(70, 70);
            this.pictureBoxSine.TabIndex = 201;
            this.pictureBoxSine.TabStop = false;
            this.pictureBoxSine.Click += new System.EventHandler(this.pictureBoxSine_Click);
            // 
            // pictureBoxSquare
            // 
            this.pictureBoxSquare.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxSquare.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxSquare.Image = global::SynthAnvil.Properties.Resources.square;
            this.pictureBoxSquare.InitialImage = null;
            this.pictureBoxSquare.Location = new System.Drawing.Point(12, 199);
            this.pictureBoxSquare.Name = "pictureBoxSquare";
            this.pictureBoxSquare.Size = new System.Drawing.Size(70, 70);
            this.pictureBoxSquare.TabIndex = 202;
            this.pictureBoxSquare.TabStop = false;
            this.pictureBoxSquare.Click += new System.EventHandler(this.pictureBoxSquare_Click);
            // 
            // pictureBoxTriangle
            // 
            this.pictureBoxTriangle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxTriangle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxTriangle.Image = global::SynthAnvil.Properties.Resources.triangle;
            this.pictureBoxTriangle.InitialImage = null;
            this.pictureBoxTriangle.Location = new System.Drawing.Point(12, 113);
            this.pictureBoxTriangle.Name = "pictureBoxTriangle";
            this.pictureBoxTriangle.Size = new System.Drawing.Size(70, 70);
            this.pictureBoxTriangle.TabIndex = 203;
            this.pictureBoxTriangle.TabStop = false;
            this.pictureBoxTriangle.Click += new System.EventHandler(this.pictureBoxTriangle_Click);
            // 
            // pictureBoxSawtooth
            // 
            this.pictureBoxSawtooth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxSawtooth.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxSawtooth.Image = global::SynthAnvil.Properties.Resources.sawtooth;
            this.pictureBoxSawtooth.InitialImage = null;
            this.pictureBoxSawtooth.Location = new System.Drawing.Point(12, 284);
            this.pictureBoxSawtooth.Name = "pictureBoxSawtooth";
            this.pictureBoxSawtooth.Size = new System.Drawing.Size(70, 70);
            this.pictureBoxSawtooth.TabIndex = 204;
            this.pictureBoxSawtooth.TabStop = false;
            this.pictureBoxSawtooth.Click += new System.EventHandler(this.pictureBoxSawtooth_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Active = false;
            this.buttonCancel.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonCancel.FlatAppearance.BorderSize = 2;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCancel.ForeColor = System.Drawing.Color.Black;
            this.buttonCancel.HorizontalGradient = false;
            this.buttonCancel.Location = new System.Drawing.Point(616, 534);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(87, 22);
            this.buttonCancel.TabIndex = 103;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonApply
            // 
            this.buttonApply.Active = false;
            this.buttonApply.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonApply.FlatAppearance.BorderSize = 2;
            this.buttonApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonApply.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonApply.ForeColor = System.Drawing.Color.Black;
            this.buttonApply.HorizontalGradient = false;
            this.buttonApply.Location = new System.Drawing.Point(389, 534);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(87, 22);
            this.buttonApply.TabIndex = 102;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // FormCustomShape
            // 
            this.ClientSize = new System.Drawing.Size(1111, 583);
            this.Controls.Add(this.pictureBoxSawtooth);
            this.Controls.Add(this.pictureBoxTriangle);
            this.Controls.Add(this.pictureBoxSquare);
            this.Controls.Add(this.pictureBoxSine);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.pictureBoxCustomWave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCustomShape";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.FormCustomWave_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCustomWave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSquare)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTriangle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSawtooth)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxCustomWave;
        private CustomControls.GradientButton buttonApply;
        private CustomControls.GradientButton buttonCancel;
        private System.Windows.Forms.PictureBox pictureBoxSine;
        private System.Windows.Forms.PictureBox pictureBoxSquare;
        private System.Windows.Forms.PictureBox pictureBoxTriangle;
        private System.Windows.Forms.PictureBox pictureBoxSawtooth;
    }
}
