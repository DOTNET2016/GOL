
﻿using System;
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
        Grid grid = new Grid();
        Cell cell = new Cell();

        

        int[,] gameBoard = new int[800, 600];
        public MainWindow()
        {
            InitializeComponent();
            initializeGameBoard();
        }
        private void initializeGameBoard()
        {
            int[,] gameBoard = new int[800,600];
            
            for (int i = 0;i < 800; i += 10)
            {
                for(int j = 0; j < 600; j += 10)
                {
                    grid.AddCell(new Cell(i, j));
                    DrawPoint(i, j);
                }
            }
        }
        private void DrawPoint(int x,int y)
        {
            Random rnd = new Random();
            gameBoardCanvas.Background = Brushes.White;
            Ellipse e = new Ellipse();
            e.Width = 10;
            e.Height = 10;
            e.Fill = (Brushes.WhiteSmoke);
            Canvas.SetLeft(e,x);
            Canvas.SetTop(e,y);
            gameBoardCanvas.Children.Add(e);
          
            
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            int x = 0;
            int y = 0;
            DrawPoint(x,y);
        }

        public void CheckCellState()
        {       
            for (int i = 0; i < cell.X; i++)
            {
                for (int j = 0; j < cell.Y; j++)
                {
                   //check if alive etc....
                }
            }
        }
    }
}

