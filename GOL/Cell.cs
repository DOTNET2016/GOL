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
        public double Width { get; set; }
        public double Height { get; set; }

        public bool IsAlive { get; set; }

        //private int[,] _generation;
        //private int[,] _lastGeneration;
        //private int _generationCount;

        public Point Coords { get; set; }

        //public int GenerationCount { get { return _generationCount; } }

        public Cell(Point coords)
        {
            Coords = coords;

            Width = coords.X;
            Height = coords.Y;

            IsAlive = false;

        }
    }
}
