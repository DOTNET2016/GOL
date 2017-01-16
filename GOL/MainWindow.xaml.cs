using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GOL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields
        GOLHandler handler;
        DispatcherTimer timer;
        private bool _IsOn;
        private bool _ReplayIsOn;
        private int _playerId;
        private int genNumber = 0;
        private string _playerName;
        private bool GameBoardIsPressed = false;
        int[,] gameBoard;
        #endregion

        #region Properties
        public int SavedGameId { get; set; }

        private bool TimerIsOn
        {
            get
            {
                return _IsOn;
            }
            set
            {
                _IsOn = value;
                buttonStartTimer.Content = _IsOn ? "Stop Timer" : "Start Timer";
            }
        }

        private bool ReplayOn
        {
            get
            {
                return _ReplayIsOn;
            }
            set
            {
                _ReplayIsOn = value;
                buttonReplay.Content = _ReplayIsOn ? "Stop Replay" : "Replay";
            }
        }
        #endregion
        public MainWindow()
        {
            timer = new DispatcherTimer();
            InitializeComponent();
            gameBoardCanvas.Background = Brushes.Black;
            handler = new GOLHandler();
            initializeGameBoard();
            timer.Tick += Timer_Tick;
        }

        /// <summary>
        /// Method for Set-up the gameboard(canvas). it's 800x600 pixlar/points. It setting one cell at every 10x10 coordinates. 
        /// It rounds the value to nearest 10 so we will be sure that the Coordinates will be 0x0,0x10,0x20,0x30,0x40,0x50............
        /// It will set all cells as dead by default and with the 8x8 size with the color WhiteSmoke. It's 10x10 and Black when alive.
        /// </summary>
        private void initializeGameBoard()
        {
            gameBoard = new int[800, 600];

            #region LoopThroughTheCanvas
            for (int i = 0; i < 800; i += 10)
            {
                for (int j = 0; j < 600; j += 10)
                {
                    int xPosition = 0;
                    int YPosition = 0;
                    //Algorithm for get the nearest value with 10.
                    xPosition = ((int)Math.Round(i / 10.0)) * 10;
                    YPosition = ((int)Math.Round(j / 10.0)) * 10;

                    /*Add the cell to the ActualGeneration in the handler class, 
                    it divides the coordinates by 10 so we get the actual indexes in the Multidimensional Generation-Array.*/
                    handler.setupActualGeneration(new Cell(xPosition / 10, YPosition / 10));

                    Rectangle r = new Rectangle();
                    r.Width = 8;
                    r.Height = 8;
                    r.Fill = (Brushes.SaddleBrown);
                    Canvas.SetLeft(r, i + 1);
                    Canvas.SetTop(r, j + 1);
                    gameBoardCanvas.Children.Add(r);
                    #endregion
                }
                DisableButtons();
            }
        }

        private void resetGameBoard()
        {
            gameBoardCanvas.Children.Clear();
            for (int i = 0; i < 800; i += 10)
            {
                for (int j = 0; j < 600; j += 10)
                {
                    int xPosition = 0;
                    int YPosition = 0;
                    //Algorithm for get the nearest value with 10.
                    xPosition = ((int)Math.Round(i / 10.0)) * 10;
                    YPosition = ((int)Math.Round(j / 10.0)) * 10;

                    Rectangle r = new Rectangle();
                    r.Width = 8;
                    r.Height = 8;
                    r.Fill = (Brushes.SaddleBrown);
                    Canvas.SetLeft(r, i + 1);
                    Canvas.SetTop(r, j + 1);
                    gameBoardCanvas.Children.Add(r);
                }
            }
        }

        private void EnableAllButtons()
        {
            buttonGetNxtGen.Foreground = Brushes.Black;
            buttonStartTimer.Foreground = Brushes.Black;
            buttonSaveGame.Foreground = Brushes.Black;
            buttonResetBoard.Foreground = Brushes.Black;

            buttonGetNxtGen.IsHitTestVisible = true;
            buttonStartTimer.IsHitTestVisible = true;
            buttonSaveGame.IsHitTestVisible = true;
            comboxBoxSavedGames.IsHitTestVisible = true;
            buttonResetBoard.IsHitTestVisible = true;
        }

        private void DisableButtons()
        {
            buttonGetNxtGen.Foreground = Brushes.Gray;
            buttonStartTimer.Foreground = Brushes.Gray;
            buttonSaveGame.Foreground = Brushes.Gray;
            buttonResetBoard.Foreground = Brushes.Gray;

            buttonGetNxtGen.IsHitTestVisible = false;
            buttonStartTimer.IsHitTestVisible = false;
            buttonSaveGame.IsHitTestVisible = false;
            comboxBoxSavedGames.IsHitTestVisible = false;
            buttonResetBoard.IsHitTestVisible = false;
            buttonReplay.IsHitTestVisible = false;
            buttonReplay.Foreground = Brushes.Gray;
            buttonDelete.Foreground = Brushes.Gray;
            buttonDelete.IsHitTestVisible = false;
        }

        /// <summary>
        /// Method for set and update the Canvas with a Dead or Alive Cell, It will multiplicate the X and Y with 10 and get the Canvas Coordinates to Draw the Rectangle.
        /// If you send true it will draw a rectangle 10x10 black. If you send false it will draw a rewctangle 8x8 WhiteSmoke.
        /// </summary>
        /// <param name="x">Send the Cell.X Propertie.</param>
        /// <param name="y">Send the Cell.Y Propertie</param>
        /// <param name="IsAlive">Put it True if the cell is Alive, Put it false if it's dead.</param>
        private void PrintCell(int x, int y, bool IsAlive)
        {
            #region CellIsAlive
            if (IsAlive == true)
            {
                Rectangle r = new Rectangle();
                r.Width = 10;
                r.Height = 10;
                r.Fill = (Brushes.Black);
                Canvas.SetLeft(r, x * 10);
                Canvas.SetTop(r, y * 10);
                gameBoardCanvas.Children.Add(r);
            }
            #endregion

            #region CellIsDead
            else
            {
                Rectangle r = new Rectangle();
                r.Width = 8;
                r.Height = 8;
                r.Fill = (Brushes.SaddleBrown);
                Canvas.SetLeft(r, x * 10 + 1);
                Canvas.SetTop(r, y * 10 + 1);
                gameBoardCanvas.Children.Add(r);
            }
            #endregion
        }

        /// <summary>
        /// Method for fill up the combobox with the activeplayer savedgames.
        /// </summary>
        private void LoadSavedGames()
        {
            comboxBoxSavedGames.IsHitTestVisible = true;
            comboxBoxSavedGames.Items.Clear();
            using (GContext db = new GContext())
            {
                var SavedGames = db.SavedGames.Where(x => x.Player_id == _playerId);
                if (SavedGames != null)
                {
                    foreach (var SavedGame_id in SavedGames)
                    {
                        comboxBoxSavedGames.Items.Add(SavedGame_id.id);
                    }
                }
                else
                {
                    MessageBox.Show("Wops! Something went wrong, please slect a player again.");
                }
            }
        }

        private void GetNextGeneration()
        {
            int countCellsAlive = 0;

            //Adds the first generation only the first time you run this method when genNumber is zero.
            #region AddTheFirstGenerationToSave
            if (genNumber == 0)
            {
                var firstGeneration = handler.GetActualGeneration();
                for (int i = 0; i < firstGeneration.GetLength(0); i++)
                {
                    for (int j = 0; j < firstGeneration.GetLength(1); j++)
                    {
                        if (firstGeneration[i, j].IsAlive)
                        {
                            countCellsAlive++;
                            handler.AddGeneration(new Cell(i, j, true), genNumber);
                        }
                    }
                }
                aliveCellLabel.Content = "Alive Cells: " + countCellsAlive;
                countCellsAlive = 0;
                genNumber++;
            }
            #endregion

            //Load the nextGeneration.
            var arrayToUpdateFrom = handler.GetNextGeneration();

            //Loops through all the Cells from the Array, So we can populate the Canvas with the Next Generation. 
            #region Print out and calculates The NextGeneration
            for (int i = 0; i < arrayToUpdateFrom.GetLength(0); i++)
            {
                for (int j = 0; j < arrayToUpdateFrom.GetLength(1); j++)
                {
                    //temp bool to use when looks if the cell is dead or alive.
                    bool tempAliveOrNot = arrayToUpdateFrom[i, j].IsAlive;

                    if (tempAliveOrNot == true)
                    {
                        //Add the cell to the List in handler that we will save to Database when press save game.
                        handler.AddGeneration(new Cell(i, j, true), genNumber);
                        countCellsAlive++;
                    }

                    // A Switch where we looking if we have to change the cell in the canvas or not for this new generation.
                    switch (handler.CheckIfHaveToChange(i, j, tempAliveOrNot))
                    {
                        //If it's true we not do anything it's already printed the right condition.
                        case true:
                            {
                                break;
                            }
                            //if it's false we have to change it in the canvas.
                        case false:
                            {
                                //if it's true we print it alive.
                                if (tempAliveOrNot)
                                {
                                    PrintCell(i, j, true);
                                }
                                //else we print it dead.
                                else
                                {
                                    PrintCell(i, j, false);
                                }
                                break;
                            }
                    }
                    //And last of all the actualgeneration gets the cells from this new generation.
                    handler.setupActualGeneration(new Cell(i, j, tempAliveOrNot, genNumber));
                }
            }
            #endregion
            currentGenlabel.Content = "Gen: " + genNumber;
            genNumber++;
            aliveCellLabel.Content = "Alive Cells: " + countCellsAlive;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            currentGenlabel.Content = "Gen: " + handler.UpdateLabels().x1;
            labelTimerSpeed.Content = String.Format("Timer Speed: {0} ms", timer.Interval.TotalMilliseconds);

            //If the combobox has a savedgame selected we enter in here.
            if (comboxBoxSavedGames.SelectedItem != null)
            {
                resetGameBoard();

                //Print out all the cells, and print out next generation in the next tick until it's printed all generations. And then it start from generation 0 again.
                foreach (var g in handler.GetNextGenerationLoadedFromDB())
                {
                    PrintCell(g.X, g.Y, true);
                }
                
                aliveCellLabel.Content = "Alive Cells: " + handler.UpdateLabels().x2;
            }
            //if there is no selected savedgame we run the getnextgeneration as usual.
            else
            {
                GetNextGeneration();
            }
        }

        /// <summary>
        /// When the left button is pressed this method runs every 200 milliseconds and then you can painting out the cells instantly.
        /// </summary>
        /// <param name="e"></param>
        private async void buttonIsPressed(MouseButtonEventArgs e)
        {
            int X = 0;
            int Y = 0;
            try
            {
                //runs this loop until the left button is up again and the GameBoardIsPressed changes to false.
                while (GameBoardIsPressed == true)
                {

                    //Clear the canvas before updating it.
                    // Taking the position from the cursor.
                    Y = (int)e.GetPosition(gameBoardCanvas).Y;
                    X = (int)e.GetPosition(gameBoardCanvas).X;

                    //Substract the radius value so it will be the center point.
                    X -= 5;
                    Y -= 5;

                    //Rounds it to the nearest 10.
                    X = ((int)Math.Round(X / 10.0));
                    Y = ((int)Math.Round(Y / 10.0));

                    //A switch that looks if we have to change the cell or not in the canvas, depends on if it's alive or dead.
                    switch (handler.ClickKillOrMakeCell(X, Y))
                    {
                        //If it's true we have to print it as alive
                        case true:
                            {
                                PrintCell(X, Y, true);
                                break;
                            }
                            //If it's false we have to print it as dead.
                        case false:
                            {
                                PrintCell(X, Y, false);
                                break;
                            }
                    }
                    await Task.Delay(200);
                }
            }
            //If the user keep the button pressed and goes outside of the canvas range you will get to this catch and we stop collecting the coordinates.
            catch
            {
                GameBoardIsPressed = false;
            }
        }

        /// <summary>
        /// When this event fires it sends the MouseButtonEventArgs with the coordinates to the ButtonIsPressed method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gameBoardCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            GameBoardIsPressed = true;
            buttonIsPressed(e);
        }

        private void gameBoardCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            GameBoardIsPressed = false;
        }

        private void buttonGetNxtGen_Click(object sender, RoutedEventArgs e)
        {
            GetNextGeneration();
        }

        private void buttonStartTimer_Click(object sender, RoutedEventArgs e)
        {
            TimerIsOn = !TimerIsOn;
            if (TimerIsOn)
                timer.Start();

            if (!TimerIsOn)
                timer.Stop();
        }

        /// <summary>
        /// Eventhandler that runs when you want to replay a savedgame from the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonReplay_Click(object sender, RoutedEventArgs e)
        {

            ReplayOn = !ReplayOn;
            if (ReplayOn)
            {
                handler.LoadGenFromDatabase();
                timer.Start();
                comboxBoxSavedGames.IsHitTestVisible = false;
                DisableButtons();
                buttonReplay.Foreground = Brushes.Black;
                buttonReplay.IsHitTestVisible = true;
            }
            if (!ReplayOn)
            {
                timer.Stop();
                buttonResetBoard.Foreground = Brushes.Black;
                buttonResetBoard.IsHitTestVisible = true;
                buttonDelete.Foreground = Brushes.Black;
                buttonDelete.IsHitTestVisible = true;
                comboxBoxSavedGames.IsHitTestVisible = true;
            }
        }

        private async void buttonSaveGame_Click(object sender, RoutedEventArgs e)
        {
            if (handler.CheckIfHaveGenerationsToSaveToDB())
            {
                DisableButtons();
                handler.SetupPlayer(_playerId);
                await Task.Run(new Action(handler.SaveToDatabase));
                MessageBox.Show("Sucessfully saved to database");
                EnableAllButtons();
                LoadSavedGames();
            }
            else
            {
                MessageBox.Show("There is nothing to save. Please run some generations first.");
            }
        }

        private void buttonResetBoard_Click(object sender, RoutedEventArgs e)
        {
            handler.ClearAliveCellsList();
            handler.ResetGenNumber();
            resetGameBoard();
            initializeGameBoard();
            genNumber = 0;
            currentGenlabel.Content = "Gen: 0";
            aliveCellLabel.Content = "Alive Cells: 0";
            comboxBoxSavedGames.SelectedItem = null;
            EnableAllButtons();
            if (TimerIsOn = TimerIsOn)
            {
                TimerIsOn = !TimerIsOn;
                timer.Stop();
            }
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private async void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            DisableButtons();
            //Lambda expression for making a Action Delegate that runs an method with one parameter.
            await Task.Run(() => handler.DeleteSavedGame(SavedGameId));
            LoadSavedGames();
            MessageBox.Show("Successfully deleted");
        }

        private void comboBoxSavedGames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic itemSelected = comboxBoxSavedGames.SelectedItem;
            if (itemSelected != null)
            {
                SavedGameId = itemSelected;
                handler.ResetGenNumber();
                handler.setSavedGameId(SavedGameId);
                buttonReplay.IsHitTestVisible = true;
                buttonReplay.Foreground = Brushes.Black;
                buttonDelete.Foreground = Brushes.Black;
                buttonDelete.IsHitTestVisible = true;
                buttonSaveGame.IsHitTestVisible = false;
                buttonSaveGame.Foreground = Brushes.Gray;
            }
            else
                comboxBoxSavedGames.ItemsSource = null;
        }

        private void sliderTimerSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            timer.Interval = new TimeSpan(0, 0, 0, 0, (int)e.NewValue);

            if (labelTimerSpeed != null)
            {
                labelTimerSpeed.Content = String.Format("Timer Speed: {0} ms", timer.Interval.TotalMilliseconds);
            }
        }

        private void buttonShowDB_Click(object sender, RoutedEventArgs e)
        {
            ShowDB showDB = new ShowDB();
            showDB.ShowDialog();
        }

        private void buttonPickPlayer_Click(object sender, RoutedEventArgs e)
        {
            PickPlayerWin pickedPlayer = new PickPlayerWin();
            if (pickedPlayer.ShowDialog() == true)
            {
                _playerId = pickedPlayer.AnswerOne;
                _playerName = pickedPlayer.AnswerTwo;
                PlayerLabel.Content = "Name: " + _playerName;
                LoadSavedGames();
                EnableAllButtons();
                handler.SetupPlayer(_playerId);
            }
        }

        private void aboutButton_Click(object sender, RoutedEventArgs e)
        {
            AboutGolWin aboutGol = new AboutGolWin();
            aboutGol.ShowDialog();
        }
    }
}

