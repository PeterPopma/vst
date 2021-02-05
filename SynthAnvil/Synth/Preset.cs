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
                name = "default";
//                throw new Exception("preset name not set!");
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
                    file.WriteLine(waveInfo.Name);
                    file.WriteLine(waveInfo.MinFrequency);
                    file.WriteLine(waveInfo.MinVolume);
                    file.WriteLine(waveInfo.Channel);
                    file.WriteLine(waveInfo.MaxFrequency);
                    file.WriteLine(waveInfo.MaxVolume);
                    file.WriteLine(waveInfo.NumSamples());
                    file.WriteLine(waveInfo.StartPosition);
                    file.WriteLine(waveInfo.WaveFile);
                    file.WriteLine(waveInfo.WaveForm);
                    file.WriteLine(waveInfo.Weight);
                    file.WriteLine(waveInfo.ShapeWave.Length);
                    for (int j = 0; j < waveInfo.ShapeWave.Length; j++)
                    {
                        file.WriteLine(waveInfo.ShapeWave[j]);
                    }
                    file.WriteLine(waveInfo.ShapeFrequency.Length);
                    for (int j = 0; j < waveInfo.ShapeFrequency.Length; j++)
                    {
                        file.WriteLine(waveInfo.ShapeFrequency[j]);
                    }
                    file.WriteLine(waveInfo.ShapeVolume.Length);
                    for (int j = 0; j < waveInfo.ShapeVolume.Length; j++)
                    {
                        file.WriteLine(waveInfo.ShapeVolume[j]);
                    }
                    file.WriteLine(waveInfo.WaveFileData.Length);
                    for (int j = 0; j < waveInfo.WaveFileData.Length; j++)
                    {
                        file.WriteLine(waveInfo.WaveFileData[j]);
                    }
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
                    newWave.Name = srFile.ReadLine();
                    newWave.MinFrequency = double.Parse(srFile.ReadLine());
                    newWave.MinVolume = int.Parse(srFile.ReadLine());
                    newWave.Channel = int.Parse(srFile.ReadLine());
                    newWave.MaxFrequency = double.Parse(srFile.ReadLine());
                    newWave.MaxVolume = int.Parse(srFile.ReadLine());
                    newWave.WaveData = new double[int.Parse(srFile.ReadLine()) * 2];
                    newWave.StartPosition = int.Parse(srFile.ReadLine());
                    newWave.WaveFile = srFile.ReadLine();
                    newWave.WaveForm = srFile.ReadLine();
                    newWave.Weight = int.Parse(srFile.ReadLine());
                    int length = int.Parse(srFile.ReadLine());
                    newWave.ShapeWave = new int[length];
                    for (int j = 0; j < length; j++)
                    {
                        newWave.ShapeWave[j] = int.Parse(srFile.ReadLine());
                    }
                    length = int.Parse(srFile.ReadLine());
                    newWave.ShapeFrequency = new int[length];
                    for (int j = 0; j < length; j++)
                    {
                        newWave.ShapeFrequency[j] = int.Parse(srFile.ReadLine());
                    }
                    length = int.Parse(srFile.ReadLine());
                    newWave.ShapeVolume = new int[length];
                    for (int j = 0; j < length; j++)
                    {
                        newWave.ShapeVolume[j] = int.Parse(srFile.ReadLine());
                    }
                    length = int.Parse(srFile.ReadLine());
                    newWave.WaveFileData = new int[length];
                    for (int j = 0; j < length; j++)
                    {
                        newWave.WaveFileData[j] = int.Parse(srFile.ReadLine());
                    }

                    synthGenerator.Waves.Add(newWave);
                }

                synthGenerator.CurrentWave = synthGenerator.Waves.Last();
            }
        }

        public void Delete(string name)
        {
            File.Delete((@".\presets\" + name + ".pst"));
        }
    }
}
