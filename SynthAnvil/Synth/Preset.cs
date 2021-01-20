using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthAnvil.Synth
{
    class Preset
    {
        public void Save(SynthGenerator synthGenerator, string name)
        {
            if(name.Length==0)
            {
                throw new Exception("preset name not set!");
            }
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@".\presets\" + name + ".pst"))
            {
                file.WriteLine(synthGenerator.EnvelopAttack);
                file.WriteLine(synthGenerator.EnvelopHold);
                file.WriteLine(synthGenerator.EnvelopDecay);
                file.WriteLine(synthGenerator.EnvelopSustain);
                file.WriteLine(synthGenerator.EnvelopRelease);
                file.WriteLine(synthGenerator.Waves.Count);
                for (int i = 0; i < synthGenerator.Waves.Count; i++)
                {
                    WaveInfo waveInfo = synthGenerator.Waves[i];
                    file.WriteLine(waveInfo.BeginFrequency);
                    file.WriteLine(waveInfo.BeginVolume);
                    file.WriteLine(waveInfo.BeginEndBeginFrequencyEnabled);
                    file.WriteLine(waveInfo.BeginEndBeginVolumeEnabled);
                    file.WriteLine(waveInfo.Channel);
                    file.WriteLine(waveInfo.EndFrequency);
                    file.WriteLine(waveInfo.EndFrequencyEnabled);
                    file.WriteLine(waveInfo.EndVolume);
                    file.WriteLine(waveInfo.EndVolumeEnabled);
                    file.WriteLine(waveInfo.HarmonicsOdd);
                    file.WriteLine(waveInfo.HarmonicsEven);
                    file.WriteLine(waveInfo.HarmonicDecayOdd);
                    file.WriteLine(waveInfo.HarmonicDecayEven);
                    file.WriteLine(waveInfo.InHarmonicSpread);
                    file.WriteLine(waveInfo.InHarmonicDecay);
                    file.WriteLine(waveInfo.InHarmonics);
                    file.WriteLine(waveInfo.Number);
                    file.WriteLine(waveInfo.NumSamples);
                    file.WriteLine(waveInfo.StartPosition);
                    file.WriteLine(waveInfo.WaveFile);
                    file.WriteLine(waveInfo.WaveForm);
                    file.WriteLine(waveInfo.Weight);
                }
            }
        }

        public void Load(SynthGenerator synthGenerator, string name)
        {
            using (StreamReader srFile = new StreamReader(@".\presets\" + name + ".pst"))
            {
                synthGenerator.EnvelopAttack = float.Parse(srFile.ReadLine());
                synthGenerator.EnvelopHold = float.Parse(srFile.ReadLine());
                synthGenerator.EnvelopDecay = float.Parse(srFile.ReadLine());
                synthGenerator.EnvelopSustain = float.Parse(srFile.ReadLine());
                synthGenerator.EnvelopRelease = float.Parse(srFile.ReadLine());
                int numWaves = int.Parse(srFile.ReadLine());
                for (int i = 0; i < numWaves; i++)
                {
                    WaveInfo waveInfo = synthGenerator.Waves[i];
                    synthGenerator.Waves[i].BeginFrequency = float.Parse(srFile.ReadLine());
                    synthGenerator.Waves[i].BeginVolume = int.Parse(srFile.ReadLine());
                    synthGenerator.Waves[i].BeginEndBeginFrequencyEnabled = bool.Parse(srFile.ReadLine());
                    synthGenerator.Waves[i].BeginEndBeginVolumeEnabled = bool.Parse(srFile.ReadLine());
                    synthGenerator.Waves[i].Channel = int.Parse(srFile.ReadLine());
                    synthGenerator.Waves[i].EndFrequency = float.Parse(srFile.ReadLine());
                    synthGenerator.Waves[i].EndFrequencyEnabled = bool.Parse(srFile.ReadLine());
                    synthGenerator.Waves[i].EndVolume = int.Parse(srFile.ReadLine());
                    synthGenerator.Waves[i].EndVolumeEnabled = bool.Parse(srFile.ReadLine());
                    synthGenerator.Waves[i].HarmonicsOdd = int.Parse(srFile.ReadLine());
                    synthGenerator.Waves[i].HarmonicsEven = int.Parse(srFile.ReadLine());
                    synthGenerator.Waves[i].HarmonicDecayOdd = float.Parse(srFile.ReadLine());
                    synthGenerator.Waves[i].HarmonicDecayEven = float.Parse(srFile.ReadLine());
                    synthGenerator.Waves[i].InHarmonicSpread = float.Parse(srFile.ReadLine());
                    synthGenerator.Waves[i].InHarmonicDecay = float.Parse(srFile.ReadLine());
                    synthGenerator.Waves[i].InHarmonics = int.Parse(srFile.ReadLine());
                    synthGenerator.Waves[i].Number = int.Parse(srFile.ReadLine());
                    synthGenerator.Waves[i].NumSamples = int.Parse(srFile.ReadLine());
                    synthGenerator.Waves[i].StartPosition = int.Parse(srFile.ReadLine());
                    synthGenerator.Waves[i].WaveFile = srFile.ReadLine();
                    synthGenerator.Waves[i].WaveForm = srFile.ReadLine();
                    synthGenerator.Waves[i].Weight = int.Parse(srFile.ReadLine());
                }
            }
        }
    }
}
