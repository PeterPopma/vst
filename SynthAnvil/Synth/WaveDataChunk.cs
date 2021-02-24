using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthAnvil.Synth
{
    class WaveDataChunk
    {
        public string sChunkID;     // "data"
        public uint dwChunkSize;    // Length of header in bytes
        public int[] audioData;  // 16-bit/32-bit audio data

        /// <summary>
        /// Initializes a new data chunk with default values.
        /// </summary>
        public WaveDataChunk()
        {
            audioData = new int[0];
            dwChunkSize = 0;
            sChunkID = "data";
        }
    }
}
