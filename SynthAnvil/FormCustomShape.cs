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
    public partial class FormCustomShape : Form
    {
        private FormMain myParent = null;
        private int[] WaveData = new int[SynthGenerator.SHAPE_WAVE_NUMPOINTS];
        bool isMouseButtonDown = false;
        Point previousPoint;
        Random random = new Random();
        Timer aTimer = new Timer();
        int AdjustDataWidth = 0;


        public FormCustomShape()
        {
            InitializeComponent();
            aTimer.Interval = 50;
            aTimer.Tick += new EventHandler(TimerEventProcessor);
        }

        public FormMain MyParent { get => myParent; set => myParent = value; }

        private void PictureBoxPaint(object sender, PaintEventArgs e)
        {
            Control control = (Control)sender;

            using (LinearGradientBrush brush = new LinearGradientBrush(control.ClientRectangle,
                                                                       Color.FromArgb(195, 70, 70),
                                                                       Color.FromArgb(15, 0, 0),
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

        private void FormCustomWave_Load(object sender, EventArgs e)
        {
            if (myParent.SynthGenerator.CurrentWave.ShapeWave.Length < SynthGenerator.SHAPE_WAVE_NUMPOINTS)
            {
                for (int i = 0; i < WaveData.Length; i++)
                {
                    WaveData[i] = SynthGenerator.SHAPE_WAVE_MAX_VALUE;
                }
            }
            else
            {
                for (int i = 0; i < WaveData.Length; i++)
                {
                    WaveData[i] = myParent.SynthGenerator.CurrentWave.ShapeWave[i] + SynthGenerator.SHAPE_WAVE_MAX_VALUE;
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
            myParent.SynthGenerator.CurrentWave.ShapeWave = new int[SynthGenerator.SHAPE_WAVE_NUMPOINTS];
            for (int i = 0; i < WaveData.Length; i++)
            {
               myParent.SynthGenerator.CurrentWave.ShapeWave[i] = WaveData[i] - SynthGenerator.SHAPE_WAVE_MAX_VALUE;
            }
            myParent.pictureBoxCustomWave.Refresh();
            myParent.GenerateSound();

            Close();
        }

        private void pictureBoxCustomWave_MouseDown(object sender, MouseEventArgs e)
        {
            if (!isMouseButtonDown && e.X >= 0 && e.X < SynthGenerator.SHAPE_WAVE_NUMPOINTS)
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
            if (isMouseButtonDown && e.X!=previousPoint.X && e.X >= 0 && e.X < SynthGenerator.SHAPE_WAVE_NUMPOINTS)
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

        private void buttonSine_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length; i++)
            {
                WaveData[i] = (int)(Math.Sin(i / (double)WaveData.Length * 2 * Math.PI) * SynthGenerator.SHAPE_WAVE_MAX_VALUE + SynthGenerator.SHAPE_WAVE_MAX_VALUE);
            }
            Refresh();
        }

        private void buttonFlat_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length; i++)
            {
                WaveData[i] = SynthGenerator.SHAPE_WAVE_MAX_VALUE;
            }
            Refresh();
        }

        private void buttonIncreasing_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length; i++)
            {
                WaveData[i] = (int)((SynthGenerator.SHAPE_WAVE_NUMPOINTS-i) / (double)SynthGenerator.SHAPE_WAVE_NUMPOINTS * 2 * SynthGenerator.SHAPE_WAVE_MAX_VALUE);
            }
            Refresh();
        }

        private void buttonDecreasing_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length; i++)
            {
                WaveData[i] = (int)(i / (double)SynthGenerator.SHAPE_WAVE_NUMPOINTS * 2 * SynthGenerator.SHAPE_WAVE_MAX_VALUE);
            }
            Refresh();
        }

        private void buttonRipples_Click(object sender, EventArgs e)
        {
            int x_position = 0;
            while (x_position < WaveData.Length)
            {
                if (x_position < SynthGenerator.SHAPE_WAVE_NUMPOINTS - 5 && random.Next(30) == 11)
                {
                    int amplitude = SynthGenerator.SHAPE_WAVE_MAX_VALUE;
                    int up_down_spike = 1;
                    if (random.Next(100) < 50)
                    {
                        // down spike
                        up_down_spike = -1;
                    }
                    for (int j = 0; j < 5; j++)
                    {
                        WaveData[x_position] = SynthGenerator.SHAPE_WAVE_MAX_VALUE - ((int)(amplitude * Math.Sin(((j + 1) / 6.0) * Math.PI)) * up_down_spike);
                        x_position++;
                    }
                }
                else
                {
                    WaveData[x_position] = SynthGenerator.SHAPE_WAVE_MAX_VALUE;
                }
                x_position++;
            }
            Refresh();
        }

        private void buttonRandomRipples_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length; i++)
            {
                if (i< SynthGenerator.SHAPE_WAVE_NUMPOINTS-5 && random.Next(12) == 11)
                {
                    int amplitude = random.Next(SynthGenerator.SHAPE_WAVE_MAX_VALUE);
                    for (int j = 0; j < 5; j++)
                    {
                        if(random.Next(100)<50)
                        {
                            // down spike
                            WaveData[i] = SynthGenerator.SHAPE_WAVE_MAX_VALUE + ((int)(amplitude * Math.Sin(((j + 1) / 6.0) * Math.PI)));
                        }
                        else
                        {
                            // up spike
                            WaveData[i] = SynthGenerator.SHAPE_WAVE_MAX_VALUE - ((int)(amplitude * Math.Sin(((j + 1) / 6.0) * Math.PI)));
                        }
                        i++;
                    }
                }
                else
                {
                    WaveData[i] = SynthGenerator.SHAPE_WAVE_MAX_VALUE;
                }
            }
            Refresh();
        }

        private void buttonFlatHigh_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length; i++)
            {
                WaveData[i] = 0;
            }
            Refresh();
        }

        private void buttonFlatLow_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < WaveData.Length; i++)
            {
                WaveData[i] = 2 * SynthGenerator.SHAPE_WAVE_MAX_VALUE;
            }
            Refresh();
        }
    }
}
