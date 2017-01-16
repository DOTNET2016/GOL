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
    public partial class PickPlayerWin : Window
    {
        int playerId;
        public string PlayerName { get; set; }
        public string NewPlayerName { get; set; }

        public string CurrentPlayerName { get; set; }

        public PickPlayerWin()
        {
            InitializeComponent();
            loadPlayers();
        }

        public int AnswerOne
        {
            get { return playerId; }
        }

        public string AnswerTwo
        {
            get { return CurrentPlayerName; }
        }

        private void buttonStartGame_Click(object sender, RoutedEventArgs e)
        {
            if (PlayerName == null && NewPlayerName == null)
                MessageBox.Show("Error, you have not picked a name. or made a new player");
            else if (PlayerName != null && NewPlayerName != null)
                {
                    MessageBox.Show("Error, you managed to pick a player while a new was made!");
                    this.DialogResult = false;
                }
            else if(PlayerName != null)
                {
                    PickPlayer();
                    this.DialogResult = true;
                }
            else if (NewPlayerName != null)
                {
                    AddPlayer();
                    this.DialogResult = true;
                }
        }

        //load all existing players from databse
        private void loadPlayers()
        {
            using (GContext db = new GContext())
            {
                var players = db.Player;

                foreach (var player in players)
                {
                    comboBoxPlayers.Items.Add(player.PlayerName);
                }
            }
        }

        //Saves the players name to the player table and gives them an id_number & Adds the players id to the SavedGames Table
        private void AddPlayer()
        {
            using (GContext db = new GContext())
            {
                Player player = new Player();
                player.PlayerName = NewPlayerName.ToLower();
                db.Player.Add(player);
                db.SaveChanges();
                playerId = (from p in db.Player
                            select p.id).Max();
            }
            CurrentPlayerName = NewPlayerName;
        }

        private void PickPlayer()
        {
            using (GContext db = new GContext())
            {
                playerId = (from l in db.Player
                            where l.PlayerName.ToLower().StartsWith(PlayerName)
                            select l.id).FirstOrDefault();
                CurrentPlayerName = (from l in db.Player
                            where l.PlayerName.ToLower().StartsWith(PlayerName)
                            select l.PlayerName).FirstOrDefault();
            }
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void comboBoxPlayers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NewPlayerButton.IsHitTestVisible = false;
            dynamic itemSelected = comboBoxPlayers.SelectedItem;
            PlayerName = itemSelected;
        }

        private void New_Player_Click(object sender, RoutedEventArgs e)
        {
            NewPlayer newPlayer = new NewPlayer("Please enter your name:", "Adam");
            comboBoxPlayers.IsHitTestVisible = true;
            if (newPlayer.ShowDialog() == true)
            {
                NewPlayerName = newPlayer.Answer;
                comboBoxPlayers.IsHitTestVisible = false;
            }
        }
    }
}
