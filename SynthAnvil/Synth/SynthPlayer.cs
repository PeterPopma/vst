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

    class SynthPlayer
    {
        public const int NUM_GENERATORS = 4;
        private const int MAX_AMPLITUDE = 32767;     // Max amplitude for 16-bit audio
        private const int NUM_CHANNELS_GENERATORS = 2;
        private const int GRAPH_POINTS_PLOTTED = 300;

        private float envelopAttack = 0;
        private float envelopDecay = 0;
        private float envelopSustain = 1;
        private float envelopRelease = 0;

        // Header, Format, Data chunks
        WaveHeader header = new WaveHeader();
        WaveFormatChunk format = new WaveFormatChunk();
        WaveDataChunk finalData = new WaveDataChunk();
        Double[] tempData;
        Generator[] generators = new Generator[NUM_GENERATORS];

        FormMain ParentForm;

        public SynthPlayer(FormMain parentForm)
        {
            ParentForm = parentForm;
        }

        internal Generator[] Generators { get => generators; set => generators = value; }
        public float EnvelopAttack { get => envelopAttack; set => envelopAttack = value; }
        public float EnvelopDecay { get => envelopDecay; set => envelopDecay = value; }
        public float EnvelopSustain { get => envelopSustain; set => envelopSustain = value; }
        public float EnvelopRelease { get => envelopRelease; set => envelopRelease = value; }

        public void GenerateSound(int generatorNumber)
        {
            CreateShape(Generators[generatorNumber - 1]);
            UpdateGraph(Generators[generatorNumber - 1]);
        }

        private void CreateShape(Generator generator)
        {
            switch (generator.WaveForm)
            {
                case "Sine":
                    CreateSine(generator);
                    break;
                case "Square":
                    CreateSquare(generator);
                    break;
                case "Sawtooth":
                    CreateSawtooth(generator);
                    break;
                case "Triangle":
                    CreateTriangle(generator);
                    break;
                case "Noise":
                    CreateNoise(generator);
                    break;
            }
        }

        public void GenerateSound()
        {
            foreach (Generator currentGenerator in generators)
            {
                CreateShape(currentGenerator);
            }

            MixGenerators();
            UpdateGraphs();
        }

        private int findMaxNumSamples()
        {
            int max_duration = 0;
            foreach (Generator currentGenerator in generators)
            {
                if ((currentGenerator.Weight>0) && currentGenerator.NumSamples > max_duration)
                {
                    max_duration = currentGenerator.NumSamples;
                }
            }
            return max_duration;
        }

        private float CalculateCurrentVolume(uint currentPosition, Generator generator)
        {
            if (generator.EndVolumeEnabled)
            {
                return ((generator.BeginVolume * (1 - currentPosition / (float)generator.NumSamples)) + (generator.EndVolume * currentPosition / (float)generator.NumSamples)) / 100.0f;
            }
            else
            {
                return generator.BeginVolume / 100.0f;
            }
        }
        
        private void ApplyVolume(Generator generator)
        {
            for (uint samplePosition = 0; samplePosition < generator.NumSamples - 1; samplePosition++)
            {
                for (int channel = 0; channel < format.wChannels; channel++)
                {
                    generator.WaveData[samplePosition * 2 + channel] = (short)(CalculateCurrentVolume(samplePosition, generator) * generator.WaveData[samplePosition * 2 + channel]);
                }
            }
        }


        private float CalculateCurrentFrequency(uint currentPosition, Generator generator)
        {
            if (generator.EndFrequencyEnabled)
            {
                return (generator.BeginFrequency * (1 - currentPosition / (float)generator.NumSamples)) + (generator.EndFrequency * currentPosition / (float)generator.NumSamples);
            }
            else
            {
                return generator.BeginFrequency;
            }
        }

        private int CalcTotalWeight()
        {
            int total_weight = 0;
            for(int num_generator = 0; num_generator<NUM_GENERATORS; num_generator++)
            {
                total_weight += Generators[num_generator].Weight;
            }

            if(total_weight==0)
            {
                for (int num_generator = 0; num_generator < NUM_GENERATORS; num_generator++)
                {
                    Generators[num_generator].Weight = 100;
                }
                total_weight = 100 * NUM_GENERATORS;
            }

            return total_weight;
        }

        private void MixGenerators()
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
                    foreach (Generator currentGenerator in Generators)
                    {
                        int value = 0;
                        if (currentGenerator.Channel == 2 || currentGenerator.Channel == channel)
                        {
                            num_participating_generators++;
                            if (samplePosition >= currentGenerator.StartPosition && samplePosition < currentGenerator.StartPosition + currentGenerator.NumSamples)
                            {
                                value = (int)(currentGenerator.Weight / (float)total_weight * CalculateCurrentVolume(samplePosition, currentGenerator) * currentGenerator.WaveData[samplePosition * 2 + channel]);
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
                ApplyADSR(samplePosition);
            }

            NormalizeVolume();
            CopyTempToFinalData(tempData);

            ParentForm.labelDuration.Text = string.Format("{0:0.00} s", numSamples/44100.0);
        }

        private void NormalizeVolume()
        {
            double max_volume = 0;
            for (uint samplePosition = 0; samplePosition < tempData.Length; samplePosition++)
            {
                if (Math.Abs(tempData[samplePosition]) > max_volume)
                {
                    max_volume = Math.Abs(tempData[samplePosition]);
                }
            }
            if(max_volume==0)
            {
                return;
            }

            double scale_factor = MAX_AMPLITUDE / max_volume;
            for (uint samplePosition = 0; samplePosition < tempData.Length; samplePosition++)
            {
                tempData[samplePosition] *= scale_factor;
            }
        }

        public void UpdateADSRChart()
        {
            ParentForm.chartADSR.Series["Series1"].Points.Clear();
            float position;

            for (uint graphPoint = 0; graphPoint < GRAPH_POINTS_PLOTTED; graphPoint++)
            {
                position = graphPoint / (float)GRAPH_POINTS_PLOTTED;
                ParentForm.chartADSR.Series["Series1"].Points.AddY(CalcADSR(position));
            }
        }

        private float CalcADSR(float position)
        {
            float volume_factor;
            if (envelopAttack > 0 && position <= envelopAttack)
            {
                volume_factor = position / envelopAttack;
            }
            else if (position <= envelopAttack + envelopDecay)
            {
                volume_factor = 1 - ((position-envelopAttack)/(envelopDecay)) * (1 - envelopSustain);
            }
            else if (position >= 1 - envelopRelease)
            {
                volume_factor = envelopSustain * (1 - position) / envelopRelease;
            }
            else
            {
                volume_factor = envelopSustain;
            }

            return volume_factor;
        }

        private void ApplyADSR(uint samplePosition)
        {
            float position = samplePosition / (tempData.Length / (float)NUM_CHANNELS_GENERATORS);
            float volume_factor = CalcADSR(position);

            tempData[samplePosition * 2] = (short)(tempData[samplePosition * 2] * volume_factor);
            tempData[samplePosition * 2 + 1] = (short)(tempData[samplePosition * 2 + 1] * volume_factor);
        }

        private void UpdateGraphs()
        {
            for (int current_generator = 0; current_generator < NUM_GENERATORS; current_generator++)
            {
                UpdateGraph(Generators[current_generator]);
            }
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

        private void UpdateGraph(Generator generator)
        {
            if (generator.Number == 1)
            {
                ParentForm.chartGenerator1Left.Series["Series1"].Points.Clear();
                ParentForm.chartGenerator1Right.Series["Series1"].Points.Clear();
            }
            if (generator.Number == 2)
            {
                ParentForm.chartGenerator2Left.Series["Series1"].Points.Clear();
                ParentForm.chartGenerator2Right.Series["Series1"].Points.Clear();
            }
            if (generator.Number == 3)
            {
                ParentForm.chartGenerator3Left.Series["Series1"].Points.Clear();
                ParentForm.chartGenerator3Right.Series["Series1"].Points.Clear();
            }
            if (generator.Number == 4)
            {
                ParentForm.chartGenerator4Left.Series["Series1"].Points.Clear();
                ParentForm.chartGenerator4Right.Series["Series1"].Points.Clear();
            }

            int num_samples = finalData.shortArray.Length / 2;
            for (uint pointNumber = 0; pointNumber < GRAPH_POINTS_PLOTTED; pointNumber++)
            {
                uint position = (uint)(num_samples * NUM_CHANNELS_GENERATORS * pointNumber / (float)GRAPH_POINTS_PLOTTED);
                if (position % 2 != 0)
                {
                    position++;     // start with left channel
                }

                for (int channel = 0; channel < NUM_CHANNELS_GENERATORS; channel++)
                {
                    int value;
                    if (position >= (NUM_CHANNELS_GENERATORS * generator.StartPosition) &&
                        position < (NUM_CHANNELS_GENERATORS * (generator.StartPosition + generator.NumSamples)))
                    {
                        value = (int)(CalculateCurrentVolume(position / NUM_CHANNELS_GENERATORS, generator) * generator.WaveData[position + channel]);
                    }
                    else
                    {
                        value = 0;
                    }

                    if (channel == 0)
                    {
                        if (generator.Number == 1)
                        {
                            ParentForm.chartGenerator1Left.Series["Series1"].Points.AddY(value);
                        }
                        if (generator.Number == 2)
                        {
                            ParentForm.chartGenerator2Left.Series["Series1"].Points.AddY(value);
                        }
                        if (generator.Number == 3)
                        {
                            ParentForm.chartGenerator3Left.Series["Series1"].Points.AddY(value);
                        }
                        if (generator.Number == 4)
                        {
                            ParentForm.chartGenerator4Left.Series["Series1"].Points.AddY(value);
                        }
                    }
                    else
                    {
                        if (generator.Number == 1)
                        {
                            ParentForm.chartGenerator1Right.Series["Series1"].Points.AddY(value);
                        }
                        if (generator.Number == 2)
                        {
                            ParentForm.chartGenerator2Right.Series["Series1"].Points.AddY(value);
                        }
                        if (generator.Number == 3)
                        {
                            ParentForm.chartGenerator3Right.Series["Series1"].Points.AddY(value);
                        }
                        if (generator.Number == 4)
                        {
                            ParentForm.chartGenerator4Right.Series["Series1"].Points.AddY(value);
                        }
                    }
                }
            }

        }

        // Adds a wave 
        private void AddWave(Generator generator, int frequency_factor, double amplitude)
        {
            double position = 0;
            for (uint current_sample = 0; current_sample < generator.NumSamples - 1; current_sample++)
            {
                double frequency = frequency_factor * CalculateCurrentFrequency(current_sample, generator);
                position += CreateSineData(position, generator, frequency, current_sample, amplitude);
            }

            Console.WriteLine();
        }

        private double CreateSineData(double position, Generator generator, double frequency, uint current_sample, double amplitude)
        {
            if (frequency<0)
            {
                throw new Exception("frequency should not be negative!");
            }
            // The "angle" used in the function, adjusted for the number of channels and sample rate.
            // This value is the period of the wave.
            position += (Math.PI * 2 * frequency) / (format.dwSamplesPerSec * format.wChannels);
// TODO : if frequency is diminishing fast, [t] is not high enough to make wave progress
// in other words: we must keep track of position in sine wave
            // Fill with a simple sine wave at max amplitude
            for (int channel = 0; channel < NUM_CHANNELS_GENERATORS; channel++)
            {
                if (generator.Channel == 2 || generator.Channel == channel)
                {
                    generator.WaveData[current_sample * 2 + channel] += Convert.ToDouble(amplitude * MAX_AMPLITUDE * Math.Sin(position));
                    //        Console.Write(t * current_sample + " ");
                    Console.Write(Convert.ToInt16(amplitude * MAX_AMPLITUDE * Math.Sin(position * current_sample)) + " ");
                }
            }

            return position;
        }


        private void CreateSine(Generator generator)
        {
            double scaleFactor = 1;
   //        double decayFactor = 0.6 + (generator.Harmonics/65.0);
            for (int i = 1; i <= generator.Harmonics; i++)
            {
                if (i % 2 == 0)
                {
                    scaleFactor += Math.Pow(generator.HarmDecayOdd, i);
                }
                else
                {
                    scaleFactor += Math.Pow(generator.HarmDecayEven, i);
                }
            }
            scaleFactor = 1 / scaleFactor;

            // Initialize the 128-bit array
            generator.WaveData = new double[generator.NumSamples * NUM_CHANNELS_GENERATORS];

            AddWave(generator, 1, scaleFactor);

            for (int i = 1; i <= generator.Harmonics; i++)
            {
                double amplitude;
                if (i % 2 == 0)
                {
                    amplitude = Math.Pow(generator.HarmDecayEven, i);
                }
                else
                {
                    amplitude = Math.Pow(generator.HarmDecayOdd, i);
                }
                AddWave(generator, i+1, amplitude*scaleFactor);
            }
        }



        // load a .wav file. Supported is PCM, mono/stereo, 8/16 bits, all samplerates.
        // loaded file is transformed into a 44100 Kz 16 bits stereo stream. 
        // if the source is mono, the data is copied to both streams
        public void LoadWaveFile(Generator generator, string fileName)
        {
            generator.WaveFile = fileName;
            using (WaveFileReader reader = new WaveFileReader("wav\\" + generator.WaveFile + ".wav"))
            {
                byte[] buffer = new byte[reader.Length];
                int read = reader.Read(buffer, 0, buffer.Length);
                double sampleRatio = 44100.0 / reader.WaveFormat.SampleRate;
                generator.NumSamples = (int)(reader.SampleCount * sampleRatio);
                // Initialize the 128-bit array
                generator.WaveData = new double[generator.NumSamples * 2];            // output=2 channels

                short[] test = new short[(int)Math.Ceiling(buffer.Length / 2.0)];
                Buffer.BlockCopy(buffer, 0, test, 0, buffer.Length);

                if (reader.WaveFormat.SampleRate == 44100 && reader.WaveFormat.Channels == 2)
                {
                    // We can copy everything 
                    if (reader.WaveFormat.BitsPerSample == 8)
                    {
                        generator.WaveData = Array.ConvertAll(buffer, b => (double)((b - 128) * 256));
                    }
                    else
                    {
                        // Initialize the 16-bit array
                        generator.WaveData = new double[(int)Math.Ceiling(buffer.Length / 2.0)];
                        Buffer.BlockCopy(buffer, 0, generator.WaveData, 0, buffer.Length);
                    }
                }
                else
                {
                    for (int current_sample = 0; current_sample < generator.NumSamples; current_sample++)
                    {
                        int position = (int)(current_sample / sampleRatio);
                        if (reader.WaveFormat.BitsPerSample == 8)
                        {
                            generator.WaveData[NUM_CHANNELS_GENERATORS * current_sample] = (short)((buffer[position] - 128) * 256);
                            if (reader.WaveFormat.Channels == 2)
                            {
                                generator.WaveData[NUM_CHANNELS_GENERATORS * current_sample + 1] = (short)((buffer[position + 1] - 128) * 256);
                            }
                            else
                            {
                                // use data of first channel for right channel
                                generator.WaveData[NUM_CHANNELS_GENERATORS * current_sample + 1] = (short)((buffer[position] - 128) * 256);
                            }
                        }
                        else
                        {
                            generator.WaveData[NUM_CHANNELS_GENERATORS * current_sample] = BitConverter.ToInt16(new byte[2] { buffer[position], buffer[position + 1] }, 0);
                            if (reader.WaveFormat.Channels == 2)
                            {
                                generator.WaveData[NUM_CHANNELS_GENERATORS * current_sample + 1] = BitConverter.ToInt16(new byte[2] { buffer[position + 2], buffer[position + 3] }, 0);
                            }
                            else
                            {
                                // use data of first channel for right channel
                                generator.WaveData[NUM_CHANNELS_GENERATORS * current_sample + 1] = BitConverter.ToInt16(new byte[2] { buffer[position], buffer[position + 1] }, 0);
                            }
                        }
                    }
                }

                // update duration
                generator.NumSamples = generator.WaveData.Length / 2;
                ColorSlider.ColorSlider colorSlider = (ColorSlider.ColorSlider)ParentForm.Controls.Find("colorSliderDuration" + generator.Number, true)[0];
                colorSlider.Value = (decimal)(generator.NumSamples / 441);
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

        private void CreateSquare(Generator generator)
        {
            double freq = generator.BeginFrequency;   // Concert A: 440Hz

            // Initialize the 128-bit array
            generator.WaveData = new double[generator.NumSamples * NUM_CHANNELS_GENERATORS];

            // The "angle" used in the function, adjusted for the number of channels and sample rate.
            // This value is like the period of the wave.
            double t = (Math.PI * 2 * freq) / (format.dwSamplesPerSec * format.wChannels);

            for (int i = 0; i < generator.NumSamples - 1; i++)
            {
                for (int channel = 0; channel < format.wChannels; channel++)
                {
                    if (generator.Channel == 2 || generator.Channel == channel)
                    {
                        generator.WaveData[i * 2 + channel] = Convert.ToDouble(MAX_AMPLITUDE * Math.Sign(Math.Sin(t * i)));
                    }
                }
            }
        }

        private void CreateSawtooth(Generator generator)
        {
            double frequency = generator.BeginFrequency;   // Concert A: 440Hz

            // Initialize the 16-bit array
            generator.WaveData = new double[generator.NumSamples * NUM_CHANNELS_GENERATORS];

            // Determine the number of samples per wavelength
            int samplesPerWavelength = Convert.ToInt32(format.dwSamplesPerSec / (frequency / format.wChannels));

            // Determine the amplitude step for consecutive samples
            short ampStep = Convert.ToInt16((MAX_AMPLITUDE * 2) / samplesPerWavelength);
            // Temporary sample value, added to as we go through the loop
            short tempSample;

            // Total number of samples written so we know when to stop
            int totalSamplesWritten = 0;

            while (totalSamplesWritten < generator.NumSamples)
            {
                tempSample = (short)-MAX_AMPLITUDE;

                for (uint i = 0; i < samplesPerWavelength && totalSamplesWritten < generator.NumSamples; i++)
                {
                    for (int channel = 0; channel < format.wChannels; channel++)
                    {
                        if (generator.Channel == 2 || generator.Channel == channel)
                        {
                            tempSample += ampStep;
                            generator.WaveData[totalSamplesWritten * 2 + channel] = tempSample;
                        }
                    }
                    totalSamplesWritten++;
                }
            }
        }

        private void CreateTriangle(Generator generator)
        {
            double frequency = generator.BeginFrequency;   // Concert A: 440Hz

            // Initialize the 128-bit array
            generator.WaveData = new double[generator.NumSamples * NUM_CHANNELS_GENERATORS];

            // Determine the number of samples per wavelength
            int samplesPerWavelength = Convert.ToInt32(format.dwSamplesPerSec / (frequency / format.wChannels));

            short ampStep = Convert.ToInt16((MAX_AMPLITUDE * 2) / samplesPerWavelength);
            // Determine the amplitude step for consecutive samples
            // Temporary sample value, added to as we go through the loop
            int tempSample = -MAX_AMPLITUDE;

            for (int i = 0; i < generator.NumSamples - 1; i++)
            {
                tempSample += ampStep;

                // Negate ampstep whenever it hits the amplitude boundary
                if (tempSample >= MAX_AMPLITUDE)
                {
                    ampStep = (short)-ampStep;
                    tempSample = MAX_AMPLITUDE;
                }
                else if (tempSample <= -MAX_AMPLITUDE)
                {
                    ampStep = (short)-ampStep;
                    tempSample = -MAX_AMPLITUDE;
                }

                for (int channel = 0; channel < format.wChannels; channel++)
                {
                    if (generator.Channel == 2 || generator.Channel == channel)
                    {
                        generator.WaveData[i * 2 + channel] = (short)tempSample;
                    }
                }
            }
        }

        private void CreateNoise(Generator generator)
        {
            Random rnd = new Random();
            short randomValue = 0;

            for (int i = 0; i < generator.NumSamples; i++)
            {
                for (int channel = 0; channel < format.wChannels; channel++)
                {
                    if (generator.Channel == 2 || generator.Channel == channel)
                    {
                        randomValue = Convert.ToInt16(rnd.Next(-MAX_AMPLITUDE, MAX_AMPLITUDE));
                        generator.WaveData[i * 2 + channel] = randomValue;
                    }
                }
            }
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
                    ApplyVolume(Generators[generator - 1]);
                    CopyTempToFinalData(generators[generator - 1].WaveData);
                }
                WriteWaveData(writer, finalData);

                memoryStream.Position = 0;
                new SoundPlayer(memoryStream).Play();
            }
        }

    }
}
