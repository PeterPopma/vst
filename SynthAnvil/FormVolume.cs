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
    public partial class FormVolume : Form
    {
        private FormMain myParent = null;
        private int[] WaveData = new int[SynthGenerator.SHAPE_VOLUME_NUMPOINTS];
        bool isMouseButtonDown = false;
        Point previousPoint;
        Timer aTimer = new Timer();
        int AdjustDataWidth = 0;
        Random random = new Random();

        public FormVolume()
        {
            InitializeComponent();
            aTimer.Interval = 50;
            aTimer.Tick += new EventHandler(TimerEventProcessor);
        }

        public FormMain MyParent { get => myParent; set => myParent = value; }

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

        private void pictureBoxCustomWave_MouseDown(object sender, MouseEventArgs e)
        {
            if (!isMouseButtonDown && e.X >= 0 && e.X < SynthGenerator.SHAPE_VOLUME_NUMPOINTS)
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

        private void pictureBoxCustomWave_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseButtonDown && e.X != previousPoint.X && e.X >= 0 && e.X < SynthGenerator.SHAPE_VOLUME_NUMPOINTS)
            {
                EditData(e.X, e.Y);
                previousPoint.X = e.X;
                previousPoint.Y = e.Y;
                Refresh();
            }
        }

        private void pictureBoxCustomWave_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseButtonDown = false;
            aTimer.Enabled = false;
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            myParent.SynthGenerator.CurrentWave.ShapeVolume = new int[SynthGenerator.SHAPE_VOLUME_NUMPOINTS];
            for (int i = 0; i < WaveData.Length; i++)
            {
                // Note that graph is upside-down
                myParent.SynthGenerator.CurrentWave.ShapeVolume[i] = SynthGenerator.SHAPE_VOLUME_MAX_VALUE - WaveData[i];
            }
            myParent.pictureBoxVolumeShape.Refresh();
            myParent.SynthGenerator.CurrentWave.MinVolume = Convert.ToInt32(labelVolumeMin.Text);
            myParent.SynthGenerator.CurrentWave.MaxVolume = Convert.ToInt32(labelVolumeMax.Text);
            myParent.UpdateWaveControls();
            myParent.SynthGenerator.UpdateCurrentWaveData();

            Close();
        }

        private void PictureBoxPaint(object sender, PaintEventArgs e)
        {
            Control control = (Control)sender;

            using (LinearGradientBrush brush = new LinearGradientBrush(control.ClientRectangle,
                                                                       Color.FromArgb(70, 87, 195),
                                                                       Color.FromArgb(0, 0, 15),
                                                                       90F))
            {
                e.Graphics.FillRectangle(brush, control.ClientRectangle);
                ControlPaint.DrawBorder(e.Graphics, control.ClientRectangle, Color.Gray, ButtonBorderStyle.Solid);
                DrawGraph(e);
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
            if (end_x > SynthGenerator.SHAPE_VOLUME_NUMPOINTS - 1)
            {
                end_x = SynthGenerator.SHAPE_VOLUME_NUMPOINTS - 1;
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

        private void FormVolume_Load(object sender, EventArgs e)
        {
            if (myParent.SynthGenerator.CurrentWave.ShapeVolume.Length < SynthGenerator.SHAPE_VOLUME_NUMPOINTS)     // No data yet
            {
                for (int i = 0; i < WaveData.Length; i++)
                {
                    WaveData[i] = SynthGenerator.SHAPE_VOLUME_MAX_VALUE / 2;
                }
            }
            else
            {
                for (int i = 0; i < WaveData.Length; i++)
                {
                    WaveData[i] = SynthGenerator.SHAPE_VOLUME_MAX_VALUE - myParent.SynthGenerator.CurrentWave.ShapeVolume[i];
                }
            }

            colorSliderVolume1.Value = myParent.SynthGenerator.CurrentWave.MinVolume;
            colorSliderVolume2.Value = myParent.SynthGenerator.CurrentWave.MaxVolume;
            textBoxVolume1.Text = colorSliderVolume1.Value.ToString();
            textBoxVolume2.Text = colorSliderVolume2.Value.ToString();

            pictureBoxCustomWave.Paint += new PaintEventHandler(PictureBoxPaint);
            pictureBoxCustomWave.Refresh();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonSine_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < WaveData.Length; i++)
                {
                    WaveData[i] = (int)(((int)(Math.Sin(i / (double)WaveData.Length * 2 * Convert.ToInt32(textBoxNumSines.Text) * Math.PI) * SynthGenerator.SHAPE_VOLUME_MAX_VALUE + SynthGenerator.SHAPE_VOLUME_MAX_VALUE)) / 2.0);
                }
            }
            catch (Exception)
            {
                // probably bad input from textbox; ignore
            }

            Refresh();
        }

        private void buttonFlat_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length; i++)
            {
                WaveData[i] = (int)(SynthGenerator.SHAPE_VOLUME_MAX_VALUE / 2.0);
            }
            Refresh();
        }

        private void gradientButton1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length; i++)
            {
                WaveData[i] = (int)(((int)(Math.Sin(i / (double)WaveData.Length * 4 * Math.PI) * SynthGenerator.SHAPE_VOLUME_MAX_VALUE + SynthGenerator.SHAPE_VOLUME_MAX_VALUE)) / 2.0);
            }
            Refresh();
        }

        private void gradientButton2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length; i++)
            {
                WaveData[i] = (int)(((int)(Math.Sin(i / (double)WaveData.Length * 6 * Math.PI) * SynthGenerator.SHAPE_VOLUME_MAX_VALUE + SynthGenerator.SHAPE_VOLUME_MAX_VALUE)) / 2.0);
            }
            Refresh();
        }

        private void buttonIncreasing_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length; i++)
            {
                WaveData[i] = (int)((SynthGenerator.SHAPE_VOLUME_NUMPOINTS - i) / (double)SynthGenerator.SHAPE_VOLUME_NUMPOINTS * SynthGenerator.SHAPE_VOLUME_MAX_VALUE);
            }
            Refresh();
        }

        private void buttonDecreasing_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length; i++)
            {
                WaveData[i] = (int)(i / (double)SynthGenerator.SHAPE_VOLUME_NUMPOINTS * SynthGenerator.SHAPE_VOLUME_MAX_VALUE);
            }
            Refresh();
        }

        private void buttonVolumeMinus1_Click(object sender, EventArgs e)
        {
            if (colorSliderVolume1.Value > 0)
            {
                colorSliderVolume1.Value -= 1;
            }
            textBoxVolume1.Text = colorSliderVolume1.Value.ToString();
        }

        private void buttonVolumeMinus10_Click(object sender, EventArgs e)
        {
            if (colorSliderVolume1.Value > 9)
            {
                colorSliderVolume1.Value -= 10;
            }
            textBoxVolume1.Text = colorSliderVolume1.Value.ToString();
        }

        private void buttonEndVolMinus1_Click(object sender, EventArgs e)
        {
            if (colorSliderVolume2.Value > 0)
            {
                colorSliderVolume2.Value -= 1;
            }
            textBoxVolume2.Text = colorSliderVolume2.Value.ToString();
        }

        private void buttonEndVolMinus10_Click(object sender, EventArgs e)
        {
            if (colorSliderVolume2.Value > 9)
            {
                colorSliderVolume2.Value -= 10;
            }
            textBoxVolume2.Text = colorSliderVolume2.Value.ToString();
        }

        private void buttonEndVolPlus1_Click(object sender, EventArgs e)
        {
            if (colorSliderVolume2.Value < colorSliderVolume2.Maximum)
            {
                colorSliderVolume2.Value += 1;
            }
            textBoxVolume2.Text = colorSliderVolume2.Value.ToString();
        }

        private void buttonEndVolPlus10_Click(object sender, EventArgs e)
        {
            if (colorSliderVolume2.Value < colorSliderVolume2.Maximum - 9)
            {
                colorSliderVolume2.Value += 10;
            }
            textBoxVolume2.Text = colorSliderVolume2.Value.ToString();
        }

        private void buttonVolumePlus1_Click(object sender, EventArgs e)
        {
            if (colorSliderVolume1.Value < colorSliderVolume1.Maximum)
            {
                colorSliderVolume1.Value += 1;
            }
            textBoxVolume1.Text = colorSliderVolume1.Value.ToString();
        }

        private void buttonVolumePlus10_Click(object sender, EventArgs e)
        {
            if (colorSliderVolume1.Value < colorSliderVolume1.Maximum - 9)
            {
                colorSliderVolume1.Value += 10;
            }
            textBoxVolume1.Text = colorSliderVolume1.Value.ToString();
        }

        private void UpdateVolumeLabels()
        {
            if (colorSliderVolume1.Value>colorSliderVolume2.Value)
            {
                labelVolumeMin.Text = colorSliderVolume2.Value.ToString();
                labelVolumeMax.Text = colorSliderVolume1.Value.ToString();
            }
            else
            {
                labelVolumeMin.Text = colorSliderVolume1.Value.ToString();
                labelVolumeMax.Text = colorSliderVolume2.Value.ToString();
            }
        }

        private void colorSliderBeginVolume_ValueChanged(object sender, EventArgs e)
        {
            UpdateVolumeLabels();
        }

        private void colorSliderEndVolume_ValueChanged(object sender, EventArgs e)
        {
            UpdateVolumeLabels();
        }

        private void textBoxVolume1_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                colorSliderVolume1.Value = colorSliderVolume2.Value = Convert.ToDecimal(textBoxVolume1.Text);
                textBoxVolume2.Text = textBoxVolume1.Text;
            }
            catch (Exception)
            {
            }
        }

        private void textBoxVolume2_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                colorSliderVolume2.Value = Convert.ToDecimal(textBoxVolume2.Text);
            }
            catch (Exception)
            {
            }
        }

        private void gradientButton5_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length; i++)
            {
                WaveData[i] = (int)(((int)(Math.Sin(i / (double)WaveData.Length * 8 * Math.PI) * SynthGenerator.SHAPE_VOLUME_MAX_VALUE + SynthGenerator.SHAPE_VOLUME_MAX_VALUE)) / 2.0);
            }
            Refresh();
        }

        private void gradientButton6_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length; i++)
            {
                WaveData[i] = (int)(((int)(Math.Sin(i / (double)WaveData.Length * 10 * Math.PI) * SynthGenerator.SHAPE_VOLUME_MAX_VALUE + SynthGenerator.SHAPE_VOLUME_MAX_VALUE)) / 2.0);
            }
            Refresh();
        }

        private void gradientButton7_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length; i++)
            {
                WaveData[i] = (int)(((int)(Math.Sin(i / (double)WaveData.Length * 12 * Math.PI) * SynthGenerator.SHAPE_VOLUME_MAX_VALUE + SynthGenerator.SHAPE_VOLUME_MAX_VALUE)) / 2.0);
            }
            Refresh();
        }

        private void gradientButton3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length / 2; i++)
            {
                WaveData[i] = (int)(((SynthGenerator.SHAPE_VOLUME_NUMPOINTS - i * 2) / (double)SynthGenerator.SHAPE_VOLUME_NUMPOINTS) * SynthGenerator.SHAPE_VOLUME_MAX_VALUE);
            }
            for (int i = WaveData.Length / 2; i < WaveData.Length; i++)
            {
                WaveData[i] = (int)(((i - WaveData.Length / 2) * SynthGenerator.SHAPE_VOLUME_MAX_VALUE) / (WaveData.Length / 2));
            }
            Refresh();
        }

        private void gradientButton4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length / 2; i++)
            {
                WaveData[i] = (int)(((i) * SynthGenerator.SHAPE_VOLUME_MAX_VALUE) / (WaveData.Length / 2));
            }
            for (int i = WaveData.Length / 2; i < WaveData.Length; i++)
            {
                WaveData[i] = (int)(((SynthGenerator.SHAPE_VOLUME_NUMPOINTS - (i - WaveData.Length / 2) * 2) / (double)SynthGenerator.SHAPE_VOLUME_NUMPOINTS) * SynthGenerator.SHAPE_VOLUME_MAX_VALUE);
            }
            Refresh();
        }

        private void buttonRipples_Click(object sender, EventArgs e)
        {
            int x_position = 0;
            while (x_position < WaveData.Length)
            {
                if (x_position < SynthGenerator.SHAPE_VOLUME_NUMPOINTS - 5 && random.Next(15) == 11)
                {
                    int amplitude = random.Next(SynthGenerator.SHAPE_VOLUME_MAX_VALUE/2);
                    int up_down_spike = 1;
                    if (random.Next(100) < 50)
                    {
                        // up spike
                        up_down_spike = -1;
                    }
                    for (int j = 0; j < 5; j++)
                    {
                        WaveData[x_position] = SynthGenerator.SHAPE_VOLUME_MAX_VALUE/2 - ((int)(amplitude * Math.Sin(((j + 1) / 6.0) * Math.PI)) * up_down_spike);
                        x_position++;
                    }
                }
                else
                {
                    WaveData[x_position] = SynthGenerator.SHAPE_VOLUME_MAX_VALUE/2;
                    x_position++;
                }
            }
            Refresh();
        }

        private void button1SizeRipples_Click(object sender, EventArgs e)
        {
            int x_position = 0;
            while (x_position < WaveData.Length)
            {
                if (x_position < SynthGenerator.SHAPE_VOLUME_NUMPOINTS - 5 && random.Next(20) == 11)
                {
                    int amplitude = SynthGenerator.SHAPE_VOLUME_MAX_VALUE/2;
                    int up_down_spike = 1;
                    if (random.Next(100) < 50)
                    {
                        // up spike
                        up_down_spike = -1;
                    }
                    for (int j = 0; j < 5; j++)
                    {
                        WaveData[x_position] = SynthGenerator.SHAPE_VOLUME_MAX_VALUE/2 - ((int)(amplitude * Math.Sin(((j + 1) / 6.0) * Math.PI)) * up_down_spike);
                        x_position++;
                    }
                }
                else
                {
                    WaveData[x_position] = SynthGenerator.SHAPE_VOLUME_MAX_VALUE/2;
                    x_position++;
                }
            }
            Refresh();
        }

        private void buttonIncSine_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length; i++)
            {
                double factor = Math.Pow(1.003, i);   // between 1 and 20
                WaveData[i] = (int)(((int)(Math.Sin(i / (double)WaveData.Length * factor * Math.PI) * SynthGenerator.SHAPE_VOLUME_MAX_VALUE + SynthGenerator.SHAPE_VOLUME_MAX_VALUE)) / 2.0);
            }
            Refresh();
        }

        private void buttonDecSine_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length; i++)
            {
                double factor = Math.Pow(1.003, i);   // between 1 and 20
                WaveData[WaveData.Length-1-i] = (int)(((int)(Math.Sin(i / (double)WaveData.Length * factor * Math.PI) * SynthGenerator.SHAPE_VOLUME_MAX_VALUE + SynthGenerator.SHAPE_VOLUME_MAX_VALUE)) / 2.0);
            }
            Refresh();
        }

        private void buttonWaves_Click(object sender, EventArgs e)
        {
            int amplitude = random.Next(SynthGenerator.SHAPE_VOLUME_MAX_VALUE/2);
            int period = random.Next(50) + 5;
            bool amplitude_increasing = false;
            for (int i = 0; i < WaveData.Length; i++)
            {
                if (random.Next(20)==3)
                {
                    amplitude_increasing = !amplitude_increasing;
                }
                if (amplitude_increasing)
                {
                    if (amplitude< SynthGenerator.SHAPE_VOLUME_MAX_VALUE / 2)
                    {
                        amplitude++;
                    }
                    else
                    {
                        amplitude_increasing = false;
                    }
                }
                else
                {
                    if (amplitude > 0)
                    {
                        amplitude--;
                    }
                    else
                    {
                        amplitude_increasing = true;
                    }
                }
                WaveData[i] = (int)((Math.Sin(i / (double)WaveData.Length * period * Math.PI) * amplitude) + (SynthGenerator.SHAPE_VOLUME_MAX_VALUE / 2.0));
            }
            Refresh();
        }

        private void button7xSine_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length; i++)
            {
                WaveData[i] = (int)(((int)(Math.Sin(i / (double)WaveData.Length * 14 * Math.PI) * SynthGenerator.SHAPE_VOLUME_MAX_VALUE + SynthGenerator.SHAPE_VOLUME_MAX_VALUE)) / 2.0);
            }
            Refresh();
        }

        private void buttonLogInc_Click(object sender, EventArgs e)
        {
            double factor = Math.Pow(SynthGenerator.SHAPE_VOLUME_MAX_VALUE, 1.0 / SynthGenerator.SHAPE_VOLUME_NUMPOINTS);
            for (int i = 0; i < WaveData.Length; i++)
            {
                WaveData[i] = SynthGenerator.SHAPE_VOLUME_MAX_VALUE - (int)Math.Pow(factor, i); 
            }
            Refresh();
        }

        private void buttonLogDec_Click(object sender, EventArgs e)
        {
            double factor = Math.Pow(SynthGenerator.SHAPE_VOLUME_MAX_VALUE, 1.0 / SynthGenerator.SHAPE_VOLUME_NUMPOINTS);
            for (int i = 0; i < WaveData.Length; i++)
            {
                WaveData[i] = SynthGenerator.SHAPE_VOLUME_MAX_VALUE - (int)Math.Pow(factor, SynthGenerator.SHAPE_VOLUME_NUMPOINTS-i);
            }
            Refresh();
        }

        private void colorSliderVolume1_MouseUp(object sender, MouseEventArgs e)
        {
            textBoxVolume1.Text = colorSliderVolume1.Value.ToString();
        }

        private void colorSliderVolume2_MouseUp(object sender, MouseEventArgs e)
        {
            textBoxVolume2.Text = colorSliderVolume2.Value.ToString();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length; i++)
            {
                WaveData[i] = (int)(SynthGenerator.SHAPE_VOLUME_MAX_VALUE / 2.0);
            }
            Refresh();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length; i++)
            {
                WaveData[i] = (int)((SynthGenerator.SHAPE_VOLUME_NUMPOINTS - i) / (double)SynthGenerator.SHAPE_VOLUME_NUMPOINTS * SynthGenerator.SHAPE_VOLUME_MAX_VALUE);
            }
            Refresh();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length; i++)
            {
                WaveData[i] = (int)(i / (double)SynthGenerator.SHAPE_VOLUME_NUMPOINTS * SynthGenerator.SHAPE_VOLUME_MAX_VALUE);
            }
            Refresh();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            double factor = Math.Pow(SynthGenerator.SHAPE_VOLUME_MAX_VALUE, 1.0 / SynthGenerator.SHAPE_VOLUME_NUMPOINTS);
            for (int i = 0; i < WaveData.Length; i++)
            {
                WaveData[i] = SynthGenerator.SHAPE_VOLUME_MAX_VALUE - (int)Math.Pow(factor, i);
            }
            Refresh();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            double factor = Math.Pow(SynthGenerator.SHAPE_VOLUME_MAX_VALUE, 1.0 / SynthGenerator.SHAPE_VOLUME_NUMPOINTS);
            for (int i = 0; i < WaveData.Length; i++)
            {
                WaveData[i] = SynthGenerator.SHAPE_VOLUME_MAX_VALUE - (int)Math.Pow(factor, SynthGenerator.SHAPE_VOLUME_NUMPOINTS - i);
            }
            Refresh();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length / 2; i++)
            {
                WaveData[i] = (int)(((SynthGenerator.SHAPE_VOLUME_NUMPOINTS - i * 2) / (double)SynthGenerator.SHAPE_VOLUME_NUMPOINTS) * SynthGenerator.SHAPE_VOLUME_MAX_VALUE);
            }
            for (int i = WaveData.Length / 2; i < WaveData.Length; i++)
            {
                WaveData[i] = (int)(((i - WaveData.Length / 2) * SynthGenerator.SHAPE_VOLUME_MAX_VALUE) / (WaveData.Length / 2));
            }
            Refresh();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length / 2; i++)
            {
                WaveData[i] = (int)(((i) * SynthGenerator.SHAPE_VOLUME_MAX_VALUE) / (WaveData.Length / 2));
            }
            for (int i = WaveData.Length / 2; i < WaveData.Length; i++)
            {
                WaveData[i] = (int)(((SynthGenerator.SHAPE_VOLUME_NUMPOINTS - (i - WaveData.Length / 2) * 2) / (double)SynthGenerator.SHAPE_VOLUME_NUMPOINTS) * SynthGenerator.SHAPE_VOLUME_MAX_VALUE);
            }
            Refresh();
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length; i++)
            {
                double factor = Math.Pow(1.003, i);   // between 1 and 20
                WaveData[i] = (int)(((int)(Math.Sin(i / (double)WaveData.Length * factor * Math.PI) * SynthGenerator.SHAPE_VOLUME_MAX_VALUE + SynthGenerator.SHAPE_VOLUME_MAX_VALUE)) / 2.0);
            }
            Refresh();
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length; i++)
            {
                double factor = Math.Pow(1.003, i);   // between 1 and 20
                WaveData[WaveData.Length - 1 - i] = (int)(((int)(Math.Sin(i / (double)WaveData.Length * factor * Math.PI) * SynthGenerator.SHAPE_VOLUME_MAX_VALUE + SynthGenerator.SHAPE_VOLUME_MAX_VALUE)) / 2.0);
            }
            Refresh();
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            int amplitude = random.Next(SynthGenerator.SHAPE_VOLUME_MAX_VALUE / 2);
            int period = random.Next(50) + 5;
            bool amplitude_increasing = false;
            for (int i = 0; i < WaveData.Length; i++)
            {
                if (random.Next(20) == 3)
                {
                    amplitude_increasing = !amplitude_increasing;
                }
                if (amplitude_increasing)
                {
                    if (amplitude < SynthGenerator.SHAPE_VOLUME_MAX_VALUE / 2)
                    {
                        amplitude++;
                    }
                    else
                    {
                        amplitude_increasing = false;
                    }
                }
                else
                {
                    if (amplitude > 0)
                    {
                        amplitude--;
                    }
                    else
                    {
                        amplitude_increasing = true;
                    }
                }
                WaveData[i] = (int)((Math.Sin(i / (double)WaveData.Length * period * Math.PI) * amplitude) + (SynthGenerator.SHAPE_VOLUME_MAX_VALUE / 2.0));
            }
            Refresh();
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < WaveData.Length; i++)
                {
                    WaveData[i] = (int)(((int)(Math.Sin(i / (double)WaveData.Length * 2 * Convert.ToInt32(textBoxNumSines.Text) * Math.PI) * SynthGenerator.SHAPE_VOLUME_MAX_VALUE + SynthGenerator.SHAPE_VOLUME_MAX_VALUE)) / 2.0);
                }
            }
            catch (Exception)
            {
                // probably bad input from textbox; ignore
            }

            Refresh();
        }
    }
}
