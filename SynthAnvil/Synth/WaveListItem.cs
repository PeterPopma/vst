using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthAnvil.Synth
{
    class WaveListItem
    {
        string name;
        string displayText;

        public string Name { get => name; set => name = value; }
        public string DisplayText { get => displayText; set => displayText = value; }

        public WaveListItem(string name, string displayText)
        {
            this.name = name;
            this.displayText = displayText;
        }
    }
}
