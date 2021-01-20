using SynthAnvil.Synth;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace SynthAnvil
{
    public partial class FormMain : Form
    {
        Timer aTimer = new Timer();
        bool generatorEnabled = false;

        SynthGenerator synthGenerator;
        Preset preset = new Preset();
        string currentPreset = "";

        internal SynthGenerator SynthGenerator { get => synthGenerator; set => synthGenerator = value; }

        public FormMain()
        {
            InitializeComponent();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if(this.ClientRectangle.Width==0 || this.ClientRectangle.Height == 0)
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
        private void GroupBoxPaint(object sender, PaintEventArgs e)
        {
            Control control = (Control)sender;

            using (LinearGradientBrush brush = new LinearGradientBrush(control.ClientRectangle,
                                                                       Color.Black,
                                                                       Color.FromArgb(70, 77, 95),
                                                                       90F))
            {
                e.Graphics.FillRectangle(brush, control.ClientRectangle);
                ControlPaint.DrawBorder(e.Graphics, control.ClientRectangle, Color.Gray, ButtonBorderStyle.Solid);
            }
        }

        private void UpdateWaveFormPicture()
        {
            switch (synthGenerator.CurrentWave.WaveForm)
            {
                case "Sine":
                    pictureBoxWaveForm.Image = Properties.Resources.sine;
                    break;
                case "Square":
                    pictureBoxWaveForm.Image = Properties.Resources.square;
                    break;
                case "Triangle":
                    pictureBoxWaveForm.Image = Properties.Resources.triangle;
                    break;
                case "Sawtooth":
                    pictureBoxWaveForm.Image = Properties.Resources.sawtooth;
                    break;
                case "Noise":
                    pictureBoxWaveForm.Image = Properties.Resources.random;
                    break;
                case "File (.wav)":
                    pictureBoxWaveForm.Image = Properties.Resources.wav;
                    break;
            }
        }
        public void UpdateGeneratorControls()
        {
            generatorEnabled = false;

            colorSliderDuration1.Value = (decimal)(synthGenerator.CurrentWave.NumSamples / 441.0);
            colorSliderBeginFrequency1.Value = convertFrequencyToValue(synthGenerator.CurrentWave.BeginFrequency);
            colorSliderEndFrequency1.Value = convertFrequencyToValue(synthGenerator.CurrentWave.EndFrequency);
            colorSliderBeginVolume1.Value = synthGenerator.CurrentWave.BeginVolume;
            colorSliderEndVolume1.Value = synthGenerator.CurrentWave.EndVolume;
            colorSliderDuration1.Value = synthGenerator.CurrentWave.NumSamples / 441;
            colorSliderDelay1.Value = synthGenerator.CurrentWave.StartPosition / 441;
            checkBoxEndVolume1.Checked = synthGenerator.CurrentWave.EndVolumeEnabled;
            checkBoxEndFrequency1.Checked = synthGenerator.CurrentWave.EndFrequencyEnabled;
            colorSliderWeight1.Value = synthGenerator.CurrentWave.Weight;
            checkBoxBeginEndBeginVolume1.Checked = synthGenerator.CurrentWave.BeginEndBeginVolumeEnabled;
            checkBoxBeginEndBeginFrequency1.Checked = synthGenerator.CurrentWave.BeginEndBeginFrequencyEnabled;

            labelWaveForm.Text = synthGenerator.CurrentWave.WaveForm;
            UpdateWaveFormPicture();

            colorSliderAttack.Value = (decimal)synthGenerator.EnvelopAttack * 100;
            colorSliderHold.Value = (decimal)synthGenerator.EnvelopHold * 100;
            colorSliderDecay.Value = (decimal)synthGenerator.EnvelopDecay * 100;
            colorSliderSustain.Value = (decimal)synthGenerator.EnvelopSustain * 100;
            colorSliderRelease.Value = (decimal)synthGenerator.EnvelopRelease * 100;

            generatorEnabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            groupBox1.Paint += new PaintEventHandler(GroupBoxPaint);
            groupBox1.Refresh();

            groupBox2.Paint += new PaintEventHandler(GroupBoxPaint);
            groupBox2.Refresh();

            groupBox3.Paint += new PaintEventHandler(GroupBoxPaint);
            groupBox3.Refresh();
            
            groupBox5.Paint += new PaintEventHandler(GroupBoxPaint);
            groupBox5.Refresh();

            groupBox6.Paint += new PaintEventHandler(GroupBoxPaint);
            groupBox6.Refresh();

            synthGenerator = new SynthGenerator(this);

            // Init generators
            synthGenerator.Waves.Add(new WaveInfo());
            synthGenerator.CurrentWave = synthGenerator.Waves[0];

            UpdateGeneratorControls();

            chartResultLeft.Cursor = new Cursor(Properties.Resources.magnifying_glass.Handle);
            chartResultRight.Cursor = new Cursor(Properties.Resources.magnifying_glass.Handle);

            chartAHDSR.ChartAreas[0].AxisY.Minimum = 0;
            chartAHDSR.ChartAreas[0].AxisY.Maximum = 1;
            chartAHDSR.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
            chartAHDSR.ChartAreas[0].AxisY.LabelStyle.Enabled = false;

            chartCurrentWave.ChartAreas[0].AxisY.Maximum = 40000;
            chartCurrentWave.ChartAreas[0].AxisY.Minimum = -40000;
            chartCurrentWave.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
            chartCurrentWave.ChartAreas[0].AxisY.LabelStyle.Enabled = false;

            chartResultLeft.ChartAreas[0].AxisY.Maximum = 40000;
            chartResultLeft.ChartAreas[0].AxisY.Minimum = -40000;
            chartResultLeft.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
            chartResultLeft.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
            chartResultRight.ChartAreas[0].AxisY.Maximum = 40000;
            chartResultRight.ChartAreas[0].AxisY.Minimum = -40000;
            chartResultRight.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
            chartResultRight.ChartAreas[0].AxisY.LabelStyle.Enabled = false;

            generatorEnabled = false;


            // duration has been lost because of loading wave files.
            colorSliderDuration1.Value = 100;

            generatorEnabled = true;

            // find all presets
            try
            {
                string[] fileEntries = Directory.GetFiles("presets\\");
                foreach (string fileName in fileEntries)
                {
                    if (fileName.EndsWith(".pst"))
                    {
                        comboBoxPresets.Items.Add(fileName.Substring(8, fileName.Length - 12));
                    }
                }
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                Directory.CreateDirectory("presets");
            }
            if (comboBoxPresets.Items.Count > 0)
            {
                comboBoxPresets.SelectedIndex = 0;
                currentPreset = comboBoxPresets.Items[0].ToString();
            }

            CreateSound();
        }

        private int GetChannel(int generatorNumber)
        {
            int channel = 2;
            Label label = (Label)this.Controls.Find("labelChannel" + generatorNumber, true)[0];

            if (label.Text.Equals("left"))
            {
                channel = 0;
            }
            if (label.Text.Equals("right"))
            {
                channel = 1;
            }

            return channel;
        }

        public void CreateSound()
        {
            if (generatorEnabled)
            {
                synthGenerator.GenerateSound();
            }
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            synthGenerator.Play();
        }

        private void TimerEventProcessor(Object myObject,
                                                   EventArgs myEventArgs)
        {
            synthGenerator.Play();
        }

        private void buttonPlayContinuous_Click(object sender, EventArgs e)
        {
            if (buttonPlayContinuous.Text.Equals("Play Continuous"))
            {
                buttonPlayContinuous.Text = "Stop playing";
                aTimer.Interval = 2000;
                aTimer.Tick += new EventHandler(TimerEventProcessor);
                aTimer.Enabled = true;
            }
            else
            {
                buttonPlayContinuous.Text = "Play Continuous";
                aTimer.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            synthGenerator.Play(1);
            // generate mixed sound, so we can play this without delay
            synthGenerator.GenerateSound();
        }

        private float convertValueToFrequency(decimal value)
        {
            return (float)(Math.Pow((double)value, 1.27)/100.0);
        }

        private decimal convertFrequencyToValue(double frequency)
        {
            decimal val = (decimal)Math.Pow(frequency * 100, 1 / 1.27);
            if(val < 1)
            {
                val = 1;
            }
            return val;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Wave file|*.wav";
            saveFileDialog1.Title = "Save to .wav file";
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "")
            {
                synthGenerator.Save(saveFileDialog1.FileName);
            }
        }
        
        private void colorSliderBeginFrequency1_ValueChanged(object sender, EventArgs e)
        {
            labelBeginFrequency1.Text = convertValueToFrequency(colorSliderBeginFrequency1.Value).ToString("0.00");
            synthGenerator.Waves[0].BeginFrequency = convertValueToFrequency(colorSliderBeginFrequency1.Value);
        }

        private void colorSliderBeginFrequency1_MouseUp(object sender, MouseEventArgs e)
        {
            CreateSound();
        }

        private void colorSliderEndFrequency1_ValueChanged(object sender, EventArgs e)
        {
            labelEndFrequency1.Text = convertValueToFrequency(colorSliderEndFrequency1.Value).ToString("0.00");
            synthGenerator.Waves[0].EndFrequency = convertValueToFrequency(colorSliderEndFrequency1.Value);
        }

        private void colorSliderEndFrequency1_MouseUp(object sender, MouseEventArgs e)
        {
            CreateSound();
        }

        private void labelChannel1_Click(object sender, EventArgs e)
        {
            if(labelChannel1.Text.Equals("both"))
            {
                labelChannel1.Text = "left";
            }
            else if (labelChannel1.Text.Equals("left"))
            {
                labelChannel1.Text = "right";
            }
            else 
            {
                labelChannel1.Text = "both";
            }
            synthGenerator.Waves[0].Channel = GetChannel(1);
            CreateSound();
        }


        private void colorSliderDuration1_ValueChanged(object sender, EventArgs e)
        {
            labelDuration1.Text = string.Format("{0:0.00}", ((double)colorSliderDuration1.Value / 100.0));
            synthGenerator.Waves[0].NumSamples = (int)(441 * (float)colorSliderDuration1.Value);
        }

        private void colorSlider21_ValueChanged(object sender, EventArgs e)
        {
            labelWeight1.Text = string.Format("{0:0}", colorSliderWeight1.Value);
            synthGenerator.Waves[0].Weight = (int)colorSliderWeight1.Value;
        }

        private void colorSliderBeginVolume1_ValueChanged(object sender, EventArgs e)
        {
            labelBeginVolume1.Text = colorSliderBeginVolume1.Value.ToString();
            synthGenerator.Waves[0].BeginVolume = (int)colorSliderBeginVolume1.Value;
        }

        private void colorSliderEndVolume1_ValueChanged(object sender, EventArgs e)
        {
            labelEndVolume1.Text = colorSliderEndVolume1.Value.ToString();
            synthGenerator.Waves[0].EndVolume = (int)colorSliderEndVolume1.Value;
        }

        private void colorSliderDelay1_ValueChanged(object sender, EventArgs e)
        {
            labelDelay1.Text = string.Format("{0:0.00}", ((double)colorSliderDelay1.Value / 100.0));
            synthGenerator.Waves[0].StartPosition = (int)(441 * (float)colorSliderDelay1.Value);
        }

        private void colorSliderEndFrequency1_Scroll(object sender, ScrollEventArgs e)
        {
            checkBoxEndFrequency1.Checked = true;
        }

        private void colorSliderEndVolume1_Scroll(object sender, ScrollEventArgs e)
        {
            checkBoxEndVolume1.Checked = true;
        }

        private void colorSliderAttack_ValueChanged_1(object sender, EventArgs e)
        {
            labelAttack.Text = string.Format("{0:0.0} %", (double)colorSliderAttack.Value);
            synthGenerator.EnvelopAttack = (float)(colorSliderAttack.Value/100);
            synthGenerator.UpdateADSRChart();
        }

        private void colorSliderDecay_ValueChanged_1(object sender, EventArgs e)
        {
            labelDecay.Text = string.Format("{0:0.0} %", (double)colorSliderDecay.Value);
            synthGenerator.EnvelopDecay = (float)(colorSliderDecay.Value/100);
            synthGenerator.UpdateADSRChart();
        }

        private void colorSliderSustain_ValueChanged_1(object sender, EventArgs e)
        {
            labelSustain.Text = string.Format("{0:0.0} %", (double)colorSliderSustain.Value);
            synthGenerator.EnvelopSustain = (float)(colorSliderSustain.Value/100);
            synthGenerator.UpdateADSRChart();
        }

        private void colorSliderRelease_ValueChanged_1(object sender, EventArgs e)
        {
            labelRelease.Text = string.Format("{0:0.0} %", (double)colorSliderRelease.Value);
            synthGenerator.EnvelopRelease = (float)(colorSliderRelease.Value/100);
            synthGenerator.UpdateADSRChart();
        }

        private void colorSliderEndVolume1_MouseUp(object sender, MouseEventArgs e)
        {
            CreateSound();
        }

        private void colorSliderBeginVolume1_MouseUp(object sender, MouseEventArgs e)
        {
            CreateSound();
        }

        private void colorSliderAttack_MouseUp(object sender, MouseEventArgs e)
        {
            CreateSound();
        }

        private void colorSliderDecay_MouseUp(object sender, MouseEventArgs e)
        {
            CreateSound();
        }

        private void colorSliderSustain_MouseUp(object sender, MouseEventArgs e)
        {
            CreateSound();
        }

        private void colorSliderRelease_MouseUp(object sender, MouseEventArgs e)
        {
            CreateSound();
        }

        private void colorSliderDuration1_MouseUp(object sender, MouseEventArgs e)
        {
            CreateSound();
        }

        private void colorSliderDelay1_MouseUp(object sender, MouseEventArgs e)
        {
            CreateSound();
        }

        private void colorSliderWeight1_MouseUp(object sender, MouseEventArgs e)
        {
            CreateSound();
        }

        private void checkBoxEndVolume1_CheckedChanged(object sender, EventArgs e)
        {
            synthGenerator.Waves[0].EndVolumeEnabled = checkBoxEndVolume1.Checked;
            CreateSound();
        }

        private void checkBoxEndFrequency1_CheckedChanged(object sender, EventArgs e)
        {
            synthGenerator.Waves[0].EndFrequencyEnabled = checkBoxEndFrequency1.Checked;
            CreateSound();
        }

        private void buttonAddPreset_Click(object sender, EventArgs e)
        {
            if (textBoxPresetName.Text.Length>0 && !comboBoxPresets.Items.Contains(textBoxPresetName.Text))
            {
                preset.Save(synthGenerator, textBoxPresetName.Text);
                comboBoxPresets.Items.Add(textBoxPresetName.Text);
                comboBoxPresets.SelectedIndex = comboBoxPresets.FindStringExact(textBoxPresetName.Text);
                currentPreset = comboBoxPresets.Text;
            }
        }

        private void comboBoxPresets_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentPreset = comboBoxPresets.Text;
            preset.Load(synthGenerator, currentPreset); 
            UpdateGeneratorControls();
            synthGenerator.GenerateSound();
        }

        private void checkBoxBeginEndBeginVolume1_CheckedChanged(object sender, EventArgs e)
        {
            synthGenerator.Waves[0].BeginEndBeginVolumeEnabled = checkBoxBeginEndBeginVolume1.Checked;
            CreateSound();
        }

        private void checkBoxBeginEndBeginFrequency1_CheckedChanged(object sender, EventArgs e)
        {
            synthGenerator.Waves[0].BeginEndBeginFrequencyEnabled = checkBoxBeginEndBeginFrequency1.Checked;
            CreateSound();
        }

        private void colorSliderHold_ValueChanged(object sender, EventArgs e)
        {
            labelHold.Text = string.Format("{0:0.0} %", (double)colorSliderHold.Value);
            synthGenerator.EnvelopHold = (float)(colorSliderHold.Value / 100);
            synthGenerator.UpdateADSRChart();
        }

        private void buttonFreqMinus1_Click(object sender, EventArgs e)
        {
            colorSliderEndFrequency1.Value -= 10;
            CreateSound();
        }

        private void buttonFreqPlus1_Click(object sender, EventArgs e)
        {
            colorSliderEndFrequency1.Value += 10;
            CreateSound();
        }

        private void buttonFFT_Click(object sender, EventArgs e)
        {
            FormFFT formFFT = new FormFFT();
            formFFT.MyParent = this;
            formFFT.ShowDialog();
        }

        private void buttonBeginFreqMinus1_Click(object sender, EventArgs e)
        {
            colorSliderBeginFrequency1.Value -= 10;
            CreateSound();

        }

        private void buttonBeginFreqPlus1_Click(object sender, EventArgs e)
        {
            colorSliderBeginFrequency1.Value += 10;
            CreateSound();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            FormAmplitude formAmplitude = new FormAmplitude();
            formAmplitude.MyParent = this;
            formAmplitude.ShowDialog();
        }

        private void buttonSavePreset_Click(object sender, EventArgs e)
        {
            if (currentPreset.Length > 0)
            {
                preset.Save(synthGenerator, currentPreset);
            }
        }

        private void buttonLoadWav_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Wave file|*.wav";
            openFileDialog1.Title = "Load .wav file";
            openFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (openFileDialog1.FileName != "")
            {
                synthGenerator.LoadWaveFile(openFileDialog1.FileName);
            }
        }

        private void NextWaveForm()
        {
            switch (synthGenerator.CurrentWave.WaveForm)
            {
                case "Sine":
                    synthGenerator.CurrentWave.WaveForm = "Square";
                    break;
                case "Square":
                    synthGenerator.CurrentWave.WaveForm = "Triangle";
                    break;
                case "Triangle":
                    synthGenerator.CurrentWave.WaveForm = "Sawtooth";
                    break;
                case "Sawtooth":
                    synthGenerator.CurrentWave.WaveForm = "Noise";
                    break;
                case "Noise":
                    synthGenerator.CurrentWave.WaveForm = "File (.wav)";
                    break;
                case "File (.wav)":
                    synthGenerator.CurrentWave.WaveForm = "Sine";
                    break;
            }

            UpdateWaveFormPicture();
            labelWaveForm.Text = synthGenerator.CurrentWave.WaveForm;

            if (synthGenerator.CurrentWave.WaveForm.Equals("File (.wav)"))
            {
                buttonLoadWav.Visible = true;
                colorSliderBeginFrequency1.Visible = false;
                colorSliderEndFrequency1.Visible = false;
                labelBeginFreq1.Visible = false;
                labelBeginFrequency1.Visible = false;
                labelEndFrequency1.Visible = false;
                checkBoxEndFrequency1.Visible = false;
            }
            else
            {
                buttonLoadWav.Visible = false;
                colorSliderBeginFrequency1.Visible = true;
                colorSliderEndFrequency1.Visible = true;
                labelBeginFreq1.Visible = true;
                labelBeginFrequency1.Visible = true;
                labelEndFrequency1.Visible = true;
                checkBoxEndFrequency1.Visible = true;
            }
            CreateSound();
        }


        private void pictureBoxWaveForm_Click(object sender, EventArgs e)
        {
            NextWaveForm();
        }

        private void chartResultLeft_Click(object sender, EventArgs e)
        {
            FormAmplitude formAmplitude = new FormAmplitude();
            formAmplitude.MyParent = this;
            formAmplitude.ShowDialog();
        }

        private void chartResultRight_Click(object sender, EventArgs e)
        {
            FormAmplitude formAmplitude = new FormAmplitude();
            formAmplitude.MyParent = this;
            formAmplitude.ShowDialog();
        }

    }
}
