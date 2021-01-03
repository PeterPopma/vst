using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthAnvil.Synth
{
    class Generator
    {
        int number;
        int numSamples;       
        int beginFrequency;     // in Hz
        int endFrequency;          // in Hz
        int beginVolume;     // 0..100
        int endVolume;          // 0..100
        int channel;        // 2=both, 0=left, 1=right
        string waveForm;
        string waveFile;
        bool enabled;
        WaveDataChunk waveData = new WaveDataChunk();

        public int Number { get => number; set => number = value; }
        public int NumSamples { get => numSamples; set => numSamples = value; }
        public int BeginFrequency { get => beginFrequency; set => beginFrequency = value; }
        public int EndFrequency { get => endFrequency; set => endFrequency = value; }
        public string WaveForm { get => waveForm; set => waveForm = value; }
        public int Channel { get => channel; set => channel = value; }
        public int BeginVolume { get => beginVolume; set => beginVolume = value; }
        public int EndVolume { get => endVolume; set => endVolume = value; }
        public bool Enabled { get => enabled; set => enabled = value; }
        internal WaveDataChunk WaveData { get => waveData; set => waveData = value; }
        public string WaveFile { get => waveFile; set => waveFile = value; }

        public Generator(int number, int numSamples, int beginFrequency, int endFrequency, int beginVolume, int endVolume, int channel, string waveForm, string waveFile, bool enabled)
        {
            this.number = number;
            this.numSamples = numSamples;
            this.beginFrequency = beginFrequency;
            this.endFrequency = endFrequency;
            this.beginVolume = beginVolume;
            this.endVolume = endVolume;
            this.channel = channel;
            this.waveForm = waveForm;
            this.waveFile = waveFile;
            this.enabled = enabled;
        }

        public Generator(int number)
        {
            this.number = number;
            this.enabled = false;
        }
    }
}
