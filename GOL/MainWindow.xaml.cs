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

        public MainWindow()
        {
            InitializeComponent();
            Paint();
        }
        private void Paint()
        {
            int[,] gameBoard = new int[800,600];
            
            for (int i = 0;i < 800; i += 10)
            {
                for(int j = 0; j < 600; j += 10)
                {
                    DrawPoint(i, j);
                }
            }
        }
        private void DrawPoint(int x,int y)
        {
            Ellipse e = new Ellipse();
            e.Width = 5;
            e.Height = 5;
            e.Fill = (Brushes.Black);
            Canvas.SetLeft(e, x);
            Canvas.SetTop(e, y);
            gameBoardCanvas.Children.Add(e);
            
        }

        
    }
}
