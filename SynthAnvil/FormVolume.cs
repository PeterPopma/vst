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
            myParent.GenerateSound();

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
            int x_position = begin_x + 1;
            int end_x = previousPoint.X;
            int begin_y = WaveData[begin_x];
            int end_y = previousPoint.Y;

            // adjust all points left of mouse pointer
            while (x_position < end_x)
            {
                int interpolated_value = (((x_position - begin_x) * end_y) + ((end_x - x_position) * begin_y))/(end_x - begin_x);
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

            pictureBoxCustomWave.Paint += new PaintEventHandler(PictureBoxPaint);
            pictureBoxCustomWave.Refresh();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonSine_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length; i++)
            {
                WaveData[i] = (int)(((int)(Math.Sin(i / (double)WaveData.Length * 2 * Math.PI) * SynthGenerator.SHAPE_VOLUME_MAX_VALUE + SynthGenerator.SHAPE_VOLUME_MAX_VALUE)) / 2.0);
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
        }

        private void buttonVolumeMinus10_Click(object sender, EventArgs e)
        {
            if (colorSliderVolume1.Value > 9)
            {
                colorSliderVolume1.Value -= 10;
            }
        }

        private void buttonEndVolMinus1_Click(object sender, EventArgs e)
        {
            if (colorSliderVolume2.Value > 0)
            {
                colorSliderVolume2.Value -= 1;
            }
        }

        private void buttonEndVolMinus10_Click(object sender, EventArgs e)
        {
            if (colorSliderVolume2.Value > 9)
            {
                colorSliderVolume2.Value -= 10;
            }
        }

        private void buttonEndVolPlus1_Click(object sender, EventArgs e)
        {
            if (colorSliderVolume2.Value < colorSliderVolume2.Maximum)
            {
                colorSliderVolume2.Value += 1;
            }
        }

        private void buttonEndVolPlus10_Click(object sender, EventArgs e)
        {
            if (colorSliderVolume2.Value < colorSliderVolume2.Maximum - 9)
            {
                colorSliderVolume2.Value += 10;
            }
        }

        private void buttonVolumePlus1_Click(object sender, EventArgs e)
        {
            if (colorSliderVolume1.Value < colorSliderVolume1.Maximum)
            {
                colorSliderVolume1.Value += 1;
            }
        }

        private void buttonVolumePlus10_Click(object sender, EventArgs e)
        {
            if (colorSliderVolume1.Value < colorSliderVolume1.Maximum - 9)
            {
                colorSliderVolume1.Value += 10;
            }
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
            textBoxVolume1.Text = colorSliderVolume1.Value.ToString();
            textBoxVolume2.Text = colorSliderVolume2.Value.ToString();
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
                colorSliderVolume1.Value = Convert.ToDecimal(textBoxVolume1.Text);
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
    }
}
