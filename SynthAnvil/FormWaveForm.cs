using SynthAnvil.Synth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace SynthAnvil
{
    public partial class FormWaveForm : Form
    {
        private FormMain myParent = null;

        public FormMain MyParent { get => myParent; set => myParent = value; }

        public FormWaveForm()
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

        private void pictureBoxSine_Click(object sender, EventArgs e)
        {
            myParent.SynthGenerator.CurrentWave.WaveForm = "Sine";
            myParent.UpdateWaveFormPicture();
            myParent.UpdateVisibility();
            myParent.SynthGenerator.UpdateCurrentWaveData();
            Close();
        }

        private void pictureBoxSquare_Click(object sender, EventArgs e)
        {
            myParent.SynthGenerator.CurrentWave.WaveForm = "Square";
            myParent.UpdateWaveFormPicture();
            myParent.UpdateVisibility();
            myParent.SynthGenerator.UpdateCurrentWaveData();
            Close();
        }

        private void pictureBoxTriangle_Click(object sender, EventArgs e)
        {
            myParent.SynthGenerator.CurrentWave.WaveForm = "Triangle";
            myParent.UpdateWaveFormPicture();
            myParent.UpdateVisibility();
            myParent.SynthGenerator.UpdateCurrentWaveData();
            Close();
        }

        private void pictureBoxSawtooth_Click(object sender, EventArgs e)
        {
            myParent.SynthGenerator.CurrentWave.WaveForm = "Sawtooth";
            myParent.UpdateWaveFormPicture();
            myParent.UpdateVisibility();
            myParent.SynthGenerator.UpdateCurrentWaveData();
            Close();
        }

        private void pictureBoxRandom_Click(object sender, EventArgs e)
        {
            myParent.SynthGenerator.CurrentWave.WaveForm = "Noise";
            myParent.UpdateWaveFormPicture();
            myParent.UpdateVisibility();
            myParent.SynthGenerator.UpdateCurrentWaveData();
            Close();
        }

        private void pictureBoxCustom_Click(object sender, EventArgs e)
        {
            myParent.SynthGenerator.CurrentWave.WaveForm = "Custom";
            if(myParent.SynthGenerator.CurrentWave.ShapeWave.Length==0)
            {
                myParent.SynthGenerator.CurrentWave.ShapeWave = new int[SynthGenerator.SHAPE_NUMPOINTS];
                ArrayUtils.Populate(myParent.SynthGenerator.CurrentWave.ShapeWave, SynthGenerator.SHAPE_MAX_VALUE / 2);
            }
            myParent.UpdateWaveFormPicture();
            myParent.UpdateVisibility();
            myParent.SynthGenerator.UpdateCurrentWaveData();
            Close();
        }

        private void pictureBoxWav_Click(object sender, EventArgs e)
        {
            myParent.SynthGenerator.CurrentWave.WaveForm = "WavFile";
            myParent.UpdateWaveFormPicture();
            myParent.UpdateVisibility();
            myParent.SynthGenerator.UpdateCurrentWaveData();
            Close();
        }
    }
}
