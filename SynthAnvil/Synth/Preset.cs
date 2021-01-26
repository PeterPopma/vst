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
                synthGenerator.Waves.Clear();
                for (int i = 0; i < numWaves; i++)
                {
                    WaveInfo newWave = new WaveInfo();
                    newWave.Number = i;
                    newWave.BeginFrequency = float.Parse(srFile.ReadLine());
                    newWave.BeginVolume = int.Parse(srFile.ReadLine());
                    newWave.BeginEndBeginFrequencyEnabled = bool.Parse(srFile.ReadLine());
                    newWave.BeginEndBeginVolumeEnabled = bool.Parse(srFile.ReadLine());
                    newWave.Channel = int.Parse(srFile.ReadLine());
                    newWave.EndFrequency = float.Parse(srFile.ReadLine());
                    newWave.EndFrequencyEnabled = bool.Parse(srFile.ReadLine());
                    newWave.EndVolume = int.Parse(srFile.ReadLine());
                    newWave.EndVolumeEnabled = bool.Parse(srFile.ReadLine());
                    newWave.NumSamples = int.Parse(srFile.ReadLine());
                    newWave.StartPosition = int.Parse(srFile.ReadLine());
                    newWave.WaveFile = srFile.ReadLine();
                    newWave.WaveForm = srFile.ReadLine();
                    newWave.Weight = int.Parse(srFile.ReadLine());
                    newWave.SetName();
                    synthGenerator.Waves.Add(newWave);
                }
                synthGenerator.CurrentWave = synthGenerator.Waves.Last();
            }
        }
    }
}
