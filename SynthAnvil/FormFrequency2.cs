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
    public partial class FormFrequency2 : Form
    {
        private const int SHAPE_NUM_POINTS = 1000;
        private const int SHAPE_MAX_VALUE = 500;

        private FormMain myParent = null;
        private int[] waveData = new int[SHAPE_NUM_POINTS];
        bool isMouseButtonDown = false;
        Point previousPoint;
        Timer aTimer = new Timer();
        int AdjustDataWidth = 0;
        Random random = new Random();

        public FormFrequency2()
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

        public double CalculateCurrentFactor(int position)
        {
            double min_change = Convert.ToDouble(textBoxChange1.Text);
            double max_change = Convert.ToDouble(textBoxChange2.Text);
            // note that graph is upside down
            return (max_change * (SHAPE_MAX_VALUE-waveData[position]) + min_change * waveData[position]) / SHAPE_MAX_VALUE;
        }

        private double CalculateCurrentFrequency(int position, WaveInfo waveInfo)
        {
            return (waveInfo.MaxFrequency * waveInfo.ShapeFrequency[position] + (waveInfo.MinFrequency * (SynthGenerator.SHAPE_MAX_VALUE - waveInfo.ShapeFrequency[position]))) / (double)SynthGenerator.SHAPE_MAX_VALUE;
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            int totalSamples = (int)myParent.SynthGenerator.NumSamples();
            double MinChange = Convert.ToDouble(labelChangeMin.Text);
            double MaxChange = Convert.ToDouble(labelChangeMax.Text);

            foreach (WaveInfo wave in myParent.SynthGenerator.Waves)
            {
                // change the freq. graph, so that it matches the desired pattern
                // if min = 0.1 and max = 1.8, minfreq. and maxfrex must be multiplied by this
                // every point in graph must be mapped to a point in desired pattern and 

                double new_min = wave.MinFrequency * MinChange;
                if(new_min > SynthGenerator.MAX_FREQUENCY)
                {
                    new_min = SynthGenerator.MAX_FREQUENCY;
                }
                double new_max = wave.MaxFrequency * MaxChange;
                if (new_max > SynthGenerator.MAX_FREQUENCY)
                {
                    new_max = SynthGenerator.MAX_FREQUENCY;
                }
                for (int position = 0; position<wave.ShapeFrequency.Length;  position++)
                {
                    int position_in_wavedata = (wave.NumSamples() * position / wave.ShapeFrequency.Length);
                    int desired_pattern_position = SHAPE_NUM_POINTS * (wave.StartPosition + position_in_wavedata) / totalSamples;
                    double factor = CalculateCurrentFactor(desired_pattern_position);
                    double freq = CalculateCurrentFrequency(position, wave);
                    freq *= factor;
                    wave.ShapeFrequency[position] = (int)(SynthGenerator.SHAPE_MAX_VALUE * ((freq - new_min) / (new_max - new_min)));
                }
                wave.MinFrequency = new_min;
                wave.MaxFrequency = new_max;
            }

            myParent.pictureBoxFrequencyShape.Refresh();
            myParent.UpdateWaveControls();
            myParent.SynthGenerator.UpdateAllWaveData();

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
            for (int i = 0; i < waveData.Length - 1; i++)
            {
                e.Graphics.DrawLine(pen, new Point(i, waveData[i]), new Point(i + 1, waveData[i + 1]));
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
                waveData[position] = (int)value;
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
            int begin_y = waveData[begin_x];
            int end_y = previousPoint.Y;

            // adjust all points left of mouse pointer
            while (x_position < end_x)
            {
                int interpolated_value = (((x_position - begin_x) * end_y) + ((end_x - x_position) * begin_y)) / (end_x - begin_x);
                waveData[x_position] = (waveData[x_position] + interpolated_value) / 2;
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
            end_y = waveData[end_x];

            // adjust all points right of mouse pointer
            while (x_position < end_x)
            {
                int interpolated_value = (((x_position - begin_x) * end_y) + ((end_x - x_position) * begin_y)) / (end_x - begin_x);
                waveData[x_position] = (waveData[x_position] + interpolated_value) / 2;
                x_position++;
            }

            Refresh();
        }

        private void FormFrequency_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < waveData.Length; i++)
            {
                waveData[i] = 250;
            }

            textBoxChange1.Text = labelChangeMin.Text = "1.000";
            textBoxChange2.Text = labelChangeMax.Text = "1.500";

            pictureBoxFrequencyShape.Paint += new PaintEventHandler(PictureBoxPaint);
            pictureBoxFrequencyShape.Refresh();
        }

        private void pictureBoxFrequencyShape_MouseDown(object sender, MouseEventArgs e)
        {
            if (!isMouseButtonDown && e.X >= 0 && e.X < SynthGenerator.SHAPE_NUMPOINTS)
            {
                waveData[e.X] = e.Y;
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
                double change = Convert.ToDouble(textBoxChange1.Text);
                if (change>0 && change<=1000)
                {
                    if (change<Convert.ToDouble(textBoxChange2.Text))
                    {
                        labelChangeMin.Text = change.ToString("0.000");
                    }
                    else
                    {
                        labelChangeMax.Text = change.ToString("0.000");
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < waveData.Length; i++)
            {
                waveData[i] = (int)(SynthGenerator.SHAPE_MAX_VALUE / 2.0);
            }
            Refresh();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < waveData.Length; i++)
            {
                waveData[i] = (int)((SynthGenerator.SHAPE_NUMPOINTS - i) / (double)SynthGenerator.SHAPE_NUMPOINTS * SynthGenerator.SHAPE_MAX_VALUE);
            }
            Refresh();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < waveData.Length; i++)
            {
                waveData[i] = (int)(i / (double)SynthGenerator.SHAPE_NUMPOINTS * SynthGenerator.SHAPE_MAX_VALUE);
            }
            Refresh();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            double factor = Math.Pow(SynthGenerator.SHAPE_MAX_VALUE, 1.0 / SynthGenerator.SHAPE_NUMPOINTS);
            for (int i = 0; i < waveData.Length; i++)
            {
                waveData[i] = SynthGenerator.SHAPE_MAX_VALUE - (int)Math.Pow(factor, i);
            }
            Refresh();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            double factor = Math.Pow(SynthGenerator.SHAPE_MAX_VALUE, 1.0 / SynthGenerator.SHAPE_NUMPOINTS);
            for (int i = 0; i < waveData.Length; i++)
            {
                waveData[i] = SynthGenerator.SHAPE_MAX_VALUE - (int)Math.Pow(factor, SynthGenerator.SHAPE_NUMPOINTS - i);
            }
            Refresh();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < waveData.Length / 2; i++)
            {
                waveData[i] = (int)(((SynthGenerator.SHAPE_NUMPOINTS - i * 2) / (double)SynthGenerator.SHAPE_NUMPOINTS) * SynthGenerator.SHAPE_MAX_VALUE);
            }
            for (int i = waveData.Length / 2; i < waveData.Length; i++)
            {
                waveData[i] = (int)(((i - waveData.Length / 2) * SynthGenerator.SHAPE_MAX_VALUE) / (waveData.Length / 2));
            }
            Refresh();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < waveData.Length / 2; i++)
            {
                waveData[i] = (int)(((i) * SynthGenerator.SHAPE_MAX_VALUE) / (waveData.Length / 2));
            }
            for (int i = waveData.Length / 2; i < waveData.Length; i++)
            {
                waveData[i] = (int)(((SynthGenerator.SHAPE_NUMPOINTS - (i - waveData.Length / 2) * 2) / (double)SynthGenerator.SHAPE_NUMPOINTS) * SynthGenerator.SHAPE_MAX_VALUE);
            }
            Refresh();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            int x_position = 0;
            while (x_position < waveData.Length)
            {
                if (x_position < SynthGenerator.SHAPE_NUMPOINTS - 5 && random.Next(15) == 11)
                {
                    int amplitude = random.Next(SynthGenerator.SHAPE_MAX_VALUE / 2);
                    int up_down_spike = 1;
                    if (random.Next(100) < 50)
                    {
                        // up spike
                        up_down_spike = -1;
                    }
                    for (int j = 0; j < 5; j++)
                    {
                        waveData[x_position] = SynthGenerator.SHAPE_MAX_VALUE / 2 - ((int)(amplitude * Math.Sin(((j + 1) / 6.0) * Math.PI)) * up_down_spike);
                        x_position++;
                    }
                }
                else
                {
                    waveData[x_position] = SynthGenerator.SHAPE_MAX_VALUE / 2;
                    x_position++;
                }
            }
            Refresh();
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < waveData.Length; i++)
                {
                    waveData[i] = (int)(((int)(Math.Sin(i / (double)waveData.Length * 2 * Convert.ToInt32(textBoxNumSines.Text) * Math.PI) * SynthGenerator.SHAPE_MAX_VALUE + SynthGenerator.SHAPE_MAX_VALUE)) / 2.0);
                }
            }
            catch (Exception)
            {
                // probably bad input from textbox; ignore
            }
            Refresh();
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < waveData.Length; i++)
            {
                double factor = Math.Pow(1.003, i);   // between 1 and 20
                waveData[i] = (int)(((int)(Math.Sin(i / (double)waveData.Length * factor * Math.PI) * SynthGenerator.SHAPE_MAX_VALUE + SynthGenerator.SHAPE_MAX_VALUE)) / 2.0);
            }
            Refresh();
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < waveData.Length; i++)
            {
                double factor = Math.Pow(1.003, i);   // between 1 and 20
                waveData[waveData.Length - 1 - i] = (int)(((int)(Math.Sin(i / (double)waveData.Length * factor * Math.PI) * SynthGenerator.SHAPE_MAX_VALUE + SynthGenerator.SHAPE_MAX_VALUE)) / 2.0);
            }
            Refresh();
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            int amplitude = random.Next(SynthGenerator.SHAPE_MAX_VALUE / 2);
            int period = random.Next(50) + 5;
            bool amplitude_increasing = false;
            for (int i = 0; i < waveData.Length; i++)
            {
                if (random.Next(20) == 3)
                {
                    amplitude_increasing = !amplitude_increasing;
                }
                if (amplitude_increasing)
                {
                    if (amplitude < SynthGenerator.SHAPE_MAX_VALUE / 2)
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
                waveData[i] = (int)((Math.Sin(i / (double)waveData.Length * period * Math.PI) * amplitude) + (SynthGenerator.SHAPE_MAX_VALUE / 2.0));
            }
            Refresh();
        }

        private void textBoxChange2_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                double change = Convert.ToDouble(textBoxChange2.Text);
                if (change > 0 && change <= 1000)
                {
                    if (change < Convert.ToDouble(textBoxChange1.Text))
                    {
                        labelChangeMin.Text = change.ToString("0.000");
                    }
                    else
                    {
                        labelChangeMax.Text = change.ToString("0.000");
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
