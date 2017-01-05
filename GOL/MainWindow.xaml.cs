
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

namespace GOL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GOLHandler handler;
        public MainWindow()
        {
            InitializeComponent();
            handler = new GOLHandler();
            initializeGameBoard();
        }

        /// <summary>
        /// Method for Set-up the gameboard(canvas). it's 800x600 pixlar/points. It setting one cell at every 10x10 coordinates. 
        /// It rounds the value to nearest 10 so we will be sure that the Coordinates will be 0x0,0x10,0x20,0x30,0x40,0x50............
        /// It will set all cells as dead by default and with the 8x8 size with the color WhiteSmoke. It's 10x10 and Black when alive.
        /// </summary>
        private void initializeGameBoard()
        {
            int[,] gameBoard = new int[800, 600];
            int xPosition = 0;
            int YPosition = 0;

            //loop through all the Canvas-Coordinates.
            for (int i = 0; i < 800; i += 10)
            {
                for (int j = 0; j < 600; j += 10)
                {
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
                    Canvas.SetLeft(r, i);
                    Canvas.SetTop(r, j);
                    gameBoardCanvas.Children.Add(r);
                }
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
            gameBoardCanvas.Children.Clear();//Clear the canvas before updating it.

            // Taking the position from the cursor and - 5 so we get the center of the cell.
            int tempY = (int)(e.GetPosition(gameBoardCanvas).Y) - 5;
            int tempX = (int)(e.GetPosition(gameBoardCanvas).X) - 5;

            //rounds it to the nearest int and divide it with 10 so we get the actual index numbers we want before we send it for Kill or make it Alive.
            tempX = ((int)Math.Round(tempX / 10.0));
            tempY = ((int)Math.Round(tempY / 10.0));
            handler.KillOrMakeCell(tempX, tempY);

            //makes a temporary Array from the GolHandler with all the Cells.
            var arrayToUpdateFrom = handler.GetActualGeneration();

            //Loops through all the Cells from the Array, So we can populate the Canvas with the Actual Generation. 
            #region LoopThroughTheGeneration
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

        private void buttonGetNxtGen_Click(object sender, RoutedEventArgs e)
        {
            handler.calculateNextGeneration();

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            gameBoardCanvas.Children.Clear();
            Cell[,] arrayToUpdateFrom = handler.GetNextGeneration();
            //Loops through all the Cells from the Array, So we can populate the Canvas with the Actual Generation. 
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
        }
    }
}

