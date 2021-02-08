
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
            this.buttonRandomRipples = new SynthAnvil.CustomControls.GradientButton();
            this.buttonRipples = new SynthAnvil.CustomControls.GradientButton();
            this.buttonDecreasing = new SynthAnvil.CustomControls.GradientButton();
            this.buttonIncreasing = new SynthAnvil.CustomControls.GradientButton();
            this.buttonFlat = new SynthAnvil.CustomControls.GradientButton();
            this.buttonSine = new SynthAnvil.CustomControls.GradientButton();
            this.buttonCancel = new SynthAnvil.CustomControls.GradientButton();
            this.buttonApply = new SynthAnvil.CustomControls.GradientButton();
            this.buttonFlatLow = new SynthAnvil.CustomControls.GradientButton();
            this.buttonFlatHigh = new SynthAnvil.CustomControls.GradientButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCustomWave)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxCustomWave
            // 
            this.pictureBoxCustomWave.Location = new System.Drawing.Point(105, 7);
            this.pictureBoxCustomWave.Name = "pictureBoxCustomWave";
            this.pictureBoxCustomWave.Size = new System.Drawing.Size(1000, 654);
            this.pictureBoxCustomWave.TabIndex = 0;
            this.pictureBoxCustomWave.TabStop = false;
            this.pictureBoxCustomWave.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxCustomWave_MouseDown);
            this.pictureBoxCustomWave.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxCustomWave_MouseMove);
            this.pictureBoxCustomWave.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxCustomWave_MouseUp);
            // 
            // buttonRandomRipples
            // 
            this.buttonRandomRipples.Active = false;
            this.buttonRandomRipples.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonRandomRipples.FlatAppearance.BorderSize = 2;
            this.buttonRandomRipples.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRandomRipples.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRandomRipples.ForeColor = System.Drawing.Color.Black;
            this.buttonRandomRipples.HorizontalGradient = false;
            this.buttonRandomRipples.Location = new System.Drawing.Point(9, 354);
            this.buttonRandomRipples.Name = "buttonRandomRipples";
            this.buttonRandomRipples.Size = new System.Drawing.Size(87, 22);
            this.buttonRandomRipples.TabIndex = 109;
            this.buttonRandomRipples.Text = "Ripples";
            this.buttonRandomRipples.UseVisualStyleBackColor = true;
            this.buttonRandomRipples.Click += new System.EventHandler(this.buttonRandomRipples_Click);
            // 
            // buttonRipples
            // 
            this.buttonRipples.Active = false;
            this.buttonRipples.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonRipples.FlatAppearance.BorderSize = 2;
            this.buttonRipples.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRipples.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRipples.ForeColor = System.Drawing.Color.Black;
            this.buttonRipples.HorizontalGradient = false;
            this.buttonRipples.Location = new System.Drawing.Point(9, 311);
            this.buttonRipples.Name = "buttonRipples";
            this.buttonRipples.Size = new System.Drawing.Size(87, 22);
            this.buttonRipples.TabIndex = 108;
            this.buttonRipples.Text = "1 Size Ripples";
            this.buttonRipples.UseVisualStyleBackColor = true;
            this.buttonRipples.Click += new System.EventHandler(this.buttonRipples_Click);
            // 
            // buttonDecreasing
            // 
            this.buttonDecreasing.Active = false;
            this.buttonDecreasing.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonDecreasing.FlatAppearance.BorderSize = 2;
            this.buttonDecreasing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDecreasing.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDecreasing.ForeColor = System.Drawing.Color.Black;
            this.buttonDecreasing.HorizontalGradient = false;
            this.buttonDecreasing.Location = new System.Drawing.Point(9, 268);
            this.buttonDecreasing.Name = "buttonDecreasing";
            this.buttonDecreasing.Size = new System.Drawing.Size(87, 22);
            this.buttonDecreasing.TabIndex = 107;
            this.buttonDecreasing.Text = "Decreasing";
            this.buttonDecreasing.UseVisualStyleBackColor = true;
            this.buttonDecreasing.Click += new System.EventHandler(this.buttonDecreasing_Click);
            // 
            // buttonIncreasing
            // 
            this.buttonIncreasing.Active = false;
            this.buttonIncreasing.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonIncreasing.FlatAppearance.BorderSize = 2;
            this.buttonIncreasing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonIncreasing.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonIncreasing.ForeColor = System.Drawing.Color.Black;
            this.buttonIncreasing.HorizontalGradient = false;
            this.buttonIncreasing.Location = new System.Drawing.Point(9, 227);
            this.buttonIncreasing.Name = "buttonIncreasing";
            this.buttonIncreasing.Size = new System.Drawing.Size(87, 22);
            this.buttonIncreasing.TabIndex = 106;
            this.buttonIncreasing.Text = "Increasing";
            this.buttonIncreasing.UseVisualStyleBackColor = true;
            this.buttonIncreasing.Click += new System.EventHandler(this.buttonIncreasing_Click);
            // 
            // buttonFlat
            // 
            this.buttonFlat.Active = false;
            this.buttonFlat.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonFlat.FlatAppearance.BorderSize = 2;
            this.buttonFlat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFlat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonFlat.ForeColor = System.Drawing.Color.Black;
            this.buttonFlat.HorizontalGradient = false;
            this.buttonFlat.Location = new System.Drawing.Point(9, 111);
            this.buttonFlat.Name = "buttonFlat";
            this.buttonFlat.Size = new System.Drawing.Size(87, 22);
            this.buttonFlat.TabIndex = 105;
            this.buttonFlat.Text = "Flat";
            this.buttonFlat.UseVisualStyleBackColor = true;
            this.buttonFlat.Click += new System.EventHandler(this.buttonFlat_Click);
            // 
            // buttonSine
            // 
            this.buttonSine.Active = false;
            this.buttonSine.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonSine.FlatAppearance.BorderSize = 2;
            this.buttonSine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSine.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSine.ForeColor = System.Drawing.Color.Black;
            this.buttonSine.HorizontalGradient = false;
            this.buttonSine.Location = new System.Drawing.Point(9, 72);
            this.buttonSine.Name = "buttonSine";
            this.buttonSine.Size = new System.Drawing.Size(87, 22);
            this.buttonSine.TabIndex = 104;
            this.buttonSine.Text = "Sine";
            this.buttonSine.UseVisualStyleBackColor = true;
            this.buttonSine.Click += new System.EventHandler(this.buttonSine_Click);
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
            this.buttonCancel.Location = new System.Drawing.Point(640, 678);
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
            this.buttonApply.Location = new System.Drawing.Point(415, 678);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(87, 22);
            this.buttonApply.TabIndex = 102;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // buttonFlatLow
            // 
            this.buttonFlatLow.Active = false;
            this.buttonFlatLow.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonFlatLow.FlatAppearance.BorderSize = 2;
            this.buttonFlatLow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFlatLow.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonFlatLow.ForeColor = System.Drawing.Color.Black;
            this.buttonFlatLow.HorizontalGradient = false;
            this.buttonFlatLow.Location = new System.Drawing.Point(9, 149);
            this.buttonFlatLow.Name = "buttonFlatLow";
            this.buttonFlatLow.Size = new System.Drawing.Size(87, 22);
            this.buttonFlatLow.TabIndex = 110;
            this.buttonFlatLow.Text = "Flat low";
            this.buttonFlatLow.UseVisualStyleBackColor = true;
            this.buttonFlatLow.Click += new System.EventHandler(this.buttonFlatLow_Click);
            // 
            // buttonFlatHigh
            // 
            this.buttonFlatHigh.Active = false;
            this.buttonFlatHigh.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonFlatHigh.FlatAppearance.BorderSize = 2;
            this.buttonFlatHigh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFlatHigh.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonFlatHigh.ForeColor = System.Drawing.Color.Black;
            this.buttonFlatHigh.HorizontalGradient = false;
            this.buttonFlatHigh.Location = new System.Drawing.Point(9, 188);
            this.buttonFlatHigh.Name = "buttonFlatHigh";
            this.buttonFlatHigh.Size = new System.Drawing.Size(87, 22);
            this.buttonFlatHigh.TabIndex = 111;
            this.buttonFlatHigh.Text = "Flat high";
            this.buttonFlatHigh.UseVisualStyleBackColor = true;
            this.buttonFlatHigh.Click += new System.EventHandler(this.buttonFlatHigh_Click);
            // 
            // FormCustomShape
            // 
            this.ClientSize = new System.Drawing.Size(1111, 715);
            this.Controls.Add(this.buttonFlatHigh);
            this.Controls.Add(this.buttonFlatLow);
            this.Controls.Add(this.buttonRandomRipples);
            this.Controls.Add(this.buttonRipples);
            this.Controls.Add(this.buttonDecreasing);
            this.Controls.Add(this.buttonIncreasing);
            this.Controls.Add(this.buttonFlat);
            this.Controls.Add(this.buttonSine);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.pictureBoxCustomWave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCustomShape";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.FormCustomWave_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCustomWave)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxCustomWave;
        private CustomControls.GradientButton buttonApply;
        private CustomControls.GradientButton buttonCancel;
        private CustomControls.GradientButton buttonSine;
        private CustomControls.GradientButton buttonFlat;
        private CustomControls.GradientButton buttonIncreasing;
        private CustomControls.GradientButton buttonDecreasing;
        private CustomControls.GradientButton buttonRipples;
        private CustomControls.GradientButton buttonRandomRipples;
        private CustomControls.GradientButton buttonFlatLow;
        private CustomControls.GradientButton buttonFlatHigh;
    }
}
