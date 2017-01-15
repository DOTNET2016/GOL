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
using System.Windows.Shapes;

namespace GOL
{
    /// <summary>
    /// Interaction logic for ShowDB.xaml
    /// </summary>
    public partial class ShowDB : Window
    {
        public ShowDB()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            GOL.GameOfLifeEFDataSet gameOfLifeEFDataSet = ((GOL.GameOfLifeEFDataSet)(this.FindResource("gameOfLifeEFDataSet")));
            // Load data into the table Generation. You can modify this code as needed.
            GOL.GameOfLifeEFDataSetTableAdapters.GenerationTableAdapter gameOfLifeEFDataSetGenerationTableAdapter = new GOL.GameOfLifeEFDataSetTableAdapters.GenerationTableAdapter();
            gameOfLifeEFDataSetGenerationTableAdapter.Fill(gameOfLifeEFDataSet.Generation);
            System.Windows.Data.CollectionViewSource generationViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("generationViewSource")));
            generationViewSource.View.MoveCurrentToFirst();
            // Load data into the table Player. You can modify this code as needed.
            GOL.GameOfLifeEFDataSetTableAdapters.PlayerTableAdapter gameOfLifeEFDataSetPlayerTableAdapter = new GOL.GameOfLifeEFDataSetTableAdapters.PlayerTableAdapter();
            gameOfLifeEFDataSetPlayerTableAdapter.Fill(gameOfLifeEFDataSet.Player);
            System.Windows.Data.CollectionViewSource playerViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("playerViewSource")));
            playerViewSource.View.MoveCurrentToFirst();
            // Load data into the table SavedGames. You can modify this code as needed.
            GOL.GameOfLifeEFDataSetTableAdapters.SavedGamesTableAdapter gameOfLifeEFDataSetSavedGamesTableAdapter = new GOL.GameOfLifeEFDataSetTableAdapters.SavedGamesTableAdapter();
            gameOfLifeEFDataSetSavedGamesTableAdapter.Fill(gameOfLifeEFDataSet.SavedGames);
            System.Windows.Data.CollectionViewSource savedGamesViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("savedGamesViewSource")));
            savedGamesViewSource.View.MoveCurrentToFirst();
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
