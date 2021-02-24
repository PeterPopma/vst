
namespace SynthAnvil
{
    partial class FormSettings
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
            this.buttonCancel = new SynthAnvil.CustomControls.GradientButton();
            this.buttonApply = new SynthAnvil.CustomControls.GradientButton();
            this.comboBoxSamplesPerSecond = new System.Windows.Forms.ComboBox();
            this.label48 = new System.Windows.Forms.Label();
            this.comboBoxBitsPerSample = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
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
            this.buttonCancel.Location = new System.Drawing.Point(428, 188);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(87, 22);
            this.buttonCancel.TabIndex = 162;
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
            this.buttonApply.Location = new System.Drawing.Point(203, 188);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(87, 22);
            this.buttonApply.TabIndex = 161;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // comboBoxSamplesPerSecond
            // 
            this.comboBoxSamplesPerSecond.BackColor = System.Drawing.Color.DimGray;
            this.comboBoxSamplesPerSecond.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSamplesPerSecond.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxSamplesPerSecond.ForeColor = System.Drawing.Color.White;
            this.comboBoxSamplesPerSecond.FormattingEnabled = true;
            this.comboBoxSamplesPerSecond.Items.AddRange(new object[] {
            "44100",
            "48000",
            "88200",
            "96000",
            "132300",
            "144000",
            "176400",
            "192000"});
            this.comboBoxSamplesPerSecond.Location = new System.Drawing.Point(327, 36);
            this.comboBoxSamplesPerSecond.Name = "comboBoxSamplesPerSecond";
            this.comboBoxSamplesPerSecond.Size = new System.Drawing.Size(158, 24);
            this.comboBoxSamplesPerSecond.TabIndex = 163;
            this.comboBoxSamplesPerSecond.TabStop = false;
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.BackColor = System.Drawing.Color.Transparent;
            this.label48.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label48.ForeColor = System.Drawing.Color.White;
            this.label48.Location = new System.Drawing.Point(183, 39);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(137, 17);
            this.label48.TabIndex = 164;
            this.label48.Text = "Samples per second";
            // 
            // comboBoxBitsPerSample
            // 
            this.comboBoxBitsPerSample.BackColor = System.Drawing.Color.DimGray;
            this.comboBoxBitsPerSample.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBitsPerSample.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxBitsPerSample.ForeColor = System.Drawing.Color.White;
            this.comboBoxBitsPerSample.FormattingEnabled = true;
            this.comboBoxBitsPerSample.Items.AddRange(new object[] {
            "16",
            "32"});
            this.comboBoxBitsPerSample.Location = new System.Drawing.Point(327, 79);
            this.comboBoxBitsPerSample.Name = "comboBoxBitsPerSample";
            this.comboBoxBitsPerSample.Size = new System.Drawing.Size(158, 24);
            this.comboBoxBitsPerSample.TabIndex = 165;
            this.comboBoxBitsPerSample.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(215, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 17);
            this.label1.TabIndex = 166;
            this.label1.Text = "Bits per sample";
            // 
            // FormSettings
            // 
            this.ClientSize = new System.Drawing.Size(754, 240);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxBitsPerSample);
            this.Controls.Add(this.label48);
            this.Controls.Add(this.comboBoxSamplesPerSecond);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonApply);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSettings";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.FormSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CustomControls.GradientButton buttonCancel;
        private CustomControls.GradientButton buttonApply;
        private System.Windows.Forms.ComboBox comboBoxSamplesPerSecond;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.ComboBox comboBoxBitsPerSample;
        private System.Windows.Forms.Label label1;
    }
}
