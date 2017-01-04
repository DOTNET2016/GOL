
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
        GOLHandler handler = new GOLHandler();
        public MainWindow()
        {
            InitializeComponent();
            initializeGameBoard();
        }
        private void initializeGameBoard()
        {
            int[,] gameBoard = new int[800, 600];
            int xPosition = 0;
            int YPosition = 0;

            for (int i = 0; i < 800; i += 10)
            {
                for (int j = 0; j < 600; j += 10)
                {

                    xPosition = ((int)Math.Round(i / 10.0)) * 10;
                    YPosition = ((int)Math.Round(j / 10.0)) * 10;

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

        private void UpdatePoint(int x, int y, bool IsAlive)
        {
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
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {


        }

        private void gameBoardCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            gameBoardCanvas.Children.Clear();
            int tempY = (int)(e.GetPosition(gameBoardCanvas).Y) - 5;
            int tempX = (int)(e.GetPosition(gameBoardCanvas).X) - 5;
            tempX = ((int)Math.Round(tempX / 10.0));
            tempY = ((int)Math.Round(tempY / 10.0));

            handler.KillOrMakeCell(tempX, tempY);
            var arrayToUpdateFrom = handler.GetCellArray();

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

