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
    public partial class FormCustomWave : Form
    {
        private FormMain myParent = null;
        private int[] WaveData = new int[SynthGenerator.NUM_CUSTOMWAVE_POINTS];
        bool isMouseButtonDown = false;
        Point previousPoint;

        public FormCustomWave()
        {
            InitializeComponent();
        }

        public FormMain MyParent { get => myParent; set => myParent = value; }

        private void PictureBoxPaint(object sender, PaintEventArgs e)
        {
            Control control = (Control)sender;

            using (LinearGradientBrush brush = new LinearGradientBrush(control.ClientRectangle,
                                                                       Color.FromArgb(70, 87, 195),
                                                                       Color.FromArgb(0, 0, 65),
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
            for(int i=0; i<WaveData.Length-1; i++)
            {
                e.Graphics.DrawLine(pen, new Point(i, WaveData[i]), new Point(i + 1, WaveData[i+1]));
            }
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

        private void FormCustomWave_Load(object sender, EventArgs e)
        {
            if (myParent.SynthGenerator.CurrentWave.WaveFormData.Length < SynthGenerator.NUM_CUSTOMWAVE_POINTS)
            {
                for (int i = 0; i < WaveData.Length; i++)
                {
                    WaveData[i] = SynthGenerator.CUSTOMWAVE_MAX_VALUE;
                }
            }
            else
            {
                for (int i = 0; i < WaveData.Length; i++)
                {
                    WaveData[i] = myParent.SynthGenerator.CurrentWave.WaveFormData[i] + SynthGenerator.CUSTOMWAVE_MAX_VALUE;
                }
            }

            pictureBoxCustomWave.Paint += new PaintEventHandler(PictureBoxPaint);
            pictureBoxCustomWave.Refresh();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            myParent.SynthGenerator.CurrentWave.WaveFormData = new int[SynthGenerator.NUM_CUSTOMWAVE_POINTS];
            for (int i = 0; i < WaveData.Length; i++)
            {
               myParent.SynthGenerator.CurrentWave.WaveFormData[i] = WaveData[i] - SynthGenerator.CUSTOMWAVE_MAX_VALUE;
            }
            myParent.pictureBoxCustomWave.Refresh();
            myParent.UpdateCurrentWaveInfo();

            Close();
        }

        private void pictureBoxCustomWave_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.X >= 0 && e.X < SynthGenerator.NUM_CUSTOMWAVE_POINTS)
            {
                WaveData[e.X] = e.Y;
                Refresh();
                previousPoint.X = e.X;
                previousPoint.Y = e.Y;
                isMouseButtonDown = true;
            }
        }

        private void pictureBoxCustomWave_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseButtonDown && e.X!=previousPoint.X && e.X >= 0 && e.X < SynthGenerator.NUM_CUSTOMWAVE_POINTS)
            {
                EditData(e.X, e.Y);
                previousPoint.X = e.X;
                previousPoint.Y = e.Y;
                Refresh();
            }
        }

        // Fill all data from previous point to current point with interpolated values
        private void EditData(int X, int Y)
        {
            double increment = (previousPoint.Y-Y) / Math.Abs(X-previousPoint.X);
            int position = X;
            double value = Y;
            while (position != previousPoint.X)
            {
                WaveData[position] = (int)value;
                value += increment;
                if ( X>previousPoint.X)
                {
                    position--;
                }
                else
                {
                    position++;
                }
            }
        }

        private void pictureBoxCustomWave_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseButtonDown = false;
        }

        private void buttonSine_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length; i++)
            {
                WaveData[i] = (int)(Math.Sin(i / (double)WaveData.Length * 2 * Math.PI) * SynthGenerator.CUSTOMWAVE_MAX_VALUE + SynthGenerator.CUSTOMWAVE_MAX_VALUE);
            }
            Refresh();
        }

        private void buttonFlat_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length; i++)
            {
                WaveData[i] = SynthGenerator.CUSTOMWAVE_MAX_VALUE;
            }
            Refresh();
        }

        private void buttonIncreasing_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length; i++)
            {
                WaveData[i] = (int)((SynthGenerator.NUM_CUSTOMWAVE_POINTS-i) / (double)SynthGenerator.NUM_CUSTOMWAVE_POINTS * 2 * SynthGenerator.CUSTOMWAVE_MAX_VALUE);
            }
            Refresh();
        }

        private void buttonDecreasing_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length; i++)
            {
                WaveData[i] = (int)(i / (double)SynthGenerator.NUM_CUSTOMWAVE_POINTS * 2 * SynthGenerator.CUSTOMWAVE_MAX_VALUE);
            }
            Refresh();
        }
    }
}
