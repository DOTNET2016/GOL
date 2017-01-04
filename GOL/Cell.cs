using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace GOL
{
    class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }

        public bool IsAlive { get; set; }

        //private int[,] _generation;
        //private int[,] _lastGeneration;
        //private int _generationCount;

        //public int GenerationCount { get { return _generationCount; } }

        public Cell(int x,int y)
        {
            IsAlive = false;
            X = x;
            Y = y;
        }
    }
}
