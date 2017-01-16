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
        GOLHandler handler;
        DispatcherTimer timer;
        private bool _IsOn;
        private bool _ReplayIsOn;
        private int _playerId;
        private int genNumber = 0;
        private string _playerName;
        private bool GameBoardIsPressed = false;
        int[,] gameBoard;

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
            buttonClear.Foreground = Brushes.Black;

            buttonGetNxtGen.IsHitTestVisible = true;
            buttonStartTimer.IsHitTestVisible = true;
            buttonSaveGame.IsHitTestVisible = true;
            comboxBoxSavedGames.IsHitTestVisible = true;
            buttonClear.IsHitTestVisible = true;
        }

        private void DisableButtons()
        {
            buttonGetNxtGen.Foreground = Brushes.Gray;
            buttonStartTimer.Foreground = Brushes.Gray;
            buttonSaveGame.Foreground = Brushes.Gray;
            buttonClear.Foreground = Brushes.Gray;

            buttonGetNxtGen.IsHitTestVisible = false;
            buttonStartTimer.IsHitTestVisible = false;
            buttonSaveGame.IsHitTestVisible = false;
            comboxBoxSavedGames.IsHitTestVisible = false;
            buttonClear.IsHitTestVisible = false;
            buttonReplay.IsHitTestVisible = false;
            buttonReplay.Foreground = Brushes.Gray;
            buttonDelete.Foreground = Brushes.Gray;
            buttonDelete.IsHitTestVisible = false;
        }

        /// <summary>
        /// Method for Set and update Canvas with a Dead or Alive Cell, It will multiplicate the X and Y with then and get the Canvas Coordinates to Draw the Rectangle.
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
        private void LoadSavedGames()
        {
            comboxBoxSavedGames.IsHitTestVisible = true;
            comboxBoxSavedGames.Items.Clear();
            using (GContext db = new GContext())
            {
                var SavedGames = db.SavedGames.Where(x => x.Player_id == _playerId);
                foreach (var SavedGame_id in SavedGames)
                {
                    comboxBoxSavedGames.Items.Add(SavedGame_id.id);
                }
            }
        }

        private void GetNextGeneration()
        {
            int countCellsAlive = 0;
            
            //An Temporary holder for the NextGeneration Array from the handler.

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

            handler.CalculateNextGeneration();
            var arrayToUpdateFrom = handler.GetNextGeneration();

            //Loops through all the Cells from the Array, So we can populate the Canvas with the Next Generation. 
            for (int i = 0; i < arrayToUpdateFrom.GetLength(0); i++)
            {
                for (int j = 0; j < arrayToUpdateFrom.GetLength(1); j++)
                {
                    bool tempAliveOrNot = arrayToUpdateFrom[i, j].IsAlive;

                    if (tempAliveOrNot == true)
                    {
                        handler.AddGeneration(new Cell(i, j, true), genNumber);
                        countCellsAlive++;
                    }
                    switch (handler.CheckIfHaveToChange(i, j, tempAliveOrNot))
                    {
                        case true:
                            {
                                break;
                            }
                        case false:
                            {
                                if (tempAliveOrNot)
                                {
                                    PrintCell(i, j, true);
                                }
                                else
                                {
                                    PrintCell(i, j, false);
                                }
                                break;
                            }
                    }
                    handler.setupActualGeneration(new Cell(i, j, tempAliveOrNot, genNumber));
                }
            }
            
            currentGenlabel.Content = "Gen: " + genNumber;
            genNumber++;
            aliveCellLabel.Content = "Alive Cells: " + countCellsAlive;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {

            labelTimerSpeed.Content = String.Format("Timer Speed: {0} ms", timer.Interval.TotalMilliseconds);
            if (comboxBoxSavedGames.SelectedItem != null)
            {
                resetGameBoard();
                
                foreach (var g in handler.GetNextGenerationLoadedFromDB())
                {
                    PrintCell(g.X, g.Y, true);
                }
                currentGenlabel.Content = "Gen: " + handler.UpdateLabels().x1;
                //handler.ResetAliveCellCount();
                aliveCellLabel.Content = "Alive Cells: " + handler.UpdateLabels().x2;
            }
            else
            {
                GetNextGeneration();
            }
        }

        private async void buttonIsPressed(MouseButtonEventArgs e)
        {
            int X = 0;
            int Y = 0;
            try
            {
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

                    switch (handler.ClickKillOrMakeCell(X, Y))
                    {
                        case true:
                            {
                                PrintCell(X, Y, true);
                                break;
                            }
                        case false:
                            {
                                PrintCell(X, Y, false);
                                break;
                            }
                    }
                    await Task.Delay(200);
                }
            }
            catch
            {
                GameBoardIsPressed = false;
            }
        }

        /// <summary>
        /// Method for Choose the Cells you want alive or not before you save and get the next Generation.
        /// </summary>
        /// <param name="sender"></param>        /// <param name="e"></param>
        private void gameBoardCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            GameBoardIsPressed = true;
            buttonIsPressed(e);
        }

        private void gameBoardCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            GameBoardIsPressed = false;
        }

        /// <summary>
        /// Handler for the NextGenerationButton.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonGetNxtGen_Click(object sender, RoutedEventArgs e)
        {
            GetNextGeneration();
        }

        /// <summary>
        /// Handler for start or stop the timer in the handler class.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStartTimer_Click(object sender, RoutedEventArgs e)
        {
            TimerIsOn = !TimerIsOn;
            if (TimerIsOn)
                timer.Start();

            if (!TimerIsOn)
                timer.Stop();
        }

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
                buttonClear.Foreground = Brushes.Black;
                buttonClear.IsHitTestVisible = true;
                buttonDelete.Foreground = Brushes.Black;
                buttonDelete.IsHitTestVisible = true;
                comboxBoxSavedGames.IsHitTestVisible = true;
            }
        }

        private async void buttonSaveGame_Click(object sender, RoutedEventArgs e)
        {
            DisableButtons();
            handler.SetupPlayer(_playerId);
            await Task.Run(new Action(handler.SaveToDatabase));
            MessageBox.Show("Sucessfully saved to database");
            EnableAllButtons();
            LoadSavedGames();
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
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

        private void buttonPickerPlayer_Click(object sender, RoutedEventArgs e)
        {
            PlayerPickerWin pickedPlayer = new PlayerPickerWin();
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

