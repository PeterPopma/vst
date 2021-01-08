using NAudio.Wave;
using SynthAnvil.Synth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SynthAnvil
{
    public partial class FormMain : Form
    {
        Timer aTimer = new Timer();
        bool isInitializing = true;

        SynthPlayer synthGenerator;
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

        public void UpdateGeneratorControls(Generator generator)
        {
            ColorSlider.ColorSlider colorSlider = (ColorSlider.ColorSlider)this.Controls.Find("colorSliderDuration"+generator.Number, true)[0];
            colorSlider.Value = (decimal)(synthGenerator.Generators[generator.Number - 1].NumSamples / 441.0);

            colorSlider = (ColorSlider.ColorSlider)this.Controls.Find("colorSliderBeginFrequency" + generator.Number, true)[0];
            colorSlider.Value = convertFrequencyToValue(generator.BeginFrequency);

            colorSlider = (ColorSlider.ColorSlider)this.Controls.Find("colorSliderEndFrequency" + generator.Number, true)[0];
            colorSlider.Value = convertFrequencyToValue(generator.EndFrequency);

            colorSlider = (ColorSlider.ColorSlider)this.Controls.Find("colorSliderBeginVolume" + generator.Number, true)[0];
            colorSlider.Value = (decimal)(generator.BeginVolume);

            colorSlider = (ColorSlider.ColorSlider)this.Controls.Find("colorSliderEndVolume" + generator.Number, true)[0];
            colorSlider.Value = (decimal)(generator.EndVolume);

            colorSlider = (ColorSlider.ColorSlider)this.Controls.Find("colorSliderDuration" + generator.Number, true)[0];
            colorSlider.Value = (decimal)(generator.NumSamples/441);

            colorSlider = (ColorSlider.ColorSlider)this.Controls.Find("colorSliderDelay" + generator.Number, true)[0];
            colorSlider.Value = (decimal)(generator.StartPosition/441);

            CheckBox checkBox = (CheckBox)this.Controls.Find("checkBoxEndVolume" + generator.Number, true)[0];
            checkBox.Checked = generator.EndVolumeEnabled;

            checkBox = (CheckBox)this.Controls.Find("checkBoxEndFrequency" + generator.Number, true)[0];
            checkBox.Checked = generator.EndFrequencyEnabled;

            ColorSlider.ColorSlider2 colorSlider2 = (ColorSlider.ColorSlider2)this.Controls.Find("colorSliderWeight" + generator.Number, true)[0];
            colorSlider2.Value = (decimal)(generator.Weight);

            colorSliderAttack.Value = (decimal)synthGenerator.EnvelopAttack * 100;
            colorSliderDecay.Value = (decimal)synthGenerator.EnvelopDecay * 100;
            colorSliderSustain.Value = (decimal)synthGenerator.EnvelopSustain * 100;
            colorSliderRelease.Value = (decimal)synthGenerator.EnvelopRelease * 100;
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

            synthGenerator = new SynthPlayer(this);

            // Init generators
            for (int i = 0; i < SynthPlayer.NUM_GENERATORS; i++)
            {
                synthGenerator.Generators[i] = new Generator(i + 1);
            }
            for (int i = 0; i < SynthPlayer.NUM_GENERATORS; i++)
            {
                UpdateGeneratorControls(synthGenerator.Generators[i]);
            }

            chartADSR.ChartAreas[0].AxisY.Minimum = 0;
            chartADSR.ChartAreas[0].AxisY.Maximum = 1;
            chartADSR.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
            chartADSR.ChartAreas[0].AxisY.LabelStyle.Enabled = false;

            chartGenerator1Left.ChartAreas[0].AxisY.Maximum = 40000;
            chartGenerator1Left.ChartAreas[0].AxisY.Minimum = -40000;
            chartGenerator1Left.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
            chartGenerator1Left.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
            chartGenerator1Right.ChartAreas[0].AxisY.Maximum = 40000;
            chartGenerator1Right.ChartAreas[0].AxisY.Minimum = -40000;
            chartGenerator1Right.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
            chartGenerator1Right.ChartAreas[0].AxisY.LabelStyle.Enabled = false;

            chartGenerator2Left.ChartAreas[0].AxisY.Maximum = 40000;
            chartGenerator2Left.ChartAreas[0].AxisY.Minimum = -40000;
            chartGenerator2Left.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
            chartGenerator2Left.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
            chartGenerator2Right.ChartAreas[0].AxisY.Maximum = 40000;
            chartGenerator2Right.ChartAreas[0].AxisY.Minimum = -40000;
            chartGenerator2Right.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
            chartGenerator2Right.ChartAreas[0].AxisY.LabelStyle.Enabled = false;

            chartGenerator3Left.ChartAreas[0].AxisY.Maximum = 40000;
            chartGenerator3Left.ChartAreas[0].AxisY.Minimum = -40000;
            chartGenerator3Left.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
            chartGenerator3Left.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
            chartGenerator3Right.ChartAreas[0].AxisY.Maximum = 40000;
            chartGenerator3Right.ChartAreas[0].AxisY.Minimum = -40000;
            chartGenerator3Right.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
            chartGenerator3Right.ChartAreas[0].AxisY.LabelStyle.Enabled = false;

            chartGenerator4Left.ChartAreas[0].AxisY.Maximum = 40000;
            chartGenerator4Left.ChartAreas[0].AxisY.Minimum = -40000;
            chartGenerator4Left.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
            chartGenerator4Left.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
            chartGenerator4Right.ChartAreas[0].AxisY.Maximum = 40000;
            chartGenerator4Right.ChartAreas[0].AxisY.Minimum = -40000;
            chartGenerator4Right.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
            chartGenerator4Right.ChartAreas[0].AxisY.LabelStyle.Enabled = false;

            chartResultLeft.ChartAreas[0].AxisY.Maximum = 40000;
            chartResultLeft.ChartAreas[0].AxisY.Minimum = -40000;
            chartResultLeft.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
            chartResultLeft.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
            chartResultRight.ChartAreas[0].AxisY.Maximum = 40000;
            chartResultRight.ChartAreas[0].AxisY.Minimum = -40000;
            chartResultRight.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
            chartResultRight.ChartAreas[0].AxisY.LabelStyle.Enabled = false; 

            // find all wav files
            string[] fileEntries = Directory.GetFiles("wav\\");
            foreach (string fileName in fileEntries)
            {
                if (fileName.EndsWith(".wav"))
                {
                    comboBoxWaveFile1.Items.Add(fileName.Substring(4, fileName.Length - 8));
                    comboBoxWaveFile2.Items.Add(fileName.Substring(4, fileName.Length - 8));
                    comboBoxWaveFile3.Items.Add(fileName.Substring(4, fileName.Length - 8));
                    comboBoxWaveFile4.Items.Add(fileName.Substring(4, fileName.Length - 8));
                }
            }
            comboBoxWaveform1.SelectedIndex = 0;
            comboBoxWaveform2.SelectedIndex = 0;
            comboBoxWaveform3.SelectedIndex = 0;
            comboBoxWaveform4.SelectedIndex = 0;

            comboBoxWaveFile1.SelectedIndex = 0;
            comboBoxWaveFile2.SelectedIndex = 0;
            comboBoxWaveFile3.SelectedIndex = 0;
            comboBoxWaveFile4.SelectedIndex = 0;

            // duration has been lost because of loading wave files.
            colorSliderDuration1.Value = 100;
            colorSliderDuration2.Value = 100;
            colorSliderDuration3.Value = 100;
            colorSliderDuration4.Value = 100;

            isInitializing = false;
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
            if (!isInitializing)
            {
                synthGenerator.GenerateSound();
            }
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            CreateSound();
            synthGenerator.Play();
        }

        private void TimerEventProcessor(Object myObject,
                                                   EventArgs myEventArgs)
        {
            CreateSound();
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
            synthGenerator.GenerateSound(1);
            synthGenerator.Play(1);
        }

        private void buttonPlay2_Click(object sender, EventArgs e)
        {
            synthGenerator.GenerateSound(2);
            synthGenerator.Play(2);
        }

        private void buttonPlay3_Click(object sender, EventArgs e)
        {
            synthGenerator.GenerateSound(3);
            synthGenerator.Play(3);
        }

        private void buttonPlay4_Click(object sender, EventArgs e)
        {
            synthGenerator.GenerateSound(4);
            synthGenerator.Play(4);
        }

        private float convertValueToFrequency(decimal value)
        {
            return (float)(Math.Pow((double)value, 1.27)/100.0);
        }

        private decimal convertFrequencyToValue(double frequency)
        {
            return (decimal)Math.Pow(frequency*100, 1/ 1.27);
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
        
        private void comboBoxWaveform1_SelectedIndexChanged(object sender, EventArgs e)
        {
            synthGenerator.Generators[0].WaveForm = comboBoxWaveform1.GetItemText(comboBoxWaveform1.SelectedItem);
            if (comboBoxWaveform1.Text.Equals("File (.wav)"))
            {
                comboBoxWaveFile1.Visible = true;
                colorSliderBeginFrequency1.Visible = false;
                colorSliderEndFrequency1.Visible = false;
                labelBeginFreq1.Visible = false;
                labelEndFreq1.Visible = false;
                labelBeginFrequency1.Visible = false;
                labelEndFrequency1.Visible = false;
                checkBoxEndFrequency1.Visible = false;

                synthGenerator.LoadWaveFile(synthGenerator.Generators[0], comboBoxWaveFile1.SelectedItem.ToString());
            }
            else
            {
                comboBoxWaveFile1.Visible = false;
                colorSliderBeginFrequency1.Visible = true;
                colorSliderEndFrequency1.Visible = true;
                labelBeginFreq1.Visible = true;
                labelEndFreq1.Visible = true;
                labelBeginFrequency1.Visible = true;
                labelEndFrequency1.Visible = true;
                checkBoxEndFrequency1.Visible = true;
            }
            CreateSound();
        }

        private void comboBoxWaveform2_SelectedIndexChanged(object sender, EventArgs e)
        {
            synthGenerator.Generators[1].WaveForm = comboBoxWaveform2.GetItemText(comboBoxWaveform2.SelectedItem);
            if (comboBoxWaveform2.Text.Equals("File (.wav)"))
            {
                comboBoxWaveFile2.Visible = true;

                synthGenerator.LoadWaveFile(synthGenerator.Generators[1], comboBoxWaveFile2.SelectedItem.ToString());
            }
            else
            {
                comboBoxWaveFile2.Visible = false;
            }
            CreateSound();
        }

        private void comboBoxWaveform3_SelectedIndexChanged(object sender, EventArgs e)
        {
            synthGenerator.Generators[2].WaveForm = comboBoxWaveform3.GetItemText(comboBoxWaveform3.SelectedItem);
            if (comboBoxWaveform3.Text.Equals("File (.wav)"))
            {
                comboBoxWaveFile3.Visible = true;

                synthGenerator.LoadWaveFile(synthGenerator.Generators[2], comboBoxWaveFile3.SelectedItem.ToString());
            }
            else
            {
                comboBoxWaveFile3.Visible = false;
            }
            CreateSound();
        }

        private void comboBoxWaveform4_SelectedIndexChanged(object sender, EventArgs e)
        {
            synthGenerator.Generators[3].WaveForm = comboBoxWaveform4.GetItemText(comboBoxWaveform4.SelectedItem);
            if (comboBoxWaveform4.Text.Equals("File (.wav)"))
            {
                comboBoxWaveFile4.Visible = true;

                synthGenerator.LoadWaveFile(synthGenerator.Generators[3], comboBoxWaveFile4.SelectedItem.ToString());
            }
            else
            {
                comboBoxWaveFile4.Visible = false;
            }
            CreateSound();
        }

        private void colorSliderBeginFrequency1_ValueChanged(object sender, EventArgs e)
        {
            labelBeginFrequency1.Text = convertValueToFrequency(colorSliderBeginFrequency1.Value).ToString("0.00");
            synthGenerator.Generators[0].BeginFrequency = convertValueToFrequency(colorSliderBeginFrequency1.Value);
        }

        private void colorSliderBeginFrequency1_MouseUp(object sender, MouseEventArgs e)
        {
            CreateSound();
        }

        private void colorSliderEndFrequency1_ValueChanged(object sender, EventArgs e)
        {
            labelEndFrequency1.Text = convertValueToFrequency(colorSliderEndFrequency1.Value).ToString("0.00");
            synthGenerator.Generators[0].EndFrequency = convertValueToFrequency(colorSliderEndFrequency1.Value);
        }

        private void colorSliderEndFrequency2_ValueChanged(object sender, EventArgs e)
        {
            labelEndFrequency2.Text = convertValueToFrequency(colorSliderEndFrequency2.Value).ToString("0.00");
            synthGenerator.Generators[1].EndFrequency = convertValueToFrequency(colorSliderEndFrequency2.Value);
        }

        private void colorSliderEndFrequency3_ValueChanged(object sender, EventArgs e)
        {
            labelEndFrequency3.Text = convertValueToFrequency(colorSliderEndFrequency3.Value).ToString("0.00");
            synthGenerator.Generators[2].EndFrequency = convertValueToFrequency(colorSliderEndFrequency3.Value);
        }

        private void colorSliderEndFrequency4_ValueChanged(object sender, EventArgs e)
        {
            labelEndFrequency4.Text = convertValueToFrequency(colorSliderEndFrequency4.Value).ToString("0.00");
            synthGenerator.Generators[3].EndFrequency = convertValueToFrequency(colorSliderEndFrequency4.Value);
        }

        private void colorSliderBeginFrequency2_ValueChanged(object sender, EventArgs e)
        {
            labelBeginFrequency2.Text = convertValueToFrequency(colorSliderBeginFrequency2.Value).ToString("0.00");
            synthGenerator.Generators[1].BeginFrequency = convertValueToFrequency(colorSliderBeginFrequency2.Value);
        }

        private void colorSliderBeginFrequency3_ValueChanged(object sender, EventArgs e)
        {
            labelBeginFrequency3.Text = convertValueToFrequency(colorSliderBeginFrequency3.Value).ToString("0.00");
            synthGenerator.Generators[2].BeginFrequency = convertValueToFrequency(colorSliderBeginFrequency3.Value);
        }

        private void colorSliderBeginFrequency4_ValueChanged(object sender, EventArgs e)
        {
            labelBeginFrequency4.Text = convertValueToFrequency(colorSliderBeginFrequency4.Value).ToString("0.00");
            synthGenerator.Generators[3].BeginFrequency = convertValueToFrequency(colorSliderBeginFrequency4.Value);
        }

        private void colorSliderEndFrequency1_MouseUp(object sender, MouseEventArgs e)
        {
            CreateSound();
        }

        private void colorSliderBeginFrequency2_MouseUp(object sender, MouseEventArgs e)
        {
            CreateSound();
        }

        private void colorSliderEndFrequency2_MouseUp(object sender, MouseEventArgs e)
        {
            CreateSound();
        }

        private void colorSliderEndFrequency3_MouseUp(object sender, MouseEventArgs e)
        {
            CreateSound();
        }

        private void colorSliderBeginFrequency3_MouseUp(object sender, MouseEventArgs e)
        {
            CreateSound();
        }

        private void colorSliderBeginFrequency4_MouseUp(object sender, MouseEventArgs e)
        {
            CreateSound();
        }

        private void colorSliderEndFrequency4_MouseUp(object sender, MouseEventArgs e)
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
            synthGenerator.Generators[0].Channel = GetChannel(1);
            CreateSound();
        }

        private void labelChannel2_Click(object sender, EventArgs e)
        {
            if (labelChannel2.Text.Equals("both"))
            {
                labelChannel2.Text = "left";
            }
            else if (labelChannel2.Text.Equals("left"))
            {
                labelChannel2.Text = "right";
            }
            else
            {
                labelChannel2.Text = "both";
            }
            synthGenerator.Generators[1].Channel = GetChannel(2);
            CreateSound();
        }

        private void labelChannel3_Click(object sender, EventArgs e)
        {
            if (labelChannel3.Text.Equals("both"))
            {
                labelChannel3.Text = "left";
            }
            else if (labelChannel3.Text.Equals("left"))
            {
                labelChannel3.Text = "right";
            }
            else
            {
                labelChannel3.Text = "both";
            }
            synthGenerator.Generators[2].Channel = GetChannel(3);
            CreateSound();
        }

        private void labelChannel4_Click(object sender, EventArgs e)
        {
            if (labelChannel4.Text.Equals("both"))
            {
                labelChannel4.Text = "left";
            }
            else if (labelChannel4.Text.Equals("left"))
            {
                labelChannel4.Text = "right";
            }
            else
            {
                labelChannel4.Text = "both";
            }
            synthGenerator.Generators[3].Channel = GetChannel(4);
            CreateSound();
        }

        private void colorSliderDuration1_ValueChanged(object sender, EventArgs e)
        {
            labelDuration1.Text = string.Format("{0:0.00}", ((double)colorSliderDuration1.Value / 100.0));
            synthGenerator.Generators[0].NumSamples = (int)(441 * (float)colorSliderDuration1.Value);
        }

        private void colorDuration2_ValueChanged(object sender, EventArgs e)
        {
            labelDuration2.Text = string.Format("{0:0.00}", ((double)colorSliderDuration2.Value / 100.0));
            synthGenerator.Generators[1].NumSamples = (int)(441 * (float)colorSliderDuration2.Value);
        }

        private void colorDuration3_ValueChanged(object sender, EventArgs e)
        {
            labelDuration3.Text = string.Format("{0:0.00}", ((double)colorSliderDuration3.Value / 100.0));
            synthGenerator.Generators[2].NumSamples = (int)(441 * (float)colorSliderDuration3.Value);
        }

        private void colorSliderDuration4_ValueChanged(object sender, EventArgs e)
        {
            labelDuration4.Text = string.Format("{0:0.00}", ((double)colorSliderDuration4.Value / 100.0));
            synthGenerator.Generators[3].NumSamples = (int)(441 * (float)colorSliderDuration4.Value);
        }

        private void colorSlider21_ValueChanged(object sender, EventArgs e)
        {
            labelWeight1.Text = string.Format("{0:0}", colorSliderWeight1.Value);
            synthGenerator.Generators[0].Weight = (int)colorSliderWeight1.Value;
        }

        private void colorSliderWeight2_ValueChanged(object sender, EventArgs e)
        {
            labelWeight2.Text = string.Format("{0:0}", colorSliderWeight2.Value);
            synthGenerator.Generators[1].Weight = (int)colorSliderWeight2.Value;
        }

        private void colorSliderWeight3_ValueChanged(object sender, EventArgs e)
        {
            labelWeight3.Text = string.Format("{0:0}", colorSliderWeight3.Value);
            synthGenerator.Generators[3].Weight = (int)colorSliderWeight3.Value;
        }

        private void colorSliderWeight4_ValueChanged(object sender, EventArgs e)
        {
            labelWeight4.Text = string.Format("{0:0}", colorSliderWeight4.Value);
            synthGenerator.Generators[3].Weight = (int)colorSliderWeight4.Value;
        }

        private void colorSliderBeginVolume1_ValueChanged(object sender, EventArgs e)
        {
            labelBeginVolume1.Text = colorSliderBeginVolume1.Value.ToString();
            synthGenerator.Generators[0].BeginVolume = (int)colorSliderBeginVolume1.Value;
        }

        private void colorSliderBeginVolume2_ValueChanged(object sender, EventArgs e)
        {
            labelBeginVolume2.Text = colorSliderBeginVolume2.Value.ToString();
            synthGenerator.Generators[1].BeginVolume = (int)colorSliderBeginVolume2.Value;
        }

        private void colorSliderBeginVolume3_ValueChanged(object sender, EventArgs e)
        {
            labelBeginVolume3.Text = colorSliderBeginVolume3.Value.ToString();
            synthGenerator.Generators[2].BeginVolume = (int)colorSliderBeginVolume3.Value;
        }

        private void colorSliderBeginVolume4_ValueChanged(object sender, EventArgs e)
        {
            labelBeginVolume4.Text = colorSliderBeginVolume4.Value.ToString();
            synthGenerator.Generators[3].BeginVolume = (int)colorSliderBeginVolume4.Value;
        }

        private void colorSliderEndVolume1_ValueChanged(object sender, EventArgs e)
        {
            labelEndVolume1.Text = colorSliderEndVolume1.Value.ToString();
            synthGenerator.Generators[0].EndVolume = (int)colorSliderEndVolume1.Value;
        }

        private void colorSliderEndVolume2_ValueChanged(object sender, EventArgs e)
        {
            labelEndVolume2.Text = colorSliderEndVolume2.Value.ToString();
            synthGenerator.Generators[1].EndVolume = (int)colorSliderEndVolume2.Value;
        }

        private void colorSliderEndVolume3_ValueChanged(object sender, EventArgs e)
        {
            labelEndVolume3.Text = colorSliderEndVolume3.Value.ToString();
            synthGenerator.Generators[2].EndVolume = (int)colorSliderEndVolume3.Value;
        }

        private void colorSliderEndVolume4_ValueChanged(object sender, EventArgs e)
        {
            labelEndVolume4.Text = colorSliderEndVolume4.Value.ToString();
            synthGenerator.Generators[3].EndVolume = (int)colorSliderEndVolume4.Value;
            CreateSound();
        }

        private void comboBoxWaveFile1_SelectedIndexChanged(object sender, EventArgs e)
        {
            synthGenerator.LoadWaveFile(synthGenerator.Generators[0], comboBoxWaveFile1.SelectedItem.ToString());
            CreateSound();
        }

        private void comboBoxWaveFile2_SelectedIndexChanged(object sender, EventArgs e)
        {
            synthGenerator.LoadWaveFile(synthGenerator.Generators[1], comboBoxWaveFile2.SelectedItem.ToString());
            CreateSound();
        }

        private void comboBoxWaveFile3_SelectedIndexChanged(object sender, EventArgs e)
        {
            synthGenerator.LoadWaveFile(synthGenerator.Generators[2], comboBoxWaveFile3.SelectedItem.ToString());
            CreateSound();
        }

        private void comboBoxWaveFile4_SelectedIndexChanged(object sender, EventArgs e)
        {
            synthGenerator.LoadWaveFile(synthGenerator.Generators[3], comboBoxWaveFile4.SelectedItem.ToString());
            CreateSound();
        }

        private void colorSliderEndVolume3_Scroll(object sender, ScrollEventArgs e)
        {
            checkBoxEndVolume3.Checked = true;
        }

        private void colorSliderEndVolume4_Scroll(object sender, ScrollEventArgs e)
        {
            checkBoxEndVolume4.Checked = true;
        }

        private void colorSliderDelay1_ValueChanged(object sender, EventArgs e)
        {
            labelDelay1.Text = string.Format("{0:0.00}", ((double)colorSliderDelay1.Value / 100.0));
            synthGenerator.Generators[0].StartPosition = (int)(441 * (float)colorSliderDelay1.Value);
        }

        private void colorSliderDelay2_ValueChanged(object sender, EventArgs e)
        {
            labelDelay2.Text = string.Format("{0:0.00}", ((double)colorSliderDelay2.Value / 100.0));
            synthGenerator.Generators[1].StartPosition = (int)(441 * (float)colorSliderDelay2.Value);
        }

        private void colorSliderDelay3_ValueChanged(object sender, EventArgs e)
        {
            labelDelay3.Text = string.Format("{0:0.00}", ((double)colorSliderDelay3.Value / 100.0));
            synthGenerator.Generators[2].StartPosition = (int)(441 * (float)colorSliderDelay3.Value);
        }

        private void colorSliderDelay4_ValueChanged(object sender, EventArgs e)
        {
            labelDelay4.Text = string.Format("{0:0.00}", ((double)colorSliderDelay4.Value / 100.0));
            synthGenerator.Generators[3].StartPosition = (int)(441 * (float)colorSliderDelay4.Value);
        }

        private void colorSliderEndFrequency1_Scroll(object sender, ScrollEventArgs e)
        {
            checkBoxEndFrequency1.Checked = true;
        }

        private void colorSliderEndFrequency2_Scroll(object sender, ScrollEventArgs e)
        {
            checkBoxEndFrequency2.Checked = true;
        }

        private void colorSliderEndFrequency3_Scroll(object sender, ScrollEventArgs e)
        {
            checkBoxEndFrequency3.Checked = true;
        }

        private void colorSliderEndFrequency4_Scroll(object sender, ScrollEventArgs e)
        {
            checkBoxEndFrequency4.Checked = true;
        }

        private void colorSliderEndVolume1_Scroll(object sender, ScrollEventArgs e)
        {
            checkBoxEndVolume1.Checked = true;
        }

        private void colorSliderEndVolume2_Scroll(object sender, ScrollEventArgs e)
        {
            checkBoxEndVolume2.Checked = true;
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

        private void colorSliderEndVolume2_MouseUp(object sender, MouseEventArgs e)
        {
            CreateSound();
        }

        private void colorSliderEndVolume3_MouseUp(object sender, MouseEventArgs e)
        {
            CreateSound();
        }

        private void colorSliderEndVolume4_MouseUp(object sender, MouseEventArgs e)
        {
            CreateSound();
        }

        private void colorSliderBeginVolume1_MouseUp(object sender, MouseEventArgs e)
        {
            CreateSound();
        }

        private void colorSliderBeginVolume2_MouseUp(object sender, MouseEventArgs e)
        {
            CreateSound();
        }

        private void colorSliderBeginVolume3_MouseUp(object sender, MouseEventArgs e)
        {
            CreateSound();
        }

        private void colorSliderBeginVolume4_MouseUp(object sender, MouseEventArgs e)
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

        private void colorSliderDuration2_MouseUp(object sender, MouseEventArgs e)
        {
            CreateSound();
        }

        private void colorSliderDuration3_MouseUp(object sender, MouseEventArgs e)
        {
            CreateSound();
        }

        private void colorSliderDuration4_MouseUp(object sender, MouseEventArgs e)
        {
            CreateSound();
        }

        private void colorSliderDelay1_MouseUp(object sender, MouseEventArgs e)
        {
            CreateSound();
        }

        private void colorSliderDelay2_MouseUp(object sender, MouseEventArgs e)
        {
            CreateSound();
        }

        private void colorSliderDelay3_MouseUp(object sender, MouseEventArgs e)
        {
            CreateSound();
        }

        private void colorSliderDelay4_MouseUp(object sender, MouseEventArgs e)
        {
            CreateSound();
        }

        private void colorSliderWeight1_MouseUp(object sender, MouseEventArgs e)
        {
            CreateSound();
        }

        private void colorSliderWeight2_MouseUp(object sender, MouseEventArgs e)
        {
            CreateSound();
        }

        private void colorSliderWeight3_MouseUp(object sender, MouseEventArgs e)
        {
            CreateSound();
        }

        private void colorSliderWeight4_MouseUp(object sender, MouseEventArgs e)
        {
            CreateSound();
        }

        private void numericUpDownHarmonics1_ValueChanged(object sender, EventArgs e)
        {
            synthGenerator.Generators[0].Harmonics = (int)numericUpDownHarmonics1.Value;
            CreateSound();
        }

        private void numericUpDownHarmonics2_ValueChanged(object sender, EventArgs e)
        {
            synthGenerator.Generators[1].Harmonics = (int)numericUpDownHarmonics2.Value;
            CreateSound();
        }

        private void numericUpDownHarmonics3_ValueChanged(object sender, EventArgs e)
        {
            synthGenerator.Generators[2].Harmonics = (int)numericUpDownHarmonics3.Value;
            CreateSound();
        }

        private void numericUpDownHarmonics4_ValueChanged(object sender, EventArgs e)
        {
            synthGenerator.Generators[3].Harmonics = (int)numericUpDownHarmonics4.Value;
            CreateSound();
        }

        private void checkBoxEndVolume1_CheckedChanged(object sender, EventArgs e)
        {
            synthGenerator.Generators[0].EndVolumeEnabled = checkBoxEndVolume1.Checked;
            CreateSound();
        }

        private void checkBoxEndVolume2_CheckedChanged(object sender, EventArgs e)
        {
            synthGenerator.Generators[1].EndVolumeEnabled = checkBoxEndVolume2.Checked;
            CreateSound();
        }

        private void checkBoxEndVolume3_CheckedChanged(object sender, EventArgs e)
        {
            synthGenerator.Generators[2].EndVolumeEnabled = checkBoxEndVolume3.Checked;
            CreateSound();
        }

        private void checkBoxEndVolume4_CheckedChanged(object sender, EventArgs e)
        {
            synthGenerator.Generators[3].EndVolumeEnabled = checkBoxEndVolume4.Checked;
            CreateSound();
        }

        private void checkBoxEndFrequency1_CheckedChanged(object sender, EventArgs e)
        {
            synthGenerator.Generators[0].EndFrequencyEnabled = checkBoxEndFrequency1.Checked;
            CreateSound();
        }

        private void checkBoxEndFrequency2_CheckedChanged(object sender, EventArgs e)
        {
            synthGenerator.Generators[1].EndFrequencyEnabled = checkBoxEndFrequency2.Checked;
            CreateSound();
        }

        private void checkBoxEndFrequency3_CheckedChanged(object sender, EventArgs e)
        {
            synthGenerator.Generators[2].EndFrequencyEnabled = checkBoxEndFrequency3.Checked;
            CreateSound();
        }

        private void checkBoxEndFrequency4_CheckedChanged(object sender, EventArgs e)
        {
            synthGenerator.Generators[3].EndFrequencyEnabled = checkBoxEndFrequency4.Checked;
            CreateSound();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            synthGenerator.Generators[0].HarmDecayOdd = (float)numericUpDownHarmDecayOdd.Value;
        }

        private void numericUpDownHarmDecayEven_ValueChanged(object sender, EventArgs e)
        {
            synthGenerator.Generators[0].HarmDecayEven = (float)numericUpDownHarmDecayEven.Value;
        }
    }
}
