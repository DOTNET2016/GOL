using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOL
{
    class Grid
    {
        private List<Cell> cellList = new List<Cell>();

        public void AddCell(Cell cell)
        {
            cellList.Add(cell);
        }
            

    }
}
