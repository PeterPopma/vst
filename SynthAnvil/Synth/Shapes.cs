using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthAnvil.Synth
{
    class Shapes
    {
        public static void IncreasingLineair(int[] waveData)
        {
            for (int i = 0; i < waveData.Length; i++)
            {
                waveData[i] = (int)(i / (double)SynthGenerator.SHAPE_NUMPOINTS * SynthGenerator.SHAPE_MAX_VALUE);
            }
        }

        public static void DecreasingLineair(int[] waveData)
        {
            for (int i = 0; i < waveData.Length; i++)
            {
                waveData[i] = (int)((SynthGenerator.SHAPE_NUMPOINTS - i) / (double)SynthGenerator.SHAPE_NUMPOINTS * SynthGenerator.SHAPE_MAX_VALUE);
            }
        }

        public static void IncreasingLogarithmic(int[] waveData)
        {
            double factor = Math.Pow(SynthGenerator.SHAPE_MAX_VALUE, 1.0 / SynthGenerator.SHAPE_NUMPOINTS);
            for (int i = 0; i < waveData.Length; i++)
            {
                waveData[i] = SynthGenerator.SHAPE_MAX_VALUE - (int)Math.Pow(factor, SynthGenerator.SHAPE_NUMPOINTS - i);
            }
        }

        public static void DecreasingLogarithmic(int[] waveData)
        {
            double factor = Math.Pow(SynthGenerator.SHAPE_MAX_VALUE, 1.0 / SynthGenerator.SHAPE_NUMPOINTS);
            for (int i = 0; i < waveData.Length; i++)
            {
                waveData[i] = SynthGenerator.SHAPE_MAX_VALUE - (int)Math.Pow(factor, i);
            }
        }

        public static void Sines(int[] waveData, int numSines = 1)
        {
            for (int i = 0; i < waveData.Length; i++)
            {
                waveData[i] = (int)(((int)(Math.Sin(i / (double)waveData.Length * 2 * numSines * Math.PI) * SynthGenerator.SHAPE_MAX_VALUE + SynthGenerator.SHAPE_MAX_VALUE)) / 2.0);
            }
        }

        public static void Flat(int[] waveData)
        {
            for (int i = 0; i < waveData.Length; i++)
            {
                waveData[i] = (int)(SynthGenerator.SHAPE_MAX_VALUE / 2.0);
            }
        }

        public static void RandomWaves(int[] waveData)
        {
            Random random = new Random();

            int amplitude = random.Next(SynthGenerator.SHAPE_MAX_VALUE / 2);
            int period = random.Next(50) + 5;
            bool amplitude_increasing = false;
            for (int i = 0; i < waveData.Length; i++)
            {
                if (random.Next(20) == 3)
                {
                    amplitude_increasing = !amplitude_increasing;
                }
                if (amplitude_increasing)
                {
                    if (amplitude < SynthGenerator.SHAPE_MAX_VALUE / 2)
                    {
                        amplitude++;
                    }
                    else
                    {
                        amplitude_increasing = false;
                    }
                }
                else
                {
                    if (amplitude > 0)
                    {
                        amplitude--;
                    }
                    else
                    {
                        amplitude_increasing = true;
                    }
                }
                waveData[i] = (int)((Math.Sin(i / (double)waveData.Length * period * Math.PI) * amplitude) + (SynthGenerator.SHAPE_MAX_VALUE / 2.0));
            }
        }

        public static void IncDec(int[] waveData)
        {
            for (int i = 0; i < waveData.Length / 2; i++)
            {
                waveData[i] = (int)(((SynthGenerator.SHAPE_NUMPOINTS - i * 2) / (double)SynthGenerator.SHAPE_NUMPOINTS) * SynthGenerator.SHAPE_MAX_VALUE);
            }
            for (int i = waveData.Length / 2; i < waveData.Length; i++)
            {
                waveData[i] = (int)(((i - waveData.Length / 2) * SynthGenerator.SHAPE_MAX_VALUE) / (waveData.Length / 2));
            }
        }

        public static void DecInc(int[] waveData)
        {
            for (int i = 0; i < waveData.Length / 2; i++)
            {
                waveData[i] = (int)(((i) * SynthGenerator.SHAPE_MAX_VALUE) / (waveData.Length / 2));
            }
            for (int i = waveData.Length / 2; i < waveData.Length; i++)
            {
                waveData[i] = (int)(((SynthGenerator.SHAPE_NUMPOINTS - (i - waveData.Length / 2) * 2) / (double)SynthGenerator.SHAPE_NUMPOINTS) * SynthGenerator.SHAPE_MAX_VALUE);
            }
        }

        public static void Spikes(int[] waveData)
        {
            Random random = new Random();

            int x_position = 0;
            while (x_position < waveData.Length)
            {
                if (x_position < SynthGenerator.SHAPE_NUMPOINTS - 5 && random.Next(15) == 11)
                {
                    int amplitude = random.Next(SynthGenerator.SHAPE_MAX_VALUE / 2);
                    int up_down_spike = 1;
                    if (random.Next(100) < 50)
                    {
                        // up spike
                        up_down_spike = -1;
                    }
                    for (int j = 0; j < 5; j++)
                    {
                        waveData[x_position] = SynthGenerator.SHAPE_MAX_VALUE / 2 - ((int)(amplitude * Math.Sin(((j + 1) / 6.0) * Math.PI)) * up_down_spike);
                        x_position++;
                    }
                }
                else
                {
                    waveData[x_position] = SynthGenerator.SHAPE_MAX_VALUE / 2;
                    x_position++;
                }
            }
        }

        public static void IncSines(int[] waveData, int numSines = 1)
        {
            for (int i = 0; i < waveData.Length; i++)
            {
                double factor = Math.Pow(1 + numSines*0.001, i);   // between 1 and 20
                waveData[i] = (int)(((int)(Math.Sin(i / (double)waveData.Length * factor * Math.PI) * SynthGenerator.SHAPE_MAX_VALUE + SynthGenerator.SHAPE_MAX_VALUE)) / 2.0);
            }
        }

        public static void DecSines(int[] waveData, int numSines = 1)
        {
            for (int i = 0; i < waveData.Length; i++)
            {
                double factor = Math.Pow(1+numSines * 0.001, i);   // between 1 and 20
                waveData[waveData.Length - 1 - i] = (int)(((int)(Math.Sin(i / (double)waveData.Length * factor * Math.PI) * SynthGenerator.SHAPE_MAX_VALUE + SynthGenerator.SHAPE_MAX_VALUE)) / 2.0);
            }
        }
    }
}
