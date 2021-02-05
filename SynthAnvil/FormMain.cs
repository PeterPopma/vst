using SynthAnvil.Synth;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace SynthAnvil
{
    public partial class FormMain : Form
    {
        public const int MAX_FREQUENCY = 22387;

        Timer aTimer = new Timer();
        bool generatorEnabled = false;
        bool changedPresetData = false;
        bool isPlaying = false;

        Random random = new Random();
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
                case "WavFile":
                    pictureBoxWaveForm.Image = Properties.Resources.wav;
                    break;
                case "Custom":
                    pictureBoxWaveForm.Image = Properties.Resources.custom;
                    break;
            }
        }
        public void UpdateWaveControls()
        {
            generatorEnabled = false;

            colorSliderDuration.Value = synthGenerator.CurrentWave.NumSamples();
            colorSliderDelay.Value = synthGenerator.CurrentWave.StartPosition;
            colorSliderWeight1.Value = synthGenerator.CurrentWave.Weight;
            UpdateChannelText();

            labelWaveForm.Text = synthGenerator.CurrentWave.WaveForm;
            UpdateWaveFormPicture();

            colorSliderAttack.Value = (decimal)synthGenerator.EnvelopAttack * 100;
            colorSliderHold.Value = (decimal)synthGenerator.EnvelopHold * 100;
            colorSliderDecay.Value = (decimal)synthGenerator.EnvelopDecay * 100;
            colorSliderSustain.Value = (decimal)synthGenerator.EnvelopSustain * 100;
            colorSliderRelease.Value = (decimal)synthGenerator.EnvelopRelease * 100;

            labelVolumeMin.Text = synthGenerator.CurrentWave.MinVolume.ToString();
            labelVolumeMax.Text = synthGenerator.CurrentWave.MaxVolume.ToString();
            labelFrequencyMin.Text = synthGenerator.CurrentWave.MinFrequency.ToString();
            labelFrequencyMax.Text = synthGenerator.CurrentWave.MaxFrequency.ToString();
            

            UpdateVisibility();

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

            groupBox4.Paint += new PaintEventHandler(GroupBoxPaint);
            groupBox4.Refresh();

            groupBox5.Paint += new PaintEventHandler(GroupBoxPaint);
            groupBox5.Refresh();

            groupBox6.Paint += new PaintEventHandler(GroupBoxPaint);
            groupBox6.Refresh();

            synthGenerator = new SynthGenerator(this);

            // Init generators
            synthGenerator.Waves.Add(new WaveInfo());
            synthGenerator.CurrentWave = synthGenerator.Waves[0];

            UpdateWaveControls();

            chartAHDSR.ChartAreas[0].AxisY.Minimum = 0;
            chartAHDSR.ChartAreas[0].AxisY.Maximum = 1;

            chartCurrentWave.ChartAreas[0].AxisY.Maximum = 40000;
            chartCurrentWave.ChartAreas[0].AxisY.Minimum = -40000;

            chartResultLeft.ChartAreas[0].AxisY.Maximum = 40000;
            chartResultLeft.ChartAreas[0].AxisY.Minimum = -40000;
            chartResultRight.ChartAreas[0].AxisY.Maximum = 40000;
            chartResultRight.ChartAreas[0].AxisY.Minimum = -40000;

            generatorEnabled = false;

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

            RecreateWavesLists();

            synthGenerator.GenerateSound();
            generatorEnabled = true;
        }

        private void UpdateChannelText()
        {
            switch (synthGenerator.CurrentWave.Channel)
            {
                case 0:
                    labelChannel.Text = "left";
                    break;
                case 1:
                    labelChannel.Text = "right";
                    break;
                case 2:
                    labelChannel.Text = "both";
                    break;
            }
        }

        public void GenerateSound()
        {
            if (generatorEnabled)
            {
                changedPresetData = true;
                synthGenerator.GenerateSound();
            }
        }

        private void TimerEventProcessor(Object myObject,
                                                   EventArgs myEventArgs)
        {
            synthGenerator.Play();
        }

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

        private void labelChannel1_Click(object sender, EventArgs e)
        {
            synthGenerator.CurrentWave.Channel++;
            if (synthGenerator.CurrentWave.Channel == 3)
            {
                synthGenerator.CurrentWave.Channel = 0;
            }
            UpdateChannelText();
            GenerateSound();
        }


        private void colorSliderDuration1_ValueChanged(object sender, EventArgs e)
        {
            labelDuration1.Text = string.Format("{0:0.0000}", (double)colorSliderDuration.Value / SynthGenerator.SAMPLES_PER_SECOND);
        }

        private void colorSlider21_ValueChanged(object sender, EventArgs e)
        {
            labelWeight1.Text = string.Format("{0:0}", colorSliderWeight1.Value);
            synthGenerator.CurrentWave.Weight = (int)colorSliderWeight1.Value;
        }

        private void colorSliderDelay1_ValueChanged(object sender, EventArgs e)
        {
            labelDelay1.Text = string.Format("{0:0.0000}", ((double)colorSliderDelay.Value / SynthGenerator.SAMPLES_PER_SECOND));
            synthGenerator.CurrentWave.StartPosition = (int)(colorSliderDelay.Value);
        }

        private void colorSliderAttack_ValueChanged_1(object sender, EventArgs e)
        {
            labelAttack.Text = string.Format("{0:0.0} %", (double)colorSliderAttack.Value);
            synthGenerator.EnvelopAttack = (float)(colorSliderAttack.Value / 100);
            synthGenerator.UpdateADSRChart();
        }

        private void colorSliderDecay_ValueChanged_1(object sender, EventArgs e)
        {
            labelDecay.Text = string.Format("{0:0.0} %", (double)colorSliderDecay.Value);
            synthGenerator.EnvelopDecay = (float)(colorSliderDecay.Value / 100);
            synthGenerator.UpdateADSRChart();
        }

        private void colorSliderSustain_ValueChanged_1(object sender, EventArgs e)
        {
            labelSustain.Text = string.Format("{0:0.0} %", (double)colorSliderSustain.Value);
            synthGenerator.EnvelopSustain = (float)(colorSliderSustain.Value / 100);
            synthGenerator.UpdateADSRChart();
        }

        private void colorSliderRelease_ValueChanged_1(object sender, EventArgs e)
        {
            labelRelease.Text = string.Format("{0:0.0} %", (double)colorSliderRelease.Value);
            synthGenerator.EnvelopRelease = (float)(colorSliderRelease.Value / 100);
            synthGenerator.UpdateADSRChart();
        }

        private void colorSliderEndVolume1_MouseUp(object sender, MouseEventArgs e)
        {
            GenerateSound();
        }

        private void colorSliderBeginVolume1_MouseUp(object sender, MouseEventArgs e)
        {
            GenerateSound();
        }

        private void colorSliderAttack_MouseUp(object sender, MouseEventArgs e)
        {
            GenerateSound();
        }

        private void colorSliderDecay_MouseUp(object sender, MouseEventArgs e)
        {
            GenerateSound();
        }

        private void colorSliderSustain_MouseUp(object sender, MouseEventArgs e)
        {
            GenerateSound();
        }

        private void colorSliderRelease_MouseUp(object sender, MouseEventArgs e)
        {
            GenerateSound();
        }

        private void colorSliderDuration1_MouseUp(object sender, MouseEventArgs e)
        {
            if (synthGenerator.CurrentWave.WaveForm == "WavFile")
            {
                // The wav file is selected; stretch its wavefile data to the new duration
                // TODO
            }
            else
            {
                synthGenerator.CurrentWave.WaveData = new double[(int)(colorSliderDuration.Value) * SynthGenerator.NUM_AUDIO_CHANNELS];
            }
            GenerateSound();
        }

        private void colorSliderDelay1_MouseUp(object sender, MouseEventArgs e)
        {
            GenerateSound();
        }

        private void colorSliderWeight1_MouseUp(object sender, MouseEventArgs e)
        {
            GenerateSound();
        }

        private void comboBoxPresets_SelectedIndexChanged(object sender, EventArgs e)
        {
            changedPresetData = false;
            currentPreset = comboBoxPresets.Text;
            preset.Load(synthGenerator, currentPreset);
            RecreateWavesLists();
            UpdateWaveControls();
            synthGenerator.GenerateSound();
        }

        private void RecreateWavesLists()
        {
            listBoxWaves.Items.Clear();
            foreach (WaveInfo wave in synthGenerator.Waves)
            {
                listBoxWaves.Items.Add(wave.Name);
            }
            listBoxWaves.SelectedIndex = listBoxWaves.FindStringExact(synthGenerator.CurrentWave.Name);
        }

        private void AddWaveToLists(WaveInfo newWave)
        {
            listBoxWaves.Items.Add(newWave.Name);
        }

        private void RemoveWaveFromList(WaveInfo waveInfo)
        {
            listBoxWaves.Items.RemoveAt(listBoxWaves.FindStringExact(waveInfo.Name));
        }

        private void colorSliderHold_ValueChanged(object sender, EventArgs e)
        {
            labelHold.Text = string.Format("{0:0.0} %", (double)colorSliderHold.Value);
            synthGenerator.EnvelopHold = (float)(colorSliderHold.Value / 100);
            synthGenerator.UpdateADSRChart();
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
                    synthGenerator.CurrentWave.WaveForm = "WavFile";
                    break;
                case "WavFile":
                    synthGenerator.CurrentWave.WaveForm = "Custom";
                    break;
                case "Custom":
                    synthGenerator.CurrentWave.WaveForm = "Sine";
                    break;
            }

            UpdateWaveFormPicture();
            if (synthGenerator.CurrentWave.WaveForm.Equals("WavFile"))
            {
                labelWaveForm.Text = "File (.wav)";
            }
            else
            {
                labelWaveForm.Text = synthGenerator.CurrentWave.WaveForm;
            }


            UpdateVisibility();

            GenerateSound();
        }

        private void UpdateVisibility()
        {
            if (synthGenerator.CurrentWave.WaveForm.Equals("WavFile") || synthGenerator.CurrentWave.WaveForm.Equals("Noise"))
            {
                labelFrequencyMax.Visible = false;
                labelFrequencyMin.Visible = false;
                buttonCreateInharmonics.Enabled = false;
                buttonCreateHarmonics.Enabled = false;
                labelFrequencyTitle.Visible = false;
                pictureBoxFrequencyShape.Visible = false;
                labelFreqMaxTitle.Visible = false;
                labelFreqMinTitle.Visible = false;
                labelFrequencyMax.Visible = false;
                labelFrequencyMin.Visible = false;
            }
            else
            {
                buttonSelectWav.Visible = false;
                labelFrequencyMax.Visible = true;
                labelFrequencyMin.Visible = true;
                buttonCreateInharmonics.Enabled = true;
                buttonCreateHarmonics.Enabled = true;
                labelFrequencyTitle.Visible = true;
                pictureBoxFrequencyShape.Visible = true;
                labelFreqMaxTitle.Visible = true;
                labelFreqMinTitle.Visible = true;
                labelFrequencyMax.Visible = true;
                labelFrequencyMin.Visible = true;
            }
            if (synthGenerator.CurrentWave.WaveForm.Equals("WavFile"))
            {
                buttonSelectWav.Visible = true;
                labelFileName.Visible = true;
            }
            else
            {
                buttonSelectWav.Visible = false;
                labelFileName.Visible = false;
            }
            if (synthGenerator.CurrentWave.WaveForm.Equals("Custom"))
            {
                pictureBoxCustomWave.Visible = true;
            }
            else
            {
                pictureBoxCustomWave.Visible = false;
            }
        }

        private void pictureBoxWaveForm_Click(object sender, EventArgs e)
        {
            NextWaveForm();
        }

        private void chartResultLeft_Click(object sender, EventArgs e)
        {
            FormAmplitudeChart formAmplitude = new FormAmplitudeChart();
            formAmplitude.MyParent = this;
            formAmplitude.ShowDialog();
        }

        private void chartResultRight_Click(object sender, EventArgs e)
        {
            FormAmplitudeChart formAmplitude = new FormAmplitudeChart();
            formAmplitude.MyParent = this;
            formAmplitude.ShowDialog();
        }

        private void chartResultLeft_MouseMove(object sender, MouseEventArgs e)
        {
            chartResultLeft.Cursor = new Cursor(Properties.Resources.magnifying_glass.Handle);
        }

        private void chartResultRight_MouseMove(object sender, MouseEventArgs e)
        {
            chartResultRight.Cursor = new Cursor(Properties.Resources.magnifying_glass.Handle);
        }

        private void chartFFTLeft_MouseMove(object sender, MouseEventArgs e)
        {
            chartFFTLeft.Cursor = new Cursor(Properties.Resources.magnifying_glass.Handle);
        }

        private void chartFFTRight_MouseMove(object sender, MouseEventArgs e)
        {
            chartFFTRight.Cursor = new Cursor(Properties.Resources.magnifying_glass.Handle);
        }

        private void chartFFTLeft_Click(object sender, EventArgs e)
        {
            FormFFTChart formFFT = new FormFFTChart();
            formFFT.MyParent = this;
            formFFT.ShowDialog();
        }

        private void chartFFTRight_Click(object sender, EventArgs e)
        {
            FormFFTChart formFFT = new FormFFTChart();
            formFFT.MyParent = this;
            formFFT.ShowDialog();
        }

        private void colorSliderHold_MouseUp(object sender, MouseEventArgs e)
        {
            GenerateSound();
        }

        private void buttonSavePreset2_Click(object sender, EventArgs e)
        {
            if (currentPreset.Length > 0)
            {
                changedPresetData = false;
                preset.Save(synthGenerator, currentPreset);
            }
        }

        private void buttonAddPreset2_Click(object sender, EventArgs e)
        {
            if (textBoxPresetName.Text.Length > 0 && !comboBoxPresets.Items.Contains(textBoxPresetName.Text))
            {
                preset.Save(synthGenerator, textBoxPresetName.Text);
                comboBoxPresets.Items.Add(textBoxPresetName.Text);
                comboBoxPresets.SelectedIndex = comboBoxPresets.FindStringExact(textBoxPresetName.Text);
                currentPreset = comboBoxPresets.Text;
                textBoxPresetName.Text = "";
            }
        }

        private void buttonSelectWav_Click(object sender, EventArgs e)
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

        private void buttonAddNewWave_Click(object sender, EventArgs e)
        {
            synthGenerator.CurrentWave = synthGenerator.CloneWave();
            AddWaveToLists(synthGenerator.CurrentWave);
            listBoxWaves.SelectedIndex = listBoxWaves.FindStringExact(synthGenerator.CurrentWave.Name);
            GenerateSound();
        }

        private void listBoxWaves_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxWaves.SelectedItem != null)
            {
                synthGenerator.SetCurrentWaveByName(listBoxWaves.SelectedItem.ToString());
                synthGenerator.GenerateSound();     // redraw the graphs, etc.
                UpdateWaveControls();
            }
        }

        private void buttonDeleteWave_Click(object sender, EventArgs e)
        {
            if (listBoxWaves.Items.Count == 1)
            {
                return;
            }
            int index = listBoxWaves.SelectedIndex;
            RemoveWaveFromList(synthGenerator.CurrentWave);
            synthGenerator.RemoveCurrentWave();
            if (listBoxWaves.Items.Count > index)
            {
                listBoxWaves.SelectedIndex = index;
            }
            else
            {
                listBoxWaves.SelectedIndex = index - 1;
            }
            GenerateSound();
            UpdateWaveControls();
        }

        private void buttonDeleteAll_Click(object sender, EventArgs e)
        {
            while (synthGenerator.Waves.Count > 1)
            {
                synthGenerator.Waves.RemoveAt(synthGenerator.Waves.Count - 1);
            }
            RecreateWavesLists();
            GenerateSound();
            UpdateWaveControls();
        }

        private void buttonCreateInharmonics_Click(object sender, EventArgs e)
        {
            for (int i = 1; i <= numericUpDownInHarmonics.Value; i++)
            {
                double deviation = i / (double)numericUpDownInHarmonics.Value * (double)numericUpDownInHarmonicsSpread.Value / 100.0;
                if (checkBoxRandomInharmonics.Checked)
                {
                    // choose a random spot between this and the previous deviation
                    double previous_deviation = (i - 1) / (double)numericUpDownInHarmonics.Value * (double)numericUpDownInHarmonicsSpread.Value / 100.0;
                    double place = random.Next(1000) / 1000.0;
                    deviation = (previous_deviation * place) + (deviation * (1 - place));
                }
                double frequency_factor = 1 + deviation;
                if (i % 2 == 0)
                {
                    frequency_factor = 1 / frequency_factor;
                }

                if ((frequency_factor * synthGenerator.CurrentWave.MinFrequency) < MAX_FREQUENCY && (frequency_factor * synthGenerator.CurrentWave.MaxFrequency) < MAX_FREQUENCY)
                {
                    double amplitudeFactor = 1 / Math.Pow(1 + ((double)numericUpDownInHarmonicsDecay.Value / 100.0), i);
                    WaveInfo newWave = synthGenerator.CloneWave(frequency_factor, amplitudeFactor);
                    AddWaveToLists(newWave);
                }
            }
            synthGenerator.GenerateSound();
        }

        private void CreateHarmonic(int harmonic_number, double harmonic_factor)
        {
            double amplitudeFactor = Math.Pow(((double)numericUpDownHarmVolume.Value / 100.0), harmonic_number);
            double durationFactor = Math.Pow(((double)numericUpDownHarmDuration.Value / 100.0), harmonic_number);
            double amplitudeDecayFactor = 1 / Math.Pow(1 + ((double)numericUpDownHarmDecay.Value / 100.0), harmonic_number);
            WaveInfo newWave = synthGenerator.CloneWave(harmonic_factor, amplitudeFactor);
            newWave.MaxVolume = (int)(amplitudeDecayFactor * newWave.MinVolume);
            newWave.SetDuration(newWave.Duration() * durationFactor);
            AddWaveToLists(newWave);
        }

        private void buttonCreateHarmonics_Click(object sender, EventArgs e)
        {
            int num_harmonics = 0;
            int harmonic_factor = 2;
            while (num_harmonics < (int)numericUpDownHarmonics.Value)
            {
                if (harmonic_factor % 2 == 0 && (radioButtonHarmEven.Checked || radioButtonHarmBoth.Checked))
                {
                    num_harmonics++;
                    if (checkBoxLowerFrequencies.Checked)
                    {
                        CreateHarmonic(num_harmonics, 1 / (double)harmonic_factor);
                    }
                    else
                    {
                        CreateHarmonic(num_harmonics, harmonic_factor);
                    }
                }
                if (harmonic_factor % 2 == 1 && (radioButtonHarmOdd.Checked || radioButtonHarmBoth.Checked))
                {
                    num_harmonics++;
                    if (checkBoxLowerFrequencies.Checked)
                    {
                        CreateHarmonic(num_harmonics, 1 / (double)harmonic_factor);
                    }
                    else
                    {
                        CreateHarmonic(num_harmonics, harmonic_factor);
                    }
                }

                harmonic_factor++;
            }

            synthGenerator.GenerateSound();
        }

        private void chartResultLeft_Paint(object sender, PaintEventArgs e)
        {
            if (synthGenerator.CurrentWave.Channel == 1)
            {
                return;     // skip if only right channel
            }
            double withPercentage = synthGenerator.CurrentWave.Duration() / synthGenerator.Duration();
            double startPercentage = synthGenerator.CurrentWave.StartTime() / synthGenerator.Duration();
            Rectangle rect = new Rectangle(29 + (int)(346 * startPercentage), 4, (int)(346 * withPercentage), 110);
            Pen myBrush = new Pen(Color.White);
            e.Graphics.DrawRectangle(myBrush, rect);
        }

        private void chartResultRight_Paint(object sender, PaintEventArgs e)
        {
            if (synthGenerator.CurrentWave.Channel == 0)
            {
                return;     // skip if only left channel
            }
            double withPercentage = synthGenerator.CurrentWave.Duration() / synthGenerator.Duration();
            double startPercentage = synthGenerator.CurrentWave.StartTime() / synthGenerator.Duration();
            Rectangle rect = new Rectangle(29 + (int)(346 * startPercentage), 4, (int)(346 * withPercentage), 110);
            System.Drawing.Pen myBrush = new System.Drawing.Pen(System.Drawing.Color.White);
            e.Graphics.DrawRectangle(myBrush, rect);
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (changedPresetData)
            {
                DialogResult dialogResult = MessageBox.Show("You made changes to the Preset. Do you want to save them?", "Save Changes", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    preset.Save(synthGenerator, currentPreset);
                }
            }
        }

        private void pictureBoxCustomWave_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBoxCustomWave.Cursor = new Cursor(Properties.Resources.pencil.Handle);
        }

        private void pictureBoxCustomWave_Click(object sender, EventArgs e)
        {
            FormCustomShape formCustomWave = new FormCustomShape();
            formCustomWave.MyParent = this;
            formCustomWave.ShowDialog();
        }

        private void pictureBoxCustomWave_Paint(object sender, PaintEventArgs e)
        {
            Control control = (Control)sender;
            using (LinearGradientBrush brush = new LinearGradientBrush(control.ClientRectangle,
                                                                       Color.FromArgb(195, 70, 70),
                                                                       Color.FromArgb(15, 0, 0),
                                                                       90F))
            {
                e.Graphics.FillRectangle(brush, control.ClientRectangle);
                ControlPaint.DrawBorder(e.Graphics, control.ClientRectangle, Color.Gray, ButtonBorderStyle.Solid);
            }

            Pen pen = new Pen(Color.White);

            if (synthGenerator.CurrentWave.ShapeWave.Length == SynthGenerator.SHAPE_WAVE_NUMPOINTS)
            {
                for (int x = 0; x < pictureBoxCustomWave.Width; x++)
                {
                    int position = (int)(x / (double)pictureBoxCustomWave.Width * SynthGenerator.SHAPE_WAVE_NUMPOINTS);
                    int next_position = (int)((x + 1) / (double)pictureBoxCustomWave.Width * SynthGenerator.SHAPE_WAVE_NUMPOINTS);
                    if (next_position < SynthGenerator.SHAPE_WAVE_NUMPOINTS)
                    {
                        int value1 = (int)((synthGenerator.CurrentWave.ShapeWave[position] + SynthGenerator.SHAPE_WAVE_MAX_VALUE) * (pictureBoxCustomWave.Height / (2.0 * SynthGenerator.SHAPE_WAVE_MAX_VALUE + 1)));
                        int value2 = (int)((synthGenerator.CurrentWave.ShapeWave[next_position] + SynthGenerator.SHAPE_WAVE_MAX_VALUE) * (pictureBoxCustomWave.Height / (2.0 * SynthGenerator.SHAPE_WAVE_MAX_VALUE + 1)));
                        e.Graphics.DrawLine(pen, new Point(x, value1), new Point(x + 1, value2));
                    }
                }
            }
        }

        private void buttonDeletePreset2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this Preset?", "Delete Preset", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                preset.Delete(comboBoxPresets.Text);
                comboBoxPresets.Items.RemoveAt(comboBoxPresets.SelectedIndex);
                if (comboBoxPresets.Items.Count > 0)
                {
                    comboBoxPresets.SelectedIndex = 0;
                }
            }
        }

        private void gradientButton1_Click(object sender, EventArgs e)
        {
            synthGenerator.Play(415 / 440.0);
        }

        private void buttonPlayHigher_Click(object sender, EventArgs e)
        {
            synthGenerator.Play(466 / 440.0);
        }

        private void buttonPlayA_Click(object sender, EventArgs e)
        {
            synthGenerator.Play();
        }

        private void buttonPlayG_Click(object sender, EventArgs e)
        {
            synthGenerator.Play(392.00 / 440.0);
        }

        private void buttonPlayB_Click(object sender, EventArgs e)
        {
            synthGenerator.Play(493.88 / 440.0);
        }

        private void buttonPlayBFlat_Click(object sender, EventArgs e)
        {
            synthGenerator.Play(466.16 / 440.0);
        }

        private void buttonPlayGSharp_Click(object sender, EventArgs e)
        {
            synthGenerator.Play(415.30 / 440.0);
        }

        private void buttonPlayFSharp_Click(object sender, EventArgs e)
        {
            synthGenerator.Play(369.99 / 440.0);
        }

        private void buttonPlayF_Click(object sender, EventArgs e)
        {
            synthGenerator.Play(349.23 / 440.0);
        }

        private void buttonPlayE_Click(object sender, EventArgs e)
        {
            synthGenerator.Play(329.63 / 440.0);
        }

        private void buttonPlayC1_Click(object sender, EventArgs e)
        {
            synthGenerator.Play(523.25 / 440.0);
        }

        private void buttonPlayD1_Click(object sender, EventArgs e)
        {
            synthGenerator.Play(587.33 / 440.0);
        }

        private void buttonPlayC5Sharp_Click(object sender, EventArgs e)
        {
            synthGenerator.Play(554.37 / 440.0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!isPlaying)
            {
                buttonPlay.Image = SynthAnvil.Properties.Resources.pausebutton;
                isPlaying = true;
                aTimer.Interval = (int)(synthGenerator.CurrentWave.Duration() * 1000);
                aTimer.Tick += new EventHandler(TimerEventProcessor);
                aTimer.Enabled = true;
            }
            else
            {
                buttonPlay.Image = SynthAnvil.Properties.Resources.playbutton;
                isPlaying = false;
                aTimer.Enabled = false;
            }
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

        private void buttonPlayWav_Click(object sender, EventArgs e)
        {
            synthGenerator.Play(1, false);
            synthGenerator.GenerateSound();     // play single wave overwrites mix buffer
        }

        private void FormMain_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                    buttonPlayD4.PerformClick();
                    break;
                case Keys.S:
                    buttonPlayE4.PerformClick();
                    break;
                case Keys.D:
                    buttonPlayF4.PerformClick();
                    break;
                case Keys.F:
                    buttonPlayG4.PerformClick();
                    break;
                case Keys.G:
                    buttonPlayA4.PerformClick();
                    break;
                case Keys.H:
                    buttonPlayB4.PerformClick();
                    break;
                case Keys.J:
                    buttonPlayC5.PerformClick();
                    break;
                case Keys.K:
                    buttonPlayD5.PerformClick();
                    break;
                case Keys.L:
                    buttonPlayE5.PerformClick();
                    break;
                case Keys.P:
                    buttonPlayA4.PerformClick();
                    break;
                case Keys.W:
                    buttonPlayD4Sharp.PerformClick();
                    break;
                case Keys.R:
                    buttonPlayF4Sharp.PerformClick();
                    break;
                case Keys.T:
                    buttonPlayB4Flat.PerformClick();
                    break;
                case Keys.Y:
                    buttonPlayA4.PerformClick();
                    break;
                case Keys.I:
                    buttonPlayC5Sharp.PerformClick();
                    break;
                case Keys.Q:
                    buttonPlayD5Sharp.PerformClick();
                    break;
            }
        }

        private void buttonPlayD5Sharp_Click(object sender, EventArgs e)
        {
            synthGenerator.Play(622.25 / 440.0);
        }

        private void buttonPlayE5_Click(object sender, EventArgs e)
        {
            synthGenerator.Play(659.25 / 440.0);
        }

        private void buttonPlayD4Sharp_Click(object sender, EventArgs e)
        {
            synthGenerator.Play(311.13 / 440.0);
        }

        private void buttonPlayD4_Click(object sender, EventArgs e)
        {
            synthGenerator.Play(293.66 / 440.0);
        }

        private void comboBoxPresets_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void buttonDurationMinus1_Click(object sender, EventArgs e)
        {
            colorSliderDuration.Value -= 10;
            GenerateSound();
        }

        private void buttonDurationMinus10_Click(object sender, EventArgs e)
        {
            colorSliderDuration.Value -= 100;
            GenerateSound();
        }

        private void buttonDurationPlus10_Click(object sender, EventArgs e)
        {
            colorSliderDuration.Value += 100;
            GenerateSound();
        }

        private void buttonDurationPlus1_Click(object sender, EventArgs e)
        {
            colorSliderDuration.Value += 10;
            GenerateSound();
        }

        private void buttonDelayMinus10_Click(object sender, EventArgs e)
        {
            colorSliderDelay.Value -= 100;
            GenerateSound();
        }

        private void buttonDelayMinus1_Click(object sender, EventArgs e)
        {
            colorSliderDelay.Value -= 10;
            GenerateSound();
        }

        private void buttonDelayPlus1_Click(object sender, EventArgs e)
        {
            colorSliderDelay.Value += 10;
            GenerateSound();
        }

        private void buttonDelayPlus10_Click(object sender, EventArgs e)
        {
            colorSliderDelay.Value += 100;
            GenerateSound();
        }

        private void pictureBoxFrequencyShape_Click(object sender, EventArgs e)
        {
            FormFrequency formFrequency = new FormFrequency();
            formFrequency.MyParent = this;
            formFrequency.ShowDialog();
        }

        private void pictureBoxVolumeShape_Click(object sender, EventArgs e)
        {
            FormVolume formVolume = new FormVolume();
            formVolume.MyParent = this;
            formVolume.ShowDialog();
        }

        private void pictureBoxVolumeShape_Paint(object sender, PaintEventArgs e)
        {
            Control control = (Control)sender;
            using (LinearGradientBrush brush = new LinearGradientBrush(control.ClientRectangle,
                                                                       Color.FromArgb(70, 87, 195),
                                                                       Color.FromArgb(0, 0, 15),
                                                                       90F))
            {
                e.Graphics.FillRectangle(brush, control.ClientRectangle);
                ControlPaint.DrawBorder(e.Graphics, control.ClientRectangle, Color.Gray, ButtonBorderStyle.Solid);
            }

            Pen pen = new Pen(Color.White);

            if (synthGenerator.CurrentWave.ShapeVolume.Length == SynthGenerator.SHAPE_VOLUME_NUMPOINTS)
            {
                for (int x = 0; x < pictureBoxVolumeShape.Width; x++)
                {
                    int position = (int)(x / (double)pictureBoxVolumeShape.Width * SynthGenerator.SHAPE_VOLUME_NUMPOINTS);
                    int next_position = (int)((x + 1) / (double)pictureBoxVolumeShape.Width * SynthGenerator.SHAPE_VOLUME_NUMPOINTS);
                    if (next_position < SynthGenerator.SHAPE_VOLUME_NUMPOINTS)
                    {
                        int value1 = (int)((SynthGenerator.SHAPE_VOLUME_MAX_VALUE - synthGenerator.CurrentWave.ShapeVolume[position]) * (pictureBoxVolumeShape.Height / (double)SynthGenerator.SHAPE_VOLUME_MAX_VALUE));
                        int value2 = (int)((SynthGenerator.SHAPE_VOLUME_MAX_VALUE - synthGenerator.CurrentWave.ShapeVolume[next_position]) * (pictureBoxVolumeShape.Height / (double)SynthGenerator.SHAPE_VOLUME_MAX_VALUE));
                        e.Graphics.DrawLine(pen, new Point(x, value1), new Point(x + 1, value2));
                    }
                }
            }
        }

        private void pictureBoxFrequencyShape_Paint(object sender, PaintEventArgs e)
        {
            Control control = (Control)sender;
            using (LinearGradientBrush brush = new LinearGradientBrush(control.ClientRectangle,
                                                                       Color.FromArgb(70, 195, 87 ),
                                                                       Color.FromArgb(0, 15, 0),
                                                                       90F))
            {
                e.Graphics.FillRectangle(brush, control.ClientRectangle);
                ControlPaint.DrawBorder(e.Graphics, control.ClientRectangle, Color.Gray, ButtonBorderStyle.Solid);
            }

            Pen pen = new Pen(Color.White);

            if (synthGenerator.CurrentWave.ShapeFrequency.Length == SynthGenerator.SHAPE_FREQUENCY_NUMPOINTS)
            {
                for (int x = 0; x < pictureBoxFrequencyShape.Width; x++)
                {
                    int position = (int)(x / (double)pictureBoxFrequencyShape.Width * SynthGenerator.SHAPE_FREQUENCY_NUMPOINTS);
                    int next_position = (int)((x + 1) / (double)pictureBoxFrequencyShape.Width * SynthGenerator.SHAPE_FREQUENCY_NUMPOINTS);
                    if (next_position < SynthGenerator.SHAPE_FREQUENCY_NUMPOINTS)
                    {
                        int value1 = (int)((SynthGenerator.SHAPE_FREQUENCY_MAX_VALUE - synthGenerator.CurrentWave.ShapeFrequency[position]) * (pictureBoxFrequencyShape.Height / (double)SynthGenerator.SHAPE_FREQUENCY_MAX_VALUE));
                        int value2 = (int)((SynthGenerator.SHAPE_FREQUENCY_MAX_VALUE - synthGenerator.CurrentWave.ShapeFrequency[next_position]) * (pictureBoxFrequencyShape.Height / (double)SynthGenerator.SHAPE_FREQUENCY_MAX_VALUE));
                        e.Graphics.DrawLine(pen, new Point(x, value1), new Point(x + 1, value2));
                    }
                }
            }
        }
    }
}
