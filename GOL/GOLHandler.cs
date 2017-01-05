using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace GOL
{
    class GOLHandler
    {
        private Cell[,] ActualGeneration = new Cell[80, 60];
        private List<Cell> NextGeneration = new List<Cell>();

        DispatcherTimer timer = new DispatcherTimer();
  
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

        //Checks the surrounding cells of a single cell
        public int CheckLivingCells(Cell cell, int x, int y)
        {
            int count = 0;
            //right
            if (x != cell.X - 1)
                if (ActualGeneration[x + 1, y].IsAlive)
                    count++;
            //bottom right
            if (x != cell.X - 1 && y != cell.Y - 1)
                if (ActualGeneration[x + 1, y + 1].IsAlive)
                    count++;
            //bottom
            if (y != cell.Y - 1)
                if (ActualGeneration[x, y + 1].IsAlive)
                    count++;
            //bottom left
            if (x != 0 && y != cell.Y - 1)
                if (ActualGeneration[x - 1, y + 1].IsAlive)
                    count++;
            //left
            if (x != 0)
                if (ActualGeneration[x -1, y].IsAlive)
                    count++;
            //top left
            if (x != 0 && y != 0)
                if (ActualGeneration[x - 1, y - 1].IsAlive)
                    count++;
            //top
            if (x != 0)
                if (ActualGeneration[x, y - y].IsAlive)
                    count++;
            //top right
            if (x != cell.X - 1 && y != 0)
                if (ActualGeneration[x + 1, y - 1].IsAlive)
                    count++;

                    return count;
        }

        public void TimerToCheckState()
        {
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(500); //half a second? Too fast?
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
           
        }
    }
}
