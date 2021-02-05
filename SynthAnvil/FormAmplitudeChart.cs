using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SynthAnvil
{
    public partial class FormAmplitudeChart : Form
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
        private const double SAMPLES_PER_SECOND = 44100.0;

        public FormAmplitudeChart()
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
                chartAmplitude.Series["Series1"].Points.AddXY(currentPoint / SAMPLES_PER_SECOND, amplitudesLeft[currentPoint]);
                chartAmplitude.Series["Series2"].Points.AddXY(currentPoint / SAMPLES_PER_SECOND, amplitudesRight[currentPoint]);
            }
            chartAmplitude.ChartAreas[0].AxisX.Minimum = graphPosition / SAMPLES_PER_SECOND;
            chartAmplitude.ChartAreas[0].AxisX.Maximum = lastPoint / SAMPLES_PER_SECOND;
            labelFrequencyRange.Text = string.Format("{0:0.000} s", (graphPosition / SAMPLES_PER_SECOND)) + " - " + string.Format("{0:0.000} s", (lastPoint / SAMPLES_PER_SECOND));
            
            chartAmplitude.Series["Series1"].Enabled = (channelMode == 0 || channelMode == 1);
            chartAmplitude.Series["Series2"].Enabled = (channelMode == 0 || channelMode == 2);
        }

        private void UpdateWaveData()
        {
            amplitudesLeft = new short[myParent.SynthGenerator.TempData.Length / 2];
            amplitudesRight = new short[myParent.SynthGenerator.TempData.Length / 2];

            for (int i = 0; i < myParent.SynthGenerator.TempData.Length; i ++)
            {
                if (i % 2 == 0)
                {
                    amplitudesLeft[i/2] = (short)myParent.SynthGenerator.TempData[i];
                }
                else
                {
                    amplitudesRight[i/2] = (short)myParent.SynthGenerator.TempData[i];
                }
            }
            labelDuration.Text = "Duration: " + string.Format("{0:0.000} s", (amplitudesLeft.Length / SAMPLES_PER_SECOND));
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

        private int LastPossiblePosition()
        {
            return (int)(amplitudesLeft.Length - (amplitudesLeft.Length / graphScale));
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

        private void gradientButton3_Click(object sender, EventArgs e)
        {
            graphPosition -= (int)(amplitudesLeft.Length / 20.0);
            if (graphPosition < 0)
            {
                graphPosition = 0;
            }
            UpdateAmplitudeGraph();
        }

        private void gradientButton4_Click(object sender, EventArgs e)
        {
            graphPosition -= 100;
            if (graphPosition < 0)
            {
                graphPosition = 0;
            }
            UpdateAmplitudeGraph();
        }

        private void gradientButton5_Click(object sender, EventArgs e)
        {
            graphPosition -= 2;
            if (graphPosition < 0)
            {
                graphPosition = 0;
            }
            UpdateAmplitudeGraph();
        }

        private void gradientButton6_Click(object sender, EventArgs e)
        {
            graphPosition += 2;
            if (graphPosition > LastPossiblePosition())
            {
                graphPosition = LastPossiblePosition();
            }
            UpdateAmplitudeGraph();
        }

        private void gradientButton7_Click(object sender, EventArgs e)
        {
            graphPosition += 100;
            if (graphPosition > LastPossiblePosition())
            {
                graphPosition = LastPossiblePosition();
            }
            UpdateAmplitudeGraph();
        }

        private void gradientButton8_Click(object sender, EventArgs e)
        {
            graphPosition += (int)(amplitudesLeft.Length / 20.0);
            if (graphPosition > LastPossiblePosition())
            {
                graphPosition = LastPossiblePosition();
            }
            UpdateAmplitudeGraph();
        }
    }
}
