﻿using SynthAnvil.Synth;
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

        public FormFrequency()
        {
            InitializeComponent();
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
            if (e.X >= 0 && e.X < SynthGenerator.SHAPE_FREQUENCY_NUMPOINTS)
            {
                WaveData[e.X] = e.Y;
                Refresh();
                previousPoint.X = e.X;
                previousPoint.Y = e.Y;
                isMouseButtonDown = true;
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
            }
        }


    }
}
