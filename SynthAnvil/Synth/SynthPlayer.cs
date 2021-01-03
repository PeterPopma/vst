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

        // Header, Format, Data chunks
        WaveHeader header = new WaveHeader();
        WaveFormatChunk format = new WaveFormatChunk();
        WaveDataChunk data = new WaveDataChunk();
        Generator[] generators = new Generator[NUM_GENERATORS];
        const int maxAmplitude = 32767;     // Max amplitude for 16-bit audio

        internal Generator[] Generators { get => generators; set => generators = value; }

        public void GenerateSound(int generatorNumber, string saveLocation="")
        {
            header = new WaveHeader();
            format = new WaveFormatChunk();

            foreach (Generator currentGenerator in generators)
            {              
                if ((generatorNumber == 0 && currentGenerator.Enabled == false) || (generatorNumber > 0 && currentGenerator.Number != generatorNumber))
                    continue;

                // Initialize the 16-bit array
                currentGenerator.WaveData.shortArray = new short[currentGenerator.NumSamples * 2];

                switch (currentGenerator.WaveForm)
                {
                    case "Sine":
                        CreateSine(currentGenerator);
                        break;
                    case "Square":
                        CreateSquare(currentGenerator);
                        break;
                    case "Sawtooth":
                        CreateSawtooth(currentGenerator);
                        break;
                    case "Inverted Sawtooth":
                        CreateInverseSawtooth(currentGenerator);
                        break;
                    case "Triangle":
                        CreateTriangle(currentGenerator);
                        break;
                    case "Noise":
                        CreateNoise(currentGenerator);
                        break;
                    case "Wave File":
                        CreateWaveFile(currentGenerator);
                        break;
                }

                // Calculate data chunk size in bytes
                currentGenerator.WaveData.dwChunkSize = (uint)(currentGenerator.WaveData.shortArray.Length * (format.wBitsPerSample / 8));
            }

            MixGenerators(generatorNumber);

            if (saveLocation.Length>0)
            {
                Save(@saveLocation);
            }
            else
            {
                Play();
            }
        }

        private int findMaxNumSamples(int generatorNumber)
        {
            int max_duration = 0;
            foreach (Generator currentGenerator in generators)
            {
                if(((generatorNumber == 0 && currentGenerator.Enabled) || (generatorNumber == currentGenerator.Number)) && currentGenerator.NumSamples>max_duration)
                {
                    max_duration = currentGenerator.NumSamples;
                }
            }
            return max_duration;
        }

        private float CalculateCurrentVolume(uint currentPosition, Generator generator)
        {
            return ((generator.BeginVolume * (1 - currentPosition / (float)generator.NumSamples)) + (generator.EndVolume * currentPosition / (float)generator.NumSamples)) / 100.0f;
        }

        private float CalculateCurrentFrequency(uint currentPosition, Generator generator)
        {
            return (generator.BeginFrequency * (1 - currentPosition / (float)generator.NumSamples)) + (generator.EndFrequency * currentPosition / (float)generator.NumSamples);
        }

        private void MixGenerators(int generatorNumber)
        {
            data = new WaveDataChunk();

            int numSamples = findMaxNumSamples(generatorNumber);

            // Initialize the 16-bit array
            data.shortArray = new short[numSamples * 2]; 
            
            long mixed_value;

            for (uint samplePosition = 0; samplePosition < numSamples - 1; samplePosition++)
            {
                // Fill with a simple sine wave at max amplitude
                for (int channel = 0; channel < format.wChannels; channel++)
                {
                    mixed_value = 0;
                    int num_participating_generators = 0;
                    foreach(Generator currentGenerator in Generators)
                    {
                        if( ((generatorNumber==0 && currentGenerator.Enabled) || (generatorNumber==currentGenerator.Number)) && (currentGenerator.Channel==2 || currentGenerator.Channel==channel))
                        {
                            num_participating_generators++;
                            if (samplePosition < currentGenerator.NumSamples)
                            {
                                mixed_value += (int)(CalculateCurrentVolume(samplePosition, currentGenerator) * currentGenerator.WaveData.shortArray[samplePosition * 2 + channel]);
                                //                            Console.WriteLine(mixed_value);
                            }
                        }
                    }
                    if(num_participating_generators==0)
                    {
                        data.shortArray[samplePosition * 2 + channel] = 0;
                    }
                    else
                    {
                        data.shortArray[samplePosition * 2 + channel] = (short)(mixed_value / num_participating_generators);
                    }
                }
            }

            // Calculate data chunk size in bytes
            data.dwChunkSize = (uint)(data.shortArray.Length * (format.wBitsPerSample / 8));
        }

        private void CreateSine(Generator generator)
        {
            for (uint current_sample = 0; current_sample < generator.NumSamples - 1; current_sample++)
            {
                double freq = CalculateCurrentFrequency(current_sample, generator);
               // Console.Write(freq);
              //  Console.Write(" ");

                // The "angle" used in the function, adjusted for the number of channels and sample rate.
                // This value is like the period of the wave.
                double t = (Math.PI * 2 * freq) / (format.dwSamplesPerSec * format.wChannels);

                // Fill with a simple sine wave at max amplitude
                for (int channel = 0; channel < format.wChannels; channel++)
                {
                    if (generator.Channel == 2 || generator.Channel == channel)
                    {
                        generator.WaveData.shortArray[current_sample*2 + channel] = Convert.ToInt16(maxAmplitude * Math.Sin(t * current_sample));
                    }
                }
            }
            //Console.WriteLine();
        }

        private void CreateWaveFile(Generator generator)
        {
            using (WaveFileReader reader = new WaveFileReader("wav\\" + generator.WaveFile + ".wav"))
            {
                byte[] buffer = new byte[reader.Length];
                int read = reader.Read(buffer, 0, buffer.Length);
                double sampleRatio = 44100.0 / reader.WaveFormat.SampleRate;
                generator.NumSamples = (int)(reader.SampleCount * sampleRatio);
                // Initialize the 16-bit array
                generator.WaveData.shortArray = new short[generator.NumSamples * 2];
                /*
                                if (reader.WaveFormat.SampleRate == 44100)
                                {
                                    if (reader.WaveFormat.BitsPerSample == 8)
                                    {
                                        generator.WaveData.shortArray = Array.ConvertAll(buffer, b => (short)((b - 128) * 256));
                                    }
                                    else
                                    {
                                        // Initialize the 16-bit array
                                        generator.WaveData.shortArray = new short[read / 2];
                                        Buffer.BlockCopy(buffer, 0, generator.WaveData.shortArray, 0, read);
                                    }
                                }
                                else 
                */
                short[] test = new short[buffer.Length / 2];
                Buffer.BlockCopy(buffer, 0, test, 0, read);
// TODO take into account number of channels
                {
                    for (int current_sample = 0; current_sample < generator.NumSamples; current_sample += reader.WaveFormat.Channels)
                    {
                        int position = (int)(2*current_sample/sampleRatio);
                        if (reader.WaveFormat.BitsPerSample == 8)
                        {
                            generator.WaveData.shortArray[current_sample] = (short)((buffer[position] - 128) * 256);
                        }
                        else
                        {
                            if(generator.Channel==0 || generator.Channel == 1)
                            {
                                generator.WaveData.shortArray[current_sample] = BitConverter.ToInt16(new byte[2] { buffer[position], buffer[position + 1] }, 0);
                            }
                            else
                            {
                                // empty left channel
                                generator.WaveData.shortArray[current_sample] = 0;
                            }

                            if (generator.Channel == 0 || generator.Channel == 2)
                            {
                                if (reader.WaveFormat.Channels == 2)
                                {
                                    generator.WaveData.shortArray[current_sample + 1] = BitConverter.ToInt16(new byte[2] { buffer[position + 2], buffer[position + 3] }, 0);
                                }
                                else
                                {
                                    // use data of first channel for right channel
                                    generator.WaveData.shortArray[current_sample + 1] = BitConverter.ToInt16(new byte[2] { buffer[position], buffer[position + 1] }, 0);
                                }
                            }
                            else
                            {
                                // empty right channel
                                generator.WaveData.shortArray[current_sample + 1] = 0;
                            }
                        }
                    }
                }

                generator.NumSamples = generator.WaveData.shortArray.Length / 2;
            }
        }

        private void CreateWaveFile2(Generator generator)
        {

            FileStream fileStream = new FileStream("wav\\" + generator.WaveFile + ".wav", FileMode.Open);

            // Use BinaryWriter to write the bytes to the file
            BinaryReader reader = new BinaryReader(fileStream);

            // Read the wave file header from the buffer. 
            int chunkID = reader.ReadInt32();    // ASCII string = "RIFF"
            int fileSize = reader.ReadInt32();
            int riffType = reader.ReadInt32();   // ASCII string "WAVE"
            int fmtID = reader.ReadInt32();      // ASCII string "fmt "
            int fmtSize = reader.ReadInt32();    // 16 = PCM (actually specifies size of data of this section)
            short fmtCode = reader.ReadInt16();
            short channels = reader.ReadInt16();
            int sampleRate = reader.ReadInt32();
            int fmtAvgBPS = reader.ReadInt32();
            short fmtBlockAlign = reader.ReadInt16();
            short bitDepth = reader.ReadInt16();

            short[] extraPars = null;
            if (fmtSize != 16)
            {
                short NoOfEPs = reader.ReadInt16();
                extraPars = new short[NoOfEPs];
                for (int i = 0; i < NoOfEPs; i++)
                    extraPars[i] = reader.ReadInt16(); ;
            }

            int dataID = reader.ReadInt32();    // ASCII string = "data"
            int dataSize = reader.ReadInt32();

            byte[] byteArray = new byte[dataSize];
            byteArray = reader.ReadBytes(dataSize);
            fileStream.Close();

            // Initialize the 16-bit array
            generator.WaveData.shortArray = new short[(int)Math.Ceiling(byteArray.Length / 2.0)];
            generator.NumSamples = generator.WaveData.shortArray.Length / 2;
            Buffer.BlockCopy(byteArray, 0, generator.WaveData.shortArray, 0, byteArray.Length);
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

            // The "angle" used in the function, adjusted for the number of channels and sample rate.
            // This value is like the period of the wave.
            double t = (Math.PI * 2 * freq) / (format.dwSamplesPerSec * format.wChannels);

            for (int i = 0; i < generator.NumSamples - 1; i++)
            {
                for (int channel = 0; channel < format.wChannels; channel++)
                {
                    if (generator.Channel == 2 || generator.Channel == channel)
                    {
                        generator.WaveData.shortArray[i*2 + channel] = Convert.ToInt16(maxAmplitude * Math.Sign(Math.Sin(t * i)));
                    }
                }
            }
        }

        private void CreateSawtooth(Generator generator)
        {
            double frequency = generator.BeginFrequency;   // Concert A: 440Hz
            
            // Determine the number of samples per wavelength
            int samplesPerWavelength = Convert.ToInt32(format.dwSamplesPerSec / (frequency / format.wChannels));

            // Determine the amplitude step for consecutive samples
            short ampStep = Convert.ToInt16((maxAmplitude * 2) / samplesPerWavelength);
            // Temporary sample value, added to as we go through the loop
            short tempSample = (short)-maxAmplitude;

            // Total number of samples written so we know when to stop
            int totalSamplesWritten = 0;

            while (totalSamplesWritten < generator.NumSamples)
            {
                tempSample = (short)-maxAmplitude;

                for (uint i = 0; i < samplesPerWavelength && totalSamplesWritten < generator.NumSamples; i++)
                {
                    for (int channel = 0; channel < format.wChannels; channel++)
                    {
                        if (generator.Channel == 2 || generator.Channel == channel)
                        {
                            tempSample += ampStep;
                            generator.WaveData.shortArray[totalSamplesWritten*2 + channel] = tempSample;

                            totalSamplesWritten++;
                        }
                    }
                }
            }
        }

        private void CreateTriangle(Generator generator)
        {
            double frequency = generator.BeginFrequency;   // Concert A: 440Hz

            // Determine the number of samples per wavelength
            int samplesPerWavelength = Convert.ToInt32(format.dwSamplesPerSec / (frequency / format.wChannels));

            // Determine the amplitude step for consecutive samples
            short ampStep = Convert.ToInt16((maxAmplitude * 2) / samplesPerWavelength);
            // Temporary sample value, added to as we go through the loop
            short tempSample = (short)-maxAmplitude;

            for (int i = 0; i < generator.NumSamples - 1; i++)
            {
                for (int channel = 0; channel < format.wChannels; channel++)
                {
                    if (generator.Channel == 2 || generator.Channel == channel)
                    {
                        // Negate ampstep whenever it hits the amplitude boundary
                        if (Math.Abs((int)tempSample) > maxAmplitude)
                            ampStep = (short)-ampStep;

                        tempSample += ampStep;
                        generator.WaveData.shortArray[i*2 + channel] = tempSample;
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
                        randomValue = Convert.ToInt16(rnd.Next(-maxAmplitude, maxAmplitude));
                        generator.WaveData.shortArray[i*2 + channel] = randomValue;
                    }
                }
            }
        }


        private void CreateInverseSawtooth(Generator generator)
        {
            double frequency = generator.BeginFrequency;   // Concert A: 440Hz

            // Determine the number of samples per wavelength
            int samplesPerWavelength = Convert.ToInt32(format.dwSamplesPerSec / (frequency / format.wChannels));

            // Determine the amplitude step for consecutive samples
            short ampStep = Convert.ToInt16((maxAmplitude * 2) / samplesPerWavelength);
            // Temporary sample value, added to as we go through the loop
            short tempSample = (short)-maxAmplitude;

            // Total number of samples written so we know when to stop
            int totalSamplesWritten = 0;

            while (totalSamplesWritten < generator.NumSamples)
            {
                tempSample = 0;

                for (uint i = 0; i < samplesPerWavelength && totalSamplesWritten < generator.NumSamples; i++)
                {
                    for (int channel = 0; channel < format.wChannels; channel++)
                    {
                        if (generator.Channel == 0 || (generator.Channel == 1 && channel == 0) || (generator.Channel == 2 && channel == 1))
                        {
                            tempSample -= ampStep;
                            generator.WaveData.shortArray[totalSamplesWritten*2 + channel] = tempSample;

                            totalSamplesWritten++;
                        }
                    }
                }
            }
        }

        public void Save(string filePath)
        {
            // Create a file (it always overwrites)
            FileStream fileStream = new FileStream(filePath, FileMode.Create);

            // Use BinaryWriter to write the bytes to the file
            BinaryWriter writer = new BinaryWriter(fileStream);

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

            writer.Seek(4, SeekOrigin.Begin);
            uint filesize = (uint)writer.BaseStream.Length;
            writer.Write(filesize - 8);

            // Clean up
            writer.Close();
            fileStream.Close();
        }

        public void Play()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(memoryStream))
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
                writer.Seek(4, SeekOrigin.Begin);
                uint filesize = (uint)writer.BaseStream.Length;
                writer.Write(filesize - 8);

                memoryStream.Position = 0;
                new SoundPlayer(memoryStream).Play();
            }
        }

    }
}
