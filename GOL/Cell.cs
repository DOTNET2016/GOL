using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOL
{
    class Cell
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public bool IsAlive { get; set; }

        private int[,] _generation;
        private int[,] _lastGeneration;
        private int _generationCount;

        public int GenerationCount { get { return _generationCount; } }

    }
}
