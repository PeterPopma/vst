using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthAnvil.Synth
{
    class FrequencyAmount
    {
        double frequency;
        double amount;

        public FrequencyAmount(double frequency, double amount)
        {
            this.Frequency = frequency;
            this.Amount = amount;
        }

        public double Frequency { get => frequency; set => frequency = value; }
        public double Amount { get => amount; set => amount = value; }
    }
}
