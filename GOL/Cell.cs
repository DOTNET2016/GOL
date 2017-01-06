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
        //Properties
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsAlive { get; set; }

        //constructor.
        public Cell(int x,int y,bool isAlive = false)
        {
            IsAlive = isAlive;
            X = x;
            Y = y;
        }
    }
}

//CREATE TABLE Generation
//(
//    Gen_Name varchar(20) NOT NULL,
//	--Gen_id int IDENTITY(1,1) PRIMARY KEY,
//   Index_X int NOT NULL,
//	Index_Y int NOT NULL,
//	PRIMARY KEY(Index_X, Index_Y)
	
//)
