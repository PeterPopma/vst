
namespace SynthAnvil
{
    partial class FormFileToWaves
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.numericUpDownNumWaves = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.chartTarget = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chartCurrent = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelDifference = new System.Windows.Forms.Label();
            this.buttonCancel = new SynthAnvil.CustomControls.GradientButton();
            this.buttonApply = new SynthAnvil.CustomControls.GradientButton();
            this.buttonGenerateWaves = new SynthAnvil.CustomControls.GradientButton();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNumWaves)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartTarget)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartCurrent)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // numericUpDownNumWaves
            // 
            this.numericUpDownNumWaves.Location = new System.Drawing.Point(205, 26);
            this.numericUpDownNumWaves.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownNumWaves.Name = "numericUpDownNumWaves";
            this.numericUpDownNumWaves.Size = new System.Drawing.Size(52, 20);
            this.numericUpDownNumWaves.TabIndex = 113;
            this.numericUpDownNumWaves.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownNumWaves.ValueChanged += new System.EventHandler(this.numericUpDownNumWaves_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(77, 26);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(90, 13);
            this.label7.TabIndex = 112;
            this.label7.Text = "Number of waves";
            // 
            // chartTarget
            // 
            this.chartTarget.BackColor = System.Drawing.Color.Transparent;
            chartArea1.AxisX.LabelStyle.Enabled = false;
            chartArea1.AxisX.LineColor = System.Drawing.Color.Transparent;
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisX.MajorTickMark.Enabled = false;
            chartArea1.AxisY.LabelStyle.Enabled = false;
            chartArea1.AxisY.LineColor = System.Drawing.Color.Transparent;
            chartArea1.AxisY.MajorGrid.Enabled = false;
            chartArea1.AxisY.MajorTickMark.Enabled = false;
            chartArea1.BackColor = System.Drawing.Color.Silver;
            chartArea1.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.LeftRight;
            chartArea1.BackSecondaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.Name = "ChartArea1";
            this.chartTarget.ChartAreas.Add(chartArea1);
            this.chartTarget.Location = new System.Drawing.Point(172, 308);
            this.chartTarget.Name = "chartTarget";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Color = System.Drawing.Color.Yellow;
            series1.Name = "Series1";
            this.chartTarget.Series.Add(series1);
            this.chartTarget.Size = new System.Drawing.Size(843, 170);
            this.chartTarget.TabIndex = 165;
            this.chartTarget.Text = "chart1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(49, 308);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 166;
            this.label1.Text = "Target wave shape";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(49, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 13);
            this.label2.TabIndex = 167;
            this.label2.Text = "Current waveshape";
            // 
            // chartCurrent
            // 
            this.chartCurrent.BackColor = System.Drawing.Color.Transparent;
            chartArea2.AxisX.LabelStyle.Enabled = false;
            chartArea2.AxisX.LineColor = System.Drawing.Color.Transparent;
            chartArea2.AxisX.MajorGrid.Enabled = false;
            chartArea2.AxisX.MajorTickMark.Enabled = false;
            chartArea2.AxisY.LabelStyle.Enabled = false;
            chartArea2.AxisY.LineColor = System.Drawing.Color.Transparent;
            chartArea2.AxisY.MajorGrid.Enabled = false;
            chartArea2.AxisY.MajorTickMark.Enabled = false;
            chartArea2.BackColor = System.Drawing.Color.Silver;
            chartArea2.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.LeftRight;
            chartArea2.BackSecondaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea2.Name = "ChartArea1";
            this.chartCurrent.ChartAreas.Add(chartArea2);
            this.chartCurrent.Location = new System.Drawing.Point(172, 85);
            this.chartCurrent.Name = "chartCurrent";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Color = System.Drawing.Color.Yellow;
            series2.Name = "Series1";
            this.chartCurrent.Series.Add(series2);
            this.chartCurrent.Size = new System.Drawing.Size(843, 205);
            this.chartCurrent.TabIndex = 168;
            this.chartCurrent.Text = "chart1";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.labelDifference);
            this.panel1.Location = new System.Drawing.Point(484, 18);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(185, 36);
            this.panel1.TabIndex = 172;
            // 
            // labelDifference
            // 
            this.labelDifference.AutoSize = true;
            this.labelDifference.BackColor = System.Drawing.Color.Transparent;
            this.labelDifference.ForeColor = System.Drawing.Color.White;
            this.labelDifference.Location = new System.Drawing.Point(12, 11);
            this.labelDifference.Name = "labelDifference";
            this.labelDifference.Size = new System.Drawing.Size(79, 13);
            this.labelDifference.TabIndex = 172;
            this.labelDifference.Text = "Difference (%): ";
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
            this.buttonCancel.Location = new System.Drawing.Point(626, 511);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(87, 22);
            this.buttonCancel.TabIndex = 170;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click_1);
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
            this.buttonApply.Location = new System.Drawing.Point(261, 511);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(87, 22);
            this.buttonApply.TabIndex = 169;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // buttonGenerateWaves
            // 
            this.buttonGenerateWaves.Active = false;
            this.buttonGenerateWaves.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonGenerateWaves.FlatAppearance.BorderSize = 2;
            this.buttonGenerateWaves.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGenerateWaves.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonGenerateWaves.ForeColor = System.Drawing.Color.Black;
            this.buttonGenerateWaves.HorizontalGradient = false;
            this.buttonGenerateWaves.Location = new System.Drawing.Point(313, 24);
            this.buttonGenerateWaves.Name = "buttonGenerateWaves";
            this.buttonGenerateWaves.Size = new System.Drawing.Size(127, 22);
            this.buttonGenerateWaves.TabIndex = 163;
            this.buttonGenerateWaves.Text = "Generate wave data";
            this.buttonGenerateWaves.UseVisualStyleBackColor = true;
            this.buttonGenerateWaves.Click += new System.EventHandler(this.buttonGenerateWaves_Click);
            // 
            // FormFileToWaves
            // 
            this.ClientSize = new System.Drawing.Size(1046, 560);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.chartCurrent);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chartTarget);
            this.Controls.Add(this.buttonGenerateWaves);
            this.Controls.Add(this.numericUpDownNumWaves);
            this.Controls.Add(this.label7);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormFileToWaves";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = ".wav file to waves";
            this.Load += new System.EventHandler(this.FormFileToWaves_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNumWaves)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartTarget)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartCurrent)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numericUpDownNumWaves;
        private System.Windows.Forms.Label label7;
        private CustomControls.GradientButton buttonGenerateWaves;
        public System.Windows.Forms.DataVisualization.Charting.Chart chartTarget;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.DataVisualization.Charting.Chart chartCurrent;
        private CustomControls.GradientButton buttonApply;
        private CustomControls.GradientButton buttonCancel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelDifference;
    }
}
