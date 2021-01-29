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

        Random random = new Random();
        SynthGenerator synthGenerator;
        Preset preset = new Preset();
        string currentPreset = "";
        BindingSource bindingListboxWaves = new BindingSource();
        List<WavesListItem> listboxWavesItems = new List<WavesListItem>();

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

            colorSliderBeginFrequency1.Value = convertFrequencyToValue(synthGenerator.CurrentWave.BeginFrequency);
            colorSliderEndFrequency1.Value = convertFrequencyToValue(synthGenerator.CurrentWave.EndFrequency);
            colorSliderBeginVolume1.Value = synthGenerator.CurrentWave.BeginVolume;
            colorSliderEndVolume1.Value = synthGenerator.CurrentWave.EndVolume;
            colorSliderDuration1.Value = synthGenerator.CurrentWave.NumSamples();
            colorSliderDelay1.Value = synthGenerator.CurrentWave.StartPosition;
            checkBoxEndVolume1.Checked = synthGenerator.CurrentWave.EndVolumeEnabled;
            checkBoxEndFrequency1.Checked = synthGenerator.CurrentWave.EndFrequencyEnabled;
            colorSliderWeight1.Value = synthGenerator.CurrentWave.Weight;
            checkBoxBeginEndBeginVolume1.Checked = synthGenerator.CurrentWave.BeginEndBeginVolumeEnabled;
            checkBoxBeginEndBeginFrequency1.Checked = synthGenerator.CurrentWave.BeginEndBeginFrequencyEnabled;
            UpdateChannelText();

            labelWaveForm.Text = synthGenerator.CurrentWave.WaveForm;
            UpdateWaveFormPicture();

            colorSliderAttack.Value = (decimal)synthGenerator.EnvelopAttack * 100;
            colorSliderHold.Value = (decimal)synthGenerator.EnvelopHold * 100;
            colorSliderDecay.Value = (decimal)synthGenerator.EnvelopDecay * 100;
            colorSliderSustain.Value = (decimal)synthGenerator.EnvelopSustain * 100;
            colorSliderRelease.Value = (decimal)synthGenerator.EnvelopRelease * 100;

            UpdateVisibility();

            generatorEnabled = true;
        }

        private void RegenerateWaveListbox()
        {
            listboxWavesItems.Clear();
            for (int i = 0; i < synthGenerator.Waves.Count; i++)
            {
                listboxWavesItems.Add(new WavesListItem(synthGenerator.Waves[i].Number, synthGenerator.Waves[i].Name));
            }
            bindingListboxWaves.ResetBindings(false);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bindingListboxWaves.DataSource = listboxWavesItems;
            listBoxWaves.DataSource = bindingListboxWaves;
            listBoxWaves.DisplayMember = "Name";
            listBoxWaves.ValueMember = "Number";

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

            groupBox7.Paint += new PaintEventHandler(GroupBoxPaint);
            groupBox7.Refresh();

            groupBox8.Paint += new PaintEventHandler(GroupBoxPaint);
            groupBox8.Refresh();

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

            RegenerateWaveListbox();

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

        public void UpdateCurrentWaveInfo()
        {
            synthGenerator.CurrentWave.SetName();
            listboxWavesItems.Find(o => o.Number == synthGenerator.CurrentWave.Number).Name = synthGenerator.CurrentWave.Name;
            bindingListboxWaves.ResetBindings(false);

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

        private float convertValueToFrequency(decimal value)
        {
            return (float)(Math.Pow((double)value, 1.27) / 100.0);
        }

        private decimal convertFrequencyToValue(double frequency)
        {
            decimal val = (decimal)Math.Pow(frequency * 100, 1 / 1.27);
            if (val < 1)
            {
                val = 1;
            }
            return val;
        }

        private void colorSliderBeginFrequency1_ValueChanged(object sender, EventArgs e)
        {
            labelBeginFrequency1.Text = convertValueToFrequency(colorSliderBeginFrequency1.Value).ToString("0.00");
            synthGenerator.CurrentWave.BeginFrequency = convertValueToFrequency(colorSliderBeginFrequency1.Value);
        }

        private void colorSliderBeginFrequency1_MouseUp(object sender, MouseEventArgs e)
        {
            UpdateCurrentWaveInfo();
        }

        private void colorSliderEndFrequency1_ValueChanged(object sender, EventArgs e)
        {
            labelEndFrequency1.Text = convertValueToFrequency(colorSliderEndFrequency1.Value).ToString("0.00");
            synthGenerator.CurrentWave.EndFrequency = convertValueToFrequency(colorSliderEndFrequency1.Value);
        }

        private void colorSliderEndFrequency1_MouseUp(object sender, MouseEventArgs e)
        {
            UpdateCurrentWaveInfo();
        }

        private void labelChannel1_Click(object sender, EventArgs e)
        {
            synthGenerator.CurrentWave.Channel++;
            if (synthGenerator.CurrentWave.Channel == 3)
            {
                synthGenerator.CurrentWave.Channel = 0;
            }
            UpdateChannelText();
            UpdateCurrentWaveInfo();
        }


        private void colorSliderDuration1_ValueChanged(object sender, EventArgs e)
        {
            labelDuration1.Text = string.Format("{0:0.0000}", (double)colorSliderDuration1.Value / SynthGenerator.SAMPLES_PER_SECOND);
        }

        private void colorSlider21_ValueChanged(object sender, EventArgs e)
        {
            labelWeight1.Text = string.Format("{0:0}", colorSliderWeight1.Value);
            synthGenerator.CurrentWave.Weight = (int)colorSliderWeight1.Value;
        }

        private void colorSliderBeginVolume1_ValueChanged(object sender, EventArgs e)
        {
            labelBeginVolume1.Text = colorSliderBeginVolume1.Value.ToString();
            synthGenerator.CurrentWave.BeginVolume = (int)colorSliderBeginVolume1.Value;
        }

        private void colorSliderEndVolume1_ValueChanged(object sender, EventArgs e)
        {
            labelEndVolume1.Text = colorSliderEndVolume1.Value.ToString();
            synthGenerator.CurrentWave.EndVolume = (int)colorSliderEndVolume1.Value;
        }

        private void colorSliderDelay1_ValueChanged(object sender, EventArgs e)
        {
            labelDelay1.Text = string.Format("{0:0.0000}", ((double)colorSliderDelay1.Value / SynthGenerator.SAMPLES_PER_SECOND));
            synthGenerator.CurrentWave.StartPosition = (int)(colorSliderDelay1.Value);
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
            UpdateCurrentWaveInfo();
        }

        private void colorSliderBeginVolume1_MouseUp(object sender, MouseEventArgs e)
        {
            UpdateCurrentWaveInfo();
        }

        private void colorSliderAttack_MouseUp(object sender, MouseEventArgs e)
        {
            UpdateCurrentWaveInfo();
        }

        private void colorSliderDecay_MouseUp(object sender, MouseEventArgs e)
        {
            UpdateCurrentWaveInfo();
        }

        private void colorSliderSustain_MouseUp(object sender, MouseEventArgs e)
        {
            UpdateCurrentWaveInfo();
        }

        private void colorSliderRelease_MouseUp(object sender, MouseEventArgs e)
        {
            UpdateCurrentWaveInfo();
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
                synthGenerator.CurrentWave.WaveData = new double[(int)(colorSliderDuration1.Value) * SynthGenerator.NUM_AUDIO_CHANNELS];
            }
            UpdateCurrentWaveInfo();
        }

        private void colorSliderDelay1_MouseUp(object sender, MouseEventArgs e)
        {
            UpdateCurrentWaveInfo();
        }

        private void colorSliderWeight1_MouseUp(object sender, MouseEventArgs e)
        {
            UpdateCurrentWaveInfo();
        }

        private void checkBoxEndVolume1_CheckedChanged(object sender, EventArgs e)
        {
            synthGenerator.CurrentWave.EndVolumeEnabled = checkBoxEndVolume1.Checked;
            UpdateCurrentWaveInfo();
        }

        private void checkBoxEndFrequency1_CheckedChanged(object sender, EventArgs e)
        {
            synthGenerator.CurrentWave.EndFrequencyEnabled = checkBoxEndFrequency1.Checked;
            UpdateCurrentWaveInfo();
        }

        private void comboBoxPresets_SelectedIndexChanged(object sender, EventArgs e)
        {
            changedPresetData = false;
            currentPreset = comboBoxPresets.Text;
            preset.Load(synthGenerator, currentPreset);
            AddWavesToListbox();
            UpdateWaveControls();
            synthGenerator.GenerateSound();
        }

        private void AddWavesToListbox()
        {
            listboxWavesItems.Clear();
            foreach (WaveInfo wave in synthGenerator.Waves)
            {
                WavesListItem item = new WavesListItem(wave.Number, wave.Name);
                listboxWavesItems.Add(item);
            }
            bindingListboxWaves.ResetBindings(false);
            listBoxWaves.SelectedIndex = listboxWavesItems.IndexOf(listboxWavesItems.Find(o => o.Number == synthGenerator.CurrentWave.Number));
        }

        private void checkBoxBeginEndBeginVolume1_CheckedChanged(object sender, EventArgs e)
        {
            synthGenerator.CurrentWave.BeginEndBeginVolumeEnabled = checkBoxBeginEndBeginVolume1.Checked;
            UpdateCurrentWaveInfo();
        }

        private void checkBoxBeginEndBeginFrequency1_CheckedChanged(object sender, EventArgs e)
        {
            synthGenerator.CurrentWave.BeginEndBeginFrequencyEnabled = checkBoxBeginEndBeginFrequency1.Checked;
            UpdateCurrentWaveInfo();
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

            UpdateCurrentWaveInfo();
        }

        private void UpdateVisibility()
        {
            if (synthGenerator.CurrentWave.WaveForm.Equals("WavFile") || synthGenerator.CurrentWave.WaveForm.Equals("Noise"))
            {
                colorSliderBeginFrequency1.Visible = false;
                colorSliderEndFrequency1.Visible = false;
                labelBeginFreq1.Visible = false;
                labelBeginFrequency1.Visible = false;
                labelEndFrequency1.Visible = false;
                checkBoxEndFrequency1.Visible = false;
                checkBoxBeginEndBeginFrequency1.Visible = false;
                buttonFreqMinus1.Visible = false;
                buttonFreqPlus1.Visible = false;
                buttonFreqMinus10.Visible = false;
                buttonFreqPlus10.Visible = false;
                buttonBeginFreqPlus1.Visible = false;
                buttonBeginFreqMinus1.Visible = false;
                buttonBeginFreqPlus10.Visible = false;
                buttonBeginFreqMinus10.Visible = false;
                buttonCreateInharmonics.Enabled = false;
                buttonCreateEvenHarmonics.Enabled = false;
                buttonCreateOddHarmonics.Enabled = false;
                buttonCreateSpikes.Enabled = false;
            }
            else
            {
                buttonSelectWav.Visible = false;
                colorSliderBeginFrequency1.Visible = true;
                colorSliderEndFrequency1.Visible = true;
                labelBeginFreq1.Visible = true;
                labelBeginFrequency1.Visible = true;
                labelEndFrequency1.Visible = true;
                checkBoxEndFrequency1.Visible = true;
                checkBoxBeginEndBeginFrequency1.Visible = true;
                buttonFreqMinus1.Visible = true;
                buttonFreqPlus1.Visible = true;
                buttonFreqMinus10.Visible = true;
                buttonFreqPlus10.Visible = true;
                buttonBeginFreqPlus1.Visible = true;
                buttonBeginFreqMinus1.Visible = true;
                buttonBeginFreqPlus10.Visible = true;
                buttonBeginFreqMinus10.Visible = true;
                buttonCreateInharmonics.Enabled = true;
                buttonCreateEvenHarmonics.Enabled = true;
                buttonCreateOddHarmonics.Enabled = true;
                buttonCreateSpikes.Enabled = true;
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
            FormFFT formFFT = new FormFFT();
            formFFT.MyParent = this;
            formFFT.ShowDialog();
        }

        private void chartFFTRight_Click(object sender, EventArgs e)
        {
            FormFFT formFFT = new FormFFT();
            formFFT.MyParent = this;
            formFFT.ShowDialog();
        }

        private void colorSliderHold_MouseUp(object sender, MouseEventArgs e)
        {
            UpdateCurrentWaveInfo();
        }

        private void buttonBeginFreqMinus_Click(object sender, EventArgs e)
        {
            colorSliderBeginFrequency1.Value -= 1;
            UpdateCurrentWaveInfo();
        }

        private void buttonBeginFreqPlus_Click(object sender, EventArgs e)
        {
            colorSliderBeginFrequency1.Value += 10;
            UpdateCurrentWaveInfo();
        }

        private void buttonFreqMinus_Click(object sender, EventArgs e)
        {
            colorSliderEndFrequency1.Value -= 1;
            UpdateCurrentWaveInfo();
        }

        private void buttonFreqPlus_Click(object sender, EventArgs e)
        {
            colorSliderEndFrequency1.Value += 1;
            UpdateCurrentWaveInfo();
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

        private void buttonPlayResult_Click(object sender, EventArgs e)
        {
            synthGenerator.Play();
        }

        private void buttonPlayContinuous2_Click(object sender, EventArgs e)
        {
            if (buttonPlayContinuous.Text.Equals("Play Continuous"))
            {
                buttonPlayContinuous.Text = "Stop playing";
                aTimer.Interval = (int)(synthGenerator.CurrentWave.Duration() * 1000);
                aTimer.Tick += new EventHandler(TimerEventProcessor);
                aTimer.Enabled = true;
            }
            else
            {
                buttonPlayContinuous.Text = "Play Continuous";
                aTimer.Enabled = false;
            }
        }

        private void buttonSaveWav_Click(object sender, EventArgs e)
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

        private void buttonPlayWave_Click(object sender, EventArgs e)
        {
            synthGenerator.Play(false);
            synthGenerator.GenerateSound();     // play single wave overwrites mix buffer
        }

        private void buttonAddNewWave_Click(object sender, EventArgs e)
        {
            synthGenerator.CurrentWave = synthGenerator.CloneWave();
            WavesListItem item = new WavesListItem(synthGenerator.CurrentWave.Number, synthGenerator.CurrentWave.Name);
            listboxWavesItems.Add(item);
            bindingListboxWaves.ResetBindings(false);
            listBoxWaves.SelectedIndex = listboxWavesItems.IndexOf(item);
            UpdateCurrentWaveInfo();
        }

        private void listBoxWaves_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxWaves.SelectedItem != null)
            {
                synthGenerator.SetCurrentWaveNumber(((WavesListItem)listBoxWaves.SelectedItem).Number);
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
            WavesListItem item = new WavesListItem(synthGenerator.CurrentWave.Number, synthGenerator.CurrentWave.Name);
            synthGenerator.RemoveCurrentWave();
            int current_index = listboxWavesItems.IndexOf(item);
            listboxWavesItems.Remove(item);
            bindingListboxWaves.ResetBindings(false);
            if (listBoxWaves.Items.Count > current_index)
            {
                listBoxWaves.SelectedIndex = current_index;
            }
            else
            {
                listBoxWaves.SelectedIndex = current_index - 1;
            }
            synthGenerator.SetCurrentWaveNumber(((WavesListItem)listBoxWaves.SelectedItem).Number);
            UpdateCurrentWaveInfo();
            UpdateWaveControls();
        }

        private void buttonDeleteAll_Click(object sender, EventArgs e)
        {
            listboxWavesItems.Clear();
            synthGenerator.Waves.Clear();
            bindingListboxWaves.ResetBindings(false);

            // Init generators
            WaveInfo newWave = new WaveInfo();
            synthGenerator.Waves.Add(newWave);
            synthGenerator.CurrentWave = newWave;

            WavesListItem item = new WavesListItem(synthGenerator.CurrentWave.Number, synthGenerator.CurrentWave.Name);
            listboxWavesItems.Add(item);
            bindingListboxWaves.ResetBindings(false);
            listBoxWaves.SelectedIndex = listboxWavesItems.IndexOf(item);
            UpdateCurrentWaveInfo();
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
                    double previous_deviation = (i-1) / (double)numericUpDownInHarmonics.Value * (double)numericUpDownInHarmonicsSpread.Value / 100.0;
                    double place = random.Next(1000) / 1000.0;
                    deviation = (previous_deviation * place) + (deviation * (1 - place));
                }
                double frequency_factor = 1 + deviation;
                if (i % 2 == 0)
                {
                    frequency_factor = 1 / frequency_factor;
                }

                if ((frequency_factor * synthGenerator.CurrentWave.BeginFrequency) < MAX_FREQUENCY && (frequency_factor * synthGenerator.CurrentWave.EndFrequency) < MAX_FREQUENCY)
                {
                    double amplitudeFactor = 1 / Math.Pow(1 + ((double)numericUpDownInHarmonicsDecay.Value / 100.0), i);
                    WaveInfo newWave = synthGenerator.CloneWave(frequency_factor, amplitudeFactor);
                    WavesListItem item = new WavesListItem(newWave.Number, newWave.Name);
                    listboxWavesItems.Add(item);
                }
            }
            bindingListboxWaves.ResetBindings(false);
            synthGenerator.GenerateSound();
        }

        private void buttonCreateEvenHarmonics_Click(object sender, EventArgs e)
        {
            for (int i = 1; i <= numericUpDownHarmonicsEven.Value; i++)
            {
                if ((i * 2 * synthGenerator.CurrentWave.BeginFrequency) < MAX_FREQUENCY && (i * 2 * synthGenerator.CurrentWave.EndFrequency) < MAX_FREQUENCY)
                {
                    double amplitudeFactor = 1 / Math.Pow(1 + ((double)numericUpDownHarmDecayEven.Value / 100.0), i);
                    WaveInfo newWave = synthGenerator.CloneWave(i * 2, amplitudeFactor);
                    WavesListItem item = new WavesListItem(newWave.Number, newWave.Name);
                    listboxWavesItems.Add(item); 
                }
            }
            bindingListboxWaves.ResetBindings(false);
            synthGenerator.GenerateSound();
        }

        private void buttonCreateOddHarmonics_Click(object sender, EventArgs e)
        {
            for (int i = 1; i <= numericUpDownHarmonicsOdd.Value; i++)
            {
                if (((i * 2 + 1) * synthGenerator.CurrentWave.BeginFrequency) < MAX_FREQUENCY && ((i * 2 + 1) * synthGenerator.CurrentWave.EndFrequency) < MAX_FREQUENCY)
                {
                    double amplitudeFactor = 1 / Math.Pow(1 + ((double)numericUpDownHarmDecayOdd.Value / 100.0), i);
                    WaveInfo newWave = synthGenerator.CloneWave(i * 2 + 1, amplitudeFactor);
                    WavesListItem item = new WavesListItem(newWave.Number, newWave.Name);
                    listboxWavesItems.Add(item);
                }
            }
            bindingListboxWaves.ResetBindings(false);
            synthGenerator.GenerateSound();
        }

        private void buttonCreateSpikes_Click(object sender, EventArgs e)
        {
            WaveInfo newWave = new WaveInfo();
            newWave.BeginFrequency = synthGenerator.CurrentWave.BeginFrequency;
            newWave.BeginVolume = synthGenerator.CurrentWave.BeginVolume;
            newWave.BeginEndBeginFrequencyEnabled = synthGenerator.CurrentWave.BeginEndBeginFrequencyEnabled;
            newWave.BeginEndBeginVolumeEnabled = synthGenerator.CurrentWave.BeginEndBeginVolumeEnabled;
            newWave.EndFrequency = synthGenerator.CurrentWave.EndFrequency;
            newWave.EndFrequencyEnabled = synthGenerator.CurrentWave.EndFrequencyEnabled;
            newWave.EndVolume = synthGenerator.CurrentWave.EndVolume;
            newWave.EndVolumeEnabled = synthGenerator.CurrentWave.EndVolumeEnabled;
            newWave.WaveForm = "Custom";

            // TODO : add waveformdata equally devided with amplitude based on currentwave wavedata
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

        private void buttonBeginFreqPlus1_Click(object sender, EventArgs e)
        {
            colorSliderBeginFrequency1.Value += 1;
            UpdateCurrentWaveInfo();
        }

        private void buttonBeginFreqMinus10_Click(object sender, EventArgs e)
        {
            colorSliderBeginFrequency1.Value -= 10;
            UpdateCurrentWaveInfo();
        }

        private void buttonFreqMinus10_Click(object sender, EventArgs e)
        {
            colorSliderEndFrequency1.Value -= 10;
            UpdateCurrentWaveInfo();
        }

        private void buttonFreqPlus10_Click(object sender, EventArgs e)
        {
            colorSliderEndFrequency1.Value += 10;
            UpdateCurrentWaveInfo();
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
            FormCustomWave formCustomWave = new FormCustomWave();
            formCustomWave.MyParent = this;
            formCustomWave.ShowDialog();
        }

        private void pictureBoxCustomWave_Paint(object sender, PaintEventArgs e)
        {
            Control control = (Control)sender;
            using (LinearGradientBrush brush = new LinearGradientBrush(control.ClientRectangle,
                                                                       Color.FromArgb(70, 87, 195),
                                                                       Color.FromArgb(0, 0, 65),
                                                                       90F))
            {
                e.Graphics.FillRectangle(brush, control.ClientRectangle);
                ControlPaint.DrawBorder(e.Graphics, control.ClientRectangle, Color.Gray, ButtonBorderStyle.Solid);
            }

            Pen pen = new Pen(Color.White);

            if (synthGenerator.CurrentWave.WaveFormData.Length==SynthGenerator.NUM_CUSTOMWAVE_POINTS)
            {
                for (int x=0; x<pictureBoxCustomWave.Width; x++)
                {
                    int position = (int)(x / (double)pictureBoxCustomWave.Width * SynthGenerator.NUM_CUSTOMWAVE_POINTS);
                    int next_position = (int)((x+1) / (double)pictureBoxCustomWave.Width * SynthGenerator.NUM_CUSTOMWAVE_POINTS);
                    if (next_position < SynthGenerator.NUM_CUSTOMWAVE_POINTS)
                    {
                        int value1 = (int)((synthGenerator.CurrentWave.WaveFormData[position] + SynthGenerator.CUSTOMWAVE_MAX_VALUE) * (pictureBoxCustomWave.Height / (2.0 * SynthGenerator.CUSTOMWAVE_MAX_VALUE + 1)));
                        int value2 = (int)((synthGenerator.CurrentWave.WaveFormData[next_position] + SynthGenerator.CUSTOMWAVE_MAX_VALUE) * (pictureBoxCustomWave.Height / (2.0 * SynthGenerator.CUSTOMWAVE_MAX_VALUE + 1)));
                        e.Graphics.DrawLine(pen, new Point(x, value1), new Point(x + 1, value2));
                    }
                }
            }
        }
    }
}
