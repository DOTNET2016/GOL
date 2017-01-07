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
    public partial class PlayerNameIntro : Window
    {
       
        public string userName { get; set; }
        public PlayerNameIntro()
        {
            InitializeComponent();
        }

        private void buttonStartGame_Click(object sender, RoutedEventArgs e)
        {
            AddPlayer();
            Close();
        }

        private void textBoxEnterName_TextInput(object sender, TextCompositionEventArgs e)
        {
            userName = textBoxEnterName.Text;
        }

        //saves the players name to the player table and gives them an id_number
        private void AddPlayer()
        {
            using (GoLContext db = new GoLContext())
            {
                PlayersTable player = new PlayersTable();
                player.PlayerName = textBoxEnterName.Text;

                db.PlayersTables.Add(player);
                db.SaveChanges();
            }
        }
    }
}
