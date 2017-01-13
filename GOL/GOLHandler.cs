using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace GOL
{
    public class GOLHandler
    {
        //Fields
        private Cell[,] ActualGeneration = new Cell[80, 60];
        private Cell[,] NextGeneration = new Cell[80, 60];
        private List<Cell> AliveCells = new List<Cell>();
        DispatcherTimer timer;
        private Player activePlayer;
        private SavedGames savedGame;


        //Event
        public event EventHandler Timer_Ticked;

        //Constructor
        public GOLHandler()
        {
            int value = 300;
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(value);
            timer.IsEnabled = true;
            timer.Stop();
            timer.Tick += Timer_Tick;
        }

        //Tick handler that raises the timer_ticked event if it has subscribers.
        private void Timer_Tick(object sender, EventArgs e)
        {
            //Check so the event have a subscriber.
            if (Timer_Ticked != null)
            {
                Timer_Ticked.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Method for adding new cells to the ActualGeneration.
        /// </summary>
        /// <param name="cell">Send the cell you want to add.</param>
        public void setupActualGeneration(Cell cell)
        {
            ActualGeneration.SetValue(cell, cell.X, cell.Y);
        }

        /// <summary>
        /// Send the coordinates from the position you click in the canvas and the radius of the square the cells is in.
        /// This method will divide the coordinates by 10 and rounds it to nearest even 10.
        /// </summary>
        /// <param name="X_index">send The X coordinate.</param>
        /// <param name="Y_index">Send The Y coordinate</param>
        /// <param name="RadiusOfTheSquare">Send the Radius of the Square</param>
        public bool ClickKillOrMakeCell(int X, int Y)
        {
            //Casting the values to an int.


            //Kill or make the cell alive.
            if (ActualGeneration[X, Y].IsAlive == true)
            {
                ActualGeneration[X, Y].IsAlive = false;
                return false;
            }
            else
            {
                ActualGeneration[X, Y].IsAlive = true;
                return true;
            }
        }

        public bool CheckIfHaveToChange(int _X,int _Y,bool AliveOrNot)
        {
            if (ActualGeneration[_X,_Y].IsAlive == AliveOrNot)
            {
                return true;
            }
            
            else
            {
                return false;
            }
        }

        public void SetupPlayer(int playerId)
        {
            activePlayer = new Player();
            using (GContext g = new GContext())
            {

                var players = g.Player;
                foreach (var player in players)
                {
                    if (player.id == playerId)
                    {
                        activePlayer = player;
                        newSavedGame();
                    }
                }
            }
        }

        private void newSavedGame()
        {
            savedGame = new SavedGames();
            savedGame.Player_id = activePlayer.id;
        }

        public void addGeneration(Cell generationToSave,int _genNumber)
        {
            AliveCells.Add(new Cell(generationToSave.X, generationToSave.Y, true, _genNumber));
        }

        public void ClearCellsAlive()
        {
            AliveCells.Clear();
        }

        public void SaveToDatabase()
        {
            using (GContext db = new GContext())
            {
                db.SavedGames.Add(savedGame);

                foreach (var item in AliveCells)
                {
                    Generation g = new Generation();
                    g.GenNumber = item.GenNumber;
                    g.SavedGame_id = savedGame.id;
                    g.Cell_X = item.X;
                    g.Cell_Y = item.Y;
                    db.Generation.Add(g);

                }
                db.SaveChanges();
                AliveCells.Clear();
            }
        }

        /// <summary>
        /// Method that looping through the ActualGenerations MultiDimensionalArray,
        /// And Checks how many neighboor every cell has and then make the new generation.
        /// </summary>
        public void calculateNextGeneration()
        {
            for (int i = 0; i < ActualGeneration.GetLength(0); i++)
            {
                for (int j = 0; j < ActualGeneration.GetLength(1); j++)
                {
                    //Method for count how many neighboor every cell has and then throw it into the switch.
                    int neighboors = CheckLivingNeighboors(i, j);

                    #region SwitchOnAllTheCells
                    switch (neighboors)
                    {
                        case 0:
                            {
                                NextGeneration.SetValue(new Cell(i, j), i, j);
                                break;
                            }
                        case 1:
                            {
                                NextGeneration.SetValue(new Cell(i, j), i, j);
                                break;
                            }
                        case 2:
                            {
                                if (ActualGeneration[i, j].IsAlive)
                                {
                                    NextGeneration.SetValue(new Cell(i, j), i, j);
                                    NextGeneration[i, j].IsAlive = true;
                                    break;
                                }
                                else
                                {
                                    NextGeneration.SetValue(new Cell(i, j), i, j);
                                }
                                break;
                            }
                        case 3:
                            {
                                NextGeneration.SetValue(new Cell(i, j), i, j);
                                NextGeneration[i, j].IsAlive = true;
                                break;
                            }
                        default:
                            {
                                NextGeneration.SetValue(new Cell(i, j), i, j);
                                break;
                            }
                            #endregion
                    }
                }
            }
        }

        /// <summary>
        /// Method that returns the actualGeneration. 
        /// </summary>
        /// <returns>Actual generation MultiDimensionalArray with the cells.</returns>
        public Cell[,] GetActualGeneration()
        {
            return ActualGeneration;
        }

        /// <summary>
        /// Start the timer, The interval is 2 seconds by default.
        /// </summary>
        public void Start_Timer()
        {
            timer.Start();
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        public void Stop_Timer()
        {
            timer.Stop();
        }

        /// <summary>
        /// Method that returns the NextGeneration MultiDimensionalArrat with the cells.
        /// </summary>
        /// <returns></returns>
        public Cell[,] GetNextGeneration()
        {
            return NextGeneration;
        }

        //Checks the surrounding Neighboor-Cells
        public int CheckLivingNeighboors(int x, int y)
        {
            //take the length of X and Y from ActualGeneration.
            int Xlength = ActualGeneration.GetLength(0);
            int Ylength = ActualGeneration.GetLength(1);

            //Start counting from zero neighboors.
            int neighbours = 0;

            #region CountingNeighboors
            //Right
            if (x < Xlength - 1)
                if (ActualGeneration[x + 1, y].IsAlive)
                    neighbours++;
            //Bottom Right
            if (x < Xlength - 1 && y < Ylength - 1)
                if (ActualGeneration[x + 1, y + 1].IsAlive)
                    neighbours++;
            //Bottom
            if (y < Ylength - 1)
                if (ActualGeneration[x, y + 1].IsAlive)
                    neighbours++;
            //Bottom Left
            if (x > 0 && y < Ylength - 1)
                if (ActualGeneration[x - 1, y + 1].IsAlive)
                    neighbours++;
            //Left
            if (x > 0)
                if (ActualGeneration[x - 1, y].IsAlive)
                    neighbours++;
            //Top Left
            if (x > 0 && y > 0)
                if (ActualGeneration[x - 1, y - 1].IsAlive)
                    neighbours++;
            //Top
            if (y > 0)
                if (ActualGeneration[x, y - 1].IsAlive)
                    neighbours++;
            //Top Right
            if (x < Xlength - 1 && y != 0)
                if (ActualGeneration[x + 1, y - 1].IsAlive)
                    neighbours++;
            #endregion

            return neighbours;
        }

        public List<Generation> LoadGenFromDatabase()
        {
            List<Generation> generationsToReturn = new List<Generation>();
            using (GContext db = new GContext())
            {
                var currentGen = db.Generation.Where(x => x.SavedGames.Player_id == activePlayer.id);

                try
                {
                    foreach (var gen in currentGen)
                    {
                        generationsToReturn.Add(gen);
                    }
                }
                catch (Exception)
                {

                    MessageBox.Show("That didn't work", "Warning!");
                }

            }
            return generationsToReturn;
        }
    }
}





//    --CREATE TABLE Generation
//--(
//--	Gen_id int IDENTITY(1,1) PRIMARY KEY,
//--	GenNumber int NOT NULL,
//--	Cell_X int NOT NULL,
//--	Cell_Y int NOT NULL,
//--	IsAlive bit DEFAULT(0) NOT NULL,
//--	SavedGame_id int FOREIGN KEY REFERENCES SavedGames(SavedGame_id)
//--)

//--CREATE TABLE SavedGames
//--(
//--	SavedGame_id int IDENTITY(1,1) PRIMARY KEY,
//--	GenNumber int NOT NULL,
//--	Player_id int FOREIGN KEY REFERENCES Player(Player_id)
//--)

//--CREATE TABLE Player
//--(
//--	Player_id int IDENTITY(1,1) PRIMARY KEY,
//--	PlayerName varchar(25),
//--	--SavedGame_id int FOREIGN KEY REFERENCES SavedGames(SavedGame_id)
//--)

//--ALTER TABLE Player
//--ADD COLUMN SavedGame_id int

//--ALTER TABLE Player
//--ADD FOREIGN KEY(SavedGame_id) REFERENCES SavedGames(SavedGame_id)
