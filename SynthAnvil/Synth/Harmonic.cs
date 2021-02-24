using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthAnvil.Synth
{
    class Harmonic
    {
        int number;         // 1 .. N
        double volume;      // 0 .. 1
        double phase;       // 0 .. 2PI

        public int Number { get => number; set => number = value; }
        public double Volume { get => volume; set => volume = value; }
        public double Phase { get => phase; set => phase = value; }

        public Harmonic(int number, double volume)
        {
            this.number = number;
            this.volume = volume;
        }
    }
}
