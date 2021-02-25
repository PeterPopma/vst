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
        private int[] WaveData = new int[SynthGenerator.SHAPE_NUMPOINTS];
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
            myParent.SynthGenerator.CurrentWave.ShapeFrequency = new int[SynthGenerator.SHAPE_NUMPOINTS];
            for (int i = 0; i < WaveData.Length; i++)
            {
                // Note that graph is upside-down
                myParent.SynthGenerator.CurrentWave.ShapeFrequency[i] = SynthGenerator.SHAPE_MAX_VALUE - WaveData[i];
            }
            myParent.pictureBoxFrequencyShape.Refresh();
            myParent.SynthGenerator.CurrentWave.MinFrequency = Convert.ToDouble(labelFrequencyMin.Text);
            myParent.SynthGenerator.CurrentWave.MaxFrequency = Convert.ToDouble(labelFrequencyMax.Text);
            myParent.UpdateWaveControls();
            myParent.SynthGenerator.UpdateCurrentWaveData();

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
            textBoxFrequency1.Text = convertValueToFrequency(colorSliderFrequency1.Value).ToString("0.00");
        }

        private void buttonFrequency2Minus10_Click(object sender, EventArgs e)
        {
            if (colorSliderFrequency2.Value > 10)
            {
                colorSliderFrequency2.Value -= 10;
            }
            textBoxFrequency2.Text = convertValueToFrequency(colorSliderFrequency2.Value).ToString("0.00");
        }

        private void buttonFrequencyMinus1_Click(object sender, EventArgs e)
        {
            if (colorSliderFrequency1.Value > 1)
            {
                colorSliderFrequency1.Value -= 1;
            }
            textBoxFrequency1.Text = convertValueToFrequency(colorSliderFrequency1.Value).ToString("0.00");
        }

        private void buttonFrequency2Minus1_Click(object sender, EventArgs e)
        {
            if (colorSliderFrequency2.Value > 1)
            {
                colorSliderFrequency2.Value -= 1;
            }
            textBoxFrequency2.Text = convertValueToFrequency(colorSliderFrequency2.Value).ToString("0.00");
        }

        private void buttonFrequencyPlus1_Click(object sender, EventArgs e)
        {
            if (colorSliderFrequency1.Value < colorSliderFrequency1.Maximum)
            {
                colorSliderFrequency1.Value += 1;
            }
            textBoxFrequency1.Text = convertValueToFrequency(colorSliderFrequency1.Value).ToString("0.00");
        }

        private void buttonFrequency2Plus1_Click(object sender, EventArgs e)
        {
            if (colorSliderFrequency2.Value < colorSliderFrequency2.Maximum)
            {
                colorSliderFrequency2.Value += 1;
            }
            textBoxFrequency2.Text = convertValueToFrequency(colorSliderFrequency2.Value).ToString("0.00");
        }

        private void buttonFrequencyPlus10_Click(object sender, EventArgs e)
        {
            if (colorSliderFrequency1.Value < colorSliderFrequency1.Maximum - 9)
            {
                colorSliderFrequency1.Value += 10;
            }
            textBoxFrequency1.Text = convertValueToFrequency(colorSliderFrequency1.Value).ToString("0.00");
        }

        private void buttonFrequency2Plus10_Click(object sender, EventArgs e)
        {
            if (colorSliderFrequency2.Value < colorSliderFrequency2.Maximum - 9)
            {
                colorSliderFrequency2.Value += 10;
            }
            textBoxFrequency2.Text = convertValueToFrequency(colorSliderFrequency2.Value).ToString("0.00");
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
        }

        private void TimerEventProcessor(Object myObject,
                                   EventArgs myEventArgs)
        {
            AdjustDataWidth++;

            int begin_x = previousPoint.X - AdjustDataWidth;
            if (begin_x < 0)
            {
                begin_x = 0;
            }
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
            if (end_x > SynthGenerator.SHAPE_NUMPOINTS - 1)
            {
                end_x = SynthGenerator.SHAPE_NUMPOINTS - 1;
            }
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
            if (myParent.SynthGenerator.CurrentWave.ShapeFrequency.Length < SynthGenerator.SHAPE_NUMPOINTS)     // No data yet
            {
                for (int i = 0; i < WaveData.Length; i++)
                {
                    WaveData[i] = SynthGenerator.SHAPE_MAX_VALUE / 2;
                }
            }
            else
            {
                for (int i = 0; i < WaveData.Length; i++)
                {
                    WaveData[i] = SynthGenerator.SHAPE_MAX_VALUE - myParent.SynthGenerator.CurrentWave.ShapeFrequency[i];
                }
            }

            colorSliderFrequency1.Value = convertFrequencyToValue(myParent.SynthGenerator.CurrentWave.MinFrequency);
            colorSliderFrequency2.Value = convertFrequencyToValue(myParent.SynthGenerator.CurrentWave.MaxFrequency);
            textBoxFrequency1.Text = convertValueToFrequency(colorSliderFrequency1.Value).ToString("0.00");
            textBoxFrequency2.Text = convertValueToFrequency(colorSliderFrequency2.Value).ToString("0.00");

            pictureBoxFrequencyShape.Paint += new PaintEventHandler(PictureBoxPaint);
            pictureBoxFrequencyShape.Refresh();
        }

        private void pictureBoxFrequencyShape_MouseDown(object sender, MouseEventArgs e)
        {
            if (!isMouseButtonDown && e.X >= 0 && e.X < SynthGenerator.SHAPE_NUMPOINTS)
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
            if (isMouseButtonDown && e.X != previousPoint.X && e.X >= 0 && e.X < SynthGenerator.SHAPE_NUMPOINTS)
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

        private void textBoxFrequency1_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                colorSliderFrequency1.Value = colorSliderFrequency2.Value = colorSliderFrequency2.Value = convertFrequencyToValue(Convert.ToDouble(textBoxFrequency1.Text));
                textBoxFrequency2.Text = textBoxFrequency1.Text;
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

        private void colorSliderFrequency1_MouseUp(object sender, MouseEventArgs e)
        {
            textBoxFrequency1.Text = convertValueToFrequency(colorSliderFrequency1.Value).ToString("0.00");
        }

        private void colorSliderFrequency2_MouseUp(object sender, MouseEventArgs e)
        {
            textBoxFrequency2.Text = convertValueToFrequency(colorSliderFrequency2.Value).ToString("0.00");
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Shapes.Flat(WaveData);
            Refresh();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Shapes.DecreasingLineair(WaveData);     // note that graph is upside-down
            Refresh();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Shapes.IncreasingLineair(WaveData);     // note that graph is upside-down
            Refresh();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Shapes.DecreasingLogarithmic(WaveData);     // note that graph is upside-down
            Refresh();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Shapes.IncreasingLogarithmic(WaveData);     // note that graph is upside-down
            Refresh();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Shapes.IncDec(WaveData);
            Refresh();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Shapes.DecInc(WaveData);
            Refresh();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Shapes.Spikes(WaveData);
            Refresh();
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            try
            {
                Shapes.Sines(WaveData, Convert.ToInt32(textBoxNumSines.Text));
            }
            catch (Exception)
            {
                // probably bad input from textbox; ignore
            }
            Refresh();
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            try
            {
                int numSines = Convert.ToInt32(textBoxNumIncSines.Text);
                if(numSines>9)
                {
                    numSines = 9;
                }
                Shapes.IncSines(WaveData, numSines);
            }
            catch (Exception)
            {
                // probably bad input from textbox; ignore
            }
            Refresh();
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            try
            {
                int numSines = Convert.ToInt32(textBoxNumDecSines.Text);
                if (numSines > 9)
                {
                    numSines = 9;
                }
                Shapes.DecSines(WaveData, numSines);
            }
            catch (Exception)
            {
                // probably bad input from textbox; ignore
            }
            Refresh();
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            Shapes.RandomWaves(WaveData);
            Refresh();
        }
    }
}
