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

        //returns the playerId to MainWindow
        public int PlayerId
        {
            get { return playerId; }
        }

        //returns the playerName to Main
        public string ActivePlayerName
        {
            get { return CurrentPlayerName; }
        }

        private void buttonStartGame_Click(object sender, RoutedEventArgs e)
        {
            //prevents the user from slecting both player and creating a new
            if (PlayerName == null && NewPlayerName == null)
                MessageBox.Show("Error, you have not picked a name. or made a new player");
            else if (PlayerName != null && NewPlayerName != null)
                {
                    MessageBox.Show("Error, you managed to pick a player while a new was made!");
                    this.DialogResult = false;
                }
            else if(PlayerName != null)
                {
                    PickedPlayer();
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

        /// <summary>
        /// if a new player have been made then then it will take the name and store it to the database
        /// and then check the database for the last inserted playerid, and store the name
        /// </summary>
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

        /// <summary>
        /// when a player have been picked from the combobox it will compare the name with the 
        /// database and also check for the playername.
        /// </summary>
        private void PickedPlayer()
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

        /// <summary>
        /// sets the player the user selected in the combobox and store it in PlayerName for use later
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxPlayers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NewPlayerButton.IsHitTestVisible = false;
            dynamic itemSelected = comboBoxPlayers.SelectedItem;
            PlayerName = itemSelected;
        }

        /// <summary>
        /// Opens up a new window where the user is promted to ender a player name, and is then returned to
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void New_Player_Click(object sender, RoutedEventArgs e)
        {
            NewPlayer newPlayer = new NewPlayer("Please enter your name:", "");
            comboBoxPlayers.IsHitTestVisible = true;
            if (newPlayer.ShowDialog() == true)
            {
                NewPlayerName = newPlayer.InsertedPlayerName;
                comboBoxPlayers.IsHitTestVisible = false;
            }
        }
    }
}
