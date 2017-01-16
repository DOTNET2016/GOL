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
        #region Fields
        private Cell[,] ActualGeneration;
        private Cell[,] NextGeneration;
        private List<Cell> AliveCells;
        private Player activePlayer;
        private SavedGames savedGame;
        int generationNumber = 0;
        int currentAliveCells = 0;
        #endregion
        public GOLHandler()
        {
            ActualGeneration = new Cell[80, 60];
            NextGeneration = new Cell[80, 60];
            AliveCells = new List<Cell>();
        }

        /// <summary>
        /// Method that returns the actualgeneration.
        /// </summary>
        /// <returns></returns>
       public Cell[,] GetActualGeneration()
        {
            return ActualGeneration;
        }

        /// <summary>
        /// After the nextgeneration is calculated and printed you can transfer the cells to the actualgeneration with this method.
        /// </summary>
        /// <param name="cell"></param>
        public void setupActualGeneration(Cell cell)
        {
            ActualGeneration.SetValue(cell, cell.X, cell.Y);
        }

        /// <summary>
        /// Calculates and setting the nextgeneration everytime this method runs.
        /// </summary>
        private void CalculateNextGeneration()
        {
            for (int i = 0; i < ActualGeneration.GetLength(0); i++)
            {
                for (int j = 0; j < ActualGeneration.GetLength(1); j++)
                {
                    //Method for count how many neighboor every cell has and then throw it into the switch.
                    int neighbours = CheckLivingNeighboors(i, j);

                    #region SwitchOnAllTheCells
                    switch (neighbours)
                    {
                        case 0:
                            {
                                //If it has 0 neighbour add a dead cell to nextgeneration.
                                NextGeneration.SetValue(new Cell(i, j), i, j);
                                break;
                            }
                        case 1:
                            {
                                //If it has 1 neighbour add a dead cell in the nextgeneration.
                                NextGeneration.SetValue(new Cell(i, j), i, j);
                                break;
                            }
                        case 2:
                            {
                                //If the cell is alive and has 2 neighbour add the cell as alive in the nextgeneration.
                                if (ActualGeneration[i, j].IsAlive)
                                {
                                    NextGeneration.SetValue(new Cell(i, j), i, j);
                                    NextGeneration[i, j].IsAlive = true;
                                    break;
                                }
                                //If it's dead and has 2 neighbour add the cell as dead in the nextgeneration.
                                else
                                {
                                    NextGeneration.SetValue(new Cell(i, j), i, j);
                                }
                                break;
                            }
                        case 3:
                            {
                                //If it has 3 neighbour we add the cell as alive in the nextgeneration.
                                NextGeneration.SetValue(new Cell(i, j), i, j);
                                NextGeneration[i, j].IsAlive = true;
                                break;
                            }
                        default:
                            {
                                //If it has more then 3 neighbour add the cell as dead in the nextgeneration.
                                NextGeneration.SetValue(new Cell(i, j), i, j);
                                break;
                            }
                            #endregion
                    }
                }
            }
        }

        /// <summary>
        /// Method that's checking how many neighbour the cell have.
        /// </summary>
        /// <param name="x">The X coord of the cell.</param>
        /// <param name="y">The Y coord of the cell.</param>
        /// <returns></returns>
        private int CheckLivingNeighboors(int x, int y)
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

        public dynamic UpdateLabels()
        {
            dynamic temp = new System.Dynamic.ExpandoObject();
            temp.x1 = generationNumber;
            temp.x2 = currentAliveCells;
            return temp;
        }

        /// <summary>
        /// Method that returns the nextgeneration. 
        /// </summary>
        /// <returns></returns>
        public Cell[,] GetNextGeneration()
        {
            CalculateNextGeneration();
            return NextGeneration;
        }

        /// <summary>
        /// Method that checking if the cells is alive or dead in the actualgeneration. It's only for changing when you click our the cells in the canvas.
        /// If you click one cell as alive and then click it again it's change to dead again with this method.
        /// </summary>
        /// <param name="X">The X coord of the cell.</param>
        /// <param name="Y">The Y coord of the cell.</param>
        /// <returns>It returns false if cell in the actualgeneration is true and then change it to false. Else it change the cell to alive and returns true. </returns>
        public bool ClickKillOrMakeCell(int X, int Y)
        {
            
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

        /// <summary>
        /// Method that returns one generation from the savedgame that you have choosed before.
        /// </summary>
        /// <returns></returns>
        public List<Cell> GetNextGenerationLoadedFromDB()
        {
            //Looking what is the highest generation in this savedgame.
            int MaxGen = (from gen in AliveCells
                          select gen.GenNumber).Distinct().Count();


            List<Cell> GenerationToReturn = new List<Cell>();
            currentAliveCells = 0;
            foreach (var gen in AliveCells)
            {
                if (gen.GenNumber == generationNumber)
                {
                    GenerationToReturn.Add(gen);
                    currentAliveCells++;
                }
            }
            generationNumber++;

            //if the generation is same as the maxGen it set the generationNumber to 0 and start all over again.
            if (generationNumber == MaxGen)
            {
                generationNumber = 0;
            }

            return GenerationToReturn;
        }

        /// <summary>
        /// Method that checking if you have to change the cell or not in the nextgeneration. 
        /// </summary>
        /// <param name="_X">The X coord of the cell.</param>
        /// <param name="_Y">The Y coord of the cell.</param>
        /// <param name="AliveOrNot">true if alive</param>
        /// <returns>Returns true if the Cell already has the correct condition. else false and then you have to change it in the canvas</returns>
        public bool CheckIfHaveToChange(int _X, int _Y, bool AliveOrNot)
        {
            if (ActualGeneration[_X, _Y].IsAlive == AliveOrNot)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        /// <summary>
        /// Method for setting the SavedGameId and add it to the ActivePlayerId.
        /// </summary>
        /// <param name="savedGameId"></param>
        public void setSavedGameId(int savedGameId)
        {
            savedGame = new SavedGames();
            savedGame.id = savedGameId;
            savedGame.Player_id = activePlayer.id;
        }

        /// <summary>
        /// Method for setup a new player. 
        /// </summary>
        /// <param name="playerId">The playerId if it has one already.</param>
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

        //Method for reset the GenerationNumber.
        public void ResetGenNumber()
        {
            generationNumber = 0;

        }

        //Method for a new savedgame.
        private void newSavedGame()
        {
            savedGame = new SavedGames();
            savedGame.Player_id = activePlayer.id;
        }

        /// <summary>
        /// Method for adding the cells that are alive and you want to save to the database when you runt SaveToDatabase Method.
        /// </summary>
        /// <param name="generationToSave">The cell.</param>
        /// <param name="_genNumber">The generationNumber to add with the cell.</param>
        public void AddGeneration(Cell generationToSave, int _genNumber)
        {
            AliveCells.Add(new Cell(generationToSave.X, generationToSave.Y, true, _genNumber));
        }

        /// <summary>
        /// Method for saving all the generations to the Database.
        /// </summary>
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
        /// Method for delete the selected SavedGame.
        /// </summary>
        /// <param name="savedGame"></param>
        public void DeleteSavedGame(int savedGame)
        {
            using (var context = new GContext())
            {
                var savedGameToDelete = context.SavedGames.FirstOrDefault(i => i.id == savedGame);
                context.Generation.RemoveRange(from g in context.Generation
                                               where g.SavedGame_id == savedGame
                                               select g);
                context.SavedGames.Remove(savedGameToDelete);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Method for load all the generations belonging to the Activeplayer and the choosed savedGameId.
        /// </summary>
        public void LoadGenFromDatabase()
        {
            AliveCells.Clear();
            using (GContext db = new GContext())
            {
                var currentGen = db.Generation.Where(x => x.SavedGames.Player_id == activePlayer.id).Where(y => y.SavedGame_id == savedGame.id);

                foreach (var gen in currentGen)
                {
                    AliveCells.Add(new Cell(gen.Cell_X, gen.Cell_Y, true, gen.GenNumber));
                }
            }
        }
    }
}