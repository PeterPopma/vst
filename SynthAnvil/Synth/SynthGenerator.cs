using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace SynthAnvil.Synth
{

    class SynthGenerator
    {
        private const int MAX_AMPLITUDE = 32767;     // Max amplitude for 16-bit audio
        private const int NUM_CHANNELS_GENERATORS = 2;
        private const int GRAPH_POINTS_PLOTTED = 300;

        private float envelopAttack = 0;
        private float envelopHold = 1;
        private float envelopDecay = 0;
        private float envelopSustain = 1;
        private float envelopRelease = 0;

        // Header, Format, Data chunks
        WaveHeader header = new WaveHeader();
        WaveFormatChunk format = new WaveFormatChunk();
        WaveDataChunk finalData = new WaveDataChunk();
        Double[] tempData;
        List<WaveInfo> waves = new List<WaveInfo>();
        WaveInfo currentWave;

        Random random = new Random();

        double wavePhase;

        FormMain ParentForm;

        public SynthGenerator(FormMain parentForm)
        {
            ParentForm = parentForm;
        }

        public float EnvelopAttack { get => envelopAttack; set => envelopAttack = value; }
        public float EnvelopDecay { get => envelopDecay; set => envelopDecay = value; }
        public float EnvelopSustain { get => envelopSustain; set => envelopSustain = value; }
        public float EnvelopRelease { get => envelopRelease; set => envelopRelease = value; }
        public float EnvelopHold { get => envelopHold; set => envelopHold = value; }
        internal WaveDataChunk FinalData { get => finalData; set => finalData = value; }
        public List<WaveInfo> Waves { get => waves; set => waves = value; }
        public WaveInfo CurrentWave { get => currentWave; set => currentWave = value; }

         public void GenerateSound()
        {
            foreach (WaveInfo currentGenerator in waves)
            {
                CreateWavePattern(currentGenerator);
            }

            MixWaves();
            UpdateGraphs();
        }

        private int findMaxNumSamples()
        {
            int max_duration = 0;
            foreach (WaveInfo currentGenerator in waves)
            {
                if ((currentGenerator.Weight>0) && currentGenerator.NumSamples > max_duration)
                {
                    max_duration = currentGenerator.NumSamples;
                }
            }
            return max_duration;
        }

        private double CalculateCurrentFrequency(uint currentPosition, WaveInfo generator)
        {
            if (generator.EndFrequencyEnabled)
            {
                if (generator.BeginEndBeginFrequencyEnabled)
                {
                    double begin_share = Math.Abs(generator.NumSamples / 2 - currentPosition) * 2 / (double)generator.NumSamples;
                    return generator.BeginFrequency * begin_share + generator.EndFrequency * (1 - begin_share);
                }
                else
                {
                    return (generator.BeginFrequency * (1 - currentPosition / (double)generator.NumSamples)) + (generator.EndFrequency * currentPosition / (double)generator.NumSamples);
                }
            }
            else
            {
                return generator.BeginFrequency;
            }
        }

        private double CalculateCurrentVolume(uint currentPosition, WaveInfo wave)
        {
            if (wave.EndVolumeEnabled)
            {
                if (wave.BeginEndBeginVolumeEnabled)
                {
                    double begin_share = Math.Abs(wave.NumSamples / 2 - currentPosition) * 2 / (double)wave.NumSamples;
                    return (wave.BeginVolume * begin_share + wave.EndVolume * (1 - begin_share)) / 100.0f;
                }
                else
                {
                    return ((wave.BeginVolume * (1 - currentPosition / (double)wave.NumSamples)) + (wave.EndVolume * currentPosition / (double)wave.NumSamples)) / 100.0f;
                }
            }
            else
            {
                return wave.BeginVolume / 100.0f;
                //return 0.5 + 0.4 * Math.Sin(currentPosition/2000.0);
            }
        }
        
        private void ApplyVolume(WaveInfo generator)
        {
            for (uint samplePosition = 0; samplePosition < generator.NumSamples - 1; samplePosition++)
            {
                for (int channel = 0; channel < format.wChannels; channel++)
                {
                    generator.WaveData[samplePosition * 2 + channel] = (short)(CalculateCurrentVolume(samplePosition, generator) * generator.WaveData[samplePosition * 2 + channel]);
                }
            }
        }

        private int CalcTotalWeight()
        {
            int total_weight = 0;
            foreach(WaveInfo wave in Waves)
            {
                total_weight += wave.Weight;
            }

            if(total_weight==0)
            {
                foreach(WaveInfo wave in Waves)
                {
                    wave.Weight = 100;
                }
                total_weight = 100 * Waves.Count;
            }

            return total_weight;
        }

        private void MixWaves()
        {
            int numSamples = findMaxNumSamples();

            // Initialize the 128-bit array
            tempData = new double[numSamples * NUM_CHANNELS_GENERATORS];
            
            long mixed_value;
            int total_weight = CalcTotalWeight();

            for (uint samplePosition = 0; samplePosition < numSamples; samplePosition++)
            {
                for (int channel = 0; channel < NUM_CHANNELS_GENERATORS; channel++)
                {
                    mixed_value = 0;
                    double num_participating_generators = 0;
                    foreach (WaveInfo currentWave in Waves)
                    {
                        int value = 0;
                        if (currentWave.Channel == 2 || currentWave.Channel == channel)
                        {
                            num_participating_generators++;
                            if (samplePosition >= currentWave.StartPosition && samplePosition < currentWave.StartPosition + currentWave.NumSamples)
                            {
                                value = (int)(currentWave.Weight / (float)total_weight * CalculateCurrentVolume((uint)(samplePosition - currentWave.StartPosition), currentWave) * currentWave.WaveData[(samplePosition-currentWave.StartPosition) * 2 + channel]);
                                mixed_value += value;
                            }
                        }
                    }
                    if (num_participating_generators == 0)
                    {
                        tempData[samplePosition * 2 + channel] = 0;
                    }
                    else
                    {
                        tempData[samplePosition * 2 + channel] = mixed_value / num_participating_generators;
                    }
                }
                ApplyAHDSR(samplePosition);
            }

            NormalizeVolume(tempData);
            CopyTempToFinalData(tempData);

            ParentForm.labelDuration.Text = string.Format("{0:0.00} s", numSamples/44100.0);
        }

        private void NormalizeVolume(double[] waveData)
        {
            double max_volume = 0;
            for (uint samplePosition = 0; samplePosition < waveData.Length; samplePosition++)
            {
                if (Math.Abs(waveData[samplePosition]) > max_volume)
                {
                    max_volume = Math.Abs(waveData[samplePosition]);
                }
            }
            if(max_volume==0)
            {
                return;
            }

            double scale_factor = MAX_AMPLITUDE / max_volume;
            for (uint samplePosition = 0; samplePosition < waveData.Length; samplePosition++)
            {
                waveData[samplePosition] *= scale_factor;
            }
        }

        public void UpdateADSRChart()
        {
            ParentForm.chartAHDSR.Series["Series1"].Points.Clear();
            float position;

            for (uint graphPoint = 0; graphPoint < GRAPH_POINTS_PLOTTED; graphPoint++)
            {
                position = graphPoint / (float)GRAPH_POINTS_PLOTTED;
                ParentForm.chartAHDSR.Series["Series1"].Points.AddY(CalcAHDSR(position));
            }
        }

        private float CalcAHDSR(float position)
        {
            float volume_factor;
            if (envelopAttack > 0 && position <= envelopAttack)
            {
                volume_factor = position / envelopAttack;
            }
            else if (position <= envelopAttack + envelopHold)
            {
                volume_factor = 1;
            }
            else if (position <= envelopAttack + envelopHold + envelopDecay)
            {
                volume_factor = 1 - ((position-envelopAttack-envelopHold) /(envelopDecay)) * (1 - envelopSustain);
            }
            else if (position <= envelopAttack + envelopHold + envelopDecay + envelopRelease)
            {
                volume_factor = (envelopAttack + envelopHold + envelopDecay + envelopRelease - position) / envelopRelease * envelopSustain;
            }
            else
            {
                volume_factor = 0;
            }

            return volume_factor;
        }

        private void ApplyAHDSR(uint samplePosition)
        {
            float position = samplePosition / (tempData.Length / (float)NUM_CHANNELS_GENERATORS);
            float volume_factor = CalcAHDSR(position);

            tempData[samplePosition * 2] = (short)(tempData[samplePosition * 2] * volume_factor);
            tempData[samplePosition * 2 + 1] = (short)(tempData[samplePosition * 2 + 1] * volume_factor);
        }

        private void UpdateGraphs()
        {
            UpdateWaveGraph();
            UpdateResultGraph();
        }

        private void UpdateResultGraph()
        {
            ParentForm.chartResultLeft.Series["Series1"].Points.Clear();
            ParentForm.chartResultRight.Series["Series1"].Points.Clear();
            int num_samples = finalData.shortArray.Length / 2;
            for (uint pointNumber = 0; pointNumber < GRAPH_POINTS_PLOTTED; pointNumber++)
            {
                int position = (int)(num_samples * NUM_CHANNELS_GENERATORS * pointNumber / (float)GRAPH_POINTS_PLOTTED);
                if (position % 2 != 0)
                {
                    position++;     // start with left channel
                }

                for (int channel = 0; channel < NUM_CHANNELS_GENERATORS; channel++)
                {
                    int value = finalData.shortArray[position + channel];

                    if (channel == 0)
                    {
                        ParentForm.chartResultLeft.Series["Series1"].Points.AddY(value);
                    }
                    else
                    {
                        ParentForm.chartResultRight.Series["Series1"].Points.AddY(value);
                    }
                }
            }
        }

        private void UpdateWaveGraph()
        {
            ParentForm.chartCurrentWave.Series["Series1"].Points.Clear();

            int num_samples = finalData.shortArray.Length / 2;
            for (uint pointNumber = 0; pointNumber < GRAPH_POINTS_PLOTTED; pointNumber++)
            {
                uint position = (uint)(num_samples * NUM_CHANNELS_GENERATORS * pointNumber / (float)GRAPH_POINTS_PLOTTED);
                int channel = currentWave.Channel == 1 ? 1 : 0;
                int value;
                if (position >= (NUM_CHANNELS_GENERATORS * CurrentWave.StartPosition) &&
                    position < (NUM_CHANNELS_GENERATORS * (CurrentWave.StartPosition + CurrentWave.NumSamples)))
                {
                    value = (int)(CalculateCurrentVolume((uint)(position / NUM_CHANNELS_GENERATORS - CurrentWave.StartPosition), CurrentWave) * CurrentWave.WaveData[position - CurrentWave.StartPosition * NUM_CHANNELS_GENERATORS + channel]);
                }
                else
                {
                    value = 0;
                }

                ParentForm.chartCurrentWave.Series["Series1"].Points.AddY(value);
            }
        }

        private void CreateWavePattern(WaveInfo waveInfo)
        {
            if (waveInfo.WaveForm.Equals("File (.wav)"))
            {
                return;
            }

            // Initialize the 128-bit array
            waveInfo.WaveData = new double[waveInfo.NumSamples * NUM_CHANNELS_GENERATORS];

            AddWave(waveInfo, 1, 1);

            for (int i = 1; i <= waveInfo.HarmonicsOdd; i++)
            {
                double amplitude = 1 / Math.Pow(1 + (waveInfo.HarmonicDecayOdd / 100.0), i);
                AddWave(waveInfo, i * 2, amplitude);
            }

            for (int i = 1; i <= waveInfo.HarmonicsEven; i++)
            {
                double amplitude = 1 / Math.Pow(1 + (waveInfo.HarmonicDecayEven / 100.0), i);
                AddWave(waveInfo, i * 2 + 1, amplitude);
            }

            for (int i = 1; i <= waveInfo.InHarmonics; i++)
            {
                double deviation = (i / (double)waveInfo.InHarmonics) * waveInfo.InHarmonicSpread/100.0;

                double frequency_factor = 1 + deviation;
                if (i % 2 == 0)
                {
                    frequency_factor = 1 / frequency_factor;
                }

                double amplitude = 1 / Math.Pow(1 + (waveInfo.InHarmonicDecay / 100.0), i);
                AddWave(waveInfo, frequency_factor, amplitude);
            }
            NormalizeVolume(waveInfo.WaveData);
        }

        private void AddWave(WaveInfo generator, double frequency_factor, double amplitude)
        {
            wavePhase = 0;
            for (uint current_sample = 0; current_sample < generator.NumSamples - 1; current_sample++)
            {
                double frequency = frequency_factor * CalculateCurrentFrequency(current_sample, generator);
                CreateWaveData(generator, frequency, current_sample, amplitude);
            }
        }

        private void CreateWaveData(WaveInfo generator, double frequency, uint current_sample, double amplitude)
        {
            if (frequency<0)
            {
                throw new Exception("frequency should not be negative!");
            }
            // The period of the wave.
            double deltaT = (Math.PI * 2 * frequency) / format.dwSamplesPerSec;

            for (int channel = 0; channel < NUM_CHANNELS_GENERATORS; channel++)
            {
                if (generator.Channel == 2 || generator.Channel == channel)
                {
                    generator.WaveData[current_sample * 2 + channel] += Convert.ToDouble(amplitude * MAX_AMPLITUDE * WaveFunction(generator, (wavePhase + deltaT)%(2*Math.PI)));
                }
            }
            wavePhase += deltaT;
        }

        // input: phase 0..2PI
        // output: a value between -1 and 1
        private double WaveFunction(WaveInfo generator, double phase)
        {
            switch (generator.WaveForm)
            {
                case "Free":
                    return MyFunction(phase);
                case "Sine":
                    return Math.Sin(phase);
                case "Square":
                    return phase<Math.PI ? -1 : 1;
                case "Sawtooth":
                    return -1 + 2* (phase / (2 * Math.PI));
                case "Triangle":
                    return phase < Math.PI ? (-1 + 4 * (phase / (2 * Math.PI))) : (1 - 4 * ((phase-Math.PI) / (2 * Math.PI)));
                case "Noise":
                    return random.Next(-MAX_AMPLITUDE, MAX_AMPLITUDE)/ (double)MAX_AMPLITUDE;
            }

            return 0.0;
        }

        private double MyFunction(double phase)
        {
            return Math.Sinh(phase);
        }

        // load a .wav file. Supported is PCM, mono/stereo, 8/16 bits, all samplerates.
        // loaded file is transformed into a 44100 Kz 16 bits stereo stream. 
        // if the source is mono, the data is copied to both streams
        public void LoadWaveFile(string fileName)
        {
            CurrentWave.WaveFile = fileName;
            using (WaveFileReader reader = new WaveFileReader("wav\\" + CurrentWave.WaveFile + ".wav"))
            {
                byte[] buffer = new byte[reader.Length];
                int read = reader.Read(buffer, 0, buffer.Length);
                double sampleRatio = 44100.0 / reader.WaveFormat.SampleRate;
                CurrentWave.NumSamples = (int)(reader.SampleCount * sampleRatio);
                CurrentWave.WaveData = new double[CurrentWave.NumSamples * 2];            // output=2 channels
                short[] tempData = new short[CurrentWave.NumSamples * 2];            // output=2 channels

                if (reader.WaveFormat.SampleRate == 44100 && reader.WaveFormat.Channels == 2)
                {
                    // We can copy everything 
                    if (reader.WaveFormat.BitsPerSample == 8)
                    {
                        tempData = Array.ConvertAll(buffer, b => (short)((b - 128) * 256));
                    }
                    else
                    {
                        // Initialize the 16-bit array
                        tempData = new short[(int)Math.Ceiling(buffer.Length / 2.0)];
                        CurrentWave.WaveData = new double[(int)Math.Ceiling(buffer.Length / 2.0)];
                        Buffer.BlockCopy(buffer, 0, tempData, 0, buffer.Length);
                    }
                }
                else
                {
                    for (int current_sample = 0; current_sample < CurrentWave.NumSamples; current_sample++)
                    {
                        int position = (int)(current_sample / sampleRatio);
                        if (reader.WaveFormat.BitsPerSample == 8)
                        {
                            tempData[NUM_CHANNELS_GENERATORS * current_sample] = (short)((buffer[position] - 128) * 256);
                            if (reader.WaveFormat.Channels == 2)
                            {
                                tempData[NUM_CHANNELS_GENERATORS * current_sample + 1] = (short)((buffer[position + 1] - 128) * 256);
                            }
                            else
                            {
                                // use data of first channel for right channel
                                tempData[NUM_CHANNELS_GENERATORS * current_sample + 1] = (short)((buffer[position] - 128) * 256);
                            }
                        }
                        else
                        {
                            tempData[NUM_CHANNELS_GENERATORS * current_sample] = BitConverter.ToInt16(new byte[2] { buffer[position], buffer[position + 1] }, 0);
                            if (reader.WaveFormat.Channels == 2)
                            {
                                tempData[NUM_CHANNELS_GENERATORS * current_sample + 1] = BitConverter.ToInt16(new byte[2] { buffer[position + 2], buffer[position + 3] }, 0);
                            }
                            else
                            {
                                // use data of first channel for right channel
                                tempData[NUM_CHANNELS_GENERATORS * current_sample + 1] = BitConverter.ToInt16(new byte[2] { buffer[position], buffer[position + 1] }, 0);
                            }
                        }
                    }
                }

                for (int i = 0; i < tempData.Length; i++)
                {
                    CurrentWave.WaveData[i] = tempData[i];
                }

                // update duration
                CurrentWave.NumSamples = CurrentWave.WaveData.Length / 2;
                ColorSlider.ColorSlider colorSlider = (ColorSlider.ColorSlider)ParentForm.Controls.Find("colorSliderDuration1", true)[0];
                colorSlider.Value = (decimal)(CurrentWave.NumSamples / 441);
            }
        }

        private static byte[] ReverseAudioData(byte[] forwardsArrayWithOnlyAudioData)
        {
            int bytesPerSample = 4;     // 16 bits * 2 channels
            int length = forwardsArrayWithOnlyAudioData.Length;
            byte[] reversedArrayWithOnlyAudioData = new byte[length];
            int sampleIdentifier = 0;
            for (int i = 0; i < length; i++)
            {
                if (i != 0 && i % bytesPerSample == 0)
                {
                    sampleIdentifier += 2 * bytesPerSample;
                }
                int index = length - bytesPerSample - sampleIdentifier + i;
                reversedArrayWithOnlyAudioData[i] = forwardsArrayWithOnlyAudioData[index];
            }
            return reversedArrayWithOnlyAudioData;
        }

        private void WriteWaveData(BinaryWriter writer, WaveDataChunk data)
        {
            // Write the header
            writer.Write(header.sGroupID.ToCharArray());
            writer.Write(header.dwFileLength);
            writer.Write(header.sRiffType.ToCharArray());

            // Write the format chunk
            writer.Write(format.sChunkID.ToCharArray());
            writer.Write(format.dwChunkSize);
            writer.Write(format.wFormatTag);
            writer.Write(format.wChannels);
            writer.Write(format.dwSamplesPerSec);
            writer.Write(format.dwAvgBytesPerSec);
            writer.Write(format.wBlockAlign);
            writer.Write(format.wBitsPerSample);

            // Write the data chunk
            writer.Write(data.sChunkID.ToCharArray());
            writer.Write(data.dwChunkSize);
            foreach (short dataPoint in data.shortArray)
            {
                writer.Write(dataPoint);
            }
        }

        public void Save(string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            using (BinaryWriter writer = new BinaryWriter(fileStream))
            {
                WriteWaveData(writer, finalData);

                writer.Seek(4, SeekOrigin.Begin);
                uint filesize = (uint)writer.BaseStream.Length;
                writer.Write(filesize - 8);

            }
        }

        private void CopyTempToFinalData(double[] tempData)
        {
            finalData.shortArray = new short[tempData.Length];
            for(int i=0; i<tempData.Length; i++)
            {
                finalData.shortArray[i] = (short)tempData[i];
            }
            finalData.dwChunkSize = (uint)(tempData.Length * (format.wBitsPerSample / 8));
            header.dwFileLength = 36 + finalData.dwChunkSize;
        }

        public void Play(int generator=0)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(memoryStream))
            {
                if(generator>0)
                {
                    ApplyVolume(Waves[generator - 1]);
                    CopyTempToFinalData(waves[generator - 1].WaveData);
                }
                WriteWaveData(writer, finalData);

                memoryStream.Position = 0;
                new SoundPlayer(memoryStream).Play();
            }
        }

    }
}
