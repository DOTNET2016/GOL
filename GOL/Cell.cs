using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace GOL
{
    public class Cell 
    {
        //Properties
        public int X { get; set; }
        public int Y { get; set; }
        public int GenNumber { get; set; }
        public bool IsAlive { get; set; }

        //constructor.
        public Cell(int x,int y,bool isAlive = false, int genNumber = 0)
        {
            GenNumber = genNumber;
            IsAlive = isAlive;
            X = x;
            Y = y;
        }
    }
}
