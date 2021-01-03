using NAudio.Wave;
using SynthAnvil.Synth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        SynthPlayer synthGenerator = new SynthPlayer();
        public FormMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listBoxWaveform1.SelectedIndex = 0;
            listBoxWaveform2.SelectedIndex = 0;
            listBoxWaveform3.SelectedIndex = 0;
            listBoxWaveform4.SelectedIndex = 0;

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
            comboBoxWaveFile1.SelectedIndex = 0; 
            comboBoxWaveFile2.SelectedIndex = 0;
            comboBoxWaveFile3.SelectedIndex = 0;
            comboBoxWaveFile4.SelectedIndex = 0;

            // Init generators
            for (int i = 0; i < SynthPlayer.NUM_GENERATORS; i++)
            {
                synthGenerator.Generators[i] = new Generator(i+1);
            }
        }

        private int GetChannel(int generatorNumber)
        {
            int channel = 2;
            if (generatorNumber == 1 && radioButtonLeft1.Checked)
            {
                channel = 0;
            }
            if (generatorNumber == 1 && radioButtonRight1.Checked)
            {
                channel = 1;
            }
            if (generatorNumber == 2 && radioButtonLeft2.Checked)
            {
                channel = 0;
            }
            if (generatorNumber == 2 && radioButtonRight2.Checked)
            {
                channel = 1;
            }
            if (generatorNumber == 3 && radioButtonLeft3.Checked)
            {
                channel = 0;
            }
            if (generatorNumber == 3 && radioButtonRight3.Checked)
            {
                channel = 1;
            }
            if (generatorNumber == 4 && radioButtonLeft4.Checked)
            {
                channel = 0;
            }
            if (generatorNumber == 4 && radioButtonRight4.Checked)
            {
                channel = 1;
            }

            return channel;
        }

        // 0 = update all generators
        private void UpdateGeneratorValues(int generatorNumber)
        {
            if (generatorNumber == 0 || generatorNumber == 1)
            {
                synthGenerator.Generators[0] = new Generator(1, (int)(44100 * trackBarDuration1.Value / 10.0), (int)trackBarBeginFrequency1.Value, (int)trackBarEndFrequency1.Value, trackBarBeginVolume1.Value, trackBarEndVolume1.Value, GetChannel(1), listBoxWaveform1.GetItemText(listBoxWaveform1.SelectedItem), comboBoxWaveFile1.SelectedItem.ToString(), checkBoxEnabled1.Checked);
            }
            if (generatorNumber == 0 || generatorNumber == 2)
            {
                synthGenerator.Generators[1] = new Generator(2, (int)(44100 * trackBarDuration2.Value / 10.0), (int)trackBarBeginFrequency2.Value, (int)trackBarEndFrequency2.Value, trackBarBeginVolume2.Value, trackBarEndVolume2.Value, GetChannel(2), listBoxWaveform2.GetItemText(listBoxWaveform2.SelectedItem), comboBoxWaveFile2.SelectedItem.ToString(), checkBoxEnabled2.Checked);
            }
            if (generatorNumber == 0 || generatorNumber == 3)
            {
                synthGenerator.Generators[2] = new Generator(3, (int)(44100 * trackBarDuration3.Value / 10.0), (int)trackBarBeginFrequency3.Value, (int)trackBarEndFrequency3.Value, trackBarBeginVolume3.Value, trackBarEndVolume3.Value, GetChannel(3), listBoxWaveform3.GetItemText(listBoxWaveform3.SelectedItem), comboBoxWaveFile3.SelectedItem.ToString(), checkBoxEnabled3.Checked);
            }
            if (generatorNumber == 0 || generatorNumber == 4)
            {
                synthGenerator.Generators[3] = new Generator(4, (int)(44100 * trackBarDuration4.Value / 10.0), (int)trackBarBeginFrequency4.Value, (int)trackBarEndFrequency4.Value, trackBarBeginVolume4.Value, trackBarEndVolume4.Value, GetChannel(4), listBoxWaveform4.GetItemText(listBoxWaveform4.SelectedItem), comboBoxWaveFile4.SelectedItem.ToString(), checkBoxEnabled4.Checked);
            }
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            UpdateGeneratorValues(0);
            synthGenerator.GenerateSound(0);
        }

        private void TimerEventProcessor(Object myObject,
                                                   EventArgs myEventArgs)
        {
            UpdateGeneratorValues(0);
            synthGenerator.GenerateSound(0);
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
            UpdateGeneratorValues(1);
            synthGenerator.GenerateSound(1);
        }

        private void buttonPlay2_Click(object sender, EventArgs e)
        {
            UpdateGeneratorValues(2);
            synthGenerator.GenerateSound(2);
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            labelBeginVolume1.Text = trackBarBeginVolume1.Value.ToString();
        }

        private void comboBoxWaveFile2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void trackBarEndVolume1_ValueChanged(object sender, EventArgs e)
        {
            labelEndVolume1.Text = trackBarEndVolume1.Value.ToString();
        }

        private void trackBarBeginVolume2_ValueChanged(object sender, EventArgs e)
        {
            labelBeginVolume2.Text = trackBarBeginVolume2.Value.ToString();
        }

        private void trackBarEndVolume2_ValueChanged(object sender, EventArgs e)
        {
            labelEndVolume2.Text = trackBarEndVolume2.Value.ToString();
        }

        private void buttonPlay3_Click(object sender, EventArgs e)
        {
            UpdateGeneratorValues(3);
            synthGenerator.GenerateSound(3);
        }

        private void buttonPlay4_Click(object sender, EventArgs e)
        {
            UpdateGeneratorValues(4);
            synthGenerator.GenerateSound(4);
        }

        private void trackBarDuration1_ValueChanged(object sender, EventArgs e)
        {
            labelDuration1.Text = string.Format("{0:0.#}", (trackBarDuration1.Value/10.0));
        }

        private void checkBoxEnabled1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxEnabled1.Checked)
            {
                groupBox1.BackColor = SystemColors.Control;
            }
            else
            {
                groupBox1.BackColor = SystemColors.ControlDark;
            }
        }

        private void checkBoxEnabled2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxEnabled2.Checked)
            {
                groupBox2.BackColor = SystemColors.Control;
            }
            else
            {
                groupBox2.BackColor = SystemColors.ControlDark;
            }
        }

        private void checkBoxEnabled3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxEnabled3.Checked)
            {
                groupBox3.BackColor = SystemColors.Control;
            }
            else
            {
                groupBox3.BackColor = SystemColors.ControlDark;
            }
        }

        private void checkBoxEnabled4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxEnabled4.Checked)
            {
                groupBox4.BackColor = SystemColors.Control;
            }
            else
            {
                groupBox4.BackColor = SystemColors.ControlDark;
            }
        }

        private void trackBarBeginFrequency1_ValueChanged(object sender, EventArgs e)
        {
            labelBeginFrequency1.Text = trackBarBeginFrequency1.Value.ToString();
        }

        private void trackBarEndFrequency1_ValueChanged(object sender, EventArgs e)
        {
            labelEndFrequency1.Text = trackBarEndFrequency1.Value.ToString();
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
                UpdateGeneratorValues(0);
                synthGenerator.GenerateSound(0, saveFileDialog1.FileName);
            }
        }

        private void trackBarBeginFrequency3_ValueChanged(object sender, EventArgs e)
        {
            labelBeginFrequency3.Text = trackBarBeginFrequency3.Value.ToString();
        }

        private void trackBarBeginFrequency4_ValueChanged(object sender, EventArgs e)
        {
            labelBeginFrequency4.Text = trackBarBeginFrequency4.Value.ToString();
        }

        private void trackBarBeginFrequency2_ValueChanged(object sender, EventArgs e)
        {
            labelBeginFrequency2.Text = trackBarBeginFrequency2.Value.ToString();
        }

        private void trackBarEndFrequency2_ValueChanged(object sender, EventArgs e)
        {
            labelEndFrequency2.Text = trackBarEndFrequency2.Value.ToString();
        }

        private void trackBarEndFrequency3_ValueChanged(object sender, EventArgs e)
        {
            labelEndFrequency3.Text = trackBarEndFrequency3.Value.ToString();
        }

        private void trackBarEndFrequency4_ValueChanged(object sender, EventArgs e)
        {
            labelEndFrequency4.Text = trackBarEndFrequency4.Value.ToString();
        }

        private void trackBarDuration2_ValueChanged(object sender, EventArgs e)
        {
            labelDuration2.Text = string.Format("{0:0.#}", (trackBarDuration2.Value / 10.0));
        }

        private void trackBarDuration3_ValueChanged(object sender, EventArgs e)
        {
            labelDuration3.Text = string.Format("{0:0.#}", (trackBarDuration3.Value / 10.0));
        }

        private void trackBarDuration4_ValueChanged(object sender, EventArgs e)
        {
            labelDuration4.Text = string.Format("{0:0.#}", (trackBarDuration4.Value / 10.0));
        }


        private void button1_Click_1(object sender, EventArgs e)
        {    
            WaveOutEvent outputDevice = new WaveOutEvent();
            AudioFileReader audioFile = new AudioFileReader(@"C:\Projects\SynthAnvil\SynthAnvil\wav\ahem_x.wav");
            outputDevice.Init(audioFile);
            outputDevice.Play();
        }
    }
}
