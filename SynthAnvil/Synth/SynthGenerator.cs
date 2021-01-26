using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SynthAnvil.Synth
{

    class SynthGenerator
    {
        public const int NUM_AUDIO_CHANNELS = 2;
        public const double SAMPLES_PER_SECOND = 44100.0;
        private const int MAX_AMPLITUDE = 32767;     // Max amplitude for 16-bit audio
        private const int GRAPH_POINTS_PLOTTED = 300;
        private const double MAX_VOLUME = 255.0;

        private float envelopAttack = 0;
        private float envelopHold = 1;
        private float envelopDecay = 0;
        private float envelopSustain = 1;
        private float envelopRelease = 0;

        // Header, Format, Data chunks
        WaveHeader header = new WaveHeader();
        WaveFormatChunk format = new WaveFormatChunk();
        WaveDataChunk finalData = new WaveDataChunk();
        int finalNumSamples;
        Double[] tempData;
        List<WaveInfo> waves = new List<WaveInfo>();
        WaveInfo currentWave;

        // FFT stuff
        int fftWindow = 16384;
        Complex[] frequencySpectrumLeft;
        double[] frequenciesLeft;
        Complex[] frequencySpectrumRight;
        double[] frequenciesRight;

        Random random = new Random();

        double wavePhase;

        FormMain parentForm;

        public SynthGenerator(FormMain parentForm)
        {
            this.parentForm = parentForm;
        }

        public float EnvelopAttack { get => envelopAttack; set => envelopAttack = value; }
        public float EnvelopDecay { get => envelopDecay; set => envelopDecay = value; }
        public float EnvelopSustain { get => envelopSustain; set => envelopSustain = value; }
        public float EnvelopRelease { get => envelopRelease; set => envelopRelease = value; }
        public float EnvelopHold { get => envelopHold; set => envelopHold = value; }
        internal WaveDataChunk FinalData { get => finalData; set => finalData = value; }
        public WaveInfo CurrentWave { get => currentWave; set => currentWave = value; }
        public List<WaveInfo> Waves { get => waves; set => waves = value; }

        public double Duration()
        {
            return finalNumSamples / SAMPLES_PER_SECOND;
        }

        public void GenerateSound()
        {
            foreach (WaveInfo currentGenerator in waves)
            {
                CreateWavePattern(currentGenerator);
            }

            MixWaves();
            UpdateGraphs();
            parentForm.labelDuration.Text = string.Format("{0:0.00} s", Duration());
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

        private int PointToFrequency(int pointNumber)
        {
            return (int)(22050 * pointNumber / frequenciesLeft.Length);
        }

        private void UpdateFFTGraph()
        {
            CalcFFT();
            parentForm.chartFFTLeft.Series["Series1"].Points.Clear();
            parentForm.chartFFTRight.Series["Series1"].Points.Clear();
            int numPoints = (int)(frequenciesLeft.Length);
            int lastPoint = numPoints;

            for (int pointNumber = 0; pointNumber < lastPoint; pointNumber++)
            {
                int frequency = PointToFrequency(pointNumber);

                parentForm.chartFFTLeft.Series["Series1"].Points.AddXY(frequency, frequenciesLeft[pointNumber]);
            }

            for (int pointNumber = 0; pointNumber < lastPoint; pointNumber++)
            {
                int frequency = PointToFrequency(pointNumber);

                parentForm.chartFFTRight.Series["Series1"].Points.AddXY(frequency, frequenciesRight[pointNumber]);
            }

            parentForm.chartFFTLeft.ChartAreas[0].AxisX.Minimum = 0;
            parentForm.chartFFTLeft.ChartAreas[0].AxisX.Maximum = PointToFrequency(lastPoint);
            parentForm.chartFFTRight.ChartAreas[0].AxisX.Minimum = 0;
            parentForm.chartFFTRight.ChartAreas[0].AxisX.Maximum = PointToFrequency(lastPoint);
        }

        private void CalcFFT()
        {
            int numSamples = FinalData.shortArray.Length;
            frequencySpectrumLeft = new Complex[fftWindow];
            frequencySpectrumRight = new Complex[fftWindow];
            for (int i = 0; i < fftWindow * 2; i++)
            {
                int position = i;
                if (i%NUM_AUDIO_CHANNELS==0)
                {
                    if(i >= numSamples)
                    {
                        frequencySpectrumLeft[i / 2] = new Complex(0, 0);
                    }
                    else
                    {
                        frequencySpectrumLeft[i / 2] = new Complex(FinalData.shortArray[i], 0);
                    }
                }
                else
                {
                    if (i >= numSamples)
                    {
                        frequencySpectrumRight[i / 2] = new Complex(0, 0);
                    }
                    else
                    {
                        frequencySpectrumRight[i / 2] = new Complex(FinalData.shortArray[i], 0);
                    }
                }

            }

            MathUtils.FFT.Transform(frequencySpectrumLeft);
            MathUtils.FFT.Transform(frequencySpectrumRight);
            ToNormalizedFrequenciesArray();
        }

        private void ToNormalizedFrequenciesArray()
        {
            double max_value = 0;
            frequenciesLeft = new double[fftWindow / 2];
            for (int i = 0; i < frequenciesLeft.Length; i++)
            {
                frequenciesLeft[i] = Math.Abs(frequencySpectrumLeft[i].Real);
                if (frequenciesLeft[i] > max_value)
                {
                    max_value = frequenciesLeft[i];
                }
            }

            if (max_value > 0)
            {
                double scale_factor = 100 / max_value;
                for (int i = 0; i < frequenciesLeft.Length; i++)
                {
                    frequenciesLeft[i] *= scale_factor;
                    if (frequenciesLeft[i] >= 100)     // rounding errors
                    {
                        frequenciesLeft[i] = 99.999;
                    }
                }
            }

            max_value = 0;
            frequenciesRight = new double[fftWindow / 2];
            for (int i = 0; i < frequenciesRight.Length; i++)
            {
                frequenciesRight[i] = Math.Abs(frequencySpectrumRight[i].Real);
                if (frequenciesRight[i] > max_value)
                {
                    max_value = frequenciesRight[i];
                }
            }

            if (max_value > 0)
            {
                double scale_factor = 100 / max_value;
                for (int i = 0; i < frequenciesRight.Length; i++)
                {
                    frequenciesRight[i] *= scale_factor;
                    if (frequenciesRight[i] >= 100)     // rounding errors
                    {
                        frequenciesRight[i] = 99.999;
                    }
                }
            }
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

        // Calculates the current volume
        // position = sampleposition so divide by NUM_AUDIO_CHANNELS before calling 
        private double CalculateCurrentVolume(uint currentPosition, WaveInfo wave)
        {
            if (wave.EndVolumeEnabled)
            {
                if (wave.BeginEndBeginVolumeEnabled)
                {
                    double begin_share = Math.Abs(wave.NumSamples / 2 - currentPosition) * 2 / (double)wave.NumSamples;
                    return (wave.BeginVolume * begin_share + wave.EndVolume * (1 - begin_share)) / MAX_VOLUME;
                }
                else
                {
                    return ((wave.BeginVolume * (1 - currentPosition / (double)wave.NumSamples)) + (wave.EndVolume * currentPosition / (double)wave.NumSamples)) / MAX_VOLUME;
                }
            }
            else
            {
                return wave.BeginVolume / MAX_VOLUME;
                //return 0.5 + 0.4 * Math.Sin(currentPosition/2000.0);
            }
        }

        // Apply [wave] volume to [waveData]
        private void ApplyVolume(WaveInfo wave, double[] waveData)
        {
            for (uint samplePosition = 0; samplePosition < waveData.Length / NUM_AUDIO_CHANNELS; samplePosition++)
            {
                for (int channel = 0; channel < NUM_AUDIO_CHANNELS; channel++)
                {
                    waveData[samplePosition * 2 + channel] = (short)(CalculateCurrentVolume(samplePosition / NUM_AUDIO_CHANNELS, wave) * waveData[samplePosition * 2 + channel]);
                }
            }
        }

        private void MixWaves()
        {
            finalNumSamples = findMaxNumSamples();

            // Initialize the 128-bit array
            tempData = new double[finalNumSamples * NUM_AUDIO_CHANNELS];
            
            double mixed_value;

            for (uint samplePosition = 0; samplePosition < finalNumSamples; samplePosition++)
            {
                for (int channel = 0; channel < NUM_AUDIO_CHANNELS; channel++)
                {
                    mixed_value = 0;
                    foreach (WaveInfo currentWave in Waves)
                    {
                        if (currentWave.Channel == 2 || currentWave.Channel == channel)
                        {
                            if (samplePosition >= currentWave.StartPosition && samplePosition < currentWave.StartPosition + currentWave.NumSamples)
                            {
                                mixed_value += currentWave.Weight * CalculateCurrentVolume((uint)(samplePosition - currentWave.StartPosition), currentWave) * currentWave.WaveData[(samplePosition - currentWave.StartPosition) * 2 + channel];
                            }
                        }
                    }
                    tempData[samplePosition * 2 + channel] = mixed_value;
                }
            }

            NormalizeVolume(tempData);
            ApplyAHDSR(tempData);
            CopyToFinalData(tempData);
        }

        private void CopyWaveToTempData(double[] waveData)
        {
            tempData = waveData.Clone() as double[];
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
            parentForm.chartAHDSR.Series["Series1"].Points.Clear();
            float position;

            for (uint graphPoint = 0; graphPoint < GRAPH_POINTS_PLOTTED; graphPoint++)
            {
                position = graphPoint / (float)GRAPH_POINTS_PLOTTED;
                parentForm.chartAHDSR.Series["Series1"].Points.AddY(CalcAHDSR(position));
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

        private void ApplyAHDSR(double[] waveData)
        {
            for (uint samplePosition = 0; samplePosition < waveData.Length; samplePosition++)
            {
                float position = samplePosition / (waveData.Length / (float)NUM_AUDIO_CHANNELS);
                float volume_factor = CalcAHDSR(position/2);

                waveData[samplePosition] = (short)(waveData[samplePosition] * volume_factor);
            }
        }

        private void UpdateGraphs()
        {
            UpdateWaveGraph();
            UpdateResultGraph();
            UpdateFFTGraph();
        }

        private void UpdateResultGraph()
        {
            parentForm.chartResultLeft.Series["Series1"].Points.Clear();
            parentForm.chartResultRight.Series["Series1"].Points.Clear();
            int num_samples = finalData.shortArray.Length / 2;
            for (uint pointNumber = 0; pointNumber < GRAPH_POINTS_PLOTTED; pointNumber++)
            {
                int position = (int)(num_samples * NUM_AUDIO_CHANNELS * pointNumber / (float)GRAPH_POINTS_PLOTTED);
                if (position % 2 != 0)
                {
                    position++;     // start with left channel
                }

                for (int channel = 0; channel < NUM_AUDIO_CHANNELS; channel++)
                {
                    int value = finalData.shortArray[position + channel];

                    if (channel == 0)
                    {
                        parentForm.chartResultLeft.Series["Series1"].Points.AddY(value);
                    }
                    else
                    {
                        parentForm.chartResultRight.Series["Series1"].Points.AddY(value);
                    }
                }
            }
        }

        private void UpdateWaveGraph()
        {
            parentForm.chartCurrentWave.Series["Series1"].Points.Clear();

            int num_samples = finalData.shortArray.Length / 2;
            for (uint pointNumber = 0; pointNumber < GRAPH_POINTS_PLOTTED; pointNumber++)
            {
                uint position = (uint)(num_samples * NUM_AUDIO_CHANNELS * pointNumber / (float)GRAPH_POINTS_PLOTTED);
                int channel = currentWave.Channel == 1 ? 1 : 0;
                int value = 0;
                if (position >= (NUM_AUDIO_CHANNELS * CurrentWave.StartPosition) &&
                    position < (NUM_AUDIO_CHANNELS * (CurrentWave.StartPosition + CurrentWave.NumSamples)))
                {
                    value = (int)(CalculateCurrentVolume((uint)(position / NUM_AUDIO_CHANNELS - CurrentWave.StartPosition), CurrentWave) * CurrentWave.WaveData[position - CurrentWave.StartPosition * NUM_AUDIO_CHANNELS + channel]);
                }

                parentForm.chartCurrentWave.Series["Series1"].Points.AddY(value);
            }
        }

        private void CreateWavePattern(WaveInfo waveInfo)
        {
            if (waveInfo.WaveForm.Equals("File (.wav)"))
            {
                if(waveInfo.WaveFile.Length==0)
                {
                    // create data if no .wav selected
                    waveInfo.WaveData = new double[waveInfo.NumSamples * NUM_AUDIO_CHANNELS];
                }
                return;
            }

            // Initialize the 128-bit array
            waveInfo.WaveData = new double[waveInfo.NumSamples * NUM_AUDIO_CHANNELS];

            AddWave(waveInfo, 1, 1);

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

            for (int channel = 0; channel < NUM_AUDIO_CHANNELS; channel++)
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
            using (WaveFileReader reader = new WaveFileReader(CurrentWave.WaveFile))
            {
                byte[] buffer = new byte[reader.Length];
                int read = reader.Read(buffer, 0, buffer.Length);
                double sampleRatio = SAMPLES_PER_SECOND / reader.WaveFormat.SampleRate;
                CurrentWave.NumSamples = (int)(reader.SampleCount * sampleRatio);
                CurrentWave.WaveData = new double[CurrentWave.NumSamples * 2];            // output=2 channels
                short[] tempData = new short[CurrentWave.NumSamples * 2];            // output=2 channels

                if (reader.WaveFormat.SampleRate == SAMPLES_PER_SECOND && reader.WaveFormat.Channels == 2)
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
                            tempData[NUM_AUDIO_CHANNELS * current_sample] = (short)((buffer[position] - 128) * 256);
                            if (reader.WaveFormat.Channels == 2)
                            {
                                tempData[NUM_AUDIO_CHANNELS * current_sample + 1] = (short)((buffer[position + 1] - 128) * 256);
                            }
                            else
                            {
                                // use data of first channel for right channel
                                tempData[NUM_AUDIO_CHANNELS * current_sample + 1] = (short)((buffer[position] - 128) * 256);
                            }
                        }
                        else
                        {
                            tempData[NUM_AUDIO_CHANNELS * current_sample] = BitConverter.ToInt16(new byte[2] { buffer[position], buffer[position + 1] }, 0);
                            if (reader.WaveFormat.Channels == 2)
                            {
                                tempData[NUM_AUDIO_CHANNELS * current_sample + 1] = BitConverter.ToInt16(new byte[2] { buffer[position + 2], buffer[position + 3] }, 0);
                            }
                            else
                            {
                                // use data of first channel for right channel
                                tempData[NUM_AUDIO_CHANNELS * current_sample + 1] = BitConverter.ToInt16(new byte[2] { buffer[position], buffer[position + 1] }, 0);
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
                ColorSlider.ColorSlider colorSlider = (ColorSlider.ColorSlider)parentForm.Controls.Find("colorSliderDuration1", true)[0];
                colorSlider.Value = (decimal)(CurrentWave.NumSamples / 441);

                parentForm.labelFileName.Text = Path.GetFileName(CurrentWave.WaveFile);
                parentForm.UpdateCurrentWaveInfo();
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

        private void CopyToFinalData(double[] tempData)
        {
            finalData.shortArray = new short[tempData.Length];
            for(int i=0; i<tempData.Length; i++)
            {
                finalData.shortArray[i] = (short)tempData[i];
            }
            finalData.dwChunkSize = (uint)(tempData.Length * (format.wBitsPerSample / 8));
            header.dwFileLength = 36 + finalData.dwChunkSize;
        }

        public void Play(bool all=true)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(memoryStream))
            {
                if(!all)
                {
                    CopyWaveToTempData(currentWave.WaveData);
                    ApplyVolume(currentWave, tempData);
                    CopyToFinalData(tempData);
                }
                WriteWaveData(writer, finalData);

                memoryStream.Position = 0;
                new SoundPlayer(memoryStream).Play();
            }
        }

        public WaveInfo CloneWave(double frequencyFactor = 1, double amplitudeFactor = 1)
        {
            int highestNumber = 0;
            foreach (WaveInfo wave in Waves)
            {
                if (wave.Number > highestNumber)
                {
                    highestNumber = wave.Number;
                }
            }
            WaveInfo newWave = new WaveInfo(highestNumber + 1, currentWave.NumSamples, currentWave.StartPosition,
                currentWave.BeginFrequency * frequencyFactor, currentWave.EndFrequency * frequencyFactor, (int)(currentWave.BeginVolume * amplitudeFactor), (int)(currentWave.EndVolume * amplitudeFactor),
                currentWave.Channel, currentWave.WaveForm, currentWave.WaveFile, currentWave.EndFrequencyEnabled,
                currentWave.EndVolumeEnabled, currentWave.BeginEndBeginFrequencyEnabled, currentWave.BeginEndBeginVolumeEnabled, currentWave.Weight);
            newWave.WaveData = new double[newWave.NumSamples * NUM_AUDIO_CHANNELS];
            newWave.SetName();
            Waves.Add(newWave);

            return newWave;
        }

        public void SetCurrentWaveNumber(int number)
        {
            CurrentWave = Waves.Find(o => o.Number==number);
        }

        public void RemoveCurrentWave()
        {
            Waves.Remove(currentWave);
        }

    }
}
