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
        public const int SHAPE_WAVE_NUMPOINTS = 1000;
        public const int SHAPE_VOLUME_NUMPOINTS = 1000;
        public const int SHAPE_FREQUENCY_NUMPOINTS = 1000;
        public const int SHAPE_WAVE_MAX_VALUE = 327;
        public const int SHAPE_VOLUME_MAX_VALUE = 500;
        public const int SHAPE_FREQUENCY_MAX_VALUE = 500;
        public const int MAX_VOLUME = 500;
        public const int MAX_FREQUENCY = 22387;
        public const double SAMPLES_PER_SECOND = 44100.0;
        private const int MAX_AMPLITUDE = 32767;     // Max amplitude for 16-bit audio
        private const int GRAPH_POINTS_PLOTTED = 300;
        private const string DEFAULT_WAVE_NAME = "Wave";

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
        public double[] TempData { get => tempData; set => tempData = value; }

        public double Duration()
        {
            return tempData.Length / NUM_AUDIO_CHANNELS / SAMPLES_PER_SECOND;
        }

        public void UpdateCurrentWaveData()
        {
            if (currentWave.WaveForm.Equals("WavFile"))
            {
                if (currentWave.WaveFileData.Length > 0)
                {
                    currentWave.WaveData = currentWave.WaveFileData.Select(d => (double)d).ToArray();
                }
                else
                {
                    // create empty data
                    currentWave.WaveData = new double[currentWave.WaveData.Length];
                }
            }
            else
            {
                CreateWave(currentWave, 1, 1);
            }

            UpdateMixedSound();
            parentForm.UpdateWavesList();
            parentForm.pictureBoxCustomWave.Refresh();
            parentForm.pictureBoxFrequencyShape.Refresh();
            parentForm.pictureBoxVolumeShape.Refresh();
            parentForm.ChangedPresetData = true;
        }

        public void UpdateAllWaveData()
        {
            foreach (WaveInfo waveInfo in Waves)
            {
                if (waveInfo.WaveForm.Equals("WavFile"))
                {
                    if (waveInfo.WaveFileData.Length > 0)
                    {
                        waveInfo.WaveData = waveInfo.WaveFileData.Select(d => (double)d).ToArray();
                    }
                    else
                    {
                        // create empty data
                        waveInfo.WaveData = new double[waveInfo.WaveData.Length];
                    }
                }
                else
                {
                    CreateWave(waveInfo, 1, 1);
                }
            }

            UpdateMixedSound();
        }

        public void UpdateMixedSound()
        {
            MixWaves();
            UpdateGraphs();
            parentForm.labelDuration.Text = string.Format("{0:0.00} s", Duration());
        }

        private int findMaxNumSamples()
        {
            int max_duration = 0;
            foreach (WaveInfo waveInfo in waves)
            {
                if ((waveInfo.Weight>0) && waveInfo.NumSamples() + waveInfo.StartPosition > max_duration)
                {
                    max_duration = waveInfo.NumSamples() + waveInfo.StartPosition;
                }
            }
            return max_duration;
        }

        private int PointToFrequency(int pointNumber)
        {
            return 22050 * pointNumber / frequenciesLeft.Length;
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
            int numSamples = tempData.Length;
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
                        frequencySpectrumLeft[i / 2] = new Complex(tempData[i], 0);
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
                        frequencySpectrumRight[i / 2] = new Complex(tempData[i], 0);
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

        private double CalculateCurrentFrequency(uint currentPosition, WaveInfo waveInfo)
        {
            int position = (int)(currentPosition / (double)waveInfo.NumSamples() * SHAPE_FREQUENCY_NUMPOINTS);

            // a value of 0 means max. frequency, a value of SHAPE_FREQUENCY_MAX_VALUE means min. frequency.
            return (waveInfo.MaxFrequency * waveInfo.ShapeFrequency[position] + (waveInfo.MinFrequency * (SHAPE_FREQUENCY_MAX_VALUE-waveInfo.ShapeFrequency[position]))) / (double)SHAPE_FREQUENCY_MAX_VALUE;
        }

        // Calculates the current volume
        // currentPosition = current sample position regardless the number of channels
        // returns the volume as a value between 0 and 1
        private double CalculateCurrentVolume(uint currentPosition, WaveInfo waveInfo)
        {
            int position = (int)(currentPosition / (double)waveInfo.NumSamples() * SHAPE_VOLUME_NUMPOINTS);

            // a value of 0 means max. volume, a value of SHAPE_VOLUME_MAX_VALUE means min. volume.
            return (waveInfo.MaxVolume * waveInfo.ShapeVolume[position] + (waveInfo.MinVolume * (SHAPE_VOLUME_MAX_VALUE - waveInfo.ShapeVolume[position]))) / (double)(SHAPE_VOLUME_MAX_VALUE * MAX_VOLUME);
        }

        // Apply [wave] volume to [waveData]
        private void ApplyVolume(WaveInfo wave, double[] waveData)
        {
            for (uint samplePosition = 0; samplePosition < waveData.Length / NUM_AUDIO_CHANNELS; samplePosition++)
            {
                for (int channel = 0; channel < NUM_AUDIO_CHANNELS; channel++)
                {
                    waveData[samplePosition * 2 + channel] = (short)(CalculateCurrentVolume(samplePosition, wave) * waveData[samplePosition * 2 + channel]);
                }
            }
        }

        private void MixWaves()
        {
            // Initialize the 128-bit array
            tempData = new double[findMaxNumSamples() * NUM_AUDIO_CHANNELS];
            
            double mixed_value, total_weight;

            for (uint samplePosition = 0; samplePosition < tempData.Length/2; samplePosition++)
            {
                for (int channel = 0; channel < NUM_AUDIO_CHANNELS; channel++)
                {
                    mixed_value = 0;
                    total_weight = 0;
                    foreach (WaveInfo currentWave in Waves)
                    {
                        if (currentWave.Channel == 2 || currentWave.Channel == channel)
                        {
                            if (samplePosition >= currentWave.StartPosition && samplePosition < currentWave.StartPosition + currentWave.NumSamples())
                            {
                                mixed_value += currentWave.Weight * CalculateCurrentVolume((uint)(samplePosition - currentWave.StartPosition), currentWave) * currentWave.WaveData[(samplePosition - currentWave.StartPosition) * 2 + channel];
                                total_weight += currentWave.Weight;
                            }
                        }
                    }
                    tempData[samplePosition * 2 + channel] = mixed_value / total_weight;
                }
            }

            NormalizeVolume(tempData);
            ApplyAHDSR(tempData);
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
            int num_samples = tempData.Length / 2;

            if (num_samples == 0)
            {
                return;
            }

            for (uint pointNumber = 0; pointNumber < GRAPH_POINTS_PLOTTED; pointNumber++)
            {
                int position = (int)(num_samples * NUM_AUDIO_CHANNELS * pointNumber / (float)GRAPH_POINTS_PLOTTED);
                if (position % 2 != 0)
                {
                    position++;     // start with left channel
                }
                if (position >= tempData.Length)
                {
                    position = tempData.Length - 1;
                }


                for (int channel = 0; channel < NUM_AUDIO_CHANNELS; channel++)
                {
                    int value = (int)tempData[position + channel];

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

        // Draw graph of the current wave pattern.
        // use channel left, right or average of both, depending on channel mode
        public void UpdateWaveGraph()
        {
            parentForm.chartCurrentWave.Series["Series1"].Points.Clear();

            if (CurrentWave.NumSamples()==0)
            {
                return;
            }

            for (uint pointNumber = 0; pointNumber < GRAPH_POINTS_PLOTTED; pointNumber++)
            {
                uint position = (uint)(CurrentWave.NumSamples() * pointNumber / (float)GRAPH_POINTS_PLOTTED);
                double wave_value = 0;
                if (currentWave.Channel != 1)   // add left channel
                {
                    wave_value += CurrentWave.WaveData[position * NUM_AUDIO_CHANNELS];
                }
                if (currentWave.Channel != 0)   // add right channel
                {
                    wave_value += CurrentWave.WaveData[position * NUM_AUDIO_CHANNELS + 1];
                }
                if (currentWave.Channel == 2)   // both channels used
                {
                    wave_value /= 2.0;
                }
                int value = (int)(CalculateCurrentVolume(position, CurrentWave) * wave_value);

                parentForm.chartCurrentWave.Series["Series1"].Points.AddY(value);
            }
        }

        private void CreateWave(WaveInfo waveInfo, double frequency_factor, double amplitude)
        {
            wavePhase = 0;
            for (uint current_sample = 0; current_sample < waveInfo.NumSamples() - 1; current_sample++)
            {
                double frequency = frequency_factor * CalculateCurrentFrequency(current_sample, waveInfo);
                CreateWaveData(waveInfo, frequency, current_sample, amplitude);
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
                    generator.WaveData[current_sample * 2 + channel] = Convert.ToDouble(amplitude * MAX_AMPLITUDE * WaveFunction(generator, (wavePhase + deltaT)%(2*Math.PI), frequency));
                }
            }
            wavePhase += deltaT;
        }

        // input: phase 0..2PI
        // output: a value between -1 and 1
        private double WaveFunction(WaveInfo waveInfo, double phase, double frequency)
        {
            switch (waveInfo.WaveForm)
            {
                case "Custom":
                    return GetWaveFormData(waveInfo, phase, frequency);
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

        private double GetWaveFormData(WaveInfo waveInfo, double phase, double frequency)
        {
            double position = (phase / (2*Math.PI)) * SHAPE_WAVE_NUMPOINTS;
            int int_position = (int)position;
            int int_next_position = (int)position + 1;
            double value = 0;

            if(waveInfo.ShapeWave.Length<=int_position)     // no data
            {
                return 0;
            }

            double waveformSamplesPerStep = SynthGenerator.SHAPE_WAVE_NUMPOINTS / (format.dwSamplesPerSec / frequency);

            if (waveformSamplesPerStep <= 1)
            {
                if (int_next_position == SynthGenerator.SHAPE_WAVE_NUMPOINTS)
                {
                    // note the minus, because the waveform data graph is upside-down
                    value = -waveInfo.ShapeWave[int_position] / (double)SHAPE_WAVE_MAX_VALUE;
                }
                else
                {
                    // interpolate between points from custom graph
                    // note the minus, because the waveform data graph is upside-down
                    value = (-waveInfo.ShapeWave[int_position] * Math.Abs(int_position - position) + -waveInfo.ShapeWave[int_next_position] * Math.Abs(int_next_position - position)) / (double)SHAPE_WAVE_MAX_VALUE;
                }
            }
            else        // there is multiple waveformdata for this part of the wave; take the average of all values
            {
                double next_phase_position = (phase + (Math.PI * 2 * frequency) / format.dwSamplesPerSec) / (2 * Math.PI) * SHAPE_WAVE_NUMPOINTS;
                int count = 0;
                while(int_position < next_phase_position)
                {
                    // note the minus, because the waveform data graph is upside-down
                    value -= waveInfo.ShapeWave[int_position% SynthGenerator.SHAPE_WAVE_NUMPOINTS] / (double)SHAPE_WAVE_MAX_VALUE;
                    count++;
                    int_position++;
                }
                value /= count;
            }

            return value;
        }

        // load a .wav file. Supported is PCM, mono/stereo, 8/16/24 bits, all samplerates.
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
                int numSamples = (int)(reader.SampleCount * sampleRatio);
                CurrentWave.WaveFileData = new int[numSamples * 2];            // output=2 channels
                short[] tempData = new short[numSamples * 2];            // output=2 channels

                if (reader.WaveFormat.BitsPerSample == 8 && reader.WaveFormat.SampleRate == SAMPLES_PER_SECOND && reader.WaveFormat.Channels == 2)
                {
                    // We can copy everything 
                    tempData = Array.ConvertAll(buffer, b => (short)((b - 128) * 256));
                }
                else if (reader.WaveFormat.BitsPerSample == 16 && reader.WaveFormat.SampleRate == SAMPLES_PER_SECOND && reader.WaveFormat.Channels == 2)
                {
                    // We can copy everything 
                    tempData = new short[(int)Math.Ceiling(buffer.Length / 2.0)];
                    CurrentWave.WaveFileData = new int[(int)Math.Ceiling(buffer.Length / 2.0)];
                    Buffer.BlockCopy(buffer, 0, tempData, 0, buffer.Length);
                }
                else
                {
                    for (int current_sample = 0; current_sample < numSamples; current_sample++)
                    {
                        if (reader.WaveFormat.BitsPerSample == 8)
                        {
                            int position = reader.WaveFormat.Channels * (int)(current_sample / sampleRatio);
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
                        else if (reader.WaveFormat.BitsPerSample == 24)
                        {
                            int position = 3 * reader.WaveFormat.Channels * (int)(current_sample / sampleRatio);

                            // Create smaller array in order to add the 4th 8-bit value
                            byte[] byteArrayLeft = new byte[4] { 0, buffer[position + 2], buffer[position + 1], buffer[position] };
                            byte[] byteArrayRight = new byte[4] { 0, buffer[position + 5], buffer[position + 4], buffer[position + 3] };
//                            Array.Reverse(byteArrayLeft);
//                            Array.Reverse(byteArrayRight);

                            // Convert values to 32-bit variables
                            tempData[NUM_AUDIO_CHANNELS * current_sample] = BitConverter.ToInt16(byteArrayLeft, 0);

                            if (reader.WaveFormat.Channels == 2)
                            {
                                tempData[NUM_AUDIO_CHANNELS * current_sample + 1] = BitConverter.ToInt16(byteArrayRight, 0);
                            }
                            else
                            {
                                // use data of first channel for right channel
                                tempData[NUM_AUDIO_CHANNELS * current_sample + 1] = BitConverter.ToInt16(byteArrayLeft, 0);
                            }
                        }
                        else
                        // 16-bits audio
                        {
                            int position = 2 * reader.WaveFormat.Channels * (int)(current_sample / sampleRatio);
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
                    CurrentWave.WaveFileData[i] = tempData[i];
                }

                // update duration
                parentForm.colorSliderDuration.Value = (int)(CurrentWave.NumSamples());

                parentForm.labelFileName.Text = Path.GetFileName(CurrentWave.WaveFile);
                parentForm.SynthGenerator.UpdateCurrentWaveData();
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

        private void CopyToFinalData(double frequencyFactor, double[] tempData)
        {
            if (frequencyFactor == 1)
            {
                finalData.shortArray = new short[tempData.Length];
                for (int i = 0; i < tempData.Length; i++)
                {
                    finalData.shortArray[i] = (short)tempData[i];
                }
            }
            else
            {
                int desired_length = (int)(tempData.Length * frequencyFactor);
                finalData.shortArray = new short[desired_length];
                for (int i = 0; i < finalData.shortArray.Length; i++)
                {
                    int position = (int)(i * frequencyFactor);
                    if (position >= tempData.Length)
                    {
                        position = tempData.Length - 1;
                    }
                    finalData.shortArray[i] = (short)tempData[position];
                }
            }

            finalData.dwChunkSize = (uint)(finalData.shortArray.Length * (format.wBitsPerSample / 8));
            header.dwFileLength = 36 + finalData.dwChunkSize;
        }

        public void Play(double frequencyFactor=1, bool all=true)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(memoryStream))
            {
                if(!all)
                {
                    CopyWaveToTempData(currentWave.WaveData);
                    ApplyVolume(currentWave, tempData);
                }
                CopyToFinalData(frequencyFactor, tempData);
                WriteWaveData(writer, finalData);

                memoryStream.Position = 0;
                new SoundPlayer(memoryStream).Play();
            }
        }

        public WaveInfo CloneWave(double frequencyFactor = 1, double amplitudeFactor = 1)
        {
            int count = 1;
            string name;
            do
            {
                name = DEFAULT_WAVE_NAME + count;
                count++;
            } while(Waves.FindIndex(o => o.Name.Equals(name))!=-1);

            WaveInfo newWave = new WaveInfo(name, currentWave.NumSamples(), currentWave.StartPosition,
                currentWave.MinFrequency * frequencyFactor, currentWave.MaxFrequency * frequencyFactor, (int)(currentWave.MinVolume * amplitudeFactor), (int)(currentWave.MaxVolume * amplitudeFactor),
                currentWave.Channel, currentWave.WaveForm, currentWave.WaveFile, currentWave.Weight);
            newWave.WaveData = new double[currentWave.WaveData.Length];
            newWave.WaveFileData = currentWave.WaveFileData.Clone() as int[];
            newWave.ShapeWave = currentWave.ShapeWave.Clone() as int[];
            newWave.ShapeVolume = currentWave.ShapeVolume.Clone() as int[];
            newWave.ShapeFrequency = currentWave.ShapeFrequency.Clone() as int[];
            Waves.Add(newWave);

            return newWave;
        }

        public void SetCurrentWaveByDisplayName(string name)
        {
            string name_part = name.Substring(0, name.IndexOf(" "));
            CurrentWave = Waves.Find(o => o.Name.Equals(name_part));
        }

        public void RemoveCurrentWave()
        {
            Waves.Remove(currentWave);
        }

    }
}
