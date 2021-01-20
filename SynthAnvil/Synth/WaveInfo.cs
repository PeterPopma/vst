using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthAnvil.Synth
{
    public class WaveInfo
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
        double[] waveData;
        bool endFrequencyEnabled;
        bool endVolumeEnabled;
        bool beginEndBeginFrequencyEnabled;
        bool beginEndBeginVolumeEnabled;
        int weight;
        int harmonicsEven;
        int harmonicsOdd;
        int inHarmonics;
        float inHarmonicSpread;
        float inHarmonicDecay;
        float harmonicDecayEven;
        float harmonicDecayOdd;


        public int Number { get => number; set => number = value; }
        public int NumSamples { get => numSamples; set => numSamples = value; }
        public float BeginFrequency { get => beginFrequency; set => beginFrequency = value; }
        public float EndFrequency { get => endFrequency; set => endFrequency = value; }
        public string WaveForm { get => waveForm; set => waveForm = value; }
        public int Channel { get => channel; set => channel = value; }
        public int BeginVolume { get => beginVolume; set => beginVolume = value; }
        public int EndVolume { get => endVolume; set => endVolume = value; }

        public string WaveFile { get => waveFile; set => waveFile = value; }
        public bool EndFrequencyEnabled { get => endFrequencyEnabled; set => endFrequencyEnabled = value; }
        public bool EndVolumeEnabled { get => endVolumeEnabled; set => endVolumeEnabled = value; }
        public int StartPosition { get => startPosition; set => startPosition = value; }
        public int Weight { get => weight; set => weight = value; }
        public int HarmonicsEven { get => harmonicsEven; set => harmonicsEven = value; }
        public float HarmonicDecayEven { get => harmonicDecayEven; set => harmonicDecayEven = value; }
        public float HarmonicDecayOdd { get => harmonicDecayOdd; set => harmonicDecayOdd = value; }
        public double[] WaveData { get => waveData; set => waveData = value; }
        public int InHarmonics { get => inHarmonics; set => inHarmonics = value; }
        public float InHarmonicDecay { get => inHarmonicDecay; set => inHarmonicDecay = value; }
        public int HarmonicsOdd { get => harmonicsOdd; set => harmonicsOdd = value; }
        public float InHarmonicSpread { get => inHarmonicSpread; set => inHarmonicSpread = value; }
        public bool BeginEndBeginFrequencyEnabled { get => beginEndBeginFrequencyEnabled; set => beginEndBeginFrequencyEnabled = value; }
        public bool BeginEndBeginVolumeEnabled { get => beginEndBeginVolumeEnabled; set => beginEndBeginVolumeEnabled = value; }

        public WaveInfo()
        {
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
            this.harmonicsEven = 0;
            this.harmonicsOdd = 0;
            this.harmonicDecayEven = 20.0f;
            this.harmonicDecayOdd = 20.0f;
            this.inHarmonics = 0;
            this.inHarmonicDecay = 20.0f;
            this.inHarmonicSpread = 5.0f;
        }
    }
}
