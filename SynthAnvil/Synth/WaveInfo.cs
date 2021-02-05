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
        string name;
        int startPosition;          // delay in seconds * 44100 (start sample)
        double minFrequency;     // in Hz
        double maxFrequency;          // in Hz
        int minVolume;     // 0..100
        int maxVolume;          // 0..100
        int channel;        // 2=both, 0=left, 1=right
        string waveForm;
        string waveFile;
        double[] waveData;
        int[] waveFileData = new int[0];          // data read from .wav file
        int[] shapeWave = new int[0];          // shape of the custom waveform. this consists of 1000 items with value between -327 and 327
        int[] shapeFrequency = new int[0];          // shape of the frequency. this consists of 1000 items with value between 0 and 1000
        int[] shapeVolume = new int[0];          // shape of the volume. this consists of 1000 items with value between 0 and 1000
        int weight;

        public double MinFrequency { get => minFrequency; set => minFrequency = value; }
        public double MaxFrequency { get => maxFrequency; set => maxFrequency = value; }
        public string WaveForm { get => waveForm; set => waveForm = value; }
        public int Channel { get => channel; set => channel = value; }
        public int MinVolume { get => minVolume; set => minVolume = value; }
        public int MaxVolume { get => maxVolume; set => maxVolume = value; }

        public string WaveFile { get => waveFile; set => waveFile = value; }
        public int StartPosition { get => startPosition; set => startPosition = value; }
        public int Weight { get => weight; set => weight = value; }
        public string Name { get => name; set => name = value; }
        public double[] WaveData { get => waveData; set => waveData = value; }
        public int[] WaveFileData { get => waveFileData; set => waveFileData = value; }
        public int[] ShapeWave { get => shapeWave; set => shapeWave = value; }
        public int[] ShapeVolume { get => shapeVolume; set => shapeVolume = value; }
        public int[] ShapeFrequency { get => shapeFrequency; set => shapeFrequency = value; }

        public WaveInfo()
        {
            this.name = "Wave1";
            this.startPosition = 0;
            this.minFrequency = 440;
            this.maxFrequency = 440;
            this.minVolume = 255;
            this.maxVolume = 255;
            this.channel = 2;
            this.waveForm = "Sine";
            this.waveFile = "";
            this.waveData = new double[44100 * 2];
            this.weight = 255;
        }

        public WaveInfo(string name, int numSamples, int startPosition, double beginFrequency, double endFrequency, int beginVolume, int endVolume, int channel, string waveForm, string waveFile, int weight)
        {
            this.name = name;
            waveData = new double[numSamples * 2];
            this.startPosition = startPosition;
            this.minFrequency = beginFrequency;
            this.maxFrequency = endFrequency;
            this.minVolume = beginVolume;
            this.maxVolume = endVolume;
            this.channel = channel;
            this.waveForm = waveForm;
            this.waveFile = waveFile;
            this.weight = weight;
        }

        // stereo samples are counted as one (so 1 second contains 44100 samples)
        public int NumSamples()
        {
            return waveData.Length / 2;
        }

        public string GetInfo()
        {
            string info = startPosition + "-" + waveForm + "-" + NumSamples() + "-";
            if (waveForm.Equals("WavFile"))
            {
                info += "-" + Path.GetFileName(waveFile); 
            }
            else if (!waveForm.Equals("Noise"))
            {
                info += "-" + minFrequency;
                info += "->" + maxFrequency;
            }

            return info;
        }

        public double Duration()
        {
            return NumSamples() / SynthGenerator.SAMPLES_PER_SECOND;
        }

        public void SetDuration(double seconds)
        {
            waveData = new double[(int)(2 * seconds * SynthGenerator.SAMPLES_PER_SECOND)];
        }

        public double StartTime()
        {
            return startPosition / SynthGenerator.SAMPLES_PER_SECOND;
        }
    }
}
