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

        public static void Flat(int[] waveData)
        {
            for (int i = 0; i < waveData.Length; i++)
            {
                waveData[i] = (int)(SynthGenerator.SHAPE_MAX_VALUE / 2.0);
            }
        }
    }
}
