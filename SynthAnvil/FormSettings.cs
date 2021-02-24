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
    public partial class FormSettings : Form
    {
        private FormMain myParent = null;

        public FormMain MyParent { get => myParent; set => myParent = value; }

        public FormSettings()
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

        private void FormSettings_Load(object sender, EventArgs e)
        {
            comboBoxSamplesPerSecond.SelectedIndex = comboBoxSamplesPerSecond.FindStringExact(myParent.SynthGenerator.SamplesPerSecond.ToString());
            comboBoxBitsPerSample.SelectedIndex = comboBoxBitsPerSample.FindStringExact(myParent.SynthGenerator.BitsPerSample.ToString());
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            int oldSamplesPerSecond = myParent.SynthGenerator.SamplesPerSecond;
            myParent.SynthGenerator.SamplesPerSecond = Convert.ToInt32(comboBoxSamplesPerSecond.Text);
            myParent.SynthGenerator.BitsPerSample = Convert.ToInt32(comboBoxBitsPerSample.Text);
            myParent.SynthGenerator.UpdateAllWaveData(myParent.SynthGenerator.SamplesPerSecond / oldSamplesPerSecond);
            Close();
        }
    }
}
