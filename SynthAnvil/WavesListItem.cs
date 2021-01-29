using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthAnvil
{
    class WavesListItem
    {
        int number;
        string name;

        public int Number { get => number; set => number = value; }
        public string Name { get => name; set => name = value; }

        public WavesListItem(int number, string name)
        {
            this.number = number;
            this.name = name;
        }

        public override bool Equals(Object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            WavesListItem other = (WavesListItem)obj;
            return (Number == other.Number);
        }

        public override int GetHashCode()
        {
            int hashCode = -1156126842;
            hashCode = hashCode * -1521134295 + number.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name);
            return hashCode;
        }
    }
}
