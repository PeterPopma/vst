using SynthAnvil.Synth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace SynthAnvil
{
    public partial class FormFrequency : Form
    {
        private FormMain myParent = null;
        private int[] WaveData = new int[SynthGenerator.SHAPE_FREQUENCY_NUMPOINTS];
        bool isMouseButtonDown = false;
        Point previousPoint;
        Timer aTimer = new Timer();
        int AdjustDataWidth = 0;

        public FormFrequency()
        {
            InitializeComponent();
            aTimer.Interval = 50;
            aTimer.Tick += new EventHandler(TimerEventProcessor);
        }

        public FormMain MyParent { get => myParent; set => myParent = value; }

        public float convertValueToFrequency(decimal value)
        {
            return (float)(Math.Pow((double)value, 1.27) / 100.0);
        }

        public decimal convertFrequencyToValue(double frequency)
        {
            decimal val = (decimal)Math.Pow(frequency * 100, 1 / 1.27);
            if (val < 1)
            {
                val = 1;
            }
            return val;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (this.ClientRectangle.Width == 0 || this.ClientRectangle.Height == 0)
            {
                return;
            }
            using (LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle,
                                                                       Color.Black,
                                                                       Color.FromArgb(70, 77, 95),
                                                                       90F))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            myParent.SynthGenerator.CurrentWave.ShapeFrequency = new int[SynthGenerator.SHAPE_FREQUENCY_NUMPOINTS];
            for (int i = 0; i < WaveData.Length; i++)
            {
                // Note that graph is upside-down
                myParent.SynthGenerator.CurrentWave.ShapeFrequency[i] = SynthGenerator.SHAPE_FREQUENCY_MAX_VALUE - WaveData[i];
            }
            myParent.pictureBoxFrequencyShape.Refresh();
            myParent.SynthGenerator.CurrentWave.MinFrequency = Convert.ToDouble(labelFrequencyMin.Text);
            myParent.SynthGenerator.CurrentWave.MaxFrequency = Convert.ToDouble(labelFrequencyMax.Text);
            myParent.UpdateWaveControls();
            myParent.GenerateSound();

            Close();
        }

        private void PictureBoxPaint(object sender, PaintEventArgs e)
        {
            Control control = (Control)sender;

            using (LinearGradientBrush brush = new LinearGradientBrush(control.ClientRectangle,
                                                                       Color.FromArgb(70, 195, 87),
                                                                       Color.FromArgb(0, 15, 0),
                                                                       90F))
            {
                e.Graphics.FillRectangle(brush, control.ClientRectangle);
                ControlPaint.DrawBorder(e.Graphics, control.ClientRectangle, Color.Gray, ButtonBorderStyle.Solid);
                DrawGraph(e);
            }
        }

        private void DrawGraph(PaintEventArgs e)
        {
            Pen pen = new Pen(Color.White);
            for (int i = 0; i < WaveData.Length - 1; i++)
            {
                e.Graphics.DrawLine(pen, new Point(i, WaveData[i]), new Point(i + 1, WaveData[i + 1]));
            }
        }

        // Fill all data from previous point to current point with interpolated values
        private void EditData(int X, int Y)
        {
            double increment = (previousPoint.Y - Y) / Math.Abs(X - previousPoint.X);
            int position = X;
            double value = Y;
            while (position != previousPoint.X)
            {
                WaveData[position] = (int)value;
                value += increment;
                if (X > previousPoint.X)
                {
                    position--;
                }
                else
                {
                    position++;
                }
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonFrequencyMinus10_Click(object sender, EventArgs e)
        {
            if (colorSliderFrequency1.Value > 10)
            {
                colorSliderFrequency1.Value -= 10;
            }
        }

        private void buttonFrequency2Minus10_Click(object sender, EventArgs e)
        {
            if (colorSliderFrequency2.Value > 10)
            {
                colorSliderFrequency2.Value -= 10;
            }
        }

        private void buttonFrequencyMinus1_Click(object sender, EventArgs e)
        {
            if (colorSliderFrequency1.Value > 1)
            {
                colorSliderFrequency1.Value -= 1;
            }
        }

        private void buttonFrequency2Minus1_Click(object sender, EventArgs e)
        {
            if (colorSliderFrequency2.Value > 1)
            {
                colorSliderFrequency2.Value -= 1;
            }
        }

        private void buttonFrequencyPlus1_Click(object sender, EventArgs e)
        {
            if (colorSliderFrequency1.Value < colorSliderFrequency1.Maximum)
            {
                colorSliderFrequency1.Value += 1;
            }
        }

        private void buttonFrequency2Plus1_Click(object sender, EventArgs e)
        {
            if (colorSliderFrequency2.Value < colorSliderFrequency2.Maximum)
            {
                colorSliderFrequency2.Value += 1;
            }
        }

        private void buttonFrequencyPlus10_Click(object sender, EventArgs e)
        {
            if (colorSliderFrequency1.Value < colorSliderFrequency1.Maximum - 9)
            {
                colorSliderFrequency1.Value += 10;
            }
        }

        private void buttonFrequency2Plus10_Click(object sender, EventArgs e)
        {
            if (colorSliderFrequency2.Value < colorSliderFrequency2.Maximum - 9)
            {
                colorSliderFrequency2.Value += 10;
            }
        }

        private void colorSliderFrequency1_ValueChanged(object sender, EventArgs e)
        {
            UpdateFrequencyLabels();
        }

        private void colorSliderFrequency2_ValueChanged(object sender, EventArgs e)
        {
            UpdateFrequencyLabels();
        }

        private void UpdateFrequencyLabels()
        {
            if (colorSliderFrequency1.Value > colorSliderFrequency2.Value)
            {
                labelFrequencyMin.Text = convertValueToFrequency(colorSliderFrequency2.Value).ToString("0.00");
                labelFrequencyMax.Text = convertValueToFrequency(colorSliderFrequency1.Value).ToString("0.00");
            }
            else
            {
                labelFrequencyMin.Text = convertValueToFrequency(colorSliderFrequency1.Value).ToString("0.00");
                labelFrequencyMax.Text = convertValueToFrequency(colorSliderFrequency2.Value).ToString("0.00");
            }

            if (textBoxFrequency1.Text.Length==0 || (!Convert.ToDouble(textBoxFrequency1.Text).Equals(convertValueToFrequency(colorSliderFrequency1.Value))))
            {
                textBoxFrequency1.Text = convertValueToFrequency(colorSliderFrequency1.Value).ToString("0.00");
            }
            if (textBoxFrequency2.Text.Length == 0 || (!Convert.ToDouble(textBoxFrequency2.Text).Equals(convertValueToFrequency(colorSliderFrequency2.Value))))
            {
                textBoxFrequency2.Text = convertValueToFrequency(colorSliderFrequency2.Value).ToString("0.00");
            }
        }

        private void buttonSine_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length; i++)
            {
                WaveData[i] = (int)(((int)(Math.Sin(i / (double)WaveData.Length * 2 * Math.PI) * SynthGenerator.SHAPE_FREQUENCY_MAX_VALUE + SynthGenerator.SHAPE_FREQUENCY_MAX_VALUE)) / 2.0);
            }
            Refresh();
        }

        private void Button2XSine_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length; i++)
            {
                WaveData[i] = (int)(((int)(Math.Sin(i / (double)WaveData.Length * 4 * Math.PI) * SynthGenerator.SHAPE_FREQUENCY_MAX_VALUE + SynthGenerator.SHAPE_FREQUENCY_MAX_VALUE)) / 2.0);
            }
            Refresh();
        }

        private void button3XSine_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length; i++)
            {
                WaveData[i] = (int)(((int)(Math.Sin(i / (double)WaveData.Length * 6 * Math.PI) * SynthGenerator.SHAPE_FREQUENCY_MAX_VALUE + SynthGenerator.SHAPE_FREQUENCY_MAX_VALUE)) / 2.0);
            }
            Refresh();
        }

        private void buttonFlat_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length; i++)
            {
                WaveData[i] = (int)(SynthGenerator.SHAPE_FREQUENCY_MAX_VALUE / 2.0);
            }
            Refresh();
        }

        private void buttonIncreasing_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length; i++)
            {
                WaveData[i] = (int)((SynthGenerator.SHAPE_FREQUENCY_NUMPOINTS - i) / (double)SynthGenerator.SHAPE_FREQUENCY_NUMPOINTS * SynthGenerator.SHAPE_FREQUENCY_MAX_VALUE);
            }
            Refresh();
        }

        private void buttonDecreasing_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length; i++)
            {
                WaveData[i] = (int)(i / (double)SynthGenerator.SHAPE_FREQUENCY_NUMPOINTS * SynthGenerator.SHAPE_FREQUENCY_MAX_VALUE);
            }
            Refresh();
        }

        private void TimerEventProcessor(Object myObject,
                                   EventArgs myEventArgs)
        {
            AdjustDataWidth++;

            int begin_x = previousPoint.X - AdjustDataWidth;
            int x_position = begin_x + 1;
            int end_x = previousPoint.X;
            int begin_y = WaveData[begin_x];
            int end_y = previousPoint.Y;

            // adjust all points left of mouse pointer
            while (x_position < end_x)
            {
                int interpolated_value = (((x_position - begin_x) * end_y) + ((end_x - x_position) * begin_y)) / (end_x - begin_x);
                WaveData[x_position] = (WaveData[x_position] + interpolated_value) / 2;
                x_position++;
            }

            begin_x = previousPoint.X;
            x_position = begin_x + 1;
            end_x = begin_x + AdjustDataWidth;
            begin_y = previousPoint.Y;
            end_y = WaveData[end_x];

            // adjust all points right of mouse pointer
            while (x_position < end_x)
            {
                int interpolated_value = (((x_position - begin_x) * end_y) + ((end_x - x_position) * begin_y)) / (end_x - begin_x);
                WaveData[x_position] = (WaveData[x_position] + interpolated_value) / 2;
                x_position++;
            }

            Refresh();
        }

        private void FormFrequency_Load(object sender, EventArgs e)
        {
            if (myParent.SynthGenerator.CurrentWave.ShapeFrequency.Length < SynthGenerator.SHAPE_FREQUENCY_NUMPOINTS)     // No data yet
            {
                for (int i = 0; i < WaveData.Length; i++)
                {
                    WaveData[i] = SynthGenerator.SHAPE_FREQUENCY_MAX_VALUE / 2;
                }
            }
            else
            {
                for (int i = 0; i < WaveData.Length; i++)
                {
                    WaveData[i] = SynthGenerator.SHAPE_FREQUENCY_MAX_VALUE - myParent.SynthGenerator.CurrentWave.ShapeFrequency[i];
                }
            }

            colorSliderFrequency1.Value = convertFrequencyToValue(myParent.SynthGenerator.CurrentWave.MinFrequency);
            colorSliderFrequency2.Value = convertFrequencyToValue(myParent.SynthGenerator.CurrentWave.MaxFrequency);

            pictureBoxFrequencyShape.Paint += new PaintEventHandler(PictureBoxPaint);
            pictureBoxFrequencyShape.Refresh();
        }

        private void pictureBoxFrequencyShape_MouseDown(object sender, MouseEventArgs e)
        {
            if (!isMouseButtonDown && e.X >= 0 && e.X < SynthGenerator.SHAPE_FREQUENCY_NUMPOINTS)
            {
                WaveData[e.X] = e.Y;
                Refresh();
                previousPoint.X = e.X;
                previousPoint.Y = e.Y;
                isMouseButtonDown = true;
                AdjustDataWidth = 1;
                aTimer.Enabled = true;
            }
        }

        private void pictureBoxFrequencyShape_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseButtonDown && e.X != previousPoint.X && e.X >= 0 && e.X < SynthGenerator.SHAPE_FREQUENCY_NUMPOINTS)
            {
                EditData(e.X, e.Y);
                previousPoint.X = e.X;
                previousPoint.Y = e.Y;
                Refresh();
            }
        }

        private void pictureBoxFrequencyShape_MouseUp(object sender, MouseEventArgs e)
        {
            {
                isMouseButtonDown = false;
                aTimer.Enabled = false;
            }
        }

        private void textBoxFrequency1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxFrequency2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4XSine_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length; i++)
            {
                WaveData[i] = (int)(((int)(Math.Sin(i / (double)WaveData.Length * 8 * Math.PI) * SynthGenerator.SHAPE_FREQUENCY_MAX_VALUE + SynthGenerator.SHAPE_FREQUENCY_MAX_VALUE)) / 2.0);
            }
            Refresh();
        }

        private void button5XSine_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length; i++)
            {
                WaveData[i] = (int)(((int)(Math.Sin(i / (double)WaveData.Length * 10 * Math.PI) * SynthGenerator.SHAPE_FREQUENCY_MAX_VALUE + SynthGenerator.SHAPE_FREQUENCY_MAX_VALUE)) / 2.0);
            }
            Refresh();
        }

        private void gradientButton1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length / 2; i++)
            {
                WaveData[i] = (int)(((SynthGenerator.SHAPE_FREQUENCY_NUMPOINTS - i*2) / (double)SynthGenerator.SHAPE_FREQUENCY_NUMPOINTS) * SynthGenerator.SHAPE_FREQUENCY_MAX_VALUE);
            }
            for (int i = WaveData.Length / 2; i < WaveData.Length; i++)
            {
                WaveData[i] = (int)(((i-WaveData.Length /2) * SynthGenerator.SHAPE_FREQUENCY_MAX_VALUE)/ (WaveData.Length / 2));
            }
            Refresh();
        }

        private void textBoxFrequency1_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                colorSliderFrequency1.Value = convertFrequencyToValue(Convert.ToDouble(textBoxFrequency1.Text));
            }
            catch (Exception)
            {
            }
        }

        private void textBoxFrequency2_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                colorSliderFrequency2.Value = convertFrequencyToValue(Convert.ToDouble(textBoxFrequency2.Text));
            }
            catch (Exception)
            {
            }
        }

        private void gradientButton2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length / 2; i++)
            {
                WaveData[i] = (int)(((i) * SynthGenerator.SHAPE_FREQUENCY_MAX_VALUE) / (WaveData.Length / 2));
            }
            for (int i = WaveData.Length / 2; i < WaveData.Length; i++)
            {
                WaveData[i] = (int)(((SynthGenerator.SHAPE_FREQUENCY_NUMPOINTS - (i - WaveData.Length / 2) * 2) / (double)SynthGenerator.SHAPE_FREQUENCY_NUMPOINTS) * SynthGenerator.SHAPE_FREQUENCY_MAX_VALUE);
            }
            Refresh();
        }

        private void gradientButton3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length; i++)
            {
                WaveData[i] = (int)(((int)(Math.Sin(i / (double)WaveData.Length * 12 * Math.PI) * SynthGenerator.SHAPE_FREQUENCY_MAX_VALUE + SynthGenerator.SHAPE_FREQUENCY_MAX_VALUE)) / 2.0);
            }
            Refresh();
        }
    }
}
