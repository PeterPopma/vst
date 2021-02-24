using SynthAnvil.Synth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Numerics;
using System.Text;
using System.Windows.Forms;

namespace SynthAnvil
{
    public partial class FormFFTChart : Form
    {
        private FormMain myParent = null;
        int startSample = 0;
        int fftWindow = 4096;
        int numSamples;
        Complex[] frequencySpectrumLeft;
        double[] frequenciesLeft;
        uint graphScale = 1;     // 1..1024 in steps of *2
        int graphPosition = 0;

        public FormFFTChart()
        {
            InitializeComponent();
        }

        public FormMain MyParent { get => myParent; set => myParent = value; }
        public int StartSample { get => startSample; set => startSample = value; }
        public int FftWindow { get => fftWindow; set => fftWindow = value; }
        public uint GraphScale { get => graphScale; set => graphScale = value; }
        public int GraphPosition { get => graphPosition; set => graphPosition = value; }

        private int PointToFrequency(int pointNumber)
        {
            return (int)(22050 * pointNumber / frequenciesLeft.Length);
        }

        private void UpdateFFTGraph()
        {
            chartFFT.Series["Series1"].Points.Clear();
            int numPoints = (int)(frequenciesLeft.Length / graphScale);
            int lastPoint = numPoints + graphPosition;

            for (int pointNumber = graphPosition; pointNumber < lastPoint; pointNumber++)
            {
                int frequency = PointToFrequency(pointNumber);
                frequency = frequency * myParent.SynthGenerator.SamplesPerSecond / 44100;
                chartFFT.Series["Series1"].Points.AddXY(frequency, frequenciesLeft[pointNumber]);
            }
            chartFFT.ChartAreas[0].AxisX.Minimum = PointToFrequency(graphPosition);
            chartFFT.ChartAreas[0].AxisX.Maximum = PointToFrequency(lastPoint);
            labelFrequencyRange.Text = PointToFrequency(graphPosition).ToString() + " - " + PointToFrequency(lastPoint).ToString() + " Hz.";
        }

        private void CalcFFT()
        {
            numSamples = myParent.SynthGenerator.TempData.Length / 2;
            frequencySpectrumLeft = new Complex[fftWindow];
            for (int i = 0; i < fftWindow; i++)
            {
                if (2 * (startSample + i) > numSamples)
                {
                    frequencySpectrumLeft[i] = new Complex(0, 0);
                }
                else
                {
                    // TODO : use also second channel here when asked
                    frequencySpectrumLeft[i] = new Complex(myParent.SynthGenerator.TempData[2*(startSample + i)], 0);
                }
            }

            MathUtils.FFT.Transform(frequencySpectrumLeft);
            ToNormalizedFrequenciesArray();
        }

        private void ToNormalizedFrequenciesArray()
        {
            double max_value = 0;
            frequenciesLeft = new double[fftWindow/2];
            for (int i = 0; i < frequenciesLeft.Length; i++)
            {
                frequenciesLeft[i] = Math.Abs(frequencySpectrumLeft[i].Real);
                if (frequenciesLeft[i] > max_value)
                {
                    max_value = frequenciesLeft[i];
                }
            }

            if (max_value == 0)
            {
                return;
            }

            double scale_factor = 100 / max_value;
            for (int i = 0; i < frequenciesLeft.Length; i++)
            {
                frequenciesLeft[i] *= scale_factor;
                if (frequenciesLeft[i] >= 100)     // rounding errors
                {
                    frequenciesLeft[i] = 99.999;
                }
            }
        }

        private void FormFFT_Load(object sender, EventArgs e)
        {
            chartFFT.Cursor = new Cursor(Properties.Resources.magnifying_glass.Handle);
            comboBoxFFTWindow.SelectedIndex = 5;
            UpdateFFT();
        }

        private void UpdateFFT()
        {
            labelFFTPeriod.Text = string.Format("{0:0.00}", startSample / (double)myParent.SynthGenerator.SamplesPerSecond) + " s - " + string.Format("{0:0.00}", (startSample + fftWindow) / (double)myParent.SynthGenerator.SamplesPerSecond) + " s";
            CalcFFT();
            LimitGraphPosition();
            UpdateFFTGraph();
        }

        private void comboBoxFFTWindow_SelectedIndexChanged(object sender, EventArgs e)
        {
            FftWindow = Convert.ToInt32(comboBoxFFTWindow.SelectedItem);
            UpdateFFT();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (this.ClientRectangle.Width == 0 || this.ClientRectangle.Height == 0)
            {
                return;
            }
            using (LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle,
                                                                       Color.FromArgb(70, 77, 95),
                                                                       Color.Black,
                                                                       90F))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }

        private void LimitGraphPosition()
        {
            if (graphPosition < 0)
            {
                graphPosition = 0;
            }
            if (graphPosition >= (frequenciesLeft.Length - (frequenciesLeft.Length / graphScale)))
            {
                graphPosition = (int)(frequenciesLeft.Length - (frequenciesLeft.Length / graphScale));
            }
        }

        private void chartFFT_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            double position = me.X / (double)chartFFT.Width;
            int numPoints = (int)(frequenciesLeft.Length / graphScale);
            int graph_center = (int)(graphPosition + position * numPoints);

            if (me.Button == System.Windows.Forms.MouseButtons.Left && graphScale < 1024)
            {
                graphScale *= 2;
            }
            if (me.Button == System.Windows.Forms.MouseButtons.Right && graphScale > 1)
            {
                graphScale /= 2;
            }

            graphPosition = (int)(graph_center - (frequenciesLeft.Length / graphScale / 2));
            
            LimitGraphPosition();
            UpdateFFTGraph();

            labelScale.Text = graphScale.ToString();
        }

        private int LastPossiblePosition()
        {
            return (int)(frequenciesLeft.Length - (frequenciesLeft.Length / graphScale));
        }

        private void gradientButton2_Click(object sender, EventArgs e)
        {
            startSample -= fftWindow;
            if (startSample < 0)
            {
                startSample = 0;
            }
            UpdateFFT();
        }

        private void gradientButton1_Click(object sender, EventArgs e)
        {
            startSample += fftWindow;
            UpdateFFT();
        }

        private void gradientButton3_Click(object sender, EventArgs e)
        {
            graphPosition -= (int)(frequenciesLeft.Length / 20.0);
            if (graphPosition < 0)
            {
                graphPosition = 0;
            }
            UpdateFFTGraph();
        }

        private void gradientButton4_Click(object sender, EventArgs e)
        {
            graphPosition -= 100;
            if (graphPosition < 0)
            {
                graphPosition = 0;
            }
            UpdateFFTGraph();
        }

        private void gradientButton5_Click(object sender, EventArgs e)
        {
            graphPosition -= 2;
            if (graphPosition < 0)
            {
                graphPosition = 0;
            }
            UpdateFFTGraph();
        }

        private void gradientButton6_Click(object sender, EventArgs e)
        {
            graphPosition += 2;
            if (graphPosition > LastPossiblePosition())
            {
                graphPosition = LastPossiblePosition();
            }
            UpdateFFTGraph();
        }

        private void gradientButton7_Click(object sender, EventArgs e)
        {
            graphPosition += 100;
            if (graphPosition > LastPossiblePosition())
            {
                graphPosition = LastPossiblePosition();
            }
            UpdateFFTGraph();
        }

        private void gradientButton8_Click(object sender, EventArgs e)
        {
            graphPosition += (int)(frequenciesLeft.Length / 20.0);
            if (graphPosition > LastPossiblePosition())
            {
                graphPosition = LastPossiblePosition();
            }
            UpdateFFTGraph();
        }
    }
}
