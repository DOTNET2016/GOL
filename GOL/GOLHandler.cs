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
        private Player activePlayer;
        private SavedGames savedGame;
        int generationNumber = 0;


        public GOLHandler()
        {
           
        }

        public void setupActualGeneration(Cell cell)
        {
            ActualGeneration.SetValue(cell, cell.X, cell.Y);
        }

        public void CalculateNextGeneration()
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

        public Cell[,] GetNextGeneration()
        {
            return NextGeneration;
        }

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

        public List<Cell> GetNextGenerationLoadedFromDB()
        {
            

            int MaxGen = (from gen in AliveCells
                                       select gen.GenNumber).Distinct().Count();
            

            List<Cell> GenerationToReturn = new List<Cell>();

            foreach (var gen in AliveCells)
            {
                if(gen.GenNumber == generationNumber)
                {
                    GenerationToReturn.Add(gen);
                }
            }
            generationNumber++;

            if (generationNumber == MaxGen)
            {
                generationNumber = 0;
            }
            
            return GenerationToReturn;
        }

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

        public void setSavedGameId(int savedGameId)
        {
            savedGame = new SavedGames();
            savedGame.id = savedGameId;
            savedGame.Player_id = activePlayer.id;
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

        public void ResetGenNumber()
        {
            generationNumber = 0;
        }

        private void newSavedGame()
        {
            savedGame = new SavedGames();
            savedGame.Player_id = activePlayer.id;
        }

        public void AddGeneration(Cell generationToSave, int _genNumber)
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

        public void LoadGenFromDatabase()
        {
            AliveCells.Clear();
            using (GContext db = new GContext())
            {
                var currentGen = db.Generation.Where(x => x.SavedGames.Player_id == activePlayer.id).Where(y=>y.SavedGame_id == savedGame.id);

                foreach (var gen in currentGen)
                {
                        AliveCells.Add(new Cell(gen.Cell_X, gen.Cell_Y, true, gen.GenNumber));
                }
            }
        }      
    }
}