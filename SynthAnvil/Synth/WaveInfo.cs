using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthAnvil.Synth
{
    public class WaveInfo
    {
        int number;
        string name;
        int startPosition;          // delay in seconds * 44100 (start sample)
        double beginFrequency;     // in Hz
        double endFrequency;          // in Hz
        int beginVolume;     // 0..100
        int endVolume;          // 0..100
        int channel;        // 2=both, 0=left, 1=right
        string waveForm;
        string waveFile;
        double[] waveData;
        int[] waveFileData = new int[0];          // data read from .wav file
        int[] waveFormData = new int[SynthGenerator.NUM_CUSTOMWAVE_POINTS];          // shape of the custom waveform. this consists of 1000 items with value between -327 and 327
        bool endFrequencyEnabled;
        bool endVolumeEnabled;
        bool beginEndBeginFrequencyEnabled;
        bool beginEndBeginVolumeEnabled;
        int weight;

        public double BeginFrequency { get => beginFrequency; set => beginFrequency = value; }
        public double EndFrequency { get => endFrequency; set => endFrequency = value; }
        public string WaveForm { get => waveForm; set => waveForm = value; }
        public int Channel { get => channel; set => channel = value; }
        public int BeginVolume { get => beginVolume; set => beginVolume = value; }
        public int EndVolume { get => endVolume; set => endVolume = value; }

        public string WaveFile { get => waveFile; set => waveFile = value; }
        public bool EndFrequencyEnabled { get => endFrequencyEnabled; set => endFrequencyEnabled = value; }
        public bool EndVolumeEnabled { get => endVolumeEnabled; set => endVolumeEnabled = value; }
        public int StartPosition { get => startPosition; set => startPosition = value; }
        public int Weight { get => weight; set => weight = value; }
        public bool BeginEndBeginFrequencyEnabled { get => beginEndBeginFrequencyEnabled; set => beginEndBeginFrequencyEnabled = value; }
        public bool BeginEndBeginVolumeEnabled { get => beginEndBeginVolumeEnabled; set => beginEndBeginVolumeEnabled = value; }
        public string Name { get => name; set => name = value; }
        public int Number { get => number; set => number = value; }
        public double[] WaveData { get => waveData; set => waveData = value; }
        public int[] WaveFileData { get => waveFileData; set => waveFileData = value; }
        public int[] WaveFormData { get => waveFormData; set => waveFormData = value; }

        public WaveInfo()
        {
            this.startPosition = 0;
            this.beginFrequency = 440;
            this.endFrequency = 440;
            this.beginVolume = 255;
            this.endVolume = 255;
            this.channel = 2;
            this.waveForm = "Sine";
            this.waveFile = "";
            this.waveData = new double[44100 * 2];
            this.endFrequencyEnabled = false;
            this.endVolumeEnabled = false;
            this.weight = 255;

            SetName();
        }

        public WaveInfo(int number, int numSamples, int startPosition, double beginFrequency, double endFrequency, int beginVolume, int endVolume, int channel, string waveForm, string waveFile, bool endFrequencyEnabled, bool endVolumeEnabled, bool beginEndBeginFrequencyEnabled, bool beginEndBeginVolumeEnabled, int weight)
        {
            this.number = number;
            this.waveData = new double[numSamples * 2];
            this.startPosition = startPosition;
            this.beginFrequency = beginFrequency;
            this.endFrequency = endFrequency;
            this.beginVolume = beginVolume;
            this.endVolume = endVolume;
            this.channel = channel;
            this.waveForm = waveForm;
            this.waveFile = waveFile;
            this.endFrequencyEnabled = endFrequencyEnabled;
            this.endVolumeEnabled = endVolumeEnabled;
            this.beginEndBeginFrequencyEnabled = beginEndBeginFrequencyEnabled;
            this.beginEndBeginVolumeEnabled = beginEndBeginVolumeEnabled;
            this.weight = weight;

            SetName();
        }

        // stereo samples are counted as one (so 1 second contains 44100 samples)
        public int NumSamples()
        {
            return waveData.Length / 2;
        }

        public void SetName()
        {
            name = startPosition + "-" + waveForm + "-" + NumSamples() + "-";
            if (waveForm.Equals("WavFile"))
            {
                name += "-" + Path.GetFileName(waveFile); 
            }
            else if (!waveForm.Equals("Noise"))
            {
                name += "-" + beginFrequency;
                if(endFrequencyEnabled)
                {
                    name += "->" + endFrequency;
                }
            }
        }

        public double Duration()
        {
            return NumSamples() / SynthGenerator.SAMPLES_PER_SECOND;
        }

        public double StartTime()
        {
            return startPosition / SynthGenerator.SAMPLES_PER_SECOND;
        }
    }
}
