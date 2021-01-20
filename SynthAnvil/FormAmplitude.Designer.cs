
namespace SynthAnvil
{
    partial class FormAmplitude
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
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chartAmplitude = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label2 = new System.Windows.Forms.Label();
            this.labelScale = new System.Windows.Forms.Label();
            this.buttonDecreaseFrequency = new System.Windows.Forms.Button();
            this.buttonIncreaseFrequency = new System.Windows.Forms.Button();
            this.labelFrequencyRange = new System.Windows.Forms.Label();
            this.buttonDecreaseFrequencyMajor = new System.Windows.Forms.Button();
            this.buttonIncreaseFrequencyMajor = new System.Windows.Forms.Button();
            this.buttonDecreaseMiddle = new System.Windows.Forms.Button();
            this.buttonIncreaseMiddle = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonChannelBoth = new System.Windows.Forms.RadioButton();
            this.radioButtonChannelRight = new System.Windows.Forms.RadioButton();
            this.radioButtonChannelLeft = new System.Windows.Forms.RadioButton();
            this.labelDuration = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chartAmplitude)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chartAmplitude
            // 
            this.chartAmplitude.BackColor = System.Drawing.Color.Transparent;
            this.chartAmplitude.BackSecondaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
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
            chartArea1.AxisY.Maximum = 32767D;
            chartArea1.AxisY.Minimum = -32767D;
            chartArea1.AxisY.MinorGrid.LineColor = System.Drawing.Color.DarkGray;
            chartArea1.AxisY.MinorTickMark.LineColor = System.Drawing.Color.DarkGray;
            chartArea1.BackColor = System.Drawing.Color.Silver;
            chartArea1.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.LeftRight;
            chartArea1.BackSecondaryColor = System.Drawing.Color.White;
            chartArea1.Name = "ChartArea1";
            this.chartAmplitude.ChartAreas.Add(chartArea1);
            this.chartAmplitude.Cursor = System.Windows.Forms.Cursors.Default;
            this.chartAmplitude.Location = new System.Drawing.Point(31, 52);
            this.chartAmplitude.Name = "chartAmplitude";
            series1.BackSecondaryColor = System.Drawing.Color.Black;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Color = System.Drawing.Color.Blue;
            series1.Name = "Series1";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Color = System.Drawing.Color.Red;
            series2.Name = "Series2";
            this.chartAmplitude.Series.Add(series1);
            this.chartAmplitude.Series.Add(series2);
            this.chartAmplitude.Size = new System.Drawing.Size(1272, 748);
            this.chartAmplitude.TabIndex = 0;
            this.chartAmplitude.Text = "chartFFT";
            this.chartAmplitude.Click += new System.EventHandler(this.chartAmplitude_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(254, 774);
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
            this.labelScale.Location = new System.Drawing.Point(318, 774);
            this.labelScale.Name = "labelScale";
            this.labelScale.Size = new System.Drawing.Size(18, 20);
            this.labelScale.TabIndex = 7;
            this.labelScale.Text = "1";
            // 
            // buttonDecreaseFrequency
            // 
            this.buttonDecreaseFrequency.Location = new System.Drawing.Point(540, 774);
            this.buttonDecreaseFrequency.Name = "buttonDecreaseFrequency";
            this.buttonDecreaseFrequency.Size = new System.Drawing.Size(66, 27);
            this.buttonDecreaseFrequency.TabIndex = 8;
            this.buttonDecreaseFrequency.Text = "<";
            this.buttonDecreaseFrequency.UseVisualStyleBackColor = true;
            this.buttonDecreaseFrequency.Click += new System.EventHandler(this.buttonDecreaseFrequency_Click);
            // 
            // buttonIncreaseFrequency
            // 
            this.buttonIncreaseFrequency.Location = new System.Drawing.Point(788, 773);
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
            this.labelFrequencyRange.Location = new System.Drawing.Point(620, 776);
            this.labelFrequencyRange.Name = "labelFrequencyRange";
            this.labelFrequencyRange.Size = new System.Drawing.Size(156, 20);
            this.labelFrequencyRange.TabIndex = 10;
            this.labelFrequencyRange.Text = "xxxxxxxxxxxxxxxxxxxxx";
            this.labelFrequencyRange.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonDecreaseFrequencyMajor
            // 
            this.buttonDecreaseFrequencyMajor.Location = new System.Drawing.Point(396, 774);
            this.buttonDecreaseFrequencyMajor.Name = "buttonDecreaseFrequencyMajor";
            this.buttonDecreaseFrequencyMajor.Size = new System.Drawing.Size(64, 27);
            this.buttonDecreaseFrequencyMajor.TabIndex = 11;
            this.buttonDecreaseFrequencyMajor.Text = "<<<";
            this.buttonDecreaseFrequencyMajor.UseVisualStyleBackColor = true;
            this.buttonDecreaseFrequencyMajor.Click += new System.EventHandler(this.buttonDecreaseFrequencyMajor_Click);
            // 
            // buttonIncreaseFrequencyMajor
            // 
            this.buttonIncreaseFrequencyMajor.Location = new System.Drawing.Point(931, 773);
            this.buttonIncreaseFrequencyMajor.Name = "buttonIncreaseFrequencyMajor";
            this.buttonIncreaseFrequencyMajor.Size = new System.Drawing.Size(67, 27);
            this.buttonIncreaseFrequencyMajor.TabIndex = 12;
            this.buttonIncreaseFrequencyMajor.Text = ">>>";
            this.buttonIncreaseFrequencyMajor.UseVisualStyleBackColor = true;
            this.buttonIncreaseFrequencyMajor.Click += new System.EventHandler(this.buttonIncreaseFrequencyMajor_Click);
            // 
            // buttonDecreaseMiddle
            // 
            this.buttonDecreaseMiddle.Location = new System.Drawing.Point(467, 774);
            this.buttonDecreaseMiddle.Name = "buttonDecreaseMiddle";
            this.buttonDecreaseMiddle.Size = new System.Drawing.Size(66, 27);
            this.buttonDecreaseMiddle.TabIndex = 13;
            this.buttonDecreaseMiddle.Text = "<<";
            this.buttonDecreaseMiddle.UseVisualStyleBackColor = true;
            this.buttonDecreaseMiddle.Click += new System.EventHandler(this.buttonDecreaseMiddle_Click);
            // 
            // buttonIncreaseMiddle
            // 
            this.buttonIncreaseMiddle.Location = new System.Drawing.Point(860, 773);
            this.buttonIncreaseMiddle.Name = "buttonIncreaseMiddle";
            this.buttonIncreaseMiddle.Size = new System.Drawing.Size(65, 27);
            this.buttonIncreaseMiddle.TabIndex = 14;
            this.buttonIncreaseMiddle.Text = ">>";
            this.buttonIncreaseMiddle.UseVisualStyleBackColor = true;
            this.buttonIncreaseMiddle.Click += new System.EventHandler(this.buttonIncreaseMiddle_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.radioButtonChannelBoth);
            this.groupBox1.Controls.Add(this.radioButtonChannelRight);
            this.groupBox1.Controls.Add(this.radioButtonChannelLeft);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(117, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(203, 51);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "channel";
            // 
            // radioButtonChannelBoth
            // 
            this.radioButtonChannelBoth.AutoSize = true;
            this.radioButtonChannelBoth.Location = new System.Drawing.Point(29, 20);
            this.radioButtonChannelBoth.Name = "radioButtonChannelBoth";
            this.radioButtonChannelBoth.Size = new System.Drawing.Size(46, 17);
            this.radioButtonChannelBoth.TabIndex = 22;
            this.radioButtonChannelBoth.Text = "both";
            this.radioButtonChannelBoth.UseVisualStyleBackColor = true;
            this.radioButtonChannelBoth.CheckedChanged += new System.EventHandler(this.radioButtonChannelBoth_CheckedChanged);
            // 
            // radioButtonChannelRight
            // 
            this.radioButtonChannelRight.AutoSize = true;
            this.radioButtonChannelRight.Location = new System.Drawing.Point(131, 20);
            this.radioButtonChannelRight.Name = "radioButtonChannelRight";
            this.radioButtonChannelRight.Size = new System.Drawing.Size(45, 17);
            this.radioButtonChannelRight.TabIndex = 21;
            this.radioButtonChannelRight.Text = "right";
            this.radioButtonChannelRight.UseVisualStyleBackColor = true;
            this.radioButtonChannelRight.CheckedChanged += new System.EventHandler(this.radioButtonChannelRight_CheckedChanged);
            // 
            // radioButtonChannelLeft
            // 
            this.radioButtonChannelLeft.AutoSize = true;
            this.radioButtonChannelLeft.Checked = true;
            this.radioButtonChannelLeft.Location = new System.Drawing.Point(81, 20);
            this.radioButtonChannelLeft.Name = "radioButtonChannelLeft";
            this.radioButtonChannelLeft.Size = new System.Drawing.Size(39, 17);
            this.radioButtonChannelLeft.TabIndex = 20;
            this.radioButtonChannelLeft.TabStop = true;
            this.radioButtonChannelLeft.Text = "left";
            this.radioButtonChannelLeft.UseVisualStyleBackColor = true;
            this.radioButtonChannelLeft.CheckedChanged += new System.EventHandler(this.radioButtonChannelLeft_CheckedChanged);
            // 
            // labelDuration
            // 
            this.labelDuration.AutoSize = true;
            this.labelDuration.BackColor = System.Drawing.Color.Transparent;
            this.labelDuration.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDuration.ForeColor = System.Drawing.Color.White;
            this.labelDuration.Location = new System.Drawing.Point(1097, 51);
            this.labelDuration.Name = "labelDuration";
            this.labelDuration.Size = new System.Drawing.Size(57, 20);
            this.labelDuration.TabIndex = 19;
            this.labelDuration.Text = "label1";
            // 
            // FormAmplitude
            // 
            this.ClientSize = new System.Drawing.Size(1335, 812);
            this.Controls.Add(this.labelDuration);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonIncreaseMiddle);
            this.Controls.Add(this.buttonDecreaseMiddle);
            this.Controls.Add(this.buttonIncreaseFrequencyMajor);
            this.Controls.Add(this.buttonDecreaseFrequencyMajor);
            this.Controls.Add(this.labelFrequencyRange);
            this.Controls.Add(this.buttonIncreaseFrequency);
            this.Controls.Add(this.buttonDecreaseFrequency);
            this.Controls.Add(this.labelScale);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chartAmplitude);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAmplitude";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormAmplitude_FormClosing);
            this.Load += new System.EventHandler(this.FormAmplitude_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartAmplitude)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartAmplitude;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelScale;
        private System.Windows.Forms.Button buttonDecreaseFrequency;
        private System.Windows.Forms.Button buttonIncreaseFrequency;
        private System.Windows.Forms.Label labelFrequencyRange;
        private System.Windows.Forms.Button buttonDecreaseFrequencyMajor;
        private System.Windows.Forms.Button buttonIncreaseFrequencyMajor;
        private System.Windows.Forms.Button buttonDecreaseMiddle;
        private System.Windows.Forms.Button buttonIncreaseMiddle;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonChannelBoth;
        private System.Windows.Forms.RadioButton radioButtonChannelRight;
        private System.Windows.Forms.RadioButton radioButtonChannelLeft;
        private System.Windows.Forms.Label labelDuration;
    }
}
