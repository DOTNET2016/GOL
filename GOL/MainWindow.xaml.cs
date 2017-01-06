using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        //Fields
        private bool _IsOn;
        GOLHandler handler;

        //propertie
        public bool TimerIsOn
        {
            get
            {
                return _IsOn;
            }
            set
            {
                _IsOn = value;
                StartTimer.Content = _IsOn ? "Stop Timer" : "Start Timer";
            }
        }
        //constructor.
        public MainWindow()
        {
            InitializeComponent();
            handler = new GOLHandler();
            initializeGameBoard();
            handler.Timer_Ticked += Handler_Timer_Ticked;
        }

        //Eventhandler for the Timer_Ticked event in the handler class.
        private void Handler_Timer_Ticked(object sender, EventArgs e)
        {
            LoadNextGeneration();
        }

        /// <summary>
        /// Method for Set-up the gameboard(canvas). it's 800x600 pixlar/points. It setting one cell at every 10x10 coordinates. 
        /// It rounds the value to nearest 10 so we will be sure that the Coordinates will be 0x0,0x10,0x20,0x30,0x40,0x50............
        /// It will set all cells as dead by default and with the 8x8 size with the color WhiteSmoke. It's 10x10 and Black when alive.
        /// </summary>
        private void initializeGameBoard()
        {
            int[,] gameBoard = new int[800, 600];

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
                    handler.AddCell(new Cell(xPosition / 10, YPosition / 10));
                    Rectangle r = new Rectangle();
                    r.Width = 8;
                    r.Height = 8;
                    r.Fill = (Brushes.WhiteSmoke);
                    Canvas.SetLeft(r, i + 1);
                    Canvas.SetTop(r, j + 1);
                    gameBoardCanvas.Children.Add(r);
                }
                #endregion
            }
        }

        /// <summary>
        /// Method for Set and update Canvas with a Dead or Alive Cell, It will multiplicate the X and Y with then and get the Canvas Coordinates to Draw the Rectangle.
        /// If you send true it will draw a rectangle 10x10 black. If you send false it will draw a rewctangle 8x8 WhiteSmoke.
        /// </summary>
        /// <param name="x">Send the Cell.X Propertie.</param>
        /// <param name="y">Send the Cell.Y Propertie</param>
        /// <param name="IsAlive">Put it True if the cell is Alive, Put it false if it's dead.</param>
        private void UpdatePoint(int x, int y, bool IsAlive)
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
                r.Fill = (Brushes.WhiteSmoke);
                Canvas.SetLeft(r, x * 10);
                Canvas.SetTop(r, y * 10);
                gameBoardCanvas.Children.Add(r);
            }
            #endregion
        }

        /// <summary>
        /// Method for Choose the Cells you want alive or not before you save and get the next Generation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gameBoardCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Clear the canvas before updating it.
            gameBoardCanvas.Children.Clear();

            // Taking the position from the cursor.
            double tempY = e.GetPosition(gameBoardCanvas).Y;
            double tempX = e.GetPosition(gameBoardCanvas).X;

            handler.KillOrMakeCell(tempX, tempY,5);

            //An Temporary holder for the ActualGeneration Array from the handler.
            var arrayToUpdateFrom = handler.GetActualGeneration();

            //Loops through all the Cells from the Array, So we can populate the Canvas with the Actual Generation. 
            #region LoopThroughTheActualGeneration
            for (int i = 0; i < arrayToUpdateFrom.GetLength(0); i++)
            {
                for (int j = 0; j < arrayToUpdateFrom.GetLength(1); j++)
                {
                    if (arrayToUpdateFrom[i, j].IsAlive == true)
                    {
                        UpdatePoint(i, j, true);
                    }
                    else
                    {
                        UpdatePoint(i, j, false);
                    }
                }
            }
            #endregion
        }

       
        private void LoadNextGeneration()
        {
            handler.calculateNextGeneration();
            gameBoardCanvas.Children.Clear();

            //An Temporary holder for the NextGeneration Array from the handler.
            var arrayToUpdateFrom = handler.GetNextGeneration();

            //Loops through all the Cells from the Array, So we can populate the Canvas with the Next Generation. 
            #region LoopThroughTheNextGeneration
            for (int i = 0; i < arrayToUpdateFrom.GetLength(0); i++)
            {
                for (int j = 0; j < arrayToUpdateFrom.GetLength(1); j++)
                {
                    if (arrayToUpdateFrom[i, j].IsAlive == true)
                    {
                        handler.AddCell(new Cell(i, j, true));
                        UpdatePoint(i, j, true);
                    }
                    else
                    {
                        handler.AddCell(new Cell(i, j));
                        UpdatePoint(i, j, false);
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// Handler for the NextGenerationButton.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonGetNxtGen_Click(object sender, RoutedEventArgs e)
        {
            LoadNextGeneration();
        }

        /// <summary>
        /// Handler for start or stop the timer in the handler class.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartTimer_Click(object sender, RoutedEventArgs e)
        {
            TimerIsOn = !TimerIsOn;
            if (TimerIsOn)
                handler.Start_Timer();

            if (!TimerIsOn)
                handler.Stop_Timer();
        }      
    }
}

