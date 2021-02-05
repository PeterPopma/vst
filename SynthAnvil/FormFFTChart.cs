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
        Complex[] frequencySpectrum;
        double[] frequencies;
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
            return (int)(22050 * pointNumber / frequencies.Length);
        }

        private void UpdateFFTGraph()
        {
            chartFFT.Series["Series1"].Points.Clear();
            int numPoints = (int)(frequencies.Length / graphScale);
            int lastPoint = numPoints + graphPosition;

            for (int pointNumber = graphPosition; pointNumber < lastPoint; pointNumber++)
            {
                int frequency = PointToFrequency(pointNumber);

                chartFFT.Series["Series1"].Points.AddXY(frequency, frequencies[pointNumber]);
            }
            chartFFT.ChartAreas[0].AxisX.Minimum = PointToFrequency(graphPosition);
            chartFFT.ChartAreas[0].AxisX.Maximum = PointToFrequency(lastPoint);
            labelFrequencyRange.Text = PointToFrequency(graphPosition).ToString() + " - " + PointToFrequency(lastPoint).ToString() + " Hz.";
        }

        private void CalcFFT()
        {
            numSamples = myParent.SynthGenerator.TempData.Length / 2;
            frequencySpectrum = new Complex[fftWindow];
            for (int i = 0; i < fftWindow; i++)
            {
                if (2 * (startSample + i) > numSamples)
                {
                    frequencySpectrum[i] = new Complex(0, 0);
                }
                else
                {
                    frequencySpectrum[i] = new Complex(myParent.SynthGenerator.TempData[2*(startSample + i)], 0);
                }
            }

            MathUtils.FFT.Transform(frequencySpectrum);
            ToNormalizedFrequenciesArray();
        }

        private void ToNormalizedFrequenciesArray()
        {
            double max_value = 0;
            frequencies = new double[fftWindow/2];
            for (int i = 0; i < frequencies.Length; i++)
            {
                frequencies[i] = Math.Abs(frequencySpectrum[i].Real);
                if (frequencies[i] > max_value)
                {
                    max_value = frequencies[i];
                }
            }

            if (max_value == 0)
            {
                return;
            }

            double scale_factor = 100 / max_value;
            for (int i = 0; i < frequencies.Length; i++)
            {
                frequencies[i] *= scale_factor;
                if (frequencies[i] >= 100)     // rounding errors
                {
                    frequencies[i] = 99.999;
                }
            }
            
        }

        private void FormFFT_Load(object sender, EventArgs e)
        {
            chartFFT.Cursor = new Cursor(Properties.Resources.magnifying_glass.Handle);
            comboBoxFFTWindow.SelectedIndex = 2;
            UpdateFFT();
        }

        private void UpdateFFT()
        {
            labelFFTPeriod.Text = string.Format("{0:0.00}", startSample / 44100.0) + " s - " + string.Format("{0:0.00}", (startSample + fftWindow) / 44100.0) + " s";
            CalcFFT();
            UpdateFFTGraph();
        }

        private void buttonNextFFTPeriod_Click(object sender, EventArgs e)
        {
            startSample += fftWindow;
            UpdateFFT();
        }

        private void buttonPreviousFFTPeriod_Click(object sender, EventArgs e)
        {
            startSample -= fftWindow;
            if(startSample<0)
            {
                startSample = 0;
            }
            UpdateFFT();
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

        private void chartFFT_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            double position = me.X / (double)chartFFT.Width;
            int numPoints = (int)(frequencies.Length / graphScale);
            int graph_center = (int)(graphPosition + position * numPoints);

            if (me.Button == System.Windows.Forms.MouseButtons.Left && graphScale < 1024)
            {
                graphScale *= 2;
            }
            if (me.Button == System.Windows.Forms.MouseButtons.Right && graphScale > 1)
            {
                graphScale /= 2;
            }

            graphPosition = (int)(graph_center - (frequencies.Length / graphScale / 2));

            if (graphPosition < 0)
            {
                graphPosition = 0;
            }
            if (graphPosition >= (frequencies.Length - (frequencies.Length / graphScale)))
            {
                graphPosition = (int)(frequencies.Length - (frequencies.Length / graphScale));
            }

            UpdateFFTGraph();

            labelScale.Text = graphScale.ToString();
        }

        private void buttonDecreaseFrequency_Click(object sender, EventArgs e)
        {
            graphPosition -= 2;
            if (graphPosition < 0)
            {
                graphPosition = 0;
            }
            UpdateFFTGraph();
        }

        private void buttonIncreaseFrequency_Click(object sender, EventArgs e)
        {
            graphPosition +=2;
            if (graphPosition > LastPossiblePosition())
            {
                graphPosition = LastPossiblePosition();
            }
            UpdateFFTGraph();
        }

        private void buttonDecreaseFrequencyMajor_Click(object sender, EventArgs e)
        {
            graphPosition -= (int)(frequencies.Length / 20.0);
            if (graphPosition < 0)
            {
                graphPosition = 0;
            }
            UpdateFFTGraph();
        }

        private int LastPossiblePosition()
        {
            return (int)(frequencies.Length - (frequencies.Length / graphScale));
        }

        private void buttonIncreaseFrequencyMajor_Click(object sender, EventArgs e)
        {
            graphPosition += (int)(frequencies.Length / 20.0);
            if (graphPosition > LastPossiblePosition())
            {
                graphPosition = LastPossiblePosition();
            }
            UpdateFFTGraph();
        }

        private void buttonDecreaseMiddle_Click(object sender, EventArgs e)
        {
            graphPosition -= 100;
            if (graphPosition < 0)
            {
                graphPosition = 0;
            }
            UpdateFFTGraph();
        }

        private void buttonIncreaseMiddle_Click(object sender, EventArgs e)
        {
            graphPosition += 100;
            if (graphPosition > LastPossiblePosition())
            {
                graphPosition = LastPossiblePosition();
            }
            UpdateFFTGraph();
        }
    }
}
