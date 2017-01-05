using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GOL
{
    class GOLHandler
    {
        private Cell[,] ActualGeneration = new Cell[80, 60];
        private List<Cell> NextGeneration = new List<Cell>();
        public void AddCell(Cell cell)
        {
            ActualGeneration.SetValue(cell, cell.X, cell.Y);
        }
        public void KillOrMakeCell(int X_index,int Y_index)
        {

            if (ActualGeneration[X_index, Y_index].IsAlive == true)
            {
                ActualGeneration[X_index, Y_index].IsAlive = false;
            }
            else
            {
                ActualGeneration[X_index, Y_index].IsAlive = true;
            }
        }

        public void calculateNextGeneration()
        {

        }

        public Cell[,] GetCellArray()
        {
            return ActualGeneration;
        }
    }
}
