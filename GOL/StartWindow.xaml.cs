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
    /// Interaction logic for PlayerNameIntro.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        MainWindow window;
        int playerId;
        public string PlayerName { get; set; }

        public StartWindow()
        {
            InitializeComponent();

            using (GContext db = new GContext())
            {
                var players = db.Players;
 
                foreach (var player in players)
                {
                    comboBoxPlayers.Items.Add(player.PlayerName);
                }

            }
        }

        private void buttonStartGame_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxPlayers.SelectedItem == null)
                MessageBox.Show("Error, you have not picked a name.");
            else
                AddPlayer();
        }

        ////Saves the players name to the player table and gives them an id_number & Adds the players id to the SavedGames Table
        //private void AddPlayer()
        //{
        //    using (GContext db = new GContext())
        //    {
        //        Player player = new Player();
        //        player.PlayerName = textBoxEnterName.Text.ToLower();
        //        db.Players.Add(player);
        //        db.SaveChanges();
        //        playerId = (from p in db.Players
        //                    select p.id).Max();
        //    }

        //    window = new MainWindow(playerId);
        //    window.ShowDialog();
        //}

        private void AddPlayer()
        {
            using (GContext db = new GContext())
            {
                playerId = (from l in db.Players
                            where l.PlayerName.ToLower().StartsWith(PlayerName)
                            select l.id).FirstOrDefault();
            }

            window = new MainWindow(playerId);
            window.ShowDialog();
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void comboBoxPlayers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic itemSelected = comboBoxPlayers.SelectedItem;
            PlayerName = itemSelected;
        }
    }
}
