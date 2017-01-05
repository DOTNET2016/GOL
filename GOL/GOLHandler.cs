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
        private Cell[,] NextGeneration = new Cell[80, 60];
        
        public void AddCell(Cell cell)
        {
            ActualGeneration.SetValue(cell, cell.X, cell.Y);
        }
        public void KillOrMakeCell(int X_index, int Y_index)
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
            for (int i = 0; i < ActualGeneration.GetLength(0); i++)
            {
                for (int j = 0; j < ActualGeneration.GetLength(1); j++)
                {
                    switch (CheckLivingCells(i,j))
                    {
                        case 0:
                            {

                                NextGeneration[i,j] = ActualGeneration[i, j];
                                NextGeneration[i, j].IsAlive = false;
                                break;
                            }
                        case 1:
                            {
                                NextGeneration[i, j] = ActualGeneration[i, j];
                                NextGeneration[i, j].IsAlive = false;
                                break;
                            }
                        case 2:
                            {
                                if (ActualGeneration[i, j].IsAlive == true)
                                {
                                    NextGeneration[i, j] = ActualGeneration[i, j];
                                    NextGeneration[i, j].IsAlive = true;
                                }
                                break;
                            }
                        case 3:
                            {
                                if(ActualGeneration[i,j].IsAlive == true)
                                {
                                    NextGeneration[i, j] = ActualGeneration[i, j];
                                    NextGeneration[i, j].IsAlive = true;
                                }
                                else
                                {
                                    NextGeneration[i, j] = ActualGeneration[i, j];
                                    NextGeneration[i, j].IsAlive = true;
                                }
                                break;
                            }
                        default:
                            {
                                NextGeneration[i, j] = ActualGeneration[i, j];
                                NextGeneration[i, j].IsAlive = false;
                                break;
                            }
                    }
                }
            }
        }

        //private int CountConnectingCells(Point ArrayindexXY)
        //{
        //    int ConnectedCells = 0;

        //    var cell = ActualGeneration[(int)ArrayindexXY.X, (int)ArrayindexXY.Y];
        //    var temp = ActualGeneration;

        //    // Check cell on the right.
        //    if (cell.X != temp.GetLength(0) - 1)
        //        if (temp[cell.X + 1, cell.Y].IsAlive)
        //            ConnectedCells++;

        //    if (cell.X != temp.GetLength(0) - 1 && cell.Y != temp.GetLength(1) - 1)
        //        if (temp[cell.X + 1, cell.Y + 1].IsAlive)
        //            ConnectedCells++;

        //    if (cell.Y != temp.GetLength(1) - 1)
        //        if (temp[cell.X, cell.Y + 1].IsAlive)
        //            ConnectedCells++;

        //    if (cell.X != 0 && cell.Y != temp.GetLength(1) - 1)
        //        if (temp[cell.X - 1, cell.Y + 1].IsAlive)
        //            ConnectedCells++;

        //    if (cell.X != 0)
        //        if (temp[cell.X - 1, cell.Y].IsAlive)
        //            ConnectedCells++;

        //    if (cell.X != 0 && cell.Y != 0)
        //        if (temp[cell.X - 1, cell.Y - 1].IsAlive)
        //            ConnectedCells++;

        //    if (cell.Y != 0)
        //        if (temp[cell.X, cell.Y - 1].IsAlive)
        //            ConnectedCells++;

        //    if (cell.X != temp.GetLength(0) - 1 && cell.Y != 0)
        //        if (temp[cell.X + 1, cell.Y - 1].IsAlive)
        //            ConnectedCells++;

        //    return ConnectedCells;
        //}

        public Cell[,] GetActualGeneration()
        {
            return ActualGeneration;
        }

        //Checks the surrounding cells of a single cell
        public int CheckLivingCells(int x, int y)
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

        public Cell[,] GetNextGeneration()
        {
            return NextGeneration;
        }

        //public void TimerToCheckState()
        //{
        //    timer.Tick += Timer_Tick;
        //    timer.Interval = new TimeSpan(500); //half a second? Too fast?
        //    timer.Start();
        //}

        //private void Timer_Tick(object sender, EventArgs e)
        //{

        //}
    }
}
