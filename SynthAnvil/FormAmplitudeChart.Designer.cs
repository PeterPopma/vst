
namespace SynthAnvil
{
    partial class FormAmplitudeChart
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
            this.labelFrequencyRange = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonChannelBoth = new System.Windows.Forms.RadioButton();
            this.radioButtonChannelRight = new System.Windows.Forms.RadioButton();
            this.radioButtonChannelLeft = new System.Windows.Forms.RadioButton();
            this.labelDuration = new System.Windows.Forms.Label();
            this.gradientButton8 = new SynthAnvil.CustomControls.GradientButton();
            this.gradientButton7 = new SynthAnvil.CustomControls.GradientButton();
            this.gradientButton6 = new SynthAnvil.CustomControls.GradientButton();
            this.gradientButton5 = new SynthAnvil.CustomControls.GradientButton();
            this.gradientButton4 = new SynthAnvil.CustomControls.GradientButton();
            this.gradientButton3 = new SynthAnvil.CustomControls.GradientButton();
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
            chartArea1.BackSecondaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.Name = "ChartArea1";
            this.chartAmplitude.ChartAreas.Add(chartArea1);
            this.chartAmplitude.Cursor = System.Windows.Forms.Cursors.Default;
            this.chartAmplitude.Location = new System.Drawing.Point(31, 52);
            this.chartAmplitude.Name = "chartAmplitude";
            series1.BackSecondaryColor = System.Drawing.Color.Black;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Color = System.Drawing.Color.Yellow;
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
            // gradientButton8
            // 
            this.gradientButton8.Active = false;
            this.gradientButton8.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.gradientButton8.FlatAppearance.BorderSize = 2;
            this.gradientButton8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gradientButton8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gradientButton8.ForeColor = System.Drawing.Color.Black;
            this.gradientButton8.HorizontalGradient = false;
            this.gradientButton8.Location = new System.Drawing.Point(935, 776);
            this.gradientButton8.Name = "gradientButton8";
            this.gradientButton8.Size = new System.Drawing.Size(64, 24);
            this.gradientButton8.TabIndex = 136;
            this.gradientButton8.Text = ">>>";
            this.gradientButton8.UseVisualStyleBackColor = true;
            this.gradientButton8.Click += new System.EventHandler(this.gradientButton8_Click);
            // 
            // gradientButton7
            // 
            this.gradientButton7.Active = false;
            this.gradientButton7.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.gradientButton7.FlatAppearance.BorderSize = 2;
            this.gradientButton7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gradientButton7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gradientButton7.ForeColor = System.Drawing.Color.Black;
            this.gradientButton7.HorizontalGradient = false;
            this.gradientButton7.Location = new System.Drawing.Point(863, 776);
            this.gradientButton7.Name = "gradientButton7";
            this.gradientButton7.Size = new System.Drawing.Size(64, 24);
            this.gradientButton7.TabIndex = 135;
            this.gradientButton7.Text = ">>";
            this.gradientButton7.UseVisualStyleBackColor = true;
            this.gradientButton7.Click += new System.EventHandler(this.gradientButton7_Click);
            // 
            // gradientButton6
            // 
            this.gradientButton6.Active = false;
            this.gradientButton6.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.gradientButton6.FlatAppearance.BorderSize = 2;
            this.gradientButton6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gradientButton6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gradientButton6.ForeColor = System.Drawing.Color.Black;
            this.gradientButton6.HorizontalGradient = false;
            this.gradientButton6.Location = new System.Drawing.Point(791, 776);
            this.gradientButton6.Name = "gradientButton6";
            this.gradientButton6.Size = new System.Drawing.Size(64, 24);
            this.gradientButton6.TabIndex = 134;
            this.gradientButton6.Text = ">";
            this.gradientButton6.UseVisualStyleBackColor = true;
            this.gradientButton6.Click += new System.EventHandler(this.gradientButton6_Click);
            // 
            // gradientButton5
            // 
            this.gradientButton5.Active = false;
            this.gradientButton5.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.gradientButton5.FlatAppearance.BorderSize = 2;
            this.gradientButton5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gradientButton5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gradientButton5.ForeColor = System.Drawing.Color.Black;
            this.gradientButton5.HorizontalGradient = false;
            this.gradientButton5.Location = new System.Drawing.Point(542, 776);
            this.gradientButton5.Name = "gradientButton5";
            this.gradientButton5.Size = new System.Drawing.Size(64, 24);
            this.gradientButton5.TabIndex = 133;
            this.gradientButton5.Text = "<";
            this.gradientButton5.UseVisualStyleBackColor = true;
            this.gradientButton5.Click += new System.EventHandler(this.gradientButton5_Click);
            // 
            // gradientButton4
            // 
            this.gradientButton4.Active = false;
            this.gradientButton4.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.gradientButton4.FlatAppearance.BorderSize = 2;
            this.gradientButton4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gradientButton4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gradientButton4.ForeColor = System.Drawing.Color.Black;
            this.gradientButton4.HorizontalGradient = false;
            this.gradientButton4.Location = new System.Drawing.Point(470, 776);
            this.gradientButton4.Name = "gradientButton4";
            this.gradientButton4.Size = new System.Drawing.Size(64, 24);
            this.gradientButton4.TabIndex = 132;
            this.gradientButton4.Text = "<<";
            this.gradientButton4.UseVisualStyleBackColor = true;
            this.gradientButton4.Click += new System.EventHandler(this.gradientButton4_Click);
            // 
            // gradientButton3
            // 
            this.gradientButton3.Active = false;
            this.gradientButton3.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.gradientButton3.FlatAppearance.BorderSize = 2;
            this.gradientButton3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gradientButton3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gradientButton3.ForeColor = System.Drawing.Color.Black;
            this.gradientButton3.HorizontalGradient = false;
            this.gradientButton3.Location = new System.Drawing.Point(398, 776);
            this.gradientButton3.Name = "gradientButton3";
            this.gradientButton3.Size = new System.Drawing.Size(64, 24);
            this.gradientButton3.TabIndex = 131;
            this.gradientButton3.Text = "<<<";
            this.gradientButton3.UseVisualStyleBackColor = true;
            this.gradientButton3.Click += new System.EventHandler(this.gradientButton3_Click);
            // 
            // FormAmplitudeChart
            // 
            this.ClientSize = new System.Drawing.Size(1335, 812);
            this.Controls.Add(this.gradientButton8);
            this.Controls.Add(this.gradientButton7);
            this.Controls.Add(this.gradientButton6);
            this.Controls.Add(this.gradientButton5);
            this.Controls.Add(this.gradientButton4);
            this.Controls.Add(this.gradientButton3);
            this.Controls.Add(this.labelDuration);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.labelFrequencyRange);
            this.Controls.Add(this.labelScale);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chartAmplitude);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAmplitudeChart";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Amplitude Analysis";
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
        private System.Windows.Forms.Label labelFrequencyRange;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonChannelBoth;
        private System.Windows.Forms.RadioButton radioButtonChannelRight;
        private System.Windows.Forms.RadioButton radioButtonChannelLeft;
        private System.Windows.Forms.Label labelDuration;
        private CustomControls.GradientButton gradientButton8;
        private CustomControls.GradientButton gradientButton7;
        private CustomControls.GradientButton gradientButton6;
        private CustomControls.GradientButton gradientButton5;
        private CustomControls.GradientButton gradientButton4;
        private CustomControls.GradientButton gradientButton3;
    }
}
