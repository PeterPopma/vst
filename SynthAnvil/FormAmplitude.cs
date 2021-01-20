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
    public partial class FormAmplitude : Form
    {
        private FormMain myParent = null;
        int startSample = 0;
        int timeWindow = 4096;
        short[] amplitudesLeft;
        short[] amplitudesRight;
        uint graphScale = 1;     // 1..1024 in steps of *2
        int graphPosition = 0;
        int channelMode = 1;
        private const int GRAPH_POINTS_PLOTTED = 5000;

        public FormAmplitude()
        {
            InitializeComponent();
        }

        public FormMain MyParent { get => myParent; set => myParent = value; }
        public int StartSample { get => startSample; set => startSample = value; }
        public int TimeWindow { get => timeWindow; set => timeWindow = value; }
        public uint GraphScale { get => graphScale; set => graphScale = value; }
        public int GraphPosition { get => graphPosition; set => graphPosition = value; }

        private void UpdateAmplitudeGraph()
        {
            chartAmplitude.Series["Series1"].Points.Clear();
            chartAmplitude.Series["Series2"].Points.Clear();

            int numPoints = (int)(amplitudesLeft.Length / graphScale);
            int lastPoint = numPoints + graphPosition;

            for (int i = 0; i < GRAPH_POINTS_PLOTTED; i++)
            {
                int currentPoint = (int)(graphPosition + numPoints * (i / (double)GRAPH_POINTS_PLOTTED));
                chartAmplitude.Series["Series1"].Points.AddXY(currentPoint / 44100.0, amplitudesLeft[currentPoint]);
                chartAmplitude.Series["Series2"].Points.AddXY(currentPoint / 44100.0, amplitudesRight[currentPoint]);
            }
            chartAmplitude.ChartAreas[0].AxisX.Minimum = graphPosition / 44100.0;
            chartAmplitude.ChartAreas[0].AxisX.Maximum = lastPoint / 44100.0;
            labelFrequencyRange.Text = string.Format("{0:0.000} s", (graphPosition / 44100.0)) + " - " + string.Format("{0:0.000} s", (lastPoint / 44100.0));
            
            chartAmplitude.Series["Series1"].Enabled = (channelMode == 0 || channelMode == 1);
            chartAmplitude.Series["Series2"].Enabled = (channelMode == 0 || channelMode == 2);
        }

        private void UpdateWaveData()
        {
            amplitudesLeft = new short[myParent.SynthGenerator.FinalData.shortArray.Length / 2];
            amplitudesRight = new short[myParent.SynthGenerator.FinalData.shortArray.Length / 2];

            for (int i = 0; i < myParent.SynthGenerator.FinalData.shortArray.Length; i ++)
            {
                if (i % 2 == 0)
                {
                    amplitudesLeft[i/2] = myParent.SynthGenerator.FinalData.shortArray[i];
                }
                else
                {
                    amplitudesRight[i/2] = myParent.SynthGenerator.FinalData.shortArray[i];
                }
            }
            labelDuration.Text = "Duration: " + string.Format("{0:0.000} s", (amplitudesLeft.Length / 44100.0));
        }

        private void FormAmplitude_Load(object sender, EventArgs e)
        {
            UpdateWaveData();
            chartAmplitude.Cursor = new Cursor(Properties.Resources.magnifying_glass.Handle);
            UpdateAmplitudeGraph();
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

        private void chartAmplitude_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            double position = me.X / (double)chartAmplitude.Width;
            int numPoints = (int)(amplitudesLeft.Length / graphScale);
            int graph_center = (int)(graphPosition + position * numPoints);
            
            if (me.Button == System.Windows.Forms.MouseButtons.Left && graphScale < 1024)
            {
                graphScale *= 2;
            }
            if (me.Button == System.Windows.Forms.MouseButtons.Right && graphScale > 1)
            {
                graphScale /= 2;
            }

            graphPosition = (int)(graph_center - (amplitudesLeft.Length / graphScale / 2));

            if (graphPosition < 0)
            {
                graphPosition = 0;
            }
            if (graphPosition >= (amplitudesLeft.Length - (amplitudesLeft.Length / graphScale)))
            {
                graphPosition = (int)(amplitudesLeft.Length - (amplitudesLeft.Length / graphScale));
            }

            UpdateAmplitudeGraph();

            labelScale.Text = graphScale.ToString();
        }

        private void buttonDecreaseFrequency_Click(object sender, EventArgs e)
        {
            graphPosition -= 2;
            if (graphPosition < 0)
            {
                graphPosition = 0;
            }
            UpdateAmplitudeGraph();
        }

        private void buttonIncreaseFrequency_Click(object sender, EventArgs e)
        {
            graphPosition +=2;
            if (graphPosition > LastPossiblePosition())
            {
                graphPosition = LastPossiblePosition();
            }
            UpdateAmplitudeGraph();
        }

        private void buttonDecreaseFrequencyMajor_Click(object sender, EventArgs e)
        {
            graphPosition -= (int)(amplitudesLeft.Length / 20.0);
            if (graphPosition < 0)
            {
                graphPosition = 0;
            }
            UpdateAmplitudeGraph();
        }

        private int LastPossiblePosition()
        {
            return (int)(amplitudesLeft.Length - (amplitudesLeft.Length / graphScale));
        }

        private void buttonIncreaseFrequencyMajor_Click(object sender, EventArgs e)
        {
            graphPosition += (int)(amplitudesLeft.Length / 20.0);
            if (graphPosition > LastPossiblePosition())
            {
                graphPosition = LastPossiblePosition();
            }
            UpdateAmplitudeGraph();
        }

        private void buttonDecreaseMiddle_Click(object sender, EventArgs e)
        {
            graphPosition -= 100;
            if (graphPosition < 0)
            {
                graphPosition = 0;
            }
            UpdateAmplitudeGraph();
        }

        private void buttonIncreaseMiddle_Click(object sender, EventArgs e)
        {
            graphPosition += 100;
            if (graphPosition > LastPossiblePosition())
            {
                graphPosition = LastPossiblePosition();
            }
            UpdateAmplitudeGraph();
        }

        private void radioButtonChannelBoth_CheckedChanged(object sender, EventArgs e)
        {
            channelMode = 0;
            UpdateAmplitudeGraph();
        }

        private void radioButtonChannelLeft_CheckedChanged(object sender, EventArgs e)
        {
            channelMode = 1;
            UpdateAmplitudeGraph();
        }

        private void radioButtonChannelRight_CheckedChanged(object sender, EventArgs e)
        {
            channelMode = 2;
            UpdateAmplitudeGraph();
        }

        private void FormAmplitude_FormClosing(object sender, FormClosingEventArgs e)
        {
            myParent.chartResultLeft.Cursor = new Cursor(Properties.Resources.magnifying_glass.Handle);
            myParent.chartResultRight.Cursor = new Cursor(Properties.Resources.magnifying_glass.Handle);
        }
    }
}
