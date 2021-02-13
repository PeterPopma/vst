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
        Timer aTimer = new Timer();
        bool generatorEnabled = false;
        bool changedPresetData = false;
        bool isPlaying = false;

        Random random = new Random();
        SynthGenerator synthGenerator;
        Preset preset = new Preset();
        string currentPreset = "";

        internal SynthGenerator SynthGenerator { get => synthGenerator; set => synthGenerator = value; }
        public bool ChangedPresetData { get => changedPresetData; set => changedPresetData = value; }

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
            textBoxDuration.Text = string.Format("{0:0.0000}", (double)colorSliderDuration.Value / SynthGenerator.SAMPLES_PER_SECOND);
            textBoxDelay.Text = string.Format("{0:0.0000}", (double)colorSliderDelay.Value / SynthGenerator.SAMPLES_PER_SECOND);
            colorSliderDelay.Value = synthGenerator.CurrentWave.StartPosition;
            colorSliderWeight1.Value = synthGenerator.CurrentWave.Weight;
            UpdateChannelText();

            if (synthGenerator.CurrentWave.WaveForm.Equals("WavFile"))
            {
                labelWaveForm.Text = "File (.wav)";
            }
            else
            {
                labelWaveForm.Text = synthGenerator.CurrentWave.WaveForm;
            }
            UpdateWaveFormPicture();

            colorSliderAttack.Value = (int)synthGenerator.EnvelopAttack * 100;
            colorSliderHold.Value = (int)synthGenerator.EnvelopHold * 100;
            colorSliderDecay.Value = (int)synthGenerator.EnvelopDecay * 100;
            colorSliderSustain.Value = (int)synthGenerator.EnvelopSustain * 100;
            colorSliderRelease.Value = (int)synthGenerator.EnvelopRelease * 100;

            labelVolumeMin.Text = synthGenerator.CurrentWave.MinVolume.ToString();
            labelVolumeMax.Text = synthGenerator.CurrentWave.MaxVolume.ToString();
            labelFrequencyMin.Text = synthGenerator.CurrentWave.MinFrequency.ToString("0.00");
            labelFrequencyMax.Text = synthGenerator.CurrentWave.MaxFrequency.ToString("0.00");

            UpdateVisibility();

            generatorEnabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            groupBox1.Paint += new PaintEventHandler(GroupBoxPaint);
            groupBox1.Refresh();

            groupBox3.Paint += new PaintEventHandler(GroupBoxPaint);
            groupBox3.Refresh();

            groupBox5.Paint += new PaintEventHandler(GroupBoxPaint);
            groupBox5.Refresh();

            groupBox6.Paint += new PaintEventHandler(GroupBoxPaint);
            groupBox6.Refresh();

            groupBox4.Paint += new PaintEventHandler(GroupBoxPaint);
            groupBox4.Refresh();

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

            synthGenerator.UpdateAllWaveData();
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

        public void UpdateMixedSound()
        {
            if (generatorEnabled)
            {
                ChangedPresetData = true;
                synthGenerator.UpdateMixedSound();
            }
        }

        private void TimerEventProcessor(Object myObject,
                                                   EventArgs myEventArgs)
        {
            synthGenerator.Play();
        }

        private void labelChannel1_Click(object sender, EventArgs e)
        {
            synthGenerator.CurrentWave.Channel++;
            if (synthGenerator.CurrentWave.Channel == 3)
            {
                synthGenerator.CurrentWave.Channel = 0;
            }
            UpdateChannelText();
            synthGenerator.UpdateCurrentWaveData();
        }

        private void colorSlider21_ValueChanged(object sender, EventArgs e)
        {
            labelWeight1.Text = string.Format("{0:0}", colorSliderWeight1.Value);
            synthGenerator.CurrentWave.Weight = (int)colorSliderWeight1.Value;
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

        private void colorSliderAttack_MouseUp(object sender, MouseEventArgs e)
        {
            UpdateMixedSound();
        }

        private void colorSliderDecay_MouseUp(object sender, MouseEventArgs e)
        {
            UpdateMixedSound();
        }

        private void colorSliderSustain_MouseUp(object sender, MouseEventArgs e)
        {
            UpdateMixedSound();
        }

        private void colorSliderRelease_MouseUp(object sender, MouseEventArgs e)
        {
            UpdateMixedSound();
        }

        private void colorSliderDuration1_MouseUp(object sender, MouseEventArgs e)
        {
            UpdateDuration();
            textBoxDuration.Text = string.Format("{0:0.0000}", (double)colorSliderDuration.Value / SynthGenerator.SAMPLES_PER_SECOND);
        }

        private void UpdateDuration()
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
            synthGenerator.UpdateCurrentWaveData();
        }

        private void colorSliderDelay1_MouseUp(object sender, MouseEventArgs e)
        {
            synthGenerator.CurrentWave.StartPosition = (int)(colorSliderDelay.Value);
            synthGenerator.UpdateCurrentWaveData();
            textBoxDelay.Text = string.Format("{0:0.0000}", ((double)colorSliderDelay.Value / SynthGenerator.SAMPLES_PER_SECOND));
        }

        private void colorSliderWeight1_MouseUp(object sender, MouseEventArgs e)
        {
            synthGenerator.UpdateCurrentWaveData();
        }

        private void comboBoxPresets_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangedPresetData = false;
            currentPreset = comboBoxPresets.Text;
            preset.Load(synthGenerator, currentPreset);
            RecreateWavesLists();
            UpdateWaveControls();
            synthGenerator.UpdateAllWaveData();
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
                /*
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
                */
                case "Noise":
                    synthGenerator.CurrentWave.WaveForm = "WavFile";
                    break;
                case "WavFile":
                    synthGenerator.CurrentWave.WaveForm = "Custom";
                    break;
                case "Custom":
                    synthGenerator.CurrentWave.WaveForm = "Noise";
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

            UpdateMixedSound();
        }

        private void UpdateVisibility()
        {
            if (synthGenerator.CurrentWave.WaveForm.Equals("WavFile") || synthGenerator.CurrentWave.WaveForm.Equals("Noise"))
            {
                labelFrequencyMax.Visible = false;
                labelFrequencyMin.Visible = false;
                buttonCreatePartials.Enabled = false;
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
                buttonCreatePartials.Enabled = true;
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
            synthGenerator.UpdateCurrentWaveData();
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
            UpdateMixedSound();
        }

        private void buttonSavePreset2_Click(object sender, EventArgs e)
        {
            if (currentPreset.Length > 0)
            {
                ChangedPresetData = false;
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

        public void RecreateWavesLists()
        {
            listBoxWaves.Items.Clear();
            foreach (WaveInfo wave in synthGenerator.Waves)
            {
                listBoxWaves.Items.Add(wave.DisplayName());
            }
            listBoxWaves.SelectedIndex = listBoxWaves.FindStringExact(synthGenerator.CurrentWave.DisplayName());
        }

        public void UpdateWavesList()
        {
            listBoxWaves.Items[listBoxWaves.SelectedIndex] = synthGenerator.CurrentWave.DisplayName();
            /*
            for (int index=0; index<listBoxWaves.Items.Count; index++)
            {
                WaveInfo wave = synthGenerator.FindWaveByNamePart(listBoxWaves.Items[index].ToString());
                listBoxWaves.Items[index] = wave.DisplayName();
            }
            */
        }

        private void AddWaveToLists(WaveInfo newWave)
        {
            listBoxWaves.Items.Add(newWave.DisplayName());
        }

        private void RemoveWaveFromList(WaveInfo waveInfo)
        {
            listBoxWaves.Items.RemoveAt(listBoxWaves.FindStringExact(waveInfo.DisplayName()));
        }

        private void buttonAddNewWave_Click(object sender, EventArgs e)
        {
            synthGenerator.CurrentWave = synthGenerator.CloneWave();
            AddWaveToLists(synthGenerator.CurrentWave);
            listBoxWaves.SelectedIndex = listBoxWaves.FindStringExact(synthGenerator.CurrentWave.DisplayName());
            UpdateMixedSound();
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
            synthGenerator.SetCurrentWaveByDisplayName(listBoxWaves.SelectedItem.ToString());
            synthGenerator.UpdateCurrentWaveData();
            UpdateWaveControls();
        }

        private void buttonDeleteAll_Click(object sender, EventArgs e)
        {
            while (synthGenerator.Waves.Count > 1)
            {
                synthGenerator.Waves.RemoveAt(synthGenerator.Waves.Count - 1);
            }
            synthGenerator.CurrentWave = synthGenerator.Waves[0];
            listBoxWaves.SelectedIndex = 0;
            RecreateWavesLists();
            synthGenerator.UpdateCurrentWaveData();
            UpdateWaveControls();
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
            if (ChangedPresetData)
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

        private void buttonPlayContinuous(object sender, EventArgs e)
        {
            if (!isPlaying)
            {
                synthGenerator.Play();
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
            synthGenerator.UpdateMixedSound();     // play single wave overwrites mix buffer
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
            if (colorSliderDuration.Value > 9)
            {
                colorSliderDuration.Value -= 10;
                synthGenerator.CurrentWave.WaveData = new double[(int)(colorSliderDuration.Value) * SynthGenerator.NUM_AUDIO_CHANNELS];
                synthGenerator.UpdateCurrentWaveData();
                textBoxDuration.Text = string.Format("{0:0.0000}", (double)colorSliderDuration.Value / SynthGenerator.SAMPLES_PER_SECOND);
            }
        }

        private void buttonDurationMinus10_Click(object sender, EventArgs e)
        {
            if (colorSliderDuration.Value > 99)
            {
                colorSliderDuration.Value -= 100;
                synthGenerator.CurrentWave.WaveData = new double[(int)(colorSliderDuration.Value) * SynthGenerator.NUM_AUDIO_CHANNELS];
                synthGenerator.UpdateCurrentWaveData();
                textBoxDuration.Text = string.Format("{0:0.0000}", (double)colorSliderDuration.Value / SynthGenerator.SAMPLES_PER_SECOND);
            }
        }

        private void buttonDurationPlus10_Click(object sender, EventArgs e)
        {
            if (colorSliderDuration.Value + 100 <= colorSliderDuration.Maximum)
            {
                colorSliderDuration.Value += 100;
                synthGenerator.CurrentWave.WaveData = new double[(int)(colorSliderDuration.Value) * SynthGenerator.NUM_AUDIO_CHANNELS];
                synthGenerator.UpdateCurrentWaveData();
                textBoxDuration.Text = string.Format("{0:0.0000}", (double)colorSliderDuration.Value / SynthGenerator.SAMPLES_PER_SECOND);
            }
        }

        private void buttonDurationPlus1_Click(object sender, EventArgs e)
        {
            if (colorSliderDuration.Value + 10 <= colorSliderDuration.Maximum)
            {
                colorSliderDuration.Value += 10;
                synthGenerator.CurrentWave.WaveData = new double[(int)(colorSliderDuration.Value) * SynthGenerator.NUM_AUDIO_CHANNELS];
                synthGenerator.UpdateCurrentWaveData();
                textBoxDuration.Text = string.Format("{0:0.0000}", (double)colorSliderDuration.Value / SynthGenerator.SAMPLES_PER_SECOND);
            }
        }

        private void buttonDelayMinus10_Click(object sender, EventArgs e)
        {
            if (colorSliderDelay.Value > 99)
            {
                colorSliderDelay.Value -= 100;
                synthGenerator.CurrentWave.StartPosition = (int)(colorSliderDelay.Value);
                synthGenerator.UpdateCurrentWaveData();
                textBoxDelay.Text = string.Format("{0:0.0000}", (double)colorSliderDelay.Value / SynthGenerator.SAMPLES_PER_SECOND);

            }
        }

        private void buttonDelayMinus1_Click(object sender, EventArgs e)
        {
            if (colorSliderDelay.Value > 9)
            {
                colorSliderDelay.Value -= 10;
                synthGenerator.CurrentWave.StartPosition = (int)(colorSliderDelay.Value);
                synthGenerator.UpdateCurrentWaveData();
                textBoxDelay.Text = string.Format("{0:0.0000}", (double)colorSliderDelay.Value / SynthGenerator.SAMPLES_PER_SECOND);
            }
        }

        private void buttonDelayPlus1_Click(object sender, EventArgs e)
        {
            if (colorSliderDelay.Value + 10 <= colorSliderDelay.Maximum)
            {
                colorSliderDelay.Value += 10;
                synthGenerator.CurrentWave.StartPosition = (int)(colorSliderDelay.Value);
                synthGenerator.UpdateCurrentWaveData();
                textBoxDelay.Text = string.Format("{0:0.0000}", (double)colorSliderDelay.Value / SynthGenerator.SAMPLES_PER_SECOND);
            }
        }

        private void buttonDelayPlus10_Click(object sender, EventArgs e)
        {
            if (colorSliderDelay.Value + 100 <= colorSliderDelay.Maximum)
            {
                colorSliderDelay.Value += 100;
                synthGenerator.CurrentWave.StartPosition = (int)(colorSliderDelay.Value);
                synthGenerator.UpdateCurrentWaveData();
                textBoxDelay.Text = string.Format("{0:0.0000}", (double)colorSliderDelay.Value / SynthGenerator.SAMPLES_PER_SECOND);
            }
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
                                                                       Color.FromArgb(70, 195, 87),
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

        private void CreateInharmonics()
        {
            if(!radioButtonUnderTones.Checked)
            {
                CreateInharmonics(false);
            }
            if (!radioButtonOverTones.Checked)
            {
                CreateInharmonics(true);
            }
        }

        private void CreateInharmonics(bool undertones)
        {
            for (int inharmonic_number = 1; inharmonic_number <= numericUpDownAmount.Value; inharmonic_number++)
            {
                double durationFactor = Math.Pow(((double)numericUpDownDuration.Value / 100.0), inharmonic_number);
                double deviation = inharmonic_number / (double)numericUpDownAmount.Value * (double)numericUpDownSpread.Value / 100.0;
                if (checkBoxRandomFrequency.Checked)
                {
                    // choose a random spot between this and the previous deviation
                    double previous_deviation = (inharmonic_number - 1) / (double)numericUpDownAmount.Value * (double)numericUpDownSpread.Value / 100.0;
                    double place = random.Next(1000) / 1000.0;
                    deviation = (previous_deviation * place) + (deviation * (1 - place));
                }
                double frequency_factor = 1 + deviation;
                if (undertones)
                {
                    frequency_factor = 1 / frequency_factor;
                }

                if ((frequency_factor * synthGenerator.CurrentWave.MinFrequency) < SynthGenerator.MAX_FREQUENCY && (frequency_factor * synthGenerator.CurrentWave.MaxFrequency) < SynthGenerator.MAX_FREQUENCY)
                {
                    double amplitudeDecayFactor = 1 / Math.Pow(1 + ((double)numericUpDownVolumeDecay.Value / 100.0), inharmonic_number);
                    if (checkBoxRandomDecay.Checked)
                    {
                        double previousAmplitudeDecayFactor = 1 / Math.Pow(1 + ((double)numericUpDownVolumeDecay.Value / 100.0), inharmonic_number - 1);
                        amplitudeDecayFactor = previousAmplitudeDecayFactor + random.NextDouble() * (amplitudeDecayFactor - previousAmplitudeDecayFactor);
                    }
                    WaveInfo newWave = synthGenerator.CloneWave(frequency_factor, amplitudeDecayFactor);
                    newWave.SetDuration(newWave.Duration() * durationFactor);
                    AddWaveToLists(newWave);
                }
            }
        }

        private void CreateHarmonic(int harmonic_number, double frequency_factor)
        {
            double amplitudeFactor = Math.Pow(((double)numericUpDownVolume.Value / 100.0), harmonic_number);
            double durationFactor = Math.Pow(((double)numericUpDownDuration.Value / 100.0), harmonic_number);
            double amplitudeDecayFactor = 1 / Math.Pow(1 + ((double)numericUpDownVolumeDecay.Value / 100.0), harmonic_number);
            if (checkBoxRandomDecay.Checked)
            {
                double previousAmplitudeDecayFactor = 1 / Math.Pow(1 + ((double)numericUpDownVolumeDecay.Value / 100.0), harmonic_number - 1);
                amplitudeDecayFactor = previousAmplitudeDecayFactor + random.NextDouble() * (amplitudeDecayFactor - previousAmplitudeDecayFactor);
            }
            WaveInfo newWave = synthGenerator.CloneWave(frequency_factor, amplitudeFactor);
            newWave.MinVolume = (int)(amplitudeDecayFactor * newWave.MinVolume);
            newWave.MaxVolume = (int)(amplitudeDecayFactor * newWave.MaxVolume);
            newWave.SetDuration(newWave.Duration() * durationFactor);
            AddWaveToLists(newWave);
        }

        private void CreateHarmonics(int startFactor)
        {
            int num_undertones = radioButtonOverTones.Checked ? 0 : (int)numericUpDownAmount.Value;
            int num_overtones = radioButtonUnderTones.Checked ? 0 : (int)numericUpDownAmount.Value;

            int harmonic_factor = startFactor;
            for (int harmonic_number=1; harmonic_number<=num_undertones; harmonic_number++)
            {
                CreateHarmonic(harmonic_number, 1 / (double)harmonic_factor);
                harmonic_factor += 2;
            }

            harmonic_factor = startFactor;
            for (int harmonic_number = 1; harmonic_number <= num_overtones; harmonic_number++)
            {
                CreateHarmonic(harmonic_number, (double)harmonic_factor);
                harmonic_factor += 2;
            }
        }

        private void CreateSplitMerge()
        {
            double min_frequency = synthGenerator.CurrentWave.MinFrequency;
            double max_frequency = synthGenerator.CurrentWave.MinFrequency;
            FadeCurrentWave(radioButtonSplit.Checked);
            if (!radioButtonUnderTones.Checked)
            {
                CreateSplitMerge(min_frequency, max_frequency, false);
            }
            if (!radioButtonOverTones.Checked)
            {
                CreateSplitMerge(min_frequency, max_frequency, true);
            }
        }

        private void FadeCurrentWave(bool fadeOut)
        {
            synthGenerator.CurrentWave.MinVolume = 0;
            synthGenerator.CurrentWave.MaxVolume = SynthGenerator.MAX_VOLUME;
            for (int i = 0; i < synthGenerator.CurrentWave.ShapeVolume.Length; i++)
            {
                if (fadeOut)
                {
                    // fade out
                    synthGenerator.CurrentWave.ShapeVolume[i] = (int)(SynthGenerator.SHAPE_VOLUME_MAX_VALUE - i / (double)SynthGenerator.SHAPE_VOLUME_NUMPOINTS * SynthGenerator.SHAPE_VOLUME_MAX_VALUE);
                }
                else
                {
                    // fade in
                    synthGenerator.CurrentWave.ShapeVolume[i] = (int)(SynthGenerator.SHAPE_VOLUME_MAX_VALUE - (SynthGenerator.SHAPE_VOLUME_NUMPOINTS - i) / (double)SynthGenerator.SHAPE_VOLUME_NUMPOINTS * SynthGenerator.SHAPE_VOLUME_MAX_VALUE);
                }
            }
        }

        private void CreateSplitMerge(double min_frequency, double max_frequency, bool undertones)
        {
            double min_frequency_low;
            if (undertones)
            {
                min_frequency_low = min_frequency - ((double)numericUpDownSpread.Value / 100.0) * min_frequency;
            }
            else
            {
                min_frequency_low = min_frequency;
            }
            if (min_frequency_low < 0.01)
            {
                min_frequency_low = 0.01;
            }
            double min_frequency_high;
            if (!undertones)
            {
                min_frequency_high = min_frequency + ((double)numericUpDownSpread.Value / 100.0) * min_frequency;
            }
            else
            {
                min_frequency_high = min_frequency;
            }
            if (min_frequency_high > SynthGenerator.MAX_FREQUENCY)
            {
                min_frequency_high = SynthGenerator.MAX_FREQUENCY;
            }
            double max_frequency_low;
            if (undertones)
            {
                max_frequency_low = max_frequency - ((double)numericUpDownSpread.Value / 100.0) * max_frequency;
            }
            else
            {
                max_frequency_low = max_frequency;
            }
            if (max_frequency_low < 0.01)
            {
                max_frequency_low = 0.01;
            }
            double max_frequency_high;
            if (!undertones)
            {
                max_frequency_high = max_frequency + ((double)numericUpDownSpread.Value / 100.0) * max_frequency;
            }
            else
            {
                max_frequency_high = max_frequency;
            }
            if (max_frequency_high > SynthGenerator.MAX_FREQUENCY)
            {
                max_frequency_high = SynthGenerator.MAX_FREQUENCY;
            }

            for (int split_wave_number = 0; split_wave_number < numericUpDownAmount.Value; split_wave_number++)
            {
                synthGenerator.CurrentWave = synthGenerator.CloneWave();
                if (undertones)
                {
                    synthGenerator.CurrentWave.MinFrequency = min_frequency_low + (split_wave_number / (double)numericUpDownAmount.Value * (min_frequency_high - min_frequency_low));
                    synthGenerator.CurrentWave.MaxFrequency = max_frequency_low + (split_wave_number / (double)numericUpDownAmount.Value * (max_frequency_high - max_frequency_low));
                }
                else
                {
                    synthGenerator.CurrentWave.MinFrequency = min_frequency_high - (split_wave_number / (double)numericUpDownAmount.Value * (min_frequency_high - min_frequency_low));
                    synthGenerator.CurrentWave.MaxFrequency = max_frequency_high - (split_wave_number / (double)numericUpDownAmount.Value * (max_frequency_high - max_frequency_low));
                }

                // fade in/out this wave
                FadeCurrentWave(radioButtonMerge.Checked);

                AddWaveToLists(synthGenerator.CurrentWave);
            }
        }

        private void buttonCreatePartials_Click(object sender, EventArgs e)
        {
            if (radioButtonMerge.Checked || radioButtonSplit.Checked)
            {
                CreateSplitMerge();
            }
            if (radioButtonOddHarmonics.Checked)
            {
                CreateHarmonics(3);
            }
            if (radioButtonEvenHarmonics.Checked)
            {
                CreateHarmonics(2);
            }
            if (radioButtonInharmonics.Checked)
            {
                CreateInharmonics();
            }

            synthGenerator.UpdateAllWaveData();
            UpdateSelectedWave();
        }

        private void pictureBoxFrequencyShape_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBoxFrequencyShape.Cursor = new Cursor(Properties.Resources.pencil.Handle);
        }

        private void pictureBoxVolumeShape_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBoxVolumeShape.Cursor = new Cursor(Properties.Resources.pencil.Handle);
        }

        private void textBoxDuration_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                colorSliderDuration.Value = (int)(Convert.ToDecimal(textBoxDuration.Text) * Convert.ToDecimal(SynthGenerator.SAMPLES_PER_SECOND));
                UpdateDuration();
            }
            catch (Exception)
            {
            }
        }

        private void UpdateSelectedWave()
        {
            if (listBoxWaves.SelectedItem != null && generatorEnabled)
            {
                synthGenerator.SetCurrentWaveByDisplayName(listBoxWaves.SelectedItem.ToString());
                synthGenerator.UpdateWaveGraph();
                pictureBoxCustomWave.Refresh();
                pictureBoxFrequencyShape.Refresh();
                pictureBoxVolumeShape.Refresh();
                chartResultLeft.Refresh();
                chartResultRight.Refresh();
                UpdateWaveControls();
            }
        }

        private void listBoxWaves_MouseUp(object sender, MouseEventArgs e)
        {
            UpdateSelectedWave();
        }

        private void textBoxDelay_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                colorSliderDelay.Value = (int)(Convert.ToDecimal(textBoxDelay.Text) * Convert.ToDecimal(SynthGenerator.SAMPLES_PER_SECOND));
                synthGenerator.CurrentWave.StartPosition = (int)(colorSliderDelay.Value);
                synthGenerator.UpdateCurrentWaveData();
            }
            catch (Exception)
            {
            }
        }

        private void buttonChange_Click(object sender, EventArgs e)
        {
            NextWaveForm();
            synthGenerator.UpdateCurrentWaveData();
        }

        private void listBoxWaves_KeyUp(object sender, KeyEventArgs e)
        {
            UpdateSelectedWave();
        }

        private void ChangePartialsVisibility()
        {
            if (radioButtonEvenHarmonics.Checked || radioButtonOddHarmonics.Checked)
            {
                numericUpDownSpread.Enabled = false;
                checkBoxRandomFrequency.Enabled = false;
            }
            else
            {
                numericUpDownSpread.Enabled = true;
                checkBoxRandomFrequency.Enabled = true;
            }
        }

        private void radioButtonEvenHarmonics_CheckedChanged(object sender, EventArgs e)
        {
            ChangePartialsVisibility();
        }

        private void radioButtonOddHarmonics_CheckedChanged(object sender, EventArgs e)
        {
            ChangePartialsVisibility();
        }

        private void radioButtonInharmonics_CheckedChanged(object sender, EventArgs e)
        {
            ChangePartialsVisibility();
        }

        private void radioButtonSplit_CheckedChanged(object sender, EventArgs e)
        {
            ChangePartialsVisibility();
        }

        private void radioButtonMerge_CheckedChanged(object sender, EventArgs e)
        {
            ChangePartialsVisibility();
        }
    }
}
