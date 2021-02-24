using SynthAnvil.Synth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Windows.Forms;

namespace SynthAnvil
{
    public partial class FormFileToWaves : Form
    {
        int fftWindow;
        int numSamples;
        Complex[] frequencySpectrum;
        List<FrequencyAmount> frequencies;
        List<WaveInfo> waves;
        double[] tempData;
        decimal maxDifference;
        decimal difference;
        Timer timerGenerateWavesJob = new Timer();

        private FormMain myParent = null;

        public FormMain MyParent { get => myParent; set => myParent = value; }

        public FormFileToWaves()
        {
            InitializeComponent();
            timerGenerateWavesJob.Interval = 50;
            timerGenerateWavesJob.Tick += new EventHandler(TimerEventProcessor);
        }

        private void TimerEventProcessor(Object myObject,
                           EventArgs myEventArgs)
        {
            RunOptimization();
        }

        private void RunOptimization()
        {
            string[] best_volume_shape = new string[waves.Count];
            string[] best_frequency_shape = new string[waves.Count];

            // try increasing volume ranges
            for (int wave_number = 0; wave_number < waves.Count; wave_number++)
            {
                WaveInfo wave = waves[wave_number];
                Shapes.IncreasingLineair(wave.ShapeVolume);
                bool improved = true;

                while (improved)
                {
                    wave.MinVolume++;
                    myParent.SynthGenerator.RefreshWaveData(wave);
                    myParent.SynthGenerator.MixWaves(tempData, waves);
                    decimal new_difference = CountDifference();
                    if (new_difference < difference)
                    {
                        difference = new_difference;
                        best_frequency_shape[wave_number] = "increasing";
                    }
                    else
                    {
                        wave.MinVolume--;     // revert
                        improved = false;
                    }
                }

                while (improved)
                {
                    wave.MaxVolume--;
                    myParent.SynthGenerator.RefreshWaveData(wave);
                    myParent.SynthGenerator.MixWaves(tempData, waves);
                    decimal new_difference = CountDifference();
                    if (new_difference < difference)
                    {
                        difference = new_difference;
                        best_frequency_shape[wave_number] = "increasing";
                    }
                    else
                    {
                        wave.MaxVolume++;     // revert
                        improved = false;
                    }
                }
            }

            // try decreasing volume ranges
            for (int wave_number = 0; wave_number < waves.Count; wave_number++)
            {
                WaveInfo wave = waves[wave_number];
                Shapes.DecreasingLineair(wave.ShapeVolume);
                bool improved = true;

                while (improved)
                {
                    wave.MinVolume++;
                    myParent.SynthGenerator.RefreshWaveData(wave);
                    myParent.SynthGenerator.MixWaves(tempData, waves);
                    decimal new_difference = CountDifference();
                    if (new_difference < difference)
                    {
                        difference = new_difference;
                        best_volume_shape[wave_number] = "decreasing";
                    }
                    else
                    {
                        wave.MinVolume--;     // revert
                        improved = false;
                    }
                }

                while (improved)
                {
                    wave.MaxVolume--;
                    myParent.SynthGenerator.RefreshWaveData(wave);
                    myParent.SynthGenerator.MixWaves(tempData, waves);
                    decimal new_difference = CountDifference();
                    if (new_difference < difference)
                    {
                        difference = new_difference;
                        best_volume_shape[wave_number] = "decreasing";
                    }
                    else
                    {
                        wave.MaxVolume++;     // revert
                        improved = false;
                    }
                }
            }

            // try decreasing frequencies
            for (int wave_number = 0; wave_number < waves.Count; wave_number++)
            {
                WaveInfo wave = waves[wave_number];
                Shapes.DecreasingLineair(wave.ShapeFrequency);
                double decrement = 0.01;
                bool improved = true;
                while (improved)
                {
                    wave.MinFrequency -= decrement;
                    if (wave.MinFrequency<0.001)
                    {
                        wave.MinFrequency = 0.001;
                    }
                    myParent.SynthGenerator.RefreshWaveData(wave);
                    myParent.SynthGenerator.MixWaves(tempData, waves);
                    decimal new_difference = CountDifference();
                    if (new_difference < difference)
                    {
                        difference = new_difference;
                        best_frequency_shape[wave_number] = "decreasing";
                    }
                    else
                    {
                        wave.MinFrequency += decrement;     // revert
                        improved = false;
                    }
                    decrement *= 2;
                }

                double increment = 0.01;
                improved = true;
                while (improved)
                {
                    wave.MaxFrequency += increment;
                    if (wave.MaxFrequency > SynthGenerator.MAX_FREQUENCY)
                    {
                        wave.MaxFrequency = SynthGenerator.MAX_FREQUENCY;
                    }
                    myParent.SynthGenerator.RefreshWaveData(wave);
                    myParent.SynthGenerator.MixWaves(tempData, waves);
                    decimal new_difference = CountDifference();
                    if (new_difference < difference)
                    {
                        difference = new_difference;
                        best_frequency_shape[wave_number] = "decreasing";
                    }
                    else
                    {
                        wave.MaxFrequency -= increment;     // revert
                        improved = false;
                    }
                    increment *= 2;
                }
            }


            // try increasing frequencies
            for (int wave_number = 0; wave_number<waves.Count; wave_number++)
            {
                WaveInfo wave = waves[wave_number];
                Shapes.IncreasingLineair(wave.ShapeFrequency);
                double decrement = 0.01;
                bool improved = true;
                while (improved)
                {
                    wave.MinFrequency -= decrement;
                    if (wave.MinFrequency < 0.001)
                    {
                        wave.MinFrequency = 0.001;
                    }
                    myParent.SynthGenerator.RefreshWaveData(wave);
                    myParent.SynthGenerator.MixWaves(tempData, waves);
                    decimal new_difference = CountDifference();
                    if (new_difference < difference)
                    {
                        difference = new_difference;
                        best_frequency_shape[wave_number] = "increasing";
                    }
                    else
                    {
                        wave.MinFrequency += decrement;     // revert
                        improved = false;
                    }
                    decrement *= 2;
                }

                double increment = 0.01;
                improved = true;
                while (improved)
                {
                    wave.MaxFrequency += increment;
                    if (wave.MaxFrequency > SynthGenerator.MAX_FREQUENCY)
                    {
                        wave.MaxFrequency = SynthGenerator.MAX_FREQUENCY;
                    }
                    myParent.SynthGenerator.RefreshWaveData(wave);
                    myParent.SynthGenerator.MixWaves(tempData, waves);
                    decimal new_difference = CountDifference();
                    if (new_difference < difference)
                    {
                        difference = new_difference;
                        best_frequency_shape[wave_number] = "increasing";
                    }
                    else
                    {
                        wave.MaxFrequency -= increment;     // revert
                        improved = false;
                    }
                    increment *= 2;
                }
            }

            // Apply best shapes
            for (int wave_number = 0; wave_number < waves.Count; wave_number++)
            {
                WaveInfo wave = waves[wave_number];
                if (best_frequency_shape[wave_number] == "increasing")
                {
                    Shapes.IncreasingLineair(wave.ShapeFrequency);
                }
                if (best_frequency_shape[wave_number] == "decreasing")
                {
                    Shapes.DecreasingLineair(wave.ShapeFrequency);
                }
                if (best_volume_shape[wave_number] == "increasing")
                {
                    Shapes.IncreasingLineair(wave.ShapeVolume);
                }
                if (best_volume_shape[wave_number] == "decreasing")
                {
                    Shapes.DecreasingLineair(wave.ShapeVolume);
                }
            }

            UpdateStatus();

            // Done!
            buttonGenerateWaves.Text = "Generate wave data";
            timerGenerateWavesJob.Enabled = false;
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

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonGenerateWaves_Click(object sender, EventArgs e)
        {
            if (buttonGenerateWaves.Text.Equals("Generate wave data"))
            {
                buttonGenerateWaves.Text = "Pause matching job";
                if (waves == null)
                {
                    maxDifference = (decimal)(myParent.SynthGenerator.CurrentWave.WaveData.Length * SynthGenerator.MAX_AMPLITUDE * 2.0);
                    CreateSineWaves();
                }
                timerGenerateWavesJob.Enabled = true;
            }
            else
            {
                buttonGenerateWaves.Text = "Generate wave data";
                timerGenerateWavesJob.Enabled = false;
            }
        }

        private void UpdateTargetGraph()
        {
            chartTarget.Series["Series1"].Points.Clear();

            int numPoints = myParent.SynthGenerator.CurrentWave.WaveData.Length/2;

            for (int i = 0; i < numPoints; i++)
            {
                chartTarget.Series["Series1"].Points.AddY(myParent.SynthGenerator.CurrentWave.WaveData[i*2]);
            }
        }

        private void UpdateCurrentGraph()
        {
            chartCurrent.Series["Series1"].Points.Clear();

            int numPoints = tempData.Length / 2;

            for (int i = 0; i < numPoints; i++)
            {
                chartCurrent.Series["Series1"].Points.AddY(tempData[i * 2]);
            }
        }

        private void CreateSineWaves()
        {
            waves = new List<WaveInfo>();

            // determine FFT window size
            fftWindow = 256;
            while (fftWindow < myParent.SynthGenerator.CurrentWave.WaveFileData.Length)
            {
                fftWindow *= 2;
            }

            numSamples = myParent.SynthGenerator.TempData.Length / 2;
            frequencySpectrum = new Complex[fftWindow];
            for (int i = 0; i < fftWindow; i++)
            {
                if (2 * i > numSamples)
                {
                    frequencySpectrum[i] = new Complex(0, 0);
                }
                else
                {
                    frequencySpectrum[i] = new Complex(myParent.SynthGenerator.TempData[2 * i], 0);
                }
            }

            MathUtils.FFT.Transform(frequencySpectrum);
            CreateFrequencyList();

            for (int wave_number=0; wave_number<numericUpDownNumWaves.Value; wave_number++)
            {
                WaveInfo newWave = new WaveInfo(myParent.SynthGenerator.SamplesPerSecond);
                newWave.MinFrequency = frequencies[wave_number].Frequency;
                newWave.MaxFrequency = frequencies[wave_number].Frequency;
                newWave.WaveData = new double[myParent.SynthGenerator.CurrentWave.WaveData.Length];
                myParent.SynthGenerator.RefreshWaveData(newWave);
                waves.Add(newWave);
            }

            myParent.SynthGenerator.MixWaves(tempData, waves);
            difference = CountDifference();
        }

        private void UpdateStatus()
        {
            UpdateCurrentGraph();
            double differencePercentage = 100*(double)(difference / maxDifference);
            labelDifference.Text = "Difference (%): " + differencePercentage.ToString("0.00000");
            Refresh();
        }

        private decimal CountDifference()
        {
            decimal difference = 0;
            for(int i=0; i<tempData.Length; i++)
            {
                difference = difference + (decimal)Math.Abs(tempData[i]-myParent.SynthGenerator.CurrentWave.WaveData[i]);
            }
            return difference;
        }

        private double PointToFrequency(int pointNumber, int numPoints)
        {
            return (22050 * pointNumber / numPoints);
        }

        private void CreateFrequencyList()
        {
            frequencies = new List<FrequencyAmount>();
            int numPoints = fftWindow / 2;
            for (int i = 0; i < numPoints; i++)
            {
                frequencies.Add(new FrequencyAmount(PointToFrequency(i+1, numPoints), Math.Abs(frequencySpectrum[i].Real)));
            }
            frequencies = frequencies.OrderByDescending(o=>o.Amount).ToList();
        }

        private void FormFileToWaves_Load(object sender, EventArgs e)
        {            
            tempData = new double[myParent.SynthGenerator.CurrentWave.WaveData.Length];

            UpdateTargetGraph();
        }

        private void buttonCancel_Click_1(object sender, EventArgs e)
        {
            timerGenerateWavesJob.Enabled = false;
            Close();
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            timerGenerateWavesJob.Enabled = false;
            if (waves != null)
            {
                foreach (WaveInfo wave in waves)
                {
                    wave.Name = myParent.SynthGenerator.CreateUniqueName();
                    myParent.SynthGenerator.Waves.Add(wave);
                }
                myParent.UpdateWaveControls();
                myParent.SynthGenerator.UpdateCurrentWaveData();
                myParent.RecreateWavesLists();
            }
            Close();
        }

        private void numericUpDownNumWaves_ValueChanged(object sender, EventArgs e)
        {
            waves = null;           // reset, so waves get generated again
        }
    }
}
