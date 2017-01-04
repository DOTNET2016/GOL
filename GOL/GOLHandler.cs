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
        private List<Cell> cellList = new List<Cell>();

        public void AddCell(Cell cell)
        {
            cellList.Add(cell);
        }
        public void ChoosenCell(Point cellCoordXY)
        {
            var cellClicked = from c in cellList
                                 where c.X >= cellCoordXY.X - 10 && c.X <= cellCoordXY.X
                                 where c.Y >= cellCoordXY.Y - 10 && c.Y <= cellCoordXY.Y
                                 select c;

            var cellToChange = cellClicked.FirstOrDefault();

            if (cellToChange.IsAlive == true)
            {
                cellToChange.IsAlive = false;
            }
            else
            {
                cellToChange.IsAlive = true;
            }
        }

        public List<Cell> GetCellList()
        {
            return cellList;
        }
    }
}
