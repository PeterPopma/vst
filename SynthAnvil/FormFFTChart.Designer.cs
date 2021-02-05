
namespace SynthAnvil
{
    partial class FormFFTChart
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chartFFT = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.buttonNextFFTPeriod = new System.Windows.Forms.Button();
            this.buttonPreviousFFTPeriod = new System.Windows.Forms.Button();
            this.labelFFTPeriod = new System.Windows.Forms.Label();
            this.comboBoxFFTWindow = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelScale = new System.Windows.Forms.Label();
            this.buttonDecreaseFrequency = new System.Windows.Forms.Button();
            this.buttonIncreaseFrequency = new System.Windows.Forms.Button();
            this.labelFrequencyRange = new System.Windows.Forms.Label();
            this.buttonDecreaseFrequencyMajor = new System.Windows.Forms.Button();
            this.buttonIncreaseFrequencyMajor = new System.Windows.Forms.Button();
            this.buttonDecreaseMiddle = new System.Windows.Forms.Button();
            this.buttonIncreaseMiddle = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chartFFT)).BeginInit();
            this.SuspendLayout();
            // 
            // chartFFT
            // 
            this.chartFFT.BackColor = System.Drawing.Color.Transparent;
            this.chartFFT.BackSecondaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            chartArea1.AxisX.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea1.AxisX.LineColor = System.Drawing.Color.DarkGray;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.DarkGray;
            chartArea1.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea1.AxisX.ScaleBreakStyle.LineColor = System.Drawing.Color.White;
            chartArea1.AxisX.TitleForeColor = System.Drawing.Color.Maroon;
            chartArea1.AxisY.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.Gray;
            chartArea1.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea1.AxisY.MajorTickMark.LineColor = System.Drawing.Color.DimGray;
            chartArea1.AxisY.Maximum = 100D;
            chartArea1.AxisY.Minimum = 0D;
            chartArea1.AxisY.MinorGrid.LineColor = System.Drawing.Color.DarkGray;
            chartArea1.AxisY.MinorTickMark.LineColor = System.Drawing.Color.DarkGray;
            chartArea1.BackColor = System.Drawing.Color.Silver;
            chartArea1.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.LeftRight;
            chartArea1.BackSecondaryColor = System.Drawing.Color.White;
            chartArea1.Name = "ChartArea1";
            this.chartFFT.ChartAreas.Add(chartArea1);
            this.chartFFT.Cursor = System.Windows.Forms.Cursors.Default;
            this.chartFFT.Location = new System.Drawing.Point(31, 40);
            this.chartFFT.Name = "chartFFT";
            series1.BackSecondaryColor = System.Drawing.Color.Black;
            series1.ChartArea = "ChartArea1";
            series1.Color = System.Drawing.Color.Blue;
            series1.Name = "Series1";
            this.chartFFT.Series.Add(series1);
            this.chartFFT.Size = new System.Drawing.Size(1272, 748);
            this.chartFFT.TabIndex = 0;
            this.chartFFT.Text = "chartFFT";
            this.chartFFT.Click += new System.EventHandler(this.chartFFT_Click);
            // 
            // buttonNextFFTPeriod
            // 
            this.buttonNextFFTPeriod.Location = new System.Drawing.Point(726, 22);
            this.buttonNextFFTPeriod.Name = "buttonNextFFTPeriod";
            this.buttonNextFFTPeriod.Size = new System.Drawing.Size(102, 27);
            this.buttonNextFFTPeriod.TabIndex = 1;
            this.buttonNextFFTPeriod.Text = ">>";
            this.buttonNextFFTPeriod.UseVisualStyleBackColor = true;
            this.buttonNextFFTPeriod.Click += new System.EventHandler(this.buttonNextFFTPeriod_Click);
            // 
            // buttonPreviousFFTPeriod
            // 
            this.buttonPreviousFFTPeriod.Location = new System.Drawing.Point(422, 22);
            this.buttonPreviousFFTPeriod.Name = "buttonPreviousFFTPeriod";
            this.buttonPreviousFFTPeriod.Size = new System.Drawing.Size(102, 27);
            this.buttonPreviousFFTPeriod.TabIndex = 2;
            this.buttonPreviousFFTPeriod.Text = "<<";
            this.buttonPreviousFFTPeriod.UseVisualStyleBackColor = true;
            this.buttonPreviousFFTPeriod.Click += new System.EventHandler(this.buttonPreviousFFTPeriod_Click);
            // 
            // labelFFTPeriod
            // 
            this.labelFFTPeriod.AutoSize = true;
            this.labelFFTPeriod.BackColor = System.Drawing.Color.Transparent;
            this.labelFFTPeriod.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelFFTPeriod.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFFTPeriod.ForeColor = System.Drawing.Color.White;
            this.labelFFTPeriod.Location = new System.Drawing.Point(572, 25);
            this.labelFFTPeriod.Name = "labelFFTPeriod";
            this.labelFFTPeriod.Size = new System.Drawing.Size(51, 20);
            this.labelFFTPeriod.TabIndex = 3;
            this.labelFFTPeriod.Text = "label1";
            // 
            // comboBoxFFTWindow
            // 
            this.comboBoxFFTWindow.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFFTWindow.FormattingEnabled = true;
            this.comboBoxFFTWindow.Items.AddRange(new object[] {
            "512",
            "1024",
            "2048",
            "4096",
            "8192",
            "16384",
            "32768",
            "65536",
            "131072"});
            this.comboBoxFFTWindow.Location = new System.Drawing.Point(1007, 28);
            this.comboBoxFFTWindow.Name = "comboBoxFFTWindow";
            this.comboBoxFFTWindow.Size = new System.Drawing.Size(135, 21);
            this.comboBoxFFTWindow.TabIndex = 4;
            this.comboBoxFFTWindow.SelectedIndexChanged += new System.EventHandler(this.comboBoxFFTWindow_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(935, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "# Samples";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(265, 764);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Scale: 1:";
            // 
            // labelScale
            // 
            this.labelScale.AutoSize = true;
            this.labelScale.BackColor = System.Drawing.Color.Transparent;
            this.labelScale.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelScale.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelScale.ForeColor = System.Drawing.Color.White;
            this.labelScale.Location = new System.Drawing.Point(329, 764);
            this.labelScale.Name = "labelScale";
            this.labelScale.Size = new System.Drawing.Size(18, 20);
            this.labelScale.TabIndex = 7;
            this.labelScale.Text = "1";
            // 
            // buttonDecreaseFrequency
            // 
            this.buttonDecreaseFrequency.Location = new System.Drawing.Point(540, 763);
            this.buttonDecreaseFrequency.Name = "buttonDecreaseFrequency";
            this.buttonDecreaseFrequency.Size = new System.Drawing.Size(66, 27);
            this.buttonDecreaseFrequency.TabIndex = 8;
            this.buttonDecreaseFrequency.Text = "<";
            this.buttonDecreaseFrequency.UseVisualStyleBackColor = true;
            this.buttonDecreaseFrequency.Click += new System.EventHandler(this.buttonDecreaseFrequency_Click);
            // 
            // buttonIncreaseFrequency
            // 
            this.buttonIncreaseFrequency.Location = new System.Drawing.Point(788, 762);
            this.buttonIncreaseFrequency.Name = "buttonIncreaseFrequency";
            this.buttonIncreaseFrequency.Size = new System.Drawing.Size(65, 27);
            this.buttonIncreaseFrequency.TabIndex = 9;
            this.buttonIncreaseFrequency.Text = ">";
            this.buttonIncreaseFrequency.UseVisualStyleBackColor = true;
            this.buttonIncreaseFrequency.Click += new System.EventHandler(this.buttonIncreaseFrequency_Click);
            // 
            // labelFrequencyRange
            // 
            this.labelFrequencyRange.AutoSize = true;
            this.labelFrequencyRange.BackColor = System.Drawing.Color.Transparent;
            this.labelFrequencyRange.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelFrequencyRange.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFrequencyRange.ForeColor = System.Drawing.Color.White;
            this.labelFrequencyRange.Location = new System.Drawing.Point(620, 765);
            this.labelFrequencyRange.Name = "labelFrequencyRange";
            this.labelFrequencyRange.Size = new System.Drawing.Size(156, 20);
            this.labelFrequencyRange.TabIndex = 10;
            this.labelFrequencyRange.Text = "xxxxxxxxxxxxxxxxxxxxx";
            this.labelFrequencyRange.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonDecreaseFrequencyMajor
            // 
            this.buttonDecreaseFrequencyMajor.Location = new System.Drawing.Point(396, 763);
            this.buttonDecreaseFrequencyMajor.Name = "buttonDecreaseFrequencyMajor";
            this.buttonDecreaseFrequencyMajor.Size = new System.Drawing.Size(64, 27);
            this.buttonDecreaseFrequencyMajor.TabIndex = 11;
            this.buttonDecreaseFrequencyMajor.Text = "<<<";
            this.buttonDecreaseFrequencyMajor.UseVisualStyleBackColor = true;
            this.buttonDecreaseFrequencyMajor.Click += new System.EventHandler(this.buttonDecreaseFrequencyMajor_Click);
            // 
            // buttonIncreaseFrequencyMajor
            // 
            this.buttonIncreaseFrequencyMajor.Location = new System.Drawing.Point(931, 762);
            this.buttonIncreaseFrequencyMajor.Name = "buttonIncreaseFrequencyMajor";
            this.buttonIncreaseFrequencyMajor.Size = new System.Drawing.Size(67, 27);
            this.buttonIncreaseFrequencyMajor.TabIndex = 12;
            this.buttonIncreaseFrequencyMajor.Text = ">>>";
            this.buttonIncreaseFrequencyMajor.UseVisualStyleBackColor = true;
            this.buttonIncreaseFrequencyMajor.Click += new System.EventHandler(this.buttonIncreaseFrequencyMajor_Click);
            // 
            // buttonDecreaseMiddle
            // 
            this.buttonDecreaseMiddle.Location = new System.Drawing.Point(467, 763);
            this.buttonDecreaseMiddle.Name = "buttonDecreaseMiddle";
            this.buttonDecreaseMiddle.Size = new System.Drawing.Size(66, 27);
            this.buttonDecreaseMiddle.TabIndex = 13;
            this.buttonDecreaseMiddle.Text = "<<";
            this.buttonDecreaseMiddle.UseVisualStyleBackColor = true;
            this.buttonDecreaseMiddle.Click += new System.EventHandler(this.buttonDecreaseMiddle_Click);
            // 
            // buttonIncreaseMiddle
            // 
            this.buttonIncreaseMiddle.Location = new System.Drawing.Point(860, 762);
            this.buttonIncreaseMiddle.Name = "buttonIncreaseMiddle";
            this.buttonIncreaseMiddle.Size = new System.Drawing.Size(65, 27);
            this.buttonIncreaseMiddle.TabIndex = 14;
            this.buttonIncreaseMiddle.Text = ">>";
            this.buttonIncreaseMiddle.UseVisualStyleBackColor = true;
            this.buttonIncreaseMiddle.Click += new System.EventHandler(this.buttonIncreaseMiddle_Click);
            // 
            // FormFFT
            // 
            this.ClientSize = new System.Drawing.Size(1335, 812);
            this.Controls.Add(this.buttonIncreaseMiddle);
            this.Controls.Add(this.buttonDecreaseMiddle);
            this.Controls.Add(this.buttonIncreaseFrequencyMajor);
            this.Controls.Add(this.buttonDecreaseFrequencyMajor);
            this.Controls.Add(this.labelFrequencyRange);
            this.Controls.Add(this.buttonIncreaseFrequency);
            this.Controls.Add(this.buttonDecreaseFrequency);
            this.Controls.Add(this.labelScale);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxFFTWindow);
            this.Controls.Add(this.labelFFTPeriod);
            this.Controls.Add(this.buttonPreviousFFTPeriod);
            this.Controls.Add(this.buttonNextFFTPeriod);
            this.Controls.Add(this.chartFFT);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormFFT";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Frequency Analysis";
            this.Load += new System.EventHandler(this.FormFFT_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartFFT)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartFFT;
        private System.Windows.Forms.Button buttonNextFFTPeriod;
        private System.Windows.Forms.Button buttonPreviousFFTPeriod;
        private System.Windows.Forms.Label labelFFTPeriod;
        private System.Windows.Forms.ComboBox comboBoxFFTWindow;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelScale;
        private System.Windows.Forms.Button buttonDecreaseFrequency;
        private System.Windows.Forms.Button buttonIncreaseFrequency;
        private System.Windows.Forms.Label labelFrequencyRange;
        private System.Windows.Forms.Button buttonDecreaseFrequencyMajor;
        private System.Windows.Forms.Button buttonIncreaseFrequencyMajor;
        private System.Windows.Forms.Button buttonDecreaseMiddle;
        private System.Windows.Forms.Button buttonIncreaseMiddle;
    }
}
