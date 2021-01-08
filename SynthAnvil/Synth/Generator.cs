using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthAnvil.Synth
{
    public class Generator
    {
        int number;
        int numSamples;             // stereo samples are counted as one (so 1 second contains 44100 samples)
        int startPosition;          // delay in seconds * 44100 (start sample)
        float beginFrequency;     // in Hz
        float endFrequency;          // in Hz
        int beginVolume;     // 0..100
        int endVolume;          // 0..100
        int channel;        // 2=both, 0=left, 1=right
        string waveForm;
        string waveFile;
        bool endFrequencyEnabled;
        bool endVolumeEnabled;
        int weight;
        int harmonics;
        float harmDecayEven;
        float harmDecayOdd;

        WaveDataChunk waveData = new WaveDataChunk();

        public int Number { get => number; set => number = value; }
        public int NumSamples { get => numSamples; set => numSamples = value; }
        public float BeginFrequency { get => beginFrequency; set => beginFrequency = value; }
        public float EndFrequency { get => endFrequency; set => endFrequency = value; }
        public string WaveForm { get => waveForm; set => waveForm = value; }
        public int Channel { get => channel; set => channel = value; }
        public int BeginVolume { get => beginVolume; set => beginVolume = value; }
        public int EndVolume { get => endVolume; set => endVolume = value; }
        internal WaveDataChunk WaveData { get => waveData; set => waveData = value; }
        public string WaveFile { get => waveFile; set => waveFile = value; }
        public bool EndFrequencyEnabled { get => endFrequencyEnabled; set => endFrequencyEnabled = value; }
        public bool EndVolumeEnabled { get => endVolumeEnabled; set => endVolumeEnabled = value; }
        public int StartPosition { get => startPosition; set => startPosition = value; }
        public int Weight { get => weight; set => weight = value; }
        public int Harmonics { get => harmonics; set => harmonics = value; }
        public float HarmDecayEven { get => harmDecayEven; set => harmDecayEven = value; }
        public float HarmDecayOdd { get => harmDecayOdd; set => harmDecayOdd = value; }

        public Generator(int number)
        {
            this.number = number;
            this.startPosition = 0;
            this.numSamples = 44100;
            this.beginFrequency = 440;
            this.endFrequency = 440;
            this.beginVolume = 100;
            this.endVolume = 100;
            this.channel = 2;
            this.waveForm = "Sine";
            this.waveFile = "";
            this.endFrequencyEnabled = false;
            this.endVolumeEnabled = false;
            this.weight = (number < 2) ? 100 : 0;
            this.harmonics = 0;
            this.harmDecayEven = 0.7f;
            this.harmDecayOdd = 0.7f;
        }
    }
}
